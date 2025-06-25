using Application.ApiResponse;
using Application.Dto;
using Application.Interface.NotificationInterface;
using Application.Interface.StudentInterface;
using Domain.Enum;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services.StudentServices
{
    public class StudentProjectService : IStudentProjectService
    {
        private readonly IStudentProjectRepository _repository;
        private readonly INotificationService _notificationService;

        public StudentProjectService(IStudentProjectRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task<ApiResponse<string>> UploadProject(int studentId, FileUploadDto dto)
        {
            try
            {
                if (dto.ProjectFiles == null || dto.ProjectFiles.Count == 0)
                    return new ApiResponse<string>(null, "No files uploaded", false);

                if (dto.ProjectFiles.Count > 2)
                    return new ApiResponse<string>(null, "You can only upload up to 2 files at a time.", false);

                var student = await _repository.GetStudentById(studentId);
                if (student == null)
                    return new ApiResponse<string>(null, "Student not found", false);

                if (student.GroupId == null)
                    return new ApiResponse<string>(null, "Student is not assigned to any project group.", false);

                var studentWithGroup = await _repository.GetStudentWithGroup(studentId);
                var group = studentWithGroup?.Group;

                if (group == null)
                    return new ApiResponse<string>(null, "Project group not found.", false);

                var groupId = group.Id;
                var tutorId = group.TutorId;

                if (!tutorId.HasValue)
                    return new ApiResponse<string>(null, "Tutor not assigned to the student's group.", false);

                // ✅ Check if the last non-final submission is reviewed
                var hasUnreviewedProject = await _repository.GetUnreviewedProjectSubmissions(studentId);
                if (hasUnreviewedProject)
                    return new ApiResponse<string>(null, "Wait for tutor to review your previous project submission.", false);


                foreach (var file in dto.ProjectFiles)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();

                    var project = new StudentProject
                    {
                        StudentId = studentId,
                        GroupId = groupId,
                        FileName = file.FileName,
                        FileData = fileBytes,
                        ContentType = file.ContentType,
                        FileSize = file.Length,
                        UploadedAt = DateTime.UtcNow,
                        FinalSubmission = false,
                        IsReviewed = false
                    };

                    await _repository.Add(project);
                }

                await _notificationService.SendNotification(
                    tutorId.Value,
                    "Project Uploaded",
                    $"Student {studentWithGroup.Name} uploaded a new project file."
                );

                return new ApiResponse<string>(null, "Project files uploaded successfully.", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null, $"Error uploading project: {ex.Message}", false);
            }
        }


        public async Task<ApiResponse<string>> UploadFinalProject(int studentId, FileUploadDto dto)
        {
            try
            {
                if (dto.ProjectFiles == null || dto.ProjectFiles.Count == 0)
                    return new ApiResponse<string>(null, "No files uploaded", false);

                if (dto.ProjectFiles.Count > 2)
                    return new ApiResponse<string>(null, "You can only upload up to 2 final project files.", false);

                var student = await _repository.GetStudentById(studentId);
                if (student == null)
                    return new ApiResponse<string>(null, "Student not found.", false);

                if (student.GroupId == null)
                    return new ApiResponse<string>(null, "Student is not assigned to a project group.", false);

                var studentWithGroup = await _repository.GetStudentWithGroup(studentId);
                var group = studentWithGroup?.Group;

                if (group == null)
                    return new ApiResponse<string>(null, "Project group not found.", false);

                var groupId = group.Id;
                var tutorId = group.TutorId;

                if (!tutorId.HasValue)
                    return new ApiResponse<string>(null, "Tutor not assigned to the student's group.", false);

                // ✅ Must have an approved request
                var approvedRequest = await _repository.GetPendingRequestByStudentAndTutor(studentId, tutorId.Value);
                if (approvedRequest == null)
                    return new ApiResponse<string>(null, "You can only upload final project after your request is approved.", false);

                // ✅ Allow only if last final submission is reviewed
                var isLastReviewed = await _repository.HasReviewedLastFinalProject(studentId);
                if (!isLastReviewed)
                    return new ApiResponse<string>(null, "Wait for tutor to review your previous final submission.", false);

                foreach (var file in dto.ProjectFiles)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();

                    var finalProject = new StudentProject
                    {
                        StudentId = studentId,
                        GroupId = groupId,
                        FileName = file.FileName,
                        FileData = fileBytes,
                        ContentType = file.ContentType,
                        FileSize = file.Length,
                        UploadedAt = DateTime.UtcNow,
                        FinalSubmission = true,
                        IsReviewed = false
                    };

                    await _repository.Add(finalProject);
                }

                await _notificationService.SendNotification(
                    tutorId.Value,
                    "Final Project Submitted",
                    $"Student {studentWithGroup.Name} submitted final project files."
                );

                return new ApiResponse<string>(null, "Final project files submitted successfully.", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null, $"Error submitting final project: {ex.Message}", false);
            }
        }


        public async Task<ApiResponse<string>> SubmitProjectRequest(ProjectRequestDto dto, int studentId)
        {
            try
            {
                var student = await _repository.GetStudentById(studentId);
                var tutor = await _repository.GetStudentById(dto.TutorId);
                var project = await _repository.GetProjectById(dto.ProjectId);

                if (student == null || tutor == null || tutor.Role != "Tutor")
                    return new ApiResponse<string>(null, "Invalid student or tutor.", false);

                if (project == null)
                    return new ApiResponse<string>(null, "Invalid or non-existent project ID.", false);

                if (student.Department != tutor.Department)
                    return new ApiResponse<string>(null, "Tutor must be from the same department.", false);

                // ✅ Check if a pending request already exists
                var existingRequest = await _repository.GetPendingRequestByStudentAndTutor(studentId, dto.TutorId);
                if (existingRequest != null)
                {
                    return new ApiResponse<string>(null, "You already have a pending request with this tutor. Please wait for their response.", false);
                }

                var request = new ProjectRequest
                {
                    StudentId = studentId,
                    TutorId = dto.TutorId,
                    ProjectId = dto.ProjectId,
                    ProjectDescription = dto.ProjectDescription,
                    Status = Domain.Enum.RequestStatus.Requested
                };

                await _repository.SaveProjectGroupRequest(request);

                await _notificationService.SendNotification(
                    dto.TutorId,
                    "New Project Request",
                    $"Student {student.Name} has sent you a project request ID'{dto.ProjectId}'."
                );

                return new ApiResponse<string>("Project request submitted successfully.", "Success", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null, $"Error: {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<List<ReviewRequestDto>>> GetReviewedRequests(int studentId)
        {
            try
            {
                var requests = await _repository.GetReviewedRequestsByStudentId(studentId);

                var response = requests.Select(r => new ReviewRequestDto
                {
                    RequestId = r.Id,
                    Status = r.Status.ToString()
                }).ToList();

                if (response.Any())
                {
                    await _notificationService.SendNotification(
                        studentId,
                        "Reviewed Requests Fetched",
                        "You have viewed your reviewed project requests."
                    );
                }

                return new ApiResponse<List<ReviewRequestDto>>(response, "Reviewed requests fetched", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ReviewRequestDto>>(null, $"Error fetching review requests: {ex.Message}", false);
            }
        }

    }
}

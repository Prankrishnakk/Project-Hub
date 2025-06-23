using Application.ApiResponse;
using Application.Dto;
using Application.Interface.NotificationInterface;
using Application.Interface.TutorInterface;
using AutoMapper;
using Domain.Enum;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TutorReviewService : ITutorReviewService
{
    private readonly ITutorReviewRepository _repository;
    private readonly INotificationService _notificationService;

    public TutorReviewService(ITutorReviewRepository repository, IMapper mapper, INotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
    }
    public async Task<ApiResponse<string>> ReviewGroupProject(TutorReviewDto dto, int tutorId)
    {
        try
        {
            var projectGroup = await _repository.GetProjectGroupById(dto.GroupId);
            if (projectGroup == null)
                return new ApiResponse<string>(null, $"Group with ID {dto.GroupId} does not exist.", false);

            if (projectGroup.TutorId != tutorId)
                return new ApiResponse<string>(null, "Unauthorized: You are not assigned to this group.", false);

            var groupStudents = await _repository.GetStudentsByGroupId(dto.GroupId);
            if (!groupStudents.Any())
                return new ApiResponse<string>(null, "No students found in this group.", false);

            var studentProjects = await _repository.GetStudentProjectsByGroupId(dto.GroupId);
            var submittedStudentIds = studentProjects.Select(p => p.StudentId).ToHashSet();
            var unsubmittedStudents = groupStudents.Where(s => !submittedStudentIds.Contains(s.Id)).ToList();

            if (unsubmittedStudents.Any())
            {
                var names = string.Join(", ", unsubmittedStudents.Select(s => s.Name));
                return new ApiResponse<string>(null, $"Review not allowed. The following students haven't submitted their project: {names}", false);
            }

            var review = new TutorReview
            {
                ReviewId = dto.ReviewId,
                GroupId = dto.GroupId,
                TutorId = tutorId,
                Feedback = dto.Feedback,
                Mark = dto.Mark,
                ReviewedAt = DateTime.Now
            };
            await _repository.Add(review);

            projectGroup.Status = ProjectStatus.Ongoing;
            await _repository.Save();

            foreach (var student in groupStudents)
            {
                await _notificationService.SendNotification(
                    student.Id,
                    "Project Reviewed",
                    $"Your project group has been reviewed. Feedback: {dto.Feedback}"
                );
            }

            return new ApiResponse<string>("Feedback submitted for all group members.", "Review successful", true);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(null, $"Error reviewing project: {ex.Message}", false);
        }
    }

    public async Task<ApiResponse<string>> FinalReviewGroupProject(TutorReviewDto dto, int tutorId)
    {
        try
        {
            var projectGroup = await _repository.GetProjectGroupById(dto.GroupId);
            if (projectGroup == null)
                return new ApiResponse<string>(null, $"Group with ID {dto.GroupId} does not exist.", false);

            if (projectGroup.TutorId != tutorId)
                return new ApiResponse<string>(null, "Unauthorized: You are not assigned to this group.", false);

            var groupStudents = await _repository.GetStudentsByGroupId(dto.GroupId);
            if (!groupStudents.Any())
                return new ApiResponse<string>(null, "No students found in this group.", false);

            var finalProjects = await _repository.GetFinalProjectsByGroupId(dto.GroupId);
            var submittedStudentIds = finalProjects.Select(p => p.StudentId).ToHashSet();
            var unsubmittedStudents = groupStudents
                .Where(s => !submittedStudentIds.Contains(s.Id))
                .ToList();

            if (unsubmittedStudents.Any())
            {
                var names = string.Join(", ", unsubmittedStudents.Select(s => s.Name));
                return new ApiResponse<string>(null, $"Final review not allowed. These students haven't submitted their final project: {names}", false);
            }

            var review = new TutorReview
            {
                ReviewId = dto.ReviewId,
                GroupId = dto.GroupId,
                TutorId = tutorId,
                Feedback = dto.Feedback,
                Mark = dto.Mark,
                ReviewedAt = DateTime.Now,
            };
            await _repository.Add(review);

            projectGroup.Status = ProjectStatus.Completed;
            await _repository.Save();

            foreach (var student in groupStudents)
            {
                await _notificationService.SendNotification(
                    student.Id,
                    "Final Review Completed",
                    $"Your final project has been reviewed. Feedback: {dto.Feedback}"
                );
            }

            return new ApiResponse<string>(
                "Final review submitted successfully.",
                "Final review completed",
                true
            );
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(null, $"Error during final review: {ex.Message}", false);
        }
    }

    public async Task<ApiResponse<string>> ReviewProjectRequest(int tutorId, ReviewRequestDto dto)
    {
        try
        {
            var request = await _repository.GetRequestById(dto.RequestId);

            if (request == null || request.TutorId != tutorId)
                return new ApiResponse<string>(null, "Invalid request ID or unauthorized", false);

            if (!Enum.TryParse<RequestStatus>(dto.Status, out var newStatus) || newStatus == RequestStatus.Requested)
                return new ApiResponse<string>(null, "Invalid status. Must be Approved or Rejected.", false);

            request.Status = newStatus;
            await _repository.Save();

            await _notificationService.SendNotification(
                request.StudentId,
                "Project Request Reviewed",
                $"Your project request has been {newStatus} by the tutor."
            );

            return new ApiResponse<string>("Status updated", "Success", true);
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(null, $"Error updating request: {ex.Message}", false);
        }
    }

    public async Task<ApiResponse<ICollection<ProjectRequestDetailsDto>>> GetRequestsForTutor(int tutorId)
    {
        try
        {
            var requests = await _repository.GetRequestsForTutor(tutorId);

            var dtoList = requests.Select(r => new ProjectRequestDetailsDto
            {
                ReviewID = r.Id,
                ProjectID = r.ProjectId,
                ProjectTitle = r.ProjectTitle,
                ProjectDescription = r.ProjectDescription,
            }).ToList();

            return new ApiResponse<ICollection<ProjectRequestDetailsDto>>(dtoList, "Requests fetched successfully.", true);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ICollection<ProjectRequestDetailsDto>>(null, $"Error fetching requests: {ex.Message}", false);
        }
    }

}

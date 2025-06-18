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
using System.Text.Json.Serialization;
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
        var groupExists = await _repository.GroupExists(dto.GroupId);
        if (!groupExists)
            return new ApiResponse<string>(null, $"Group with ID {dto.GroupId} does not exist.", false);

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

        var projectGroup = await _repository.GetProjectGroupById(dto.GroupId);
        if (projectGroup != null)
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

    public async Task<ApiResponse<string>> FinalReviewGroupProject(TutorReviewDto dto, int tutorId)
    {
        var groupExists = await _repository.GroupExists(dto.GroupId);
        if (!groupExists)
            return new ApiResponse<string>(null, $"Group with ID {dto.GroupId} does not exist.", false);

        var groupStudents = await _repository.GetStudentsByGroupId(dto.GroupId);
        if (!groupStudents.Any())
            return new ApiResponse<string>(null, "No students found in this group.", false);

        var finalProjects = await _repository.GetFinalProjectsByGroupId(dto.GroupId);
        var submittedStudentIds = finalProjects.Select(p => p.StudentId).ToHashSet();
        var unsubmittedStudents = groupStudents.Where(s => !submittedStudentIds.Contains(s.Id)).ToList();

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

        var projectGroup = await _repository.GetProjectGroupById(dto.GroupId);
        if (projectGroup != null)
        {
            projectGroup.Status = ProjectStatus.Completed;
            _repository.UpdateProjectGroup(projectGroup); 
        }
     

        await _repository.Save();


        foreach (var student in groupStudents)
        {
            await _notificationService.SendNotification(
                student.Id,
                "Final Review Completed",
                $"Your final project has been reviewed. Feedback: {dto.Feedback}"
            );
        }

        return new ApiResponse<string>("Final review submitted successfully.", "Final review completed", true);
    }

    public async Task<ApiResponse<string>> ReviewProjectRequest(int tutorId, ReviewRequestDto dto)
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

    public async Task<ApiResponse<ICollection<ProjectRequestDetailsDto>>> GetRequestsForTutor(int tutorId)
    {
        var requests = await _repository.GetRequestsForTutor(tutorId);

        var dtoList = requests.Select(r => new ProjectRequestDetailsDto
        {
            ReviewID = r.Id,
            ProjectTitle = r.ProjectTitle,
            ProjectDescription = r.ProjectDescription,
        }).ToList();

        return new ApiResponse<ICollection<ProjectRequestDetailsDto>>(dtoList, "Requests fetched successfully.", true);
    }
}

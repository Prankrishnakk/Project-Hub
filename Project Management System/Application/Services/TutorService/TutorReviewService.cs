using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
using Domain.Enum;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TutorReviewService : ITutorReviewService
{
    private readonly ITutorReviewRepository _repository;

    public TutorReviewService(ITutorReviewRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<string>> ReviewGroupProject(TutorReviewDto dto, int tutorId)
    {
        var groupExists = await _repository.GroupExists(dto.GroupId);
        if (!groupExists)
        {
            return new ApiResponse<string>(null, $"Group with ID {dto.GroupId} does not exist.", false);
        }

        var groupStudents = await _repository.GetStudentsByGroupId(dto.GroupId);
        if (!groupStudents.Any())
        {
            return new ApiResponse<string>(null, "No students found in this group.", false);
        }

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
        {
            projectGroup.Status = Domain.Enum.ProjectStatus.Ongoing;
        }

        await _repository.Save();

        return new ApiResponse<string>("Feedback submitted for all group members.", "Review successful", true);
    }

    public async Task<ApiResponse<string>> FinalReviewGroupProject(TutorReviewDto dto, int tutorId)
    {
        var groupExists = await _repository.GroupExists(dto.GroupId);
        if (!groupExists)
        {
            return new ApiResponse<string>(null, $"Group with ID {dto.GroupId} does not exist.", false);
        }

        var groupStudents = await _repository.GetStudentsByGroupId(dto.GroupId);
        if (!groupStudents.Any())
        {
            return new ApiResponse<string>(null, "No students found in this group.", false);
        }

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
        Console.WriteLine(projectGroup.Status);
        await _repository.Save();

        return new ApiResponse<string>("Final review submitted successfully.", "Final review completed", true);
    }

}

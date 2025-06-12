using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
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
        // Step 1: Check if group exists
        var groupExists = await _repository.GroupExists(dto.GroupId);
        if (!groupExists)
        {
            return new ApiResponse<string>(null, $"Group with ID {dto.GroupId} does not exist.", false);
        }

        // Step 2: Get all students in the group
        var groupStudents = await _repository.GetStudentsByGroupId(dto.GroupId);
        if (!groupStudents.Any())
        {
            return new ApiResponse<string>(null, "No students found in this group.", false);
        }

        // Step 3: Get projects submitted by students in the group
        var studentProjects = await _repository.GetStudentProjectsByGroupId(dto.GroupId);

        // Step 4: Check if every student has submitted a project
        var submittedStudentIds = studentProjects.Select(p => p.StudentId).ToHashSet();
        var unsubmittedStudents = groupStudents.Where(s => !submittedStudentIds.Contains(s.Id)).ToList();

        if (unsubmittedStudents.Any())
        {
            var names = string.Join(", ", unsubmittedStudents.Select(s => s.Name));
            return new ApiResponse<string>(null, $"Review not allowed. The following students haven't submitted their project: {names}", false);
        }

        // Step 5: Proceed with review
        var review = new TutorReview
        {
            GroupId = dto.GroupId,
            TutorId = tutorId,
            Feedback = dto.Feedback,
            ReviewedAt = DateTime.Now
        };

        await _repository.Add(review);

        return new ApiResponse<string>("Feedback submitted for all group members.", "Review successful", true);
    }
}

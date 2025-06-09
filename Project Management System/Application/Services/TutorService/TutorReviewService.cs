using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
using Domain.Model;
using System;
using System.Threading.Tasks;

public class TutorReviewService : ITutorReviewService
{
    private readonly ITutorReviewRepository _repository;

    public TutorReviewService(ITutorReviewRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<TutorReviewDto>> ReviewProject(TutorReviewDto dto, int tutorId)
    {
        var studentProjectExists = await _repository.StudentProjectExists(dto.StudentProjectId);
        if (!studentProjectExists)
        {
            return new ApiResponse<TutorReviewDto>(
                data: null,
                message: $"Student project with ID {dto.StudentProjectId} does not exist.",
                success: false
            );
        }
        var now = DateTime.Now;

        var review = new TutorReview
        {
            StudentProjectId = dto.StudentProjectId,
            TutorId = tutorId,
            Feedback = dto.Feedback,
            ReviewedAt = now,
         
        };

        await _repository.Add(review);

        return new ApiResponse<TutorReviewDto>(
            data: null,
            message: "Project reviewed successfully.",
            success: true
        );
    }

   
}

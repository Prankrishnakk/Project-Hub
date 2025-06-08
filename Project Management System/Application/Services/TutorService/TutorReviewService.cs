using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
using Domain.Model;

public class TutorReviewService : ITutorReviewService
{
    private readonly ITutorReviewRepository _repository;

    public TutorReviewService(ITutorReviewRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<TutorReviewDto>> ReviewProjectAsync(TutorReviewDto dto)
    {
       
        var studentProjectExists = await _repository.StudentProjectExists(dto.StudentProjectId);
        if (!studentProjectExists)
        {
            return new ApiResponse<TutorReviewDto>(
                data: null,
                message: $"StudentProject with Id {dto.StudentProjectId} does not exist.",
                success: false
            );
        }

        var review = new TutorReview
        {
            StudentProjectId = dto.StudentProjectId,
            TutorId = dto.TutorId,
            Feedback = dto.Feedback,
            ReviewedAt = DateTime.Now,
            WeekStartDate = GetCurrentWeekStartDate()
        };

        await _repository.Add(review);

        return new ApiResponse<TutorReviewDto>(
            data: dto,
            message: "Project reviewed successfully",
            success: true
        ); ; ;
    }

    private DateTime GetCurrentWeekStartDate()
    {
        var today = DateTime.Today;
        int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        return today.AddDays(-diff);
    }



}
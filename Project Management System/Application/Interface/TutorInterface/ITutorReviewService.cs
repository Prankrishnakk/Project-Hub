using Application.ApiResponse;
using Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interface.TutorInterface
{
    public interface ITutorReviewService
    {
        Task<ApiResponse<string>> ReviewGroupProject(TutorReviewDto dto, int tutorId);
        Task<ApiResponse<string>> FinalReviewGroupProject(TutorReviewDto dto, int tutorId);
        Task<ApiResponse<ICollection<ProjectRequestDetailsDto>>> GetRequestsForTutor(int tutorId);
        Task<ApiResponse<string>> ReviewProjectRequest(int tutorId, ReviewRequestDto dto);
    }
}

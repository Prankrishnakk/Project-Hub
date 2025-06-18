using Application.ApiResponse;
using Application.Dto;
using System.Threading.Tasks;


namespace Application.Interface.StudentInterface
{
    public interface IStudentProjectService
    {
        Task<ApiResponse<string>> UploadProject(int studentId, FileUploadDto dto);
        Task<ApiResponse<string>> UploadFinalProject(int studentId, FileUploadDto dto);
        Task<ApiResponse<string>> SubmitProjectRequest(ProjectRequestDto dto, int studentId);
        Task<ApiResponse<List<ReviewRequestDto>>> GetReviewedRequests(int studentId);
    }
}

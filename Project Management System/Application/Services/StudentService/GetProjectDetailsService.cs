using Application.ApiResponse;
using Application.Dto;
using Application.Interface.StudentInterface;
using System;
using System.Threading.Tasks;

namespace Application.Services.StudentServices
{
    public class GetProjectDetailsService : IGetProjectDetailsService
    {
        private readonly IGetProjectDetailsRepository _repository;

        public GetProjectDetailsService(IGetProjectDetailsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<MyGroupProjectSimpleDto>> GetMyGroupAndProjects(int studentId)
        {
            try
            {
                var result = await _repository.GetStudentGroupAndProjects(studentId);

                if (result == null)
                    return new ApiResponse<MyGroupProjectSimpleDto>(null, "No group or project found.", false);

                return new ApiResponse<MyGroupProjectSimpleDto>(result, "Group and projects fetched successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<MyGroupProjectSimpleDto>(null, $"An error occurred: {ex.Message}", false);
            }
        }
    }
}

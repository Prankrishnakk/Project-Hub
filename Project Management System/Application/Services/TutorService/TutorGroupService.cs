using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.TutorService
{
    public class TutorGroupService : ITutorGroupService
    {
        private readonly ITutorGroupRepository _repository;

        public TutorGroupService(ITutorGroupRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<ICollection<TutorGroupDto>>> GetMyGroups(int tutorId)
        {
            try
            {
                var result = await _repository.GetGroupsByTutor(tutorId);

                if (result == null || result.Count == 0)
                    return new ApiResponse<ICollection<TutorGroupDto>>(null, "No groups found for this tutor.", false);

                return new ApiResponse<ICollection<TutorGroupDto>>(result, "Tutor groups fetched successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ICollection<TutorGroupDto>>(null, $"An error occurred: {ex.Message}", false);
            }
        }
    }
}

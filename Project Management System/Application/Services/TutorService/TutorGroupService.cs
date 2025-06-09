using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var result = await _repository.GetGroupsByTutor(tutorId);
            return new ApiResponse<ICollection<TutorGroupDto>>(result, "Tutor groups fetched successfully", true);
        }
    }

}

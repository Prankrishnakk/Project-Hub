using Application.ApiResponse;
using Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.TutorInterface
{
    public interface ITutorGroupService
    {
        Task<ApiResponse<ICollection<TutorGroupDto>>> GetMyGroups(int tutorId);
    }

}

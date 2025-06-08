using Application.ApiResponse;
using Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.TutorInterface
{
    public interface IProjectGroupService
    {
        Task<ApiResponse<string>> CreateProjectGroup(ProjectGroupCreateDto dto, int tutorId);
        Task<ApiResponse<string>> UpdateProjectGroup(int groupId, ProjectGroupCreateDto dto, int tutorId);
        Task<ApiResponse<string>> DeleteProjectGroup(int groupId);
    }
}

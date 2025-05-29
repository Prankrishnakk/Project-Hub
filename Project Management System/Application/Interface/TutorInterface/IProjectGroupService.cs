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
        Task<string> CreateProjectGroupAsync(ProjectGroupCreateDto dto);
        Task<string> UpdateProjectGroupAsync(int groupId, ProjectGroupCreateDto dto);
        Task<string> DeleteProjectGroupAsync(int groupId);
    }
}

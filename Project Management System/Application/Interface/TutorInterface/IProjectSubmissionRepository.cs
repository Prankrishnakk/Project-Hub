using Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.TutorInterface
{
    public interface IProjectSubmissionRepository
    {
        Task<ICollection<ProjectByGroupDto>> GetProjectsByGroup(int groupId, int tutorId);
    }

}

using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.TutorInterface
{
    public interface IProjectGroupRepository
    {
        Task<List<Student>> GetUngroupedStudentsAsync(List<int> studentIds);
        Task AddProjectGroupAsync(ProjectGroup group);
        Task SaveAsync();
        Task<List<Student>> GetUngroupedOrBelongToGroupAsync(List<int> studentIds, int groupId);
        Task UpdateProjectGroupAsync(ProjectGroup group);
        Task<ProjectGroup> GetProjectGroupByIdAsync(int groupId);
    }
}

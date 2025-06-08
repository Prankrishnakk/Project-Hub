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
        Task<Student> GetStudentByIdAsync(int id);
        Task<List<Student>> GetUngroupedStudentsAsync(List<int> studentIds);
        Task<List<Student>> GetUngroupedOrBelongToGroupAsync(List<int> studentIds, int groupId);
        Task UpdateProjectGroupAsync(ProjectGroup group);
        Task<ProjectGroup> GetProjectGroupByIdAsync(int groupId);
        Task DeleteProjectGroupAsync(ProjectGroup group);
        Task AddProjectGroupAsync(ProjectGroup group);
        Task SaveAsync();
    }
}

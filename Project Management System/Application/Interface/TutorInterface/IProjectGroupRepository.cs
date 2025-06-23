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
        Task<Student> GetStudentById(int id);
        Task<List<Student>> GetUngroupedStudents(List<int> studentIds);
        Task<List<Student>> GetUngroupedOrBelongToGroup(List<int> studentIds, int groupId);
        Task<List<ProjectRequest>> GetApprovedRequestsForStudentsAndTutor(List<int> studentIds, int tutorId);
        Task UpdateProjectGroup(ProjectGroup group);
        Task<Project> GetProjectById(int projectId);    
        Task<ProjectGroup> GetProjectGroupById(int groupId);
        Task DeleteProjectGroup(ProjectGroup group);
        Task AddProjectGroup(ProjectGroup group);
        Task Save();
    }
}

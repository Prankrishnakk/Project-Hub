using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.StudentInterface
{
    public interface IStudentProjectRepository
    {
        Task<StudentProject> GetByStudentId(int studentId);
        Task<Student> GetStudentById(int studentId);
        Task<Project> GetProjectById(int projectId);
        Task SaveProjectGroupRequest(ProjectRequest request);
        Task<ProjectRequest?> GetPendingRequestByStudentId(int studentId);
        Task<List<ProjectRequest>> GetReviewedRequestsByStudentId(int studentId);
        Task<ProjectRequest> GetPendingRequestByStudentAndTutor(int studentId, int tutorId);
        Task<Student?> GetStudentWithGroup(int studentId);
        Task Add(StudentProject project);
      
    }
}

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
        Task SaveProjectGroupRequest(ProjectRequest request);
        Task<ProjectRequest?> GetPendingRequestByStudentId(int studentId);
        Task Add(StudentProject project);
    }
}

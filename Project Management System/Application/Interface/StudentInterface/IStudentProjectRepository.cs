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
        Task<StudentProject> GetByStudentIdAsync(int studentId);
        Task<Student> GetStudentByIdAsync(int studentId);
        Task AddAsync(StudentProject project);
    }
}

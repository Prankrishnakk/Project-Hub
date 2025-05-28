using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.AuthInterface
{
    public interface IStudentAuthRepository
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task AddAsync(Student student);
        Task<Student> GetByEmailAsync(string email);
    }
}

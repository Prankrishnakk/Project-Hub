using Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.AuthInterface
{
    public interface IStudentAuthService
    {
        Task<string> Register(StudentRegDto dto);
        Task<AuthResponseDto> Login(StudentLoginDto dto);
    }
}

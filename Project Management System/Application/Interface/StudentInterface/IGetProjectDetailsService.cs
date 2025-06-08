using Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.StudentInterface
{
    public interface IGetProjectDetailsService
    {
        Task<MyGroupProjectSimpleDto> GetMyGroupAndProjects(int studentId);
    }
}

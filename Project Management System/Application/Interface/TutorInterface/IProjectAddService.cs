using Application.ApiResponse;
using Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.TutorInterface
{
    public interface IProjectAddService
    {
        Task<ApiResponse<string>> AddProject(ProjectCreateDto dto);
    }
}

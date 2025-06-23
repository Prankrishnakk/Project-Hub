using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.TutorService
{
    public class ProjectAddService : IProjectAddService
    {
        private readonly IProjectAddRepository _repository;

        public ProjectAddService(IProjectAddRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<string>> AddProject(ProjectCreateDto dto)
        {
            try
            {
                var project = new Project
                {
                    Title = dto.Title,
                };

                await _repository.AddProject(project);
                await _repository.Save();

                return new ApiResponse<string>("Project added successfully.", "Success", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null, $"Error adding project: {ex.Message}", false);
            }
        }
    }
}

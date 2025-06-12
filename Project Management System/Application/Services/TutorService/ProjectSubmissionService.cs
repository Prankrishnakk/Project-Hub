using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.TutorService
{
    public class ProjectSubmissionService : IProjectSubmissionService
    {
        private readonly IProjectSubmissionRepository _repository;

        public ProjectSubmissionService(IProjectSubmissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<ICollection<ProjectByGroupDto>>> GetProjectsByGroup(int groupId, int tutorId)
        {
            var projects = await _repository.GetProjectsByGroup(groupId, tutorId);

            if (projects.Count == 0)
            {
                return new ApiResponse<ICollection<ProjectByGroupDto>>(null, "No projects found.", false);
            }

            return new ApiResponse<ICollection<ProjectByGroupDto>>(projects, "Projects fetched successfully.", true);
        }
    }

}

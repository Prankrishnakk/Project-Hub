using Application.Dto;
using Application.Interface.HodInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.HodService
{
    public class GetDepartmentGroupsService : IGetDepartmentGroupsService
    {
        private readonly IGetDepartmentGroupsRepository _repository;

        public GetDepartmentGroupsService(IGetDepartmentGroupsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<DepartmentProjectGroupDto>> GetGroupsByDepartment(string department)
        {
            try
            {
                var groups = await _repository.FetchGroupsByDepartmentAsync(department);

                return groups.Select(g => new DepartmentProjectGroupDto
                {
                    GroupId = g.Id,
                    GroupName = g.GroupName,
                    TutorName = g.Tutor?.Name ?? "Unknown",
                    StudentNames = g.Students.Select(s => s.Name).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to fetch project groups for department '{department}': {ex.Message}", ex);
            }
        }

        public async Task<ICollection<CompletedProjectDto>> GetCompletedProjectsByDepartment(string department)
        {
            try
            {
                var projects = await _repository.FetchCompletedProjectsByDepartmentAsync(department);

                return projects.Select(p => new CompletedProjectDto
                {
                    ProjectId = p.Id,
                    GroupName = p.GroupName,
                    TutorName = p.Tutor?.Name ?? "Unknown",
                    StudentNames = p.Students.Select(s => s.Name).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to fetch completed projects for department '{department}': {ex.Message}", ex);
            }
        }
    }
}

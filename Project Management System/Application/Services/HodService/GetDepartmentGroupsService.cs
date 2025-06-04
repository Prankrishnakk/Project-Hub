using Application.Dto;
using Application.Interface.HodInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<List<DepartmentProjectGroupDto>> GetGroupsByDepartment(string department)
        {
            var groups = await _repository.FetchGroupsByDepartmentAsync(department);
            Console.WriteLine(groups);
            return groups.Select(g => new DepartmentProjectGroupDto
            
            {
                GroupId = g.Id,
                GroupName = g.GroupName,
                ProjectTitle = g.ProjectTitle,
                TutorName = g.Tutor?.Name ?? "Unknown",
                StudentNames = g.Students.Select(s => s.Name).ToList()
            }).ToList();
            Console.WriteLine(groups);
        }

    }
}

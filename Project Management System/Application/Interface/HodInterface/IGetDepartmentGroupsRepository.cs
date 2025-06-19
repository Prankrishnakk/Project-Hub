using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.HodInterface
{
    public interface IGetDepartmentGroupsRepository
    {
        Task<List<ProjectGroup>> FetchGroupsByDepartmentAsync(string department);
        Task<List<ProjectGroup>> FetchCompletedProjectsByDepartmentAsync(string department);
    }
}

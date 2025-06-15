using Application.Dto;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.AdminInterface
{
    public interface IAdminUserRepository
    {
        Task<List<string>> GetUsersByRoleAndDepartment(string role, string department);
        Task<List<ProjectGroup>> GetAllGroupsWithStatus(string department);
        Task<ProjectGroup?> GetGroupById(int groupId);
        Task<bool> DeleteGroup(ProjectGroup group);
        Task<Student?> GetById(int id);
        Task<bool> Update(Student user);
        Task<bool> Delete(Student user);
    }
}

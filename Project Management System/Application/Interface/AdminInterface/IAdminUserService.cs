using Application.ApiResponse;
using Application.Dto;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.AdminInterface
{
    public interface IAdminUserService
    {
        Task<ApiResponse<ICollection<string>>> GetUsersByRoleAndDepartment(string role, string department);
        Task<ApiResponse<string>> ChangeUserRole(int userId, string newRole);
        Task<ApiResponse<string>> UserBlockStatus(int userId, bool block);
        Task<ApiResponse<ICollection<AdminGroupSummaryDto>>> GetAllGroups(string department);
        Task<ApiResponse<string>> DeleteGroup(int groupId);
        Task<ApiResponse<string>> DeleteUser(int id);
    }
}


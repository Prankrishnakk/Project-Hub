using Application.ApiResponse;
using Application.Dto;
using Application.Interface.AdminInterface;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AdminService
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAdminUserRepository _repository;

        public AdminUserService(IAdminUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<ApiResponse<ICollection<string>>> GetUsersByRoleAndDepartment(string role, string department)
        {
            try
            {
       
                if (string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(department))
                {
                    return new ApiResponse<ICollection<string>>(null, "Role and department must be provided.", false);
                }

               
                var validRoles = new List<string> { "Student", "Tutor", "HOD" };
                if (!validRoles.Contains(role))
                {
                    return new ApiResponse<ICollection<string>>(null, $"Invalid role. Allowed roles are: {string.Join(", ", validRoles)}.", false);
                }

          
                var users = await _repository.GetUsersByRoleAndDepartment(role, department);

                if (users == null || users.Count == 0)
                {
                    return new ApiResponse<ICollection<string>>(new List<string>(), "No users found for the given role and department.", true);
                }

                return new ApiResponse<ICollection<string>>(users, "Fetched users successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ICollection<string>>(null, $"An error occurred: {ex.Message}", false);
            }
        }
        public async Task<ApiResponse<ICollection<AdminGroupSummaryDto>>> GetAllGroups(string department)
        {
            try
            {
                var groups = await _repository.GetAllGroupsWithStatus(department);

                var result = groups.Select(g => new AdminGroupSummaryDto
                {
                    GroupId = g.Id,
                    GroupName = g.GroupName,
                    Status = (Domain.Enum.ProjectStatus)g.Status,
                    TutorName = g.Tutor?.Name ?? "Unassigned",
                    StudentNames = g.Students.Select(s => s.Name).ToList()
                }).ToList();

                return new ApiResponse<ICollection<AdminGroupSummaryDto>>(result, "Groups fetched successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ICollection<AdminGroupSummaryDto>>(null, $"Error: {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<string>> DeleteUser(int id)
        {
            try
            {
                var user = await _repository.GetById(id);

                if (user == null)
                {
                    return new ApiResponse<string>(null, "User not found", false);
                }

                var deleted = await _repository.Delete(user);

                if (deleted)
                {
                    return new ApiResponse<string>("User deleted successfully", "Success", true);
                }
                else
                {
                    return new ApiResponse<string>(null, "Delete failed", false);
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null, $"Error: {ex.Message}", false);
            }
        }
        public async Task<ApiResponse<string>> DeleteGroup(int groupId)
        {
            try
            {
                var group = await _repository.GetGroupById(groupId);
                if (group == null)
                    return new ApiResponse<string>(null, "Group not found", false);

                var deleted = await _repository.DeleteGroup(group);
                if (deleted)
                    return new ApiResponse<string>("Group deleted successfully", "Success", true);
                else
                    return new ApiResponse<string>(null, "Failed to delete group", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null, $"Error: {ex.Message}", false);
            }
        }





    }
}

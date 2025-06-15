using Application.Dto;
using Application.Interface.AdminInterface;
using Domain.Model;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.AdminRepository
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly AppDbContext _context;

        public AdminUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetUsersByRoleAndDepartment(string role, string department)
        {
            return await _context.Students
                .Where(s => s.Role == role && s.Department == department)
                .Select(s => s.Name)
                .ToListAsync();
        }
        public async Task<Student?> GetById(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<bool> Delete(Student user)
        {
            _context.Students.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<ProjectGroup>> GetAllGroupsWithStatus(string department)
        {
            return await _context.ProjectGroups
                .Include(g => g.Tutor)
                .Include(g => g.Students)
                .Where(g => g.Tutor != null && g.Tutor.Department == department)
                .ToListAsync();
        }
        public async Task<ProjectGroup?> GetGroupById(int groupId)
        {
            return await _context.ProjectGroups.FindAsync(groupId);
        }

        public async Task<bool> DeleteGroup(ProjectGroup group)
        {
            _context.ProjectGroups.Remove(group);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> Update(Student user)
        {
            _context.Students.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}


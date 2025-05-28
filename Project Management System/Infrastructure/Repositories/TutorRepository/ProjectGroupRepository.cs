using Application.Interface.TutorInterface;
using Domain.Model;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.TutorRepository
{

    public class ProjectGroupRepository : IProjectGroupRepository
    {
        private readonly AppDbContext _context;

        public ProjectGroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetUngroupedStudentsAsync(List<int> studentIds)
        {
            return await _context.Students
                .Where(s => studentIds.Contains(s.Id) && s.GroupId == null)
                .ToListAsync();
        }

        public async Task<List<Student>> GetUngroupedOrBelongToGroupAsync(List<int> studentIds, int groupId)
        {
            return await _context.Students
                .Where(s => studentIds.Contains(s.Id) &&
                            (s.GroupId == null || s.GroupId == groupId))
                .ToListAsync();
        }

        public async Task<ProjectGroup?> GetProjectGroupByIdAsync(int groupId)
        {
            return await _context.ProjectGroups
                .Include(g => g.Students)
                .FirstOrDefaultAsync(g => g.Id == groupId);
        }

        public async Task AddProjectGroupAsync(ProjectGroup group)
        {
            await _context.ProjectGroups.AddAsync(group);
        }

        public async Task UpdateProjectGroupAsync(ProjectGroup group)
        {
            _context.ProjectGroups.Update(group);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}

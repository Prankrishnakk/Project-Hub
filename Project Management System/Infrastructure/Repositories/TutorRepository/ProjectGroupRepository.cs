using Application.Interface.TutorInterface;
using Domain.Enum;
using Domain.Model;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Student> GetStudentById(int id)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Student>> GetUngroupedStudents(List<int> studentIds)
        {
            return await _context.Students
                .Where(s => studentIds.Contains(s.Id)
                            && s.GroupId == null
                            && s.Role == "Student") 
                .ToListAsync();
        }

        public async Task<List<Student>> GetUngroupedOrBelongToGroup(List<int> studentIds, int groupId)
        {
            return await _context.Students
                .Where(s => studentIds.Contains(s.Id)
                            && (s.GroupId == null || s.GroupId == groupId)
                            && s.Role == "Student") 
                .ToListAsync();
        }

        public async Task<ProjectGroup?> GetProjectGroupById(int groupId)
        {
            return await _context.ProjectGroups
                .Include(g => g.Students)
                .FirstOrDefaultAsync(g => g.Id == groupId);
        }

        public async Task AddProjectGroup(ProjectGroup group)
        {
            await _context.ProjectGroups.AddAsync(group);

        }
        public async Task<List<ProjectRequest>> GetApprovedRequestsForStudentsAndTutor(List<int> studentIds, int tutorId)
        {
            return await _context.ProjectRequests
                .Where(pr => studentIds.Contains(pr.StudentId) &&
                             pr.TutorId == tutorId &&
                             pr.Status == RequestStatus.Approved)
                .ToListAsync();
        }
        public async Task<Project> GetProjectById(int projectId)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        }


        public async Task UpdateProjectGroup(ProjectGroup group)
        {
            _context.ProjectGroups.Update(group);
        }

        public async Task DeleteProjectGroup(ProjectGroup group)
        {
            _context.ProjectGroups.Remove(group);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}

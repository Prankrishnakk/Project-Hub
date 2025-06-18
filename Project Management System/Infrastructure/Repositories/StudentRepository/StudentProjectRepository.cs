using Application.Interface.StudentInterface;
using Domain.Enum;
using Domain.Model;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.StudentRepository
{
    public class StudentProjectRepository : IStudentProjectRepository
    {
        private readonly AppDbContext _context;

        public StudentProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StudentProject> GetByStudentId(int studentId)
        {
            return await _context.StudentProjects
                .FirstOrDefaultAsync(p => p.StudentId == studentId);
        }

        public async Task Add(StudentProject project)
        {
            await _context.StudentProjects.AddAsync(project);
            await _context.SaveChangesAsync();
        }
        public async Task<Student?> GetStudentById(int studentId)
        {
            return await _context.Students.FindAsync(studentId);
        }

        public async Task SaveProjectGroupRequest(ProjectRequest request)
        {
            await _context.ProjectRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }
        public async Task<ProjectRequest?> GetPendingRequestByStudentId(int studentId)
        {
            return await _context.ProjectRequests
                .FirstOrDefaultAsync(r => r.StudentId == studentId && r.Status == RequestStatus.Requested);
        }
        public async Task<Student?> GetStudentWithGroup(int studentId)
        {
            return await _context.Students
                .Include(s => s.Group)
                .FirstOrDefaultAsync(s => s.Id == studentId);
        }
        public async Task<List<ProjectRequest>> GetReviewedRequestsByStudentId(int studentId)
        {
            return await _context.ProjectRequests
                .Where(r => r.StudentId == studentId && r.Status != RequestStatus.Requested)
                .ToListAsync();
        }




    }
}

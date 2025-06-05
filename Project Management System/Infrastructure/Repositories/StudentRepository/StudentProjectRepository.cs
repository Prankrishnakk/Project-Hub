using Application.Interface.StudentInterface;
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

        public async Task<StudentProject> GetByStudentIdAsync(int studentId)
        {
            return await _context.StudentProjects
                .FirstOrDefaultAsync(p => p.StudentId == studentId);
        }

        public async Task AddOrUpdateAsync(StudentProject project)
        {
            var existing = await _context.StudentProjects
                .FirstOrDefaultAsync(p => p.StudentId == project.StudentId);

            if (existing == null)
            {
                await _context.StudentProjects.AddAsync(project);
            }
            else
            {
                existing.FileName = project.FileName;
                existing.FileData = project.FileData;
                existing.ContentType = project.ContentType;
                existing.FileSize = project.FileSize;
                existing.UploadedAt = project.UploadedAt;
                existing.IsReviewed = false;
                existing.Feedback = null;
                existing.ReviewedAt = null;
            }

            await _context.SaveChangesAsync();
        }
    }
}

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

        public async Task AddAsync(StudentProject project)
        {
            await _context.StudentProjects.AddAsync(project);
            await _context.SaveChangesAsync();
        }
        public async Task<Student?> GetStudentByIdAsync(int studentId)
        {
            return await _context.Students.FindAsync(studentId);
        }

        public async Task<StudentProject?> GetFinalSubmissionAsync(int studentId)
        {
            return await _context.StudentProjects
                .FirstOrDefaultAsync(p => p.StudentId == studentId && p.FinalSubmission);
        }

    }
}

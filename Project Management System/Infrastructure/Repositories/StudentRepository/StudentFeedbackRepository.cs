using Application.Dto;
using Application.Interface.StudentInterface;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.StudentRepository
{
    public class StudentFeedbackRepository : IStudentFeedbackRepository
    {
        private readonly AppDbContext _context;

        public StudentFeedbackRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<TutorFeedbackDto>> GetFeedbacksByStudentId(int studentId)
        {
            return await _context.TutorReviews
                .Where(r => r.StudentProject.StudentId == studentId)
                .Select(r => new TutorFeedbackDto
                {
                    Feedback = r.Feedback,
                    ReviewedAt = r.ReviewedAt,
                    TutorName = r.Tutor.Name
                })
                .ToListAsync();
        }
    }
}

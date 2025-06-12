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
            var groupId = await _context.Students
                .Where(s => s.Id == studentId)
                .Select(s => s.GroupId)
                .FirstOrDefaultAsync();

            var feedbacks = await _context.TutorReviews
                .Where(tr => tr.GroupId == groupId)
                .OrderByDescending(tr => tr.ReviewedAt)
                .Select(tr => new TutorFeedbackDto
                {
                    Feedback = tr.Feedback,
                    ReviewedAt = tr.ReviewedAt,
                    TutorName = tr.Tutor.Name
                })
                .Distinct()
                .ToListAsync();

            return feedbacks;
        }

    }
}

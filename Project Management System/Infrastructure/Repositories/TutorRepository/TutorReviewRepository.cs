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
    public class TutorReviewRepository : ITutorReviewRepository
    {
        private readonly AppDbContext _context;

        public TutorReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(TutorReview review)
        {
            await _context.TutorReviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TutorReview>> GetReviewsByTutorId(int tutorId)
        {
            return await _context.TutorReviews
                .Include(r => r.StudentProject)
                .Where(r => r.TutorId == tutorId)
                .ToListAsync();
        }

        public async Task<TutorReview?> GetReviewByProjectId(int projectId)
        {
            return await _context.TutorReviews
                .Include(r => r.Tutor)
                .FirstOrDefaultAsync(r => r.StudentProjectId == projectId);
        }
        public async Task<bool> StudentProjectExists(int studentProjectId)
        {
            return await _context.StudentProjects.AnyAsync(sp => sp.Id == studentProjectId);
        }
     

    }
}

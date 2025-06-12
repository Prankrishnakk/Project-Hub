using Application.Interface.TutorInterface;
using Domain.Model;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<StudentProject>> GetStudentProjectsByGroupId(int groupId)
        {
            return await _context.StudentProjects
                .Where(sp => sp.Student.GroupId == groupId)
                .ToListAsync();
        }

        public async Task<bool> GroupExists(int groupId)
        {
            return await _context.Students.AnyAsync(s => s.GroupId == groupId);
        }

        public async Task<List<TutorReview>> GetReviewsByTutorId(int tutorId)
        {
            return await _context.TutorReviews
                .Include(r => r.Tutor)
                .Where(r => r.TutorId == tutorId)
                .ToListAsync();
        }

        public async Task<List<Student>> GetStudentsByGroupId(int groupId)
        {
            return await _context.Students
                .Where(s => s.GroupId == groupId)
                .ToListAsync();
        }
        public async Task Add(TutorReview review)
        {
            await _context.TutorReviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

    }
}

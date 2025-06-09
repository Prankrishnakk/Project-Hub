using Application.Dto;
using Application.Interface.TutorInterface;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.TutorRepository
{
    public class TutorGroupRepository : ITutorGroupRepository
    {
        private readonly AppDbContext _context;

        public TutorGroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<TutorGroupDto>> GetGroupsByTutor(int tutorId)
        {
            return await _context.ProjectGroups
                .Where(g => g.TutorId == tutorId)
                .Select(g => new TutorGroupDto
                {
                    GroupId = g.Id,
                    GroupName = g.GroupName,
                    StudentNames = g.Students.Select(s => s.Name).ToList()
                })
                .ToListAsync();
        }
    }

}

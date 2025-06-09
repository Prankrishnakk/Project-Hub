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
    public class ProjectSubmissionRepository : IProjectSubmissionRepository
    {
        private readonly AppDbContext _context;

        public ProjectSubmissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ProjectByGroupDto>> GetProjectsByGroup(int groupId, int tutorId)
        {
            var isGroupOwnedByTutor = await _context.ProjectGroups
                .AnyAsync(g => g.Id == groupId && g.TutorId == tutorId);

            if (!isGroupOwnedByTutor)
                return new List<ProjectByGroupDto>();

            return await _context.StudentProjects
                .Where(p => p.Student.GroupId == groupId)
                .Select(p => new ProjectByGroupDto
                {
                    ProjectId = p.Id,
                    FileName = p.FileName,
                    StudentName = p.Student.Name,
                    SubmittedAt = p.UploadedAt
                })
                .ToListAsync();
        }
    }

}

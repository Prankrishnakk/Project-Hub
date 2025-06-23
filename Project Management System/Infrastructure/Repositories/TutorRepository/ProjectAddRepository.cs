using Application.Interface.TutorInterface;
using Domain.Model;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.TutorRepository
{
    public class ProjectAddRepository : IProjectAddRepository
    {
        private readonly AppDbContext _context;

        public ProjectAddRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddProject(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}

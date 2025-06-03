using Application.Interface.HodInterface;
using Domain.Model;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.HodRepository
{
    public class GetDepartmentGroupsRepository : IGetDepartmentGroupsRepository
    {
        private readonly AppDbContext _context;

        public GetDepartmentGroupsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectGroup>> FetchGroupsByDepartmentAsync(string department)
        {
            return await _context.ProjectGroups
                .Include(pg => pg.Tutor)
                .Include(pg => pg.Students)
                .Where(pg => pg.Tutor.Department == department)
                .ToListAsync();
        }
    }
}

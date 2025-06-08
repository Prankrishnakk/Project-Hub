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
    public class GetProjectDetalisRepository : IGetProjectDetailsRepository
    {
        private readonly AppDbContext _context;
        public GetProjectDetalisRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<MyGroupProjectSimpleDto> GetStudentGroupAndProjects(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.Group)
                    .ThenInclude(g => g.Students)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null || student.Group == null)
                return null;

            var group = student.Group;

            return new MyGroupProjectSimpleDto
            {
                GroupName = group.GroupName,
                MemberNames = group.Students.Select(s => s.Name).ToList(),
                ProjectTitles = new List<string> { group.ProjectTitle }
            };
        }
    }






}



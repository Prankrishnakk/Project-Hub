using Application.Interface.StudentInterface;
using Domain.Enum;
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

        public async Task<StudentProject> GetByStudentId(int studentId)
        {
            return await _context.StudentProjects
                .FirstOrDefaultAsync(p => p.StudentId == studentId);
        }

        public async Task Add(StudentProject project)
        {
            await _context.StudentProjects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task<Student?> GetStudentById(int studentId)
        {
            return await _context.Students.FindAsync(studentId);
        }

        public async Task SaveProjectGroupRequest(ProjectRequest request)
        {
            await _context.ProjectRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<ProjectRequest?> GetPendingRequestByStudentId(int studentId)
        {
            return await _context.ProjectRequests
                .FirstOrDefaultAsync(r => r.StudentId == studentId && r.Status == RequestStatus.Requested);
        }

        public async Task<Student?> GetStudentWithGroup(int studentId)
        {
            return await _context.Students
                .Include(s => s.Group)
                .FirstOrDefaultAsync(s => s.Id == studentId);
        }

        public async Task<List<ProjectRequest>> GetReviewedRequestsByStudentId(int studentId)
        {
            return await _context.ProjectRequests
                .Where(r => r.StudentId == studentId && r.Status != RequestStatus.Requested)
                .ToListAsync();
        }

        public async Task<Project> GetProjectById(int projectId)
        {
            return await _context.Projects
                .Include(p => p.ProjectRequests)
                .Include(p => p.ProjectGroups)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<ProjectRequest> GetPendingRequestByStudentAndTutor(int studentId, int tutorId)
        {
            return await _context.ProjectRequests
                .FirstOrDefaultAsync(r =>
                    r.StudentId == studentId &&
                    r.TutorId == tutorId &&
                    r.Status == RequestStatus.Requested);
        }

        public async Task<ProjectRequest> GetRequestByStudentAndTutor(int studentId, int tutorId)
        {
            return await _context.ProjectRequests
                .FirstOrDefaultAsync(r => r.StudentId == studentId && r.TutorId == tutorId);
        }

        public async Task<bool> HasStudentSubmittedProject(int studentId, int groupId)
        {
            return await _context.StudentProjects
                .AnyAsync(p => p.StudentId == studentId && p.GroupId == groupId);
        }

        public async Task<bool> IsGroupReviewedByTutor(int groupId, int tutorId)
        {
            return await _context.TutorReviews
                .AnyAsync(r => r.GroupId == groupId && r.TutorId == tutorId);
        }

        public async Task<bool> GetUnreviewedProjectSubmissions(int studentId)
        {
            return await _context.StudentProjects
                .AnyAsync(p => p.StudentId == studentId && !p.FinalSubmission && !p.IsReviewed);
        }


        public async Task<bool> HasReviewedLastFinalProject(int studentId)
        {
            var lastFinal = await _context.StudentProjects
                .Where(p => p.StudentId == studentId && p.FinalSubmission)
                .OrderByDescending(p => p.UploadedAt)
                .FirstOrDefaultAsync();

            return lastFinal == null || lastFinal.IsReviewed;
        }


        public async Task<ProjectRequest> GetApprovedRequestByStudentAndTutor(int studentId, int tutorId)
        {
            return await _context.ProjectRequests
                .FirstOrDefaultAsync(r =>
                    r.StudentId == studentId &&
                    r.TutorId == tutorId &&
                    r.Status == RequestStatus.Approved); // ✅ ensure this enum matches your status system
        }

    }



}


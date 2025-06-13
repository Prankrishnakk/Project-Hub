using Application.ApiResponse;
using Application.Dto;
using Application.Interface.StudentInterface;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.StudentServices
{
    public class StudentProjectService : IStudentProjectService
    {
        private readonly IStudentProjectRepository _repository;

        public StudentProjectService(IStudentProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<string>> UploadProject(int studentId, FileUploadDto dto)
        {
            if (dto.ProjectFile == null || dto.ProjectFile.Length == 0)
                return new ApiResponse<string>(null, "No file uploaded",false);

            using var ms = new MemoryStream();
            await dto.ProjectFile.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            var project = new StudentProject
            {
                StudentId = studentId,
                FileName = dto.ProjectFile.FileName,
                FileData = fileBytes,
                ContentType = dto.ProjectFile.ContentType,
                FileSize = dto.ProjectFile.Length,
                UploadedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(project);
            return new ApiResponse<string>(dto.ProjectFile.FileName, "Project uploaded successfully", true );
        }
        public async Task<ApiResponse<string>> UploadFinalProject(int studentId, FileUploadDto dto)
        {
            if (dto.ProjectFile == null || dto.ProjectFile.Length == 0)
                return new ApiResponse<string>(null, "No file uploaded", false);

            var student = await _repository.GetStudentByIdAsync(studentId);
            if (student == null)
                return new ApiResponse<string>(null, "Student not found.", false);

            if (student.GroupId == null)
                return new ApiResponse<string>(null, "Student is not assigned to a project group.", false);

            // Prevent multiple final submissions
            var existingFinal = await _repository.GetFinalSubmissionAsync(studentId);
            if (existingFinal != null)
                return new ApiResponse<string>(null, "Final project already submitted.", false);

            using var ms = new MemoryStream();
            await dto.ProjectFile.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            var finalProject = new StudentProject
            {
                StudentId = studentId,
                FileName = dto.ProjectFile.FileName,
                FileData = fileBytes,
                ContentType = dto.ProjectFile.ContentType,
                FileSize = dto.ProjectFile.Length,
                UploadedAt = DateTime.UtcNow,
                FinalSubmission = true,
               
            };

            await _repository.AddAsync(finalProject);
            return new ApiResponse<string>(dto.ProjectFile.FileName, "Final project submitted successfully.", true);
        }

    }
}

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
                return new ApiResponse<string>(null, "No file uploaded", false);

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

            await _repository.Add(project);
            return new ApiResponse<string>(dto.ProjectFile.FileName, "Project uploaded successfully", true);
        }
        public async Task<ApiResponse<string>> UploadFinalProject(int studentId, FileUploadDto dto)
        {
            if (dto.ProjectFile == null || dto.ProjectFile.Length == 0)
                return new ApiResponse<string>(null, "No file uploaded", false);

            var student = await _repository.GetStudentById(studentId);
            if (student == null)
                return new ApiResponse<string>(null, "Student not found.", false);

            if (student.GroupId == null)
                return new ApiResponse<string>(null, "Student is not assigned to a project group.", false);

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

            await _repository.Add(finalProject);
            return new ApiResponse<string>(dto.ProjectFile.FileName, "Final project submitted successfully.", true);
        }
      public async Task<ApiResponse<string>> SubmitProjectRequest(ProjectRequestDto dto, int studentId)
      {
            try
            {
                var student = await _repository.GetStudentById(studentId);
                var tutor = await _repository.GetStudentById(dto.TutorId);

                if (student == null || tutor == null || tutor.Role != "Tutor")
                    return new ApiResponse<string>(null, "Invalid student or tutor.", false);

                if (student.Department != tutor.Department)
                    return new ApiResponse<string>(null, "Tutor must be from the same department.", false);

                var request = new ProjectRequest
                {
                   
                    StudentId = studentId,
                    TutorId = dto.TutorId,
                    ProjectTitle = dto.ProjectTitle,
                    ProjectDescription = dto.ProjectDescription,
                    Status = Domain.Enum.RequestStatus.Requested
                };

                await _repository.SaveProjectGroupRequest(request);
                return new ApiResponse<string>("Project request submitted successfully.", "Success", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null, $"Error: {ex.Message}", false);
            }
      }

    }
}


    













using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
using AutoMapper;
using Domain.Enum;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.TutorService
{
    public class ProjectGroupService : IProjectGroupService
    {
        private readonly IProjectGroupRepository _repository;
        private readonly IMapper _mapper;

        public ProjectGroupService(IProjectGroupRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateProjectGroup(ProjectGroupCreateDto dto, int tutorId)
        {
            try
            {
                if (dto.StudentIds == null || dto.StudentIds.Count < 2)
                    return new ApiResponse<string>(null, "Project group must contain at least 2 students.", false);

                var tutor = await _repository.GetStudentByIdAsync(tutorId);
                if (tutor == null || tutor.Role != "Tutor")
                    return new ApiResponse<string>(null, "Invalid tutor ID or user is not a tutor.", false);

                var students = await _repository.GetUngroupedStudentsAsync(dto.StudentIds);

                if (students.Count != dto.StudentIds.Count)
                    return new ApiResponse<string>(null, "Some students are already in a group, not found, or have invalid roles.", false);

                if (students.Any(s => s.Department != tutor.Department))
                    return new ApiResponse<string>(null, "All students must be from the same department as the tutor.", false);

                var group = _mapper.Map<ProjectGroup>(dto);
                group.Students = students;
                group.TutorId = tutorId;
                group.Status = ProjectStatus.Assigned;

                await _repository.AddProjectGroupAsync(group);
                await _repository.SaveAsync();

                return new ApiResponse<string>(
                    $"Group '{dto.GroupName}' created with {students.Count} students.",
                    "Project group created successfully.",
                    true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null, $"Error creating project group: {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<string>> UpdateProjectGroup(int groupId, ProjectGroupCreateDto dto, int tutorId)
        {
            try
            {
                var existingGroup = await _repository.GetProjectGroupByIdAsync(groupId);
                if (existingGroup == null)
                    return new ApiResponse<string>(null, "Project group not found.", false);

                var tutor = await _repository.GetStudentByIdAsync(tutorId);
                if (tutor == null || tutor.Role != "Tutor")
                    return new ApiResponse<string>(null, "Invalid tutor ID or user is not a tutor.", false);

                if (!string.IsNullOrWhiteSpace(dto.GroupName))
                    existingGroup.GroupName = dto.GroupName;

                if (!string.IsNullOrWhiteSpace(dto.ProjectTitle))
                    existingGroup.ProjectTitle = dto.ProjectTitle;

                existingGroup.TutorId = tutorId;

                if (dto.StudentIds != null && dto.StudentIds.Count > 0)
                {
                    if (dto.StudentIds.Count < 2)
                        return new ApiResponse<string>(null, "Project group must contain at least 2 students.", false);

                    var students = await _repository.GetUngroupedOrBelongToGroupAsync(dto.StudentIds, groupId);

                    if (students.Count != dto.StudentIds.Count)
                        return new ApiResponse<string>(null, "Some students are already in another group, not found, or have invalid roles.", false);

                    if (students.Any(s => s.Department != tutor.Department))
                        return new ApiResponse<string>(null, "All students must be from the same department as the tutor.", false);

                    existingGroup.Students.Clear();
                    foreach (var student in students)
                        existingGroup.Students.Add(student);
                }

                await _repository.UpdateProjectGroupAsync(existingGroup);
                await _repository.SaveAsync();

                return new ApiResponse<string>(
                    $"Group '{existingGroup.GroupName}' updated successfully with {existingGroup.Students.Count} students.",
                    "Project group updated successfully.",
                    true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null, $"Error updating project group: {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<string>> DeleteProjectGroup(int groupId)
        {
            try
            {
                var group = await _repository.GetProjectGroupByIdAsync(groupId);
                if (group == null)
                    return new ApiResponse<string>(null, "Project group not found.", false);

                await _repository.DeleteProjectGroupAsync(group);
                await _repository.SaveAsync();

                return new ApiResponse<string>("Group deletion executed", "Project group and its students deleted successfully.", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null, $"Error deleting project group: {ex.Message}", false);
            }
        }
    }
}

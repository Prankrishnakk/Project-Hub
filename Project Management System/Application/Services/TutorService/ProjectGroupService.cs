using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
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

        public async Task<ApiResponse<string>> CreateProjectGroup(ProjectGroupCreateDto dto, int TutorId)
        {
            try
            {
                if (dto.StudentIds.Count < 2)
                    return new ApiResponse<string>(null, "Project group must contain at least 2 students.", false);

                var students = await _repository.GetUngroupedStudentsAsync(dto.StudentIds);

                if (students.Count != dto.StudentIds.Count)
                    return new ApiResponse<string>(null, "Some students are already in a group or not found.", false);

                var group = _mapper.Map<ProjectGroup>(dto);
                group.Students = students;
                group.TutorId = TutorId;

                await _repository.AddProjectGroupAsync(group);
                await _repository.SaveAsync();

                return new ApiResponse<string>("Project group created successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null, $"Error creating project group: {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<string>> UpdateProjectGroup(int groupId, ProjectGroupCreateDto dto, int TutorId)
        {
            try
            {
                var existingGroup = await _repository.GetProjectGroupByIdAsync(groupId);
                if (existingGroup == null)
                    return new ApiResponse<string>(null, "Project group not found.", false);

                if (!string.IsNullOrWhiteSpace(dto.GroupName))
                    existingGroup.GroupName = dto.GroupName;

                if (!string.IsNullOrWhiteSpace(dto.ProjectTitle))
                    existingGroup.ProjectTitle = dto.ProjectTitle;

                if (TutorId != 0)
                    existingGroup.TutorId = TutorId;

                if (dto.StudentIds != null && dto.StudentIds.Count > 0)
                {
                    if (dto.StudentIds.Count < 2)
                        return new ApiResponse<string>(null, "Project group must contain at least 2 students.", false);

                    var students = await _repository.GetUngroupedOrBelongToGroupAsync(dto.StudentIds, groupId);

                    if (students.Count != dto.StudentIds.Count)
                        return new ApiResponse<string>(null, "Some students are already in another group or not found.", false);

                    existingGroup.Students.Clear();
                    foreach (var student in students)
                        existingGroup.Students.Add(student);
                }

                await _repository.UpdateProjectGroupAsync(existingGroup);
                await _repository.SaveAsync();

                return new ApiResponse<string>("Project group updated successfully.");
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

                return new ApiResponse<string>("Project group and its students deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null, $"Error deleting project group: {ex.Message}", false);
            }
        }
    }
}

using Application.Dto;
using Application.Interface.TutorInterface;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<string> CreateProjectGroupAsync(ProjectGroupCreateDto dto)
        {
            try
            {
                if (dto.StudentIds.Count < 2)
                   return "Project group must contain at least 2 students.";

                var students = await _repository.GetUngroupedStudentsAsync(dto.StudentIds);

                if (students.Count != dto.StudentIds.Count)
                    return "Some students are already in a group or not found.";

                var group = _mapper.Map<ProjectGroup>(dto);
                group.Students = students;


                await _repository.AddProjectGroupAsync(group);
                await _repository.SaveAsync();

                return "Project group created successfully.";
            }
            catch (Exception ex)
            {
                return $"Error creating project group: {ex.Message}";
            }
        }

        public async Task<string> UpdateProjectGroupAsync(int groupId, ProjectGroupCreateDto dto)
        {
            try
            {
                var existingGroup = await _repository.GetProjectGroupByIdAsync(groupId);
                if (existingGroup == null)
                    return "Project group not found.";

                if (!string.IsNullOrWhiteSpace(dto.GroupName))
                    existingGroup.GroupName = dto.GroupName;

                if (!string.IsNullOrWhiteSpace(dto.ProjectTitle))
                    existingGroup.ProjectTitle = dto.ProjectTitle;

                if (dto.StudentIds != null && dto.StudentIds.Count > 0)
                {
                    if (dto.StudentIds.Count < 2)
                        return "Project group must contain at least 2 students.";

                    var students = await _repository.GetUngroupedOrBelongToGroupAsync(dto.StudentIds, groupId);

                    if (students.Count != dto.StudentIds.Count)
                        return "Some students are already in another group or not found.";

                    existingGroup.Students.Clear();
                    foreach (var student in students)
                    {
                        existingGroup.Students.Add(student);
                    }
                }

                await _repository.UpdateProjectGroupAsync(existingGroup);
                await _repository.SaveAsync();

                return "Project group updated successfully.";
            }
            catch (Exception ex)
            {
                return $"Error updating project group: {ex.Message}";
            }
        }

        public async Task<string> DeleteProjectGroupAsync(int groupId)
        {
            try
            {
                var group = await _repository.GetProjectGroupByIdAsync(groupId);
                if (group == null)
                    return "Project group not found.";

                await _repository.DeleteProjectGroupAsync(group);
                await _repository.SaveAsync();

                return "Project group and its students deleted successfully.";
            }
            catch (Exception ex)
            {
                return $"Error deleting project group: {ex.Message}";
            }
        }
    }

}

using Application.Dto;
using Application.Interface.TutorInterface;
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

        public ProjectGroupService(IProjectGroupRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> CreateProjectGroupAsync(ProjectGroupCreateDto dto)
        {
            if (dto.StudentIds.Count < 4 || dto.StudentIds.Count > 6)
                return "Group must contain 4 to 6 students.";

            var students = await _repository.GetUngroupedStudentsAsync(dto.StudentIds);

            if (students.Count != dto.StudentIds.Count)
                return "Some students are already in a group or not found.";

            var group = new ProjectGroup
            {
                GroupName = dto.GroupName,
                ProjectTitle = dto.ProjectTitle,
                Students = students
            };

            await _repository.AddProjectGroupAsync(group);
            await _repository.SaveAsync();

            return "Project group created successfully.";
        }
        public async Task<string> UpdateProjectGroupAsync(int groupId, ProjectGroupCreateDto dto)
        {
            try
            {
                var existingGroup = await _repository.GetProjectGroupByIdAsync(groupId);
                if (existingGroup == null)
                    return "Project group not found.";

                // Update GroupName if provided
                if (!string.IsNullOrWhiteSpace(dto.GroupName))
                    existingGroup.GroupName = dto.GroupName;

                // Update ProjectTitle if provided
                if (!string.IsNullOrWhiteSpace(dto.ProjectTitle))
                    existingGroup.ProjectTitle = dto.ProjectTitle;

                // Update students only if provided
                if (dto.StudentIds != null && dto.StudentIds.Count > 0)
                {
                    if (dto.StudentIds.Count < 4 || dto.StudentIds.Count > 6)
                        return "Group must contain 4 to 6 students.";

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
                throw new Exception($"Error updating project group: {ex.Message}");
            }
        }

    }
}

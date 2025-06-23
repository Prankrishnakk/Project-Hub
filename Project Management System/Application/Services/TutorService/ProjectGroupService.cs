using Application.ApiResponse;
using Application.Dto;
using Application.Interface.NotificationInterface;
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
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public ProjectGroupService(IProjectGroupRepository repository, IMapper mapper,INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateProjectGroup(ProjectGroupCreateDto dto, int tutorId)
        {
            try
            {
                if (dto.StudentIds == null || dto.StudentIds.Count < 2)
                    return new ApiResponse<string>(null, "Project group must contain at least 2 students.", false);

                var tutor = await _repository.GetStudentById(tutorId);
                if (tutor == null || tutor.Role != "Tutor")
                    return new ApiResponse<string>(null, "Invalid tutor ID or user is not a tutor.", false);

                var approvedRequests = await _repository.GetApprovedRequestsForStudentsAndTutor(dto.StudentIds, tutorId);
                if (approvedRequests.Count != dto.StudentIds.Count)
                    return new ApiResponse<string>(null, "All students must have an approved project request for this tutor.", false);

                var students = await _repository.GetUngroupedStudents(dto.StudentIds);
                if (students.Count != dto.StudentIds.Count)
                    return new ApiResponse<string>(null, "Some students are already in a group, not found, or have invalid roles.", false);

                if (students.Any(s => s.Department != tutor.Department))
                    return new ApiResponse<string>(null, "All students must be from the same department as the tutor.", false);

                var project = await _repository.GetProjectById(dto.ProjectId);
                if (project == null)
                    return new ApiResponse<string>(null, "Invalid Project ID.", false);


                var group = _mapper.Map<ProjectGroup>(dto);
                group.Students = students;
                group.TutorId = tutorId;
                group.ProjectId = dto.ProjectId;  
                group.Status = ProjectStatus.Assigned;


                await _repository.AddProjectGroup(group);
                await _repository.Save();

                
                foreach (var student in students)
                {
                    await _notificationService.SendNotification(
                        student.Id,
                        "Added to Project Group",
                        $"You have been added to the project group '{group.GroupName}' under tutor {tutor.Name}."
                    );
                }

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
                var existingGroup = await _repository.GetProjectGroupById(groupId);
                if (existingGroup == null)
                    return new ApiResponse<string>(null, "Project group not found.", false);

                var tutor = await _repository.GetStudentById(tutorId);
                if (tutor == null || tutor.Role != "Tutor")
                    return new ApiResponse<string>(null, "Invalid tutor ID or user is not a tutor.", false);

                if (dto.ProjectId != 0)
                {
                    var project = await _repository.GetProjectById(dto.ProjectId);
                    if (project == null)
                        return new ApiResponse<string>(null, "Invalid Project ID.", false);

                    existingGroup.ProjectId = dto.ProjectId;
                }

                if (!string.IsNullOrWhiteSpace(dto.GroupName))
                    existingGroup.GroupName = dto.GroupName;

                if (!string.IsNullOrWhiteSpace(dto.ProjectTitle))
                    existingGroup.ProjectTitle = dto.ProjectTitle;

                existingGroup.TutorId = tutorId;

                if (dto.StudentIds != null && dto.StudentIds.Count > 0)
                {
                    if (dto.StudentIds.Count < 2)
                        return new ApiResponse<string>(null, "Project group must contain at least 2 students.", false);

                    var students = await _repository.GetUngroupedOrBelongToGroup(dto.StudentIds, groupId);

                    if (students.Count != dto.StudentIds.Count)
                        return new ApiResponse<string>(null, "Some students are already in another group, not found, or have invalid roles.", false);

                    if (students.Any(s => s.Department != tutor.Department))
                        return new ApiResponse<string>(null, "All students must be from the same department as the tutor.", false);

                    existingGroup.Students.Clear();
                    foreach (var student in students)
                    {
                        existingGroup.Students.Add(student);

                        await _notificationService.SendNotification(
                            student.Id,
                            "Updated Project Group",
                            $"You have been added/updated in the project group '{existingGroup.GroupName}' under tutor {tutor.Name}."
                        );
                    }
                }

                await _repository.UpdateProjectGroup(existingGroup);
                await _repository.Save();

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



        public async Task<ApiResponse<string>> RemoveStudentFromGroup(int groupId, int studentId)
        {
            try
            {
                var group = await _repository.GetProjectGroupById(groupId);
                if (group == null)
                    return new ApiResponse<string>(null, "Project group not found.", false);

                var student = group.Students.FirstOrDefault(s => s.Id == studentId);
                if (student == null)
                    return new ApiResponse<string>(null, "Student not found in this group.", false);

                if (student.ProjectStatus == (int)ProjectStatus.Assigned)
                    return new ApiResponse<string>(null, "Cannot remove student. Project status is not 'Assigned'.", false);

                group.Students.Remove(student);

                await _repository.Save();
                await _notificationService.SendNotification(
                student.Id,"Removed from Project Group",$"You have been removed from the project group '{group.GroupName}'." );

                return new ApiResponse<string>("Student removed", "Student removed from group successfully.", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null, $"Error removing student from group: {ex.Message}", false);
            }
        }
    }
}

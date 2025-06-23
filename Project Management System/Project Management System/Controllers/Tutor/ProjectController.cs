using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project_Management_System.Controllers.Tutor
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectGroupService _service;
        private readonly IProjectAddService _projectService;

        public ProjectController(IProjectGroupService service, IProjectAddService projectService)
        {
            _service = service;
            _projectService = projectService;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddProject([FromBody] ProjectCreateDto dto)
        {
            var response = await _projectService.AddProject(dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    

        [HttpPost("create-Tutor")]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> CreateProjectGroup([FromForm] ProjectGroupCreateDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var response = await _service.CreateProjectGroup(dto, userId);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"Internal Server Error: {ex.Message}", false));
            }
        }

        [HttpPatch("update-Tutor/{groupId}")]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> UpdateProjectGroup(int groupId,[FromBody] ProjectGroupCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new ApiResponse<string>(null, "Invalid input data", false));

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var response = await _service.UpdateProjectGroup(groupId, dto, userId);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"Internal Server Error: {ex.Message}", false));
            }
        }

        [HttpDelete("{groupId}/student/{studentId}")]
         [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> RemoveStudentFromGroup(int groupId, int studentId)
        {
            try
            {
                var response = await _service.RemoveStudentFromGroup(groupId, studentId);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"Internal Server Error: {ex.Message}", false));
            }
        }
    }
}

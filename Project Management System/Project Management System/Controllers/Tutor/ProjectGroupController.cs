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
    public class ProjectGroupController : ControllerBase
    {
        private readonly IProjectGroupService _service;

        public ProjectGroupController(IProjectGroupService service)
        {
            _service = service;
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
        public async Task<IActionResult> UpdateProjectGroup(int groupId, [FromBody] ProjectGroupCreateDto dto)
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

        [HttpDelete("delete-Tutor/{groupId}")]
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> DeleteProjectGroup(int groupId)
        {
            try
            {
                var response = await _service.DeleteProjectGroup(groupId);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"Internal Server Error: {ex.Message}", false));
            }
        }
    }
}

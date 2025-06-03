using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
                var result = await _service.CreateProjectGroupAsync(dto);
                var success = result == "Project group created successfully.";
                var response = new ApiResponse<string>(result, result, success);

                return success ? Ok(response) : BadRequest(response);
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

                var result = await _service.UpdateProjectGroupAsync(groupId, dto);
                bool success = result == "Project group updated successfully.";

                return success
                    ? Ok(new ApiResponse<string>(result, result, true))
                    : BadRequest(new ApiResponse<string>(null, result, false));
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
                var result = await _service.DeleteProjectGroupAsync(groupId);
                bool success = result == "Project group and its students deleted successfully.";

                return success
                    ? Ok(new ApiResponse<string>(result, result, true))
                    : NotFound(new ApiResponse<string>(null, result, false));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"Internal Server Error: {ex.Message}", false));
            }
        }
    }
}

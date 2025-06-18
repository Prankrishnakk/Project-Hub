using Application.Dto;
using Application.Interface.StudentInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Project_Management_System.Controllers.Student
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentProjectController : ControllerBase
    {
        private readonly IStudentProjectService _studentProjectService;

        public StudentProjectController(IStudentProjectService studentProjectService)
        {
            _studentProjectService = studentProjectService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadProject([FromForm] FileUploadDto dto)
        {
            var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (studentIdClaim == null)
                return Unauthorized(new { message = "Invalid token or student not logged in" });

            int studentId = int.Parse(studentIdClaim.Value);


            var response = await _studentProjectService.UploadProject(studentId, dto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpPost("upload-final")]
        public async Task<IActionResult> UploadFinalProject([FromForm] FileUploadDto dto)
        {
            var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (studentIdClaim == null)
                return Unauthorized(new { message = "Invalid token or student not logged in" });

            int studentId = int.Parse(studentIdClaim.Value);

            var response = await _studentProjectService.UploadFinalProject(studentId, dto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpPost("submit-project-request")]
        public async Task<IActionResult> SubmitProjectRequest([FromBody] ProjectRequestDto dto)
        {
            var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (studentIdClaim == null)
                return Unauthorized(new { message = "Invalid token or student not logged in" });

            int studentId = int.Parse(studentIdClaim.Value);

            var response = await _studentProjectService.SubmitProjectRequest(dto, studentId);

            return response.Success ? Ok(response) : BadRequest(response);
        }


        [HttpGet("reviewed-requests")]
        public async Task<IActionResult> GetReviewedRequests()
        {
            var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (studentIdClaim == null)
                return Unauthorized(new { message = "Invalid token or student not logged in" });

            int studentId = int.Parse(studentIdClaim.Value);
            var result = await _studentProjectService.GetReviewedRequests(studentId);

            return result.Success ? Ok(result) : BadRequest(result);
        }



    }
}

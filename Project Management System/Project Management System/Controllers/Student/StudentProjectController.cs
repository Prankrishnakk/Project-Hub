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
            // Get Student ID from JWT Claims
            var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (studentIdClaim == null)
                return Unauthorized(new { message = "Invalid token or student not logged in" });

            int studentId = int.Parse(studentIdClaim.Value);

            // Call the service
            var response = await _studentProjectService.UploadProject(studentId, dto);

            if (!response.Success)
                return BadRequest(response); 

            return Ok(response); 
        }
    }
}

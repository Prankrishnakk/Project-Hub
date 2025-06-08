using Application.Dto;
using Application.Interface.StudentInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project_Management_System.Controllers.Student
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectDetailsController : ControllerBase
    {
        private readonly IGetProjectDetailsService _projectDetailsService;

        public ProjectDetailsController(IGetProjectDetailsService projectDetailsService)
        {
            _projectDetailsService = projectDetailsService;
        }

        [Authorize(Roles = "Student")]
        [HttpGet("my-group")]
        public async Task<IActionResult> GetMyGroupAndProjects()
        {
            var studentIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(studentIdStr, out int studentId))
                return Unauthorized("Invalid or missing student id in token.");

            var result = await _projectDetailsService.GetMyGroupAndProjects(studentId);

            if (result == null)
                return NotFound("Group or project not found for this student.");

            return Ok(result);
        }
    }
}

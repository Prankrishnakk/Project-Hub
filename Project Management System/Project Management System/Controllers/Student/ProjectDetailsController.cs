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
        public async Task<IActionResult> GetMyGroupProject()
        {
            int studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var response = await _projectDetailsService.GetMyGroupAndProjects(studentId);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
    }
}

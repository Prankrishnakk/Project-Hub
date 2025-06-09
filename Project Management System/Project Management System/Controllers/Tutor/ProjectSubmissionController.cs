using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Project_Management_System.Controllers.Tutor
{
    using Application.ApiResponse;
    using Application.Dto;
    using Application.Interface.TutorInterface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using System.Threading.Tasks;

    namespace Project_Management_System.Controllers.Tutor
    {
        [Route("api/[controller]")]
        [ApiController]
        [Authorize(Roles = "Tutor")]
        public class ProjectSubmissionController : ControllerBase
        {
            private readonly IProjectSubmissionService _service;

            public ProjectSubmissionController(IProjectSubmissionService service)
            {
                _service = service;
            }

            [HttpGet("group/{groupId}")]
            public async Task<IActionResult> GetProjectsByGroup(int groupId)
            {
                int tutorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var response = await _service.GetProjectsByGroup(groupId, tutorId);

                return response.Success ? Ok(response) : BadRequest(response);
            }
        }
    }

}

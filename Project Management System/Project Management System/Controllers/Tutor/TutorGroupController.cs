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
    public class TutorGroupController : ControllerBase
    {
        private readonly ITutorGroupService _tutorGroupService;

        public TutorGroupController(ITutorGroupService tutorGroupService)
        {
            _tutorGroupService = tutorGroupService;
        }

        [HttpGet("my-groups")]
        public async Task<IActionResult> GetMyGroups()
        {
            int tutorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var groups = await _tutorGroupService.GetMyGroups(tutorId);
            return Ok(groups);
        }
    }
}

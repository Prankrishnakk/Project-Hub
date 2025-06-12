using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
using Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Project_Management_System.Controllers.Tutor
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorReviewController : ControllerBase
    {
        private readonly ITutorReviewService _tutorReviewService;

        public TutorReviewController(ITutorReviewService tutorReviewService)
        {
            _tutorReviewService = tutorReviewService;
        }

        [HttpPost("review")]
        public async Task<IActionResult> ReviewProject([FromBody] TutorReviewDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse<string>(null, "Invalid data", false));

            int tutorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = await _tutorReviewService.ReviewGroupProject(dto, tutorId);

            return response.Success ? Ok(response) : BadRequest(response);
        }


    }
}


    



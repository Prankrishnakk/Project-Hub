using Application.ApiResponse;
using Application.Dto;
using Application.Interface.TutorInterface;
using Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        // POST: api/TutorReview
        [HttpPost("review")]
        public async Task<IActionResult> ReviewProject([FromBody] TutorReviewDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(null, "Invalid data", false));

            var response = await _tutorReviewService.ReviewProjectAsync(dto);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }



    }
}


    



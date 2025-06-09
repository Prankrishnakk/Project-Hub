using Application.Interface.StudentInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Project_Management_System.Controllers.Student
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class FeedbackController : ControllerBase
    {
        private readonly IStudentFeedbackService _service;

        public FeedbackController(IStudentFeedbackService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyFeedback()
        {
            int studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var feedbacks = await _service.GetMyFeedbacks(studentId);
            return Ok(feedbacks);
        }
    }
}

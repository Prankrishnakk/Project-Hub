using Application.ApiResponse;
using Application.Dto;
using Application.Interface.AuthInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Project_Management_System.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IStudentAuthService _authService;

        public AuthenticationController(IStudentAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm]StudentRegDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);
                return Ok(new ApiResponse<string>(result, "Student registered successfully", true));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(default, ex.Message, false));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm]StudentLoginDto dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(new ApiResponse<AuthResponseDto>(result, "Login successful", true));
            }
            catch (Exception ex)
            {
                return Unauthorized(new ApiResponse<AuthResponseDto>(default, ex.Message, false));
            }
        }

    }
}    

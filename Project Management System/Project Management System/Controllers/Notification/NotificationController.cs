using Application.Dto;
using Application.Interface.NotificationInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ProjectHub.Controllers.Common
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

    
        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _notificationService.GetUserNotifications(userId);
            return Ok(result);
        }

       
        [HttpPost("send")]
        [Authorize] 
        public async Task<IActionResult> Send([FromBody] SendNotificationDto dto)
        {
            await _notificationService.SendNotification(dto.RecipientId, dto.Title, dto.Message);
            return Ok(new { Message = "Notification sent." });
        }

        
    }
}

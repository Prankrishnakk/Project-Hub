using Application.Dto;
using Application.Interface.NotificationInterface;
using Domain.Model;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.SignalR
{
     public class NotificationService : INotificationService     
    {
        private readonly INotificationRepository _repository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(INotificationRepository repository, IHubContext<NotificationHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
        }

        public async Task SendNotification(int recipientId, string title, string message)
        {
            var notification = new Notification
            {
                RecipientId = recipientId,
                Title = title,
                Message = message
            };

            await _repository.Add(notification);
            await _repository.SaveChanges();

            await _hubContext.Clients.User(recipientId.ToString())
                .SendAsync("ReceiveNotification", title, message);
        }

        public async Task<ICollection<NotificationDto>> GetUserNotifications(int userId)
        {
            var list = await _repository.GetByUserId(userId);

            return list.Select(n => new NotificationDto
            {
                Id = n.Id,
                RecipientId = n.RecipientId,
                Title = n.Title,
                Message = n.Message,
                CreatedAt = n.CreatedAt,
           
            }).ToList();
        }
    
        public async Task MarkAsRead(int notificationId)
        {
            var notification = await _repository.GetById(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _repository.SaveChanges();
            }
        }
    }
}

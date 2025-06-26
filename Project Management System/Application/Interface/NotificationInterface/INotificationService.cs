using Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.NotificationInterface
{
    public interface INotificationService
    {
        Task SendNotification(int recipientId, string title, string message);
        Task<ICollection<NotificationDto>> GetUserNotifications(int userId);
    }
}

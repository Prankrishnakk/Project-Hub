using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.NotificationInterface
{
    public interface INotificationRepository
    {
        Task Add(Notification notification);
        Task<ICollection<Notification>> GetByUserId(int userId);
        Task<Notification> GetById(int id);
        Task SaveChanges();
    }
}

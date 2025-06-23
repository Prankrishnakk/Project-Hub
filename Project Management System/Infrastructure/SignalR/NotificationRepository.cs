using Application.Interface.NotificationInterface;
using Domain.Model;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SignalR
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }
        public async Task<ICollection<Notification>> GetByUserId(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.RecipientId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return notifications;
        }


        public async Task<Notification> GetById(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}

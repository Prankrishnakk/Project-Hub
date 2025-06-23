using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task SendMessageToUser(string userId, string title, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", title, message);
        }
    }
}

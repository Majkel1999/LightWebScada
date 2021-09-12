using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FrontEnd.Hubs
{
    public class ViewHub : Hub
    {
        public async Task SendMessage(string message, string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveData", message);
        }

        public Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}

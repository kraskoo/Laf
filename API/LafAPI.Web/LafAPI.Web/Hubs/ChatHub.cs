namespace LafAPI.Web.Hubs
{
    using System;
    using System.Threading.Tasks;

    using LafAPI.Web.Infrastructure.Interfaces;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    [Route("/chat")]
    public class ChatHub : Hub<IChatClient>
    {
        private const string SignalRUsers = "SignalR Users";
        private const string ReceiveMessage = nameof(ReceiveMessage);

        [HubMethodName("MessageTo")]
        public Task DirectMessage(string user, string message)
        {
            return this.Clients.User(user).SendAsync(ReceiveMessage, message);
        }

        public async Task SendMessage(string user, string message)
        {
            await this.Clients.All.ReceiveMessage(user, message);
        }

        public Task SendMessageToCaller(string message)
        {
            return this.Clients.Caller.ReceiveMessage(message);
        }

        public Task SendMessageToGroup(string message)
        {
            return this.Clients.Group(SignalRUsers).ReceiveMessage(message);
        }

        public override async Task OnConnectedAsync()
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, SignalRUsers);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, SignalRUsers);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Server.Web.hubs
{
    public class ChatHub : Hub
    {
        public static class UserHandler
        {
            public static HashSet<string> ConnectedIds = new HashSet<string>();
        }

        public override Task OnConnectedAsync()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            Console.WriteLine("New connection: " + Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            Console.WriteLine("Disconect: " + Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string userName, string message)
        {
            Console.WriteLine("Recebido pelo server: " + message);
            await Clients.All.SendAsync("ReceiveMessage", userName, message, Context.ConnectionId);
        }

        public async Task GetConnectionId()
        {
            Console.WriteLine("Solicitou ID");
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveConnectionID", Context.ConnectionId);
        }

        public async Task MensagemTo(string id, string msg)
        {
            Console.WriteLine("mensagen TO: " + id);
            await Clients.Client(id).SendAsync("ReceiveMessage", Context.ConnectionId, msg, id);
        }

        public async Task GetAllConnectionId()
        {
            var ids = JsonConvert.SerializeObject(UserHandler.ConnectedIds);
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveAllIds", ids);
        }
    }
}
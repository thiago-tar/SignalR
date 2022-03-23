using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SignalR.Client
{
    class Program
    {
        private static string server = "https://localhost:44343/chathub";

        static void Main(string[] args)
        {
            Console.WriteLine("Pess key to connect: " + server);
            Console.ReadLine();

            var connection = new HubConnectionBuilder()
                .WithUrl(server)
                .Build();

            connection.On("ReceiveMessage", (string userName, string message, string connectionId) =>
            {
                Console.WriteLine($"UserName: {userName} : Message {message} Connection: {connectionId}");
            });

            connection.On("ReceiveConnectionID", (string idConnection) =>
             {
                 Console.WriteLine($" ConnectionId: {idConnection}");
             });

            connection.On("ReceiveAllIds", (string jsonIds) =>
            {
                var ids = JsonConvert.DeserializeObject<HashSet<string>>(jsonIds);
                Console.WriteLine(jsonIds);
            });

            while (true)
            {
                using (var conec = connection.StartAsync())
                {
                    conec.Wait();
                    Console.WriteLine(connection.State);
                    while (connection.State == HubConnectionState.Connected)
                    {
                        Console.WriteLine("1 - Send Message All");
                        Console.WriteLine("2 - Obtem Id Connection");
                        Console.WriteLine("3 - Send Message To");
                        Console.WriteLine("4 - Get All Ids");
                        Console.WriteLine("5 - Disconnected");
                        var option = Console.ReadLine();

                        switch (option)
                        {
                            case "1":
                                Console.WriteLine("Send Message");
                                var msg = Console.ReadLine();
                                if (!string.IsNullOrEmpty(msg))
                                {
                                    connection.InvokeCoreAsync("SendMessage", args: new[] { "Thiago", msg });
                                }
                                break;
                            case "2":
                                Console.WriteLine("Obtem Connection ID");
                                connection.InvokeAsync("GetConnectionId");
                                break;
                            case "3":
                                Console.WriteLine("Enviar para");
                                var id = Console.ReadLine();
                                Console.WriteLine("MSG");
                                msg = Console.ReadLine();
                                if (!string.IsNullOrEmpty(id))
                                {
                                    connection.InvokeCoreAsync("MensagemTo", args: new[] { id, msg });
                                    
                                }
                                break;
                            case "4":
                                connection.InvokeAsync("GetAllConnectionId");
                                break;
                            case "5":
                                Console.WriteLine("Desconectando");
                                connection.StopAsync();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalR.Server.Hubs;
using System;

namespace SignalR.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            //app.Use(async (context, next) =>
            //{
            //    if (context.WebSockets.IsWebSocketRequest)
            //    {
            //        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            //        Console.WriteLine("WebSocket Connected");
            //    }
            //    else
            //    {
            //        Console.WriteLine("NEXT");
            //        await next();
            //    }
            //});

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
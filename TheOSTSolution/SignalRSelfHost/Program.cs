using System;

using Owin;

namespace SignalRSelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string _url = "http://localhost:8097";
            using (Microsoft.Owin.Hosting.WebApp.Start(_url))
            {
                Console.WriteLine("Service runing on {0}...", _url);
                Console.ReadLine();
            }
        }
    }

    class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }

    public class MyHub : Microsoft.AspNet.SignalR.Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }
    }
}

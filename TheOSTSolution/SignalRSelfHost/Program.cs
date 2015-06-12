using System;

using Owin;

namespace SignalRSelfHost
{
    /// <summary>
    /// 地址：http://www.asp.net/signalr/overview/deployment/tutorial-signalr-self-host
    /// 1.建项目
    /// 2.安装包
    ///     	Install-Package Microsoft.AspNet.SignalR.SelfHost --This command adds the SignalR 2 Self-Host libraries to the project.
    ///         Install-Package Microsoft.Owin.Cors --This command adds the Microsoft.Owin.Cors library to the project. This library will be used for cross-domain support, which is required for applications that host SignalR and a web page client in different domains. Since you'll be hosting the SignalR server and the web client on different ports, this means that cross-domain must be enabled for communication between these components.
    /// 3.编写Owin.Startup
    /// 4.编写Hub
    /// </summary>

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

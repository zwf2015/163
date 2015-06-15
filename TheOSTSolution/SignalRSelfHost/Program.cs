using System;
using System.Collections.Generic;
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
        public static IDictionary<string, string> users = new Dictionary<string, string>();

        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }

        public void SendMsg(string from, string to, string message)
        {
            if (users.ContainsKey(to))
            {
                Clients.User(users[to]).pushMsg(from, message);
            }
            else
            {
                Clients.Caller.sysMsg(string.Format("消息“{0}”发送失败：未找到 {1} 。", message, to));
            }
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            var _userName = Context.QueryString["userName"];
            if (users.ContainsKey(_userName))
            {
                users[_userName] = Context.ConnectionId;
            }
            else
            {
                users.Add(_userName, Context.ConnectionId);
            }
            Clients.Caller.sysMsg(string.Format("{0} 登录成功。", _userName));
            return base.OnConnected();
        }
    }
}

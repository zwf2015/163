using System.ServiceProcess;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

[assembly: OwinStartup(typeof(TestService.ChatService.Startup))]
namespace TestService
{
    /// <summary>
    /// 聊天服务
    /// 安装 installutil SERVICENAME.exe
    /// 卸载 installutil /u SERVICENAME.exe
    /// </summary>
    public partial class ChatService : ServiceBase
    {
        /// <summary>
        /// 初始化服务。
        /// </summary>
        public ChatService()
        {
            InitializeComponent();
            /// 服务的默认名称是 Service1 ，这里重写名称
            this.ServiceName = "SingalR-Chat-Service";
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            string _url = "http://localhost:65532";
            WebApp.Start(_url);
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        protected override void OnStop()
        {
        }

        /// <summary>
        /// Owin Startup 映射SignalR路由。
        /// the class containing the configuration for the SignalR server (the only configuration this tutorial uses is the call to UseCors), and the call to MapSignalR, which creates routes for any Hub objects in the project.
        /// </summary>
        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.UseCors(CorsOptions.AllowAll);
                app.MapSignalR();
            }
        }

        /// <summary>
        /// MyHub, the SignalR Hub class that the application will provide to clients. 
        /// This class has a single method: Send, that clients will call to broadcast a message to all other connected clients.
        /// </summary>
        public class MyHub : Hub
        {
            public void Send(string name, string message)
            {
                Clients.All.addMessage(name, message);
            }
        }

        ///客户端要为Hub设置指定的URL
        ///<script src="http://localhost:8097/signalr/hubs"></script>
        ///$.connection.hub.url = "http://localhost:8097/signalr";
        ///var chat = $.connection.myHub;
        ///...
    }
}

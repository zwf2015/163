using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalRChat.Startup))]

namespace SignalRChat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
            // Any connection or hub wire up and configuration should go here

            //注册重写的Hub管道错误处理。
            GlobalHost.HubPipeline.AddModule(new MyHubPipelineModule());

            app.MapSignalR();
        }
    }
}

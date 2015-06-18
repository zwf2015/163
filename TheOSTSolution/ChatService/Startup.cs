using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ChatService.Startup))]

namespace ChatService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
            var config = new Microsoft.AspNet.SignalR.HubConfiguration();
            config.EnableJSONP = true;
            //app.MapSignalR(config);

            //使用自己的Hub文件夹
            app.MapSignalR("/SignalRs", config);
        }
    }
}

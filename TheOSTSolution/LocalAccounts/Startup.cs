using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(LocalAccounts.Startup))]

namespace LocalAccounts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //注册Middleware到OWIN管道
            ConfigureAuth(app);
        }
    }
}

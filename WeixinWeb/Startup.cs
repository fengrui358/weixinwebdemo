using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeixinWeb.Startup))]
namespace WeixinWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

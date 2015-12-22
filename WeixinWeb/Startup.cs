using System.Web.Configuration;
using Microsoft.Owin;
using Owin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;
using WeixinWeb.Models.Store;

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

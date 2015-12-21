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

            //注册全局的Key
            AccessTokenContainer.Register(KeyStore.AppId, KeyStore.Secret);
        }

        private void CreateMenus()
        {
            ButtonGroup bg = new ButtonGroup();

            //单击
            bg.button.Add(new SingleClickButton()
            {
                name = "单击测试",
                key = "OneClick",
                type = ButtonType.click.ToString(),//默认已经设为此类型，这里只作为演示
            });

            //二级菜单
            var subButton = new SubButton()
            {
                name = "二级菜单"
            };
            subButton.sub_button.Add(new SingleClickButton()
            {
                key = "SubClickRoot_Text",
                name = "返回文本"
            });
            subButton.sub_button.Add(new SingleClickButton()
            {
                key = "SubClickRoot_News",
                name = "返回图文"
            });
            subButton.sub_button.Add(new SingleClickButton()
            {
                key = "SubClickRoot_Music",
                name = "返回音乐"
            });
            subButton.sub_button.Add(new SingleViewButton()
            {
                url = "http://weixin.senparc.com",
                name = "Url跳转"
            });
            bg.button.Add(subButton);

            var accessToken = AccessTokenContainer.GetAccessToken(KeyStore.AppId);
            var result = CommonApi.CreateMenu(accessToken, bg);
        }
    }
}

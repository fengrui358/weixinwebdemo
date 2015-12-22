using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Senparc.Weixin.MP.CommonAPIs;

namespace WeixinWeb.Models.Store
{
    public class KeyStore
    {
        public static string AppId
        {
            get { return WebConfigurationManager.AppSettings["WeixinAppId"]; }
        }

        public static string Secret
        {
            get { return WebConfigurationManager.AppSettings["WeixinAppSecret"]; }
        }

        public static string AccessToken
        {
            get { return AccessTokenContainer.GetAccessToken(AppId); }
        }

        public static string DemoUrl
        {
            get { return WebConfigurationManager.AppSettings["DemoUrl"]; }
        }
    }
}
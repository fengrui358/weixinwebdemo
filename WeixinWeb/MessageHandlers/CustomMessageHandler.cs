using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.AppStore;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using WeixinWeb.Models.Store;

namespace WeixinWeb.MessageHandlers
{
    public class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        public CustomMessageHandler(Stream inputStream, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) : base(inputStream, postModel, maxRecordCount, developerInfo)
        {
            OmitRepeatedMessage = true;
        }

        public CustomMessageHandler(XDocument requestDocument, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) : base(requestDocument, postModel, maxRecordCount, developerInfo)
        {
            OmitRepeatedMessage = true;
        }

        public CustomMessageHandler(RequestMessageBase requestMessageBase, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) : base(requestMessageBase, postModel, maxRecordCount, developerInfo)
        {
            OmitRepeatedMessage = true;
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型

            StringBuilder sb = new StringBuilder(string.Format("你的OpenId：{0}，你的消息Id：{1}， 你的消息类型{2}， 你的创建时间{3}， 你的消息内容{4}",
                requestMessage.FromUserName, requestMessage.MsgId, requestMessage.MsgType, requestMessage.CreateTime, responseMessage.Content));
            sb.AppendLine();
            sb.AppendLine("回复1：获取用户Token！");
            responseMessage.Content = sb.ToString();

            return responseMessage;
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            

            switch (requestMessage.Content)
            {
                case "1":
                    return GetResponseBy_1(requestMessage);
                    
                case "2":
                    return GetResponseBy_2(requestMessage);
                    
                default:
                    var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();

                    var userInfo = CommonApi.GetUserInfo(KeyStore.AccessToken, requestMessage.FromUserName);
                    StringBuilder sb =
                        new StringBuilder(string.Format("你的OpenId：{0}，你的消息Id：{1}， 你的消息类型{2}， 你的创建时间{3}， 你的消息内容{4}",
                            requestMessage.FromUserName, requestMessage.MsgId, requestMessage.MsgType,
                            requestMessage.CreateTime, requestMessage.Content));

                    var article = new Article();
                    
                    responseMessage.Articles.Add(article);

                    if (userInfo.subscribe != 0)
                    {
                        article.Title = string.Format("你好，{0}！", userInfo.nickname);
                        article.PicUrl = userInfo.headimgurl;
                        article.Url = string.Format("{0}Home/OperateMsg", KeyStore.DemoUrl);

                        article.Description = sb.ToString();
                    }
                    else
                    {
                        article.Title = string.Format("你好，{0}！", requestMessage.FromUserName);
                        article.Description = sb.ToString();
                        article.Url = string.Format("{0}Home/OperateMsg", KeyStore.DemoUrl);
                    }                                                                             

                    return responseMessage;

            }

            
        }

        /// <summary>
        /// 回复1：获取Token
        /// </summary>
        /// <param name="requestMessage"></param>
        private ResponseMessageText GetResponseBy_1(IRequestMessageBase requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("获取用户Token！");
            sb.AppendLine(KeyStore.AccessToken);
            sb.AppendLine(string.Format("Token过期时间{0}",
                AccessTokenContainer.TryGetItem(KeyStore.AppId).AccessTokenExpireTime));
            responseMessage.Content = sb.ToString();

            return responseMessage;
        }

        /// <summary>
        /// 回复2：获取Url
        /// </summary>
        /// <param name="requestMessage"></param>
        private ResponseMessageText GetResponseBy_2(IRequestMessageBase requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            var sb = new StringBuilder();
            sb.AppendLine("下面是一个作为演示的网址。");
            sb.AppendLine(string.Format("<a href=\"{0}weixin\">点击这里，链接url</a>", KeyStore.DemoUrl));

            responseMessage.Content = sb.ToString();

            return responseMessage;
        }
    }
}
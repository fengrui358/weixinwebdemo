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
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型

            switch (requestMessage.Content)
            {
                case "1":
                    GetResponseBy_1(ref responseMessage);
                    return responseMessage;
                case "2":
                    GetResponseBy_2(ref responseMessage);
                    return responseMessage;
                default:
                    var userInfo = CommonApi.GetUserInfo(KeyStore.AccessToken, requestMessage.FromUserName);
                    StringBuilder sb =
                        new StringBuilder(string.Format("你的OpenId：{0}，你的消息Id：{1}， 你的消息类型{2}， 你的创建时间{3}， 你的消息内容{4}",
                            requestMessage.FromUserName, requestMessage.MsgId, requestMessage.MsgType,
                            requestMessage.CreateTime, responseMessage.Content));
                    if (userInfo.subscribe != 0)
                    {
                        sb.AppendLine("你的详细信息：");
                        sb.AppendLine("你的昵称：" + userInfo.nickname);
                        sb.AppendLine("你的头像地址：" + userInfo.headimgurl);
                        sb.AppendLine("你的语言信息：" + userInfo.language);
                    }
                    
                    sb.AppendLine("回复1：获取用户Token！");
                    sb.AppendLine("回复2：获取一个链接！");
                    responseMessage.Content = sb.ToString();
                    break;

            }

            return responseMessage;
        }

        /// <summary>
        /// 回复1：获取Token
        /// </summary>
        /// <param name="responseMessage"></param>
        private void GetResponseBy_1(ref ResponseMessageText responseMessage)
        {
            StringBuilder sb = new StringBuilder("获取用户Token！");
            sb.AppendLine(KeyStore.AccessToken);
            sb.AppendLine(string.Format("Token过期时间{0}",
                AccessTokenContainer.TryGetItem(KeyStore.AppId).AccessTokenExpireTime));
            responseMessage.Content = sb.ToString();
        }

        /// <summary>
        /// 回复2：获取Url
        /// </summary>
        /// <param name="responseMessage"></param>
        private void GetResponseBy_2(ref ResponseMessageText responseMessage)
        {
            responseMessage.Content = "<a href=\"http://fengrui358.vicp.cc/weixin\">点击这里，链接url</a>";
        }
    }
}
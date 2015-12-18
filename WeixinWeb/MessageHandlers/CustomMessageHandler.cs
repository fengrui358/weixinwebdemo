using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.AppStore;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;

namespace WeixinWeb.MessageHandlers
{
    public class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        public CustomMessageHandler(Stream inputStream, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) : base(inputStream, postModel, maxRecordCount, developerInfo)
        {
        }

        public CustomMessageHandler(XDocument requestDocument, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) : base(requestDocument, postModel, maxRecordCount, developerInfo)
        {
        }

        public CustomMessageHandler(RequestMessageBase requestMessageBase, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) : base(requestMessageBase, postModel, maxRecordCount, developerInfo)
        {
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型

            responseMessage.Content = "这条消息来自DefaultResponseMessage。";

            return responseMessage;
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型

            responseMessage.Content = string.Format("你好吗：{0}，你的消息Id：{1}， 你的消息类型{2}， 你的创建时间{3}",
                RequestMessage.FromUserName, RequestMessage.MsgId, RequestMessage.MsgType, RequestMessage.CreateTime);

            return responseMessage;
        }
    }
}
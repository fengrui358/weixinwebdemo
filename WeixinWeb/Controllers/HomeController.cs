using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml.Linq;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;

namespace WeixinWeb.Controllers
{
    public class HomeController : Controller
    {
        private static readonly string Token = WebConfigurationManager.AppSettings["WeixinToken"];//与微信公众账号后台的Token设置保持一致，区分大小写。

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(string signature, string timestamp, string nonce, string echostr)
        {
            if (CheckSignature.Check(signature, timestamp, nonce, Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token) + "。如果您在浏览器中看到这条信息，表明此Url可以填入微信后台。");
            }
        }

        //[HttpPost]
        //[ActionName("Index")]
        //public ActionResult Post(PostModel postModel)
        //{
        //    if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
        //    {
        //        return Content("参数错误！");
        //    }

        //    return Content("success");
        //    //postModel.Token = Token;
        //    //postModel.EncodingAESKey = EncodingAESKey;//根据自己后台的设置保持一致
        //    //postModel.AppId = AppId;//根据自己后台的设置保持一致

        //    //var messageHandler = new CustomMessageHandler(Request.InputStream, postModel);//接收消息

        //    //messageHandler.Execute();//执行微信处理过程

        //    //return new WeixinResult(messageHandler);//返回结果
        //}

        /// <summary>
        /// 未使用框架的原生解析消息
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="echostr"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult OriginalPost(string signature, string timestamp, string nonce, string echostr)
        {
            //消息安全验证代码开始
            //...
            //消息安全验证代码结束

            string requestXmlString = null;//请求XML字符串
            using (var sr = new StreamReader(HttpContext.Request.InputStream))
            {
                requestXmlString = sr.ReadToEnd();
            }

            //XML消息格式正确性验证代码开始
            //...
            //XML消息格式正确性验证代码结束

            /* XML消息范例
            <xml>
                <ToUserName><![CDATA[gh_a96a4a619366]]></ToUserName>
                <FromUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></FromUserName>
                <CreateTime>{{0}}</CreateTime>
                <MsgType><![CDATA[text]]></MsgType>
                <Content><![CDATA[{0}]]></Content>
                <MsgId>5832509444155992350</MsgId>
            </xml>
            */

            XDocument xmlDocument = XDocument.Parse(requestXmlString);//XML消息对象
            var xmlRoot = xmlDocument.Root;
            var msgType = xmlRoot.Element("MsgType").Value;//消息类型
            var toUserName = xmlRoot.Element("ToUserName").Value;
            var fromUserName = xmlRoot.Element("FromUserName").Value;
            var createTime = xmlRoot.Element("CreateTime").Value;
            var msgId = xmlRoot.Element("MsgId").Value;

            //根据MsgId去重开始
            //...
            //根据MsgId去重结束

            string responseXml = null;//响应消息XML
            var responseTime = (DateTime.Now.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000 - 8 * 60 * 60;

            switch (msgType)
            {
                case "text":
                    //处理文本消息
                    var content = xmlRoot.Element("Content").Value;//文本内容
                    if (content == "你好")
                    {
                        var title = "Title";
                        var description = "Description";
                        var picUrl = "PicUrl";
                        var url = "Url";
                        var articleCount = 1;
                        responseXml = @"<xml>
                        <ToUserName><![CDATA[" + fromUserName + @"]]></ToUserName>
                        <FromUserName><![CDATA[" + toUserName + @"]]></FromUserName>
                        <CreateTime>" + responseTime + @"</CreateTime>
                        <MsgType><![CDATA[text]]></MsgType>
                        <Content>" + "你在干嘛" + @"</Content>
                    
                        </xml> ";
                    }
                    else if (content == "Senparc")
                    {
                        //相似处理逻辑
                    }
                    else
                    {
                        //...
                    }
                    break;
                case "image":
                    //处理图片消息
                    //...
                    break;
                case "event":
                    //这里会有更加复杂的事件类型处理
                    //...
                    break;
                //更多其他请求消息类型的判断...
                default:
                    //其他未知类型
                    break;
            }

            return Content(responseXml, "text/xml");//返回结果
        }
    }
}
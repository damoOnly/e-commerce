using EcShop.Membership.Context;
using Ecdev.Weixin.MP.Util;
using System;
using System.IO;
using System.Web;
using EcShop.Core.ErrorLog;
using System.Text;
namespace EcShop.UI.Web.API
{
    public class wx : System.Web.IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public void ProcessRequest(System.Web.HttpContext context)
        {
            ErrorLog.Write("进入了:EcShop.UI.Web.API->ProcessRequest");
            System.Web.HttpRequest request = context.Request;
            string weixinToken = SettingsManager.GetMasterSettings(false).WeixinToken;
            string signature = request["signature"];
            string nonce = request["nonce"];
            string timestamp = request["timestamp"];
            string s = request["echostr"];
            ErrorLog.Write("ProcessRequest获取参数：weixinToken=" + weixinToken + ";signature=" + signature + ";nonce=" + nonce + ";timestamp=" + timestamp + ";s=" + s + ";HttpMethod=" + request.HttpMethod);
            if (request.HttpMethod == "GET")
            {
                if (CheckSignature.Check(signature, timestamp, nonce, weixinToken))
                {
                    context.Response.Write(s);
                }
                else
                {
                    context.Response.Write("");
                }
                context.Response.End();
                return;
            }
            try
            {
                CustomMsgHandler customMsgHandler = new CustomMsgHandler(request.InputStream);
                customMsgHandler.Execute();

                ErrorLog.Write("customMsgHandler.ResponseDocumen:" + customMsgHandler.ResponseDocument);
                context.Response.Write(customMsgHandler.ResponseDocument);
            }
            catch (System.Exception ex)
            {
                System.IO.StreamWriter streamWriter = System.IO.File.AppendText(context.Server.MapPath("error.txt"));
                streamWriter.WriteLine(ex.Message);
                streamWriter.WriteLine(ex.StackTrace);
                streamWriter.WriteLine(System.DateTime.Now);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
    }
}

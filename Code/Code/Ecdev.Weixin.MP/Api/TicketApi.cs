using Ecdev.Weixin.MP.Domain;
using Ecdev.Weixin.MP.Util;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Ecdev.Weixin.MP.Api
{
   public class TicketApi
    {
        private static DateTime LATEST_GET_JSAPITICKET_TIME = DateTime.MinValue;
        private static string LATEST_JSAPITICKET = "";
        private static string AppId
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("AppId");
            }
        }
        private static string AppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("AppSecret");
            }
        }
        public static string GetJsapiTicket_Message(string appid, string appsecret)
        {

           
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", TokenApi.GetToken_Message(appid, appsecret));
            string text = new WebUtils().DoGet(url, null);
            ErrorLog.Write("获取了access_token:" + TokenApi.GetToken_Message(AppId, AppSecret));
            if (text.Contains("ticket"))
            {
                LATEST_JSAPITICKET = new JavaScriptSerializer().Deserialize<Ticket>(text).ticket;
                LATEST_GET_JSAPITICKET_TIME = DateTime.Now;
            }
            return LATEST_JSAPITICKET;
            
        }
        public static string GetTicketSignature(string noncestr, string timestamp, string url,string appid,string appsecret)
        {
            if (string.IsNullOrEmpty(noncestr) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(url))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            string ticket = HiCache.Get("ticket") as string;

            if (ticket == null)
            {
                ticket = GetJsapiTicket_Message(appid, appsecret);
                HiCache.Insert("ticket", ticket, 1000);  
            }

            sb.AppendFormat("jsapi_ticket={0}", ticket);
            sb.AppendFormat("&noncestr={0}",noncestr);
            sb.AppendFormat("&timestamp={0}", timestamp);
            sb.AppendFormat("&url={0}", url);
            
            //ErrorLog.Write("获取ticket:" + ticket);
            string signature = FormsAuthentication.HashPasswordForStoringInConfigFile(sb.ToString(), FormsAuthPasswordFormat.SHA1.ToString());
            return signature;
        }
    }
}


using Ecdev.Weixin.MP.Domain;
using Ecdev.Weixin.MP.Util;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Ecdev.Weixin.MP.Api
{
	public class TokenApi
	{
        private static DateTime LATEST_GET_TOKEN_TIME = DateTime.MinValue;
        private static string LATEST_TOKEN = "";

		public string AppId
		{
			get
			{
				return ConfigurationManager.AppSettings.Get("AppId");
			}
		}

		public string AppSecret
		{
			get
			{
				return ConfigurationManager.AppSettings.Get("AppSecret");
			}
		}

		public static string GetToken_Message(string appid, string secret)
		{
            if (LATEST_TOKEN != "")
            {
                TimeSpan span = DateTime.Now - LATEST_GET_TOKEN_TIME; 

                if (span.TotalSeconds < 7000)    //7200
                {
                    return LATEST_TOKEN;
                }
            }

			string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
			string text = new WebUtils().DoGet(url, null);
            
			if (text.Contains("access_token"))
			{
                LATEST_TOKEN = new JavaScriptSerializer().Deserialize<Token>(text).access_token;
                LATEST_GET_TOKEN_TIME = DateTime.Now;
			}

            return LATEST_TOKEN;
		}

		public static string GetToken(string appid, string secret)
		{
			string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
			return new WebUtils().DoGet(url, null);
		}

        /// <summary>
        /// 通过code换取网页授权access_token
        /// </summary>
        /// <param name="appid"> 公众号的唯一标识 </param>
        /// <param name="secret">公众号的appsecret </param>
        /// <param name="code">填写第一步获取的code参数 </param>
        /// <param name="grant_type">填写为authorization_code </param>
        /// <returns></returns>
        public static Token GetToken(string appid, string secret, string code, string grant_type = "authorization_code")
        {
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}", appid, secret, code, grant_type);
            string text = new WebUtils().DoGet(url, null);
            Token token = null;
            if (text.Contains("access_token"))
            {
                token = new JavaScriptSerializer().Deserialize<Token>(text);
            }
            return token;
        }

        /// <summary>
        ///根据openid，access token获得用户信息
        /// </summary>
        /// <param name="accesstoken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static Userinfo GetUserInfo(string accesstoken, string openId,string weixinAppId, string WeixinAppSecret)
        {
            //string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", accesstoken, openId);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", GetToken_Message(weixinAppId,WeixinAppSecret), openId);
            string text = new WebUtils().DoGet(url, null);
            // WriteErrorToText(text);
            Userinfo userinfo = null;
            if (text.Contains("nickname"))
            {
                userinfo = new JavaScriptSerializer().Deserialize<Userinfo>(text);
            }
            return userinfo;
        }
	}
}

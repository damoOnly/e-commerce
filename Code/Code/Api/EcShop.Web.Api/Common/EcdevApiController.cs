using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Runtime.Caching;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core;

using EcShop.Web.Api.Model.Result;
using EcShop.Web.Api.ApiException;
using EcShop.Web.Api.Utility;

using EcShop.Entities;
using EcShop.Entities.OAuth;
using EcShop.SqlDal.OAuth;

namespace EcShop.Web.Api
{
    public class EcdevApiController : ApiController
    {
        private const string HttpContext_Key = "MS_HttpContext";
        private const string RemoteEndpointMessage_Key = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private const string OwinContext_Key = "MS_OwinContext";

        private readonly string USE_CACHE = ConfigurationManager.AppSettings["USE_CACHE"].ToString();
        private readonly string NEED_VERIFY_ACCESSTOKEN = ConfigurationManager.AppSettings["NEED_VERIFY_ACCESSTOKEN"].ToString();
        private readonly string PC_ENCRYPTION = ConfigurationManager.AppSettings["PC_ENCRYPTION"].ToString();
        private readonly string IOS_ENCRYPTION = ConfigurationManager.AppSettings["IOS_ENCRYPTION"].ToString();
        private readonly string ANDROID_ENCRYPTION = ConfigurationManager.AppSettings["ANDROID_ENCRYPTION"].ToString();
        private readonly string PAY_SIGNATURE = ConfigurationManager.AppSettings["PAY_SIGNATURE"].ToString();

        public readonly int SITE_CACHE_KEEP_TIME = int.Parse(ConfigurationManager.AppSettings["SITE_CACHE_KEEP_TIME"].ToString());
        public readonly int USER_CACHE_KEEP_TIME = int.Parse(ConfigurationManager.AppSettings["USER_CACHE_KEEP_TIME"].ToString());
        public readonly int TOKEN_CACHE_KEEP_TIME = int.Parse(ConfigurationManager.AppSettings["TOKEN_CACHE_KEEP_TIME"].ToString());
        public readonly string STORAGE_HOST = ConfigurationManager.AppSettings["STORAGE_HOST"].ToString();
        public readonly string HOST = ConfigurationManager.AppSettings["HOST"].ToString();
        public readonly string HOST_BASE_PATH = ConfigurationManager.AppSettings["HOST_BASE_PATH"].ToString();
        public readonly string PRODUCT_SHARE_URL_BASE = ConfigurationManager.AppSettings["PRODUCT_SHARE_URL_BASE"].ToString();

        //短信配置内容
        public string SMS_FORART = ConfigurationManager.AppSettings["SMS_FORART"].ToString();

        public EcdevApiController() { }

        /// <summary>
        /// 获取请求者IP地址
        /// </summary>
        /// <returns></returns>
        private string GetClientIp(HttpRequestMessage request)
        {
            // Web-hosting
            if (request.Properties.ContainsKey(HttpContext_Key))
            {
                HttpContextWrapper ctx = (HttpContextWrapper)request.Properties[HttpContext_Key];

                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            // Self-hosting
            if (request.Properties.ContainsKey(RemoteEndpointMessage_Key))
            {
                RemoteEndpointMessageProperty remoteEndpoint = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessage_Key];

                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            /*
            // Self-hosting using Owin
            if (request.Properties.ContainsKey(OwinContext))
            {
                OwinContext owinContext = (OwinContext)request.Properties[OwinContext];
                if (owinContext != null)
                {
                    return owinContext.Request.RemoteIpAddress;
                }
            }
            */

            return null;
        }

        /// <summary>
        /// 抛参数传入异常
        /// </summary>
        /// <param name="param"></param>
        public void ThrowParamException(params  string[] param)
        {
            foreach (string s in param)
            {
                if (string.IsNullOrEmpty(s))
                {
                    throw  new CommonException(40007);
                }
            }
        }

        public IHttpActionResult JsonActionResult(object result)
        {
            return Ok(result);
        }

        public IHttpActionResult JsonFaultResult(FaultInfo result, object param)
        {
            FaultResult faultResult = new FaultResult(result.Code, result.Message, param.ToString());
            return Ok(faultResult);
        }

        public void CacheSet(string appId, string token, DateTime expirationUtc, int id, string sessionKey, string sessionSecret, string appSecret)
        {
            TimeSpan span = new TimeSpan(24, 0, 0);

            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.SlidingExpiration = span;

            Dictionary<string, string> accessToken = new Dictionary<string, string>();
            accessToken.Add("appId", appId);
            //accessToken.Add("accessToken", token);
            accessToken.Add("expirationUtc", expirationUtc.ToFileTimeUtc().ToString());
            accessToken.Add("id", id.ToString());
            accessToken.Add("sessionKey", sessionKey);
            accessToken.Add("sessionSecret", sessionSecret);
            accessToken.Add("appSecret", appSecret);

            string cacheKey = Common.DESCrypt.MD5(token);

            cache.Set(cacheKey, accessToken, policy);
        }

        public int CheckAccessToken(string token)
        {
            // From Cache
            ObjectCache cache = MemoryCache.Default;

            if (cache.GetCount() == 0)
            {
                LoadAccessToken();
            }

            string cacheKey = Common.DESCrypt.MD5(token);

            if (cache.Contains(cacheKey))
            {
                Dictionary<string, string> accessToken = cache.GetCacheItem(cacheKey).Value as Dictionary<string, string>;

                string appId = "", cacheTimestamp = "", cacheId = "0";

                accessToken.TryGetValue("appId", out appId);

                accessToken.TryGetValue("expirationUtc", out cacheTimestamp);
                accessToken.TryGetValue("id", out cacheId);

                if (DateTime.FromFileTimeUtc(long.Parse(cacheTimestamp)) >= DateTime.UtcNow)
                {
                    return int.Parse(cacheId);
                }
            }

            return 0;
        }

        public int CheckAccessToken(string token, out string appId)
        {
            appId = "";

            // From Cache
            ObjectCache cache = MemoryCache.Default;

            if (cache.GetCount() == 0)
            {
                LoadAccessToken();
            }

            string cacheKey = Common.DESCrypt.MD5(token);

            if (cache.Contains(cacheKey))
            {
                Dictionary<string, string> accessToken = cache.GetCacheItem(cacheKey).Value as Dictionary<string, string>;

                string cacheTimestamp = "", cacheId = "0";

                accessToken.TryGetValue("appId", out appId);

                accessToken.TryGetValue("expirationUtc", out cacheTimestamp);
                accessToken.TryGetValue("id", out cacheId);

                if (DateTime.FromFileTimeUtc(long.Parse(cacheTimestamp)) >= DateTime.UtcNow)
                {
                    return int.Parse(cacheId);
                }
            }

            return 0;
        }

        public int CheckAccessToken(string token, out string sessionKey, out string sessionSecret)
        {
            sessionKey = "";
            sessionSecret = "";

            // From Cache
            ObjectCache cache = MemoryCache.Default;

            if (cache.GetCount() == 0)
            {
                LoadAccessToken();
            }

            string cacheKey = Common.DESCrypt.MD5(token);

            if (cache.Contains(cacheKey))
            {
                Dictionary<string, string> accessToken = cache.GetCacheItem(cacheKey).Value as Dictionary<string, string>;

                string appId = "", cacheTimestamp = "", cacheId = "0";

                accessToken.TryGetValue("appId", out appId);

                accessToken.TryGetValue("expirationUtc", out cacheTimestamp);
                accessToken.TryGetValue("id", out cacheId);

                if (DateTime.FromFileTimeUtc(long.Parse(cacheTimestamp)) >= DateTime.UtcNow)
                {
                    accessToken.TryGetValue("sessionKey", out sessionKey);
                    accessToken.TryGetValue("sessionSecret", out sessionSecret);

                    return int.Parse(cacheId);
                }
            }

            return 0;
        }

        public int CheckAccessToken(string token, out string sessionKey, out string sessionSecret, out string appSecret)
        {
            sessionKey = "";
            sessionSecret = "";
            appSecret = "";

            // From Cache
            ObjectCache cache = MemoryCache.Default;

            if (cache.GetCount() == 0)
            {
                LoadAccessToken();
            }

            string cacheKey = Common.DESCrypt.MD5(token);

            if (cache.Contains(cacheKey))
            {
                Dictionary<string, string> accessToken = cache.GetCacheItem(cacheKey).Value as Dictionary<string, string>;

                string appId = "", cacheTimestamp = "", cacheId = "0";

                accessToken.TryGetValue("appId", out appId);

                accessToken.TryGetValue("expirationUtc", out cacheTimestamp);
                accessToken.TryGetValue("id", out cacheId);

                if (DateTime.FromFileTimeUtc(long.Parse(cacheTimestamp)) >= DateTime.UtcNow)
                {
                    accessToken.TryGetValue("sessionKey", out sessionKey);
                    accessToken.TryGetValue("sessionSecret", out sessionSecret);
                    accessToken.TryGetValue("appSecret", out appSecret);

                    return int.Parse(cacheId);
                }
            }

            return 0;
        }

        private void LoadAccessToken()
        {
            try
            {
                OAuthInfoDao dao = new OAuthInfoDao();
                var oauths = dao.GetOnlineOAuthInfo();

                foreach(OAuthInfo current in oauths)
                {
                    CacheSet(current.AppId, current.Token, current.ExpirationUtc.Value, current.Id, current.SessionKey, current.SessionSecret, current.AppSecret);
                }
            }
            catch (Exception ex)
            {
                Logger.WriterLogger("OAuth.AccessToken", ex, LoggerType.Error);
            }
        }

        #region User

        public void CacheUserSet(string authenUserId, string authenTicket, long timestamp, int userId)
        {
            TimeSpan span = new TimeSpan(24, 0, 0);

            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.SlidingExpiration = span;

            Dictionary<string, string> ticket = new Dictionary<string, string>();
            ticket.Add("authenTicket", authenTicket);
            ticket.Add("timestamp", timestamp.ToString());
            ticket.Add("userId", userId.ToString());

            cache.Set(authenUserId, ticket, policy);
        }

        public void CacheUserRemove(string sessionId)
        {
            // From Cache
            ObjectCache cache = MemoryCache.Default;

            string cacheKey = sessionId.ToLowerGuid();

            if (cache.Contains(cacheKey))
            {
                cache.Remove(cacheKey);

            }
        }

        public void CacheMemberSet(string authenUserId, Member member)
        {
            TimeSpan span = new TimeSpan(24, 0, 0);

            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.SlidingExpiration = span;

            cache.Set("member-" + authenUserId, member, policy);
        }

        public void CacheMemberRemove(string sessionId)
        {
            // From Cache
            ObjectCache cache = MemoryCache.Default;

            string cacheKey = "member-" + sessionId.ToLowerGuid();

            if (cache.Contains(cacheKey))
            {
                cache.Remove(cacheKey);

            }
        }

        public int GetUserId(string sessionId)
        {

            // From Cache
            ObjectCache cache = MemoryCache.Default;

            string cacheKey = sessionId;

            if (cache.Contains(cacheKey))
            {
                Dictionary<string, string> user = cache.GetCacheItem(cacheKey).Value as Dictionary<string, string>;

                string cacheUserId = "0";

                if (user.TryGetValue("userId", out cacheUserId))
                {
                    int retUserId = 0;

                    int.TryParse(cacheUserId, out retUserId);

                    return retUserId;

                };

             }

            return 0;
        }

        #endregion

        public Member GetMember(string sessionId, bool fromCache = true)
        {
            Member member = null;

            if (fromCache)
            {
                ObjectCache cache = MemoryCache.Default;

                string cacheKey = "member-" + sessionId.ToLowerGuid();

                if (cache.Contains(cacheKey))
                {
                    member = cache.Get(cacheKey) as Member;
                }
            }

            if (member == null)
            {
                
                member = Users.GetUserIdByUserSessionId(sessionId.ToSeesionId(),fromCache) as Member;
                if (member == null || member.IsAnonymous)
                {
                    return null;
                }

                CacheMemberSet(sessionId, member);
            }

            return member;
        }

        public int VerifyAccessToken(string accessToken)
        {
            string v = NEED_VERIFY_ACCESSTOKEN.ToLower();

            if (v == "y" || v == "yes")
            {
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    return 40009;
                }

                int oauthId = CheckAccessToken(accessToken);

                if (oauthId <= 0)
                {
                    return 40010;
                }
            }

            return 0;
        }

        public int VerifyAccessToken(string accessToken, out string appId)
        {
            appId = "";

            string v = NEED_VERIFY_ACCESSTOKEN.ToLower();

            if (v == "y" || v == "yes")
            {
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    return 40009;
                }

                int oauthId = CheckAccessToken(accessToken, out appId);

                if (oauthId <= 0)
                {
                    return 40010;
                }
            }

            return 0;
        }

        public int VerifyAccessToken(string accessToken, out string sessionKey, out string sessionSecret)
        {
            sessionKey = "";
            sessionSecret = "";

            string v = NEED_VERIFY_ACCESSTOKEN.ToLower();

            if (v == "y" || v == "yes")
            {
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    return 40009;
                }

                int oauthId = CheckAccessToken(accessToken, out sessionKey, out sessionSecret);

                if (oauthId <= 0)
                {
                    return 40010;
                }
            }

            return 0;
        }

        public int VerifyAccessToken(string accessToken, out string sessionKey, out string sessionSecret, out string appSecret)
        {
            sessionKey = "";
            sessionSecret = "";
            appSecret = "";

            string v = NEED_VERIFY_ACCESSTOKEN.ToLower();

            if (v == "y" || v == "yes")
            {
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    return 40009;
                }

                int oauthId = CheckAccessToken(accessToken, out sessionKey, out sessionSecret, out appSecret);

                if (oauthId <= 0)
                {
                    return 40010;
                }
            }

            return 0;
        }

        public bool IsVerifyAccessToken
        {
            get
            {
                string v = NEED_VERIFY_ACCESSTOKEN.ToLower();

                return (v == "y" || v == "yes");
            }
        }

        public bool PCIsEncryption {
            get
            {
                string v = IOS_ENCRYPTION.ToLower();

                return (v == "y" || v == "yes");
            }
        }

        public bool IOSIsEncryption
        {
            get
            {
                string v = IOS_ENCRYPTION.ToLower();

                return (v == "y" || v == "yes");
            }
        }

        public bool AndroidIsEncryption
        {
            get
            {
                string v = ANDROID_ENCRYPTION.ToLower();

                return (v == "y" || v == "yes");
            }
        }
        public bool IsPaySignature
        {
            get
            {
                string v = PAY_SIGNATURE.ToLower();

                return (v == "y" || v == "yes");
            }
        }
        public bool IsUseCache
        {
            get
            {
                string v = USE_CACHE.ToLower();

                return (v == "y" || v == "yes");
            }
        }
        public void SaveVisitInfo(string sessionId, int channel, int platform, string ver)
        {

        }

        public void SaveVisitInfo(int siteId, string sessionId, int channel, int platform, string ver)
        {

        }

        public string Encrypt(string data, string sessionKey, string sessionSecret)
        {
            string result = "";

            string v = NEED_VERIFY_ACCESSTOKEN.ToLower();

            if (v == "y" || v == "yes")
            {
                
                string key = sessionKey.Substring(8, 8);
                string iv = sessionSecret.Substring(16, 8);

                result = SecurityUtil.DESEncrypt(data, key, iv);
            }
            else
            {
                result = data;
            }

            return result;
        }

        public string Decrypt(string data, string sessionKey, string sessionSecret)
        {
            string result = "";

            string v = NEED_VERIFY_ACCESSTOKEN.ToLower();

            if (v == "y" || v == "yes")
            {
                string key = sessionKey.Substring(8, 8);
                string iv = sessionSecret.Substring(16, 8);
                result = SecurityUtil.DESDecrypt(data, key, iv);
            }
            else
            {
                result = data;
            }

            return result;
        }

        public string Sign(SortedDictionary<string, string> data, string appSecret, string sessionSecret)
        {
            string result = "";

            StringBuilder sbData = new StringBuilder();

            foreach (KeyValuePair<string, string> current in data)
            {
                if (current.Key.ToLower() != "sign")
                {
                    sbData.AppendFormat("{0}{1}={2}", sbData.Length > 0 ? "&" : "", current.Key, current.Value);
                }
            }

            string source = appSecret + sbData.ToString() + sessionSecret;

            result = SecurityUtil.MD5Encrypt(source);

            return result;
        }

    }

}
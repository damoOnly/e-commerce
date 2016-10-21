using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.Security.Cryptography;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using EcShop.Web.Api.ApiException;

using EcShop.Web.Api.Model;
using EcShop.Web.Api.Model.Query;
using EcShop.Web.Api.Model.Result;

using EcShop.Web.Api;
using EcShop.Web.Api.Helper;
//using EcShop.Web.Api.Models;

using EcShop.Web.Api.Utility;

using EcShop.Entities;
using EcShop.Entities.OAuth;
using EcShop.SqlDal.OAuth;

namespace EcShop.Web.Api.Controllers
{
    public class OAuthController : EcdevApiController
    {
        OAuthInfoDao oAuthDao = null;

        public OAuthController(
            )
        {
            oAuthDao = new OAuthInfoDao();
        }

        [HttpGet]
        public IHttpActionResult GetApp(string deviceId, int deviceType, int channel, string ver)
        {
            Logger.WriterLogger("OAuth.GetApp, Params: " + string.Format("deviceId={0}&deviceType={1}&channel={2}&ver={3}", deviceId, deviceType, channel, ver), LoggerType.Info);

            string appId = "hd" + SecurityUtil.MD5Encrypt16(deviceId, Encoding.UTF8).ToLower();
            string appSecret = "";

            OAuthInfo oauthInfo = oAuthDao.GetOAuthInfo(appId);

            if (oauthInfo != null)
            {
                appSecret = oauthInfo.AppSecret;
            }
            else
            {
                appSecret = Guid.NewGuid().ToString().Replace("-", "").ToLower();

                oauthInfo = oAuthDao.CreateOAuthInfo(deviceId, deviceType, channel, ver, appId, appSecret);

            }

            if (oauthInfo != null)
            {
                AppResult appResult = new AppResult()
                {
                    AppId = appId,
                    AppSecret = appSecret
                };

                StandardResult<AppResult> result = new StandardResult<AppResult>()
                {
                    code = 0,
                    msg = "",
                    data = appResult
                };

                return base.JsonActionResult(result);
            }

            return base.JsonFaultResult(new CommonException(40001).GetMessage(), appId);
        }

        [HttpGet]
        public IHttpActionResult AccessToken(string appId, string randstr, string timestamp, string signature)
        {
            Logger.WriterLogger("OAuth.AccessToken, Params: " + string.Format("appId={0}&randstr={1}&timestamp={2}&signature={3}", appId, randstr, timestamp, signature), LoggerType.Info);

            string appSecret = "";

            if (string.IsNullOrWhiteSpace(appId))
            {
                return base.JsonFaultResult(new CommonException(40001).GetMessage(), appId);
            }

            try
            {
                // 获取密钥
                OAuthInfo oauth = null;

                try
                {
                    oauth = oAuthDao.GetOAuthInfo(appId);

                    appSecret = oauth.AppSecret;
                }
                catch (Exception ex)
                {
                    Logger.WriterLogger("OAuth.AccessToken", ex, LoggerType.Error);
                }


                if (oauth == null)
                {
                    return base.JsonFaultResult(new CommonException(40002).GetMessage(), appId);
                }

                string mySignature = SecurityUtil.MD5Encrypt(randstr + appSecret + timestamp).ToLower();

                if (signature.ToLower() != mySignature)
                {
                    return base.JsonFaultResult(new CommonException(40005).GetMessage(), appId);
                }

                DateTime issueDateUtc = DateTime.UtcNow;
                DateTime expirationUtc = issueDateUtc.AddHours(8);

                string userKey = string.Format("{0},{1},{2}", oauth.Id, oauth.AppId, expirationUtc.ToFileTimeUtc());

                string accessToken = Common.DESCrypt.Encrypt(userKey, oauth.AppSecret);

                string sessionKey = SecurityUtil.MD5Encrypt(Guid.NewGuid().ToString()).ToLower();
                string sessionSecret = SecurityUtil.MD5Encrypt(Guid.NewGuid().ToString()).ToLower();

                CacheSet(appId, accessToken, expirationUtc, oauth.Id, sessionKey, sessionSecret, appSecret);

                oauth.IssueDateUtc = issueDateUtc;
                oauth.ExpirationUtc = expirationUtc;
                oauth.Token = accessToken;
                oauth.SessionKey = sessionKey;
                oauth.SessionSecret = sessionSecret;

                try
                {
                    oAuthDao.Update(oauth);
                }
                catch(Exception ex)
                {
                    Logger.WriterLogger("OAuth.AccessToken", ex, LoggerType.Error);
                }

                AccessTokenResult tokenResult = new AccessTokenResult();
                tokenResult.AccessToken = accessToken;
                tokenResult.ExpiresIn = 8 * 60 * 60;
                tokenResult.SessionKey = sessionKey;
                tokenResult.SessionSecret = sessionSecret;

                StandardResult<AccessTokenResult> result = new StandardResult<AccessTokenResult>()
                {
                    code = 0,
                    msg = "",
                    data = tokenResult
                };

                return base.JsonActionResult(result);
            }
            catch (CommonException ex)
            {
                FaultInfo info = ex.GetMessage();
                return base.JsonActionResult(info);
            }
        }

    }
}

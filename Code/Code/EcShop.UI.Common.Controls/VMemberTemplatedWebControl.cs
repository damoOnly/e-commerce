
using Ecdev.Weixin.MP.Api;
using Ecdev.Weixin.MP.Domain;
using EcShop.ControlPanel.Members;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Entities;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Member;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
namespace EcShop.UI.Common.Controls
{
    [ParseChildren(true), PersistChildren(false)]
    public abstract class VMemberTemplatedWebControl : VshopTemplatedWebControl
    {
        protected VMemberTemplatedWebControl()
        {
            string userName = Users.GetLoggedOnUsername();
            if (string.IsNullOrEmpty(userName) || userName == "Anonymous")
            {
                Token token = GetLoginOnToken();
                if (token != null)
                {
                    string openId = "";
                    IUser user = null;
                    if (!string.IsNullOrEmpty(token.openid))
                    {
                        openId = token.openid;
                        ErrorLog.Write(string.Format("注册新用户，openId={0}", openId));
                        user = RegisterUser(openId, token.access_token);
                        ErrorLog.Write(string.Format("新用户ID，userId={0}", user.UserId));
                    }
                    else
                    {
                        string name = "Vshop-Member";
                        HttpCookie httpCookie = new HttpCookie("Vshop-Member");
                        httpCookie.Value = Globals.UrlEncode(user.Username);
                        httpCookie.Expires = System.DateTime.Now.AddDays(7);
                        httpCookie.Domain = HttpContext.Current.Request.Url.Host;
                        if (HttpContext.Current.Response.Cookies[name] != null)
                        {
                            HttpContext.Current.Response.Cookies.Remove(name);
                        }
                        HttpContext.Current.Response.Cookies.Add(httpCookie);
                    }
                }
            }
            else
            {
                if (!(HiContext.Current.User is Member))
                {
                    HttpCookie httpCookieMember = new HttpCookie("Vshop-Member");
                    httpCookieMember.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(httpCookieMember);
                    this.Page.Response.Redirect(HttpContext.Current.Request.Url.Authority + "/Vshop/");
                    this.WriteError("会员未登陆", "");
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                    if (masterSettings.IsValidationService)
                    {
                        string text = this.Page.Request.QueryString["code"];
                        if (!string.IsNullOrEmpty(text))
                        {
                            string responseResult = this.GetResponseResult(string.Concat(new string[]
						{
							"https://api.weixin.qq.com/sns/oauth2/access_token?appid=",
							masterSettings.WeixinAppId,
							"&secret=",
							masterSettings.WeixinAppSecret,
							"&code=",
							text,
							"&grant_type=authorization_code"
						}));
                            this.WriteError("用户已同意授权" + responseResult.ToString(), string.Concat(new string[]
						{
							"https://api.weixin.qq.com/sns/oauth2/access_token?appid=",
							masterSettings.WeixinAppId,
							"&secret=",
							masterSettings.WeixinAppSecret,
							"&code=",
							text,
							"&grant_type=authorization_code"
						}));
                            if (!responseResult.Contains("access_token"))
                            {
                                this.WriteError(string.Concat(new string[]
							{
								"获取access_token失败appid=",
								masterSettings.WeixinAppId,
								"&secret=",
								masterSettings.WeixinAppSecret,
								"&code=",
								text,
								"&grant_type=authorization_code"
							}), responseResult);
                                this.Page.Response.Redirect("Login.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
                                return;
                            }
                            JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
                            this.WriteError("已获取到access_token" + jObject.ToString(), string.Concat(new string[]
						{
							"https://api.weixin.qq.com/sns/oauth2/access_token?appid=",
							masterSettings.WeixinAppId,
							"&secret=",
							masterSettings.WeixinAppSecret,
							"&code=",
							text,
							"&grant_type=authorization_code"
						}));
                            if (this.HasLogin(jObject["openid"].ToString()))
                            {
                                this.WriteError("会员OpenId已绑定过会员帐号已自动登陆！", jObject["openid"].ToString());
                                return;
                            }
                            string responseResult2 = this.GetResponseResult(string.Concat(new string[]
						{
							"https://api.weixin.qq.com/sns/userinfo?access_token=",
							jObject["access_token"].ToString(),
							"&openid=",
							jObject["openid"].ToString(),
							"&lang=zh_CN"
						}));
                            this.WriteError("已获取到OpenId" + responseResult2, string.Concat(new string[]
						{
							"https://api.weixin.qq.com/sns/userinfo?access_token=",
							jObject["access_token"].ToString(),
							"&openid=",
							jObject["openid"].ToString(),
							"&lang=zh_CN"
						}));
                            if (responseResult2.Contains("nickname"))
                            {
                                JObject jObject2 = JsonConvert.DeserializeObject(responseResult2) as JObject;
                                this.Page.Response.Redirect(string.Concat(new string[]
							{
								"Login.aspx?openId=",
								jObject["openid"].ToString(),
								"&nickname=",
								jObject2["nickname"].ToString(),
								"&sessionId=",
								jObject["openid"].ToString(),
								"&returnUrl=",
								Globals.UrlEncode(HttpContext.Current.Request.Url.ToString())
							}));
                                return;
                            }
                            this.WriteError("获取nickname失败", responseResult);
                            this.Page.Response.Redirect(string.Concat(new string[]
						{
							"Login.aspx?openId=",
							jObject["openid"].ToString(),
							"&sessionId=",
							jObject["openid"].ToString(),
							"&returnUrl=",
							Globals.UrlEncode(HttpContext.Current.Request.Url.ToString())
						}));
                            return;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["state"]))
                            {
                                this.WriteError("用户取消了授权", "");
                                this.Page.Response.Redirect("Login.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
                                return;
                            }
                            string text2 = string.Concat(new string[]
						{
							"https://open.weixin.qq.com/connect/oauth2/authorize?appid=",
							masterSettings.WeixinAppId,
							"&redirect_uri=",
							Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()),
							"&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect"
						});
                            this.WriteError("还未跳转到授权页面" + text2, "");
                            this.Page.Response.Redirect(text2);
                            return;
                        }
                    }
                    else
                    {
                        this.WriteError("跳转到通用登陆接口", "");
                        if (!string.IsNullOrEmpty(masterSettings.WeixinLoginUrl))
                        {
                            this.Page.Response.Redirect(masterSettings.WeixinLoginUrl);
                            return;
                        }
                        this.Page.Response.Redirect("Login.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
                    }
                }
            }
        }
        public void WriteError(string msg, string OpenId)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.TableName = "wxlogin";
                dataTable.Columns.Add("OperTime");
                dataTable.Columns.Add("ErrorMsg");
                dataTable.Columns.Add("OpenId");
                dataTable.Columns.Add("PageUrl");
                DataRow dataRow = dataTable.NewRow();
                dataRow["OperTime"] = DateTime.Now;
                dataRow["ErrorMsg"] = msg;
                dataRow["OpenId"] = OpenId;
                dataRow["PageUrl"] = HttpContext.Current.Request.Url;
                dataTable.Rows.Add(dataRow);
                dataTable.WriteXml(HttpContext.Current.Request.MapPath("/wxlogin.xml"));
            }
            catch (Exception)
            {

            }
        }
        public bool HasLogin(string OpenId)
        {
            int userIdByOpenId = UserHelper.GetUserIdByOpenId(OpenId);
            if (userIdByOpenId > 0)
            {
                Member member = MemberHelper.GetMember(userIdByOpenId);
                if (member != null)
                {
                    string name = "Vshop-Member";
                    HttpCookie httpCookie = new HttpCookie("Vshop-Member");
                    httpCookie.Value = Globals.UrlEncode(member.Username);
                    httpCookie.Expires = System.DateTime.Now.AddDays(7);
                    httpCookie.Domain = HttpContext.Current.Request.Url.Host;
                    if (HttpContext.Current.Response.Cookies[name] != null)
                    {
                        HttpContext.Current.Response.Cookies.Remove(name);
                    }
                    HttpContext.Current.Response.Cookies.Add(httpCookie);
                    HiContext.Current.User = member;
                    member.OnLogin();
                    return true;
                }
            }
            return false;
        }
        private string GetResponseResult(string url)
        {
            WebRequest webRequest = WebRequest.Create(url);
            string result;
            using (HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                using (Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
            }
            return result;
        }
        private void SkipWinxinOpenId(string userName, string openId)
        {
            string generateId = Globals.GetGenerateId();
            Member member = new Member(UserRole.Member);
            member.GradeId = MemberProcessor.GetDefaultMemberGrade();
            member.Username = userName;
            member.OpenId = openId;
            member.Email = generateId + "@localhost.com";
            member.SessionId = generateId;
            member.Password = generateId;
            member.PasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePassword = generateId;
            member.IsApproved = true;
            member.RealName = string.Empty;
            member.Address = string.Empty;
            CreateUserStatus createUserStatus = MemberProcessor.CreateMember(member);
            if (createUserStatus != CreateUserStatus.Created)
            {
                member.Username = "微信会员_" + generateId;
                member.Email = generateId + "@localhost.com";
                member.Password = (member.TradePassword = generateId);
                MemberProcessor.CreateMember(member);
            }
            string name = "Vshop-Member";
            HttpCookie httpCookie = new HttpCookie("Vshop-Member");
            httpCookie.Value = Globals.UrlEncode(member.Username);
            httpCookie.Expires = System.DateTime.Now.AddDays(7);
            httpCookie.Domain = HttpContext.Current.Request.Url.Host;
            if (HttpContext.Current.Response.Cookies[name] != null)
            {
                HttpContext.Current.Response.Cookies.Remove(name);
            }
            HttpContext.Current.Response.Cookies.Add(httpCookie);

            HiContext.Current.User = member;
            member.OnLogin();
        }
        /// <summary>
        /// 关注微信关注公众号返回Token对象(获取OpenId和access_token)
        /// </summary>
        /// <returns></returns>
        public static Token GetLoginOnToken()
        {
            Token token = null;
            //先看cookie里面有没有opernId,如果没有OpenId,则调用服务器微信接口获取OpenId
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string code = HttpContext.Current.Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                string text2 = string.Format(
                            "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=123#wechat_redirect",
                            masterSettings.WeixinAppId,
                            Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
                HttpContext.Current.Response.Redirect(text2);
            }
            else
            {
                token = TokenApi.GetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, code);
            }
            return token;
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static IUser RegisterUser(string openId, string access_token)
        {
            ErrorLog.Write("laile:" + access_token + "");
            if (string.IsNullOrEmpty(openId))
            {
                return null;
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            Userinfo userinfo = TokenApi.GetUserInfo(access_token, openId, masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
            string realName = "";
            string headImg = "";
            if (userinfo != null)
            {
                if (!string.IsNullOrEmpty(userinfo.nickname))
                {
                    realName = userinfo.nickname;
                }
                if (!string.IsNullOrEmpty(userinfo.headimgurl))
                {
                    headImg = userinfo.headimgurl;
                }
            }
            long ip = 0;
            int ProvinceId = 0;
            try
            {
                ip = Globals.IpToInt(Globals.IPAddress);
            }
            catch
            {

            }
            ErrorLog.Write("laileip:" + ip + "");
            if (ip != 0)
            {
                string provinceName = TradeHelper.GetProvinceName(ip);
                if (!string.IsNullOrEmpty(provinceName))
                {
                    ProvinceId = RegionHelper.GetProvinceId(provinceName);
                    ErrorLog.Write("新用户ip" + Globals.IPAddress);
                    ErrorLog.Write(string.Format("新用户省份名称{0},省份Id{1}", provinceName, ProvinceId));
                }
            }
            string PasswordSalt = GenerateSalt();
            IUser user = UserHelper.CreateUsersMemberUsersInRoles(openId, PasswordSalt, realName, headImg, ProvinceId);
            ErrorLog.Write("laileuser:" + user.UserId + "");
            if (user != null && user.UserId > 0)
            {
                System.Collections.Hashtable hashtable = Users.UserCache();
                if (hashtable.ContainsKey(Users.UserKey(openId)))
                {
                    hashtable[Users.UserKey(openId)] = user;
                }
                else
                {
                    hashtable.Add(Users.UserKey(openId), user);
                }
                string name = "Vshop-Member";
                HttpCookie httpCookie = new HttpCookie("Vshop-Member");
                httpCookie.Value = Globals.UrlEncode(user.Username);
                httpCookie.Expires = System.DateTime.Now.AddDays(7);
                httpCookie.Domain = HttpContext.Current.Request.Url.Host;
                if (HttpContext.Current.Response.Cookies[name] != null)
                {
                    HttpContext.Current.Response.Cookies.Remove(name);
                }
                HttpContext.Current.Response.Cookies.Add(httpCookie);
                ErrorLog.Write("lailecookie:" + httpCookie + "");
                return user;
            }
            return null;
        }
        public static string GenerateSalt()
        {
            byte[] array = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(array);
            return Convert.ToBase64String(array);
        }
    }
}

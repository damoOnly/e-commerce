using Ecdev.Weixin.MP.Api;
using Ecdev.Weixin.MP.Domain;
using EcShop.ControlPanel.Members;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcShop.UI.Web.Admin
{
    public class usercode : Page
    {
        private string userId;
        private System.Web.UI.WebControls.Literal litRecemmendCode;
        private System.Web.UI.WebControls.Literal litJSApi;
        private System.Web.UI.HtmlControls.HtmlInputHidden username;
        private System.Web.UI.HtmlControls.HtmlInputHidden vuserid;
        private System.Web.UI.WebControls.Literal litTotals;
        private System.Web.UI.WebControls.Literal litWeekLevel;
        private System.Web.UI.WebControls.Literal litRecommendList;

        IUser user = null;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.litRecemmendCode = (System.Web.UI.WebControls.Literal)this.FindControl("litRecemmendCode");
            this.litJSApi = (System.Web.UI.WebControls.Literal)this.FindControl("litJSApi");
            this.username = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("username");
            this.vuserid = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("vuserid");
            this.litTotals = (System.Web.UI.WebControls.Literal)this.FindControl("litTotals");
            this.litWeekLevel = (System.Web.UI.WebControls.Literal)this.FindControl("litWeekLevel");
            this.litRecommendList = (System.Web.UI.WebControls.Literal)this.FindControl("litRecommendList");
            if (!this.IsPostBack)
            {
                if (this.Page.Request["userid"] != null)
                {
                    this.userId = SuserId(this.Page.Request["userid"].ToString());
                    Member member = Users.GetUserIdByUserSessionId(this.userId, false) as Member;
                    if (member == null)
                    {
                        Response.Write("会员信息不存在：" + this.Page.Request["userid"] + ";" + this.userId);
                        return;
                    }
                    this.vuserid.Value = member.UserId.ToString();
                    this.username.Value = member.Username;
                    GetRecemmendCode(member.UserId);
                }
                else
                {
                    Member member = HiContext.Current.User as Member;
                    if (member != null)
                    {
                        this.vuserid.Value = member.UserId.ToString();
                        this.username.Value = member.Username;
                        GetRecemmendCode(member.UserId);
                    }
                    else
                    {
                        if (user == null)
                        {
                            RegisterUser();
                        }
                        else
                        {
                            ErrorLog.Write("userid:" + user.UserId);
                            SetRecemendCode(user);
                        }
                    }
                }

                litJSApi.Text = GetJSApiScript();
            }
        }

        public void SetRecemendCode(IUser user)
        {
            if (user != null)
            {
                this.vuserid.Value = user.UserId.ToString();
                this.username.Value = user.Username;
                GetRecemmendCode(user.UserId);
                ErrorLog.Write("enduserid:" + user.UserId);
                ErrorLog.Write("endusername:" + user.Username);
            }
        }

        private void GetRecemmendCode(int userId)
        {
            litRecemmendCode.Text = MemberHelper.GetRecemmendCode(userId);
            ErrorLog.Write("recemmendcode:" + litRecemmendCode.Text);

            //
            MemberRecommendCodeInfo recomendCodeInfo = MemberHelper.GetRecommendCodeInfo(userId);
            if (recomendCodeInfo != null)
            {
                if (litTotals != null)
                {
                    litTotals.Text = recomendCodeInfo.TotalNum.ToString();
                }
                if (litWeekLevel != null)
                {
                    litWeekLevel.Text = recomendCodeInfo.WeekLevel.ToString();
                }

                if (recomendCodeInfo.NextRecommendCode != null && recomendCodeInfo.NextRecommendCode.Count > 0)
                {
                    var str = "";
                    for (int i = 0; i < recomendCodeInfo.NextRecommendCode.Count; i++)
                    {
                        var item = recomendCodeInfo.NextRecommendCode[i];
                        if (i % 2 == 0)
                        {
                            str += "<tr>";
                            str += "<td width='50%'>" + item.CellPhone + "</td>";
                        }
                        else if (i%2 == 1)
                        {
                            str +="<td width='50%'>" + item.CellPhone + "</td>";
                            str += "</tr>";
                        }
                    }
                    litRecommendList.Text = str;
                }
            }
        }

        public string SuserId(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return Guid.Empty.ToString();
            }

            string str = "";

            try
            {
                str = new Guid(source).ToString().ToLower();
            }

            catch (Exception ex)
            {
                str = Guid.Empty.ToString();
            }

            return str;
        }

        public string GetJSApiScript()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            string weixinAppId = masterSettings.WeixinAppId;

            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<script>");
            stringBuilder.AppendLine("wx.config({");
            stringBuilder.AppendLine("debug: false, ");
            stringBuilder.AppendLine(string.Format("appId: '{0}', ", weixinAppId));
            stringBuilder.AppendLine(string.Format("timestamp:{0},", timestamp));
            stringBuilder.AppendLine("nonceStr: 'haimylife',");
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (url.ToLower().Contains("vshop/default.aspx"))//为了兼容自定义首页
            {
                url = url.Replace("default.aspx", "").Replace("Default.aspx", "");
            }
            stringBuilder.AppendLine(string.Format("signature: '{0}',", TicketApi.GetTicketSignature("haimylife", timestamp, url, weixinAppId, masterSettings.WeixinAppSecret)));
            stringBuilder.AppendLine(" jsApiList: [\"onMenuShareAppMessage\",\"onMenuShareTimeline\"]");
            stringBuilder.AppendLine("});");
            stringBuilder.AppendLine("</script>");
            return stringBuilder.ToString();
        }

        public void RegisterUser()
        {
            string userName = Users.GetLoggedOnUsername();
            if (string.IsNullOrEmpty(userName) || userName == "Anonymous")
            {
                Token token = VMemberTemplatedWebControl.GetLoginOnToken();
                if (token != null)
                {
                    string openId = "";
                    if (!string.IsNullOrEmpty(token.openid))
                    {
                        openId = token.openid;
                        ErrorLog.Write(string.Format("注册新用户，openId={0}", openId));
                        user = VMemberTemplatedWebControl.RegisterUser(openId, token.access_token);
                        ErrorLog.Write(string.Format("新用户ID，userId={0}", user.UserId));
                        SetRecemendCode(user);
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
        }
    }
}
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	public class Login : System.Web.UI.Page
	{
		private string verifyCodeKey = "VerifyCode";
        private readonly string noticeMsg = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body class=\"body2\">\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的B2C商城系统已过授权有效期，无法登录后台管理。请续费。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.GetSiteUrls().Home + "\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2009 ecdev.cn all Rights Reserved. 本商品资源均为 惠众云商科技有限公司 版权所有</div>\r\n</div>\r\n</body>\r\n</html>";
        private readonly string licenseMsg = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body class=\"body2\">\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的B2C商城系统未经官方授权，无法登录后台管理。请联系商城官方(www.ecdev.cn)购买软件使用权。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.GetSiteUrls().Home + "\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2012 ecdev.cn all Rights Reserved. 本商品资源均为 惠众云商科技有限公司 版权所有</div>\r\n</div>\r\n</body>\r\n</html>";
        
        protected HeadContainer HeadContainer1;
		protected PageTitle PageTitle1;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected System.Web.UI.WebControls.TextBox txtAdminName;
		protected System.Web.UI.WebControls.TextBox txtAdminPassWord;
		protected System.Web.UI.HtmlControls.HtmlGenericControl imgCode;
		protected System.Web.UI.WebControls.TextBox txtCode;
		protected System.Web.UI.WebControls.Literal lblStatus;
		protected System.Web.UI.WebControls.HiddenField ErrorTimes;
		protected System.Web.UI.WebControls.Button btnAdminLogin;

		private string ReferralLink
		{
			get
			{
				return this.ViewState["ReferralLink"] as string;
			}
			set
			{
				this.ViewState["ReferralLink"] = value;
			}
		}
		
        private bool CheckVerifyCode(string verifyCode)
		{
			return base.Request.Cookies[this.verifyCodeKey] != null && string.Compare(HiCryptographer.Decrypt(base.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, System.Globalization.CultureInfo.InvariantCulture) == 0;
		}
		
        protected override void OnInit(System.EventArgs e)
		{
			if (this.Page.Request.IsAuthenticated)
			{
				System.Web.Security.FormsAuthentication.SignOut();
				System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(HiContext.Current.User.Username, true);
				IUserCookie userCookie = HiContext.Current.User.GetUserCookie();
				if (userCookie != null)
				{
					userCookie.DeleteCookie(authCookie);
				}
				RoleHelper.SignOut(HiContext.Current.User.Username);
			}
			base.OnInit(e);
		}
		
        protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnAdminLogin.Click += new System.EventHandler(this.btnAdminLogin_Click);
		}
		
        protected void Page_Load(object sender, System.EventArgs e)
		{
			if (this.GetErrorTimes("username") >= 3)
			{
				this.imgCode.Visible = true;
			}

			bool flag = !string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true";

			if (flag)
			{
				string verifyCode = base.Request["code"];
				string arg;
				if (!this.CheckVerifyCode(verifyCode))
				{
					arg = "0";
				}
				else
				{
					arg = "1";
				}
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{ ");
				base.Response.Write(string.Format("\"flag\":\"{0}\"", arg));
				base.Response.Write("}");
				base.Response.End();
			}

			if (!this.Page.IsPostBack)
			{
				System.Uri urlReferrer = this.Context.Request.UrlReferrer;
				if (urlReferrer != null)
				{
					this.ReferralLink = urlReferrer.ToString();
				}
				this.txtAdminName.Focus();
				PageTitle.AddSiteNameTitle("后台登录");
			}
		}
		
        private void btnAdminLogin_Click(object sender, System.EventArgs e)
		{
			if (this.imgCode.Visible && !HiContext.Current.CheckVerifyCode(this.txtCode.Text.Trim()))
			{
				this.ShowMessage("验证码不正确");
				return;
			}

			IUser user = Users.GetUser(0, this.txtAdminName.Text, false, true);
			if (user == null || user.IsAnonymous || user.UserRole != UserRole.SiteManager)
			{
				this.ShowMessage("无效的用户信息");
				this.SetErrorTimes("username");
				return;
			}

			string url = null;
			SiteManager siteManager = user as SiteManager;
			siteManager.Password = this.txtAdminPassWord.Text;
			LoginUserStatus loginUserStatus = ManagerHelper.ValidLogin(siteManager);

			if (loginUserStatus == LoginUserStatus.Success)
			{
				System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(siteManager.Username, false);
				IUserCookie userCookie = siteManager.GetUserCookie();
				userCookie.WriteCookie(authCookie, 30, false);
				System.Web.HttpCookie httpCookie = new System.Web.HttpCookie("Admin-system");
				httpCookie.Value = siteManager.Username;
				httpCookie.Expires = System.DateTime.Now.AddMinutes(30.0);
				System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
				HiContext.Current.User = siteManager;
				this.RemoveCache();
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["returnUrl"]))
				{
					url = this.Page.Request.QueryString["returnUrl"];
				}
				if (url == null && this.ReferralLink != null && !string.IsNullOrEmpty(this.ReferralLink.Trim()))
				{
					url = this.ReferralLink;
				}
				if (!string.IsNullOrEmpty(url) && (url.ToLower().IndexOf(Globals.GetSiteUrls().Logout.ToLower()) >= 0 || url.ToLower().IndexOf(Globals.GetSiteUrls().UrlData.FormatUrl("register").ToLower()) >= 0 || url.ToLower().IndexOf(Globals.GetSiteUrls().UrlData.FormatUrl("vote").ToLower()) >= 0 || url.ToLower().IndexOf("loginexit") >= 0))
				{
					url = null;
				}
                System.Web.HttpCookie nowcookie = new System.Web.HttpCookie("Supplier");
                if (siteManager.IsInRole("供货商"))
                { 
                    nowcookie.Value = "Supplier";
                    nowcookie.Expires = System.DateTime.Now.AddMinutes(30.0);
                    System.Web.HttpContext.Current.Response.Cookies.Add(nowcookie);
                }
                else
                {
                    nowcookie.Value = "";
                    nowcookie.Expires = System.DateTime.Now.AddMinutes(30.0);
                    System.Web.HttpContext.Current.Response.Cookies.Add(nowcookie);
                }
				if (url != null)
				{
					this.Page.Response.Redirect(url, true);
					return;
				}          
				this.Page.Response.Redirect("default.html", true);
				return;
			}
			else
			{
				if (loginUserStatus == LoginUserStatus.AccountPending)
				{
					this.SetErrorTimes("username");
					this.ShowMessage("用户账号还没有通过审核");
					return;
				}
				if (loginUserStatus == LoginUserStatus.AccountLockedOut)
				{
					this.SetErrorTimes("username");
					this.ShowMessage("用户账号已被锁定，暂时不能登录系统");
					return;
				}
				if (loginUserStatus == LoginUserStatus.InvalidCredentials)
				{
					this.SetErrorTimes("username");
					this.ShowMessage("用户名或密码错误");
					return;
				}
				this.SetErrorTimes("username");
				this.ShowMessage("登录失败，未知错误");
				return;
			}
		}
		
        private void ShowMessage(string msg)
		{
			this.lblStatus.Text = msg;
			this.lblStatus.Visible = true;
		}
		
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
            ErrorLog.Write("Globals.IsTestDomain：" + Globals.IsTestDomain);

			if (Globals.IsTestDomain)
			{
				base.Render(writer);
				return;
			}

            int serviceStatus = -1;

			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

            if (masterSettings != null)
			{
				masterSettings.ServiceStatus = CopyrightLicenser.CheckService("service") ? 1 : 0;
                masterSettings.OpenAliho = CopyrightLicenser.CheckService("aliho") ? 1 : 0;
                masterSettings.OpenMobbile = CopyrightLicenser.CheckService("app") ? 1 : 0;
                masterSettings.OpenVstore = CopyrightLicenser.CheckService("vshop") ? 1 : 0;
                masterSettings.OpenTaobao = CopyrightLicenser.CheckService("taobao") ? 1 : 0;
                masterSettings.OpenWap = CopyrightLicenser.CheckService("wap") ? 1 : 0;

				SettingsManager.Save(masterSettings);

                serviceStatus = masterSettings.ServiceStatus;
			}

            //if (serviceStatus == 0)
            //{
            //    writer.Write(this.licenseMsg);
            //    return;
            //}

            //if (serviceStatus == -1)
            //{
            //    writer.Write(this.noticeMsg);
            //    return;
            //}

			base.Render(writer);
		}

		private int GetErrorTimes(string username)
		{
			object obj = HiContext.Current.Context.Cache.Get(username);
			return (obj == null) ? 1 : ((int)obj);
		}
		
        private int SetErrorTimes(string username)
		{
			int num = this.GetErrorTimes(username) + 1;
			HiContext.Current.Context.Cache.Insert(username, num);
			return num;
		}
		
        private void RemoveCache()
		{
			HiContext.Current.Context.Cache.Remove("username");
		}
	}
}

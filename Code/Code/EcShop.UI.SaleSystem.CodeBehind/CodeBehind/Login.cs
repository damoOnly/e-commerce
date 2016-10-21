using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Members;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class Login : HtmlTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtUserName;
		private System.Web.UI.WebControls.TextBox txtPassword;
		private IButton btnLogin;
		private System.Web.UI.WebControls.DropDownList ddlPlugins;
		private static string ReturnURL = string.Empty;
        LoginLog logdetail = new LoginLog();
        private HiddenField currFlag;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Login.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (this.Context.Request.IsAuthenticated)
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
			if (!string.IsNullOrEmpty(this.Page.Request["action"]) && this.Page.Request["action"] == "Common_UserLogin")
			{
				string text = this.UserLogin(this.Page.Request["username"], this.Page.Request["password"]);
				string text2 = string.IsNullOrEmpty(text) ? "Succes" : "Fail";
				this.Page.Response.Clear();
				this.Page.Response.ContentType = "application/json";
				string s = string.Concat(new string[]
				{
					"{\"Status\":\"",
					text2,
					"\",\"Msg\":\"",
					text,
					"\"}"
				});
				this.Page.Response.Write(s);
				this.Page.Response.End();
			}
			this.txtUserName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtUserName");
			this.txtPassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtPassword");
			this.btnLogin = ButtonManager.Create(this.FindControl("btnLogin"));
			this.ddlPlugins = (System.Web.UI.WebControls.DropDownList)this.FindControl("ddlPlugins");
            this.currFlag = (System.Web.UI.WebControls.HiddenField)this.FindControl("currFlag");
			if (this.ddlPlugins != null)
			{
				this.ddlPlugins.Items.Add(new System.Web.UI.WebControls.ListItem("请选择登录方式", ""));
				System.Collections.Generic.IList<OpenIdSettingsInfo> configedItems = MemberProcessor.GetConfigedItems();
				if (configedItems != null && configedItems.Count > 0)
				{
					foreach (OpenIdSettingsInfo current in configedItems)
					{
						this.ddlPlugins.Items.Add(new System.Web.UI.WebControls.ListItem(current.Name, current.OpenIdType));
					}
				}
				this.ddlPlugins.SelectedIndexChanged += new System.EventHandler(this.ddlPlugins_SelectedIndexChanged);
			}
			if (this.Page.Request.UrlReferrer != null && !string.IsNullOrEmpty(this.Page.Request.UrlReferrer.OriginalString))
			{
				Login.ReturnURL = this.Page.Request.UrlReferrer.OriginalString;
			}
			this.txtUserName.Focus();
			PageTitle.AddSiteNameTitle("用户登录");
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
		}
		private void ddlPlugins_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.ddlPlugins.SelectedValue.Length > 0)
			{
				this.Page.Response.Redirect("OpenId/RedirectLogin.aspx?ot=" + this.ddlPlugins.SelectedValue);
			}
		}
		protected void btnLogin_Click(object sender, System.EventArgs e)
		{
            // 判断是登入日志信息
            logdetail = EventLogs.GetLoginLogDetails(txtUserName.Text.Trim(), 3);
            // 没有日志数据
            if (logdetail.ID == -1)
            {
                CurrLogin();
            }
            else
            {
                // 在一个小时内登入错误次数大于5
                int time = 0;
                TimeSpan a = DateTime.Now - logdetail.AddTime.Value;
                time = a.Hours;
                if (logdetail.ErrorCount >= 5 && time < 1)
                {
                    this.ShowMessage("登入错误超过5次，一个小时后才能登入或通过忘记密码重新登入", false);
                    currFlag.Value = "No";
                }
                else
                {
                    CurrLogin();
                }
            }
		}

        private void CurrLogin()
        {
            if (!this.Page.IsValid)
            {
                return;
            }
            string pattern = "[\\u4e00-\\u9fa5a-zA-Z0-9]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            if (!regex.IsMatch(this.txtUserName.Text.Trim()) || this.txtUserName.Text.Trim().Length < 2 || this.txtUserName.Text.Trim().Length > 20)
            {
                this.ShowMessage("用户名不能为空，必须以汉字或是字母开头,且在2-20个字符之间", false);
                return;
            }
            if (this.txtUserName.Text.Contains(","))
            {
                this.ShowMessage("用户名不能包含逗号", false);
                return;
            }
            string text = this.UserLogin(this.txtUserName.Text.Trim(), this.txtPassword.Text);
            if (!string.IsNullOrEmpty(text))
            {
                // 登入失败插入日志
                logdetail.Type = 0;
                logdetail.MemberName = txtUserName.Text.Trim();
                logdetail.LogType = 3;
                logdetail.LoginIP = Globals.IPAddress;
                EventLogs.UpdateLoginLog(logdetail);
                this.ShowMessage(text, false);
                return;
            }
            string text2 = this.Page.Request.QueryString["ReturnUrl"];
            if (string.IsNullOrEmpty(text2))
            {
                text2 = Globals.ApplicationPath + "/User/UserDefault.aspx";
            }
            else
            {
                if (string.IsNullOrEmpty(Login.ReturnURL))
                {
                    text2 = Login.ReturnURL;
                }
            }
            // 登入成功插入日志
            logdetail.Type = 1;
            logdetail.MemberName = txtUserName.Text.Trim();
            logdetail.LogType = 3;
            logdetail.LoginIP = Globals.IPAddress;
            EventLogs.UpdateLoginLog(logdetail);
            this.Page.Response.Redirect(text2);
        }

		private string UserLogin(string userName, string password)
		{
			string result = string.Empty;
			Member member = Users.GetUser(0, userName, false, true) as Member;
			if (member == null || member.IsAnonymous)
			{
				return "用户名或密码错误";
			}
			if (member.ParentUserId.HasValue && member.ParentUserId.Value != 0)
			{
				return "您不是本站会员，请您进行注册";
			}
			member.Password = password;
			LoginUserStatus loginUserStatus = MemberProcessor.ValidLogin(member);
			if (loginUserStatus == LoginUserStatus.Success)
			{
				System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
				IUserCookie userCookie = member.GetUserCookie();
				userCookie.WriteCookie(authCookie, 30, false);
				ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
				HiContext.Current.User = member;
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
				member.OnLogin();
			}
			else
			{
				if (loginUserStatus == LoginUserStatus.AccountPending)
				{
					result = "用户账号还没有通过审核";
				}
				else
				{
					if (loginUserStatus == LoginUserStatus.InvalidCredentials)
					{
						result = "用户名或密码错误";
					}
					else
					{
						result = "未知错误";
					}
				}
			}
			return result;
		}
	}
}

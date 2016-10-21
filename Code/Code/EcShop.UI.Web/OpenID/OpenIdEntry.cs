using EcShop.Core;
using EcShop.Entities.Members;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using Ecdev.Plugins;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.Web.OpenID
{
	public class OpenIdEntry : System.Web.UI.Page
	{
		private string openIdType;
		private System.Collections.Specialized.NameValueCollection parameters;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected void Page_Load(object sender, System.EventArgs e)
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
			this.openIdType = this.Page.Request.QueryString["HIGW"];
			OpenIdSettingsInfo openIdSettings = MemberProcessor.GetOpenIdSettings(this.openIdType);
			if (openIdSettings == null)
			{
				base.Response.Write("登录失败，没有找到对应的插件配置信息。");
				return;
			}
			this.parameters = new System.Collections.Specialized.NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			};
			OpenIdNotify openIdNotify = OpenIdNotify.CreateInstance(this.openIdType, this.parameters);
			openIdNotify.Authenticated += new System.EventHandler<AuthenticatedEventArgs>(this.Notify_Authenticated);
			openIdNotify.Failed += new System.EventHandler<FailedEventArgs>(this.Notify_Failed);
			try
			{
				openIdNotify.Verify(30000, HiCryptographer.Decrypt(openIdSettings.Settings));
			}
			catch
			{
				this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
			}
		}
		private void Notify_Failed(object sender, FailedEventArgs e)
		{
			base.Response.Write("登录失败，" + e.Message);
		}
		private void Notify_Authenticated(object sender, AuthenticatedEventArgs e)
		{
			this.parameters.Add("CurrentOpenId", e.OpenId);
			HiContext current = HiContext.Current;
			string usernameWithOpenId = UserHelper.GetUsernameWithOpenId(e.OpenId, this.openIdType);
			if (!string.IsNullOrEmpty(usernameWithOpenId))
			{
				Member member = Users.GetUser(0, usernameWithOpenId, false, true) as Member;
				if (member == null)
				{
					base.Response.Write("登录失败，信任登录只能用于会员登录。");
					return;
				}
				if (member.ParentUserId.HasValue && member.ParentUserId.Value != 0)
				{
					base.Response.Write("账号已经与本平台的其它子站绑定，不能在此域名上登录。");
					return;
				}
				System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
				IUserCookie userCookie = member.GetUserCookie();
				userCookie.WriteCookie(authCookie, 30, false);
				HiContext.Current.User = member;
				ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
				current.User = member;
				if (cookieShoppingCart != null)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
					ShoppingCartProcessor.ClearCookieShoppingCart();
				}
				if (!string.IsNullOrEmpty(this.parameters["token"]))
				{
					System.Web.HttpCookie httpCookie = new System.Web.HttpCookie("Token_" + HiContext.Current.User.UserId.ToString());
					httpCookie.Expires = System.DateTime.Now.AddMinutes(30.0);
					httpCookie.Value = this.parameters["token"];
					System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
			}
			else
			{
				string a;
				if ((a = this.openIdType.ToLower()) != null)
				{
					if (a == "Ecdev.plugins.openid.alipay.alipayservice")
					{
						this.SkipAlipayOpenId();
						goto IL_1EF;
					}
					if (a == "Ecdev.plugins.openid.qq.qqservice")
					{
						this.SkipQQOpenId();
						goto IL_1EF;
					}
					if (a == "Ecdev.plugins.openid.taobao.taobaoservice")
					{
						this.SkipTaoBaoOpenId();
						goto IL_1EF;
					}
					if (a == "Ecdev.plugins.openid.sina.sinaservice")
					{
						this.SkipSinaOpenId();
						goto IL_1EF;
					}
				}
				this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
			}
			IL_1EF:
			string a2 = this.parameters["HITO"];
			if (a2 == "1")
			{
				this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("submitOrder"));
				return;
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
		}
		protected void SkipAlipayOpenId()
		{
			Member member = new Member(UserRole.Member);
			if (HiContext.Current.ReferralUserId > 0)
			{
				member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
			}
			member.GradeId = MemberProcessor.GetDefaultMemberGrade();
			member.Username = this.parameters["real_name"];
			if (string.IsNullOrEmpty(member.Username))
			{
				member.Username = "支付宝会员_" + this.parameters["user_id"];
			}
			member.Email = this.parameters["email"];
			if (string.IsNullOrEmpty(member.Email))
			{
				member.Email = this.GenerateUsername() + "@localhost.com";
			}
			string text = this.GeneratePassword();
			member.Password = text;
			member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePassword = text;
			member.IsApproved = true;
			member.RealName = string.Empty;
			member.Address = string.Empty;
			if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
			{
				member.Username = "支付宝会员_" + this.parameters["user_id"];
				member.Password = (member.TradePassword = text);
				if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
				{
					member.Username = this.GenerateUsername();
					member.Email = this.GenerateUsername() + "@localhost.com";
					member.Password = (member.TradePassword = text);
					if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
					{
						base.Response.Write("为您创建随机账户时失败，请重试。");
						return;
					}
				}
			}
			UserHelper.BindOpenId(member.Username, this.parameters["CurrentOpenId"], this.parameters["HIGW"]);
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
			if (!string.IsNullOrEmpty(this.parameters["token"]))
			{
				System.Web.HttpCookie httpCookie = new System.Web.HttpCookie("Token_" + HiContext.Current.User.UserId.ToString());
				httpCookie.Expires = System.DateTime.Now.AddMinutes(30.0);
				httpCookie.Value = this.parameters["token"];
				System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			if (!string.IsNullOrEmpty(this.parameters["target_url"]))
			{
				this.Page.Response.Redirect(this.parameters["target_url"]);
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
		}
		protected void SkipQQOpenId()
		{
			Member member = new Member(UserRole.Member);
			if (HiContext.Current.ReferralUserId > 0)
			{
				member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
			}
			member.GradeId = MemberProcessor.GetDefaultMemberGrade();
			System.Web.HttpCookie httpCookie = System.Web.HttpContext.Current.Request.Cookies["NickName"];
			if (httpCookie != null)
			{
				member.Username = System.Web.HttpUtility.UrlDecode(httpCookie.Value);
			}
			if (string.IsNullOrEmpty(member.Username))
			{
				member.Username = "腾讯会员_" + this.GenerateUsername(8);
			}
			member.Email = this.GenerateUsername() + "@localhost.com";
			string text = this.GeneratePassword();
			member.Password = text;
			member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePassword = text;
			member.IsApproved = true;
			member.RealName = string.Empty;
			member.Address = string.Empty;
			if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
			{
				member.Username = "腾讯会员_" + this.GenerateUsername(8);
				member.Password = (member.TradePassword = text);
				if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
				{
					member.Username = this.GenerateUsername();
					member.Email = this.GenerateUsername() + "@localhost.com";
					member.Password = (member.TradePassword = text);
					if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
					{
						base.Response.Write("为您创建随机账户时失败，请重试。");
						return;
					}
				}
			}
			UserHelper.BindOpenId(member.Username, this.parameters["CurrentOpenId"], this.parameters["HIGW"]);
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
			if (!string.IsNullOrEmpty(this.parameters["token"]))
			{
				System.Web.HttpCookie httpCookie2 = new System.Web.HttpCookie("Token_" + HiContext.Current.User.UserId.ToString());
				httpCookie2.Expires = System.DateTime.Now.AddMinutes(30.0);
				httpCookie2.Value = this.parameters["token"];
				System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie2);
			}
			if (!string.IsNullOrEmpty(this.parameters["target_url"]))
			{
				this.Page.Response.Redirect(this.parameters["target_url"]);
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
		}
		protected void SkipTaoBaoOpenId()
		{
			Member member = new Member(UserRole.Member);
			if (HiContext.Current.ReferralUserId > 0)
			{
				member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
			}
			member.GradeId = MemberProcessor.GetDefaultMemberGrade();
			string text = this.parameters["CurrentOpenId"];
			if (!string.IsNullOrEmpty(text))
			{
				member.Username = System.Web.HttpUtility.UrlDecode(text);
			}
			if (string.IsNullOrEmpty(member.Username))
			{
				member.Username = "淘宝会员_" + this.GenerateUsername(8);
			}
			member.Email = this.GenerateUsername() + "@localhost.com";
			if (string.IsNullOrEmpty(member.Email))
			{
				member.Email = this.GenerateUsername() + "@localhost.com";
			}
			string text2 = this.GeneratePassword();
			member.Password = text2;
			member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePassword = text2;
			member.IsApproved = true;
			member.RealName = string.Empty;
			member.Address = string.Empty;
			if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
			{
				member.Username = "淘宝会员_" + this.GenerateUsername(8);
				member.Password = (member.TradePassword = text2);
				if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
				{
					member.Username = this.GenerateUsername();
					member.Email = this.GenerateUsername() + "@localhost.com";
					member.Password = (member.TradePassword = text2);
					if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
					{
						base.Response.Write("为您创建随机账户时失败，请重试。");
						return;
					}
				}
			}
			UserHelper.BindOpenId(member.Username, this.parameters["CurrentOpenId"], this.parameters["HIGW"]);
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
			if (!string.IsNullOrEmpty(this.parameters["token"]))
			{
				System.Web.HttpCookie httpCookie = new System.Web.HttpCookie("Token_" + HiContext.Current.User.UserId.ToString());
				httpCookie.Expires = System.DateTime.Now.AddMinutes(30.0);
				httpCookie.Value = this.parameters["token"];
				System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			if (!string.IsNullOrEmpty(this.parameters["target_url"]))
			{
				this.Page.Response.Redirect(this.parameters["target_url"]);
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
		}
		protected void SkipSinaOpenId()
		{
			Member member = new Member(UserRole.Member);
			if (HiContext.Current.ReferralUserId > 0)
			{
				member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
			}
			member.GradeId = MemberProcessor.GetDefaultMemberGrade();
			member.Username = this.parameters["CurrentOpenId"];
			if (string.IsNullOrEmpty(member.Username))
			{
				member.Username = "新浪微博会员_" + this.GenerateUsername(8);
			}
			member.Email = this.GenerateUsername() + "@localhost.com";
			string text = this.GeneratePassword();
			member.Password = text;
			member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePassword = text;
			member.IsApproved = true;
			member.RealName = string.Empty;
			member.Address = string.Empty;
			if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
			{
				member.Username = "新浪微博会员_" + this.GenerateUsername(9);
				member.Password = (member.TradePassword = text);
				if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
				{
					member.Username = this.GenerateUsername();
					member.Email = this.GenerateUsername() + "@localhost.com";
					member.Password = (member.TradePassword = text);
					if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
					{
						base.Response.Write("为您创建随机账户时失败，请重试。");
						return;
					}
				}
			}
			UserHelper.BindOpenId(member.Username, this.parameters["CurrentOpenId"], this.parameters["HIGW"]);
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
			if (!string.IsNullOrEmpty(this.parameters["token"]))
			{
				System.Web.HttpCookie httpCookie = new System.Web.HttpCookie("Token_" + HiContext.Current.User.UserId.ToString());
				httpCookie.Expires = System.DateTime.Now.AddMinutes(30.0);
				httpCookie.Value = this.parameters["token"];
				System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
		}
		private string GenerateUsername(int length)
		{
			return this.GenerateRndString(length, "u_");
		}
		private string GenerateUsername()
		{
			return this.GenerateRndString(10, "u_");
		}
		private string GeneratePassword()
		{
			return this.GenerateRndString(8, "");
		}
		private string GenerateRndString(int length, string prefix)
		{
			string text = string.Empty;
			System.Random random = new System.Random();
			while (text.Length < 10)
			{
				int num = random.Next();
				char c;
				if (num % 3 == 0)
				{
					c = (char)(97 + (ushort)(num % 26));
				}
				else
				{
					c = (char)(48 + (ushort)(num % 10));
				}
				text += c.ToString();
			}
			return prefix + text;
		}
	}
}

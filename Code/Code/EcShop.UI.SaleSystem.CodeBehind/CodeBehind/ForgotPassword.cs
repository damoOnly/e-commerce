using EcShop.Core;
using EcShop.Core.Configuration;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.UI.Common.Controls;
using System;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class ForgotPassword : HtmlTemplatedWebControl
	{
		private static string mobileCode = "";
		private static string emailCode = "";
		private System.Web.UI.HtmlControls.HtmlGenericControl htmDivUserName;
		private System.Web.UI.WebControls.TextBox txtUserName;
		private IButton btnCheckUserName;
		private IButton btnCheckMobile;
		private IButton btnCheckEmail;
		private System.Web.UI.HtmlControls.HtmlGenericControl htmDivQuestionAndAnswer;
		private System.Web.UI.WebControls.Literal litUserQuestion;
		private System.Web.UI.WebControls.Literal litMobile;
		private System.Web.UI.WebControls.Literal litEmail;
		private System.Web.UI.WebControls.TextBox txtUserAnswer;
		private System.Web.UI.WebControls.TextBox txtMobileValid;
		private System.Web.UI.WebControls.TextBox txtEmailValid;
		private System.Web.UI.WebControls.Literal litAnswerMessage;
		private IButton btnCheckAnswer;
		private System.Web.UI.WebControls.DropDownList dropType;
		private System.Web.UI.HtmlControls.HtmlGenericControl htmDivPassword;
		private System.Web.UI.WebControls.TextBox txtPassword;
		private System.Web.UI.WebControls.TextBox txtRePassword;
		private IButton btnSetPassword;
		private IButton btnSendMobile;
		private IButton btnSendEmail;
		private IButton btnPrev;
		private IButton btnPrev2;
		private IButton btnPrev3;
		private System.Web.UI.HtmlControls.HtmlGenericControl htmDivCellPhone;
		private System.Web.UI.HtmlControls.HtmlGenericControl htmDivEmail;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ForgotPassword.html";
			}
			base.OnInit(e);
		}
		private void LoadType()
		{
			this.dropType.Items.Clear();
			this.dropType.Items.Add(new System.Web.UI.WebControls.ListItem("通过密保问题", "0"));
			this.dropType.Items.Add(new System.Web.UI.WebControls.ListItem("通过手机号码", "1"));
			this.dropType.Items.Add(new System.Web.UI.WebControls.ListItem("通过电子邮箱", "2"));
		}
		protected override void AttachChildControls()
		{
			this.htmDivUserName = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("htmDivUserName");
			this.txtUserName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtUserName");
			this.btnCheckUserName = ButtonManager.Create(this.FindControl("btnCheckUserName"));
			this.btnCheckMobile = ButtonManager.Create(this.FindControl("btnCheckMobile"));
			this.btnCheckEmail = ButtonManager.Create(this.FindControl("btnCheckEmail"));
			this.htmDivQuestionAndAnswer = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("htmDivQuestionAndAnswer");
			this.litUserQuestion = (System.Web.UI.WebControls.Literal)this.FindControl("litUserQuestion");
			this.litMobile = (System.Web.UI.WebControls.Literal)this.FindControl("litMobile");
			this.litEmail = (System.Web.UI.WebControls.Literal)this.FindControl("litEmail");
			this.txtUserAnswer = (System.Web.UI.WebControls.TextBox)this.FindControl("txtUserAnswer");
			this.txtMobileValid = (System.Web.UI.WebControls.TextBox)this.FindControl("txtMobileValid");
			this.txtEmailValid = (System.Web.UI.WebControls.TextBox)this.FindControl("txtEmailValid");
			this.litAnswerMessage = (System.Web.UI.WebControls.Literal)this.FindControl("litAnswerMessage");
			this.btnCheckAnswer = ButtonManager.Create(this.FindControl("btnCheckAnswer"));
			this.btnSendMobile = ButtonManager.Create(this.FindControl("btnSendMobile"));
			this.btnSendMobile.Click += new System.EventHandler(this.btnSendMobile_Click);
			this.btnSendEmail = ButtonManager.Create(this.FindControl("btnSendEmail"));
			this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
			this.btnPrev = ButtonManager.Create(this.FindControl("btnPrev"));
			this.btnPrev2 = ButtonManager.Create(this.FindControl("btnPrev2"));
			this.btnPrev3 = ButtonManager.Create(this.FindControl("btnPrev3"));
			this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
			this.btnPrev2.Click += new System.EventHandler(this.btnPrev_Click);
			this.btnPrev3.Click += new System.EventHandler(this.btnPrev_Click);
			this.htmDivPassword = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("htmDivPassword");
			this.htmDivCellPhone = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("htmDivCellPhone");
			this.htmDivEmail = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("htmDivEmail");
			this.txtPassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtPassword");
			this.txtRePassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRePassword");
			this.dropType = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropType");
			this.btnSetPassword = ButtonManager.Create(this.FindControl("btnSetPassword"));
			PageTitle.AddSiteNameTitle("找回密码");
			this.btnCheckUserName.Click += new System.EventHandler(this.btnCheckUserName_Click);
			this.btnCheckMobile.Click += new System.EventHandler(this.btnCheckMobile_Click);
			this.btnCheckEmail.Click += new System.EventHandler(this.btnCheckEmail_Click);
			this.btnCheckAnswer.Click += new System.EventHandler(this.btnCheckAnswer_Click);
			this.btnSetPassword.Click += new System.EventHandler(this.btnSetPassword_Click);
			if (!this.Page.IsPostBack)
			{
				this.LoadType();
				this.panelShow("InputUserName");
				ForgotPassword.mobileCode = "";
				ForgotPassword.emailCode = "";
			}
		}
		private void btnPrev_Click(object sender, System.EventArgs e)
		{
			this.LoadType();
			this.panelShow("InputUserName");
			ForgotPassword.mobileCode = "";
			ForgotPassword.emailCode = "";
		}
		private void btnSendMobile_Click(object sender, System.EventArgs e)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			IUser user = Users.FindUserByUsername(this.txtUserName.Text.Trim());
			if (user is Member)
			{
				Member member = user as Member;
				ForgotPassword.mobileCode = HiContext.Current.CreateVerifyCode(6);
				string text;
				SendStatus sendStatus = Messenger.SendSMS(member.CellPhone, "您本次的验证码是：" + ForgotPassword.mobileCode, siteSettings, out text);
				if (sendStatus == SendStatus.NoProvider || sendStatus == SendStatus.ConfigError)
				{
					this.ShowMessage("后台设置错误，请自行联系后台管理员", false);
					return;
				}
				if (sendStatus == SendStatus.Fail)
				{
					this.ShowMessage("发送失败", false);
					return;
				}
				if (sendStatus == SendStatus.Success)
				{
					this.ShowMessage("发送成功", true);
				}
			}
		}
		private void btnSendEmail_Click(object sender, System.EventArgs e)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			IUser user = Users.FindUserByUsername(this.txtUserName.Text.Trim());
			ForgotPassword.emailCode = HiContext.Current.CreateVerifyCode(6);
			string body = string.Format("亲爱的{0}：<br>您好！感谢您使用{1}。<br>您正在进行账户基础信息维护，请在校验码输入框中输入：{2}，以完成操作。 <br>注意：此操作可能会修改您的密码、登录邮箱或绑定手机。如非本人操作，请及时登录并修改密码以保证账户安全。（工作人员不会向您索取此校验码，请勿泄漏！） ", user.Username, siteSettings.SiteName, ForgotPassword.emailCode);
			string text;
			SendStatus sendStatus = Messenger.SendMail("验证码", body, user.Email, siteSettings, out text);
			if (sendStatus == SendStatus.NoProvider || sendStatus == SendStatus.ConfigError)
			{
				this.ShowMessage("后台设置错误，请自行联系后台管理员", false);
				return;
			}
			if (sendStatus == SendStatus.Fail)
			{
				this.ShowMessage("发送失败", false);
				return;
			}
			if (sendStatus == SendStatus.Success)
			{
				this.ShowMessage("发送成功", true);
			}
		}
		private void btnCheckEmail_Click(object sender, System.EventArgs e)
		{
			if (this.txtEmailValid.Text.Length == 0)
			{
				this.ShowMessage("请输入验证码", false);
				return;
			}
			if (!this.txtEmailValid.Text.ToLower().Equals(ForgotPassword.emailCode.ToLower()))
			{
				this.ShowMessage("验证码输入错误，请重新输入", false);
				return;
			}
			this.panelShow("InputPassword");
			ForgotPassword.emailCode = "";
		}
		private void btnCheckMobile_Click(object sender, System.EventArgs e)
		{
			if (this.txtMobileValid.Text.Length == 0)
			{
				this.ShowMessage("请输入验证码", false);
				return;
			}
			if (!this.txtMobileValid.Text.ToLower().Equals(ForgotPassword.mobileCode.ToLower()))
			{
				this.ShowMessage("验证码输入错误，请重新输入", false);
				return;
			}
			this.panelShow("InputPassword");
			ForgotPassword.mobileCode = "";
		}
		private void btnCheckUserName_Click(object sender, System.EventArgs e)
		{
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
			IUser user = Users.FindUserByUsername(this.txtUserName.Text.Trim());
			if (user != null && user.UserRole != UserRole.SiteManager && user.UserRole != UserRole.Anonymous)
			{
				IUser user2 = Users.GetUser(0, this.txtUserName.Text.Trim(), false, true);
				Member member = user2 as Member;
				if (member.ParentUserId.HasValue && member.ParentUserId.Value != 0)
				{
					this.ShowMessage("您不是本站会员，请您进行注册", false);
					return;
				}
				if (this.dropType.SelectedIndex == 0)
				{
					if (!string.IsNullOrEmpty(user.PasswordQuestion))
					{
						if (this.litUserQuestion != null)
						{
							this.litUserQuestion.Text = user.PasswordQuestion.ToString();
						}
						this.panelShow("InputAnswer");
						return;
					}
					this.ShowMessage("您没有设置密保问题，无法通过密保问题找回密码，请通过其他方式找回密码或是联系管理员修改密码", false);
					return;
				}
				else
				{
					if (this.dropType.SelectedIndex == 1)
					{
						if (user is Member)
						{
							member = (user as Member);
							if (!string.IsNullOrEmpty(member.CellPhone))
							{
								if (this.litMobile != null)
								{
									this.litMobile.Text = member.CellPhone.Substring(0, 3) + "****" + member.CellPhone.Substring(7);
								}
								this.panelShow("CellPhone");
								return;
							}
							this.ShowMessage("您没有设置手机号码，无法通过手机号码找回密码，请通过其他方式找回密码或是联系管理员修改密码", false);
							return;
						}
					}
					else
					{
						if (this.dropType.SelectedIndex == 2)
						{
                            if (!string.IsNullOrEmpty(user.Email) && user.Email.IndexOf("haimylife") == -1)
                            {
                                if (this.litEmail != null)
                                {
                                    this.litEmail.Text = user.Email;
                                }
                                this.panelShow("Email");
                                return;
                            }
							this.ShowMessage("没有设置电子邮箱", false);
							return;
						}
					}
				}
			}
			else
			{
				this.ShowMessage("该用户不存在", false);
			}
		}
		private void btnCheckAnswer_Click(object sender, System.EventArgs e)
		{
			IUser user = Users.FindUserByUsername(this.txtUserName.Text.Trim());
			if (user.MembershipUser.ValidatePasswordAnswer(this.txtUserAnswer.Text.Trim()))
			{
				this.panelShow("InputPassword");
				return;
			}
			this.litAnswerMessage.Visible = true;
		}
		private void btnSetPassword_Click(object sender, System.EventArgs e)
		{
			IUser user = Users.FindUserByUsername(this.txtUserName.Text.Trim());
			if (string.IsNullOrEmpty(this.txtPassword.Text.Trim()) || string.IsNullOrEmpty(this.txtRePassword.Text.Trim()))
			{
				this.ShowMessage("密码不允许为空！", false);
				return;
			}
			if (this.txtPassword.Text.Trim() != this.txtRePassword.Text.Trim())
			{
				this.ShowMessage("两次输入的密码需一致", false);
				return;
			}
			if (this.txtPassword.Text.Length < System.Web.Security.Membership.Provider.MinRequiredPasswordLength || this.txtPassword.Text.Length > HiConfiguration.GetConfig().PasswordMaxLength)
			{
				this.ShowMessage(string.Format("密码的长度只能在{0}和{1}个字符之间", System.Web.Security.Membership.Provider.MinRequiredPasswordLength, HiConfiguration.GetConfig().PasswordMaxLength), false);
				return;
			}
			bool flag;
			if (this.dropType.SelectedIndex == 0)
			{
				flag = user.ChangePasswordWithAnswer(this.txtUserAnswer.Text, this.txtPassword.Text);
			}
			else
			{
				if (user is Member)
				{
					Member member = user as Member;
					flag = member.ChangePasswordWithoutAnswer(this.txtPassword.Text);
				}
				else
				{
					flag = user.ChangePassword(this.txtPassword.Text);
				}
			}
			if (flag)
			{
				Messenger.UserPasswordForgotten(user, this.txtPassword.Text);
				this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("ForgotPasswordSuccess") + string.Format("?UserName={0}", user.Username));
				return;
			}
			this.ShowMessage("登录密码修改失败，请重试", false);
		}
		private void panelShow(string type)
		{
			this.litAnswerMessage.Visible = false;
			if (type == "InputUserName")
			{
				this.htmDivUserName.Visible = true;
				this.htmDivQuestionAndAnswer.Visible = false;
				this.htmDivPassword.Visible = false;
				this.htmDivCellPhone.Visible = false;
				this.htmDivEmail.Visible = false;
				return;
			}
			if (type == "CellPhone")
			{
				this.htmDivCellPhone.Visible = true;
				this.htmDivUserName.Visible = false;
				this.htmDivQuestionAndAnswer.Visible = false;
				this.htmDivPassword.Visible = false;
				this.htmDivEmail.Visible = false;
				return;
			}
			if (type == "Email")
			{
				this.htmDivEmail.Visible = true;
				this.htmDivCellPhone.Visible = false;
				this.htmDivUserName.Visible = false;
				this.htmDivQuestionAndAnswer.Visible = false;
				this.htmDivPassword.Visible = false;
				return;
			}
			if (type == "InputAnswer")
			{
				this.htmDivUserName.Visible = false;
				this.htmDivQuestionAndAnswer.Visible = true;
				this.htmDivPassword.Visible = false;
				this.htmDivCellPhone.Visible = false;
				this.htmDivEmail.Visible = false;
				return;
			}
			if (type == "InputPassword")
			{
				this.htmDivUserName.Visible = false;
				this.htmDivQuestionAndAnswer.Visible = false;
				this.htmDivPassword.Visible = true;
				this.htmDivCellPhone.Visible = false;
				this.htmDivEmail.Visible = false;
			}
		}
	}
}

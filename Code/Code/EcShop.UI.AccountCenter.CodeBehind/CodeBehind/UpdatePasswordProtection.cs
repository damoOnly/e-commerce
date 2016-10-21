using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UpdatePasswordProtection : MemberTemplatedWebControl
	{
		private SmallStatusMessage StatusPasswordProtection;
		private System.Web.UI.WebControls.Literal litOldQuestion;
		private System.Web.UI.WebControls.TextBox txtOdeAnswer;
		private System.Web.UI.WebControls.TextBox txtQuestion;
		private System.Web.UI.WebControls.TextBox txtAnswer;
		private IButton btnOK3;
		private System.Web.UI.WebControls.Panel tblrOldQuestion;
		private System.Web.UI.WebControls.Panel tblrOldAnswer;
		private System.Web.UI.HtmlControls.HtmlGenericControl LkUpdateTradePassword;
		protected virtual void ShowMessage(SmallStatusMessage state, string msg, bool success)
		{
			if (state != null)
			{
				state.Success = success;
				state.Text = msg;
				state.Visible = true;
			}
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UpdatePasswordProtection.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litOldQuestion = (System.Web.UI.WebControls.Literal)this.FindControl("litOldQuestion");
			this.txtOdeAnswer = (System.Web.UI.WebControls.TextBox)this.FindControl("txtOdeAnswer");
			this.txtQuestion = (System.Web.UI.WebControls.TextBox)this.FindControl("txtQuestion");
			this.txtAnswer = (System.Web.UI.WebControls.TextBox)this.FindControl("txtAnswer");
			this.LkUpdateTradePassword = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("one2");
			this.btnOK3 = ButtonManager.Create(this.FindControl("btnOK3"));
			this.StatusPasswordProtection = (SmallStatusMessage)this.FindControl("StatusPasswordProtection");
			this.tblrOldQuestion = (System.Web.UI.WebControls.Panel)this.FindControl("tblrOldQuestion");
			this.tblrOldAnswer = (System.Web.UI.WebControls.Panel)this.FindControl("tblrOldAnswer");
			PageTitle.AddSiteNameTitle("修改密码保护");
			this.btnOK3.Click += new System.EventHandler(this.btnOK3_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindAnswerAndQuestion();
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (!member.IsOpenBalance)
				{
					this.LkUpdateTradePassword.Visible = false;
				}
			}
		}
		private void BindAnswerAndQuestion()
		{
			IUser user = Users.GetUser(HiContext.Current.User.UserId, false);
			if (user != null)
			{
				this.tblrOldQuestion.Visible = (this.tblrOldAnswer.Visible = !string.IsNullOrEmpty(user.PasswordQuestion));
				this.litOldQuestion.Text = user.PasswordQuestion;
			}
		}
		private void btnOK3_Click(object sender, System.EventArgs e)
		{
			IUser user = Users.GetUser(HiContext.Current.User.UserId, false);
			if (user.MembershipUser != null && user.MembershipUser.IsLockedOut)
			{
				this.ShowMessage(this.StatusPasswordProtection, "你已经被管理员锁定", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtQuestion.Text) || string.IsNullOrEmpty(this.txtAnswer.Text))
			{
				this.ShowMessage(this.StatusPasswordProtection, "问题和答案为必填项", false);
				return;
			}
			if (!string.IsNullOrEmpty(user.PasswordQuestion))
			{
				if (user.ChangePasswordQuestionAndAnswer(Globals.HtmlEncode(this.txtOdeAnswer.Text), Globals.HtmlEncode(this.txtQuestion.Text), Globals.HtmlEncode(this.txtAnswer.Text)))
				{
					Users.ClearUserCache(user);
					this.BindAnswerAndQuestion();
					this.ShowMessage(this.StatusPasswordProtection, "成功修改了密码答案", true);
					return;
				}
				this.ShowMessage(this.StatusPasswordProtection, "修改密码答案失败", false);
				return;
			}
			else
			{
				if (user.ChangePasswordQuestionAndAnswer(Globals.HtmlEncode(this.txtQuestion.Text), Globals.HtmlEncode(this.txtAnswer.Text)))
				{
					Users.ClearUserCache(user);
					this.BindAnswerAndQuestion();
					this.ShowMessage(this.StatusPasswordProtection, "成功修改了密码答案", true);
					return;
				}
				this.ShowMessage(this.StatusPasswordProtection, "修改密码答案失败", false);
				return;
			}
		}
	}
}

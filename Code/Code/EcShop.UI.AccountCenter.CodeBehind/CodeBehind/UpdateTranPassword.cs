using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Messages;
using EcShop.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UpdateTranPassword : MemberTemplatedWebControl
	{
		private SmallStatusMessage StatusTransactionPass;
		private System.Web.UI.WebControls.TextBox txtOldTransactionPassWord;
		private System.Web.UI.WebControls.TextBox txtNewTransactionPassWord;
		private System.Web.UI.WebControls.TextBox txtNewTransactionPassWord2;
		private IButton btnOK2;
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
				this.SkinName = "User/Skin-UpdateTranPassword.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtOldTransactionPassWord = (System.Web.UI.WebControls.TextBox)this.FindControl("txtOldTransactionPassWord");
			this.txtNewTransactionPassWord = (System.Web.UI.WebControls.TextBox)this.FindControl("txtNewTransactionPassWord");
			this.txtNewTransactionPassWord2 = (System.Web.UI.WebControls.TextBox)this.FindControl("txtNewTransactionPassWord2");
			this.btnOK2 = ButtonManager.Create(this.FindControl("btnOK2"));
			this.StatusTransactionPass = (SmallStatusMessage)this.FindControl("StatusTransactionPass");
			PageTitle.AddSiteNameTitle("修改交易密码");
			this.btnOK2.Click += new System.EventHandler(this.btnOK2_Click);
			if (!this.Page.IsPostBack)
			{
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (!member.IsOpenBalance)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/OpenBalance.aspx?ReturnUrl={0}", System.Web.HttpContext.Current.Request.Url));
				}
			}
		}
		private void btnOK2_Click(object sender, System.EventArgs e)
		{
			Member member = HiContext.Current.User as Member;
			if (member.MembershipUser != null && member.MembershipUser.IsLockedOut)
			{
				this.ShowMessage(this.StatusTransactionPass, "你已经被管理员锁定", false);
				return;
			}
			member.TradePassword = this.txtOldTransactionPassWord.Text;
			if (member.ChangeTradePassword(this.txtOldTransactionPassWord.Text, this.txtNewTransactionPassWord.Text))
			{
				Messenger.UserDealPasswordChanged(member, this.txtNewTransactionPassWord.Text);
				member.OnDealPasswordChanged(new UserEventArgs(member.Username, null, this.txtNewTransactionPassWord.Text));
				this.ShowMessage(this.StatusTransactionPass, "你已经成功的修改了交易密码", true);
				return;
			}
			this.ShowMessage(this.StatusTransactionPass, "修改交易密码失败", false);
		}
	}
}

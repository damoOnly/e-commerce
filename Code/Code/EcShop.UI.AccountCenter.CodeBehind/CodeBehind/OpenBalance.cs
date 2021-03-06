using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class OpenBalance : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtTranPassword;
		private System.Web.UI.WebControls.TextBox txtTranPassword2;
		private IButton btnOpen;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-OpenBalance.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtTranPassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTranPassword");
			this.txtTranPassword2 = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTranPassword2");
			this.btnOpen = ButtonManager.Create(this.FindControl("btnOpen"));
			PageTitle.AddSiteNameTitle("开启预付款账户");
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			if (!this.Page.IsPostBack)
			{
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (member.IsOpenBalance)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/MyAccountSummary.aspx", new object[0]));
				}
			}
		}
		protected void btnOpen_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtTranPassword.Text))
			{
				this.ShowMessage("请输入交易密码", false);
				return;
			}
			if (this.txtTranPassword.Text.Length < 6 || this.txtTranPassword.Text.Length > 20)
			{
				this.ShowMessage("交易密码限制为6-20个字符", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtTranPassword2.Text))
			{
				this.ShowMessage("请确认交易密码", false);
				return;
			}
			if (string.Compare(this.txtTranPassword2.Text, this.txtTranPassword.Text) != 0)
			{
				this.ShowMessage("两次输入的交易密码不一致", false);
				return;
			}
			Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
			if (member.OpenBalance(this.txtTranPassword.Text))
			{
				if (string.IsNullOrEmpty(this.Page.Request.QueryString["ReturnUrl"]))
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/MyAccountSummary.aspx", new object[0]));
					return;
				}
				this.Page.Response.Redirect(this.Page.Request.QueryString["ReturnUrl"]);
			}
		}
	}
}

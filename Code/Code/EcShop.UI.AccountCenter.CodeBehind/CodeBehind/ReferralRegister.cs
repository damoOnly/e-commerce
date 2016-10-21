using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class ReferralRegister : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtRealName;
		private System.Web.UI.WebControls.TextBox txtCellPhone;
		private System.Web.UI.WebControls.TextBox txtReferralReason;
        private System.Web.UI.WebControls.TextBox txtEmail;
		private IButton btnReferral;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-ReferralRegister.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtRealName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRealName");
			this.txtCellPhone = (System.Web.UI.WebControls.TextBox)this.FindControl("txtCellPhone");
			this.txtReferralReason = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReferralReason");
			this.btnReferral = ButtonManager.Create(this.FindControl("btnReferral"));
            this.txtEmail = (System.Web.UI.WebControls.TextBox)this.FindControl("txtEmail");
			this.btnReferral.Click += new System.EventHandler(this.btnReferral_Click);
			PageTitle.AddSiteNameTitle("推广员申请表单");
			if (!this.Page.IsPostBack)
			{
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (member.ReferralStatus != 0 && this.Page.Request.QueryString["again"] != "1")
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + "/user/ReferralRegisterresults.aspx");
				}
				this.txtRealName.Text = member.RealName;
				this.txtCellPhone.Text = member.CellPhone;
                this.txtEmail.Text = member.Email;
                this.txtReferralReason.Text = member.ReferralReason;
			}
		}

		private void btnReferral_Click(object sender, System.EventArgs e)
		{
            if (MemberProcessor.ReferralRequest(HiContext.Current.User.UserId, this.txtRealName.Text, this.txtCellPhone.Text, this.txtReferralReason.Text, this.txtEmail.Text))
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/user/ReferralRegisterresults.aspx");
				return;
			}
			this.ShowMessage("申请失败，请重试", false);
		}
	}
}

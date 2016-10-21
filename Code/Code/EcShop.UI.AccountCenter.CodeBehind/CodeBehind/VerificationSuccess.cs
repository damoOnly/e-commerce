using EcShop.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VerificationSuccess : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litbanner;
		private System.Web.UI.WebControls.Literal littitle;
		private System.Web.UI.WebControls.Literal litmsg;
		private System.Web.UI.WebControls.Literal litimage;
		private System.Web.UI.WebControls.Literal litGrade;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserVerificationSuccess.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litbanner = (System.Web.UI.WebControls.Literal)this.FindControl("litbanner");
			this.littitle = (System.Web.UI.WebControls.Literal)this.FindControl("littitle");
			this.litmsg = (System.Web.UI.WebControls.Literal)this.FindControl("litmsg");
			this.litimage = (System.Web.UI.WebControls.Literal)this.FindControl("litimage");
			this.litGrade = (System.Web.UI.WebControls.Literal)this.FindControl("litGrade");
			if (!this.Page.IsPostBack)
			{
				this.ShowMessage();
			}
		}
		private void ShowMessage()
		{
			string text = this.Page.Request["type"];
			string a;
			string text2;
			if ((a = text) != null)
			{
				if (a == "1")
				{
					text2 = "邮箱验证";
					goto IL_53;
				}
				if (a == "2")
				{
					text2 = "密码问题设置";
					goto IL_53;
				}
			}
			text2 = "手机验证";
			IL_53:
			this.litbanner.Text = (this.littitle.Text = (this.litmsg.Text = (this.litimage.Text = text2)));
			Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
			if (member.EmailVerification && member.CellPhoneVerification && member.PasswordQuestion != "")
			{
				this.litGrade.Text = "恭喜您，您的账号安全等级已设置到最高级了！";
			}
		}
	}
}

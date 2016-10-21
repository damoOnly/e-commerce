using EcShop.Core;
using EcShop.Membership.Context;
using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class UserCellPhoneVerification : MemberTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlInputText txtcode;
		private System.Web.UI.HtmlControls.HtmlInputText txtcellphone;
		private System.Web.UI.WebControls.Button btnSubmit;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserCellPhoneVerification.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtcode = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtcode");
			this.txtcellphone = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtcellphone");
			this.btnSubmit = (System.Web.UI.WebControls.Button)this.FindControl("btnSubmit");
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            string txtcellphone = HiContext.Current.User.Username;
            //Regex reg = new Regex("^[0-9]*$");
            ////判断是否为手机注册 是绑定手机号到txtcellphone
            //if (reg.IsMatch(txtcellphone))
            //{
            //    this.txtcellphone.Disabled = true;
            //    this.txtcellphone.Value = txtcellphone;
            //}
		}
		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			string value = this.txtcellphone.Value;
			if (string.IsNullOrEmpty(value))
			{
				this.ShowMessage("手机号码不允许为空！", false);
				return;
			}
			if (!Regex.IsMatch(value, "^(13|14|15|17|18)\\d{9}$"))
			{
				this.ShowMessage("手机号码格式不正确！", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtcode.Value))
			{
				this.ShowMessage("验证码不允许为空！", false);
				return;
			}
			object obj = HiCache.Get(HiContext.Current.User.UserId + "cellphone");
			if (obj == null)
			{
				this.ShowMessage("验证码错误！", false);
				return;
			}
			if (this.txtcode.Value.ToLower() != obj.ToString().ToLower())
			{
				this.ShowMessage("验证码输入错误！", false);
				return;
			}
			Member member = Users.GetUser(HiContext.Current.User.UserId, true) as Member;
			member.CellPhoneVerification = true;
			member.CellPhone = value;
			if (Users.UpdateUser(member))
			{
				HiCache.Remove(HiContext.Current.User.UserId + "cellphone");
				this.Page.Response.Redirect("VerificationSuccess.aspx");
				return;
			}
			this.ShowMessage("发送验证码失败", false);
		}
	}
}

using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class Register : HtmlTemplatedWebControl
	{
        private System.Web.UI.WebControls.CheckBox chkAgree;
        private System.Web.UI.WebControls.TextBox txtEmail;
		private System.Web.UI.WebControls.TextBox txtPassword;
		private System.Web.UI.WebControls.TextBox txtPassword2;
		private System.Web.UI.WebControls.TextBox txtNumber;
		private string verifyCodeKey = "VerifyCode";

        private System.Web.UI.WebControls.TextBox txtCellPhone;
        private System.Web.UI.WebControls.TextBox regType;
        private System.Web.UI.WebControls.TextBox txtTelphoneVerifyCode;

		private bool CheckVerifyCode(string verifyCode)
		{
			return System.Web.HttpContext.Current.Request.Cookies[this.verifyCodeKey] != null && string.Compare(HiCryptographer.Decrypt(System.Web.HttpContext.Current.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, System.Globalization.CultureInfo.InvariantCulture) == 0;
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Register.html";
			}
			bool flag = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["isCallback"]) && System.Web.HttpContext.Current.Request["isCallback"] == "true";
			if (flag)
			{
				string verifyCode = System.Web.HttpContext.Current.Request["code"];
				string arg;
				if (!this.CheckVerifyCode(verifyCode))
				{
					arg = "0";
				}
				else
				{
					arg = "1";
				}
				System.Web.HttpContext.Current.Response.Clear();
				System.Web.HttpContext.Current.Response.ContentType = "application/json";
				System.Web.HttpContext.Current.Response.Write("{ ");
				System.Web.HttpContext.Current.Response.Write(string.Format("\"flag\":\"{0}\"", arg));
				System.Web.HttpContext.Current.Response.Write("}");
				System.Web.HttpContext.Current.Response.End();
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.chkAgree = (System.Web.UI.WebControls.CheckBox)this.FindControl("chkAgree");
            this.txtEmail = (System.Web.UI.WebControls.TextBox)this.FindControl("txtEmail");
			this.txtPassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtPassword");
			this.txtPassword2 = (System.Web.UI.WebControls.TextBox)this.FindControl("txtPassword2");
			this.txtNumber = (System.Web.UI.WebControls.TextBox)this.FindControl("txtNumber");
            this.txtCellPhone = (System.Web.UI.WebControls.TextBox)this.FindControl("txtCellPhone");
            this.regType = (System.Web.UI.WebControls.TextBox)this.FindControl("regType");
            this.txtTelphoneVerifyCode = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTelphoneVerifyCode");
			PageTitle.AddSiteNameTitle("会员注册");
		}
		private bool ValidationMember(Member member)
		{
			ValidationResults validationResults = Validation.Validate<Member>(member, new string[]
			{
				"ValMember"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMessage(text, false);
			}
			return validationResults.IsValid;
		}
	}
}

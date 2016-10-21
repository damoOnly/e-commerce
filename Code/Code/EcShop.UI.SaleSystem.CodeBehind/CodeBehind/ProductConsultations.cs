using EcShop.Core;
using EcShop.Entities.Comments;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class ProductConsultations : HtmlTemplatedWebControl
	{
		private int productId;
		private System.Web.UI.WebControls.TextBox txtUserName;
		private System.Web.UI.WebControls.TextBox txtEmail;
		private System.Web.UI.WebControls.TextBox txtContent;
		private IButton btnRefer;
		private System.Web.UI.HtmlControls.HtmlInputText txtConsultationCode;
		private ProductDetailsLink prodetailsLink;
		private string verifyCodeKey = "VerifyCode";
		private bool CheckVerifyCode(string verifyCode)
		{
			return System.Web.HttpContext.Current.Request.Cookies[this.verifyCodeKey] != null && string.Compare(HiCryptographer.Decrypt(System.Web.HttpContext.Current.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, System.Globalization.CultureInfo.InvariantCulture) == 0;
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ProductConsultations.html";
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
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound();
			}
			this.txtEmail = (System.Web.UI.WebControls.TextBox)this.FindControl("txtEmail");
			this.txtUserName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtUserName");
			this.txtContent = (System.Web.UI.WebControls.TextBox)this.FindControl("txtContent");
			this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
			this.txtConsultationCode = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtConsultationCode");
			this.prodetailsLink = (ProductDetailsLink)this.FindControl("ProductDetailsLink1");
			this.btnRefer.Click += new System.EventHandler(this.btnRefer_Click);
			if (!this.Page.IsPostBack && HiContext.Current.User.UserRole == UserRole.Member)
			{
				this.txtUserName.Text = HiContext.Current.User.Username;
				this.txtEmail.Text = HiContext.Current.User.Email;
				this.btnRefer.Text = "咨询";
			}
			PageTitle.AddSiteNameTitle("商品咨询");
			ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(this.productId);
			if (productSimpleInfo != null)
			{
				this.prodetailsLink.ProductId = this.productId;
				this.prodetailsLink.ProductName = productSimpleInfo.ProductName;
			}
			this.txtConsultationCode.Value = string.Empty;
		}
		public void btnRefer_Click(object sender, System.EventArgs e)
		{
			ProductConsultationInfo productConsultationInfo = new ProductConsultationInfo();
			productConsultationInfo.ConsultationDate = System.DateTime.Now;
			productConsultationInfo.ProductId = this.productId;
			productConsultationInfo.UserId = HiContext.Current.User.UserId;
			productConsultationInfo.UserName = this.txtUserName.Text;
			productConsultationInfo.UserEmail = this.txtEmail.Text;
			productConsultationInfo.ConsultationText = Globals.HtmlEncode(this.txtContent.Text);
			ValidationResults validationResults = Validation.Validate<ProductConsultationInfo>(productConsultationInfo, new string[]
			{
				"Refer"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMessage(text, false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtConsultationCode.Value))
			{
				this.ShowMessage("请输入验证码", false);
				return;
			}
			if (!HiContext.Current.CheckVerifyCode(this.txtConsultationCode.Value.Trim()))
			{
				this.ShowMessage("验证码不正确", false);
				return;
			}
			if (ProductBrowser.InsertProductConsultation(productConsultationInfo))
			{
				this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"{0}\");window.location.href=\"{1}\"</script>", "咨询成功，管理员回复即可显示", Globals.GetSiteUrls().UrlData.FormatUrl("productConsultations", new object[]
				{
					this.productId
				})));
				return;
			}
			this.ShowMessage("咨询失败，请重试", false);
		}
	}
}

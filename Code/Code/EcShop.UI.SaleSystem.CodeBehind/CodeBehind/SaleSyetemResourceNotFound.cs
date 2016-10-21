using EcShop.Core;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class SaleSyetemResourceNotFound : HtmlTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litMsg;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SaleSyetemResourceNotFound.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litMsg = (System.Web.UI.WebControls.Literal)this.FindControl("litMsg");
			if (this.litMsg != null)
			{
				this.litMsg.Text = Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["errorMsg"]));
			}
		}
	}
}

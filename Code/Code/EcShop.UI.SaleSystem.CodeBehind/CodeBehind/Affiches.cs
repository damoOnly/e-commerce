using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class Affiches : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptAffiches;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Affiches.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.rptAffiches = (ThemedTemplatedRepeater)this.FindControl("rptAffiches");
			if (!this.Page.IsPostBack)
			{
				this.rptAffiches.DataSource = CommentBrowser.GetAfficheList();
				this.rptAffiches.DataBind();
			}
		}
	}
}

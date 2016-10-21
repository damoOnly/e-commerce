using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Data;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class Category : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptCategories;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Category.html";
			}
		}
		protected override void AttachChildControls()
		{
			this.rptCategories = (ThemedTemplatedRepeater)this.FindControl("rptCategories");
			if (!this.Page.IsPostBack)
			{
				DataSet threeLayerCategories = CategoryBrowser.GetThreeLayerCategories();
				this.rptCategories.DataSource = threeLayerCategories.Tables[0].DefaultView;
				this.rptCategories.DataBind();
			}
		}
	}
}

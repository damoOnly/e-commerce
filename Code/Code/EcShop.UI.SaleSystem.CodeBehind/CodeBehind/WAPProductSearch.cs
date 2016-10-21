using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPProductSearch : WAPTemplatedWebControl
	{
		private int categoryId;
		private string keyWord;
		private HiImage imgUrl;
		private System.Web.UI.WebControls.Literal litContent;
		private WapTemplatedRepeater rptProducts;
		private WapTemplatedRepeater rptCategories;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vProductSearch.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			this.keyWord = this.Page.Request.QueryString["keyWord"];
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
			this.rptProducts = (WapTemplatedRepeater)this.FindControl("rptProducts");
			this.rptCategories = (WapTemplatedRepeater)this.FindControl("rptCategories");
			System.Collections.Generic.IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, 1000);
			this.rptCategories.DataSource = maxSubCategories;
			this.rptCategories.DataBind();
			PageTitle.AddSiteNameTitle("分类搜索页");
		}
	}
}

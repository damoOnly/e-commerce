using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VProductSearch : VshopTemplatedWebControl
	{
		private int categoryId;
		private string keyWord;
		private HiImage imgUrl;
		private System.Web.UI.WebControls.Literal litContent;
		private VshopTemplatedRepeater rptProducts;
		private VshopTemplatedRepeater rptCategories;
        private VshopTemplatedRepeater rpthistorysearch;

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
			this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
			this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
            this.rpthistorysearch = (VshopTemplatedRepeater)this.FindControl("rpthistorysearch");

			System.Collections.Generic.IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId,0, 1000);
			this.rptCategories.DataSource = maxSubCategories;
			this.rptCategories.DataBind();

            int userId = HiContext.Current.User.UserId;
            if (userId > 0 && this.rpthistorysearch != null)
            {
                this.rpthistorysearch.DataSource = HistorySearchHelp.GetSearchHistory(userId, ClientType.VShop, 6);
                this.rpthistorysearch.DataBind();
            }

			PageTitle.AddSiteNameTitle("分类搜索页");
		}
	}
}

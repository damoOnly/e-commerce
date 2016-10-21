using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPShareListProducts : WAPTemplatedWebControl
	{
		private WapTemplatedRepeater rptProducts;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;
		private int shareId;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vShareListProducts.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			ProductBrowseQuery productBrowseQuery = this.GetProductBrowseQuery();
			this.rptProducts = (WapTemplatedRepeater)this.FindControl("rptProducts");
			this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			DbQueryResult shareProducts = ProductBrowser.GetShareProducts(this.shareId, productBrowseQuery);
			if (shareProducts.TotalRecords > 0)
			{
				this.rptProducts.DataSource = shareProducts.Data;
				this.rptProducts.DataBind();
			}
			this.txtTotal.SetWhenIsNotNull(shareProducts.TotalRecords.ToString());
		}
		protected ProductBrowseQuery GetProductBrowseQuery()
		{
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			int pageIndex;
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
			{
				pageIndex = 1;
			}
			int pageSize;
			if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
			{
				pageSize = 20;
			}
			int.TryParse(this.Page.Request.QueryString["Id"], out this.shareId);
			productBrowseQuery.PageIndex = pageIndex;
			productBrowseQuery.PageSize = pageSize;
			productBrowseQuery.SortBy = "DisplaySequence";
			productBrowseQuery.SortOrder = SortAction.Desc;
			Globals.EntityCoding(productBrowseQuery, true);
			return productBrowseQuery;
		}
	}
}

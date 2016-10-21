using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Comments;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPProductReview : WAPMemberTemplatedWebControl
	{
		private int productId;
		private WapTemplatedRepeater rptProducts;
		private System.Web.UI.WebControls.Literal litProdcutName;
		private System.Web.UI.WebControls.Literal litSalePrice;
		private System.Web.UI.WebControls.Literal litSoldCount;
		private System.Web.UI.WebControls.Literal litShortDescription;
		private System.Web.UI.HtmlControls.HtmlImage productImage;
		private System.Web.UI.WebControls.HyperLink productLink;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProductReview.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound("错误的商品信息");
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			this.litProdcutName = (System.Web.UI.WebControls.Literal)this.FindControl("litProdcutName");
			this.litSalePrice = (System.Web.UI.WebControls.Literal)this.FindControl("litSalePrice");
			this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
			this.litSoldCount = (System.Web.UI.WebControls.Literal)this.FindControl("litSoldCount");
			this.productImage = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("productImage");
			this.productLink = (System.Web.UI.WebControls.HyperLink)this.FindControl("productLink");
			this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(this.productId);
			if (productSimpleInfo == null)
			{
				base.GotoResourceNotFound("该件商品已经被管理员删除");
			}
			this.litProdcutName.SetWhenIsNotNull(productSimpleInfo.ProductName);
			this.litSalePrice.SetWhenIsNotNull(productSimpleInfo.MinSalePrice.ToString("F2"));
			this.litShortDescription.SetWhenIsNotNull(productSimpleInfo.ShortDescription);
			this.litSoldCount.SetWhenIsNotNull(productSimpleInfo.ShowSaleCounts.ToString());
			if (!string.IsNullOrEmpty(productSimpleInfo.ThumbnailUrl180))
			{
				this.productImage.Src = productSimpleInfo.ThumbnailUrl180;
			}
			else
			{
				this.productImage.Src = masterSettings.DefaultProductThumbnail5;
			}
			this.productLink.NavigateUrl = "ProductDetails.aspx?ProductId=" + productSimpleInfo.ProductId;
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
			ProductReviewQuery productReviewQuery = new ProductReviewQuery();
			productReviewQuery.productId = this.productId;
			productReviewQuery.IsCount = true;
			productReviewQuery.PageIndex = pageIndex;
			productReviewQuery.PageSize = pageSize;
			productReviewQuery.SortBy = "ReviewId";
			productReviewQuery.SortOrder = SortAction.Desc;
			this.rptProducts = (WapTemplatedRepeater)this.FindControl("rptProducts");
			DbQueryResult productReviews = ProductBrowser.GetProductReviews(productReviewQuery);
			this.rptProducts.DataSource = productReviews.Data;
			this.rptProducts.DataBind();
			this.txtTotal.SetWhenIsNotNull(productReviews.TotalRecords.ToString());
			PageTitle.AddSiteNameTitle("商品评价");
		}
	}
}

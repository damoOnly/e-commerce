using EcShop.Core;
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
	public class VProductConsultations : VMemberTemplatedWebControl
	{
		private int productId;
		private VshopTemplatedRepeater rptProducts;
		private System.Web.UI.WebControls.Literal litDescription;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;
		private System.Web.UI.WebControls.Literal litProductTitle;
		private System.Web.UI.WebControls.Literal litShortDescription;
		private System.Web.UI.WebControls.Literal litSalePrice;
		private System.Web.UI.WebControls.Literal litSoldCount;
		private System.Web.UI.HtmlControls.HtmlImage imgProductImage;
		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(true);
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProductConsultations.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound("");
			}
			ProductConsultationAndReplyQuery productConsultationAndReplyQuery = new ProductConsultationAndReplyQuery();
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
			productConsultationAndReplyQuery.ProductId = this.productId;
			productConsultationAndReplyQuery.IsCount = true;
			productConsultationAndReplyQuery.PageIndex = pageIndex;
			productConsultationAndReplyQuery.PageSize = pageSize;
			productConsultationAndReplyQuery.SortBy = "ConsultationId";
			productConsultationAndReplyQuery.SortOrder = SortAction.Desc;
			productConsultationAndReplyQuery.HasReplied = new bool?(true);
			this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
			this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			DbQueryResult productConsultations = ProductBrowser.GetProductConsultations(productConsultationAndReplyQuery);
			this.rptProducts.DataSource = productConsultations.Data;
			this.rptProducts.DataBind();
			this.txtTotal.SetWhenIsNotNull(productConsultations.TotalRecords.ToString());
			this.litProductTitle = (System.Web.UI.WebControls.Literal)this.FindControl("litProductTitle");
			this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
			this.litSoldCount = (System.Web.UI.WebControls.Literal)this.FindControl("litSoldCount");
			this.litSalePrice = (System.Web.UI.WebControls.Literal)this.FindControl("litSalePrice");
			this.imgProductImage = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("imgProductImage");
			ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(this.productId);
			this.litProductTitle.SetWhenIsNotNull(productSimpleInfo.ProductName);
			this.litShortDescription.SetWhenIsNotNull(productSimpleInfo.ShortDescription);
			this.litSoldCount.SetWhenIsNotNull(productSimpleInfo.ShowSaleCounts.ToString());
			this.litSalePrice.SetWhenIsNotNull(productSimpleInfo.MinSalePrice.ToString("F2"));
			this.imgProductImage.Src = (string.IsNullOrEmpty(productSimpleInfo.ThumbnailUrl60) ? Globals.FullPath(this.siteSettings.DefaultProductThumbnail4) : productSimpleInfo.ThumbnailUrl60);
			PageTitle.AddSiteNameTitle("商品咨询");
		}
	}
}

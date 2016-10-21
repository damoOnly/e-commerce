using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPBrandDetail : WAPTemplatedWebControl
	{
		private int BrandId;
		private HiImage imgUrl;
		private System.Web.UI.WebControls.Literal litBrandDetail;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VBrandDetail.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["BrandId"], out this.BrandId))
			{
				base.GotoResourceNotFound("");
			}
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			this.litBrandDetail = (System.Web.UI.WebControls.Literal)this.FindControl("litBrandDetail");
			BrandCategoryInfo brandCategory = CategoryBrowser.GetBrandCategory(this.BrandId);
			this.litBrandDetail.SetWhenIsNotNull(brandCategory.Description);
			this.imgUrl.ImageUrl = brandCategory.Logo;
			PageTitle.AddSiteNameTitle("品牌详情");
		}
	}
}

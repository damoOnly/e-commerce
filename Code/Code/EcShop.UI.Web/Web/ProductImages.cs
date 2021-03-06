using ASPNET.WebControls;
using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web
{
	public class ProductImages : System.Web.UI.Page
	{
		protected PageTitle PageTitle1;
		protected Script Script1;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected System.Web.UI.WebControls.HyperLink productName;
		protected SiteUrl SiteUrl1;
		protected System.Web.UI.HtmlControls.HtmlInputText image1url;
		protected System.Web.UI.HtmlControls.HtmlInputText image2url;
		protected System.Web.UI.HtmlControls.HtmlInputText image3url;
		protected System.Web.UI.HtmlControls.HtmlInputText image4url;
		protected System.Web.UI.HtmlControls.HtmlInputText image5url;
		protected HiImage image1;
		protected HiImage image2;
		protected HiImage image3;
		protected HiImage image4;
		protected HiImage image5;
		protected System.Web.UI.HtmlControls.HtmlImage imgBig;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			int productId = 0;
			int.TryParse(this.Page.Request.QueryString["ProductId"], out productId);
			if (!this.Page.IsPostBack)
			{
				ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(productId);
				if (productSimpleInfo != null)
				{
					this.BindImages(productSimpleInfo);
					if (!string.IsNullOrEmpty(productSimpleInfo.Title))
					{
						PageTitle.AddSiteNameTitle(productSimpleInfo.Title + " 相册");
						return;
					}
					PageTitle.AddSiteNameTitle(productSimpleInfo.ProductName + " 相册");
				}
			}
		}
		private void BindImages(ProductInfo prductImageInfo)
		{
			this.productName.Text = prductImageInfo.ProductName;
			this.productName.NavigateUrl = Utils.ApplicationPath + "/ProductDetails.aspx?ProductId=" + prductImageInfo.ProductId;
			this.image1url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl1;
			this.imgBig.Src = ((this.image1url.Value == "") ? "" : (Utils.ApplicationPath + prductImageInfo.ImageUrl1));
			this.image2url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl2;
			this.image3url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl3;
			this.image4url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl4;
			this.image5url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl5;
			if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl1))
			{
				this.image1.ImageUrl = prductImageInfo.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
			}
			if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl2))
			{
				this.image2.ImageUrl = prductImageInfo.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
			}
			if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl3))
			{
				this.image3.ImageUrl = prductImageInfo.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
			}
			if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl4))
			{
				this.image4.ImageUrl = prductImageInfo.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
			}
			if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl5))
			{
				this.image5.ImageUrl = prductImageInfo.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
			}
			if (prductImageInfo.ImageUrl1 == null && prductImageInfo.ImageUrl2 == null && prductImageInfo.ImageUrl3 == null && prductImageInfo.ImageUrl4 == null && prductImageInfo.ImageUrl5 == null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				this.imgBig.Src = Globals.ApplicationPath + masterSettings.DefaultProductImage;
				this.imgBig.Align = "center";
			}
		}
	}
}

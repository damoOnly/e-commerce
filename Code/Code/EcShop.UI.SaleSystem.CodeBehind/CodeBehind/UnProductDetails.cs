using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class UnProductDetails : HtmlTemplatedWebControl
	{
		private int productId;
		private Common_Location common_Location;
		private System.Web.UI.WebControls.Literal litProductName;
		private System.Web.UI.WebControls.Literal lblProductCode;
		private SkuLabel lblSku;
		private StockLabel lblStock;
		private System.Web.UI.WebControls.Label litWeight;
		private System.Web.UI.WebControls.Literal litUnit;
        private System.Web.UI.WebControls.Literal litIsDown;
		private System.Web.UI.WebControls.Literal litBrosedNum;
		private System.Web.UI.WebControls.Literal litBrand;
		private FormatedMoneyLabel lblMarkerPrice;
		private System.Web.UI.WebControls.Label lblBuyPrice;
		private TotalLabel lblTotalPrice;
		private System.Web.UI.WebControls.Literal litDescription;
		private System.Web.UI.WebControls.Literal litShortDescription;
		private System.Web.UI.WebControls.HyperLink hpkProductConsultations;
		private System.Web.UI.WebControls.HyperLink hpkProductReviews;
		private Common_ProductImages images;
		private ThemedTemplatedRepeater rptExpandAttributes;
		private SKUSelector skuSelector;
		private Common_ProductReview reviews;
		private Common_ProductConsultations consultations;
		private Common_GoodsList_Correlative correlative;
		private System.Web.UI.HtmlControls.HtmlInputHidden hidden_skus;
		private System.Web.UI.HtmlControls.HtmlInputHidden hidden_skuItem;
		private System.Web.UI.HtmlControls.HtmlInputHidden hidCartQuantity;
        private System.Web.UI.WebControls.Literal litTaxRate;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-UnProductDetails.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound();
			}
			this.common_Location = (Common_Location)this.FindControl("common_Location");
			this.litProductName = (System.Web.UI.WebControls.Literal)this.FindControl("litProductName");
			this.lblProductCode = (System.Web.UI.WebControls.Literal)this.FindControl("lblProductCode");
			this.lblSku = (SkuLabel)this.FindControl("lblSku");
			this.lblStock = (StockLabel)this.FindControl("lblStock");
			this.litUnit = (System.Web.UI.WebControls.Literal)this.FindControl("litUnit");
			this.litWeight = (System.Web.UI.WebControls.Label)this.FindControl("litWeight");
			this.litBrosedNum = (System.Web.UI.WebControls.Literal)this.FindControl("litBrosedNum");
			this.litBrand = (System.Web.UI.WebControls.Literal)this.FindControl("litBrand");
			this.lblMarkerPrice = (FormatedMoneyLabel)this.FindControl("lblMarkerPrice");
			this.lblBuyPrice = (System.Web.UI.WebControls.Label)this.FindControl("lblBuyPrice");
			this.lblTotalPrice = (TotalLabel)this.FindControl("lblTotalPrice");
			this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
			this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
			this.hpkProductConsultations = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpkProductConsultations");
			this.hpkProductReviews = (System.Web.UI.WebControls.HyperLink)this.FindControl("hpkProductReviews");
			this.images = (Common_ProductImages)this.FindControl("common_ProductImages");
			this.rptExpandAttributes = (ThemedTemplatedRepeater)this.FindControl("rptExpandAttributes");
			this.skuSelector = (SKUSelector)this.FindControl("SKUSelector");
			this.reviews = (Common_ProductReview)this.FindControl("list_Common_ProductReview");
			this.consultations = (Common_ProductConsultations)this.FindControl("list_Common_ProductConsultations");
			this.correlative = (Common_GoodsList_Correlative)this.FindControl("list_Common_GoodsList_Correlative");
			this.hidden_skus = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidden_skus");
			this.hidden_skuItem = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidden_skuItem");
			this.hidCartQuantity = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txCartQuantity");
            this.litIsDown = (System.Web.UI.WebControls.Literal)this.FindControl("litIsDown");
            this.litTaxRate = (System.Web.UI.WebControls.Literal)this.FindControl("litTaxRate");
			if (!this.Page.IsPostBack)
			{
				int value = 0;
				if (this.reviews != null)
				{
					value = this.reviews.MaxNum;
				}
				int value2 = 0;
				if (this.consultations != null)
				{
					value2 = this.consultations.MaxNum;
				}
				ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(this.productId, new int?(value), new int?(value2));
				if (productBrowseInfo.Product == null || productBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件商品已经被管理员删除"));
					return;
				}
				if (productBrowseInfo.Product.SaleStatus == ProductSaleStatus.OnSale)
				{
					Globals.GetSiteUrls().UrlData.FormatUrl("productdetails", new object[]
					{
						this.Page.Request.QueryString["productId"]
					});
					return;
				}
				if (this.hidCartQuantity != null)
				{
					this.hidCartQuantity.Value = "0";
				}
				System.Collections.IEnumerable value3 = 
					from item in productBrowseInfo.Product.Skus
					select item.Value;
				if (JsonConvert.SerializeObject(productBrowseInfo.DbSKUs) != null)
				{
					this.hidden_skuItem.Value = JsonConvert.SerializeObject(productBrowseInfo.DbSKUs);
				}
				if (this.hidden_skus != null)
				{
					this.hidden_skus.Value = JsonConvert.SerializeObject(value3);
				}
				this.LoadPageSearch(productBrowseInfo.Product);
				if (this.hpkProductReviews != null)
				{
					this.hpkProductReviews.Text = "查看全部" + productBrowseInfo.ReviewCount.ToString() + "条评论";
					this.hpkProductReviews.NavigateUrl = string.Format("LookProductReviews.aspx?productId={0}", this.productId);
				}
				if (this.hpkProductConsultations != null)
				{
					this.hpkProductConsultations.Text = "查看全部" + productBrowseInfo.ConsultationCount.ToString() + "条咨询";
					this.hpkProductConsultations.NavigateUrl = string.Format("ProductConsultationsAndReplay.aspx?productId={0}", this.productId);
				}
				this.LoadProductInfo(productBrowseInfo.Product, productBrowseInfo.BrandName);
				BrowsedProductQueue.EnQueue(this.productId);
				this.images.ImageInfo = productBrowseInfo.Product;
				if (productBrowseInfo.DbAttribute != null)
				{
					this.rptExpandAttributes.DataSource = productBrowseInfo.DbAttribute;
					this.rptExpandAttributes.DataBind();
				}
				if (productBrowseInfo.DbSKUs != null)
				{
					this.skuSelector.ProductId = this.productId;
					this.skuSelector.DataSource = productBrowseInfo.DbSKUs;
				}
				if (this.reviews != null && productBrowseInfo.DBReviews != null)
				{
					this.reviews.DataSource = productBrowseInfo.DBReviews;
					this.reviews.DataBind();
				}
				if (this.consultations != null && productBrowseInfo.DBConsultations != null)
				{
					this.consultations.DataSource = productBrowseInfo.DBConsultations;
					this.consultations.DataBind();
				}
				if (this.correlative != null && productBrowseInfo.DbCorrelatives != null)
				{
					this.correlative.DataSource = productBrowseInfo.DbCorrelatives;
					this.correlative.DataBind();
				}
                if (productBrowseInfo.Product.SaleStatus == ProductSaleStatus.UnSale)
                {
                    if (this.litIsDown != null)
                    {
                        this.litIsDown.Text = "该商品已经下架";
                    }
                }
			}
		}
		private void LoadPageSearch(ProductInfo productDetails)
		{
			if (!string.IsNullOrEmpty(productDetails.MetaKeywords))
			{
				MetaTags.AddMetaKeywords(productDetails.MetaKeywords, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(productDetails.MetaDescription))
			{
				MetaTags.AddMetaDescription(productDetails.MetaDescription, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(productDetails.Title))
			{
				PageTitle.AddSiteNameTitle(productDetails.Title);
				return;
			}
			PageTitle.AddSiteNameTitle(productDetails.ProductName);
		}
		private void LoadProductInfo(ProductInfo productDetails, string brandName)
		{
			if (this.common_Location != null && !string.IsNullOrEmpty(productDetails.MainCategoryPath))
			{
				this.common_Location.CateGoryPath = productDetails.MainCategoryPath.Remove(productDetails.MainCategoryPath.Length - 1);
				this.common_Location.ProductName = productDetails.ProductName;
			}
			this.litProductName.Text = productDetails.ProductName;
			this.lblProductCode.Text = productDetails.ProductCode;
			this.lblSku.Text = productDetails.SKU;
			this.lblSku.Value = productDetails.SkuId;
			this.lblStock.Stock = 0;
			this.litUnit.Text = productDetails.Unit;
			if (productDetails.Weight > 0m)
			{
				this.litWeight.Text = string.Format("{0} g", productDetails.Weight);
			}
			else
			{
				this.litWeight.Text = "无";
			}
			this.litBrosedNum.Text = productDetails.VistiCounts.ToString();
			this.litBrand.Text = brandName;
			if (productDetails.MinSalePrice == productDetails.MaxSalePrice)
			{
				this.lblBuyPrice.Text = productDetails.MinSalePrice.ToString("F2");
			}
			else
			{
				this.lblBuyPrice.Text = productDetails.MinSalePrice.ToString("F2") + " - " + productDetails.MaxSalePrice.ToString("F2");
			}
			this.lblMarkerPrice.Money = productDetails.MarketPrice;
			this.litDescription.Text = productDetails.Description;
			if (this.litShortDescription != null)
			{
				this.litShortDescription.Text = productDetails.ShortDescription;
			}

            if (this.litTaxRate != null)
            {
                this.litTaxRate.Text = (productDetails.TaxRate * 100).ToString("0");
            }
            if (this.lblTotalPrice != null)
            {
                this.lblTotalPrice.Value = 0m;
            }
		}
	}
}

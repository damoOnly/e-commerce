using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AliOHProductDetails : AliOHTemplatedWebControl
	{
		private int productId;
		private AliOHTemplatedRepeater rptProductImages;
		private System.Web.UI.WebControls.Literal litProdcutName;
		private System.Web.UI.WebControls.Literal litSalePrice;
		private System.Web.UI.WebControls.Literal litSoldCount;
		private System.Web.UI.WebControls.Literal litMarketPrice;
		private System.Web.UI.WebControls.Literal litShortDescription;
		private System.Web.UI.WebControls.Literal litDescription;
		private System.Web.UI.WebControls.Literal litStock;
		private System.Web.UI.WebControls.Literal litConsultationsCount;
		private System.Web.UI.WebControls.Literal litReviewsCount;
		private Common_SKUSelector skuSelector;
		private Common_ExpandAttributes expandAttr;
		private System.Web.UI.WebControls.HyperLink linkDescription;
		private System.Web.UI.HtmlControls.HtmlInputHidden litHasCollected;
		private System.Web.UI.HtmlControls.HtmlInputHidden hidden_skus;
		private UserProductReferLabel lbUserProductRefer;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProductDetails.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound("");
			}
			if (HiContext.Current.User.UserRole == UserRole.Member && ((Member)HiContext.Current.User).ReferralStatus == 2 && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
			{
				string text = System.Web.HttpContext.Current.Request.Url.ToString();
				if (text.IndexOf("?") > -1)
				{
					text = text + "&ReferralUserId=" + HiContext.Current.User.UserId;
				}
				else
				{
					text = text + "?ReferralUserId=" + HiContext.Current.User.UserId;
				}
				this.Page.Response.Redirect(text);
				return;
			}
			this.rptProductImages = (AliOHTemplatedRepeater)this.FindControl("rptProductImages");
			this.litProdcutName = (System.Web.UI.WebControls.Literal)this.FindControl("litProdcutName");
			this.litSalePrice = (System.Web.UI.WebControls.Literal)this.FindControl("litSalePrice");
			this.litMarketPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litMarketPrice");
			this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
			this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
			this.litStock = (System.Web.UI.WebControls.Literal)this.FindControl("litStock");
			this.skuSelector = (Common_SKUSelector)this.FindControl("skuSelector");
			this.linkDescription = (System.Web.UI.WebControls.HyperLink)this.FindControl("linkDescription");
			this.expandAttr = (Common_ExpandAttributes)this.FindControl("ExpandAttributes");
			this.litSoldCount = (System.Web.UI.WebControls.Literal)this.FindControl("litSoldCount");
			this.litConsultationsCount = (System.Web.UI.WebControls.Literal)this.FindControl("litConsultationsCount");
			this.litReviewsCount = (System.Web.UI.WebControls.Literal)this.FindControl("litReviewsCount");
			this.litHasCollected = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litHasCollected");
			this.hidden_skus = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidden_skus");
			this.lbUserProductRefer = (UserProductReferLabel)this.FindControl("lbUserProductRefer");
			ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(this.productId, null, null);
			System.Collections.IEnumerable value = 
				from item in productBrowseInfo.Product.Skus
				select item.Value;
			this.hidden_skus.Value = JsonConvert.SerializeObject(value);
			if (productBrowseInfo == null)
			{
				base.GotoResourceNotFound("此商品已不存在");
			}
			if (productBrowseInfo.Product.SaleStatus != ProductSaleStatus.OnSale)
			{
				base.GotoResourceNotFound("此商品已下架");
			}
			if (this.lbUserProductRefer != null)
			{
				this.lbUserProductRefer.product = productBrowseInfo.Product;
			}
			if (this.rptProductImages != null)
			{
				string locationUrl = "javascript:;";
				SlideImage[] source = new SlideImage[]
				{
					new SlideImage(productBrowseInfo.Product.ImageUrl1, locationUrl),
					new SlideImage(productBrowseInfo.Product.ImageUrl2, locationUrl),
					new SlideImage(productBrowseInfo.Product.ImageUrl3, locationUrl),
					new SlideImage(productBrowseInfo.Product.ImageUrl4, locationUrl),
					new SlideImage(productBrowseInfo.Product.ImageUrl5, locationUrl)
				};
				this.rptProductImages.DataSource = 
					from item in source
					where !string.IsNullOrWhiteSpace(item.ImageUrl)
					select item;
				this.rptProductImages.DataBind();
			}
			this.litProdcutName.Text = productBrowseInfo.Product.ProductName;
			this.litSalePrice.Text = productBrowseInfo.Product.MinSalePrice.ToString("F2");
			if (productBrowseInfo.Product.MarketPrice.HasValue)
			{
				this.litMarketPrice.SetWhenIsNotNull(productBrowseInfo.Product.MarketPrice.GetValueOrDefault(0m).ToString("F2"));
			}
			this.litShortDescription.Text = productBrowseInfo.Product.ShortDescription;
			if (this.litDescription != null)
			{
				System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("<script[^>]*?>.*?</script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				if (!string.IsNullOrWhiteSpace(productBrowseInfo.Product.MobblieDescription))
				{
					this.litDescription.Text = regex.Replace(productBrowseInfo.Product.MobblieDescription, "");
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(productBrowseInfo.Product.Description))
					{
						this.litDescription.Text = regex.Replace(productBrowseInfo.Product.Description, "");
					}
				}
			}
			this.litSoldCount.SetWhenIsNotNull(productBrowseInfo.Product.ShowSaleCounts.ToString());
			this.litStock.Text = productBrowseInfo.Product.Stock.ToString();
			this.skuSelector.ProductId = this.productId;
			if (this.expandAttr != null)
			{
				this.expandAttr.ProductId = this.productId;
			}
			if (this.linkDescription != null)
			{
				this.linkDescription.NavigateUrl = "/AliOH/ProductDescription.aspx?productId=" + this.productId;
			}
			this.litConsultationsCount.SetWhenIsNotNull(productBrowseInfo.ConsultationCount.ToString());
			this.litReviewsCount.SetWhenIsNotNull(productBrowseInfo.ReviewCount.ToString());
			Member member = HiContext.Current.User as Member;
			bool flag = false;
			if (member != null)
			{
				flag = ProductBrowser.CheckHasCollect(member.UserId, this.productId);
			}
			this.litHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
			PageTitle.AddSiteNameTitle(productBrowseInfo.Product.ProductName);
		}
	}
}

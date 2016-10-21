using EcShop.Entities.Commodities;
using EcShop.Entities.Promotions;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class WAPCountDownProductDetail : WAPTemplatedWebControl
	{
		private int countDownId;
		private WapTemplatedRepeater rptProductImages;
		private System.Web.UI.WebControls.Literal litProdcutName;
		private System.Web.UI.WebControls.Literal litShortDescription;
		private System.Web.UI.WebControls.Literal litDescription;
		private System.Web.UI.WebControls.Literal litSoldCount;
		private System.Web.UI.WebControls.Literal litminCount;
		private System.Web.UI.WebControls.Literal litprice;
		private System.Web.UI.WebControls.Literal litcontent;
		private System.Web.UI.WebControls.Literal litLeftSeconds;
		private System.Web.UI.HtmlControls.HtmlInputControl litGroupBuyId;
		private Common_SKUSelector skuSelector;
		private Common_ExpandAttributes expandAttr;
		private System.Web.UI.WebControls.HyperLink linkDescription;
		private System.Web.UI.WebControls.Literal salePrice;
		private System.Web.UI.WebControls.Literal leftCount;
		private System.Web.UI.WebControls.Literal minSuccessCount;
		private System.Web.UI.HtmlControls.HtmlInputControl txtProductId;
		private System.Web.UI.WebControls.Literal litConsultationsCount;
		private System.Web.UI.WebControls.Literal litGroupbuyDescription;
		private System.Web.UI.WebControls.Literal litReviewsCount;
		private System.Web.UI.WebControls.Literal litMaxCount;
		private System.Web.UI.HtmlControls.HtmlInputHidden startTime;
		private System.Web.UI.HtmlControls.HtmlInputHidden endTime;
		private System.Web.UI.HtmlControls.HtmlInputHidden groupBuySoldCount;
		private System.Web.UI.HtmlControls.HtmlInputHidden groupBuyMinCount;
		private System.Web.UI.HtmlControls.HtmlInputHidden nowTime;
		private System.Web.UI.HtmlControls.HtmlInputHidden groupBuyMaxCount;
		protected override void OnInit(System.EventArgs e)
		{
			this.SkinName = ((this.SkinName == null) ? "Skin-VCountDownProductDetail.html" : this.SkinName);
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (!this.CheckGroupbuyIdExist())
			{
				base.GotoResourceNotFound("");
			}
			this.FindControls();
			this.SetControlsValue(this.countDownId);
			PageTitle.AddSiteNameTitle("限时抢购商品详情");
		}
		private bool CheckGroupbuyIdExist()
		{
			return int.TryParse(this.Page.Request.QueryString["CountDownId"], out this.countDownId);
		}
		private void FindControls()
		{
			this.rptProductImages = (WapTemplatedRepeater)this.FindControl("rptProductImages");
			this.litProdcutName = (System.Web.UI.WebControls.Literal)this.FindControl("litProdcutName");
			this.litShortDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litShortDescription");
			this.litDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litDescription");
			this.litSoldCount = (System.Web.UI.WebControls.Literal)this.FindControl("soldCount");
			this.litprice = (System.Web.UI.WebControls.Literal)this.FindControl("price");
			this.litcontent = (System.Web.UI.WebControls.Literal)this.FindControl("content");
			this.litminCount = (System.Web.UI.WebControls.Literal)this.FindControl("minCount");
			this.litGroupBuyId = (System.Web.UI.HtmlControls.HtmlInputControl)this.FindControl("litGroupbuyId");
			this.litLeftSeconds = (System.Web.UI.WebControls.Literal)this.FindControl("leftSeconds");
			this.skuSelector = (Common_SKUSelector)this.FindControl("skuSelector");
			this.linkDescription = (System.Web.UI.WebControls.HyperLink)this.FindControl("linkDescription");
			this.expandAttr = (Common_ExpandAttributes)this.FindControl("ExpandAttributes");
			this.salePrice = (System.Web.UI.WebControls.Literal)this.FindControl("salePrice");
			this.leftCount = (System.Web.UI.WebControls.Literal)this.FindControl("leftCount");
			this.minSuccessCount = (System.Web.UI.WebControls.Literal)this.FindControl("minSuccessCount");
			this.txtProductId = (System.Web.UI.HtmlControls.HtmlInputControl)this.FindControl("txtProductId");
			this.litConsultationsCount = (System.Web.UI.WebControls.Literal)this.FindControl("litConsultationsCount");
			this.litReviewsCount = (System.Web.UI.WebControls.Literal)this.FindControl("litReviewsCount");
			this.litMaxCount = (System.Web.UI.WebControls.Literal)this.FindControl("litMaxCount");
			this.startTime = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("startTime");
			this.endTime = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("endTime");
			this.groupBuySoldCount = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("groupBuySoldCount");
			this.groupBuyMinCount = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("groupBuyMinCount");
			this.litGroupbuyDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litGroupbuyDescription");
			this.groupBuyMaxCount = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("groupBuyMaxCount");
		}
		private void SetControlsValue(int countDownId)
		{
			CountDownInfo countDownInfoByCountDownId = ProductBrowser.GetCountDownInfoByCountDownId(countDownId);
			ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(countDownInfoByCountDownId.ProductId, null, null);
			if (productBrowseInfo == null)
			{
				base.GotoResourceNotFound("此商品已不存在");
			}
			if (productBrowseInfo.Product.SaleStatus != ProductSaleStatus.OnSale)
			{
				base.GotoResourceNotFound("此商品已下架");
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
			this.litProdcutName.SetWhenIsNotNull(productBrowseInfo.Product.ProductName);
			this.litminCount.SetWhenIsNotNull(countDownInfoByCountDownId.MaxCount.ToString());
			this.litShortDescription.SetWhenIsNotNull(productBrowseInfo.Product.ShortDescription);
			if (this.litDescription != null && !string.IsNullOrWhiteSpace(productBrowseInfo.Product.Description))
			{
				System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("<script[^>]*?>.*?</script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				if (!string.IsNullOrEmpty(productBrowseInfo.Product.MobblieDescription))
				{
					this.litDescription.Text = regex.Replace(productBrowseInfo.Product.MobblieDescription, "");
				}
				else
				{
					this.litDescription.Text = regex.Replace(productBrowseInfo.Product.Description, "");
				}
			}
			this.litprice.SetWhenIsNotNull(countDownInfoByCountDownId.CountDownPrice.ToString("F2"));
			this.litLeftSeconds.SetWhenIsNotNull(System.Math.Ceiling((countDownInfoByCountDownId.EndDate - System.DateTime.Now).TotalSeconds).ToString());
			this.litcontent.SetWhenIsNotNull(countDownInfoByCountDownId.Content);
			this.litGroupBuyId.SetWhenIsNotNull(countDownInfoByCountDownId.CountDownId.ToString());
			this.skuSelector.ProductId = countDownInfoByCountDownId.ProductId;
			this.expandAttr.ProductId = countDownInfoByCountDownId.ProductId;
			this.salePrice.SetWhenIsNotNull(productBrowseInfo.Product.MaxSalePrice.ToString("F2"));
			this.linkDescription.SetWhenIsNotNull("/Vshop/ProductDescription.aspx?productId=" + countDownInfoByCountDownId.ProductId);
			this.txtProductId.SetWhenIsNotNull(countDownInfoByCountDownId.ProductId.ToString());
			this.litConsultationsCount.SetWhenIsNotNull(productBrowseInfo.ConsultationCount.ToString());
			this.litReviewsCount.SetWhenIsNotNull(productBrowseInfo.ReviewCount.ToString());
			this.litGroupbuyDescription.SetWhenIsNotNull(countDownInfoByCountDownId.Content);
			this.litMaxCount.SetWhenIsNotNull(countDownInfoByCountDownId.MaxCount.ToString());
			this.nowTime = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("nowTime");
			this.nowTime.SetWhenIsNotNull(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
			this.startTime.SetWhenIsNotNull(countDownInfoByCountDownId.StartDate.ToString("yyyy/MM/dd HH:mm:ss"));
			this.endTime.SetWhenIsNotNull(countDownInfoByCountDownId.EndDate.ToString("yyyy/MM/dd HH:mm:ss"));
			this.groupBuyMaxCount.SetWhenIsNotNull(countDownInfoByCountDownId.MaxCount.ToString());
		}
	}
}

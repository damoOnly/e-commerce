using EcShop.Core;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.CodeBehind.Common;
using EcShop.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AliOHSubmmitOrder : AliOHMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litShipTo;
		private System.Web.UI.WebControls.Literal litCellPhone;
		private System.Web.UI.WebControls.Literal litAddress;
		private System.Web.UI.HtmlControls.HtmlInputControl groupbuyHiddenBox;
		private AliOHTemplatedRepeater rptCartProducts;
		private AliOHTemplatedRepeater rptPromotions;
		private AliOHTemplatedRepeater rptAddress;
		private Common_CouponSelect dropCoupon;
		private System.Web.UI.WebControls.Literal litOrderTotal;
		private System.Web.UI.HtmlControls.HtmlInputHidden selectShipTo;
		private System.Web.UI.HtmlControls.HtmlInputHidden regionId;
		private System.Web.UI.WebControls.Literal litProductTotalPrice;
		private int buyAmount;
		private string productSku;
		private int groupBuyId;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VSubmmitOrder.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litShipTo = (System.Web.UI.WebControls.Literal)this.FindControl("litShipTo");
			this.litCellPhone = (System.Web.UI.WebControls.Literal)this.FindControl("litCellPhone");
			this.litAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litAddress");
			this.rptCartProducts = (AliOHTemplatedRepeater)this.FindControl("rptCartProducts");
			this.dropCoupon = (Common_CouponSelect)this.FindControl("dropCoupon");
			this.litOrderTotal = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderTotal");
			this.groupbuyHiddenBox = (System.Web.UI.HtmlControls.HtmlInputControl)this.FindControl("groupbuyHiddenBox");
			this.rptAddress = (AliOHTemplatedRepeater)this.FindControl("rptAddress");
			this.selectShipTo = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("selectShipTo");
			this.regionId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("regionId");
			this.litProductTotalPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litProductTotalPrice");
			this.rptPromotions = (AliOHTemplatedRepeater)this.FindControl("rptPromotions");
			System.Collections.Generic.IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
			this.rptAddress.DataSource = 
				from item in shippingAddresses
				orderby item.IsDefault
				select item;
			this.rptAddress.DataBind();
			ShippingAddressInfo shippingAddressInfo = shippingAddresses.FirstOrDefault((ShippingAddressInfo item) => item.IsDefault);
			if (shippingAddressInfo == null)
			{
				shippingAddressInfo = ((shippingAddresses.Count > 0) ? shippingAddresses[0] : null);
			}
			if (shippingAddressInfo != null)
			{
				this.litShipTo.Text = shippingAddressInfo.ShipTo;
				this.litCellPhone.Text = shippingAddressInfo.CellPhone;
				this.litAddress.Text = shippingAddressInfo.Address;
				this.selectShipTo.SetWhenIsNotNull(shippingAddressInfo.ShippingId.ToString());
				this.regionId.SetWhenIsNotNull(shippingAddressInfo.RegionId.ToString());
			}
			if (shippingAddresses == null || shippingAddresses.Count == 0)
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/AliOH/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString()));
				return;
			}
			ShoppingCartInfo shoppingCartInfo;
			if (int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"]) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && (this.Page.Request.QueryString["from"] == "signBuy" || this.Page.Request.QueryString["from"] == "groupBuy"))
			{
				this.productSku = this.Page.Request.QueryString["productSku"];
				if (int.TryParse(this.Page.Request.QueryString["groupbuyId"], out this.groupBuyId))
				{
					this.groupbuyHiddenBox.SetWhenIsNotNull(this.groupBuyId.ToString());
					shoppingCartInfo = ShoppingCartProcessor.GetGroupBuyShoppingCart(this.productSku, this.buyAmount,0);
				}
				else
				{
					shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount,0);
				}
			}
			else
			{
				shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart();
			}
			if (shoppingCartInfo != null)
			{
				this.rptCartProducts.DataSource = shoppingCartInfo.LineItems;
				this.rptCartProducts.DataBind();
				this.dropCoupon.CartTotal = shoppingCartInfo.GetTotal();
				this.litOrderTotal.Text = (this.litProductTotalPrice.Text = shoppingCartInfo.GetTotal().ToString("F2"));
				System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> list = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>();
				if (shoppingCartInfo.IsReduced)
				{
					list.Add(new System.Collections.Generic.KeyValuePair<string, string>(PromotionHelper.GetShortName(PromoteType.Reduced), shoppingCartInfo.ReducedPromotionName + string.Format(" 优惠：{0}", shoppingCartInfo.ReducedPromotionAmount.ToString("F2"))));
				}
				if (shoppingCartInfo.IsFreightFree)
				{
					list.Add(new System.Collections.Generic.KeyValuePair<string, string>(PromotionHelper.GetShortName(PromoteType.FullAmountSentFreight), string.Format("{0}", shoppingCartInfo.FreightFreePromotionName)));
				}
				if (shoppingCartInfo.IsSendTimesPoint)
				{
					list.Add(new System.Collections.Generic.KeyValuePair<string, string>(PromotionHelper.GetShortName(PromoteType.FullAmountSentTimesPoint), string.Format("{0}：送{1}倍", shoppingCartInfo.SentTimesPointPromotionName, shoppingCartInfo.TimesPoint.ToString("F2"))));
				}
				if (this.groupBuyId == 0)
				{
					this.rptPromotions.DataSource = list;
					this.rptPromotions.DataBind();
				}
			}
			PageTitle.AddSiteNameTitle("订单确认");
		}
	}
}

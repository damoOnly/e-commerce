using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.CodeBehind.Common;
using EcShop.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPSubmmitOrder : WAPMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litShipTo;
		private System.Web.UI.WebControls.Literal litCellPhone;
		private System.Web.UI.WebControls.Literal litAddress;
		private System.Web.UI.HtmlControls.HtmlInputControl groupbuyHiddenBox;
		private WapTemplatedRepeater rptCartProducts;
		private WapTemplatedRepeater rptPromotions;
        private WAP_CouponSelect dropCoupon;
		private System.Web.UI.WebControls.Literal litOrderTotal;
		private System.Web.UI.HtmlControls.HtmlInputHidden selectShipTo;
		private System.Web.UI.HtmlControls.HtmlInputHidden regionId;
		private System.Web.UI.WebControls.Literal litProductTotalPrice;
		private int buyAmount;
		private string productSku;
		private int groupBuyId;
		private int countDownId;
        private WAP_VoucherSelect dropVoucher;
        private HtmlInputText txtVoucherCode;
        private HtmlInputText txtVoucherPwd;
        private System.Web.UI.WebControls.Literal litTotalTax;

        private System.Web.UI.WebControls.Literal litToalFreight;
        private System.Web.UI.WebControls.Literal litTotalQuantity;
        private System.Web.UI.HtmlControls.HtmlInputHidden isCustomsClearance;
        // private System.Web.UI.HtmlControls.HtmlInputHidden memberIdentityCard;
        //private System.Web.UI.HtmlControls.HtmlInputText txtmemberIdentityCard;
        //private System.Web.UI.HtmlControls.HtmlInputText txtRealName;
        private System.Web.UI.HtmlControls.HtmlInputHidden htmlIsCanMergeOrder;
        private System.Web.UI.WebControls.Literal litPromotionPrice;
        private System.Web.UI.WebControls.Literal litIdentityCard;
        private System.Web.UI.WebControls.Literal litPoint;
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
            this.rptCartProducts = (WapTemplatedRepeater)this.FindControl("rptCartProducts");
            this.dropCoupon = (WAP_CouponSelect)this.FindControl("dropCoupon");
            this.litOrderTotal = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderTotal");
            this.litPromotionPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litPromotionPrice");
            this.groupbuyHiddenBox = (System.Web.UI.HtmlControls.HtmlInputControl)this.FindControl("groupbuyHiddenBox");
            this.selectShipTo = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("selectShipTo");
            this.regionId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("regionId");
            this.litProductTotalPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litProductTotalPrice");
            this.rptPromotions = (WapTemplatedRepeater)this.FindControl("rptPromotions");
            this.litTotalTax = (System.Web.UI.WebControls.Literal)this.FindControl("litTotalTax");
            this.litToalFreight = (System.Web.UI.WebControls.Literal)this.FindControl("litToalFreight");
            this.litTotalQuantity = (System.Web.UI.WebControls.Literal)this.FindControl("litTotalQuantity");
            this.isCustomsClearance = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("isCustomsClearance");
            //this.txtmemberIdentityCard = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtmemberIdentityCard");
            //this.txtRealName = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtRealName");
            this.htmlIsCanMergeOrder = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("htmlIsCanMergeOrder");
            this.txtVoucherCode = (HtmlInputText)this.FindControl("txtVoucherCode");
            this.txtVoucherPwd = (HtmlInputText)this.FindControl("txtVoucherPwd");
            this.dropVoucher = (WAP_VoucherSelect)this.FindControl("dropVoucher"); // 现金券列表
            this.litIdentityCard = (System.Web.UI.WebControls.Literal)this.FindControl("litIdentityCard");
            this.litPoint = (System.Web.UI.WebControls.Literal)this.FindControl("litPoint");
			System.Collections.Generic.IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
			ShippingAddressInfo shippingAddressInfo = shippingAddresses.FirstOrDefault((ShippingAddressInfo item) => item.IsDefault);
			if (shippingAddressInfo == null)
			{
				shippingAddressInfo = ((shippingAddresses.Count > 0) ? shippingAddresses[0] : null);
			}
			if (shippingAddressInfo != null)
			{
				this.litShipTo.Text =  shippingAddressInfo.ShipTo;
				this.litCellPhone.Text = shippingAddressInfo.CellPhone;
                this.litAddress.Text = RegionHelper.GetFullRegion(shippingAddressInfo.RegionId, "  ") + shippingAddressInfo.Address;
                this.litIdentityCard.Text = shippingAddressInfo.IdentityCard;
				this.selectShipTo.SetWhenIsNotNull(shippingAddressInfo.ShippingId.ToString());
				this.regionId.SetWhenIsNotNull(shippingAddressInfo.RegionId.ToString());
			}
			if (shippingAddresses == null || shippingAddresses.Count == 0)
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "/Wapshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString()));
				return;
			}
			ShoppingCartInfo shoppingCartInfo;
            if (int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"]) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && (this.Page.Request.QueryString["from"] == "signBuy" || this.Page.Request.QueryString["from"] == "groupBuy"))
            {
                this.productSku = this.Page.Request.QueryString["productSku"];
                int storeId = 0;
                int.TryParse(this.Page.Request.QueryString["storeId"], out storeId);
                if (int.TryParse(this.Page.Request.QueryString["groupbuyId"], out this.groupBuyId))
                {
                    this.groupbuyHiddenBox.SetWhenIsNotNull(this.groupBuyId.ToString());
                    shoppingCartInfo = ShoppingCartProcessor.GetGroupBuyShoppingCart(this.productSku, this.buyAmount, storeId);
                }
                else
                {
                    shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount, storeId);
                }
            }
            else
            {
                if (int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"]) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && this.Page.Request.QueryString["from"] == "countDown")
                {
                    this.productSku = this.Page.Request.QueryString["productSku"];
                    int storeId = 0;
                    int.TryParse(this.Page.Request.QueryString["storeId"], out storeId);
                    if (int.TryParse(this.Page.Request.QueryString["countDownId"], out this.countDownId))
                    {
                        this.groupbuyHiddenBox.SetWhenIsNotNull(this.countDownId.ToString());
                        shoppingCartInfo = ShoppingCartProcessor.GetCountDownShoppingCart(this.productSku, this.buyAmount, storeId);
                    }
                    else
                    {
                        shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount, storeId);
                    }
                }
                else
                {
                    //shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart();
                    HttpCookie cookieSkuIds = this.Page.Request.Cookies["UserSession-SkuIds"];
                    if (cookieSkuIds != null && !string.IsNullOrEmpty(cookieSkuIds.Value))
                    {
                        shoppingCartInfo = ShoppingCartProcessor.GetPartShoppingCartInfo(Globals.UrlDecode(cookieSkuIds.Value));//获取用户选择的商品
                    }
                    else
                    {
                        shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart();
                    }
                    if (shoppingCartInfo != null && shoppingCartInfo.GetQuantity() == 0)
                    {
                        //this.buytype = "0";
                    }
                }
            }
            if (shoppingCartInfo != null)
            {
                this.rptCartProducts.DataSource = shoppingCartInfo.LineItems;
                this.rptCartProducts.DataBind();



                #region  获取优惠券筛选条件
                string bindId = "";
                string categoryId = "";

                List<int> bindIdList = new List<int>();
                List<int> categoryIdList = new List<int>();

                if (shoppingCartInfo.LineItems != null && shoppingCartInfo.LineItems.Count > 0)
                {
                    shoppingCartInfo.LineItems.ToList().ForEach(t =>
                    {
                        //单品券
                        if (t.ProductId > 0)
                        {
                            bindIdList.Add(t.ProductId);
                        }
                        //商品分类
                        if (t.CategoryId > 0)
                        {
                            categoryIdList.Add(t.CategoryId);
                        }
                        //品牌
                        if (t.BrandId > 0)
                        {
                            bindIdList.Add(t.BrandId);
                        }
                        //供货商
                        if (t.SupplierId > 0)
                        {
                            bindIdList.Add(t.SupplierId);
                        }
                    });
                }

                if (bindIdList != null && bindIdList.Count > 0)
                {
                    bindId = string.Join(",", bindIdList.Distinct().ToArray());
                }
                if (categoryIdList != null && categoryIdList.Count > 0)
                {
                    categoryId = string.Join(",", categoryIdList.Distinct().ToArray());
                }


                #endregion


                decimal totalAmount = shoppingCartInfo.GetNewTotal();
                this.dropVoucher.CartTotal = totalAmount;
                this.dropCoupon.CartTotal = totalAmount;
                this.dropCoupon.BindId = bindId;
                this.dropCoupon.CategoryId = categoryId;
                this.litPoint.Text = shoppingCartInfo.GetPoint().ToString();

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
                Member currentUser = HiContext.Current.User as Member;
                //this.txtmemberIdentityCard.Value = shippingAddressInfo.IdentityCard;
                //this.txtRealName.Value = shippingAddressInfo.ShipTo;
                #region 是否存在清关商品标识
                bool isOneTemplateId = true;
                int templateId = 0;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < shoppingCartInfo.LineItems.Count; i++)
                {
                    if (i == (shoppingCartInfo.LineItems.Count - 1))
                    {
                        stringBuilder.Append(shoppingCartInfo.LineItems[i].ProductId);
                    }
                    else
                    {
                        stringBuilder.AppendFormat("{0},", shoppingCartInfo.LineItems[i].ProductId);
                    }
                    if (i == 0)
                    {
                        templateId = shoppingCartInfo.LineItems[i].TemplateId;
                    }
                    else
                    {
                        if (templateId != shoppingCartInfo.LineItems[i].TemplateId)
                            isOneTemplateId = false;
                    }

                }
                bool b = ShoppingProcessor.CheckIsCustomsClearance(stringBuilder.ToString());
                if (b)
                {
                    this.isCustomsClearance.Value = "1";//表示存在需要清关的商品
                }
                else
                {
                    this.isCustomsClearance.Value = "0";
                }
                #endregion

                decimal tax = 0m;//输出税费+运费
                decimal freight = 0m;
                bool flag = groupBuyId > 0;
                int totalQuantity = 0;
                //Dictionary<int, decimal> dictShippingMode = new Dictionary<int, decimal>();
                foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                {
                    totalQuantity += item.Quantity;
                    tax += item.AdjustedPrice * item.TaxRate * item.Quantity;
                    #region 弃用代码
                    //if ((!shoppingCartInfo.IsFreightFree ||!item.IsfreeShipping|| flag))
                    //{
                    //    if (item.TemplateId > 0)
                    //    {
                    //        if (dictShippingMode.ContainsKey(item.TemplateId))
                    //        {
                    //            dictShippingMode[item.TemplateId] += item.Weight * item.Quantity;
                    //        }
                    //        else
                    //        {
                    //            dictShippingMode.Add(item.TemplateId, item.Weight * item.Quantity);
                    //        }
                    //    }
                    //}
                }
                //foreach (var item in dictShippingMode)//模拟分单，计算运费
                //{
                //    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(item.Key);
                //    freight += ShoppingProcessor.CalcFreight(shippingAddressInfo.RegionId, item.Value, shippingMode);
                //} 
                    #endregion
                freight = ShoppingCartProcessor.GetFreight(shoppingCartInfo, shippingAddressInfo.RegionId, false); //ShoppingProcessor.CalcShoppingCartFreight(shoppingCartInfo, shippingAddressInfo.RegionId);
                #region 判断是否符合单条件
                this.htmlIsCanMergeOrder.Value = "0";
                if (templateId != 0 && isOneTemplateId && tax <= 50)
                {
                    bool IsCanMergeOrder = ShoppingProcessor.CheckIsCanMergeOrder(templateId, tax, currentUser.UserId);
                    this.htmlIsCanMergeOrder.Value = IsCanMergeOrder ? "1" : "0";
                }
                #endregion

                //this.litTotalTax.Text =(tax<50?"0.00":tax.ToString("F2"));
                decimal totaltax = shoppingCartInfo.GetNewTotalTax();
                this.litTotalTax.Text = (totaltax < 50 ? "0.00" : totaltax.ToString("F2"));
                this.litToalFreight.Text = freight == 0 ? "0.00" : freight.ToString("F2");
                this.litProductTotalPrice.Text = shoppingCartInfo.GetTotal().ToString("F2");

                //tax=tax<50?0:tax;
                totaltax = totaltax < 50 ? 0 : totaltax;

                this.litOrderTotal.Text = (shoppingCartInfo.GetNewTotal() + totaltax + freight).ToString("F2");//总额=商品调整后价格+运费+税费 -活动优惠

                //活动优惠
                this.litPromotionPrice.Text = shoppingCartInfo.GetActivityPrice().ToString("F2");
                this.litTotalQuantity.Text = totalQuantity.ToString();
            }

            else
            {
                this.Page.Response.Redirect("ShoppingCart.aspx");
            }
            PageTitle.AddSiteNameTitle("订单确认");

            WAPHeadName.AddHeadName("确认订单");
        }
	}
}

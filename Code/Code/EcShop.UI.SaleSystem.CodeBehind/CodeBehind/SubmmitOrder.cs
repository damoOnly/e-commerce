using Ecdev.Plugins;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Commodities;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Messages;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class SubmmitOrder : HtmlTemplatedWebControl
    {
        private RegionSelector dropRegions;
        private System.Web.UI.WebControls.TextBox txtShipTo;
        private System.Web.UI.WebControls.TextBox txtShipTo1;
        private System.Web.UI.WebControls.TextBox txtAddress;
        private System.Web.UI.WebControls.TextBox txtZipcode;
        private System.Web.UI.WebControls.TextBox txtCellPhone;
        private System.Web.UI.WebControls.TextBox txtIdentityCard1;

        private System.Web.UI.WebControls.TextBox txtTelPhone;
        private System.Web.UI.HtmlControls.HtmlSelect drpShipToDate;
        private System.Web.UI.HtmlControls.HtmlInputHidden hidShippingId;
        //private Common_ShippingModeList shippingModeList;
        //private Common_PaymentModeList paymentModeList;  //旧支付方式列表
        private Common_PaymentModeList1 paymentModeList;
        //private Common_ShippingModeList1 shippingModeList1;
        private System.Web.UI.HtmlControls.HtmlInputHidden inputPaymentModeId;
        //private System.Web.UI.HtmlControls.HtmlInputHidden inputShippingModeId;
        private System.Web.UI.HtmlControls.HtmlInputHidden hdbuytype;
        private System.Web.UI.WebControls.Panel pannel_useraddress;
        private Common_SubmmintOrder_ProductList cartProductList;
        private Common_SubmmintOrder_GiftList cartGiftList;
        private FormatedMoneyLabel lblShippModePrice;
        private FormatedMoneyLabel lblPaymentPrice;
        private System.Web.UI.HtmlControls.HtmlInputHidden isCustomsClearance;//是否需要清关
        private System.Web.UI.HtmlControls.HtmlInputHidden isOneTemplateId;//是否单运费模版
        private System.Web.UI.HtmlControls.HtmlInputHidden isUnpackOrder;//是否拆单
        private System.Web.UI.HtmlControls.HtmlInputHidden shoppingCartProductQuantity;//购物车商品数量，不包含赠品
        private System.Web.UI.WebControls.Literal litProductAmount;
        private System.Web.UI.WebControls.Literal litProductBundling;
        private System.Web.UI.WebControls.Label litAllWeight;
        private System.Web.UI.WebControls.Label litPoint;
        private System.Web.UI.WebControls.HyperLink hlkSentTimesPoint;
        private FormatedMoneyLabel lblOrderTotal;
        private FormatedMoneyLabel lblTotalTax;
        private System.Web.UI.WebControls.TextBox txtMessage;
        private System.Web.UI.WebControls.Label litTaxRate;
        private System.Web.UI.WebControls.HyperLink hlkFeeFreight;
        private System.Web.UI.WebControls.HyperLink hlkReducedPromotion;
        private FormatedMoneyLabel lblTotalPrice;
        private System.Web.UI.HtmlControls.HtmlInputHidden htmlCouponCode;
        private FormatedMoneyLabel litCouponAmout;
        //去除发票
        //private System.Web.UI.HtmlControls.HtmlInputCheckBox chkTax;
        private System.Web.UI.HtmlControls.HtmlSelect CmbCoupCode;
        private System.Web.UI.HtmlControls.HtmlTable tbCoupon;
        private System.Web.UI.WebControls.TextBox txtInvoiceTitle;
        private IButton btnCreateOrder;
        private ShoppingCartInfo shoppingCart;
        private HttpCookie cookieSkuIds;
        private int buyAmount;
        private string productSku;
        private bool isGroupBuy;
        private bool isCountDown;
        private bool isSignBuy;
        private bool isBundling;
        private bool isCustomsClearance1;
        private HashSet<int> hsSupplierId;
        private HashSet<int> hsTemplateId;
        private string buytype = "";
        private int bundlingid;
        private decimal bundlingprice;
        private System.Web.UI.HtmlControls.HtmlSelect CmbVoucherCode;
        private System.Web.UI.HtmlControls.HtmlInputHidden htmlVoucherCode;
        private System.Web.UI.HtmlControls.HtmlInputHidden hdSiteId;
        private int cid = 0;
        private string wid = string.Empty;

        protected override void OnInit(System.EventArgs e)
        {
            if (int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"]) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && this.Page.Request.QueryString["from"] == "groupBuy")
            {
                this.productSku = this.Page.Request.QueryString["productSku"];
                this.isGroupBuy = true;
                this.shoppingCart = ShoppingCartProcessor.GetGroupBuyShoppingCart(this.productSku, this.buyAmount, 0);//PC端未添加门店id
                this.buytype = "GroupBuy";//团购
            }
            else
            {
                if (int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"]) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && this.Page.Request.QueryString["from"] == "countDown")
                {
                    this.productSku = this.Page.Request.QueryString["productSku"];
                    this.isCountDown = true;
                    this.shoppingCart = ShoppingCartProcessor.GetCountDownShoppingCart(this.productSku, this.buyAmount, 0);
                    this.buytype = "CountDown";
                }
                else
                {
                    if (int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"]) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && this.Page.Request.QueryString["from"] == "signBuy")
                    {
                        this.productSku = this.Page.Request.QueryString["productSku"];
                        this.isSignBuy = true;
                        this.shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount, 0);//PC端未添加门店id
                    }
                    else
                    {
                        if (int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["Bundlingid"]) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && this.Page.Request.QueryString["from"] == "Bundling")
                        {
                            this.productSku = this.Page.Request.QueryString["Bundlingid"];
                            if (int.TryParse(this.productSku, out this.bundlingid))
                            {
                                this.shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.bundlingid, this.buyAmount);
                                this.isBundling = true;
                                this.buytype = "Bundling";
                            }
                        }
                        else
                        {
                            cookieSkuIds = this.Page.Request.Cookies["UserSession-SkuIds"];
                            //if (cookieSkuIds == null || string.IsNullOrWhiteSpace(cookieSkuIds.Value))
                            //{
                            //    this.shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                            //}
                            //else
                            //{
                            //    this.shoppingCart = ShoppingCartProcessor.GetPartShoppingCartInfo(Globals.UrlDecode(cookieSkuIds.Value));//获取用户选择的商品
                            //}

                            if (cookieSkuIds != null && !string.IsNullOrWhiteSpace(cookieSkuIds.Value))
                            {
                                this.shoppingCart = ShoppingCartProcessor.GetPartShoppingCartInfo(Globals.UrlDecode(cookieSkuIds.Value));//获取用户选择的商品
                            }
                            else
                            {
                                this.shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                            }

                            if (this.shoppingCart != null && this.shoppingCart.GetQuantity() == 0)
                            {
                                this.buytype = "0";
                            }
                        }
                    }
                }
            }
            if (this.shoppingCart == null)
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件商品已经被管理员删除"));
                return;
            }
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-SubmmitOrder.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            Member member = HiContext.Current.User as Member;
            bool rest = MemberProcessor.CheckUserIsVerify(member.UserId);
            if (!rest)
            {
                this.Page.Response.Redirect("user/IdentityCardVericetion.aspx?edit=true&type=submit&buyAmount=" + this.Page.Request.QueryString["buyAmount"] + "&productSku=" + this.Page.Request.QueryString["productSku"] + "&from=" + this.Page.Request.QueryString["from"]);
                return;
            }
            this.dropRegions = (RegionSelector)this.FindControl("dropRegions");
            this.txtShipTo = (System.Web.UI.WebControls.TextBox)this.FindControl("txtShipTo");
            this.txtShipTo1 = (System.Web.UI.WebControls.TextBox)this.FindControl("txtShipTo1");
            this.txtAddress = (System.Web.UI.WebControls.TextBox)this.FindControl("txtAddress");
            this.txtZipcode = (System.Web.UI.WebControls.TextBox)this.FindControl("txtZipcode");
            this.txtCellPhone = (System.Web.UI.WebControls.TextBox)this.FindControl("txtCellPhone");
           
            this.txtIdentityCard1 = (System.Web.UI.WebControls.TextBox)this.FindControl("txtIdentityCard1");
            this.txtTelPhone = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTelPhone");
            this.txtInvoiceTitle = (System.Web.UI.WebControls.TextBox)this.FindControl("txtInvoiceTitle");
            this.drpShipToDate = (System.Web.UI.HtmlControls.HtmlSelect)this.FindControl("drpShipToDate");
            this.litTaxRate = (System.Web.UI.WebControls.Label)this.FindControl("litTaxRate");
            //this.shippingModeList = (Common_ShippingModeList)this.FindControl("Common_ShippingModeList");
            //this.paymentModeList = (Common_PaymentModeList)this.FindControl("grd_Common_PaymentModeList");
            this.paymentModeList = (Common_PaymentModeList1)this.FindControl("list_Common_PaymentModeList1");
            this.inputPaymentModeId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("inputPaymentModeId");
            //this.inputShippingModeId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("inputShippingModeId");
            this.hdbuytype = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdbuytype");
            this.hdSiteId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdSiteId");
            this.pannel_useraddress = (System.Web.UI.WebControls.Panel)this.FindControl("pannel_useraddress");
            this.lblPaymentPrice = (FormatedMoneyLabel)this.FindControl("lblPaymentPrice");
            this.lblShippModePrice = (FormatedMoneyLabel)this.FindControl("lblShippModePrice");
            //去除发票
            //this.chkTax = (System.Web.UI.HtmlControls.HtmlInputCheckBox)this.FindControl("chkTax");
            this.cartProductList = (Common_SubmmintOrder_ProductList)this.FindControl("Common_SubmmintOrder_ProductList");
            this.cartGiftList = (Common_SubmmintOrder_GiftList)this.FindControl("Common_SubmmintOrder_GiftList");
            this.litProductAmount = (System.Web.UI.WebControls.Literal)this.FindControl("litProductAmount");
            this.litProductBundling = (System.Web.UI.WebControls.Literal)this.FindControl("litProductBundling");
            this.litAllWeight = (System.Web.UI.WebControls.Label)this.FindControl("litAllWeight");
            this.litPoint = (System.Web.UI.WebControls.Label)this.FindControl("litPoint");
            this.hlkSentTimesPoint = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlkSentTimesPoint");
            this.lblOrderTotal = (FormatedMoneyLabel)this.FindControl("lblOrderTotal");
            this.txtMessage = (System.Web.UI.WebControls.TextBox)this.FindControl("txtMessage");
            this.hlkFeeFreight = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlkFeeFreight");
            this.hlkReducedPromotion = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlkReducedPromotion");
            this.lblTotalPrice = (FormatedMoneyLabel)this.FindControl("lblTotalPrice");
            this.htmlCouponCode = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("htmlCouponCode");
            this.CmbCoupCode = (System.Web.UI.HtmlControls.HtmlSelect)this.FindControl("CmbCoupCode");
            this.tbCoupon = (System.Web.UI.HtmlControls.HtmlTable)this.FindControl("tbCoupon");
            this.litCouponAmout = (FormatedMoneyLabel)this.FindControl("litCouponAmout");
            this.btnCreateOrder = ButtonManager.Create(this.FindControl("btnCreateOrder"));
            this.btnCreateOrder.Click += new System.EventHandler(this.btnCreateOrder_Click);
            this.hidShippingId = (HtmlInputHidden)this.FindControl("hidShippingId");
            this.isCustomsClearance = (HtmlInputHidden)this.FindControl("isCustomsClearance");
            this.isOneTemplateId = (HtmlInputHidden)this.FindControl("isOneTemplateId");
            this.isUnpackOrder = (HtmlInputHidden)this.FindControl("isUnpackOrder");
            this.shoppingCartProductQuantity = (HtmlInputHidden)this.FindControl("shoppingCartProductQuantity");
            this.lblTotalTax = (FormatedMoneyLabel)this.FindControl("lblTotalTax");

            this.CmbVoucherCode = (System.Web.UI.HtmlControls.HtmlSelect)this.FindControl("CmbVoucherCode");
            this.htmlVoucherCode = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("htmlVoucherCode");
            this.cartProductList.ItemDataBound += cartProductList_ItemDataBound;

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (!this.Page.IsPostBack)
            {
                this.BindUserAddress();
                //this.shippingModeList.DataSource = ShoppingProcessor.GetShippingModes();
                //this.shippingModeList.DataBind();
                this.ReBindPayment();
                if (this.shoppingCart != null)
                {
                    this.litTaxRate.Text = masterSettings.TaxRate.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    this.BindShoppingCartInfo(this.shoppingCart);
                    if (this.isGroupBuy || this.isCountDown || this.isBundling || this.shoppingCart.LineItems.Count == 0)
                    {
                        // this.tbCoupon.Visible = false;
                    }
                    string bindId = "";
                    string categoryId = "";
                    
                    List<int> bindIdList = new List<int>();
                    List<int> categoryIdList = new List<int>();

                    if (shoppingCart.LineItems != null && shoppingCart.LineItems.Count > 0)
                    {
                        shoppingCart.LineItems.ToList().ForEach(t =>
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

                    //优惠券数据绑定
                    this.CmbCoupCode.DataTextField = "Name";
                    this.CmbCoupCode.DataValueField = "ClaimCode";
                    this.CmbCoupCode.DataSource = ShoppingProcessor.GetCoupon(this.shoppingCart.GetNewTotal(), bindId, categoryId);
                    this.CmbCoupCode.DataBind();
                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem("请选择一张优惠券 如无则无需选择", "0");
                    this.CmbCoupCode.Items.Insert(0, item);

                    //现金券数据绑定
                    this.CmbVoucherCode.DataTextField = "Name";
                    this.CmbVoucherCode.DataValueField = "ClaimCode";
                    this.CmbVoucherCode.DataSource = ShoppingProcessor.GetVoucher(this.shoppingCart.GetNewTotal());
                    this.CmbVoucherCode.DataBind();
                    System.Web.UI.WebControls.ListItem item2 = new System.Web.UI.WebControls.ListItem("请选择一张现金券 如无则无需选择", "0");
                    this.CmbVoucherCode.Items.Insert(0, item2);

                    this.hdbuytype.Value = this.buytype;
                    //  this.pannel_useraddress.Visible = (!HiContext.Current.User.IsAnonymous && MemberProcessor.GetShippingAddressCount() > 0);
                    return;
                }
            }
            else
            {
                if (this.shoppingCart != null)
                {
                    this.BindShoppingCartInfo(this.shoppingCart);
                }
            }
        }

        void cartProductList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                System.Web.UI.HtmlControls.HtmlInputHidden promotionProductId = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Item.FindControl("promotionProductId");
                System.Web.UI.WebControls.Repeater dtPresendPro = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("dtPresendPro");
                if (shoppingCart != null)
                {
                    // 赠送活动
                    List<ShoppingCartPresentInfo> presentList = (List<ShoppingCartPresentInfo>)shoppingCart.LinePresentPro;
                    if (presentList != null && presentList.Count > 0)
                    {
                        var p = presentList.Where(m => m.PromotionProductId == (string.IsNullOrWhiteSpace(promotionProductId.Value) ? 0 : Convert.ToInt32(promotionProductId.Value))).Select(n => n);
                        dtPresendPro.DataSource = p;
                        dtPresendPro.DataBind();
                    }

                }

            }
        }
        public void btnCreateOrder_Click(object sender, System.EventArgs e)
        {

            //int CategoriesId = 0;
            //int.TryParse(this.Page.Request.QueryString["CategoriesId"], out CategoriesId);
            //if (CategoriesId>0)
            //{
            //    bool rest = ProductBrowser.CheckActiveIsRunding(CategoriesId, int.Parse(this.productSku.ToString()));
            //    if(!rest)
            //    {
            //        this.ShowMessage("活动已经结束，不能再提交", false);
            //        return;
            //    }
            //}

            int orderCountIn3Min;
            int todayOrderCount;
            TradeHelper.GetOrderCount4MaliciousOrder(HiContext.Current.User.UserId, out orderCountIn3Min, out todayOrderCount);
            if (todayOrderCount >= 20)
            {
                this.ShowMessage("您今天的订单数已经达到20上限", false);
                return;
            }
            if (orderCountIn3Min >= 5)
            {
                this.ShowMessage("下单频率太快，请稍后再试", false);
                return;
            }           

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            if (!this.ValidateCreateOrder())
            {
                return;
            }

            int result = -1;

            //PC首单0.88折（14 - 20） 每件商品都是0.88
            if (DateTime.Now < DateTime.Parse("2015-08-21"))
            {
                //该用户的第一条订单
                result = ShoppingProcessor.CheckIsFirstOrder(HiContext.Current.User.UserId, (int)OrderSource.PC);
                //没有数据就是首单
                if (result == 0)
                {
                    if (shoppingCart.LineItems.Count > 0)
                    {
                        List<ShoppingCartItemInfo> list = shoppingCart.LineItems.ToList();
                        if (list != null && list.Count > 0)
                        {
                            list.ForEach(t =>
                            {
                                t.AdjustedPrice = t.AdjustedPrice * 0.88M;
                            });
                        }
                    }
                }
            }

            OrderInfo orderInfo = this.GetOrderInfo(this.shoppingCart);

            if (!string.IsNullOrEmpty(this.hdSiteId.Value))
            {
                orderInfo.SiteId = int.Parse(this.hdSiteId.Value);
            }


            if (result == 0)
            {
                orderInfo.ActivityType = 1;
            }
            else
            {
                orderInfo.ActivityType = 0;
            }

            if (this.shoppingCart.GetQuantity() > 1)
            {
                this.isSignBuy = false;
            }

            #region 不在限额
            //if (this.shoppingCart.GetAmount() > 1000)  
            //{
            //    this.ShowMessage("抱歉，您已超过海关限额￥1000，请分次购买。 </br>海关规定：</br>① 消费者购买进口商品，以“个人自用，合理数量”为原则，每单最大购买限额为1000元人民币。</br>② 如果订单只含单件不可分割商品，则可以超过1000元限值。</br>", false);
            //    return;
            //} 
            #endregion


            //if (this.shoppingCart.GetQuantity() > 30)
            //{
            //    this.ShowMessage("购物车中的商品数量不能超过30个", false);
            //    return;
            //}
            if (orderInfo == null)
            {
                this.ShowMessage("购物车中已经没有任何商品", false);
                return;
            }
            if (orderInfo.GetNewTotal() < 0m)
            {
                this.ShowMessage("订单金额不能为负", false);
                return;
            }
            if (!HiContext.Current.User.IsAnonymous)
            {
                int totalNeedPoint = this.shoppingCart.GetTotalNeedPoint();
                int points = ((Member)HiContext.Current.User).Points;
                if (points >= 0 && totalNeedPoint > points)
                {
                    this.ShowMessage("您当前的积分不够兑换所需积分！", false);
                    return;
                }
            }

            #region 判断是否是特定二级类型的商品使用了优惠券
            string CurrentCategoryDesc = ShoppingProcessor.GetCurrentCategoryDesc(shoppingCart, this.CmbCoupCode.Value);
            if (!string.IsNullOrWhiteSpace(CurrentCategoryDesc))
            {
                this.ShowMessage(CurrentCategoryDesc, false);
                return;
            }
            #endregion

            if (this.isCountDown)
            {
                CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo(this.shoppingCart.LineItems[0].ProductId);
                if (countDownInfo.EndDate < System.DateTime.Now)
                {
                    this.ShowMessage("此订单为抢购订单，但抢购时间已到！", false);
                    return;
                }

                //限购数量为0表示限购数量不限制
                if (countDownInfo.MaxCount > 0)
                {
                    if (this.shoppingCart.LineItems[0].Quantity > countDownInfo.MaxCount)
                    {
                        this.ShowMessage("你购买的数量超过限购数量:" + countDownInfo.MaxCount.ToString(), false);
                        return;
                    }
                    int num = ShoppingProcessor.CountDownOrderCount(this.shoppingCart.LineItems[0].ProductId, HiContext.Current.User.UserId, countDownInfo.CountDownId);
                    if (num + this.shoppingCart.LineItems[0].Quantity > countDownInfo.MaxCount)
                    {
                        this.ShowMessage(string.Format("你已经抢购过该商品{0}件，每个用户只允许抢购{1}件,如果你有未付款的抢购单，请及时支付！", num, countDownInfo.MaxCount), false);
                        return;
                    }
                }

                int num2 = ShoppingProcessor.AllCountDownOrderCount(this.shoppingCart.LineItems[0].ProductId, countDownInfo.CountDownId);
                if (num2 + this.shoppingCart.LineItems[0].Quantity > countDownInfo.PlanCount)
                {
                    this.ShowMessage("该商品活动数量不足", false);
                    return;
                }
            }
            try
            {
                bool b = false;
                string firstOrderId = orderInfo.OrderId;

                //只有一个供应商时，给供应商ID赋值
                if (this.shoppingCart.LineItems != null && this.shoppingCart.LineItems.Count > 0)
                {
                    orderInfo.SupplierId = this.shoppingCart.LineItems[0].SupplierId;
                }

                if (ShoppingProcessor.CreateOrder(orderInfo, true, true))
                {
                    Messenger.OrderCreated(orderInfo, HiContext.Current.User);
                    orderInfo.OnCreated();

                    var totalQuantity = this.shoppingCart.GetQuantity();

                    //拆单处理
                    if (//hsSupplierId.Count > 1//两个及以上个供应商
                        //|| hsTemplateId.Count > 1//两个及以上个运费模版
                        (totalQuantity > 1 && orderInfo.GetNewAmount() > 1000)//订单金额超过1000
                        || (isUnpackOrder.Value == "1" && orderInfo.OriginalTax > 50 && totalQuantity > 1)//税费大于50
                        )
                    {

                        decimal unpackedTaxTotal = 0;
                        decimal unpackedOrderTotal = 0;
                        decimal unpackedOrderFreight = 0m;
                        //b = ShoppingProcessor.UnpackOrderBySupplier(orderInfo, ref unpackedTaxTotal, ref unpackedOrderTotal, ref unpackedOrderFreight);
                        b = ShoppingProcessor.CreateChildOrders(orderInfo, ref unpackedTaxTotal, ref unpackedOrderTotal, ref unpackedOrderFreight);
                        if (b)
                        {
                            //修改原订单金额
                            ShoppingProcessor.UpdateWillSplitOrder(firstOrderId, unpackedTaxTotal, unpackedOrderTotal, unpackedOrderFreight);
                            Messenger.OrderCreated(ShoppingProcessor.GetOrderInfo(firstOrderId), HiContext.Current.User);
                        }
                    }
                    int totalNeedPoint = this.shoppingCart.GetTotalNeedPoint();

                    if (totalNeedPoint > 0)
                    {
                        ShoppingProcessor.CutNeedPoint(totalNeedPoint, orderInfo.OrderId);
                    }
                    if (!this.isCountDown && !this.isGroupBuy && !this.isSignBuy && !this.isBundling)
                    {
                        //有选择删除购物车商品
                        if (cookieSkuIds == null || string.IsNullOrEmpty(cookieSkuIds.Value))
                        {
                            ShoppingCartProcessor.ClearShoppingCart();
                        }
                        else
                        {
                            ShoppingCartProcessor.ClearPartShoppingCart(Globals.UrlDecode(cookieSkuIds.Value));
                            cookieSkuIds.Expires = DateTime.Now.AddDays(-1);
                            this.Page.Response.AppendCookie(cookieSkuIds);
                        }
                    }

                    #region 推送广告信息
                    try
                    {
                        if (System.Web.HttpContext.Current.Request.Cookies["AdCookies_cid"] != null && System.Web.HttpContext.Current.Request.Cookies["AdCookies_wi"] != null)
                        {
                            int.TryParse(System.Web.HttpContext.Current.Request.Cookies["AdCookies_cid"].Value.ToString(), out cid);
                            wid = System.Web.HttpContext.Current.Request.Cookies["AdCookies_wi"].Value.ToString();
                            string filexml = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["CurrentCategoryId"]) ? "" : System.Configuration.ConfigurationManager.AppSettings["CurrentCategoryId"].ToString();

                            Action ac = new Action(() =>
                            {
                                orders adInfo = new orders();
                                adInfo.OrderNo = firstOrderId;
                                adInfo.Campaignid = cid;
                                adInfo.Feedback = wid;
                                adInfo.OrderTime = orderInfo.OrderDate.ToString();
                                DataTable dt = OrderHelper.GetOrderItemInfo(firstOrderId);
                                List<products> list = new List<products>();
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    products prd = null;
                                    foreach (DataRow rows in dt.Rows)
                                    {
                                        prd = new products();
                                        prd.Amount = int.Parse(rows["Quantity"].ToString());

                                        double totalprice = double.Parse(rows["OrderTotal"].ToString());
                                        double couponamount = double.Parse(rows["CouponValue"].ToString());

                                        prd.Price = double.Parse(rows["ItemAdjustedPrice"].ToString());
                                        double baifb = couponamount / (totalprice + couponamount);
                                        double price = prd.Price * (1 - baifb);

                                        prd.Price = Math.Round(price, 2); ;
                                        prd.Name = rows["ProductName"].ToString();
                                        prd.ProductNo = rows["SkuId"].ToString();
                                        prd.Category = rows["CategoryId"].ToString();

                                        bool rest = CategoryBrowser.CheckPructCag(filexml, prd.Category);

                                        if (rest)
                                        {
                                            prd.CommissionType = "A";
                                        }
                                        else
                                        {
                                            prd.CommissionType = "B";
                                        }
                                        list.Add(prd);
                                    }
                                    adInfo.products = list;
                                    adInfo.UpdateTime = orderInfo.OrderDate.ToString();
                                    adInfo.OrderStatus = "待付款";
                                    adInfo.PaymentStatus = "未支付";
                                    adInfo.PaymentType = orderInfo.PaymentType;
                                    List<orders> OrderList = new List<orders>();
                                    OrderList.Add(adInfo);
                                    AdOrderInfo ad = new AdOrderInfo();
                                    ad.orders = OrderList;
                                    string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(ad);
                                    OrderHelper.InsertAdOrderInfo(adInfo, jsonStr);
                                    string url = "http://o.yiqifa.com/servlet/handleCpsInterIn";
                                    string interId = System.Configuration.ConfigurationManager.AppSettings["interId"].ToString();
                                    string DataStr = "interId=" + interId + "&json=" + HttpUtility.UrlEncode(jsonStr) + "&encoding=UTF-8";
                                    string RqRest = HttpGet(url, DataStr);
                                    ErrorLog.Write("推送CPS返回结果：" + RqRest.ToString());
                                }
                            });
                            ac.BeginInvoke(null, ac);
                        }
                        else
                        {
                            ErrorLog.Write("没有获取Cookies值！");
                        }
                    }
                    catch (Exception ee)
                    { }
                    #endregion

                    this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("FinishOrder", new object[]
                    {
                        firstOrderId
                    }));
                }
            }
            catch (System.Exception ex)
            {
                this.ShowMessage(ex.ToString(), false);
            }
            sw.Stop();
            ErrorLog.Write("执行提交订单所用的时间（毫秒）:" + sw.ElapsedMilliseconds.ToString());
        }
        /// <summary>
        /// GET请求与获取结果
        /// </summary>
        public static string HttpGet(string Url, string postDataStr)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception ee)
            {
                ErrorLog.Write("推送CPS返回结果：" + ee.Message, ee);
                return ee.Message.ToString();
            }
        }
        private void ReBindPayment()
        {
            System.Collections.Generic.IList<PaymentModeInfo> list = ShoppingProcessor.GetPaymentModes(PayApplicationType.payOnPC);
            System.Collections.Generic.IList<PaymentModeInfo> list2 = new System.Collections.Generic.List<PaymentModeInfo>();
            if (this.Page.Request.QueryString["from"] == "groupBuy")
            {
                list = (
                    from item in list
                    where item.Gateway != "ecdev.plugins.payment.podrequest"
                    select item).ToList<PaymentModeInfo>();
            }
            System.Web.HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
            if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
            {
                using (System.Collections.Generic.IEnumerator<PaymentModeInfo> enumerator = list.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        PaymentModeInfo current = enumerator.Current;
                        if (string.Compare(current.Gateway, "ecdev.plugins.payment.alipay_shortcut.shortcutrequest", true) == 0 || string.Compare(current.Gateway, "ecdev.plugins.payment.alipaydirect.directrequest", true) == 0 || string.Compare(current.Gateway, "ecdev.plugins.payment.alipayassure.assurerequest", true) == 0 || string.Compare(current.Gateway, "ecdev.plugins.payment.alipay.standardrequest", true) == 0 || (string.Compare(current.Gateway, "ecdev.plugins.payment.advancerequest", true) == 0 && !HiContext.Current.User.IsAnonymous))
                        {
                            list2.Add(current);
                        }
                    }
                    goto IL_1C8;
                }
            }
            foreach (PaymentModeInfo current2 in list)
            {
                if (string.Compare(current2.Gateway, "ecdev.plugins.payment.alipay_shortcut.shortcutrequest", true) != 0)
                {
                    list2.Add(current2);
                }
                if (string.Compare(current2.Gateway, "ecdev.plugins.payment.advancerequest", true) == 0 && HiContext.Current.User.IsAnonymous)
                {
                    list2.Remove(current2);
                }
            }
        IL_1C8:
            this.paymentModeList.DataSource = list2;
            this.paymentModeList.DataBind();
        }
        private void BindUserAddress()
        {
            if (!HiContext.Current.User.IsAnonymous)
            {
                Member member = HiContext.Current.User as Member;
                this.txtShipTo.Text = member.RealName;
                this.dropRegions.SetSelectedRegionId(new int?(member.RegionId));
                this.dropRegions.DataBind();
                this.txtAddress.Text = member.Address;
                this.txtTelPhone.Text = member.TelPhone;
                this.txtCellPhone.Text = member.CellPhone;

                DataTable dt = SitesManagementHelper.GetMySubMemberByUserId(HiContext.Current.User.UserId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.txtIdentityCard1.Text = dt.Rows[0]["IdentityCard"].ToString();
                    this.txtShipTo1.Text = dt.Rows[0]["RealName"].ToString();
                }
                else
                {
                    this.txtShipTo1.Text = member.RealName;
                    this.txtIdentityCard1.Text = member.IdentityCard;
                }
            }
        }
        private void BindShoppingCartInfo(ShoppingCartInfo shoppingCart)
        {
            if (shoppingCart.LineItems.Count > 0)
            {
                this.cartProductList.DataSource = shoppingCart.LineItems;
                this.cartProductList.DataBind();
                this.cartProductList.ShowProductCart();
            }
            if (shoppingCart.LineGifts.Count > 0)
            {
                System.Collections.Generic.IEnumerable<ShoppingCartGiftInfo> enumerable =
                    from s in shoppingCart.LineGifts
                    where s.PromoType == 0
                    select s;
                System.Collections.Generic.IEnumerable<ShoppingCartGiftInfo> enumerable2 =
                    from s in shoppingCart.LineGifts
                    where s.PromoType == 5
                    select s;
                this.cartGiftList.DataSource = enumerable;
                this.cartGiftList.FreeDataSource = enumerable2;
                this.cartGiftList.DataBind();
                this.cartGiftList.ShowGiftCart(enumerable.Count<ShoppingCartGiftInfo>() > 0, enumerable2.Count<ShoppingCartGiftInfo>() > 0);
            }
            if (shoppingCart.IsReduced)
            {
                this.litProductAmount.Text = string.Format("商品金额：{0}", Globals.FormatMoney(shoppingCart.GetNewAmount()));
                this.hlkReducedPromotion.Text = shoppingCart.ReducedPromotionName + string.Format(" 优惠：{0}", shoppingCart.ReducedPromotionAmount.ToString("F2"));
                this.hlkReducedPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					shoppingCart.ReducedPromotionId
				});
            }
            if (shoppingCart.IsFreightFree)
            {
                this.hlkFeeFreight.Text = string.Format("（{0}）", shoppingCart.FreightFreePromotionName);
                this.hlkFeeFreight.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					shoppingCart.FreightFreePromotionId
				});
            }
            StringBuilder stringBuilder = new StringBuilder();
            int templateId = 0;
            bool isOneTemplateId = false;
            for (int i = 0; i < shoppingCart.LineItems.Count; i++)
            {
                if (i == (shoppingCart.LineItems.Count - 1))
                {
                    stringBuilder.Append(shoppingCart.LineItems[i].ProductId);
                }
                else
                {
                    stringBuilder.AppendFormat("{0},", shoppingCart.LineItems[i].ProductId);
                }
                if (i == 0)
                {
                    templateId = shoppingCart.LineItems[i].TemplateId;
                }
                else
                {
                    if (templateId != shoppingCart.LineItems[i].TemplateId)
                        isOneTemplateId = false;
                }
            }
            bool b = ShoppingProcessor.CheckIsCustomsClearance(stringBuilder.ToString());
            if (b)
            {
                isCustomsClearance1 = true;
                this.isCustomsClearance.Value = "1";//表示存在需要清关的商品
            }
            else
            {
                this.isCustomsClearance.Value = "0";
            }
            this.isOneTemplateId.Value = isOneTemplateId ? "1" : "0";
            this.shoppingCartProductQuantity.Value = shoppingCart.GetQuantity().ToString();
            this.lblTotalPrice.Money = shoppingCart.GetNewTotal();
            this.lblTotalTax.Money = shoppingCart.GetNewTotalTax();
            this.lblOrderTotal.Money = shoppingCart.GetNewTotal() + shoppingCart.GetNewTotalTax();

            this.litPoint.Text = shoppingCart.GetPoint().ToString();
            if (this.isBundling)
            {
                BundlingInfo bundlingInfo = ProductBrowser.GetBundlingInfo(this.bundlingid);
                this.lblTotalPrice.Money = bundlingInfo.Price;
                this.lblOrderTotal.Money = bundlingInfo.Price;
                this.litPoint.Text = shoppingCart.GetPoint(bundlingInfo.Price).ToString();
                this.litProductBundling.Text = "（捆绑价）";
            }
            this.litAllWeight.Text = shoppingCart.TotalWeight.ToString("F2");
            if (shoppingCart.IsSendTimesPoint)
            {
                this.hlkSentTimesPoint.Text = string.Format("（{0}；送{1}倍）", shoppingCart.SentTimesPointPromotionName, shoppingCart.TimesPoint.ToString("F2"));
                this.hlkSentTimesPoint.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					shoppingCart.SentTimesPointPromotionId
				});
            }
        }
        private bool ValidateCreateOrder()
        {
            Member member = HiContext.Current.User as Member;
            if (member==null)
            {
                this.ShowMessage("请登录", false);
                return false;
            }
          
            if (!this.dropRegions.GetSelectedRegionId().HasValue || this.dropRegions.GetSelectedRegionId().Value == 0)
            {
                this.ShowMessage("请选择收货地址", false);
                return false;
            }
            string pattern = "^[\\u4e00-\\u9fa5]{2,6}$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            string patternIdentityCard = "^[1-9]\\d{5}[1-9]\\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\\d{3}(\\d|x|X)";
            System.Text.RegularExpressions.Regex regexIdentityCard = new System.Text.RegularExpressions.Regex(patternIdentityCard);
            if (string.IsNullOrEmpty(this.txtShipTo.Text) || !regex.IsMatch(this.txtShipTo.Text.Trim()))
            {
                this.ShowMessage("收货人名字只能为2-6个汉字", false);
                return false;
            }
            //if (this.txtShipTo.Text.Length < 2 || this.txtShipTo.Text.Length > 20)
            //{
            //    this.ShowMessage("收货人姓名长度应在2-20个字符之间", false);
            //    return false;
            //}
            if (string.IsNullOrEmpty(this.txtAddress.Text))
            {
                this.ShowMessage("请输入收货人详细地址", false);
                return false;
            }
            //if (string.IsNullOrEmpty(this.inputShippingModeId.Value))
            //{
            //    this.ShowMessage("请选择配送方式", false);
            //    return false;
            //}
            if (string.IsNullOrEmpty(this.inputPaymentModeId.Value))
            {
                this.ShowMessage("请选择支付方式", false);
                return false;
            }
            if (string.IsNullOrEmpty(this.txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()))
            {
                this.ShowMessage("电话号码和手机号码必填其一", false);
                return false;
            }

            if (!string.IsNullOrEmpty(this.txtIdentityCard1.Text) && !regexIdentityCard.IsMatch(this.txtIdentityCard1.Text.Trim()))
            {
                this.ShowMessage("请输入正确的身份证号码", false);
                return false;
            }
            //验证库存
            foreach (ShoppingCartItemInfo item in this.shoppingCart.LineItems)
            {
                int stock = ShoppingProcessor.GetProductStock(item.SkuId);
                if (stock <= 0 || stock < item.Quantity)
                {
                    this.ShowMessage(string.Format("购物车中的商品\"{0}\"库存不足", item.Name), false);
                    return false;
                }
            }
            hsSupplierId = new HashSet<int>();
            hsTemplateId = new HashSet<int>();
            if (shoppingCart.LineItems.Count != shoppingCart.LineItems.Count((ShoppingCartItemInfo a) => a.IsfreeShipping) && !shoppingCart.IsFreightFree)
            {
                foreach (ShoppingCartItemInfo item in this.shoppingCart.LineItems)
                {
                    if (!hsSupplierId.Contains(item.SupplierId))
                    {
                        hsSupplierId.Add(item.SupplierId);
                    }
                    if (!hsTemplateId.Contains(item.TemplateId))
                    {
                        hsTemplateId.Add(item.TemplateId);
                    }
                }
            }
            return true;
        }
        private OrderInfo GetOrderInfo(ShoppingCartInfo shoppingCartInfo)
        {
            OrderInfo orderInfo = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCartInfo, this.isGroupBuy, this.isCountDown, this.isSignBuy, HiContext.Current.User.UserId);
            if (orderInfo == null)
            {
                return null;
            }

            //去除发票
            //if (this.chkTax.Checked)
            //{
            //    orderInfo.Tax = orderInfo.GetNewTotal() * decimal.Parse(this.litTaxRate.Text) / 100m;
            //    if (this.isBundling)
            //    {
            //        BundlingInfo bundlingInfo = ProductBrowser.GetBundlingInfo(this.bundlingid);
            //        orderInfo.Tax = bundlingInfo.Price * decimal.Parse(this.litTaxRate.Text) / 100m;
            //    }
            //}
            orderInfo.IsCustomsClearance = isCustomsClearance1 ? 1 : 0;
            orderInfo.InvoiceTitle = this.txtInvoiceTitle.Text;
            if (this.isGroupBuy)
            {
                GroupBuyInfo productGroupBuyInfo = ProductBrowser.GetProductGroupBuyInfo(shoppingCartInfo.LineItems[0].ProductId);
                orderInfo.GroupBuyId = productGroupBuyInfo.GroupBuyId;
                orderInfo.NeedPrice = productGroupBuyInfo.NeedPrice;
            }
            if (this.isCountDown)
            {
                CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo(this.shoppingCart.LineItems[0].ProductId);
                orderInfo.CountDownBuyId = countDownInfo.CountDownId;
            }
            if (this.isBundling)
            {
                BundlingInfo bundlingInfo2 = ProductBrowser.GetBundlingInfo(this.bundlingid);
                orderInfo.BundlingID = bundlingInfo2.BundlingID;
                orderInfo.BundlingPrice = bundlingInfo2.Price;
                orderInfo.Points = this.shoppingCart.GetPoint(bundlingInfo2.Price);
            }
            DateTime pCustomsClearanceDate = ShoppingProcessor.GetUserLastOrderPCustomsClearanceDate(orderInfo.UserId);
            orderInfo.PCustomsClearanceDate = pCustomsClearanceDate.AddDays(1);//预清关时间
            orderInfo.OrderId = this.GenerateOrderId();
            orderInfo.OrderDate = System.DateTime.Now;
            orderInfo.PayDate = orderInfo.OrderDate;
            IUser user = HiContext.Current.User;
            orderInfo.UserId = user.UserId;
            orderInfo.Username = user.Username;
            if (!user.IsAnonymous)
            {
                Member member = user as Member;
                orderInfo.EmailAddress = member.Email;
                orderInfo.RealName = member.RealName;
                orderInfo.QQ = member.QQ;
                orderInfo.Wangwang = member.Wangwang;
                orderInfo.MSN = member.MSN;
            }
            orderInfo.Remark = Globals.HtmlEncode(this.txtMessage.Text);
            orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;
            orderInfo.RefundStatus = RefundStatus.None;
            this.FillOrderCoupon(orderInfo);
            this.FillOrderShippingMode(orderInfo, shoppingCartInfo);
            this.FillOrderPaymentMode(orderInfo);
            this.FillOrderVoucher(orderInfo);
            orderInfo.OrderSource = OrderSource.PC;
            return orderInfo;
        }
        private void FillOrderCoupon(OrderInfo orderInfo)
        {
            if (!string.IsNullOrEmpty(this.htmlCouponCode.Value))
            {
                CouponInfo couponInfo = ShoppingProcessor.UseCoupon(System.Convert.ToDecimal(this.lblOrderTotal.Money), this.htmlCouponCode.Value);
                if (couponInfo != null)
                {
                    orderInfo.CouponName = couponInfo.Name;
                    if (couponInfo.Amount.HasValue)
                    {
                        orderInfo.CouponAmount = couponInfo.Amount.Value;
                    }
                    orderInfo.CouponCode = this.htmlCouponCode.Value;
                    orderInfo.CouponValue = couponInfo.DiscountValue;
                }
            }
        }

        private void FillOrderVoucher(OrderInfo orderInfo)
        {
            if (!string.IsNullOrEmpty(this.htmlVoucherCode.Value))
            {
                VoucherInfo voucherInfo = ShoppingProcessor.UseVoucher(System.Convert.ToDecimal(this.lblOrderTotal.Money), this.htmlVoucherCode.Value);
                if (voucherInfo != null)
                {
                    orderInfo.VoucherName = voucherInfo.Name;
                    if (voucherInfo.Amount.HasValue)
                    {
                        orderInfo.VoucherAmount = voucherInfo.Amount.Value;
                    }
                    orderInfo.VoucherCode = this.htmlVoucherCode.Value;
                    orderInfo.VoucherValue = voucherInfo.DiscountValue;
                }
            }
        }


        private void FillOrderShippingMode(OrderInfo orderInfo, ShoppingCartInfo shoppingCartInfo)//调整成海购网的运费方法
        {
            orderInfo.ShippingRegion = this.dropRegions.SelectedRegions;
            orderInfo.Address = Globals.HtmlEncode(this.txtAddress.Text);
            orderInfo.ZipCode = this.txtZipcode.Text;
            orderInfo.ShipTo = Globals.HtmlEncode(this.txtShipTo.Text);//Globals.HtmlEncode(this.txtShipTo1.Text);20160314修改之前
            orderInfo.TelPhone = this.txtTelPhone.Text;
            orderInfo.CellPhone = this.txtCellPhone.Text;
            orderInfo.IdentityCard = this.txtIdentityCard1.Text;
            int shippingId = 0;
            if (!string.IsNullOrEmpty(this.hidShippingId.Value))
            {
                int.TryParse(this.hidShippingId.Value, out shippingId);
            }
            if (!HiContext.Current.User.IsAnonymous && MemberProcessor.GetShippingAddressCount() == 0)//新增收货地址
            {
                shippingId = MemberProcessor.AddShippingAddress(new ShippingAddressInfo
                  {
                      UserId = HiContext.Current.User.UserId,
                      ShipTo = Globals.HtmlEncode(this.txtShipTo.Text),
                      RegionId = this.dropRegions.GetSelectedRegionId().Value,
                      Address = Globals.HtmlEncode(this.txtAddress.Text),
                      Zipcode = this.txtZipcode.Text,
                      CellPhone = this.txtCellPhone.Text,
                      TelPhone = this.txtTelPhone.Text,
                      IdentityCard = ""
                  });
            }
            orderInfo.ShippingId = shippingId;
            //if (!string.IsNullOrEmpty(this.inputShippingModeId.Value))
            //{
            //    orderInfo.ShippingModeId = int.Parse(this.inputShippingModeId.Value, System.Globalization.NumberStyles.None);
            //}
            if (this.dropRegions.GetSelectedRegionId().HasValue)
            {
                orderInfo.RegionId = this.dropRegions.GetSelectedRegionId().Value;
            }
            orderInfo.ShipToDate = this.drpShipToDate.Value;
            ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(orderInfo.ShippingModeId, true);
            if (shippingMode != null)
            {
                orderInfo.ShippingModeId = shippingMode.ModeId;
                orderInfo.ModeName = shippingMode.Name;
                //if (!shoppingCartInfo.IsFreightFree)
                //{
                //    if (shoppingCartInfo.LineItems.Count((ShoppingCartItemInfo a) => !a.IsfreeShipping) > 0 || shoppingCartInfo.LineGifts.Count > 0)
                //    {
                //        orderInfo.AdjustedFreight = (orderInfo.Freight = ShoppingProcessor.CalcFreight(orderInfo.RegionId, shoppingCartInfo.Weight, shippingMode));
                //        return;
                //    }
                //}
                //orderInfo.AdjustedFreight = (orderInfo.Freight = 0m);
            }
            //
            decimal tax = 0;
            decimal freight = 0;
            Dictionary<int, decimal> dictShippingMode = new Dictionary<int, decimal>();
            if (shoppingCartInfo.LineItems.Count != shoppingCartInfo.LineItems.Count((ShoppingCartItemInfo a) => a.IsfreeShipping) && !shoppingCartInfo.IsFreightFree)
            {
                foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                {
                    //tax += item.AdjustedPrice * item.TaxRate * item.Quantity;
                    if ((!item.IsfreeShipping))// ||!flag
                    {
                        if (item.TemplateId > 0)
                        {
                            if (dictShippingMode.ContainsKey(item.TemplateId))
                            {
                                dictShippingMode[item.TemplateId] += item.Weight * item.Quantity;
                            }
                            else
                            {
                                dictShippingMode.Add(item.TemplateId, item.Weight * item.Quantity);
                            }
                        }
                    }
                    if (orderInfo.LineItems.ContainsKey(item.SkuId))
                    {
                        orderInfo.LineItems[item.SkuId].TaxRate = item.TaxRate;
                    }
                }
                ShippingAddressInfo memberDefaultShippingAddressInfo = MemberProcessor.GetShippingAddress(shippingId);
                freight = ShoppingCartProcessor.GetFreight(shoppingCart, memberDefaultShippingAddressInfo, false);
                orderInfo.AdjustedFreight = (orderInfo.Freight = freight);
                //orderInfo.Tax = tax <= 50 ? 0 : tax;
                //orderInfo.OriginalTax = tax;
            }
            else
            {
                orderInfo.AdjustedFreight = (orderInfo.Freight = 0m);
            }


            #region 计算税费
            //foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
            //{

            //    tax += item.AdjustedPrice * item.TaxRate * item.Quantity;
            //}

            tax = shoppingCartInfo.GetNewTotalTax();
            orderInfo.Tax = tax <= 50 ? 0 : tax;
            orderInfo.OriginalTax = tax;
            #endregion 
        }
        //private void FillOrderShippingMode1(OrderInfo orderInfo, ShoppingCartInfo shoppingCartInfo)//原方法
        //{
        //    orderInfo.ShippingRegion = this.dropRegions.SelectedRegions;
        //    orderInfo.Address = Globals.HtmlEncode(this.txtAddress.Text);
        //    orderInfo.ZipCode = this.txtZipcode.Text;
        //    orderInfo.ShipTo = Globals.HtmlEncode(this.txtShipTo.Text);
        //    orderInfo.TelPhone = this.txtTelPhone.Text;
        //    orderInfo.CellPhone = this.txtCellPhone.Text;
        //    orderInfo.IdentityCard = this.txtIdentityCard.Text;
        //    if (!string.IsNullOrEmpty(this.inputShippingModeId.Value))
        //    {
        //        orderInfo.ShippingModeId = int.Parse(this.inputShippingModeId.Value, System.Globalization.NumberStyles.None);
        //    }
        //    if (this.dropRegions.GetSelectedRegionId().HasValue)
        //    {
        //        orderInfo.RegionId = this.dropRegions.GetSelectedRegionId().Value;
        //    }
        //    orderInfo.ShipToDate = this.drpShipToDate.Value;
        //    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(orderInfo.ShippingModeId, true);
        //    if (shippingMode != null)
        //    {
        //        orderInfo.ShippingModeId = shippingMode.ModeId;
        //        orderInfo.ModeName = shippingMode.Name;
        //        if (!shoppingCartInfo.IsFreightFree)
        //        {
        //            if (shoppingCartInfo.LineItems.Count((ShoppingCartItemInfo a) => !a.IsfreeShipping) > 0 || shoppingCartInfo.LineGifts.Count > 0)
        //            {
        //                orderInfo.AdjustedFreight = (orderInfo.Freight = ShoppingProcessor.CalcFreight(orderInfo.RegionId, shoppingCartInfo.Weight, shippingMode));
        //                return;
        //            }
        //        }
        //        orderInfo.AdjustedFreight = (orderInfo.Freight = 0m);
        //    }
        //}
        private void FillOrderPaymentMode(OrderInfo orderInfo)
        {
            orderInfo.PaymentTypeId = int.Parse(this.inputPaymentModeId.Value);
            PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentModeByModeId(orderInfo.PaymentTypeId);
            if (paymentMode != null)
            {
                orderInfo.PaymentType = Globals.HtmlEncode(paymentMode.Name);
                orderInfo.PayCharge = ShoppingProcessor.CalcPayCharge(orderInfo.GetNewTotal(), paymentMode);
                orderInfo.Gateway = paymentMode.Gateway;
            }
        }
        private string GenerateOrderId()
        {
            string text = string.Empty;
            System.Random random = new System.Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                text += ((char)(48 + (ushort)(num % 10))).ToString();
            }
            return System.DateTime.Now.ToString("yyyyMMdd") + text;
        }
    }
}



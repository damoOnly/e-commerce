using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Store;
﻿using EcShop.ControlPanel.Comments;
using EcShop.ControlPanel.Sales;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Comments;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Comments;
using EcShop.SaleSystem.Member;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.Entities.Sales;
using EcShop.SaleSystem.Shopping;
using EcShop.Entities.Commodities;
using EcShop.Entities.Promotions;
using System.Web;


namespace EcShop.UI.Web.API
{
    public class WapProcess : System.Web.IHttpHandler
    {

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(System.Web.HttpContext context)
        {
            string action = context.Request["action"];
            switch (action)
            {
                case "LoadMoreFav":
                    this.LoadMoreFav(context);
                    break;

                case "DelHistorySearch":
                    this.DelHistorySearch(context);
                    break;

                case "GetOtherHotSearch":
                    this.GetOtherHotSearch(context);
                    break;
                case "GetTopUpRecords":
                    this.GetTopUpRecords(context);
                    break;
                case "GetBalanceDrawRequest":
                    GetBalanceDrawRequest(context);
                    break;

                //根据商品，订单，skuid,用户获取评论
                case "GetOneProductReview":
                    this.GetOneProductReview(context);
                    break;

                case "AddProductReview":
                    this.AddProductReview(context);
                    break;

                //添加反馈
                case "AddFeedback":
                    this.AddFeedback(context);
                    break;

                case "SplittinDraws":
                    SplittinDraws(context);
                    break;

                case "LoadMoreMessage":
                    LoadMoreMessage(context);
                    break;

                case "RegisterUser":
                    RegisterUser(context);
                    break;
                case "Submmitorder":
                    this.ProcessSubmmitorder(context);
                    break;

                case "LoadMoreProducts":
                    LoadMoreProducts(context);
                    break;

                case "LoadMoreGroupProduct":
                    LoadMoreGroupProduct(context);
                    break;

                case "MyConsultations":
                    MyConsultations(context);
                    break;
            }

        }
        private void ProcessSubmmitorder(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");

            int num = 0;
            int siteId = 0;
            int.TryParse(context.Request["siteId"], out siteId);
            int shippingId = int.Parse(context.Request["shippingId"]);
            OrderSource orderSource = (OrderSource)int.Parse(context.Request["orderSource"]);
            string couponCode = context.Request["couponCode"];
            string selectedVoucherCode = context.Request["SelVoucherCode"];
            string inputVoucherCode = context.Request["VoucherCode"];
            string inputVoucherPwd = context.Request["VoucherPwd"];
            int num2;
            bool flag = int.TryParse(context.Request["groupbuyId"], out num2);
            string remark = context.Request["remark"];
            string realName = context.Request["txtRealName"];
            string a2 = "";
            if (!string.IsNullOrEmpty(context.Request["from"]))
            {
                a2 = context.Request["from"].ToString().ToLower();
            }
            bool unpackOrder = context.Request["unpack"] == "1";
            bool mergeOrder = context.Request["merge"] == "1";
            string identityCard = context.Request["identityCard"];//身份证号码


            #region  获取购物车信息
            int buyAmount;
            ShoppingCartInfo shoppingCartInfo;
            if (int.TryParse(context.Request["buyAmount"], out buyAmount) && !string.IsNullOrEmpty(context.Request["productSku"]) && (a2 == "signbuy" || a2 == "groupbuy"))
            {
                string productSkuId = context.Request["productSku"];
                int storeId = 0;
                int.TryParse(context.Request["storeId"], out storeId);
                if (a2 == "signbuy")
                {
                    shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(productSkuId, buyAmount, storeId);
                }
                else
                {
                    shoppingCartInfo = ShoppingCartProcessor.GetGroupBuyShoppingCart(productSkuId, buyAmount, storeId);
                }
            }
            else
            {
                HttpCookie cookieSkuIds = context.Request.Cookies["UserSession-SkuIds"];
                if (cookieSkuIds != null && !string.IsNullOrWhiteSpace(cookieSkuIds.Value))
                {
                    shoppingCartInfo = ShoppingCartProcessor.GetPartShoppingCartInfo(Globals.UrlDecode(cookieSkuIds.Value));//获取用户选择的商品
                }
                else
                {
                    shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart();
                }
                // shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart();
            }

            #endregion


            //根据购物车填充订单
            OrderInfo orderInfo = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCartInfo, false, false, false, HiContext.Current.User.UserId);
            if (orderInfo != null)
            {

                #region 填充部分订单信息
                orderInfo.OrderId = this.GenerateOrderId();
                orderInfo.OrderDate = System.DateTime.Now;
                orderInfo.OrderSource = orderSource;
                Member member = HiContext.Current.User as Member;
                orderInfo.UserId = member.UserId;
                orderInfo.Username = member.Username;
                orderInfo.EmailAddress = member.Email;
                orderInfo.RealName = member.RealName;
                orderInfo.QQ = member.QQ;
                orderInfo.Remark = remark;
                orderInfo.RealName = realName;
                orderInfo.SiteId = siteId;
                orderInfo.IdentityCard = identityCard;//收货人身份证号码

                if (flag)
                {
                    GroupBuyInfo groupBuy = ProductBrowser.GetGroupBuy(num2);
                    orderInfo.GroupBuyId = num2;
                    orderInfo.NeedPrice = groupBuy.NeedPrice;
                    orderInfo.GroupBuyStatus = groupBuy.Status;
                }
                orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;
                orderInfo.RefundStatus = RefundStatus.None;
                orderInfo.ShipToDate = context.Request["shiptoDate"];
                orderInfo.ShippingModeId = -1;
                orderInfo.ModeName = "";
                #endregion 

                #region 验证库存
                int mayCount = 0;
                foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                {
                    mayCount += item.Quantity;
                    int stock = ShoppingProcessor.GetProductStock(item.SkuId);
                    if (stock <= 0 || stock < item.Quantity)
                    {
                        stringBuilder.Append("\"Status\":\"Error\"");
                        stringBuilder.Append(",\"ErrorMsg\":\"商品 ");
                        stringBuilder.Append(item.Name.Replace("\r\n", ""));
                        stringBuilder.Append(" 库存不足\"}");
                        context.Response.Write(stringBuilder.ToString());
                        context.Response.End();
                    }
                }
                #endregion

                #region  收货地址
                ShippingAddressInfo shippingAddress = EcShop.SaleSystem.Member.MemberProcessor.GetShippingAddress(shippingId);
                if (shippingAddress != null)
                {
                    #region 验证是否符合清关条件
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < shoppingCartInfo.LineItems.Count; i++)
                    {
                        if (i == (shoppingCartInfo.LineItems.Count - 1))
                        {
                            sb.Append(shoppingCartInfo.LineItems[i].ProductId);
                        }
                        else
                        {
                            sb.AppendFormat("{0},", shoppingCartInfo.LineItems[i].ProductId);
                        }
                    }
                    bool b = ShoppingProcessor.CheckIsCustomsClearance(sb.ToString());
                    if (b)
                    {
                        orderInfo.IsCustomsClearance = 1;
                        if (string.IsNullOrEmpty(identityCard))
                        {
                            stringBuilder.Append("\"Status\":\"Error\"");
                            stringBuilder.Append(",\"ErrorMsg\":\"有需要清关的商品，身份证号码不能为空\"}");
                            context.Response.Write(stringBuilder.ToString());
                            context.Response.End();
                        }
                    }
                    else
                    {
                        orderInfo.IsCustomsClearance = 0;
                    }


                    #endregion

                    orderInfo.ShippingRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, "，");
                    orderInfo.RegionId = shippingAddress.RegionId;
                    orderInfo.Address = shippingAddress.Address;
                    orderInfo.ZipCode = shippingAddress.Zipcode;
                    orderInfo.ShipTo = shippingAddress.ShipTo;
                    orderInfo.TelPhone = shippingAddress.TelPhone;
                    orderInfo.CellPhone = shippingAddress.CellPhone;
                    orderInfo.ShippingId = shippingAddress.ShippingId;
                    EcShop.SaleSystem.Member.MemberProcessor.SetDefaultShippingAddress(shippingId, HiContext.Current.User.UserId);
                    if (shippingAddress.IdentityCard != identityCard)
                    {
                        shippingAddress.IdentityCard = identityCard;
                        EcShop.SaleSystem.Member.MemberProcessor.UpdateShippingAddress(shippingAddress);
                    }
                }
                else
                {
                    stringBuilder.Append("\"Status\":\"Error\"");
                    stringBuilder.Append(",\"ErrorMsg\":\"收货地址不能为空\"}");
                    context.Response.Write(stringBuilder.ToString());
                    context.Response.End();

                }

                #endregion

                #region  获取商品供应商集合和运费模版集合
                decimal tax = 0m;
                decimal freight = 0m;
                int totalGoodsCount = 0;
                HashSet<int> hsSupplierId = new HashSet<int>();
                HashSet<int> hsTemplateId = new HashSet<int>();
                Dictionary<int, decimal> dictShippingMode = new Dictionary<int, decimal>();
                if (shoppingCartInfo.LineItems.Count != shoppingCartInfo.LineItems.Count((ShoppingCartItemInfo a) => a.IsfreeShipping) && !shoppingCartInfo.IsFreightFree)
                {
                    foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                    {
                        totalGoodsCount += item.Quantity;
                        tax += item.AdjustedPrice * item.TaxRate * item.Quantity;
                        if ((!item.IsfreeShipping || !flag))
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
                        hsSupplierId.Add(item.SupplierId);//商品供应商集合
                        hsTemplateId.Add(item.TemplateId);//运费模版Id
                    }
                    foreach (var item in dictShippingMode)
                    {
                        ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(item.Key);
                        freight += ShoppingProcessor.CalcFreight(shippingAddress.RegionId, item.Value, shippingMode);
                    }
                    orderInfo.AdjustedFreight = (orderInfo.Freight = freight);
                    orderInfo.Tax = tax <= 50 ? 0 : tax;
                    orderInfo.OriginalTax = tax;
                }
                else
                {
                    orderInfo.AdjustedFreight = (orderInfo.Freight = 0m);
                }

                //数量限制
                if (totalGoodsCount > 30)
                {
                    stringBuilder.Append("\"Status\":\"Error\"");
                    stringBuilder.Append(",\"ErrorMsg\":\"购买数量不能超过30个\"}");
                    context.Response.Write(stringBuilder.ToString());
                    context.Response.End();
                }
                #endregion

                #region 获取付款方式
                if (int.TryParse(context.Request["paymentType"], out num))
                {
                    orderInfo.PaymentTypeId = num;
                    if (num == 0)
                    {
                        orderInfo.PaymentType = "货到付款";
                        orderInfo.Gateway = "Ecdev.plugins.payment.podrequest";
                    }
                    else
                    {
                        if (num == -2)
                        {
                            orderInfo.PaymentType = "微信支付";
                            orderInfo.Gateway = "Ecdev.plugins.payment.weixinrequest";
                        }
                        else
                        {
                            if (num == -1)
                            {
                                orderInfo.PaymentType = "线下付款";
                                orderInfo.Gateway = "Ecdev.plugins.payment.bankrequest";
                            }
                            else
                            {
                                if (num == -3)
                                {
                                    orderInfo.PaymentType = "支付宝手机应用内支付";
                                    orderInfo.Gateway = "Ecdev.plugins.payment.ws_apppay.wswappayrequest";
                                }
                                else
                                {
                                    if (num == -4)
                                    {
                                        orderInfo.PaymentType = "支付宝手机网页支付";
                                        orderInfo.Gateway = "Ecdev.plugins.payment.ws_wappay.wswappayrequest";
                                    }
                                    else
                                    {
                                        if (num == -5)
                                        {
                                            orderInfo.PaymentType = "盛付通手机网页支付";
                                            orderInfo.Gateway = "Ecdev.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest";
                                        }
                                        else
                                        {
                                            if (num == -6)
                                            {
                                                orderInfo.PaymentType = "预付款帐户支付";
                                                orderInfo.Gateway = "Ecdev.plugins.payment.advancerequest";
                                            }
                                            else
                                            {
                                                PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(num);
                                                if (paymentMode != null)
                                                {
                                                    orderInfo.PaymentTypeId = paymentMode.ModeId;
                                                    orderInfo.PaymentType = paymentMode.Name;
                                                    orderInfo.Gateway = paymentMode.Gateway;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion


                #region 处理现金券和优惠券
                decimal totalProductAmount = shoppingCartInfo.GetNewTotal();
                if (!string.IsNullOrEmpty(couponCode))
                {
                    CouponInfo couponInfo = ShoppingProcessor.UseCoupon(totalProductAmount, couponCode);
                    orderInfo.CouponName = couponInfo.Name;
                    if (couponInfo.Amount.HasValue)
                    {
                        orderInfo.CouponAmount = couponInfo.Amount.Value;
                    }
                    orderInfo.CouponCode = couponCode;
                    orderInfo.CouponValue = couponInfo.DiscountValue;
                }
                CalculateVoucherToOrder(selectedVoucherCode, totalProductAmount, inputVoucherCode, inputVoucherPwd, orderInfo);
                #endregion

                #region 创建订单
                try
                {
                    string firstOrderId = orderInfo.OrderId;
                    orderInfo.OrderType = (int)OrderType.Normal;

                    if (unpackOrder)
                    {
                        orderInfo.OrderType = (int)OrderType.WillSplit;
                    }
                    if (mergeOrder)
                    {
                        orderInfo.OrderType = (int)OrderType.WillMerge;
                    }
                    DateTime pCustomsClearanceDate = ShoppingProcessor.GetUserLastOrderPCustomsClearanceDate(orderInfo.UserId);
                    bool b = false;
                    if (ShoppingProcessor.CreateOrder(orderInfo, true, true))
                    {

                        #region 先拆开，需要用到金额

                        if (hsSupplierId.Count > 1 || hsTemplateId.Count > 1 || (unpackOrder && orderInfo.OriginalTax > 50 && totalGoodsCount > 1))
                        {
                            decimal unpackedTaxTotal = 0;
                            decimal unpackedOrderTotal = 0;
                            decimal unpackedFreight = 0;
                            b = ShoppingProcessor.UnpackOrderBySupplier(orderInfo, ref unpackedTaxTotal, ref unpackedOrderTotal, ref unpackedFreight);
                            if (b)//修改原订单金额
                            {
                                ShoppingProcessor.UpdateWillSplitOrder(firstOrderId, unpackedTaxTotal, unpackedOrderTotal, unpackedFreight);
                                Messenger.OrderCreated(ShoppingProcessor.GetOrderInfo(firstOrderId), HiContext.Current.User);
                            }
                        }
                        if (!b)
                        {
                            Messenger.OrderCreated(orderInfo, HiContext.Current.User);
                        }
                        #endregion

                        #region 积分处理
                        int totalNeedPoint = shoppingCartInfo.GetTotalNeedPoint();

                        if (totalNeedPoint > 0)
                        {
                            ShoppingProcessor.CutNeedPoint(totalNeedPoint, orderInfo.OrderId);
                        }
                        #endregion


                        #region 有选择删除购物车商品
                        HttpCookie cookieSkuIds = context.Request.Cookies["UserSession-SkuIds"];
                        if (cookieSkuIds == null || string.IsNullOrWhiteSpace(cookieSkuIds.Value))
                        {
                            ShoppingCartProcessor.ClearShoppingCart();
                        }
                        else
                        {
                            ShoppingCartProcessor.ClearPartShoppingCart(Globals.UrlDecode(cookieSkuIds.Value));
                            cookieSkuIds.Expires = DateTime.Now.AddDays(-1);
                            context.Response.AppendCookie(cookieSkuIds);
                        }
                        #endregion 

                        stringBuilder.Append("\"Status\":\"OK\",");
                        if (num == -6)
                        {
                            stringBuilder.Append("\"paymentType\":\"NO\",");
                        }
                        else
                        {
                            stringBuilder.Append("\"paymentType\":\"OK\",");
                        }
                        stringBuilder.AppendFormat("\"OrderId\":\"{0}\"", firstOrderId);
                    }
                    else
                    {
                        stringBuilder.Append("\"Status\":\"Error\"");
                    }
                    stringBuilder.Append("}");
                }
                catch (EcShop.SaleSystem.Vshop.OrderException ex)
                {
                    stringBuilder.Append("\"Status\":\"Error\"");
                    stringBuilder.AppendFormat(",\"ErrorMsg\":\"{0}\"", ex.Message);
                    stringBuilder.Append("}");
                }

                #endregion 
            }

            else
            {
                stringBuilder.Append("\"Status\":\"None\"");
                stringBuilder.Append("}");
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(stringBuilder.ToString());
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
        private static void CalculateVoucherToOrder(string selectedVoucherCode, decimal totalProductAmount, string inputVoucherCode, string inputVoucherPwd, OrderInfo orderInfo)
        {
            string voucherCode = string.Empty;
            VoucherInfo voucherInfo = null;
            if (!string.IsNullOrEmpty(selectedVoucherCode))
            {
                voucherCode = selectedVoucherCode;
                voucherInfo = ShoppingProcessor.UseVoucher(totalProductAmount, voucherCode);
            }
            else if (!string.IsNullOrEmpty(inputVoucherCode)) // 根据输入的现金券号码和密码获取现金券
            {
                voucherInfo = ShoppingProcessor.GetVoucherByItem(inputVoucherCode, inputVoucherPwd,
                    totalProductAmount);
                if (null != voucherInfo)
                {
                    voucherCode = inputVoucherCode;
                }
            }
            if (null != voucherInfo)
            {
                orderInfo.VoucherName = voucherInfo.Name;
                if (voucherInfo.Amount.HasValue)
                {
                    orderInfo.VoucherAmount = voucherInfo.Amount.Value;
                }
                orderInfo.VoucherCode = voucherCode;
                orderInfo.VoucherValue = voucherInfo.DiscountValue;
            }
        }
        /// <summary>
        /// 申请提现操作
        /// </summary>
        /// <param name="context"></param>
        public void SplittinDraws(System.Web.HttpContext context)
        {
            //data.Amount = amount;
            //data.Account = account;
            //data.TradePassword = tradePassword;
            //data.BankName = bankName;
            Member member = HiContext.Current.User as Member;
            if (member.RequestBalance > 0m)
            {
                context.Response.Write("{SUCCESS:0,MSG:\"上笔提现管理员还没有处理，只有处理完后才能再次申请提现！\"}");
                context.Response.End();
            }
            decimal amount;
            if (!decimal.TryParse(context.Request.Form["Amount"], out amount))
            {
                context.Response.Write("{SUCCESS:0,MSG:\"金额填写错误！\"}");
                context.Response.End();
            }
            string bankName = context.Request.Form["BankName"];
            if (string.IsNullOrWhiteSpace(bankName))
            {
                context.Response.Write("{SUCCESS:0,MSG:\"请选择银行！\"}");
                context.Response.End();
            }
            //判断银行
            switch (bankName)
            {
                case "NOBANK":
                case "ICBC":
                case "ABC":
                case "BOCSH":
                case "CCB":
                case "CMB":
                case "SPDB":
                case "GDB":
                case "BOCOM":
                case "CMBC":
                case "CIB":
                case "CEB":
                case "HXB":
                case "BOS":
                case "SRCB":
                case "PSBC":
                case "BCCB":
                case "BRCB":
                case "PAB":
                    break;
                default:
                    context.Response.Write("{SUCCESS:0,MSG:\"银行选择错误！\"}");
                    context.Response.End();
                    break;
            }
            string account = context.Request.Form["Account"];
            if (string.IsNullOrWhiteSpace(account))
            {
                context.Response.Write("{SUCCESS:0,MSG:\"请填写卡号！\"}");
                context.Response.End();
            }
            string accountName = context.Request.Form["AccountName"];
            if (string.IsNullOrWhiteSpace(accountName))
            {
                context.Response.Write("{SUCCESS:0,MSG:\"请填写开户人姓名！\"}");
                context.Response.End();
            }
            BalanceDrawRequestInfo balanceDrawRequest = new BalanceDrawRequestInfo() { Remark = "", Amount = amount, BankName = bankName, UserId = HiContext.Current.User.UserId, UserName = HiContext.Current.User.Username, MerchantCode = account, RequestTime = DateTime.Now, AccountName = accountName };
            if (MemberProcessor.BalanceDrawRequest(balanceDrawRequest))
            {
                context.Response.Write("{SUCCESS:1}");
                context.Response.End();
            }
            else
            {
                context.Response.Write("{SUCCESS:0,MSG:\"操作失败！请稍后重试！\"}");
                context.Response.End();
            }
        }

        /// <summary>
        /// 获取提现/充值记录
        /// </summary>
        /// <param name="context"></param>
        private void GetBalanceDrawRequest(System.Web.HttpContext context)
        {
            int pageIndex;
            int pageSize;
            if (!int.TryParse(context.Request.QueryString["pageIndex"], out pageIndex))
            {
                context.Response.Write("{SUCCESS:0}");
                context.Response.End();
            }
            if (!int.TryParse(context.Request.QueryString["pageSize"], out pageSize))
            {
                context.Response.Write("{SUCCESS:0}");
                context.Response.End();
            }
            int tradeType;
            if (!int.TryParse(context.Request.QueryString["TradeType"], out tradeType))
            {
                context.Response.Write("{SUCCESS:0}");
                context.Response.End();
            }
            BalanceDetailQuery balanceDetailQuery = new BalanceDetailQuery() { PageIndex = pageIndex, PageSize = pageSize, UserId = new int?(HiContext.Current.User.UserId), TradeType = (TradeTypes)tradeType };
            DbQueryResult balanceDetails = MemberProcessor.GetBalanceDetails(balanceDetailQuery);
            //DbQueryResult balanceDrawRequests = MemberHelper.GetBalanceDrawRequests(new BalanceDrawRequestQuery
            //{
            //    UserId =  HiContext.Current.User.UserId,
            //    PageIndex = pageIndex,
            //    PageSize = pageSize
            //});
            if ((balanceDetails.Data as DataTable).Rows.Count == 0)
            {
                context.Response.Write("{SUCCESS:1}");
                context.Response.End();
            }
            foreach (DataRow r in (balanceDetails.Data as DataTable).Rows)
            {
                string[] str = r["Remark"] != null ? r["Remark"].ToString().Split('~') : null;
                if (str.Length >= 3)
                {
                    if (str[2].Length > 5)
                        r["Remark"] = str[2].Substring(0, 4) + "****" + str[2].Substring(str[2].Length - 4, 4);
                    else
                    {
                        r["Remark"] = str[2] + "****";
                    }
                }
                else
                {
                    r["Remark"] = "**********";
                }
            }
            string strProducts = Newtonsoft.Json.JsonConvert.SerializeObject(balanceDetails.Data);
            context.Response.Write("{SUCCESS:2,data:" + strProducts + "}");
            context.Response.End();
        }
        /// <summary>
        /// 获取当前用户充值记录
        /// </summary>
        /// <param name="context"></param>
        private void GetTopUpRecords(System.Web.HttpContext context)
        {
            int userId = HiContext.Current.User.UserId;
            int pageIndex;
            int pageSize;
            if (int.TryParse(context.Request.QueryString["pageIndex"], out pageIndex))
            {
                context.Response.Write("{SUCCESS:0}");
                context.Response.End();
            }
            if (int.TryParse(context.Request.QueryString["pageSize"], out pageSize))
            {
                context.Response.Write("{SUCCESS:0}");
                context.Response.End();
            }
            DbQueryResult mySplittinDraws = MemberProcessor.GetMySplittinDraws(new BalanceDrawRequestQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                UserId = new int?(HiContext.Current.User.UserId)
            }, null);
            string strProducts = Newtonsoft.Json.JsonConvert.SerializeObject(mySplittinDraws.Data);
            if (pageIndex == 0 && string.IsNullOrWhiteSpace(strProducts == "[]" ? "" : strProducts))
            {
                context.Response.Write("{SUCCESS:1}");
                context.Response.End();
            }
            context.Response.Write("{SUCCESS:2,data:" + strProducts + "}");
            context.Response.End();
        }


        /// <summary>
        /// 加载更多收藏商品
        /// </summary>
        /// <param name="context"></param>
        public void LoadMoreFav(System.Web.HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Pagination page = new Pagination();

            page.SortBy = "FavoriteId";
            page.SortOrder = SortAction.Desc;
            page.PageIndex = int.Parse(context.Request["pageNumber"]);
            page.PageSize = int.Parse(context.Request["size"]);

            Globals.EntityCoding(page, true);
            DataTable dt = (DataTable)ProductBrowser.GetProductFavorites(page).Data;
            stringBuilder.Append("{");
            if (dt.Rows.Count > 0)
            {
                stringBuilder.Append("\"Success\":1,");
                stringBuilder.Append("\"favs\":");
                string strFavs = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                stringBuilder.Append(strFavs);
                stringBuilder.Append("}");
            }
            else
            {
                stringBuilder.Append("\"Success\":0");
                stringBuilder.Append("}");
            }

            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }

        /// <summary>
        /// 删除历史搜索
        /// </summary>
        /// <param name="context"></param>
        public void DelHistorySearch(System.Web.HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int userId = HiContext.Current.User.UserId;
            if (userId > 0)
            {
                int amount = HistorySearchHelp.DeleteSearchHistory(userId, ClientType.WAP);
                if (amount > 0)
                {
                    stringBuilder.Append("{\"Success\":1}");
                }
                else
                {
                    stringBuilder.Append("{\"Success\":0}");
                }
            }
            else
            {
                stringBuilder.Append("{\"Success\":0}");
            }

            context.Response.Write(stringBuilder.ToString());
            context.Response.End();

        }


        /// <summary>
        /// 换一换热门搜索
        /// </summary>
        /// <param name="context"></param>
        public void GetOtherHotSearch(System.Web.HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Pagination page = new Pagination();
            page.SortBy = "SearchTime";
            page.SortOrder = SortAction.Desc;
            page.PageIndex = int.Parse(context.Request["pageIndex"]);
            page.PageSize = int.Parse(context.Request["pageSize"]);
            Globals.EntityCoding(page, true);
            DbQueryResult dq = StoreHelper.GetHotKeywords(ClientType.WAP, page);
            DataTable dt = (DataTable)dq.Data;
            stringBuilder.Append("{");
            if (dt.Rows.Count > 0)
            {
                stringBuilder.Append("\"Success\":1,");
                stringBuilder.Append("\"hotsearch\":");
                string strHotsearch = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                stringBuilder.Append(strHotsearch);
                stringBuilder.AppendFormat(",\"total\":{0}", dq.TotalRecords);
                stringBuilder.Append("}");
            }
            else
            {
                stringBuilder.Append("\"Success\":0");
                stringBuilder.Append("}");
            }

            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }

        /// <summary>
        /// 获取一个商品评论
        /// </summary>
        /// <param name="context"></param>

        public void GetOneProductReview(System.Web.HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int productid = int.Parse(context.Request["productid"]);
            string skuid = context.Request["skuid"].ToString();
            string orderid = context.Request["orderid"].ToString();
            int userid = HiContext.Current.User.UserId;
            ProductReviewInfo info = ProductCommentHelper.GetProductReview(productid, skuid, orderid, userid);
            stringBuilder.Append("{");
            if (info.ReviewId != 0)
            {
                stringBuilder.Append("\"Success\":1,");
                stringBuilder.Append("\"ReviewInfo\":");
                string strReviewInfo = Newtonsoft.Json.JsonConvert.SerializeObject(info);
                stringBuilder.Append(strReviewInfo);
                stringBuilder.Append("}");
            }

            else
            {
                stringBuilder.Append("\"Success\":0");
                stringBuilder.Append("}");
            }

            context.Response.Write(stringBuilder.ToString());
            context.Response.End();

        }


        /// <summary>
        /// 添加商品评论
        /// </summary>
        /// <param name="context"></param>
        private void AddProductReview(System.Web.HttpContext context)
        {
            Member member = HiContext.Current.User as Member;
            int productId = 0;
            if (!int.TryParse((context.Request["ProductId"] == null) ? "0" : context.Request["ProductId"].ToString(), out productId))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"错误的商品ID，因此不能进行评论\"}");
                return;
            }

            int score = 0;
            if (!int.TryParse((context.Request["Score"] == null) ? "0" : context.Request["Score"].ToString(), out score))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"还未进行评级\"}");
                return;
            }


            string text = (context.Request["OrderId"] == null) ? "" : context.Request["OrderId"].ToString();
            if (string.IsNullOrEmpty(text))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"错误的订单ID，因此不能进行评论\"}");
                return;
            }
            string text2 = (context.Request["SkuId"] == null) ? "" : context.Request["SkuId"].ToString();
            text2 = Globals.StripAllTags(text2);
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
            if (orderInfo.OrderStatus != OrderStatus.Finished)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"您的订单还未完成，因此不能对该商品进行评论\"}");
                return;
            }
            int num;
            int num2;
            ProductBrowser.LoadProductReview(productId, out num, out num2, text);
            if (num == 0)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"您没有购买此商品(或此商品的订单尚未完成)，因此不能进行评论\"}");
                return;
            }
            if (num2 >= num)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"您已经对此商品进行过评论(或此商品的订单尚未完成)，因此不能再次进行评论\"}");
                return;
            }

            int isAnonymous;
            bool flag;
            if (!int.TryParse((context.Request["IsAnonymous"] == null) ? "0" : context.Request["IsAnonymous"].ToString(), out isAnonymous))
            {
                flag = false;
            }
            else
            {
                if (isAnonymous == 1)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }


            if (ProductBrowser.InsertProductReview(new ProductReviewInfo
            {
                ReviewDate = System.DateTime.Now,
                ReviewText = context.Request["ReviewText"],
                ProductId = productId,
                UserEmail = member.Email,
                UserId = member.UserId,
                UserName = member.Username,
                OrderID = text,
                SkuId = text2,
                Score = score,
                IsAnonymous = flag
            }))
            {
                context.Response.Write("{\"success\":true}");
                return;
            }
            context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
        }


        /// <summary>
        /// 添加反馈
        /// </summary>
        /// <param name="context"></param>
        private void AddFeedback(System.Web.HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string content = context.Request["content"].ToString();
            string contact = context.Request["contact"].ToString();
            string title = context.Request["title"].ToString();
            int feedbacktype = int.Parse(context.Request["FeedbackType"].ToString());

            int useid = HiContext.Current.User.UserId;

            LeaveCommentInfo leaveCommentInfo = new LeaveCommentInfo();
            leaveCommentInfo.UserName = HiContext.Current.User.Username;
            leaveCommentInfo.UserId = new int?(HiContext.Current.User.UserId);
            //leaveCommentInfo.Title = useid.ToString() + "date=" + DateTime.Now.ToString();
            leaveCommentInfo.Title = title;
            leaveCommentInfo.PublishContent = content;
            leaveCommentInfo.ContactWay = contact;
            leaveCommentInfo.FeedbackType = feedbacktype;
            if (CommentBrowser.InsertLeaveComment(leaveCommentInfo))
            {
                stringBuilder.Append("{\"Success\":1}");
            }

            else
            {
                stringBuilder.Append("{\"Success\":0}");
            }

            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }


        /// <summary>
        /// 加载更多消息
        /// </summary>
        /// <param name="context"></param>
        public void LoadMoreMessage(System.Web.HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            MessageBoxQuery mxQuery = new MessageBoxQuery();
            mxQuery.Accepter = HiContext.Current.User.Username;
            mxQuery.PageIndex = int.Parse(context.Request["pageNumber"]);
            mxQuery.PageSize = int.Parse(context.Request["size"]);

            DataTable dt = (DataTable)CommentBrowser.GetMemberReceivedMessages(mxQuery).Data;
            stringBuilder.Append("{");
            if (dt.Rows.Count > 0)
            {
                stringBuilder.Append("\"Success\":1,");
                stringBuilder.Append("\"mes\":");
                IsoDateTimeConverter convert = new IsoDateTimeConverter();
                convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                string strMes = Newtonsoft.Json.JsonConvert.SerializeObject(dt, Formatting.None, convert);
                stringBuilder.Append(strMes);
                stringBuilder.Append("}");
            }
            else
            {
                stringBuilder.Append("\"Success\":0");
                stringBuilder.Append("}");
            }

            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="context"></param>
        public void RegisterUser(System.Web.HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string userName = context.Request["userName"];
            string telphone = context.Request["telphone"];
            string password = context.Request["password"];
            string checkcode = context.Request["checkcode"];
            if (string.IsNullOrEmpty(checkcode))
            {
                stringBuilder.Append("{\"success\":false,\"msg\":\"手机验证码不允许为空\"}");

            }

            else if (!TelVerifyHelper.CheckVerify(telphone, checkcode))
            {
                stringBuilder.Append("{\"success\":false,\"msg\":\"手机验证码验证错误\"}");
            }

            else if (string.IsNullOrEmpty(telphone))
            {
                stringBuilder.Append("{\"success\":false,\"msg\":\"手机号码不允许为空\"}");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(telphone, "^(13|14|15|17|18)\\d{9}$"))
            {
                stringBuilder.Append("{\"success\":false,\"msg\":\"请输入正确的手机号码\"}");
            }
            else if (UserHelper.IsExistCellPhone(telphone))
            {
                stringBuilder.Append("{\"success\":false,\"msg\":\"已经存在相同的手机号码\"}");
            }

            else if (UserHelper.IsExistUserName(userName))
            {
                stringBuilder.Append("{\"success\":false,\"msg\":\"已经存在相同用户名\"}");
            }

            else
            {
                Member member = new Member(UserRole.Member);
                if (HiContext.Current.ReferralUserId > 0)
                {
                    member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
                }
                member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                member.Username = userName;
                member.Email = "";//telphone + "@mail.haimylife.com";
                member.CellPhone = telphone;

                member.Password = password;
                member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
                member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
                member.TradePassword = password;
                member.IsApproved = true;
                member.RealName = string.Empty;
                member.Address = string.Empty;

                CreateUserStatus createUserStatus = MemberProcessor.CreateMember(member);
                CreateUserStatus createUserStatus2 = createUserStatus;
                switch (createUserStatus2)
                {
                    case CreateUserStatus.UnknownFailure:
                        stringBuilder.Append("{\"success\":false,\"msg\":\"未知错误\"}");
                        break;
                    case CreateUserStatus.Created:
                        {
                            if (System.Text.RegularExpressions.Regex.IsMatch(member.Username, "^(13|14|15|17|18)\\d{9}$"))
                            {
                                member.CellPhone = member.Username;
                            }
                            if (System.Text.RegularExpressions.Regex.IsMatch(member.Username, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
                            {
                                member.Email = member.Username;
                            }
                            Messenger.UserRegister(member, member.Password);
                            member.OnRegister(new UserEventArgs(member.Username, member.Password, null));
                            IUser user = Users.GetUser(0, member.Username, false, true);
                            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                            if (shoppingCart != null)
                            {
                                ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
                                ShoppingCartProcessor.ClearCookieShoppingCart();
                            }
                            HiContext.Current.User = user;
                            if (shoppingCart != null)
                            {
                                ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
                            }
                            System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
                            IUserCookie userCookie = user.GetUserCookie();
                            userCookie.WriteCookie(authCookie, 30, false);

                            stringBuilder.Append("{\"success\":true,\"msg\":\"注册成功\"}");
                            break;
                        }
                    case CreateUserStatus.DuplicateUsername:
                        stringBuilder.Append("{\"success\":false,\"msg\":\"已经存在相同的用户名\"}");
                        break;
                    case CreateUserStatus.DuplicateEmailAddress:
                        stringBuilder.Append("{\"success\":false,\"msg\":\"已经存在相同的邮箱\"}");
                        break;
                    case CreateUserStatus.InvalidFirstCharacter:
                        stringBuilder.Append("{\"success\":false,\"msg\":\"未知错误\"}");
                        break;
                    case CreateUserStatus.DisallowedUsername:
                        stringBuilder.Append("{\"success\":false,\"msg\":\"用户名禁止注册\"}");
                        break;
                    default:
                        if (createUserStatus2 != CreateUserStatus.InvalidPassword)
                        {
                            break;
                        }
                        stringBuilder.Append("{\"success\":false,\"msg\":\"无效的密码\"}");
                        break;
                }
            }

            context.Response.Write(stringBuilder.ToString());
            context.Response.End();



        }



        /// <summary>
        /// 加载更多商品
        /// </summary>
        /// <param name="context"></param>
        private void LoadMoreProducts(System.Web.HttpContext context)
        {

            int pageNumber = 1;
            int pageSize = 0;
            int brandid = 0;
            if (!string.IsNullOrEmpty(context.Request["brandid"]))
            {
                int.TryParse(context.Request["brandid"], out brandid);//品牌
            }
            int.TryParse(context.Request["size"], out pageSize);

            int.TryParse(context.Request["pageNumber"], out pageNumber);//页码

            int? topicId = null;
            int tmpTopicId = 0;
            int.TryParse(context.Request["TopicId"], out tmpTopicId);
            if (tmpTopicId != 0) topicId = tmpTopicId;//topicId

            string keyWord = context.Request["keyWord"];//搜索词
            keyWord = Globals.UrlDecode(keyWord);
            int? categoryId = null;
            int tmpCategoryId = 0;
            int.TryParse(context.Request["categoryId"], out tmpCategoryId);
            if (tmpCategoryId != 0) categoryId = tmpCategoryId;//分类

            int? importsourceid = null;
            int tmpimportsourceid = 0;
            int.TryParse(context.Request["importsourceid"], out tmpimportsourceid);
            if (tmpimportsourceid != 0) importsourceid = tmpimportsourceid;//产地

            string sort = "DisplaySequence";//排序字段
            string tmpSort = context.Request["sort"];
            if (!string.IsNullOrEmpty(tmpSort))
            {
                sort = tmpSort;
            }
            string order = "asc";//排序方式
            string tmpOrder = context.Request["order"];
            if (!string.IsNullOrEmpty(tmpOrder))
            {
                order = tmpOrder;
            }
            ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();

            if (brandid != 0)
            {
                productBrowseQuery.BrandId = brandid;
            }


            productBrowseQuery.CategoryId = categoryId;
            productBrowseQuery.ImportsourceId = importsourceid;
            productBrowseQuery.Keywords = keyWord;
            productBrowseQuery.TopId = topicId;
            productBrowseQuery.PageIndex = pageNumber;
            productBrowseQuery.PageSize = pageSize;
            productBrowseQuery.SortBy = sort;
            if (order.ToLower() == "asc")
            {
                productBrowseQuery.SortOrder = EcShop.Core.Enums.SortAction.Asc;
            }
            else
            {
                productBrowseQuery.SortOrder = EcShop.Core.Enums.SortAction.Desc;
            }
            StringBuilder stringBuilder = new StringBuilder();

            if (topicId == null)
            {
                DbQueryResult dr = ProductBrowser.GetBrowseProductList(productBrowseQuery);
                DataTable dtProducts = (DataTable)dr.Data;
                stringBuilder.Append("{\"products\":");
                if (dtProducts.Rows.Count > 0)
                {

                    string strProducts = Newtonsoft.Json.JsonConvert.SerializeObject(dtProducts);


                    //strProducts = strProducts.Remove(strProducts.Length - 1).Substring(1);
                    stringBuilder.Append(strProducts);
                }
                else
                {
                    stringBuilder.Append("\"\"");
                }
                stringBuilder.Append(",");
                stringBuilder.AppendFormat("\"total\":{0}", dr.TotalRecords);
                stringBuilder.Append("}");
            }

            else
            {
                int total = 0;
                DataTable dtProducts = ProductBrowser.GetProducts(topicId, categoryId, keyWord, pageNumber, pageSize, out total, sort, true, order);//分页查询
                stringBuilder.Append("{\"products\":");
                if (dtProducts.Rows.Count > 0)
                {

                    string strProducts = Newtonsoft.Json.JsonConvert.SerializeObject(dtProducts);


                    //strProducts = strProducts.Remove(strProducts.Length - 1).Substring(1);
                    stringBuilder.Append(strProducts);
                }
                else
                {
                    stringBuilder.Append("\"\"");
                }
                stringBuilder.Append(",");
                stringBuilder.AppendFormat("\"total\":{0}", total);
                stringBuilder.Append("}");
            }
            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }


        /// <summary>
        /// 加载团购商品
        /// </summary>
        /// <param name="context"></param>
        private void LoadMoreGroupProduct(System.Web.HttpContext context)
        {
            int pageNumber = 1;
            int pageSize = 0;
            int categoryId;
            string keyWord;

            int.TryParse(context.Request["size"], out pageSize);

            int.TryParse(context.Request["pageNumber"], out pageNumber);//页码

            int.TryParse(context.Request["categoryId"], out categoryId);
            keyWord = context.Request["keyWord"];

            int num;
            DataTable dtGroupProduct = ProductBrowser.GetGroupBuyProducts(new int?(categoryId), keyWord, pageNumber, pageSize, out num, true);

            StringBuilder stringBuilder = new StringBuilder();
            if (dtGroupProduct.Rows.Count > 0)
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"success\":true,");
                stringBuilder.Append("\"products\":");
                string strProducts = Newtonsoft.Json.JsonConvert.SerializeObject(dtGroupProduct);
                stringBuilder.Append(strProducts);
                stringBuilder.Append(",");
                stringBuilder.AppendFormat("\"total\":{0}", num);
                stringBuilder.Append("}");
            }
            else
            {
                stringBuilder.Append("{\"success\":false}");
            }

            context.Response.Write(stringBuilder.ToString());
            context.Response.End();

        }


        /// <summary>
        /// 加载我的咨询
        /// </summary>
        /// <param name="context"></param>
        private void MyConsultations(System.Web.HttpContext context)
        {
            int pageNumber = 1;
            int pageSize = 20;
            int.TryParse(context.Request["size"], out pageSize);

            int.TryParse(context.Request["pageNumber"], out pageNumber);//页码
            DbQueryResult productConsultations = ProductBrowser.GetProductConsultations(new ProductConsultationAndReplyQuery
            {
                UserId = HiContext.Current.User.UserId,
                IsCount = true,
                PageIndex = pageNumber,
                PageSize = pageSize,
                SortBy = "ConsultationId",
                SortOrder = SortAction.Desc
            });


            StringBuilder stringBuilder = new StringBuilder();

            DataTable dtProductConsultations = (DataTable)productConsultations.Data;
            if (dtProductConsultations.Rows.Count > 0)
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"success\":true,");
                stringBuilder.Append("\"productConsultations\":");
                IsoDateTimeConverter convert = new IsoDateTimeConverter();
                convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                string strProducts = Newtonsoft.Json.JsonConvert.SerializeObject(dtProductConsultations, Formatting.None, convert);
                stringBuilder.Append(strProducts);
                stringBuilder.Append(",");
                stringBuilder.AppendFormat("\"total\":{0}", productConsultations.TotalRecords);
                stringBuilder.Append("}");
            }
            else
            {
                stringBuilder.Append("{\"success\":false}");
            }

            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }



    }
}

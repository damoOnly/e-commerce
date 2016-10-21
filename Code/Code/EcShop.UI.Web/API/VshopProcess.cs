using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Comments;
using EcShop.Entities.Commodities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.SaleSystem.Vshop;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using EcShop.Core.ErrorLog;
using EcShop.Core.Enums;
using Ecdev.Weixin.MP.Util;
using Entities;
using Commodities;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Xml;
using EcShop.Entities.YJF;
using System.Configuration;
using EcShop.ControlPanel.Members;
using Ecdev.Weixin.MP.Domain;
using Ecdev.Weixin.MP.Api;
using Members;
namespace EcShop.UI.Web.API
{
    public class VshopProcess : System.Web.IHttpHandler
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
                case "AddToCartBySkus":
                    this.ProcessAddToCartBySkus(context);
                    return;
                case "SelectItemToBuy"://微商城，选择购物车的商品结算
                    this.SelectItemToBuy(context);
                    break;
                case "GetSkuByOptions":
                    this.ProcessGetSkuByOptions(context);
                    return;
                case "DeleteCartProduct":
                    this.ProcessDeleteCartProduct(context);
                    return;
                case "DeleteSelectCartProduct":
                    this.ProcessDeleteSelectCartProduct(context);
                    return;
                case "ChageQuantity":
                    this.ProcessChageQuantity(context);
                    return;
                case "Submmitorder":
                    this.ProcessSubmmitorder(context);
                    return;
                case "AddShippingAddress":
                    this.AddShippingAddress(context);
                    return;
                case "DelShippingAddress":
                    this.DelShippingAddress(context);
                    return;
                case "SetDefaultShippingAddress":
                    this.SetDefaultShippingAddress(context);
                    return;
                case "UpdateShippingAddress":
                    this.UpdateShippingAddress(context);
                    return;
                case "GetPrize":
                    this.GetPrize(context);
                    return;
                case "SubmitActivity":
                    this.SubmitActivity(context);
                    return;
                case "AddSignUp":
                    this.AddSignUp(context);
                    return;
                case "AddTicket":
                    this.AddTicket(context);
                    return;
                case "FinishOrder":
                    this.FinishOrder(context);
                    return;
                case "AddUserPrize":
                    this.AddUserPrize(context);
                    return;
                case "SubmitWinnerInfo":
                    this.SubmitWinnerInfo(context);
                    return;
                case "SetUserName":
                    this.SetUserName(context);
                    return;
                case "BindUser":
                    this.BindUser(context);
                    return;
                case "BindPCUser":
                    this.BindPCUser(context);
                    return;
                case "SwitchUser":
                    this.SwitchUser(context);
                    return;
                case "SwitchAccount":
                    this.SwitchAccount(context);
                    return;
                case "RegisterUser":
                    this.RegisterUser(context);
                    return;
                case "AddProductConsultations":
                    this.AddProductConsultations(context);
                    return;
                case "AddProductReview":
                    this.AddProductReview(context);
                    return;
                case "AddFavorite":
                    this.AddFavorite(context);
                    return;
                case "DelFavorite":
                    this.DelFavorite(context);
                    return;
                case "CheckFavorite":
                    this.CheckFavorite(context);
                    return;
                case "Logistic":
                    this.SearchExpressData(context);
                    return;
                case "GetShippingTypes":
                    this.GetShippingTypes(context);
                    return;
                case "Transactionsubmitorder":
                    this.submitorder(context);
                    return;
                case "CancelOrder"://新增取消订单功能
                    this.CancelOrder(context);
                    return;
                case "ApplyForRefund"://新增退款申请
                    this.ApplyForRefund(context);
                    return;
                case "ApplyForReturns"://新增退货申请
                    this.ApplyForReturns(context);
                    return;
                case "ApplyForReplacement"://新增换货申请
                    this.ApplyForReplacement(context);
                    return;
                case "CalcFreight":
                    this.CalcFreight(context);
                    break;
                case "ReferralRegister":
                    this.ReferralRegister(context);
                    return;
                case "SplittinDraws":
                    this.SplittinDraws(context);
                    return;
                case "ShowSubCategory":
                    this.ShowSubCategory(context);
                    return;
                case "onMenuShare":
                    //ErrorLog.Write("测试1：" + context);
                    this.onMenuShare(context);
                    //ErrorLog.Write("测试2");
                    return;
                case "SkuSelector":
                    this.SkuSelector(context);
                    break;
                case "GetAllSkusInfo":
                    this.GetAllSkusInfo(context);
                    break;
                case "LoadMoreProducts":// 查找安全问题
                    this.LoadMoreProducts(context);
                    break;
                case "LoadMoreSuppliers":// 
                    this.LoadMoreSuppliers(context);
                    break;
                case "LoadMoreFavProducts":// 
                    this.LoadMoreFavProducts(context);
                    break;
                case "LoadMoreFavSuppliers":// 
                    this.LoadMoreFavSuppliers(context);
                    break;
                case "LoadMoreUserPoints":// 
                    this.LoadMoreUserPoints(context);
                    break;

                case "GetSiteInfo":
                    this.GetSiteInfo(context);
                    break;
                case "GetSitesList":
                    this.GetSitesList(context);
                    break;
                case "GetBrowserPDetailsHistory":
                    this.GetBrowserPDetailsHistory(context);
                    break;
                case "CalculateFreight":
                    this.CalculateFreight(context);
                    break;
                case "RegisterPCUser":
                    this.RegisterPCUser(context);
                    break;
                //case "UseVoucher":
                //    UseVoucher(context);
                //    break;

                case "GetShoppingCartAmount":
                    this.GetShoppingCartAmount(context);
                    break;
                case "CheckBuyCardinality":
                    this.CheckBuyCardinality(context);
                    break;
                case "GetVshopPromotional":
                    this.GetVshopPromotional(context);
                    break;
                case "AddSupplierFav":
                    this.AddSupplierFav(context);
                    break;
                case "DelCollectSupplier":
                    this.DelCollectSupplier(context);
                    break;

                case "ShowAllCategory":
                    this.ShowAllCategory(context);
                    break;
                case "ShowCategoryBrands":
                    this.ShowCategoryBrands(context);
                    break;
                case "DelHistorySearch":
                    this.DelHistorySearch(context);
                    break;

                case "GetOtherHotSearch":
                    this.GetOtherHotSearch(context);
                    break;
                case "ShowShopActivityData":
                    this.ShowShopActivityData(context);
                    break;
                case "GetTopicProduct":
                    this.GetTopicProduct(context);
                    break;
                case "GetUserInfo":
                    this.getUserInfo(context);
                    break;
                case "BindPhoneNumber":
                    this.BindPhoneNumber(context);
                    break;
                case "Getlistofchildren":
                    this.Getlistofchildren(context);
                    break;
                case "ShowSupplierThreeCategory":
                    this.ShowSupplierThreeCategory(context);
                    break;
                case "ShowTwoCategory":
                    this.ShowTwoCategory(context);
                    break;
                case "ShowThirdCategoryOrBrand":
                    this.ShowThirdCategoryOrBrand(context);
                    break;
                case "RqRefundOrder":
                    this.RqRefundOrder(context);
                    break;
                   
            }
            if (action.Equals("UseVoucher"))
            {
                UseVoucher(context);
            }
        }
        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="context"></param>
        public void RqRefundOrder(System.Web.HttpContext context)
        {
            int type = string.IsNullOrEmpty(context.Request["SourceOrderId"]) ? 1 : 2;
            string orderId = string.IsNullOrEmpty(context.Request["SourceOrderId"]) ? context.Request["orderid"] : context.Request["SourceOrderId"];

            int delaytime = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["OrderRefunTime"]) ? 30 : int.Parse(System.Configuration.ConfigurationManager.AppSettings["OrderRefunTime"].ToString());
            StringBuilder stringBuilder = new StringBuilder();
            if (TradeHelper.CheckOrderIsRefund(orderId, type, delaytime))
            {
        
                bool Rest = TradeHelper.ChangeRefundType(orderId, type);
                if (Rest)
                {
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"success\":\"0\"");
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                    return;
                }
            }
            stringBuilder.Append("{");
            stringBuilder.Append("\"success\":\"-1\"");
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
        }
        /// <summary>
        /// 根据来源单号获取子单号
        /// </summary>
        /// <param name="context"></param>
        public void Getlistofchildren(System.Web.HttpContext context)
        {
            string SourceOrderId = context.Request["SourceOrderId"];
            List<OrderApiCode> list = EcShop.SaleSystem.Shopping.ShoppingProcessor.Getlistofchildren(SourceOrderId);
            if (list != null && list.Count > 0)
            {
                string[] DownCategoriesStr = list.Select(t => t.OrderId).ToArray();
                context.Response.Write("{\"success\":0, \"data\":\"" + string.Join(",", DownCategoriesStr) + "\"}");
            }
            else
            {
                context.Response.Write("{\"success\":-1, \"data\":\"\"}");
            }
        }
          

        public void getUserInfo(System.Web.HttpContext context)
        {
            Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
            StringBuilder text = new StringBuilder("[");
            string DownCategoriesStr = Newtonsoft.Json.JsonConvert.SerializeObject(member);
            text.Append("]");
            context.Response.Write("{\"data\":" + DownCategoriesStr + "}");
        }
        public void SelectItemToBuy(System.Web.HttpContext context)
        {
            string skuIds = context.Request["pids"];
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            if (skuIds == null)
            {
                stringBuilder.Append("\"Success\":0");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                context.Response.End();
            }
            else
            {
                HttpCookie cookie = context.Request.Cookies["UserSession-SkuIds"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("UserSession-SkuIds");
                }
                cookie.Value = Globals.UrlEncode(skuIds);
                cookie.Expires = DateTime.Now.AddDays(2);
                context.Response.AppendCookie(cookie);
            }
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetPartShoppingCartInfo(skuIds);
            if (shoppingCart == null || shoppingCart.LineItems.Count == 0)
            {
                stringBuilder.Append("\"Success\":1,");
                stringBuilder.Append("\"AmoutPrice\":\"0.00\",");
                stringBuilder.Append("\"ReducedPromotion\":\"0.00\",");
                stringBuilder.Append("\"ReducedPromotionName\":\"\",");
                stringBuilder.Append("\"Tax\":\"0.00\",");
                stringBuilder.Append("\"TotalPrice\":\"0.00\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                context.Response.End();
                return;
            }
            stringBuilder.Append("\"Success\":1,");
            stringBuilder.AppendFormat("\"AmoutPrice\":\"{0}\",", shoppingCart.GetNewAmount().ToString("F2"));
            stringBuilder.AppendFormat("\"ReducedPromotion\":\"{0}\",", shoppingCart.ReducedPromotionAmount.ToString("F2"));
            stringBuilder.AppendFormat("\"ReducedPromotionName\":\"{0}\",", shoppingCart.ReducedPromotionName);
            decimal tax = shoppingCart.GetNewTotalTax();
            stringBuilder.AppendFormat("\"Tax\":\"{0}\",", tax.ToString("F2"));
            stringBuilder.AppendFormat("\"NavigateUrl\":\"{0}\",", Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					shoppingCart.ReducedPromotionId
				}).ClearForJson());
            stringBuilder.AppendFormat("\"TotalPrice\":\"{0}\"", shoppingCart.GetTotalIncludeTax().ToString("F2"));
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
        }
        /// <summary>
        /// 显示第二三级商品分类
        /// </summary>
        /// <param name="context"></param>
        public void ShowSubCategory(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string categoryStr = "";

            string categoryId = context.Request["categoryId"];
            StringBuilder stringBuilder = new StringBuilder();

            //为原产地数据
            if (categoryId == "ImportSourceType")
            {
                IList<ImportSourceTypeInfo> list = ImportSourceTypeHelper.GetAllImportSourceTypes();

                if (list == null)
                {
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"Status\":\"-1\"");
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                    return;
                }

                categoryStr += Newtonsoft.Json.JsonConvert.SerializeObject(list);

                stringBuilder.Append(categoryStr.ToString());

            }
            else
            {

                IList<CategoryInfo> secondList = CatalogHelper.GetAllCategories();

                if (secondList == null)
                {
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"Status\":\"-1\"");
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                    return;
                }

                categoryStr += Newtonsoft.Json.JsonConvert.SerializeObject(secondList);

                stringBuilder.Append(categoryStr.ToString());
            }



            context.Response.Write(stringBuilder.ToString());
        }
        public void SkuSelector(System.Web.HttpContext context)
        {
            int productId = 0;
            int.TryParse(context.Request["productId"], out productId);
            DataTable skus = ProductBrowser.GetSkus(productId);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("<input type=\"hidden\" id=\"hiddenSkuId\" value=\"{0}_0\"  />", productId).AppendLine();
            if (skus != null && skus.Rows.Count > 0)
            {
                IList<string> list = new List<string>();
                stringBuilder.AppendFormat("<input type=\"hidden\" id=\"hiddenProductId\" value=\"{0}\" />", productId).AppendLine();
                stringBuilder.AppendLine("<div class=\"specification\">");
                foreach (DataRow dataRow in skus.Rows)
                {
                    if (!list.Contains((string)dataRow["AttributeName"]))
                    {
                        list.Add((string)dataRow["AttributeName"]);
                        stringBuilder.AppendFormat("<div class=\"title text-muted\">{0}：</div><input type=\"hidden\" name=\"skuCountname\" AttributeName=\"{0}\" id=\"skuContent_{1}\" />", dataRow["AttributeName"], dataRow["AttributeId"]);
                        stringBuilder.AppendFormat("<div class=\"list clearfix\" id=\"skuRow_{0}\">", dataRow["AttributeId"]);
                        IList<string> list2 = new List<string>();
                        foreach (DataRow dataRow2 in skus.Rows)
                        {
                            if (string.Compare((string)dataRow["AttributeName"], (string)dataRow2["AttributeName"]) == 0 && !list2.Contains((string)dataRow2["ValueStr"]))
                            {
                                string text = string.Concat(new object[]
								{
									"skuValueId_",
									dataRow["AttributeId"],
									"_",
									dataRow2["ValueId"]
								});
                                list2.Add((string)dataRow2["ValueStr"]);
                                stringBuilder.AppendFormat("<div class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\">{3}</div>", new object[]
								{
									text,
									dataRow["AttributeId"],
									dataRow2["ValueId"],
									dataRow2["ValueStr"]
								});
                            }
                        }
                        stringBuilder.AppendLine("</div>");
                    }
                }
                stringBuilder.AppendLine("</div>");
            }
            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }
        public void GetAllSkusInfo(System.Web.HttpContext context)
        {
            int productId = 0;
            int.TryParse(context.Request["productId"], out productId);
            ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(productId, null, null);
            System.Collections.IEnumerable value =
                from item in productBrowseInfo.Product.Skus
                select item.Value;
            string skusJson = JsonConvert.SerializeObject(value);
            context.Response.ContentType = "application/json";
            context.Response.Write(skusJson);
            context.Response.End();
        }

        public void SplittinDraws(System.Web.HttpContext context)
        {
            DbQueryResult mySplittinDraws = EcShop.SaleSystem.Member.MemberProcessor.GetMySplittinDraws(new BalanceDrawRequestQuery
            {
                PageIndex = 1,
                PageSize = 1,
                UserId = new int?(HiContext.Current.User.UserId)
            }, new int?(1));
            decimal num = 0m;
            context.Response.ContentType = "application/json";
            if (context.Request["Amount"] != null)
            {
                decimal.TryParse(context.Request["Amount"], out num);
            }
            decimal userUseSplittin = EcShop.SaleSystem.Member.MemberProcessor.GetUserUseSplittin(HiContext.Current.User.UserId);
            string text = context.Request["Account"];
            string text2 = context.Request["TradePassword"];
            if (mySplittinDraws.TotalRecords > 0)
            {
                this.ShowMessage(context, "上笔提现管理员还没有处理，只有处理完后才能再次申请提现", true);
                return;
            }
            if (num <= 0m || num > userUseSplittin)
            {
                this.ShowMessage(context, "提现金额输入错误,请重新输入提现金额！", true);
                return;
            }
            if (num > userUseSplittin)
            {
                this.ShowMessage(context, "可提现佣金不足,请重新输入提现金额！", true);
                return;
            }
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMessage(context, "请输入提现帐号！", true);
                return;
            }
            if (string.IsNullOrEmpty(text2))
            {
                this.ShowMessage(context, "请输入交易密码！", true);
                return;
            }
            Member member = HiContext.Current.User as Member;
            member.TradePassword = text2;
            if (!Users.ValidTradePassword(member))
            {
                this.ShowMessage(context, "交易密码不正确,请重新输入", true);
                return;
            }
            if (EcShop.SaleSystem.Member.MemberProcessor.SplittinDrawRequest(new SplittinDrawInfo
            {
                UserId = member.UserId,
                UserName = member.Username,
                Amount = num,
                Account = text,
                RequestDate = System.DateTime.Now,
                AuditStatus = 1
            }))
            {
                this.ShowMessage(context, "提现申请成功，等待管理员的审核", false);
                return;
            }
            this.ShowMessage(context, "提现申请失败，请重试", true);
        }
        public void ShowMessage(System.Web.HttpContext context, string msg, bool IsError = true)
        {
            if (IsError)
            {
                context.Response.Write("{\"success\":false,\"msg\":\"" + msg + "\"}");
                return;
            }
            context.Response.Write("{\"success\":true,\"msg\":\"" + msg + "\"}");
        }
        public void ReferralRegister(System.Web.HttpContext context)
        {
            string text = context.Request["RealName"];
            string cellPhone = context.Request["CellPhone"];
            string text2 = context.Request["ReferralReason"];
            string Email = context.Request["Email"];
            context.Response.ContentType = "application/json";
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(cellPhone))
            {
                this.ShowMessage(context, "请填写姓名和电话", true);
                return;
            }
            if (string.IsNullOrEmpty(Email))
            {
                this.ShowMessage(context, "请填写邮箱", true);
                return;
            }
            if (string.IsNullOrEmpty(text2) || text2.Trim().Length > 300)
            {
                this.ShowMessage(context, "请填写申请理由，字数必须在300字以内！", true);
                return;
            }
            if (HiContext.Current.User == null)
            {
                this.ShowMessage(context, "请您先登陆！", true);
                return;
            }
            if (EcShop.SaleSystem.Member.MemberProcessor.ReferralRequest(HiContext.Current.User.UserId, text, cellPhone, text2, Email))
            {
                this.ShowMessage(context, "申请提交成功！", false);
                return;
            }
            this.ShowMessage(context, "提交申请失败！", true);
        }
        private void submitorder(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            OrderInfo orderInfo = TradeHelper.GetOrderInfo(context.Request["orderId"]);
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            if (orderInfo.CountDownBuyId > 0)
            {
                CountDownInfo countDownBuy = TradeHelper.GetCountDownBuy(orderInfo.CountDownBuyId);
                if (countDownBuy == null || countDownBuy.EndDate < System.DateTime.Now)
                {
                    context.Response.Write("{\"success\":false,\"mesg\":\"当前的订单为限时抢购订单，此活动已结束，所以不能支付\"}");
                    return;
                }
            }
            if (orderInfo.GroupBuyId > 0)
            {
                GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(orderInfo.GroupBuyId);
                if (groupBuy == null || groupBuy.Status != GroupBuyStatus.UnderWay)
                {
                    context.Response.Write("{\"success\":false,\"mesg\":\"当前的订单为团购订单，此团购活动已结束，所以不能支付\"}");
                    return;
                }
                num2 = TradeHelper.GetOrderCount(orderInfo.GroupBuyId);
                num3 = orderInfo.GetGroupBuyOerderNumber();
                num = groupBuy.MaxCount;
                if (num < num2 + num3)
                {
                    context.Response.Write("{\"success\":false,\"mesg\":\"当前的订单为团购订单，订购数量已超过订购总数，所以不能支付\"}");
                    return;
                }
            }
            if (!orderInfo.CheckAction(OrderActions.BUYER_PAY))
            {
                context.Response.Write("{\"success\":false,\"mesg\":\"当前的订单订单状态不是等待付款，所以不能支付\"}");
                return;
            }
            if (HiContext.Current.User.UserId != orderInfo.UserId)
            {
                context.Response.Write("{\"success\":false,\"mesg\":\"预付款只能为自己下的订单付款,查一查该订单是不是你的\"}");
                return;
            }
            Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
            if (!member.IsOpenBalance)
            {
                context.Response.Write("{\"success\":false,\"mesg\":\"您的预付款功能未启用\"}");
                return;
            }
            IUser user = HiContext.Current.User;
            user.TradePassword = context.Request["password"];
            if (!Users.ValidTradePassword(user))
            {
                context.Response.Write("{\"success\":false,\"mesg\":\"密码输入错误！\"}");
                return;
            }
            if (member.Balance - member.RequestBalance < orderInfo.GetTotal())
            {
                context.Response.Write("{\"success\":false,\"mesg\":\"预付款余额不足,支付失败\"}");
                return;
            }
            System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
            foreach (LineItemInfo current in lineItems.Values)
            {
                int skuStock = ShoppingCartProcessor.GetSkuStock(current.SkuId);
                if (skuStock < current.ShipmentQuantity)
                {
                    context.Response.Write("{\"success\":false,\"mesg\":\"订单中商品库存不足，禁止支付！\"}");
                    return;
                }
            }
            if (TradeHelper.UserPayOrder(orderInfo, true))
            {
                try
                {
                    TradeHelper.SaveDebitNote(new DebitNoteInfo
                    {
                        NoteId = Globals.GetGenerateId(),
                        OrderId = context.Request["orderId"],
                        Operator = HiContext.Current.User.Username,
                        Remark = "客户预付款订单支付成功"
                    });
                    if (orderInfo.GroupBuyId > 0 && num == num2 + num3)
                    {
                        TradeHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
                    }
                    Messenger.OrderPayment(user, orderInfo, orderInfo.GetTotal());
                    orderInfo.OnPayment();
                    context.Response.Write("{\"success\":true,\"mesg\":\"交易成功！\"}");
                    return;
                }
                catch (System.Exception)
                {
                    context.Response.Write("{\"success\":true,\"mesg\":\"支付失败！\"}");
                    return;
                }
            }
            context.Response.Write("{\"success\":false,\"mesg\":\"对订单" + orderInfo.OrderId + " 支付失败\"}");
        }
        private void CheckFavorite(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if (!(HiContext.Current.User is Member))
            {
                context.Response.Write("{\"success\":false}");
                return;
            }
            int productId = System.Convert.ToInt32(context.Request["ProductId"]);
            if (ProductBrowser.GetProductSimpleInfo(productId) != null)
            {
                context.Response.Write("{\"success\":true}");
                return;
            }
            context.Response.Write("{\"success\":false}");
        }
        private void DelFavorite(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int favoriteId = System.Convert.ToInt32(context.Request["favoriteId"]);
            if (ProductBrowser.DeleteFavorite(favoriteId) == 1)
            {
                context.Response.Write("{\"success\":true}");
                return;
            }
            context.Response.Write("{\"success\":false, \"msg\":\"取消失败\"}");
        }
        private void AddFavorite(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Member member = HiContext.Current.User as Member;
            int productId = System.Convert.ToInt32(context.Request["ProductId"]);
            if (member == null)
            {
                System.Web.HttpCookie cookieFavorite = context.Request.Cookies["Hid_Ecshop_Favorite_Data_New"];
                if (cookieFavorite == null || string.IsNullOrEmpty(cookieFavorite.Value))
                {
                    cookieFavorite = new HttpCookie("Hid_Ecshop_Favorite_Data_New");
                    cookieFavorite.Value = productId.ToString();
                }
                else
                {
                    string productIds = cookieFavorite.Value;
                    if (!productIds.Contains(productId.ToString()))
                    {
                        cookieFavorite.Value += "|" + productId.ToString();
                    }
                }
                cookieFavorite.Expires = DateTime.Now.AddYears(1);
                context.Response.Cookies.Add(cookieFavorite);
                context.Response.Write("{\"success\":true}");
                //context.Response.Write("{\"success\":false, \"msg\":\"请先登录才可以收藏商品\"}");
                return;
            }

            int favoriteId;
            if (ProductBrowser.AddProductToFavorite(productId, member.UserId, out favoriteId))
            {
                context.Response.Write("{\"success\":true,\"favoriteId\":" + favoriteId + "}");
                return;
            }
            context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
        }
        private void AddProductReview(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
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
                //context.Response.Write("{\"success\":false, \"msg\":\"还未进行评级\"}");
                //return;
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
        private void AddProductConsultations(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Member member = HiContext.Current.User as Member;
            if (ProductBrowser.InsertProductConsultation(new ProductConsultationInfo
            {
                ConsultationDate = System.DateTime.Now,
                ConsultationText = context.Request["ConsultationText"],
                ProductId = System.Convert.ToInt32(context.Request["ProductId"]),
                UserEmail = member.Email,
                UserId = member.UserId,
                UserName = member.Username

            }))
            {
                context.Response.Write("{\"success\":true}");
                return;
            }
            context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
        }
        private void FinishOrder(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string orderId = System.Convert.ToString(context.Request["orderId"]);
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
            if (orderInfo != null && EcShop.SaleSystem.Vshop.MemberProcessor.ConfirmOrderFinish(orderInfo))
            {
                context.Response.Write("{\"success\":true}");
                return;
            }
            context.Response.Write("{\"success\":false, \"msg\":\"订单当前状态不允许完成\"}");
        }
        private void CancelOrder(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string orderId = System.Convert.ToString(context.Request["orderId"]);
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
            if (orderInfo != null && EcShop.SaleSystem.Vshop.MemberProcessor.ConfirmOrderCancel(orderInfo))
            {
                #region 取消订单还原库存 2015-08-19
                //ShoppingProcessor.UpdateRefundOrderStock(orderId);
                #endregion

                #region 取消订单还原优惠券，现金券的使用,就是将Ecshop_CouponItems和Ecshop_VoucherItems的OrderId和UsedTime设为null 2015-08-19

                //bool rs = ShoppingProcessor.RevertVoucher(orderId);
                //ErrorLog.Write("关闭订单还原现金券结果：" + rs);
                //bool rs2 = ShoppingProcessor.RevertCoupon(orderId);
                //ErrorLog.Write("关闭订单还原优惠券结果：" + rs2);
                #endregion
                //写日志
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format("用户取消了订单{0}", orderId));
                context.Response.Write("{\"success\":true}");
                return;
            }
            context.Response.Write("{\"success\":false, \"msg\":\"订单当前状态不允许取消\"}");
        }
        private void SearchExpressData(System.Web.HttpContext context)
        {
            List<object> result = new List<object>();
            if (!string.IsNullOrEmpty(context.Request["OrderId"]))
            {
                string orderId = context.Request["OrderId"];
                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
                if (orderInfo != null && (orderInfo.OrderStatus == OrderStatus.SellerAlreadySent || orderInfo.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb))
                {
                    result = ExpressHelper.GetExpressInfoByNum(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);
                }
            }
            //result = GetExpressInfoByNum();
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(result));
            context.Response.End();
        }

        //public List<object> GetExpressInfoByNum(string expressNum = "805939174754")
        public List<object> GetExpressInfoByNum(string expressNum)
        {
            List<object> result = new List<object>();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string temp1 = string.Format(@"<ufinterface>
<Result>
   <WaybillCode>
      <Number>{0}</Number>
    </WaybillCode>
 </Result>
</ufinterface>
", expressNum);
            string urlapth = string.Format("{0}?reqYTOStr={1}&methodName=yto.Marketing.WaybillTrace", masterSettings.ExpressAddress, temp1);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(urlapth);
            httpWebRequest.Timeout = 8000;
            string text = "暂时没有此快递单号的信息";
            try
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    System.IO.Stream responseStream = httpWebResponse.GetResponseStream();
                    System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.GetEncoding("UTF-8"));
                    text = streamReader.ReadToEnd();
                    if (text.IndexOf("ufinterface") > -1)
                    {
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.LoadXml(text);
                        string code = xdoc.SelectSingleNode("//code").InnerText;
                        XmlNodeList xmlNode = xdoc.SelectNodes("//WaybillProcessInfo");
                        foreach (XmlNode xmlNode2 in xmlNode)
                        {
                            var a = xmlNode2.SelectSingleNode("Upload_Time").InnerText;
                            var b = xmlNode2.SelectSingleNode("ProcessInfo").InnerText;
                            result.Add(new { time = a, context = b, code = code });
                        }
                    }
                }
            }
            catch
            {

            }
            return result;
        }

        public object GetPropertyValue(object info, string field)
        {
            if (info == null) return null;
            Type t = info.GetType();
            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            return property.First().GetValue(info, null);
        }
        public class TrackingDataListItem
        {
            public TrackingDataListItem()
            {

            }
            public TrackingDataListItem(string time, string content, string code)
            {
                this.time = time;
                this.context = content;
                this.code = code;
            }
            public string time { get; set; }
            public string context { get; set; }
            public string code { get; set; }
        }
        private void SearchExpressDataOld(System.Web.HttpContext context)
        {
            /*
            string expressInfoJson = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["OrderId"]))
            {
                string orderId = context.Request["OrderId"];
                OrderExpress orderExpress = ShoppingProcessor.GetOrderExpressInfo(orderId);
                if (orderExpress != null && (orderExpress.OrderStatus == OrderStatus.SellerAlreadySent || orderExpress.OrderStatus == OrderStatus.Finished))
                {
                    expressInfoJson = Newtonsoft.Json.JsonConvert.SerializeObject(orderExpress);
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(expressInfoJson);
            context.Response.End();
            */
            string s = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["OrderId"]))
            {
                string orderId = context.Request["OrderId"];
                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
                if (orderInfo != null && (orderInfo.OrderStatus == OrderStatus.SellerAlreadySent || orderInfo.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb))
                {
                    s = Express.GetExpressData(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(s);
            context.Response.End();
        }

        private void AddSignUp(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int num = System.Convert.ToInt32(context.Request["id"]);
            string b = System.Convert.ToString(context.Request["code"]);
            LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(num);
            if (!string.IsNullOrEmpty(lotteryTicket.InvitationCode) && lotteryTicket.InvitationCode != b)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"邀请码不正确\"}");
                return;
            }
            if (lotteryTicket.EndTime < System.DateTime.Now)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"活动已结束\"}");
                return;
            }
            if (lotteryTicket.OpenTime < System.DateTime.Now)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"报名已结束\"}");
                return;
            }
            if (VshopBrowser.GetUserPrizeRecord(num) == null)
            {
                PrizeRecordInfo prizeRecordInfo = new PrizeRecordInfo();
                prizeRecordInfo.ActivityID = num;
                Member member = HiContext.Current.User as Member;
                prizeRecordInfo.UserID = member.UserId;
                prizeRecordInfo.UserName = member.Username;
                prizeRecordInfo.IsPrize = true;
                prizeRecordInfo.Prizelevel = "已报名";
                prizeRecordInfo.PrizeTime = new System.DateTime?(System.DateTime.Now);
                VshopBrowser.AddPrizeRecord(prizeRecordInfo);
                context.Response.Write("{\"success\":true, \"msg\":\"报名成功\"}");
                return;
            }
            context.Response.Write("{\"success\":false, \"msg\":\"你已经报名了，请不要重复报名！\"}");
        }
        private void AddTicket(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int num = System.Convert.ToInt32(context.Request["activityid"]);
            LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(num);
            Member member = HiContext.Current.User as Member;
            if (member != null && !lotteryTicket.GradeIds.Contains(member.GradeId.ToString()))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"您的会员等级不在此活动范内\"}");
                return;
            }
            if (lotteryTicket.EndTime < System.DateTime.Now)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"活动已结束\"}");
                return;
            }
            if (System.DateTime.Now < lotteryTicket.OpenTime)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"抽奖还未开始\"}");
                return;
            }
            if (VshopBrowser.GetCountBySignUp(num) < lotteryTicket.MinValue)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"还未达到人数下限\"}");
                return;
            }
            PrizeRecordInfo userPrizeRecord = VshopBrowser.GetUserPrizeRecord(num);
            try
            {
                if (!lotteryTicket.IsOpened)
                {
                    VshopBrowser.OpenTicket(num);
                    userPrizeRecord = VshopBrowser.GetUserPrizeRecord(num);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(userPrizeRecord.RealName) && !string.IsNullOrWhiteSpace(userPrizeRecord.CellPhone))
                    {
                        context.Response.Write("{\"success\":false, \"msg\":\"您已经抽过奖了\"}");
                        return;
                    }
                }
                if (userPrizeRecord == null || string.IsNullOrEmpty(userPrizeRecord.PrizeName))
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"很可惜,你未中奖\"}");
                    return;
                }
                if (!userPrizeRecord.PrizeTime.HasValue)
                {
                    userPrizeRecord.PrizeTime = new System.DateTime?(System.DateTime.Now);
                    VshopBrowser.UpdatePrizeRecord(userPrizeRecord);
                }
            }
            catch (System.Exception ex)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"" + ex.Message + "\"}");
                return;
            }
            context.Response.Write("{\"success\":true, \"msg\":\"恭喜你获得" + userPrizeRecord.Prizelevel + "\"}");
        }
        private void SubmitActivity(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                context.Response.Write("{\"success\":false}");
                return;
            }
            int activityId = System.Convert.ToInt32(context.Request.Form.Get("id"));
            ActivityInfo activity = VshopBrowser.GetActivity(activityId);
            if (System.DateTime.Now < activity.StartDate || System.DateTime.Now > activity.EndDate)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"报名还未开始或已结束\"}");
                return;
            }
            if (VshopBrowser.GetActivityCount(activityId) >= activity.MaxValue && activity.MaxValue > 0)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"报名人数已达到限制人数\"}");
                return;
            }
            string s = VshopBrowser.SaveActivitySignUp(new ActivitySignUpInfo
            {
                ActivityId = System.Convert.ToInt32(context.Request.Form.Get("id")),
                Item1 = context.Request.Form.Get("item1"),
                Item2 = context.Request.Form.Get("item2"),
                Item3 = context.Request.Form.Get("item3"),
                Item4 = context.Request.Form.Get("item4"),
                Item5 = context.Request.Form.Get("item5"),
                RealName = member.RealName,
                SignUpDate = System.DateTime.Now,
                UserId = member.UserId,
                UserName = member.Username
            }) ? "{\"success\":true}" : "{\"success\":false, \"msg\":\"你已经报过名了,请勿重复报名\"}";
            context.Response.Write(s);
        }
        private void DelShippingAddress(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                context.Response.Write("{\"success\":false}");
                return;
            }
            int userId = member.UserId;
            int shippingid = System.Convert.ToInt32(context.Request.Form["shippingid"]);
            if (EcShop.SaleSystem.Member.MemberProcessor.DelShippingAddress(shippingid, userId))
            {
                context.Response.Write("{\"success\":true}");
                return;
            }
            context.Response.Write("{\"success\":false}");
        }
        private void SetDefaultShippingAddress(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                context.Response.Write("{\"success\":false}");
                return;
            }
            int userId = member.UserId;
            int shippingId = System.Convert.ToInt32(context.Request.Form["shippingid"]);
            if (EcShop.SaleSystem.Member.MemberProcessor.SetDefaultShippingAddress(shippingId, userId))
            {
                context.Response.Write("{\"success\":true}");
                return;
            }
            context.Response.Write("{\"success\":false}");
        }
        private void AddShippingAddress(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                context.Response.Write("{\"success\":false}");
                return;
            }
            if (EcShop.SaleSystem.Member.MemberProcessor.GetShippingAddressCount(member.UserId) >= HiContext.Current.Config.ShippingAddressQuantity)
            {
                context.Response.Write("{\"success\":false}");
                return;
            }
            if (EcShop.SaleSystem.Member.MemberProcessor.AddShippingAddress(new ShippingAddressInfo
            {
                Address = context.Request.Form["address"],
                CellPhone = context.Request.Form["cellphone"],
                ShipTo = context.Request.Form["shipTo"],
                Zipcode = context.Request.Form["zipcode"],
                IsDefault = true,
                UserId = member.UserId,
                RegionId = System.Convert.ToInt32(context.Request.Form["regionSelectorValue"]),
                IdentityCard = context.Request.Form["identityCard"]
            }) > 0)
            {
                context.Response.Write("{\"success\":true}");
                return;
            }
            context.Response.Write("{\"success\":false}");
        }
        private void UpdateShippingAddress(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                context.Response.Write("{\"success\":false}");
                return;
            }
            if (EcShop.SaleSystem.Member.MemberProcessor.UpdateShippingAddress(new ShippingAddressInfo
            {
                Address = context.Request.Form["address"],
                CellPhone = context.Request.Form["cellphone"],
                ShipTo = context.Request.Form["shipTo"],
                Zipcode = context.Request.Form["zipcode"],
                UserId = member.UserId,
                ShippingId = System.Convert.ToInt32(context.Request.Form["shippingid"]),
                RegionId = System.Convert.ToInt32(context.Request.Form["regionSelectorValue"]),
                IdentityCard = context.Request.Form["identityCard"]
            }))
            {
                context.Response.Write("{\"success\":true}");
                return;
            }
            context.Response.Write("{\"success\":false}");
        }
        private void SubmitWinnerInfo(System.Web.HttpContext context)
        {
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                context.Response.Write("{\"success\":false}");
                return;
            }
            int activityId = System.Convert.ToInt32(context.Request.Form.Get("id"));
            string realName = context.Request.Form.Get("name");
            string cellPhone = context.Request.Form.Get("phone");
            string s = VshopBrowser.UpdatePrizeRecord(activityId, member.UserId, realName, cellPhone) ? "{\"success\":true}" : "{\"success\":false}";
            context.Response.ContentType = "application/json";
            context.Response.Write(s);
        }
        private void ProcessAddToCartBySkus(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int quantity = int.Parse(context.Request["quantity"], System.Globalization.NumberStyles.None);
            string skuId = context.Request["productSkuId"];
            int storeId = 0;
            int.TryParse(context.Request["storeId"], out storeId);


            //int stock = ShoppingProcessor.GetProductStock(skuId);

            int stock = ShoppingCartProcessor.GetSkuStock(skuId);

            if (stock <= 0 || stock < quantity)
            {
                context.Response.Write("{\"Status\":\"Error\",\"ErrorMsg\":\"库存不足\"}");
                return;
            }
            //检查商品是否超过限购数量
            Member member = HiContext.Current.User as Member;
            if (member != null)
            {
                int MaxCount = 0;
                int Payquantity = ProductHelper.CheckPurchaseCount(skuId, member.UserId, out MaxCount);
                if ((Payquantity + quantity) > MaxCount && MaxCount != 0) //当前购买数量大于限购剩余购买数量
                {
                    context.Response.Write("{\"Status\":\"purchase\",\"ErrorMsg\":\"超出限购数量\"}");
                    return;
                }
            }
            ShoppingCartProcessor.AddLineItem(skuId, quantity, storeId);
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            if (shoppingCart != null)
            {
                context.Response.Write(string.Concat(new string[]
				{
					"{\"Status\":\"OK\",\"TotalMoney\":\"",
					shoppingCart.GetTotal().ToString(".00"),
					"\",\"Quantity\":\"",
					shoppingCart.GetQuantity().ToString(),
					"\"}"
				}));
                return;
            }
            context.Response.Write("{\"Status\":\"0\"}");
        }
        private void GetShoppingCartAmount(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            if (shoppingCart != null)
            {
                context.Response.Write("{\"Status\":\"OK\",\"Quantity\":\"" + shoppingCart.GetQuantity().ToString() + "\"}");
                return;
            }
            else
            {
                context.Response.Write("{\"Status\":\"OK\",\"Quantity\":\"0\"}");
            }
        }
        private void ProcessGetSkuByOptions(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int productId = int.Parse(context.Request["productId"], System.Globalization.NumberStyles.None);
            string text = context.Request["options"];
            if (string.IsNullOrEmpty(text))
            {
                context.Response.Write("{\"Status\":\"0\"}");
                return;
            }
            if (text.EndsWith(","))
            {
                text = text.Substring(0, text.Length - 1);
            }
            SKUItem productAndSku = ShoppingProcessor.GetProductAndSku(productId, text);
            if (productAndSku == null)
            {
                context.Response.Write("{\"Status\":\"1\"}");
                return;
            }
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"Status\":\"OK\",");
            stringBuilder.AppendFormat("\"SkuId\":\"{0}\",", productAndSku.SkuId);
            stringBuilder.AppendFormat("\"SKU\":\"{0}\",", productAndSku.SKU);
            stringBuilder.AppendFormat("\"Weight\":\"{0}\",", productAndSku.Weight);
            stringBuilder.AppendFormat("\"Stock\":\"{0}\",", productAndSku.Stock);
            stringBuilder.AppendFormat("\"SalePrice\":\"{0}\"", productAndSku.SalePrice.ToString("F2"));
            stringBuilder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(stringBuilder.ToString());
        }
        private void ProcessDeleteCartProduct(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string skuId = context.Request["skuId"];
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            ShoppingCartProcessor.RemoveLineItem(skuId);
            stringBuilder.Append("{");
            stringBuilder.Append("\"Status\":\"OK\",");
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            int cartQuantity = 0;
            if (shoppingCart != null)
            {
                cartQuantity = shoppingCart.GetQuantity();
            }
            stringBuilder.AppendFormat("\"Quantity\":\"{0}\"", cartQuantity);
            stringBuilder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(stringBuilder.ToString());
        }
        private void ProcessChageQuantity(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string skuId = context.Request["skuId"];
            string skuIds = context.Request["skuIds"];
            int num = 1;
            int.TryParse(context.Request["quantity"], out num);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            int skuStock = ShoppingCartProcessor.GetSkuStock(skuId);
            if (num > skuStock)
            {
                stringBuilder.AppendFormat("\"Status\":\"{0}\"", skuStock);
                num = skuStock;
            }
            else
            {
                stringBuilder.Append("\"Status\":\"OK\",");
                ShoppingCartProcessor.UpdateLineItemQuantity(skuId, (num > 0) ? num : 1);
                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetPartShoppingCartInfo(skuIds);

                decimal totaltax = shoppingCart.GetNewTotalTax();
                stringBuilder.AppendFormat("\"TotalPrice\":\"{0}\",", shoppingCart.GetNewAmount());
                stringBuilder.AppendFormat("\"Quantity\":\"{0}\",", shoppingCart.GetQuantity());
                stringBuilder.AppendFormat("\"TotalTax\":\"{0}\"", totaltax.ToString("F2"));
            }
            stringBuilder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(stringBuilder.ToString());
        }
        private void ProcessSubmmitorder(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            int orderCountIn3Min;
            int todayOrderCount;
            TradeHelper.GetOrderCount4MaliciousOrder(HiContext.Current.User.UserId, out orderCountIn3Min, out todayOrderCount);
            if (todayOrderCount >= 20)
            {
                var result = new { Status = "Error", ErrorMsg = "您今天的订单数已经达到20上限" };
                context.Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            if (orderCountIn3Min >= 5)
            {
                var result = new { Status = "Error", ErrorMsg = "下单频率太快，请稍后再试" };
                context.Response.Write(JsonConvert.SerializeObject(result));
                return;
            }

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            int modeId = 0;
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
            int countDownId;
            bool downFlag = int.TryParse(context.Request["countDownId"], out countDownId);
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
            int buyAmount;
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            if (int.TryParse(context.Request["buyAmount"], out buyAmount) && !string.IsNullOrEmpty(context.Request["productSku"]) && (a2 == "signbuy" || a2 == "groupbuy" || a2 == "countdown"))
            {
                string productSkuId = context.Request["productSku"];
                int storeId = 0;
                int.TryParse(context.Request["storeId"], out storeId);
                if (a2 == "signbuy")
                {
                    shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(productSkuId, buyAmount, storeId);
                }
                else if (a2 == "groupbuy")
                {
                    shoppingCartInfo = ShoppingCartProcessor.GetGroupBuyShoppingCart(productSkuId, buyAmount, storeId);
                }
                else if (a2 == "countdown")
                {
                    shoppingCartInfo = ShoppingCartProcessor.GetCountDownShoppingCart(productSkuId, buyAmount, storeId);
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

            #region 判断是否是特定二级类型的商品使用了优惠券
            string CurrentCategoryDesc = ShoppingProcessor.GetCurrentCategoryDesc(shoppingCartInfo, couponCode);
            if (!string.IsNullOrWhiteSpace(CurrentCategoryDesc))
            {
                stringBuilder.Append("\"Status\":\"Error\"");
                stringBuilder.Append(",\"ErrorMsg\":\"" + CurrentCategoryDesc + "\"}");
                context.Response.Write(stringBuilder.ToString());
                context.Response.End();
            }
            #endregion
            OrderInfo orderInfo = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCartInfo, false, false, false, HiContext.Current.User.UserId);
            if (orderInfo != null)
            {
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
                if (downFlag)
                {
                    orderInfo.CountDownBuyId = countDownId;
                }
                orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;
                orderInfo.RefundStatus = RefundStatus.None;
                orderInfo.ShipToDate = context.Request["shiptoDate"];
                ShippingAddressInfo shippingAddress = EcShop.SaleSystem.Member.MemberProcessor.GetShippingAddress(shippingId);
                if (shippingAddress != null)
                {
                    #region 验证每人每日最多消费1000元，1000元以上为单件商品
                    int mayCount = 0;
                    foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                    {
                        mayCount += item.Quantity;
                        //#region 验证库存
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

                    #region  多件商品总额不能超过1000
                    if (!unpackOrder)
                    {
                        if (mayCount > 1)
                        {
                            decimal money = orderInfo.GetAmount();
                            if (money > 1000)
                            {
                                stringBuilder.Append("\"Status\":\"Error\"");
                                stringBuilder.Append(",\"ErrorMsg\":\"抱歉，您已超过海关限额￥1000，请分次购买。 </br>海关规定：</br>① 消费者购买进口商品，以“个人自用，合理数量”为原则，每单最大购买限额为1000元人民币。</br>② 如果订单只含单件不可分割商品，则可以超过1000元限值。</br>\"}");
                                context.Response.Write(stringBuilder.ToString());
                                context.Response.End();
                            }
                        }
                    }

                    #endregion


                    #region 限时抢购数量验证
                    if (a2 == "countdown")
                    {
                        CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo(shoppingCartInfo.LineItems[0].ProductId);
                        if (countDownInfo.EndDate < System.DateTime.Now)
                        {

                            stringBuilder.Append("\"Status\":\"Error\"");
                            stringBuilder.Append(",\"ErrorMsg\":\"此订单为抢购订单，但抢购时间已到！\"}");
                            context.Response.Write(stringBuilder.ToString());
                            context.Response.End();
                        }

                        //限购数量为0表示限购数量不限制
                        if (countDownInfo.MaxCount > 0)
                        {
                            if (shoppingCartInfo.LineItems[0].Quantity > countDownInfo.MaxCount)
                            {
                                stringBuilder.Append("\"Status\":\"Error\"");
                                stringBuilder.AppendFormat(",\"ErrorMsg\":\"你购买的数量超过限购数量{0}\"}", countDownInfo.MaxCount.ToString());
                                context.Response.Write(stringBuilder.ToString());
                                context.Response.End();
                            }
                            int countdownnum = ShoppingProcessor.CountDownOrderCount(shoppingCartInfo.LineItems[0].ProductId, member.UserId, countDownInfo.CountDownId);
                            if (countdownnum + shoppingCartInfo.LineItems[0].Quantity > countDownInfo.MaxCount)
                            {

                                stringBuilder.Append("\"Status\":\"Error\"");
                                stringBuilder.AppendFormat(",\"ErrorMsg\":\"你已经抢购过该商品{0}件，每个用户只允许抢购{1}件,如果你有未付款的抢购单，请及时支付！\"}", countdownnum, countDownInfo.MaxCount.ToString());
                                context.Response.Write(stringBuilder.ToString());
                                context.Response.End();
                            }
                        }

                        int countdownnum2 = ShoppingProcessor.AllCountDownOrderCount(shoppingCartInfo.LineItems[0].ProductId, countDownInfo.CountDownId);
                        if (countdownnum2 + shoppingCartInfo.LineItems[0].Quantity > countDownInfo.PlanCount)
                        {
                            stringBuilder.Append("\"Status\":\"Error\"");
                            stringBuilder.Append(",\"ErrorMsg\":\"该商品活动数量不足\"}");
                            context.Response.Write(stringBuilder.ToString());
                            context.Response.End();
                        }
                    }

                    #endregion


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
                        //if (string.IsNullOrEmpty(identityCard))
                        //{
                        //    stringBuilder.Append("\"Status\":\"Error\"");
                        //    stringBuilder.Append(",\"ErrorMsg\":\"有需要清关的商品，身份证号码不能为空\"}");
                        //    context.Response.Write(stringBuilder.ToString());
                        //    context.Response.End();
                        //}
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
                    orderInfo.ShippingId = shippingAddress.ShippingId;//新增地址ShippingId
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
                //订单确认不再出现配送方式,运费改为按商品的运费模版
                //if (int.TryParse(context.Request["shippingType"], out modeId))
                //{
                //ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(modeId, true);
                //if (shippingMode != null)
                //{
                orderInfo.ShippingModeId = -1;
                orderInfo.ModeName = "";
                decimal tax = 0m;
                decimal freight = 0m;
                int totalGoodsCount = 0;
                //
                HashSet<int> hsSupplierId = new HashSet<int>();
                HashSet<int> hsTemplateId = new HashSet<int>();
                Dictionary<int, decimal> dictShippingMode = new Dictionary<int, decimal>();
                if (shoppingCartInfo.LineItems.Count != shoppingCartInfo.LineItems.Count((ShoppingCartItemInfo a) => a.IsfreeShipping) && !shoppingCartInfo.IsFreightFree)
                {
                    //orderInfo.AdjustedFreight = (orderInfo.Freight = ShoppingProcessor.CalcFreight(orderInfo.RegionId, shoppingCartInfo.Weight, shippingMode));                
                    foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                    {
                        //totalGoodsCount += item.Quantity;
                        //tax += item.AdjustedPrice * item.TaxRate * item.Quantity;
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
                        if (!hsSupplierId.Contains(item.SupplierId))
                        {
                            hsSupplierId.Add(item.SupplierId);//商品供应商集合
                        }
                        if (!hsTemplateId.Contains(item.TemplateId))
                        {
                            hsTemplateId.Add(item.TemplateId);//运费模版Id
                        }
                    }
                    foreach (var item in dictShippingMode)
                    {
                        ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(item.Key);
                        freight += ShoppingProcessor.CalcFreight(shippingAddress.RegionId, item.Value, shippingMode);
                    }
                    orderInfo.AdjustedFreight = (orderInfo.Freight = freight);
                    //orderInfo.Tax = tax <= 50 ? 0 : tax;
                    //orderInfo.OriginalTax = tax;
                }
                else
                {
                    orderInfo.AdjustedFreight = (orderInfo.Freight = 0m);
                }


                #region 计算税费
                foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                {
                    totalGoodsCount += item.Quantity;
                    //tax += item.AdjustedPrice * item.TaxRate * item.Quantity;
                }

                tax = shoppingCartInfo.GetNewTotalTax();
                orderInfo.Tax = tax <= 50 ? 0 : tax;
                orderInfo.OriginalTax = tax;
                #endregion


                //}
                //}
                //去除购买数量超过30的限制
                //if (totalGoodsCount > 30)
                //{
                //    stringBuilder.Append("\"Status\":\"Error\"");
                //    stringBuilder.Append(",\"ErrorMsg\":\"购买数量不能超过30个\"}");
                //    context.Response.Write(stringBuilder.ToString());
                //    context.Response.End();
                //}
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
                decimal totalProductAmount = shoppingCartInfo.GetTotal();
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
                // 处理现金券
                CalculateVoucherToOrder(selectedVoucherCode, totalProductAmount, inputVoucherCode, inputVoucherPwd, orderInfo);

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

                    //只有一个供应商时，给供应商ID赋值
                    if (shoppingCartInfo.LineItems != null && shoppingCartInfo.LineItems.Count > 0)
                    {
                        orderInfo.SupplierId = shoppingCartInfo.LineItems[0].SupplierId;
                    }

                    if (ShoppingProcessor.CreateOrder(orderInfo, true, true))
                    {

                        #region 先拆开，需要用到金额
                        if ((unpackOrder && totalGoodsCount > 1 && tax > 50)
                            || (totalGoodsCount > 1 && orderInfo.GetNewAmount() > 1000)) //
                        {
                            decimal unpackedTaxTotal = 0;
                            decimal unpackedOrderTotal = 0;
                            decimal unpackedFreight = 0;
                            //b = ShoppingProcessor.UnpackOrderBySupplier(orderInfo, ref unpackedTaxTotal, ref unpackedOrderTotal, ref unpackedFreight);
                            b = ShoppingProcessor.CreateChildOrders(orderInfo, ref unpackedTaxTotal, ref unpackedOrderTotal, ref unpackedFreight);
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
                        // ShoppingCartProcessor.ClearShoppingCart();
                        HttpCookie cookieSkuIds = context.Request.Cookies["UserSession-SkuIds"];
                        //有选择删除购物车商品
                        if (a2 != "signbuy" && a2 != "groupbuy" && a2 != "countdown")
                        {
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
                        }

                        OrderQuery orderQuery = new OrderQuery();
                        orderQuery.Status = OrderStatus.WaitBuyerPay;
                        int userOrderCount = EcShop.SaleSystem.Member.MemberProcessor.GetUserOrderCount(HiContext.Current.User.UserId, orderQuery);
                        stringBuilder.Append("\"Status\":\"OK\",");
                        stringBuilder.AppendFormat("\"Quantity\":\"{0}\",", userOrderCount);
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
                    goto IL_5E3;
                }
                catch (OrderException ex)
                {
                    stringBuilder.Append("\"Status\":\"Error\"");
                    stringBuilder.AppendFormat(",\"ErrorMsg\":\"{0}\"", ex.Message);
                    goto IL_5E3;
                }
            }
            stringBuilder.Append("\"Status\":\"None\"");
        IL_5E3:
            stringBuilder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(stringBuilder.ToString());
        }

        private static void CalculateVoucherToOrder(string selectedVoucherCode, decimal totalProductAmount,
            string inputVoucherCode, string inputVoucherPwd, OrderInfo orderInfo)
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

        private void AddUserPrize(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int num = 1;
            int.TryParse(context.Request["activityid"], out num);
            string text = context.Request["prize"];
            LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(num);
            PrizeRecordInfo prizeRecordInfo = new PrizeRecordInfo();
            prizeRecordInfo.PrizeTime = new System.DateTime?(System.DateTime.Now);
            prizeRecordInfo.UserID = HiContext.Current.User.UserId;
            prizeRecordInfo.ActivityName = lotteryActivity.ActivityName;
            prizeRecordInfo.ActivityID = num;
            prizeRecordInfo.Prizelevel = text;
            string key;
            switch (key = text)
            {
                case "一等奖":
                    prizeRecordInfo.PrizeName = lotteryActivity.PrizeSettingList[0].PrizeName;
                    prizeRecordInfo.IsPrize = true;
                    goto IL_1FF;
                case "二等奖":
                    prizeRecordInfo.PrizeName = (prizeRecordInfo.PrizeName = lotteryActivity.PrizeSettingList[1].PrizeName);
                    prizeRecordInfo.IsPrize = true;
                    goto IL_1FF;
                case "三等奖":
                    prizeRecordInfo.PrizeName = lotteryActivity.PrizeSettingList[2].PrizeName;
                    prizeRecordInfo.IsPrize = true;
                    goto IL_1FF;
                case "四等奖":
                    prizeRecordInfo.PrizeName = lotteryActivity.PrizeSettingList[3].PrizeName;
                    prizeRecordInfo.IsPrize = true;
                    goto IL_1FF;
                case "五等奖":
                    prizeRecordInfo.PrizeName = lotteryActivity.PrizeSettingList[4].PrizeName;
                    prizeRecordInfo.IsPrize = true;
                    goto IL_1FF;
                case "六等奖":
                    prizeRecordInfo.PrizeName = lotteryActivity.PrizeSettingList[5].PrizeName;
                    prizeRecordInfo.IsPrize = true;
                    goto IL_1FF;
            }
            prizeRecordInfo.IsPrize = false;
        IL_1FF:
            VshopBrowser.AddPrizeRecord(prizeRecordInfo);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"Status\":\"OK\"");
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder);
        }
        private void GetPrize(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int num = 1;
            int.TryParse(context.Request["activityid"], out num);
            LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(num);
            int userPrizeCount = VshopBrowser.GetUserPrizeCount(num);
            IUser arg_42_0 = HiContext.Current.User;
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            if (userPrizeCount >= lotteryActivity.MaxNum)
            {
                stringBuilder.Append("\"No\":\"-1\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }
            if (System.DateTime.Now < lotteryActivity.StartTime || System.DateTime.Now > lotteryActivity.EndTime)
            {
                stringBuilder.Append("\"No\":\"-3\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }
            System.Collections.Generic.List<PrizeRecordInfo> prizeList = VshopBrowser.GetPrizeList(new PrizeQuery
            {
                ActivityId = num
            });
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            if (prizeList != null && prizeList.Count > 0)
            {
                num2 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "一等奖");
                num3 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "二等奖");
                num4 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "三等奖");
            }
            PrizeRecordInfo prizeRecordInfo = new PrizeRecordInfo();
            prizeRecordInfo.PrizeTime = new System.DateTime?(System.DateTime.Now);
            prizeRecordInfo.UserID = HiContext.Current.User.UserId;
            prizeRecordInfo.ActivityName = lotteryActivity.ActivityName;
            prizeRecordInfo.ActivityID = num;
            prizeRecordInfo.IsPrize = true;
            System.Collections.Generic.List<PrizeSetting> prizeSettingList = lotteryActivity.PrizeSettingList;
            decimal d = prizeSettingList[0].Probability * 100m;
            decimal d2 = prizeSettingList[1].Probability * 100m;
            decimal d3 = prizeSettingList[2].Probability * 100m;
            System.Random random = new System.Random(System.Guid.NewGuid().GetHashCode());
            int value = random.Next(1, 10001);
            if (prizeSettingList.Count > 3)
            {
                decimal d4 = prizeSettingList[3].Probability * 100m;
                decimal d5 = prizeSettingList[4].Probability * 100m;
                decimal d6 = prizeSettingList[5].Probability * 100m;
                int num5 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "四等奖");
                int num6 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "五等奖");
                int num7 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "六等奖");
                if (value < d && prizeSettingList[0].PrizeNum > num2)
                {
                    stringBuilder.Append("\"No\":\"9\"");
                    prizeRecordInfo.Prizelevel = "一等奖";
                    prizeRecordInfo.PrizeName = prizeSettingList[0].PrizeName;
                }
                else
                {
                    if (value < d2 && prizeSettingList[1].PrizeNum > num3)
                    {
                        stringBuilder.Append("\"No\":\"11\"");
                        prizeRecordInfo.Prizelevel = "二等奖";
                        prizeRecordInfo.PrizeName = prizeSettingList[1].PrizeName;
                    }
                    else
                    {
                        if (value < d3 && prizeSettingList[2].PrizeNum > num4)
                        {
                            stringBuilder.Append("\"No\":\"1\"");
                            prizeRecordInfo.Prizelevel = "三等奖";
                            prizeRecordInfo.PrizeName = prizeSettingList[2].PrizeName;
                        }
                        else
                        {
                            if (value < d4 && prizeSettingList[3].PrizeNum > num5)
                            {
                                stringBuilder.Append("\"No\":\"3\"");
                                prizeRecordInfo.Prizelevel = "四等奖";
                                prizeRecordInfo.PrizeName = prizeSettingList[3].PrizeName;
                            }
                            else
                            {
                                if (value < d5 && prizeSettingList[4].PrizeNum > num6)
                                {
                                    stringBuilder.Append("\"No\":\"5\"");
                                    prizeRecordInfo.Prizelevel = "五等奖";
                                    prizeRecordInfo.PrizeName = prizeSettingList[4].PrizeName;
                                }
                                else
                                {
                                    if (value < d6 && prizeSettingList[5].PrizeNum > num7)
                                    {
                                        stringBuilder.Append("\"No\":\"7\"");
                                        prizeRecordInfo.Prizelevel = "六等奖";
                                        prizeRecordInfo.PrizeName = prizeSettingList[5].PrizeName;
                                    }
                                    else
                                    {
                                        prizeRecordInfo.IsPrize = false;
                                        stringBuilder.Append("\"No\":\"0\"");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (value < d && prizeSettingList[0].PrizeNum > num2)
                {
                    stringBuilder.Append("\"No\":\"9\"");
                    prizeRecordInfo.Prizelevel = "一等奖";
                    prizeRecordInfo.PrizeName = prizeSettingList[0].PrizeName;
                }
                else
                {
                    if (value < d2 && prizeSettingList[1].PrizeNum > num3)
                    {
                        stringBuilder.Append("\"No\":\"11\"");
                        prizeRecordInfo.Prizelevel = "二等奖";
                        prizeRecordInfo.PrizeName = prizeSettingList[1].PrizeName;
                    }
                    else
                    {
                        if (value < d3 && prizeSettingList[2].PrizeNum > num4)
                        {
                            stringBuilder.Append("\"No\":\"1\"");
                            prizeRecordInfo.Prizelevel = "三等奖";
                            prizeRecordInfo.PrizeName = prizeSettingList[2].PrizeName;
                        }
                        else
                        {
                            prizeRecordInfo.IsPrize = false;
                            stringBuilder.Append("\"No\":\"0\"");
                        }
                    }
                }
            }
            stringBuilder.Append("}");
            if (context.Request["activitytype"] != "scratch")
            {
                VshopBrowser.AddPrizeRecord(prizeRecordInfo);
            }
            context.Response.Write(stringBuilder.ToString());
        }
        public void SetUserName(System.Web.HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            context.Response.ContentType = "application/json";
            Member member = HiContext.Current.User as Member;

            if (member == null)
            {
                stringBuilder.Append("\"Status\":\"Error\",\"IsVerify\":\"IsNotLog\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }

            if (string.IsNullOrWhiteSpace(member.IdentityCard))
            {
                member.IdentityCard = "";
            }

            if ((member.RealName != context.Request["RealName"] || member.IdentityCard.ToUpper() != context.Request["IdentityCard"].ToUpper()) && UserHelper.IsExistIdentityCard(context.Request["IdentityCard"].Trim(), HiContext.Current.User.UserId) > 0)
            {
                //this.ShowMessage("存在已认证的身份证号码，不能保存!", false);
                stringBuilder.Append("\"Status\":\"Error\",\"IsVerify\":\"IsNotVerify\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }

            bool rest=OrderHelper.Checkisverify(HiContext.Current.User.UserId);
            if (!rest || member.RealName != context.Request["RealName"] || member.IdentityCard.ToUpper() != context.Request["IdentityCard"].ToUpper())
            {
                int MaxCount = string.IsNullOrEmpty(ConfigurationManager.AppSettings["IsVerifyCount"]) ? 3 : int.Parse(ConfigurationManager.AppSettings["IsVerifyCount"]);
                int fcount = MemberHelper.CheckErrorCount(HiContext.Current.User.UserId);
                if (fcount >= MaxCount)//超过失败次数 || (MaxCount - fcount - 1) == 0
                {
                    stringBuilder.Append("\"Status\":\"Error\",\"IsVerify\":\"SendMax\",\"mscount\":" + (MaxCount).ToString());
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                    return;
                }

                #region  ==验证身份证实名认证
                string realName = "";
                if (!string.IsNullOrWhiteSpace(context.Request["RealName"].Trim()))
                {
                    realName = context.Request["RealName"].Trim();
                }
                string identityCard = "";
                if (!string.IsNullOrWhiteSpace(context.Request["IdentityCard"].Trim()))
                {
                    identityCard = context.Request["IdentityCard"].Trim().ToUpper();
                }
                CertNoValidHelper cartno = new CertNoValidHelper();
                string result = cartno.CertNoValid(realName, identityCard);
                ErrorLog.Write("微信实名验证结果：" + result + ";Name=" + member.RealName + ";Card=" + member.IdentityCard);
                MemberHelper.AddIsVerifyMsg(HiContext.Current.User.UserId);
                if (result != "实名通过")
                {
                    stringBuilder.Append("\"Status\":\"Error\",\"IsVerify\":\"UNPASS\",\"mscount\":" + (MaxCount - fcount - 1).ToString());
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                    return;
                }
                #endregion
            }

            if (!string.IsNullOrEmpty(context.Request["userName"]))
            {
                member.Username = context.Request["userName"];
            }
            if (!string.IsNullOrEmpty(context.Request["CellPhone"]))
            {
                member.CellPhone = context.Request["CellPhone"];
            }
            if (!string.IsNullOrEmpty(context.Request["QQ"]))
            {
                member.QQ = context.Request["QQ"];
            }
            if (!string.IsNullOrEmpty(context.Request["Email"]))
            {
                member.Email = context.Request["Email"];
            }
            if (!string.IsNullOrEmpty(context.Request["RealName"]))
            {
                member.RealName = context.Request["RealName"];
            }
            if (!string.IsNullOrEmpty(context.Request["IdentityCard"]))
            {
                member.IdentityCard = context.Request["IdentityCard"];//身份证号码
            }
            member.IsVerify = 1;
            member.VerifyDate = DateTime.Now;
            ErrorLog.Write("会员信息:" +Newtonsoft.Json.JsonConvert.SerializeObject(member));
            if (Users.UpdateUser(member))
            {
                //if (MembersUser != null && !string.IsNullOrEmpty(MembersUser.RealName))
                //{
                //    member.RealName = MembersUser.RealName;
                //}
                stringBuilder.Append("\"Status\":\"OK\",\"IsVerify\":\"PASS\"");
            }
            else
            {
                bool UpRest = OrderHelper.UpdateMemberInfo(member.UserId, member.CellPhone, member.IdentityCard, member.RealName);
                if (UpRest)
                {
                    //System.Collections.Hashtable hashtable = Users.UserCache();
                    //string key = (member.UserId > 0) ? Users.UserKey(member.UserId.ToString(System.Globalization.CultureInfo.InvariantCulture)) : Users.UserKey(member.Username);
                    //hashtable.Remove(key);
                    stringBuilder.Append("\"Status\":\"OK\",\"IsVerify\":\"PASS\"");
                }
                else
                {
                    stringBuilder.Append("\"Status\":\"Error\",\"IsVerify\":\"PASS\"");
                }
            }
            
          
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
        }
        public void BindUser(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string text = context.Request["openId"];
            string text2 = context.Request["userName"];
            string password = context.Request["password"];
            string text3 = context.Request["nickname"];
            Member member = Users.GetUser(0, text2, false, true) as Member;
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            if (member == null || member.IsAnonymous)
            {
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }
            member.Password = password;
            LoginUserStatus loginUserStatus = EcShop.SaleSystem.Member.MemberProcessor.ValidLogin(member);
            if (loginUserStatus == LoginUserStatus.Success)
            {

                string name = "Vshop-Member";
                HttpCookie httpCookie = new HttpCookie("Vshop-Member");
                httpCookie.Value = Globals.UrlEncode(text2);
                httpCookie.Expires = System.DateTime.Now.AddDays(7);
                httpCookie.Domain = HttpContext.Current.Request.Url.Host;
                if (HttpContext.Current.Response.Cookies[name] != null)
                {
                    HttpContext.Current.Response.Cookies.Remove(name);
                }
                HttpContext.Current.Response.Cookies.Add(httpCookie);

                if (!string.IsNullOrEmpty(text))
                {
                    if (context.Request["client"] == "alioh")
                    {
                        member.AliOpenId = text;
                    }
                    else
                    {
                        member.OpenId = text;
                        if (string.IsNullOrEmpty(member.RealName) && !string.IsNullOrEmpty(text3))
                        {
                            member.RealName = System.Web.HttpUtility.UrlDecode(text3);
                        }
                    }
                    Users.UpdateUser(member);
                }
                ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
                HiContext.Current.User = member;
                if (cookieShoppingCart != null)
                {
                    ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
                    ShoppingCartProcessor.ClearCookieShoppingCart();
                }
                stringBuilder.Append("\"Status\":\"OK\"");
            }
            else
            {
                stringBuilder.Append("\"Status\":\"" + (int)loginUserStatus + "\"");
            }
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
        }

        /// <summary>
        /// 绑定PC端账号
        /// </summary>
        /// <param name="context"></param>
        public void BindPCUser(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string username = context.Request["userName"];
            string password = context.Request["password"];

            Member member = Users.GetUser(0, username, false, true) as Member;
            Member curMember = HiContext.Current.User as Member;
            string openId = curMember.OpenId;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            if (member == null || member.IsAnonymous)
            {
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }
            member.Password = password;
            LoginUserStatus loginUserStatus = EcShop.SaleSystem.Member.MemberProcessor.ValidLogin(member);
            if (loginUserStatus == LoginUserStatus.Success)
            {
                //修改当前绑定的OpenId,绑定PC端的当前状态为0
                if (member.UserId > 0 && !string.IsNullOrEmpty(openId))
                {
                    bool userResult = UserHelper.UpdateUserOpenId(member.UserId, 0, openId);
                    if (!userResult)
                    {
                        stringBuilder.Append("\"Status\":\"-4\"");
                        stringBuilder.Append("}");
                        context.Response.Write(stringBuilder.ToString());
                        return;
                    }

                    /*int result = 0;
                    UserHelper.BindPCAccount(member.UserId, openId, ref result);

                    if (result != -1)
                    {
                        stringBuilder.Append("\"Status\":\"-4\"");
                        stringBuilder.Append("}");
                        context.Response.Write(stringBuilder.ToString());
                        return;
                    }*/
                }
                stringBuilder.Append("\"Status\":\"OK\"");
            }
            else
            {
                stringBuilder.Append("\"Status\":\"" + (int)loginUserStatus + "\"");
            }
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
        }


        /// <summary>
        /// 注册PC端账户
        /// </summary>
        /// <param name="context"></param>
        public void RegisterPCUser(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string username = context.Request["userName"];
            string password = context.Request["password"];
            string email = context.Request["email"];

            Member curMember = HiContext.Current.User as Member;

            string openId = curMember.OpenId;

            int userId = UserHelper.GetUserIdByOpenId(openId);

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("{");
            if (UserHelper.IsExistUserName(username))
            {

                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }

            if (UserHelper.IsExistEmal(email, username))
            {

                stringBuilder.Append("\"Status\":\"-2\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }

            if (userId <= 0)
            {
                stringBuilder.Append("\"Status\":\"-4\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }

            else
            {
                bool userResult = UserHelper.RegisterPCUser(userId, 0, password, email, username);
                if (!userResult)
                {
                    stringBuilder.Append("\"Status\":\"-4\"");
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                    return;
                }

                Member member = Users.GetUser(userId) as Member;
                if (member != null)
                {
                    //设置缓存
                    Hashtable hashtable = Users.UserCache();
                    hashtable[Users.UserKey(username)] = member;

                    string name = "Vshop-Member";
                    HttpCookie httpCookie = new HttpCookie("Vshop-Member");
                    httpCookie.Value = Globals.UrlEncode(username);
                    httpCookie.Expires = System.DateTime.Now.AddDays(7);
                    httpCookie.Domain = HttpContext.Current.Request.Url.Host;
                    if (HttpContext.Current.Response.Cookies[name] != null)
                    {
                        HttpContext.Current.Response.Cookies.Remove(name);
                    }
                    HttpContext.Current.Response.Cookies.Add(httpCookie);


                    stringBuilder.Append("\"Status\":\"OK\"");
                }

                else
                {
                    stringBuilder.Append("\"Status\":\"-4\"");
                }

            }

            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
        }

        /// <summary>
        /// 切换账号
        /// </summary>
        /// <param name="context"></param>
        public void SwitchUser(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            StringBuilder stringBuilder = new StringBuilder();
            Member curMember = HiContext.Current.User as Member;
            string openId = curMember.OpenId;
            //openId = "oCrlLs9ESzcmemrBbAktZKPDQGBU";
            if (string.IsNullOrEmpty(openId))
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }

            DataTable dt = UserHelper.GetSwitchUsers(openId);

            int count = dt.Rows.Count;
            if (dt != null && count > 0)
            {
                stringBuilder.Append("{\"Users\":[");
                for (int i = 0; i < count; i++)
                {
                    stringBuilder.Append("{");
                    if (dt.Rows[i]["RealName"] != null && !string.IsNullOrEmpty(dt.Rows[i]["RealName"].ToString()))
                    {
                        stringBuilder.Append("\"RealName\":\"" + dt.Rows[i]["RealName"].ToString() + "\",");
                    }
                    else
                    {
                        stringBuilder.Append("\"RealName\":\"\",");
                    }
                    if (dt.Rows[i]["UserCurrent"] != null && !string.IsNullOrEmpty(dt.Rows[i]["UserCurrent"].ToString()))
                    {
                        stringBuilder.Append("\"UserCurrent\":\"" + dt.Rows[i]["UserCurrent"].ToString() + "\",");
                    }
                    if (dt.Rows[i]["UserType"] != null && !string.IsNullOrEmpty(dt.Rows[i]["UserType"].ToString()))
                    {
                        stringBuilder.Append("\"UserType\":\"" + dt.Rows[i]["UserType"].ToString() + "\",");
                    }
                    if (dt.Rows[i]["HeadImgUrl"] != null && !string.IsNullOrEmpty(dt.Rows[i]["HeadImgUrl"].ToString()))
                    {
                        stringBuilder.Append("\"HeadImgUrl\":\"" + dt.Rows[i]["HeadImgUrl"].ToString() + "\",");
                    }
                    if (dt.Rows[i]["UserName"] != null && !string.IsNullOrEmpty(dt.Rows[i]["UserName"].ToString()))
                    {
                        stringBuilder.Append("\"UserName\":\"" + dt.Rows[i]["UserName"].ToString() + "\",");
                    }
                    if (dt.Rows[i]["Email"] != null && !string.IsNullOrEmpty(dt.Rows[i]["Email"].ToString()))
                    {
                        stringBuilder.Append("\"Email\":\"" + dt.Rows[i]["Email"].ToString() + "\",");
                    }
                    else
                    {
                        stringBuilder.Append("\"Email\":\"\",");
                    }
                    if (dt.Rows[i]["UserId"] != null && !string.IsNullOrEmpty(dt.Rows[i]["UserId"].ToString()))
                    {
                        stringBuilder.Append("\"UserId\":\"" + dt.Rows[i]["UserId"].ToString() + "\"");
                    }
                    stringBuilder.Append("}");

                    if (i != count - 1)
                    {
                        stringBuilder.Append(",");
                    }
                }
                stringBuilder.Append("]}");
            }
            else
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"-2\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }


            context.Response.Write(stringBuilder.ToString());

        }

        /// <summary>
        /// 切换账号
        /// </summary>
        /// <param name="context"></param>
        public void SwitchAccount(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Member curMember = HiContext.Current.User as Member;
            string usernName = string.Empty;

            int nowUserId = curMember.UserId;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");

            int switchUserId = 0;
            if (!string.IsNullOrEmpty(context.Request["userId"]))
            {
                switchUserId = int.Parse(context.Request["userId"]);
            }

            if (nowUserId == 0 || switchUserId == 0)
            {
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }

            IUser user = UserHelper.UpdateUsersCurrent(nowUserId, switchUserId);
            Member member = user as Member;

            if (member != null)
            {

                if (!string.IsNullOrEmpty(user.Username))
                {
                    usernName = user.Username;
                }

                stringBuilder.Append("\"Status\":\"OK\"");


                //设置缓存
                Hashtable hashtable = Users.UserCache();
                hashtable[Users.UserKey(usernName)] = member;

                //cookie替换
                string name = "Vshop-Member";
                HttpCookie httpCookie = new HttpCookie("Vshop-Member");
                httpCookie.Value = Globals.UrlEncode(usernName);
                httpCookie.Expires = System.DateTime.Now.AddDays(7);
                httpCookie.Domain = HttpContext.Current.Request.Url.Host;
                if (HttpContext.Current.Response.Cookies[name] != null)
                {
                    HttpContext.Current.Response.Cookies.Remove(name);
                }
                HttpContext.Current.Response.Cookies.Add(httpCookie);
            }

            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());

        }

        public void RegisterUser(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string text = context.Request["openId"];
            string text2 = context.Request["userName"];
            string email = context.Request["email"];
            string text3 = context.Request["password"];
            string text4 = context.Request["nickname"];
            Member member = new Member(UserRole.Member);
            if (HiContext.Current.ReferralUserId > 0)
            {
                member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
            }
            member.GradeId = EcShop.SaleSystem.Member.MemberProcessor.GetDefaultMemberGrade();
            if (context.Request["client"] == "alioh")
            {
                member.AliOpenId = text;
            }
            else
            {
                member.OpenId = text;
            }
            member.Username = text2;
            member.Email = email;
            member.Password = text3;
            member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
            member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
            member.TradePassword = text3;
            member.IsApproved = true;
            member.RealName = string.Empty;
            if (!string.IsNullOrEmpty(text4))
            {
                member.RealName = System.Web.HttpUtility.UrlDecode(text4);
            }
            member.Address = string.Empty;
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            CreateUserStatus createUserStatus = EcShop.SaleSystem.Member.MemberProcessor.CreateMember(member);
            if (createUserStatus == CreateUserStatus.DuplicateUsername || createUserStatus == CreateUserStatus.DisallowedUsername)
            {
                stringBuilder.Append("\"Status\":\"-1\"");
            }
            else
            {
                if (createUserStatus == CreateUserStatus.DuplicateEmailAddress)
                {
                    stringBuilder.Append("\"Status\":\"-2\"");
                }
                else
                {
                    if (createUserStatus == CreateUserStatus.Created)
                    {
                        Messenger.UserRegister(member, text3);
                        member.OnRegister(new UserEventArgs(member.Username, text3, null));

                        string name = "Vshop-Member";
                        HttpCookie httpCookie = new HttpCookie("Vshop-Member");
                        httpCookie.Value = Globals.UrlEncode(text2);
                        httpCookie.Expires = System.DateTime.Now.AddDays(7);
                        httpCookie.Domain = HttpContext.Current.Request.Url.Host;
                        if (HttpContext.Current.Response.Cookies[name] != null)
                        {
                            HttpContext.Current.Response.Cookies.Remove(name);
                        }
                        HttpContext.Current.Response.Cookies.Add(httpCookie);

                        stringBuilder.Append("\"Status\":\"OK\"");
                        ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
                        if (cookieShoppingCart != null)
                        {
                            ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
                            ShoppingCartProcessor.ClearCookieShoppingCart();
                        }
                    }
                    else
                    {
                        stringBuilder.Append("\"Status\":\"0\"");
                    }
                }
            }
            stringBuilder.Append("}");
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
        public void GetShippingTypes(System.Web.HttpContext context)
        {
            int regionId = System.Convert.ToInt32(context.Request["regionId"]);
            int num = (!string.IsNullOrWhiteSpace(context.Request["groupBuyId"])) ? System.Convert.ToInt32(context.Request["groupBuyId"]) : 0;
            bool flag = num > 0;
            int buyAmount;
            ShoppingCartInfo shoppingCart;
            if (int.TryParse(context.Request["buyAmount"], out buyAmount) && !string.IsNullOrWhiteSpace(context.Request["productSku"]))
            {
                string productSkuId = System.Convert.ToString(context.Request["productSku"]);
                int storeId = 0;
                int.TryParse(context.Request["storeId"], out storeId);
                shoppingCart = ShoppingCartProcessor.GetShoppingCart(productSkuId, buyAmount, storeId);
            }
            else
            {
                shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            }
            System.Collections.Generic.IEnumerable<int> enumerable =
                from item in ShoppingProcessor.GetShippingModes()
                select item.ModeId;
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            if (enumerable != null && enumerable.Count<int>() > 0)
            {
                foreach (int current in enumerable)
                {
                    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(current, true);
                    decimal num2 = 0m;
                    if (shoppingCart.LineItems.Count != shoppingCart.LineItems.Count((ShoppingCartItemInfo a) => a.IsfreeShipping) && (!shoppingCart.IsFreightFree || flag))
                    {
                        num2 = ShoppingProcessor.CalcFreight(regionId, shoppingCart.Weight, shippingMode);
                    }
                    stringBuilder.Append(string.Concat(new string[]
					{
						",{\"modelId\":\"",
						shippingMode.ModeId.ToString(),
						"\",\"text\":\"",
						shippingMode.Name,
						"： ￥",
						num2.ToString("F2"),
						"\",\"freight\":\"",
						num2.ToString("F2"),
						"\"}"
					}));
                }
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Remove(0, 1);
                }
            }
            stringBuilder.Insert(0, "{\"data\":[").Append("]}");
            context.Response.ContentType = "application/json";
            context.Response.Write(stringBuilder.ToString());
        }
        public void ApplyForRefund(System.Web.HttpContext context)
        {
            string orderId = context.Request["orderId"];
            string remark = context.Request["remark"];
            int refundType = 0;
            int.TryParse(context.Request["refundType"], out refundType);

            string flagMsg = "";

            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(remark) || refundType == 0)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"传入参数错误\"}");
                return;
            }
            if (!TradeHelper.CanRefund(orderId))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"已有待确认的申请\"}");
                return;
            }
            if (TradeHelper.ApplyForRefund(orderId, remark, refundType, out flagMsg))
            {
                context.Response.Write("{\"success\":true, \"msg\":\"成功的申请了退款\"}");
                return;
            }
            context.Response.Write("{\"success\":false, \"msg\":\"申请退款失败 " + flagMsg + "\"}");
        }
        public void ApplyForReturns(System.Web.HttpContext context)//退货申请
        {
            string orderId = context.Request["orderId"];
            string remark = context.Request["remark"];
            string skuIds = context.Request["skuIds"];
            string quantityList = context.Request["quantityList"];
            string logisticsCompany = context.Request["logisticsCompany"];//物流公司
            string logisticsId = context.Request["logisticsId"];//物流单号

            int returnType = 0;
            int.TryParse(context.Request["returnType"], out returnType);
            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(remark) || returnType == 0)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"传入参数错误\"}");
                return;
            }
            if (string.IsNullOrEmpty(skuIds))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"请选择商品\"}");
                return;
            }
            if (string.IsNullOrEmpty(quantityList))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"数量不能为空\"}");
                return;
            }
            OrderInfo order = OrderHelper.GetOrderInfo(orderId);
            if (order.OrderStatus == OrderStatus.ApplyForReturns)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"该订单已经申请退货\"}");
                return;
            }
            if (!TradeHelper.CanReturn(orderId))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"已有待确认的申请\"}");
                return;
            }
            //重新方法
            //insert into Ecshop_OrderReturns(OrderId,ApplyForTime,Comments,HandleStatus,RefundType,RefundMoney) values(@OrderId,@ApplyForTime,@Comments,0,@RefundType,0);


            //if (TradeHelper.ApplyForReturn(orderId, remark, returnType))
            if (TradeHelper.CreateReturnsEntityAndAdd(orderId, remark, returnType, skuIds, quantityList, logisticsCompany, logisticsId))
            {
                context.Response.Write("{\"success\":true, \"msg\":\"成功的申请了退货\"}");
                return;
            }
            context.Response.Write("{\"success\":false, \"msg\":\"申请退货失败\"}");
        }
        public void ApplyForReplacement(System.Web.HttpContext context)
        {
            string orderId = context.Request["orderId"];
            string remark = context.Request["remark"];
            string skuIds = context.Request["skuIds"];
            string quantityList = context.Request["quantityList"];
            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(remark))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"传入参数错误\"}");
                return;
            }
            if (string.IsNullOrEmpty(skuIds))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"请选择商品\"}");
                return;
            }
            if (string.IsNullOrEmpty(quantityList))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"数量不能为空\"}");
                return;
            }
            if (!TradeHelper.CanReplace(orderId))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"已有待确认的申请\"}");
                return;
            }

            // if (TradeHelper.ApplyForReplace(orderId, remark))
            if (TradeHelper.CreateReplaceEntityAndAdd(orderId, remark, skuIds, quantityList))
            {
                context.Response.Write("{\"success\":true, \"msg\":\"成功的申请了换货\"}");
                return;
            }
            context.Response.Write("{\"success\":false, \"msg\":\"申请换货失败\"}");
        }
        private void CalcFreight(System.Web.HttpContext context)//动态计算运费待完善
        {
            string a2 = "";
            ShoppingCartInfo shoppingCartInfo;
            shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart();
            decimal freight = 0m;
            int num2 = 0;
            bool flag = int.TryParse(context.Request["groupbuyId"], out num2);
            int regionId = int.Parse(context.Request["RegionId"]);
            Dictionary<int, decimal> dictShippingMode = new Dictionary<int, decimal>();
            if (shoppingCartInfo.LineItems.Count != shoppingCartInfo.LineItems.Count((ShoppingCartItemInfo a) => a.IsfreeShipping) && !shoppingCartInfo.IsFreightFree)
            {
                foreach (ShoppingCartItemInfo item in shoppingCartInfo.LineItems)
                {
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
                }
                foreach (var item in dictShippingMode)
                {
                    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(item.Key);
                    freight += ShoppingProcessor.CalcFreight(regionId, item.Value, shippingMode);
                }
            }
        }
        private void LoadBrandProduct(System.Web.HttpContext context)//获取品牌列表的商品
        {
            ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
            productBrowseQuery.PageIndex = 1;
            productBrowseQuery.PageSize = 12;
            productBrowseQuery.SortOrder = SortAction.Desc;
            productBrowseQuery.SortBy = "DisplaySequence";
            Globals.EntityCoding(productBrowseQuery, true);
        }
        private void LoadMoreProducts(System.Web.HttpContext context)
        {
            #region 获取查询条件
            int total = 0;
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

            int? supplierId = null;
            int tmpSupplierId = 0;
            int.TryParse(context.Request["SupplierId"], out tmpSupplierId);
            if (tmpSupplierId != 0) supplierId = tmpSupplierId;//

            string keyWord = context.Request["keyWord"];//搜索词
            keyWord = Globals.UrlDecode(keyWord);
            int? categoryId = null;
            int tmpCategoryId = 0;
            int.TryParse(context.Request["categoryId"], out tmpCategoryId);
            if (tmpCategoryId != 0) categoryId = tmpCategoryId;//分类

            string sort = "DisplaySequence";//排序字段
            string tmpSort = context.Request["sort"];
            if (!string.IsNullOrEmpty(tmpSort))
            {
                sort = tmpSort;
            }
            string order = "desc";//排序方式
            string tmpOrder = context.Request["order"];
            if (!string.IsNullOrEmpty(tmpOrder))
            {
                order = tmpOrder;
            }
            //if (topicId == null)
            //{
            //    sort = "DisplaySequence";
            //    order = "desc";
            //    pageSize = 20;
            //}
            //else
            //{
            //    pageSize = new VTemplateHelper().GetTopicProductMaxNum();
            //}
            #endregion

            DataTable dtProducts = null;
            int totalPage = 0;
            #region 品牌商品
            //if (brandid != 0)
            //{
            //    ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
            //    productBrowseQuery.PageIndex = pageNumber;
            //    productBrowseQuery.PageSize = pageSize = 12;
            //    if (!string.IsNullOrEmpty(tmpOrder))
            //    {
            //        if (tmpOrder == "asc")
            //        {
            //            productBrowseQuery.SortOrder = SortAction.Asc;
            //        }
            //        else
            //        {
            //            productBrowseQuery.SortOrder = SortAction.Desc;
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(tmpSort))
            //    {
            //        productBrowseQuery.SortBy = tmpSort;
            //    }
            //    else
            //    {
            //        productBrowseQuery.SortBy = "DisplaySequence";//saleprice
            //    }

            //    Globals.EntityCoding(productBrowseQuery, true);
            //    DbQueryResult dataProducts = ProductBrowser.GetBrandProducts(new int?(brandid), productBrowseQuery);
            //    dtProducts = (DataTable)dataProducts.Data;
            //    totalPage = dataProducts.TotalRecords % pageSize != 0 ? (dataProducts.TotalRecords / pageSize + 1) : (dataProducts.TotalRecords / pageSize);//总页数
            //}
            #endregion

            #region 分类商品列表或者topic商品列表
            //else
            //{
            //    dtProducts = ProductBrowser.GetProducts(topicId, categoryId, supplierId, keyWord, pageNumber, pageSize, out total, sort, true, order, true);//分页查询
            //    totalPage = total % pageSize != 0 ? (total / pageSize + 1) : (total / pageSize);//总页数
            //}
            #endregion

            ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
            if (!string.IsNullOrEmpty(tmpOrder))
            {
                if (tmpOrder == "asc")
                {
                    productBrowseQuery.SortOrder = SortAction.Asc;
                }
                else
                {
                    productBrowseQuery.SortOrder = SortAction.Desc;
                }
            }
            if (!string.IsNullOrEmpty(tmpSort))
            {
                productBrowseQuery.SortBy = tmpSort;
            }
            else
            {
                productBrowseQuery.SortBy = "DisplaySequence";//saleprice
            }


            if (categoryId.HasValue && categoryId.Value > 0)
            {
                productBrowseQuery.StrCategoryId = categoryId.Value.ToString();
            }

            if (supplierId.HasValue && supplierId.Value > 0)
            {
                productBrowseQuery.supplierid = supplierId.Value;
            }

            if (brandid > 0)
            {
                productBrowseQuery.StrBrandId = brandid.ToString();
            }

            //if (this.importsourceid > 0)
            //{
            //    productBrowseQuery.StrImportsourceId = this.importsourceid.ToString();
            //}


            if (!string.IsNullOrWhiteSpace(keyWord))
            {
                productBrowseQuery.Keywords = keyWord;
            }

            productBrowseQuery.PageIndex = pageNumber;
            productBrowseQuery.PageSize = pageSize;

            DbQueryResult dr = ProductBrowser.GetCurrBrowseProductList(productBrowseQuery);

            dtProducts = (DataTable)dr.Data;
            totalPage = dr.TotalRecords;
            StringBuilder stringBuilder = new StringBuilder();
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
            stringBuilder.Append("\"totalPage\":");
            stringBuilder.Append(totalPage);
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }

        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="context"></param>
        public void onMenuShare(System.Web.HttpContext context)
        {

            ErrorLog.Write("onMenuShare:");
            context.Response.ContentType = "application/json";

            string shareContent = context.Request["ShareContent"];
            ErrorLog.Write("SharePerson:" + shareContent);
            string link = context.Request["Link"];
            ErrorLog.Write("link:" + link);

            //分享类型
            int shareType;
            int.TryParse(context.Request["ShareType"], out shareType);
            ErrorLog.Write("shareType:" + shareType);

            //商品Id
            int productId;
            int.TryParse(context.Request["ProductId"], out productId);
            ErrorLog.Write("productId:" + productId);

            //订单Id
            string orderId = context.Request["OrderId"];
            ErrorLog.Write("orderId:" + orderId);

            int shareUserId = 0;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");

            IUser user = HiContext.Current.User as Member;

            ShareInfo shareinfo = new ShareInfo();
            shareinfo.ShareContent = shareContent;
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.Username))
                {
                    shareinfo.SharePerson = user.Username;
                }
                if (user.UserId > 0)
                {
                    shareUserId = user.UserId;
                }
            }

            ErrorLog.Write("shareUserId:" + shareUserId);

            shareinfo.Link = link;
            shareinfo.ShareType = shareType;
            shareinfo.OrderId = orderId;
            shareinfo.ProductId = productId;
            shareinfo.ShareUserId = shareUserId;

            bool flag = TradeHelper.ShareDeal(shareinfo);
            if (flag)
            {
                stringBuilder.Append("\"Status\":\"OK\"");
                stringBuilder.Append("}");
            }
            else
            {
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
            }

            ErrorLog.Write("stringBuilder.ToString():" + stringBuilder.ToString());
            context.Response.Write(stringBuilder.ToString());
        }
        /// <summary>
        /// 根据经纬度定位到城市，获取站点信息
        /// </summary>
        /// <param name="context"></param>
        public void GetSiteInfo(System.Web.HttpContext context)
        {
            DataTable dtSites = SitesManagementHelper.GetSites();
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            string latitude = context.Request["latitude"];
            string longitude = context.Request["longitude"];
            if (string.IsNullOrEmpty(latitude) || string.IsNullOrEmpty(longitude))
            {
                goto Return;
            }
            WebUtils web = new WebUtils();

            string result = string.Empty;
            string url1 = "http://api.map.baidu.com/geocoder/v2/";
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("ak", "CmoTY51No30euLHjvVr0HbXV");
            dict.Add("callback", "renderReverse");
            dict.Add("location", string.Format("{0},{1}", latitude, longitude));
            dict.Add("output", "json");
            dict.Add("pois", "1");
            try
            {
                result = web.DoGet(url1, dict);
            }
            catch
            {
                goto Return;
            }
            LocationInfo location = null;
            try
            {
                result = result.Remove(result.Length - 1).Replace("renderReverse&&renderReverse(", "");
                location = Newtonsoft.Json.JsonConvert.DeserializeObject<LocationInfo>(result);
            }
            catch
            {
                goto Return;
            }

            if (location == null)
            {
                goto Return;
            }
            if (location.Status != 0)
            {
                goto Return;
            }
            if (location.Result == null
                || location.Result.AddressComponent == null
                || string.IsNullOrEmpty(location.Result.AddressComponent.City))
            {
                goto Return;
            }
            string city = location.Result.AddressComponent.City;//城市名称
            int cityId = RegionHelper.GetCityId(city);
            if (cityId == 0)
            {
                goto Return;
            }
            if (dtSites != null)
            {
                DataRow[] row = dtSites.Select("City=" + cityId + "");
                if (row.Length == 0)
                {
                    row = dtSites.Select("IsDefault=1");
                }
                string defaultSite = row[0]["SitesName"].ToString();
                string defaultSiteId = row[0]["SitesId"].ToString();
                sb.Append("\"status\":0,");
                sb.AppendFormat("\"DefaultSite\":\"{0}\",", defaultSite);
                sb.AppendFormat("\"DefaultSiteId\":\"{0}\"", defaultSiteId);
                sb.Append("}");
                context.Response.Write(sb.ToString());
                context.Response.End();
            }
        //this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(dt), DefaultSite, DefaultSiteId);

        Return:
            {
                DataRow[] row = row = dtSites.Select("IsDefault=1");
                string defaultSite = row[0]["SitesName"].ToString();
                string defaultSiteId = row[0]["SitesId"].ToString();
                sb.Append("\"status\":0,");
                sb.AppendFormat("\"DefaultSite\":\"{0}\",", defaultSite);
                sb.AppendFormat("\"DefaultSiteId\":\"{0}\"", defaultSiteId);
                sb.Append("}");
                context.Response.Write(sb.ToString());
                context.Response.End();
            }
        }
        public void GetSitesList(System.Web.HttpContext context)
        {
            DataTable dtSites = SitesManagementHelper.GetSites();
            string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(dtSites);
            context.Response.Write(strJson);
            context.Response.End();
        }

        /// <summary>
        /// IP的地址转换
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long IpToInt(string ip)
        {
            long ipVal = 0;

            if (!string.IsNullOrEmpty(ip))
            {
                try
                {
                    char[] separator = new char[] { '.' };
                    string[] items = ip.Split(separator);

                    if (items.Length == 4)
                    {
                        ipVal = long.Parse(items[0]) << 24
                                | long.Parse(items[1]) << 16
                                | long.Parse(items[2]) << 8
                                | long.Parse(items[3]);
                    }
                }
                catch { }
            }

            return ipVal;
        }

        public void GetBrowserPDetailsHistory(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            //浏览URL
            string url = context.Request["Url"];
            //商品Id
            int productId;
            int.TryParse(context.Request["ProductId"], out productId);
            //平台类型
            int platType;
            int.TryParse(context.Request["PlatType"], out platType);

            StringBuilder stringBuilder = new StringBuilder();

            string ipAddress = Globals.IPAddress;

            IUser user = HiContext.Current.User as Member;

            UserbrowsehistoryInfo historyInfo = new UserbrowsehistoryInfo();

            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.Username))
                {
                    historyInfo.UserName = user.Username;
                }
                if (user.UserId > 0)
                {
                    historyInfo.UserId = user.UserId;
                }
            }

            historyInfo.Url = url;
            historyInfo.UserIP = ipAddress;
            historyInfo.PlatType = platType;
            historyInfo.ProductId = productId;
            historyInfo.Ip = IpToInt(ipAddress);

            DataTable dtUserHistory = UserbrowsehistoryHelper.GetUserBrowseHistory(historyInfo);

            if (dtUserHistory != null && dtUserHistory.Rows.Count > 0)
            {
                string strJson = Newtonsoft.Json.JsonConvert.SerializeObject(dtUserHistory);
                context.Response.Write(strJson);
            }
            else
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"NO\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
            }

            context.Response.End();
        }

        /// <summary>
        /// 使用代金券
        /// </summary>
        public void UseVoucher(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            // 取输入的号码和密码
            string code = context.Request.QueryString["txtVoucherCode"];
            string pwd = context.Request.QueryString["txtVoucherPwd"];
            decimal orderTotal = decimal.Parse(context.Request.QueryString["orderTotal"]);
            decimal discountAmount = ShoppingProcessor.GetVoucherAmount(code, pwd, orderTotal);
            StringBuilder stringBuilder = new StringBuilder(100);
            stringBuilder.Append("{");
            stringBuilder.AppendFormat("\"Amount\":{0}", discountAmount);
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }

        private void CalculateFreight(System.Web.HttpContext context)//动态计算运费
        {
            int shippingid = 0;
            int buyAmount = 0;
            string a2 = "";
            if (!string.IsNullOrEmpty(context.Request["from"]))
            {
                a2 = context.Request["from"].ToString().ToLower();
            }
            if (string.IsNullOrEmpty(context.Request["shippingid"]))
            {

            }
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
                shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart();
            }
            //
            ShippingAddressInfo memberDefaultShippingAddressInfo = EcShop.SaleSystem.Member.MemberProcessor.GetShippingAddress(shippingid);
            if (memberDefaultShippingAddressInfo != null)
            {
                decimal freight = ShoppingCartProcessor.GetFreight(shoppingCartInfo, memberDefaultShippingAddressInfo.RegionId, false); //ShoppingProcessor.CalcShoppingCartFreight(shoppingCartInfo, memberDefaultShippingAddressInfo.RegionId);
            }
        }

        /// <summary>
        /// 检查购买基数
        /// </summary>
        /// <param name="context"></param>
        private void CheckBuyCardinality(System.Web.HttpContext context)
        {
            var unselectedProductId = context.Request["productIds"];
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            if (shoppingCart.LineItems.Count == 0)
            {
                return;
            }

            if (!string.IsNullOrEmpty(unselectedProductId))
            {
                string[] pid_skuids = unselectedProductId.Split(',');
                foreach (var s in pid_skuids)
                {
                    int productId = int.Parse(s.Split('|')[0]);
                    string skuId = s.Split('|')[1];
                    var item = shoppingCart.LineItems.FirstOrDefault(p => p.ProductId == productId && p.SkuId == skuId);
                    if (item != null)
                    {
                        shoppingCart.LineItems.Remove(item);
                    }
                }
            }
            List<object> result = new List<object>();
            var checkProduct = context.Request["checkProducts"];
            if (!string.IsNullOrEmpty(checkProduct))
            {
                //检查商品是否超过限购数量
                Member member = HiContext.Current.User as Member;
                string[] pid_skuids = checkProduct.Split(',');
                foreach (var s in pid_skuids)
                {
                    int productId = int.Parse(s.Split('|')[0]);
                    string skuId = s.Split('|')[1];
                    var item = shoppingCart.LineItems.FirstOrDefault(p => p.ProductId == productId && p.SkuId == skuId);
                    if (member != null)
                    {
                        int MaxCount = 0;
                        int count = ProductHelper.CheckPurchaseCount(skuId, member.UserId, out MaxCount);
                        if ((count + item.Quantity) > MaxCount && MaxCount != 0) //当前购买数量大于限购剩余购买数量
                        {
                            result.Add(new { ProductId = item.ProductId, BuyCardinality = ((MaxCount - count) < 0 ? 0 : MaxCount - count), ProductName = item.Name, Quantity = item.Quantity, Purchase = "1" });
                        }

                    }
                }
            }
            if (result.Count > 0)
            {
                context.Response.ContentType = "application/json";
                context.Response.Write(JsonConvert.SerializeObject(result));
                return;
            }

            var productIds = shoppingCart.LineItems.Select(p => p.ProductId).Distinct();
            if (productIds.Count() <= 0)
            {
                return;
            }

            Dictionary<int, int> dict = ProductHelper.GetBuyCardinality(productIds.ToArray());
            if (dict == null || dict.Count == 0)
            {
                return;
            }


            foreach (KeyValuePair<int, int> item in dict)
            {
                string productName = shoppingCart.LineItems.First(p => p.ProductId == item.Key).Name;
                int quantity = shoppingCart.LineItems.Where(p => p.ProductId == item.Key).Sum(p => p.Quantity);
                if (quantity >= item.Value)
                {
                    continue;
                }

                result.Add(new { ProductId = item.Key, BuyCardinality = item.Value, ProductName = productName, Quantity = quantity });
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(result));
        }
        private void GetVshopPromotional(System.Web.HttpContext context)
        {
            var unselectedProductId = context.Request["productIds"];
            var shoppingCart = VshopBrowser.GetAllPromotional(ClientType.VShop);
            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                return;
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(shoppingCart));
        }

        private void AddSupplierFav(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Member member = HiContext.Current.User as Member;
            int supplierId = System.Convert.ToInt32(context.Request["SupplierId"]);
            if (member == null)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"请先登录才可以收藏店铺\"}");
                return;
            }
            SupplierCollectInfo info = new SupplierCollectInfo();
            info.UserId = member.UserId;
            info.SupplierId = supplierId;
            info.Remark = "";

            bool isCollect = SupplierHelper.SupplierIsCollect(member.UserId, supplierId);
            if (!isCollect)
            {
                int favoriteId = SupplierHelper.CollectSupplier(info);
                if (favoriteId > 0)
                {
                    context.Response.Write("{\"success\":true,\"favoriteId\":" + favoriteId + "}");
                    return;
                }
                context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"已收藏\"}");
            }
        }

        private void DelCollectSupplier(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Member member = HiContext.Current.User as Member;
            int supplierId = System.Convert.ToInt32(context.Request["SupplierId"]);
            if (member == null)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"请先登录\"}");
                return;
            }



            bool isCollect = SupplierHelper.DelCollectSupplier(member.UserId, supplierId);
            if (!isCollect)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"操作失败\"}");
            }
            else
            {
                context.Response.Write("{\"success\":true, \"msg\":\"操作成功\"}");
            }
        }



        /// <summary>
        /// 显示第所有级商品分类
        /// </summary>
        /// <param name="context"></param>
        public void ShowAllCategory(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string categoryStr = "";

            string categoryId = context.Request["categoryId"];
            StringBuilder stringBuilder = new StringBuilder();

            //为原产地数据
            if (categoryId == "ImportSourceType")
            {
                IList<ImportSourceTypeInfo> list = ImportSourceTypeHelper.GetAllImportSourceTypes();

                if (list == null)
                {
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"Status\":\"-1\"");
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                    return;
                }

                categoryStr += Newtonsoft.Json.JsonConvert.SerializeObject(list);

                stringBuilder.Append(categoryStr.ToString());

            }
            else
            {

                IList<CategoryInfo> secondList = CatalogHelper.GetAllCategorieList();

                if (secondList == null)
                {
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"Status\":\"-1\"");
                    stringBuilder.Append("}");
                    context.Response.Write(stringBuilder.ToString());
                    return;
                }

                categoryStr += Newtonsoft.Json.JsonConvert.SerializeObject(secondList);

                stringBuilder.Append(categoryStr.ToString());
            }



            context.Response.Write(stringBuilder.ToString());
        }




        /// <summary>
        /// 显示分类，品牌，原产地
        /// </summary>
        /// <param name="context"></param>
        public void ShowCategoryBrands(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string tempcategoryId = context.Request["categoryId"];
            string tempsupplierid = context.Request["supplierid"];
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");

            int categoryId = 0;
            int supplierid = 0;
            int.TryParse(tempcategoryId, out categoryId);
            int.TryParse(tempsupplierid, out supplierid);
            List<ImportSourceTypeInfo> importSources = null;

            if (supplierid == 0)
            {
                importSources = ImportSourceTypeHelper.GetAllImportSourceTypes().ToList();
            }
            else
            {
                importSources = ImportSourceTypeHelper.GetAllImportSourceBySupplierId(supplierid).ToList();
            }

            if (importSources != null)
            {
                stringBuilder.Append("\"importSources\":");
                stringBuilder.Append(Newtonsoft.Json.JsonConvert.SerializeObject(importSources));
            }
            else
            {
                stringBuilder.Append("\"importSources\":\"\",");
                stringBuilder.Append("\"importSourcesStatus\":\"-1\"");
            }

            stringBuilder.Append(",");

            #region == 品牌
            //DataTable brands = null;

            //if (categoryId == 0)
            //{
            //    brands = CatalogHelper.GetBrandCategories();
            //}
            //else
            //{
            //    brands = CatalogHelper.GetBrandCategories(categoryId);
            //}
            //if (brands != null && brands.Rows.Count > 0)
            //{
            //    stringBuilder.Append("\"brands\":");
            //    stringBuilder.Append(Newtonsoft.Json.JsonConvert.SerializeObject(brands));
            //}
            //else
            //{
            //    stringBuilder.Append("\"brands\":\"\",");
            //    stringBuilder.Append("\"brandsSourcesStatus\":\"-1\"");
            //}
            //stringBuilder.Append(",");
            stringBuilder.Append("\"brands\":\"\",");
            stringBuilder.Append("\"brandsSourcesStatus\":\"-1\"");
            stringBuilder.Append(",");
            #endregion
            //为分类数据
            IList<CategoryInfo> secondList = CatalogHelper.GetAllCategorieList();
            if (secondList != null && secondList.Count > 0)
            {
                stringBuilder.Append("\"Categorys\":");
                stringBuilder.Append(Newtonsoft.Json.JsonConvert.SerializeObject(secondList));
            }
            else
            {
                stringBuilder.Append("\"Categorys\":\"\",");
                stringBuilder.Append("\"categoryStatus\":\"-1\"");
            }

            stringBuilder.Append("}");


            context.Response.Write(stringBuilder.ToString());
        }


        private void ProcessDeleteSelectCartProduct(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string skuId = context.Request["skuIds"];
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            string[] arr = skuId.Split(',');
            foreach (string id in arr)
            {
                ShoppingCartProcessor.RemoveLineItem(id);
            }
            stringBuilder.Append("{");
            stringBuilder.Append("\"Status\":\"OK\",");
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            int cartQuantity = 0;
            if (shoppingCart != null)
            {
                cartQuantity = shoppingCart.GetQuantity();
            }
            stringBuilder.AppendFormat("\"Quantity\":\"{0}\"", cartQuantity);
            stringBuilder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(stringBuilder.ToString());
        }


        /// <summary>
        /// 热搜关键字
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
            DbQueryResult dq = StoreHelper.GetHotKeywords(ClientType.VShop, page);
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
        /// 删除历史搜索
        /// </summary>
        /// <param name="context"></param>
        public void DelHistorySearch(System.Web.HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int userId = HiContext.Current.User.UserId;
            if (userId > 0)
            {
                int amount = HistorySearchHelp.DeleteSearchHistory(userId, ClientType.VShop);
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


        private void LoadMoreSuppliers(System.Web.HttpContext context)
        {
            #region 获取查询条件
            int total = 0;
            int pageNumber = 1;
            int pageSize = 0;

            int.TryParse(context.Request["size"], out pageSize);

            int.TryParse(context.Request["pageNumber"], out pageNumber);//页码
            string supplierName = Globals.UrlDecode(context.Request["keyWord"]);

            #endregion

            Member member = HiContext.Current.User as Member;
            int supplierId = System.Convert.ToInt32(context.Request["SupplierId"]);
            if (member == null)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"请先登录\"}");
                return;
            }

            SupplierQuery query = new SupplierQuery();
            query.SupplierName = supplierName;
            query.PageIndex = pageNumber;
            query.PageSize = pageSize;
            query.UserId = member.UserId;

            DbQueryResult dr = SupplierHelper.GetAppSupplier(query);
            DataTable dt = dr.Data as DataTable;
            int totalPage = 0;
            total = dr.TotalRecords;
            totalPage = total % pageSize != 0 ? (total / pageSize + 1) : (total / pageSize);//总页数
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{\"suppliers\":");
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add("CountyName", Type.GetType("System.String"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow newRow = dt.Rows[i];
                    int countyid = 0;
                    int.TryParse(dt.Rows[i]["County"].ToString(), out countyid);
                    newRow["CountyName"] = EcShop.Entities.RegionHelper.GetFullRegion(countyid, ",").Split(',')[0];
                }

                string strProducts = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                stringBuilder.Append(strProducts);
            }
            else
            {
                stringBuilder.Append("\"\"");
            }
            stringBuilder.Append(",");
            stringBuilder.Append("\"totalPage\":");
            stringBuilder.Append(totalPage);
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }


        private void LoadMoreFavProducts(System.Web.HttpContext context)
        {
            #region 获取查询条件
            int total = 0;
            int pageNumber = 1;
            int pageSize = 0;
            int.TryParse(context.Request["size"], out pageSize);
            int.TryParse(context.Request["pageNumber"], out pageNumber);//页码

            #endregion
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"请先登录\"}");
                return;
            }

            ProductFavoriteQuery query = new ProductFavoriteQuery();
            query.PageIndex = pageNumber;
            query.PageSize = pageSize;
            query.UserId = member.UserId;
            query.GradeId = member.GradeId;

            DbQueryResult dr = ProductBrowser.GetFavorites(query);
            DataTable dt = dr.Data as DataTable;
            int totalPage = 0;
            total = dr.TotalRecords;
            totalPage = total % pageSize != 0 ? (total / pageSize + 1) : (total / pageSize);//总页数
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{\"favproducts\":");
            if (dt.Rows.Count > 0)
            {
                string strProducts = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                stringBuilder.Append(strProducts);
            }
            else
            {
                stringBuilder.Append("\"\"");
            }
            stringBuilder.Append(",");
            stringBuilder.Append("\"totalPage\":");
            stringBuilder.Append(totalPage);
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }

        private void LoadMoreFavSuppliers(System.Web.HttpContext context)
        {
            #region 获取查询条件
            int total = 0;
            int pageNumber = 1;
            int pageSize = 0;
            int.TryParse(context.Request["size"], out pageSize);
            int.TryParse(context.Request["pageNumber"], out pageNumber);//页码

            #endregion
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"请先登录\"}");
                return;
            }

            SupplierCollectQuery query = new SupplierCollectQuery();
            query.UserId = member.UserId;
            query.PageIndex = pageNumber;
            query.PageSize = pageSize;

            DbQueryResult dr = SupplierHelper.GetSupplierCollect(query);

            DataTable dt = dr.Data as DataTable;
            int totalPage = 0;
            total = dr.TotalRecords;
            totalPage = total % pageSize != 0 ? (total / pageSize + 1) : (total / pageSize);//总页数
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{\"favsuppliers\":");
            if (dt.Rows.Count > 0)
            {
                string strProducts = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                stringBuilder.Append(strProducts);
            }
            else
            {
                stringBuilder.Append("\"\"");
            }
            stringBuilder.Append(",");
            stringBuilder.Append("\"totalPage\":");
            stringBuilder.Append(totalPage);
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }

        private void LoadMoreUserPoints(System.Web.HttpContext context)
        {
            #region 获取查询条件
            int total = 0;
            int pageNumber = 1;
            int pageSize = 0;
            int.TryParse(context.Request["size"], out pageSize);
            int.TryParse(context.Request["pageNumber"], out pageNumber);//页码

            #endregion
            Member member = HiContext.Current.User as Member;
            if (member == null)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"请先登录\"}");
                return;
            }


            DbQueryResult dr = TradeHelper.GetUserPoints(member.UserId, pageNumber, pageSize);

            DataTable dt = dr.Data as DataTable;
            int totalPage = 0;
            total = dr.TotalRecords;
            totalPage = total % pageSize != 0 ? (total / pageSize + 1) : (total / pageSize);//总页数
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{\"myuserpoints\":");
            if (dt.Rows.Count > 0)
            {
                string strProducts = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                stringBuilder.Append(strProducts);
            }
            else
            {
                stringBuilder.Append("\"\"");
            }
            stringBuilder.Append(",");
            stringBuilder.Append("\"totalPage\":");
            stringBuilder.Append(totalPage);
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }

        /// <summary>
        /// 获取当前1.1-1.5号的抢购商品
        /// </summary>
        /// <param name="context"></param>
        public void ShowShopActivityData(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
            productBrowseQuery.IsCount = true;
            productBrowseQuery.PageIndex = 1;
            productBrowseQuery.PageSize = 1000;
            productBrowseQuery.SortBy = "DisplaySequence";
            productBrowseQuery.SortOrder = SortAction.Asc;
            DataTable countDownProductList = (DataTable)ProductBrowser.GetActivityProductList(productBrowseQuery).Data;
            var t = context.Request["times"];
            var times = t.Split(',');
            if (countDownProductList != null && countDownProductList.Rows.Count > 0)
            {
                IsoDateTimeConverter convert = new IsoDateTimeConverter();
                convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                DataRow[] temp1 = countDownProductList.Select(" begintime = '" + times[0] + "'");
                if (temp1.Count() > 0)
                {
                    DataTable dtNew1 = countDownProductList.Clone();
                    for (int i = 0; i < temp1.Length; i++)
                    {
                        dtNew1.ImportRow(temp1[i]);
                    }
                    stringBuilder.Append("\"Activity1Status\":true,\"ActivityData1\":");
                    stringBuilder.Append(Newtonsoft.Json.JsonConvert.SerializeObject(dtNew1, Newtonsoft.Json.Formatting.None, convert));
                    stringBuilder.Append(",");
                }
                var temp2 = countDownProductList.Select(" begintime = '" + times[1] + "'");
                if (temp2.Count() > 0)
                {
                    DataTable dtNew2 = countDownProductList.Clone();
                    for (int i = 0; i < temp2.Length; i++)
                    {
                        dtNew2.ImportRow(temp2[i]);
                    }
                    stringBuilder.Append("\"Activity2Status\":true,\"ActivityData2\":");
                    stringBuilder.Append(Newtonsoft.Json.JsonConvert.SerializeObject(dtNew2, Newtonsoft.Json.Formatting.None, convert));
                    stringBuilder.Append(",");
                }
                var temp3 = countDownProductList.Select(" begintime =  '" + times[2] + "'");
                if (temp3.Count() > 0)
                {
                    DataTable dtNew3 = countDownProductList.Clone();
                    for (int i = 0; i < temp3.Length; i++)
                    {
                        dtNew3.ImportRow(temp3[i]);
                    }
                    stringBuilder.Append("\"Activity3Status\":true,\"ActivityData3\":");
                    stringBuilder.Append(Newtonsoft.Json.JsonConvert.SerializeObject(dtNew3, Newtonsoft.Json.Formatting.None, convert));
                    stringBuilder.Append(",");
                }
                var temp4 = countDownProductList.Select(" begintime =  '" + times[3] + "'");
                if (temp4.Count() > 0)
                {
                    DataTable dtNew4 = countDownProductList.Clone();
                    for (int i = 0; i < temp4.Length; i++)
                    {
                        dtNew4.ImportRow(temp4[i]);
                    }
                    stringBuilder.Append("\"Activity4Status\":true,\"ActivityData4\":");
                    stringBuilder.Append(Newtonsoft.Json.JsonConvert.SerializeObject(dtNew4, Newtonsoft.Json.Formatting.None, convert));
                    stringBuilder.Append(",");
                }
                var temp5 = countDownProductList.Select(" begintime =  '" + times[4] + "'");
                if (temp5.Count() > 0)
                {
                    DataTable dtNew5 = countDownProductList.Clone();
                    for (int i = 0; i < temp5.Length; i++)
                    {
                        dtNew5.ImportRow(temp5[i]);
                    }
                    stringBuilder.Append("\"Activity5Status\":true,\"ActivityData5\":");
                    stringBuilder.Append(Newtonsoft.Json.JsonConvert.SerializeObject(dtNew5, Newtonsoft.Json.Formatting.None, convert));
                    stringBuilder.Append(",");
                }
            }
            else
            {
                stringBuilder.Append("\"Success\":\"false\"");
            }
            string curstr = stringBuilder.ToString();
            if (curstr.EndsWith(","))
            {
                curstr = curstr.Substring(0, curstr.Length - 1);
            }
            curstr += "}";
            //stringBuilder.Append("}");
            context.Response.Write(curstr);
        }


        /// <summary>
        /// 获取专题商品列表
        /// </summary>
        /// <param name="context"></param>
        public void GetTopicProduct(System.Web.HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            //int pageSize = new VTemplateHelper().GetTopicProductMaxNum();
            int PageIndex = int.Parse(context.Request["pageNumber"]);
            int PageSize = int.Parse(context.Request["size"]);

            stringBuilder.Append("{");
            int topicid;
            if (!int.TryParse(context.Request["topicId"], out  topicid))
            {
                stringBuilder.Append("\"Success\":0");
                stringBuilder.Append("}");
            }

            int total = 0;

            DataTable dt = ProductBrowser.GetProducts(topicid, null, "", PageIndex, PageSize, out total, " sort2 ", true, "asc");//分页查询


            if (dt.Rows.Count > 0)
            {
                stringBuilder.Append("\"Success\":1,");
                stringBuilder.Append("\"productlist\":");
                IsoDateTimeConverter convert = new IsoDateTimeConverter();
                convert.DateTimeFormat = "yyyy-MM-dd";
                string strproductlist = Newtonsoft.Json.JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.None, convert);
                stringBuilder.Append(strproductlist);
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
        /// 绑定手机号码
        /// </summary>
        /// <param name="context"></param>
        public void BindPhoneNumber(System.Web.HttpContext context)
        {
           

            string phonenumber = context.Request["phonenumber"];

            string smscode = context.Request["smscode"];

            string password = context.Request["password"];

            string invitecode = context.Request["invitecode"];

            #region ==数据验证
            if (string.IsNullOrEmpty(phonenumber))
            {
                context.Response.Write("{\"success\":false,\"msg\":\"手机号码不允许为空\"}");
                context.Response.End();
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(phonenumber, "^(13|14|15|17|18)\\d{9}$"))
            {
                context.Response.Write("{\"success\":false,\"msg\":\"请输入正确的手机号码\"}");
                context.Response.End();
            }
            if (string.IsNullOrEmpty(smscode))
            {
                context.Response.Write("{\"success\":false,\"msg\":\"手机验证码不允许为空\"}");
                context.Response.End();
            }

            if (!TelVerifyHelper.CheckVerify(phonenumber, smscode))
            {
                context.Response.Write("{\"success\":false,\"msg\":\"手机验证码验证错误\"}");
                context.Response.End();
            }

            if (UserHelper.IsExistCellPhoneAndUserName(phonenumber) > 0)
            {
                context.Response.Write("{\"success\":false,\"msg\":\"已经存在相同的手机号码\"}");
                context.Response.End();
            }

            #endregion 

            Member member = HiContext.Current.User as Member;
            string openId = member.OpenId;


            if (!string.IsNullOrWhiteSpace(invitecode))
            {
                if (!MemberHelper.IsExsitRecommendCode(invitecode.Trim().ToUpper(), member.UserId))
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"邀请码错误，请填写正确的邀请码或者选择不填\"}");
                    context.Response.End();
                }
            }

            if (!string.IsNullOrWhiteSpace(openId))
            {
                member.Username = phonenumber;
                member.Password = password;
                member.CellPhone = phonenumber;
                bool isSendCoupon = false;
                if (MemberHelper.UpdateUserNameCoupon(member, invitecode.Trim().ToUpper(), out isSendCoupon))
                {
                    Member curmember = Users.GetUser(member.UserId,false) as Member;
                    if (curmember != null)
                    {
                        //设置缓存
                        Hashtable hashtable = Users.UserCache();
                        hashtable[Users.UserKey(phonenumber)] = curmember;

                        string name = "Vshop-Member";
                        HttpCookie httpCookie = new HttpCookie("Vshop-Member");
                        httpCookie.Value = Globals.UrlEncode(phonenumber);
                        httpCookie.Expires = System.DateTime.Now.AddDays(7);
                        httpCookie.Domain = HttpContext.Current.Request.Url.Host;
                        if (HttpContext.Current.Response.Cookies[name] != null)
                        {
                            HttpContext.Current.Response.Cookies.Remove(name);
                        }
                        HttpContext.Current.Response.Cookies.Add(httpCookie);
                        if (isSendCoupon)
                        {
                            context.Response.Write("{\"success\":true,\"msg\":\"确定成功，恭喜您，50元现金券已经打到您的海美账户了，现在就去购物吧！\"}");
                        }
                        else
                        {
                            context.Response.Write("{\"success\":true,\"msg\":\"确定成功\"}");
                        }
                        context.Response.End();
                    }

                    else
                    {
                        context.Response.Write("{\"success\":false,\"msg\":\"获取会员信息失败\"}");
                        context.Response.End();
                    }
                }

                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"确定失败\"}");
                    context.Response.End();
                }

            }

            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"获取不到openid\"}");
                context.Response.End();
            }
        }

        public void ShowSupplierThreeCategory(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string str = "";

            int supplierId = 0;
            int.TryParse(context.Request["SupplierId"], out supplierId);
            StringBuilder stringBuilder = new StringBuilder();

            if (supplierId <= 0)
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }

            // 根据供应商ID(店铺ID)获取其最后一层的商品分类名称
            List<CategoryInfo> categories = CategoryBrowser.GetCategoryiesBySupplierId(supplierId).ToList();

            if (categories != null && categories.Count > 0)
            {
                str = Newtonsoft.Json.JsonConvert.SerializeObject(categories);
                stringBuilder.Append(str.ToString());
            }
            else
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }

            context.Response.Write(stringBuilder.ToString());
        }


        public void ShowTwoCategory(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string categoryStr = "";

            int categoryId = 0;//800225
            int.TryParse(context.Request["CategoryId"], out categoryId);
            StringBuilder stringBuilder = new StringBuilder();

            if (categoryId <= 0)
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }

            // 根据类别ID获取类别以及子类的类别信息
            List<CategoryInfo> categoryList = CategoryBrowser.GetTwoCategoryByCategoryId(categoryId);

            if (categoryList != null && categoryList.Count > 0)
            {
                categoryStr = Newtonsoft.Json.JsonConvert.SerializeObject(categoryList);
                stringBuilder.Append(categoryStr.ToString());
            }   

            context.Response.Write(stringBuilder.ToString());
        }

        public void ShowThirdCategoryOrBrand(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string str = "";

            int categoryId = 0;
            int brandId = 0;
            int.TryParse(context.Request["CategoryId"], out categoryId);
            int.TryParse(context.Request["BrandId"], out brandId);
            StringBuilder stringBuilder = new StringBuilder();

            if (categoryId <= 0)
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }

            List<CategoryInfo> listCategory = new List<CategoryInfo>();
            List<BrandCategoryInfo> listBrand = new List<BrandCategoryInfo>();
            if (categoryId > 0) //CategoryType.Brand
            {
                // 类别
                listCategory = CategoryBrowser.GetThreeCategoryByCategoryId(categoryId);
                if (listCategory != null && listCategory.Count > 0)
                {
                    str = Newtonsoft.Json.JsonConvert.SerializeObject(listCategory);
                    stringBuilder.Append(str.ToString());
                }
            }
            else if (brandId > 0)
            {
                // 品牌
                listBrand = CategoryBrowser.GetSecondBrandByCategoryId(categoryId);
                if (listBrand != null && listBrand.Count > 0)
                {
                    str = Newtonsoft.Json.JsonConvert.SerializeObject(listBrand);
                    stringBuilder.Append(str.ToString());
                }
            }

            context.Response.Write(stringBuilder.ToString());
        }
    }
}

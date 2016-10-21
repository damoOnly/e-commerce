using ASPNET.WebControls;
using Ecdev.Weixin.Pay;
using Ecdev.Weixin.Pay.Domain;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.Messages;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
namespace EcShop.UI.AccountCenter.CodeBehind
{
    public class UserOrders : MemberTemplatedWebControl
    {
        private WebCalendar calendarStartDate;
        private WebCalendar calendarEndDate;
        private System.Web.UI.WebControls.TextBox txtOrderId;
        private System.Web.UI.WebControls.TextBox txtProductName;
        private System.Web.UI.WebControls.TextBox txtShipId;
        private System.Web.UI.WebControls.TextBox txtShipTo;
        private System.Web.UI.WebControls.TextBox txtCellPhone;
        private OrderStautsDropDownList dropOrderStatus;
        private System.Web.UI.WebControls.DropDownList dropPayType;
        private System.Web.UI.WebControls.ImageButton imgbtnSearch;
        private IButton btnPay;
        private IButton btnOk;
        private IButton btnReturn;
        private IButton btnReplace;
        private System.Web.UI.WebControls.Literal litOrderTotal;
        private System.Web.UI.HtmlControls.HtmlInputHidden hdorderId;
        private System.Web.UI.WebControls.TextBox txtRemark;
        private System.Web.UI.WebControls.TextBox txtReturnRemark;
        private System.Web.UI.WebControls.TextBox txtReplaceRemark;
        private Common_OrderManage_OrderList listOrders;
        private System.Web.UI.WebControls.DropDownList dropRefundType;
        private System.Web.UI.WebControls.DropDownList dropRefundReason;//退款原因
        private System.Web.UI.WebControls.DropDownList dropReturnReason;//退货原因
        private System.Web.UI.WebControls.DropDownList dropReturnRefundType;
        private Pager pager;
        protected System.Web.UI.WebControls.HyperLink hlinkAllOrder;
        protected System.Web.UI.WebControls.HyperLink hlinkNotPay;
        protected System.Web.UI.WebControls.HyperLink hlinkNotGetGoods;
        protected System.Web.UI.WebControls.HyperLink hlinkRefund;
        protected System.Web.UI.WebControls.HyperLink hlinkReturn;
        protected System.Web.UI.WebControls.HyperLink hlinkFinished;
        protected System.Web.UI.WebControls.Label lb_refundtitle;

        private LogisticsCompanyDropDownList dropLogisticsCompany;//物流公司
        private System.Web.UI.WebControls.TextBox txtLogisticsId;//物流单号
        /// <summary>
        /// 退货的skuids
        /// </summary>
        private System.Web.UI.HtmlControls.HtmlInputHidden skuIds;
        /// <summary>
        /// 退货的数量s
        /// </summary>
        private System.Web.UI.HtmlControls.HtmlInputHidden quantityList;
        /// <summary>
        /// 保存选择的物流公司
        /// </summary>
        private System.Web.UI.HtmlControls.HtmlInputHidden LogisticsCompany;
        private System.Web.UI.HtmlControls.HtmlInputHidden LogisticsId;

        private int OrderRefundTime = 0;
        //private VshopTemplatedRepeater rptOrderProducts;

        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserOrders.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.calendarStartDate = (WebCalendar)this.FindControl("calendarStartDate");
            this.calendarEndDate = (WebCalendar)this.FindControl("calendarEndDate");
            this.hdorderId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdorderId");
            this.txtOrderId = (System.Web.UI.WebControls.TextBox)this.FindControl("txtOrderId");
            this.txtProductName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtProductName");
            this.txtShipId = (System.Web.UI.WebControls.TextBox)this.FindControl("txtShipId");
            this.txtShipTo = (System.Web.UI.WebControls.TextBox)this.FindControl("txtShipTo");
            this.txtCellPhone = (System.Web.UI.WebControls.TextBox)this.FindControl("txtCellPhone");
            this.txtRemark = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRemark");
            this.txtReturnRemark = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReturnRemark");
            this.txtReplaceRemark = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReplaceRemark");
            this.dropOrderStatus = (OrderStautsDropDownList)this.FindControl("dropOrderStatus");
            this.dropPayType = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropPayType");
            this.btnPay = ButtonManager.Create(this.FindControl("btnPay"));
            this.imgbtnSearch = (System.Web.UI.WebControls.ImageButton)this.FindControl("imgbtnSearch");
            this.btnOk = ButtonManager.Create(this.FindControl("btnOk"));
            this.btnReturn = ButtonManager.Create(this.FindControl("btnReturn"));
            this.btnReplace = ButtonManager.Create(this.FindControl("btnReplace"));
            this.litOrderTotal = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderTotal");
            this.dropRefundType = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropRefundType");
            this.dropRefundReason = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropRefundReason");
            this.dropReturnReason = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropReturnReason");
            this.dropReturnRefundType = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropReturnRefundType");
            this.listOrders = (Common_OrderManage_OrderList)this.FindControl("Common_OrderManage_OrderList");
            this.pager = (Pager)this.FindControl("pager");

            this.hlinkAllOrder = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkAllOrder");
            this.hlinkNotPay = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkNotPay");
            this.hlinkNotGetGoods = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkNotGetGoods");
            this.hlinkFinished = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkFinished");

            this.hlinkRefund = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkRefund");
            this.hlinkReturn = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkReturn");

            this.dropLogisticsCompany = (LogisticsCompanyDropDownList)this.FindControl("dropLogisticsCompany");
            this.txtLogisticsId = (System.Web.UI.WebControls.TextBox)this.FindControl("txtLogisticsId");
            this.quantityList = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("quantityList");
            this.skuIds = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("skuIds");
            this.LogisticsCompany = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("LogisticsCompany");
            this.LogisticsId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("LogisticsId");


            this.imgbtnSearch.Click += new System.Web.UI.ImageClickEventHandler(this.imgbtnSearch_Click);
            this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            this.listOrders.ItemDataBound += new Common_OrderManage_OrderList.DataBindEventHandler(this.listOrders_ItemDataBound);
            this.listOrders.ItemCommand += new Common_OrderManage_OrderList.CommandEventHandler(this.listOrders_ItemCommand);
            //this.rptOrderProducts = (VshopTemplatedRepeater)this.FindControl("rptOrderProducts");
            //物流选择绑定值
            //IList<string> list = ExpressHelper.GetAllExpressName();
            //List<ListItem> item = new List<ListItem>();
            //foreach (string s in list)
            //{
            //    item.Add(new ListItem(s, s));
            //}
            //dropLogisticsCompany.Items.AddRange(item.ToArray());

            PageTitle.AddSiteNameTitle("我的订单");
            if (!this.Page.IsPostBack)
            {
                this.OrderRefundTime=string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["OrderRefunTime"]) ? 30 : int.Parse(System.Configuration.ConfigurationManager.AppSettings["OrderRefunTime"].ToString());
                this.SetOrderStatusLink();
                this.dropPayType.DataSource = TradeHelper.GetPaymentModes(PayApplicationType.payOnPC);
                this.dropPayType.DataTextField = "Name";
                this.dropPayType.DataValueField = "ModeId";
                this.dropPayType.DataBind();
                this.BindOrders();
                dropLogisticsCompany.DataBind();
                BindRefundReason();
                BindReturnReason();
            }
            //this.rptOrderProducts.DataSource = ShoppingProcessor.GetOrderItems(orderId);
            //this.rptOrderProducts.DataBind();
        }
        private void BindRefundReason()
        {
            var reasons = TradeHelper.GetOrderHandleReason(OrderHandleReasonType.Refund);
            this.dropRefundReason.DataSource = reasons;
            this.dropRefundReason.DataBind();

            this.dropRefundReason.Items.Insert(this.dropRefundReason.Items.Count, "其他原因...");
        }
        private void BindReturnReason()
        {
            var reasons = TradeHelper.GetOrderHandleReason(OrderHandleReasonType.Return);
            this.dropReturnReason.DataSource = reasons;
            this.dropReturnReason.DataBind();

            this.dropReturnReason.Items.Insert(this.dropReturnReason.Items.Count, "其他原因...");
        }
        private void SetOrderStatusLink()
        {
            string format = Globals.ApplicationPath + "/user/UserOrders.aspx?orderStatus={0}";
            //全部订单
            this.hlinkAllOrder.NavigateUrl = string.Format(format, (int)OrderStatus.All);
            //待付款(等待买家付款)
            this.hlinkNotPay.NavigateUrl = string.Format(format, (int)OrderStatus.WaitBuyerPay);
            //待收货(已发货)
            this.hlinkNotGetGoods.NavigateUrl = string.Format(format, (int)OrderStatus.SellerAlreadySent);
            //已完成
            this.hlinkFinished.NavigateUrl = string.Format(format, (int)OrderStatus.Finished);
            //退款单
            this.hlinkRefund.NavigateUrl = string.Format(format, (int)OrderStatus.Refunded);
            //退货单
            this.hlinkReturn.NavigateUrl = string.Format(format, (int)OrderStatus.Returned);

        }

        private void btnPay_Click(object sender, System.EventArgs e)
        {
            string value = this.hdorderId.Value;
            int modeId = 0;
            int.TryParse(this.dropPayType.SelectedValue, out modeId);
            PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(modeId);

            if (paymentMode != null)
            {
                OrderInfo orderInfo = TradeHelper.GetOrderInfo(value);
                orderInfo.PaymentTypeId = paymentMode.ModeId;
                orderInfo.PaymentType = paymentMode.Name;
                orderInfo.Gateway = paymentMode.Gateway;
                TradeHelper.UpdateOrderPaymentType(orderInfo);
                orderInfo = TradeHelper.GetOrderInfo(value);
            }

            try
            {
                List<OrderInfo> listChildOrder = ShoppingProcessor.GetChildOrdersBySourceOrder(value);
                string orderIdstr = "";
                List<string> orderIdIdList = new List<string>();
                if (listChildOrder != null && listChildOrder.Count > 0)
                {
                    listChildOrder.ForEach(t =>
                    {
                        //单品券
                        if (!string.IsNullOrWhiteSpace(t.OrderId))
                        {
                            string ordeId = "'"+t.OrderId+"'";
                            orderIdIdList.Add(ordeId);
                        }
                    });
                }

                if (orderIdIdList != null && orderIdIdList.Count > 0)
                {
                    orderIdstr = string.Join(",", orderIdIdList.Distinct().ToArray());
                }

                if (!string.IsNullOrWhiteSpace(orderIdstr))
                {
                    if (paymentMode != null)
                    {
                        OrderInfo orderInfo = new OrderInfo();
                        orderInfo.PaymentTypeId = paymentMode.ModeId;
                        orderInfo.PaymentType = paymentMode.Name;
                        orderInfo.Gateway = paymentMode.Gateway;
                        TradeHelper.ModifyOrderPaymentType(orderInfo, orderIdstr);
                    }
                }
            }
            catch
            {
                
            }

            //判断为微信扫码支付
            //if (paymentMode.Settings == "1hSUSkKQ/ENo0JDZah8KKQweixin")
            if (paymentMode.Gateway.ToLower() == "Ecdev.plugins.payment.WxpayQrCode.QrCodeRequest".ToLower())
            {
                Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                this.Page.Response.Redirect("/FinishOrder.aspx?orderId=" + value + "&umid=" + member.UserId.ToString());
            }
            else
            {
                this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("sendPayment", new object[]
			{
				value
			}));
            }
        }

        private void btnReplace_Click(object sender, System.EventArgs e)
        {
            if (!TradeHelper.CanReplace(this.hdorderId.Value))
            {
                this.ShowMessage("已有待确认的申请！", false);
                return;
            }
            if (TradeHelper.ApplyForReplace(this.hdorderId.Value, this.txtReplaceRemark.Text))
            {
                this.BindOrders();
                this.ShowMessage("成功的申请了换货", true);
                return;
            }
            this.ShowMessage("申请换货失败", false);
        }
        private void btnReturn_Click(object sender, System.EventArgs e)
        {
            if (!TradeHelper.CanReturn(this.hdorderId.Value))
            {
                this.ShowMessage("已有待确认的申请！", false);
                return;
            }
            if (!this.CanReturnBalance())
            {
                this.ShowMessage("请先开通预付款账户", false);
                return;
            }
            string dropLogisticsCompanyVal = LogisticsCompany.Value; //dropLogisticsCompany.SelectedItem.Text;
            string txtLogisticsIdVal = LogisticsId.Value;
            string skuids = skuIds.Value;
            string quantitylist = quantityList.Value;
            //if (TradeHelper.ApplyForReturn(this.hdorderId.Value, this.txtReturnRemark.Text, int.Parse(this.dropReturnRefundType.SelectedValue), dropLogisticsCompanyVal,txtLogisticsIdVal))
            //{
            //    this.BindOrders();
            //    this.ShowMessage("成功的申请了退货", true);
            //    return;
            //}
            string reason = string.Empty;
            if (this.dropReturnReason.SelectedIndex == this.dropReturnReason.Items.Count - 1)
            {
                reason = this.txtReturnRemark.Text;
            }
            else
            {
                reason = this.dropReturnReason.Text + Environment.NewLine + this.txtReturnRemark.Text;
            }
            if (TradeHelper.CreateReturnsEntityAndAdd(this.hdorderId.Value, reason, int.Parse(this.dropReturnRefundType.SelectedValue), skuids, quantitylist, dropLogisticsCompanyVal, txtLogisticsIdVal))
            {
                this.BindOrders();
                this.ShowMessage("成功的申请了退货", true);
                return;
            }
            this.ShowMessage("申请退货失败", false);
        }
        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (!TradeHelper.CanRefund(this.hdorderId.Value))
            {
                this.ShowMessage("已有待确认的申请！", false);
                return;
            }
            if (!this.CanRefundBalance())
            {
                this.ShowMessage("请先开通预付款账户", false);
                return;
            }
            OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.hdorderId.Value);
            //if (orderInfo.InClearance)
            //{
            //    this.BindOrders();
            //    this.ShowMessage("清关中，不支持退款", false);
            //    return;
            //}
            string reason = this.dropRefundReason.SelectedIndex == this.dropRefundReason.Items.Count - 1 ? this.txtRemark.Text : this.dropRefundReason.Text;

            string flagMsg = "";

            if (TradeHelper.ApplyForRefund(this.hdorderId.Value, reason, int.Parse(this.dropRefundType.SelectedValue), out flagMsg))
            {
                this.BindOrders();
                this.ShowMessage("成功的申请了退款", true);
                return;
            }
            this.ShowMessage("申请退款失败 " + flagMsg, false);
        }
        private bool CanReturnBalance()
        {
            if (System.Convert.ToInt32(this.dropReturnRefundType.SelectedValue) != 1)
            {
                return true;
            }
            Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
            return member.IsOpenBalance;
        }
        private bool CanRefundBalance()
        {
            if (System.Convert.ToInt32(this.dropRefundType.SelectedValue) != 1)
            {
                return true;
            }
            Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
            return member.IsOpenBalance;
        }
        public List<string> dic = new List<string>();
        protected void listOrders_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                OrderStatus orderStatus = (OrderStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderStatus");

                System.DateTime t = (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "FinishDate") == System.DBNull.Value) ? System.DateTime.Now.AddYears(-1) : ((System.DateTime)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "FinishDate"));
                string a = "";
                if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway") != null && !(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway") is System.DBNull))
                {
                    a = (string)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway");
                }

                System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("hplinkorderreview");

                System.Web.UI.HtmlControls.HtmlAnchor linkNowPay = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("hlinkPay");

                ImageLinkButton confirmOrder = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmOrder");
                ImageLinkButton cancelOrder = (ImageLinkButton)e.Item.FindControl("lkbtnCloseOrder");
                
                
                //ImageLinkButton refundOrder = (ImageLinkButton)e.Item.FindControl("lkbtnRefund");
                System.Web.UI.WebControls.LinkButton refundOrder = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("lkbtnRefund");
                System.Web.UI.HtmlControls.HtmlAnchor lkbtnApplyForRefund = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnApplyForRefund");
                System.Web.UI.HtmlControls.HtmlAnchor lkbtnApplyForReturn = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnApplyForReturn");
                System.Web.UI.HtmlControls.HtmlAnchor lkbtnNotPay = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnNotPay");
                System.Web.UI.HtmlControls.HtmlAnchor lkbtnApplyForReplace = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnApplyForReplace");
                lb_refundtitle = (System.Web.UI.WebControls.Label)e.Item.FindControl("lb_refundtitle");
                System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rpProduct");
                //查看物流
                //System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Item.FindControl("Logistics");
                if (hyperLink != null)
                {
                    hyperLink.Visible = (orderStatus == OrderStatus.Finished);
                }
               

                //立即付款
                linkNowPay.Visible = (orderStatus == OrderStatus.WaitBuyerPay && a != "ecdev.plugins.payment.podrequest");
                //确认收货
                confirmOrder.Visible = (orderStatus == OrderStatus.SellerAlreadySent);
                if (cancelOrder != null)
                {
                    //取消订单
                    cancelOrder.Visible = (orderStatus == OrderStatus.WaitBuyerPay);
                }
                string SourceOrderId = ((DataRowView)e.Item.DataItem).Row["SourceOrderId"].ToString();
                string orderId = ((DataRowView)e.Item.DataItem).Row["OrderId"].ToString();
                string IsRefund = ((DataRowView)e.Item.DataItem).Row["IsRefund"].ToString();
              
                //申请退款
                if (repeater != null&&orderStatus==OrderStatus.BuyerAlreadyPaid)
                {
                    string PayDate = ((DataRowView)e.Item.DataItem).Row["PayDate"].ToString();
                    if (!string.IsNullOrWhiteSpace(PayDate))
                    {
                        if (Convert.ToDateTime(PayDate) > DateTime.Now.AddMinutes(-OrderRefundTime) && IsRefund == "0")//可以取消
                        {
                            refundOrder.Visible = true;
                            refundOrder.OnClientClick = "return IsRefund('" + orderId + "','" + SourceOrderId + "')";
                        }
                    }
                }
                if (IsRefund == "1")
                {
                    this.lb_refundtitle.Visible = true;
                    this.lb_refundtitle.Text = "退款成功";
                }
                if (IsRefund == "2")
                {
                    this.lb_refundtitle.Visible = true;
                    this.lb_refundtitle.Text = "退款中";
                }
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                //未付款
                lkbtnNotPay.Visible = (orderStatus == OrderStatus.WaitBuyerPay);
                //退货
                //lkbtnApplyForReturn.Visible = (orderStatus == OrderStatus.Finished && t >= System.DateTime.Now.AddDays((double)(-(double)masterSettings.EndOrderDays)));

                //退款
                //lkbtnApplyForRefund.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid || orderStatus == OrderStatus.SellerAlreadySent);
                //object payDate = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "PayDate");
                //if (OrderInfo.IsInClearance(payDate))
                //{
                //    lkbtnApplyForReturn.Visible = true;
                //    lkbtnApplyForRefund.Visible = false;
                //}
                //换货
                //lkbtnApplyForReplace.Visible = (orderStatus == OrderStatus.Finished && t >= System.DateTime.Now.AddDays((double)(-(double)masterSettings.EndOrderDays)));

                /*1.等待买家付款（WaitBuyerPay） ->        未付款/取消     
                  2.已付款,等待发货（BuyerAlreadyPaid） -> 退款        
                  3.已发货（SellerAlreadySent）          ->退款,确认收货，查看物流     
                  4.订单已完成（Finished）  -> 退货/换货 查看物流
                */

                //
                if (repeater != null)
                {
                  
                    if (!string.IsNullOrEmpty(orderId))
                    {
                        DataTable dt = TradeHelper.GetOrderItemThumbnailsUrl(orderId);
                        repeater.DataSource = dt;
                        repeater.DataBind();
                    }

                }
                if (orderStatus == OrderStatus.SellerAlreadySent || orderStatus == OrderStatus.Finished || orderStatus == OrderStatus.Returned || orderStatus == OrderStatus.ApplyForReplacement || orderStatus == OrderStatus.ApplyForReturns)
                {
                    //查看物流
                    //label.Visible = true;
                }
            }
        }
        protected void listOrders_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            string orderId = e.CommandArgument.ToString();
            OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
            if (orderInfo != null)
            {
                if (e.CommandName == "FINISH_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
                {
                    if (TradeHelper.ConfirmOrderFinish(orderInfo))
                    {
                        this.BindOrders();
                        this.ShowMessage("成功的完成了该订单", true);
                    }
                    else
                    {
                        this.ShowMessage("完成订单失败", false);
                    }
                }
                if (e.CommandName == "CLOSE_TRADE")
                {
                    if (orderInfo.CheckAction(OrderActions.SELLER_CLOSE))
                    {
                        if (TradeHelper.CloseOrder(orderInfo.OrderId, CloseOrderType.Manually))
                        {
                            Messenger.OrderClosed(HiContext.Current.User, orderInfo, "用户自己关闭订单");
                            this.BindOrders();
                            this.ShowMessage("成功的关闭了该订单", true);
                            return;
                        }
                        this.ShowMessage("关闭订单失败", false);
                    }
                    this.BindOrders();              
                }
                if (e.CommandName == "CLOSE_Refund")//申请退款
                {
                    this.ReloadUserOrders(true);
                }
                //    string refundrest = string.Empty;
                //    OrderRefundTime = OrderRefundTime > 0 ? OrderRefundTime : 30;
                //    if (orderInfo.IsRefund == 2)
                //    {
                //        this.ShowMessage("您退款已申请成功，无需再提交！", true);
                //        return;
                //    }
                //    if (Convert.ToDateTime(orderInfo.PayDate) > DateTime.Now.AddMinutes(-OrderRefundTime) && orderInfo.IsRefund != 1 && (int)orderInfo.OrderStatus == 2)//可以取消
                //    {
                //        int type = string.IsNullOrEmpty(orderInfo.SourceOrderId) ? 1 : 2;
                //        orderId = type == 1 ? orderId : orderInfo.SourceOrderId;
                //        bool Rest = TradeHelper.ChangeRefundType(orderId, type);
                //        if (Rest)
                //        {
                //            this.Enabled = false;
                //            this.ShowMessage("退款申请成功", true);
                //            this.BindOrders();
                //        }
                //        else
                //        {
                //            this.ShowMessage("退款失败，", false);
                //        }
                //    }
                //    else
                //    {
                //        this.ShowMessage("您离付款时间已经超过" + OrderRefundTime + "分钟，不能自动退款！", false);
                //    }

                //}
            }

            //删除订单（逻辑删除，放到回收站）
            if (e.CommandName == "LogicDelete")
            {
                int result = TradeHelper.LogicDeleteOrder(orderId, (int)UserStatus.RecycleDelete);
                if (result > 0)
                {
                    this.BindOrders();
                    this.ShowMessage("删除订单成功", true);

                }
                else
                {
                    this.ShowMessage("删除订单失败", false);
                }
            }
        }
        protected void listOrders_ReBindData(object sender)
        {
            this.ReloadUserOrders(false);
        }
        protected void imgbtnSearch_Click(object sender, System.EventArgs e)
        {
            this.ReloadUserOrders(true);
        }
        private void BindOrders()
        {
            OrderQuery orderQuery = this.GetOrderQuery();
            DbQueryResult userOrder = TradeHelper.GetUserOrder(HiContext.Current.User.UserId, orderQuery);
            this.listOrders.DataSource = userOrder.Data;
            this.listOrders.DataBind();
            this.txtOrderId.Text = orderQuery.OrderId;
            this.txtProductName.Text = orderQuery.ProductName;
            this.txtShipId.Text = orderQuery.ShipId;
            this.txtShipTo.Text = orderQuery.ShipTo;
            this.txtCellPhone.Text = orderQuery.CellPhone;

            this.dropOrderStatus.SelectedValue = orderQuery.Status;
            this.calendarStartDate.SelectedDate = orderQuery.StartDate;
            this.calendarEndDate.SelectedDate = orderQuery.EndDate;
            this.pager.TotalRecords = userOrder.TotalRecords;
        }
        private OrderQuery GetOrderQuery()
        {
            OrderQuery orderQuery = new OrderQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
            {
                orderQuery.OrderId = this.Page.Request.QueryString["orderId"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shipId"]))
            {
                orderQuery.ShipId = this.Page.Request.QueryString["shipId"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shipTo"]))
            {
                orderQuery.ShipTo = this.Page.Request.QueryString["shipTo"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CellPhone"]))
            {
                orderQuery.CellPhone = this.Page.Request.QueryString["CellPhone"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
            {
                orderQuery.OrderId = this.Page.Request.QueryString["orderId"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                orderQuery.StartDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["startDate"])));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                orderQuery.EndDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["endDate"])));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderStatus"]))
            {
                int status = 0;
                if (int.TryParse(this.Page.Request.QueryString["orderStatus"], out status))
                {
                    orderQuery.Status = (OrderStatus)status;
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortBy"]))
            {
                orderQuery.SortBy = this.Page.Request.QueryString["sortBy"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
            {
                int sortOrder = 0;
                if (int.TryParse(this.Page.Request.QueryString["sortOrder"], out sortOrder))
                {
                    orderQuery.SortOrder = (SortAction)sortOrder;
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
                orderQuery.ProductName = this.Page.Request.QueryString["productName"];
            }
            orderQuery.UseStatus = UserStatus.DefaultStatus;
            orderQuery.PageIndex = this.pager.PageIndex;
            orderQuery.PageSize = this.pager.PageSize;
            return orderQuery;
        }
        private void ReloadUserOrders(bool isSearch)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("orderId", this.txtOrderId.Text.Trim());
            nameValueCollection.Add("shipId", this.txtShipId.Text.Trim());
            nameValueCollection.Add("shipTo", this.txtShipTo.Text.Trim());
            nameValueCollection.Add("CellPhone", this.txtCellPhone.Text.Trim());
            nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.ToString());
            nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.ToString());
            nameValueCollection.Add("orderStatus", ((int)this.dropOrderStatus.SelectedValue).ToString());
            nameValueCollection.Add("productName", this.txtProductName.Text.Trim());
            nameValueCollection.Add("sortOrder", ((int)this.listOrders.SortOrder).ToString());
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(nameValueCollection);
        }
    }
}

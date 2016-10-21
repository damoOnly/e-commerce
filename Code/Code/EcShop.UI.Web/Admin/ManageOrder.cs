using ASPNET.WebControls;
using Ecdev.Weixin.Pay;
using Ecdev.Weixin.Pay.Domain;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.Jobs;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using WxPayAPI;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Orders)]
    public class ManageOrder : AdminPage
    {
        protected System.Web.UI.WebControls.HyperLink hlinkAllOrder;
        protected System.Web.UI.WebControls.HyperLink hlinkNotPay;
        protected System.Web.UI.WebControls.HyperLink hlinkYetPay;
        protected System.Web.UI.WebControls.HyperLink hlinkSendGoods;
        protected System.Web.UI.WebControls.HyperLink hlinkTradeFinished;
        protected System.Web.UI.WebControls.HyperLink hlinkClose;
        protected System.Web.UI.WebControls.HyperLink hlinkHistory;
        protected System.Web.UI.WebControls.DropDownList DDL_IsRefund;
        protected WebCalendar calendarStartDate;
        protected HourDropDownList ddListStartHour;
        protected HourDropDownList ddListEndHour;
        protected WebCalendar calendarEndDate;
        protected System.Web.UI.WebControls.TextBox txtUserName;
        protected System.Web.UI.WebControls.TextBox txtOrderId;
        protected System.Web.UI.WebControls.Label lblStatus;
        protected System.Web.UI.WebControls.Label lblOrderTimeType;
        protected System.Web.UI.WebControls.TextBox txtProductName;
        protected System.Web.UI.WebControls.TextBox txtShopTo;
        protected System.Web.UI.WebControls.TextBox txtCellPhone;
        protected System.Web.UI.WebControls.DropDownList ddlIsPrinted;
        protected SupplierDropDownList ddlSupplier;//供货商
        protected ShippingModeDropDownList shippingModeDropDownList;
        protected SitesDropDownList sitesDropDownList;//站点
        protected RegionSelector dropRegion;
        protected SourceOrderDrowpDownList dropsourceorder;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected PageSize hrefPageSize;
        protected Pager pager1;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;
        protected System.Web.UI.WebControls.DataList dlstOrders;
        protected Pager pager;
        protected CloseTranReasonDropDownList ddlCloseReason;
        protected FormatedMoneyLabel lblOrderTotalForRemark;
        protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
        protected System.Web.UI.WebControls.TextBox txtRemark;
        protected System.Web.UI.WebControls.Label lblOrderId;
        protected System.Web.UI.WebControls.Label lblOrderTotal;
        protected System.Web.UI.WebControls.Label lblRefundType;
        protected System.Web.UI.WebControls.Label lblRefundRemark;
        protected System.Web.UI.WebControls.Label lblContacts;
        protected System.Web.UI.WebControls.Label lblEmail;
        protected System.Web.UI.WebControls.Label lblTelephone;
        protected System.Web.UI.WebControls.Label lblAddress;
        protected System.Web.UI.WebControls.TextBox txtAdminRemark;
        protected System.Web.UI.WebControls.Label return_lblOrderId;
        protected System.Web.UI.WebControls.Label return_lblOrderTotal;
        protected System.Web.UI.WebControls.Label return_lblRefundType;
        protected System.Web.UI.WebControls.Label return_lblReturnRemark;
        protected System.Web.UI.WebControls.Label return_lblContacts;
        protected System.Web.UI.WebControls.Label return_lblEmail;
        protected System.Web.UI.WebControls.Label return_lblTelephone;
        protected System.Web.UI.WebControls.Label return_lblAddress;
        protected System.Web.UI.WebControls.TextBox return_txtRefundMoney;
        protected System.Web.UI.WebControls.TextBox return_txtAdminRemark;
        protected System.Web.UI.WebControls.Label replace_lblOrderId;
        protected System.Web.UI.WebControls.Label replace_lblOrderTotal;
        protected System.Web.UI.WebControls.Label replace_lblComments;
        protected System.Web.UI.WebControls.Label replace_lblContacts;
        protected System.Web.UI.WebControls.Label replace_lblEmail;
        protected System.Web.UI.WebControls.Label replace_lblTelephone;
        protected System.Web.UI.WebControls.Label replace_lblAddress;
        protected System.Web.UI.WebControls.Label replace_lblPostCode;
        protected System.Web.UI.WebControls.TextBox replace_txtAdminRemark;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderTotal;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundType;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundMoney;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidAdminRemark;
        protected System.Web.UI.WebControls.Button btnCloseOrder;
        protected System.Web.UI.WebControls.Button btnAcceptRefund;
        protected System.Web.UI.WebControls.Button btnRefuseRefund;
        protected System.Web.UI.WebControls.Button btnAcceptReturn;
        protected System.Web.UI.WebControls.Button btnRefuseReturn;
        protected System.Web.UI.WebControls.Button btnAcceptReplace;
        protected System.Web.UI.WebControls.Button btnRefuseReplace;
        protected System.Web.UI.WebControls.Button btnRemark;
        protected System.Web.UI.WebControls.Button btnOrderGoods;
        protected System.Web.UI.WebControls.Button btnOrderGoods2;
        protected System.Web.UI.WebControls.Button btnProductGoods;
        protected System.Web.UI.WebControls.Button btnClearOrder1;
        protected System.Web.UI.WebControls.Button btnClearOrder2;
        protected System.Web.UI.WebControls.Button btnClearOrder3;
        //protected System.Web.UI.WebControls.Button btnBatchSend;
        protected FileUpload excelFile;
        protected OrderTimeTypeDropDownList ddListOrderTimeType;
        protected System.Web.UI.WebControls.Label lblPageCount;
        protected System.Web.UI.WebControls.Label lblSearchCount;

        //是否退券
        protected System.Web.UI.WebControls.RadioButtonList radBtnList;

        //运单号
        protected System.Web.UI.WebControls.TextBox txtShipOrderNumber;


        //实名认证
        protected PayerIdStatusDownList ddlpayerIdStatus;


        protected void Page_Load(object sender, System.EventArgs e)
        {
            bool flag = !string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true";
            if (flag)
            {
                if (string.IsNullOrEmpty(base.Request["orderId"]))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(base.Request["orderId"]);

                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                int num;
                string text;
                decimal canRefundMoney = 0M;
                if (base.Request["type"] == "refund")
                {
                    OrderHelper.GetRefundType(base.Request["orderId"], out num, out text);
                    //获取退款信息
                }
                else
                {
                    if (base.Request["type"] == "return")
                    {
                        OrderHelper.GetRefundTypeFromReturn(base.Request["orderId"], out num, out text);
                        int aid = 0;
                        int.TryParse(base.Request["aid"], out aid);
                        if (aid != 0)
                            canRefundMoney = OrderHelper.GetReturnsAmount(aid);
                    }
                    else
                    {
                        num = 0;
                        text = "";
                    }
                }
                string arg;
                if (num == 1)
                {
                    arg = "退到预存款";
                }
                else if (num == 2)
                {
                    arg = "银行转帐";
                }
                else
                {
                    arg = "原路返回";
                }
                stringBuilder.AppendFormat(",\"OrderTotal\":\"{0}\"", Globals.FormatMoney(orderInfo.GetTotal()));
                if (base.Request["type"] == "replace")
                {
                    string replaceComments = OrderHelper.GetReplaceComments(base.Request["orderId"]);
                    stringBuilder.AppendFormat(",\"Comments\":\"{0}\"", replaceComments.ClearForJson());
                }
                else
                {
                    stringBuilder.AppendFormat(",\"RefundType\":\"{0}\"", num);
                    stringBuilder.AppendFormat(",\"RefundTypeStr\":\"{0}\"", arg);
                }

                stringBuilder.AppendFormat(",\"Contacts\":\"{0}\"", orderInfo.RealName.ClearForJson());
                stringBuilder.AppendFormat(",\"Email\":\"{0}\"", orderInfo.EmailAddress.ClearForJson());
                stringBuilder.AppendFormat(",\"Telephone\":\"{0}\"", orderInfo.TelPhone.ClearForJson());
                stringBuilder.AppendFormat(",\"Address\":\"{0}\"", orderInfo.Address.ClearForJson());
                stringBuilder.AppendFormat(",\"Remark\":\"{0}\"", text.ClearForJson());
                stringBuilder.AppendFormat(",\"PostCode\":\"{0}\"", orderInfo.ZipCode.ClearForJson());
                stringBuilder.AppendFormat(",\"CanRefundMoney\":\"{0}\"", canRefundMoney.ToString("0.00"));//允许退款金额
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write("{\"Status\":\"1\"" + stringBuilder.ToString() + "}");
                base.Response.End();
            }
            this.dlstOrders.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlstOrders_ItemDataBound);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.dlstOrders.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstOrders_ItemCommand);
            this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
            this.btnCloseOrder.Click += new System.EventHandler(this.btnCloseOrder_Click);
            this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
            this.btnOrderGoods.Click += new System.EventHandler(this.btnOrderGoods_Click);
            this.btnOrderGoods2.Click += new System.EventHandler(this.btnOrderGoods2_Click);
            this.btnProductGoods.Click += new System.EventHandler(this.btnProductGoods_Click);
            this.btnClearOrder1.Click += new System.EventHandler(this.btnClearOrder1_Click);
            this.btnClearOrder2.Click += new System.EventHandler(this.btnClearOrder2_Click);
            this.btnClearOrder3.Click += new System.EventHandler(this.btnClearOrder3_Click);
            this.btnAcceptRefund.Click += new System.EventHandler(this.btnAcceptRefund_Click);
            this.btnRefuseRefund.Click += new System.EventHandler(this.btnRefuseRefund_Click);
            this.btnAcceptReturn.Click += new System.EventHandler(this.btnAcceptReturn_Click);
            this.btnRefuseReturn.Click += new System.EventHandler(this.btnRefuseReturn_Click);
            this.btnAcceptReplace.Click += new System.EventHandler(this.btnAcceptReplace_Click);
            this.btnRefuseReplace.Click += new System.EventHandler(this.btnRefuseReplace_Click);
            if (!this.Page.IsPostBack)
            {
                this.shippingModeDropDownList.DataBind();
                this.sitesDropDownList.DataBind();
                this.ddListStartHour.DataBind();
                this.ddListEndHour.DataBind();
                this.ddListOrderTimeType.DataBind();
                this.ddlpayerIdStatus.DataBind();

                this.ddlIsPrinted.Items.Clear();
                this.ddlIsPrinted.Items.Add(new System.Web.UI.WebControls.ListItem("全部", string.Empty));
                this.ddlIsPrinted.Items.Add(new System.Web.UI.WebControls.ListItem("已打印", "1"));
                this.ddlIsPrinted.Items.Add(new System.Web.UI.WebControls.ListItem("未打印", "0"));
                this.SetOrderStatusLink();
                this.dropsourceorder.DataBind();

                this.ddlSupplier.DataBind();
                this.BindOrders();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }
        private void btnRefuseReplace_Click(object sender, System.EventArgs e)
        {
            OrderHelper.CheckReplace(this.hidOrderId.Value, this.hidAdminRemark.Value, false);
            this.BindOrders();

            this.ShowMsg("成功的拒绝了订单换货", true);
        }
        private void btnAcceptReplace_Click(object sender, System.EventArgs e)
        {
            OrderHelper.CheckReplace(this.hidOrderId.Value, this.hidAdminRemark.Value, true);
            this.BindOrders();

            this.ShowMsg("成功的确认了订单换货", true);
        }
        private void btnRefuseReturn_Click(object sender, System.EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            OrderHelper.CheckReturn(orderInfo, HiContext.Current.User.Username, 0m, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
            this.BindOrders();
            this.ShowMsg("成功的拒绝了订单退货", true);
        }
        private void btnAcceptReturn_Click(object sender, System.EventArgs e)
        {
            decimal num;
            if (!decimal.TryParse(this.hidRefundMoney.Value, out num))
            {
                this.ShowMsg("退款金额需为数字格式！", false);
                return;
            }
            decimal d;
            decimal.TryParse(this.hidOrderTotal.Value, out d);
            if (num > d)
            {
                this.ShowMsg("退款金额不能大于订单金额！", false);
                return;
            }
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            if (OrderHelper.CheckReturn(orderInfo, HiContext.Current.User.Username, num, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true))
            {
                this.BindOrders();
                Member user = Users.GetUser(orderInfo.UserId) as Member;
                Messenger.OrderRefund(user, orderInfo, num);
                this.ShowMsg("成功的确认了订单退货", true);
            }
        }
        protected void btnAcceptRefund_Click(object sender, System.EventArgs e)
        {
            bool isReturn = false;
            if (this.radBtnList.SelectedValue == "1")
            {
                isReturn = true;
            }

            string username = HiContext.Current.User.Username;
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            if (OrderHelper.CheckRefund(orderInfo, username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), orderInfo.GetTotal(), true, isReturn))
            {
                this.BindOrders();

                decimal amount = orderInfo.GetTotal();
                if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus != GroupBuyStatus.Failed)
                {
                    amount = orderInfo.GetTotal() - orderInfo.NeedPrice;
                }
                Member user = Users.GetUser(orderInfo.UserId) as Member;
                Messenger.OrderRefund(user, orderInfo, amount);

                TradeHelper.RefundSuccess(orderInfo.OrderId.ToString(), 1);

                this.ShowMsg("成功的确认了订单退款", true);
            }
        }
        private void btnRefuseRefund_Click(object sender, System.EventArgs e)
        {
            string username = HiContext.Current.User.Username;
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            OrderHelper.CheckRefund(orderInfo, username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), 0, false, false);
            this.BindOrders();
            this.ShowMsg("成功的拒绝了订单退款", true);
        }
        private void btnProductGoods_Click(object sender, System.EventArgs e)
        {
            string text = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                text = base.Request["CheckBoxGroup"];
            }
            if (text.Length <= 0)
            {
                this.ShowMsg("请选要下载配货表的订单", false);
                return;
            }
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            string[] array = text.Split(new char[]
			{
				','
			});
            for (int i = 0; i < array.Length; i++)
            {
                string str = array[i];
                list.Add("'" + str + "'");
            }
            System.Data.DataSet productGoods = OrderHelper.GetProductGoods(string.Join(",", list.ToArray()));
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            if (productGoods.Tables.Count < 2 || productGoods.Tables[1] == null || productGoods.Tables[1].Rows.Count <= 0)
            {
                stringBuilder.AppendLine("<td>商品名称</td>");
            }
            else
            {
                stringBuilder.AppendLine("<td>商品(礼品)名称</td>");
            }
            stringBuilder.AppendLine("<td>货号</td>");
            stringBuilder.AppendLine("<td>规格</td>");
            stringBuilder.AppendLine("<td>拣货数量</td>");
            stringBuilder.AppendLine("<td>现库存数</td>");
            stringBuilder.AppendLine("</tr>");
            foreach (System.Data.DataRow dataRow in productGoods.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td>" + dataRow["ProductName"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["SKU"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["SKUContent"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["ShipmentQuantity"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["Stock"] + "</td>");
                stringBuilder.AppendLine("</tr>");
            }
            if (productGoods.Tables.Count >= 2 && productGoods.Tables[1] != null && productGoods.Tables[1].Rows.Count > 0)
            {
                foreach (System.Data.DataRow dataRow2 in productGoods.Tables[1].Rows)
                {
                    stringBuilder.AppendLine("<tr>");
                    stringBuilder.AppendLine("<td>" + dataRow2["GiftName"] + "[礼品]</td>");
                    stringBuilder.AppendLine("<td></td>");
                    stringBuilder.AppendLine("<td></td>");
                    stringBuilder.AppendLine("<td>" + dataRow2["Quantity"] + "</td>");
                    stringBuilder.AppendLine("<td></td>");
                    stringBuilder.AppendLine("</tr>");
                }
            }
            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</body></html>");
            base.Response.Clear();
            base.Response.Buffer = false;
            base.Response.Charset = "GB2312";
            //base.Response.AppendHeader("Content-Disposition", "attachment;filename=productgoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=商品配货单.xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }
        private void btnOrderGoods2_Click(object sender, System.EventArgs e)//按支付时间过滤
        {
            int orderStatus = 0;
            int.TryParse(Request.QueryString["orderStatus"], out orderStatus);
            //供货商
            int supplierId = this.ddlSupplier.SelectedValue != null ? this.ddlSupplier.SelectedValue.Value : 0;
            int orderTimeType = this.ddListOrderTimeType.SelectedValue != null ? this.ddListOrderTimeType.SelectedValue.Value : 0;
            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                this.ShowMsg("请选择要下载配货表的订单的时间区域", false);
                return;
            }

            double sHours = double.Parse(ddListStartHour.SelectedValue.ToString());
            string startPayTime = DateTime.Parse(this.calendarStartDate.SelectedDate.Value.ToShortDateString()).AddHours(sHours).ToString();
            double eHours = double.Parse(ddListEndHour.SelectedValue.ToString());
            string endPayTime = DateTime.Parse(this.calendarEndDate.SelectedDate.Value.ToShortDateString()).AddHours(eHours).ToString();
            System.Data.DataSet orderGoods = OrderHelper.GetOrderGoods(startPayTime, endPayTime, "", orderStatus, orderTimeType, supplierId);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>订单单号</td>");
            //if (orderGoods.Tables.Count < 2 || orderGoods.Tables[1] == null || orderGoods.Tables[1].Rows.Count <= 0)
            //{
            //    stringBuilder.AppendLine("<td>商品名称</td>");
            //}
            //else
            //{
            //    stringBuilder.AppendLine("<td>商品(礼品)名称</td>");
            //}
            stringBuilder.AppendLine("<td>买家昵称</td>");
            stringBuilder.AppendLine("<td>拍单时间</td>");
            stringBuilder.AppendLine("<td>付款时间</td>");
            stringBuilder.AppendLine("<td>商品总金额</td>");
            stringBuilder.AppendLine("<td>订单金额</td>");
            stringBuilder.AppendLine("<td>优惠金额</td>");
            stringBuilder.AppendLine("<td>运费</td>");
            stringBuilder.AppendLine("<td>货到付款服务费</td>");
            stringBuilder.AppendLine("<td>订单备注</td>");
            stringBuilder.AppendLine("<td>买家留言</td>");
            stringBuilder.AppendLine("<td>收货地址</td>");
            stringBuilder.AppendLine("<td>收货人名称</td>");
            stringBuilder.AppendLine("<td>收货国家</td>");
            stringBuilder.AppendLine("<td>州/省</td>");
            stringBuilder.AppendLine("<td>城市</td>");
            stringBuilder.AppendLine("<td>区</td>");
            stringBuilder.AppendLine("<td>邮编</td>");
            stringBuilder.AppendLine("<td>联系电话</td>");
            stringBuilder.AppendLine("<td>手机</td>");
            stringBuilder.AppendLine("<td>身份证号码</td>");
            stringBuilder.AppendLine("<td>买家选择物流</td>");
            stringBuilder.AppendLine("<td>最晚发货时间</td>");
            stringBuilder.AppendLine("<td>支付方式</td>");
            stringBuilder.AppendLine("<td>交易流水号</td>");
            stringBuilder.AppendLine("<td>海外订单</td>");
            stringBuilder.AppendLine("<td>是否货到付款</td>");
            stringBuilder.AppendLine("<td>是否已发货</td>");
            stringBuilder.AppendLine("<td>发货快递单号</td>");
            stringBuilder.AppendLine("<td>商品名称</td>");
            stringBuilder.AppendLine("<td>商品条码</td>");
            stringBuilder.AppendLine("<td>商品规格</td>");
            stringBuilder.AppendLine("<td>购买数量</td>");
            stringBuilder.AppendLine("<td>售价</td>");
            stringBuilder.AppendLine("<td>供货商</td>");

            stringBuilder.AppendLine("<td>订单状态</td>");
            stringBuilder.AppendLine("<td>优惠券金额</td>");
            stringBuilder.AppendLine("<td>优惠券面值</td>");
            stringBuilder.AppendLine("<td>用户名</td>");
            stringBuilder.AppendLine("<td>发货时间</td>");
            stringBuilder.AppendLine("<td>完成时间</td>");
            stringBuilder.AppendLine("<td>是否匹配促销模板</td>");


            stringBuilder.AppendLine("</tr>");
            foreach (System.Data.DataRow dataRow in orderGoods.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["订单号"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["买家昵称"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["拍单时间"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["付款时间"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["商品总金额"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["订单金额"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["优惠金额"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["运费"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["货到付款服务费"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["订单备注"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["买家留言"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["收货地址"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["收货人名称"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["收货国家"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["州/省"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["城市"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["区"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["邮编"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["联系电话"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["手机"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["收货人证件号"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["买家选择物流"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["最晚发货时间"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["支付方式"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["交易流水号"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["海外订单"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["是否货到付款"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["是否已发货"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["发货快递单号"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["商品名称"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["商品条码"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["规格"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["购买数量"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["售价"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["供货商"] + "</td>");

                string orderststusstr = OrderInfo.GetOrderStatusName((OrderStatus)dataRow["订单状态"], "");

                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + orderststusstr + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["优惠券金额"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["优惠券面值"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["用户名"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["发货时间"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["完成时间"] + "</td>");

                stringBuilder.AppendLine("<td >" + dataRow["是否匹配促销模板"].ToString() + "</td>");

                stringBuilder.AppendLine("</tr>");
            }
            //if (orderGoods.Tables.Count >= 2 && orderGoods.Tables[1] != null && orderGoods.Tables[1].Rows.Count > 0)
            //{
            //    foreach (System.Data.DataRow dataRow2 in orderGoods.Tables[1].Rows)
            //    {
            //        stringBuilder.AppendLine("<tr>");
            //        stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow2["GiftOrderId"] + "</td>");
            //        stringBuilder.AppendLine("<td>" + dataRow2["GiftName"] + "[礼品]</td>");
            //        stringBuilder.AppendLine("<td></td>");
            //        stringBuilder.AppendLine("<td></td>");
            //        stringBuilder.AppendLine("<td>" + dataRow2["Quantity"] + "</td>");
            //        stringBuilder.AppendLine("<td></td>");
            //        stringBuilder.AppendLine("<td></td>");
            //        stringBuilder.AppendLine("</tr>");
            //    }
            //}
            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</body></html>");
            base.Response.Clear();
            base.Response.Buffer = false;
            base.Response.Charset = "GB2312";
            // base.Response.AppendHeader("Content-Disposition", "attachment;filename=ordergoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=订单配货单_" + this.calendarStartDate.SelectedDate.Value.ToString("yyyyMMdd") + "-" + this.calendarEndDate.SelectedDate.Value.ToString("yyyyMMdd") + ".xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }
        private void btnOrderGoods_Click(object sender, System.EventArgs e)
        {
            int orderStatus = 0;
            int.TryParse(Request.QueryString["orderStatus"], out orderStatus);
            int orderTimeType = 0;
            int.TryParse(Request.QueryString["orderTimeType"], out orderTimeType);
            string text = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                text = base.Request["CheckBoxGroup"];
            }
            if (text.Length <= 0)
            {
                this.ShowMsg("请选要下载配货表的订单", false);
                return;
            }
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            string[] array = text.Split(new char[]
			{
				','
			});
            for (int i = 0; i < array.Length; i++)
            {
                string str = array[i];
                list.Add("'" + str + "'");
            }
            System.Data.DataSet orderGoods = OrderHelper.GetOrderGoods("", "", string.Join(",", list.ToArray()), orderStatus, orderTimeType);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>订单单号</td>");
            //if (orderGoods.Tables.Count < 2 || orderGoods.Tables[1] == null || orderGoods.Tables[1].Rows.Count <= 0)
            //{
            //    stringBuilder.AppendLine("<td>商品名称</td>");
            //}
            //else
            //{
            //    stringBuilder.AppendLine("<td>商品(礼品)名称</td>");
            //}
            stringBuilder.AppendLine("<td>买家昵称</td>");
            stringBuilder.AppendLine("<td>拍单时间</td>");
            stringBuilder.AppendLine("<td>付款时间</td>");
            stringBuilder.AppendLine("<td>商品总金额</td>");
            stringBuilder.AppendLine("<td>订单金额</td>");
            stringBuilder.AppendLine("<td>优惠金额</td>");
            stringBuilder.AppendLine("<td>运费</td>");
            stringBuilder.AppendLine("<td>货到付款服务费</td>");
            stringBuilder.AppendLine("<td>订单备注</td>");
            stringBuilder.AppendLine("<td>买家留言</td>");
            stringBuilder.AppendLine("<td>收货地址</td>");
            stringBuilder.AppendLine("<td>收货人名称</td>");
            stringBuilder.AppendLine("<td>收货国家</td>");
            stringBuilder.AppendLine("<td>州/省</td>");
            stringBuilder.AppendLine("<td>城市</td>");
            stringBuilder.AppendLine("<td>区</td>");
            stringBuilder.AppendLine("<td>邮编</td>");
            stringBuilder.AppendLine("<td>联系电话</td>");
            stringBuilder.AppendLine("<td>手机</td>");
            stringBuilder.AppendLine("<td>身份证号码</td>");
            stringBuilder.AppendLine("<td>买家选择物流</td>");
            stringBuilder.AppendLine("<td>最晚发货时间</td>");
            stringBuilder.AppendLine("<td>支付方式</td>");
            stringBuilder.AppendLine("<td>交易流水号</td>");
            stringBuilder.AppendLine("<td>海外订单</td>");
            stringBuilder.AppendLine("<td>是否货到付款</td>");
            stringBuilder.AppendLine("<td>是否已发货</td>");
            stringBuilder.AppendLine("<td>发货快递单号</td>");
            stringBuilder.AppendLine("<td>商品名称</td>");
            stringBuilder.AppendLine("<td>商品条码</td>");
            stringBuilder.AppendLine("<td>商品规格</td>");
            stringBuilder.AppendLine("<td>购买数量</td>");
            stringBuilder.AppendLine("<td>售价</td>");
            stringBuilder.AppendLine("<td>供货商</td>");

            stringBuilder.AppendLine("<td>订单状态</td>");
            stringBuilder.AppendLine("<td>优惠券金额</td>");
            stringBuilder.AppendLine("<td>优惠券面值</td>");
            stringBuilder.AppendLine("<td>用户名</td>");
            stringBuilder.AppendLine("<td>发货时间</td>");
            stringBuilder.AppendLine("<td>完成时间</td>");
            stringBuilder.AppendLine("<td>是否匹配促销模板</td>");

            stringBuilder.AppendLine("</tr>");
            foreach (System.Data.DataRow dataRow in orderGoods.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["订单号"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["买家昵称"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["拍单时间"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["付款时间"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["商品总金额"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["订单金额"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["优惠金额"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["运费"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["货到付款服务费"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["订单备注"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["买家留言"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["收货地址"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["收货人名称"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["收货国家"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["州/省"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["城市"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["区"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["邮编"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["联系电话"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["手机"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["收货人证件号"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["买家选择物流"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["最晚发货时间"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["支付方式"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["交易流水号"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["海外订单"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["是否货到付款"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["是否已发货"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["发货快递单号"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["商品名称"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["商品条码"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["规格"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["购买数量"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["售价"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["供货商"] + "</td>");

                string orderststusstr = OrderInfo.GetOrderStatusName((OrderStatus)dataRow["订单状态"], "");

                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + orderststusstr + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["优惠券金额"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["优惠券面值"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["用户名"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["发货时间"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["完成时间"] + "</td>");

                stringBuilder.AppendLine("<td >" + dataRow["是否匹配促销模板"].ToString() + "</td>");


                stringBuilder.AppendLine("</tr>");
            }
            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</body></html>");
            base.Response.Clear();
            base.Response.Buffer = false;
            base.Response.Charset = "GB2312";
            //base.Response.AppendHeader("Content-Disposition", "attachment;filename=ordergoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=订单配货单.xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }

        protected void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReloadOrders(true);
        }
        private void dlstOrders_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                OrderStatus orderStatus = (OrderStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderStatus");
                string text = "";
                if (!(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway") is System.DBNull))
                {
                    text = (string)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway");
                }
                int num = (int)((System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyId") is System.DBNull) ? 0 : System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyId"));
                System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("lkbtnEditPrice");
                System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Item.FindControl("lkbtnSendGoods");
                ImageLinkButton imageLinkButton = (ImageLinkButton)e.Item.FindControl("lkbtnPayOrder");
                ImageLinkButton imageLinkButton2 = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmOrder");
                ImageLinkButton lkbtnSetOrderNoPay = (ImageLinkButton)e.Item.FindControl("lkbtnSetOrderNoPay");
                ImageLinkButton lkbtnrefund = (ImageLinkButton)e.Item.FindControl("lkbtnrefund");
                string IsRefund = ((DataRowView)e.Item.DataItem).Row["IsRefund"].ToString();
                if (IsRefund == "2" && lkbtnrefund != null && orderStatus == OrderStatus.ApplyForRefund)
                {
                    lkbtnrefund.Visible = true;
                }

                System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litCloseOrder");
                System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckRefund");
                System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor2 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReturn");
                System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor3 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReplace");
                if (orderStatus == OrderStatus.WaitBuyerPay)
                {
                    hyperLink.Visible = true;
                    literal.Visible = true;
                    if (text != "hishop.plugins.payment.podrequest")
                    {
                        imageLinkButton.Visible = true;
                    }
                }
                if (orderStatus == OrderStatus.ApplyForRefund)
                {
                    htmlAnchor.Visible = true;
                }
                if (orderStatus == OrderStatus.ApplyForReturns)
                {
                    htmlAnchor2.Visible = true;
                }
                if (orderStatus == OrderStatus.ApplyForReplacement)
                {
                    htmlAnchor3.Visible = true;
                }
                if (num > 0)
                {
                    string[] source = new string[]
					{
						"Ecdev.plugins.payment.podrequest"
					};
                    GroupBuyStatus groupBuyStatus = (GroupBuyStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyStatus");
                    label.Visible = ((orderStatus == OrderStatus.BuyerAlreadyPaid || (orderStatus == OrderStatus.WaitBuyerPay && source.Contains(text))) && groupBuyStatus == GroupBuyStatus.Success);
                }
                else
                {
                    label.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid || (orderStatus == OrderStatus.WaitBuyerPay && text == "Ecdev.plugins.payment.podrequest"));
                }
                imageLinkButton2.Visible = (orderStatus == OrderStatus.SellerAlreadySent);

                lkbtnSetOrderNoPay.Visible = (orderStatus == OrderStatus.Closed);

                string orderId = (string)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderId");

                //绑定Repeter数据源
                Repeater repeterOrderItemsList = (Repeater)e.Item.FindControl("repeterOrderItems");
                if (repeterOrderItemsList != null)
                {
                    DataTable orderInfo = OrderHelper.GetOrderItems(orderId);
                    repeterOrderItemsList.DataSource = orderInfo;
                    repeterOrderItemsList.DataBind();

                }

            }
        }
        /// <summary>
        /// 订单退款
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected string OrderRefund(OrderInfo order)
        {
            try
            {
                ErrorLog.Write("进入退款中：" + order.PaymentType.ToString());
                #region  微信退款
                string ret = string.Empty;
                if (order.PaymentType.ToString().IndexOf("微信") >= 0)//微信退款
                {
                    ErrorLog.Write("【微信退款】开始：" + order.PaymentType.ToString());
                    int totle = Convert.ToInt32(order.GetTotal() * 100);
                    int type = 1;//公众平台
                    switch (order.OrderSource)
                    { 
                        case OrderSource.IOS:
                        case OrderSource.Android:
                            type = 2; //开发平台
                            break;
                    }
                    //PC 退款
                    ret = Refund.Run(order.GatewayOrderId, order.OrderId, totle.ToString(), totle.ToString(), type);
                    //ret = Refund.Run("4004142001201603244249344119", "201603246559659", "1", "1", type);
                    ErrorLog.Write("【微信退款】" + order.OrderId.ToString() + "退款结果：" + ret);
                    if (string.IsNullOrEmpty(ret) || ret.ToUpper().IndexOf("SUCCESS")>=0) //退款成功
                    {
                        Action ac = new Action(() =>
                        {
                            //if (string.IsNullOrEmpty(order.SourceOrderId.ToString()))//非拆单
                            //{
                                TradeHelper.RefundOrder(order.OrderId.ToString());//恢复库存及现金卷
                            //}
                            //else
                            //{
                            TradeHelper.RefundOrder_Split(order.OrderId.ToString(), order.SourceOrderId.ToString());
                            //}
                            TradeHelper.RefundSuccess(order.OrderId.ToString(), 1);
                        });
                        ac.BeginInvoke(null, ac);
                        return "退款成功！";
                    }
                    return "微信退款失败:"+ret.ToString();
                }
                #endregion

                #region 支付宝退款
                if (order.PaymentType.ToString().IndexOf("支付宝") >= 0)//支付宝退款
                {
                    //退款批次号
                    string batch_no = order.OrderId.ToString();
                    //必填，每进行一次即时到账批量退款，都需要提供一个批次号，必须保证唯一性
                    //退款请求时间
                    string refund_date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    //必填，格式为：yyyy-MM-dd hh:mm:ss
                    //退款总笔数
                    string batch_num = "1";
                    //必填，即参数detail_data的值中，“#”字符出现的数量加1，最大支持1000笔（即“#”字符出现的最大数量999个）
                    //单笔数据集
                    string detail_data = order.GatewayOrderId + "^" + Math.Round(order.GetTotal(), 2).ToString() + "^协商退款";
                    //必填，格式详见“4.3 单笔数据集参数说明”
                    ////////////////////////////////////////////////////////////////////////////////////////////////
                    Config.Key = "vec1v8gshy4mo8lz735boyupyuhotjct";
                    //把请求参数打包成数组
                    SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                    sParaTemp.Add("partner", "2088121134306407");
                    sParaTemp.Add("_input_charset", "utf-8");
                    sParaTemp.Add("service", "refund_fastpay_by_platform_nopwd");
                    sParaTemp.Add("batch_no", batch_no);
                    sParaTemp.Add("refund_date", refund_date);
                    sParaTemp.Add("batch_num", batch_num);
                    sParaTemp.Add("detail_data", detail_data);
                    ErrorLog.Write("【支付宝退款】退款参数：" + Newtonsoft.Json.JsonConvert.SerializeObject(sParaTemp));
                    //建立请求
                    string sHtmlText = Submit.BuildRequest(sParaTemp);
                    ErrorLog.Write("【支付宝退款】" + order.OrderId.ToString() + ";海美生活订单退款：" + sHtmlText);

                    XmlDocument xmlDoc = new XmlDocument();
                    try
                    {
                        xmlDoc.LoadXml(sHtmlText);
                        string strXmlResponse = xmlDoc.SelectSingleNode("/alipay/is_success").InnerText;
                        if (strXmlResponse.ToLower() == "t")//退款成功
                        {
                            Action ac = new Action(() =>
                            {
                                //if (string.IsNullOrEmpty(order.SourceOrderId.ToString()))//非拆单
                                //{
                                TradeHelper.RefundOrder(order.OrderId.ToString());//恢复库存及现金卷
                                //}
                                //else
                                //{
                                TradeHelper.RefundOrder_Split(order.OrderId.ToString(), order.SourceOrderId.ToString());
                                //}
                                TradeHelper.RefundSuccess(order.OrderId.ToString(), 1);
                            });
                            ac.BeginInvoke(null, ac);
                            return "Ok";
                        }
                    }
                    catch (Exception exp)
                    {
                        ErrorLog.Write("【支付宝退款】" + exp.Message, exp);
                    }
                }
                #endregion
                ErrorLog.Write("【支付宝退款】" + order.OrderId.ToString() + "退款异常，请重试或联系客服退款！");
                return "支付宝退款失败！";
            }
            catch (Exception ee)
            {
                ErrorLog.Write("【退款异常】" + Newtonsoft.Json.JsonConvert.SerializeObject(order), ee);
                return "退款执行异常:"+ee.Message;
            }
        }
        protected void dlstOrders_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(e.CommandArgument.ToString());
            if (orderInfo != null)
            {

                if (e.CommandName == "SetOrderNoPay" && orderInfo.OrderStatus == OrderStatus.Closed)
                {
                    if (OrderHelper.UpdateOrderStatus(orderInfo.OrderId, 1))
                    {
                        this.BindOrders();
                        this.ShowMsg("成功的设置该订单为未支付", true);
                        return;
                    }
                    this.ShowMsg("设置该订单为未支付失败", false);
                }
                if (e.CommandName == "CONFIRM_refund" && orderInfo.IsRefund==2)
                {

                    if (OrderHelper.CheckOrderIsbg(orderInfo.OrderId))
                    {
                        string rest = OrderRefund(orderInfo);
                        this.ShowMsg(rest, true);
                        this.BindOrders();
                        return;
                    }
                    this.ShowMsg("订单已申请报关，不能再退款!", false);
                    return;
                }

                if (e.CommandName == "CONFIRM_PAY" && orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
                {
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    if (orderInfo.CountDownBuyId > 0)
                    {
                        CountDownInfo countDownBuy = TradeHelper.GetCountDownBuy(orderInfo.CountDownBuyId);
                        if (countDownBuy == null || countDownBuy.EndDate < System.DateTime.Now)
                        {
                            this.ShowMsg("当前的订单为限时抢购订单，此活动已结束，所以不能支付", false);
                            return;
                        }
                    }
                    if (orderInfo.GroupBuyId > 0)
                    {
                        GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(orderInfo.GroupBuyId);
                        if (groupBuy == null || groupBuy.Status != GroupBuyStatus.UnderWay)
                        {
                            this.ShowMsg("当前的订单为团购订单，此团购活动已结束，所以不能支付", false);
                            return;
                        }
                        num2 = PromoteHelper.GetOrderCount(orderInfo.GroupBuyId);
                        num = groupBuy.MaxCount;
                        num3 = orderInfo.GetGroupBuyOerderNumber();
                        if (num < num2 + num3)
                        {
                            this.ShowMsg("当前的订单为团购订单，订购数量已超过订购总数，所以不能支付", false);
                            return;
                        }
                    }
                    //如果需要拆单
                    if (TradeHelper.CheckIsUnpack(orderInfo.OrderId))
                    {
                        if (OrderHelper.ManagerConfirmPay(orderInfo, false, true))
                        {
                            OrderHelper.SetOrderPayStatus(orderInfo.OrderId, 2);
                            if (orderInfo.UserId != 0 && orderInfo.UserId != 1100)
                            {
                                IUser user = Users.GetUser(orderInfo.UserId);
                                if (user != null && user.UserRole == UserRole.Member)
                                {
                                    Messenger.OrderPayment(user, orderInfo, orderInfo.GetTotal());
                                }
                            }
                            DebitNoteInfo debitNoteInfo = new DebitNoteInfo();
                            debitNoteInfo.NoteId = Globals.GetGenerateId();
                            debitNoteInfo.OrderId = e.CommandArgument.ToString();
                            debitNoteInfo.Operator = HiContext.Current.User.Username;
                            debitNoteInfo.Remark = "后台" + debitNoteInfo.Operator + "收款成功";
                            OrderHelper.SaveDebitNote(debitNoteInfo);
                            if (orderInfo.GroupBuyId > 0 && num == num2 + num3)
                            {
                                PromoteHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
                            }
                            this.BindOrders();
                            this.ShowMsg("成功的确认了订单收款", true);
                            return;
                        }
                    }
                    //如果需要合单
                    //if (orderInfo.OrderType == (int)OrderType.WillMerge)
                    //{
                    //    ShoppingProcessor.mergeOrder(orderInfo);
                    //    DebitNoteInfo debitNoteInfo = new DebitNoteInfo();
                    //    debitNoteInfo.NoteId = Globals.GetGenerateId();
                    //    debitNoteInfo.OrderId = e.CommandArgument.ToString();
                    //    debitNoteInfo.Operator = HiContext.Current.User.Username;
                    //    debitNoteInfo.Remark = "后台" + debitNoteInfo.Operator + "收款成功";
                    //    OrderHelper.SaveDebitNote(debitNoteInfo);
                    //    if (orderInfo.GroupBuyId > 0 && num == num2 + num3)
                    //    {
                    //        PromoteHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
                    //    }
                    //    this.BindOrders();
                    //    int num4 = orderInfo.UserId;
                    //    if (num4 == 1100)
                    //    {
                    //        num4 = 0;
                    //    }
                    //    IUser user = Users.GetUser(num4);
                    //    Messenger.OrderPayment(user, orderInfo, orderInfo.GetTotal());
                    //    orderInfo.OnPayment();
                    //    this.ShowMsg("成功的确认了订单收款", true);
                    //    return;
                    //}
                    else
                    {
                        if (OrderHelper.ConfirmPay(orderInfo))
                        {
                            OrderHelper.SetOrderPayStatus(orderInfo.OrderId, 2);
                            DebitNoteInfo debitNoteInfo = new DebitNoteInfo();
                            debitNoteInfo.NoteId = Globals.GetGenerateId();
                            debitNoteInfo.OrderId = e.CommandArgument.ToString();
                            debitNoteInfo.Operator = HiContext.Current.User.Username;
                            debitNoteInfo.Remark = "后台" + debitNoteInfo.Operator + "收款成功";
                            OrderHelper.SaveDebitNote(debitNoteInfo);
                            if (orderInfo.GroupBuyId > 0 && num == num2 + num3)
                            {
                                PromoteHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
                            }
                            this.BindOrders();
                            int num4 = orderInfo.UserId;
                            if (num4 == 1100)
                            {
                                num4 = 0;
                            }
                            IUser user = Users.GetUser(num4);
                            Messenger.OrderPayment(user, orderInfo, orderInfo.GetTotal());
                            orderInfo.OnPayment();
                            this.ShowMsg("成功的确认了订单收款", true);
                            return;
                        }
                    }
                    this.ShowMsg("确认订单收款失败", false);
                    return;
                }
                else
                {
                    if (e.CommandName == "FINISH_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
                    {
                        if (OrderHelper.ConfirmOrderFinish(orderInfo))
                        {
                            OrderHelper.SetOrderPayStatus(orderInfo.OrderId, 2);
                            this.BindOrders();
                            this.ShowMsg("成功的完成了该订单", true);
                            return;
                        }
                        this.ShowMsg("完成订单失败", false);
                    }
                }
            }
        }
        protected void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            string text = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                text = base.Request["CheckBoxGroup"];
            }
            if (text.Length <= 0)
            {
                this.ShowMsg("请选要删除的订单", false);
                return;
            }
            text = "'" + text.Replace(",", "','") + "'";
            int num = OrderHelper.DeleteOrders(text);
            this.BindOrders();
            this.ShowMsg(string.Format("成功删除了{0}个订单", num), true);
        }
        private void btnSendGoods_Click(object sender, System.EventArgs e)
        {
            string text = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                text = base.Request["CheckBoxGroup"];
            }
            if (text.Length <= 0)
            {
                this.ShowMsg("请选要发货的订单", false);
                return;
            }
            this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/Sales/BatchSendOrderGoods.aspx?OrderIds=" + text));
        }

        private void BindOrders()
        {

            OrderQuery orderQuery = this.GetOrderQuery();

            // 2015-08-31修改
            /*DbQueryResult orders = OrderHelper.GetOrders(orderQuery);
            DataTable dt = orders.Data != null ? (DataTable)orders.Data : null;
            if (dt != null)
            {
                this.dlstOrders.DataSource = dt;
                this.dlstOrders.DataBind();
            }
            this.pager.TotalRecords = orders.TotalRecords;
            this.pager1.TotalRecords = orders.TotalRecords;*/

            OrdersInfo userOrders = OrderHelper.GetUserOrders(orderQuery);
            this.dlstOrders.DataSource = userOrders.OrderTbl;
            this.dlstOrders.DataBind();
            this.pager.TotalRecords = userOrders.TotalCount;
            this.pager1.TotalRecords = userOrders.TotalCount;

            this.lblPageCount.Text = string.Format("当前页共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">订单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">订单毛利润共计</span><span class=\"colorG\">{2}</span>元 ", userOrders.OrderTbl.Rows.Count, Globals.FormatMoney(userOrders.TotalOfPage), Globals.FormatMoney(userOrders.ProfitsOfPage));
            this.lblSearchCount.Text = string.Format("当前查询结果共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">订单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">订单毛利润共计</span><span class=\"colorG\">{2}</span>元 ", userOrders.TotalCount, Globals.FormatMoney(userOrders.TotalOfSearch), Globals.FormatMoney(userOrders.ProfitsOfSearch));


            this.txtUserName.Text = orderQuery.UserName;
            this.txtOrderId.Text = orderQuery.OrderId;

            this.txtProductName.Text = orderQuery.ProductName;
            this.txtShopTo.Text = orderQuery.ShipTo;
            this.calendarStartDate.SelectedDate = orderQuery.StartDate;

            if (orderQuery.StartDate.HasValue)
            {
                this.ddListStartHour.SelectedValue = DateTime.Parse(orderQuery.StartDate.ToString()).Hour;
            }
            if (orderQuery.SupplierId.HasValue && orderQuery.SupplierId != 0)
            {
                this.ddlSupplier.SelectedValue = orderQuery.SupplierId;
            }
            if (orderQuery.OrderTimeType.HasValue && orderQuery.OrderTimeType != 0)
            {
                this.ddListOrderTimeType.SelectedValue = orderQuery.OrderTimeType;
            }
            this.calendarEndDate.SelectedDate = orderQuery.EndDate;
            if (orderQuery.EndDate.HasValue)
            {
                this.ddListEndHour.SelectedValue = DateTime.Parse(orderQuery.EndDate.ToString()).Hour;
            }

            this.lblStatus.Text = ((int)orderQuery.Status).ToString();
            this.lblOrderTimeType.Text = orderQuery.OrderTimeType.ToString();
            this.shippingModeDropDownList.SelectedValue = orderQuery.ShippingModeId;
            this.txtCellPhone.Text = orderQuery.CellPhone;
            if (orderQuery.IsPrinted.HasValue)
            {
                this.ddlIsPrinted.SelectedValue = orderQuery.IsPrinted.Value.ToString();
            }
            if (orderQuery.RegionId.HasValue)
            {
                this.dropRegion.SetSelectedRegionId(orderQuery.RegionId);
            }
            if (orderQuery.SourceOrder.HasValue)
            {
                this.dropsourceorder.SelectedValue = new int?(orderQuery.SourceOrder.Value);
            }
            if(orderQuery.IsRefund>0)
            {
                this.DDL_IsRefund.SelectedValue = orderQuery.IsRefund.ToString();
            }
            if (orderQuery.SiteId.HasValue)
            {
                this.sitesDropDownList.SelectedValue = new int?(orderQuery.SiteId.Value);
            }

            this.txtShipOrderNumber.Text = orderQuery.ShipOrderNumber;

            if (orderQuery.PayerIdStatus.HasValue)
            {
                this.ddlpayerIdStatus.SelectedValue = new int?(orderQuery.PayerIdStatus.Value);
            }

        }
        private OrderQuery GetOrderQuery()
        {
            OrderQuery orderQuery = new OrderQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                orderQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsRefund"]))
            {
                orderQuery.IsRefund = int.Parse(this.Page.Request.QueryString["IsRefund"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipOrderNumber"]))
            {
                orderQuery.ShipOrderNumber = Globals.UrlDecode(this.Page.Request.QueryString["ShipOrderNumber"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
            {
                orderQuery.ProductName = Globals.UrlDecode(this.Page.Request.QueryString["ProductName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipTo"]))
            {
                orderQuery.ShipTo = Globals.UrlDecode(this.Page.Request.QueryString["ShipTo"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SiteId"]))//站点Id
            {
                orderQuery.SiteId = new int?(int.Parse(this.Page.Request.QueryString["SiteId"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CellPhone"]))
            {
                orderQuery.CellPhone = Globals.UrlDecode(this.Page.Request.QueryString["CellPhone"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
            {
                orderQuery.UserName = Globals.UrlDecode(this.Page.Request.QueryString["UserName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
            {
                orderQuery.StartDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["StartDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                orderQuery.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
            }




            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
            {
                orderQuery.EndDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["EndDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["supplierId"]))
            {
                int supplierId = 0;
                int.TryParse(this.Page.Request.QueryString["supplierId"], out supplierId);
                orderQuery.SupplierId = supplierId;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderStatus"]))
            {
                int status = 0;
                if (int.TryParse(this.Page.Request.QueryString["OrderStatus"], out status))
                {
                    orderQuery.Status = (OrderStatus)status;
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsPrinted"]))
            {
                int value = 0;
                if (int.TryParse(this.Page.Request.QueryString["IsPrinted"], out value))
                {
                    orderQuery.IsPrinted = new int?(value);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ModeId"]))
            {
                int value2 = 0;
                if (int.TryParse(this.Page.Request.QueryString["ModeId"], out value2))
                {
                    orderQuery.ShippingModeId = new int?(value2);
                }
            }
            int value3;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["region"]) && int.TryParse(this.Page.Request.QueryString["region"], out value3))
            {
                orderQuery.RegionId = new int?(value3);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sourceorder"]))
            {
                int value4 = 0;
                if (int.TryParse(this.Page.Request.QueryString["sourceorder"], out value4))
                {
                    orderQuery.SourceOrder = new int?(value4);
                }
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["payerIdStatus"]))
            {
                int payerIdStatus = 0;
                if (int.TryParse(this.Page.Request.QueryString["payerIdStatus"], out payerIdStatus))
                {
                    orderQuery.PayerIdStatus = new int?(payerIdStatus);
                }
            }




            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderTimeType"]))
            {
                int orderTimeType = 0;
                if (int.TryParse(this.Page.Request.QueryString["OrderTimeType"], out orderTimeType))
                {
                    orderQuery.OrderTimeType = new int?(orderTimeType);
                }
            }
            else
            {
                orderQuery.OrderTimeType = ddListOrderTimeType.SelectedValue.Value;
            }
            //门店Id
            int? userStoreId = ManagerHelper.GetStoreIdByUserId(HiContext.Current.User.UserId);
            orderQuery.StoreId = userStoreId;
            orderQuery.PageIndex = this.pager.PageIndex;
            orderQuery.PageSize = this.pager.PageSize;
            orderQuery.SortBy = "OrderDate";
            orderQuery.SortOrder = SortAction.Desc;
            return orderQuery;
        }
        private void ReloadOrders(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("UserName", this.txtUserName.Text);
            nameValueCollection.Add("OrderId", this.txtOrderId.Text);
            nameValueCollection.Add("ProductName", this.txtProductName.Text);
            nameValueCollection.Add("ShipTo", this.txtShopTo.Text);
            nameValueCollection.Add("CellPhone", this.txtCellPhone.Text);
            nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
            nameValueCollection.Add("OrderStatus", this.lblStatus.Text);
            nameValueCollection.Add("ShipOrderNumber", this.txtShipOrderNumber.Text);

            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                double sHours = double.Parse(ddListStartHour.SelectedValue.ToString());
                nameValueCollection.Add("StartDate", DateTime.Parse(this.calendarStartDate.SelectedDate.Value.ToString()).AddHours(sHours).ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                double eHours = double.Parse(ddListEndHour.SelectedValue.ToString());
                nameValueCollection.Add("EndDate", DateTime.Parse(this.calendarEndDate.SelectedDate.Value.AddHours(-23).AddMinutes(-59).AddSeconds(-59).ToString()).AddHours(eHours).ToString());
            }
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                nameValueCollection.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
            }
            if (this.shippingModeDropDownList.SelectedValue.HasValue)
            {
                nameValueCollection.Add("ModeId", this.shippingModeDropDownList.SelectedValue.Value.ToString());
            }
            if (!string.IsNullOrEmpty(this.ddlIsPrinted.SelectedValue))
            {
                nameValueCollection.Add("IsPrinted", this.ddlIsPrinted.SelectedValue);
            }
            if (this.dropRegion.GetSelectedRegionId().HasValue)
            {
                nameValueCollection.Add("region", this.dropRegion.GetSelectedRegionId().Value.ToString());
            }
            if (this.dropsourceorder.SelectedValue.HasValue)
            {
                nameValueCollection.Add("sourceorder", this.dropsourceorder.SelectedValue.Value.ToString());
            }

            if(!string.IsNullOrEmpty(this.DDL_IsRefund.SelectedValue))
            {
                nameValueCollection.Add("IsRefund", this.DDL_IsRefund.SelectedValue);
            }
            if (this.ddlpayerIdStatus.SelectedValue.HasValue)
            {
                nameValueCollection.Add("payerIdStatus", this.ddlpayerIdStatus.SelectedValue.Value.ToString());
            }


            if (this.ddlSupplier.SelectedValue.HasValue)
            {
                nameValueCollection.Add("supplierId", this.ddlSupplier.SelectedValue.Value.ToString());
            }
            if (this.sitesDropDownList.SelectedValue.HasValue)
            {
                nameValueCollection.Add("SiteId", this.sitesDropDownList.SelectedValue.Value.ToString());
            }
            if (this.ddListOrderTimeType.SelectedValue.HasValue)
            {
                nameValueCollection.Add("OrderTimeType", this.ddListOrderTimeType.SelectedValue.Value.ToString());
            }
            else
            {
                nameValueCollection.Add("OrderTimeType", this.lblOrderTimeType.Text);
            }
            base.ReloadPage(nameValueCollection);
        }
        private void SetOrderStatusLink()
        {
            string format = Globals.ApplicationPath + "/Admin/sales/ManageOrder.aspx?orderStatus={0}";
            this.hlinkAllOrder.NavigateUrl = string.Format(format, 0);
            this.hlinkNotPay.NavigateUrl = string.Format(format, 1);
            this.hlinkYetPay.NavigateUrl = string.Format(format, 2);
            this.hlinkSendGoods.NavigateUrl = string.Format(format, 3);
            this.hlinkClose.NavigateUrl = string.Format(format, 4);
            this.hlinkTradeFinished.NavigateUrl = string.Format(format, 5);
            this.hlinkHistory.NavigateUrl = string.Format(format, 99);
        }
        private void btnRemark_Click(object sender, System.EventArgs e)
        {
            if (this.txtRemark.Text.Length > 300)
            {
                this.ShowMsg("备忘录长度限制在300个字符以内", false);
                return;
            }
            //System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
            //if (!regex.IsMatch(this.txtRemark.Text))
            //{
            //    this.ShowMsg("备忘录只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
            //    return;
            //}
            if (string.IsNullOrWhiteSpace(this.txtRemark.Text))
            {
                this.ShowMsg("请输入备注信息！", false);
                return;
            }
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            if (this.orderRemarkImageForRemark.SelectedItem != null)
            {
                orderInfo.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
            }
            orderInfo.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
            orderInfo.RemarkPeople = HiContext.Current.User.Username;
            orderInfo.RemarkTime = DateTime.Now;
            if (OrderHelper.SaveRemark(orderInfo))
            {
                this.BindOrders();
                this.ShowMsg("保存备忘录成功", true);
                return;
            }
            this.ShowMsg("保存失败", false);
        }
        private void btnCloseOrder_Click(object sender, System.EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            orderInfo.CloseReason = this.ddlCloseReason.SelectedValue;
            if (OrderHelper.CloseTransaction(orderInfo))
            {
                int num = orderInfo.UserId;
                if (num == 1100)
                {
                    num = 0;
                }
                IUser user = Users.GetUser(num);
                Messenger.OrderClosed(user, orderInfo, orderInfo.CloseReason);
                orderInfo.OnClosed();
                this.BindOrders();

                this.ShowMsg("关闭订单成功", true);
                return;
            }
            this.ShowMsg("关闭订单失败", false);
        }

        #region 清关订单
        private void btnClearOrder2_Click(object sender, System.EventArgs e)
        {
            int orderStatus = 0;
            int.TryParse(Request.QueryString["orderStatus"], out orderStatus);
            int orderTimeType = this.ddListOrderTimeType.SelectedValue != null ? this.ddListOrderTimeType.SelectedValue.Value : 0;
            //供货商
            int supplierId = this.ddlSupplier.SelectedValue != null ? this.ddlSupplier.SelectedValue.Value : 0;
            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                this.ShowMsg("请选择要下载清关的订单的时间区域", false);
                return;
            }

            double sHours = double.Parse(ddListStartHour.SelectedValue.ToString());
            string startPayTime = DateTime.Parse(this.calendarStartDate.SelectedDate.Value.ToShortDateString()).AddHours(sHours).ToString();
            double eHours = double.Parse(ddListEndHour.SelectedValue.ToString());
            string endPayTime = DateTime.Parse(this.calendarEndDate.SelectedDate.Value.ToShortDateString()).AddHours(eHours).ToString();
            System.Data.DataSet orderGoods = OrderHelper.GetClearOrderGoods(startPayTime, endPayTime, "", orderStatus, orderTimeType, supplierId);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>清关订单</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>销售订单号</td>");
            stringBuilder.AppendLine("<td>电商平台名称</td>");
            stringBuilder.AppendLine("<td>电商平台备案号</td>");
            stringBuilder.AppendLine("<td>电商客户名称</td>");
            stringBuilder.AppendLine("<td>电商商户备案号</td>");
            stringBuilder.AppendLine("<td>电商客户电话</td>");
            stringBuilder.AppendLine("<td>订单人姓名</td>");
            stringBuilder.AppendLine("<td>订单人证件类型</td>");
            stringBuilder.AppendLine("<td>订单人证件号</td>");
            stringBuilder.AppendLine("<td>订单人电话</td>");
            stringBuilder.AppendLine("<td>订单日期</td>");
            stringBuilder.AppendLine("<td>收件人姓名</td>");
            stringBuilder.AppendLine("<td>收件人证件号码</td>");
            stringBuilder.AppendLine("<td>收件人地址</td>");
            stringBuilder.AppendLine("<td>收件人电话</td>");
            stringBuilder.AppendLine("<td>商品名称</td>");
            stringBuilder.AppendLine("<td>商品货号</td>");
            stringBuilder.AppendLine("<td>供货商</td>");
            stringBuilder.AppendLine("<td>申报数量</td>");
            stringBuilder.AppendLine("<td>申报单价</td>");
            stringBuilder.AppendLine("<td>申报总价</td>");
            stringBuilder.AppendLine("<td>运费</td>");
            stringBuilder.AppendLine("<td>保价费</td>");
            stringBuilder.AppendLine("<td>税款</td>");
            stringBuilder.AppendLine("<td>毛重</td>");
            stringBuilder.AppendLine("<td>净重</td>");
            stringBuilder.AppendLine("<td>选用的快递公司</td>");
            stringBuilder.AppendLine("<td>网址</td>");

            stringBuilder.AppendLine("</tr>");
            foreach (System.Data.DataRow dataRow in orderGoods.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["销售订单号"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["电商平台名称"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["电商平台备案号"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["电商客户名称"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["电商商户备案号"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["电商客户电话"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["订单人姓名"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["订单人证件类型"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["订单人证件号"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["订单人电话"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["订单日期"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["收件人姓名"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["收件人证件号码"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["收件人地址"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["收件人电话"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["商品名称"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:@\">" + dataRow["商品货号"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["供货商"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["申报数量"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["申报单价"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["申报总价"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["运费"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["保价费"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["税款"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["毛重"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["净重"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["选用的快递公司"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["网址"] + "</td>");
                stringBuilder.AppendLine("</tr>");
            }

            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</body></html>");
            base.Response.Clear();
            base.Response.Buffer = false;
            base.Response.Charset = "GB2312";
            //base.Response.AppendHeader("Content-Disposition", "attachment;filename=ordergoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=清关订单_" + this.calendarStartDate.SelectedDate.Value.ToString("yyyyMMdd") + "-" + this.calendarEndDate.SelectedDate.Value.ToString("yyyyMMdd") + ".xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }
        private void btnClearOrder1_Click(object sender, System.EventArgs e)
        {
            int orderStatus = 0; int orderTimeType = 0;
            int.TryParse(Request.QueryString["orderStatus"], out orderStatus);
            int.TryParse(Request.QueryString["orderTimeType"], out orderTimeType);
            string text = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                text = base.Request["CheckBoxGroup"];
            }
            if (text.Length <= 0)
            {
                this.ShowMsg("请选要下载清关的订单", false);
                return;
            }
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            string[] array = text.Split(new char[]
			{
				','
			});
            for (int i = 0; i < array.Length; i++)
            {
                string str = array[i];
                list.Add("'" + str + "'");
            }
            System.Data.DataSet orderGoods = OrderHelper.GetClearOrderGoods("", "", string.Join(",", list.ToArray()), orderStatus, orderTimeType);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>清关订单</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>销售订单号</td>");
            stringBuilder.AppendLine("<td>电商平台名称</td>");
            stringBuilder.AppendLine("<td>电商平台备案号</td>");
            stringBuilder.AppendLine("<td>电商客户名称</td>");
            stringBuilder.AppendLine("<td>电商商户备案号</td>");
            stringBuilder.AppendLine("<td>电商客户电话</td>");
            stringBuilder.AppendLine("<td>订单人姓名</td>");
            stringBuilder.AppendLine("<td>订单人证件类型</td>");
            stringBuilder.AppendLine("<td>订单人证件号</td>");
            stringBuilder.AppendLine("<td>订单人电话</td>");
            stringBuilder.AppendLine("<td>订单日期</td>");
            stringBuilder.AppendLine("<td>收件人姓名</td>");
            stringBuilder.AppendLine("<td>收件人证件号码</td>");
            stringBuilder.AppendLine("<td>收件人地址</td>");
            stringBuilder.AppendLine("<td>收件人电话</td>");
            stringBuilder.AppendLine("<td>商品名称</td>");
            stringBuilder.AppendLine("<td>商品货号</td>");
            stringBuilder.AppendLine("<td>供货商</td>");
            stringBuilder.AppendLine("<td>申报数量</td>");
            stringBuilder.AppendLine("<td>申报单价</td>");
            stringBuilder.AppendLine("<td>申报总价</td>");
            stringBuilder.AppendLine("<td>运费</td>");
            stringBuilder.AppendLine("<td>保价费</td>");
            stringBuilder.AppendLine("<td>税款</td>");
            stringBuilder.AppendLine("<td>毛重</td>");
            stringBuilder.AppendLine("<td>净重</td>");
            stringBuilder.AppendLine("<td>选用的快递公司</td>");
            stringBuilder.AppendLine("<td>网址</td>");


            stringBuilder.AppendLine("</tr>");
            foreach (System.Data.DataRow dataRow in orderGoods.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["销售订单号"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["电商平台名称"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["电商平台备案号"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["电商客户名称"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["电商商户备案号"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["电商客户电话"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["订单人姓名"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["订单人证件类型"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["订单人证件号"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["订单人电话"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["订单日期"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["收件人姓名"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["订单人证件号"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["收件人地址"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["收件人电话"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["商品名称"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:@\">" + dataRow["商品货号"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["供货商"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["申报数量"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["申报单价"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["申报总价"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["运费"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["保价费"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["税款"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["毛重"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["净重"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["选用的快递公司"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["网址"] + "</td>");
                stringBuilder.AppendLine("</tr>");
            }
            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</body></html>");
            base.Response.Clear();
            base.Response.Buffer = false;
            base.Response.Charset = "GB2312";
            //base.Response.AppendHeader("Content-Disposition", "attachment;filename=ordergoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=清关订单.xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }


        private void btnClearOrder3_Click(object sender, System.EventArgs e)
        {
            int orderStatus = 0;
            int.TryParse(Request.QueryString["orderStatus"], out orderStatus);
            string text = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                text = base.Request["CheckBoxGroup"];
            }
            if (text.Length <= 0)
            {
                this.ShowMsg("请选要下载配货表的订单", false);
                return;
            }
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            string[] array = text.Split(new char[]
			{
				','
			});
            for (int i = 0; i < array.Length; i++)
            {
                string str = array[i];
                list.Add("'" + str + "'");
            }
            System.Data.DataSet productGoods = OrderHelper.GetClearProductGoods(string.Join(",", list.ToArray()), orderStatus);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            if (productGoods.Tables.Count < 2 || productGoods.Tables[1] == null || productGoods.Tables[1].Rows.Count <= 0)
            {
                stringBuilder.AppendLine("<td>商品名称</td>");
            }
            else
            {
                stringBuilder.AppendLine("<td>商品(礼品)名称</td>");
            }
            stringBuilder.AppendLine("<td>货号</td>");
            stringBuilder.AppendLine("<td>规格</td>");
            stringBuilder.AppendLine("<td>拣货数量</td>");
            stringBuilder.AppendLine("<td>现库存数</td>");
            stringBuilder.AppendLine("</tr>");
            foreach (System.Data.DataRow dataRow in productGoods.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td>" + dataRow["ProductName"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["SKU"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["SKUContent"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["ShipmentQuantity"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["Stock"] + "</td>");
                stringBuilder.AppendLine("</tr>");
            }
            if (productGoods.Tables.Count >= 2 && productGoods.Tables[1] != null && productGoods.Tables[1].Rows.Count > 0)
            {
                foreach (System.Data.DataRow dataRow2 in productGoods.Tables[1].Rows)
                {
                    stringBuilder.AppendLine("<tr>");
                    stringBuilder.AppendLine("<td>" + dataRow2["GiftName"] + "[礼品]</td>");
                    stringBuilder.AppendLine("<td></td>");
                    stringBuilder.AppendLine("<td></td>");
                    stringBuilder.AppendLine("<td>" + dataRow2["Quantity"] + "</td>");
                    stringBuilder.AppendLine("<td></td>");
                    stringBuilder.AppendLine("</tr>");
                }
            }
            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</body></html>");
            base.Response.Clear();
            base.Response.Buffer = false;
            base.Response.Charset = "GB2312";
            //base.Response.AppendHeader("Content-Disposition", "attachment;filename=productgoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=清关商品配货单.xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }
        #endregion
    }
}

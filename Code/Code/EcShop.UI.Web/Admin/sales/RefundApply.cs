using ASPNET.WebControls;
using Ecdev.Weixin.Pay;
using Ecdev.Weixin.Pay.Domain;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Core.ErrorLog;
using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Messages;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.sales
{
    [PrivilegeCheck(Privilege.OrderRefundApplyView)]
    public class RefundApply : AdminPage
    {
        protected System.Web.UI.WebControls.TextBox txtOrderId;
        protected System.Web.UI.WebControls.Label lblStatus;
        protected System.Web.UI.WebControls.DropDownList ddlHandleStatus;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected PageSize hrefPageSize;
        protected Pager pager1;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;
        protected System.Web.UI.WebControls.DataList dlstRefund;
        protected Pager pager;
        protected System.Web.UI.WebControls.Label lblOrderId;
        protected System.Web.UI.WebControls.Label lblOrderTotal;
        protected System.Web.UI.WebControls.Label lblRefundType;
        protected System.Web.UI.WebControls.Label lblRefundRemark;
        protected System.Web.UI.WebControls.Label lblContacts;
        protected System.Web.UI.WebControls.Label lblEmail;
        protected System.Web.UI.WebControls.Label lblTelephone;
        protected System.Web.UI.WebControls.Label lblAddress;
        protected System.Web.UI.WebControls.TextBox txtAdminRemark;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderTotal;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundType;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundMoney;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidAdminRemark;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundTotal;
        protected System.Web.UI.WebControls.Button btnAcceptRefund;
        protected System.Web.UI.WebControls.TextBox txtRefundTotal;
        protected System.Web.UI.WebControls.Button btnRefuseRefund;
        protected System.Web.UI.WebControls.Button btnExcelOrderRefund;
        protected System.Web.UI.WebControls.Button btnExcelOrderRefundTime;

        protected System.Web.UI.WebControls.Button btnExcelOrderRefundDetails;
        protected System.Web.UI.WebControls.Button btnExcelOrderRefundDetailsTime;
        protected SupplierDropDownList ddlSupplier;//供货商


        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;

        protected System.Web.UI.WebControls.TextBox txtHandler;//处理人

        protected System.Web.UI.HtmlControls.HtmlInputHidden hidIsAutoRefund;

        //是否退券
        protected System.Web.UI.WebControls.RadioButtonList radBtnList;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.dlstRefund.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlstRefund_ItemDataBound);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
            this.btnAcceptRefund.Click += new System.EventHandler(this.btnAcceptRefund_Click);
            this.btnRefuseRefund.Click += new System.EventHandler(this.btnRefuseRefund_Click);
            this.btnExcelOrderRefund.Click += new EventHandler(this.btnExcelOrderRefund_Click);
            this.btnExcelOrderRefundTime.Click += new EventHandler(this.btnExcelOrderRefundTime_Click);
            this.btnExcelOrderRefundDetails.Click += new EventHandler(this.btnExcelOrderRefundDetails_Click);
            this.btnExcelOrderRefundDetailsTime.Click += new EventHandler(this.btnExcelOrderRefundDetailsTime_Click);

            if (!base.IsPostBack)
            {
                this.ddlSupplier.DataBind();
                this.BindRefund();
            }
        }

        private void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderRefundApplyDelete);
            string text = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                text = base.Request["CheckBoxGroup"];
            }
            if (text.Length <= 0)
            {
                this.ShowMsg("请选要删除的退款申请单", false);
                return;
            }
            string text2 = "成功删除了{0}个退款申请单";
            int num;
            if (OrderHelper.DelRefundApply(text.Split(new char[]
			{
				','
			}), out num))
            {
                text2 = string.Format(text2, num);
            }
            else
            {
                text2 = string.Format(text2, num) + ",待处理的申请不能删除";
            }
            this.BindRefund();
            this.ShowMsg(text2, true);
        }
        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReloadRefunds(true);
        }
        private void dlstRefund_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckRefund");
                System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblHandleStatus");
                System.Web.UI.HtmlControls.HtmlAnchor lkbtnReceive = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnReceive");
                if (label.Text == "0")
                {

                    label.Text = "待处理";
                    lkbtnReceive.Visible = true;
                    return;
                }
                if (label.Text == "1")
                {
                    label.Text = "已完成";
                    return;
                }
                else if (label.Text == "3")
                {
                    htmlAnchor.Visible = true;
                    label.Text = "已受理";
                    return;
                }
                if (label.Text == "2")
                {
                    label.Text = "已拒绝";
                    return;
                }
            }
        }
        private void BindRefund()
        {
            RefundApplyQuery refundQuery = this.GetRefundQuery();
            DbQueryResult refundApplys = OrderHelper.GetRefundApplysMoney(refundQuery);
            this.dlstRefund.DataSource = refundApplys.Data;
            this.dlstRefund.DataBind();
            this.pager.TotalRecords = refundApplys.TotalRecords;
            this.pager1.TotalRecords = refundApplys.TotalRecords;
            this.txtOrderId.Text = refundQuery.OrderId;
            this.txtHandler.Text = refundQuery.Operator;
            this.ddlHandleStatus.SelectedIndex = 0;

            this.calendarEndDate.Text = refundQuery.EndTime;
            this.calendarStartDate.Text = refundQuery.StratTime;

            if (refundQuery.HandleStatus.HasValue && refundQuery.HandleStatus.Value > -1)
            {
                this.ddlHandleStatus.SelectedValue = refundQuery.HandleStatus.Value.ToString();
            }
            if (refundQuery.SupplierId.HasValue && refundQuery.SupplierId.Value > 0)
            {
                this.ddlSupplier.SelectedValue = refundQuery.SupplierId;
            }
        }
        private RefundApplyQuery GetRefundQuery()
        {
            RefundApplyQuery refundApplyQuery = new RefundApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                refundApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Operator"]))
            {
                refundApplyQuery.Operator = Globals.UrlDecode(this.Page.Request.QueryString["Operator"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SupplierId"]))
            {
                int supplierId;
                int.TryParse(this.Page.Request.QueryString["SupplierId"], out supplierId);
                refundApplyQuery.SupplierId = supplierId;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
            {
                int num = 0;
                if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
                {
                    refundApplyQuery.HandleStatus = new int?(num);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
            {
                refundApplyQuery.StratTime = this.Page.Request.QueryString["StartTime"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
            {
                refundApplyQuery.EndTime = this.Page.Request.QueryString["EndtTime"];
            }
            refundApplyQuery.PageIndex = this.pager.PageIndex;
            refundApplyQuery.PageSize = this.pager.PageSize;
            refundApplyQuery.SortBy = "ApplyForTime";
            refundApplyQuery.SortOrder = SortAction.Desc;
            return refundApplyQuery;
        }
        private void ReloadRefunds(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("OrderId", this.txtOrderId.Text);
            nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                nameValueCollection.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
            }
            if (!string.IsNullOrEmpty(this.ddlHandleStatus.SelectedValue))
            {
                nameValueCollection.Add("HandleStatus", this.ddlHandleStatus.SelectedValue);
            }
            if (this.ddlSupplier.SelectedIndex > 0)
            {
                nameValueCollection.Add("SupplierId", this.ddlSupplier.SelectedValue.ToString());
            }
            nameValueCollection.Add("StartTime", this.calendarStartDate.Text);
            nameValueCollection.Add("EndtTime", this.calendarEndDate.Text);
            nameValueCollection.Add("Operator", this.txtHandler.Text);
            base.ReloadPage(nameValueCollection);
        }
        protected void btnAcceptRefund_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderRefundApplyAccept);
            decimal refundTotal = 0m;

            if (string.IsNullOrEmpty(this.hidRefundTotal.Value))
            {
                this.ShowMsg("退款金额不能为空", false);
                return;
            }
            if (!decimal.TryParse(this.hidRefundTotal.Value, out refundTotal))
            {
                this.ShowMsg("退款金额只能为数字", false);
                return;
            }

            string username = HiContext.Current.User.Username;
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            if (refundTotal > orderInfo.GetTotal())
            {
                this.ShowMsg("退款金额不能大于订单金额", false);
                return;
            }

            bool isReturn = false;
            if (this.radBtnList.SelectedValue == "1")
            {
                isReturn = true;
            }

            if (OrderHelper.CheckRefund(orderInfo, username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), refundTotal, true, isReturn))
            {
                this.BindRefund();
                decimal amount = orderInfo.GetTotal();
                if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus != GroupBuyStatus.Failed)
                {
                    amount = orderInfo.GetTotal() - orderInfo.NeedPrice;
                }
                //调用微信接口退款
                bool isAutoRefund = this.hidIsAutoRefund.Value == "1";
                if (1 == 2)
                {
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    PackageInfo packageInfo = new PackageInfo();
                    PayClient payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);
                    RefundInfo refundinfo = OrderHelper.GetRefundByOrderId(orderInfo.OrderId);
                    packageInfo.TransactionId = orderInfo.GatewayOrderId;
                    packageInfo.OutTradeNo = orderInfo.OrderId;
                    packageInfo.OutRefundNo = refundinfo.RefundId.ToString();
                    packageInfo.RefundFee = (int)refundTotal * 100m;
                    packageInfo.TotalFee = (int)orderInfo.GetTotal() * 100m;
                    string ret = string.Empty;
                    try
                    {
                        ret = payClient.RequestRefund(packageInfo);
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Write("申请退款失败:" + ex.ToString());
                    }
                    ErrorLog.Write(ret);
                }
                Member user = Users.GetUser(orderInfo.UserId) as Member;
                //是否退券 radBtnList
                Messenger.OrderRefund(user, orderInfo, amount);
                this.ShowMsg("成功的确认了订单退款", true);
            }
        }
        private void btnRefuseRefund_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderRefundApplyRefuse);
            string username = HiContext.Current.User.Username;
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            OrderHelper.CheckRefund(orderInfo, username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), 0, false, false);
            this.BindRefund();
            this.ShowMsg("成功的拒绝了订单退款", true);
        }

        /// <summary>
        /// 按选择的的单据导出退款申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelOrderRefund_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderRefundApplySelExcel);
            int handleStatus = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["HandleStatus"]))
            {
                handleStatus = int.Parse(Request.QueryString["HandleStatus"]);
            }
            string text = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                text = base.Request["CheckBoxGroup"];
            }
            if (text.Length <= 0)
            {
                this.ShowMsg("请选选择要导出的退款申请单", false);
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
            DataSet orderRefunds = OrderHelper.GetExcelOrderRefund("", "", string.Join(",", list.ToArray()), handleStatus);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>退款申请单</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>订单编号</td>");
            stringBuilder.AppendLine("<td>订单金额</td>");
            stringBuilder.AppendLine("<td>退款金额</td>");
            stringBuilder.AppendLine("<td>申请时间</td>");
            stringBuilder.AppendLine("<td>退款备注</td>");
            stringBuilder.AppendLine("<td>处理状态</td>");
            stringBuilder.AppendLine("<td>处理时间</td>");
            stringBuilder.AppendLine("<td>管理员备注</td>");
            stringBuilder.AppendLine("<td>订单备注</td>");
            stringBuilder.AppendLine("<td>处理人</td>");


            stringBuilder.AppendLine("</tr>");
            foreach (DataRow dataRow in orderRefunds.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["OrderId"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["OrderTotal"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["RefundMoney"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["ApplyForTime"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["RefundRemark"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["HandleStatusStr"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["HandleTime"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["AdminRemark"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["Remark"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["Operator"] + "</td>");
                stringBuilder.AppendLine("</tr>");
            }
            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</body></html>");
            base.Response.Clear();
            base.Response.Buffer = false;
            base.Response.Charset = "GB2312";
            //base.Response.AppendHeader("Content-Disposition", "attachment;filename=OrderRefunds_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=退款申请单.xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }
        /// <summary>
        /// 按时间段导出退款详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelOrderRefundDetailsTime_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderRefundApplyTimeExcel);
            int handleStatus = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["HandleStatus"]))
            {
                handleStatus = int.Parse(Request.QueryString["HandleStatus"]);
            }
            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                this.ShowMsg("请选择要要导出退款申请单的时间区域", false);
                return;
            }

            string startTime = this.calendarStartDate.SelectedDate.Value.ToString();
            string endTime = this.calendarEndDate.SelectedDate.Value.ToString();

            DataSet orderRefunds = OrderHelper.GetExcelOrderRefundDetails(startTime, endTime, "", handleStatus);
            ExportExcel(orderRefunds, true);

        }
        /// <summary>
        /// 按选中退货单导出退款详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelOrderRefundDetails_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderRefundApplySelExcel);
            int handleStatus = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["HandleStatus"]))
            {
                handleStatus = int.Parse(Request.QueryString["HandleStatus"]);
            }
            string text = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                text = base.Request["CheckBoxGroup"];
            }
            if (text.Length <= 0)
            {
                this.ShowMsg("请选选择要导出的退款申请单", false);
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
            DataSet orderRefunds = OrderHelper.GetExcelOrderRefundDetails("", "", string.Join(",", list.ToArray()), handleStatus);

            ExportExcel(orderRefunds, false);
        }
        private void ExportExcel(DataSet orderRefunds, bool flag)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>退款申请单</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>订单编号</td>");
            stringBuilder.AppendLine("<td>订单金额</td>");
            stringBuilder.AppendLine("<td>退款金额</td>");
            stringBuilder.AppendLine("<td>申请时间</td>");
            stringBuilder.AppendLine("<td>退款备注</td>");
            stringBuilder.AppendLine("<td>处理状态</td>");
            stringBuilder.AppendLine("<td>处理时间</td>");
            stringBuilder.AppendLine("<td>管理员备注</td>");
            stringBuilder.AppendLine("<td>订单备注</td>");
            stringBuilder.AppendLine("<td>商品编号</td>");//订单商品明细
            stringBuilder.AppendLine("<td>商品名称</td>");
            stringBuilder.AppendLine("<td>商品单价</td>");
            stringBuilder.AppendLine("<td>商品数量</td>");
            stringBuilder.AppendLine("<td>商品税率</td>");//订单商品明细结束
            stringBuilder.AppendLine("<td>供应商</td>");
            stringBuilder.AppendLine("<td>规格</td>");
            stringBuilder.AppendLine("<td>处理人</td>");


            stringBuilder.AppendLine("</tr>");
            foreach (DataRow dataRow in orderRefunds.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["OrderId"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["OrderTotal"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["RefundMoney"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["ApplyForTime"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["RefundRemark"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["HandleStatusStr"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["HandleTime"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["AdminRemark"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["Remark"] + "</td>");

                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["ProductId"] + "</td>");//订单商品明细
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["ItemDescription"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["ItemAdjustedPrice"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:#,##0\">" + dataRow["Quantity"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["ItemTaxRate"] + "</td>");//订单商品明细结束
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["SupplierName"] + "</td>");//供应商
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["SKUContent"] + "</td>");

                stringBuilder.AppendLine("<td >" + dataRow["Operator"] + "</td>");

                stringBuilder.AppendLine("</tr>");
            }
            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</body></html>");

            base.Response.Clear();
            base.Response.Buffer = false;
            base.Response.Charset = "GB2312";
            //base.Response.AppendHeader("Content-Disposition", "attachment;filename=OrderRefunds_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            if (!flag)
            {
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=退款申请详情单.xls");
            }
            else
            {
                string startTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyyMMdd");
                string endTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyyMMdd");
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=退款申请详情单_" + startTime + "-" + endTime + ".xls");
            }
            // base.Response.AppendHeader("Content-Disposition", "attachment;filename=退款申请详情单_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }
        /// <summary>
        /// 按时间段导出退款申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelOrderRefundTime_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderRefundApplyTimeExcel);
            int handleStatus = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["HandleStatus"]))
            {
                handleStatus = int.Parse(Request.QueryString["HandleStatus"]);
            }
            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                this.ShowMsg("请选择要要导出退款申请单的时间区域", false);
                return;
            }

            string startTime = this.calendarStartDate.SelectedDate.Value.ToString();
            string endTime = this.calendarEndDate.SelectedDate.Value.ToString();

            DataSet orderRefunds = OrderHelper.GetExcelOrderRefund(startTime, endTime, "", handleStatus);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>退款申请单</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>订单编号</td>");
            stringBuilder.AppendLine("<td>订单金额</td>");
            stringBuilder.AppendLine("<td>退款金额</td>");
            stringBuilder.AppendLine("<td>申请时间</td>");
            stringBuilder.AppendLine("<td>退款备注</td>");
            stringBuilder.AppendLine("<td>处理状态</td>");
            stringBuilder.AppendLine("<td>处理时间</td>");
            stringBuilder.AppendLine("<td>管理员备注</td>");
            stringBuilder.AppendLine("<td>订单备注</td>");
            stringBuilder.AppendLine("<td>处理人</td>");


            stringBuilder.AppendLine("</tr>");
            foreach (DataRow dataRow in orderRefunds.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["OrderId"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["OrderTotal"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["RefundMoney"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["ApplyForTime"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["RefundRemark"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["HandleStatusStr"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["HandleTime"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["AdminRemark"] + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["Remark"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["Operator"] + "</td>");
                stringBuilder.AppendLine("</tr>");
            }
            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</body></html>");

            base.Response.Clear();
            base.Response.Buffer = false;
            base.Response.Charset = "GB2312";
            //base.Response.AppendHeader("Content-Disposition", "attachment;filename=OrderRefunds_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=退款申请单_" + this.calendarStartDate.SelectedDate.Value.ToString("yyyyMMdd") + "-" + this.calendarEndDate.SelectedDate.Value.ToString("yyyyMMdd") + ".xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }
    }
}

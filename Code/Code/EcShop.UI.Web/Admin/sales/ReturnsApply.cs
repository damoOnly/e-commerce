using ASPNET.WebControls;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Orders;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
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
	[PrivilegeCheck(Privilege.OrderReturnsApplyView)]
	public class ReturnsApply : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.DropDownList ddlHandleStatus;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;
		protected System.Web.UI.WebControls.DataList dlstReturns;
		protected Pager pager;
		protected System.Web.UI.WebControls.Label return_lblOrderId;
		protected System.Web.UI.WebControls.Label return_lblOrderTotal;
		protected System.Web.UI.WebControls.Label return_lblRefundType;
		protected System.Web.UI.WebControls.Label return_lblReturnRemark;
		protected System.Web.UI.WebControls.Label return_lblContacts;
		protected System.Web.UI.WebControls.Label return_lblEmail;
		protected System.Web.UI.WebControls.Label return_lblTelephone;
		protected System.Web.UI.WebControls.Label return_lblAddress;
		protected System.Web.UI.WebControls.TextBox return_txtRefundMoney;
        /// <summary>
        /// 收取清关费
        /// </summary>
        protected System.Web.UI.WebControls.TextBox return_txtCustomsClearanceFee;
        /// <summary>
        /// 收取快递费
        /// </summary>
        protected System.Web.UI.WebControls.TextBox return_txtExpressFee;
		protected System.Web.UI.WebControls.TextBox return_txtAdminRemark;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderTotal;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundType;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundMoney;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidAdminRemark;
        /// <summary>
        /// 清关费
        /// </summary>
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidCustomsClearanceFee;
        /// <summary>
        /// 快递费
        /// </summary>
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidExpressFee;
        /// <summary>
        /// 快递费用归属
        /// </summary>
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidFeeAffiliation;
        /// <summary>
        /// 确认退货按钮
        /// </summary>
		protected System.Web.UI.WebControls.Button btnAcceptReturn;
        
		protected System.Web.UI.WebControls.Button btnRefuseReturn;
        protected System.Web.UI.WebControls.Button btnExcelOrderReturns;
        protected System.Web.UI.WebControls.Button btnExcelOrderReturnsTime;

        protected System.Web.UI.WebControls.Button btnExcelOrderReturnsDetails;
        protected System.Web.UI.WebControls.Button btnExcelOrderReturnsDetailsTime;

        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;

        protected System.Web.UI.WebControls.TextBox txtHandler;//处理人
        protected SupplierDropDownList ddlSupplier;//供货商

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dlstReturns.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlstReturns_ItemDataBound);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			this.btnAcceptReturn.Click += new System.EventHandler(this.btnAcceptReturns_Click);
			this.btnRefuseReturn.Click += new System.EventHandler(this.btnRefuseReturns_Click);
            this.btnExcelOrderReturns.Click += new EventHandler(this.btnExcelOrderReturns_Click);
            this.btnExcelOrderReturnsTime.Click += new EventHandler(this.btnExcelOrderReturnsTime_Click);

            this.btnExcelOrderReturnsDetails.Click += new EventHandler(this.btnExcelOrderReturnsDetails_Click);
            this.btnExcelOrderReturnsDetailsTime.Click += new EventHandler(this.btnExcelOrderReturnsDetailsTime_Click);

			if (!base.IsPostBack)
			{
                this.ddlSupplier.DataBind();
				this.BindReturns();
			}
		}
		private void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApplyDelete);
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要删除的退货申请单", false);
				return;
			}
			string text2 = "成功删除了{0}个退货申请单";
			int num;
			if (OrderHelper.DelReturnsApply(text.Split(new char[]
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
			this.BindReturns();
			this.ShowMsg(text2, true);
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadReturnss(true);
		}
		private void dlstReturns_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReturns");
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
                if (label.Text == "3")
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
		private void BindReturns()
		{
			ReturnsApplyQuery returnsQuery = this.GetReturnsQuery();
            DbQueryResult returnsApplys = OrderHelper.GetReturnsApplysMoney(returnsQuery);
			this.dlstReturns.DataSource = returnsApplys.Data;
			this.dlstReturns.DataBind();
			this.pager.TotalRecords = returnsApplys.TotalRecords;
			this.pager1.TotalRecords = returnsApplys.TotalRecords;
			this.txtOrderId.Text = returnsQuery.OrderId;
            this.txtHandler.Text = returnsQuery.Operator;
            this.calendarEndDate.Text = returnsQuery.EndTime;
            this.calendarStartDate.Text = returnsQuery.StratTime;

			this.ddlHandleStatus.SelectedIndex = 0;
			if (returnsQuery.HandleStatus.HasValue && returnsQuery.HandleStatus.Value > -1)
			{
				this.ddlHandleStatus.SelectedValue = returnsQuery.HandleStatus.Value.ToString();
			}
            if (returnsQuery.SupplierId.HasValue && returnsQuery.SupplierId.Value > 0)
            {
                this.ddlSupplier.SelectedValue = returnsQuery.SupplierId;
            }
		}
		private ReturnsApplyQuery GetReturnsQuery()
		{
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				returnsApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				int num = 0;
				if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
				{
					returnsApplyQuery.HandleStatus = new int?(num);
				}
			}
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
            {
                returnsApplyQuery.StratTime = this.Page.Request.QueryString["StartTime"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
            {
                returnsApplyQuery.EndTime = this.Page.Request.QueryString["EndtTime"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Operator"]))
            {
                returnsApplyQuery.Operator = Globals.UrlDecode(this.Page.Request.QueryString["Operator"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SupplierId"]))
            {
                int supplierId;
                int.TryParse(this.Page.Request.QueryString["SupplierId"], out supplierId);
                returnsApplyQuery.SupplierId = supplierId;
            }    
			returnsApplyQuery.PageIndex = this.pager.PageIndex;
			returnsApplyQuery.PageSize = this.pager.PageSize;
			returnsApplyQuery.SortBy = "ApplyForTime";
			returnsApplyQuery.SortOrder = SortAction.Desc;
			return returnsApplyQuery;
		}
		private void ReloadReturnss(bool isSearch)
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
            nameValueCollection.Add("StartTime",this.calendarStartDate.Text);
            nameValueCollection.Add("EndtTime", this.calendarEndDate.Text);
            nameValueCollection.Add("Operator", this.txtHandler.Text);
			base.ReloadPage(nameValueCollection);
		}
		protected void btnAcceptReturns_Click(object sender, System.EventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApplyAccept);
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
            decimal customsClearanceFee;
            if (!decimal.TryParse(this.hidCustomsClearanceFee.Value, out customsClearanceFee))
            {
                this.ShowMsg("清关费需为数字格式！", false);
                return;
            }
            decimal expressFee;
            if (!decimal.TryParse(this.hidExpressFee.Value, out expressFee))
            {
                this.ShowMsg("快递费需为数字格式！", false);
                return;
            }
            int feeAffiliation;
            if (!int.TryParse(hidFeeAffiliation.Value, out feeAffiliation))
            {
                this.ShowMsg("请选择快递费用归属！", false);
                return;
            }
            string Affiliation;
            switch (feeAffiliation)
            {
                case 1:
                    Affiliation = "客户承担";
                    break;
                case 2:
                    Affiliation = "公司承担";
                    break;
                case 3:
                    Affiliation = "供应商承担";
                    break;
                default:
                    this.ShowMsg("快递费用归属选择错误！", false);
                return;
            }
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            OrderHelper.CheckReturn(orderInfo, HiContext.Current.User.Username, num, expressFee, customsClearanceFee, Affiliation, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true);
			this.BindReturns();
			this.ShowMsg("成功的确认了订单退货", true);
		}
		private void btnRefuseReturns_Click(object sender, System.EventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApplyRefuse);
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            OrderHelper.CheckReturn(orderInfo, HiContext.Current.User.Username, 0m, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
			this.BindReturns();
			this.ShowMsg("成功的拒绝了订单退货", true);
		}
        private void btnExcelOrderReturnsDetailsTime_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApplyTimeExcel);
            int handleStatus = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["HandleStatus"]))
            {
                handleStatus = int.Parse(Request.QueryString["HandleStatus"]);
            }

            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                this.ShowMsg("请选择要要导出退货申请单的时间区域", false);
                return;
            }

            string startTime = this.calendarStartDate.SelectedDate.Value.ToString();
            string endTime = this.calendarEndDate.SelectedDate.Value.ToString();

            DataSet orderReturns = OrderHelper.GetExcelOrderReturnsDetails(startTime, endTime, "", handleStatus);
            ExportExcel(orderReturns,true);
        }

        /// <summary>
        /// 按照选中的退回单导出退回单详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelOrderReturnsDetails_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApplySelExcel);
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
                this.ShowMsg("请选选择要导出的退货申请单", false);
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
            DataSet orderReturns = OrderHelper.GetExcelOrderReturnsDetails("", "", string.Join(",", list.ToArray()), handleStatus);
            ExportExcel(orderReturns,false);
        }
        private void ExportExcel(DataSet orderReturns,bool flag)
        {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>退货申请单</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>订单编号</td>");
            stringBuilder.AppendLine("<td>订单金额</td>");
            stringBuilder.AppendLine("<td>退款金额</td>");
            stringBuilder.AppendLine("<td>申请退货时间</td>");
            stringBuilder.AppendLine("<td>换货备注</td>");
            stringBuilder.AppendLine("<td>处理状态</td>");
            stringBuilder.AppendLine("<td>处理时间</td>");
            stringBuilder.AppendLine("<td>管理员备注</td>");
            stringBuilder.AppendLine("<td>订单备注</td>");
            stringBuilder.AppendLine("<td>商品编码</td>");//订单商品明细
            stringBuilder.AppendLine("<td>商品名称</td>");
            stringBuilder.AppendLine("<td>商品单价</td>");
            stringBuilder.AppendLine("<td>商品数量</td>");
            stringBuilder.AppendLine("<td>商品税率</td>");//订单商品明细结束
            stringBuilder.AppendLine("<td>供应商</td>");
            stringBuilder.AppendLine("<td>规格</td>");
            stringBuilder.AppendLine("<td>处理人</td>");


            stringBuilder.AppendLine("</tr>");
            foreach (DataRow dataRow in orderReturns.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["OrderId"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["OrderTotal"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["RefundMoney"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["ApplyForTime"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["Comments"] + "</td>");
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

                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["Operator"] + "</td>");
                stringBuilder.AppendLine("</tr>");
            }
            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</body></html>");
            base.Response.Clear();
            base.Response.Buffer = false;
            base.Response.Charset = "GB2312";
            //base.Response.AppendHeader("Content-Disposition", "attachment;filename=OrderReturns_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            if (!flag)
            {
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=退货申请详情单.xls");
            }
            else
            {
                string startTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyyMMdd");
                string endTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyyMMdd");
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=退货申请详情单_"+startTime+"-"+endTime+".xls");
            }
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }
        /// <summary>
        /// 按选择的的单据导出退货申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelOrderReturns_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApplySelExcel);
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
                this.ShowMsg("请选选择要导出的退货申请单", false);
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
            DataSet orderReturns = OrderHelper.GetExcelOrderReturns("", "", string.Join(",", list.ToArray()), handleStatus);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>退货申请单</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>订单编号</td>");
            stringBuilder.AppendLine("<td>订单金额</td>");
            stringBuilder.AppendLine("<td>退款金额</td>");
            stringBuilder.AppendLine("<td>申请退货时间</td>");
            stringBuilder.AppendLine("<td>换货备注</td>");
            stringBuilder.AppendLine("<td>处理状态</td>");
            stringBuilder.AppendLine("<td>处理时间</td>");
            stringBuilder.AppendLine("<td>管理员备注</td>");
            stringBuilder.AppendLine("<td>订单备注</td>");
            stringBuilder.AppendLine("<td>处理人</td>");


            stringBuilder.AppendLine("</tr>");
            foreach (DataRow dataRow in orderReturns.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["OrderId"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["OrderTotal"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["RefundMoney"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["ApplyForTime"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["Comments"] + "</td>");
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
            //base.Response.AppendHeader("Content-Disposition", "attachment;filename=OrderReturns_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=退货申请单.xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }



        /// <summary>
        /// 按时间段导出退货申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelOrderReturnsTime_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApplyTimeExcel);
            int handleStatus = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["HandleStatus"]))
            {
                handleStatus = int.Parse(Request.QueryString["HandleStatus"]);
            }

            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                this.ShowMsg("请选择要要导出退货申请单的时间区域", false);
                return;
            }

            string startTime = this.calendarStartDate.SelectedDate.Value.ToString();
            string endTime = this.calendarEndDate.SelectedDate.Value.ToString();

            DataSet orderReturns = OrderHelper.GetExcelOrderReturns(startTime, endTime, "", handleStatus);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>退货申请单</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>订单编号</td>");
            stringBuilder.AppendLine("<td>订单金额</td>");
            stringBuilder.AppendLine("<td>退款金额</td>");
            stringBuilder.AppendLine("<td>申请退货时间</td>");
            stringBuilder.AppendLine("<td>换货备注</td>");
            stringBuilder.AppendLine("<td>处理状态</td>");
            stringBuilder.AppendLine("<td>处理时间</td>");
            stringBuilder.AppendLine("<td>管理员备注</td>");
            stringBuilder.AppendLine("<td>订单备注</td>");
            stringBuilder.AppendLine("<td>处理人</td>");


            stringBuilder.AppendLine("</tr>");
            foreach (DataRow dataRow in orderReturns.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["OrderId"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["OrderTotal"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["RefundMoney"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["ApplyForTime"] + "</td>");
                stringBuilder.AppendLine("<td >" + dataRow["Comments"] + "</td>");
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
            //base.Response.AppendHeader("Content-Disposition", "attachment;filename=OrderReturns_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=退货申请单" + this.calendarStartDate.SelectedDate.Value.ToString("yyyyMMdd") +"-"+ this.calendarEndDate.SelectedDate.Value.ToString("yyyyMMdd")+".xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }
	}
}

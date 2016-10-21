using ASPNET.WebControls;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Orders;
using EcShop.Entities.Store;
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
	[PrivilegeCheck(Privilege.OrderReplaceApplyView)]
	public class ReplaceApply : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.DropDownList ddlHandleStatus;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;
		protected System.Web.UI.WebControls.DataList dlstReplace;
		protected Pager pager;
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
		protected System.Web.UI.WebControls.Button btnAcceptReplace;
		protected System.Web.UI.WebControls.Button btnRefuseReplace;
        protected System.Web.UI.WebControls.Button btnExcelOrderReplace;
        protected System.Web.UI.WebControls.Button btnExcelOrderReplaceTime;

        protected System.Web.UI.WebControls.Button btnExcelOrderReplaceDetails;
        protected System.Web.UI.WebControls.Button btnExcelOrderReplaceDetailsTime;

        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;

        protected System.Web.UI.WebControls.TextBox txtHandler;//处理人

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dlstReplace.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlstReplace_ItemDataBound);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			this.btnAcceptReplace.Click += new System.EventHandler(this.btnAcceptReplace_Click);
			this.btnRefuseReplace.Click += new System.EventHandler(this.btnRefuseReplace_Click);
            this.btnExcelOrderReplace.Click += new EventHandler(this.btnExcelOrderReplace_Click);
            this.btnExcelOrderReplaceTime.Click += new EventHandler(this.btnExcelOrderReplaceTime_Click);

            this.btnExcelOrderReplaceDetails.Click += new EventHandler(this.btnExcelOrderReplaceDetails_Click);
            this.btnExcelOrderReplaceDetailsTime.Click += new EventHandler(this.btnExcelOrderReplaceDetailsTime_Click);
			if (!base.IsPostBack)
			{
				this.BindReplace();
			}
		}
		private void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.OrderReplaceApplyDelete);
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要删除的换货申请单", false);
				return;
			}
			string text2 = "成功删除了{0}个换货申请单";
			int num;
			if (OrderHelper.DelReplaceApply(text.Split(new char[]
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
			this.BindReplace();
			this.ShowMsg(text2, true);
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadReplaces(true);
		}
		private void dlstReplace_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReplace");
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
		private void BindReplace()
		{
			ReplaceApplyQuery replaceQuery = this.GetReplaceQuery();
			DbQueryResult replaceApplys = OrderHelper.GetReplaceApplys(replaceQuery);
			this.dlstReplace.DataSource = replaceApplys.Data;
			this.dlstReplace.DataBind();
			this.pager.TotalRecords = replaceApplys.TotalRecords;
			this.pager1.TotalRecords = replaceApplys.TotalRecords;
			this.txtOrderId.Text = replaceQuery.OrderId;
            this.txtHandler.Text = replaceQuery.Operator;
			this.ddlHandleStatus.SelectedIndex = 0;

            this.calendarEndDate.Text = replaceQuery.EndTime;
            this.calendarStartDate.Text = replaceQuery.StratTime;

			if (replaceQuery.HandleStatus.HasValue && replaceQuery.HandleStatus.Value > -1)
			{
				this.ddlHandleStatus.SelectedValue = replaceQuery.HandleStatus.Value.ToString();
			}
		}
		private ReplaceApplyQuery GetReplaceQuery()
		{
			ReplaceApplyQuery replaceApplyQuery = new ReplaceApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				replaceApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				int num = 0;
				if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
				{
					replaceApplyQuery.HandleStatus = new int?(num);
				}
			}
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
            {
                replaceApplyQuery.StratTime = this.Page.Request.QueryString["StartTime"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
            {
                replaceApplyQuery.EndTime = this.Page.Request.QueryString["EndtTime"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Operator"]))
            {
                replaceApplyQuery.Operator = Globals.UrlDecode(this.Page.Request.QueryString["Operator"]);
            }
			replaceApplyQuery.PageIndex = this.pager.PageIndex;
			replaceApplyQuery.PageSize = this.pager.PageSize;
			replaceApplyQuery.SortBy = "ApplyForTime";
			replaceApplyQuery.SortOrder = SortAction.Desc;
			return replaceApplyQuery;
		}
		private void ReloadReplaces(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("OrderId", this.txtOrderId.Text);
            nameValueCollection.Add("Operator", this.txtHandler.Text);
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
            nameValueCollection.Add("StartTime", this.calendarStartDate.Text);
            nameValueCollection.Add("EndtTime", this.calendarEndDate.Text);
			base.ReloadPage(nameValueCollection);
		}
		protected void btnAcceptReplace_Click(object sender, System.EventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.OrderReplaceApplyAccept);
            OrderHelper.CheckReplace(this.hidOrderId.Value, this.hidAdminRemark.Value, true);
			this.BindReplace();
			this.ShowMsg("成功的确认了订单换货", true);
		}
		private void btnRefuseReplace_Click(object sender, System.EventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.OrderReplaceApplyRefuse);
            OrderHelper.CheckReplace(this.hidOrderId.Value, this.hidAdminRemark.Value, false);
			this.BindReplace();
			this.ShowMsg("成功的拒绝了订单换货", true);
		}
        /// <summary>
        /// 按选择的单据导出换货详情列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelOrderReplaceDetails_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderReplaceApplySelExcel);
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
                this.ShowMsg("请选选择要导出的换货申请单", false);
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

            DataSet orderReplaces = OrderHelper.GetExcelOrdeReplaceDetails("", "", string.Join(",", list.ToArray()), handleStatus);

            ExportExcel(orderReplaces,false);
        }
        /// <summary>
        /// 按选择的时间段导出换货详情列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelOrderReplaceDetailsTime_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderReplaceApplyTimeExcel);
            int handleStatus = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["HandleStatus"]))
            {
                handleStatus = int.Parse(Request.QueryString["HandleStatus"]);
            }
            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                this.ShowMsg("请选择要要导出换货申请单的时间区域", false);
                return;
            }

            string startTime = this.calendarStartDate.SelectedDate.Value.ToString();
            string endTime = this.calendarEndDate.SelectedDate.Value.ToString();

            DataSet orderReplaces = OrderHelper.GetExcelOrdeReplaceDetails(startTime, endTime, "", handleStatus);
            ExportExcel(orderReplaces,true);
        }
        private void ExportExcel(DataSet orderReplaces,bool flag)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>换货申请单</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>订单编号</td>");
            stringBuilder.AppendLine("<td>订单金额</td>");
            stringBuilder.AppendLine("<td>退款金额</td>");
            stringBuilder.AppendLine("<td>申请时间</td>");
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
            foreach (DataRow dataRow in orderReplaces.Tables[0].Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["OrderId"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["OrderTotal"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:#,##0.00\">" + dataRow["RefundMoney"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["ApplyForTime"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:@\">" + dataRow["Comments"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["HandleStatusStr"] + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:yyyy/mm/dd\">" + dataRow["HandleTime"] + "</td>");
                stringBuilder.AppendLine("<td  style=\"vnd.ms-excel.numberformat:@\">" + dataRow["AdminRemark"] + "</td>");
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
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=换货申请详情单.xls");
            }
            else
            {
                string startTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyyMMdd");
                string endTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyyMMdd");
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=换货申请详情单_" + startTime +"-"+endTime+ ".xls");
            }
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }
        /// <summary>
        /// 按选择的的单据导出换货申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelOrderReplace_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderReplaceApplySelExcel);
            int handleStatus = -1;
            if(!string.IsNullOrEmpty(Request.QueryString["HandleStatus"]))
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
                this.ShowMsg("请选选择要导出的换货申请单", false);
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
            DataSet orderReplaces = OrderHelper.GetExcelOrdeReplace("", "", string.Join(",", list.ToArray()), handleStatus);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>换货申请单</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>订单编号</td>");
            stringBuilder.AppendLine("<td>订单金额</td>");
            stringBuilder.AppendLine("<td>退款金额</td>");
            stringBuilder.AppendLine("<td>申请时间</td>");
            stringBuilder.AppendLine("<td>换货备注</td>");
            stringBuilder.AppendLine("<td>处理状态</td>");
            stringBuilder.AppendLine("<td>处理时间</td>");
            stringBuilder.AppendLine("<td>管理员备注</td>");
            stringBuilder.AppendLine("<td>订单备注</td>");
            stringBuilder.AppendLine("<td>处理人</td>");


            stringBuilder.AppendLine("</tr>");
            foreach (DataRow dataRow in orderReplaces.Tables[0].Rows)
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
           // base.Response.AppendHeader("Content-Disposition", "attachment;filename=OrderRefunds_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=换货申请单.xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }



        /// <summary>
        /// 按时间段导出换货申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelOrderReplaceTime_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderReplaceApplyTimeExcel);
            int handleStatus = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["HandleStatus"]))
            {
                handleStatus = int.Parse(Request.QueryString["HandleStatus"]);
            }
            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                this.ShowMsg("请选择要要导出换货申请单的时间区域", false);
                return;
            }

            string startTime = this.calendarStartDate.SelectedDate.Value.ToString();
            string endTime = this.calendarEndDate.SelectedDate.Value.ToString();

            DataSet orderReplaces = OrderHelper.GetExcelOrdeReplace(startTime, endTime, "", handleStatus);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>换货申请单</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>订单编号</td>");
            stringBuilder.AppendLine("<td>订单金额</td>");
            stringBuilder.AppendLine("<td>退款金额</td>");
            stringBuilder.AppendLine("<td>申请时间</td>");
            stringBuilder.AppendLine("<td>换货备注</td>");
            stringBuilder.AppendLine("<td>处理状态</td>");
            stringBuilder.AppendLine("<td>处理时间</td>");
            stringBuilder.AppendLine("<td>管理员备注</td>");
            stringBuilder.AppendLine("<td>订单备注</td>");
            stringBuilder.AppendLine("<td>处理人</td>");


            stringBuilder.AppendLine("</tr>");
            foreach (DataRow dataRow in orderReplaces.Tables[0].Rows)
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
            //base.Response.AppendHeader("Content-Disposition", "attachment;filename=OrderReplaces_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=换货申请单_" + this.calendarStartDate.SelectedDate.Value.ToString("yyyyMMdd") + "-" + this.calendarEndDate.SelectedDate.Value.ToString("yyyyMMdd") + ".xls");
            base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            this.EnableViewState = false;
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }
	}
}

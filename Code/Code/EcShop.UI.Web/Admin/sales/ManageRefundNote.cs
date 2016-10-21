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
    [PrivilegeCheck(Privilege.RefundOrderView)]
	public class ManageRefundNote : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;
		protected System.Web.UI.WebControls.DataList dlstRefundNote;
		protected Pager pager;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        protected System.Web.UI.WebControls.LinkButton btnCreateReport;


		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
            this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
			if (!base.IsPostBack)
			{
				this.BindRefundNote();
			}
		}
		private void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.RefundOrderDelete);
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要删除的退款单", false);
				return;
			}
			int num;
			OrderHelper.DelRefundApply(text.Split(new char[]
			{
				','
			}), out num);
			this.BindRefundNote();
			this.ShowMsg(string.Format("成功删除了{0}个退款单", num), true);
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadRefundNotes(true);
		}
		private void BindRefundNote()
		{
			RefundApplyQuery refundApplyQuery = new RefundApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				refundApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
            {
                refundApplyQuery.StratTime = Globals.UrlDecode(this.Page.Request.QueryString["StartDate"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
            {
                refundApplyQuery.EndTime = Globals.UrlDecode(this.Page.Request.QueryString["EndDate"]);
            }
			refundApplyQuery.HandleStatus = new int?(1);
			refundApplyQuery.PageIndex = this.pager.PageIndex;
			refundApplyQuery.PageSize = this.pager.PageSize;
			refundApplyQuery.SortBy = "HandleTime";
			refundApplyQuery.SortOrder = SortAction.Desc;
			DbQueryResult refundApplys = OrderHelper.GetRefundApplys(refundApplyQuery);
			this.dlstRefundNote.DataSource = refundApplys.Data;
			this.dlstRefundNote.DataBind();
			this.pager.TotalRecords = refundApplys.TotalRecords;
			this.pager1.TotalRecords = refundApplys.TotalRecords;
			this.txtOrderId.Text = refundApplyQuery.OrderId;
            this.calendarStartDate.Text = refundApplyQuery.StratTime;
            this.calendarEndDate.Text = refundApplyQuery.EndTime;
		}
		private void ReloadRefundNotes(bool isSearch)
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
            nameValueCollection.Add("StartDate", this.calendarStartDate.Text);
            nameValueCollection.Add("EndDate", this.calendarEndDate.Text);
			base.ReloadPage(nameValueCollection);
		}
        private void btnCreateReport_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.RefundOrderExcel);
            RefundApplyQuery refundApplyQuery = new RefundApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                refundApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
            {
                refundApplyQuery.StratTime = Globals.UrlDecode(this.Page.Request.QueryString["StartDate"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
            {
                refundApplyQuery.EndTime = Globals.UrlDecode(this.Page.Request.QueryString["EndDate"]);
            }
            refundApplyQuery.HandleStatus = new int?(1);
            refundApplyQuery.SortBy = "HandleTime";
            refundApplyQuery.SortOrder = SortAction.Desc;
            DataSet refundApplys = OrderHelper.ExportRefundApplys(refundApplyQuery);
            StringBuilder sbstr = new StringBuilder();

            string text = string.Empty;
            sbstr.Append("退款单号");
            sbstr.Append(",订单号");
            sbstr.Append(",退款人");
            sbstr.Append(",退款备注");
            sbstr.Append(",退款金额(元)");
            sbstr.Append(",退款时间");
            sbstr.Append(",退款方式");
            sbstr.Append(",操作员");
            sbstr.Append(",管理员备注\r\n");
            foreach (System.Data.DataRow dataRow in refundApplys.Tables[0].Rows)
            {
                sbstr.Append("`" + dataRow["RefundId"] );
                sbstr.Append(",`" + dataRow["OrderId"] );
                sbstr.Append(",`" + dataRow["Username"] );
                sbstr.Append(",`" + (StringExtension.ClearForJson(dataRow["RefundRemark"].ToString().Replace(",", "，"))) );
                sbstr.Append(",`" + dataRow["OrderTotal"] );
                sbstr.Append(",`" + dataRow["HandleTime"] );
                sbstr.Append(",`" + dataRow["RefundType"]);
                sbstr.Append(",`" + (StringExtension.ClearForJson(dataRow["Operator"].ToString().Replace(",", "，"))));
                sbstr.Append(",`" + (StringExtension.ClearForJson(dataRow["AdminRemark"].ToString().Replace(",", "，"))) + "\r\n");
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            //this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=ManageRefundNote.csv");
            if (this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue)
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=退款单_" + this.calendarStartDate.SelectedDate.Value.ToString("yyyyMMdd") + "-" + this.calendarEndDate.SelectedDate.Value.ToString("yyyyMMdd") + ".csv");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=退款单.csv");
            }
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(sbstr.ToString());
            this.Page.Response.End();
        }
	
	}
}

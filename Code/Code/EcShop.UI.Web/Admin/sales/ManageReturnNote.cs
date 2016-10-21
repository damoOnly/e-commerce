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
    [PrivilegeCheck(Privilege.ReturnOrderView)]
	public class ManageReturnNote : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;
		protected System.Web.UI.WebControls.DataList dlstReturnNote;
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
            ManagerHelper.CheckPrivilege(Privilege.ReturnOrderDelete);
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要删除的退货单", false);
				return;
			}
			int num;
			OrderHelper.DelReturnsApply(text.Split(new char[]
			{
				','
			}), out num);
			this.BindRefundNote();
			this.ShowMsg(string.Format("成功删除了{0}个退货单", num), true);
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadRefundNotes(true);
		}
		private void BindRefundNote()
		{
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				returnsApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
            {
                returnsApplyQuery.StratTime = Globals.UrlDecode(this.Page.Request.QueryString["StartDate"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
            {
                returnsApplyQuery.EndTime = Globals.UrlDecode(this.Page.Request.QueryString["EndDate"]);
            }
			returnsApplyQuery.HandleStatus = new int?(1);
			returnsApplyQuery.PageIndex = this.pager.PageIndex;
			returnsApplyQuery.PageSize = this.pager.PageSize;
			returnsApplyQuery.SortBy = "HandleTime";
			returnsApplyQuery.SortOrder = SortAction.Desc;
			DbQueryResult returnsApplys = OrderHelper.GetReturnsApplys(returnsApplyQuery);
			this.dlstReturnNote.DataSource = returnsApplys.Data;
			this.dlstReturnNote.DataBind();
			this.pager.TotalRecords = returnsApplys.TotalRecords;
			this.pager1.TotalRecords = returnsApplys.TotalRecords;
			this.txtOrderId.Text = returnsApplyQuery.OrderId;
            this.calendarEndDate.Text = returnsApplyQuery.EndTime;
            this.calendarStartDate.Text = returnsApplyQuery.StratTime;

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
            ManagerHelper.CheckPrivilege(Privilege.ReturnOrderExcel);
            ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                returnsApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
            {
                returnsApplyQuery.StratTime = Globals.UrlDecode(this.Page.Request.QueryString["StartDate"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
            {
                returnsApplyQuery.EndTime = Globals.UrlDecode(this.Page.Request.QueryString["EndDate"]);
            }
            returnsApplyQuery.HandleStatus = new int?(1);
            returnsApplyQuery.SortBy = "HandleTime";
            returnsApplyQuery.SortOrder = SortAction.Desc;
            DataSet returnsApplys = OrderHelper.ExportReturnsApplys(returnsApplyQuery);

            StringBuilder sbstr = new StringBuilder();

            string text = string.Empty;
            sbstr.Append("退货单号");
            sbstr.Append(",订单号");
            sbstr.Append(",会员");
            sbstr.Append(",退货备注");
            sbstr.Append(",退款金额(元)");
            sbstr.Append(",退货时间");
            sbstr.Append(",操作人");
            sbstr.Append(",管理员备注\r\n");
            foreach (System.Data.DataRow dataRow in returnsApplys.Tables[0].Rows)
            {

                sbstr.Append("`" + dataRow["ReturnsId"] );
                sbstr.Append(",`" + dataRow["OrderId"] );
                sbstr.Append(",`" + dataRow["Username"] );
                sbstr.Append(",`" + (StringExtension.ClearForJson(dataRow["Comments"].ToString().Replace(",", "，"))) );
                sbstr.Append(",`" + dataRow["RefundMoney"]);
                sbstr.Append(",`" + dataRow["HandleTime"] );
                sbstr.Append(",`" + (StringExtension.ClearForJson(dataRow["Operator"].ToString().Replace(",", "，"))) );
                sbstr.Append(",`" + (StringExtension.ClearForJson(dataRow["AdminRemark"].ToString().Replace(",", "，"))) + "\r\n");
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            if (this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue)
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=退货单_"+this.calendarStartDate.SelectedDate.Value.ToString("yyyyMMdd")+"-"+this.calendarEndDate.SelectedDate.Value.ToString("yyyyMMdd")+".csv");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=退货单.csv");
            }
            //this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=ManageReturnNote.csv");
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(sbstr.ToString());
            this.Page.Response.End();
        }
	
	}
}

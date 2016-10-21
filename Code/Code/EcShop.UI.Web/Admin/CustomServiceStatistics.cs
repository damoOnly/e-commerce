using ASPNET.WebControls;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.CustomServiceView)]
    public class CustomServiceStatistics : AdminPage
    {
        private System.DateTime dateStart;
        private System.DateTime dateEnd;
        private System.Int32 type = 1;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;

		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected System.Web.UI.WebControls.LinkButton btnCreateReport;
        protected System.Web.UI.HtmlControls.HtmlSelect selType;
		protected Grid grdCustomServiceStatistics;
		protected Pager pager;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.grdCustomServiceStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdProductSaleStatistics_ReBindData);
			this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
                this.BindCustomServiceStatistics();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
				{
                    this.dateStart = DateTime.Parse(this.Page.Request.QueryString["dateStart"]);
				}
                else
                {
                    this.dateStart = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
                }
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
				{
                    this.dateEnd = DateTime.Parse(this.Page.Request.QueryString["dateEnd"]);
				}
                else
                {
                    this.dateEnd = DateTime.Now.Date;
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["type"]))
                {
                    int.TryParse(this.Page.Request.QueryString["type"], out type);
                }
                this.calendarStartDate.SelectedDate = this.dateStart;
                this.calendarEndDate.SelectedDate = this.dateEnd;
                this.selType.SelectedIndex = type - 1;
               
				return;
			}
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("dateStart", this.calendarStartDate.SelectedDate.ToString());
			nameValueCollection.Add("dateEnd", this.calendarEndDate.SelectedDate.ToString());
            nameValueCollection.Add("type", this.selType.Value);
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                this.dateStart = this.calendarStartDate.SelectedDate.Value.Date;
                if (dateStart.Year != DateTime.Now.Year)
                {
                    ShowMsg("客服数据统计只能统计今年的数据，如果要统计往年的数据，请联系系统管理员", false);
                    return;
                }
            }
			base.ReloadPage(nameValueCollection);
		}
        private void BindCustomServiceStatistics()
		{
            //SaleStatisticsQuery saleStatisticsQuery = new SaleStatisticsQuery();
            //saleStatisticsQuery.StartDate = this.dateStart;
            //saleStatisticsQuery.EndDate = this.dateEnd;
            //saleStatisticsQuery.PageSize = this.pager.PageSize;
            //saleStatisticsQuery.PageIndex = this.pager.PageIndex;
            //saleStatisticsQuery.SortBy = "ProductSaleCounts";
            //saleStatisticsQuery.SortOrder = SortAction.Desc;
            int totalRecords = 0;
            System.Data.DataTable dtCustomServiceStatistics = SalesHelper.GetCustomServiceStatistics(this.dateStart, this.dateEnd, type, "", this.pager.PageSize, this.pager.PageIndex-1, ref totalRecords);
            this.grdCustomServiceStatistics.DataSource = dtCustomServiceStatistics;
            this.grdCustomServiceStatistics.DataBind();
			this.pager.TotalRecords = totalRecords;
		}
		private void btnCreateReport_Click(object sender, System.EventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.CustomServiceCreateReport);
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                this.dateStart = this.calendarStartDate.SelectedDate.Value.Date;
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                this.dateEnd = this.calendarEndDate.SelectedDate.Value.Date;
            }
            int totalRecords = 0;
            System.Data.DataTable dtCustomServiceStatistics = SalesHelper.GetCustomServiceStatistics(this.dateStart, this.dateEnd, type, "", 0, 0, ref totalRecords);//type=3 表示不分页
			string text = string.Empty;
			text += "日期";
			text += ",工号";
			text += ",接待人数";
			text += ",首次响应时间/秒";
			text += ",平均响应时间";
			text += ",回复次数\r\n";
            foreach (System.Data.DataRow dataRow in dtCustomServiceStatistics.Rows)
			{
                text += dataRow["HappenDate"].ToString();
                text = text + "," + dataRow["WorkerNo"].ToString();
                text = text + "," + dataRow["ReceptionPerson"].ToString();
                text = text + "," + dataRow["FirstReply"].ToString();
                text = text + "," + dataRow["AverageReply"].ToString();
                text = text + "," + dataRow["ReplyCount"].ToString() + "\r\n";
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
           // this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=CustomServiceStatistics.csv");
            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=客服数据统计.csv");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=客服数据统计_" +this.dateStart.ToString("yyyyMMdd") +"-" + this.dateEnd.ToString("yyyyMMdd") + ".csv");
            }
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.Response.ContentType = "application/octet-stream";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(text);
			this.Page.Response.End();
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		private void grdProductSaleStatistics_ReBindData(object sender)
		{
			this.ReBind(false);
		}
	}
}

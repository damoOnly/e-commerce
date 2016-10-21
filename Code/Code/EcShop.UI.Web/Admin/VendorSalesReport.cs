using ASPNET.WebControls;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Members;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	//[PrivilegeCheck(Privilege.MemberDrawRequestStatistics)]
    [PrivilegeCheck(Privilege.SuppliersSalesView)]
    public class VendorSalesReport : AdminPage
	{
        private string SupplierName;
		private System.DateTime? dateStart;
		private System.DateTime? dateEnd;
        protected System.Web.UI.WebControls.TextBox txtSupplierName;
		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
        protected System.Web.UI.WebControls.Button btnQuerySupplierDetails;
		protected System.Web.UI.WebControls.LinkButton btnCreateReport;
		protected PageSize hrefPageSize;
		protected Pager pager;
        protected Grid grdSupplierDetails;
		protected Pager pager1;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
            this.btnQuerySupplierDetails.Click += new System.EventHandler(this.btnQuerySupplierDetails_Click);
			this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.GetBalanceDrawRequest();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SupplierName"]))
				{
                    this.SupplierName = base.Server.UrlDecode(this.Page.Request.QueryString["SupplierName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
				{
					this.dateStart = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateStart"])));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
				{
					this.dateEnd = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateEnd"])));
				}
                this.txtSupplierName.Text = this.SupplierName;
				this.calendarStart.SelectedDate = this.dateStart;
				this.calendarEnd.SelectedDate = this.dateEnd;
				return;
			}
            this.SupplierName = this.txtSupplierName.Text;
			this.dateStart = this.calendarStart.SelectedDate;
			this.dateEnd = this.calendarEnd.SelectedDate;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("SupplierName", this.txtSupplierName.Text);
			nameValueCollection.Add("dateStart", this.calendarStart.SelectedDate.ToString());
			nameValueCollection.Add("dateEnd", this.calendarEnd.SelectedDate.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}
		public void GetBalanceDrawRequest()
		{

			DbQueryResult balanceDetails = MemberHelper.GetVendorSalesReport(new VendorSalesReportQuery
			{
                SupplierName = this.SupplierName,
				StartDate = this.dateStart,
				EndDate = this.dateEnd,
				PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
				TradeType = TradeTypes.DrawRequest
			});
            this.grdSupplierDetails.DataSource = balanceDetails.Data;
            this.grdSupplierDetails.DataBind();
			this.pager1.TotalRecords = (this.pager.TotalRecords = balanceDetails.TotalRecords);
		}
		private void btnCreateReport_Click(object sender, System.EventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.SuppliersSalesExcel);
            DbQueryResult balanceDetails = MemberHelper.GetVendorSalesReport(new VendorSalesReportQuery
            {
                SupplierName = this.SupplierName,
                StartDate = this.dateStart,
                EndDate = this.dateEnd,
                PageIndex = this.pager.PageIndex,
                PageSize = int.MaxValue,
                TradeType = TradeTypes.DrawRequest
            });
			string text = string.Empty;
			text += "供应商名称";
			text += ",销售总金额";
            text += ",订单总数\r\n";
            foreach (System.Data.DataRow dataRow in ((System.Data.DataTable)balanceDetails.Data).Rows)
			{
                text += dataRow["SupplierName"];
                text = text + "," + dataRow["OrderTotal"];
                text = text + "," + dataRow["OrderCount"];
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					",",
					"\r\n"
				});
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			//this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=BalanceDetailsStatistics.csv");
            if (!(this.calendarStart.SelectedDate.HasValue && this.calendarEnd.SelectedDate.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=供应商销售统计报表.csv");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=供应商销售统计报表_" + this.dateStart.Value.ToString("yyyyMMdd") + "-" + this.dateEnd.Value.ToString("yyyyMMdd") + ".csv");
            }
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.Response.ContentType = "application/octet-stream";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(text);

            EventLogs.WriteOperationLog(Privilege.SuppliersSalesExcel, string.Format(CultureInfo.InvariantCulture, "用户{0}，生成供应商为{1}供应商销售成功", new object[]
			{
               this.User.Identity.Name,
               this.SupplierName
			}));

			this.Page.Response.End();


           
		}
        private void btnQuerySupplierDetails_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
	}
}

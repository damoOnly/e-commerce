using ASPNET.WebControls;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core.Enums;
using EcShop.Entities.Sales;
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
    [PrivilegeCheck(Privilege.ProductSaleAsImportSourceView)]
    public class ProductSaleAsImportSource : AdminPage
	{
		private System.DateTime? dateStart;
		private System.DateTime? dateEnd;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected System.Web.UI.WebControls.LinkButton btnCreateReport;
        protected Grid grdProductSaleAsImportSource;
		protected Pager pager;
        protected ImportSourceTypeDropDownList ddlImportSourceType;
        
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.grdProductSaleAsImportSource.ReBindData += new Grid.ReBindDataEventHandler(this.grdProductSaleAsImportSource_ReBindData);
			this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
                this.ddlImportSourceType.DataBind();
                this.BindProductSaleAsImportSource();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
				{
					this.dateStart = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["dateStart"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
				{
					this.dateEnd = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["dateEnd"]));
				}
                int value2 = 0;
                if (int.TryParse(this.Page.Request.QueryString["importSourceId"], out value2))
                {
                    this.ddlImportSourceType.SelectedValue = new int?(value2);
                }
				this.calendarStartDate.SelectedDate = this.dateStart;
				this.calendarEndDate.SelectedDate = this.dateEnd;
				return;
			}
			this.dateStart = this.calendarStartDate.SelectedDate;
			this.dateEnd = this.calendarEndDate.SelectedDate;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("dateStart", this.calendarStartDate.SelectedDate.ToString());
			nameValueCollection.Add("dateEnd", this.calendarEndDate.SelectedDate.ToString());
            if (this.ddlImportSourceType.SelectedValue.HasValue)
            {
                nameValueCollection.Add("importSourceId", this.ddlImportSourceType.SelectedValue.ToString());
            }
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}
        private void BindProductSaleAsImportSource()
		{
            this.LoadParameters();
			SaleStatisticsQuery saleStatisticsQuery = new SaleStatisticsQuery();
			saleStatisticsQuery.StartDate = this.dateStart;
			saleStatisticsQuery.EndDate = this.dateEnd;
			saleStatisticsQuery.PageSize = this.pager.PageSize;
			saleStatisticsQuery.PageIndex = this.pager.PageIndex;
            saleStatisticsQuery.SortBy = "b.ImportSourceId";
            saleStatisticsQuery.SortOrder = SortAction.Asc;
            saleStatisticsQuery.ImportSourceId = this.ddlImportSourceType.SelectedValue.HasValue ? this.ddlImportSourceType.SelectedValue : null;
			int totalRecords = 0;
			System.Data.DataTable productSales = SalesHelper.GetProductSalesAsImportSource(saleStatisticsQuery, out totalRecords);
            this.grdProductSaleAsImportSource.DataSource = productSales;
            this.grdProductSaleAsImportSource.DataBind();
			this.pager.TotalRecords = totalRecords;
		}
		private void btnCreateReport_Click(object sender, System.EventArgs e)
		{
            ManagerHelper.CheckPrivilege(Privilege.ProductSaleAsImportSourceExcel);
			SaleStatisticsQuery saleStatisticsQuery = new SaleStatisticsQuery();
			saleStatisticsQuery.StartDate = this.dateStart;
			saleStatisticsQuery.EndDate = this.dateEnd;
			saleStatisticsQuery.PageSize = this.pager.PageSize;
            saleStatisticsQuery.SortBy = "b.ImportSourceId";
            saleStatisticsQuery.SortOrder = SortAction.Asc;
			int num = 0;
			System.Data.DataTable productSalesNoPage = SalesHelper.GetProductSaleAsImportSourceNoPage(saleStatisticsQuery, out num);
			string text = string.Empty;
            text += "原产地";
            //text += ",排行";
			text += ",商品名称";
			text += ",商家编码";
			text += ",销售量";
			text += ",销售额";
            text += ",利润\r\n";
			foreach (System.Data.DataRow dataRow in productSalesNoPage.Rows)
			{
                text += dataRow["CnArea"].ToString();
                //text = text + "," + dataRow["IDOfSaleTotals"].ToString();
				text = text + "," + dataRow["ProductName"].ToString();
				text = text + "," + dataRow["SKU"].ToString();
				text = text + "," + dataRow["ProductSaleCounts"].ToString();
				text = text + "," + dataRow["ProductSaleTotals"].ToString();
                text = text + "," + dataRow["ProductProfitsTotals"].ToString() + "\r\n";
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
            //this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=ProductSaleAsImportSource.csv");
            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=按产地汇总商品销售.csv");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=按产地汇总商品销售_" + this.dateStart.Value.ToString("yyyyMMdd") + "-" + this.dateEnd.Value.ToString("yyyyMMdd") + ".csv");
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
        private void grdProductSaleAsImportSource_ReBindData(object sender)
		{
			this.ReBind(false);
		}
	}
}

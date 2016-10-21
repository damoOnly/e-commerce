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
	[PrivilegeCheck(Privilege.ProductSaleRanking)]
	public class ProductSaleRanking : AdminPage
	{
		private System.DateTime? dateStart;
		private System.DateTime? dateEnd;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected System.Web.UI.WebControls.LinkButton btnCreateReport;
		protected Grid grdProductSaleStatistics;
		protected Pager pager;
        protected SupplierDropDownList ddlSupplier;
        protected System.Web.UI.WebControls.TextBox txtProductName;
        protected System.Web.UI.WebControls.TextBox txtProductCode;

        private int? supplierId;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.grdProductSaleStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdProductSaleStatistics_ReBindData);
			this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindProductSaleRanking();
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
                int iSupplier = 0;
                if (int.TryParse(this.Page.Request.QueryString["supplierId"], out iSupplier))
                {
                    this.supplierId = new int?(iSupplier);
                }
                this.ddlSupplier.DataBind();
                this.ddlSupplier.SelectedValue = this.supplierId;
				this.calendarStartDate.SelectedDate = this.dateStart;
				this.calendarEndDate.SelectedDate = this.dateEnd;
                this.txtProductName.Text = this.Page.Request.QueryString["productName"];
                this.txtProductCode.Text = this.Page.Request.QueryString["productCode"];

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
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
            if (this.ddlSupplier.SelectedValue.HasValue)
            {
                nameValueCollection.Add("supplierId", this.ddlSupplier.SelectedValue.ToString());
            }

            nameValueCollection.Add("productName", this.txtProductName.Text);
            nameValueCollection.Add("productCode", this.txtProductCode.Text);

			base.ReloadPage(nameValueCollection);
		}
		private void BindProductSaleRanking()
		{
			SaleStatisticsQuery saleStatisticsQuery = new SaleStatisticsQuery();
			saleStatisticsQuery.StartDate = this.dateStart;
			saleStatisticsQuery.EndDate = this.dateEnd;
			saleStatisticsQuery.PageSize = this.pager.PageSize;
			saleStatisticsQuery.PageIndex = this.pager.PageIndex;
			saleStatisticsQuery.SortBy = "ProductSaleCounts";
			saleStatisticsQuery.SortOrder = SortAction.Desc;
            saleStatisticsQuery.SupplierId = this.supplierId;
            saleStatisticsQuery.ProductName = this.txtProductName.Text.Trim();
            saleStatisticsQuery.ProductCode = this.txtProductCode.Text.Trim();

			int totalRecords = 0;
			System.Data.DataTable productSales = SalesHelper.GetProductSales(saleStatisticsQuery, out totalRecords);
			this.grdProductSaleStatistics.DataSource = productSales;
			this.grdProductSaleStatistics.DataBind();
			this.pager.TotalRecords = totalRecords;
		}
		private void btnCreateReport_Click(object sender, System.EventArgs e)
		{
			SaleStatisticsQuery saleStatisticsQuery = new SaleStatisticsQuery();
			saleStatisticsQuery.StartDate = this.dateStart;
			saleStatisticsQuery.EndDate = this.dateEnd;
			saleStatisticsQuery.PageSize = this.pager.PageSize;
			saleStatisticsQuery.SortBy = "ProductSaleCounts";
			saleStatisticsQuery.SortOrder = SortAction.Desc;
            saleStatisticsQuery.SupplierId = this.ddlSupplier.SelectedValue.HasValue ? this.ddlSupplier.SelectedValue : 0;
			int num = 0;
			System.Data.DataTable productSalesNoPage = SalesHelper.GetProductSalesNoPage(saleStatisticsQuery, out num);
			string text = string.Empty;
			text += "排行";
			text += ",商品名称";
            text += ",英文名称";
            text += ",供应商";
			text += ",商家编码";
			text += ",销售量";
			text += ",销售额";
			text += ",利润\r\n";
			foreach (System.Data.DataRow dataRow in productSalesNoPage.Rows)
			{
				text += dataRow["IDOfSaleTotals"].ToString();
				text = text + "," + dataRow["ProductName"].ToString();
                text = text + "," + dataRow["EnglishName"].ToString();
                text = text + "," + dataRow["SupplierName"].ToString();
				text = text + "," + dataRow["SKU"].ToString();
				text = text + "," + dataRow["ProductSaleCounts"].ToString();
				text = text + "," + dataRow["ProductSaleTotals"].ToString();
				text = text + "," + dataRow["ProductProfitsTotals"].ToString() + "\r\n";
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			//this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=ProductSaleRanking.csv");
            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=商品销售排行.csv");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=商品销售排行_" + this.dateStart.Value.ToString("yyyyMMdd") + "-" + this.dateEnd.Value.ToString("yyyyMMdd") + ".csv");
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

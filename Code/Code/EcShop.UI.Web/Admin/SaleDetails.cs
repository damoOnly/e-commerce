using ASPNET.WebControls;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SaleDetails)]
	public class SaleDetails : AdminPage
	{
		private System.DateTime? startTime;
		private System.DateTime? endTime;
        private int? supplierId;
        private string productName;
		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
		protected System.Web.UI.WebControls.Button btnQuery;
		protected System.Web.UI.WebControls.GridView grdOrderLineItem;
        protected System.Web.UI.WebControls.TextBox txtProductName;//商品名称
        protected SupplierDropDownList ddlSupplier;//供货商
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
                this.ddlSupplier.DataBind();
                this.ddlSupplier.SelectedValue = this.supplierId;
				this.BindList();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startTime"]))
				{
					this.startTime = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startTime"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endTime"]))
				{
					this.endTime = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endTime"]));
				}
                if (!string.IsNullOrEmpty(Request.QueryString["productName"]))
                {
                    this.productName = Request.QueryString["productName"];                    
                }
                if (!string.IsNullOrEmpty(Request.QueryString["supplierId"]))
                {
                    int result;
                    int.TryParse(Request.QueryString["supplierId"], out result);
                    this.supplierId = result;                    
                }
				this.calendarStart.SelectedDate = this.startTime;
				this.calendarEnd.SelectedDate = this.endTime;
                this.txtProductName.Text = this.productName;
                this.ddlSupplier.SelectedValue = this.supplierId;
				return;
			}
			this.startTime = this.calendarStart.SelectedDate;
			this.endTime = this.calendarEnd.SelectedDate;
            this.supplierId = this.ddlSupplier.SelectedValue;
            this.productName = this.txtProductName.Text;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("startTime", this.calendarStart.SelectedDate.ToString());
			nameValueCollection.Add("endTime", this.calendarEnd.SelectedDate.ToString());
            nameValueCollection.Add("productName", this.txtProductName.Text);
            if (this.ddlSupplier.SelectedValue.HasValue)
            {
                nameValueCollection.Add("supplierId", this.ddlSupplier.SelectedValue.Value.ToString());
            }
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
		private void btnQuery_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		private void BindList()
		{
			SaleStatisticsQuery saleStatisticsQuery = new SaleStatisticsQuery();
			saleStatisticsQuery.PageIndex = this.pager.PageIndex;
			saleStatisticsQuery.PageSize = this.pager.PageSize;
			saleStatisticsQuery.StartDate = this.startTime;
			saleStatisticsQuery.EndDate = this.endTime;
            saleStatisticsQuery.ProductName = this.productName;
            saleStatisticsQuery.SupplierId = this.supplierId;
			DbQueryResult saleOrderLineItemsStatistics = SalesHelper.GetSaleOrderLineItemsStatistics(saleStatisticsQuery);
			this.grdOrderLineItem.DataSource = saleOrderLineItemsStatistics.Data;
			this.grdOrderLineItem.DataBind();
			this.pager.TotalRecords = saleOrderLineItemsStatistics.TotalRecords;
			this.grdOrderLineItem.PageSize = saleStatisticsQuery.PageSize;
		}
	}
}

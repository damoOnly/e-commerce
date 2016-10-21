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
    [PrivilegeCheck(Privilege.SaleStoreDetails)]
    public class SaleStoreDetails : AdminPage
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
        protected System.Web.UI.WebControls.LinkButton btnCreateReport;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);

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

        /// <summary>
        /// 生成报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateReport_Click(object sender, System.EventArgs e)
        {
            SaleStatisticsQuery saleStatisticsQuery = new SaleStatisticsQuery();
            saleStatisticsQuery.StartDate = this.startTime;
            saleStatisticsQuery.EndDate = this.endTime;
            saleStatisticsQuery.ProductName = this.productName;
            saleStatisticsQuery.SupplierId = this.supplierId;
            System.Data.DataTable saleOrderLineDt = SalesHelper.GetProductVisitAndDt(saleStatisticsQuery);

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>商品代码</td>");
            stringBuilder.AppendLine("<td>商品条码</td>");
            stringBuilder.AppendLine("<td>商品中文名称</td>");
            stringBuilder.AppendLine("<td>商品英文名称</td>");
            stringBuilder.AppendLine("<td>商品规格</td>");
            stringBuilder.AppendLine("<td>产地</td>");
            stringBuilder.AppendLine("<td>品牌</td>");
            stringBuilder.AppendLine("<td>销售单价</td>");
            stringBuilder.AppendLine("<td>销售总额</td>");
            stringBuilder.AppendLine("<td>销售数量</td>");
            stringBuilder.AppendLine("<td>库存</td>");
            stringBuilder.AppendLine("<td>库存总额</td>");
            stringBuilder.AppendLine("<td>商家</td>");
            stringBuilder.AppendLine("</tr>");
            foreach (System.Data.DataRow dataRow in saleOrderLineDt.Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td>" + dataRow["SkuId"].ToString() + "</td>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["BarCode"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["ProductName"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["EnglishName"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["ProductStandard"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["CnArea"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["BrandName"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["SalePrice"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["Sumsales"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["SalesNum"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["Stock"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["InventoryCost"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["SupplierName"].ToString() + "</td>");
                stringBuilder.AppendLine("</tr>");
            }
            //stringBuilder.AppendLine("<tr>");
            //stringBuilder.AppendLine("<td>当前查询结果共计," + userOrdersNoPage.TotalCount + "</td>");
            //stringBuilder.AppendLine("<td>订单金额共计," + userOrdersNoPage.TotalOfSearch + "</td>");
            //stringBuilder.AppendLine("<td>订单毛利润共计," + userOrdersNoPage.ProfitsOfSearch + "</td>");
            //stringBuilder.AppendLine("<td></td>");
            //stringBuilder.AppendLine("</tr>");
            stringBuilder.AppendLine("</table>");
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            //this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=UserOrderStatistics.xls");
            if (!(startTime.HasValue && this.endTime.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=订单统计.xls");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=订单统计_" + this.startTime.Value.ToString("yyyyMMdd") + "-" + this.endTime.Value.ToString("yyyyMMdd") + ".xls");
            }
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/ms-excel";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(stringBuilder.ToString());
            this.Page.Response.End();
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
            DbQueryResult saleOrderLineItemsStatistics = SalesHelper.GetSaleStoreOrderLineItemsStatistics(saleStatisticsQuery);
			this.grdOrderLineItem.DataSource = saleOrderLineItemsStatistics.Data;
			this.grdOrderLineItem.DataBind();
			this.pager.TotalRecords = saleOrderLineItemsStatistics.TotalRecords;
			this.grdOrderLineItem.PageSize = saleStatisticsQuery.PageSize;
		}
	}
}

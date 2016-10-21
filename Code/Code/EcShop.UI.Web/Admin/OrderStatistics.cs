using ASPNET.WebControls;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Entities.Orders;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.OrderStatistics)]
    public class OrderStatistics : AdminPage
    {
        private string productName;
        private int? supplierId;
        private string userName;
        private string shipTo;
        private System.DateTime? dateStart;
        private System.DateTime? dateEnd;
        private string orderId;
        protected System.Web.UI.WebControls.TextBox txtUserName;
        protected System.Web.UI.WebControls.TextBox txtShipTo;
        protected System.Web.UI.WebControls.TextBox txtOrderId;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.LinkButton btnCreateReport;
        protected System.Web.UI.WebControls.Repeater rp_orderStatistics;
        protected System.Web.UI.WebControls.Label lblPageCount;
        protected System.Web.UI.WebControls.Label lblSearchCount;
        protected Pager pager;
        /// <summary>
        /// 站点
        /// </summary>
        protected SitesDropDownList sitesDropDownList;//站点
        /// <summary>
        /// 订单来源
        /// </summary>
        protected System.Web.UI.WebControls.DropDownList ddlOrderSource;
        protected System.Web.UI.WebControls.TextBox txtProductName;//商品名称
        protected SupplierDropDownList ddlSupplier;//供货商
        private int orderSource;
        private int? siteId;
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindUserOrderStatistics();
                this.sitesDropDownList.DataBind();
                this.sitesDropDownList.SelectedValue = this.siteId;
                this.ddlSupplier.DataBind();
                this.ddlSupplier.SelectedValue = this.supplierId;
            }
        }
        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
                {
                    this.userName = base.Server.UrlDecode(this.Page.Request.QueryString["userName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shipTo"]))
                {
                    this.shipTo = base.Server.UrlDecode(this.Page.Request.QueryString["shipTo"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
                {
                    this.dateStart = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["dateStart"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
                {
                    this.dateEnd = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["dateEnd"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
                {
                    this.orderId = Globals.UrlDecode(this.Page.Request.QueryString["orderId"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["siteId"]))//站点Id
                {
                    this.siteId = new int?(int.Parse(this.Page.Request.QueryString["siteId"]));
                }
                if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["orderSource"]))
                {
                    this.orderSource = int.Parse(this.Page.Request.QueryString["orderSource"]);
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
                this.txtUserName.Text = this.userName;
                this.txtShipTo.Text = this.shipTo;
                this.calendarStartDate.SelectedDate = this.dateStart;
                this.calendarEndDate.SelectedDate = this.dateEnd;
                this.txtOrderId.Text = this.orderId;
                this.sitesDropDownList.SelectedValue = this.siteId;
                this.ddlOrderSource.BindEnum<EcShop.Entities.Orders.OrderSource>();//默认显示全部
                this.ddlOrderSource.SelectedValue = ((OrderSource)this.orderSource).ToShowText();
                this.ddlSupplier.SelectedValue = this.supplierId;
                this.txtProductName.Text = this.productName;
                return;
            }
            this.userName = this.txtUserName.Text;
            this.shipTo = this.txtShipTo.Text;
            this.dateStart = this.calendarStartDate.SelectedDate;
            this.dateEnd = this.calendarEndDate.SelectedDate;
            this.orderId = this.txtOrderId.Text;
            this.siteId = this.sitesDropDownList.SelectedValue;
            this.orderSource = (int)(OrderSource)System.Enum.Parse(typeof(OrderSource), this.ddlOrderSource.SelectedValue);
            this.productName = this.txtProductName.Text;
            this.supplierId = this.ddlSupplier.SelectedValue;
        }
        private void ReBind(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("userName", this.txtUserName.Text);
            nameValueCollection.Add("shipTo", this.txtShipTo.Text);
            nameValueCollection.Add("dateStart", this.calendarStartDate.SelectedDate.ToString());
            nameValueCollection.Add("dateEnd", this.calendarEndDate.SelectedDate.ToString());
            nameValueCollection.Add("orderId", this.txtOrderId.Text);
            nameValueCollection.Add("siteId", ((int)this.sitesDropDownList.SelectedValue).ToString());
            nameValueCollection.Add("orderSource", ((int)(OrderSource)System.Enum.Parse(typeof(OrderSource), this.ddlOrderSource.SelectedValue)).ToString());
            nameValueCollection.Add("productName", this.txtProductName.Text);
            if (this.ddlSupplier.SelectedValue.HasValue)
            {
                nameValueCollection.Add("supplierId", this.ddlSupplier.SelectedValue.Value.ToString());
            }
            //{
            //        "siteId",
            //        ((int)this.sitesDropDownList.SelectedValue).ToString()
            //    }
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            base.ReloadPage(nameValueCollection);
        }
        protected void rp_orderStatistics_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rp_orderitem");
            string a = ((System.Data.DataRowView)e.Item.DataItem)["OrderId"].ToString();
            if (a != "")
            {
                repeater.DataSource = OrderHelper.GetOrderInfo(a).LineItems.Values;
                repeater.DataBind();
            }
        }
        private void BindUserOrderStatistics()
        {
            OrderStatisticsInfo userOrders = SalesHelper.GetUserOrders(new OrderQuery
            {
                UserName = this.userName,
                ShipTo = this.shipTo,
                StartDate = this.dateStart,
                EndDate = this.dateEnd,
                OrderId = this.orderId,
                PageSize = this.pager.PageSize,
                SiteId = this.siteId,
                PageIndex = this.pager.PageIndex,
                SortBy = "OrderDate",
                SortOrder = SortAction.Desc,
                SourceOrder = this.orderSource,
                ProductName = this.productName,
                SupplierId = this.supplierId
            });
            this.rp_orderStatistics.DataSource = userOrders.OrderTbl;
            this.rp_orderStatistics.DataBind();
            this.pager.TotalRecords = userOrders.TotalCount;
            this.lblPageCount.Text = string.Format("当前页共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">订单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">订单毛利润共计</span><span class=\"colorG\">{2}</span>元 ", userOrders.OrderTbl.Rows.Count, Globals.FormatMoney(userOrders.TotalOfPage), Globals.FormatMoney(userOrders.ProfitsOfPage));
            this.lblSearchCount.Text = string.Format("当前查询结果共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">订单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">订单毛利润共计</span><span class=\"colorG\">{2}</span>元 ", userOrders.TotalCount, Globals.FormatMoney(userOrders.TotalOfSearch), Globals.FormatMoney(userOrders.ProfitsOfSearch));
        }
        private void btnCreateReport_Click(object sender, System.EventArgs e)
        {
            OrderStatisticsInfo userOrdersNoPage = SalesHelper.GetUserOrdersNoPage(new OrderQuery
            {
                UserName = this.userName,
                ShipTo = this.shipTo,
                StartDate = this.dateStart,
                EndDate = this.dateEnd,
                OrderId = this.orderId,
                SiteId = this.siteId,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortBy = "OrderDate",
                SortOrder = SortAction.Desc,
                ProductName = this.productName,
                SupplierId = this.supplierId
            });
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>订单号</td>");
            stringBuilder.AppendLine("<td>下单时间</td>");
            stringBuilder.AppendLine("<td>总订单金额</td>");
            stringBuilder.AppendLine("<td>用户名</td>");
            stringBuilder.AppendLine("<td>收货人</td>");
            stringBuilder.AppendLine("<td>利润</td>");
            stringBuilder.AppendLine("</tr>");
            foreach (System.Data.DataRow dataRow in userOrdersNoPage.OrderTbl.Rows)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["OrderId"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["OrderDate"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["Total"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["UserName"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["ShipTo"].ToString() + "</td>");
                stringBuilder.AppendLine("<td>" + dataRow["Profits"].ToString() + "</td>");
                stringBuilder.AppendLine("</tr>");
            }
            stringBuilder.AppendLine("<tr>");
            stringBuilder.AppendLine("<td>当前查询结果共计," + userOrdersNoPage.TotalCount + "</td>");
            stringBuilder.AppendLine("<td>订单金额共计," + userOrdersNoPage.TotalOfSearch + "</td>");
            stringBuilder.AppendLine("<td>订单毛利润共计," + userOrdersNoPage.ProfitsOfSearch + "</td>");
            stringBuilder.AppendLine("<td></td>");
            stringBuilder.AppendLine("</tr>");
            stringBuilder.AppendLine("</table>");
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            //this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=UserOrderStatistics.xls");
            if (!(this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue))
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=订单统计.xls");
            }
            else
            {
                this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=订单统计_" + this.dateStart.Value.ToString("yyyyMMdd") + "-" + this.dateEnd.Value.ToString("yyyyMMdd") + ".xls");
            }
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/ms-excel";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(stringBuilder.ToString());
            this.Page.Response.End();
        }
        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReBind(true);
        }
    }
}

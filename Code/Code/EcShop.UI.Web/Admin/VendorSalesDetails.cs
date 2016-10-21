using ASPNET.WebControls;
using EcShop.ControlPanel.Members;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MemberAccount)]
    public class VendorSalesDetails : AdminPage
	{

		private System.DateTime? dataStart;
		private System.DateTime? dataEnd;
        private string OrderId;
        private string ProductName;
        private string ProductCode;
        private string SupplierId=string.Empty;
        protected System.Web.UI.WebControls.TextBox txtOrderId;
        protected System.Web.UI.WebControls.TextBox txtProductName;
        protected System.Web.UI.WebControls.TextBox txtProductCode;


		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;

        protected System.Web.UI.WebControls.Button btnQuerySupplierDetails;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdBalanceDetails;
		protected Pager pager1;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
            this.btnQuerySupplierDetails.Click += new System.EventHandler(this.btnQuerySupplierDetails_Click);

		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindBalanceDetails();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
                if (string.IsNullOrEmpty(this.Page.Request.QueryString["SupplierId"]))
                {
                    base.GotoResourceNotFound();
                    return;
                }
               
                this.SupplierId=this.Page.Request.QueryString["SupplierId"];
                this.ViewState["UserId"] = this.SupplierId;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
				{
                    this.OrderId=this.Page.Request.QueryString["OrderId"];
				}
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
				{
                    this.ProductName = this.Page.Request.QueryString["ProductName"];
				}
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductCode"]))
                {
                    this.ProductCode = this.Page.Request.QueryString["ProductCode"];
                }
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
				{
					this.dataStart = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
				{
					this.dataEnd = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
				}

				this.calendarStart.SelectedDate = this.dataStart;
				this.calendarEnd.SelectedDate = this.dataEnd;
				return;
			}

			this.dataStart = this.calendarStart.SelectedDate;
			this.dataEnd = this.calendarEnd.SelectedDate;
		}
		private void ReBind(bool isSearch)
		{
           
             this.SupplierId = this.ViewState["UserId"].ToString();
        
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();

            nameValueCollection.Add("OrderId", this.txtOrderId.Text);
            nameValueCollection.Add("ProductCode",this.txtProductCode.Text);
            nameValueCollection.Add("ProductName",this.txtProductName.Text);
            nameValueCollection.Add("SupplierId", this.SupplierId);
			
            nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("dataStart", this.calendarStart.SelectedDate.ToString());
			nameValueCollection.Add("dataEnd", this.calendarEnd.SelectedDate.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void BindBalanceDetails()
		{
            DbQueryResult balanceDetails = MemberHelper.GetVendorSalesDetail(new VendorSalesDetailQuery
			{
                SupplierId=this.SupplierId,
				FromDate = this.dataStart,
				ToDate = this.dataEnd,
				OrderId=this.OrderId,
                ProductCode=this.ProductCode,
                ProductName=this.ProductName,
				PageIndex = this.pager.PageIndex,
				
				PageSize = this.pager.PageSize
			});
			this.grdBalanceDetails.DataSource = balanceDetails.Data;
			this.grdBalanceDetails.DataBind();
			this.pager.TotalRecords = balanceDetails.TotalRecords;
			this.pager1.TotalRecords = balanceDetails.TotalRecords;


            this.txtProductName.Text = this.ProductName;
            this.txtProductCode.Text = this.ProductCode;
            this.txtOrderId.Text = this.OrderId;
            this.calendarEnd.SelectedDate = this.dataEnd;
            this.calendarStart.SelectedDate = this.dataStart;
		}

        private void btnQuerySupplierDetails_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
  
		}
	}
}

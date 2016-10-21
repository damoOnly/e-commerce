using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.HS;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.UI.Web.Admin
{
    public class HSDockingDisplay : AdminPage
    {
        private string searchkey;
        private string OrderStatus;
        private string PaymentStatus;
        private string LogisticsStatus;
        private string payerIdStatus;
        protected System.Web.UI.WebControls.TextBox OrderIDSearchText;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.ImageButton lkbReSend;
        protected System.Web.UI.WebControls.DropDownList OrderIDList;
        protected System.Web.UI.WebControls.DropDownList LogisticsNoList;
        protected System.Web.UI.WebControls.DropDownList PaymentList;
        protected System.Web.UI.WebControls.DropDownList payerIdList;
        protected Grid grdHSDocking;
        protected Pager pager;


        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.grdHSDocking.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdHSDocking_RowDataBound);
        }

        private void grdHSDocking_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Label lblOrderStatus = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblOrderStatus");
                System.Web.UI.WebControls.Label lblLogisticsStatus = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblLogisticsStatus");
                System.Web.UI.WebControls.Label lblPaymentStatus = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblPaymentStatus");
                System.Web.UI.WebControls.Label lblPayerIdStatus = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblPayerIdStatus");

                SetRowValue(lblOrderStatus);
                SetRowValue(lblLogisticsStatus);
                SetRowValue(lblPaymentStatus);
                SetRowValue(lblPayerIdStatus);
            }
        }

        private void SetRowValue(System.Web.UI.WebControls.Label status)
        { 
            switch(status.Text)
            {
                case "0":
                    status.Text = "未开始";
                    break;
                case "1":
                    status.Text = "进行中";
                    break;
                case "2":
                    status.Text = "已完成";
                    break;
                case "3":
                    status.Text = "失败";
                    break;
                case "4":
                    status.Text = "重试";
                    break;
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }


        protected void Page_Load()
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
                {
                    this.searchkey = Globals.UrlDecode(this.Page.Request.QueryString["searchKey"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderStatus"]))
                {
                    this.OrderStatus = Globals.UrlDecode(this.Page.Request.QueryString["OrderStatus"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["LogisticsStatus"]))
                {
                    this.LogisticsStatus = Globals.UrlDecode(this.Page.Request.QueryString["LogisticsStatus"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PaymentStatus"]))
                {
                    this.PaymentStatus = Globals.UrlDecode(this.Page.Request.QueryString["PaymentStatus"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["payerIdStatus"]))
                {
                    this.payerIdStatus = Globals.UrlDecode(this.Page.Request.QueryString["payerIdStatus"]);
                }
                this.OrderIDSearchText.Text = this.searchkey;
                this.OrderIDList.SelectedValue = this.OrderStatus;
                this.LogisticsNoList.SelectedValue = this.LogisticsStatus;
                this.PaymentList.SelectedValue = this.PaymentStatus;
                this.payerIdList.SelectedValue = this.payerIdStatus;

                return;
            }
            this.searchkey = this.OrderIDSearchText.Text.Trim();

            this.OrderStatus = this.OrderIDList.SelectedValue;
            this.LogisticsStatus = this.LogisticsNoList.SelectedValue;
            this.PaymentStatus = this.PaymentList.SelectedValue;
            this.payerIdStatus = this.payerIdList.SelectedValue;
            
        }
        private void BindData()
        {
            DbQueryResult HSDocking = HSCodeHelper.GetHSDocking(new HSDockingQuery
            {
                OrderId = this.searchkey,
                LogisticsStatus = this.LogisticsNoList.SelectedValue,
                OrderStatus = this.OrderIDList.SelectedValue,
                PaymentStatus = this.PaymentList.SelectedValue,
                payerIdStatus = this.payerIdList.SelectedValue,
                //HS_NAME = this.HSName,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize
            });
            this.grdHSDocking.DataSource = HSDocking.Data;
            this.grdHSDocking.DataBind();
            this.pager.TotalRecords = HSDocking.TotalRecords;
        }

        private void ReBind(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("searchKey", this.OrderIDSearchText.Text);
            //nameValueCollection.Add("pageSize", "10");
            //nameValueCollection.Add("HS_NAME", this.nameSearchText.Text);
            nameValueCollection.Add("OrderStatus", this.OrderIDList.SelectedValue);
            nameValueCollection.Add("PaymentStatus", this.PaymentList.SelectedValue);
            nameValueCollection.Add("LogisticsStatus", this.LogisticsNoList.SelectedValue);
            nameValueCollection.Add("payerIdStatus", this.payerIdList.SelectedValue);
            nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            base.ReloadPage(nameValueCollection);
        }

    }
}

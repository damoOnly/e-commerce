using ASPNET.WebControls;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.UsedVouchers)]
    public class UsedVouchers:AdminPage
    {
        private string voucherName = string.Empty;
        private string voucherOrder = string.Empty;
        private int? voucherstatus;
        private int? VoucherId;
        protected System.Web.UI.WebControls.TextBox txtVoucherName;
        protected System.Web.UI.WebControls.TextBox txtOrderID;
        protected System.Web.UI.WebControls.DropDownList Dpstatus;
        protected System.Web.UI.WebControls.Button btnSearch;
        protected Grid grdVouchers;
        protected Pager pager;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindVoucherList();
            }
        }
        private void ReloadHelpList(bool isSearch)
        {
            string voucherName = Globals.UrlEncode(this.txtVoucherName.Text.Trim());
            string OrderID = Globals.UrlEncode(this.txtOrderID.Text.Trim());
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("voucherName", voucherName);
            nameValueCollection.Add("OrderID", OrderID);
            nameValueCollection.Add("VoucherStatus", this.Dpstatus.SelectedValue);
            if (!isSearch)
            {
                nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
            if (this.Dpstatus.SelectedValue != "1")
            {
                if (string.IsNullOrEmpty(OrderID) && string.IsNullOrEmpty(voucherName))
                {
                    ShowMsg("使用状态没有选择 已使用，则需要填写现金券名称或者订单号", false);
                    return;
                }
            }
            base.ReloadPage(nameValueCollection);
        }
        protected string IsVoucherEnd(object endtime)
        {
            System.DateTime dateTime = System.Convert.ToDateTime(endtime);
            if (dateTime.CompareTo(System.DateTime.Now) > 0)
            {
                return dateTime.ToString();
            }
            return "已过期";
        }
        protected void BindVoucherList()
        {
            if (string.IsNullOrEmpty(this.voucherName) &&
                string.IsNullOrEmpty(this.voucherOrder) &&
                !this.voucherstatus.HasValue &&
                !this.VoucherId.HasValue)
            {
                return;
            }
            DbQueryResult vouchersList = VoucherHelper.GetVouchersList(new VoucherItemInfoQuery
            {
                VoucherName = this.voucherName,
                OrderId = this.voucherOrder,
                VoucherId = this.VoucherId,
                VoucherStatus = this.voucherstatus,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "GenerateTime",
                SortOrder = SortAction.Desc
            });
            this.pager.TotalRecords = vouchersList.TotalRecords;
            this.grdVouchers.DataSource = vouchersList.Data;
            this.grdVouchers.DataBind();
        }
        private void LoadParameters()
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["voucherName"]))
                {
                    this.voucherName = Globals.UrlDecode(this.Page.Request.QueryString["voucherName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderID"]))
                {
                    this.voucherOrder = Globals.UrlDecode(this.Page.Request.QueryString["OrderID"]);
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["VoucherStatus"]))
                {
                    this.voucherstatus = new int?(System.Convert.ToInt32(base.Request.QueryString["VoucherStatus"]));
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["VoucherId"]))
                {
                    this.VoucherId = new int?(System.Convert.ToInt32(base.Request.QueryString["VoucherId"]));
                }
                this.txtOrderID.Text = this.voucherOrder;
                this.txtVoucherName.Text = this.voucherName;
                this.Dpstatus.SelectedValue = System.Convert.ToString(this.voucherstatus);
                return;
            }
            this.voucherName = this.txtVoucherName.Text;
            this.voucherOrder = this.txtOrderID.Text;
        }
        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            this.ReloadHelpList(true);
        }
    }
}

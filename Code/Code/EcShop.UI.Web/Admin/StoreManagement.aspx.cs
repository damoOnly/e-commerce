using ASPNET.WebControls;
using Commodities;
using EcShop;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.StoreView)]
    public partial class StoreManagement : AdminPage
    {

        private string searchkey;
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected Grid grdStore;
        protected Pager pager;
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.grdStore.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdStore_RowDeleting);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindStore();
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
                this.txtSearchText.Text = this.searchkey;
                return;
            }
            this.searchkey = this.txtSearchText.Text.Trim();
        }
        private void ReBind(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("searchKey", this.txtSearchText.Text);
            nameValueCollection.Add("pageSize", "10");
            if (!isSearch)
            {
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            base.ReloadPage(nameValueCollection);
        }
        private void BindStore()
        {
            int userStoreId = ManagerHelper.GetStoreIdByUserId(HiContext.Current.User.UserId);
            DbQueryResult productTypes = StoreManagementHelper.GetStore(new StoreQuery
            {
                StoreName = this.searchkey,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                StoreId = userStoreId
            });
            this.grdStore.DataSource = productTypes.Data;
            this.grdStore.DataBind();
            this.pager.TotalRecords = productTypes.TotalRecords;
        }
        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReBind(true);
        }
        private void grdStore_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.StoreDelete);
            int supplierId = (int)this.grdStore.DataKeys[e.RowIndex].Value;
            if (StoreManagementHelper.DeleteStore(supplierId))
            {
                this.BindStore();
                this.ShowMsg("删除成功", true);
                return;
            }
            this.ShowMsg("删除失败", false);
        }
    }
}

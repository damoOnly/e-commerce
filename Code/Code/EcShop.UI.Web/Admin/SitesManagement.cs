using ASPNET.WebControls;
using Commodities;
using EcShop;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SitesView)]
    public partial class SitesManagement : AdminPage
    {

        private string searchkey;
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected Grid grdSupplier;
        protected Pager pager;
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.grdSupplier.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdSupplier_RowDeleting);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindSites();
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
        private void BindSites()
        {
            DbQueryResult productTypes = SitesManagementHelper.GetSite(new SiteQuery
            {
                SiteName = this.searchkey,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize
            });
            this.grdSupplier.DataSource = productTypes.Data;
            this.grdSupplier.DataBind();
            this.pager.TotalRecords = productTypes.TotalRecords;
        }
        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReBind(true);
        }
        private void grdSupplier_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {

            ManagerHelper.CheckPrivilege(Privilege.SitesDelete);
            int SitesId = (int)this.grdSupplier.DataKeys[e.RowIndex].Value;
            if (SitesManagementHelper.DeleteSites(SitesId))
            {
                this.BindSites();
                this.ShowMsg("删除成功", true);
                return;
            }
            this.ShowMsg("删除失败", false);
        }
    }
}

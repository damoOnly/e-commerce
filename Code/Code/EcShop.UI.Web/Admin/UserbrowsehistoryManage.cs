using ASPNET.WebControls;
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
    public partial class UserbrowsehistoryManage : AdminPage
    {

        private string searchkey;
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected Grid grdBrowseHistory;
        protected Pager pager;
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.grdBrowseHistory.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdBrowseHistory_RowDeleting);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindBrowseHistorys();
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
        private void BindBrowseHistorys()
        {
            DbQueryResult productTypes = UserbrowsehistoryHelper.GetBrowseHistory(new UserbrowsehistoryQuery
            {
                UserName = this.searchkey,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize
            });

            this.grdBrowseHistory.DataSource = productTypes.Data;
            this.grdBrowseHistory.DataBind();
            this.pager.TotalRecords = productTypes.TotalRecords;
        }
        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReBind(true);
        }
        private void grdBrowseHistory_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {

            //ManagerHelper.CheckPrivilege(Privilege.SitesDelete);
            int HistoryId = (int)this.grdBrowseHistory.DataKeys[e.RowIndex].Value;
            if (UserbrowsehistoryHelper.DeleteUserBrowseHistory(HistoryId))
            {
                this.BindBrowseHistorys();
                this.ShowMsg("删除成功", true);
                return;
            }
            this.ShowMsg("删除失败", false);
        }
    }
}

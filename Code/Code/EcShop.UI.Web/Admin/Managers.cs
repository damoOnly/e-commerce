using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [AdministerCheck(true)]
    public class Managers : AdminPage
    {
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        protected RoleDropDownList dropRolesList;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected Pager pager;
        protected Grid grdManager;
        protected Pager pager1;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.grdManager.ReBindData += new Grid.ReBindDataEventHandler(this.grdManager_ReBindData);
            this.grdManager.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdManager_RowDeleting);
            this.grdManager.RowDataBound += grdManager_RowDataBound;
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropRolesList.DataBind();
                this.BindData();
            }
        }

        void grdManager_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label label = e.Row.FindControl("lblDept") as Label;
            if (label != null)
            {
                var userName = label.Text;
                string[] userRoleNames = RoleHelper.GetUserRoleNames(userName);
                foreach (var roleName in userRoleNames)
                {
                    if (string.Compare(roleName, "SystemAdministrator") == 0)
                    {
                        label.Text = "超级管理员";
                    }
                    else if (!RoleHelper.IsBuiltInRole(roleName))
                    {
                        label.Text = roleName;
                    }
                }
            }
        }

        private void grdManager_ReBindData(object sender)
        {
            this.ReloadManagerLogs(false);
        }
        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReloadManagerLogs(true);
        }
        private void grdManager_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int num = (int)this.grdManager.DataKeys[e.RowIndex].Value;
            if (HiContext.Current.User.UserId == num)
            {
                this.ShowMsg("不能删除自己", false);
                return;
            }
            SiteManager manager = ManagerHelper.GetManager(num);
            if (!ManagerHelper.Delete(manager.UserId))
            {
                this.ShowMsg("未知错误", false);
                return;
            }
            this.BindData();
            this.ShowMsg("成功删除了一个管理员", true);
        }
        private void BindData()
        {
            ManagerQuery managerQuery = this.GetManagerQuery();
            DbQueryResult managers = ManagerHelper.GetManagers(managerQuery);
            this.grdManager.DataSource = managers.Data;
            this.grdManager.DataBind();
            this.txtSearchText.Text = managerQuery.Username;
            this.dropRolesList.SelectedValue = managerQuery.RoleId;
            this.pager.TotalRecords = managers.TotalRecords;
            this.pager1.TotalRecords = managers.TotalRecords;
        }
        private void ReloadManagerLogs(bool isSearch)
        {
            System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
            nameValueCollection.Add("Username", this.txtSearchText.Text);
            nameValueCollection.Add("RoleId", System.Convert.ToString(this.dropRolesList.SelectedValue));
            if (!isSearch)
            {
                nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            nameValueCollection.Add("SortBy", this.grdManager.SortOrderBy);
            nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(nameValueCollection);
        }
        private ManagerQuery GetManagerQuery()
        {
            ManagerQuery managerQuery = new ManagerQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Username"]))
            {
                managerQuery.Username = base.Server.UrlDecode(this.Page.Request.QueryString["Username"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RoleId"]))
            {
                managerQuery.RoleId = new System.Guid(this.Page.Request.QueryString["RoleId"]);
            }
            managerQuery.PageSize = this.pager.PageSize;
            managerQuery.PageIndex = this.pager.PageIndex;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortBy"]))
            {
                managerQuery.SortBy = this.Page.Request.QueryString["SortBy"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
            {
                managerQuery.SortOrder = SortAction.Desc;
            }
            return managerQuery;
        }
    }
}

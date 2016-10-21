using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.SqlDal.PCMenu;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Linq;
namespace EcShop.UI.Web.Admin
{


    [AdministerCheck(true)]
    public class RolePermissions : AdminPage
    {
        protected LinkButton btnSetTop;
        private bool hasAliohRight;
        private bool hasAppRight;
        private bool hasVstoreRight;
        private bool hasWapRight;
        protected Literal lblRoleName;
        private string RequestRoleId;

        private void btnSet_Click(object sender, EventArgs e)
        {
            Guid roleId = new Guid(this.RequestRoleId);
            this.RequestRoleId = base.Request.Form["ctl00$contentHolder$newprivilegeid"];
            IEnumerable<IGrouping<string, string>> enumerable = from c in this.RequestRoleId.Split(new char[] { ',' }) group c by c;
            List<string> values = new List<string>();
            foreach (IGrouping<string, string> grouping in enumerable)
            {
                values.Add(grouping.Key);
            }
            string text = string.Join(",", values);
            this.PermissionsSet(roleId, text);
            this.Page.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/store/RolePermissions.aspx?roleId={0}&Status=1", roleId)));
        }

        private void LoadData(Guid roleId)
        {
            IList<int> privilegeByRoles = RoleHelper.GetPrivilegeByRoles(roleId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            bool flag = false;
            string domainName = Globals.DomainName;
            if ((((domainName.EndsWith("vip.ecdev.cn") || domainName.StartsWith("localhost")) || (domainName.StartsWith("127.0.0.1") || domainName.EndsWith("example.com"))) || domainName.EndsWith("ecdev.cn")) || domainName.EndsWith("haimylife.com"))
            {
                flag = true;
            }
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["roleId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.RequestRoleId = this.Page.Request.QueryString["roleId"];
                this.btnSetTop.Click += new EventHandler(this.btnSet_Click);
                if (!this.Page.IsPostBack)
                {
                    Guid roleID = new Guid(this.RequestRoleId);
                    if (Regex.IsMatch(this.RequestRoleId, "[A-Z0-9]{8}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{12}", RegexOptions.IgnoreCase))
                    {
                        RoleInfo role = RoleHelper.GetRole(roleID);
                        if (role.Name == "SystemAdministrator")
                        {
                            this.lblRoleName.Text = "超级管理员";
                        }
                        else
                        {
                            this.lblRoleName.Text = role.Name;
                        }
                    }
                    if (this.Page.Request.QueryString["Status"] == "1")
                    {
                        this.ShowMsg("设置部门权限成功", true);
                    }
                    this.LoadData(roleID);
                }
            }
        }

        private void PermissionsSet(Guid roleId, string text)
        {
            RoleHelper.AddPrivilegeInRoles(roleId, text);
            ManagerHelper.ClearRolePrivilege(roleId);
        }
    }
}


using ASPNET.WebControls;
using Commodities;
using EcShop;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SitesAdd)]
    public class AddSites : AdminPage
    {
        protected System.Web.UI.WebControls.TextBox txtSitesName;
        protected System.Web.UI.WebControls.TextBox txtsitCode;
        protected System.Web.UI.WebControls.RadioButtonList Rb_IsDefault;
        protected System.Web.UI.WebControls.TextBox txtSort;
        protected System.Web.UI.WebControls.TextBox txtDescription;
        protected System.Web.UI.WebControls.Button btnSave;
        protected RegionSelectoNoCountys ddlRegions;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            int RSort=0;
            SitesManagementInfo SiteInfo = new SitesManagementInfo();
            SiteInfo.SitesName = this.txtSitesName.Text.Trim();
            if (!this.ddlRegions.GetSelectedRegionId().HasValue)
            {
                this.ShowMsg("请选择地区", false);
                return;
            }
            SiteInfo.City = this.ddlRegions.GetSelectedRegionId().Value;
            SiteInfo.Code = this.txtsitCode.Text.Trim();
            SiteInfo.IsDefault = int.Parse(this.Rb_IsDefault.SelectedValue.Trim());
            SiteInfo.Sort = int.TryParse(this.txtSort.Text.Trim(), out RSort) ? RSort : 0;
            SiteInfo.Description = this.txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(SiteInfo.SitesName ) && string.IsNullOrEmpty(SiteInfo.Code))
            {
                this.ShowMsg("站点名称和站点编码必填其一", false);
                return;
            }
            int issucess = SitesManagementHelper.AddSites(SiteInfo);
            if (issucess > 0)
            {
                this.txtSitesName.Text = "";
                this.txtsitCode.Text = "";
                this.Rb_IsDefault.SelectedValue = "0";
                this.txtSort.Text = "";
                this.txtDescription.Text = "";

                this.ShowMsg("添加成功", true);
                base.Response.Redirect(Globals.GetAdminAbsolutePath("/sites/SitesManagement.aspx"), true);
                return;
            }
            this.ShowMsg("添加失败", false);
        }
    }
}

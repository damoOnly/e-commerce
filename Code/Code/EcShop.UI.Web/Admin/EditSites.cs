using ASPNET.WebControls;
using Commodities;
using EcShop;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Sales;
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
    [PrivilegeCheck(Privilege.SitesEdit)]
    public class EditSites : AdminPage
    {
        private int Siteid;
        protected System.Web.UI.WebControls.TextBox txtSitesName;
        protected System.Web.UI.WebControls.TextBox txtsitCode;
        protected System.Web.UI.WebControls.RadioButtonList Rb_IsDefault;
        protected System.Web.UI.WebControls.TextBox txtSort;
        protected System.Web.UI.WebControls.TextBox txtDescription;
        protected System.Web.UI.WebControls.Button btnSave;
        protected RegionSelectoNoCountys ddlRegions;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["SitesId"], out this.Siteid))
            {
                base.GotoResourceNotFound();
                return;
            }
            this.btnSave.Click += new System.EventHandler(this.btnEditShipper_Click);
            if (!this.Page.IsPostBack)
            {
                SitesManagementInfo Sites = SitesManagementHelper.GetSites(this.Siteid);
                if (Sites == null)
                {
                    base.GotoResourceNotFound();
                    return;
                }
                Globals.EntityCoding(Sites, false);
                this.txtSitesName.Text =Sites.SitesName ;
                this.ddlRegions.SetSelectedRegionId(new int?(Sites.City));
                this.txtsitCode.Text =Sites.Code;
                this.Rb_IsDefault.SelectedValue = Sites.IsDefault.ToString();
                this.txtSort.Text = Sites.Sort.ToString(); ;
                this.txtDescription.Text = Sites.Description; ;
            }
        }
        private void btnEditShipper_Click(object sender, System.EventArgs e)
        {
            int RSort = 0;
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
            SiteInfo.SitesId=this.Siteid;
            if (string.IsNullOrEmpty(SiteInfo.SitesName) && string.IsNullOrEmpty(SiteInfo.Code))
            {
                this.ShowMsg("站点名称和站点编码必填其一", false);
                return;
            }
            if (SitesManagementHelper.UpdateStore(SiteInfo))
            {
                this.txtSitesName.Text = "";
                this.txtsitCode.Text = "";
                this.Rb_IsDefault.SelectedValue = "0";
                this.txtSort.Text = "";
                this.txtDescription.Text = "";

                this.ShowMsg("修改成功", true);
                base.Response.Redirect(Globals.GetAdminAbsolutePath("/sites/SitesManagement.aspx"), true);
                return;
            }
            this.ShowMsg("修改失败", false);
        }

    }
}

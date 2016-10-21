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
    [PrivilegeCheck(Privilege.StoreAdd)]
    public class AddStore : AdminPage
    {
        protected System.Web.UI.WebControls.TextBox txtSupplierName;
        protected System.Web.UI.WebControls.TextBox txtPhone;
        protected System.Web.UI.WebControls.TextBox txtMobile;
        protected System.Web.UI.WebControls.TextBox txtAddress;
        protected System.Web.UI.WebControls.TextBox txtDescription;
        protected System.Web.UI.WebControls.Button btnSave;
        protected RegionSelector ddlReggion;


        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //this.ddlReggion.FindControl("ddlRegions3").Visible = false;
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {

            StoreManagementInfo StoreInfo = new StoreManagementInfo();
            StoreInfo.StoreName = this.txtSupplierName.Text.Trim();
            if (!this.ddlReggion.GetSelectedRegionId().HasValue)
            {
                this.ShowMsg("请选择地区", false);
                return;
            }
            StoreInfo.County = this.ddlReggion.GetSelectedRegionId().Value;
            StoreInfo.Address = this.txtAddress.Text.Trim();
            StoreInfo.Phone = this.txtPhone.Text.Trim();
            StoreInfo.Mobile = this.txtMobile.Text.Trim();
            StoreInfo.Description = this.txtDescription.Text.Trim();

  

            if (string.IsNullOrEmpty(StoreInfo.Phone) && string.IsNullOrEmpty(StoreInfo.Mobile))
            {
                this.ShowMsg("手机号码和电话号码必填其一", false);
                return;
            }
            int issucess = StoreManagementHelper.AddStore(StoreInfo);
            if (issucess > 0)
            {
                this.txtSupplierName.Text = "";
                this.txtAddress.Text = "";
                this.txtPhone.Text = "";
                this.txtMobile.Text = "";
                this.txtDescription.Text = "";

                this.ShowMsg("添加成功", true);
                base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/StoreManagement.aspx"), true);
                return;
            }
            this.ShowMsg("添加失败", false);
        }
    }
}

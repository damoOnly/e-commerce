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
    [PrivilegeCheck(Privilege.StoreEdit)]
    public class EditStore : AdminPage
    {
        private int SotreId;
        protected System.Web.UI.WebControls.TextBox txtSupplierName;
        protected System.Web.UI.WebControls.TextBox txtPhone;
        protected System.Web.UI.WebControls.TextBox txtMobile;
        protected System.Web.UI.WebControls.TextBox txtAddress;
        protected System.Web.UI.WebControls.TextBox txtDescription;
        protected System.Web.UI.WebControls.Button btnSave;
        protected RegionSelector ddlReggion;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["StoreId"], out this.SotreId))
            {
                base.GotoResourceNotFound();
                return;
            }
            this.btnSave.Click += new System.EventHandler(this.btnEditShipper_Click);
            if (!this.Page.IsPostBack)
            {
                StoreManagementInfo store = StoreManagementHelper.GetStore(this.SotreId);
                if (store == null)
                {
                    base.GotoResourceNotFound();
                    return;
                }
                Globals.EntityCoding(store, false);
                this.txtSupplierName.Text = store.StoreName;
                this.ddlReggion.SetSelectedRegionId(new int?(store.County));
                this.txtAddress.Text = store.Address;
                this.txtPhone.Text = store.Phone;
                this.txtMobile.Text = store.Mobile;
                this.txtAddress.Text = store.Address;
                this.txtDescription.Text = store.Description;
            }
        }
        private void btnEditShipper_Click(object sender, System.EventArgs e)
        {
            StoreManagementInfo StoreInfo = new StoreManagementInfo();
            StoreInfo.StoreId = SotreId;
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
            if (StoreManagementHelper.UpdateStore(StoreInfo))
            {
                this.txtSupplierName.Text = "";
                this.txtAddress.Text = "";
                this.txtPhone.Text = "";
                this.txtMobile.Text = "";
                this.txtDescription.Text = "";

                this.ShowMsg("修改成功", true);
                base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/StoreManagement.aspx"), true);
                return;
            }
            this.ShowMsg("修改失败", false);
        }

    }
}

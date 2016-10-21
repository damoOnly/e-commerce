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
    [PrivilegeCheck(Privilege.EditMySupplierInfo)]
    public class EditMySupplierInfo : AdminPage
    {
        private int supplierId;
        protected System.Web.UI.WebControls.TextBox txtSupplierName;
        protected System.Web.UI.WebControls.TextBox txtSupplierCode;
        protected System.Web.UI.WebControls.TextBox txtWarehouseName;
        protected System.Web.UI.WebControls.TextBox txtPhone;
        protected System.Web.UI.WebControls.TextBox txtMobile;
        protected System.Web.UI.WebControls.TextBox txtAddress;
        protected TrimTextBox txtDescription;
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.CheckBox ckbApproveKey;
        protected RegionSelector ddlReggion;

        protected System.Web.UI.HtmlControls.HtmlInputHidden fmSrc1;

        protected System.Web.UI.HtmlControls.HtmlInputHidden fmSrc2;


        protected System.Web.UI.HtmlControls.HtmlInputHidden fmSrc3;

        protected System.Web.UI.WebControls.TextBox txtSupplierOwnerName;

        protected System.Web.UI.HtmlControls.HtmlImage littlepic1;

        protected System.Web.UI.HtmlControls.HtmlImage littlepic2;

        protected System.Web.UI.HtmlControls.HtmlImage littlepic3;
        protected System.Web.UI.WebControls.TextBox txtShopName;



        protected void Page_Load(object sender, System.EventArgs e)
        {
            supplierId = CheckSupplierRole();

            if(supplierId<=0)
            {
                base.GotoResourceNotFound();
                return;
            }


            this.btnSave.Click += new System.EventHandler(this.btnEditShipper_Click);
            if (!this.Page.IsPostBack)
            {
                SupplierInfo shipper = SupplierHelper.GetSupplier(this.supplierId);
                if (shipper == null)
                {
                    base.GotoResourceNotFound();
                    return;
                }
                Globals.EntityCoding(shipper, false);
                this.txtSupplierName.Text = shipper.SupplierName;
                this.txtShopName.Text = shipper.ShopName;
                this.txtSupplierCode.Text = shipper.SupplierCode;
                this.txtWarehouseName.Text = shipper.ShipWarehouseName;
                this.ddlReggion.SetSelectedRegionId(new int?(shipper.County));
                this.txtAddress.Text = shipper.Address;
                this.txtPhone.Text = shipper.Phone;
                this.txtMobile.Text = shipper.Mobile;
                this.txtAddress.Text = shipper.Address;
                this.txtDescription.Text = shipper.Description;
                this.ckbApproveKey.Checked = shipper.ApproveKey;
                this.ckbApproveKey.Enabled = false;
                this.littlepic1.Src = shipper.Logo;
                this.fmSrc1.Value = shipper.Logo;

                this.littlepic2.Src = shipper.PCImage;
                this.fmSrc2.Value = shipper.PCImage;

                this.littlepic3.Src = shipper.MobileImage;
                this.fmSrc3.Value = shipper.MobileImage;

                this.txtSupplierOwnerName.Text = shipper.ShopOwner;
            }
        }
        private void btnEditShipper_Click(object sender, System.EventArgs e)
        {
            SupplierInfo supplierInfo = new SupplierInfo();
            supplierInfo.SupplierId = supplierId;
            supplierInfo.SupplierName = this.txtSupplierName.Text.Trim();
            supplierInfo.SupplierCode = this.txtSupplierCode.Text.Trim();
            supplierInfo.ShipWarehouseName = this.txtWarehouseName.Text.Trim();
            if (!this.ddlReggion.GetSelectedRegionId().HasValue)
            {
                this.ShowMsg("请选择地区", false);
                return;
            }
            supplierInfo.County = this.ddlReggion.GetSelectedRegionId().Value;
            supplierInfo.Address = this.txtAddress.Text.Trim();
            supplierInfo.Phone = this.txtPhone.Text.Trim();
            supplierInfo.Mobile = this.txtMobile.Text.Trim();
            supplierInfo.Description = this.txtDescription.Text.Trim();
            supplierInfo.ShopName = this.txtShopName.Text.Trim();
            supplierInfo.ApproveKey = this.ckbApproveKey.Checked;            
            supplierInfo.ShopOwner = this.txtSupplierOwnerName.Text.Trim();
            supplierInfo.Logo = this.fmSrc1.Value;
            supplierInfo.PCImage = this.fmSrc2.Value;
            supplierInfo.MobileImage = this.fmSrc3.Value;
            this.littlepic1.Src = supplierInfo.Logo;
            this.littlepic2.Src = supplierInfo.PCImage;
            this.littlepic3.Src = supplierInfo.MobileImage;


            if (string.IsNullOrEmpty(supplierInfo.Phone) && string.IsNullOrEmpty(supplierInfo.Mobile))
            {
                this.ShowMsg("手机号码和电话号码必填其一", false);
                return;
            }
            if (SupplierHelper.UpdateSupplier(supplierInfo))
            {
                this.ShowMsg("修改成功", true);
                return;
            }
            this.ShowMsg("修改失败", false);
        }

    }
}


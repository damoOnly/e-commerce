using ASPNET.WebControls;
using Commodities;
using EcShop;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SupplierAdd)]
    public class AddSupplier : AdminPage
    {
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
        protected System.Web.UI.WebControls.TextBox txtShopName;


        protected System.Web.UI.WebControls.TextBox txtContact;
        protected System.Web.UI.WebControls.TextBox txtEmail;
        protected System.Web.UI.WebControls.TextBox txtFax;
        protected System.Web.UI.WebControls.TextBox txtCategory;
        protected System.Web.UI.WebControls.TextBox txtBeneficiaryName;
        protected System.Web.UI.WebControls.TextBox txtSwiftCode;
        protected System.Web.UI.WebControls.TextBox txtBankAccount;
        protected System.Web.UI.WebControls.TextBox txtBankName;
        protected System.Web.UI.WebControls.TextBox txtBankAddress;
        protected System.Web.UI.WebControls.TextBox txtIBAN;
        protected TrimTextBox TtxtRemark;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            SupplierInfo supplierInfo = new SupplierInfo();
            supplierInfo.SupplierName = this.txtSupplierName.Text.Trim();
            supplierInfo.ShopName = this.txtShopName.Text.Trim();
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
            supplierInfo.ApproveKey = this.ckbApproveKey.Checked;

            supplierInfo.ShopOwner = this.txtSupplierOwnerName.Text.Trim();
            supplierInfo.Logo = this.fmSrc1.Value;
            supplierInfo.PCImage = this.fmSrc2.Value;
            supplierInfo.MobileImage = this.fmSrc3.Value;

            supplierInfo.Contact = txtContact.Text.Trim();
            supplierInfo.Email = txtEmail.Text.Trim();
            supplierInfo.Fax = txtFax.Text.Trim();
            supplierInfo.Category = txtCategory.Text.Trim();
            supplierInfo.BeneficiaryName = txtBeneficiaryName.Text.Trim();
            supplierInfo.SwiftCode = txtSwiftCode.Text.Trim();
            supplierInfo.BankAccount = txtBankAccount.Text.Trim();
            supplierInfo.BankName = txtBankName.Text.Trim();
            supplierInfo.BankAddress = txtBankAddress.Text.Trim();
            supplierInfo.IBAN = txtIBAN.Text.Trim();
            supplierInfo.Remark = TtxtRemark.Text.Trim();

            if (string.IsNullOrEmpty(supplierInfo.Phone) && string.IsNullOrEmpty(supplierInfo.Mobile))
            {
                this.ShowMsg("手机号码和电话号码必填其一", false);
                return;
            }
            int issucess = SupplierHelper.AddSupplier(supplierInfo);
            if (issucess > 0)
            {
                this.txtSupplierName.Text = "";
                this.txtSupplierCode.Text = "";
                this.txtWarehouseName.Text = "";
                this.txtAddress.Text = "";
                this.txtPhone.Text = "";
                this.txtMobile.Text = "";
                this.txtDescription.Text = "";
                this.txtShopName.Text = "";
                this.txtSupplierOwnerName.Text = "";

                this.txtContact.Text = "";
                this.txtEmail.Text = "";
                this.txtFax.Text = "";
                this.txtCategory.Text = "";
                this.txtBeneficiaryName.Text = "";
                this.txtSwiftCode.Text = "";
                this.txtBankAccount.Text = "";
                this.txtBankName.Text = "";
                this.txtBankAddress.Text = "";
                this.txtIBAN.Text = "";
                this.TtxtRemark.Text = "";

                this.ShowMsg("添加成功", true);
                return;
            }
            this.ShowMsg("添加失败", false);
        }
    }
}

using Commodities;
using EcShop.ControlPanel.Commodities;
using EcShop.Entities;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.Tags
{
    public class Common_SupplierHeader : AscxTemplatedWebControl
    {
        int supplierId;

        private System.Web.UI.WebControls.Literal litStoreName;

        private System.Web.UI.WebControls.Literal litStoreOwnerName;

        private System.Web.UI.WebControls.Literal litStoreAddress;

        private System.Web.UI.WebControls.Image SupplierADImg;


        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Skin-Common_Header.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["supplierId"], out supplierId))
            {
                
                return;
            }

            SupplierInfo shipper = SupplierHelper.GetSupplier(this.supplierId);
            if (shipper == null)
            {
                return;
            }

            this.litStoreName = (System.Web.UI.WebControls.Literal)this.FindControl("litStoreName");

            this.litStoreOwnerName = (System.Web.UI.WebControls.Literal)this.FindControl("litStoreOwnerName");

            this.litStoreAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litStoreAddress");

            this.SupplierADImg = (System.Web.UI.WebControls.Image)this.FindControl("SupplierADImg");

            this.litStoreName.Text = string.IsNullOrWhiteSpace(shipper.ShopName) ? shipper.SupplierName : shipper.ShopName; //shipper.SupplierName;

            this.litStoreOwnerName.Text = shipper.ShopOwner;

            this.litStoreAddress.Text = RegionHelper.GetFullRegion(shipper.County, "");

            if(!string.IsNullOrWhiteSpace(shipper.PCImage))
            {
                this.SupplierADImg.ImageUrl = shipper.PCImage;
            }


        }
    }
}


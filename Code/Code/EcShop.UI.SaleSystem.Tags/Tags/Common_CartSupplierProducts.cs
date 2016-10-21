using EcShop.Entities.VShop;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
    public class Common_CartSupplierProducts : VshopTemplatedSupplierWebControl
    {
        public delegate void DataBindEventHandler(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e);
        public delegate void CommandEventHandler(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e);
        public const string TagID = "Common_CartSupplierProducts";
        private System.Web.UI.WebControls.Repeater listOrders;
        private System.Web.UI.WebControls.Panel panl_nodata;
        public event Common_CartSupplierProducts.DataBindEventHandler ItemDataBound;
        public event Common_CartSupplierProducts.CommandEventHandler ItemCommand;
        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }

        public object DataSource
        {
            get
            {
                return this.listOrders.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.listOrders.DataSource = value;
            }
        }

        public Common_CartSupplierProducts()
        {
            base.ID = "Common_CartSupplierProducts";
        }
     
        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "tags/Skin-Common_CartSupplierProducts.ascx";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.listOrders = (System.Web.UI.WebControls.Repeater)this.FindControl("repeaterSupplier");
            this.panl_nodata = (System.Web.UI.WebControls.Panel)this.FindControl("panl_nodata");
            this.listOrders.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.listOrders_ItemDataBound);
            this.listOrders.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.listOrders_RowCommand);
        }
        private void listOrders_RowCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            this.ItemCommand(sender, e);
        }
        private void listOrders_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            this.ItemDataBound(sender, e);
        }
        public override void DataBind()
        {
            this.EnsureChildControls();
            this.listOrders.DataSource = this.DataSource;
            this.listOrders.DataBind();
            if (this.listOrders.Items.Count <= 0)
            {
                this.panl_nodata.Visible = true;
            }
        }
    }
}

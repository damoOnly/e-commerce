using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    //[PrivilegeCheck(Privilege.AddProducts)]
	public class AddProductComplete : AdminPage
	{
		private int categoryId;
		private int productId;
		private int isEdit;
        private bool approve;
        private bool supplier;
		protected System.Web.UI.WebControls.Literal txtAction1;
		protected System.Web.UI.WebControls.Literal txtAction2;
		protected System.Web.UI.WebControls.Literal txtAction;
		protected System.Web.UI.WebControls.HyperLink hlinkProductDetails;
		protected System.Web.UI.WebControls.HyperLink hlinkProductEdit;
		protected System.Web.UI.WebControls.HyperLink hlinkAddProduct;
        protected System.Web.UI.WebControls.HyperLink hlinkSelectCategory;
        protected System.Web.UI.WebControls.HyperLink hlinkProductOnSales;
        protected Literal litDetail;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!int.TryParse(base.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound();
				return;
			}
			int.TryParse(base.Request.QueryString["isEdit"], out this.isEdit);
            bool.TryParse(base.Request.QueryString["approve"], out this.approve);
            bool.TryParse(base.Request.QueryString["supplier"], out this.supplier);
			if (!this.Page.IsPostBack)
			{
				if (this.isEdit == 1)
				{
					this.txtAction.Text = (this.txtAction1.Text = (this.txtAction2.Text = "编辑"));
				}
				else
				{
					this.txtAction.Text = (this.txtAction1.Text = (this.txtAction2.Text = "添加"));
				}
                if (approve)
                {
                    litDetail.Text = "<a id='ctl00_contentHolder_hlinkProductDetails' href='/product_detail-" + this.productId + ".aspx' target='_blank'>查看</a>商品&nbsp;&nbsp;或者&nbsp;&nbsp;";
                }
                else
                {
                //    this.hlinkProductDetails.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
                //{
                //    this.productId
                //});
                }
                if (supplier)
                {
                    this.hlinkAddProduct.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/product/SupplierAddProduct.aspx?categoryId={0}", this.categoryId));
                    this.hlinkProductEdit.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/product/SupplierEditProduct.aspx?productId={0}", this.productId));
                    this.hlinkSelectCategory.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/product/SupplierSelectCategory.aspx"));
                    this.hlinkProductOnSales.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/product/SupplierProductOnSales.aspx"));
                }
                else
                {
                    this.hlinkAddProduct.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/product/AddProduct.aspx?categoryId={0}", this.categoryId));
                    this.hlinkProductEdit.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/product/EditProduct.aspx?productId={0}", this.productId));
                    this.hlinkSelectCategory.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/product/SelectCategory.aspx"));
                    this.hlinkProductOnSales.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/product/ProductOnSales.aspx"));
                }
			}
		}
	}
}

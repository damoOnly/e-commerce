using ASPNET.WebControls;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EcShop.UI.Web.Admin.product
{
	
    [PrivilegeCheck(Privilege.SubjectProducts)]
    public class SuggestProducts : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdtopic;
		protected ImageLinkButton btnDeleteAll;
		protected System.Web.UI.WebControls.LinkButton btnFinish;
        protected Grid grdSuggestProducts;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            this.grdSuggestProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdSuggestProducts_RowDeleting);
			if (!this.Page.IsPostBack)
			{
                this.BindSuggestProducts();
			}
		}
        private void grdSuggestProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
            if (ProductHelper.RemoveSuggestProduct((int)this.grdSuggestProducts.DataKeys[e.RowIndex].Value, ClientType.PC))
			{
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}
        private void BindSuggestProducts()
		{
            this.grdSuggestProducts.DataSource = ProductHelper.GeSuggestProducts(ClientType.PC);
            this.grdSuggestProducts.DataBind();
		}
		private void btnDeleteAll_Click(object sender, System.EventArgs e)
		{
            if (ProductHelper.RemoveAllSuggestProduct(ClientType.PC))
			{
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}
		protected void btnFinish_Click(object sender, System.EventArgs e)
		{
            foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdSuggestProducts.Rows)
			{
				int displaysequence = 0;
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)gridViewRow.FindControl("txtSequence");
				if (int.TryParse(textBox.Text.Trim(), out displaysequence))
				{
                    int productId = System.Convert.ToInt32(this.grdSuggestProducts.DataKeys[gridViewRow.DataItemIndex].Value);
                    ProductHelper.UpdateSuggestProductSequence(ClientType.PC, productId, displaysequence);
				}
			}
            this.BindSuggestProducts();
		}
	}
}

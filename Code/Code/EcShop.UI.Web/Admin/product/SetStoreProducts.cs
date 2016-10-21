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
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.StoreRelatedProduct)]
    public class SetStoreProducts : AdminPage
	{
		private int StoreId;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdtopic;
		protected System.Web.UI.WebControls.Literal litPromotionName;
		protected ImageLinkButton btnDeleteAll;
		protected Grid grdTopicProducts;
		protected System.Web.UI.WebControls.LinkButton btnFinish;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!int.TryParse(this.Page.Request.QueryString["StoreId"], out this.StoreId))
			{
				base.GotoResourceNotFound();
				return;
			}
            this.hdtopic.Value = this.StoreId.ToString();
			this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
			this.grdTopicProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdTopicProducts_RowDeleting);
			if (!this.Page.IsPostBack)
			{
                StoreManagementInfo store = StoreManagementHelper.GetStore(this.StoreId);
                if (store == null)
				{
					base.GotoResourceNotFound();
					return;
				}
                this.litPromotionName.Text = store.StoreName;
				this.BindStoreProducts();
			}
		}
		private void grdTopicProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
            if (StoreManagementHelper.RemoveReleatesProductByStore(this.StoreId, (int)this.grdTopicProducts.DataKeys[e.RowIndex].Value))
			{
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}
        private void BindStoreProducts()
		{
            this.grdTopicProducts.DataSource = StoreManagementHelper.GetRelatedStoreProducts(this.StoreId);
			this.grdTopicProducts.DataBind();
		}
		private void btnDeleteAll_Click(object sender, System.EventArgs e)
		{
            if (StoreManagementHelper.RemoveReleatesProductByStore(this.StoreId))
			{
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}
		protected void btnFinish_Click(object sender, System.EventArgs e)
		{
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdTopicProducts.Rows)
			{
				int displaysequence = 0;
                string QRcode = string.Empty;
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)gridViewRow.FindControl("txtSequence");
				if (int.TryParse(textBox.Text.Trim(), out displaysequence))
				{
					int relatedProductId = System.Convert.ToInt32(this.grdTopicProducts.DataKeys[gridViewRow.DataItemIndex].Value);
                    StoreManagementHelper.UpdateRelateProductSequenceByStore(this.StoreId, relatedProductId, displaysequence);
				}
			}
            this.BindStoreProducts();
		}
	}
}

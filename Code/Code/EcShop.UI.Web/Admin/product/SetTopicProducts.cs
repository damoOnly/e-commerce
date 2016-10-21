using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Entities.VShop;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.product
{
	public class SetTopicProducts : AdminPage
	{
		private int topicId;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdtopic;
		protected System.Web.UI.WebControls.Literal litPromotionName;
		protected ImageLinkButton btnDeleteAll;
		protected Grid grdTopicProducts;
		protected System.Web.UI.WebControls.LinkButton btnFinish;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["topicid"], out this.topicId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.hdtopic.Value = this.topicId.ToString();
			this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
			this.grdTopicProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdTopicProducts_RowDeleting);
			if (!this.Page.IsPostBack)
			{
				TopicInfo topicInfo = VShopHelper.Gettopic(this.topicId);
				if (topicInfo == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litPromotionName.Text = topicInfo.Title;
				this.BindTopicProducts();
			}
		}
		private void grdTopicProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (VShopHelper.RemoveReleatesProductBytopicid(this.topicId, (int)this.grdTopicProducts.DataKeys[e.RowIndex].Value))
			{
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}
		private void BindTopicProducts()
		{
			this.grdTopicProducts.DataSource = VShopHelper.GetRelatedTopicProducts(this.topicId,false);
			this.grdTopicProducts.DataBind();
		}
		private void btnDeleteAll_Click(object sender, System.EventArgs e)
		{
			if (VShopHelper.RemoveReleatesProductBytopicid(this.topicId))
			{
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}
		protected void btnFinish_Click(object sender, System.EventArgs e)
		{
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdTopicProducts.Rows)
			{
				int displaysequence = 0;
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)gridViewRow.FindControl("txtSequence");
				if (int.TryParse(textBox.Text.Trim(), out displaysequence))
				{
					int relatedProductId = System.Convert.ToInt32(this.grdTopicProducts.DataKeys[gridViewRow.DataItemIndex].Value);
					VShopHelper.UpdateRelateProductSequence(this.topicId, relatedProductId, displaysequence);
				}
			}
			this.BindTopicProducts();
		}
	}
}

using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Entities.Comments;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class ProductConsultationsAndReplay : HtmlTemplatedWebControl
	{
		private int productId;
		private Pager pager;
		private ThemedTemplatedRepeater rptRecords;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ProductConsultationsAndReplay.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound();
			}
			this.rptRecords = (ThemedTemplatedRepeater)this.FindControl("rptRecords");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				PageTitle.AddSiteNameTitle("商品咨询");
				this.BindData();
			}
		}
		private void BindData()
		{
			ProductConsultationAndReplyQuery productConsultationAndReplyQuery = new ProductConsultationAndReplyQuery();
			productConsultationAndReplyQuery.PageIndex = this.pager.PageIndex;
			productConsultationAndReplyQuery.PageSize = productConsultationAndReplyQuery.PageSize;
			productConsultationAndReplyQuery.ProductId = this.productId;
			productConsultationAndReplyQuery.HasReplied = new bool?(true);
			DbQueryResult productConsultations = ProductBrowser.GetProductConsultations(productConsultationAndReplyQuery);
			this.rptRecords.DataSource = productConsultations.Data;
			this.rptRecords.DataBind();
			this.pager.TotalRecords = productConsultations.TotalRecords;
		}
	}
}

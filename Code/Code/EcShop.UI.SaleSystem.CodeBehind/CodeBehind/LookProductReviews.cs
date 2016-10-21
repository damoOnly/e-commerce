using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Entities.Comments;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class LookProductReviews : HtmlTemplatedWebControl
	{
		private int productId;
		private ThemedTemplatedRepeater rptRecords;
		private Pager pager;
        private Literal noReviews;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-LookProductReviews.html";
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
            this.noReviews = (System.Web.UI.WebControls.Literal)this.FindControl("noReviews");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				PageTitle.AddSiteNameTitle("商品评论");
				this.BindData();
			}
		}
		private void ReBind()
		{
			base.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{

				{
					"pageIndex",
					this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture)
				}
			});
		}
		private void BindData()
		{
			DbQueryResult productReviews = ProductBrowser.GetProductReviews(new ProductReviewQuery
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				productId = this.productId
			});
			this.rptRecords.DataSource = productReviews.Data;
			this.rptRecords.DataBind();
			this.pager.TotalRecords = productReviews.TotalRecords;
            if (string.IsNullOrWhiteSpace(productReviews.Data.ToString()))
            {
                this.noReviews.Text = "暂时没有评论信息";
            }
		}
	}
}

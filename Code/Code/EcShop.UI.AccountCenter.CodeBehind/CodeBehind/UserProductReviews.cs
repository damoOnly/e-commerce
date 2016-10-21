using ASPNET.WebControls;
using EcShop.Entities.Comments;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UserProductReviews : MemberTemplatedWebControl
	{
		private ThemedTemplatedList dlstPts;
		private Pager pager;
		private System.Web.UI.WebControls.Literal litReviewCount;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserProductReviews.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.dlstPts = (ThemedTemplatedList)this.FindControl("dlstPts");
			this.pager = (Pager)this.FindControl("pager");
			this.litReviewCount = (System.Web.UI.WebControls.Literal)this.FindControl("litReviewCount");
			PageTitle.AddSiteNameTitle("我参与的评论");
			if (!this.Page.IsPostBack)
			{
				if (this.litReviewCount != null)
				{
					this.litReviewCount.Text = ProductBrowser.GetUserProductReviewsCount().ToString();
				}
				this.BindPtAndReviewsAndReplys();
			}
		}
		private void BindPtAndReviewsAndReplys()
		{
			UserProductReviewAndReplyQuery userProductReviewAndReplyQuery = new UserProductReviewAndReplyQuery();
			userProductReviewAndReplyQuery.PageIndex = this.pager.PageIndex;
			userProductReviewAndReplyQuery.PageSize = this.pager.PageSize;
			int totalRecords = 0;
			DataSet userProductReviewsAndReplys = ProductBrowser.GetUserProductReviewsAndReplys(userProductReviewAndReplyQuery, out totalRecords);
			this.dlstPts.DataSource = userProductReviewsAndReplys.Tables[0].DefaultView;
			this.dlstPts.DataBind();
			this.pager.TotalRecords = totalRecords;
		}
	}
}

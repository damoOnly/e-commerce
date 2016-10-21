using EcShop.Entities.Comments;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class AliOHArticleCategory : AliOHTemplatedWebControl
	{
		private AliOHTemplatedRepeater rptArticles;
		private System.Web.UI.WebControls.Repeater categroy;
		private int categoryId;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ArticleCategory.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.rptArticles = (AliOHTemplatedRepeater)this.FindControl("rpt_ArticleCategory");
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
				{
					int.TryParse(this.Page.Request.QueryString["CategoryId"], out this.categoryId);
					ArticleCategoryInfo articleCategory = CommentBrowser.GetArticleCategory(this.categoryId);
					if (articleCategory != null)
					{
						PageTitle.AddSiteNameTitle(articleCategory.Name);
					}
				}
				else
				{
					PageTitle.AddSiteNameTitle("文章中心");
				}
				this.rptArticles.DataSource = this.GetDataSource();
				this.rptArticles.DataBind();
			}
		}
		protected void Category_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				int num = (int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "cateGoryId");
				System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rep_article");
				System.Collections.Generic.IList<ArticleInfo> articleList = CommentBrowser.GetArticleList(num, 1000);
				if (articleList != null && articleList.Count > 0 && repeater != null)
				{
					repeater.DataSource = articleList;
					repeater.DataBind();
				}
			}
		}
		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);
		}
		private System.Collections.Generic.IList<ArticleCategoryInfo> GetDataSource()
		{
			return CommentBrowser.GetArticleMainCategories();
		}
	}
}

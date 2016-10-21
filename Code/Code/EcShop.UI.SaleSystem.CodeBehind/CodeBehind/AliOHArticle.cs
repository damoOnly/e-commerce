using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Comments;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class AliOHArticle : AliOHTemplatedWebControl
	{
		private AliOHTemplatedRepeater rptArticles;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotalPages;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtCategoryName;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtCategoryId;
		private string keyWord;
		private int categoryId;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Articles.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			this.txtCategoryName = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtCategoryName");
			this.txtCategoryId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtCategoryId");
			this.keyWord = this.Page.Request.QueryString["keyWord"];
			if (!string.IsNullOrWhiteSpace(this.keyWord))
			{
				this.keyWord = this.keyWord.Trim();
			}
			this.rptArticles = (AliOHTemplatedRepeater)this.FindControl("rptArticles");
			this.txtTotalPages = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			int pageIndex;
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
			{
				pageIndex = 1;
			}
			int pageSize;
			if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
			{
				pageSize = 20;
			}
			ArticleQuery articleQuery = new ArticleQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out value))
				{
					ArticleCategoryInfo articleCategory = CommentBrowser.GetArticleCategory(value);
					if (articleCategory != null)
					{
						PageTitle.AddSiteNameTitle(articleCategory.Name);
						articleQuery.CategoryId = new int?(value);
						this.txtCategoryId.Value = value.ToString();
						this.txtCategoryName.Value = articleCategory.Name;
					}
					else
					{
						PageTitle.AddSiteNameTitle("文章分类搜索页");
					}
				}
			}
			articleQuery.Keywords = this.keyWord;
			articleQuery.PageIndex = pageIndex;
			articleQuery.PageSize = pageSize;
			articleQuery.SortBy = "AddedDate";
			articleQuery.SortOrder = SortAction.Desc;
			DbQueryResult articleList = CommentBrowser.GetArticleList(articleQuery);
			this.rptArticles.DataSource = articleList.Data;
			this.rptArticles.DataBind();
			int totalRecords = articleList.TotalRecords;
			this.txtTotalPages.SetWhenIsNotNull(totalRecords.ToString());
		}
		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);
		}
	}
}

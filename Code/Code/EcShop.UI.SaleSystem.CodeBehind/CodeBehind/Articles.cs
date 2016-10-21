using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Comments;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class Articles : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptArticles;
		private Pager pager;
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
			this.rptArticles = (ThemedTemplatedRepeater)this.FindControl("rptArticles");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
                /*if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
                {
                    int categoryId = 0;
                    int.TryParse(this.Page.Request.QueryString["CategoryId"], out categoryId);
                    ArticleCategoryInfo articleCategory = CommentBrowser.GetArticleCategory(categoryId);
                    if (articleCategory != null)
                    {
                        PageTitle.AddSiteNameTitle(articleCategory.Name);
                    }
                }
                else
                {
                    PageTitle.AddSiteNameTitle("文章中心");
                }*/
                PageTitle.AddSiteNameTitle("商城公告");
				this.BindList();
			}
		}
		private void BindList()
		{
            AfficheQuery articleQuery = new AfficheQuery();
            /*if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
            {
                int value = 0;
                if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out value))
                {
                    articleQuery.CategoryId = new int?(value);
                }
            }//CommentBrowser.GetArticleList(articleQuery);*/
			articleQuery.PageIndex = this.pager.PageIndex;
			articleQuery.PageSize = this.pager.PageSize;
			articleQuery.SortBy = "AddedDate";
			articleQuery.SortOrder = SortAction.Desc;
            DbQueryResult articleList = CommentBrowser.GetAfficheList(articleQuery);
                
			this.rptArticles.DataSource = articleList.Data;
			this.rptArticles.DataBind();
			this.pager.TotalRecords = articleList.TotalRecords;
		}
	}
}

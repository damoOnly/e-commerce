using EcShop.Core;
using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using EcShop.UI.SaleSystem.Tags;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WapArticleDetail : WAPTemplatedWebControl
	{
		private int articleId;
		private System.Web.UI.HtmlControls.HtmlInputHidden txtCatgoryId;
		private System.Web.UI.WebControls.Literal litArticleTitle;
		private System.Web.UI.WebControls.Literal litArticleDescription;
		private System.Web.UI.WebControls.Literal litArticleContent;
		private FormatedTimeLabel litArticleAddedDate;
		private System.Web.UI.WebControls.Label lblFront;
		private System.Web.UI.WebControls.Label lblNext;
		private System.Web.UI.WebControls.Label lblFrontTitle;
		private System.Web.UI.WebControls.Label lblNextTitle;
		private System.Web.UI.HtmlControls.HtmlAnchor aFront;
		private System.Web.UI.HtmlControls.HtmlAnchor aNext;
		private Common_ArticleRelative ariticlative;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ArticleDetail.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["articleId"], out this.articleId))
			{
				base.GotoResourceNotFound("");
			}
			this.txtCatgoryId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtCatgoryId");
			this.litArticleAddedDate = (FormatedTimeLabel)this.FindControl("litArticleAddedDate");
			this.litArticleContent = (System.Web.UI.WebControls.Literal)this.FindControl("litArticleContent");
			this.litArticleDescription = (System.Web.UI.WebControls.Literal)this.FindControl("litArticleDescription");
			this.litArticleTitle = (System.Web.UI.WebControls.Literal)this.FindControl("litArticleTitle");
			this.lblFront = (System.Web.UI.WebControls.Label)this.FindControl("lblFront");
			this.lblNext = (System.Web.UI.WebControls.Label)this.FindControl("lblNext");
			this.lblFrontTitle = (System.Web.UI.WebControls.Label)this.FindControl("lblFrontTitle");
			this.lblNextTitle = (System.Web.UI.WebControls.Label)this.FindControl("lblNextTitle");
			this.aFront = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("front");
			this.aNext = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("next");
			this.ariticlative = (Common_ArticleRelative)this.FindControl("list_Common_ArticleRelative");
			if (!this.Page.IsPostBack)
			{
				ArticleInfo article = CommentBrowser.GetArticle(this.articleId);
				if (article != null && article.IsRelease)
				{
					if (this.txtCatgoryId != null)
					{
						this.txtCatgoryId.Value = article.CategoryId.ToString();
					}
					PageTitle.AddSiteNameTitle(article.Title);
					if (!string.IsNullOrEmpty(article.MetaKeywords))
					{
						MetaTags.AddMetaKeywords(article.MetaKeywords, HiContext.Current.Context);
					}
					if (!string.IsNullOrEmpty(article.MetaDescription))
					{
						MetaTags.AddMetaDescription(article.MetaDescription, HiContext.Current.Context);
					}
					this.litArticleTitle.Text = article.Title;
					this.litArticleDescription.Text = article.Description;
					string str = HiContext.Current.HostPath + Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails", new object[]
					{
						this.articleId
					});
					this.litArticleContent.Text = article.Content.Replace("href=\"#\"", "href=\"" + str + "\"");
					this.litArticleAddedDate.Time = article.AddedDate;
					ArticleInfo frontOrNextArticle = CommentBrowser.GetFrontOrNextArticle(this.articleId, "Front", article.CategoryId);
					if (frontOrNextArticle != null && frontOrNextArticle.ArticleId > 0)
					{
						if (this.lblFront != null)
						{
							this.lblFront.Visible = true;
							this.aFront.HRef = "/WapShop/ArticleDetails.aspx?ArticleId=" + frontOrNextArticle.ArticleId;
							this.lblFrontTitle.Text = frontOrNextArticle.Title;
						}
					}
					else
					{
						if (this.lblFront != null)
						{
							this.lblFront.Visible = false;
						}
					}
					ArticleInfo frontOrNextArticle2 = CommentBrowser.GetFrontOrNextArticle(this.articleId, "Next", article.CategoryId);
					if (frontOrNextArticle2 != null && frontOrNextArticle2.ArticleId > 0)
					{
						if (this.lblNext != null)
						{
							this.lblNext.Visible = true;
							this.aNext.HRef = "/WapShop/ArticleDetails.aspx?ArticleId=" + frontOrNextArticle2.ArticleId;
							this.lblNextTitle.Text = frontOrNextArticle2.Title;
							return;
						}
					}
					else
					{
						if (this.lblNext != null)
						{
							this.lblNext.Visible = false;
						}
					}
				}
			}
		}
	}
}

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using EcShop.SqlDal.Comments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
namespace EcShop.ControlPanel.Comments
{
	public static class ArticleHelper
	{
		public static ArticleCategoryInfo GetArticleCategory(int categoryId)
		{
			return new ArticleCategoryDao().GetArticleCategory(categoryId);
		}
		public static IList<ArticleCategoryInfo> GetMainArticleCategories()
		{
			return new ArticleCategoryDao().GetMainArticleCategories();
		}
		public static ArticleInfo GetArticle(int articleId)
		{
			return new ArticleDao().GetArticle(articleId);
		}
		public static DbQueryResult GetArticleList(ArticleQuery articleQuery)
		{
			return new ArticleDao().GetArticleList(articleQuery);
		}
		public static bool CreateArticleCategory(ArticleCategoryInfo articleCategory)
		{
			bool result;
			if (null == articleCategory)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(articleCategory, true);
				result = new ArticleCategoryDao().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Create);
			}
			return result;
		}
		public static bool UpdateArticleCategory(ArticleCategoryInfo articleCategory)
		{
			bool result;
			if (null == articleCategory)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(articleCategory, true);
				result = new ArticleCategoryDao().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Update);
			}
			return result;
		}
		public static bool DeleteArticleCategory(int categoryId)
		{
			ArticleCategoryInfo articleCategoryInfo = new ArticleCategoryInfo();
			articleCategoryInfo.CategoryId = categoryId;
			return new ArticleCategoryDao().CreateUpdateDeleteArticleCategory(articleCategoryInfo, DataProviderAction.Delete);
		}
		public static int DeleteArticles(IList<int> articles)
		{
			int result;
			if (articles == null || articles.Count == 0)
			{
				result = 0;
			}
			else
			{
				int num = 0;
				foreach (int current in articles)
				{
					new ArticleDao().DeleteArticle(current);
					num++;
				}
				result = num;
			}
			return result;
		}
		public static bool CreateArticle(ArticleInfo article)
		{
			bool result;
			if (null == article)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(article, true);
				result = new ArticleDao().AddArticle(article);
			}
			return result;
		}
		public static bool UpdateArticle(ArticleInfo article)
		{
			bool result;
			if (null == article)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(article, true);
				result = new ArticleDao().UpdateArticle(article);
			}
			return result;
		}
		public static bool DeleteArticle(int articleId)
		{
			return new ArticleDao().DeleteArticle(articleId);
		}
		public static void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
		{
			new ArticleCategoryDao().SwapArticleCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
		}
		public static DbQueryResult GetRelatedArticsProducts(Pagination page, int articId)
		{
			return new ArticleDao().GetRelatedArticsProducts(page, articId);
		}
		public static bool AddReleatesProdcutByArticId(int articId, int productId)
		{
			return new ArticleDao().AddReleatesProdcutByArticId(articId, productId);
		}
		public static bool RemoveReleatesProductByArticId(int articId, int productId)
		{
			return new ArticleDao().RemoveReleatesProductByArticId(articId, productId);
		}
		public static bool RemoveReleatesProductByArticId(int articId)
		{
			return new ArticleDao().RemoveReleatesProductByArticId(articId);
		}
		public static bool UpdateRelease(int articId, bool release)
		{
			return new ArticleDao().UpdateRelease(articId, release);
		}
		public static HelpCategoryInfo GetHelpCategory(int categoryId)
		{
			return new HelpCategoryDao().GetHelpCategory(categoryId);
		}
		public static IList<HelpCategoryInfo> GetHelpCategorys()
		{
			return new HelpCategoryDao().GetHelpCategorys();
		}
		public static DbQueryResult GetHelpList(HelpQuery helpQuery)
		{
			return new HelpDao().GetHelpList(helpQuery);
		}
		public static HelpInfo GetHelp(int helpId)
		{
			return new HelpDao().GetHelp(helpId);
		}
		public static bool CreateHelpCategory(HelpCategoryInfo helpCategory)
		{
			bool result;
			if (null == helpCategory)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(helpCategory, true);
				result = new HelpCategoryDao().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Create);
			}
			return result;
		}
		public static bool UpdateHelpCategory(HelpCategoryInfo helpCategory)
		{
			bool result;
			if (null == helpCategory)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(helpCategory, true);
				result = new HelpCategoryDao().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Update);
			}
			return result;
		}
		public static bool DeleteHelpCategory(int categoryId)
		{
			HelpCategoryInfo helpCategoryInfo = new HelpCategoryInfo();
			helpCategoryInfo.CategoryId = new int?(categoryId);
			return new HelpCategoryDao().CreateUpdateDeleteHelpCategory(helpCategoryInfo, DataProviderAction.Delete);
		}
		public static int DeleteHelpCategorys(List<int> categoryIds)
		{
			int result;
			if (categoryIds == null || categoryIds.Count == 0)
			{
				result = 0;
			}
			else
			{
				result = new HelpCategoryDao().DeleteHelpCategorys(categoryIds);
			}
			return result;
		}
		public static int DeleteHelps(IList<int> helps)
		{
			int result;
			if (helps == null || helps.Count == 0)
			{
				result = 0;
			}
			else
			{
				int num = 0;
				foreach (int current in helps)
				{
					if (new HelpDao().DeleteHelp(current))
					{
						num++;
					}
				}
				result = num;
			}
			return result;
		}
		public static bool CreateHelp(HelpInfo help)
		{
			bool result;
			if (null == help)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(help, true);
				result = new HelpDao().AddHelp(help);
			}
			return result;
		}
		public static bool UpdateHelp(HelpInfo help)
		{
			bool result;
			if (null == help)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(help, true);
				result = new HelpDao().UpdateHelp(help);
			}
			return result;
		}
		public static bool DeleteHelp(int helpId)
		{
			return new HelpDao().DeleteHelp(helpId);
		}
		public static void SwapHelpCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
		{
			new HelpCategoryDao().SwapHelpCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
		}
		public static string UploadArticleImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile))
			{
				result = string.Empty;
			}
			else
			{
				string text = HiContext.Current.GetStoragePath() + "/article/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
		public static string UploadHelpImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile))
			{
				result = string.Empty;
			}
			else
			{
				string text = HiContext.Current.GetStoragePath() + "/help/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
	}
}

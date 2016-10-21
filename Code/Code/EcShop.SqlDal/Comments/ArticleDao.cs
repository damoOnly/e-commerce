using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Comments;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Comments
{
	public class ArticleDao
	{
		private Database database;
		public ArticleDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public bool AddArticle(ArticleInfo article)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Articles(CategoryId, Title, Meta_Description, Meta_Keywords, IconUrl, Description, Content, AddedDate,IsRelease) VALUES (@CategoryId, @Title, @Meta_Description, @Meta_Keywords,  @IconUrl, @Description, @Content, @AddedDate,@IsRelease)");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, article.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, article.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, article.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "IconUrl", DbType.String, article.IconUrl);
			this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, article.Description);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, article.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "IsRelease", DbType.Boolean, article.IsRelease);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public bool UpdateArticle(ArticleInfo article)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Articles SET CategoryId = @CategoryId,AddedDate = @AddedDate,Title = @Title, Meta_Description = @Meta_Description, Meta_Keywords = @Meta_Keywords, IconUrl=@IconUrl,Description = @Description,Content = @Content,IsRelease=@IsRelease WHERE ArticleId = @ArticleId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, article.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, article.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, article.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "IconUrl", DbType.String, article.IconUrl);
			this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, article.Description);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
			this.database.AddInParameter(sqlStringCommand, "IsRelease", DbType.Boolean, article.IsRelease);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", DbType.DateTime, article.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, article.ArticleId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public bool DeleteArticle(int articleId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_Articles WHERE ArticleId = @ArticleId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public ArticleInfo GetArticle(int articleId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Articles WHERE ArticleId = @ArticleId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleId);
			ArticleInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateArticle(dataReader);
				}
			}
			return result;
		}
		public DbQueryResult GetArticleList(ArticleQuery articleQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Title LIKE '%{0}%'", DataHelper.CleanSearchString(articleQuery.Keywords));
			if (articleQuery.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND CategoryId = {0}", articleQuery.CategoryId.Value);
			}
			if (articleQuery.StartArticleTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >= '{0}'", articleQuery.StartArticleTime.Value);
			}
			if (articleQuery.EndArticleTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <= '{0}'", articleQuery.EndArticleTime.Value);
			}
			return DataHelper.PagingByRownumber(articleQuery.PageIndex, articleQuery.PageSize, articleQuery.SortBy, articleQuery.SortOrder, articleQuery.IsCount, "vw_Ecshop_Articles", "ArticleId", stringBuilder.ToString(), "*");
		}
		public DbQueryResult GetRelatedArticsProducts(Pagination page, int articId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SaleStatus = {0}", 1);
			stringBuilder.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Ecshop_RelatedArticsProducts WHERE ArticleId = {0})", articId);
			string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40,ThumbnailUrl160,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310 MarketPrice, SalePrice, Stock, DisplaySequence";
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Ecshop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}
		public bool AddReleatesProdcutByArticId(int articId, int prodcutId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_RelatedArticsProducts(ArticleId, RelatedProductId) VALUES (@ArticleId, @RelatedProductId)");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
			this.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, prodcutId);
			bool result;
			try
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public bool RemoveReleatesProductByArticId(int articId, int productId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_RelatedArticsProducts WHERE ArticleId = @ArticleId AND RelatedProductId = @RelatedProductId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
			this.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, productId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool RemoveReleatesProductByArticId(int articId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_RelatedArticsProducts WHERE ArticleId = @ArticleId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool UpdateRelease(int articId, bool release)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_Articles set IsRelease=@IsRelease  where ArticleId = @ArticleId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
			this.database.AddInParameter(sqlStringCommand, "IsRelease", DbType.Boolean, release);
			bool result;
			try
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public ArticleInfo GetFrontOrNextArticle(int articleId, string type, int categoryId)
		{
			string query = string.Empty;
			if (type == "Next")
			{
				query = "SELECT TOP 1 * FROM Ecshop_Articles WHERE ArticleId < @ArticleId AND CategoryId=@CategoryId AND IsRelease=1 ORDER BY ArticleId DESC";
			}
			else
			{
				query = "SELECT TOP 1 * FROM Ecshop_Articles WHERE  ArticleId > @ArticleId AND CategoryId=@CategoryId AND IsRelease=1 ORDER BY ArticleId ASC";
			}
			ArticleInfo result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleId);
			this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateArticle(dataReader);
				}
			}
			return result;
		}
		public IList<ArticleInfo> GetArticleList(int categoryId, int maxNum)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP {0} * FROM vw_Ecshop_Articles WHERE IsRelease=1", maxNum);
			if (categoryId != 0)
			{
				stringBuilder.AppendFormat(" AND CategoryId = {0}", categoryId);
			}
			stringBuilder.Append(" ORDER BY AddedDate DESC");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			IList<ArticleInfo> list = new List<ArticleInfo>();
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ArticleInfo item = DataMapper.PopulateArticle(dataReader);
					list.Add(item);
				}
			}
			return list;
		}
	}
}

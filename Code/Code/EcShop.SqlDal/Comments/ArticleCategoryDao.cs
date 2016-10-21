using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Comments;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Comments
{
	public class ArticleCategoryDao
	{
		private Database database;
		public ArticleCategoryDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Ecshop_ArticleCategories", "CategoryId", "DisplaySequence", categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
		}
		public bool CreateUpdateDeleteArticleCategory(ArticleCategoryInfo articleCategory, DataProviderAction action)
		{
			bool result;
			if (null == articleCategory)
			{
				result = false;
			}
			else
			{
				DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ArticleCategory_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
				if (action != DataProviderAction.Create)
				{
					this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, articleCategory.CategoryId);
				}
				if (action != DataProviderAction.Delete)
				{
					this.database.AddInParameter(storedProcCommand, "Name", DbType.String, articleCategory.Name);
					this.database.AddInParameter(storedProcCommand, "IconUrl", DbType.String, articleCategory.IconUrl);
					this.database.AddInParameter(storedProcCommand, "Description", DbType.String, articleCategory.Description);
				}
				this.database.ExecuteNonQuery(storedProcCommand);
				result = ((int)this.database.GetParameterValue(storedProcCommand, "Status") == 0);
			}
			return result;
		}
		public ArticleCategoryInfo GetArticleCategory(int categoryId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_ArticleCategories WHERE CategoryId = @CategoryId ORDER BY [DisplaySequence]");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			ArticleCategoryInfo result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateArticleCategory(dataReader);
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
		public IList<ArticleCategoryInfo> GetMainArticleCategories()
		{
			IList<ArticleCategoryInfo> list = new List<ArticleCategoryInfo>();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * From Ecshop_ArticleCategories ORDER BY [DisplaySequence]");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ArticleCategoryInfo item = DataMapper.PopulateArticleCategory(dataReader);
					list.Add(item);
				}
			}
			return list;
		}
	}
}

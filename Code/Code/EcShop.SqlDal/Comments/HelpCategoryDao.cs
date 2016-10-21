using EcShop.Core;
using EcShop.Core.Enums;
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
	public class HelpCategoryDao
	{
		private Database database;
		public HelpCategoryDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public bool CreateUpdateDeleteHelpCategory(HelpCategoryInfo helpCategory, DataProviderAction action)
		{
			bool result;
			if (null == helpCategory)
			{
				result = false;
			}
			else
			{
				DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_HelpCategory_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
				if (action != DataProviderAction.Create)
				{
					this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, helpCategory.CategoryId);
				}
				if (action != DataProviderAction.Delete)
				{
					this.database.AddInParameter(storedProcCommand, "Name", DbType.String, helpCategory.Name);
					this.database.AddInParameter(storedProcCommand, "IconUrl", DbType.String, helpCategory.IconUrl);
					this.database.AddInParameter(storedProcCommand, "IndexChar", DbType.String, helpCategory.IndexChar);
					this.database.AddInParameter(storedProcCommand, "Description", DbType.String, helpCategory.Description);
					this.database.AddInParameter(storedProcCommand, "IsShowFooter", DbType.Boolean, helpCategory.IsShowFooter);
				}
				this.database.ExecuteNonQuery(storedProcCommand);
				result = ((int)this.database.GetParameterValue(storedProcCommand, "Status") == 0);
			}
			return result;
		}
		public int DeleteHelpCategorys(List<int> categoryIds)
		{
			int result;
			if (null == categoryIds)
			{
				result = 0;
			}
			else
			{
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_HelpCategories WHERE CategoryId=@CategoryId");
				this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32);
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				foreach (int current in categoryIds)
				{
					this.database.SetParameterValue(sqlStringCommand, "CategoryId", current);
					this.database.ExecuteNonQuery(sqlStringCommand);
					num++;
				}
				result = num;
			}
			return result;
		}
		public void SwapHelpCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Ecshop_HelpCategories", "CategoryId", "DisplaySequence", categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
		}
		public HelpCategoryInfo GetHelpCategory(int categoryId)
		{
			HelpCategoryInfo result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_HelpCategories WHERE CategoryId=@CategoryId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateHelpCategory(dataReader);
				}
			}
			return result;
		}
		public IList<HelpCategoryInfo> GetHelpCategorys()
		{
			IList<HelpCategoryInfo> list = new List<HelpCategoryInfo>();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_HelpCategories ORDER BY DisplaySequence");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateHelpCategory(dataReader));
				}
			}
			return list;
		}
	}
}

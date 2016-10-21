using EcShop.Core;
using EcShop.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
namespace EcShop.SqlDal.Commodities
{
	public class TagDao
	{
		private Database database;
		public TagDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DataTable GetTags()
		{
			DataTable result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Ecshop_Tags");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public string GetTagName(int tagId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT TagName  FROM  Ecshop_Tags WHERE TagID = {0}", tagId));
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj != null)
			{
				result = obj.ToString();
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
		public int AddTags(string tagname)
		{
			int result = 0;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Tags VALUES(@TagName);SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, tagname);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				result = Convert.ToInt32(obj.ToString());
			}
			return result;
		}
		public bool UpdateTags(int tagId, string tagName)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Tags SET TagName=@TagName WHERE TagID=@TagID");
			this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, tagName);
			this.database.AddInParameter(sqlStringCommand, "TagID", DbType.Int32, tagId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool DeleteTags(int tagId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_ProductTag WHERE TagID=@TagID;DELETE FROM Ecshop_Tags WHERE TagID=@TagID;");
			this.database.AddInParameter(sqlStringCommand, "TagID", DbType.Int32, tagId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public int GetTags(string tagName)
		{
			int result = 0;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TagID  FROM  Ecshop_Tags WHERE TagName=@TagName");
			this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, tagName);
			IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				result = Convert.ToInt32(dataReader["TagID"].ToString());
			}
			return result;
		}
		public bool AddProductTags(int productId, IList<int> tagIds, DbTransaction tran)
		{
			bool flag = false;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_ProductTag VALUES(@TagId,@ProductId)");
			this.database.AddInParameter(sqlStringCommand, "TagId", DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32);
			foreach (int current in tagIds)
			{
				this.database.SetParameterValue(sqlStringCommand, "ProductId", productId);
				this.database.SetParameterValue(sqlStringCommand, "TagId", current);
				if (tran != null)
				{
					flag = (this.database.ExecuteNonQuery(sqlStringCommand, tran) > 0);
				}
				else
				{
					flag = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
				}
				if (!flag)
				{
					break;
				}
			}
			return flag;
		}
		public bool DeleteProductTags(int productId, DbTransaction tran)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_ProductTag WHERE ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			bool result;
			if (tran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, tran) >= 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 0);
			}
			return result;
		}
	}
}

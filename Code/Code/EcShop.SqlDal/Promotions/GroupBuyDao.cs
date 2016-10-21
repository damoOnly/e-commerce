using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Promotions;
using EcShop.SqlDal.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Promotions
{
	public class GroupBuyDao
	{
		private Database database;
		public GroupBuyDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public string GetPriceByProductId(int productId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SalePrice FROM vw_Ecshop_BrowseProductList WHERE ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return this.database.ExecuteScalar(sqlStringCommand).ToString();
		}
		public int AddGroupBuy(GroupBuyInfo groupBuy, DbTransaction dbTran)
		{
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Ecshop_GroupBuy;INSERT INTO Ecshop_GroupBuy(ProductId,NeedPrice,StartDate,EndDate,MaxCount,Content,Status,DisplaySequence,SupplierId) VALUES(@ProductId,@NeedPrice,@StartDate,@EndDate,@MaxCount,@Content,@Status,@DisplaySequence,@SupplierId); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, groupBuy.ProductId);
			this.database.AddInParameter(sqlStringCommand, "NeedPrice", DbType.Currency, groupBuy.NeedPrice);
			this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, groupBuy.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, groupBuy.EndDate);
			this.database.AddInParameter(sqlStringCommand, "MaxCount", DbType.Int32, groupBuy.MaxCount);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, groupBuy.Content);
			this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, groupBuy.SupplierId);
			object obj;
			if (dbTran != null)
			{
				obj = this.database.ExecuteScalar(sqlStringCommand, dbTran);
			}
			else
			{
				obj = this.database.ExecuteScalar(sqlStringCommand);
			}
			int result;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public bool ProductGroupBuyExist(int productId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Ecshop_GroupBuy WHERE ProductId=@ProductId AND Status=@Status");
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public bool AddGroupBuyCondition(int groupBuyId, IList<GropBuyConditionInfo> gropBuyConditions, DbTransaction dbTran)
		{
			bool result;
			if (gropBuyConditions.Count <= 0)
			{
				result = false;
			}
			else
			{
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_GroupBuyCondition(GroupBuyId,Count,Price) VALUES(@GroupBuyId,@Count,@Price)");
				this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
				this.database.AddInParameter(sqlStringCommand, "Count", DbType.Int32);
				this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency);
				try
				{
					foreach (GropBuyConditionInfo current in gropBuyConditions)
					{
						this.database.SetParameterValue(sqlStringCommand, "Count", current.Count);
						this.database.SetParameterValue(sqlStringCommand, "Price", current.Price);
						if (dbTran != null)
						{
							this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
						}
						else
						{
							this.database.ExecuteNonQuery(sqlStringCommand);
						}
					}
					result = true;
				}
				catch
				{
					result = false;
				}
			}
			return result;
		}
		public bool DeleteGroupBuy(int groupBuyId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_GroupBuy WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool DeleteGroupBuyCondition(int groupBuyId, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
		public bool UpdateGroupBuy(GroupBuyInfo groupBuy, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_GroupBuy SET ProductId=@ProductId,NeedPrice=@NeedPrice,StartDate=@StartDate,EndDate=@EndDate,MaxCount=@MaxCount,Content=@Content WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuy.GroupBuyId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, groupBuy.ProductId);
			this.database.AddInParameter(sqlStringCommand, "NeedPrice", DbType.Currency, groupBuy.NeedPrice);
			this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, groupBuy.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, groupBuy.EndDate);
			this.database.AddInParameter(sqlStringCommand, "MaxCount", DbType.Int32, groupBuy.MaxCount);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, groupBuy.Content);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public GroupBuyInfo GetGroupBuy(int groupBuyId)
		{
			GroupBuyInfo groupBuyInfo = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Ecshop_GroupBuy WHERE GroupBuyId=@GroupBuyId;SELECT * FROM Ecshop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					groupBuyInfo = DataMapper.PopulateGroupBuy(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					GropBuyConditionInfo gropBuyConditionInfo = new GropBuyConditionInfo();
					gropBuyConditionInfo.Count = (int)dataReader["Count"];
					gropBuyConditionInfo.Price = (decimal)dataReader["Price"];
					groupBuyInfo.GroupBuyConditions.Add(gropBuyConditionInfo);
				}
			}
			return groupBuyInfo;
		}
		public DbQueryResult GetGroupBuyList(GroupBuyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
            if (query.SupplierId.HasValue)
            {
                stringBuilder.AppendFormat(" and SupplierId = {0} ",query.SupplierId);
            }
            string selectFields = "GroupBuyId,ProductId,ProductName,MaxCount,NeedPrice,Status,OrderCount,ISNULL(ProdcutQuantity,0) AS ProdcutQuantity,StartDate,EndDate,DisplaySequence,SupplierId";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_GroupBuy", "GroupBuyId", stringBuilder.ToString(), selectFields);
		}
		public void SwapGroupBuySequence(int groupBuyId, int displaySequence)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_GroupBuy SET DisplaySequence = @DisplaySequence WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, displaySequence);
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @price money;SELECT @price = MIN(price) FROM Ecshop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND Count<=@prodcutQuantity;if @price IS NULL SELECT @price = max(price) FROM Ecshop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId ;select @price");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "prodcutQuantity", DbType.Int32, prodcutQuantity);
			return (decimal)this.database.ExecuteScalar(sqlStringCommand);
		}
		public int GetOrderCount(int groupBuyId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM Ecshop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE GroupBuyId = @GroupBuyId AND OrderStatus <> 1 AND OrderStatus <> 4)");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_GroupBuy SET Status=@Status WHERE GroupBuyId=@GroupBuyId;UPDATE Ecshop_Orders SET GroupBuyStatus=@Status WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, (int)status);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool SetGroupBuyEndUntreated(int groupBuyId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_GroupBuy SET Status=@Status,EndDate=@EndDate WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 2);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public DataSet GetGroupByProductList(ProductBrowseQuery query, out int TotalGroupBuyProducts)
		{
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_GroupBuyProducts_Get");
			string text = string.Format("SELECT GroupBuyId,ProductId,StartDate,EndDate FROM Ecshop_GroupBuy WHERE datediff(hh,EndDate,getdate())<=0 AND Status={0}", 1);
			text += string.Format(" AND ProductId IN(SELECT ProductId FROM Ecshop_Products WHERE SaleStatus={0}) ORDER BY DisplaySequence DESC", 1);
			this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, text);
			this.database.AddOutParameter(storedProcCommand, "TotalGroupBuyProducts", DbType.Int32, 4);
			DataSet result = this.database.ExecuteDataSet(storedProcCommand);
			TotalGroupBuyProducts = (int)this.database.GetParameterValue(storedProcCommand, "TotalGroupBuyProducts");
			return result;
		}
		public DataTable GetGroupByProductList(int maxnum)
		{
			DataTable result = new DataTable();
            string query = string.Format("SELECT top " + maxnum + "  S.GroupBuyId,S.StartDate,S.EndDate,P.ProductName,p.MarketPrice, P.SalePrice as OldPrice,ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310, P.ProductId,G.[Count],G.Price from vw_Ecshop_CDisableBrowseProductList as P inner join Ecshop_GroupBuy as S on P.ProductId=s.ProductId inner join  Ecshop_GroupBuyCondition as G on G.GroupBuyId=S.GroupBuyId where datediff(hh,S.EndDate,getdate())<=0 and P.SaleStatus={0} order by S.DisplaySequence desc", 1);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public GroupBuyInfo GetProductGroupBuyInfo(int productId)
		{
			GroupBuyInfo groupBuyInfo = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_GroupBuy WHERE ProductId=@ProductId AND Status = @Status; SELECT * FROM Ecshop_GroupBuyCondition WHERE GroupBuyId=(SELECT GroupBuyId FROM Ecshop_GroupBuy WHERE ProductId=@ProductId AND Status = @Status)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					groupBuyInfo = DataMapper.PopulateGroupBuy(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					GropBuyConditionInfo gropBuyConditionInfo = new GropBuyConditionInfo();
					gropBuyConditionInfo.Count = (int)dataReader["Count"];
					gropBuyConditionInfo.Price = (decimal)dataReader["Price"];
					if (groupBuyInfo != null)
					{
						groupBuyInfo.GroupBuyConditions.Add(gropBuyConditionInfo);
					}
				}
			}
			return groupBuyInfo;
		}
		public DataTable GetGroupBuyProducts(int? categoryId, string keyWord, int page, int size, out int total, bool onlyUnFinished = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("a.GroupBuyId,a.ProductId,a.ProductName,b.ProductCode,b.ShortDescription,SoldCount,ProdcutQuantity,");
			stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,a.Price,b.SalePrice");
			StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append(" vw_Ecshop_GroupBuy a left join vw_Ecshop_CDisableBrowseProductList b on a.ProductId = b.ProductId  ");
			StringBuilder stringBuilder3 = new StringBuilder(" SaleStatus=1");
			if (onlyUnFinished)
			{
				stringBuilder3.AppendFormat(" AND  a.Status = {0}", 1);
			}
			if (categoryId.HasValue)
			{
				CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
				if (category != null)
				{
					stringBuilder3.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
				}
			}
			if (!string.IsNullOrEmpty(keyWord))
			{
				stringBuilder3.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%')", DataHelper.CleanSearchString(keyWord));
			}
			string sortBy = "a.DisplaySequence";
			DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(page, size, sortBy, SortAction.Desc, true, stringBuilder2.ToString(), "GroupBuyId", stringBuilder3.ToString(), stringBuilder.ToString());
			DataTable result = (DataTable)dbQueryResult.Data;
			total = dbQueryResult.TotalRecords;
			return result;
		}
	}
}

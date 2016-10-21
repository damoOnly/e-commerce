using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Orders
{
	public class OrderGiftDao
	{
		private Database database;
		public OrderGiftDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DbQueryResult GetOrderGifts(OrderGiftQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("select top {0} * from Ecshop_OrderGifts where OrderId=@OrderId", query.PageSize);
			if (query.PageIndex == 1)
			{
				stringBuilder.Append(" ORDER BY GiftId ASC");
			}
			else
			{
				stringBuilder.AppendFormat(" and GiftId > (select max(GiftId) from (select top {0} GiftId from Ecshop_OrderGifts where 0=0 and OrderId=@OrderId ORDER BY GiftId ASC ) as tbltemp) ORDER BY GiftId ASC", (query.PageIndex - 1) * query.PageSize);
			}
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";select count(GiftId) as Total from Ecshop_OrderGifts where OrderId=@OrderId", new object[0]);
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, query.OrderId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}
		public DbQueryResult GetGifts(GiftQuery query)
		{
			string filter = null;
			if (!string.IsNullOrEmpty(query.Name))
			{
				filter = string.Format("[Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
			}
			Pagination page = query.Page;
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Ecshop_Gifts", "GiftId", filter, "*");
		}
		public bool ClearOrderGifts(string orderId, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_OrderGifts WHERE OrderId =@OrderId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
			}
			return result;
		}
		public bool AddOrderGift(string orderId, OrderGiftInfo gift, int quantity, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Ecshop_OrderGifts where OrderId=@OrderId AND GiftId=@GiftId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, gift.GiftId);
			bool result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("update Ecshop_OrderGifts set Quantity=@Quantity where OrderId=@OrderId AND GiftId=@GiftId");
					this.database.AddInParameter(sqlStringCommand2, "OrderId", DbType.String, orderId);
					this.database.AddInParameter(sqlStringCommand2, "GiftId", DbType.Int32, gift.GiftId);
					this.database.AddInParameter(sqlStringCommand2, "Quantity", DbType.Int32, (int)dataReader["Quantity"] + quantity);
					if (dbTran != null)
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand2, dbTran) == 1);
					}
					else
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand2) == 1);
					}
				}
				else
				{
					DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("INSERT INTO Ecshop_OrderGifts(OrderId,GiftId,GiftName,CostPrice,ThumbnailsUrl,Quantity,PromoType) VALUES(@OrderId,@GiftId,@GiftName,@CostPrice,@ThumbnailsUrl,@Quantity,@PromoType)");
					this.database.AddInParameter(sqlStringCommand3, "OrderId", DbType.String, orderId);
					this.database.AddInParameter(sqlStringCommand3, "GiftId", DbType.Int32, gift.GiftId);
					this.database.AddInParameter(sqlStringCommand3, "GiftName", DbType.String, gift.GiftName);
					this.database.AddInParameter(sqlStringCommand3, "CostPrice", DbType.Currency, gift.CostPrice);
					this.database.AddInParameter(sqlStringCommand3, "ThumbnailsUrl", DbType.String, gift.ThumbnailsUrl);
					this.database.AddInParameter(sqlStringCommand3, "Quantity", DbType.Int32, gift.Quantity);
					this.database.AddInParameter(sqlStringCommand3, "PromoType", DbType.Int16, gift.PromoteType);
					if (dbTran != null)
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand3, dbTran) == 1);
					}
					else
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand3) == 1);
					}
				}
			}
			return result;
		}
		public IList<GiftInfo> GetGiftList(GiftQuery query)
		{
			IList<GiftInfo> list = new List<GiftInfo>();
			string query2 = string.Format("SELECT * FROM Ecshop_Gifts WHERE [Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query2);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateGift(dataReader));
				}
			}
			return list;
		}
		public OrderGiftInfo GetOrderGift(int giftId, string orderId)
		{
			OrderGiftInfo result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateOrderGift(dataReader);
				}
			}
			return result;
		}
		public bool DeleteOrderGift(string orderId, int giftId, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId ");
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
			}
			return result;
		}
	}
}

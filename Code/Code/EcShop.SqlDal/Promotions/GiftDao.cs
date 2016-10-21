using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Promotions
{
	public class GiftDao
	{
		private Database database;
		public GiftDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public GiftActionStatus CreateUpdateDeleteGift(GiftInfo gift, DataProviderAction action)
		{
			GiftActionStatus result;
			if (null == gift)
			{
				result = GiftActionStatus.UnknowError;
			}
			else
			{
				DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Gift_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
				if (DataProviderAction.Create != action)
				{
					this.database.AddInParameter(storedProcCommand, "GiftId", DbType.Int32, gift.GiftId);
				}
				else
				{
					this.database.AddOutParameter(storedProcCommand, "GiftId", DbType.Int32, 4);
				}
				if (DataProviderAction.Delete != action)
				{
					this.database.AddInParameter(storedProcCommand, "Name", DbType.String, gift.Name);
					this.database.AddInParameter(storedProcCommand, "ShortDescription", DbType.String, gift.ShortDescription);
					this.database.AddInParameter(storedProcCommand, "Unit", DbType.String, gift.Unit);
					this.database.AddInParameter(storedProcCommand, "LongDescription", DbType.String, gift.LongDescription);
					this.database.AddInParameter(storedProcCommand, "Title", DbType.String, gift.Title);
					this.database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, gift.Meta_Description);
					this.database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, gift.Meta_Keywords);
					this.database.AddInParameter(storedProcCommand, "CostPrice", DbType.Currency, gift.CostPrice);
					this.database.AddInParameter(storedProcCommand, "ImageUrl", DbType.String, gift.ImageUrl);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl40", DbType.String, gift.ThumbnailUrl40);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl60", DbType.String, gift.ThumbnailUrl60);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl100", DbType.String, gift.ThumbnailUrl100);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl160", DbType.String, gift.ThumbnailUrl160);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl180", DbType.String, gift.ThumbnailUrl180);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl220", DbType.String, gift.ThumbnailUrl220);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl310", DbType.String, gift.ThumbnailUrl310);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl410", DbType.String, gift.ThumbnailUrl410);
					this.database.AddInParameter(storedProcCommand, "MarketPrice", DbType.Currency, gift.MarketPrice);
					this.database.AddInParameter(storedProcCommand, "NeedPoint", DbType.Int32, gift.NeedPoint);
					this.database.AddInParameter(storedProcCommand, "IsPromotion", DbType.Boolean, gift.IsPromotion);
				}
				else
				{
					this.database.AddInParameter(storedProcCommand, "IsPromotion", DbType.Boolean, false);
				}
				this.database.ExecuteNonQuery(storedProcCommand);
				GiftActionStatus giftActionStatus = (GiftActionStatus)((int)this.database.GetParameterValue(storedProcCommand, "Status"));
				result = giftActionStatus;
			}
			return result;
		}
		public DbQueryResult GetGifts(GiftQuery query)
		{
			string text = string.Format("[Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
			if (query.IsPromotion)
			{
				text += " AND IsPromotion = 1";
			}
			if (query.IsOnline)
			{
				text += " AND NeedPoint > 0";
			}
			Pagination page = query.Page;
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Ecshop_Gifts", "GiftId", text, "*");
		}
		public GiftInfo GetGiftDetails(int giftId)
		{
			GiftInfo result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Gifts WHERE GiftId = @GiftId");
			this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateGift(dataReader);
				}
			}
			return result;
		}
		public bool UpdateIsDownLoad(int giftId)
		{
			string query = "update Ecshop_Gifts set where GiftId = @GiftId;";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
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
		public IList<GiftInfo> GetGifts(int maxnum)
		{
			List<GiftInfo> list = new List<GiftInfo>();
			string query = "SELECT TOP " + maxnum + " * FROM Ecshop_Gifts";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateGift(dataReader));
				}
			}
			return list;
		}
		public IList<GiftInfo> GetOnlinePromotionGifts()
		{
			List<GiftInfo> list = new List<GiftInfo>();
			string query = "SELECT * FROM Ecshop_Gifts WHERE IsPromotion=1";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateGift(dataReader));
				}
			}
			return list;
		}
	}
}

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.VShop
{
	public class FeedBackDao
	{
		private Database database;
		public FeedBackDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public bool Save(FeedBackInfo info)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO vshop_FeedBackNotify (AppId, TimeStamp, OpenId, MsgType, FeedBackId, TransId, Reason, Solution, ExtInfo) VALUES (@AppId, @TimeStamp, @OpenId, @MsgType, @FeedBackId, @TransId, @Reason, @Solution, @ExtInfo)");
			this.database.AddInParameter(sqlStringCommand, "AppId", DbType.String, info.AppId);
			this.database.AddInParameter(sqlStringCommand, "TimeStamp", DbType.DateTime, info.TimeStamp);
			this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, info.OpenId);
			this.database.AddInParameter(sqlStringCommand, "MsgType", DbType.String, info.MsgType);
			this.database.AddInParameter(sqlStringCommand, "FeedBackId", DbType.String, info.FeedBackId);
			this.database.AddInParameter(sqlStringCommand, "TransId", DbType.String, info.TransId);
			this.database.AddInParameter(sqlStringCommand, "Reason", DbType.String, info.Reason);
			this.database.AddInParameter(sqlStringCommand, "Solution", DbType.String, info.Solution);
			this.database.AddInParameter(sqlStringCommand, "ExtInfo", DbType.String, info.ExtInfo);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public FeedBackInfo Get(int id)
		{
			FeedBackInfo result = new FeedBackInfo();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_FeedBackNotify WHERE FeedBackNotifyID=@FeedBackNotifyID");
			this.database.AddInParameter(sqlStringCommand, "FeedBackNotifyID", DbType.Int32, id);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<FeedBackInfo>(dataReader);
			}
			return result;
		}
		public FeedBackInfo Get(string feedBackID)
		{
			FeedBackInfo result = new FeedBackInfo();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_FeedBackNotify WHERE FeedBackId=@FeedBackId");
			this.database.AddInParameter(sqlStringCommand, "FeedBackId", DbType.String, feedBackID);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<FeedBackInfo>(dataReader);
			}
			return result;
		}
		public bool UpdateMsgType(string feedBackId, string msgType)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_FeedBackNotify SET MsgType=@MsgType WHERE FeedBackId=@FeedBackId");
			this.database.AddInParameter(sqlStringCommand, "MsgType", DbType.String, msgType);
			this.database.AddInParameter(sqlStringCommand, "FeedBackId", DbType.String, feedBackId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool Delete(int id)
		{
			string query = string.Format("DELETE FROM vshop_FeedBackNotify WHERE FeedBackNotifyID = {0}", id);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public DbQueryResult List(int pageIndex, int pageSize, string msgType)
		{
			string filter = " 1=1 ";
			if (!string.IsNullOrEmpty(msgType))
			{
				filter = string.Format(" MsgType = '{0}' ", DataHelper.CleanSearchString(msgType));
			}
			return DataHelper.PagingByRownumber(pageIndex, pageSize, "FeedBackNotifyID", SortAction.Desc, true, "vshop_FeedBackNotify p", "FeedBackNotifyID", filter, "*");
		}
	}
}

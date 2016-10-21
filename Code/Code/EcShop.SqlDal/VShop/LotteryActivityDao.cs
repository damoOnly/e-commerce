using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.VShop
{
	public class LotteryActivityDao
	{
		private Database database;
		public LotteryActivityDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public int SaveLotteryTicket(LotteryTicketInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into Vshop_LotteryActivity(");
			stringBuilder.Append("ActivityName,ActivityType,StartTime,OpenTime,EndTime,ActivityDesc,ActivityPic,ActivityKey,PrizeSetting,GradeIds,MinValue,InvitationCode,IsOpened)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ActivityName,@ActivityType,@StartTime,@OpenTime,@EndTime,@ActivityDesc,@ActivityPic,@ActivityKey,@PrizeSetting,@GradeIds,@MinValue,@InvitationCode,@IsOpened)");
			stringBuilder.Append(";select @@IDENTITY");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityName", DbType.String, info.ActivityName);
			this.database.AddInParameter(sqlStringCommand, "ActivityType", DbType.Int32, info.ActivityType);
			this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, info.StartTime);
			this.database.AddInParameter(sqlStringCommand, "OpenTime", DbType.DateTime, info.OpenTime);
			this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, info.EndTime);
			this.database.AddInParameter(sqlStringCommand, "ActivityDesc", DbType.String, info.ActivityDesc);
			this.database.AddInParameter(sqlStringCommand, "ActivityPic", DbType.String, info.ActivityPic);
			this.database.AddInParameter(sqlStringCommand, "ActivityKey", DbType.String, info.ActivityKey);
			this.database.AddInParameter(sqlStringCommand, "PrizeSetting", DbType.String, info.PrizeSetting);
			this.database.AddInParameter(sqlStringCommand, "GradeIds", DbType.String, info.GradeIds);
			this.database.AddInParameter(sqlStringCommand, "MinValue", DbType.Int32, info.MinValue);
			this.database.AddInParameter(sqlStringCommand, "InvitationCode", DbType.String, info.InvitationCode);
			this.database.AddInParameter(sqlStringCommand, "IsOpened", DbType.Boolean, info.IsOpened);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int num;
			int result;
			if (!int.TryParse(obj.ToString(), out num))
			{
				result = 0;
			}
			else
			{
				result = num;
			}
			return result;
		}
		public bool UpdateLotteryTicket(LotteryTicketInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update Vshop_LotteryActivity set ");
			stringBuilder.Append("ActivityName=@ActivityName,");
			stringBuilder.Append("ActivityType=@ActivityType,");
			stringBuilder.Append("StartTime=@StartTime,");
			stringBuilder.Append("OpenTime=@OpenTime,");
			stringBuilder.Append("EndTime=@EndTime,");
			stringBuilder.Append("ActivityDesc=@ActivityDesc,");
			stringBuilder.Append("ActivityPic=@ActivityPic,");
			stringBuilder.Append("ActivityKey=@ActivityKey,");
			stringBuilder.Append("PrizeSetting=@PrizeSetting,");
			stringBuilder.Append("GradeIds=@GradeIds,");
			stringBuilder.Append("MinValue=@MinValue,");
			stringBuilder.Append("InvitationCode=@InvitationCode,");
			stringBuilder.Append("IsOpened=@IsOpened");
			stringBuilder.Append(" where ActivityId=@ActivityId ");
			stringBuilder.Append(";UPDATE vshop_Reply SET Keys = @ActivityKey WHERE ActivityId = @ActivityId  AND [ReplyType] = @ReplyType");
			string value = ((LotteryActivityType)info.ActivityType).ToString();
			object obj = Enum.Parse(typeof(ReplyType), value);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, info.ActivityId);
			this.database.AddInParameter(sqlStringCommand, "ActivityName", DbType.String, info.ActivityName);
			this.database.AddInParameter(sqlStringCommand, "ActivityType", DbType.Int32, info.ActivityType);
			this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, info.StartTime);
			this.database.AddInParameter(sqlStringCommand, "OpenTime", DbType.DateTime, info.OpenTime);
			this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, info.EndTime);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)obj);
			this.database.AddInParameter(sqlStringCommand, "ActivityDesc", DbType.String, info.ActivityDesc);
			this.database.AddInParameter(sqlStringCommand, "ActivityPic", DbType.String, info.ActivityPic);
			this.database.AddInParameter(sqlStringCommand, "ActivityKey", DbType.String, info.ActivityKey);
			this.database.AddInParameter(sqlStringCommand, "PrizeSetting", DbType.String, info.PrizeSetting);
			this.database.AddInParameter(sqlStringCommand, "GradeIds", DbType.String, info.GradeIds);
			this.database.AddInParameter(sqlStringCommand, "MinValue", DbType.Int32, info.MinValue);
			this.database.AddInParameter(sqlStringCommand, "InvitationCode", DbType.String, info.InvitationCode);
			this.database.AddInParameter(sqlStringCommand, "IsOpened", DbType.Boolean, info.IsOpened);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool DelteLotteryTicket(int activityId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("delete from Vshop_LotteryActivity ");
			stringBuilder.Append(" where ActivityId=@ActivityId;DELETE FROM vshop_Reply WHERE ActivityId = @ActivityId AND [ReplyType] = @ReplyType");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 64);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public LotteryTicketInfo GetLotteryTicket(int activityid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ActivityId,ActivityName,ActivityType,StartTime,OpenTime,EndTime,ActivityDesc,ActivityPic,ActivityKey,PrizeSetting,GradeIds,MinValue,InvitationCode,IsOpened from Vshop_LotteryActivity ");
			stringBuilder.Append(" where ActivityId=@ActivityId ");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityid);
			LotteryTicketInfo result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<LotteryTicketInfo>(dataReader);
			}
			return result;
		}
		public DbQueryResult GetLotteryTicketList(LotteryActivityQuery page)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (page.ActivityType != (LotteryActivityType)0)
			{
				stringBuilder.AppendFormat("ActivityType={0}", (int)page.ActivityType);
			}
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Vshop_LotteryActivity", "ActivityId", stringBuilder.ToString(), "*");
		}
		public int InsertLotteryActivity(LotteryActivityInfo model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into Vshop_LotteryActivity(");
			stringBuilder.Append("ActivityName,ActivityType,StartTime,EndTime,ActivityDesc,ActivityPic,ActivityKey,PrizeSetting,MaxNum)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ActivityName,@ActivityType,@StartTime,@EndTime,@ActivityDesc,@ActivityPic,@ActivityKey,@PrizeSetting,@MaxNum)");
			stringBuilder.Append(";select @@IDENTITY");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityName", DbType.String, model.ActivityName);
			this.database.AddInParameter(sqlStringCommand, "ActivityType", DbType.Int32, model.ActivityType);
			this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, model.StartTime);
			this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, model.EndTime);
			this.database.AddInParameter(sqlStringCommand, "ActivityDesc", DbType.String, model.ActivityDesc);
			this.database.AddInParameter(sqlStringCommand, "ActivityPic", DbType.String, model.ActivityPic);
			this.database.AddInParameter(sqlStringCommand, "ActivityKey", DbType.String, model.ActivityKey);
			this.database.AddInParameter(sqlStringCommand, "PrizeSetting", DbType.String, model.PrizeSetting);
			this.database.AddInParameter(sqlStringCommand, "MaxNum", DbType.Int32, model.MaxNum);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int num;
			int result;
			if (!int.TryParse(obj.ToString(), out num))
			{
				result = 0;
			}
			else
			{
				result = num;
			}
			return result;
		}
		public bool UpdateLotteryActivity(LotteryActivityInfo model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update Vshop_LotteryActivity set ");
			stringBuilder.Append("ActivityName=@ActivityName,");
			stringBuilder.Append("ActivityType=@ActivityType,");
			stringBuilder.Append("StartTime=@StartTime,");
			stringBuilder.Append("EndTime=@EndTime,");
			stringBuilder.Append("ActivityDesc=@ActivityDesc,");
			stringBuilder.Append("ActivityPic=@ActivityPic,");
			stringBuilder.Append("ActivityKey=@ActivityKey,");
			stringBuilder.Append("PrizeSetting=@PrizeSetting,");
			stringBuilder.Append("MaxNum=@MaxNum ");
			stringBuilder.Append(" where ActivityId=@ActivityId ");
			stringBuilder.Append(";UPDATE vshop_Reply SET Keys = @ActivityKey WHERE ActivityId = @ActivityId  AND [ReplyType] = @ReplyType");
			string value = ((LotteryActivityType)model.ActivityType).ToString();
			object obj = Enum.Parse(typeof(ReplyType), value);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, model.ActivityId);
			this.database.AddInParameter(sqlStringCommand, "ActivityName", DbType.String, model.ActivityName);
			this.database.AddInParameter(sqlStringCommand, "ActivityType", DbType.Int32, model.ActivityType);
			this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, model.StartTime);
			this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, model.EndTime);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)obj);
			this.database.AddInParameter(sqlStringCommand, "ActivityDesc", DbType.String, model.ActivityDesc);
			this.database.AddInParameter(sqlStringCommand, "ActivityPic", DbType.String, model.ActivityPic);
			this.database.AddInParameter(sqlStringCommand, "ActivityKey", DbType.String, model.ActivityKey);
			this.database.AddInParameter(sqlStringCommand, "PrizeSetting", DbType.String, model.PrizeSetting);
			this.database.AddInParameter(sqlStringCommand, "MaxNum", DbType.Int32, model.MaxNum);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool DelteLotteryActivity(int activityid, string type)
		{
			object obj = Enum.Parse(typeof(ReplyType), type);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("delete from Vshop_LotteryActivity ");
			stringBuilder.Append(" where ActivityId=@ActivityId;DELETE FROM vshop_Reply WHERE ActivityId = @ActivityId AND [ReplyType] = @ReplyType");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityid);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)obj);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public LotteryActivityInfo GetLotteryActivityInfo(int activityid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ActivityId,ActivityName,ActivityType,StartTime,EndTime,ActivityDesc,ActivityPic,ActivityKey,PrizeSetting,MaxNum from Vshop_LotteryActivity ");
			stringBuilder.Append(" where ActivityId=@ActivityId ");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityid);
			LotteryActivityInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<LotteryActivityInfo>(dataReader);
			}
			return result;
		}
		public IList<LotteryActivityInfo> GetLotteryActivityByType(LotteryActivityType type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ActivityId,ActivityName from Vshop_LotteryActivity ");
			stringBuilder.Append(" where ActivityType=@ActivityType order by ActivityId desc ");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityType", DbType.Int32, (int)type);
			IList<LotteryActivityInfo> result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<LotteryActivityInfo>(dataReader);
			}
			return result;
		}
		public DbQueryResult GetLotteryActivityList(LotteryActivityQuery page)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (page.ActivityType != (LotteryActivityType)0)
			{
				stringBuilder.AppendFormat("ActivityType={0}", (int)page.ActivityType);
			}
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Vshop_LotteryActivity", "ActivityId", stringBuilder.ToString(), "*");
		}
		public int AddPrizeRecord(PrizeRecordInfo model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into Vshop_PrizeRecord(");
			stringBuilder.Append("ActivityID,PrizeTime,UserID,PrizeName,Prizelevel,IsPrize)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@ActivityID,@PrizeTime,@UserID,@PrizeName,@Prizelevel,@IsPrize)");
			stringBuilder.Append(";select @@IDENTITY");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityID", DbType.Int32, model.ActivityID);
			this.database.AddInParameter(sqlStringCommand, "PrizeTime", DbType.DateTime, model.PrizeTime);
			this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, model.UserID);
			this.database.AddInParameter(sqlStringCommand, "PrizeName", DbType.String, model.PrizeName);
			this.database.AddInParameter(sqlStringCommand, "Prizelevel", DbType.String, model.Prizelevel);
			this.database.AddInParameter(sqlStringCommand, "IsPrize", DbType.Boolean, model.IsPrize);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int num;
			int result;
			if (!int.TryParse(obj.ToString(), out num))
			{
				result = 0;
			}
			else
			{
				result = num;
			}
			return result;
		}
		public bool DeletePrizeRecord(int RecordId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("delete from Vshop_PrizeRecord ");
			stringBuilder.Append(" where RecordId=@RecordId ");
			Database database = DatabaseFactory.CreateDatabase();
			DbCommand sqlStringCommand = database.GetSqlStringCommand(stringBuilder.ToString());
			database.AddInParameter(sqlStringCommand, "RecordId", DbType.Int32, RecordId);
			return database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public List<PrizeRecordInfo> GetPrizeList(PrizeQuery page)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ActivityName=(select  ActivityName from Vshop_LotteryActivity a where a.ActivityId=b.ActivityId),");
			stringBuilder.Append("UserName=(select UserName from aspnet_Users c where  c.UserId=b.UserId),");
			stringBuilder.Append(" b.* from Vshop_PrizeRecord b");
			if (page.ActivityId != 0)
			{
				stringBuilder.AppendFormat(" where b.ActivityId={0}", page.ActivityId);
			}
			stringBuilder.AppendFormat(" and b.IsPrize=1 order by b.PrizeTime desc", new object[0]);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			List<PrizeRecordInfo> result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = (ReaderConvert.ReaderToList<PrizeRecordInfo>(dataReader) as List<PrizeRecordInfo>);
			}
			return result;
		}
	}
}

using EcShop.Entities;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.VShop
{
	public class PrizeRecordDao
	{
		private Database database;
		public PrizeRecordDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public bool HasSignUp(int activityId, int userId)
		{
			string query = "select count(*) from Vshop_PrizeRecord where ActivityID=@ActivityID and UserID=@UserID";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityID", DbType.Int32, activityId);
			this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, userId);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) > 0;
		}
		public int GetCountBySignUp(int activityId)
		{
			string query = "select count(*) from Vshop_PrizeRecord where ActivityID=@ActivityID";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityID", DbType.Int32, activityId);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}
		public bool OpenTicket(int ticketId, List<PrizeSetting> list)
		{
			bool result;
			if (list == null || list.Count == 0)
			{
				result = false;
			}
			else
			{
				string format = "UPDATE Vshop_PrizeRecord SET Prizelevel=@Prizelevel, PrizeName=@PrizeName WHERE RecordId IN(SELECT TOP {0} RecordId FROM Vshop_PrizeRecord WHERE ActivityID=@ActivityID AND PrizeName IS NULL ORDER BY NewID())";
				foreach (PrizeSetting current in list)
				{
					DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format(format, current.PrizeNum));
					this.database.AddInParameter(sqlStringCommand, "Prizelevel", DbType.String, current.PrizeLevel);
					this.database.AddInParameter(sqlStringCommand, "PrizeName", DbType.String, current.PrizeName);
					this.database.AddInParameter(sqlStringCommand, "ActivityID", DbType.Int32, ticketId);
					this.database.ExecuteNonQuery(sqlStringCommand);
				}
				result = true;
			}
			return result;
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
		public bool UpdatePrizeRecord(PrizeRecordInfo model)
		{
			string query = "UPDATE Vshop_PrizeRecord SET  RealName=@RealName, CellPhone=@CellPhone WHERE ActivityID=@ActivityID AND UserId=@UserId AND IsPrize = 1 AND CellPhone IS NULL AND RealName IS NULL";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityID", DbType.Int32, model.ActivityID);
			this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, model.UserID);
			this.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, model.RealName);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, model.CellPhone);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public PrizeRecordInfo GetUserPrizeRecord(int activityid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select top 1 * from Vshop_PrizeRecord where ActivityId=@ActivityId  and UserID=@UserID order by RecordId desc");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityid);
			this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, HiContext.Current.User.UserId);
			PrizeRecordInfo result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<PrizeRecordInfo>(dataReader);
			}
			return result;
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
			stringBuilder.AppendFormat(" and b.IsPrize=1", new object[0]);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			List<PrizeRecordInfo> result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = (ReaderConvert.ReaderToList<PrizeRecordInfo>(dataReader) as List<PrizeRecordInfo>);
			}
			return result;
		}
		public int GetUserPrizeCount(int ActivityId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select count(*) from Vshop_PrizeRecord where ActivityId=@ActivityId  and UserID=@UserID");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, ActivityId);
			this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, HiContext.Current.User.UserId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
			{
				result = 0;
			}
			else
			{
				result = int.Parse(obj.ToString());
			}
			return result;
		}
	}
}

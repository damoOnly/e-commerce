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
	public class ActivityDao
	{
		private Database database;
		public ActivityDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public int SaveActivity(ActivityInfo activity)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("INSERT INTO vshop_Activity(").Append("Name,Description,StartDate,EndDate,CloseRemark,Keys").Append(",MaxValue,PicUrl,Item1,Item2,Item3,Item4,Item5)").Append(" VALUES (").Append("@Name,@Description,@StartDate,@EndDate,@CloseRemark,@Keys").Append(",@MaxValue,@PicUrl,@Item1,@Item2,@Item3,@Item4,@Item5)").Append(";select @@IDENTITY");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, activity.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, activity.Description);
			this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, activity.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, activity.EndDate);
			this.database.AddInParameter(sqlStringCommand, "CloseRemark", DbType.String, activity.CloseRemark);
			this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, activity.Keys);
			this.database.AddInParameter(sqlStringCommand, "MaxValue", DbType.Int32, activity.MaxValue);
			this.database.AddInParameter(sqlStringCommand, "PicUrl", DbType.String, activity.PicUrl);
			this.database.AddInParameter(sqlStringCommand, "Item1", DbType.String, activity.Item1);
			this.database.AddInParameter(sqlStringCommand, "Item2", DbType.String, activity.Item2);
			this.database.AddInParameter(sqlStringCommand, "Item3", DbType.String, activity.Item3);
			this.database.AddInParameter(sqlStringCommand, "Item4", DbType.String, activity.Item4);
			this.database.AddInParameter(sqlStringCommand, "Item5", DbType.String, activity.Item5);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			int.TryParse(obj.ToString(), out result);
			return result;
		}
		public bool UpdateActivity(ActivityInfo activity)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE vshop_Activity SET ").Append("Name=@Name,").Append("Description=@Description,").Append("StartDate=@StartDate,").Append("EndDate=@EndDate,").Append("CloseRemark=@CloseRemark,").Append("Keys=@Keys,").Append("MaxValue=@MaxValue,").Append("PicUrl=@PicUrl,").Append("Item1=@Item1,").Append("Item2=@Item2,").Append("Item3=@Item3,").Append("Item4=@Item4,").Append("Item5=@Item5").Append(" WHERE ActivityId=@ActivityId").Append(";UPDATE vshop_Reply SET Keys = @Keys WHERE ActivityId = @ActivityId AND [ReplyType] = @ReplyType");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, activity.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, activity.Description);
			this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, activity.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, activity.EndDate);
			this.database.AddInParameter(sqlStringCommand, "CloseRemark", DbType.String, activity.CloseRemark);
			this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, activity.Keys);
			this.database.AddInParameter(sqlStringCommand, "MaxValue", DbType.Int32, activity.MaxValue);
			this.database.AddInParameter(sqlStringCommand, "PicUrl", DbType.String, activity.PicUrl);
			this.database.AddInParameter(sqlStringCommand, "Item1", DbType.String, activity.Item1);
			this.database.AddInParameter(sqlStringCommand, "Item2", DbType.String, activity.Item2);
			this.database.AddInParameter(sqlStringCommand, "Item3", DbType.String, activity.Item3);
			this.database.AddInParameter(sqlStringCommand, "Item4", DbType.String, activity.Item4);
			this.database.AddInParameter(sqlStringCommand, "Item5", DbType.String, activity.Item5);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activity.ActivityId);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 256);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool DeleteActivity(int activityId)
		{
			string query = "DELETE FROM vshop_Activity WHERE ActivityId=@ActivityId; DELETE FROM vshop_Reply WHERE ActivityId = @ActivityId AND [ReplyType] = @ReplyType";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 256);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public ActivityInfo GetActivity(int activityId)
		{
			string query = "SELECT * FROM vshop_Activity WHERE ActivityId=@ActivityId";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			ActivityInfo result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ActivityInfo>(dataReader);
			}
			return result;
		}
		public IList<ActivityInfo> GetAllActivity()
		{
			string query = "SELECT *, (SELECT Count(ActivityId) FROM vshop_ActivitySignUp WHERE ActivityId = a.ActivityId) AS CurrentValue FROM vshop_Activity a ORDER BY ActivityId DESC";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			IList<ActivityInfo> result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ActivityInfo>(dataReader);
			}
			return result;
		}
	}
}

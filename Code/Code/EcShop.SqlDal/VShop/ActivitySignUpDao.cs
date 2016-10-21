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
	public class ActivitySignUpDao
	{
		private Database database;
		public ActivitySignUpDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public int GetActivityCount(int ActivityId)
		{
			string query = "SELECT Count(ActivitySignUpId) FROM vshop_ActivitySignUp WHERE ActivityId =  @ActivityId";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, ActivityId);
			int result = 0;
			try
			{
				result = (int)this.database.ExecuteScalar(sqlStringCommand);
			}
			catch
			{
			}
			return result;
		}
		public IList<ActivitySignUpInfo> GetActivitySignUpById(int activityId)
		{
			string query = "SELECT * FROM vshop_ActivitySignUp WHERE ActivityId = @ActivityId";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			IList<ActivitySignUpInfo> result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ActivitySignUpInfo>(dataReader);
			}
			return result;
		}
		public bool SaveActivitySignUp(ActivitySignUpInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("IF NOT EXISTS (select 1 from vshop_ActivitySignUp WHERE ActivityId=@ActivityId and UserId=@UserId) ").Append("INSERT INTO vshop_ActivitySignUp(").Append("ActivityId,UserId,UserName,RealName,SignUpDate").Append(",Item1,Item2,Item3,Item4,Item5)").Append(" VALUES (").Append("@ActivityId,@UserId,@UserName,@RealName,@SignUpDate").Append(",@Item1,@Item2,@Item3,@Item4,@Item5)");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, info.ActivityId);
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, info.UserId);
			this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, info.UserName);
			this.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, info.RealName);
			this.database.AddInParameter(sqlStringCommand, "SignUpDate", DbType.DateTime, info.SignUpDate);
			this.database.AddInParameter(sqlStringCommand, "Item1", DbType.String, info.Item1);
			this.database.AddInParameter(sqlStringCommand, "Item2", DbType.String, info.Item2);
			this.database.AddInParameter(sqlStringCommand, "Item3", DbType.String, info.Item3);
			this.database.AddInParameter(sqlStringCommand, "Item4", DbType.String, info.Item4);
			this.database.AddInParameter(sqlStringCommand, "Item5", DbType.String, info.Item5);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}

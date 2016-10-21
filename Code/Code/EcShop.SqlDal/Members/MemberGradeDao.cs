using EcShop.Entities;
using EcShop.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Members
{
	public class MemberGradeDao
	{
		private Database database;
		public MemberGradeDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public IList<MemberGradeInfo> GetMemberGrades()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades");
			IList<MemberGradeInfo> result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MemberGradeInfo>(dataReader);
			}
			return result;
		}
		public MemberGradeInfo GetMemberGrade(int gradeId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades WHERE GradeId = @GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			MemberGradeInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<MemberGradeInfo>(dataReader);
			}
			return result;
		}
		public int GetDefaultMemberGrade()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT GradeId FROM aspnet_MemberGrades WHERE IsDefault = 1");
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
		public bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(GradeId) as Count FROM aspnet_MemberGrades WHERE Points=@Points AND GradeId<>@GradeId;");
			this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, memberGrade.Points);
			this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, memberGrade.GradeId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public bool CreateMemberGrade(MemberGradeInfo memberGrade)
		{
			string text = string.Empty;
			if (memberGrade.IsDefault)
			{
				text += "UPDATE aspnet_MemberGrades SET IsDefault = 0";
			}
			text += " INSERT INTO aspnet_MemberGrades ([Name], Description, Points, IsDefault, Discount) VALUES (@Name, @Description, @Points, @IsDefault, @Discount)";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, memberGrade.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, memberGrade.Description);
			this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, memberGrade.Points);
			this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Boolean, memberGrade.IsDefault);
			this.database.AddInParameter(sqlStringCommand, "Discount", DbType.Int32, memberGrade.Discount);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool DeleteMemberGrade(int gradeId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_MemberGrades WHERE GradeId = @GradeId AND IsDefault = 0 AND NOT EXISTS(SELECT * FROM aspnet_Members WHERE GradeId = @GradeId)");
			this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool UpdateMemberGrade(MemberGradeInfo memberGrade)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_MemberGrades SET [Name] = @Name, Description = @Description, Points = @Points, Discount = @Discount WHERE GradeId = @GradeId");
			this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, memberGrade.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, memberGrade.Description);
			this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, memberGrade.Points);
			this.database.AddInParameter(sqlStringCommand, "Discount", DbType.Int32, memberGrade.Discount);
			this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, memberGrade.GradeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public void SetDefalutMemberGrade(int gradeId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_MemberGrades SET IsDefault = 0;UPDATE aspnet_MemberGrades SET IsDefault = 1 WHERE GradeId = @GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}

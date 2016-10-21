using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
namespace EcShop.SqlDal.Comments
{
	public class MessageBoxDao
	{
		private Database database;
		public MessageBoxDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DbQueryResult GetManagerReceivedMessages(MessageBoxQuery query, UserRole role)
		{
			string text = string.Format("Accepter='{0}' AND Sernder IN (SELECT UserName FROM aspnet_Users WHERE UserRole = {1})", query.Accepter, (int)role);
			if (query.MessageStatus == MessageStatus.Replied)
			{
				text += " AND IsRead = 1";
			}
			if (query.MessageStatus == MessageStatus.NoReply)
			{
				text += " AND IsRead = 0";
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Ecshop_ManagerMessageBox", "MessageId", text, "*");
		}
		public DbQueryResult GetManagerSendedMessages(MessageBoxQuery query, UserRole role)
		{
		    string sender = DataHelper.CleanSearchString(query.Sernder);
            string filter = string.Format("Sernder='{0}' AND Accepter IN (SELECT UserName FROM aspnet_Users WHERE UserRole = {1})", sender, (int)role);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Ecshop_ManagerMessageBox", "MessageId", filter, "*");
		}
		public MessageBoxInfo GetManagerMessage(long messageId)
		{
			MessageBoxInfo result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Ecshop_ManagerMessageBox WHERE MessageId=@MessageId;");
			this.database.AddInParameter(sqlStringCommand, "MessageId", DbType.Int64, messageId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateMessageBox(dataReader);
				}
			}
			return result;
		}
		public bool InsertMessage(MessageBoxInfo messageBoxInfo, UserRole toRole)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("BEGIN TRAN ");
			stringBuilder.Append("DECLARE @ContentId int ");
			stringBuilder.Append("DECLARE @errorSun INT ");
			stringBuilder.Append("SET @errorSun=0 ");
			stringBuilder.Append("INSERT INTO [Ecshop_MessageContent]([Title],[Content],[Date]) ");
			stringBuilder.Append("VALUES(@Title,@Content,@Date) ");
			stringBuilder.Append("SET @ContentId = @@IDENTITY  ");
			stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
			stringBuilder.Append("INSERT INTO [Ecshop_ManagerMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) ");
			stringBuilder.Append("VALUES(@ContentId,@Sernder ,@Accepter,@IsRead) ");
			stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
			stringBuilder.AppendFormat("INSERT INTO [{0}]([ContentId],[Sernder],[Accepter],[IsRead]) ", "Ecshop_MemberMessageBox");
			stringBuilder.Append("VALUES(@ContentId,@Sernder ,@Accepter,@IsRead) ");
			stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
			stringBuilder.Append("IF @errorSun<>0 ");
			stringBuilder.Append("BEGIN ");
			stringBuilder.Append("ROLLBACK TRANSACTION  ");
			stringBuilder.Append("END ");
			stringBuilder.Append("ELSE ");
			stringBuilder.Append("BEGIN ");
			stringBuilder.Append("COMMIT TRANSACTION  ");
			stringBuilder.Append("END ");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, messageBoxInfo.Title);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, messageBoxInfo.Content);
			this.database.AddInParameter(sqlStringCommand, "Date", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
			this.database.AddInParameter(sqlStringCommand, "Sernder", DbType.String, messageBoxInfo.Sernder);
			this.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, messageBoxInfo.Accepter);
			this.database.AddInParameter(sqlStringCommand, "IsRead", DbType.Boolean, messageBoxInfo.IsRead);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool AddMessage(MessageBoxInfo messageBoxInfo, UserRole toRole)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("BEGIN TRAN ");
			stringBuilder.Append("DECLARE @ContentId int ");
			stringBuilder.Append("DECLARE @errorSun INT ");
			stringBuilder.Append("SET @errorSun=0 ");
			stringBuilder.Append("INSERT INTO [Ecshop_MessageContent]([Title],[Content],[Date]) ");
			stringBuilder.Append("VALUES(@Title,@Content,@Date) ");
			stringBuilder.Append("SET @ContentId = @@IDENTITY  ");
			stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
			stringBuilder.AppendFormat("INSERT INTO [{0}]([ContentId],[Sernder],[Accepter],[IsRead]) ", "Ecshop_MemberMessageBox");
			stringBuilder.Append("VALUES(@ContentId,@Sernder ,@Accepter,@IsRead) ");
			stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
			stringBuilder.Append("IF @errorSun<>0 ");
			stringBuilder.Append("BEGIN ");
			stringBuilder.Append("ROLLBACK TRANSACTION  ");
			stringBuilder.Append("END ");
			stringBuilder.Append("ELSE ");
			stringBuilder.Append("BEGIN ");
			stringBuilder.Append("COMMIT TRANSACTION  ");
			stringBuilder.Append("END ");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, messageBoxInfo.Title);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, messageBoxInfo.Content);
			this.database.AddInParameter(sqlStringCommand, "Date", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
			this.database.AddInParameter(sqlStringCommand, "Sernder", DbType.String, messageBoxInfo.Sernder);
			this.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, messageBoxInfo.Accepter);
			this.database.AddInParameter(sqlStringCommand, "IsRead", DbType.Boolean, messageBoxInfo.IsRead);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool PostManagerMessageIsRead(long messageId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Ecshop_ManagerMessageBox set IsRead=1 where MessageId=@MessageId");
			this.database.AddInParameter(sqlStringCommand, "MessageId", DbType.Int64, messageId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public int DeleteManagerMessages(IList<long> messageList)
		{
			string text = string.Empty;
			foreach (long current in messageList)
			{
				if (string.IsNullOrEmpty(text))
				{
					text += current.ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					text = text + "," + current.ToString(CultureInfo.InvariantCulture);
				}
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("delete from Ecshop_ManagerMessageBox where MessageId in ({0}) ", text));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public int GetMemberUnReadMessageNum()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(COUNT(*),0) FROM Ecshop_MemberMessageBox WHERE IsRead=0 and Accepter=@Accepter");
			this.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, HiContext.Current.User.Username);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public IList<Member> GetMembersByRank(int? gradeId)
		{
			IList<Member> list = new List<Member>();
			DbCommand sqlStringCommand;
			if (gradeId > 0)
			{
				sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members WHERE GradeId=@GradeId");
				this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			}
			else
			{
				sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members");
			}
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(new Member(UserRole.Member)
					{
						UserId = (int)dataReader["UserId"],
						Email = dataReader["Email"].ToString(),
						Username = dataReader["UserName"].ToString()
					});
				}
			}
			return list;
		}

        public IList<Member> GetMembersByCondition(string condition)
        {
            IList<Member> list = new List<Member>();
            DbCommand sqlStringCommand;
            if (!String.IsNullOrEmpty(condition))
            {
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members WHERE "+condition);
            }
            else
            {
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members");
            }
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    list.Add(new Member(UserRole.Member)
                    {
                        UserId = (int)dataReader["UserId"],
                        Email = dataReader["Email"].ToString(),
                        Username = dataReader["UserName"].ToString()
                    });
                }
            }
            return list;
        }


        public IList<Member> GetMembersByCreateDate(string CreateDate, int top)
        {
            IList<Member> list = new List<Member>();
            DbCommand sqlStringCommand;

            sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT TOP({0})* FROM vw_aspnet_Members WHERE CreateDate>@CreateDate ORDER BY CreateDate", top));
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, CreateDate);
            
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    list.Add(new Member(UserRole.Member)
                    {
                        UserId = (int)dataReader["UserId"],
                        Email = dataReader["Email"].ToString(),
                        Username = dataReader["UserName"].ToString(),
                        AccurateCreateDate = dataReader["CreateDate"].ToString()
                    });
                }
            }
            return list;
        }
	}
}

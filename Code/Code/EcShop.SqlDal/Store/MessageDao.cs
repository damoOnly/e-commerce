using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Comments;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
namespace EcShop.SqlDal.Store
{
    public class MessageDao
    {
        private Database database;
        public MessageDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public bool InsertMessage(MessageBoxInfo messageBoxInfo)
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
            stringBuilder.Append("INSERT INTO [Ecshop_MemberMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) ");
            stringBuilder.Append("VALUES(@ContentId,@Sernder ,@Accepter,@IsRead) ");
            stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
            stringBuilder.Append("INSERT INTO [Ecshop_ManagerMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) ");
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
        public int DeleteMemberMessages(IList<long> messageList)
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("delete from Ecshop_MemberMessageBox where MessageId in ({0}) ", text));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public DbQueryResult GetMemberSendedMessages(MessageBoxQuery query)
        {
            string filter = string.Format("Sernder='{0}'", DataHelper.CleanSearchString(query.Sernder));
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Ecshop_MemberMessageBox", "MessageId", filter, "*");
        }
        public DbQueryResult GetMemberReceivedMessages(MessageBoxQuery query)
        {
            string filter = string.Format("Accepter='{0}'", DataHelper.CleanSearchString(query.Accepter));
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Ecshop_MemberMessageBox", "MessageId", filter, "*");
        }
        public MessageBoxInfo GetMemberMessage(long messageId)
        {
            MessageBoxInfo result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Ecshop_MemberMessageBox WHERE MessageId=@MessageId;");
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
        public bool PostMemberMessageIsRead(long messageId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Ecshop_MemberMessageBox set IsRead=1 where MessageId=@MessageId");
            this.database.AddInParameter(sqlStringCommand, "MessageId", DbType.Int64, messageId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool SendMessage(string subject, string message, string sendto)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @ContentId int; INSERT INTO [Ecshop_MessageContent]([Title],[Content],[Date]) VALUES (@Title,@Content,@Date) SET @ContentId = @@IDENTITY INSERT INTO [Ecshop_MemberMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) VALUES (@ContentId,'admin' ,@Accepter,0)");
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, subject);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, message);
            this.database.AddInParameter(sqlStringCommand, "Date", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, sendto);
            return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
        }
        public int InsertMessageBox(int contentId, string sernder, string accepter, int userId, int messageType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_MemberMessageBox (ContentId,Sernder,Accepter,UserId,MessageType,IsRead) VALUES(@ContentId,@Sernder,@Accepter,@UserId,@MessageType,0); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "ContentId", DbType.Int32, contentId);
            this.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, accepter);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, messageType);
            this.database.AddInParameter(sqlStringCommand, "Sernder", DbType.String, sernder);

            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }
        public int InsertMessageContent(string title, string content, string createTime, int contentType, string code, int actionId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_MessageContent (Title,Content,Date,ContentType,Code,ActionType) VALUES(@Title,@Content,@Date,@ContentType,@Code,@ActionType); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, title);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, content);
            this.database.AddInParameter(sqlStringCommand, "Date", DbType.DateTime, createTime);
            this.database.AddInParameter(sqlStringCommand, "ContentType", DbType.Int32, contentType);
            this.database.AddInParameter(sqlStringCommand, "Code", DbType.String, code);
            this.database.AddInParameter(sqlStringCommand, "ActionType", DbType.Int32, actionId);

            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }
        public int GetUnreadMessageCount(string accepter)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Ecshop_MemberMessageBox WHERE IsRead=0 AND Accepter=@accepter");
            this.database.AddInParameter(sqlStringCommand, "accepter", DbType.String, accepter);

            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }

        public bool DeleteMessage(long messageid, string username)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from Ecshop_MemberMessageBox where MessageId=@MessageId and Accepter=@Accepter");

            this.database.AddInParameter(sqlStringCommand, "MessageId", DbType.Int64, messageid);

            this.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, username);

            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public int SetMemberMessageIsRead(string accepter)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update  Ecshop_MemberMessageBox set IsRead=1 WHERE IsRead=0 AND Accepter=@accepter");
            this.database.AddInParameter(sqlStringCommand, "accepter", DbType.String, accepter);

            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

    }
}

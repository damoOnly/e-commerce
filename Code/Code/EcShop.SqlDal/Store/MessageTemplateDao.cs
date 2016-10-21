using EcShop.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Store
{
	public class MessageTemplateDao
	{
		private Database database;
		public MessageTemplateDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
        public void UpdateSettings(IList<MessageTemplate> templates)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_MessageTemplates SET SendEmail = @SendEmail, SendSMS = @SendSMS, SendInnerMessage = @SendInnerMessage,SendWeixin = @SendWeixin,XinGeSend=@XinGeSend WHERE LOWER(MessageType) = LOWER(@MessageType)");
            this.database.AddInParameter(sqlStringCommand, "SendEmail", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendSMS", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendInnerMessage", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "SendWeixin", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "XinGeSend", DbType.Boolean);

            foreach (MessageTemplate current in templates)
            {
                this.database.SetParameterValue(sqlStringCommand, "SendEmail", current.SendEmail);
                this.database.SetParameterValue(sqlStringCommand, "SendSMS", current.SendSMS);
                this.database.SetParameterValue(sqlStringCommand, "SendInnerMessage", current.SendInnerMessage);
                this.database.SetParameterValue(sqlStringCommand, "MessageType", current.MessageType);
                this.database.SetParameterValue(sqlStringCommand, "SendWeixin", current.SendWeixin);
                this.database.SetParameterValue(sqlStringCommand, "XinGeSend", current.XinGeSend);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }
        public void UpdateTemplate(MessageTemplate template)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_MessageTemplates SET EmailSubject = @EmailSubject, EmailBody = @EmailBody, InnerMessageSubject = @InnerMessageSubject, InnerMessageBody = @InnerMessageBody,WeixinTemplateId=@WeixinTemplateId, SMSBody = @SMSBody,XinGeBody=@XinGeBody, XinGeSubject=@XinGeSubject WHERE LOWER(MessageType) = LOWER(@MessageType)");
            this.database.AddInParameter(sqlStringCommand, "EmailSubject", DbType.String, template.EmailSubject);
            this.database.AddInParameter(sqlStringCommand, "EmailBody", DbType.String, template.EmailBody);
            this.database.AddInParameter(sqlStringCommand, "InnerMessageSubject", DbType.String, template.InnerMessageSubject);
            this.database.AddInParameter(sqlStringCommand, "InnerMessageBody", DbType.String, template.InnerMessageBody);
            this.database.AddInParameter(sqlStringCommand, "SMSBody", DbType.String, template.SMSBody);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String, template.MessageType);
            this.database.AddInParameter(sqlStringCommand, "WeixinTemplateId", DbType.String, template.WeixinTemplateId);
            this.database.AddInParameter(sqlStringCommand, "XinGeBody", DbType.String, template.XinGeBody);
            this.database.AddInParameter(sqlStringCommand, "XinGeSubject", DbType.String, template.XinGeSubject);

            this.database.ExecuteNonQuery(sqlStringCommand);
        }
		public MessageTemplate GetMessageTemplate(string messageType)
		{
			MessageTemplate result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_MessageTemplates WHERE LOWER(MessageType) = LOWER(@MessageType)");
			this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String, messageType);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					result = this.PopulateEmailTempletFromIDataReader(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}
		public IList<MessageTemplate> GetMessageTemplates()
		{
			IList<MessageTemplate> list = new List<MessageTemplate>();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_MessageTemplates");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(this.PopulateEmailTempletFromIDataReader(dataReader));
				}
				dataReader.Close();
			}
			return list;
		}
		public MessageTemplate PopulateEmailTempletFromIDataReader(IDataReader reader)
		{
			MessageTemplate result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				MessageTemplate messageTemplate = new MessageTemplate((string)reader["TagDescription"], (string)reader["Name"])
				{
					MessageType = (string)reader["MessageType"],
					SendInnerMessage = (bool)reader["SendInnerMessage"],
					SendWeixin = (bool)reader["SendWeixin"],
					SendSMS = (bool)reader["SendSMS"],
					SendEmail = (bool)reader["SendEmail"],
					EmailSubject = (string)reader["EmailSubject"],
					EmailBody = (string)reader["EmailBody"],
					InnerMessageSubject = (string)reader["InnerMessageSubject"],
					InnerMessageBody = (string)reader["InnerMessageBody"],
					SMSBody = (string)reader["SMSBody"],
					WeixinTemplateId = (reader["WeixinTemplateId"] != DBNull.Value) ? ((string)reader["WeixinTemplateId"]) : "",
                    XinGeSend = (reader["XinGeSend"] != DBNull.Value) ? (bool)reader["XinGeSend"]:false,
                    XinGeBody = (reader["XinGeBody"] != DBNull.Value) ? ((string)reader["XinGeBody"]) : "",
                    XinGeSubject = (reader["XinGeSubject"] != DBNull.Value) ? ((string)reader["XinGeSubject"]) : ""
				};
				result = messageTemplate;
			}
			return result;
		}
	}
}

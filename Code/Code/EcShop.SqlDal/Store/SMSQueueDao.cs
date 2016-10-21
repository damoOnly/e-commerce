using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;

using EcShop.Core;
using EcShop.Entities.Store;
using System.Text;

namespace EcShop.SqlDal.Store
{
    public class SMSQueueDao
	{
		private Database database;
		public SMSQueueDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
        public bool QueueSMS(SmsInfo message)
		{
			if (message != null)
			{
                StringBuilder sql = new StringBuilder();            
                sql.Append("INSERT INTO Ecshop_SMSQueue(SmsId, Mobile, Subject, Body, Priority, NextTryTime, NumberOfTries,Type) VALUES(@SmsId, @Mobile, @Subject, @Body, @Priority, @NextTryTime, @NumberOfTries,@Type);");
                if (!string.IsNullOrWhiteSpace(message.ClaimCode))
                {
                    // 更改为已提醒
                    sql.Append(" update Ecshop_CouponItems set WarnStatus = 1 where ClaimCode = @claimcode ");
                }
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql.ToString());
				this.database.AddInParameter(sqlStringCommand, "SmsId", DbType.Guid, Guid.NewGuid());
				this.database.AddInParameter(sqlStringCommand, "Mobile", DbType.String, message.Mobile);		
				this.database.AddInParameter(sqlStringCommand, "Subject", DbType.String, message.Subject);
				this.database.AddInParameter(sqlStringCommand, "Body", DbType.String, message.Body);
				this.database.AddInParameter(sqlStringCommand, "Priority", DbType.Int32, (int)message.Priority);
				this.database.AddInParameter(sqlStringCommand, "NextTryTime", DbType.DateTime, DateTime.Parse("1900-1-1 12:00:00"));
				this.database.AddInParameter(sqlStringCommand, "NumberOfTries", DbType.Int32, 0);
                this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, message.type);
                this.database.AddInParameter(sqlStringCommand, "claimcode", DbType.String, message.ClaimCode);
				return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
			}

            return false;
		}
		public Dictionary<Guid, SmsInfo> DequeueSMS()
		{
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_SMSQueue WHERE NextTryTime < GETDATE() ORDER BY Priority DESC");
			Dictionary<Guid, SmsInfo> dictionary = new Dictionary<Guid, SmsInfo>();
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					SmsInfo mailMessage = this.PopulateSMSFromIDataReader(dataReader);
					if (mailMessage != null)
					{
						dictionary.Add((Guid)dataReader["SmsId"], mailMessage);
					}
					else
					{
						this.DeleteQueuedSMS((Guid)dataReader["SmsId"]);
					}
				}
				dataReader.Close();
			}
			return dictionary;
		}
		public SmsInfo PopulateSMSFromIDataReader(IDataReader reader)
		{
			SmsInfo result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				try
				{
					SmsInfo mailMessage = new SmsInfo();
                    if (reader["SmsId"] != DBNull.Value)
                    {
                        mailMessage.SmsId = (Guid)reader["SmsId"];
                    }
                    if (reader["Priority"] != DBNull.Value)
                    {
                        mailMessage.Priority = (int)reader["Priority"];
                    }
					if (reader["Subject"] != DBNull.Value)
					{
						mailMessage.Subject = (string)reader["Subject"];
					}
					if (reader["Mobile"] != DBNull.Value)
					{
						mailMessage.Mobile = (string)reader["Mobile"];
					}
					if (reader["Body"] != DBNull.Value)
					{
						mailMessage.Body = (string)reader["Body"];
					}

                    if (reader["Type"] != DBNull.Value)
                    {
                        mailMessage.type = (int)reader["Type"];
                    }

					result = mailMessage;
				}
				catch
				{
					result = null;
				}
			}

			return result;
		}
		public void DeleteQueuedSMS(Guid SmsId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_SMSQueue WHERE SmsId = @SmsId");
			this.database.AddInParameter(sqlStringCommand, "SmsId", DbType.Guid, SmsId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public void QueueSendingFailure(IList<Guid> list, int failureInterval, int maxNumberOfTries)
		{
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_SMSQueue_Failure");
			this.database.AddInParameter(storedProcCommand, "SmsId", DbType.Guid);
			this.database.AddInParameter(storedProcCommand, "FailureInterval", DbType.Int32, failureInterval);
			this.database.AddInParameter(storedProcCommand, "MaxNumberOfTries", DbType.Int32, maxNumberOfTries);
			foreach (Guid current in list)
			{
				storedProcCommand.Parameters[0].Value = current;
				this.database.ExecuteNonQuery(storedProcCommand);
			}
		}
	}
}

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Store
{
	public class LogDao
	{
		private Database database;
		public LogDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public bool DeleteLog(long logId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_Logs WHERE LogId = @LogId");
			this.database.AddInParameter(sqlStringCommand, "LogId", DbType.Int64, logId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public bool DeleteAllLogs()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("TRUNCATE TABLE Ecshop_Logs");
			bool result;
			try
			{
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			catch (Exception var_1_24)
			{
				result = false;
			}
			return result;
		}
		public int DeleteLogs(string strIds)
		{
			int result;
			if (strIds.Length <= 0)
			{
				result = 0;
			}
			else
			{
				string query = string.Format("DELETE FROM Ecshop_Logs WHERE LogId IN ({0})", strIds);
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			return result;
		}
		public DbQueryResult GetLogs(OperationLogQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pagination page = query.Page;
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat("AddedTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				if (!string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					stringBuilder.Append(" AND");
				}
				stringBuilder.AppendFormat(" AddedTime <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
			}
			if (!string.IsNullOrEmpty(query.OperationUserName))
			{
				if (!string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					stringBuilder.Append(" AND");
				}
				stringBuilder.AppendFormat(" UserName = '{0}'", DataHelper.CleanSearchString(query.OperationUserName));
			}
			return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Ecshop_Logs", "LogId", stringBuilder.ToString(), "*");
		}
		public IList<string> GetOperationUserNames()
		{
			IList<string> list = new List<string>();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DISTINCT UserName FROM Ecshop_Logs");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(dataReader["UserName"].ToString());
				}
			}
			return list;
		}
		public void WriteOperationLogEntry(OperationLogEntry entry)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Ecshop_Logs]([PageUrl],[AddedTime],[UserName],[IPAddress],[Privilege],[Description]) VALUES(@PageUrl,@AddedTime,@UserName,@IPAddress,@Privilege,@Description)");
			this.database.AddInParameter(sqlStringCommand, "PageUrl", DbType.String, entry.PageUrl);
			this.database.AddInParameter(sqlStringCommand, "AddedTime", DbType.DateTime, entry.AddedTime);
			this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, entry.UserName);
			this.database.AddInParameter(sqlStringCommand, "IPAddress", DbType.String, entry.IpAddress);
			this.database.AddInParameter(sqlStringCommand, "Privilege", DbType.Int32, (int)entry.Privilege);
			this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, entry.Description);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

        /// <summary>
        /// 获取当前登入的日志数据
        /// </summary>
        /// <param name="uername"></param>
        /// <param name="logintype"></param>
        public  LoginLog GetLoginLogDetails(string uername, int logintype)
        {
            LoginLog detail = new LoginLog();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Ecshop_LoginLog where MemberName = @username and LogType = @logtype");
            this.database.AddInParameter(sqlStringCommand, "username", DbType.String, uername);
            this.database.AddInParameter(sqlStringCommand, "logtype", DbType.Int32, logintype);
            DataSet dtLoginlog = this.database.ExecuteDataSet(sqlStringCommand);
            if (dtLoginlog != null && dtLoginlog.Tables[0].Rows.Count > 0)
            {
                var table = dtLoginlog.Tables[0];
                detail.MemberName = table.Rows[0]["MemberName"].ToString();
                detail.LogType = !string.IsNullOrWhiteSpace(table.Rows[0]["LogType"].ToString()) ? Int32.Parse(table.Rows[0]["LogType"].ToString()) : -1;
                detail.LoginIP = table.Rows[0]["LoginIP"].ToString();
                detail.ErrorCount = !string.IsNullOrWhiteSpace(table.Rows[0]["ErrorCount"].ToString()) ? Int32.Parse(table.Rows[0]["ErrorCount"].ToString()) : -1;
                detail.ID = !string.IsNullOrWhiteSpace(table.Rows[0]["ID"].ToString()) ? Int32.Parse(table.Rows[0]["ID"].ToString()) : -1;
                if (!string.IsNullOrWhiteSpace(table.Rows[0]["AddTime"].ToString()))
                {
                    detail.AddTime = DateTime.Parse(table.Rows[0]["AddTime"].ToString());
                }

            }
            else
            {
                detail.ID = -1;
            }
            return detail;
        }

        /// <summary>
        /// 更新登入日志
        /// </summary>
        /// <param name="uername"></param>
        /// <param name="logintype"></param>
        public bool UpdateLoginLog(LoginLog detail)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_UpdateLoginLog");
            database.AddInParameter(storedProcCommand, "membername", DbType.String, detail.MemberName);
            database.AddInParameter(storedProcCommand, "logintype", DbType.Int32, detail.LogType);
            database.AddInParameter(storedProcCommand, "loginip", DbType.String, detail.LoginIP);
            database.AddInParameter(storedProcCommand, "type", DbType.Int32, detail.Type);
            return database.ExecuteNonQuery(storedProcCommand) > 0;
        }
	}
}

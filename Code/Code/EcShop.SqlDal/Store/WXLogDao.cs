using EcShop.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EcShop.SqlDal.Store
{
    public class WXLogDao
    {
        private Database database;
        public WXLogDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public bool InsertLog(WXLog log)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Ecshop_WXLog]([AddTime],[UpdateTime],[StartTime],[EndTime],[Type],[Remark],[IsSuccess]) VALUES(@AddTime,@UpdateTime,@StartTime,@EndTime,@Type,@Remark,@IsSuccess)");
            this.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, log.AddTime);
            this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, log.StartTime);
            this.database.AddInParameter(sqlStringCommand, "UpdateTime", DbType.DateTime, log.UpdateTime);
            this.database.AddInParameter(sqlStringCommand, "EndTime", DbType.String, log.EndTime);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.String, log.Type);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, log.Remark);
            this.database.AddInParameter(sqlStringCommand, "IsSuccess", DbType.Boolean, log.IsSuccess);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateLog(WXLog log)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Ecshop_WXLog set  [UpdateTime]=@UpdateTime,[Remark]=@Remark,[IsSuccess]=@IsSuccess Where Id=@Id");
            this.database.AddInParameter(sqlStringCommand, "UpdateTime", DbType.DateTime, log.UpdateTime);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, log.Remark);
            this.database.AddInParameter(sqlStringCommand, "IsSuccess", DbType.Boolean, log.IsSuccess);
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, log.Id);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public WXLog GetWXLog(string strWhere)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("SELECT Top 1 * from Ecshop_WXLog ");
            if(!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" WHERE ");
                strSql.Append(strWhere);
            }
            WXLog log = new WXLog();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    if (System.DBNull.Value != dataReader["Id"])
                    {
                        log.Id = (int)dataReader["Id"];
                    }
                    if (System.DBNull.Value != dataReader["AddTime"])
                    {
                        log.AddTime = DateTime.Parse(dataReader["AddTime"].ToString());
                    }
                    if (System.DBNull.Value != dataReader["UpdateTime"])
                    {
                        log.UpdateTime = DateTime.Parse(dataReader["UpdateTime"].ToString());
                    }
                    if (System.DBNull.Value != dataReader["StartTime"])
                    {
                        log.StartTime = DateTime.Parse(dataReader["StartTime"].ToString());
                    }
                    if (System.DBNull.Value != dataReader["EndTime"])
                    {
                        log.EndTime = DateTime.Parse(dataReader["EndTime"].ToString());
                    }
                    if (System.DBNull.Value != dataReader["Type"])
                    {
                        log.Type = Convert.ToInt32(dataReader["Type"]);
                    }
                    if (System.DBNull.Value != dataReader["Remark"])
                    {
                        log.Remark = dataReader["Remark"].ToString();
                    }
                    else
                    {
                        log.Remark = "";
                    }
                    if (System.DBNull.Value != dataReader["IsSuccess"])
                    {
                        log.IsSuccess = (bool)dataReader["IsSuccess"];
                    }
                }
            }
            return log;
        }
    }
}

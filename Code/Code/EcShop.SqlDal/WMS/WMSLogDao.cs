using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EcShop.SqlDal.WMS
{
    public class WMSLogDao
    {
        private Database database;
        public WMSLogDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

        /// <summary>
        /// 记录WMS日志
        /// </summary>
        /// <param name="method">方法名</param>
        /// <param name="param">参数</param>
        /// <param name="logcontent">日志内容</param>
        /// <param name="logtype">日志类型 info error</param>
        /// <param name="methodtype">方法类型  in out </param>
        /// <returns></returns>
        public bool SaveLog(string method,string param,string logcontent,string logtype,string methodtype)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_WMSlog (method, param, logcontent, createtime, logtype, methodtype) values( @method, @param, @logcontent, @createtime, @logtype, @methodtype)");
            this.database.AddInParameter(sqlStringCommand, "method", DbType.String, method);
            this.database.AddInParameter(sqlStringCommand, "param", DbType.String, param);
            this.database.AddInParameter(sqlStringCommand, "logcontent", DbType.String, logcontent);
            this.database.AddInParameter(sqlStringCommand, "createtime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "logtype", DbType.String, logtype);
            this.database.AddInParameter(sqlStringCommand, "methodtype", DbType.String, methodtype);
            return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
        }
    }
}

using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Promotions
{
    public class TradeDetailsDao
    {
        private Database database;
        public TradeDetailsDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 删除指定时间段的数据信息AliPay or WxPay
        /// </summary>
        /// <param name="beginTime">开起始时间</param>
        /// <param name="endTiem">结束时间</param>
        /// <param name="detailsType">订单类型</param>
        /// <returns></returns>
        public bool DeleteEcshop_TradeDetails(DateTime? beginTime, DateTime? endTiem, string detailsType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete Ecshop_TradeDetails where DetailsType=@DetailsType and TradingTime>=@beginTime and TradingTime<=@endTime");
            this.database.AddInParameter(sqlStringCommand, "DetailsType", DbType.String, detailsType);
            this.database.AddInParameter(sqlStringCommand, "beginTime", DbType.DateTime, beginTime);
            this.database.AddInParameter(sqlStringCommand, "endTime", DbType.DateTime, endTiem);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        /// <summary>
        /// 将datatable批量添加至数据库表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool ExecuteTransactionScopeInsertToEcshop_TradeDetails(DataTable dt, out Exception ex)
        {
            return ExecuteTransactionScopeInsertToEcshop_TradeDetails(dt, database.ConnectionString, out ex);
        }
        /// <summary>
        /// 将datatable批量添加至数据库表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="connectionString"></param>
        /// <param name="mess"></param>
        /// <returns></returns>
        private bool ExecuteTransactionScopeInsertToEcshop_TradeDetails(DataTable dt, string connectionString, out Exception mess)
        {
            mess = null;
            int count = dt.Rows.Count;
            if (count == 0)
            {
                return true;
            }
            string tableName = "Ecshop_TradeDetails";
            int copyTimeout = 200;
            bool flag = false;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        cn.Open();
                        using (SqlBulkCopy sbc = new SqlBulkCopy(cn))
                        {
                            //服务器上目标表的名称     
                            sbc.DestinationTableName = tableName;
                            sbc.BatchSize = count;
                            sbc.BulkCopyTimeout = copyTimeout;
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                //列映射定义数据源中的列和目标表中的列之间的关系     
                                sbc.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                            }
                            sbc.WriteToServer(dt);
                            flag = true;
                            scope.Complete();//有效的事务     
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mess = ex;
                return false;
            }
            return flag;
        }
    }
}

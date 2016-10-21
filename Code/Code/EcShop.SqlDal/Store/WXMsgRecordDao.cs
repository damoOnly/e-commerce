using EcShop.Core.ErrorLog;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EcShop.SqlDal.Store
{
    public class WXMsgRecordDao
    {
        private Database database;
        public WXMsgRecordDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public bool BathAddWXMsgRecord(DataTable dtMsgRecord)
        {
            bool success = true;
            SqlBulkCopy bcp = new SqlBulkCopy(this.database.ConnectionString);
            bcp.DestinationTableName = "[Ecshop_WXMsgRecord]";
            bcp.ColumnMappings.Add("Id", "Id");
            bcp.ColumnMappings.Add("OpenId", "OpenId");
            bcp.ColumnMappings.Add("OperCode", "OperCode");
            bcp.ColumnMappings.Add("Text", "Text");
            bcp.ColumnMappings.Add("Time", "Time");
            bcp.ColumnMappings.Add("HappenDate", "HappenDate");
            bcp.ColumnMappings.Add("Worker", "Worker");
            bcp.ColumnMappings.Add("WorkerNo", "WorkerNo");
            bcp.ColumnMappings.Add("HappenMonth", "HappenMonth");
            try
            {
                bcp.WriteToServer(dtMsgRecord);
            }
            catch (Exception ex)
            {
                success = false;
                ErrorLog.Write(ex.ToString());
            }
            finally
            {
                bcp.Close();
            }
            return success;
        }
    }
}

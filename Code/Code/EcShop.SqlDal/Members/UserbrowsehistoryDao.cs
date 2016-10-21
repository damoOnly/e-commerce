using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EcShop.SqlDal.Members
{
    public class UserbrowsehistoryDao
    {
        private Database database;
        public UserbrowsehistoryDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public DataTable GetUserBrowseHistorys()
        {
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select HistoryId,ProductId,UserId,UserName,UserIP,[Description],Url from Ecshop_UserBrowseHistory  order by Sort  desc");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public UserbrowsehistoryInfo GetUserBrowseHistory(int historyId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_UserBrowseHistory WHERE HistoryId = @HistoryId");
            this.database.AddInParameter(sqlStringCommand, "HistoryId", DbType.Int32, historyId);
            UserbrowsehistoryInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<UserbrowsehistoryInfo>(dataReader);
            }
            return result;
        }


        public bool DeleteUserBrowseHistory(int historyId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE  FROM Ecshop_UserBrowseHistory WHERE HistoryId=@HistoryId;");
            this.database.AddInParameter(sqlStringCommand, "HistoryId", DbType.Int32, historyId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public DbQueryResult GetBrowseHistory(UserbrowsehistoryQuery query)
        {
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Ecshop_UserBrowseHistory", "Sort", string.IsNullOrEmpty(query.UserName) ? string.Empty : string.Format("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName)), "*");
        }

        public DataTable GetUserBrowseHistory(UserbrowsehistoryInfo browserhistory)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_UserBrowseHistoryAdd");
            this.database.AddInParameter(storedProcCommand, "PlatType", DbType.Int32, browserhistory.PlatType);
            this.database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, browserhistory.ProductId);
            this.database.AddInParameter(storedProcCommand, "Url", DbType.String, browserhistory.Url);
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, browserhistory.UserName);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, browserhistory.UserId);
            this.database.AddInParameter(storedProcCommand, "UserIP", DbType.String, browserhistory.UserIP);
            this.database.AddInParameter(storedProcCommand, "IP", DbType.Int64, browserhistory.Ip);
            
            //return this.database.ExecuteNonQuery(storedProcCommand) == 1;返回bool类型

            DataTable historyinfo = null;
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                historyinfo = DataHelper.ConverDataReaderToDataTable(dataReader);
            }

            return historyinfo;
        }

        public void BulkUserBrowserHistory(DataTable browserHistory)
        {
            if (browserHistory != null)
            {
                if (browserHistory.Rows.Count > 0)
                {
                    try
                    {
                        using (SqlBulkCopy bcp = new SqlBulkCopy(this.database.ConnectionString))
                        {
                            bcp.DestinationTableName = "Ecshop_UserBrowseHistory";

                            bcp.ColumnMappings.Add("ProductId", "ProductId");
                            bcp.ColumnMappings.Add("UserId", "UserId");
                            bcp.ColumnMappings.Add("UserName", "UserName");
                            bcp.ColumnMappings.Add("BrowseTime", "BrowseTime");
                            bcp.ColumnMappings.Add("UserIP", "UserIP");
                            bcp.ColumnMappings.Add("Description", "Description");
                            bcp.ColumnMappings.Add("Sort", "Sort");
                            bcp.ColumnMappings.Add("Url", "Url");
                            bcp.ColumnMappings.Add("BrowerTimes", "BrowerTimes");
                            bcp.ColumnMappings.Add("PlatType", "PlatType");
                            bcp.ColumnMappings.Add("IP", "IP");
                            bcp.ColumnMappings.Add("CategoryId", "CategoryId");

                            bcp.WriteToServer(browserHistory);
                            bcp.Close();
                        }
                    }
                    catch
                    { }
                }
            }
        }
    }
}

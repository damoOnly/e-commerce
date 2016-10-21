using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Store
{
    public class HotkeywordDao
    {
        private Database database;
        public HotkeywordDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public void AddHotkeywords(int categoryId, string Keywords)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Hotkeywords_Log");
            this.database.AddInParameter(storedProcCommand, "Keywords", DbType.String, Keywords);
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(storedProcCommand, "SearchTime", DbType.DateTime, DateTime.Now);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        /// <summary>
        /// 添加关键字
        /// </summary>
        /// <param name="categoryId">热搜类别</param>
        /// <param name="Keywords">热搜字</param>
        /// <param name="clientType">终端</param>
        public void AddHotkeywords(int? categoryId, string Keywords, ClientType clientType, int SupplierId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Hotkeywords_LogNew");
            this.database.AddInParameter(storedProcCommand, "Keywords", DbType.String, Keywords);
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(storedProcCommand, "SearchTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(storedProcCommand, "ClientType", DbType.Int32, (int)clientType);
            this.database.AddInParameter(storedProcCommand, "SupplierId", DbType.Int32, SupplierId);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        public string GetHotkeyword(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Keywords FROM Ecshop_Hotkeywords WHERE Hid=@Hid");
            this.database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, id);
            return this.database.ExecuteScalar(sqlStringCommand).ToString();
        }
        public DataTable GetHotKeywords()
        {
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *,(SELECT Name FROM Ecshop_Categories WHERE CategoryId = h.CategoryId) AS CategoryName FROM Ecshop_Hotkeywords h ORDER BY Frequency DESC");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 获取某个终端的所有搜索关键字
        /// </summary>
        /// <param name="clientType"></param>
        /// <returns></returns>
        public DataTable GetHotKeywords(ClientType clientType)
        {
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *,(SELECT Name FROM Ecshop_Categories WHERE CategoryId = h.CategoryId) AS CategoryName FROM Ecshop_Hotkeywords h where h.ClientType=@ClientType ORDER BY Frequency DESC");
            this.database.AddInParameter(sqlStringCommand, "ClientType", DbType.Int32, (int)clientType);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        /// <summary>
        /// 获取某个终端的所有搜索关键字
        /// </summary>
        /// <param name="clientType"></param>
        /// <returns></returns>
        public DataTable GetHotKeywords(ClientType clientType, int categoryId, int hotKeywordsNum, int supplierId)
        {
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *,(SELECT Name FROM Ecshop_Categories WHERE CategoryId = h.CategoryId) AS CategoryName FROM Ecshop_Hotkeywords h where h.ClientType=@ClientType and h.SupplierId=@supplierId ORDER BY Frequency DESC");
            this.database.AddInParameter(sqlStringCommand, "ClientType", DbType.Int32, (int)clientType);
            this.database.AddInParameter(sqlStringCommand, "supplierId", DbType.Int32, supplierId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        public DbQueryResult GetHotKeywords(ClientType clientType, Pagination page)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1");

            stringBuilder.AppendFormat(" AND ClientType = '{0}'", (int)clientType);


            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Ecshop_Hotkeywords", "Hid", stringBuilder.ToString(), "*");
        }


        public void DeleteHotKeywords(int hId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" Delete FROM Ecshop_Hotkeywords Where Hid =@Hid");
            this.database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, hId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public void SwapHotWordsSequence(int hid, int replaceHid, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Ecshop_Hotkeywords", "Hid", "Frequency", hid, replaceHid, displaySequence, replaceDisplaySequence);
        }
        public void UpdateHotWords(int hid, int? categoryId, string hotKeyWords, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Ecshop_Hotkeywords Set CategoryId = @CategoryId, Keywords =@Keywords,supplierId=@supplierId Where Hid =@Hid");
            this.database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, hid);
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(sqlStringCommand, "Keywords", DbType.String, hotKeyWords);
            this.database.AddInParameter(sqlStringCommand, "supplierId", DbType.Int32, supplierId);
            
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public DataTable GetHotKeywords(int? categoryId, int hotKeywordsNum)
        {
            DataTable result = null;
            string text = string.Format("SELECT TOP {0} * FROM Ecshop_Hotkeywords", hotKeywordsNum);
            if (categoryId != 0)
            {
                text += string.Format(" WHERE CategoryId = {0}", categoryId);
            }
            text += " ORDER BY Frequency DESC";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public DataTable GetHotKeywords(int? categoryId, int hotKeywordsNum, ClientType clientType)
        {
            DataTable result = null;
            string text = string.Format("SELECT TOP {0} * FROM Ecshop_Hotkeywords", hotKeywordsNum);
            text += string.Format(" where 1=1 ");
            if (categoryId != 0)
            {
                text += string.Format(" and CategoryId = {0}", categoryId);
            }

            text += string.Format(" and ClientType={0} ", (int)clientType);
            text += " ORDER BY Frequency DESC";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public DataSet GetAllHotKeywords()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CategoryId, Name AS CategoryName, RewriteName FROM Ecshop_Categories WHERE Depth = 1 ORDER BY DisplaySequence; SELECT * FROM Ecshop_Hotkeywords ORDER BY Frequency DESC");
            DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
            dataSet.Relations.Add("relation", dataSet.Tables[0].Columns["CategoryId"], dataSet.Tables[1].Columns["CategoryId"], false);
            return dataSet;
        }

        public DataTable GetHotKeywords(int clientType, int hotKeywordsNum, bool IsRandom)
        {
            DataTable result = null;
            string sql = string.Format("SELECT TOP {0} * FROM Ecshop_Hotkeywords", hotKeywordsNum);
            sql += string.Format(" WHERE ClientType = {0} OR ClientType = 0", clientType);
            if (IsRandom)
            {
                sql += " ORDER BY NEWID()";
            }
            else
            {
                sql += " ORDER BY Frequency DESC";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public DataTable GetMyHotKeywords(int userId, int hotKeywordsNum, bool IsRandom)
        {
            // TODO
            DataTable result = null;
            string sql = string.Format("SELECT TOP {0} * FROM Ecshop_Hotkeywords", hotKeywordsNum);
            //sql += string.Format(" WHERE ClientType = {0} OR ClientType = 0", clientType);
            if (IsRandom)
            {
                sql += " ORDER BY NEWID()";
            }
            else
            {
                sql += " ORDER BY Frequency DESC";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
    }
}

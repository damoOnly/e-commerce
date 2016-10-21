using EcShop.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Store
{
    public class SearchHistoryDao
    {
        private Database database;
        public SearchHistoryDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }

        public bool NewSearchHistory(string searchword, int userId, ClientType clientType)
        {
            if (userId > 0)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Select count(1) from Ecshop_SearchHistory where UserId=@UserId and Keywords=@Keywords and ClientType=@ClientType");
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
                this.database.AddInParameter(sqlStringCommand, "Keywords", DbType.String, searchword);
                this.database.AddInParameter(sqlStringCommand, "ClientType", DbType.Int32, (int)clientType);
                //如果是第一次搜索，则插入数据
                if(int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString())<=0)
                {
                    sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_SearchHistory(UserId, Keywords, SearchTime, Frequency,ClientType)VALUES(@UserId, @Keywords,@SearchTime,1,@ClientType)");
                    this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
                    this.database.AddInParameter(sqlStringCommand, "Keywords", DbType.String, searchword);
                    this.database.AddInParameter(sqlStringCommand, "ClientType", DbType.Int32, (int)clientType);
                    this.database.AddInParameter(sqlStringCommand, "SearchTime", DbType.DateTime, DateTime.Now);
                    return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
                }
                //如果是再次搜索，则更新搜索时间和搜索次数
                else
                {
                    sqlStringCommand = this.database.GetSqlStringCommand("Update Ecshop_SearchHistory set Frequency=Frequency+1,SearchTime=@SearchTime where UserId=@UserId and Keywords=@Keywords and ClientType=@ClientType");
                    this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
                    this.database.AddInParameter(sqlStringCommand, "Keywords", DbType.String, searchword);
                    this.database.AddInParameter(sqlStringCommand, "ClientType", DbType.Int32, (int)clientType);
                    this.database.AddInParameter(sqlStringCommand, "SearchTime", DbType.DateTime, DateTime.Now);
                    return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
                }
            
            }
            else
            {
                return false;
            }
        }


        public DataTable GetSearchHistory(int userId, ClientType clientType, int num)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("select top {0} * from Ecshop_SearchHistory where UserId=@UserId and ClientType=@ClientType order by SearchTime desc", num);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ClientType", DbType.Int32, (int)clientType);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }


        public int DeleteSearchHistory(int userId, ClientType clientType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Delete from Ecshop_SearchHistory where UserId=@UserId and ClientType=@ClientType");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ClientType", DbType.Int32, (int)clientType);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

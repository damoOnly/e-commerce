using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace  EcShop.SqlDal.Commodities
{
    public class BaseCountryDao
    {
        private Database database;
        public BaseCountryDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 获取国家
        /// </summary>
        /// <returns></returns>
        public DataTable GetBaseCountry()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM BaseCountry");
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 获取国家
        /// </summary>
        /// <returns></returns>
        public DataTable GetBaseCountryByName(string Name)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT HSCode,CName label,Ename,ZH FROM BaseCountry where CName like '%" + DataHelper.CleanSearchString(Name) + "%'");
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        
    }
}

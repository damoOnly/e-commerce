using EcShop.Entities;
using EcShop.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace EcShop.SqlDal.Store
{
	public class StoreDao
	{
		private Database database;
        public StoreDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

        public StoreInfo GetStoreByStoreId(int storeId)
        {
            StoreInfo result = new StoreInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_StoreManagement WHERE StoreId=@StoreId");
            this.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    result = DataMapper.PopulateStoreInfo(dataReader);
                }
            }
            return result;
        }
		
	}
}

using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EcShop.SqlDal.Sales
{
    public class BaseEnumDictDao
    {
        private Database database;
        public BaseEnumDictDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

        /// <summary>
        /// 获取对应类型的字典
        /// </summary>
        /// <param name="dictType"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetBaseEnumDictItems(string dictType)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select DictValue,DictName from BaseEnumDict WHERE DictType = @DictType");
            this.database.AddInParameter(sqlStringCommand, "DictType", DbType.String, dictType);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    string value = (dataReader["DictName"] == DBNull.Value) ? "0" : ((string)dataReader["DictName"]);
                    dictionary.Add((string)dataReader["DictValue"], value);
                }
            }
            return dictionary;
        }
    }
}

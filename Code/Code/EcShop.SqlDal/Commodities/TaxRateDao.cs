using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Comments;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Commodities
{
    public class TaxRateDao
    {
        private Database database;
        public TaxRateDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public DataTable GetTaxRate()
        {
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Ecshop_TaxRate");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public string GetTaxRate(int TaxId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT TaxRate  FROM  Ecshop_TaxRate WHERE TaxId = {0}", TaxId));
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            string result;
            if (obj != null)
            {
                result = obj.ToString();
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }
        public int AddTaxRate(decimal TaxRate, string code, string CodeDescription)
        {
            int result = 0;
            //修改添加行邮编码支持
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_TaxRate VALUES(@TaxRate,@PersonalPostalArticlesCode,@CodeDescription);SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "TaxRate", DbType.Decimal, TaxRate);
            this.database.AddInParameter(sqlStringCommand, "PersonalPostalArticlesCode", DbType.String, code);
            this.database.AddInParameter(sqlStringCommand, "CodeDescription", DbType.String, CodeDescription);
            
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj != null)
            {
                result = Convert.ToInt32(obj.ToString());
            }
            return result;
        }
        public bool UpdateTaxRate(int TaxId, decimal TaxRate, string code,string CodeDescription)
        {
            //修改添加行邮编码支持
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_TaxRate SET TaxRate=@TaxRate,PersonalPostalArticlesCode=@PersonalPostalArticlesCode,CodeDescription=@CodeDescription WHERE TaxId=@TaxId");
            this.database.AddInParameter(sqlStringCommand, "TaxRate", DbType.Decimal, TaxRate);
            this.database.AddInParameter(sqlStringCommand, "PersonalPostalArticlesCode", DbType.String, code);
            this.database.AddInParameter(sqlStringCommand, "CodeDescription", DbType.String, CodeDescription);
            this.database.AddInParameter(sqlStringCommand, "TaxId", DbType.Int32, TaxId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool DeleteTaxRate(int TaxId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_TaxRate WHERE TaxId=@TaxId;");
            this.database.AddInParameter(sqlStringCommand, "TaxId", DbType.Int32, TaxId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public int GetTaxRate(int TaxId, string code)
        {
            int result = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TaxId  FROM  Ecshop_TaxRate WHERE PersonalPostalArticlesCode=@PersonalPostalArticlesCode and TaxId<>@TaxId");
            this.database.AddInParameter(sqlStringCommand, "TaxId", DbType.Int32, TaxId);
            this.database.AddInParameter(sqlStringCommand, "PersonalPostalArticlesCode", DbType.String, code);
            IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
            if (dataReader.Read())
            {
                result = Convert.ToInt32(dataReader["TaxId"].ToString());
            }
            return result;
        }

        public DataTable GetTaxRate(string taxRate)
        {
            string text = "1=1";
            if (!string.IsNullOrEmpty(taxRate))
            {
                text = text + " AND (TaxRate LIKE '%" + DataHelper.CleanSearchString(taxRate) + "%' or PersonalPostalArticlesCode LIKE '%" + DataHelper.CleanSearchString(taxRate) + "%' or CodeDescription LIKE '%" + DataHelper.CleanSearchString(taxRate) + "%')";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_TaxRate  WHERE " + text + " ORDER BY TaxId");
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        public IList<TaxRateInfo> GetMainTaxRate()
        {
            IList<TaxRateInfo> list = new List<TaxRateInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * From Ecshop_TaxRate ORDER BY TaxId");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    TaxRateInfo item = DataMapper.PopulateTaxRate(dataReader);
                    list.Add(item);
                }
            }
            return list;
        }

        public IList<TaxRateInfo> GetMainTaxRate(int categoryId)
        {
            IList<TaxRateInfo> list = new List<TaxRateInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select isnull(b.TaxId,0) TaxId,isnull(b.TaxRate,0) TaxRate from Ecshop_Categories a left join Ecshop_TaxRate b on a.TaxRateId = b.TaxId where CategoryId = " + categoryId + " order by b.TaxId");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    TaxRateInfo item = DataMapper.PopulateTaxRate(dataReader);
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// 根据行邮编码获取数据
        /// </summary>
        /// <param name="Code">行邮编码</param>
        /// <returns></returns>
        public DataTable GetTaxRateBuyCode(string Code)
        {
            string text = " 1=1";
            if (!string.IsNullOrEmpty(Code))
            {
                text = text + " AND PersonalPostalArticlesCode LIKE '%" + DataHelper.CleanSearchString(Code) + "%'";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TaxId,TaxRate,PersonalPostalArticlesCode label FROM Ecshop_TaxRate  WHERE " + text + " ORDER BY TaxId");
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
    }
}

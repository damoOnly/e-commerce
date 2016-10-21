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

namespace EcShop.SqlDal.Commodities
{
   public class BrandTagDao
    {
       private Database database;
       public BrandTagDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
       public int AddBrandTag(BrandTagInfo model)
		{
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(TagSort) IS NULL THEN 1 ELSE MAX(TagSort) + 1 END) FROM Ecshop_BrandTag;INSERT INTO dbo.Ecshop_BrandTag( TagName, TagSort )VALUES(@TagName,@DisplaySequence); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, model.TagName);  
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public void AddBrandBrandTags(int brandTagId, IList<int> brandIds)
		{
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_BrandToBrandTag(BrandTagId,BrandId) VALUES(@BrandTagId,@BrandId)");
            this.database.AddInParameter(sqlStringCommand, "BrandTagId", DbType.Int32, brandTagId);
			this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32);
            foreach (int current in brandIds)
			{
                this.database.SetParameterValue(sqlStringCommand, "BrandId", current);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}
		public bool DeleteBrandBrandTags(int brandId)
		{
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_BrandToBrandTag WHERE BrandId=@BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
			bool result;
			try
			{
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public DataTable GetBrandTags(string brandTagName)
		{
			string text = "1=1";
            if (!string.IsNullOrEmpty(brandTagName))
			{
                text = text + " AND TagName LIKE '%" + DataHelper.CleanSearchString(brandTagName) + "%'";
			}
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_BrandTag  WHERE " + text + " ORDER BY TagSort");
			DataTable result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public DataTable GetBrandTags()
		{
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_BrandTag ORDER BY TagSort");
			DataTable result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
      
        public BrandTagInfo GetBrandTag(int brandTagId)
        {
            BrandTagInfo brandCategoryInfo = new BrandTagInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_BrandTag WHERE BrandTagId = @BrandId;SELECT * FROM Ecshop_BrandToBrandTag WHERE BrandTagId = @BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandTagId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                brandCategoryInfo = ReaderConvert.ReaderToModel<BrandTagInfo>(dataReader);
                IList<int> list = new List<int>();
                dataReader.NextResult();
                while (dataReader.Read())
                {
                    list.Add((int)dataReader["BrandId"]);
                }
                brandCategoryInfo.BrandCategoryInfos = list;
            }
            return brandCategoryInfo;
        }
        public bool BrandTagHaveBrand(int brandTagId)
        {
            bool result = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(BrandTagId) FROM Ecshop_BrandToBrandTag Where BrandTagId=@brandTagId");
            this.database.AddInParameter(sqlStringCommand, "brandTagId", DbType.Int32, brandTagId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    result = (dataReader.GetInt32(0) > 0);
                }
            }
            return result;
        }
        public bool DeleteBrandTag(int brandTagId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_BrandTag WHERE BrandTagId = @BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandTagId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
       
        public bool UpdateBrandCategoryDisplaySequence(int brandTagId, int displaysequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_BrandTag set TagSort=@DisplaySequence where BrandTagId=@brandTagId");
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            this.database.AddInParameter(sqlStringCommand, "@brandTagId", DbType.Int32, brandTagId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateBrandTag(BrandTagInfo brandCategory)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_BrandTag SET TagName = @TagName WHERE BrandTagId = @BrandTagId");
            this.database.AddInParameter(sqlStringCommand, "BrandTagId", DbType.Int32, brandCategory.BrandTagId);
            this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, brandCategory.TagName);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool DeleteBrandTagBrand(int brandId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_BrandToBrandTag WHERE BrandTagId=@BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            bool result;
            try
            {
                this.database.ExecuteNonQuery(sqlStringCommand);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public void AddBrandAndBrandTags(int brandTagId, IList<int> brandTypes)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_BrandToBrandTag(BrandTagId,BrandId) VALUES(@BrandTagId,@BrandId)");
            this.database.AddInParameter(sqlStringCommand, "BrandTagId", DbType.Int32, brandTagId);
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32);
            foreach (int current in brandTypes)
            {
                this.database.SetParameterValue(sqlStringCommand, "BrandId", current);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }
        public DataTable GetDefaultBrand(IList<int> brandIds)
        {
            DataTable result;
            if (brandIds.Count == 0)
            {
                result = null;
            }
            else
            {
                string productId = string.Join(",", brandIds.ToArray());
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT BrandId,BrandName,Logo FROM dbo.Ecshop_BrandCategories WHERE BrandId IN({0}) ", productId));
                DataTable dataTable;
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                result = dataTable;
            }
            return result;
        }
        public DataTable GetVistiedBrandList(int count, int minNum, int maxNum,int brandTagId)
        {
            DataTable result;
            DbCommand command = this.database.GetStoredProcCommand("cp_Index_SuggestBrandPage_Get");
            this.database.AddInParameter(command, "TopNum", DbType.Int32, count);
            this.database.AddInParameter(command, "MinNum", DbType.Int32, minNum);
            this.database.AddInParameter(command, "MaxNum", DbType.Int32, maxNum);
            this.database.AddInParameter(command, "BrandTagId", DbType.Int32, brandTagId);

            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }

            return result;          
        }
        public DataTable GetVistiedBrandTagList()
        {
            DataTable result;
         
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 6 BrandTagId,TagName FROM dbo.Ecshop_BrandTag ORDER BY TagSort");
                DataTable dataTable;
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                result = dataTable;          
            return result;
        }
    }
}

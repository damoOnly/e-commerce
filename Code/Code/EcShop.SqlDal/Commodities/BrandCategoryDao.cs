using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Commodities
{
	public class BrandCategoryDao
	{
		private Database database;
		public BrandCategoryDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public int AddBrandCategory(BrandCategoryInfo brandCategory)
		{
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Ecshop_BrandCategories;INSERT INTO Ecshop_BrandCategories(BrandName, Logo, BigLogo,CompanyUrl,RewriteName,MetaKeywords,MetaDescription, Description, DisplaySequence) VALUES(@BrandName, @Logo,@BigLogo, @CompanyUrl,@RewriteName,@MetaKeywords,@MetaDescription, @Description, @DisplaySequence); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "BrandName", DbType.String, brandCategory.BrandName);
			this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, brandCategory.Logo);
            this.database.AddInParameter(sqlStringCommand, "BigLogo", DbType.String, brandCategory.BigLogo);
			this.database.AddInParameter(sqlStringCommand, "CompanyUrl", DbType.String, brandCategory.CompanyUrl);
			this.database.AddInParameter(sqlStringCommand, "RewriteName", DbType.String, brandCategory.RewriteName);
			this.database.AddInParameter(sqlStringCommand, "MetaKeywords", DbType.String, brandCategory.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "MetaDescription", DbType.String, brandCategory.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, brandCategory.Description);
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
		public void AddBrandProductTypes(int brandId, IList<int> productTypes)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
			foreach (int current in productTypes)
			{
				this.database.SetParameterValue(sqlStringCommand, "ProductTypeId", current);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}
		public bool DeleteBrandProductTypes(int brandId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_ProductTypeBrands WHERE BrandId=@BrandId");
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
		public DataTable GetBrandCategories(string brandName)
		{
			string text = "1=1";
			if (!string.IsNullOrEmpty(brandName))
			{
				text = text + " AND BrandName LIKE '%" + DataHelper.CleanSearchString(brandName) + "%'";
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_BrandCategories  WHERE " + text + " ORDER BY DisplaySequence");
			DataTable result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public DataTable GetBrandCategories()
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_BrandCategories ORDER BY DisplaySequence");
			DataTable result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
        public DataTable GetBrandCategories(int categoryId)
		{
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT a.* FROM Ecshop_BrandCategories a JOIN Ecshop_ProductTypeBrands b ON b.[BrandId] = a.BrandId JOIN [Ecshop_Categories] c ON c.[AssociatedProductType] = b.ProductTypeId WHERE c.CategoryId = @CategoryId ORDER BY a.DisplaySequence");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);

			DataTable result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public BrandCategoryInfo GetBrandCategory(int brandId)
		{
			BrandCategoryInfo brandCategoryInfo = new BrandCategoryInfo();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_BrandCategories WHERE BrandId = @BrandId;SELECT * FROM Ecshop_ProductTypeBrands WHERE BrandId = @BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				brandCategoryInfo = ReaderConvert.ReaderToModel<BrandCategoryInfo>(dataReader);
				IList<int> list = new List<int>();
				dataReader.NextResult();
				while (dataReader.Read())
				{
					list.Add((int)dataReader["ProductTypeId"]);
				}
                if (brandCategoryInfo != null)
                {
                    brandCategoryInfo.ProductTypes = list;
                }
			}
			return brandCategoryInfo;
		}
		public bool UpdateBrandCategory(BrandCategoryInfo brandCategory)
		{
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_BrandCategories SET BrandName = @BrandName, Logo = @Logo,BigLogo=@BigLogo, CompanyUrl = @CompanyUrl,RewriteName=@RewriteName,MetaKeywords=@MetaKeywords,MetaDescription=@MetaDescription, Description = @Description WHERE BrandId = @BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandCategory.BrandId);
			this.database.AddInParameter(sqlStringCommand, "BrandName", DbType.String, brandCategory.BrandName);
			this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, brandCategory.Logo);
            this.database.AddInParameter(sqlStringCommand, "BigLogo", DbType.String, brandCategory.BigLogo);
			this.database.AddInParameter(sqlStringCommand, "CompanyUrl", DbType.String, brandCategory.CompanyUrl);
			this.database.AddInParameter(sqlStringCommand, "RewriteName", DbType.String, brandCategory.RewriteName);
			this.database.AddInParameter(sqlStringCommand, "MetaKeywords", DbType.String, brandCategory.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "MetaDescription", DbType.String, brandCategory.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, brandCategory.Description);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool BrandHvaeProducts(int brandId)
		{
			bool result = false;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(ProductName) FROM Ecshop_Products Where BrandId=@BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = (dataReader.GetInt32(0) > 0);
				}
			}
			return result;
		}
		public bool DeleteBrandCategory(int brandId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_BrandCategories WHERE BrandId = @BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public void UpdateBrandCategoryDisplaySequence(int brandId, SortAction action)
		{
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_BrandCategory_DisplaySequence");
			this.database.AddInParameter(storedProcCommand, "BrandId", DbType.Int32, brandId);
			this.database.AddInParameter(storedProcCommand, "Sort", DbType.Int32, action);
			this.database.ExecuteNonQuery(storedProcCommand);
		}
		public bool UpdateBrandCategoryDisplaySequence(int brandId, int displaysequence)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_BrandCategories set DisplaySequence=@DisplaySequence where BrandId=@BrandId");
			this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
			this.database.AddInParameter(sqlStringCommand, "@BrandId", DbType.Int32, brandId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool SetBrandCategoryThemes(int brandid, string themeName)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_BrandCategories set Theme = @Theme where BrandId = @BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandid);
			this.database.AddInParameter(sqlStringCommand, "Theme", DbType.String, themeName);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public DataTable GetBrandCategories(int categoryId, int maxNum)
		{
			StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT TOP {0} BrandId, BrandName, Logo,BigLogo, RewriteName FROM Ecshop_BrandCategories where Logo <> '' and Logo is not null", maxNum);
			CategoryInfo category = new CategoryDao().GetCategory(categoryId);
			if (category != null)
			{
				stringBuilder.AppendFormat("  and BrandId IN (SELECT BrandId FROM Ecshop_Products WHERE MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%')", category.Path);
			}
			stringBuilder.Append(" ORDER BY DisplaySequence DESC");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			DataTable result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

        public DataTable GetBindList(int categoryId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT Ecshop_BrandCategories.* FROM Ecshop_BrandCategories where Logo <> '' and Logo is not null ");
            CategoryInfo category = new CategoryDao().GetCategory(categoryId);
            if (category != null)
            {
                stringBuilder.AppendFormat(" and BrandId IN (SELECT BrandId FROM Ecshop_Products WHERE MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%')", category.Path);
            }
            stringBuilder.Append(" ORDER BY DisplaySequence DESC");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
	}
}

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.Commodities
{
	public class ProductTypeDao
	{
		private Database database;
		public ProductTypeDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DbQueryResult GetProductTypes(ProductTypeQuery query)
		{
			return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Ecshop_ProductTypes", "TypeId", string.IsNullOrEmpty(query.TypeName) ? string.Empty : string.Format("TypeName LIKE '%{0}%'", DataHelper.CleanSearchString(query.TypeName)), "*");
		}
		public IList<ProductTypeInfo> GetProductTypes()
		{
			IList<ProductTypeInfo> result = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_ProductTypes");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ProductTypeInfo>(dataReader);
			}
			return result;
		}
		public ProductTypeInfo GetProductType(int typeId)
		{
			ProductTypeInfo productTypeInfo = null;
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_ProductTypes WHERE TypeId = @TypeId;SELECT * FROM Ecshop_ProductTypeBrands WHERE ProductTypeId = @TypeId");
			this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				productTypeInfo = ReaderConvert.ReaderToModel<ProductTypeInfo>(dataReader);
				dataReader.NextResult();
				while (dataReader.Read())
				{
					productTypeInfo.Brands.Add((int)dataReader["BrandId"]);
				}
			}
			return productTypeInfo;
		}
		public DataTable GetBrandCategoriesByTypeId(int typeId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT B.BrandId,B.BrandName FROM Ecshop_BrandCategories B INNER JOIN Ecshop_ProductTypeBrands PB ON B.BrandId=PB.BrandId WHERE PB.ProductTypeId=@ProductTypeId ORDER BY B.DisplaySequence DESC");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
			DataTable result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public int GetTypeId(string typeName)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TypeId FROM Ecshop_ProductTypes where TypeName = @TypeName");
			this.database.AddInParameter(sqlStringCommand, "TypeName", DbType.String, typeName);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public int AddProductType(ProductTypeInfo productType)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_ProductTypes(TypeName, Remark) VALUES (@TypeName, @Remark); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "TypeName", DbType.String, productType.TypeName);
			this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, productType.Remark);
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
		public void AddProductTypeBrands(int typeId, IList<int> brands)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
			this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32);
			foreach (int current in brands)
			{
				this.database.SetParameterValue(sqlStringCommand, "BrandId", current);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}
		public bool UpdateProductType(ProductTypeInfo productType)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_ProductTypes SET TypeName = @TypeName, Remark = @Remark WHERE TypeId = @TypeId");
			this.database.AddInParameter(sqlStringCommand, "TypeName", DbType.String, productType.TypeName);
			this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, productType.Remark);
			this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, productType.TypeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool DeleteProductTypeBrands(int typeId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_ProductTypeBrands WHERE ProductTypeId=@ProductTypeId");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
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
		public bool DeleteProducType(int typeId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_ProductTypes WHERE TypeId = @TypeId AND not exists (SELECT productId FROM Ecshop_Products WHERE TypeId = @TypeId)");
			this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}

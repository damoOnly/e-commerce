using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.HOP;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Commodities
{
	public class TaobaoProductDao
	{
		private Database database;
		public TaobaoProductDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DataSet GetTaobaoProductDetails(int productId)
		{
			DataSet dataSet = new DataSet();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId, HasSKU, ProductName, ProductCode, MarketPrice, (SELECT [Name] FROM Ecshop_Categories WHERE CategoryId = p.CategoryId) AS CategoryName, (SELECT BrandName FROM Ecshop_BrandCategories WHERE BrandId = p.BrandId) AS BrandName, (SELECT MIN(SalePrice) FROM Ecshop_SKUs WHERE ProductId = p.ProductId) AS SalePrice, (SELECT MIN(CostPrice) FROM Ecshop_SKUs WHERE ProductId = p.ProductId) AS CostPrice, (SELECT SUM(Stock) FROM Ecshop_SKUs WHERE ProductId = p.ProductId) AS Stock FROM Ecshop_Products p WHERE ProductId = @ProductId SELECT AttributeName, ValueStr FROM Ecshop_ProductAttributes pa join Ecshop_Attributes a ON pa.AttributeId = a.AttributeId JOIN Ecshop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC SELECT Weight AS '重量', Stock AS '库存', CostPrice AS '成本价', SalePrice AS '一口价', SkuId AS '商家编码' FROM Ecshop_SKUs s WHERE ProductId = @ProductId; SELECT SkuId AS '商家编码',AttributeName,UseAttributeImage,ValueStr,ImageUrl FROM Ecshop_SKUItems s join Ecshop_Attributes a on s.AttributeId = a.AttributeId join Ecshop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Ecshop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC SELECT * FROM Taobao_Products WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			DataTable table;
			DataTable table2;
			DataTable dataTable;
			DataTable dataTable2;
			DataTable table3;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				table = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				table2 = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				table3 = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				if (dataTable2 != null && dataTable2.Rows.Count > 0)
				{
					foreach (DataRow dataRow in dataTable2.Rows)
					{
						DataColumn dataColumn = new DataColumn();
						dataColumn.ColumnName = (string)dataRow["AttributeName"];
						if (!dataTable.Columns.Contains(dataColumn.ColumnName))
						{
							dataTable.Columns.Add(dataColumn);
						}
					}
					foreach (DataRow dataRow2 in dataTable.Rows)
					{
						foreach (DataRow dataRow in dataTable2.Rows)
						{
							if (string.Compare((string)dataRow2["商家编码"], (string)dataRow["商家编码"]) == 0)
							{
								dataRow2[(string)dataRow["AttributeName"]] = dataRow["ValueStr"];
							}
						}
					}
				}
			}
			dataSet.Tables.Add(table);
			dataSet.Tables.Add(table2);
			dataSet.Tables.Add(dataTable);
			dataSet.Tables.Add(table3);
			return dataSet;
		}
		public bool UpdateToaobProduct(TaobaoProductInfo taobaoProduct)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Taobao_Products WHERE ProductId = @ProductId;INSERT INTO Taobao_Products(Cid, StuffStatus, ProductId, ProTitle,Num, LocationState, LocationCity, FreightPayer, PostFee, ExpressFee, EMSFee, HasInvoice, HasWarranty, HasDiscount, ValidThru, ListTime, PropertyAlias,InputPids,InputStr, SkuProperties, SkuQuantities, SkuPrices, SkuOuterIds) VALUES(@Cid, @StuffStatus, @ProductId, @ProTitle,@Num, @LocationState, @LocationCity, @FreightPayer, @PostFee, @ExpressFee, @EMSFee, @HasInvoice, @HasWarranty, @HasDiscount, @ValidThru, @ListTime,@PropertyAlias,@InputPids, @InputStr, @SkuProperties, @SkuQuantities, @SkuPrices, @SkuOuterIds);");
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, taobaoProduct.ProductId);
			this.database.AddInParameter(sqlStringCommand, "Cid", DbType.Int64, taobaoProduct.Cid);
			this.database.AddInParameter(sqlStringCommand, "StuffStatus", DbType.String, taobaoProduct.StuffStatus);
			this.database.AddInParameter(sqlStringCommand, "ProTitle", DbType.String, taobaoProduct.ProTitle);
			this.database.AddInParameter(sqlStringCommand, "Num", DbType.Int64, taobaoProduct.Num);
			this.database.AddInParameter(sqlStringCommand, "LocationState", DbType.String, taobaoProduct.LocationState);
			this.database.AddInParameter(sqlStringCommand, "LocationCity", DbType.String, taobaoProduct.LocationCity);
			this.database.AddInParameter(sqlStringCommand, "FreightPayer", DbType.String, taobaoProduct.FreightPayer);
			this.database.AddInParameter(sqlStringCommand, "PostFee", DbType.Currency, taobaoProduct.PostFee);
			this.database.AddInParameter(sqlStringCommand, "ExpressFee", DbType.Currency, taobaoProduct.ExpressFee);
			this.database.AddInParameter(sqlStringCommand, "EMSFee", DbType.Currency, taobaoProduct.EMSFee);
			this.database.AddInParameter(sqlStringCommand, "HasInvoice", DbType.Boolean, taobaoProduct.HasInvoice);
			this.database.AddInParameter(sqlStringCommand, "HasWarranty", DbType.Boolean, taobaoProduct.HasWarranty);
			this.database.AddInParameter(sqlStringCommand, "HasDiscount", DbType.Boolean, taobaoProduct.HasDiscount);
			this.database.AddInParameter(sqlStringCommand, "ValidThru", DbType.Int64, taobaoProduct.ValidThru);
			this.database.AddInParameter(sqlStringCommand, "ListTime", DbType.DateTime, taobaoProduct.ListTime);
			this.database.AddInParameter(sqlStringCommand, "PropertyAlias", DbType.String, taobaoProduct.PropertyAlias);
			this.database.AddInParameter(sqlStringCommand, "InputPids", DbType.String, taobaoProduct.InputPids);
			this.database.AddInParameter(sqlStringCommand, "InputStr", DbType.String, taobaoProduct.InputStr);
			this.database.AddInParameter(sqlStringCommand, "SkuProperties", DbType.String, taobaoProduct.SkuProperties);
			this.database.AddInParameter(sqlStringCommand, "SkuQuantities", DbType.String, taobaoProduct.SkuQuantities);
			this.database.AddInParameter(sqlStringCommand, "SkuPrices", DbType.String, taobaoProduct.SkuPrices);
			this.database.AddInParameter(sqlStringCommand, "SkuOuterIds", DbType.String, taobaoProduct.SkuOuterIds);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool IsExitTaobaoProduct(long taobaoProductId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT COUNT(*) FROM Ecshop_Products WHERE TaobaoProductId = {0}", taobaoProductId));
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public DbQueryResult GetToTaobaoProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SaleStatus<>{0}", 0);
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode = '{0}'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%')", query.MaiCategoryPath);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.PublishStatus == PublishStatus.Already)
			{
				stringBuilder.Append(" AND TaobaoProductId <> 0");
			}
			else
			{
				if (query.PublishStatus == PublishStatus.Notyet)
				{
					stringBuilder.Append(" AND TaobaoProductId = 0");
				}
			}
			if (query.IsMakeTaobao.HasValue)
			{
				if (query.IsMakeTaobao.Value == 1)
				{
					stringBuilder.Append(" AND ProductId IN (SELECT ProductId FROM Taobao_Products where ProductId = p.ProductId)");
				}
				else
				{
					if (query.IsMakeTaobao.Value == 0)
					{
						stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Taobao_Products where ProductId = p.ProductId)");
					}
				}
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence,TaobaoProductId");
			stringBuilder2.Append(",(SELECT COUNT(*) FROM Taobao_Products WHERE ProductId = p.ProductId) AS IsMakeTaobao");
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_CDisableBrowseProductList p", "ProductId", stringBuilder.ToString(), stringBuilder2.ToString());
		}
		public PublishToTaobaoProductInfo GetTaobaoProduct(int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT tp.*, p.TaobaoProductId,p.ProductCode, p.Description, p.ImageUrl1, p.ImageUrl2, p.ImageUrl3, p.ImageUrl4, p.ImageUrl5,");
			stringBuilder.Append(" (SELECT MIN(SalePrice) FROM Ecshop_SKUs WHERE ProductId = p.ProductId) AS SalePrice,");
			stringBuilder.Append(" (SELECT MIN(Weight) FROM Ecshop_SKUs WHERE ProductId = p.ProductId) AS Weight");
			stringBuilder.AppendFormat(" FROM Ecshop_Products p JOIN Taobao_Products tp ON p.ProductId = tp.ProductId WHERE p.ProductId = {0}", productId);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			PublishToTaobaoProductInfo publishToTaobaoProductInfo = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					publishToTaobaoProductInfo = new PublishToTaobaoProductInfo();
					publishToTaobaoProductInfo.Cid = (long)dataReader["Cid"];
					if (dataReader["StuffStatus"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.StuffStatus = (string)dataReader["StuffStatus"];
					}
					publishToTaobaoProductInfo.ProductId = (int)dataReader["ProductId"];
					publishToTaobaoProductInfo.ProTitle = (string)dataReader["ProTitle"];
					publishToTaobaoProductInfo.Num = (long)dataReader["Num"];
					publishToTaobaoProductInfo.LocationState = (string)dataReader["LocationState"];
					publishToTaobaoProductInfo.LocationCity = (string)dataReader["LocationCity"];
					publishToTaobaoProductInfo.FreightPayer = (string)dataReader["FreightPayer"];
					if (dataReader["PostFee"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.PostFee = (decimal)dataReader["PostFee"];
					}
					if (dataReader["ExpressFee"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ExpressFee = (decimal)dataReader["ExpressFee"];
					}
					if (dataReader["EMSFee"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.EMSFee = (decimal)dataReader["EMSFee"];
					}
					publishToTaobaoProductInfo.HasInvoice = (bool)dataReader["HasInvoice"];
					publishToTaobaoProductInfo.HasWarranty = (bool)dataReader["HasWarranty"];
					publishToTaobaoProductInfo.HasDiscount = (bool)dataReader["HasDiscount"];
					publishToTaobaoProductInfo.ValidThru = (long)dataReader["ValidThru"];
					if (dataReader["ListTime"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ListTime = (DateTime)dataReader["ListTime"];
					}
					if (dataReader["PropertyAlias"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.PropertyAlias = (string)dataReader["PropertyAlias"];
					}
					if (dataReader["InputPids"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.InputPids = (string)dataReader["InputPids"];
					}
					if (dataReader["InputStr"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.InputStr = (string)dataReader["InputStr"];
					}
					if (dataReader["SkuProperties"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.SkuProperties = (string)dataReader["SkuProperties"];
					}
					if (dataReader["SkuQuantities"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.SkuQuantities = (string)dataReader["SkuQuantities"];
					}
					if (dataReader["SkuPrices"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.SkuPrices = (string)dataReader["SkuPrices"];
					}
					if (dataReader["SkuOuterIds"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.SkuOuterIds = (string)dataReader["SkuOuterIds"];
					}
					if (dataReader["FoodAttributes"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.FoodAttributes = (string)dataReader["FoodAttributes"];
					}
					if (dataReader["TaobaoProductId"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.TaobaoProductId = (long)dataReader["TaobaoProductId"];
					}
					if (dataReader["ProductCode"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ProductCode = (string)dataReader["ProductCode"];
					}
					if (dataReader["Description"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.Description = (string)dataReader["Description"];
					}
					if (dataReader["ImageUrl1"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl1 = (string)dataReader["ImageUrl1"];
					}
					if (dataReader["ImageUrl2"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl2 = (string)dataReader["ImageUrl2"];
					}
					if (dataReader["ImageUrl3"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl3 = (string)dataReader["ImageUrl3"];
					}
					if (dataReader["ImageUrl4"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl4 = (string)dataReader["ImageUrl4"];
					}
					if (dataReader["ImageUrl5"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl5 = (string)dataReader["ImageUrl5"];
					}
					publishToTaobaoProductInfo.SalePrice = (decimal)dataReader["SalePrice"];
					if (dataReader["Weight"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.Weight = (decimal)dataReader["Weight"];
					}
				}
			}
			return publishToTaobaoProductInfo;
		}
		public bool UpdateTaobaoProductId(int productId, long taobaoProductId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Products SET TaobaoProductId = {0} WHERE ProductId = {1}", taobaoProductId, productId));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}

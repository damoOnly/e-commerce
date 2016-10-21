using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.SqlDal.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
namespace EcShop.SqlDal.Commodities
{
	public class ProductApiDao
	{
		private Database database;
		public ProductApiDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DataSet GetProductsByQuery(ProductQuery query, int gradeId, out int totalrecord)
		{
			DataSet dataSet = new DataSet();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.SaleStatus != ProductSaleStatus.All)
			{
				stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
			}
			else
			{
				stringBuilder.AppendFormat(" AND SaleStatus not in ({0})", 0);
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.TagId.HasValue)
			{
				stringBuilder.AppendFormat("AND ProductId IN (SELECT ProductId FROM Ecshop_ProductTag WHERE TagId={0})", query.TagId);
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				int num = 1;
				while (num < array.Length && num <= 4)
				{
					stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
					num++;
				}
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
			{
				stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
			}
			if (query.PublishStatus != PublishStatus.NotSet)
			{
				if (query.PublishStatus == PublishStatus.Notyet)
				{
					stringBuilder.Append(" AND TaobaoProductId = 0");
				}
				else
				{
					stringBuilder.Append(" AND TaobaoProductId <> 0");
				}
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND UpdateDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND UpdateDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			string text = "ProductId,ProductName,ProductCode,ThumbnailUrl60,isnull(MarketPrice,0) as MarketPrice,SaleStatus,isnull(SalePrice,0) as SalePrice,CostPrice,Weight,ShowSaleCounts as SaleCounts, ShortDescription,Stock,AddedDate,[Description],BrandId,Unit";
			if (gradeId > 0)
			{
				int discount = new MemberGradeDao().GetMemberGrade(gradeId).Discount;
				text += string.Format(",CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", gradeId);
				text += string.Format("THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", gradeId, discount);
			}
			else
			{
				text += ",SalePrice as RankPrice";
			}
			string text2 = "";
			if (query.SortOrder == SortAction.Desc)
			{
				text2 = "desc";
			}
			string text3;
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				text3 = " order by " + query.SortBy + " " + text2;
			}
			else
			{
				text3 = " order by ProductId " + text2;
			}
			string text4 = string.Concat(new object[]
			{
				"SELECT TOP ",
				query.PageSize,
				" ",
				text,
				" from vw_Ecshop_CDisableBrowseProductList as p WHERE ",
				stringBuilder.ToString(),
				text3,
				";"
			});
			if (query.PageIndex > 1)
			{
				text4 = string.Concat(new object[]
				{
					"SELECT TOP ",
					query.PageSize,
					" ",
					text,
					" from vw_Ecshop_CDisableBrowseProductList as p WHERE ProductId not in (SELECT ProductId from (SELECT TOP ",
					(query.PageIndex - 1) * query.PageSize,
					" ProductId FROM vw_Ecshop_CDisableBrowseProductList WHERE ",
					stringBuilder.ToString(),
					text3,
					") as T) AND ",
					stringBuilder.ToString(),
					text3,
					";"
				});
			}
			text4 += "select ProductId,SkuId,SKU,Stock,isnull(SalePrice,0) as SalePrice,isnull(CostPrice,0) as CostPrice  from dbo.Ecshop_SKUs;";
			text4 = text4 + "SELECT COUNT(*) AS SumRecord FROM vw_Ecshop_CDisableBrowseProductList WHERE " + stringBuilder.ToString();
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text4);
			DataSet dataSet2;
			dataSet = (dataSet2 = this.database.ExecuteDataSet(sqlStringCommand));
			try
			{
				dataSet.Relations.Add("ProductRealation", dataSet.Tables[0].Columns["ProductId"], dataSet.Tables[1].Columns["ProductId"], false);
			}
			finally
			{
				if (dataSet2 != null)
				{
					((IDisposable)dataSet2).Dispose();
				}
			}
			totalrecord = Convert.ToInt32(dataSet.Tables[2].Rows[0]["SumRecord"].ToString());
			return dataSet;
		}
		public DataSet GetProductSkuDetials(int productId, int gradeId)
		{
			DataSet result = new DataSet();
			if (!string.IsNullOrEmpty(productId.ToString()) && Convert.ToInt32(productId) > 0)
			{
				string text = "SELECT ProductId,ProductName,ProductCode,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,ThumbnailUrl60,isnull(MarketPrice,0)  as MarketPrice,isnull(CostPrice,0) as CostPrice,SaleStatus,isnull(SalePrice,0) as SalePrice, Weight,SaleCounts, ShortDescription,Stock,AddedDate,[Description],BrandId,Unit from vw_Ecshop_CDisableBrowseProductList WHERE ProductId=" + productId + ";";
				if (gradeId > 0)
				{
					Member member = HiContext.Current.User as Member;
					int discount = new MemberGradeDao().GetMemberGrade(gradeId).Discount;
					text += "SELECT ProductId,SkuId, SKU,Stock,";
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = ",
						gradeId,
						") = 1"
					});
					text += string.Format(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) as SalePrice", gradeId, discount);
					text += ",Weight,isnull(CostPrice,0) as CostPrice";
					text += string.Format(" FROM Ecshop_SKUs s WHERE ProductId =" + productId, new object[0]);
				}
				else
				{
					text = text + "SELECT ProductId,SkuId, SKU,Stock,isnull(SalePrice,0) as SalePrice,Weight, CostPrice FROM Ecshop_SKUs WHERE ProductId =" + productId;
				}
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				result = this.database.ExecuteDataSet(sqlStringCommand);
			}
			return result;
		}
		public DataTable GetSkuStocks(string productIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT p.ProductId,ProductName, SkuId, SKU, Stock,ThumbnailUrl40 FROM Ecshop_Products p JOIN Ecshop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
			stringBuilder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Ecshop_SKUItems si JOIN Ecshop_Attributes a ON si.AttributeId = a.AttributeId JOIN Ecshop_AttributeValues av ON si.ValueId = av.ValueId");
			stringBuilder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Ecshop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			DataTable dataTable = null;
			DataTable dataTable2 = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			dataTable.Columns.Add("SKUContent");
			if (dataTable != null && dataTable.Rows.Count > 0 && dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					string text = string.Empty;
					foreach (DataRow dataRow2 in dataTable2.Rows)
					{
						if ((string)dataRow["SkuId"] == (string)dataRow2["SkuId"])
						{
							object obj = text;
							text = string.Concat(new object[]
							{
								obj,
								dataRow2["AttributeName"],
								"：",
								dataRow2["ValueStr"],
								"; "
							});
						}
					}
					dataRow["SKUContent"] = text;
				}
			}
			return dataTable;
		}
		public bool UpdateSkuStock(string productIds, int stock)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_SKUs SET Stock = {0} WHERE ProductId IN ({1})", stock, DataHelper.CleanSearchString(productIds)));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool UpdateSkuStock(Dictionary<string, int> skuStocks)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string current in skuStocks.Keys)
			{
				stringBuilder.AppendFormat(" UPDATE Ecshop_SKUs SET Stock = {0} WHERE SkuId = '{1}'", skuStocks[current], DataHelper.CleanSearchString(current));
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}

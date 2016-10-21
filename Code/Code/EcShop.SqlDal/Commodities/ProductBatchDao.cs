using EcShop.Core;
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
    public class ProductBatchDao
    {
        private Database database;
        public ProductBatchDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public DataTable GetProductBaseInfo(string productIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT ProductId, ProductName, ProductCode, MarketPrice, ThumbnailUrl40, SaleCounts, ShowSaleCounts,isnull(TaxRateId,0) TaxRateId,isnull(BrandId,0) BrandId,isnull(ImportSourceId,0) ImportSourceId FROM Ecshop_Products WHERE ProductId IN ({0})", DataHelper.CleanSearchString(productIds)));
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public ProductInfo GetProductBaseInfo(int productId)
        {
            ProductInfo result = null;
            IList<ProductInfo> productBaseInfo = this.GetProductBaseInfo(new int[]
			{
				productId
			});
            if (productBaseInfo.Count > 0)
            {
                result = productBaseInfo[0];
            }
            return result;
        }
        public IList<ProductInfo> GetProductBaseInfo(IEnumerable<int> productIds)
        {
            string productBaseInfoSelectSQL = this.getProductBaseInfoSelectSQL(productIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(productBaseInfoSelectSQL);
            IList<ProductInfo> result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<ProductInfo>(dataReader);
            }
            return result;
        }
        private string getProductBaseInfoSelectSQL(IEnumerable<int> productIds)
        {
            return string.Format("SELECT ProductId, ProductName, ProductCode, MarketPrice, ThumbnailUrl40, SaleCounts, ShowSaleCounts FROM Ecshop_Products WHERE ProductId IN ({0})", string.Join<int>(",", productIds));
        }
        public bool UpdateProductReferralDeduct(string productIds, decimal referralDeduct, decimal subMemberDeduct, decimal subReferralDeduct)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Products SET ReferralDeduct = {0}, SubMemberDeduct = {1}, SubReferralDeduct = {2} WHERE ProductId IN ({3})", new object[]
			{
				referralDeduct,
				subMemberDeduct,
				subReferralDeduct,
				productIds
			}));
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateProductNames(string productIds, string prefix, string suffix)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Products SET ProductName = '{0}'+ProductName+'{1}' WHERE ProductId IN ({2})", DataHelper.CleanSearchString(prefix), DataHelper.CleanSearchString(suffix), productIds));
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool ReplaceProductNames(string productIds, string oldWord, string newWord)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Products SET ProductName = REPLACE(ProductName, '{0}', '{1}') WHERE ProductId IN ({2})", DataHelper.CleanSearchString(oldWord), DataHelper.CleanSearchString(newWord), productIds));
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateProductBaseInfo(DataTable dt)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            foreach (DataRow dataRow in dt.Rows)
            {
                num++;
                string text = num.ToString();
                stringBuilder.AppendFormat(" UPDATE Ecshop_Products SET ProductName = @ProductName{0}, ProductCode = @ProductCode{0}, MarketPrice = @MarketPrice{0}", text);
                stringBuilder.AppendFormat(" WHERE ProductId = {0}", dataRow["ProductId"]);
                this.database.AddInParameter(sqlStringCommand, "ProductName" + text, DbType.String, dataRow["ProductName"]);
                this.database.AddInParameter(sqlStringCommand, "ProductCode" + text, DbType.String, dataRow["ProductCode"]);
                this.database.AddInParameter(sqlStringCommand, "MarketPrice" + text, DbType.String, dataRow["MarketPrice"]);
            }
            sqlStringCommand.CommandText = stringBuilder.ToString();
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Products SET ShowSaleCounts = {0} WHERE ProductId IN ({1})", showSaleCounts, productIds));
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateVisitCounts(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Products SET VistiCounts = VistiCounts + 1 WHERE ProductId = {0}", productId));
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Products SET ShowSaleCounts = SaleCounts {0} {1} WHERE ProductId IN ({2})", operation, showSaleCounts, productIds));
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateShowSaleCounts(DataTable dt)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (DataRow dataRow in dt.Rows)
            {
                stringBuilder.AppendFormat(" UPDATE Ecshop_Products SET ShowSaleCounts = {0} WHERE ProductId = {1}", dataRow["ShowSaleCounts"], dataRow["ProductId"]);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        /// <summary>
        /// 获取商品权重信息
        /// </summary>
        /// <param name="productIds">商品ids</param>
        /// <returns></returns>
        public DataTable GetProductsFractionChange(string productIds)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT p.ProductId,ProductName, SkuId, SKU, AdminFraction,Fraction, ThumbnailUrl40 FROM Ecshop_Products p JOIN Ecshop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
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
            //dataTable.Columns.Add("SKUContent");
            //if (dataTable != null && dataTable.Rows.Count > 0 && dataTable2 != null && dataTable2.Rows.Count > 0)
            //{
            //    foreach (DataRow dataRow in dataTable.Rows)
            //    {
            //        string text = string.Empty;
            //        foreach (DataRow dataRow2 in dataTable2.Rows)
            //        {
            //            if ((string)dataRow["SkuId"] == (string)dataRow2["SkuId"])
            //            {
            //                object obj = text;
            //                text = string.Concat(new object[]
            //                {
            //                    obj,
            //                    dataRow2["AttributeName"],
            //                    "：",
            //                    dataRow2["ValueStr"],
            //                    "; "
            //                });
            //            }
            //        }
            //        dataRow["SKUContent"] = text;
            //    }
            //}
            return dataTable;
        }
        public DataTable GetSkuStocks(string productIds)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT p.ProductId,ProductName, SkuId, SKU, Stock, ThumbnailUrl40 FROM Ecshop_Products p JOIN Ecshop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
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
        public bool AddSkuStock(string productIds, int addStock)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_SKUs SET Stock = CASE WHEN Stock + ({0}) < 0 THEN 0 ELSE Stock + ({0}) END WHERE ProductId IN ({1})", addStock, DataHelper.CleanSearchString(productIds)));
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        /// <summary>
        /// 修改商品的权重信息为固定值
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="addStock"></param>
        /// <returns></returns>
        public bool UpdataProductsAdminFraction(string productIds, decimal fraction)
        {
            Dictionary<string, decimal> productFraction = CalculateProductFraction(productIds);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string k in productFraction.Keys)
            {
                stringBuilder.AppendFormat("UPDATE Ecshop_Products SET AdminFraction= {0},Fraction={2} WHERE ProductId = {1}", fraction, DataHelper.CleanSearchString(k),productFraction[k]+fraction);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        /// <summary>
        /// 修改商品的权重信息（在原基础上增加多少值）
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="fraction"></param>
        /// <returns></returns>
        public bool AddProductsAdminFraction(string productIds, decimal fraction)
        {
            Dictionary<string, decimal> productFraction = CalculateProductFraction(productIds);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string k in productFraction.Keys)
            {
                stringBuilder.AppendFormat("UPDATE Ecshop_Products SET AdminFraction = CASE WHEN AdminFraction + {0} < 0 THEN 0 ELSE AdminFraction + {0} END,Fraction= {2} + AdminFraction + {0} WHERE ProductId = {1}", fraction, DataHelper.CleanSearchString(k), productFraction[k]);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        /// <summary>
        /// 批量修改商品的权重信息
        /// </summary>
        /// <param name="fractions"></param>
        /// <returns></returns>
        /// <summary>
        public bool UpdateFraction(Dictionary<string, decimal> fractions)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string productIds = string.Join(",", fractions.Keys);
            //获得商品商品分值+活跃度值 key：productId
            Dictionary<string, decimal> productFraction = CalculateProductFraction(productIds);
            foreach (string current in fractions.Keys)
            {
                stringBuilder.AppendFormat(" UPDATE Ecshop_Products SET AdminFraction = {0},Fraction={2} WHERE ProductId = {1}", fractions[current], DataHelper.CleanSearchString(current), productFraction[DataHelper.CleanSearchString(current)]+fractions[current]);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        /// <summary>
        /// 计算商品权重值(不含手动增加权重值 Products表->AdminFraction字段值)
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> CalculateProductFraction(string productIds)
        {
            //1.计算商品分值
            Dictionary<string, decimal> productScore = GetProducts(productIds);
            //2.计算活跃度
            Dictionary<string, decimal> vitality = CalculateVitality(productIds);
            //3.获取商品分值+活跃度值
            foreach (string k in productScore.Keys)
            {
                if (vitality.ContainsKey(k))
                {
                    vitality[k] += productScore[k];
                }
            }
            return vitality;
        }
        /// <summary>
        /// 计算商品分值
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        private Dictionary<string, decimal> GetProducts(string productIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT ProductId,ProductName,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,[Description],MobbileDescription FROM Ecshop_Products WHERE ProductId IN({0})", DataHelper.CleanSearchString(productIds));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql.ToString());
            DataTable dataTable = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();
            if (dataTable != null)
            {
                foreach (DataRow r in dataTable.Rows)
                {
                    decimal productScore = CalculateProductScore(r);
                    dic.Add(r["ProductId"].ToString(), productScore);
                }
            }
            return dic;
        }
        /// <summary>
        /// 计算商品分值
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private decimal CalculateProductScore(DataRow row)
        {
            string productName = row["ProductName"].ToString();
            decimal titleScore = productName.Length < 10 ? 1M : 1.5M;
            decimal imageScore = 0M;
            if (row["ImageUrl1"] != DBNull.Value)
            {
                imageScore += 0.5M;
            }
            if (row["ImageUrl2"] != DBNull.Value)
            {
                imageScore += 0.5M;
            }
            if (row["ImageUrl3"] != DBNull.Value)
            {
                imageScore += 0.5M;
            }
            if (row["ImageUrl4"] != DBNull.Value)
            {
                imageScore += 0.5M;
            }
            if (row["ImageUrl5"] != DBNull.Value)
            {
                imageScore += 0.5M;
            }
            string description = row["Description"].ToString();// RemoveHTMLTags(row["Description"].ToString());
            string mobileDescription = row["MobbileDescription"].ToString();// RemoveHTMLTags(row["MobbileDescription"].ToString());
            int descLength = (description + mobileDescription).Length;
            decimal descScore = 0M;

            if (descLength > 500)
            {
                descScore = 0.5M;
            }
            else if (descLength > 1000)
            {
                descScore = 1M;
            }
            else if (descLength > 2000)
            {
                descScore = 1.5M;
            }
            decimal productScore = titleScore + imageScore + descScore;
            return productScore;
        }
        /// <summary>
        /// 计算活跃度
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> CalculateVitality(string productIds)
        {
            //点击率VistiCounts
            //收藏数率CollectSum
            //交易量率SaleCounts
            //商品新鲜度Freshness
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT a.ProductId,CONVERT(decimal(18,4), VistiCounts)/1000 +");
            strSql.Append("(SELECT CONVERT(decimal(18,4),COUNT(*))/100 FROM dbo.Ecshop_Favorite WHERE ProductId=a.ProductId) +");
            strSql.Append("CONVERT(decimal(18,4),(SELECT COUNT(*) FROM Ecshop_OrderItems WHERE ProductId=a.ProductId))/100 +");
            strSql.Append("case when (SELECT COUNT(*) FROM Ecshop_Products WHERE AddedDate>=DATEADD(day,-7, GETDATE()) AND ProductId=a.ProductId)>0 then (CONVERT(decimal, (SELECT COUNT(*) FROM Ecshop_Products WHERE AddedDate>=DATEADD(day,-7, GETDATE())))/(SELECT COUNT(*) FROM Ecshop_Products)) else 0 end Vitality");
            strSql.AppendFormat(" FROM Ecshop_Products AS a WHERE ProductId IN({0})", DataHelper.CleanSearchString(productIds));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql.ToString());
            DataTable dataTable = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();
            if (dataTable != null)
            {
                foreach (DataRow r in dataTable.Rows)
                {
                    dic.Add(r["ProductId"].ToString(), (decimal)r["Vitality"]);
                }
            }
            return dic;
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
        public DataTable GetSkuMemberPrices(string productIds)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT SkuId, ProductName, SKU, CostPrice, MarketPrice, SalePrice FROM Ecshop_Products p JOIN Ecshop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
            stringBuilder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Ecshop_SKUItems si JOIN Ecshop_Attributes a ON si.AttributeId = a.AttributeId JOIN Ecshop_AttributeValues av ON si.ValueId = av.ValueId");
            stringBuilder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Ecshop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
            stringBuilder.AppendLine(" SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] AS MemberGradeName,Discount FROM aspnet_MemberGrades");
            stringBuilder.AppendLine(" SELECT SkuId, (SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] FROM aspnet_MemberGrades WHERE GradeId = sm.GradeId) AS MemberGradeName,MemberSalePrice");
            stringBuilder.AppendFormat(" FROM Ecshop_SKUMemberPrice sm WHERE SkuId IN (SELECT SkuId FROM Ecshop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            DataTable dataTable = null;
            DataTable dataTable2 = null;
            DataTable dataTable3 = null;
            DataTable dataTable4 = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    dataTable.Columns.Add("SKUContent");
                    dataReader.NextResult();
                    dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
                    dataReader.NextResult();
                    dataTable4 = DataHelper.ConverDataReaderToDataTable(dataReader);
                    if (dataTable4 != null && dataTable4.Rows.Count > 0)
                    {
                        foreach (DataRow dataRow in dataTable4.Rows)
                        {
                            dataTable.Columns.Add((string)dataRow["MemberGradeName"]);
                        }
                    }
                    dataReader.NextResult();
                    dataTable3 = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
            }
            if (dataTable2 != null && dataTable2.Rows.Count > 0)
            {
                foreach (DataRow dataRow2 in dataTable.Rows)
                {
                    string text = string.Empty;
                    foreach (DataRow dataRow3 in dataTable2.Rows)
                    {
                        if ((string)dataRow2["SkuId"] == (string)dataRow3["SkuId"])
                        {
                            object obj = text;
                            text = string.Concat(new object[]
							{
								obj,
								dataRow3["AttributeName"],
								"：",
								dataRow3["ValueStr"],
								"; "
							});
                        }
                    }
                    dataRow2["SKUContent"] = text;
                }
            }
            if (dataTable3 != null && dataTable3.Rows.Count > 0)
            {
                foreach (DataRow dataRow2 in dataTable.Rows)
                {
                    foreach (DataRow dataRow4 in dataTable3.Rows)
                    {
                        if ((string)dataRow2["SkuId"] == (string)dataRow4["SkuId"])
                        {
                            dataRow2[(string)dataRow4["MemberGradeName"]] = dataRow4["MemberSalePrice"];
                        }
                    }
                }
            }
            if (dataTable4 != null && dataTable4.Rows.Count > 0)
            {
                foreach (DataRow dataRow2 in dataTable.Rows)
                {
                    decimal d = decimal.Parse(dataRow2["SalePrice"].ToString());
                    foreach (DataRow dataRow5 in dataTable4.Rows)
                    {
                        decimal d2 = decimal.Parse(dataRow5["Discount"].ToString());
                        string arg = (d * (d2 / 100m)).ToString("F2");
                        dataRow2[(string)dataRow5["MemberGradeName"]] = dataRow2[(string)dataRow5["MemberGradeName"]] + "|" + arg;
                    }
                }
            }
            return dataTable;
        }
        public bool CheckPrice(string productIds, int baseGradeId, decimal checkPrice, bool isMember)
        {
            StringBuilder stringBuilder = new StringBuilder(" ");
            if (baseGradeId == -2)
            {
                stringBuilder.AppendFormat("SELECT COUNT(*) FROM Ecshop_SKUs WHERE ProductId IN ({0}) AND CostPrice - {1} < 0", productIds, checkPrice);
            }
            else
            {
                if (baseGradeId == -3)
                {
                    stringBuilder.AppendFormat("SELECT COUNT(*) FROM Ecshop_SKUs WHERE ProductId IN ({0}) AND SalePrice - {1} < 0", productIds, checkPrice);
                }
                else
                {
                    if (isMember)
                    {
                        stringBuilder.AppendFormat("SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE MemberSalePrice - {0} < 0 AND GradeId = {1}", checkPrice, baseGradeId);
                        stringBuilder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Ecshop_SKUs WHERE ProductId IN ({0})) ", productIds);
                    }
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
        }
        public bool UpdateSkuMemberPrices(string productIds, int gradeId, decimal price)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (gradeId == -2)
            {
                stringBuilder.AppendFormat("UPDATE Ecshop_SKUs SET CostPrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
            }
            else
            {
                if (gradeId == -3)
                {
                    stringBuilder.AppendFormat("UPDATE Ecshop_SKUs SET SalePrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
                }
                else
                {
                    stringBuilder.AppendFormat("DELETE FROM Ecshop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Ecshop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
                    stringBuilder.AppendFormat(" INSERT INTO Ecshop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, {1} AS MemberSalePrice FROM Ecshop_SKUs WHERE ProductId IN ({2})", gradeId, price, DataHelper.CleanSearchString(productIds));
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateSkuMemberPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
        {
            StringBuilder stringBuilder = new StringBuilder(" ");
            if (gradeId == -2)
            {
                if (baseGradeId == -2)
                {
                    stringBuilder.AppendFormat("UPDATE Ecshop_SKUs SET CostPrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
                }
                else
                {
                    if (baseGradeId == -3)
                    {
                        stringBuilder.AppendFormat("UPDATE Ecshop_SKUs SET CostPrice = SalePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
                    }
                }
            }
            else
            {
                if (gradeId == -3)
                {
                    if (baseGradeId == -2)
                    {
                        stringBuilder.AppendFormat("UPDATE Ecshop_SKUs SET SalePrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
                    }
                    else
                    {
                        if (baseGradeId == -3)
                        {
                            stringBuilder.AppendFormat("UPDATE Ecshop_SKUs SET SalePrice = SalePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
                        }
                    }
                }
                else
                {
                    stringBuilder.AppendFormat("DELETE FROM Ecshop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Ecshop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
                    if (baseGradeId == -2)
                    {
                        stringBuilder.AppendFormat(" INSERT INTO Ecshop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, CostPrice {1} ({2}) AS MemberSalePrice FROM Ecshop_SKUs WHERE ProductId IN ({3})", new object[]
						{
							gradeId,
							operation,
							price,
							DataHelper.CleanSearchString(productIds)
						});
                    }
                    else
                    {
                        if (baseGradeId == -3)
                        {
                            stringBuilder.AppendFormat(" INSERT INTO Ecshop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, SalePrice {1} ({2}) AS MemberSalePrice FROM Ecshop_SKUs WHERE ProductId IN ({3})", new object[]
							{
								gradeId,
								operation,
								price,
								DataHelper.CleanSearchString(productIds)
							});
                        }
                        else
                        {
                            stringBuilder.AppendFormat(" INSERT INTO Ecshop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, MemberSalePrice {1} ({2}) AS MemberSalePrice", gradeId, operation, price);
                            stringBuilder.AppendFormat(" FROM Ecshop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Ecshop_SKUs WHERE ProductId IN ({1}))", baseGradeId, DataHelper.CleanSearchString(productIds));
                        }
                    }
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateSkuMemberPrices(DataSet ds)
        {
            StringBuilder stringBuilder = new StringBuilder();
            DataTable dataTable = ds.Tables["skuPriceTable"];
            DataTable dataTable2 = ds.Tables["skuMemberPriceTable"];
            string text = string.Empty;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    object obj = text;
                    text = string.Concat(new object[]
					{
						obj,
						"'",
						dataRow["skuId"],
						"',"
					});
                    stringBuilder.AppendFormat(" UPDATE Ecshop_SKUs SET CostPrice = {0}, SalePrice = {1} WHERE SkuId = '{2}'", dataRow["costPrice"], dataRow["salePrice"], dataRow["skuId"]);
                }
            }
            if (text.Length > 1)
            {
                stringBuilder.AppendFormat(" DELETE FROM Ecshop_SKUMemberPrice WHERE SkuId IN ({0}) ", text.Remove(text.Length - 1));
            }
            if (dataTable2 != null && dataTable2.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable2.Rows)
                {
                    stringBuilder.AppendFormat(" INSERT INTO Ecshop_SKUMemberPrice (SkuId, GradeId, MemberSalePrice) VALUES ('{0}', {1}, {2})", dataRow["skuId"], dataRow["gradeId"], dataRow["memberPrice"]);
                }
            }
            bool result;
            if (stringBuilder.Length <= 0)
            {
                result = false;
            }
            else
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
                result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return result;
        }

        public bool UpdateProductFraction(Dictionary<string, decimal> productFractions)
        {
            string sqlTemplate = "UPDATE Ecshop_Products SET Fraction = AdminFraction + {0} WHERE ProductId = {1};";

            StringBuilder sbSql = new StringBuilder();

            foreach (var current in productFractions)
            {
                sbSql.AppendFormat(sqlTemplate, current.Value, current.Key);
            }

            if (sbSql.Length > 0)
            {
                DbCommand command = this.database.GetSqlStringCommand(sbSql.ToString());

                return (this.database.ExecuteNonQuery(command) > 0);
            }

            return false;
        }

        /// <summary>
        /// 批量修改商品的税率
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool UpdateProductTaxRate(DataTable dt)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            foreach (DataRow dataRow in dt.Rows)
            {
                num++;
                string text = num.ToString();
                stringBuilder.AppendFormat(" UPDATE Ecshop_Products SET TaxRateId = @TaxRateId{0}", text);
                stringBuilder.AppendFormat(" WHERE ProductId = {0}", dataRow["ProductId"]);
                this.database.AddInParameter(sqlStringCommand, "TaxRateId" + text, DbType.String, dataRow["TaxRateId"]);
            }
            sqlStringCommand.CommandText = stringBuilder.ToString();
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 批量修改商品的原产地
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool UpdateProductImportSource(DataTable dt)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            foreach (DataRow dataRow in dt.Rows)
            {
                num++;
                string text = num.ToString();
                stringBuilder.AppendFormat(" UPDATE Ecshop_Products SET ImportSourceId = @ImportSourceId{0}", text);
                stringBuilder.AppendFormat(" WHERE ProductId = {0}", dataRow["ProductId"]);
                this.database.AddInParameter(sqlStringCommand, "ImportSourceId" + text, DbType.String, dataRow["ImportSourceId"]);
            }
            sqlStringCommand.CommandText = stringBuilder.ToString();
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 批量修改商品的品牌
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool UpdateProductBrand(DataTable dt)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            foreach (DataRow dataRow in dt.Rows)
            {
                num++;
                string text = num.ToString();
                stringBuilder.AppendFormat(" UPDATE Ecshop_Products SET BrandId = @BrandId{0}", text);
                stringBuilder.AppendFormat(" WHERE ProductId = {0}", dataRow["ProductId"]);
                this.database.AddInParameter(sqlStringCommand, "BrandId" + text, DbType.String, dataRow["BrandId"]);
            }
            sqlStringCommand.CommandText = stringBuilder.ToString();
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
    }
}

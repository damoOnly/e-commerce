using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SqlDal.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace EcShop.SqlDal.Commodities
{
    public class ProductBrowseDao
    {
        private Database database;
        public ProductBrowseDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public DataTable GetSaleProductRanking(int? categoryId, int maxNum)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT TOP {0} ProductId, ProductName, ProductCode, ShowSaleCounts AS SaleCounts, ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100,", maxNum);
            stringBuilder.AppendFormat("  ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220, SalePrice, MarketPrice, CategoryId FROM vw_Ecshop_CDisableBrowseProductList WHERE SaleStatus = {0}", 1);
            if (categoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
                if (category != null)
                {
                    stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
            }
            stringBuilder.Append("ORDER BY ShowSaleCounts DESC");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public DataTable GetSubjectList(SubjectListQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                stringBuilder.AppendFormat("SELECT TOP {0} ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,", query.MaxNum);
                stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice,CategoryId,");
                stringBuilder.AppendFormat(" CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", member.GradeId);
                stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", member.GradeId, discount);
            }
            else
            {
                stringBuilder.AppendFormat("SELECT TOP {0} ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,", query.MaxNum);
                stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice,SalePrice AS RankPrice,CategoryId");
            }
            stringBuilder.Append(" FROM vw_Ecshop_BrowseProductList p WHERE ");
            stringBuilder.Append(this.BuildProductSubjectQuerySearch(query));
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), DataHelper.CleanSearchString(query.SortOrder.ToString()));
            }
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        private int GetMemberDiscount(int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Discount FROM aspnet_MemberGrades WHERE GradeId=@GradeId");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }
        public DbQueryResult GetShareProducts(int shareId, ProductBrowseQuery query)
        {
            string text = this.BuildProductBrowseQuerySearch(query);
            object obj = text;
            text = string.Concat(new object[]
			{
				obj,
				" AND ProductId IN (SELECT RelatedProductId as  ProductId FROM dbo.Vshop_RelatedTopicProducts where TopicId=",
				shareId,
				")"
			});
            string text2 = "ProductId,ProductName,ProductCode, ShowSaleCounts AS SaleCounts, ShortDescription, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice, SalePrice,Stock,CategoryId";
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                int memberDiscount = new MemberDao().GetMemberDiscount(member.GradeId);
                text2 += string.Format(",CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", member.GradeId);
                text2 += string.Format("THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", member.GradeId, memberDiscount);
            }
            else
            {
                text2 += ",SalePrice as RankPrice";
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseTopicProductList p", "ProductId", text, text2);
        }
        public DataTable GetSkusByProductId(int productId)
        {
            Member member = HiContext.Current.User as Member;
            int memberDiscount = this.GetMemberDiscount(member.GradeId);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice,");
            stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
            stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0})", member.GradeId);
            stringBuilder.AppendFormat(" ELSE SalePrice * {0} /100 END) AS SalePrice", memberDiscount);
            stringBuilder.Append(" FROM Ecshop_SKUs s WHERE ProductId = @ProductId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        protected string BuildProductSubjectQuerySearch(SubjectListQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(" SaleStatus = {0}", 1);
            if (query.TagId != 0)
            {
                stringBuilder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM Ecshop_ProductTag WHERE TagId = {0})", query.TagId);
            }
            if (!string.IsNullOrEmpty(query.CategoryIds))
            {
                string[] array = query.CategoryIds.Split(new char[]
				{
					','
				});
                int categoryId = 0;
                bool flag = false;
                stringBuilder.AppendFormat(" AND (", new object[0]);
                for (int i = 0; i < array.Length; i++)
                {
                    categoryId = 0;
                    int.TryParse(array[i], out categoryId);
                    CategoryInfo category = new CategoryDao().GetCategory(categoryId);
                    if (category != null)
                    {
                        if (flag)
                        {
                            stringBuilder.Append(" OR ");
                        }
                        stringBuilder.AppendFormat(" ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                        flag = true;
                    }
                }
                if (!flag)
                {
                    stringBuilder.Append("1=1");
                }
                stringBuilder.Append(")");
            }
            if (query.BrandCategoryId.HasValue)
            {
                stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandCategoryId.Value);
            }
            if (query.ProductTypeId.HasValue)
            {
                stringBuilder.AppendFormat(" AND TypeId = {0}", query.ProductTypeId.Value);
            }
            if (query.AttributeValues.Count > 0)
            {
                foreach (AttributeValueInfo current in query.AttributeValues)
                {
                    stringBuilder.AppendFormat(" AND (ProductId IN ( SELECT ProductId FROM Ecshop_ProductAttributes WHERE AttributeId={0} And ValueId={1})) ", current.AttributeId, current.ValueId);
                }
            }
            if (query.MinPrice.HasValue)
            {
                stringBuilder.AppendFormat(" AND SalePrice >= {0}", query.MinPrice.Value);
            }
            if (query.MaxPrice.HasValue)
            {
                stringBuilder.AppendFormat(" AND SalePrice <= {0}", query.MaxPrice.Value);
            }
            if (!string.IsNullOrEmpty(query.Keywords) && query.Keywords.Trim().Length > 0)
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] array2 = Regex.Split(query.Keywords.Trim(), "\\s+");
                stringBuilder.AppendFormat(" AND (ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array2[0]));
                int i = 1;
                while (i < array2.Length && i <= 5)
                {
                    stringBuilder.AppendFormat(" OR ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array2[i]));
                    i++;
                }
                stringBuilder.AppendFormat(" or ShopName like '%{0}%')", query.Keywords);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        ///检查活动是否运行中
        /// </summary>
        /// <returns></returns>
        public bool CheckActiveIsRunding(int CateGoryId, int productId)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat(@"  select top 1 datediff(ms,getdate(),a.EndDate)  as 'ActiveTime' from  (SELECT 
            StartDate = CONVERT(datetime, 
									  CASE WHEN (StartTime > CONVERT(varchar(8), GETDATE(), 114) OR EndTime > CONVERT(varchar(8), GETDATE(), 114)) 
										 THEN CONVERT(varchar(10), GETDATE(), 120) 
									   ELSE CONVERT(varchar(10), GETDATE() + 1, 120) 
									   END + ' ' + CONVERT(varchar(8), StartTime, 114))
									   ,
			 EndDate = DATEADD(MINUTE, 
					 CASE WHEN DATEDIFF(MINUTE, StartTime, EndTime) > 0 
					 THEN DATEDIFF(MINUTE, StartTime, EndTime) ELSE 24 * 60 + DATEDIFF(MINUTE, StartTime, EndTime) END,
					 CASE WHEN (StartTime > CONVERT(varchar(8), GETDATE(), 114) OR EndTime > CONVERT(varchar(8), GETDATE(), 114)) 
					 THEN CONVERT(varchar(10), GETDATE(), 120) ELSE CONVERT(varchar(10), GETDATE() + 1, 120) END + ' ' + CONVERT(varchar(8), StartTime, 114) ) 
			FROM [dbo].[Ecshop_CountDownCategories]  where  CountDownCategoryId={0})   as a 
			inner join [Ecshop_CountDown] as b on b.StartDate = a.StartDate AND b.EndDate = a.EndDate
			where b.ProductId={1}", CateGoryId, productId);
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
                int ActiveTime = 0;

                int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(), out ActiveTime);
                if (ActiveTime > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取活动商品
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="CateGoryId"></param>
        /// <param name="maxReviewNum"></param>
        /// <param name="maxConsultationNum"></param>
        /// <returns></returns>
        public ProductBrowseInfo GetProductBrowseInfoByActive(int productId, int CateGoryId, int? maxReviewNum, int? maxConsultationNum)
        {
            ProductBrowseInfo productBrowseInfo = new ProductBrowseInfo();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE Ecshop_Products SET VistiCounts = VistiCounts + 1 WHERE ProductId = @ProductId;");
            stringBuilder.Append(@" SELECT (select min(ISNull(t.TaxRate,0)) from Ecshop_Products p 
left join Ecshop_SKUs s on p.ProductId = s.ProductId 
left join Ecshop_ProductsCombination  com on com.CombinationSkuId = s.SkuId 
inner join Ecshop_Products comp on comp.ProductId = com.Productid 
left join Ecshop_TaxRate as t on comp.TaxRateId=t.taxId where p.ProductId=@ProductId ) as MinTaxRate,
(select max(ISNull(t.TaxRate,0)) from Ecshop_Products p 
left join Ecshop_SKUs s on p.ProductId = s.ProductId 
left join Ecshop_ProductsCombination  com on com.CombinationSkuId = s.SkuId 
inner join Ecshop_Products comp on comp.ProductId = com.Productid 
left join Ecshop_TaxRate as t on comp.TaxRateId=t.taxId where p.ProductId=@ProductId ) as MaxTaxRate,");

            stringBuilder.Append(@"case when p.SaleType=2 then ISNULL((select  sum(ISNull(t1.TaxRate,0)*com.Quantity*com.Price)/sum(com.Quantity*com.Price) from Ecshop_Products p1 
left join Ecshop_SKUs s1 on p1.ProductId = s1.ProductId 
left join Ecshop_ProductsCombination  com on com.CombinationSkuId = s1.SkuId 
inner join Ecshop_Products comp on comp.ProductId = com.Productid 
left join Ecshop_TaxRate as t1 on comp.TaxRateId=t1.taxId  where p1.ProductId=p.ProductId ),0)

else ISNULL(t.TaxRate, 0) end  AS TaxRate,");

            stringBuilder.Append("p.* ,ISNULL(IsDisable,0) as cIsDisable,ship.*,CASE WHEN BrandId IS NULL THEN NULL ELSE (SELECT BrandName FROM Ecshop_BrandCategories WHERE BrandId= p.BrandId) END AS BrandName,isnull(esu.SupplierName,'海美自营') SupplierName,esu.Logo AS SupplierLogo ");
            stringBuilder.Append(" FROM Ecshop_Products p left join  Ecshop_TaxRate as t on p.TaxRateId=t.taxId left join Ecshop_ShippingTemplates ship on p.TemplateId = ship.TemplateId");
            stringBuilder.Append(" LEFT JOIN Ecshop_Supplier AS esu ON p.SupplierId=esu.SupplierId ");
            stringBuilder.Append(" INNER JOIN  Ecshop_Categories AS ec ON p.CategoryId=ec.CategoryId  ");
            stringBuilder.Append("  where ProductId=@ProductId;");

            //
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                //stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,FactStock,CostPrice,DeductFee,");
                //stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
                //stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
                //stringBuilder.Append(" FROM Ecshop_SKUs s WHERE ProductId = @ProductId");

                stringBuilder.Append("SELECT skus.ProductId, skus.SkuId, s.AttributeId, s.ValueId, skus.SKU,skus.CostPrice,isnull(skus.FactStock,0) FactStock,skus.[Weight],skus.DeductFee,");

                //组合商品库存处理
                stringBuilder.Append(@"(select case when p.saletype=2 then   
(select min(s1.Stock/pc.Quantity)  from  dbo.Ecshop_SKUs s1 inner join

 Ecshop_ProductsCombination pc 
 
 on pc.Skuid=s1.Skuid

where pc.CombinationSkuId=s.Skuid)   

else s.stock end   

from dbo.Ecshop_SKUs s inner join

dbo.Ecshop_Products p on p.productid=s.productid where s.Skuid=skus.Skuid) as Stock,");
                stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
                stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
                stringBuilder.Append(" FROM Ecshop_SKUItems s right outer join Ecshop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId =@ProductId ORDER BY skus.SkuId");
            }
            else
            {
                //stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,FactStock,CostPrice, SalePrice,DeductFee FROM Ecshop_SKUs WHERE ProductId = @ProductId");
                //stringBuilder.Append("SELECT skus.ProductId, skus.SkuId, s.AttributeId, s.ValueId, skus.SKU, skus.SalePrice, skus.CostPrice, skus.Stock,isnull(skus.FactStock,0) FactStock,skus.[Weight],skus.DeductFee FROM Ecshop_SKUItems s right outer join Ecshop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId =@ProductId ORDER BY skus.SkuId");

                stringBuilder.Append("SELECT skus.ProductId, skus.SkuId, s.AttributeId, s.ValueId, skus.SKU, skus.SalePrice, skus.CostPrice,");
                //组合商品库存处理
                stringBuilder.Append(@"(select case when p.saletype=2 then   
(select min(s1.Stock/pc.Quantity)  from  dbo.Ecshop_SKUs s1 inner join

 Ecshop_ProductsCombination pc 
 
 on pc.Skuid=s1.Skuid

where pc.CombinationSkuId=s.Skuid)   

else s.stock end   

from dbo.Ecshop_SKUs s inner join

dbo.Ecshop_Products p on p.productid=s.productid where s.Skuid=skus.Skuid) as Stock,");
                stringBuilder.Append("isnull(skus.FactStock,0) FactStock,skus.[Weight],skus.DeductFee FROM Ecshop_SKUItems s right outer join Ecshop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId =@ProductId ORDER BY skus.SkuId");
            }
            if (maxReviewNum.HasValue)
            {
                stringBuilder.AppendFormat(" SELECT TOP {0} * FROM Ecshop_ProductReviews where ProductId=@ProductId ORDER BY ReviewId DESC; ", maxReviewNum);
                stringBuilder.Append(" SELECT Count(*) FROM Ecshop_ProductReviews where ProductId=@ProductId; ");
            }
            else
            {
                stringBuilder.Append(" SELECT * FROM Ecshop_ProductReviews where ProductId=@ProductId ORDER BY ReviewId DESC; ");
                stringBuilder.Append(" SELECT Count(*) FROM Ecshop_ProductReviews where ProductId=@ProductId; ");
            }
            if (maxConsultationNum.HasValue)
            {
                stringBuilder.AppendFormat(" SELECT TOP {0} * FROM Ecshop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;", maxConsultationNum);
                stringBuilder.Append(" SELECT Count(*) FROM Ecshop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ; ");
            }
            else
            {
                stringBuilder.Append(" SELECT * FROM Ecshop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;");
                stringBuilder.Append(" SELECT Count(*) FROM Ecshop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ; ");
            }
            stringBuilder.Append(" SELECT a.AttributeId, AttributeName, ValueStr FROM Ecshop_ProductAttributes pa JOIN Ecshop_Attributes a ON pa.AttributeId = a.AttributeId");
            stringBuilder.Append(" JOIN Ecshop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
            stringBuilder.Append(" SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl FROM Ecshop_SKUItems s join Ecshop_Attributes a on s.AttributeId = a.AttributeId join Ecshop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Ecshop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;");
            stringBuilder.Append(" SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Ecshop_CDisableBrowseProductList");
            stringBuilder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId IN (SELECT RelatedProductId FROM Ecshop_RelatedProducts WHERE ProductId = {1}) ORDER BY DisplaySequence DESC;", 1, productId);

            stringBuilder.Append("SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Ecshop_CDisableBrowseProductList");
            stringBuilder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId<>{1}  AND CategoryId = (SELECT CategoryId FROM Ecshop_Products WHERE ProductId={1} AND SaleStatus = {0})", 1, productId);
            stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT RelatedProductId FROM Ecshop_RelatedProducts WHERE ProductId = {0})", productId);
            stringBuilder.AppendFormat(" AND productId IN (SELECT productId FROM Ecshop_ProductTag WHERE tagId={0})", 4);//推荐标签
            stringBuilder.AppendFormat(" ORDER BY DisplaySequence DESC;");

            stringBuilder.Append("SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Ecshop_CDisableBrowseProductList");
            stringBuilder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId<>{1}  AND CategoryId = (SELECT CategoryId FROM Ecshop_Products WHERE ProductId={1} AND SaleStatus = {0})", 1, productId);
            stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT RelatedProductId FROM Ecshop_RelatedProducts WHERE ProductId = {0})", productId);
            stringBuilder.AppendFormat(" ORDER BY ShowSaleCounts DESC;");//销售排行

            stringBuilder.AppendFormat("SELECT Notes3 FROM Ecshop_Categories WHERE CategoryId = (SELECT CategoryId FROM Ecshop_Products WHERE ProductId={1} AND SaleStatus = {0})", 1, productId);//再来一个广告图片

         

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    productBrowseInfo.Product = DataMapper.PopulateProduct(dataReader);
                    if (dataReader["BrandName"] != DBNull.Value)
                    {
                        productBrowseInfo.BrandName = (string)dataReader["BrandName"];
                    }
                    if (dataReader["SupplierName"] != DBNull.Value)
                    {
                        productBrowseInfo.SupplierName = (string)dataReader["SupplierName"];
                    }
                    if (dataReader["SupplierLogo"] != DBNull.Value)
                    {
                        productBrowseInfo.SupplierLogo = (string)dataReader["SupplierLogo"];
                    }
                    if (dataReader["cIsDisable"] != DBNull.Value)
                    {
                        productBrowseInfo.cIsDisable = (Int32)dataReader["cIsDisable"];
                    }

                }
                if (dataReader.NextResult())
                {
                    while (dataReader.Read())
                    {
                        string key = (string)dataReader["SkuId"];
                        if (!productBrowseInfo.Product.Skus.ContainsKey(key))
                        {
                            productBrowseInfo.Product.Skus.Add(key, DataMapper.PopulateSKU(dataReader));
                        }
                        if (dataReader["AttributeId"] != DBNull.Value && dataReader["ValueId"] != DBNull.Value)
                        {
                            productBrowseInfo.Product.Skus[key].SkuItems.Add((int)dataReader["AttributeId"], (int)dataReader["ValueId"]);
                        }
                        //productBrowseInfo.Product.Skus.Add((string)dataReader["SkuId"], DataMapper.PopulateSKU(dataReader));
                    }
                }
                if (dataReader.NextResult())
                {
                    productBrowseInfo.DBReviews = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                if (dataReader.NextResult() && dataReader.Read())
                {
                    productBrowseInfo.ReviewCount = (int)dataReader[0];
                }
                if (dataReader.NextResult())
                {
                    productBrowseInfo.DBConsultations = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                if (dataReader.NextResult() && dataReader.Read())
                {
                    productBrowseInfo.ConsultationCount = (int)dataReader[0];
                }
                if (dataReader.NextResult())
                {
                    DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        DataTable dataTable2 = dataTable.Clone();
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            bool flag = false;
                            if (dataTable2.Rows.Count > 0)
                            {
                                foreach (DataRow dataRow2 in dataTable2.Rows)
                                {
                                    if ((int)dataRow2["AttributeId"] == (int)dataRow["AttributeId"])
                                    {
                                        flag = true;
                                        DataRow dataRow3;
                                        (dataRow3 = dataRow2)["ValueStr"] = dataRow3["ValueStr"] + ", " + dataRow["ValueStr"];
                                    }
                                }
                            }
                            if (!flag)
                            {
                                DataRow dataRow4 = dataTable2.NewRow();
                                dataRow4["AttributeId"] = dataRow["AttributeId"];
                                dataRow4["AttributeName"] = dataRow["AttributeName"];
                                dataRow4["ValueStr"] = dataRow["ValueStr"];
                                dataTable2.Rows.Add(dataRow4);
                            }
                        }
                        productBrowseInfo.DbAttribute = dataTable2;
                    }
                }
                if (dataReader.NextResult())
                {
                    productBrowseInfo.DbSKUs = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                if (dataReader.NextResult())
                {
                    productBrowseInfo.DbCorrelatives = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                if (dataReader.NextResult())
                {
                    productBrowseInfo.DbCorrelatives.Merge(DataHelper.ConverDataReaderToDataTable(dataReader), true);
                }
                if (dataReader.NextResult())//销售排行
                {
                    productBrowseInfo.DBHotSale = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                if (dataReader.NextResult())//详情页广告图
                {
                    DataTable dtNotes3 = DataHelper.ConverDataReaderToDataTable(dataReader);
                    if (dtNotes3 != null && dtNotes3.Rows.Count > 0)
                    {
                        productBrowseInfo.CategoryNote3 = dtNotes3.Rows[0]["Notes3"].ToString();
                    }
                }
               
            }
            return productBrowseInfo;
        }
       
        public ProductBrowseInfo GetProductBrowseInfo(int productId, int? maxReviewNum, int? maxConsultationNum)
        {
            ProductBrowseInfo productBrowseInfo = new ProductBrowseInfo();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE Ecshop_Products SET VistiCounts = VistiCounts + 1 WHERE ProductId = @ProductId;");
            stringBuilder.Append(@" SELECT (select min(ISNull(t.TaxRate,0)) from Ecshop_Products p 
left join Ecshop_SKUs s on p.ProductId = s.ProductId 
left join Ecshop_ProductsCombination  com on com.CombinationSkuId = s.SkuId 
inner join Ecshop_Products comp on comp.ProductId = com.Productid 
left join Ecshop_TaxRate as t on comp.TaxRateId=t.taxId where p.ProductId=@ProductId ) as MinTaxRate,
(select max(ISNull(t.TaxRate,0)) from Ecshop_Products p 
left join Ecshop_SKUs s on p.ProductId = s.ProductId 
left join Ecshop_ProductsCombination  com on com.CombinationSkuId = s.SkuId 
inner join Ecshop_Products comp on comp.ProductId = com.Productid 
left join Ecshop_TaxRate as t on comp.TaxRateId=t.taxId where p.ProductId=@ProductId ) as MaxTaxRate,");

            stringBuilder.Append(@"case when p.SaleType=2 then ISNULL((select  sum(ISNull(t1.TaxRate,0)*com.Quantity*com.Price)/sum(com.Quantity*com.Price) from Ecshop_Products p1 
left join Ecshop_SKUs s1 on p1.ProductId = s1.ProductId 
left join Ecshop_ProductsCombination  com on com.CombinationSkuId = s1.SkuId 
inner join Ecshop_Products comp on comp.ProductId = com.Productid 
left join Ecshop_TaxRate as t1 on comp.TaxRateId=t1.taxId  where p1.ProductId=p.ProductId ),0)

else ISNULL(t.TaxRate, 0) end  AS TaxRate,");

            stringBuilder.Append("p.* ,ISNULL(IsDisable,0) as cIsDisable,ship.*,CASE WHEN BrandId IS NULL THEN NULL ELSE (SELECT BrandName FROM Ecshop_BrandCategories WHERE BrandId= p.BrandId) END AS BrandName,isnull(esu.SupplierName,'海美自营') SupplierName,esu.Logo AS SupplierLogo ");
            stringBuilder.Append(" FROM Ecshop_Products p left join  Ecshop_TaxRate as t on p.TaxRateId=t.taxId left join Ecshop_ShippingTemplates ship on p.TemplateId = ship.TemplateId");
            stringBuilder.Append(" LEFT JOIN Ecshop_Supplier AS esu ON p.SupplierId=esu.SupplierId ");
            stringBuilder.Append(" INNER JOIN  Ecshop_Categories AS ec ON p.CategoryId=ec.CategoryId  ");
            stringBuilder.Append("  where ProductId=@ProductId;");

            //
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                //stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,FactStock,CostPrice,DeductFee,");
                //stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
                //stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
                //stringBuilder.Append(" FROM Ecshop_SKUs s WHERE ProductId = @ProductId");

                stringBuilder.Append("SELECT skus.ProductId, skus.SkuId, s.AttributeId, s.ValueId, skus.SKU,skus.CostPrice,isnull(skus.FactStock,0) FactStock,skus.[Weight],skus.DeductFee,");

                //组合商品库存处理
                stringBuilder.Append(@"(select case when p.saletype=2 then   
(select min(s1.Stock/pc.Quantity)  from  dbo.Ecshop_SKUs s1 inner join

 Ecshop_ProductsCombination pc 
 
 on pc.Skuid=s1.Skuid

where pc.CombinationSkuId=s.Skuid)   

else s.stock end   

from dbo.Ecshop_SKUs s inner join

dbo.Ecshop_Products p on p.productid=s.productid where s.Skuid=skus.Skuid) as Stock,");
                stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
                stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
                stringBuilder.Append(" FROM Ecshop_SKUItems s right outer join Ecshop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId =@ProductId ORDER BY skus.SkuId");
            }
            else
            {
                //stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,FactStock,CostPrice, SalePrice,DeductFee FROM Ecshop_SKUs WHERE ProductId = @ProductId");
                //stringBuilder.Append("SELECT skus.ProductId, skus.SkuId, s.AttributeId, s.ValueId, skus.SKU, skus.SalePrice, skus.CostPrice, skus.Stock,isnull(skus.FactStock,0) FactStock,skus.[Weight],skus.DeductFee FROM Ecshop_SKUItems s right outer join Ecshop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId =@ProductId ORDER BY skus.SkuId");

                stringBuilder.Append("SELECT skus.ProductId, skus.SkuId, s.AttributeId, s.ValueId, skus.SKU, skus.SalePrice, skus.CostPrice,");
                //组合商品库存处理
                stringBuilder.Append(@"(select case when p.saletype=2 then   
(select min(s1.Stock/pc.Quantity)  from  dbo.Ecshop_SKUs s1 inner join

 Ecshop_ProductsCombination pc 
 
 on pc.Skuid=s1.Skuid

where pc.CombinationSkuId=s.Skuid)   

else s.stock end   

from dbo.Ecshop_SKUs s inner join

dbo.Ecshop_Products p on p.productid=s.productid where s.Skuid=skus.Skuid) as Stock,");
                 stringBuilder.Append("isnull(skus.FactStock,0) FactStock,skus.[Weight],skus.DeductFee FROM Ecshop_SKUItems s right outer join Ecshop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId =@ProductId ORDER BY skus.SkuId");
            }
            if (maxReviewNum.HasValue)
            {
                stringBuilder.AppendFormat(" SELECT TOP {0} * FROM Ecshop_ProductReviews where ProductId=@ProductId ORDER BY ReviewId DESC; ", maxReviewNum);
                stringBuilder.Append(" SELECT Count(*) FROM Ecshop_ProductReviews where ProductId=@ProductId and IsNull(IsType,0)=0 ; ");
            }
            else
            {
                stringBuilder.Append(" SELECT * FROM Ecshop_ProductReviews where ProductId=@ProductId ORDER BY ReviewId DESC; ");
                stringBuilder.Append(" SELECT Count(*) FROM Ecshop_ProductReviews where ProductId=@ProductId and IsNull(IsType,0)=0 ; ");
            }
            if (maxConsultationNum.HasValue)
            {
                stringBuilder.AppendFormat(" SELECT TOP {0} * FROM Ecshop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;", maxConsultationNum);
                stringBuilder.Append(" SELECT Count(*) FROM Ecshop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ; ");
            }
            else
            {
                stringBuilder.Append(" SELECT * FROM Ecshop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;");
                stringBuilder.Append(" SELECT Count(*) FROM Ecshop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ; ");
            }
            stringBuilder.Append(" SELECT a.AttributeId, AttributeName, ValueStr FROM Ecshop_ProductAttributes pa JOIN Ecshop_Attributes a ON pa.AttributeId = a.AttributeId");
            stringBuilder.Append(" JOIN Ecshop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
            stringBuilder.Append(" SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl FROM Ecshop_SKUItems s join Ecshop_Attributes a on s.AttributeId = a.AttributeId join Ecshop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Ecshop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;");
            stringBuilder.Append(" SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Ecshop_CDisableBrowseProductList");
            stringBuilder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId IN (SELECT RelatedProductId FROM Ecshop_RelatedProducts WHERE ProductId = {1}) ORDER BY DisplaySequence DESC;", 1, productId);

            stringBuilder.Append("SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Ecshop_CDisableBrowseProductList");
            stringBuilder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId<>{1}  AND CategoryId = (SELECT CategoryId FROM Ecshop_Products WHERE ProductId={1} AND SaleStatus = {0})", 1, productId);
            stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT RelatedProductId FROM Ecshop_RelatedProducts WHERE ProductId = {0})", productId);
            stringBuilder.AppendFormat(" AND productId IN (SELECT productId FROM Ecshop_ProductTag WHERE tagId={0})", 4);//推荐标签
            stringBuilder.AppendFormat(" ORDER BY DisplaySequence DESC;");

            stringBuilder.Append("SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Ecshop_CDisableBrowseProductList");
            stringBuilder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId<>{1}  AND CategoryId = (SELECT CategoryId FROM Ecshop_Products WHERE ProductId={1} AND SaleStatus = {0})", 1, productId);
            stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT RelatedProductId FROM Ecshop_RelatedProducts WHERE ProductId = {0})", productId);
            stringBuilder.AppendFormat(" ORDER BY ShowSaleCounts DESC;");//销售排行

            stringBuilder.AppendFormat("SELECT Notes3,CategoryId,Name  FROM Ecshop_Categories WHERE CategoryId = (SELECT CategoryId FROM Ecshop_Products WHERE ProductId={1} AND SaleStatus = {0})", 1, productId);//再来一个广告图片

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    productBrowseInfo.Product = DataMapper.PopulateProduct(dataReader);
                    if (dataReader["BrandName"] != DBNull.Value)
                    {
                        productBrowseInfo.BrandName = (string)dataReader["BrandName"];
                    }
                    if (dataReader["SupplierName"] != DBNull.Value)
                    {
                        productBrowseInfo.SupplierName = (string)dataReader["SupplierName"];
                    }
                    if (dataReader["SupplierLogo"] != DBNull.Value)
                    {
                        productBrowseInfo.SupplierLogo = (string)dataReader["SupplierLogo"];
                    }
                    if (dataReader["cIsDisable"] != DBNull.Value)
                    {
                        productBrowseInfo.cIsDisable = (Int32)dataReader["cIsDisable"];
                    }

                }
                if (dataReader.NextResult())
                {
                    while (dataReader.Read())
                    {
                        string key = (string)dataReader["SkuId"];
                        if (!productBrowseInfo.Product.Skus.ContainsKey(key))
                        {
                            productBrowseInfo.Product.Skus.Add(key, DataMapper.PopulateSKU(dataReader));
                        }
                        if (dataReader["AttributeId"] != DBNull.Value && dataReader["ValueId"] != DBNull.Value)
                        {
                            productBrowseInfo.Product.Skus[key].SkuItems.Add((int)dataReader["AttributeId"], (int)dataReader["ValueId"]);
                        }
                        //productBrowseInfo.Product.Skus.Add((string)dataReader["SkuId"], DataMapper.PopulateSKU(dataReader));
                    }
                }
                if (dataReader.NextResult())
                {
                    productBrowseInfo.DBReviews = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                if (dataReader.NextResult() && dataReader.Read())
                {
                    productBrowseInfo.ReviewCount = (int)dataReader[0];
                }
                if (dataReader.NextResult())
                {
                    productBrowseInfo.DBConsultations = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                if (dataReader.NextResult() && dataReader.Read())
                {
                    productBrowseInfo.ConsultationCount = (int)dataReader[0];
                }
                if (dataReader.NextResult())
                {
                    DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        DataTable dataTable2 = dataTable.Clone();
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            bool flag = false;
                            if (dataTable2.Rows.Count > 0)
                            {
                                foreach (DataRow dataRow2 in dataTable2.Rows)
                                {
                                    if ((int)dataRow2["AttributeId"] == (int)dataRow["AttributeId"])
                                    {
                                        flag = true;
                                        DataRow dataRow3;
                                        (dataRow3 = dataRow2)["ValueStr"] = dataRow3["ValueStr"] + ", " + dataRow["ValueStr"];
                                    }
                                }
                            }
                            if (!flag)
                            {
                                DataRow dataRow4 = dataTable2.NewRow();
                                dataRow4["AttributeId"] = dataRow["AttributeId"];
                                dataRow4["AttributeName"] = dataRow["AttributeName"];
                                dataRow4["ValueStr"] = dataRow["ValueStr"];
                                dataTable2.Rows.Add(dataRow4);
                            }
                        }
                        productBrowseInfo.DbAttribute = dataTable2;
                    }
                }
                if (dataReader.NextResult())
                {
                    productBrowseInfo.DbSKUs = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                if (dataReader.NextResult())
                {
                    productBrowseInfo.DbCorrelatives = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                if (dataReader.NextResult())
                {
                    productBrowseInfo.DbCorrelatives.Merge(DataHelper.ConverDataReaderToDataTable(dataReader), true);
                }
                if (dataReader.NextResult())//销售排行
                {
                    productBrowseInfo.DBHotSale = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                if (dataReader.NextResult())//详情页广告图
                {
                    DataTable dtNotes3 = DataHelper.ConverDataReaderToDataTable(dataReader);
                    if (dtNotes3 != null && dtNotes3.Rows.Count > 0)
                    {
                        productBrowseInfo.CategoryNote3 = dtNotes3.Rows[0]["Notes3"].ToString();
                        productBrowseInfo.CategoryId = dtNotes3.Rows[0]["CategoryId"].ToString();
                        productBrowseInfo.CategoryName = dtNotes3.Rows[0]["Name"].ToString();
                    }
                }
            }
            return productBrowseInfo;
        }
        public DbQueryResult GetBrowseProductList(ProductBrowseQuery query)
        {
            string filter = this.BuildProductBrowseQuerySearch(query);
            string text = "ShopName,ShopOwner,fastbuy_skuid,TaxRate,ProductId,ProductName,ProductCode, ShowSaleCounts AS SaleCounts, ShortDescription, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice, SalePrice,Stock,CategoryId,SupplierId, isnull(IsDisplayDisCount,0) as 'IsDisplayDisCount',Convert(decimal(10,1), (SalePrice/MarketPrice)*10) as 'DiscountRate',Icon,SupplierName";
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                text += string.Format(",CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", member.GradeId);
                text += string.Format("THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", member.GradeId, discount);
            }
            else
            {
                text += ",SalePrice as RankPrice";
            }

            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseProductList p", "ProductId", filter, text);

        }

        public DbQueryResult GetWMSBrowseProductList(ProductBrowseQuery query)
        {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" SupplierId IN(SELECT SupplierId FROM dbo.Ecshop_Supplier) and LEN(ProductRegistrationNumber)>0 ");

            if (query.SendWMSCount.HasValue)
            {
                stringBuilder.AppendFormat(" and SendWMSCount<={0} ", query.SendWMSCount.Value);
            }

            if (query.DateContrastType.HasValue)
            {
                if (query.DateContrastType.Value == 1)
                {
                    if (query.DateContrastValue.HasValue)
                    {
                        stringBuilder.AppendFormat(" AND UpdateDate BETWEEN DATEADD(hour,-{0},GETDATE())  AND GETDATE() ", query.DateContrastValue.Value);
                    }
                }
                else if (query.DateContrastType.Value == 2)
                {
                    if (query.DateContrastValue.HasValue)
                    {
                        stringBuilder.AppendFormat(" AND UpdateDate BETWEEN DATEADD(day,-{0},GETDATE())  AND GETDATE() ", query.DateContrastValue.Value);
                    }
                }
                else if (query.DateContrastType.Value == 2)
                {
                    if (query.DateContrastValue.HasValue)
                    {
                        stringBuilder.AppendFormat(" AND UpdateDate BETWEEN DATEADD(month,-{0},GETDATE())  AND GETDATE() ", query.DateContrastValue.Value);
                    }
                }
            }
            //
            if (query.DataVersion != "")
            {
                stringBuilder.Append(" and (IsSendWMS=0 OR IsSendWMS IS NULL) ");
            }

            string text = " ProductId,ProductName,ProductCode,SupplierId,UpdateDate,IsSendWMS,SaleStatus,BarCode,DisplaySequence,SkuId,SKU,[Weight],[GrossWeight],Stock,CostPrice,SalePrice,ProductRegistrationNumber,strAttName ";

            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_WMSProductSkuList p", "ProductId", stringBuilder.ToString(), text);

        }


        public DataTable GetProductInfoByPId(string productId)
        {
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr FROM Ecshop_SKUs s left join Ecshop_SKUItems si on s.SkuId = si.SkuId left join Ecshop_Attributes a on si.AttributeId = a.AttributeId left join Ecshop_AttributeValues av on si.ValueId = av.ValueId WHERE ProductId=@productId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.String, productId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        public DbQueryResult GetCurrBrowseProductList(ProductBrowseQuery query)
        {
            string filter = this.BuildCurrProductBrowseQuerySearch(query);
            string text = "BrandId,ShopName,ShopOwner,SupplierId,SupplierName,fastbuy_skuid,TaxRate,ProductId,ProductName,ProductCode, ShowSaleCounts AS SaleCounts, ShortDescription, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice, SalePrice,Stock,CategoryId,ShippingMode,CollectCount,IsCustomsClearance,IsfreeShipping,VistiCounts,BuyCardinality,Icon,IsPromotion,SaleType,MinTaxRate,MaxTaxRate,Convert(decimal(10,1), (SalePrice/MarketPrice)*10) as 'DiscountRate', isnull(IsDisplayDisCount,0) as 'IsDisplayDisCount'";
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                text += string.Format(",CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", member.GradeId);
                text += string.Format("THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", member.GradeId, discount);
            }
            else
            {
                text += ",SalePrice as RankPrice";
            }

            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                text += string.Format(",(select Name from Ecshop_Promotions pm  left join Ecshop_PromotionMemberGrades pg on pm.ActivityId=pg.ActivityId where pm.ActivityId=p.ActivityId  and pg.GradeId={0}) as PromotionName", member.GradeId);
            }

            else
            {
                text += ",'' as PromotionName";
            }

            if (query.TopId.HasValue && query.TopId.Value > 0)
            {
                return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseTopicProductList p", "ProductId", filter, text);
            }
            else
            {
                //return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseProductList p", "ProductId", filter, text);
                //add by xfteng；2016-01-12，新增视图，增加了前台对商品分类的过滤
                return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_CDisableBrowseProductList p", "ProductId", filter, text);
            }


            //return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseProductList p", "ProductId", filter, text);
        }


        /// <summary>
        /// 根据筛选条件获取所有的商品，不做分页
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetAllProductList(ProductBrowseQuery query)
        {
            string filter = this.BuildCurrProductBrowseQuerySearch(query);
            string text = "BrandId,ShopName,ShopOwner,SupplierId,SupplierName,fastbuy_skuid,TaxRate,ProductId,ProductName,ProductCode, ShowSaleCounts AS SaleCounts, ShortDescription, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice, SalePrice,Stock,CategoryId,ShippingMode,CollectCount,IsCustomsClearance,IsfreeShipping,VistiCounts,BuyCardinality,Icon,IsPromotion,SaleType,MinTaxRate,MaxTaxRate,Convert(decimal(10,1), (SalePrice/MarketPrice)*10) as 'DiscountRate', isnull(IsDisplayDisCount,0) as 'IsDisplayDisCount'";
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                text += string.Format(",CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", member.GradeId);
                text += string.Format("THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", member.GradeId, discount);
            }
            else
            {
                text += ",SalePrice as RankPrice";
            }

            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member member = HiContext.Current.User as Member;
                text += string.Format(",(select Name from Ecshop_Promotions pm  left join Ecshop_PromotionMemberGrades pg on pm.ActivityId=pg.ActivityId where pm.ActivityId=p.ActivityId  and pg.GradeId={0}) as PromotionName", member.GradeId);
            }

            else
            {
                text += ",'' as PromotionName";
            }

            
            string sortorder="";
            if(query.SortOrder==SortAction.Asc)
            {
                sortorder="asc";
            }
            else
            {
                sortorder="desc";
            }
            if (query.TopId.HasValue && query.TopId.Value > 0)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select " + text + " from vw_Ecshop_BrowseTopicProductList p where " + filter + " order by " + query.SortBy + " " + sortorder);
                DataTable result;
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    result = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                return result;
            }
            else
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select " + text + " from vw_Ecshop_CDisableBrowseProductList p where " + filter + " order by " + query.SortBy + " " + sortorder);
                DataTable result;
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    result = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                return result;
            }
        }
        public ProductBrowseInfo GetBrowseHotProductList(int categoryId, int brandId)
        {
            ProductBrowseInfo productBrowseInfo = new ProductBrowseInfo();
            StringBuilder stringBuilder = new StringBuilder();
//            stringBuilder.Append(@"SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice
//                                     FROM vw_Ecshop_BrowseProductList
//                                     WHERE SaleStatus =1 
//                                    ");

            stringBuilder.Append(@"SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice
                                    FROM vw_Ecshop_BrowseProductList
                                    WHERE SaleStatus =1 ");
            if (categoryId > 0)
            {
                stringBuilder.Append(@" AND  CategoryId in
                                     (
                                      select CategoryId from dbo.Ecshop_Categories WHERE Path LIKE '%" + categoryId + @"%'
                                     )");
            }
            if (brandId > 0)
            {
                stringBuilder.Append(@" AND BrandId=" + brandId);
            }
            stringBuilder.Append(" ORDER BY ShowSaleCounts DESC");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                productBrowseInfo.DBHotSale = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return productBrowseInfo;

        }


        /// <summary>
        /// 门店商品销量排行
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="brandId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public ProductBrowseInfo GetBrowseHotProductList(int categoryId, int brandId, int supplierId)
        {
            ProductBrowseInfo productBrowseInfo = new ProductBrowseInfo();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@"SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice
                                     FROM vw_Ecshop_BrowseProductList
                                     WHERE SaleStatus =1 
                                    ");
            if (categoryId > 0)
            {
                stringBuilder.Append(@" AND  CategoryId in
                                     (
                                      select CategoryId from dbo.Ecshop_Categories WHERE Path LIKE '%" + categoryId + @"%'
                                     )");
            }
            if (brandId > 0)
            {
                stringBuilder.Append(@" AND BrandId=" + brandId);
            }

            if (supplierId > 0)
            {
                stringBuilder.Append(@" AND supplierid=" + supplierId);
            }
            stringBuilder.Append(" ORDER BY ShowSaleCounts DESC");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                productBrowseInfo.DBHotSale = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return productBrowseInfo;

        }
        /// <summary>
        /// 查询搜索集合中的品牌，原产地
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetBrowseProductImportSourceIdAndBrandIdList(ProductBrowseQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ImportSourceId,BrandId FROM vw_Ecshop_BrowseTopicProductList WHERE 1=1 AND SaleStatus = 1 ");
            //stringBuilder.Append(" FROM vw_Ecshop_BrowseProductList WHERE "+filter);
            if (!string.IsNullOrEmpty(query.Keywords) && query.Keywords.Trim().Length > 0)
            {
                if (!query.IsPrecise)
                {
                    query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                    string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
                    List<string> list = new List<string>();
                    list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[0])));
                    int num = 1;
                    while (num < array.Length && num <= 4)
                    {
                        list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[num])));
                        num++;
                    }
                    stringBuilder.Append(" and (" + string.Join(" and ", list.ToArray()) + ")");
                }
                else
                {
                    stringBuilder.AppendFormat(" AND (ProductName = '{0}' or ShopName = '{0}')", DataHelper.CleanSearchString(query.Keywords));
                }
            }
            if (query.CategoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(query.CategoryId.Value);
                if (category != null)
                {
                    stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
                else
                {
                    stringBuilder.Append(" and 1>2 ");
                }
            }
            stringBuilder.Append(" group by ImportSourceId,BrandId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public DataTable GetVistiedProducts(IList<int> productIds)
        {
            DataTable result;
            if (productIds.Count == 0)
            {
                result = null;
            }
            else
            {
                string productId = string.Join(",", productIds.ToArray());
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,MarketPrice,SalePrice  FROM  vw_Ecshop_BrowseTopicProductList WHERE ProductId IN({0}) and SaleStatus=1", productId));
                DataTable dataTable;
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                result = dataTable;
            }
            return result;
        }

        public DataTable GetLimitProducts(IList<int> productIds)
        {
            DataTable result;
            if (productIds.Count == 0)
            {
                result = null;
            }
            else
            {
                string productId = string.Join(",", productIds.ToArray());
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,MarketPrice,SalePrice  FROM  vw_Ecshop_BrowseProductList WHERE ProductId IN({0}) and SaleStatus=1", productId));
                DataTable dataTable;
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                result = dataTable;
            }
            return result;
        }

        public DataTable GetSuggestProductsProducts(IList<int> productIds, int count)
        {
            DataTable result;
            if (productIds.Count == 0)
            {
                result = null;
            }
            else
            {
                int userId = HiContext.Current.User.UserId;
                string productId = string.Join(",", productIds.ToArray());
                DbCommand command = this.database.GetStoredProcCommand("cp_User_SuggestProducts_Get");
                this.database.AddInParameter(command, "UserId", DbType.Int32, userId);
                this.database.AddInParameter(command, "Count", DbType.Int32, count);
                this.database.AddInParameter(command, "productId", DbType.String, productId);

                using (IDataReader dataReader = this.database.ExecuteReader(command))
                {
                    result = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
            }
            return result;
        }

        /// <summary>
        /// 随机获取商品信息
        /// </summary>
        /// <param name="count">获取商品条数</param>
        /// <param name="between">跳过多少</param>
        /// <param name="and">取多少</param>
        /// <returns></returns>
        public DataTable GetVistiedProducts(int count, int minNum, int maxNum)
        {
            DataTable result;
            DbCommand command = this.database.GetStoredProcCommand("cp_Index_SuggestProductsPage_Get");
            this.database.AddInParameter(command, "TopNum", DbType.Int32, count);
            this.database.AddInParameter(command, "MinNum", DbType.Int32, minNum);
            this.database.AddInParameter(command, "MaxNum", DbType.Int32, maxNum);

            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }

            return result;
            /*DataTable result;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT TOP {0} ProductId,ProductName,ProductCode,SaleCounts, ShortDescription,ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,MarketPrice,SalePrice FROM (SELECT ROW_NUMBER() over(order by p.productId) AS row_num,p.ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts, ShortDescription,ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,MarketPrice,SalePrice  FROM  vw_Ecshop_BrowseProductList  p inner join Ecshop_SuggestProducts s on p.ProductId = s.ProductId where SaleStatus=1 and s.Client={3}) as A WHERE a.row_num BETWEEN {1} AND {2} ORDER BY ProductId ASC", count, between, and, (int)clientType));
            DataTable dataTable;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            result = dataTable;
            return result;*/
        }
        public DataTable GetLimitProducts(int count, int minNum, int maxNum)
        {
            DataTable result;
            DbCommand command = this.database.GetStoredProcCommand("cp_Index_LimitProductsPage_Get");
            this.database.AddInParameter(command, "TopNum", DbType.Int32, count);
            this.database.AddInParameter(command, "MinNum", DbType.Int32, minNum);
            this.database.AddInParameter(command, "MaxNum", DbType.Int32, maxNum);

            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }

            return result;
            /*DataTable result;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT TOP {0} ProductId,ProductName,ProductCode,SaleCounts, ShortDescription,ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,MarketPrice,SalePrice FROM (SELECT ROW_NUMBER() over(order by p.productId) AS row_num,p.ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts, ShortDescription,ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,MarketPrice,SalePrice  FROM  vw_Ecshop_BrowseProductList  p inner join Ecshop_SuggestProducts s on p.ProductId = s.ProductId where SaleStatus=1 and s.Client={3}) as A WHERE a.row_num BETWEEN {1} AND {2} ORDER BY ProductId ASC", count, between, and, (int)clientType));
            DataTable dataTable;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            result = dataTable;
            return result;*/
        }
        public DataTable GetTopicProducts(int topicid, int maxNum)
        {
            Member member = HiContext.Current.User as Member;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select top " + maxNum);
            stringBuilder.Append(" p.ProductId, ProductCode, ProductName,ShortDescription,ShowSaleCounts,ThumbnailUrl40,ThumbnailUrl100,ThumbnailUrl160,MarketPrice,CategoryId,MainCategoryPath,IsCustomsClearance,ShippingMode,IsfreeShipping,");
            if (member != null)
            {
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", member.GradeId);
                stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice, ", member.GradeId, discount);
            }
            else
            {
                stringBuilder.Append("SalePrice,");
            }
            stringBuilder.Append("SaleCounts, Stock,t.DisplaySequence from vw_Ecshop_BrowseTopicProductList p inner join  Vshop_RelatedTopicProducts t on p.productid=t.RelatedProductId where t.topicid=" + topicid);
            stringBuilder.AppendFormat(" and SaleStatus = {0}", 1);
            stringBuilder.AppendFormat(" and IsApproved = {0}", 1);
            stringBuilder.Append(" order by t.DisplaySequence asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
        public DbQueryResult GetBrandProducts(int? brandId, ProductBrowseQuery query)//新增分页查询
        {
            Member member = HiContext.Current.User as Member;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,ShowSaleCounts,fastbuy_skuid,");
            stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,TaxRate,CategoryId,MainCategoryPath,IsCustomsClearance,ShippingMode,IsfreeShipping,");
            if (member != null)
            {
                stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, GETDATE()) >= 0 AND DATEDIFF(DD, EndDate, GETDATE()) <= 0
		                    and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=p.ProductId)
		                    AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = " + member.GradeId + ")),0) as ActivityId,");
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
                stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, getdate()) >= 0 AND DATEDIFF(DD, EndDate, getdate()) <= 0
		             and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=p.ProductId)
	                 AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades)),0) as ActivityId,");
                stringBuilder.Append("SalePrice");
            }
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append(" SaleStatus=1");
            if (brandId.HasValue)
            {
                stringBuilder2.AppendFormat(" AND BrandId = {0}", brandId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseTopicProductList p", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
            //DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, "DisplaySequence", SortAction.Desc, true, "vw_Ecshop_BrowseProductList s", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
            //DataTable result = (DataTable)dbQueryResult.Data;
            //total = dbQueryResult.TotalRecords;
        }
        public DataTable GetBrandProducts(int? brandId, int pageNumber, int maxNum, out int total)
        {
            Member member = HiContext.Current.User as Member;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,ShowSaleCounts,");
            stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,CategoryId,MainCategoryPath,IsCustomsClearance,ShippingMode,IsfreeShipping,");
            if (member != null)
            {
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
                stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                stringBuilder.Append("SalePrice");
            }
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append(" SaleStatus=1");
            if (brandId.HasValue)
            {
                stringBuilder2.AppendFormat(" AND BrandId = {0}", brandId);
            }
            DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, "DisplaySequence", SortAction.Desc, true, "vw_Ecshop_BrowseTopicProductList s", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
            DataTable result = (DataTable)dbQueryResult.Data;
            total = dbQueryResult.TotalRecords;
            return result;
        }

        public DataTable GetProducts(int? topicId, int? categoryId, int? brandId, int? originPlaceId, bool? isFreeShip, string keyword, int pageNumber, int maxNum, int? gradeId, out int toal, string sort, bool isAsc = false)
        {
            StringBuilder sbSql = new StringBuilder();

            sbSql.AppendFormat("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,Stock,ShortDescription,", maxNum);
            sbSql.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,VistiCounts,TaxRate,fastbuy_skuid,CategoryId,MainCategoryPath,IsCustomsClearance,ShippingMode,IsfreeShipping,");

            if (gradeId.HasValue)
            {
                int discount = new MemberGradeDao().GetMemberGrade(gradeId.Value).Discount;
                if (topicId.HasValue)
                {
                    sbSql.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_BrowseTopicProductList.SkuId AND GradeId = {0}) = 1", gradeId.Value);
                    sbSql.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_BrowseTopicProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", gradeId.Value, discount);
                }
                else
                {
                    sbSql.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_CDisableBrowseProductList.SkuId AND GradeId = {0}) = 1", gradeId.Value);
                    sbSql.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_CDisableBrowseProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", gradeId.Value, discount);
                }
            }
            else
            {
                sbSql.Append("SalePrice");
            }

            StringBuilder sbSql2 = new StringBuilder();
            sbSql2.Append(" SaleStatus=1");

            if (topicId.HasValue)
            {
                sbSql2.AppendFormat(" AND TopicId = {0} ", topicId.Value);
                //stringBuilder2.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Vshop_RelatedTopicProducts WHERE TopicId = {0})", topicId.Value);
            }
            if (categoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
                if (category != null)
                {
                    sbSql2.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
            }
            if (brandId.HasValue)
            {
                sbSql2.AppendFormat(" AND (BrandId = {0})", brandId.Value);
            }
            if (originPlaceId.HasValue)
            {
                sbSql2.AppendFormat(" AND (ImportSourceId = {0})", originPlaceId.Value);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = DataHelper.CleanSearchString(keyword);
                sbSql2.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%'  OR Meta_Keywords LIKE '%{0}%' or ShopName like '%{0}%')", keyword);
            }
            if (isFreeShip.HasValue)
            {
                if (isFreeShip.Value)
                {
                    sbSql2.AppendFormat(" AND (IsfreeShipping = {0})", 1);
                }
            }
            if (string.IsNullOrWhiteSpace(sort))
            {
                sort = "ProductId";
            }
            DbQueryResult dbQueryResult = null;
            if (topicId.HasValue)
            {
                dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Ecshop_BrowseTopicProductList", "ProductId", sbSql2.ToString(), sbSql.ToString());
            }
            else
            {
                dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Ecshop_CDisableBrowseProductList", "ProductId", sbSql2.ToString(), sbSql.ToString());
            }
            DataTable result = (DataTable)dbQueryResult.Data;
            toal = dbQueryResult.TotalRecords;
            return result;
        }

        public DataTable GetProducts(int? topicId, int? categoryId, string keyWord, int pageNumber, int maxNum, out int toal, string sort, bool isAsc = false)
        {
            Member member = HiContext.Current.User as Member;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SupplierId,SupplierName,ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,Stock,ShortDescription,", maxNum);
            stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,VistiCounts,TaxRate,fastbuy_skuid,CategoryId,MainCategoryPath,IsCustomsClearance,ShippingMode,IsfreeShipping,");
            if (member != null)
            {
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                if (topicId.HasValue)
                {
                    stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_BrowseTopicProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
                    stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_BrowseTopicProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
                }
                else
                {
                    stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_CDisableBrowseProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
                    stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_CDisableBrowseProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
                }
            }
            else
            {
                stringBuilder.Append("SalePrice");
            }
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append(" SaleStatus=1");
            if (topicId.HasValue)
            {
                stringBuilder2.AppendFormat(" AND TopicId = {0} ", topicId.Value);
                //stringBuilder2.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Vshop_RelatedTopicProducts WHERE TopicId = {0})", topicId.Value);
            }
            if (categoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
                if (category != null)
                {
                    stringBuilder2.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                keyWord = DataHelper.CleanSearchString(keyWord);
                stringBuilder2.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%' or ShopName like '%{0}%')", keyWord);
            }
            if (string.IsNullOrWhiteSpace(sort))
            {
                sort = "ProductId";
            }
            DbQueryResult dbQueryResult = null;
            if (topicId.HasValue)
            {
                dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Ecshop_BrowseTopicProductList", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
            }
            else
            {
                dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Ecshop_CDisableBrowseProductList", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
            }
            DataTable result = (DataTable)dbQueryResult.Data;
            toal = dbQueryResult.TotalRecords;
            return result;
        }

        public DataTable GetProducts(int? topicId, int? categoryId, string keyWord, int pageNumber, int maxNum, out int toal, string sort, bool isAnonymous, bool isAsc = false)
        {
            Member member = HiContext.Current.User as Member;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SupplierId,SupplierName,ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,Stock,ShortDescription,", maxNum);
            stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,VistiCounts,TaxRate,fastbuy_skuid,CategoryId,MainCategoryPath,IsCustomsClearance,ShippingMode,IsfreeShipping,");


            if (isAnonymous)
            {
                if (topicId.HasValue)
                {
                    stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, getdate()) >= 0 AND DATEDIFF(DD, EndDate, getdate()) <= 0
		             and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_BrowseTopicProductList.ProductId)
	                 AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades)),0) as ActivityId,");
                }
                else
                {
                    stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, getdate()) >= 0 AND DATEDIFF(DD, EndDate, getdate()) <= 0
		             and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_CDisableBrowseProductList.ProductId)
	                 AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades)),0) as ActivityId,");
                }
            }
            else
            {
                if (member != null)
                {
                    if (topicId.HasValue)
                    {
                        stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, GETDATE()) >= 0 AND DATEDIFF(DD, EndDate, GETDATE()) <= 0
		                    and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_BrowseTopicProductList.ProductId)
		                    AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = " + member.GradeId + ")),0) as ActivityId,");
                    }
                    else
                    {
                        stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, GETDATE()) >= 0 AND DATEDIFF(DD, EndDate, GETDATE()) <= 0
		                    and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_CDisableBrowseProductList.ProductId)
		                    AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = " + member.GradeId + ")),0) as ActivityId,");
                    }
                }
            }


            if (member != null)
            {
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                if (topicId.HasValue)
                {
                    stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_BrowseTopicProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
                    stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_BrowseTopicProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
                }
                else
                {
                    stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_CDisableBrowseProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
                    stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_CDisableBrowseProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
                }
            }
            else
            {
                stringBuilder.Append("SalePrice");
            }
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append(" SaleStatus=1");
            stringBuilder2.Append(" AND  IsApproved=1");//加入审核状态
            if (topicId.HasValue)
            {
                stringBuilder2.AppendFormat(" AND TopicId = {0} ", topicId.Value);
                //stringBuilder2.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Vshop_RelatedTopicProducts WHERE TopicId = {0})", topicId.Value);
            }
            if (categoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
                if (category != null)
                {
                    stringBuilder2.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                keyWord = DataHelper.CleanSearchString(keyWord);
                stringBuilder2.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%'  OR Meta_Keywords LIKE '%{0}%' or ShopName like '%{0}%')", keyWord);
            }
            if (string.IsNullOrWhiteSpace(sort))
            {
                sort = "ProductId";
            }
            DbQueryResult dbQueryResult = null;
            if (topicId.HasValue)
            {
                dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Ecshop_BrowseTopicProductList", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
            }
            else
            {
                dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Ecshop_CDisableBrowseProductList", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
            }
            DataTable result = (DataTable)dbQueryResult.Data;
            toal = dbQueryResult.TotalRecords;
            return result;
        }


        public DataTable GetProducts(int? topicId, int? categoryId, int? supplierId, string keyWord, int pageNumber, int maxNum, out int toal, string sort, bool isAnonymous, bool isAsc = false)
        {
            Member member = HiContext.Current.User as Member;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("ShopName,ShopOwner,SupplierId,SupplierName,ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,Stock,ShortDescription,", maxNum);
            stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,VistiCounts,TaxRate,fastbuy_skuid,CategoryId,MainCategoryPath,IsCustomsClearance,ShippingMode,IsfreeShipping,IsPromotion,IsDisplayDiscount,Icon,");


            if (isAnonymous)
            {
                if (topicId.HasValue)
                {
                    stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, getdate()) >= 0 AND DATEDIFF(DD, EndDate, getdate()) <= 0
		             and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_BrowseTopicProductList.ProductId)
	                 AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades)),0) as ActivityId,");
                }
                else
                {
                    stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, getdate()) >= 0 AND DATEDIFF(DD, EndDate, getdate()) <= 0
		             and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_CDisableBrowseProductList.ProductId)
	                 AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades)),0) as ActivityId,");
                }
            }
            else
            {
                if (member != null)
                {
                    if (topicId.HasValue)
                    {
                        stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, GETDATE()) >= 0 AND DATEDIFF(DD, EndDate, GETDATE()) <= 0
		                    and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_BrowseTopicProductList.ProductId)
		                    AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = " + member.GradeId + ")),0) as ActivityId,");
                    }
                    else
                    {
                        stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, GETDATE()) >= 0 AND DATEDIFF(DD, EndDate, GETDATE()) <= 0
		                    and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_CDisableBrowseProductList.ProductId)
		                    AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = " + member.GradeId + ")),0) as ActivityId,");
                    }
                }
            }


            if (member != null)
            {
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                if (topicId.HasValue)
                {
                    stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_BrowseTopicProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
                    stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_BrowseTopicProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
                }
                else
                {
                    stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_CDisableBrowseProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
                    stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_CDisableBrowseProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
                }
            }
            else
            {
                stringBuilder.Append("SalePrice");
            }
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append(" SaleStatus=1");
            stringBuilder2.Append(" AND  IsApproved=1");//加入审核状态
            if (topicId.HasValue)
            {
                stringBuilder2.AppendFormat(" AND TopicId = {0} ", topicId.Value);
                //stringBuilder2.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Vshop_RelatedTopicProducts WHERE TopicId = {0})", topicId.Value);
            }
            if (categoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
                if (category != null)
                {
                    stringBuilder2.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
            }

            if (supplierId.HasValue && supplierId > 0)
            {
                stringBuilder2.AppendFormat(" AND supplierId = {0} ", supplierId.Value);
            }


            if (!string.IsNullOrEmpty(keyWord))
            {
                keyWord = DataHelper.CleanSearchString(keyWord);
                stringBuilder2.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%'  OR Meta_Keywords LIKE '%{0}%' or ShopName like '%{0}%')", keyWord);
            }
            if (string.IsNullOrWhiteSpace(sort))
            {
                sort = "ProductId";
            }
            DbQueryResult dbQueryResult = null;
            if (topicId.HasValue)
            {
                dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Ecshop_BrowseTopicProductList", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
            }
            else
            {
                dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Ecshop_CDisableBrowseProductList", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
            }
            DataTable result = (DataTable)dbQueryResult.Data;
            toal = dbQueryResult.TotalRecords;
            return result;
        }

        public DataTable GetProducts(int? topicId, int? categoryId, int? supplierId, string keyWord, int pageNumber, int maxNum, out int toal, string sort, bool isAnonymous, int? brandid, int? importsourceid, bool isAsc = false)
        {
            Member member = HiContext.Current.User as Member;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("ShopName,ShopOwner,SupplierId,SupplierName,ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,Stock,ShortDescription,", maxNum);
            stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,VistiCounts,TaxRate,fastbuy_skuid,CategoryId,MainCategoryPath,IsCustomsClearance,ShippingMode,IsfreeShipping,IsPromotion,IsDisplayDiscount,Icon,");


            if (isAnonymous)
            {
                if (topicId.HasValue)
                {
                    stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, getdate()) >= 0 AND DATEDIFF(DD, EndDate, getdate()) <= 0
		             and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_BrowseTopicProductList.ProductId)
	                 AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades)),0) as ActivityId,");
                }
                else
                {
                    stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, getdate()) >= 0 AND DATEDIFF(DD, EndDate, getdate()) <= 0
		             and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_CDisableBrowseProductList.ProductId)
	                 AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades)),0) as ActivityId,");
                }
            }
            else
            {
                if (member != null)
                {
                    if (topicId.HasValue)
                    {
                        stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, GETDATE()) >= 0 AND DATEDIFF(DD, EndDate, GETDATE()) <= 0
		                    and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_BrowseTopicProductList.ProductId)
		                    AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = " + member.GradeId + ")),0) as ActivityId,");
                    }
                    else
                    {
                        stringBuilder.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, GETDATE()) >= 0 AND DATEDIFF(DD, EndDate, GETDATE()) <= 0
		                    and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_CDisableBrowseProductList.ProductId)
		                    AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = " + member.GradeId + ")),0) as ActivityId,");
                    }
                }
            }


            if (member != null)
            {
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                if (topicId.HasValue)
                {
                    stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_BrowseTopicProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
                    stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_BrowseTopicProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
                }
                else
                {
                    stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_CDisableBrowseProductList.SkuId AND GradeId = {0}) = 1", member.GradeId);
                    stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_CDisableBrowseProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
                }
            }
            else
            {
                stringBuilder.Append("SalePrice");
            }
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append(" SaleStatus=1");
            stringBuilder2.Append(" AND  IsApproved=1");//加入审核状态
            if (topicId.HasValue)
            {
                stringBuilder2.AppendFormat(" AND TopicId = {0} ", topicId.Value);
                //stringBuilder2.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Vshop_RelatedTopicProducts WHERE TopicId = {0})", topicId.Value);
            }
            if (categoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
                if (category != null)
                {
                    stringBuilder2.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
            }

            if (supplierId.HasValue && supplierId > 0)
            {
                stringBuilder2.AppendFormat(" AND supplierId = {0} ", supplierId.Value);
            }

            if (brandid.HasValue && brandid > 0)
            {
                stringBuilder2.AppendFormat(" AND BrandId in ({0}) ", brandid);
            }
            if (importsourceid.HasValue && importsourceid > 0)
            {
                stringBuilder2.AppendFormat(" AND ImportsourceId in ({0}) ", importsourceid);
            }

            if (!string.IsNullOrEmpty(keyWord))
            {
                keyWord = DataHelper.CleanSearchString(keyWord);
                stringBuilder2.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%'  OR Meta_Keywords LIKE '%{0}%' or ShopName like '%{0}%')", keyWord);
            }
            if (string.IsNullOrWhiteSpace(sort))
            {
                sort = "ProductId";
            }
            DbQueryResult dbQueryResult = null;
            if (topicId.HasValue)
            {
                dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Ecshop_BrowseTopicProductList", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
            }
            else
            {
                dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Ecshop_CDisableBrowseProductList", "ProductId", stringBuilder2.ToString(), stringBuilder.ToString());
            }
            DataTable result = (DataTable)dbQueryResult.Data;
            toal = dbQueryResult.TotalRecords;
            return result;
        }

        public DataTable GetProducts(int? topicId, int? categoryId, string keyWord, int pageNumber, int maxNum, out int toal, string sort, bool isAnonymous, int? gradeId, bool isAsc = false)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,Stock,ShortDescription,", maxNum);
            sbSql.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,VistiCounts,TaxRate,fastbuy_skuid,CategoryId,MainCategoryPath,IsCustomsClearance,ShippingMode,IsfreeShipping,");


            if (isAnonymous)
            {
                if (topicId.HasValue)
                {
                    sbSql.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, getdate()) >= 0 AND DATEDIFF(DD, EndDate, getdate()) <= 0
		             and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_BrowseTopicProductList.ProductId)
	                 AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades)),0) as ActivityId,");
                }
                else
                {
                    sbSql.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, getdate()) >= 0 AND DATEDIFF(DD, EndDate, getdate()) <= 0
		             and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_CDisableBrowseProductList.ProductId)
	                 AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades)),0) as ActivityId,");
                }
            }
            else
            {
                if (gradeId.HasValue)
                {
                    if (topicId.HasValue)
                    {
                        sbSql.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, GETDATE()) >= 0 AND DATEDIFF(DD, EndDate, GETDATE()) <= 0
		                    and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_BrowseTopicProductList.ProductId)
		                    AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = " + gradeId.Value.ToString() + ")),0) as ActivityId,");
                    }
                    else
                    {
                        sbSql.Append(@"isnull((SELECT top 1 ActivityId FROM Ecshop_Promotions WHERE DATEDIFF(DD, StartDate, GETDATE()) >= 0 AND DATEDIFF(DD, EndDate, GETDATE()) <= 0
		                    and ActivityId=(SELECT top 1 ActivityId FROM dbo.Ecshop_PromotionProducts WHERE productid=vw_Ecshop_CDisableBrowseProductList.ProductId)
		                    AND ActivityId IN (SELECT ActivityId FROM Ecshop_PromotionMemberGrades WHERE GradeId = " + gradeId.Value.ToString() + ")),0) as ActivityId,");
                    }
                }
            }


            if (gradeId.HasValue)
            {
                int discount = new MemberGradeDao().GetMemberGrade(gradeId.Value).Discount;
                if (topicId.HasValue)
                {
                    sbSql.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_BrowseTopicProductList.SkuId AND GradeId = {0}) = 1", gradeId.Value);
                    sbSql.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_BrowseTopicProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", gradeId.Value, discount);
                }
                else
                {
                    sbSql.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_CDisableBrowseProductList.SkuId AND GradeId = {0}) = 1", gradeId.Value);
                    sbSql.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = vw_Ecshop_CDisableBrowseProductList.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", gradeId.Value, discount);
                }
            }
            else
            {
                sbSql.Append("SalePrice");
            }
            StringBuilder sbSql2 = new StringBuilder();
            sbSql2.Append(" SaleStatus=1");
            sbSql2.Append(" AND  IsApproved=1");//加入审核状态
            if (topicId.HasValue)
            {
                sbSql2.AppendFormat(" AND TopicId = {0} ", topicId.Value);
                //stringBuilder2.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Vshop_RelatedTopicProducts WHERE TopicId = {0})", topicId.Value);
            }
            if (categoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(categoryId.Value);
                if (category != null)
                {
                    sbSql2.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                keyWord = DataHelper.CleanSearchString(keyWord);
                sbSql2.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%'  OR Meta_Keywords LIKE '%{0}%' or ShopName like '%{0}%')", keyWord);
            }
            if (string.IsNullOrWhiteSpace(sort))
            {
                sort = "ProductId";
            }
            DbQueryResult dbQueryResult = null;
            if (topicId.HasValue)
            {
                dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Ecshop_BrowseTopicProductList", "ProductId", sbSql2.ToString(), sbSql.ToString());
            }
            else
            {
                dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, isAsc ? SortAction.Asc : SortAction.Desc, true, "vw_Ecshop_CDisableBrowseProductList", "ProductId", sbSql2.ToString(), sbSql.ToString());
            }
            DataTable result = (DataTable)dbQueryResult.Data;
            toal = dbQueryResult.TotalRecords;
            return result;
        }


        public DbQueryResult GetBatchBuyProducts(ProductQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" SaleStatus = 1 ");
            if (query.BrandId.HasValue)
            {
                stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
            }
            if (query.TypeId.HasValue)
            {
                stringBuilder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
            }
            if (query.TagId.HasValue)
            {
                stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Ecshop_ProductTag WHERE TagId={0})", query.TagId);
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
                stringBuilder.AppendFormat(" AND (ProductName LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[0]));
                int num = 1;
                while (num < array.Length && num <= 4)
                {
                    stringBuilder.AppendFormat("AND (ProductName LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[num]));
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
            string selectFields = "ProductId, ProductCode,ProductName,ThumbnailUrl100";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_CDisableBrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
        }
        protected string BuildProductBrowseQuerySearch(ProductBrowseQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SaleStatus = {0}", (int)query.ProductSaleStatus);
            stringBuilder.AppendFormat(" AND SaleStatus = {0}", 1);
            if (query.IsApproved.HasValue)
            {
                stringBuilder.AppendFormat(" AND IsApproved = {0}", query.IsApproved.Value);
            }
            if (!query.IsPrecise)
            {
                if (!string.IsNullOrEmpty(query.ProductCode))
                {
                    stringBuilder.AppendFormat(" AND LOWER(ProductCode) Like  '%{0}%'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(query.ProductCode))
                {
                    stringBuilder.AppendFormat(" AND LOWER(ProductCode)='{0}'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
                }
            }
            if (query.AttributeValues.Count > 0)
            {
                foreach (AttributeValueInfo current in query.AttributeValues)
                {
                    stringBuilder.AppendFormat(" AND ProductId IN ( SELECT ProductId FROM Ecshop_ProductAttributes WHERE AttributeId={0} And ValueId={1}) ", current.AttributeId, current.ValueId);
                }
            }
            if (query.BrandId.HasValue)
            {
                if (query.BrandId.Value == 0)
                {
                    stringBuilder.Append(" AND BrandId IS NOT NULL");
                }
                else
                {
                    stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
                }
            }

            if (query.MinSalePrice.HasValue)
            {
                stringBuilder.AppendFormat(" AND SalePrice >= {0}", query.MinSalePrice.Value);
            }
            if (query.MaxSalePrice.HasValue)
            {
                stringBuilder.AppendFormat(" AND SalePrice <= {0}", query.MaxSalePrice.Value);
            }
            if (!string.IsNullOrEmpty(query.Keywords) && query.Keywords.Trim().Length > 0)
            {
                if (!query.IsPrecise)
                {
                    query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                    string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
                    List<string> list = new List<string>();
                    list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[0])));
                    int num = 1;
                    while (num < array.Length && num <= 4)
                    {
                        list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[num])));
                        num++;
                    }
                    stringBuilder.Append(" and (" + string.Join(" and ", list.ToArray()) + ")");
                }
                else
                {
                    stringBuilder.AppendFormat(" AND (ProductName = '{0}' or ShopName = '{0}')", DataHelper.CleanSearchString(query.Keywords));
                }
            }

            if (!string.IsNullOrEmpty(query.SubKeywords) && query.SubKeywords.Trim().Length > 0)
            {
                if (!query.IsPrecise)
                {
                    query.SubKeywords = DataHelper.CleanSearchString(query.SubKeywords);
                    string[] array = Regex.Split(query.SubKeywords.Trim(), "\\s+");
                    List<string> list = new List<string>();
                    list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[0])));
                    int num = 1;
                    while (num < array.Length && num <= 4)
                    {
                        list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[num])));
                        num++;
                    }
                    stringBuilder.Append(" and (" + string.Join(" and ", list.ToArray()) + ")");
                }
                else
                {
                    stringBuilder.AppendFormat(" AND (ProductName = '{0}' or ShopName = '{0}')", DataHelper.CleanSearchString(query.SubKeywords));
                }
            }

            if (query.CategoryId.HasValue)
            {
                CategoryInfo category = new CategoryDao().GetCategory(query.CategoryId.Value);
                if (category != null)
                {
                    stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
                else
                {
                    stringBuilder.Append(" and 1>2 ");
                }
            }
            if (query.ImportsourceId.HasValue)//原产地
            {
                if (query.ImportsourceId.Value != 0)
                {
                    stringBuilder.AppendFormat(" AND ImportsourceId={0} ", query.ImportsourceId.Value);
                }
            }
            if (!string.IsNullOrEmpty(query.TagIds))
            {
                string[] array2 = query.TagIds.Split(new char[]
				{
					'_'
				});
                string[] array3 = array2;
                for (int i = 0; i < array3.Length; i++)
                {
                    string text = DataHelper.CleanSearchString(array3[i]);
                    if (!string.IsNullOrEmpty(text))
                    {
                        stringBuilder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM Ecshop_ProductTag WHERE TagId = {0})", text);
                    }
                }
            }
            if (query.HasStock)
            {
                stringBuilder.Append(" AND stock>0 ");
            }

            ////
            //if (query.DateContrastType.HasValue)
            //{
            //    if (query.DateContrastType.Value == 1)
            //    {
            //        if (query.DateContrastValue.HasValue)
            //        {
            //            stringBuilder.AppendFormat(" AND UpdateDate BETWEEN DATEADD(hour,-{0},GETDATE())  AND GETDATE() ", query.DateContrastValue.Value);
            //        }
            //    }
            //    else if (query.DateContrastType.Value == 2)
            //    {
            //        if (query.DateContrastValue.HasValue)
            //        {
            //            stringBuilder.AppendFormat(" AND UpdateDate BETWEEN DATEADD(day,-{0},GETDATE())  AND GETDATE() ", query.DateContrastValue.Value);
            //        }
            //    }
            //    else if (query.DateContrastType.Value == 2)
            //    {
            //        if (query.DateContrastValue.HasValue)
            //        {
            //            stringBuilder.AppendFormat(" AND UpdateDate BETWEEN DATEADD(month,-{0},GETDATE())  AND GETDATE() ", query.DateContrastValue.Value);
            //        }
            //    }

            //}


            return stringBuilder.ToString();
        }

        protected string BuildCurrProductBrowseQuerySearch(ProductBrowseQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("IsApproved = 1 and SaleStatus = {0}", (int)query.ProductSaleStatus);
            stringBuilder.AppendFormat(" AND SaleStatus = {0}", 1);
            if (!query.IsPrecise)
            {
                if (!string.IsNullOrEmpty(query.ProductCode))
                {
                    stringBuilder.AppendFormat(" AND LOWER(ProductCode) Like  '%{0}%'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(query.ProductCode))
                {
                    stringBuilder.AppendFormat(" AND LOWER(ProductCode)='{0}'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
                }
            }
            if (query.AttributeValues.Count > 0)
            {
                foreach (AttributeValueInfo current in query.AttributeValues)
                {
                    stringBuilder.AppendFormat(" AND ProductId IN ( SELECT ProductId FROM Ecshop_ProductAttributes WHERE AttributeId={0} And ValueId={1}) ", current.AttributeId, current.ValueId);
                }
            }
            if (!string.IsNullOrWhiteSpace(query.StrBrandId))
            {
                stringBuilder.AppendFormat(" AND BrandId in ({0})", query.StrBrandId);
            }

            if (query.MinSalePrice.HasValue)
            {
                stringBuilder.AppendFormat(" AND SalePrice >= {0}", query.MinSalePrice.Value);
            }
            if (query.MaxSalePrice.HasValue)
            {
                stringBuilder.AppendFormat(" AND SalePrice <= {0}", query.MaxSalePrice.Value);
            }
            if (!string.IsNullOrEmpty(query.Keywords) && query.Keywords.Trim().Length > 0)
            {
                if (!query.IsPrecise)
                {
                    query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                    string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
                    List<string> list = new List<string>();
                    list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[0])));
                    int num = 1;
                    while (num < array.Length && num <= 4)
                    {
                        list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[num])));
                        num++;
                    }
                    stringBuilder.Append(" and (" + string.Join(" and ", list.ToArray()) + ")");
                }
                else
                {
                    stringBuilder.AppendFormat(" AND (ProductName = '{0}'  or ShopName = '{0}') ", DataHelper.CleanSearchString(query.Keywords));
                }
            }

            if (!string.IsNullOrEmpty(query.SubKeywords) && query.SubKeywords.Trim().Length > 0)
            {
                if (!query.IsPrecise)
                {
                    query.SubKeywords = DataHelper.CleanSearchString(query.SubKeywords);
                    string[] array = Regex.Split(query.SubKeywords.Trim(), "\\s+");
                    List<string> list = new List<string>();
                    list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[0])));
                    int num = 1;
                    while (num < array.Length && num <= 4)
                    {
                        list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[num])));
                        num++;
                    }
                    stringBuilder.Append(" and (" + string.Join(" and ", list.ToArray()) + ")");
                }
                else
                {
                    stringBuilder.AppendFormat(" AND （ProductName = '{0}'or ShopName = '{0}')", DataHelper.CleanSearchString(query.SubKeywords));
                }
            }
            if (!string.IsNullOrWhiteSpace(query.StrCategoryId))
            {
                IList<CategoryInfo> categorys = new CategoryDao().GetListCategoryByIds(query.StrCategoryId);
                if (categorys != null && categorys.Count > 0)
                {
                    stringBuilder.Append(" and ( ");
                    foreach (CategoryInfo category in categorys)
                    {
                        stringBuilder.AppendFormat(" MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' or", category.Path);
                    }
                    stringBuilder.Remove(stringBuilder.ToString().LastIndexOf("or"), 2);
                    stringBuilder.Append(" ) ");
                }
                //stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE 'SELECT Path FROM Ecshop_Categories WHERE CategoryId in ({0})|%' OR ExtendCategoryPath LIKE 'SELECT Path FROM Ecshop_Categories WHERE CategoryId in ({0})|%') ", query.StrCategoryId);
            }
            if (!string.IsNullOrWhiteSpace(query.StrImportsourceId))//原产地
            {
                stringBuilder.AppendFormat(" AND ImportsourceId in ({0}) ", query.StrImportsourceId);
            }
            if (!string.IsNullOrEmpty(query.TagIds))
            {
                string[] array2 = query.TagIds.Split(new char[]
				{
					'_'
				});
                string[] array3 = array2;
                for (int i = 0; i < array3.Length; i++)
                {
                    string text = DataHelper.CleanSearchString(array3[i]);
                    if (!string.IsNullOrEmpty(text))
                    {
                        stringBuilder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM Ecshop_ProductTag WHERE TagId = {0})", text);
                    }
                }
            }
            if (query.HasStock)
            {
                stringBuilder.Append(" AND stock>0 ");
            }

            if (query.supplierid.HasValue)
            {
                stringBuilder.AppendFormat(" AND supplierid={0} ", query.supplierid.Value);
            }

            if (query.TopId.HasValue)
            {
                if (query.TopId.Value > 0)
                {
                    stringBuilder.AppendFormat(" AND topicid={0} ", query.TopId.Value);
                }
            }



            return stringBuilder.ToString();
        }
        protected string BuildAPIProductBrowseQuerySearch(ProductQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder(" 1=1 ");
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
                stringBuilder.AppendFormat(" AND (ProductName LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[0]));
                int num = 1;
                while (num < array.Length && num <= 4)
                {
                    stringBuilder.AppendFormat("AND (ProductName LIKE '%{0}%' or ShopName like '%{0}%')", DataHelper.CleanSearchString(array[num]));
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
                stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            return stringBuilder.ToString();
        }

        public string GetProductDescription(int productId, bool isMobile)
        {
            string description = "";

            string sql = string.Format("SELECT [{0}] AS [Description] FROM Ecshop_Products WHERE ProductId = {1};", isMobile ? "MobbileDescription" : "Description", productId);

            DbCommand command = this.database.GetSqlStringCommand(sql);

            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {
                if (dataReader.Read())
                {
                    if (dataReader["Description"] != DBNull.Value)
                    {
                        description = (string)dataReader["Description"];
                    }
                }
            }

            return description;
        }


        /// <summary>
        /// 根据限时抢购id获取商品详情
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public string GetProductDescriptionByCountDownId(int CountDownId, bool isMobile)
        {
            string description = "";

            string sql = string.Format("SELECT [{0}] AS [Description] FROM Ecshop_Products  A  inner join Ecshop_CountDown B on A.productid=B.productid where CountDownId= {1};", isMobile ? "MobbileDescription" : "Description", CountDownId);

            DbCommand command = this.database.GetSqlStringCommand(sql);

            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {
                if (dataReader.Read())
                {
                    if (dataReader["Description"] != DBNull.Value)
                    {
                        description = (string)dataReader["Description"];
                    }
                }
            }

            return description;
        }


        public bool IsExistProduct(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(ProductId) from Ecshop_Products where ProductId=@ProductId ");

            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);

            Object objresult = this.database.ExecuteScalar(sqlStringCommand);

            int result=0;

            if (objresult != null)
            {
                int.TryParse(objresult.ToString(), out result);
            }

            return result > 0;
        }
        /// <summary>
        /// 组合商品计算税费
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public decimal GetTaxByProductId(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select ISNULL((select  sum(t.TaxRate*com.Quantity*com.Price) from Ecshop_Products p1 
left join Ecshop_SKUs s1 on p1.ProductId = s1.ProductId 
left join Ecshop_ProductsCombination  com on com.CombinationSkuId = s1.SkuId 
inner join Ecshop_Products comp on comp.ProductId = com.Productid 
left join Ecshop_TaxRate as t on comp.TaxRateId=t.taxId  where p1.ProductId=p.ProductId ),0) AS TaxRate
 FROM   dbo.Ecshop_Products AS p where p.ProductId=@ProductId");

            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);

            decimal result = 0M;

            Object objresult = this.database.ExecuteScalar(sqlStringCommand);

            if(objresult!=null)
            {
                decimal.TryParse(objresult.ToString(),out result);
            }

            return result;
        }
    }
}

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SqlDal.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
namespace EcShop.SqlDal.Commodities
{
    public class ProductDao
    {
        private Database database;
        public ProductDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public DbQueryResult GetProducts(ProductQuery query, bool isAdmin, bool IsExport = true, bool special=true)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1  ");
            //if (!isAdmin)
            //{
            //    stringBuilder.Append(" and IsApproved = 1 ");
            //}
            if (special==true)//非专题
            {
                if (query.SaleStatus != ProductSaleStatus.All)
                {
                    stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
                }
                else
                {
                    stringBuilder.AppendFormat(" AND SaleStatus <> ({0})", 0);
                }


                //多种销售状态筛选
                if (!string.IsNullOrWhiteSpace(query.MulSaleStatus))
                {
                    stringBuilder.AppendFormat(" AND SaleStatus in ({0})", query.MulSaleStatus);
                }
            }

            //是否含有库存

            if (query.HasStock.HasValue)
            {
                if (query.HasStock.Value)
                {
                    stringBuilder.AppendFormat(" AND stock >= 0");
                }
                else
                {
                    stringBuilder.AppendFormat(" AND stock = 0");
                }
            }
            //商品代码
            if (!string.IsNullOrEmpty(query.SkuId))
            {
                stringBuilder.AppendFormat(" AND SkuId like '%{0}%' ", query.SkuId);
            }
            //商品条码
            if (!string.IsNullOrEmpty(query.BarCode))
            {
                stringBuilder.AppendFormat(" AND BarCode like '%{0}%' ", query.BarCode);
            }

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
            if (query.TopicId.HasValue)
            {
                stringBuilder.AppendFormat(" AND ProductId not IN (SELECT RelatedProductId FROM Vshop_RelatedTopicProducts WHERE TopicId={0})", query.TopicId);
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
                stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
                int num = 1;
                while (num < array.Length && num <= 4)
                {
                    stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
                    num++;
                }
            }
            if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
            {
                stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
            }
            //if (query.IsIncludePromotionProduct.HasValue)
            //{
            //    if (!query.IsIncludePromotionProduct.Value)
            //    {
            //        stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Ecshop_PromotionProducts)");
            //    }
            //}

            //if (query.IsIncludePromotionProduct.HasValue)
            //{
            //    if (!query.IsIncludePromotionProduct.Value)
            //    {

            //        stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Ecshop_PromotionProducts A left join Ecshop_Promotions B on A.ActivityId=B.ActivityId where B.EndDate>='" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "')");
            //    }
            //}

            if (query.IsIncludePromotionProduct.HasValue)
            {
                if (!query.IsIncludePromotionProduct.Value)
                {
                    if (query.ActivityId.HasValue)
                    {
                        stringBuilder.AppendFormat(@" AND ProductId NOT IN 

(SELECT ProductId FROM Ecshop_PromotionProducts A left join Ecshop_Promotions B on A.ActivityId=B.ActivityId where 


(not (convert(varchar(10),B.enddate,120)<(select convert(varchar(10),startdate,120) from Ecshop_Promotions where activityId={0}) 


or convert(varchar(10),B.startdate,120)>(select convert(varchar(10),enddate,120) from Ecshop_Promotions where activityId={0})))

and B.enddate>=convert(varchar(10),getdate(),120))", query.ActivityId);
                    }
                    else if (query.PresentActivityId.HasValue)  // 该促销活动中的赠送商品
                    {
                        stringBuilder.AppendFormat(@" AND ProductId NOT IN 

(SELECT ProductId FROM Ecshop_PromotionProductsPresent A left join Ecshop_Promotions B on A.ActivityId=B.ActivityId where 


(not (convert(varchar(10),B.enddate,120)<(select convert(varchar(10),startdate,120) from Ecshop_Promotions where activityId={0}) 


or convert(varchar(10),B.startdate,120)>(select convert(varchar(10),enddate,120) from Ecshop_Promotions where activityId={0})))

and B.enddate>=convert(varchar(10),getdate(),120))", query.PresentActivityId);
                    }
                }
            }
            if (query.IsIncludeBundlingProduct.HasValue)
            {
                if (!query.IsIncludeBundlingProduct.Value)
                {
                    stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Ecshop_BundlingProductItems)");
                }
            }
            if (query.IsIncludeHomeProduct.HasValue)
            {
                if (!query.IsIncludeHomeProduct.Value)
                {
                    if (!query.Client.HasValue)
                    {
                        stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Vshop_HomeProducts)");
                    }
                    else
                    {
                        stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Vshop_HomeProducts where Client=" + query.Client + " )");
                    }
                }
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue)
            {
                if (query.CategoryId.Value > 0)
                {
                    stringBuilder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%') ", query.MaiCategoryPath);
                }
                else
                {
                    stringBuilder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
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
            if (query.ImportSourceId.HasValue)
            {
                stringBuilder.AppendFormat(" AND ImportSourceId = {0}", query.ImportSourceId.Value);
            }
            if (query.SupplierId.HasValue)
            {
                stringBuilder.AppendFormat(" AND SupplierId = {0}", query.SupplierId.Value);
            }
            if (query.TemplateId.HasValue)
            {
                stringBuilder.AppendFormat(" AND TemplateId = {0}", query.TemplateId.Value);
            }
            if (query.IsApproved.HasValue)//是否审核过滤
            {
                stringBuilder.AppendFormat(" AND IsApproved={0}", query.IsApproved.Value);
            }

            if (query.IsApprovedPrice.HasValue)
            {
                stringBuilder.AppendFormat(" AND IsApprovedPrice={0}", query.IsApprovedPrice.Value);
            }


            if (query.IsAllClassify.HasValue)
            {
                stringBuilder.AppendFormat(" AND IsAllClassify={0}", query.IsAllClassify.Value);
            }

            if (query.SaleType.HasValue)
            {
                if (query.SaleType.Value == 2)
                {
                    stringBuilder.AppendFormat(" AND SaleType={0}", query.SaleType.Value);
                }
                else
                {
                    stringBuilder.AppendFormat(" AND SaleType<>{0}", 2);
                }
            }

            string selectFields = string.Empty;
            if (IsExport)
            {
                selectFields = "SkuId,QRcode,CategoryId,ProductId, ProductCode,IsMakeTaobao,ProductName,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl410, MarketPrice, SalePrice,ExtendCategoryPath,CostPrice,Stock, DisplaySequence,SaleStatus,ImportSourceId,Icon,CnArea,SupplierId,SupplierName,IsApproved,TaxRate,IsApprovedPrice,ApprovedPriceDescription,IsAllClassify";

            }
            else
            {
                // selectFields = " ProductName,ProductCode,Stock,CostPrice,SalePrice, case  when SaleStatus=1 then '出售中' when SaleStatus=2 then '下架去'  else '仓库中' end as  'SaleStatus'  , case  when IsApproved=1 then '已审核' else '未审核' end as  'IsApproved'  ,  case  when IsApprovedPrice=1 then '已审核' else '未审核' end as  'IsApprovedPrice' , case when IsAllClassify=1 then '已审核' else '未审核' end as  'IsAllClassify' ,CnArea,SupplierName ";

                selectFields = " SkuId,ProductId,ProductName,BarCode,ProductCode,Stock,CostPrice,SalePrice, case  when SaleStatus=1 then '出售中' when SaleStatus=2 then '下架去'  else '仓库中' end as  'SaleStatus'  , case  when IsApproved=1 then '已审核' else '未审核' end as  'IsApproved'  ,  case  when IsApprovedPrice=1 then '已审核' else '未审核' end as  'IsApprovedPrice' , case when IsAllClassify=1 then '已审核' else '未审核' end as  'IsAllClassify' ,CnArea,SupplierName ";
                string sql = "select " + selectFields + " from vw_Ecshop_BrowseProductList where  " + stringBuilder.ToString();

                DbCommand storedProcCommand = this.database.GetSqlStringCommand(sql);
                 DbQueryResult rs = new DbQueryResult();
                rs.Data=this.database.ExecuteDataSet(storedProcCommand).Tables[0];
                return rs;

            }
            if (isAdmin)
            {
                return DataHelper.PagingByRownumberAdmin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
            }

            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
        }
        public DbQueryResult GetShareProducts(ProductQuery query, bool share, string productIds)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1");
            if (!string.IsNullOrEmpty(productIds))
            {
                if (share)
                {
                    stringBuilder.AppendFormat(" AND ProductId IN (" + productIds + ")", new object[0]);
                }
                else
                {
                    stringBuilder.AppendFormat(" AND ProductId NOT IN (" + productIds + ")", new object[0]);
                }
            }
            if (query.SaleStatus != ProductSaleStatus.All)
            {
                stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
            }
            else
            {
                stringBuilder.AppendFormat(" AND SaleStatus <> ({0})", 0);
            }
            if (query.UserId.HasValue)
            {
                stringBuilder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM distro_Products WHERE DistributorUserId = {0})", query.UserId.Value);
            }
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
                stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
                int num = 1;
                while (num < array.Length && num <= 4)
                {
                    stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
                    num++;
                }
            }
            if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
            {
                stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
            }
            if (query.IsIncludePromotionProduct.HasValue)
            {
                if (!query.IsIncludePromotionProduct.Value)
                {
                    stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Ecshop_PromotionProducts)");
                }
            }
            if (query.IsIncludeBundlingProduct.HasValue && !query.IsIncludeBundlingProduct.Value)
            {
                stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductID FROM Ecshop_BundlingProductItems)");
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
            {
                stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
            }
            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.IsAlert)
            {
                stringBuilder.Append(" AND ProductId IN (SELECT DISTINCT ProductId FROM Ecshop_SKUs WHERE Stock <= AlertStock)");
            }
            string selectFields = "ProductId, ProductCode,IsMakeTaobao,ProductName, ThumbnailUrl40, MarketPrice, SalePrice, (SELECT CostPrice FROM Ecshop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,SaleStatus,(SELECT TOP 1 ProductId FROM Ecshop_ShareProducts WHERE Ecshop_ShareProducts.ProductId=p.ProductId) as MakeState";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
        }

        /// <summary>
        /// 归类获取商品信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetProductClassifyList(ProductQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" SaleStatus <> 0");

            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
                stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
                int num = 1;
                while (num < array.Length && num <= 4)
                {
                    stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
                    num++;
                }
            }

            if (!string.IsNullOrEmpty(query.ItemNo))
            {
                stringBuilder.AppendFormat(" AND ItemNo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ItemNo));
            }
            if (!string.IsNullOrEmpty(query.BrandName))
            {
                stringBuilder.AppendFormat(" AND BrandName LIKE '%{0}%'", DataHelper.CleanSearchString(query.BrandName));
            }
            if (!string.IsNullOrEmpty(query.HSProductName))
            {
                stringBuilder.AppendFormat(" AND HSProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.HSProductName));
            }
            if (!string.IsNullOrEmpty(query.BarCode))
            {
                stringBuilder.AppendFormat(" AND BarCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.BarCode));
            }
            if (!string.IsNullOrEmpty(query.HSCODE))
            {
                stringBuilder.AppendFormat(" AND HS_CODE LIKE '%{0}%'", DataHelper.CleanSearchString(query.HSCODE));
            }
            if (!string.IsNullOrEmpty(query.BatchNo))
            {
                stringBuilder.AppendFormat(" AND BatchNo LIKE '%{0}%'", DataHelper.CleanSearchString(query.BatchNo));
            }

            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.IsApproved != null && query.IsApproved != -1)
            {
                stringBuilder.AppendFormat(" AND PStatus ={0}", query.IsApproved);
            }

            if (query.RecordStatus != null && query.RecordStatus != -1)
            {
                stringBuilder.AppendFormat(" AND ReStatus ={0}", query.RecordStatus);
            }

            if (query.CheckStatus != null && query.CheckStatus != -1)
            {
                stringBuilder.AppendFormat(" AND CheckStatus ={0}", query.CheckStatus);
            }

            if (query.InspectionStaus != null && query.InspectionStaus != -1)
            {
                stringBuilder.AppendFormat(" AND InspectionStaus ={0}", query.InspectionStaus);
            }

            if (query.IsApprovedPrice != null && query.IsApprovedPrice != -1)
            {
                stringBuilder.AppendFormat(" AND IsApprovedPrice ={0}", query.IsApprovedPrice);
            }

            if (query.SaleType.HasValue)
            {
                if (query.SaleType.Value == 2)
                {
                    stringBuilder.AppendFormat(" AND SaleType={0}", query.SaleType.Value);
                }
                else
                {
                    stringBuilder.AppendFormat(" AND SaleType<>{0}", 2);
                }
            }

            if (!string.IsNullOrEmpty(query.SupplierCode))
            {
                stringBuilder.AppendFormat(" AND SupplierCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.SupplierCode));
            }

            if (!string.IsNullOrEmpty(query.ProductRegistrationNumber))
            {
                stringBuilder.AppendFormat(" AND ProductRegistrationNumber LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductRegistrationNumber));
            }

            if (!string.IsNullOrEmpty(query.LJNo))
            {
                stringBuilder.AppendFormat(" AND LJNo LIKE '%{0}%'", DataHelper.CleanSearchString(query.LJNo));
            }



            string selectFields = "ProductId,ProductName,ItemNo,HSItemNo,Manufacturer,BarCode,Unit,HSProductName,HSBrand,HSUnit,ProductRegistrationNumber,BrandName,EnArea,HS_CODE_ID,HS_CODE,PersonalPostalArticlesCode,TaxRate,PStatus,ReStatus,InspectionStaus,CheckStatus,AddedDate,HSTariff,TaxId,tax_Rate,tsl_Rate,low_Rate,out_Rate,high_Rate,temp_Out_Rate,temp_In_Rate,control_Ma,CONTROL_INSPECTION,note_S,BatchNo,HSUnitCode,ProductStandard,Ingredient,CostPrice,SkuId,LJNo,IsApprovedPrice,SupplierCode,SupplierId,ImportSourceCode,remarks,coustomSkuType,countrySku,beLookType,madeOf,ThumbnailUrl60,ThumbnailUrl220";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_ProductClassifyList p", "ProductId", stringBuilder.ToString(), selectFields);
        }

        /// <summary>
        /// 商品归类
        /// </summary>
        /// <param name="productInfo"></param>
        /// <param name="ElmentsValue">申报要素</param>
        /// <returns></returns>
        public DataSet ProductClassify(ProductInfo product, string ElmentsValue, int UserId, string Username, string LJNo, string SkuId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Product_Classify");
            this.database.AddInParameter(storedProcCommand, "ProductId", System.Data.DbType.Int32, product.ProductId);
            this.database.AddInParameter(storedProcCommand, "TaxRateId", System.Data.DbType.Int32, product.TaxRateId);
            this.database.AddInParameter(storedProcCommand, "HSCodeId", System.Data.DbType.Int32, product.HSCodeId);
            this.database.AddInParameter(storedProcCommand, "HSProductName", System.Data.DbType.String, product.HSProductName);
            this.database.AddInParameter(storedProcCommand, "HSBrand", System.Data.DbType.String, product.HSBrand);
            this.database.AddInParameter(storedProcCommand, "HSItemNo", System.Data.DbType.String, product.HSItemNo);
            this.database.AddInParameter(storedProcCommand, "HSUnit", DbType.String, product.HSUnit);
            this.database.AddInParameter(storedProcCommand, "HSUnitCode", DbType.String, product.HSUnitCode);

            this.database.AddInParameter(storedProcCommand, "ElmentsValue", DbType.String, ElmentsValue);
            this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, UserId);
            this.database.AddInParameter(storedProcCommand, "Username", System.Data.DbType.String, Username);
            this.database.AddInParameter(storedProcCommand, "LJNo", System.Data.DbType.String, LJNo);
            this.database.AddInParameter(storedProcCommand, "SkuId", System.Data.DbType.String, SkuId);

            this.database.AddInParameter(storedProcCommand, "coustomSkuType", System.Data.DbType.String, product.coustomSkuType);
            this.database.AddInParameter(storedProcCommand, "countrySku", System.Data.DbType.String, product.coustomSkuType);
            this.database.AddInParameter(storedProcCommand, "beLookType", System.Data.DbType.String, product.beLookType);
            this.database.AddInParameter(storedProcCommand, "madeOf", System.Data.DbType.String, product.madeOf);

            return this.database.ExecuteDataSet(storedProcCommand);
        }

        /// <summary>
        /// 商品备案
        /// </summary>
        /// <param name="ProductId">商品Id</param>
        /// <param name="ProductRegistrationNumber">备案编号</param>
        /// <returns></returns>
        public bool Registration(string ProductRegistrationNumber, string SkuId,string status)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_SKUs SET ProductRegistrationNumber=@ProductRegistrationNumber WHERE SkuId=@SkuId;"
                                                                + "UPDATE Ecshop_Products SET HSstatus=@status,RecordStatus=@status WHERE ProductId = (select ProductId from Ecshop_SKUs where SkuId=@SkuId);");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
            this.database.AddInParameter(sqlStringCommand, "ProductRegistrationNumber", DbType.String, ProductRegistrationNumber);
            this.database.AddInParameter(sqlStringCommand, "status", DbType.String, status);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 添加备案批次
        /// </summary>
        /// <param name="ProductId">商品Id</param>
        /// <param name="ProductRegistrationNumber">批次号</param>
        /// <returns></returns>
        public bool AddBatchNo(string BatchNo, string strProductIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Products SET BatchNo=@BatchNo WHERE ProductId in(" + DataHelper.CleanSearchString(strProductIds) + ")");
            this.database.AddInParameter(sqlStringCommand, "BatchNo", DbType.String, BatchNo);
            return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
        }


        /// <summary>
        /// 判断是否存在未审价通过的商品
        /// </summary>
        /// <param name="strProductIds"></param>
        /// <returns></returns>
        public bool IsExitNoApprovedPriceProduct(string strProductIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(1) from Ecshop_Products where IsApprovedPrice<>1 and ProductId in(" + DataHelper.CleanSearchString(strProductIds) + ")");
            object objresult;
            int result = 0;
            objresult = this.database.ExecuteScalar(sqlStringCommand);
            if (objresult != null)
            {
                int.TryParse(objresult.ToString(), out result);
            }

            return result > 0;

        }

        public bool AddShareProducts(int shareId, int prodcutId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT Ecshop_ShareProducts(ShareId,ProductId) VALUES (@ShareId,@ProductId)");
            this.database.AddInParameter(sqlStringCommand, "ShareId", DbType.Int32, shareId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, prodcutId);
            return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
        }
        public int AddShareLine(ShareProductInfo shareProductInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT Ecshop_ShareList(ShareTitle) VALUES(@ShareTitle);SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "ShareTitle", DbType.String, shareProductInfo.ShareTitle);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            int result;
            if (obj != null)
            {
                result = (int)Convert.ToInt16(obj);
            }
            else
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 批量导入商品明细
        /// </summary>
        /// <param name="xmlProduct"></param>
        /// <returns></returns>
        public DataTable ImportProductsList(string xmlProduct)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("proc_ImportOfGoods");
            this.database.AddInParameter(storedProcCommand, "@product", DbType.Xml, xmlProduct);
            return this.database.ExecuteDataSet(storedProcCommand).Tables[0];
        }


        public DataSet GetAllShareProducts(int pagesize, int pageindex)
        {
            ShareProductInfo shareProductInfo = new ShareProductInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[]
			{
				"SELECT TOP ",
				pagesize,
				" *  FROM Ecshop_ShareList WHERE ShareId NOT IN (SELECT TOP (",
				pagesize * (pageindex - 1),
				") ShareId FROM Ecshop_ShareList ORDER BY ShareTime DESC) ORDER BY ShareTime DESC"
			}));
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        public DbQueryResult GetShareProducts(ProductQuery query, bool share, int shareId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1");
            if (share)
            {
                stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Ecshop_ShareProducts WHERE ShareId=" + shareId + ")", new object[0]);
            }
            else
            {
                stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Ecshop_ShareProducts WHERE ShareId=" + shareId + ")", new object[0]);
            }
            if (query.SaleStatus != ProductSaleStatus.All)
            {
                stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
            }
            else
            {
                stringBuilder.AppendFormat(" AND SaleStatus <> ({0})", 0);
            }
            if (query.UserId.HasValue)
            {
                stringBuilder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM distro_Products WHERE DistributorUserId = {0})", query.UserId.Value);
            }
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
                stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
                int num = 1;
                while (num < array.Length && num <= 4)
                {
                    stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
                    num++;
                }
            }
            if (query.ProductLineId.HasValue)
            {
                stringBuilder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
            }
            if (query.PenetrationStatus != PenetrationStatus.NotSet)
            {
                stringBuilder.AppendFormat(" AND PenetrationStatus={0}", (int)query.PenetrationStatus);
            }
            if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
            {
                stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
            }
            if (query.IsIncludePromotionProduct.HasValue)
            {
                if (!query.IsIncludePromotionProduct.Value)
                {
                    stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Ecshop_PromotionProducts)");
                }
            }
            if (query.IsIncludeBundlingProduct.HasValue && !query.IsIncludeBundlingProduct.Value)
            {
                stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductID FROM Ecshop_BundlingProductItems)");
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
            {
                stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
            }
            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            string selectFields = "ProductId, ProductCode,IsMakeTaobao,ProductName, ThumbnailUrl40, MarketPrice, SalePrice, (SELECT CostPrice FROM Ecshop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,SaleStatus,(SELECT TOP 1 ProductId FROM Ecshop_ShareProducts WHERE Ecshop_ShareProducts.ProductId=p.ProductId) as MakeState";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
        }
        public bool DeleteShareProducts(int shareId, string productIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_ShareProducts WHERE ShareId=@ShareId AND ProductId IN (" + productIds + ")");
            this.database.AddInParameter(sqlStringCommand, "ShareId", DbType.Int32, shareId);
            return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
        }
        public bool UpdateShareLine(ShareProductInfo shareProductInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_ShareList SET ShareTitle=@ShareTitle,ShareUrl=@ShareUrl WHERE ShareId=@ShareId");
            this.database.AddInParameter(sqlStringCommand, "ShareId", DbType.Int32, shareProductInfo.ShareId);
            this.database.AddInParameter(sqlStringCommand, "ShareTitle", DbType.String, shareProductInfo.ShareTitle);
            this.database.AddInParameter(sqlStringCommand, "ShareUrl", DbType.String, shareProductInfo.ShareUrl);
            return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
        }
        public bool DeleteShareLine(int shareId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_ShareList WHERE ShareId=@ShareId;DELETE FROM Ecshop_ShareProducts WHERE ShareId=@ShareId");
            this.database.AddInParameter(sqlStringCommand, "ShareId", DbType.Int32, shareId);
            return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
        }
        public ShareProductInfo SelectShareProductInfo(int shareId)
        {
            ShareProductInfo shareProductInfo = new ShareProductInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM Ecshop_ShareList WHERE ShareId=@ShareId");
            this.database.AddInParameter(sqlStringCommand, "ShareId", DbType.Int32, shareId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    shareProductInfo.ShareId = (int)dataReader["ShareId"];
                    shareProductInfo.ShareTitle = (string)dataReader["ShareTitle"];
                    if (DBNull.Value != dataReader["ShareUrl"])
                    {
                        shareProductInfo.ShareUrl = (string)dataReader["ShareUrl"];
                    }
                    shareProductInfo.ShareTime = Convert.ToDateTime(dataReader["ShareTime"].ToString());
                }
            }
            return shareProductInfo;
        }
        public DataTable GetGroupBuyProducts(ProductQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(" WHERE IsApproved = 1 AND SaleStatus = {0}", 1);
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
                stringBuilder.AppendFormat(" AND MainCategoryPath LIKE '{0}|%'", query.MaiCategoryPath);
            }
            if (query.SupplierId.HasValue)
            {
                stringBuilder.AppendFormat(" and SupplierId ={0} ", query.SupplierId);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId,ProductName FROM Ecshop_Products" + stringBuilder.ToString());
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public decimal GetProductSalePrice(int productId)
        {
            string commandText = string.Format("SELECT MIN(SalePrice) FROM Ecshop_SKUs WHERE ProductId = {0}", productId);
            return (decimal)this.database.ExecuteScalar(CommandType.Text, commandText);
        }
        public ProductInfo GetProductDetails(int productId)
        {
            ProductInfo productInfo = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT p.*, r.TaxRate FROM Ecshop_Products p LEFT JOIN Ecshop_TaxRate r on p.TaxRateId = r.TaxId WHERE p.ProductId = @ProductId;SELECT skus.ProductId, skus.SkuId, s.AttributeId, s.ValueId, skus.SKU, skus.SalePrice, skus.CostPrice, skus.Stock,isnull(skus.FactStock,0) FactStock,skus.[Weight],skus.GrossWeight,skus.DeductFee,isnull(skus.ProductRegistrationNumber,'') ProductRegistrationNumber,isnull(skus.LJNo,'') LJNo,isnull(skus.WMSStock,0) WMSStock FROM Ecshop_SKUItems s right outer join Ecshop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId = @ProductId ORDER BY (SELECT DisplaySequence FROM Ecshop_Attributes WHERE AttributeId = s.AttributeId) DESC;SELECT s.SkuId, smp.GradeId, smp.MemberSalePrice FROM Ecshop_SKUMemberPrice smp INNER JOIN Ecshop_SKUs s ON smp.SkuId=s.SkuId WHERE s.ProductId=@ProductId;SELECT AttributeId, ValueId FROM Ecshop_ProductAttributes WHERE ProductId = @ProductId;SELECT * FROM Ecshop_ProductTag WHERE ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                productInfo = ReaderConvert.ReaderToModel<ProductInfo>(dataReader);
                if (productInfo != null)
                {
                    dataReader.NextResult();
                    while (dataReader.Read())
                    {
                        string key = (string)dataReader["SkuId"];
                        if (!productInfo.Skus.ContainsKey(key))
                        {
                            productInfo.Skus.Add(key, DataMapper.ExtendPopulateSKU(dataReader));
                        }
                        if (dataReader["AttributeId"] != DBNull.Value && dataReader["ValueId"] != DBNull.Value)
                        {
                            productInfo.Skus[key].SkuItems.Add((int)dataReader["AttributeId"], (int)dataReader["ValueId"]);
                        }
                    }
                    dataReader.NextResult();
                    while (dataReader.Read())
                    {
                        string key = (string)dataReader["SkuId"];
                        productInfo.Skus[key].MemberPrices.Add((int)dataReader["GradeId"], (decimal)dataReader["MemberSalePrice"]);
                    }
                }
            }
            return productInfo;
        }

        public Dictionary<string, SKUItem> GetProductSkus(int productId)
        {
            Dictionary<string, SKUItem> skus = new Dictionary<string, SKUItem>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT skus.ProductId, skus.SkuId, s.AttributeId, s.ValueId, skus.SKU, skus.SalePrice, skus.CostPrice, ");
            //组合商品库存处理
            sb.Append(@"(select case when p.saletype=2 then   
(select min(s1.Stock/pc.Quantity)  from  dbo.Ecshop_SKUs s1 inner join

 Ecshop_ProductsCombination pc 
 
 on pc.Skuid=s1.Skuid

where pc.CombinationSkuId=s.Skuid)   

else s.stock end   

from dbo.Ecshop_SKUs s inner join

dbo.Ecshop_Products p on p.productid=s.productid where s.Skuid=skus.Skuid) as Stock,");

            sb.Append("skus.FactStock,skus.[Weight],skus.DeductFee FROM Ecshop_SKUItems s right outer join Ecshop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId = @ProductId ORDER BY (SELECT DisplaySequence FROM Ecshop_Attributes WHERE AttributeId = s.AttributeId) DESC;SELECT s.SkuId, smp.GradeId, smp.MemberSalePrice FROM Ecshop_SKUMemberPrice smp INNER JOIN Ecshop_SKUs s ON smp.SkuId=s.SkuId WHERE s.ProductId=@ProductId;");


            DbCommand command = this.database.GetSqlStringCommand(sb.ToString());
            this.database.AddInParameter(command, "ProductId", DbType.Int32, productId);

            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {
                while (dataReader.Read())
                {
                    string key = (string)dataReader["SkuId"];
                    if (!skus.ContainsKey(key))
                    {
                        skus.Add(key, DataMapper.PopulateSKU(dataReader));
                    }
                    if (dataReader["AttributeId"] != DBNull.Value && dataReader["ValueId"] != DBNull.Value)
                    {
                        skus[key].SkuItems.Add((int)dataReader["AttributeId"], (int)dataReader["ValueId"]);
                    }
                }

                dataReader.NextResult();
                while (dataReader.Read())
                {
                    string key = (string)dataReader["SkuId"];
                    skus[key].MemberPrices.Add((int)dataReader["GradeId"], (decimal)dataReader["MemberSalePrice"]);
                }
            }

            return skus;
        }

        public Dictionary<int, IList<int>> GetProductAttributes(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT AttributeId, ValueId FROM Ecshop_ProductAttributes WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            Dictionary<int, IList<int>> dictionary = new Dictionary<int, IList<int>>();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    int key = (int)dataReader["AttributeId"];
                    int item = (int)dataReader["ValueId"];
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, new List<int>
						{
							item
						});
                    }
                    else
                    {
                        dictionary[key].Add(item);
                    }
                }
            }
            return dictionary;
        }
        public IList<int> GetProductTags(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_ProductTag WHERE ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            IList<int> list = new List<int>();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    list.Add((int)dataReader["TagId"]);
                }
            }
            return list;
        }
        /// <summary>
        /// 审核商品
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="flag">0表示弃审，1表示审核</param>
        /// <returns></returns>
        public bool ApproveProducts(string productIds, int flag)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Products SET IsApproved ={0} WHERE ProductId IN ({1})", flag, productIds));
            int ret = this.database.ExecuteNonQuery(sqlStringCommand);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }
        public IList<ProductInfo> GetProducts(IList<int> productIds)
        {
            IList<ProductInfo> list = new List<ProductInfo>();
            string text = "(";
            foreach (int current in productIds)
            {
                text = text + current + ",";
            }
            IList<ProductInfo> result;
            if (text.Length <= 1)
            {
                result = list;
            }
            else
            {
                text = text.Substring(0, text.Length - 1) + ")";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select P.*,T.TaxRate,S.TemplateName from Ecshop_Products as P LEFT JOIN dbo.Ecshop_TaxRate as T ON P.TaxRateId=T.TaxId LEFT JOIN  Ecshop_ShippingTemplates as S ON P.TemplateId=S.TemplateId WHERE P.ProductId IN " + text);
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    while (dataReader.Read())
                    {
                        list.Add(DataMapper.PopulateProduct(dataReader));
                    }
                }
                result = list;
            }
            return result;
        }
        public IList<int> GetProductIds(ProductQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT p.ProductId FROM Ecshop_Products p WHERE p.SaleStatus = {0}", (int)query.SaleStatus);
            if (!string.IsNullOrEmpty(query.ProductCode) && query.ProductCode.Length > 0)
            {
                stringBuilder.AppendFormat(" AND LOWER(p.ProductCode) LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                stringBuilder.AppendFormat(" AND LOWER(p.ProductName) LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
            }
            if (query.CategoryId.HasValue)
            {
                stringBuilder.AppendFormat(" AND (p.CategoryId = {0}  OR  p.CategoryId IN (SELECT CategoryId FROM Ecshop_Categories WHERE Path LIKE (SELECT Path FROM Ecshop_Categories WHERE CategoryId = {0}) + '|%'))", query.CategoryId.Value);
            }
            IList<int> list = new List<int>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    list.Add((int)dataReader["ProductId"]);
                }
            }
            return list;
        }
        public DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            if (query.IncludeOnSales)
            {
                stringBuilder.AppendFormat("SaleStatus = {0} OR ", 1);
            }
            if (query.IncludeUnSales)
            {
                stringBuilder.AppendFormat("SaleStatus = {0} OR ", 2);
            }
            if (query.IncludeInStock)
            {
                stringBuilder.AppendFormat("SaleStatus = {0} OR ", 3);
            }
            stringBuilder.Remove(stringBuilder.Length - 4, 4);
            stringBuilder.Append(")");
            if (query.BrandId.HasValue)
            {
                stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
            }
            if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao != -1)
            {
                stringBuilder.AppendFormat(" AND IsMakeTaobao={0}  ", query.IsMakeTaobao);
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
            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (!string.IsNullOrEmpty(removeProductIds))
            {
                stringBuilder.AppendFormat(" AND ProductId NOT IN ({0})", removeProductIds);
            }
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, (SELECT CostPrice FROM Ecshop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
        }
        public DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT a.[ProductId], [TypeId], [ProductName], [ProductCode], [ShortDescription], [Unit], [Description], ").Append("[Title], [Meta_Description], [Meta_Keywords], [SaleStatus], [ImageUrl1], [ImageUrl2], [ImageUrl3], ").Append("[ImageUrl4], [ImageUrl5], [MarketPrice], [HasSKU] ").Append("FROM Ecshop_Products a  left join Taobao_Products b on a.productid=b.productid WHERE ");
            stringBuilder.Append("(");
            if (query.IncludeOnSales)
            {
                stringBuilder.AppendFormat("SaleStatus = {0} OR ", 1);
            }
            if (query.IncludeUnSales)
            {
                stringBuilder.AppendFormat("SaleStatus = {0} OR ", 2);
            }
            if (query.IncludeInStock)
            {
                stringBuilder.AppendFormat("SaleStatus = {0} OR ", 3);
            }
            stringBuilder.Remove(stringBuilder.Length - 4, 4);
            stringBuilder.Append(")");
            if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao != -1)
            {
                if (query.IsMakeTaobao == 1)
                {
                    stringBuilder.AppendFormat(" AND a.ProductId IN (SELECT ProductId FROM Taobao_Products)", new object[0]);
                }
                else
                {
                    stringBuilder.AppendFormat(" AND a.ProductId NOT IN (SELECT ProductId FROM Taobao_Products)", new object[0]);
                }
            }
            if (query.BrandId.HasValue)
            {
                stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
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
            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (!string.IsNullOrEmpty(removeProductIds))
            {
                stringBuilder.AppendFormat(" AND a.ProductId NOT IN ({0})", removeProductIds);
            }
            stringBuilder.AppendFormat(" ORDER BY {0} {1}", query.SortBy, query.SortOrder);
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Product_GetExportList");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, stringBuilder.ToString());
            return this.database.ExecuteDataSet(storedProcCommand);
        }
        public void EnsureMapping(DataSet mappingSet)
        {
            using (DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO  Ecshop_ProductTypes (TypeName, Remark) VALUES(@TypeName, @Remark);SELECT @@IDENTITY;"))
            {
                this.database.AddInParameter(sqlStringCommand, "TypeName", DbType.String);
                this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String);
                DataRow[] array = mappingSet.Tables["types"].Select("SelectedTypeId=0");
                DataRow[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    DataRow dataRow = array2[i];
                    this.database.SetParameterValue(sqlStringCommand, "TypeName", dataRow["TypeName"]);
                    this.database.SetParameterValue(sqlStringCommand, "Remark", dataRow["Remark"]);
                    dataRow["SelectedTypeId"] = this.database.ExecuteScalar(sqlStringCommand);
                }
            }
            using (DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Ecshop_Attributes; INSERT INTO Ecshop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage)  VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage);SELECT @@IDENTITY;"))
            {
                this.database.AddInParameter(sqlStringCommand2, "AttributeName", DbType.String);
                this.database.AddInParameter(sqlStringCommand2, "TypeId", DbType.Int32);
                this.database.AddInParameter(sqlStringCommand2, "UsageMode", DbType.Int32);
                this.database.AddInParameter(sqlStringCommand2, "UseAttributeImage", DbType.Boolean);
                DataRow[] array3 = mappingSet.Tables["attributes"].Select("SelectedAttributeId=0");
                DataRow[] array2 = array3;
                for (int i = 0; i < array2.Length; i++)
                {
                    DataRow dataRow2 = array2[i];
                    int num = (int)mappingSet.Tables["types"].Select(string.Format("MappedTypeId={0}", dataRow2["MappedTypeId"]))[0]["SelectedTypeId"];
                    this.database.SetParameterValue(sqlStringCommand2, "AttributeName", dataRow2["AttributeName"]);
                    this.database.SetParameterValue(sqlStringCommand2, "TypeId", num);
                    this.database.SetParameterValue(sqlStringCommand2, "UsageMode", int.Parse(dataRow2["UsageMode"].ToString()));
                    this.database.SetParameterValue(sqlStringCommand2, "UseAttributeImage", bool.Parse(dataRow2["UseAttributeImage"].ToString()));
                    dataRow2["SelectedAttributeId"] = this.database.ExecuteScalar(sqlStringCommand2);
                }
            }
            using (DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Ecshop_AttributeValues;INSERT INTO Ecshop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl);SELECT @@IDENTITY;"))
            {
                this.database.AddInParameter(sqlStringCommand3, "AttributeId", DbType.Int32);
                this.database.AddInParameter(sqlStringCommand3, "ValueStr", DbType.String);
                this.database.AddInParameter(sqlStringCommand3, "ImageUrl", DbType.String);
                DataRow[] array4 = mappingSet.Tables["values"].Select("SelectedValueId=0");
                DataRow[] array2 = array4;
                for (int i = 0; i < array2.Length; i++)
                {
                    DataRow dataRow3 = array2[i];
                    int num2 = (int)mappingSet.Tables["attributes"].Select(string.Format("MappedAttributeId={0}", dataRow3["MappedAttributeId"]))[0]["SelectedAttributeId"];
                    this.database.SetParameterValue(sqlStringCommand3, "AttributeId", num2);
                    this.database.SetParameterValue(sqlStringCommand3, "ValueStr", dataRow3["ValueStr"]);
                    this.database.SetParameterValue(sqlStringCommand3, "ImageUrl", dataRow3["ImageUrl"]);
                    dataRow3["SelectedValueId"] = this.database.ExecuteScalar(sqlStringCommand3);
                }
            }
            mappingSet.AcceptChanges();
        }
        /// <summary>
        /// 获取当前可购买的商品数量
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int CheckPurchaseCount(string SkuId, int userId, out int MaxCount)
        {

            try
            {
                DbCommand storedProcCommand = this.database.GetStoredProcCommand("proc_CheckPurchaseCount");
                this.database.AddInParameter(storedProcCommand, "@SkuId", DbType.String, SkuId);
                this.database.AddInParameter(storedProcCommand, "@userId", DbType.Int32, userId);
                this.database.AddOutParameter(storedProcCommand, "MaxCount", DbType.Int32, 4);
                this.database.ExecuteNonQuery(storedProcCommand);
                int.TryParse(this.database.GetParameterValue(storedProcCommand, "MaxCount").ToString(), out MaxCount);
                return int.Parse(this.database.ExecuteScalar(storedProcCommand).ToString());
            }
            catch (Exception ee)
            {
                MaxCount = 0;
                return 0;
            }


        }

        /// <summary>
        /// 获取当前可购买的商品数量
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="userId"></param>
        /// <param name="MaxCount">限制数量</param>
        /// <param name="LimiteDay">限制天数</param>
        /// <returns></returns>
        public int CheckPurchaseCount(string SkuId, int userId, out int MaxCount, out int LimiteDay)
        {

            try
            {
                DbCommand storedProcCommand = this.database.GetStoredProcCommand("proc_NewCheckPurchaseCount");
                this.database.AddInParameter(storedProcCommand, "@SkuId", DbType.String, SkuId);
                this.database.AddInParameter(storedProcCommand, "@userId", DbType.Int32, userId);
                this.database.AddOutParameter(storedProcCommand, "MaxCount", DbType.Int32, 4);
                this.database.AddOutParameter(storedProcCommand, "LimiteDay", DbType.Int32, 4);
                this.database.ExecuteNonQuery(storedProcCommand);
                int.TryParse(this.database.GetParameterValue(storedProcCommand, "MaxCount").ToString(), out MaxCount);
                int.TryParse(this.database.GetParameterValue(storedProcCommand, "LimiteDay").ToString(), out LimiteDay);
                return int.Parse(this.database.ExecuteScalar(storedProcCommand).ToString());
            }
            catch (Exception ex)
            {
                MaxCount = 0;
                LimiteDay = 0;
                return 0;
            }


        }


        public int AddProduct(ProductInfo product, DbTransaction dbTran)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Product_Create");
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, product.CategoryId);
            this.database.AddInParameter(storedProcCommand, "MainCategoryPath", DbType.String, product.MainCategoryPath);
            this.database.AddInParameter(storedProcCommand, "TypeId", DbType.Int32, product.TypeId);
            this.database.AddInParameter(storedProcCommand, "ProductName", DbType.String, product.ProductName);
            this.database.AddInParameter(storedProcCommand, "ProductCode", DbType.String, product.ProductCode);
            this.database.AddInParameter(storedProcCommand, "ShortDescription", DbType.String, product.ShortDescription);
            this.database.AddInParameter(storedProcCommand, "Unit", DbType.String, product.Unit);
            this.database.AddInParameter(storedProcCommand, "Description", DbType.String, product.Description);
            this.database.AddInParameter(storedProcCommand, "MobbileDescription", DbType.String, product.MobblieDescription);
            this.database.AddInParameter(storedProcCommand, "Title", DbType.String, product.Title);
            this.database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, product.MetaDescription);
            this.database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, product.MetaKeywords);
            this.database.AddInParameter(storedProcCommand, "SaleStatus", DbType.Int32, (int)product.SaleStatus);
            this.database.AddInParameter(storedProcCommand, "AddedDate", DbType.DateTime, product.AddedDate);
            this.database.AddInParameter(storedProcCommand, "ImageUrl1", DbType.String, product.ImageUrl1);
            this.database.AddInParameter(storedProcCommand, "ImageUrl2", DbType.String, product.ImageUrl2);
            this.database.AddInParameter(storedProcCommand, "ImageUrl3", DbType.String, product.ImageUrl3);
            this.database.AddInParameter(storedProcCommand, "ImageUrl4", DbType.String, product.ImageUrl4);
            this.database.AddInParameter(storedProcCommand, "ImageUrl5", DbType.String, product.ImageUrl5);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl40", DbType.String, product.ThumbnailUrl40);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl60", DbType.String, product.ThumbnailUrl60);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl100", DbType.String, product.ThumbnailUrl100);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl160", DbType.String, product.ThumbnailUrl160);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl180", DbType.String, product.ThumbnailUrl180);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl220", DbType.String, product.ThumbnailUrl220);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl310", DbType.String, product.ThumbnailUrl310);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl410", DbType.String, product.ThumbnailUrl410);
            this.database.AddInParameter(storedProcCommand, "MarketPrice", DbType.Currency, product.MarketPrice);
            this.database.AddInParameter(storedProcCommand, "BrandId", DbType.Int32, product.BrandId);
            this.database.AddInParameter(storedProcCommand, "HasSKU", DbType.Boolean, product.HasSKU);
            this.database.AddInParameter(storedProcCommand, "IsfreeShipping", DbType.Boolean, product.IsfreeShipping);
            this.database.AddInParameter(storedProcCommand, "TaobaoProductId", DbType.Int64, product.TaobaoProductId);
            this.database.AddInParameter(storedProcCommand, "ReferralDeduct", DbType.Currency, product.ReferralDeduct);
            this.database.AddInParameter(storedProcCommand, "SubMemberDeduct", DbType.Currency, product.SubMemberDeduct);
            this.database.AddInParameter(storedProcCommand, "SubReferralDeduct", DbType.Currency, product.SubReferralDeduct);
            this.database.AddOutParameter(storedProcCommand, "ProductId", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "IsCustomsClearance", System.Data.DbType.Boolean, product.IsCustomsClearance);
            this.database.AddInParameter(storedProcCommand, "TaxRateId", System.Data.DbType.Int32, product.TaxRateId);
            this.database.AddInParameter(storedProcCommand, "SupplierId", System.Data.DbType.Int32, product.SupplierId);
            this.database.AddInParameter(storedProcCommand, "TemplateId", System.Data.DbType.Int32, product.TemplateId);
            this.database.AddInParameter(storedProcCommand, "ImportSourceId", System.Data.DbType.Int32, product.ImportSourceId);
            this.database.AddInParameter(storedProcCommand, "IsApproved", System.Data.DbType.Boolean, product.IsApproved);//是否核准
            this.database.AddInParameter(storedProcCommand, "BuyCardinality", System.Data.DbType.Int32, product.BuyCardinality);//购买基数

            this.database.AddInParameter(storedProcCommand, "UnitCode", DbType.String, product.UnitCode);
            this.database.AddInParameter(storedProcCommand, "Manufacturer", DbType.String, product.Manufacturer);
            this.database.AddInParameter(storedProcCommand, "ItemNo", DbType.String, product.ItemNo);
            this.database.AddInParameter(storedProcCommand, "BarCode", DbType.String, product.BarCode);

            this.database.AddInParameter(storedProcCommand, "Ingredient", DbType.String, product.Ingredient);
            this.database.AddInParameter(storedProcCommand, "ProductStandard", DbType.String, product.ProductStandard);
            this.database.AddInParameter(storedProcCommand, "ConversionRelation", DbType.Int32, product.ConversionRelation);
            this.database.AddInParameter(storedProcCommand, "ProductTitle", DbType.String, product.ProductTitle);
            this.database.AddInParameter(storedProcCommand, "SaleType", System.Data.DbType.Int32, product.SaleType == 0 ? 1 : product.SaleType);

            this.database.AddInParameter(storedProcCommand, "IsPromotion", System.Data.DbType.Boolean, product.IsPromotion);
            this.database.AddInParameter(storedProcCommand, "IsDisplayDiscount", System.Data.DbType.Boolean, product.IsDisplayDiscount);
            this.database.AddInParameter(storedProcCommand, "IsApprovedPrice", System.Data.DbType.Int32, product.IsApprovedPrice);
            this.database.AddInParameter(storedProcCommand, "EnglishName", System.Data.DbType.String, product.EnglishName);
            this.database.AddInParameter(storedProcCommand, "SysProductName", System.Data.DbType.String, product.SysProductName);

            this.database.AddInParameter(storedProcCommand, "Purchase", System.Data.DbType.Int32, product.Purchase);
            this.database.AddInParameter(storedProcCommand, "SectionDay", System.Data.DbType.Int32, product.SectionDay);
            this.database.AddInParameter(storedProcCommand, "Purchase_MaxNum", System.Data.DbType.Int32, product.PurchaseMaxNum);

            this.database.ExecuteNonQuery(storedProcCommand, dbTran);
            return (int)this.database.GetParameterValue(storedProcCommand, "ProductId");
        }
        private decimal Opreateion(decimal opreation1, decimal opreation2, string operation)
        {
            decimal result = 0m;
            if (operation != null)
            {
                if (!(operation == "+"))
                {
                    if (!(operation == "-"))
                    {
                        if (!(operation == "*"))
                        {
                            if (operation == "/")
                            {
                                result = opreation1 * opreation2;
                            }
                        }
                        else
                        {
                            result = opreation1 * opreation2;
                        }
                    }
                    else
                    {
                        result = opreation1 - opreation2;
                    }
                }
                else
                {
                    result = opreation1 + opreation2;
                }
            }
            return result;
        }
        public bool AddProductSKUs(int productId, Dictionary<string, SKUItem> skus, DbTransaction dbTran, List<ProductsCombination> combinationInfo = null)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_SKUs(SkuId, ProductId, SKU, Weight, Stock,FactStock,CostPrice, SalePrice,DeductFee,ProductRegistrationNumber,LJNo,WMSStock,GrossWeight) VALUES(@SkuId, @ProductId, @SKU, @Weight, @Stock,@FactStock,@CostPrice, @SalePrice,@DeductFee,@ProductRegistrationNumber,@LJNo,@WMSStock,@GrossWeight)");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "SKU", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "Weight", DbType.Decimal);
            this.database.AddInParameter(sqlStringCommand, "Stock", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "FactStock", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "CostPrice", DbType.Currency);
            this.database.AddInParameter(sqlStringCommand, "SalePrice", DbType.Currency);
            this.database.AddInParameter(sqlStringCommand, "DeductFee", DbType.Decimal);
            this.database.AddInParameter(sqlStringCommand, "ProductRegistrationNumber", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "LJNo", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "WMSStock", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "GrossWeight", DbType.Decimal);
            DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("INSERT INTO Ecshop_SKUItems(SkuId, AttributeId, ValueId) VALUES(@SkuId, @AttributeId, @ValueId)");
            this.database.AddInParameter(sqlStringCommand2, "SkuId", DbType.String);
            this.database.AddInParameter(sqlStringCommand2, "AttributeId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand2, "ValueId", DbType.Int32);
            DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("INSERT INTO Ecshop_SKUMemberPrice(SkuId, GradeId, MemberSalePrice) VALUES(@SkuId, @GradeId, @MemberSalePrice)");
            this.database.AddInParameter(sqlStringCommand3, "SkuId", DbType.String);
            this.database.AddInParameter(sqlStringCommand3, "GradeId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand3, "MemberSalePrice", DbType.Currency);
            bool result;
            DbCommand sqlStringCommand4 = this.database.GetSqlStringCommand(" INSERT INTO Ecshop_ProductsCombination (ProductId,SkuId,CombinationSkuId,SKU,Quantity,Price,ProductName,ThumbnailsUrl,[Weight],SKUContent) VALUES(@ProductId,@SkuId,@CombinationSkuId,@SKU,@Quantity,@Price,@ProductName,@ThumbnailsUrl,@Weight,@SKUContent)");
            this.database.AddInParameter(sqlStringCommand4, "ProductId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand4, "SkuId", DbType.String);
            this.database.AddInParameter(sqlStringCommand4, "CombinationSkuId", DbType.String);
            this.database.AddInParameter(sqlStringCommand4, "SKU", DbType.String);
            this.database.AddInParameter(sqlStringCommand4, "Quantity", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand4, "Price", DbType.Currency);
            this.database.AddInParameter(sqlStringCommand4, "ProductName", DbType.String);
            this.database.AddInParameter(sqlStringCommand4, "ThumbnailsUrl", DbType.String);
            this.database.AddInParameter(sqlStringCommand4, "Weight", DbType.Currency);
            this.database.AddInParameter(sqlStringCommand4, "SKUContent", DbType.String);
            try
            {
                this.database.SetParameterValue(sqlStringCommand, "ProductId", productId);
                foreach (SKUItem current in skus.Values)
                {
                    string value = productId.ToString(CultureInfo.InvariantCulture) + "_" + current.SkuId;
                    this.database.SetParameterValue(sqlStringCommand, "SkuId", value);
                    this.database.SetParameterValue(sqlStringCommand, "SKU", current.SKU);
                    this.database.SetParameterValue(sqlStringCommand, "Weight", current.Weight);
                    this.database.SetParameterValue(sqlStringCommand, "Stock", current.Stock);
                    this.database.SetParameterValue(sqlStringCommand, "FactStock", current.FactStock);
                    this.database.SetParameterValue(sqlStringCommand, "CostPrice", current.CostPrice);
                    this.database.SetParameterValue(sqlStringCommand, "SalePrice", Math.Round(current.SalePrice, SettingsManager.GetMasterSettings(true).DecimalLength));
                    this.database.SetParameterValue(sqlStringCommand, "DeductFee", current.DeductFee);
                    this.database.SetParameterValue(sqlStringCommand, "ProductRegistrationNumber", current.ProductRegistrationNumber);
                    this.database.SetParameterValue(sqlStringCommand, "LJNo", current.LJNo);
                    this.database.SetParameterValue(sqlStringCommand, "WMSStock", current.WMSStock);
                    this.database.SetParameterValue(sqlStringCommand, "GrossWeight", current.GrossWeight);

                    if (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 0)
                    {
                        result = false;
                        return result;
                    }
                    this.database.SetParameterValue(sqlStringCommand2, "SkuId", value);
                    foreach (int current2 in current.SkuItems.Keys)
                    {
                        this.database.SetParameterValue(sqlStringCommand2, "AttributeId", current2);
                        this.database.SetParameterValue(sqlStringCommand2, "ValueId", current.SkuItems[current2]);
                        this.database.ExecuteNonQuery(sqlStringCommand2, dbTran);
                    }
                    this.database.SetParameterValue(sqlStringCommand3, "SkuId", value);
                    foreach (int current3 in current.MemberPrices.Keys)
                    {
                        this.database.SetParameterValue(sqlStringCommand3, "GradeId", current3);
                        this.database.SetParameterValue(sqlStringCommand3, "MemberSalePrice", current.MemberPrices[current3]);
                        this.database.ExecuteNonQuery(sqlStringCommand3, dbTran);
                    }

                    // 组合明细
                    string tempsql = string.Format("delete from Ecshop_ProductsCombination where CombinationSkuId =@comskuid");
                    DbCommand tempcommand = this.database.GetSqlStringCommand(tempsql);
                    this.database.AddInParameter(tempcommand, "comskuid", DbType.String, value);
                    this.database.ExecuteNonQuery(tempcommand, dbTran);
                    if (combinationInfo != null)
                    {
                        foreach (ProductsCombination combination in combinationInfo)
                        {
                            this.database.SetParameterValue(sqlStringCommand4, "ProductId", combination.ProductId);
                            this.database.SetParameterValue(sqlStringCommand4, "SkuId", combination.SkuId);
                            this.database.SetParameterValue(sqlStringCommand4, "CombinationSkuId", value);
                            this.database.SetParameterValue(sqlStringCommand4, "SKU", combination.SKU);
                            this.database.SetParameterValue(sqlStringCommand4, "Quantity", combination.Quantity);
                            this.database.SetParameterValue(sqlStringCommand4, "Price", combination.Price);
                            this.database.SetParameterValue(sqlStringCommand4, "ProductName", combination.ProductName);
                            this.database.SetParameterValue(sqlStringCommand4, "ThumbnailsUrl", combination.ThumbnailsUrl);
                            this.database.SetParameterValue(sqlStringCommand4, "Weight", combination.Weight);
                            this.database.SetParameterValue(sqlStringCommand4, "SKUContent", combination.SKUContent);
                            this.database.ExecuteNonQuery(sqlStringCommand4, dbTran);
                        }
                    }
                }
                result = true;
            }
            catch (Exception var_7_3BD)
            {
                result = false;
            }
            return result;
        }
        public bool DeleteProductSKUS(int productId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_SKUs WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            bool result;
            try
            {
                if (dbTran == null)
                {
                    this.database.ExecuteNonQuery(sqlStringCommand);
                }
                else
                {
                    this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public bool AddProductAttributes(int productId, Dictionary<int, IList<int>> attributes, DbTransaction dbTran)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("DELETE FROM Ecshop_ProductAttributes WHERE ProductId = {0};", productId);
            int num = 0;
            if (attributes != null)
            {
                foreach (int current in attributes.Keys)
                {
                    foreach (int current2 in attributes[current])
                    {
                        num++;
                        stringBuilder.AppendFormat(" INSERT INTO Ecshop_ProductAttributes (ProductId, AttributeId, ValueId) VALUES ({0}, {1}, {2})", productId, current, current2);
                    }
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            bool result;
            if (dbTran == null)
            {
                result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 0);
            }
            else
            {
                result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 0);
            }
            return result;
        }
        public int GetMaxSequence()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT MAX(DisplaySequence) FROM Ecshop_Products");
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            return (obj == DBNull.Value) ? 0 : ((int)obj);
        }
        public bool UpdateProduct(ProductInfo product, DbTransaction dbTran)
        {

            ProductBatchDao dao = new ProductBatchDao();
            //计算商品权重值
            Dictionary<string, decimal> dic = dao.CalculateProductFraction(product.ProductId.ToString());
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Product_Update");
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, product.CategoryId);
            this.database.AddInParameter(storedProcCommand, "MainCategoryPath", DbType.String, product.MainCategoryPath);
            this.database.AddInParameter(storedProcCommand, "TypeId", DbType.Int32, product.TypeId);
            this.database.AddInParameter(storedProcCommand, "ProductName", DbType.String, product.ProductName);
            this.database.AddInParameter(storedProcCommand, "ProductCode", DbType.String, product.ProductCode);
            this.database.AddInParameter(storedProcCommand, "ShortDescription", DbType.String, product.ShortDescription);
            this.database.AddInParameter(storedProcCommand, "Unit", DbType.String, product.Unit);
            this.database.AddInParameter(storedProcCommand, "Description", DbType.String, product.Description);
            this.database.AddInParameter(storedProcCommand, "MobbileDescription", DbType.String, product.MobblieDescription);
            this.database.AddInParameter(storedProcCommand, "Title", DbType.String, product.Title);
            this.database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, product.MetaDescription);
            this.database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, product.MetaKeywords);
            this.database.AddInParameter(storedProcCommand, "SaleStatus", DbType.Int32, (int)product.SaleStatus);
            this.database.AddInParameter(storedProcCommand, "DisplaySequence", DbType.Currency, product.DisplaySequence);
            this.database.AddInParameter(storedProcCommand, "ImageUrl1", DbType.String, product.ImageUrl1);
            this.database.AddInParameter(storedProcCommand, "ImageUrl2", DbType.String, product.ImageUrl2);
            this.database.AddInParameter(storedProcCommand, "ImageUrl3", DbType.String, product.ImageUrl3);
            this.database.AddInParameter(storedProcCommand, "ImageUrl4", DbType.String, product.ImageUrl4);
            this.database.AddInParameter(storedProcCommand, "ImageUrl5", DbType.String, product.ImageUrl5);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl40", DbType.String, product.ThumbnailUrl40);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl60", DbType.String, product.ThumbnailUrl60);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl100", DbType.String, product.ThumbnailUrl100);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl160", DbType.String, product.ThumbnailUrl160);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl180", DbType.String, product.ThumbnailUrl180);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl220", DbType.String, product.ThumbnailUrl220);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl310", DbType.String, product.ThumbnailUrl310);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl410", DbType.String, product.ThumbnailUrl410);
            this.database.AddInParameter(storedProcCommand, "MarketPrice", DbType.Currency, product.MarketPrice);
            this.database.AddInParameter(storedProcCommand, "BrandId", DbType.Int32, product.BrandId);
            this.database.AddInParameter(storedProcCommand, "HasSKU", DbType.Boolean, product.HasSKU);
            this.database.AddInParameter(storedProcCommand, "IsfreeShipping", DbType.Boolean, product.IsfreeShipping);
            this.database.AddInParameter(storedProcCommand, "VistiCounts", DbType.Int32, product.VistiCounts);
            this.database.AddInParameter(storedProcCommand, "SaleCounts", DbType.Int32, product.SaleCounts);
            this.database.AddInParameter(storedProcCommand, "ShowSaleCounts", DbType.Int32, product.ShowSaleCounts);
            this.database.AddInParameter(storedProcCommand, "ReferralDeduct", DbType.Currency, product.ReferralDeduct);
            this.database.AddInParameter(storedProcCommand, "SubMemberDeduct", DbType.Currency, product.SubMemberDeduct);
            this.database.AddInParameter(storedProcCommand, "SubReferralDeduct", DbType.Currency, product.SubReferralDeduct);
            this.database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, product.ProductId);
            this.database.AddInParameter(storedProcCommand, "TaxRateId", DbType.Int32, product.TaxRateId);
            this.database.AddInParameter(storedProcCommand, "SupplierId", System.Data.DbType.Int32, product.SupplierId);
            this.database.AddInParameter(storedProcCommand, "TemplateId", System.Data.DbType.Int32, product.TemplateId);
            this.database.AddInParameter(storedProcCommand, "IsCustomsClearance", System.Data.DbType.Boolean, product.IsCustomsClearance);
            this.database.AddInParameter(storedProcCommand, "ImportSourceId", System.Data.DbType.Int32, product.ImportSourceId);
            this.database.AddInParameter(storedProcCommand, "AdminFraction", System.Data.DbType.Decimal, product.AdminFraction);
            this.database.AddInParameter(storedProcCommand, "Fraction", System.Data.DbType.Decimal, dic[product.ProductId.ToString()] + product.AdminFraction);
            this.database.AddInParameter(storedProcCommand, "BuyCardinality", System.Data.DbType.Int32, product.BuyCardinality);
            this.database.AddInParameter(storedProcCommand, "UnitCode", System.Data.DbType.String, product.UnitCode);
            this.database.AddInParameter(storedProcCommand, "Manufacturer", DbType.String, product.Manufacturer);
            this.database.AddInParameter(storedProcCommand, "ItemNo", DbType.String, product.ItemNo);
            this.database.AddInParameter(storedProcCommand, "BarCode", DbType.String, product.BarCode);
            this.database.AddInParameter(storedProcCommand, "Ingredient", DbType.String, product.Ingredient);
            this.database.AddInParameter(storedProcCommand, "ProductStandard", DbType.String, product.ProductStandard);
            this.database.AddInParameter(storedProcCommand, "SaleType", System.Data.DbType.Int32, product.SaleType == 0 ? 1 : product.SaleType);
            this.database.AddInParameter(storedProcCommand, "ConversionRelation", DbType.Int32, product.ConversionRelation);
            this.database.AddInParameter(storedProcCommand, "ProductTitle", DbType.String, product.ProductTitle);
            this.database.AddInParameter(storedProcCommand, "IsPromotion", DbType.Boolean, product.IsPromotion);
            this.database.AddInParameter(storedProcCommand, "IsDisplayDiscount", DbType.Boolean, product.IsDisplayDiscount);
            this.database.AddInParameter(storedProcCommand, "EnglishName", DbType.String, product.EnglishName);
            this.database.AddInParameter(storedProcCommand, "SysProductName", DbType.String, product.SysProductName);
            this.database.AddInParameter(storedProcCommand, "Purchase", System.Data.DbType.Int32, product.Purchase);
            this.database.AddInParameter(storedProcCommand, "SectionDay", System.Data.DbType.Int32, product.SectionDay);
            this.database.AddInParameter(storedProcCommand, "Purchase_MaxNum", System.Data.DbType.Int32, product.PurchaseMaxNum);


            bool result;
            if (dbTran != null)
            {
                result = (this.database.ExecuteNonQuery(storedProcCommand, dbTran) > 0);
            }
            else
            {
                result = (this.database.ExecuteNonQuery(storedProcCommand) > 0);
            }
            return result;
        }
        public bool UpdateProductsSaleCounts(ProductInfo product, DbTransaction dbTran)
        {
            DbCommand cmd = this.database.GetSqlStringCommand("UPDATE Ecshop_Products SET SaleCounts=@SaleCounts,ShowSaleCounts=@ShowSaleCounts WHERE ProductId = @ProductId");

            this.database.AddInParameter(cmd, "SaleCounts", DbType.Int32, product.SaleCounts);
            this.database.AddInParameter(cmd, "ShowSaleCounts", DbType.Int32, product.ShowSaleCounts);
            this.database.AddInParameter(cmd, "ProductId", DbType.Int32, product.ProductId);

            bool result;
            if (dbTran != null)
            {
                result = (this.database.ExecuteNonQuery(cmd, dbTran) > 0);
            }
            else
            {
                result = (this.database.ExecuteNonQuery(cmd) > 0);
            }

            return result;
        }
        public bool UpdateProductCategory(int productId, int newCategoryId, string mainCategoryPath)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Products SET CategoryId = @CategoryId, MainCategoryPath = @MainCategoryPath WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, newCategoryId);
            this.database.AddInParameter(sqlStringCommand, "MainCategoryPath", DbType.String, mainCategoryPath);
            return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
        }
        public int DeleteProduct(string productIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Ecshop_Products WHERE ProductId IN ({0}); DELETE FROM Ecshop_ProductTag WHERE ProductId IN ({0})", productIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Products SET SaleStatus = {0} WHERE ProductId IN ({1})", (int)saleStatus, productIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public int UpdateProductShipFree(string productIds, bool isFree)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Products SET IsfreeShipping = {0} WHERE ProductId IN ({1})", Convert.ToInt32(isFree), productIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public DbQueryResult GetRelatedProducts(ProductQuery page)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(" SaleStatus = {0}", 1);
            if (page.SupplierId.HasValue)
            {
                stringBuilder.AppendFormat(" and  SupplierId = {0}", page.SupplierId);
            }
            stringBuilder.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Ecshop_RelatedProducts WHERE ProductId = {0})", page.ProductId);
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence";
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Ecshop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
        }
        public bool AddRelatedProduct(int productId, int relatedProductId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_RelatedProducts(ProductId, RelatedProductId) VALUES (@ProductId, @RelatedProductId)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, relatedProductId);
            bool result;
            try
            {
                result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public bool RemoveRelatedProduct(int productId, int relatedProductId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_RelatedProducts WHERE ProductId = @ProductId AND RelatedProductId = @RelatedProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, relatedProductId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool ClearRelatedProducts(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_RelatedProducts WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public decimal GetProductReferralDeduct(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ReferralDeduct FROM Ecshop_Products WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            decimal result;
            if (obj == DBNull.Value)
            {
                result = HiContext.Current.SiteSettings.ReferralDeduct;
            }
            else
            {
                result = (decimal)obj;
            }
            return result;
        }
        public decimal GetProductSubReferralDeduct(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SubReferralDeduct FROM Ecshop_Products WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            decimal result;
            if (obj == DBNull.Value)
            {
                result = HiContext.Current.SiteSettings.SubReferralDeduct;
            }
            else
            {
                result = (decimal)obj;
            }
            return result;
        }
        public decimal GetProductSubMemberDeduct(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SubMemberDeduct FROM Ecshop_Products WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            decimal result;
            if (obj == DBNull.Value)
            {
                result = HiContext.Current.SiteSettings.SubMemberDeduct;
            }
            else
            {
                result = (decimal)obj;
            }
            return result;
        }
        public IList<int> GetProductIsCustomsClearance(string productIds)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT IsCustomsClearance FROM Ecshop_Products WHERE ProductId in ({0})", productIds));
            IList<int> list = new List<int>();
            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    int flag = 0;
                    if (System.DBNull.Value == dataReader["IsCustomsClearance"])
                    {
                        flag = 0;
                        list.Add(flag);
                    }
                    else
                    {
                        if ((bool)dataReader["IsCustomsClearance"])
                        {
                            list.Add(1);
                        }
                        else
                        {
                            list.Add(0);
                        }
                    }
                }
            }
            return list;
        }
        public int GetProductStock(string skuId)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select stock from 
                dbo.Ecshop_SKUs  where SkuId=@sku");
            this.database.AddInParameter(sqlStringCommand, "sku", DbType.String, skuId);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj != null)
                return (int)obj;
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 修改商品二维码
        /// </summary>
        /// <returns></returns>
        public bool UpdateQRcode(Dictionary<string, string> dic)
        {
            try
            {

                StringBuilder sqlStr = new StringBuilder();
                foreach (string key in dic.Keys)
                {

                    sqlStr.Append(string.Format("update Ecshop_Products set QRcode='{0}' where ProductId={1};", dic[key], key));

                }
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sqlStr.ToString());
                return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
            }
            catch (Exception ee)
            {
                return false;
            }

        }

        /// <summary>
        /// 根据商品ID获取商品的原产地信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ImportSourceTypeInfo GetProductImportSourceType(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT isnull(b.ImportSourceId,0) ImportSourceId,b.Icon,b.EnArea,b.CnArea,b.Remark,b.DisplaySequence,b.AddTime,b.HSCode,b.FavourableFlag,b.StandardCName FROM Ecshop_Products p left join Ecshop_ImportSourceType b on p.ImportSourceId= b.ImportSourceId WHERE p.ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);

            ImportSourceTypeInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    result = DataMapper.PopulateImportSourceTypeInfo(dataReader);
                }
            }
            return result;
        }

        public DataTable GetSuggestProducts(int userId, int count)
        {
            DataTable result = new DataTable();
            string query = string.Format("select top {0} * from vw_SuggestProductList where UserId = {1} order by newid() asc", count, userId);
            /*if (userId == 0)
            {
                query = string.Format("select top {0} * from vw_SuggestProductList order by newid() asc", count);
            }*/

            query = string.Format("select top {0} * from vw_SuggestProductList order by newid() asc", count);

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            //this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, userId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        public DataTable GetSuggestProducts(int userId, string username, int count)
        {
            DataTable result = new DataTable();

            DbCommand command = this.database.GetStoredProcCommand("cp_API_SuggestProducts_Get");
            this.database.AddInParameter(command, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(command, "UserName", DbType.String, username);
            this.database.AddInParameter(command, "Count", DbType.Int32, count);

            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public DbQueryResult GetProductSelect(ProductQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1 ");
            if (query.SaleStatus != ProductSaleStatus.All)
            {
                stringBuilder.AppendFormat(" AND SaleStatus = {0}", 1);
            }
            if (query.BrandId.HasValue)
            {
                stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
            }
            if (query.TypeId.HasValue)
            {
                stringBuilder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
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
            if (query.IsIncludePromotionProduct.HasValue)
            {
                if (!query.IsIncludePromotionProduct.Value)
                {
                    stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Ecshop_PromotionProducts)");
                }
            }
            if (query.IsIncludeBundlingProduct.HasValue)
            {
                if (!query.IsIncludeBundlingProduct.Value)
                {
                    stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Ecshop_BundlingProductItems)");
                }
            }
            if (query.IsIncludeHomeProduct.HasValue)
            {
                if (!query.IsIncludeHomeProduct.Value)
                {
                    if (!query.Client.HasValue)
                    {
                        stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Vshop_HomeProducts)");
                    }
                    else
                    {
                        stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Vshop_HomeProducts where Client=" + query.Client + " )");
                    }
                }
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue)
            {
                if (query.CategoryId.Value > 0)
                {
                    stringBuilder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%') ", query.MaiCategoryPath);
                }
                else
                {
                    stringBuilder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
                }
            }
            if (query.ImportSourceId.HasValue)
            {
                stringBuilder.AppendFormat(" AND ImportSourceId = {0}", query.ImportSourceId.Value);
            }
            if (query.SupplierId.HasValue)
            {
                stringBuilder.AppendFormat(" AND SupplierId = {0}", query.SupplierId.Value);
            }
            string selectFields = "SkuId,ProductId,ThumbnailUrl40,ProductName,ProductCode,TaxRate,SaleStatus,Icon,CnArea,MarketPrice,CategoryId,BrandId,ImportSourceId,SupplierId,TypeId,MainCategoryPath,ExtendCategoryPath,Stock,CostPrice,SalePrice,strAttName,DisplaySequence,[Weight],DeductFee,SKU,TemplateId";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_ProductSkuSelect p", "ProductId", stringBuilder.ToString(), selectFields);
        }

        public IList<ShoppingCartItemInfo> GetSkuList(string skuIds)
        {
            /*PromotionId,PromoteType,PromotionName,IsSendGift*/
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select SkuId,ProductId,[Weight],ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,TemplateId,TaxRate,SupplierId,productcode SKU,strAttName SKUContent,productname Name,SalePrice AdjustedPrice,IsfreeShipping,SalePrice MemberPrice,TaxRateId,SupplierName,DeductFee from dbo.vw_Ecshop_ProductSkuSelect where SaleStatus =1 and skuid in {0} order by skuid desc", skuIds));
            IList<ShoppingCartItemInfo> result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<ShoppingCartItemInfo>(dataReader);
            }
            return result;
        }

        public DbQueryResult GetProducts(int saleStatus, int pageIndex, int pageSize)
        {
            ProductQuery query = new ProductQuery();
            query.SaleStatus = (ProductSaleStatus)saleStatus;
            query.PageIndex = pageIndex;
            query.PageSize = pageSize;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1 ");
            if (query.SaleStatus != ProductSaleStatus.All)
            {
                stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
            }
            else
            {
                stringBuilder.AppendFormat(" AND SaleStatus <> ({0})", 0);
            }

            string selectFields = "ProductId ";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Ecshop_Products p", "ProductId", stringBuilder.ToString(), selectFields);
        }


        public DataTable GeSuggestProducts(ClientType client)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select p.ProductId, ProductCode, ProductName,ShortDescription,ThumbnailUrl40,ThumbnailUrl160,ThumbnailUrl100,ThumbnailUrl220,ThumbnailUrl180,ThumbnailUrl310,ThumbnailUrl410,p.TaxRate,MarketPrice,ShowSaleCounts,SaleCounts, Stock,t.DisplaySequence,fastbuy_skuid,TaxRate,");
            Member member = HiContext.Current.User as Member;
            if (member != null)
            {
                int discount = new MemberGradeDao().GetMemberGrade(member.GradeId).Discount;
                stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", member.GradeId);
                stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, discount);
            }
            else
            {
                stringBuilder.Append("SalePrice");
            }
            stringBuilder.Append(" from vw_Ecshop_BrowseProductList p inner join  Ecshop_SuggestProducts t on p.productid=t.ProductId ");
            stringBuilder.AppendFormat(" and SaleStatus = {0}  and IsApproved=1 WHERE Client = {1}", 1, (int)client);
            stringBuilder.Append(" order by t.DisplaySequence asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }


        public bool RemoveSuggestProduct(int productId, ClientType client)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_SuggestProducts WHERE ProductId = @ProductId AND Client = @Client");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)client);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool RemoveAllSuggestProduct(ClientType client)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Ecshop_SuggestProducts WHERE Client = {0}", (int)client));
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool UpdateSuggestProductSequence(ClientType client, int ProductId, int displaysequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_SuggestProducts set DisplaySequence=@DisplaySequence where ProductId=@ProductId AND Client = @Client");
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            this.database.AddInParameter(sqlStringCommand, "@ProductId", DbType.Int32, ProductId);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)client);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool AddSuggestProdcut(int productId, ClientType client)
        {
            bool flag = true;
            string commandText = string.Format("SELECT COUNT(1) FROM Ecshop_SuggestProducts WHERE ProductId={0} AND Client = {1}", productId, (int)client);
            int num = (int)this.database.ExecuteScalar(CommandType.Text, commandText);
            bool result;
            if (num == 0)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_SuggestProducts(ProductId, Client,DisplaySequence) VALUES (@ProductId, @Client,0)");
                this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
                this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, client);
                try
                {
                    result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
                    return result;
                }
                catch
                {
                    flag = false;
                }
            }
            result = flag;
            return result;
        }

        /// <summary>
        /// 获取商品购买基数>1商品ID与购买基数。
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public Dictionary<int, int> GetBuyCardinality(params int[] productIds)
        {
            if (productIds == null || productIds.Length == 0)
                return null;
            Dictionary<int, int> result = new Dictionary<int, int>();
            DbCommand command = this.database.GetSqlStringCommand("SELECT ProductId,BuyCardinality FROM dbo.Ecshop_Products where ProductId in (" + string.Join(",", productIds) + ") AND BuyCardinality>1");
            IDataReader dataReader = this.database.ExecuteReader(command);
            while (dataReader.Read())
            {
                result.Add((int)dataReader["ProductId"], (int)dataReader["BuyCardinality"]);
            }

            return result;
        }

        public DataTable GetHotBuyProduct(int categoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select p.ThumbnailUrl40,p.ThumbnailUrl60,p.ThumbnailUrl100,p.ThumbnailUrl160,p.ThumbnailUrl180,p.ThumbnailUrl220,p.ProductId,ProductName from Ecshop_Products p
                                                                            join Ecshop_Categories c
                                                                            on p.CategoryId = c.CategoryId
                                                                            join (
                                                                            select ProductId,TagId from Ecshop_ProductTag where TagId = (SELECT top 1 TagID FROM  Ecshop_Tags where TagName='热卖')
                                                                            ) tag
                                                                            on p.ProductId = tag.ProductId
                                                                            where p.SaleStatus = 1 and c.ParentCategoryId =@CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        public DataSet GetSupplierProductDetail(ProductQuery query)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"select productId,ProductName,SupplierName,CostPrice,SalePrice,StrSaleStatus,Stock,SKU,ProductCode,CnArea,BrandName,SaleStatus,BrandId,TypeId,MainCategoryPath
                                   ,ExtendCategoryPath,CategoryId,AddedDate,ImportSourceId
                                   ,SupplierId,TemplateId,IsApproved,BarCode,IsApprovedPrice,IsAllClassify    
                                   from vw_SupplierProductDetail where 1= 1");

            if (query.SaleStatus != ProductSaleStatus.All)
            {
                stringBuilder.AppendFormat(" AND  SaleStatus = {0}", (int)query.SaleStatus);
            }
            else
            {
                stringBuilder.AppendFormat(" AND  SaleStatus <> {0}", 0);
            }
            if (query.BrandId.HasValue)
            {
                stringBuilder.AppendFormat(" AND  BrandId = {0}", query.BrandId.Value);
            }
            if (query.TypeId.HasValue)
            {
                stringBuilder.AppendFormat(" AND  TypeId = {0}", query.TypeId.Value);
            }
            if (query.TagId.HasValue)
            {
                stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Ecshop_ProductTag WHERE TagId={0})", query.TagId);
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
                stringBuilder.AppendFormat(" AND  ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
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
            if (query.CategoryId.HasValue)
            {
                if (query.CategoryId.Value > 0)
                {
                    stringBuilder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR  ExtendCategoryPath LIKE '{0}|%') ", query.MaiCategoryPath);
                }
                else
                {
                    stringBuilder.Append(" AND (CategoryId = 0 OR  CategoryId IS NULL)");
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
            if (query.ImportSourceId.HasValue)
            {
                stringBuilder.AppendFormat(" AND ImportSourceId = {0}", query.ImportSourceId.Value);
            }
            if (query.SupplierId.HasValue)
            {
                stringBuilder.AppendFormat(" AND SupplierId = {0}", query.SupplierId.Value);
            }
            if (query.TemplateId.HasValue)
            {
                stringBuilder.AppendFormat(" AND TemplateId = {0}", query.TemplateId.Value);
            }
            if (query.IsApproved.HasValue && query.IsApproved.Value != -1)//是否审核过滤
            {
                stringBuilder.AppendFormat(" AND IsApproved={0}", query.IsApproved.Value);
            }

            if (query.IsApprovedPrice.HasValue && query.IsApprovedPrice.Value != -1)
            {
                stringBuilder.AppendFormat(" AND IsApprovedPrice={0}", query.IsApprovedPrice.Value);
            }


            if (query.IsAllClassify.HasValue && query.IsAllClassify.Value != -1)
            {
                stringBuilder.AppendFormat(" AND IsAllClassify={0}", query.IsAllClassify.Value);
            }

            stringBuilder.Append(" order by ProductId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }



        public DataTable GetHistoryProducts(int userId, string username, int pageIndex, int pageSize, out int count)
        {
            count = 0;

            DataTable result = new DataTable();

            DbCommand command = this.database.GetStoredProcCommand("cp_API_HistoryProducts_Get");
            this.database.AddInParameter(command, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(command, "UserName", DbType.String, username);
            this.database.AddInParameter(command, "PageIndex", DbType.Int32, pageIndex);
            this.database.AddInParameter(command, "PageSize", DbType.Int32, pageSize);
            this.database.AddOutParameter(command, "Count", DbType.Int32, 4);

            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {

                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }

            count = (int)this.database.GetParameterValue(command, "Count");

            return result;
        }


        public bool AcceptPriceApprove(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_Products set IsApprovedPrice=@IsApprovedPrice,ApprovedPriceDescription=@ApprovedPriceDescription where ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "@ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "@IsApprovedPrice", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "@ApprovedPriceDescription", DbType.String, "审核通过");
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool AcceptPriceApprove(int productId, string ApprovedPriceDescription)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_Products set IsApprovedPrice=@IsApprovedPrice,ApprovedPriceDescription=@ApprovedPriceDescription where ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "@ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "@IsApprovedPrice", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "@ApprovedPriceDescription", DbType.String, ApprovedPriceDescription);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool IsPriceApproved(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(ProductId) from  Ecshop_Products where ProductId=@ProductId and IsApprovedPrice=1");
            this.database.AddInParameter(sqlStringCommand, "@ProductId", DbType.Int32, productId);
            object result = this.database.ExecuteScalar(sqlStringCommand);
            int count = 0;
            if (result != null)
            {
                int.TryParse(result.ToString(), out count);
            }

            return count > 0;

        }


        public bool RefusePriceApprove(int productId, string approvedPriceDescription)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_Products set IsApprovedPrice=@IsApprovedPrice,ApprovedPriceDescription=@ApprovedPriceDescription where ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "@ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "@IsApprovedPrice", DbType.Int32, 2);
            this.database.AddInParameter(sqlStringCommand, "@ApprovedPriceDescription", DbType.String, approvedPriceDescription);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public int ReSubmitPriceApprove(string productIds, ApprovePriceStatus status)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Products SET IsApprovedPrice = {0},ApprovedPriceDescription='' WHERE ProductId IN ({1}) and IsApprovedPrice=2", (int)status, productIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }


        /// <summary>
        /// 是否存在未归档的商品,排除组合商品
        /// </summary>
        /// <param name="strProductIds"></param>
        /// <returns></returns>
        public bool IsExitNoClassifyProduct(string strProductIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(1) from vw_Ecshop_ProductClassifyList where PStatus<>2 and SaleType<>2 and ProductId in(" + DataHelper.CleanSearchString(strProductIds) + ")");
            object objresult;
            int result = 0;
            objresult = this.database.ExecuteScalar(sqlStringCommand);
            if (objresult != null)
            {
                int.TryParse(objresult.ToString(), out result);
            }

            return result > 0;

        }

        /// <summary>
        /// 根据商品ID获取其组合商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public DataTable GetCombinationDataByProductId(int productId)
        {
            DataTable result = new DataTable();
            string query = string.Format(@"select pc.*,(select top 1 ISNULL(GrossWeight,0) from Ecshop_SKUS where SkuId=pc.SkuId) GrossWeight
                                            from Ecshop_ProductsCombination pc 
                                            inner join Ecshop_SKUS s on pc.CombinationSkuId = s.SkuId 
                                            where s.ProductId = {0} ", productId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        public List<ProductsCombination> GetProductsCombinationsBySku(string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select t.TaxRate, com.*   
 from Ecshop_ProductsCombination com   
 left join Ecshop_SKUs s on s.SkuId = com.CombinationSkuId   
 left join Ecshop_Products  p on p.ProductId = s.ProductId   
 left join Ecshop_Products pp on pp.ProductId = com.ProductId   
 left join Ecshop_TaxRate as t on pp.TaxRateId=t.taxId   
 where com.CombinationSkuId = @SkuId  and p.SaleStatus = 1");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            List<ProductsCombination> list = new List<ProductsCombination>();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    ProductsCombination combination = DataMapper.PopulateCombinationCartItem(dataReader);
                    list.Add(combination);
                }
            }
            return list;
        }

        /// <summary>
        /// 更新检测状态
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="remark"></param>
        /// <param name="status"></param>
        public void setProductCheck(string prodid, string remark, string status)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_Products set remarks=@remark,CheckStatus=@status where ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "@ProductId", DbType.Int32, prodid);
            this.database.AddInParameter(sqlStringCommand, "@remark", DbType.String, remark);
            this.database.AddInParameter(sqlStringCommand, "@status", DbType.String, status);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        /// <summary>
        /// 更新商检状态
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="remark"></param>
        /// <param name="status"></param>
        public void setProductInspection(string prodid, string remark, string status)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_Products set remarks=@remark,InspectionStaus=@status where ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "@ProductId", DbType.Int32, prodid);
            this.database.AddInParameter(sqlStringCommand, "@remark", DbType.String, remark);
            this.database.AddInParameter(sqlStringCommand, "@status", DbType.String, status);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
        /// <summary>
        /// 获取数据推送给广告商
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public DataTable GetAdOrderInfo(string skuId)
        {
              DataTable result = new DataTable();
              try
              {

                  string sql = @" SELECT b.skuId,a.ProductName,SalePrice,CategoryId
                            FROM Ecshop_products   AS a WITH(nolock)
                            INNER JOIN Ecshop_SKUs  AS b WITH(nolock)  ON a.ProductId=b.productId
                            WHERE b.skuId=@skuId";
                  DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
                  this.database.AddInParameter(sqlStringCommand, "@skuId", DbType.String, skuId);
                  using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                  {
                      result = DataHelper.ConverDataReaderToDataTable(dataReader);
                  }
                  return result;
              }
              catch (Exception ee)
              {
                  return result;
              }
        }
        /// <summary>
        /// 根据订单获取商品明细
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public DataTable GetAdOrderProductByOrderId(string OrderId)
        {
            DataTable result = new DataTable();
            try
            {
                string sql = @"SELECT b.skuId,a.ProductName,SalePrice ,CategoryId
                        FROM  Ecshop_OrderItems AS o
                        INNER join  Ecshop_products   AS a WITH(nolock) ON o.ProductId=a.ProductId
                        INNER JOIN Ecshop_SKUs  AS b WITH(nolock)  ON a.ProductId=b.productId
                        WHERE o.OrderId=@OrderId";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
                this.database.AddInParameter(sqlStringCommand, "@OrderId", DbType.String, OrderId);
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    result = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                return result;
            }
            catch (Exception ee)
            {
                return result;
            }
        
        }
    
    }
}

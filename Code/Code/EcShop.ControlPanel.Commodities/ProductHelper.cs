using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.HOP;
using EcShop.Entities.Orders;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.SqlDal.Active;
using EcShop.SqlDal.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
namespace EcShop.ControlPanel.Commodities
{
    public static class ProductHelper
    {
        public static ProductInfo GetProductDetails(int productId, out Dictionary<int, IList<int>> attrs, out IList<int> tagsId)
        {
            ProductDao productDao = new ProductDao();
            attrs = productDao.GetProductAttributes(productId);
            tagsId = productDao.GetProductTags(productId);
            return productDao.GetProductDetails(productId);
        }
         /// <summary>
        /// 根据订单获取商品明细
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public static DataTable GetAdOrderProductByOrderId(string OrderId)
        {
            ProductDao productDao = new ProductDao();
            return productDao.GetAdOrderProductByOrderId(OrderId);
        }
        public static ProductInfo GetProductDetails(int productId)
        {
            ProductDao productDao = new ProductDao();
            return productDao.GetProductDetails(productId);
        }
        public static DbQueryResult GetProducts(ProductQuery query, bool IsExport = true, bool special = true)
        {
            return new ProductDao().GetProducts(query, false, IsExport, special);
        }
        public static DbQueryResult GetProductsAdmin(ProductQuery query)
        {
            return new ProductDao().GetProducts(query, true);
        }
        /// <summary>
        /// 批量导入商品明细
        /// </summary>
        /// <param name="xmlProduct"></param>
        /// <returns></returns>
        public static DataTable ImportProductsList(string xmlProduct)
        {
            return new ProductDao().ImportProductsList(xmlProduct);
        }
        /// <summary>
        /// 归类获取商品信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetProductClassifyList(ProductQuery query)
        {
            return new ProductDao().GetProductClassifyList(query);
        }

        /// <summary>
        /// 商品归类
        /// </summary>
        /// <param name="productInfo"></param>
        /// <param name="ElmentsValue">申报要素</param>
        /// <returns></returns>
        public static DataSet ProductClassify(ProductInfo productInfo, string ElmentsValue, int UserId, string Username, string LJNo, string SkuId)
        {
            return new ProductDao().ProductClassify(productInfo, ElmentsValue, UserId, Username, LJNo, SkuId);
        }

        /// <summary>
        /// 商品备案
        /// </summary>
        /// <param name="ProductId">商品Id</param>
        /// <param name="ProductRegistrationNumber">备案编号</param>
        /// <returns></returns>
        public static bool Registration(string ProductRegistrationNumber, string SkuId,string status)
        {
            return new ProductDao().Registration(ProductRegistrationNumber, SkuId, status);
        }

        /// <summary>
        /// 添加商品备案批次
        /// </summary>
        /// <param name="ProductId">商品Id</param>
        /// <param name="ProductRegistrationNumber">批次号</param>
        /// <returns></returns>
        public static bool AddBatchNo(string BatchNo, string strProductIds)
        {
            return new ProductDao().AddBatchNo(BatchNo, strProductIds);
        }


        /// <summary>
        /// 判断是否存在未审价通过的商品
        /// </summary>
        /// <param name="strProductIds"></param>
        /// <returns></returns>
        public static bool IsExitNoApprovedPriceProduct(string strProductIds)
        {
            return new ProductDao().IsExitNoApprovedPriceProduct(strProductIds);
        }



        public static DbQueryResult GetProductSelect(ProductQuery query)
        {
            return new ProductDao().GetProductSelect(query);
        }

        public static System.Data.DataTable GetGroupBuyProducts(ProductQuery query)
        {
            return new ProductDao().GetGroupBuyProducts(query);
        }
        public static IList<ProductInfo> GetProducts(IList<int> productIds)
        {
            return new ProductDao().GetProducts(productIds);
        }
        /// <summary>
        /// 批量审核商品
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="flag">0表示弃审，1表示审核</param>
        /// <returns></returns>
        public static bool ApproveProducts(string productIds, int flag)
        {
            return new ProductDao().ApproveProducts(productIds, flag);
        }
        public static IList<int> GetProductIds(ProductQuery query)
        {
            return new ProductDao().GetProductIds(query);
        }
          /// <summary>
        /// 获取当前可购买的商品数量
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int CheckPurchaseCount(string SkuId, int userId, out int MaxCount)
        {
            return new ProductDao().CheckPurchaseCount(SkuId, userId, out MaxCount);
        }

        /// <summary>
        /// 获取当前可购买的商品数量
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="userId"></param>
        /// <param name="MaxCount">限制数量</param>
        /// <param name="LimiteDay">限制天数</param>
        /// <returns></returns>
        public static int CheckPurchaseCount(string SkuId, int userId, out int MaxCount,out int LimiteDay)
        {
            return new ProductDao().CheckPurchaseCount(SkuId, userId, out MaxCount, out LimiteDay);
        }


        public static ProductActionStatus AddProduct(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> tagsId,List<ProductsCombination> combinationInfo = null)
        {
            ProductActionStatus result;
            if (null == product)
            {
                result = ProductActionStatus.UnknowError;
            }
            else
            {
                Globals.EntityCoding(product, true);
                int decimalLength = HiContext.Current.SiteSettings.DecimalLength;
                if (product.MarketPrice.HasValue)
                {
                    product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
                }
                ProductActionStatus productActionStatus = ProductActionStatus.UnknowError;
                Database database = DatabaseFactory.CreateDatabase();
                using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
                {
                    dbConnection.Open();
                    System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                    try
                    {
                        ProductDao productDao = new ProductDao();
                        int num = productDao.AddProduct(product, dbTransaction);
                        if (num == 0)
                        {
                            dbTransaction.Rollback();
                            result = ProductActionStatus.DuplicateSKU;
                            return result;
                        }
                        product.ProductId = num;
                        if (skus != null && skus.Count > 0)
                        {
                            if (!productDao.AddProductSKUs(num, skus, dbTransaction,combinationInfo))
                            {
                                dbTransaction.Rollback();
                                result = ProductActionStatus.SKUError;
                                return result;
                            }
                        }
                        if (attrs != null && attrs.Count > 0)
                        {
                            if (!productDao.AddProductAttributes(num, attrs, dbTransaction))
                            {
                                dbTransaction.Rollback();
                                result = ProductActionStatus.AttributeError;
                                return result;
                            }
                        }
                        if (tagsId != null && tagsId.Count > 0)
                        {
                            if (!new TagDao().AddProductTags(num, tagsId, dbTransaction))
                            {
                                dbTransaction.Rollback();
                                result = ProductActionStatus.ProductTagEroor;
                                return result;
                            }
                        }
                        dbTransaction.Commit();
                        productActionStatus = ProductActionStatus.Success;
                    }
                    catch (Exception var_7_191)
                    {
                        dbTransaction.Rollback();
                    }
                    finally
                    {
                        dbConnection.Close();
                    }
                }
                if (productActionStatus == ProductActionStatus.Success)
                {
                    EventLogs.WriteOperationLog(Privilege.AddProducts, string.Format(CultureInfo.InvariantCulture, "上架了一个新商品:”{0}”", new object[]
					{
						product.ProductName
					}));
                }
                result = productActionStatus;
            }
            return result;
        }
        public static int GetMaxSequence()
        {
            return new ProductDao().GetMaxSequence();
        }
        public static ProductActionStatus UpdateProduct(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> tagIds,List<ProductsCombination> combinationInfo = null)
        {
            ProductActionStatus result;
            if (null == product)
            {
                result = ProductActionStatus.UnknowError;
            }
            else
            {
                Globals.EntityCoding(product, true);
                int decimalLength = HiContext.Current.SiteSettings.DecimalLength;
                if (product.MarketPrice.HasValue)
                {
                    product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
                }
                ProductActionStatus productActionStatus = ProductActionStatus.UnknowError;
                Database database = DatabaseFactory.CreateDatabase();
                using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
                {
                    dbConnection.Open();
                    System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                    try
                    {
                        ProductDao productDao = new ProductDao();
                        if (!productDao.UpdateProduct(product, dbTransaction))
                        {
                            dbTransaction.Rollback();
                            result = ProductActionStatus.DuplicateSKU;
                            return result;
                        }
                        if (!productDao.DeleteProductSKUS(product.ProductId, dbTransaction))
                        {
                            dbTransaction.Rollback();
                            result = ProductActionStatus.SKUError;
                            return result;
                        }
                        if (skus != null && skus.Count > 0)
                        {
                            if (!productDao.AddProductSKUs(product.ProductId, skus, dbTransaction, combinationInfo))
                            {
                                dbTransaction.Rollback();
                                result = ProductActionStatus.SKUError;
                                return result;
                            }
                        }
                        if (!productDao.AddProductAttributes(product.ProductId, attrs, dbTransaction))
                        {
                            dbTransaction.Rollback();
                            result = ProductActionStatus.AttributeError;
                            return result;
                        }
                        TagDao tagDao = new TagDao();
                        if (!tagDao.DeleteProductTags(product.ProductId, dbTransaction))
                        {
                            dbTransaction.Rollback();
                            result = ProductActionStatus.ProductTagEroor;
                            return result;
                        }
                        if (tagIds.Count > 0)
                        {
                            if (!tagDao.AddProductTags(product.ProductId, tagIds, dbTransaction))
                            {
                                dbTransaction.Rollback();
                                result = ProductActionStatus.ProductTagEroor;
                                return result;
                            }
                        }
                        dbTransaction.Commit();
                        productActionStatus = ProductActionStatus.Success;
                    }
                    catch (Exception var_7_1B7)
                    {
                        dbTransaction.Rollback();
                    }
                    finally
                    {
                        dbConnection.Close();
                    }
                }
                if (productActionStatus == ProductActionStatus.Success)
                {
                    EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的商品", new object[]
					{
						product.ProductId
					}));
                }
                result = productActionStatus;
            }
            return result;
        }
        public static bool UpdateProductCategory(int productId, int newCategoryId)
        {
            ProductDao productDao = new ProductDao();
            bool flag;
            if (newCategoryId != 0)
            {
                flag = productDao.UpdateProductCategory(productId, newCategoryId, CatalogHelper.GetCategory(newCategoryId).Path + "|");
            }
            else
            {
                flag = productDao.UpdateProductCategory(productId, newCategoryId, null);
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改编号 “{0}” 的店铺分类为 “{1}”", new object[]
				{
					productId,
					newCategoryId
				}));
            }
            return flag;
        }
        public static int DeleteProduct(string productIds, bool isDeleteImage)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProducts);
            int result;
            if (string.IsNullOrEmpty(productIds))
            {
                result = 0;
            }
            else
            {
                string[] array = productIds.Split(new char[]
				{
					','
				});
                IList<int> list = new List<int>();
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string s = array2[i];
                    list.Add(int.Parse(s));
                }
                ProductDao productDao = new ProductDao();
                IList<ProductInfo> products = productDao.GetProducts(list);
                int num = productDao.DeleteProduct(productIds);
                if (num > 0)
                {
                    EventLogs.WriteOperationLog(Privilege.DeleteProducts, string.Format(CultureInfo.InvariantCulture, "删除了 “{0}” 件商品", new object[]
					{
						list.Count
					}));
                    if (isDeleteImage)
                    {
                        foreach (ProductInfo current in products)
                        {
                            try
                            {
                                ProductHelper.DeleteProductImage(current);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                result = num;
            }
            return result;
        }
        public static int SetFreeShip(string productIds, bool isFree)
        {
            int result;
            if (string.IsNullOrEmpty(productIds))
            {
                result = 0;
            }
            else
            {
                int num = new ProductDao().UpdateProductShipFree(productIds, isFree);
                if (num > 0)
                {
                    EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "{0}了“{1}” 件商品包邮", new object[]
					{
						isFree ? "设置" : "取消",
						num
					}));
                }
                result = num;
            }
            return result;
        }
        public static int UpShelf(string productIds)
        {

            int result;
            if (string.IsNullOrEmpty(productIds))
            {
                result = 0;
            }
            else
            {
                int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.OnSale);
                if (num > 0)
                {
                    EventLogs.WriteOperationLog(Privilege.UpShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量上架了 “{0}” 件商品,商品Id为 “{1}” ", new object[]
					{
						num,
                        productIds
					}));
                }
                result = num;
            }
            return result;
        }
        public static int OffShelf(string productIds)
        {

            int result;
            if (string.IsNullOrEmpty(productIds))
            {
                result = 0;
            }
            else
            {
                int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.UnSale);
                if (num > 0)
                {
                    EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量下架了 “{0}” 件商品", new object[]
					{
						num
					}));
                }
                result = num;
            }
            return result;
        }
        public static int InStock(string productIds)
        {
            int result;
            if (string.IsNullOrEmpty(productIds))
            {
                result = 0;
            }
            else
            {
                int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.OnStock);
                if (num > 0)
                {
                    EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量入库了 “{0}” 件商品", new object[]
					{
						num
					}));
                }
                result = num;
            }
            return result;
        }
        public static int RemoveProduct(string productIds)
        {
            int result;
            if (string.IsNullOrEmpty(productIds))
            {
                result = 0;
            }
            else
            {
                int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.Delete);
                if (num > 0)
                {
                    EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量删除了 “{0}” 件商品“{1}”到回收站", new object[]
					{
						num,
                        productIds
					}));
                }
                result = num;
            }
            return result;
        }
        public static System.Data.DataTable GetProductBaseInfo(string productIds)
        {
            return new ProductBatchDao().GetProductBaseInfo(productIds);
        }
        public static bool UpdateProductNames(string productIds, string prefix, string suffix)
        {
            bool flag = new ProductBatchDao().UpdateProductNames(productIds, prefix, suffix);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "为商品 “{0}” 名称添加前后缀成功", new object[]
				{
					productIds
				}));
            }
            return flag;
        }
        public static bool ReplaceProductNames(string productIds, string oldWord, string newWord)
        {
            bool flag = new ProductBatchDao().ReplaceProductNames(productIds, oldWord, newWord);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "为商品 “{0}” 名称替换字符串缀成功", new object[]
				{
					productIds
				}));
            }
            return flag;
        }
        public static bool UpdateProductBaseInfo(System.Data.DataTable dt)
        {
            bool flag = false;
            string productName = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    if (dataRow["ProductName"] != null && !string.IsNullOrEmpty(dataRow["ProductName"].ToString()))
                    {
                        productName += dataRow["ProductName"] + ",";
                    }
                }
                flag = new ProductBatchDao().UpdateProductBaseInfo(dt);
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "批量修改商品 “{0}” 信息成功", new object[]
				{
					productName
				}));
            }
            return flag;
        }
        public static bool UpdateProductReferralDeduct(string productIds, decimal referralDeduct, decimal subMemberDeduct, decimal subReferralDeduct)
        {
            bool flag = new ProductBatchDao().UpdateProductReferralDeduct(productIds, referralDeduct, subMemberDeduct, subReferralDeduct);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "成功更新了商品 “{0}” 的推广佣金", new object[]
				{
					productIds
				}));
            }
            return flag;
        }
        public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
        {
            bool flag = new ProductBatchDao().UpdateShowSaleCounts(productIds, showSaleCounts);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "成功调整了前台商品 “{0}” 显示的销售数量", new object[]
				{
					productIds
				}));
            }
            return flag;
        }
        public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
        {
            bool flag = new ProductBatchDao().UpdateShowSaleCounts(productIds, showSaleCounts, operation);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "成功调整了前台商品 “{0}” 显示的销售数量", new object[]
				{
					productIds
				}));
            }
            return flag;
        }
        public static bool UpdateShowSaleCounts(System.Data.DataTable dt)
        {
            bool flag = false;
            string productId = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                flag = new ProductBatchDao().UpdateShowSaleCounts(dt);

                foreach (DataRow dataRow in dt.Rows)
                {
                    if (dataRow["ProductId"] != null && !string.IsNullOrEmpty(dataRow["ProductId"].ToString()))
                    {
                        productId += dataRow["ProductId"] + ",";
                    }
                }
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "成功调整了前台商品 “{0}” 显示的销售数量", new object[]
				{
					productId
				}));
            }
            return flag;
        }
        /// <summary>
        /// 获取商品权重信息
        /// </summary>
        /// <param name="productIds">商品权重集合</param>
        /// <returns></returns>
        public static System.Data.DataTable GetProductsFractionChange(string productIds)
        {
            return new ProductBatchDao().GetProductsFractionChange(productIds);
        }
        public static System.Data.DataTable GetSkuStocks(string productIds)
        {
            return new ProductBatchDao().GetSkuStocks(productIds);
        }
        public static bool UpdateSkuStock(string productIds, int stock)
        {
            bool flag = new ProductBatchDao().UpdateSkuStock(productIds, stock);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改商品 “{0}” 的库存成功", new object[]
				{
					productIds
				}));
            }
            return flag;
        }
        /// <summary>
        /// 修改商品权重信息
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="addStock"></param>
        /// <returns></returns>
        public static bool UpdataProductsAdminFraction(string productIds, decimal addStock)
        {
            bool flag = new ProductBatchDao().UpdataProductsAdminFraction(productIds, addStock);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改商品 “{0}” 的权重成功", new object[]
				{
					productIds
				}));
            }
            return flag;
        }
        public static bool AddProductsAdminFraction(string productIds, decimal addStock)
        {
            bool flag = new ProductBatchDao().AddProductsAdminFraction(productIds, addStock);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改商品 “{0}” 的权重成功", new object[]
				{
					productIds
				}));
            }
            return flag;
        }
        public static bool AddSkuStock(string productIds, int addStock)
        {
            bool flag = new ProductBatchDao().AddSkuStock(productIds, addStock);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改商品 “{0}” 的库存成功", new object[]
				{
					productIds
				}));
            }
            return flag;
        }
        public static bool UpdateProductFraction(Dictionary<string, decimal> fractions)
        {
            bool flag = new ProductBatchDao().UpdateFraction(fractions);
            if (flag)
            {
                string productIds = string.Empty;
                foreach (string current in fractions.Keys)
                {
                    if (!string.IsNullOrEmpty(current))
                    {
                        productIds += current + ",";
                    }
                }

                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "批量修改商品为 “{0}” 的权重成功", new object[]
				{
					productIds
				}));
            }
            return flag;
        }
        public static bool UpdateSkuStock(Dictionary<string, int> skuStocks)
        {
            bool flag = new ProductBatchDao().UpdateSkuStock(skuStocks);
            if (flag)
            {
                string skuIds = string.Empty;
                foreach (string current in skuStocks.Keys)
                {
                    if (!string.IsNullOrEmpty(current))
                    {
                        skuIds += current + ",";
                    }
                }

                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "批量修改SKUId为 “{0}” 的库存成功", new object[]
				{
					skuIds
				}));
            }
            return flag;
        }
        public static System.Data.DataTable GetSkuMemberPrices(string productIds)
        {
            return new ProductBatchDao().GetSkuMemberPrices(productIds);
        }
        public static bool CheckPrice(string productIds, int baseGradeId, decimal checkPrice, bool isMember)
        {
            return new ProductBatchDao().CheckPrice(productIds, baseGradeId, checkPrice, isMember);
        }
        public static bool UpdateSkuMemberPrices(string productIds, int gradeId, decimal price)
        {
            bool flag = new ProductBatchDao().UpdateSkuMemberPrices(productIds, gradeId, price);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改商品 “{0}” 的价格成功", new object[]
				{
					productIds
				}));
            }
            return flag;
        }
        public static bool UpdateSkuMemberPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
        {
            bool flag = new ProductBatchDao().UpdateSkuMemberPrices(productIds, gradeId, baseGradeId, operation, price);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改商品 “{0}” 的价格成功", new object[]
				{
					productIds
				}));
            }
            return flag;
        }
        public static bool UpdateSkuMemberPrices(System.Data.DataSet ds)
        {
            bool flag = new ProductBatchDao().UpdateSkuMemberPrices(ds);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改商品的价格成功"));
            }
            return flag;
        }
        public static DbQueryResult GetRelatedProducts(ProductQuery query)
        {
            return new ProductDao().GetRelatedProducts(query);
        }
        public static DbQueryResult GetShareProducts(ProductQuery query, bool share, string productIds)
        {
            return new ProductDao().GetShareProducts(query, share, productIds);
        }
        public static int AddShareLine(ShareProductInfo shareinfo, string productIds)
        {
            int num = new ProductDao().AddShareLine(shareinfo);
            if (num > 0)
            {
                ProductHelper.AddShareProducts(num, productIds);
            }
            return num;
        }
        public static void AddShareProducts(int shareId, string productIds)
        {
            string[] array;
            if (productIds.Contains(","))
            {
                array = productIds.Split(new char[]
				{
					','
				});
            }
            else
            {
                array = new string[]
				{
					productIds
				};
            }
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string value = array2[i];
                new ProductDao().AddShareProducts(shareId, Convert.ToInt32(value));
            }
        }
        public static bool DeleteShareProducts(int shareId, string productIds)
        {
            return new ProductDao().DeleteShareProducts(shareId, productIds);
        }
        public static DbQueryResult GetShareProducts(ProductQuery query, bool share, int shareId)
        {
            return new ProductDao().GetShareProducts(query, share, shareId);
        }
        public static System.Data.DataSet GetShareProcuts(int pagesize, int pageindex)
        {
            return new ProductDao().GetAllShareProducts(pagesize, pageindex);
        }
        public static bool DeleteShareLine(int shareId)
        {
            ShareProductInfo shareProductInfoById = ProductHelper.GetShareProductInfoById(shareId);
            ProductHelper.DeleteShareImage(shareProductInfoById);
            EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "删除了商品分享页'{0}'的记录", new object[]
			{
				shareProductInfoById.ShareTitle
			}));
            return new ProductDao().DeleteShareLine(shareId);
        }
        public static bool UpdateShareLine(ShareProductInfo shareinfo)
        {
            return new ProductDao().UpdateShareLine(shareinfo);
        }
        public static bool DeleteShareLine(string shareIds)
        {
            string[] array;
            if (shareIds.Contains(","))
            {
                array = shareIds.Split(new char[]
				{
					','
				});
            }
            else
            {
                array = new string[]
				{
					shareIds
				};
            }
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string value = array2[i];
                new ProductDao().DeleteShareLine(Convert.ToInt32(value));
                ProductHelper.DeleteShareImage(ProductHelper.GetShareProductInfoById(Convert.ToInt32(value)));
            }
            EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "批量删除了{0}条商品分享页记录", new object[]
			{
				array.Length
			}));
            return true;
        }
        private static void DeleteShareImage(ShareProductInfo shareinfo)
        {
            if (shareinfo != null)
            {
                if (!string.IsNullOrEmpty(shareinfo.ShareUrl))
                {
                    ResourcesHelper.DeleteImage(shareinfo.ShareUrl);
                }
            }
        }
        public static ShareProductInfo GetShareProductInfoById(int shareId)
        {
            return new ProductDao().SelectShareProductInfo(shareId);
        }
        public static bool AddRelatedProduct(int productId, int relatedProductId)
        {
            return new ProductDao().AddRelatedProduct(productId, relatedProductId);
        }
        public static bool RemoveRelatedProduct(int productId, int relatedProductId)
        {
            return new ProductDao().RemoveRelatedProduct(productId, relatedProductId);
        }
        public static bool ClearRelatedProducts(int productId)
        {
            return new ProductDao().ClearRelatedProducts(productId);
        }
        public static System.Data.DataSet GetTaobaoProductDetails(int productId)
        {
            return new TaobaoProductDao().GetTaobaoProductDetails(productId);
        }
        public static bool UpdateToaobProduct(TaobaoProductInfo taobaoProduct)
        {
            return new TaobaoProductDao().UpdateToaobProduct(taobaoProduct);
        }
        public static bool IsExitTaobaoProduct(long taobaoProductId)
        {
            return new TaobaoProductDao().IsExitTaobaoProduct(taobaoProductId);
        }
        public static string UploadDefaltProductImage(HttpPostedFile postedFile)
        {
            string result;
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                result = string.Empty;
            }
            else
            {
                string text = HiContext.Current.GetStoragePath() + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
                postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
                result = text;
            }
            return result;
        }
        private static void DeleteProductImage(ProductInfo product)
        {
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl1))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl1);
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
                if (!string.IsNullOrEmpty(product.ImageUrl2))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl2);
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
                if (!string.IsNullOrEmpty(product.ImageUrl3))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl3);
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
                if (!string.IsNullOrEmpty(product.ImageUrl4))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl4);
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
                if (!string.IsNullOrEmpty(product.ImageUrl5))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl5);
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
            }
        }
        public static DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
        {
            return new ProductDao().GetExportProducts(query, removeProductIds);
        }
        public static System.Data.DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
        {
            System.Data.DataSet exportProducts = new ProductDao().GetExportProducts(query, includeCostPrice, includeStock, removeProductIds);
            exportProducts.Tables[0].TableName = "types";
            exportProducts.Tables[1].TableName = "attributes";
            exportProducts.Tables[2].TableName = "values";
            exportProducts.Tables[3].TableName = "products";
            exportProducts.Tables[4].TableName = "skus";
            exportProducts.Tables[5].TableName = "skuItems";
            exportProducts.Tables[6].TableName = "productAttributes";
            exportProducts.Tables[7].TableName = "taobaosku";
            return exportProducts;
        }
        public static void EnsureMapping(System.Data.DataSet mappingSet)
        {
            new ProductDao().EnsureMapping(mappingSet);
        }
        public static void ImportProducts(System.Data.DataTable productData, int categoryId, int? brandId, ProductSaleStatus saleStatus, bool isImportFromTaobao)
        {
            if (productData != null && productData.Rows.Count > 0)
            {
                foreach (System.Data.DataRow dataRow in productData.Rows)
                {
                    ProductInfo productInfo = new ProductInfo();
                    productInfo.CategoryId = categoryId;
                    productInfo.MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|";
                    productInfo.ProductName = (string)dataRow["ProductName"];
                    productInfo.ProductCode = (string)dataRow["SKU"];
                    productInfo.BrandId = brandId;
                    if (dataRow["Description"] != DBNull.Value)
                    {
                        productInfo.Description = (string)dataRow["Description"];
                    }
                    productInfo.AddedDate = DateTime.Now;
                    productInfo.SaleStatus = saleStatus;
                    productInfo.HasSKU = false;
                    HttpContext current = HttpContext.Current;
                    if (dataRow["ImageUrl1"] != DBNull.Value)
                    {
                        productInfo.ImageUrl1 = (string)dataRow["ImageUrl1"];
                    }
                    if (!string.IsNullOrEmpty(productInfo.ImageUrl1) && productInfo.ImageUrl1.Length > 0)
                    {
                        string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl1);
                        productInfo.ThumbnailUrl40 = array[0];
                        productInfo.ThumbnailUrl60 = array[1];
                        productInfo.ThumbnailUrl100 = array[2];
                        productInfo.ThumbnailUrl160 = array[3];
                        productInfo.ThumbnailUrl180 = array[4];
                        productInfo.ThumbnailUrl220 = array[5];
                        productInfo.ThumbnailUrl310 = array[6];
                        productInfo.ThumbnailUrl410 = array[7];
                    }
                    if (dataRow["ImageUrl2"] != DBNull.Value)
                    {
                        productInfo.ImageUrl2 = (string)dataRow["ImageUrl2"];
                    }
                    if (!string.IsNullOrEmpty(productInfo.ImageUrl2) && productInfo.ImageUrl2.Length > 0)
                    {
                        string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl2);
                    }
                    if (dataRow["ImageUrl3"] != DBNull.Value)
                    {
                        productInfo.ImageUrl3 = (string)dataRow["ImageUrl3"];
                    }
                    if (!string.IsNullOrEmpty(productInfo.ImageUrl3) && productInfo.ImageUrl3.Length > 0)
                    {
                        string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl3);
                    }
                    if (dataRow["ImageUrl4"] != DBNull.Value)
                    {
                        productInfo.ImageUrl4 = (string)dataRow["ImageUrl4"];
                    }
                    if (!string.IsNullOrEmpty(productInfo.ImageUrl4) && productInfo.ImageUrl4.Length > 0)
                    {
                        string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl4);
                    }
                    if (dataRow["ImageUrl5"] != DBNull.Value)
                    {
                        productInfo.ImageUrl5 = (string)dataRow["ImageUrl5"];
                    }
                    if (!string.IsNullOrEmpty(productInfo.ImageUrl5) && productInfo.ImageUrl5.Length > 0)
                    {
                        string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl5);
                    }
                    SKUItem sKUItem = new SKUItem();
                    sKUItem.SkuId = "0";
                    sKUItem.SKU = (string)dataRow["SKU"];
                    sKUItem.SalePrice = (decimal)dataRow["SalePrice"];
                    if (dataRow["Stock"] != DBNull.Value)
                    {
                        sKUItem.Stock = (int)dataRow["Stock"];
                    }
                    if (dataRow["Weight"] != DBNull.Value)
                    {
                        sKUItem.Weight = (decimal)dataRow["Weight"];
                    }
                    ProductActionStatus productActionStatus = ProductHelper.AddProduct(productInfo, new Dictionary<string, SKUItem>
					{

						{
							sKUItem.SkuId,
							sKUItem
						}
					}, null, null);
                    if (isImportFromTaobao && productActionStatus == ProductActionStatus.Success)
                    {
                        TaobaoProductInfo taobaoProductInfo = new TaobaoProductInfo();
                        taobaoProductInfo.ProductId = productInfo.ProductId;
                        taobaoProductInfo.ProTitle = productInfo.ProductName;
                        taobaoProductInfo.Cid = (long)dataRow["Cid"];
                        if (dataRow["StuffStatus"] != DBNull.Value)
                        {
                            taobaoProductInfo.StuffStatus = (string)dataRow["StuffStatus"];
                        }
                        taobaoProductInfo.Num = (long)dataRow["Num"];
                        taobaoProductInfo.LocationState = (string)dataRow["LocationState"];
                        taobaoProductInfo.LocationCity = (string)dataRow["LocationCity"];
                        taobaoProductInfo.FreightPayer = (string)dataRow["FreightPayer"];
                        if (dataRow["PostFee"] != DBNull.Value)
                        {
                            taobaoProductInfo.PostFee = (decimal)dataRow["PostFee"];
                        }
                        if (dataRow["ExpressFee"] != DBNull.Value)
                        {
                            taobaoProductInfo.ExpressFee = (decimal)dataRow["ExpressFee"];
                        }
                        if (dataRow["EMSFee"] != DBNull.Value)
                        {
                            taobaoProductInfo.EMSFee = (decimal)dataRow["EMSFee"];
                        }
                        taobaoProductInfo.HasInvoice = (bool)dataRow["HasInvoice"];
                        taobaoProductInfo.HasWarranty = (bool)dataRow["HasWarranty"];
                        taobaoProductInfo.HasDiscount = (bool)dataRow["HasDiscount"];
                        taobaoProductInfo.ValidThru = (long)dataRow["ValidThru"];
                        if (dataRow["ListTime"] != DBNull.Value)
                        {
                            taobaoProductInfo.ListTime = (DateTime)dataRow["ListTime"];
                        }
                        else
                        {
                            taobaoProductInfo.ListTime = DateTime.Now;
                        }
                        if (dataRow["PropertyAlias"] != DBNull.Value)
                        {
                            taobaoProductInfo.PropertyAlias = (string)dataRow["PropertyAlias"];
                        }
                        if (dataRow["InputPids"] != DBNull.Value)
                        {
                            taobaoProductInfo.InputPids = (string)dataRow["InputPids"];
                        }
                        if (dataRow["InputStr"] != DBNull.Value)
                        {
                            taobaoProductInfo.InputStr = (string)dataRow["InputStr"];
                        }
                        if (dataRow["SkuProperties"] != DBNull.Value)
                        {
                            taobaoProductInfo.SkuProperties = (string)dataRow["SkuProperties"];
                        }
                        if (dataRow["SkuQuantities"] != DBNull.Value)
                        {
                            taobaoProductInfo.SkuQuantities = (string)dataRow["SkuQuantities"];
                        }
                        if (dataRow["SkuPrices"] != DBNull.Value)
                        {
                            taobaoProductInfo.SkuPrices = (string)dataRow["SkuPrices"];
                        }
                        if (dataRow["SkuOuterIds"] != DBNull.Value)
                        {
                            taobaoProductInfo.SkuOuterIds = (string)dataRow["SkuOuterIds"];
                        }
                        ProductHelper.UpdateToaobProduct(taobaoProductInfo);
                    }
                }
            }
        }
        public static void ImportProducts(System.Data.DataSet productData, int categoryId, int? bandId, ProductSaleStatus saleStatus, bool includeCostPrice, bool includeStock, bool includeImages)
        {
            foreach (System.Data.DataRow dataRow in productData.Tables["products"].Rows)
            {
                int mappedProductId = (int)dataRow["ProductId"];
                ProductInfo product = ProductHelper.ConverToProduct(dataRow, categoryId, bandId, saleStatus, includeImages);
                Dictionary<string, SKUItem> skus = ProductHelper.ConverToSkus(mappedProductId, productData, includeCostPrice, includeStock);
                Dictionary<int, IList<int>> attrs = ProductHelper.ConvertToAttributes(mappedProductId, productData);
                ProductActionStatus productActionStatus = ProductHelper.AddProduct(product, skus, attrs, null);
            }
        }
        private static Dictionary<int, IList<int>> ConvertToAttributes(int mappedProductId, System.Data.DataSet productData)
        {
            System.Data.DataRow[] array = productData.Tables["attributes"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
            Dictionary<int, IList<int>> result;
            if (array.Length == 0)
            {
                result = null;
            }
            else
            {
                Dictionary<int, IList<int>> dictionary = new Dictionary<int, IList<int>>();
                System.Data.DataRow[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    System.Data.DataRow dataRow = array2[i];
                    int key = (int)dataRow["SelectedAttributeId"];
                    if (!dictionary.ContainsKey(key))
                    {
                        IList<int> value = new List<int>();
                        dictionary.Add(key, value);
                    }
                    dictionary[key].Add((int)dataRow["SelectedValueId"]);
                }
                result = dictionary;
            }
            return result;
        }
        private static Dictionary<string, SKUItem> ConverToSkus(int mappedProductId, System.Data.DataSet productData, bool includeCostPrice, bool includeStock)
        {
            System.Data.DataRow[] array = productData.Tables["skus"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
            Dictionary<string, SKUItem> result;
            if (array.Length == 0)
            {
                result = null;
            }
            else
            {
                Dictionary<string, SKUItem> dictionary = new Dictionary<string, SKUItem>();
                System.Data.DataRow[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    System.Data.DataRow dataRow = array2[i];
                    string text = (string)dataRow["NewSkuId"];
                    SKUItem sKUItem = new SKUItem
                    {
                        SkuId = text,
                        SKU = (string)dataRow["SKU"],
                        SalePrice = (decimal)dataRow["SalePrice"]
                    };
                    if (dataRow["Weight"] != DBNull.Value)
                    {
                        sKUItem.Weight = (decimal)dataRow["Weight"];
                    }
                    if (includeCostPrice && dataRow["CostPrice"] != DBNull.Value)
                    {
                        sKUItem.CostPrice = (decimal)dataRow["CostPrice"];
                    }
                    if (includeStock)
                    {
                        sKUItem.Stock = (int)dataRow["Stock"];
                    }
                    System.Data.DataRow[] array3 = productData.Tables["skuItems"].Select("NewSkuId='" + text + "' AND MappedProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
                    System.Data.DataRow[] array4 = array3;
                    for (int j = 0; j < array4.Length; j++)
                    {
                        System.Data.DataRow dataRow2 = array4[j];
                        sKUItem.SkuItems.Add((int)dataRow2["SelectedAttributeId"], (int)dataRow2["SelectedValueId"]);
                    }
                    dictionary.Add(text, sKUItem);
                }
                result = dictionary;
            }
            return result;
        }
        private static ProductInfo ConverToProduct(System.Data.DataRow productRow, int categoryId, int? bandId, ProductSaleStatus saleStatus, bool includeImages)
        {
            ProductInfo productInfo = new ProductInfo
            {
                CategoryId = categoryId,
                TypeId = new int?((int)productRow["SelectedTypeId"]),
                ProductName = (string)productRow["ProductName"],
                ProductCode = (string)productRow["ProductCode"],
                BrandId = bandId,
                Unit = (string)productRow["Unit"],
                ShortDescription = (string)productRow["ShortDescription"],
                Description = (string)productRow["Description"],
                Title = (string)productRow["Title"],
                MetaDescription = (string)productRow["Meta_Description"],
                MetaKeywords = (string)productRow["Meta_Keywords"],
                AddedDate = DateTime.Now,
                SaleStatus = saleStatus,
                HasSKU = (bool)productRow["HasSKU"],
                MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|",
                ImageUrl1 = (string)productRow["ImageUrl1"],
                ImageUrl2 = (string)productRow["ImageUrl2"],
                ImageUrl3 = (string)productRow["ImageUrl3"],
                ImageUrl4 = (string)productRow["ImageUrl4"],
                ImageUrl5 = (string)productRow["ImageUrl5"]
            };
            if (productRow["MarketPrice"] != DBNull.Value)
            {
                productInfo.MarketPrice = new decimal?((decimal)productRow["MarketPrice"]);
            }
            if (includeImages)
            {
                HttpContext current = HttpContext.Current;
                if (!string.IsNullOrEmpty(productInfo.ImageUrl1) && productInfo.ImageUrl1.Length > 0)
                {
                    string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl1);
                    productInfo.ThumbnailUrl40 = array[0];
                    productInfo.ThumbnailUrl60 = array[1];
                    productInfo.ThumbnailUrl100 = array[2];
                    productInfo.ThumbnailUrl160 = array[3];
                    productInfo.ThumbnailUrl180 = array[4];
                    productInfo.ThumbnailUrl220 = array[5];
                    productInfo.ThumbnailUrl310 = array[6];
                    productInfo.ThumbnailUrl410 = array[7];
                }
                if (!string.IsNullOrEmpty(productInfo.ImageUrl2) && productInfo.ImageUrl2.Length > 0)
                {
                    string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl2);
                }
                if (!string.IsNullOrEmpty(productInfo.ImageUrl3) && productInfo.ImageUrl3.Length > 0)
                {
                    string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl3);
                }
                if (!string.IsNullOrEmpty(productInfo.ImageUrl4) && productInfo.ImageUrl4.Length > 0)
                {
                    string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl4);
                }
                if (!string.IsNullOrEmpty(productInfo.ImageUrl5) && productInfo.ImageUrl5.Length > 0)
                {
                    string[] array = ProductHelper.ProcessImages(current, productInfo.ImageUrl5);
                }
            }
            return productInfo;
        }
        private static string[] ProcessImages(HttpContext context, string originalSavePath)
        {
            string fileName = Path.GetFileName(originalSavePath);
            string text = "/Storage/master/product/thumbs40/40_" + fileName;
            string text2 = "/Storage/master/product/thumbs60/60_" + fileName;
            string text3 = "/Storage/master/product/thumbs100/100_" + fileName;
            string text4 = "/Storage/master/product/thumbs160/160_" + fileName;
            string text5 = "/Storage/master/product/thumbs180/180_" + fileName;
            string text6 = "/Storage/master/product/thumbs220/220_" + fileName;
            string text7 = "/Storage/master/product/thumbs310/310_" + fileName;
            string text8 = "/Storage/master/product/thumbs410/410_" + fileName;
            string text9 = context.Request.MapPath(Globals.ApplicationPath + originalSavePath);
            if (File.Exists(text9))
            {
                try
                {
                    ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text), 40, 40);
                    ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text2), 60, 60);
                    ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text3), 100, 100);
                    ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text4), 160, 160);
                    ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text5), 180, 180);
                    ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text6), 220, 220);
                    ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text7), 310, 310);
                    ResourcesHelper.CreateThumbnail(text9, context.Request.MapPath(Globals.ApplicationPath + text8), 410, 410);
                }
                catch
                {
                }
            }
            return new string[]
			{
				text,
				text2,
				text3,
				text4,
				text5,
				text6,
				text7,
				text8
			};
        }
        public static System.Data.DataSet GetProductsByQuery(ProductQuery query, int gradeId, out int totalrecord)
        {
            return new ProductApiDao().GetProductsByQuery(query, gradeId, out totalrecord);
        }
        public static System.Data.DataSet GetProductSkuDetials(int productId, int gradeId)
        {
            return new ProductApiDao().GetProductSkuDetials(productId, gradeId);
        }
        public static ApiErrorCode UpdateProductStock(int productId, string skuId, string sku, int type, int stock)
        {
            ApiErrorCode result;
            if (productId <= 0 || (type == 1 && stock <= 0))
            {
                result = ApiErrorCode.Format_Eroor;
            }
            else
            {
                skuId = DataHelper.CleanSearchString(skuId);
                sku = DataHelper.CleanSearchString(sku);
                ProductApiDao productApiDao = new ProductApiDao();
                System.Data.DataTable skuStocks = productApiDao.GetSkuStocks(productId.ToString());
                string key = "";
                bool flag = false;
                if (skuStocks.Rows.Count <= 0)
                {
                    result = ApiErrorCode.Exists_Error;
                }
                else
                {
                    int num = Convert.ToInt32(skuStocks.Rows[0]["Stock"]);
                    if (!string.IsNullOrEmpty(skuId))
                    {
                        System.Data.DataRow[] array = skuStocks.Select("SkuId='" + skuId + "'");
                        if (array.Length <= 0)
                        {
                            result = ApiErrorCode.Exists_Error;
                            return result;
                        }
                        num = Convert.ToInt32(array[0]["Stock"]);
                        key = skuId;
                        flag = true;
                    }
                    if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(skuId))
                    {
                        System.Data.DataRow[] array = skuStocks.Select("SKU='" + sku + "'");
                        if (array.Length <= 0)
                        {
                            result = ApiErrorCode.Exists_Error;
                            return result;
                        }
                        num = Convert.ToInt32(array[0]["Stock"]);
                        key = array[0]["SkuId"].ToString();
                        flag = true;
                    }
                    if (type != 1)
                    {
                        if (num + stock <= 0)
                        {
                            stock = 0;
                        }
                        else
                        {
                            stock += num;
                        }
                    }
                    bool flag2;
                    if (!flag)
                    {
                        flag2 = productApiDao.UpdateSkuStock(productId.ToString(), stock);
                    }
                    else
                    {
                        flag2 = productApiDao.UpdateSkuStock(new Dictionary<string, int>
						{

							{
								key,
								stock
							}
						});
                    }
                    if (flag2)
                    {
                        result = ApiErrorCode.Success;
                    }
                    else
                    {
                        result = ApiErrorCode.Unknown_Error;
                    }
                }
            }
            return result;
        }
        public static int OffShelfAPI(string productIds)
        {
            int result;
            if (string.IsNullOrEmpty(productIds))
            {
                result = 0;
            }
            else
            {
                int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.UnSale);
                if (num > 0)
                {
                    EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量下架了 “{0}” 件商品", new object[]
					{
						num
					}));
                }
                result = num;
            }
            return result;
        }
        public static int UpShelfAPI(string productIds)
        {
            int result;
            if (string.IsNullOrEmpty(productIds))
            {
                result = 0;
            }
            else
            {
                int num = new ProductDao().UpdateProductSaleStatus(productIds, ProductSaleStatus.OnSale);
                if (num > 0)
                {
                    EventLogs.WriteOperationLog(Privilege.UpShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量上架了 “{0}” 件商品,商品Id{1}", new object[]
					{
						num,
                        productIds
					}));
                }
                result = num;
            }
            return result;
        }
        public static bool AddProductTags(int productId, IList<int> tagsId, System.Data.Common.DbTransaction dbtran)
        {
            bool flag = new TagDao().AddProductTags(productId, tagsId, dbtran);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.SubjectProducts, string.Format(CultureInfo.InvariantCulture, "新增一个商品Id为{0}的商品标签", new object[]
				{
                    productId
				}));
            }
            return flag;
        }
        public static bool DeleteProductTags(int productId, System.Data.Common.DbTransaction tran)
        {
            bool flag = new TagDao().DeleteProductTags(productId, tran);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.SubjectProducts, string.Format(CultureInfo.InvariantCulture, "删除一个商品Id为{0}的商品标签", new object[]
				{
                    productId
				}));
            }
            return flag;
        }
        public static DbQueryResult GetToTaobaoProducts(ProductQuery query)
        {
            return new TaobaoProductDao().GetToTaobaoProducts(query);
        }
        public static PublishToTaobaoProductInfo GetTaobaoProduct(int productId)
        {
            return new TaobaoProductDao().GetTaobaoProduct(productId);
        }
        public static bool UpdateTaobaoProductId(int productId, long taobaoProductId)
        {
            return new TaobaoProductDao().UpdateTaobaoProductId(productId, taobaoProductId);
        }
        public static System.Data.DataTable GetSkusByProductId(int productId)
        {
            return new SkuDao().GetSkusByProductId(productId);
        }

        public static System.Data.DataTable GetExtendSkusByProductId(int productId)
        {
            return new SkuDao().GetExtendSkusByProductId(productId);
        }


        /// <summary>
        /// 修改商品二维码
        /// </summary>
        /// <returns></returns>
        public static bool UpdateQRcode(Dictionary<string, string> dic)
        {
            return new ProductDao().UpdateQRcode(dic);
        }

        public static Dictionary<string, decimal> CalculateProductFraction(string productIds)
        {
            return new ProductBatchDao().CalculateProductFraction(productIds);
        }

        public static bool UpdateProductFractions(Dictionary<string, decimal> productFractions)
        {
            return new ProductBatchDao().UpdateProductFraction(productFractions);
        }

        public static DbQueryResult GetProducts(int saleStatus, int pageIndex, int pageSize)
        {
            return new ProductDao().GetProducts(saleStatus, pageIndex, pageSize);
        }

        public static DataTable GeSuggestProducts(ClientType client)
        {
            return new ProductDao().GeSuggestProducts(client);
        }

        public static bool RemoveSuggestProduct(int productId, ClientType client)
        {
            return new ProductDao().RemoveSuggestProduct(productId, client);
        }

        public static bool RemoveAllSuggestProduct(ClientType client)
        {
            return new ProductDao().RemoveAllSuggestProduct(client);
        }

        public static bool UpdateSuggestProductSequence(ClientType client, int ProductId, int displaysequence)
        {
            return new ProductDao().UpdateSuggestProductSequence(client, ProductId, displaysequence);
        }

        public static bool AddSuggestProdcut(int productId, ClientType client)
        {
            return new ProductDao().AddSuggestProdcut(productId, client);
        }

        /// <summary>
        /// 获取商品购买基数>1商品ID与购买基数。
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public static Dictionary<int, int> GetBuyCardinality(params int[] productIds)
        {
            return new ProductDao().GetBuyCardinality(productIds);
        }

        public static DataSet GetSupplierProductDetail(ProductQuery productQuery)
        {
            return new ProductDao().GetSupplierProductDetail(productQuery);
        }


        public static bool UpdateProductTaxRate(System.Data.DataTable dt)
        {
            bool flag = false;
            string taxRateId = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    if (dataRow["TaxRateId"] != null && !string.IsNullOrEmpty(dataRow["TaxRateId"].ToString()))
                    {
                        taxRateId += dataRow["TaxRateId"] + ",";
                    }
                }
                flag = new ProductBatchDao().UpdateProductTaxRate(dt);
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "批量修改商品的税率 “{0}” 的成功", new object[]
				{
					taxRateId
				}));
            }
            return flag;
        }


        public static bool UpdateProductImportSource(System.Data.DataTable dt)
        {
            bool flag = false;
            string importSourceId = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    if (dataRow["ImportSourceId"] != null && !string.IsNullOrEmpty(dataRow["ImportSourceId"].ToString()))
                    {
                        importSourceId += dataRow["ImportSourceId"] + ",";
                    }
                }
                flag = new ProductBatchDao().UpdateProductImportSource(dt);
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "批量修改商品的原产地 “{0}” 的成功", new object[]
				{
					importSourceId
				}));
            }
            return flag;
        }

        public static bool UpdateProductBrand(System.Data.DataTable dt)
        {
            bool flag = false;
            string brandId = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    if (dataRow["BrandId"] != null && !string.IsNullOrEmpty(dataRow["BrandId"].ToString()))
                    {
                        brandId += dataRow["BrandId"] + ",";
                    }
                }
                flag = new ProductBatchDao().UpdateProductBrand(dt);
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "批量修改商品的品牌 “{0}” 的成功", new object[]
				{
					brandId
				}));
            }
            return flag;
        }


        public static bool AddSkusStock(List<SimpleSKUUpdateInfo> skulist, out List<ErrorSimpleSKUUpdateInfo> errorskulist)
        {
            return new SkuDao().AddSkusStock(skulist, out errorskulist);
        }

        public static bool SetGrossWeight(List<SimpleSKUUpdateInfo> skulist, out List<ErrorSimpleSKUUpdateInfo> errorskulist)
        {
            return new SkuDao().SetGrossWeight(skulist, out errorskulist);
        }

        public static bool UpdateSkusStock(string PONumber, List<SimpleSKUUpdateInfo> skulist)
        {
            return new SkuDao().UpdateSkusStock(PONumber, skulist);
        }

        public static bool AdjustSkusStock(List<SimpleSKUUpdateInfo> skulist, out List<ErrorSimpleSKUUpdateInfo> errorskulist)
        {
            return new SkuDao().AdjustSkusStock(skulist, out errorskulist);
        }

        /// <summary>
        /// 价格审核审核通过
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool AcceptPriceApprove(int productId)
        {
            return new ProductDao().AcceptPriceApprove(productId);
        }

        /// <summary>
        /// 价格审核审核通过
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="ApprovedPriceDescription"></param>
        /// <returns></returns>
        public static bool AcceptPriceApprove(int productId, string ApprovedPriceDescription)
        {
            return new ProductDao().AcceptPriceApprove(productId, ApprovedPriceDescription);
        }

        /// <summary>
        /// 商品是否已经审价
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool IsPriceApproved(int productId)
        {
            return new ProductDao().IsPriceApproved(productId);
        }


        /// <summary>
        /// 商品审价拒绝通过
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="approvedPriceDescription"></param>
        /// <returns></returns>
        public static bool RefusePriceApprove(int productId, string approvedPriceDescription)
        {
            return new ProductDao().RefusePriceApprove(productId, approvedPriceDescription);
        }

        /// <summary>
        /// 重新提交审价
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public static int ReSubmitPriceApprove(string productIds)
        {
            int result;
            if (string.IsNullOrEmpty(productIds))
            {
                result = 0;
            }
            else
            {
                int num = new ProductDao().ReSubmitPriceApprove(productIds, ApprovePriceStatus.WaitApprove);
                result = num;
            }
            return result;
        }


        /// <summary>
        /// 是否存在未归档的商品
        /// </summary>
        /// <param name="strProductIds"></param>
        /// <returns></returns>
        public static bool IsExitNoClassifyProduct(string strProductIds)
        {
            return new ProductDao().IsExitNoClassifyProduct(strProductIds);
        }

        /// <summary>
        /// 根据商品ID获取其组合商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static DataTable GetCombinationDataByProductId(int productId)
        {
            return new ProductDao().GetCombinationDataByProductId(productId);
        }


        /// <summary>
        /// 根据skuid获取其组合商品
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public static List<ProductsCombination> GetProductsCombinationsBySku(string skuId)
        {
            return new ProductDao().GetProductsCombinationsBySku(skuId);
        }

        /// <summary>
        /// 设置商品校验状态
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="Remark"></param>
        /// <param name="status"></param>
        public static void setProductCheck(string prodid, string Remark, string status)
        {
            new ProductDao().setProductCheck(prodid, Remark, status);
        }

        /// <summary>
        /// 设置商品商检状态
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="Remark"></param>
        /// <param name="status"></param>
        public static void setProductInspection(string prodid, string Remark,string status)
        {
            new ProductDao().setProductInspection(prodid, Remark, status);
        }
          /// <summary>
        /// 获取数据推送给广告商
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public static DataTable GetAdOrderInfo(string skuId)
        {
          return  new ProductDao().GetAdOrderInfo(skuId);
        }


          public static DataSet GetActiveProductListByTopicIds(string topicids,int maxnum)
        {
            return new PCActiveDao().GetActiveProductListByTopicIds(topicids,maxnum);
        }
    }
}

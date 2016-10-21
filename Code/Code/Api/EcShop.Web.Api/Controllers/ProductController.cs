using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using EcShop.Web.Api.ApiException;

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;

using EcShop.Entities;
using EcShop.Entities.Comments;
using EcShop.Entities.Commodities;
using EcShop.Entities.Promotions;
using EcShop.Entities.VShop;
using EcShop.Entities.Sales;

using EcShop.SqlDal;
using EcShop.SqlDal.Commodities;

using EcShop.Membership.Context;
using EcShop.Membership.Core;

using EcShop.SaleSystem.Comments;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Shopping;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;

using EcShop.Web.Api.Model;
using EcShop.Web.Api.Model.Result;
using EcShop.Web.Api.Model.RequestJsonParams;
using Commodities;
using EcShop.Entities.Orders;
using EcShop.ControlPanel.Sales;
using EcShop.SaleSystem.Vshop;

namespace EcShop.Web.Api.Controllers
{
    public class ProductController : EcdevApiController
    {
        public ProductController() { }

        [HttpGet]
        public IHttpActionResult Search(int siteId, string userId, string keyword, int categoryId, int sortType, string sortDirection, int pageSize, int pageIndex, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.Search, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&categoryId={5}&pageIndex={6}&pageSize={7}&sortType={8}&sortDirection={9}&keyword={10}", siteId, accessToken, channel, platform, ver, categoryId, pageIndex, pageSize, sortType, sortDirection, keyword), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Search");
            }

            int total = 0;
            string sortBy = GetSortTypeDesc(sortType);

            int? gradeId = new Nullable<int>();

            DataTable dt = ProductBrowser.GetProducts(null, categoryId, keyword, pageIndex, pageSize, out total, sortBy, true, gradeId, sortDirection);

            List<ProductListItem> products = new List<ProductListItem>();

            ProductListItem item = null;

            List<int> productIds = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                int productId = 0;
                if (row["ProductId"] != DBNull.Value)
                {
                    productId = (int)row["ProductId"];

                    if (productId > 0)
                    {
                        productIds.Add(productId);
                    }
                }
            }

            Member member = GetMember(userId.ToSeesionId());

            Dictionary<int, string> promotionInfo = new Dictionary<int, string>();

            if (productIds.Count > 0)
            {
                if (member != null)
                {
                    promotionInfo = ProductBrowser.GetProductPromotionInfo(member, productIds);
                }
                else
                {
                    promotionInfo = ProductBrowser.GetAllProductPromotionInfo(productIds);
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                // bool bol;
                int cid = 0;
                if (row["CategoryId"] != DBNull.Value)
                {
                    cid = (int)row["CategoryId"];
                }

                string mainCategoryPath = "";
                if (row["MainCategoryPath"] != DBNull.Value)
                {
                    mainCategoryPath = row["MainCategoryPath"].ToString();
                }

                // bol = (int)(row["CategoryId"] == DBNull.Value ? "0" : row["CategoryId"]) != 8000044 && !(row["MainCategoryPath"] == DBNull.Value ? "" : row["MainCategoryPath"].ToString()).Contains("8000044");

                if (cid != 8000044 && !mainCategoryPath.StartsWith("8000044|"))
                {
                    item = new ProductListItem();

                    item.ProductId = 0;
                    if (row["ProductId"] != DBNull.Value)
                    {
                        item.ProductId = (int)row["ProductId"];
                    }

                    item.CategoryId = 0;
                    if (row["CategoryId"] != DBNull.Value)
                    {
                        item.CategoryId = (int)row["CategoryId"];
                    }

                    item.Title = "";
                    if (row["ProductName"] != DBNull.Value)
                    {
                        item.Title = (string)row["ProductName"];
                    }

                    item.ImageUrl = "";
                    if (row["ThumbnailUrl180"] != DBNull.Value)
                    {
                        item.ImageUrl = Util.AppendImageHost((string)row["ThumbnailUrl180"]);
                    }

                    item.SalePrice = 0;
                    if (row["SalePrice"] != DBNull.Value)
                    {
                        item.SalePrice = (decimal)row["SalePrice"];
                    }

                    item.MarketPrice = 0;
                    if (row["MarketPrice"] != DBNull.Value)
                    {
                        item.MarketPrice = (decimal)row["MarketPrice"];
                    }

                    item.TaxRate = 0;
                    if (row["TaxRate"] != DBNull.Value)
                    {
                        item.TaxRate = (decimal)row["TaxRate"];
                    }

                    item.Quantity = 0;
                    if (row["Stock"] != DBNull.Value)
                    {
                        item.Quantity = (int)row["Stock"];
                    }

                    item.HasSku = false;
                    if (row["fastbuy_skuid"] != DBNull.Value)
                    {
                        item.SkuId = (string)row["fastbuy_skuid"];
                        item.HasSku = item.SkuId.Equals("");
                    }

                    if (item.HasSku)
                    {
                        item.SkuItems = this.GetSkuItems(item.ProductId);
                        item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId));
                    }

                    item.ShortDescription = "";
                    if (row["ShortDescription"] != DBNull.Value)
                    {
                        item.ShortDescription = (string)row["ShortDescription"];
                    }

                    item.IsCustomsClearance = false;
                    if (row["IsCustomsClearance"] != DBNull.Value)
                    {
                        item.IsCustomsClearance = (bool)row["IsCustomsClearance"];
                    }

                    item.IsFreeShipping = false;
                    if (row["IsfreeShipping"] != DBNull.Value)
                    {
                        item.IsCustomsClearance = (bool)row["IsfreeShipping"];
                    }

                    item.ShippingMode = "";
                    if (row["ShippingMode"] != DBNull.Value)
                    {
                        item.ShippingMode = (string)row["ShippingMode"];
                    }

                    item.PromotionName = "";
                    if (promotionInfo.ContainsKey(item.ProductId))
                    {
                        string promotionName = "";
                        if (promotionInfo.TryGetValue(item.ProductId, out promotionName))
                        {
                            item.PromotionName = promotionName;
                        }
                    }

                    item.SaleCounts = 0;
                    if (row["SaleCounts"] != DBNull.Value)
                    {
                        item.SaleCounts = (int)row["SaleCounts"];
                    }

                    item.VistiCounts = 0;
                    if (row["VistiCounts"] != DBNull.Value)
                    {
                        item.SaleCounts = (int)row["VistiCounts"];
                    }

                    products.Add(item);
                }
            }

            ListResult<ProductListItem> data = new ListResult<ProductListItem>();
            data.TotalNumOfRecords = total;
            data.Results = products;

            return base.JsonActionResult(new StandardResult<ListResult<ProductListItem>>()
                                        {
                                            code = 0,
                                            msg = "",
                                            data = data
                                        });
        }

        [HttpGet]
        public IHttpActionResult Category(int siteId, int categoryId, int pageSize, int pageIndex, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.Category, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&categoryId={5}&pageIndex={6}&pageSize={7}", siteId, accessToken, channel, platform, ver, categoryId, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Category");
            }

            int parentCategoryId = categoryId;

            List<CategoryInfo> categories = null;

            if (parentCategoryId == 0)
            {
                categories = CategoryBrowser.GetMainCategories().ToList();
            }
            else
            {
                categories = CategoryBrowser.GetSubCategories(parentCategoryId).ToList();
            }

            List<CategoryListItem> items = new List<CategoryListItem>();

            CategoryListItem item = null;

            foreach (CategoryInfo current in categories)
            {
                if (current.CategoryId != 8000044 && !current.Path.Contains("8000044") && (current.ParentCategoryId == null ? 0 : current.ParentCategoryId) != 8000044)
                {
                    item = new CategoryListItem();

                    item.CategoryId = current.CategoryId;
                    item.CategoryName = current.Name;
                    item.ImageUrl = current.IconUrl;
                    item.Path = current.Path;
                    item.ParentCategoryId = current.ParentCategoryId;
                    item.DisplaySequence = current.DisplaySequence;
                    item.Children = null;

                    items.Add(item);
                }
            }

            ListResult<CategoryListItem> data = new ListResult<CategoryListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<CategoryListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult AllCategories(int siteId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.AllCategories, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", siteId, accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.AllCategories");
            }

            List<CategoryInfo> categories = CategoryBrowser.GetMainCategories().ToList();
            List<CategoryListItem> items = new List<CategoryListItem>();

            if (categories != null)
            {
                CategoryListItem item = null;

                foreach (CategoryInfo current in categories)
                {
                    if (current.CategoryId != 8000044 && !current.Path.Contains("8000044") && (current.ParentCategoryId == null ? 0 : current.ParentCategoryId) != 8000044)
                    {
                        item = new CategoryListItem();

                        item.CategoryId = current.CategoryId;
                        item.CategoryName = current.Name;
                        item.ImageUrl = Util.AppendImageHost(current.Icon);
                        item.Path = current.Path;
                        item.ParentCategoryId = current.ParentCategoryId;
                        item.DisplaySequence = current.DisplaySequence;

                        item.Children = GetSubCategories(current.CategoryId);

                        items.Add(item);
                    }
                }
            }

            ListResult<CategoryListItem> data = new ListResult<CategoryListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<CategoryListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult CategoriesandPlace(int siteId, string accessToken, int channel, int platform, string ver, int categoryId = 0)
        {
            Logger.WriterLogger("Product.CategoriesandPlace, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&CategoryId={5}", siteId, accessToken, channel, platform, ver, categoryId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.AllCategories");
            }

            List<CategoryInfo> categories = new List<CategoryInfo>(); //CategoryBrowser.GetMainCategories().ToList();
            List<CategoryListItem> items = new List<CategoryListItem>();
            if (categoryId != 0 && categoryId > 0)
            {
                items = GetSubCategories(categoryId);
            }
            else
            {
                categories = CategoryBrowser.GetMainCategories().ToList();
                if (categories != null)
                {
                    CategoryListItem item = null;

                    foreach (CategoryInfo current in categories)
                    {
                        if (current.CategoryId != 8000044 && !current.Path.Contains("8000044") && (current.ParentCategoryId == null ? 0 : current.ParentCategoryId) != 8000044)
                        {
                            item = new CategoryListItem();

                            item.CategoryId = current.CategoryId;
                            item.CategoryName = current.Name;
                            item.ImageUrl = Util.AppendImageHost(current.Icon);
                            item.Path = current.Path;
                            item.ParentCategoryId = current.ParentCategoryId;
                            item.DisplaySequence = current.DisplaySequence;

                            item.Children = GetSubCategories(current.CategoryId);

                            items.Add(item);
                        }
                    }
                }
            }

            ListResult<CategoryListItem> data = new ListResult<CategoryListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            ProductFilterResult result = new ProductFilterResult();
            result.code = 0;
            result.msg = "";
            result.data = new ProductFilterData();

            // 商品种类
            result.data.Category = data;

            // 原产地
            result.data.OriginalPlace = this.GetOriginPlace(categoryId);


            return base.JsonActionResult(result);
        }

        public IHttpActionResult GetCategoriesBySupplierId(int supplierId, int siteId, string accessToken, int channel, int platform, string ver)
        {

            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.AllCategories");
            }

            // 根据供应商ID(店铺ID)获取其最后一层的商品分类名称
            List<CategoryInfo> categories = CategoryBrowser.GetCategoryiesBySupplierId(supplierId).ToList();
            List<CategoryListItem> items = new List<CategoryListItem>();
            if (categories != null)
            {
                CategoryListItem item = null;
                foreach (CategoryInfo current in categories)
                {
                    item = new CategoryListItem();
                    item.CategoryId = current.CategoryId;
                    item.CategoryName = current.Name;
                    item.ImageUrl = Util.AppendImageHost(current.Icon);
                    item.Path = current.Path;
                    item.ParentCategoryId = current.ParentCategoryId;
                    item.DisplaySequence = current.DisplaySequence;
                    item.Children = null;
                    items.Add(item);
                }
            }
            ListResult<CategoryListItem> data = new ListResult<CategoryListItem>();
            data.TotalNumOfRecords = items.Count;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<CategoryListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }


        [HttpGet]
        public IHttpActionResult Brand(int siteId, int categoryId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.Brand, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&categoryId={5}", siteId, accessToken, channel, platform, ver, categoryId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Brand");
            }

            DataTable brands = null;

            if (categoryId == 0)
            {
                brands = CatalogHelper.GetBrandCategories();
            }
            else
            {
                brands = CatalogHelper.GetBrandCategories(categoryId);
            }

            List<BrandListItem> items = new List<BrandListItem>();

            if (brands != null)
            {
                BrandListItem item = null;

                foreach (DataRow current in brands.Rows)
                {
                    item = new BrandListItem();

                    item.BrandId = 0;
                    if (current["BrandId"] != DBNull.Value)
                    {
                        item.BrandId = (int)current["BrandId"];
                    }

                    item.BrandName = "";
                    if (current["BrandName"] != DBNull.Value)
                    {
                        item.BrandName = (string)current["BrandName"];
                    }

                    item.Icon = "";
                    if (current["Logo"] != DBNull.Value)
                    {
                        item.Icon = Util.AppendImageHost(((string)current["Logo"]));
                    }

                    item.Description = "";
                    if (current["Description"] != DBNull.Value)
                    {
                        item.Description = (string)current["Description"];
                    }

                    item.DisplaySequence = 0;
                    if (current["DisplaySequence"] != DBNull.Value)
                    {
                        item.DisplaySequence = (int)current["DisplaySequence"];
                    }

                    items.Add(item);
                }
            }

            ListResult<BrandListItem> data = new ListResult<BrandListItem>();
            data.TotalNumOfRecords = items.Count;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<BrandListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult OriginalPlace(int siteId, int categoryId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.OriginalPlace, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&categoryId={5}", siteId, accessToken, channel, platform, ver, categoryId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.OriginalPlace");
            }
            #region ==
            List<ImportSourceTypeInfo> categories = null;

            if (categoryId == 0)
            {
                categories = ImportSourceTypeHelper.GetAllImportSourceTypes().ToList();
            }
            else
            {
                categories = ImportSourceTypeHelper.GetAllImportSourceTypes(categoryId).ToList();
            }

            List<OriginPlaceListItem> items = new List<OriginPlaceListItem>();

            if (categories != null)
            {
                OriginPlaceListItem item = null;

                foreach (ImportSourceTypeInfo current in categories)
                {
                    item = new OriginPlaceListItem();

                    item.PlaceId = current.ImportSourceId;
                    item.PlaceName = current.CnArea;
                    item.Icon = Util.AppendImageHost(current.Icon);
                    item.DisplaySequence = current.DisplaySequence ?? 0;

                    items.Add(item);
                }
            }

            ListResult<OriginPlaceListItem> data = new ListResult<OriginPlaceListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<OriginPlaceListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
            #endregion
        }


        /// <summary>
        /// 将一个大数字符串从M进制转换成N进制
        /// </summary>
        /// <param name="sourceValue">M进制数字字符串</param>
        /// <param name="sourceBaseChars">M进制字符集</param>
        /// <param name="newBaseChars">N进制字符集</param>
        /// <returns>N进制数字字符串</returns>
        public string BaseConvert(string sourceValue, string sourceBaseChars, string newBaseChars)
        {

            //M进制
            var sBase = sourceBaseChars.Length;
            //N进制
            var tBase = newBaseChars.Length;
            //M进制数字字符串合法性判断（判断M进制数字字符串中是否有不包含在M进制字符集中的字符）
            if (sourceValue.Any(s => !sourceBaseChars.Contains(s))) return null;
            //将M进制数字字符串的每一位字符转为十进制数字依次存入到LIST中
            var intSource = new List<int>();
            intSource.AddRange(sourceValue.Select(c => sourceBaseChars.IndexOf(c)));
            //余数列表
            var res = new List<int>();
            //开始转换（判断十进制LIST是否为空或只剩一位且这个数字小于N进制）
            while (!((intSource.Count == 1 && intSource[0] < tBase) || intSource.Count == 0))
            {
                //每一轮的商值集合
                var ans = new List<int>();
                var y = 0;
                //十进制LIST中的数字逐一除以N进制（注意：需要加上上一位计算后的余数乘以M进制）
                foreach (var t in intSource)
                {
                    //当前位的数值加上上一位计算后的余数乘以M进制
                    y = y * sBase + t;

                    //保存当前位与N进制的商值
                    ans.Add(y / tBase);

                    //计算余值
                    y %= tBase;

                }

                //将此轮的余数添加到余数列表
                res.Add(y);

                //将此轮的商值（去除0开头的数字）存入十进制LIST做为下一轮的被除数
                var flag = false;

                intSource.Clear();

                foreach (var a in ans.Where(a => a != 0 || flag))
                {
                    flag = true;

                    intSource.Add(a);

                }

            }

            //如果十进制LIST还有数字，需将此数字添加到余数列表后
            if (intSource.Count > 0) res.Add(intSource[0]);

            //将余数列表反转，并逐位转换为N进制字符
            var nValue = string.Empty;

            for (var i = res.Count - 1; i >= 0; i--)
            {

                nValue += newBaseChars[res[i]].ToString();

            }

            return nValue;
        }

        [HttpGet]
        public IHttpActionResult Topic(int siteId, int pageIndex, int pageSize, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.Topic, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}", siteId, accessToken, channel, platform, ver, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Topic");
            }

            DbQueryResult dbQueryResult = VShopHelper.GettopicList(new TopicQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                IsincludeHomeProduct = false,
                SortBy = "DisplaySequence",
                SortOrder = SortAction.Asc,
                Client = 3
            });

            DataTable dt = dbQueryResult.Data as DataTable;

            List<TopicListItem> items = new List<TopicListItem>();

            if (dt != null)
            {
                TopicListItem item = null;

                foreach (DataRow current in dt.Rows)
                {
                    item = new TopicListItem();

                    item.TopicId = 0;
                    if (current["TopicId"] != DBNull.Value)
                    {
                        item.TopicId = (int)current["TopicId"];
                    }

                    item.TopicName = "";
                    if (current["Title"] != DBNull.Value)
                    {
                        item.TopicName = (string)current["Title"];
                    }

                    item.Icon = "";
                    if (current["IconUrl"] != DBNull.Value)
                    {
                        item.Icon = Util.AppendImageHost(((string)current["IconUrl"]));
                    }

                    item.Content = "";
                    if (current["Content"] != DBNull.Value)
                    {
                        var regex = new Regex(@"""/Storage/master");

                        item.Content = regex.Replace((string)current["Content"], "\"" + base.STORAGE_HOST + @"/Storage/master");
                    }

                    item.DisplaySequence = 0;
                    if (current["DisplaySequence"] != DBNull.Value)
                    {
                        item.DisplaySequence = (int)current["DisplaySequence"];
                    }

                    items.Add(item);
                }
            }

            ListResult<TopicListItem> data = new ListResult<TopicListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<TopicListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult TopicProducts(int siteId, string userId, int topicId, int sortType, string sortDirection, int pageSize, int pageIndex, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.TopicProducts, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&topicId={5}&pageIndex={6}&pageSize={7}&sortType={8}&sortDirection={9}", siteId, accessToken, channel, platform, ver, topicId, pageIndex, pageSize, sortType, sortDirection), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.TopicProducts");
            }

            int total = 0;
            string sortBy = GetSortTypeDesc(sortType);

            int? gradeId = new Nullable<int>();

            DataTable dt = ProductBrowser.GetProducts(topicId, null, null, null, pageIndex, pageSize, gradeId, out total, sortBy, sortDirection);

            List<ProductListItem> products = new List<ProductListItem>();

            ProductListItem item = null;

            List<int> productIds = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                int productId = 0;
                if (row["ProductId"] != DBNull.Value)
                {
                    productId = (int)row["ProductId"];

                    if (productId > 0)
                    {
                        productIds.Add(productId);
                    }
                }
            }

            Member member = GetMember(userId.ToSeesionId());

            Dictionary<int, string> promotionInfo = new Dictionary<int, string>();

            if (productIds.Count > 0)
            {
                if (member != null)
                {
                    promotionInfo = ProductBrowser.GetProductPromotionInfo(member, productIds);
                }
                else
                {
                    promotionInfo = ProductBrowser.GetAllProductPromotionInfo(productIds);
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                // bool bol;
                int cid = 0;
                if (row["CategoryId"] != DBNull.Value)
                {
                    cid = (int)row["CategoryId"];
                }

                string mainCategoryPath = "";
                if (row["MainCategoryPath"] != DBNull.Value)
                {
                    mainCategoryPath = row["MainCategoryPath"].ToString();
                }

                // bol = (int)(row["CategoryId"] == DBNull.Value ? "0" : row["CategoryId"]) != 8000044 && !(row["MainCategoryPath"] == DBNull.Value ? "" : row["MainCategoryPath"].ToString()).Contains("8000044");

                if (cid != 8000044 && !mainCategoryPath.StartsWith("8000044|"))
                {
                    item = new ProductListItem();

                    item.ProductId = 0;
                    if (row["ProductId"] != DBNull.Value)
                    {
                        item.ProductId = (int)row["ProductId"];
                    }

                    item.CategoryId = 0;
                    if (row["CategoryId"] != DBNull.Value)
                    {
                        item.CategoryId = (int)row["CategoryId"];
                    }

                    item.Title = "";
                    if (row["ProductName"] != DBNull.Value)
                    {
                        item.Title = (string)row["ProductName"];
                    }

                    item.ImageUrl = "";
                    if (row["ThumbnailUrl180"] != DBNull.Value)
                    {
                        item.ImageUrl = Util.AppendImageHost((string)row["ThumbnailUrl180"]);
                    }

                    item.SalePrice = 0;
                    if (row["SalePrice"] != DBNull.Value)
                    {
                        item.SalePrice = (decimal)row["SalePrice"];
                    }

                    item.MarketPrice = 0;
                    if (row["MarketPrice"] != DBNull.Value)
                    {
                        item.MarketPrice = (decimal)row["MarketPrice"];
                    }

                    item.TaxRate = 0;
                    if (row["TaxRate"] != DBNull.Value)
                    {
                        item.TaxRate = (decimal)row["TaxRate"];
                    }

                    item.Quantity = 0;
                    if (row["Stock"] != DBNull.Value)
                    {
                        item.Quantity = (int)row["Stock"];
                    }

                    item.HasSku = false;
                    if (row["fastbuy_skuid"] != DBNull.Value)
                    {
                        item.SkuId = (string)row["fastbuy_skuid"];
                        item.HasSku = item.SkuId.Equals("");
                    }

                    if (item.HasSku)
                    {
                        item.SkuItems = this.GetSkuItems(item.ProductId);
                        item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId));
                    }

                    item.ShortDescription = "";
                    if (row["ShortDescription"] != DBNull.Value)
                    {
                        item.ShortDescription = (string)row["ShortDescription"];
                    }

                    item.IsCustomsClearance = false;
                    if (row["IsCustomsClearance"] != DBNull.Value)
                    {
                        item.IsCustomsClearance = (bool)row["IsCustomsClearance"];
                    }

                    item.IsFreeShipping = false;
                    if (row["IsfreeShipping"] != DBNull.Value)
                    {
                        item.IsCustomsClearance = (bool)row["IsfreeShipping"];
                    }

                    item.ShippingMode = "";
                    if (row["ShippingMode"] != DBNull.Value)
                    {
                        item.ShippingMode = (string)row["ShippingMode"];
                    }

                    item.PromotionName = "";
                    if (promotionInfo.ContainsKey(item.ProductId))
                    {
                        string promotionName = "";
                        if (promotionInfo.TryGetValue(item.ProductId, out promotionName))
                        {
                            item.PromotionName = promotionName;
                        }
                    }

                    item.SaleCounts = 0;
                    if (row["SaleCounts"] != DBNull.Value)
                    {
                        item.SaleCounts = (int)row["SaleCounts"];
                    }

                    item.VistiCounts = 0;
                    if (row["VistiCounts"] != DBNull.Value)
                    {
                        item.SaleCounts = (int)row["VistiCounts"];
                    }

                    products.Add(item);
                }
            }

            ListResult<ProductListItem> data = new ListResult<ProductListItem>();
            data.TotalNumOfRecords = total;
            data.Results = products;

            return base.JsonActionResult(new StandardResult<ListResult<ProductListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult Suggest(int siteId, string userId, int count, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.Suggest, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&userId={5}&count={6}", siteId, accessToken, channel, platform, ver, userId, count), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Suggest");
            }

            int intUserId = 0;
            string username = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                intUserId = member.UserId;
                username = member.Username;
            }

            DataTable dt = ProductBrowser.GetSuggestProducts(intUserId, username, count);     //TODO

            List<ProductListItem> products = new List<ProductListItem>();

            List<int> productIds = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                int productId = 0;
                if (row["ProductId"] != DBNull.Value)
                {
                    productId = (int)row["ProductId"];

                    if (productId > 0)
                    {
                        productIds.Add(productId);
                    }
                }
            }

            Dictionary<int, string> promotionInfo = new Dictionary<int, string>();

            if (productIds.Count > 0)
            {
                if (member != null)
                {
                    promotionInfo = ProductBrowser.GetProductPromotionInfo(member, productIds);
                }
                else
                {
                    promotionInfo = ProductBrowser.GetAllProductPromotionInfo(productIds);
                }
            }

            if (dt != null)
            {
                ProductListItem item = null;

                foreach (DataRow row in dt.Rows)
                {
                    item = new ProductListItem();

                    item.ProductId = 0;
                    if (row["ProductId"] != DBNull.Value)
                    {
                        item.ProductId = (int)row["ProductId"];
                    }

                    item.CategoryId = 0;
                    if (row["CategoryId"] != DBNull.Value)
                    {
                        item.CategoryId = (int)row["CategoryId"];
                    }

                    item.Title = "";
                    if (row["ProductName"] != DBNull.Value)
                    {
                        item.Title = (string)row["ProductName"];
                    }

                    item.ImageUrl = "";
                    if (row["ThumbnailUrl180"] != DBNull.Value)
                    {
                        item.ImageUrl = Util.AppendImageHost((string)row["ThumbnailUrl180"]);
                    }

                    item.SalePrice = 0;
                    if (row["SalePrice"] != DBNull.Value)
                    {
                        item.SalePrice = (decimal)row["SalePrice"];
                    }

                    item.MarketPrice = 0;
                    if (row["MarketPrice"] != DBNull.Value)
                    {
                        item.MarketPrice = (decimal)row["MarketPrice"];
                    }

                    item.TaxRate = 0;
                    if (row["TaxRate"] != DBNull.Value)
                    {
                        item.TaxRate = (decimal)row["TaxRate"];
                    }

                    item.Quantity = 0;
                    if (row["Stock"] != DBNull.Value)
                    {
                        item.Quantity = (int)row["Stock"];
                    }

                    item.HasSku = false;
                    if (row["fastbuy_skuid"] != DBNull.Value)
                    {
                        item.SkuId = (string)row["fastbuy_skuid"];
                        item.HasSku = item.SkuId.Equals("");
                    }

                    if (item.HasSku)
                    {
                        item.SkuItems = this.GetSkuItems(item.ProductId);
                        item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId));
                    }

                    item.ShortDescription = "";
                    if (row["ShortDescription"] != DBNull.Value)
                    {
                        item.ShortDescription = (string)row["ShortDescription"];
                    }

                    item.IsCustomsClearance = false;
                    if (row["IsCustomsClearance"] != DBNull.Value)
                    {
                        item.IsCustomsClearance = (bool)row["IsCustomsClearance"];
                    }

                    item.IsFreeShipping = false;
                    if (row["IsfreeShipping"] != DBNull.Value)
                    {
                        item.IsCustomsClearance = (bool)row["IsfreeShipping"];
                    }

                    item.ShippingMode = "";
                    if (row["ShippingMode"] != DBNull.Value)
                    {
                        item.ShippingMode = (string)row["ShippingMode"];
                    }

                    item.PromotionName = "";
                    if (promotionInfo.ContainsKey(item.ProductId))
                    {
                        string promotionName = "";
                        if (promotionInfo.TryGetValue(item.ProductId, out promotionName))
                        {
                            item.PromotionName = promotionName;
                        }
                    }

                    item.SaleCounts = 0;
                    if (row["SaleCounts"] != DBNull.Value)
                    {
                        item.SaleCounts = (int)row["SaleCounts"];
                    }

                    item.VistiCounts = 0;
                    if (row["VistiCounts"] != DBNull.Value)
                    {
                        item.SaleCounts = (int)row["VistiCounts"];
                    }

                    products.Add(item);
                }
            }

            ListResult<ProductListItem> data = new ListResult<ProductListItem>();
            data.TotalNumOfRecords = products.Count;
            data.Results = products;

            return base.JsonActionResult(new StandardResult<ListResult<ProductListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult History(int siteId, string userId, int pageIndex, int pageSize, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.History, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&userId={5}&pageIndex={6}&pageSize={7}", siteId, accessToken, channel, platform, ver, userId, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.History");
            }

            int intUserId = 0;
            string username = "";

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                intUserId = member.UserId;
                username = member.Username;
            }

            int count = 0;

            DataTable dt = ProductBrowser.GetHistoryProducts(intUserId, username, pageIndex, pageSize, out count);

            List<HistoryProductListItem> products = new List<HistoryProductListItem>();

            List<int> productIds = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                int productId = 0;
                if (row["ProductId"] != DBNull.Value)
                {
                    productId = (int)row["ProductId"];

                    if (productId > 0)
                    {
                        productIds.Add(productId);
                    }
                }
            }

            Dictionary<int, string> promotionInfo = new Dictionary<int, string>();

            if (productIds.Count > 0)
            {
                if (member != null)
                {
                    promotionInfo = ProductBrowser.GetProductPromotionInfo(member, productIds);
                }
                else
                {
                    promotionInfo = ProductBrowser.GetAllProductPromotionInfo(productIds);
                }
            }

            if (dt != null)
            {
                HistoryProductListItem item = null;

                foreach (DataRow row in dt.Rows)
                {
                    item = new HistoryProductListItem();

                    item.Id = 0;
                    if (row["HistoryId"] != DBNull.Value)
                    {
                        item.Id = (int)row["HistoryId"];
                    }

                    item.BrowseTime = "";
                    item.BrowseDate = "";
                    if (row["BrowseTime"] != DBNull.Value)
                    {
                        item.BrowseTime = ((DateTime)row["BrowseTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                        item.BrowseDate = item.BrowseTime.Substring(0, 10);
                    }


                    item.ProductId = 0;
                    if (row["ProductId"] != DBNull.Value)
                    {
                        item.ProductId = (int)row["ProductId"];
                    }

                    item.CategoryId = 0;
                    if (row["CategoryId"] != DBNull.Value)
                    {
                        item.CategoryId = (int)row["CategoryId"];
                    }

                    item.Title = "";
                    if (row["ProductName"] != DBNull.Value)
                    {
                        item.Title = (string)row["ProductName"];
                    }

                    item.ImageUrl = "";
                    if (row["ThumbnailUrl180"] != DBNull.Value)
                    {
                        item.ImageUrl = Util.AppendImageHost((string)row["ThumbnailUrl180"]);
                    }

                    item.SalePrice = 0;
                    if (row["SalePrice"] != DBNull.Value)
                    {
                        item.SalePrice = (decimal)row["SalePrice"];
                    }

                    item.MarketPrice = 0;
                    if (row["MarketPrice"] != DBNull.Value)
                    {
                        item.MarketPrice = (decimal)row["MarketPrice"];
                    }

                    item.TaxRate = 0;
                    if (row["TaxRate"] != DBNull.Value)
                    {
                        item.TaxRate = (decimal)row["TaxRate"];
                    }

                    item.Quantity = 0;
                    if (row["Stock"] != DBNull.Value)
                    {
                        item.Quantity = (int)row["Stock"];
                    }

                    item.HasSku = false;
                    if (row["fastbuy_skuid"] != DBNull.Value)
                    {
                        item.SkuId = (string)row["fastbuy_skuid"];
                        item.HasSku = item.SkuId.Equals("");
                    }

                    if (item.HasSku)
                    {
                        item.SkuItems = this.GetSkuItems(item.ProductId);
                        item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId));
                    }

                    item.ShortDescription = "";
                    if (row["ShortDescription"] != DBNull.Value)
                    {
                        item.ShortDescription = (string)row["ShortDescription"];
                    }

                    item.IsCustomsClearance = false;
                    if (row["IsCustomsClearance"] != DBNull.Value)
                    {
                        item.IsCustomsClearance = (bool)row["IsCustomsClearance"];
                    }

                    item.IsFreeShipping = false;
                    if (row["IsfreeShipping"] != DBNull.Value)
                    {
                        item.IsCustomsClearance = (bool)row["IsfreeShipping"];
                    }

                    item.ShippingMode = "";
                    if (row["ShippingMode"] != DBNull.Value)
                    {
                        item.ShippingMode = (string)row["ShippingMode"];
                    }

                    item.PromotionName = "";
                    if (promotionInfo.ContainsKey(item.ProductId))
                    {
                        string promotionName = "";
                        if (promotionInfo.TryGetValue(item.ProductId, out promotionName))
                        {
                            item.PromotionName = promotionName;
                        }
                    }

                    item.SaleCounts = 0;
                    if (row["SaleCounts"] != DBNull.Value)
                    {
                        item.SaleCounts = (int)row["SaleCounts"];
                    }

                    item.VistiCounts = 0;
                    if (row["VistiCounts"] != DBNull.Value)
                    {
                        item.SaleCounts = (int)row["VistiCounts"];
                    }

                    products.Add(item);
                }
            }

            ListResult<HistoryProductListItem> data = new ListResult<HistoryProductListItem>();
            data.TotalNumOfRecords = count;
            data.Results = products;

            return base.JsonActionResult(new StandardResult<ListResult<HistoryProductListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult GetProductsByCategory(int siteId, string userId, int categoryId, int brandId, int originPlaceId, int sortType, string sortDirection, int pageSize, int pageIndex, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.GetProducsByCateogry, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&categoryId={5}&pageIndex={6}&pageSize={7}&sortType={8}&sortDirection={9}&brandId={10}&originPlaceId={11}", siteId, accessToken, channel, platform, ver, categoryId, pageIndex, pageSize, sortType, sortDirection, brandId, originPlaceId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.GetProductsByCategory");
            }

            int total = 0;
            string sortBy = GetSortTypeDesc(sortType);

            int? gradeId = new Nullable<int>();
            int? pBrandId = new Nullable<int>();
            if (brandId > 0)
            {
                pBrandId = brandId;
            }
            int? pOriginPlaceId = new Nullable<int>();
            if (originPlaceId > 0)
            {
                pOriginPlaceId = originPlaceId;
            }

            DataTable dt = ProductBrowser.GetProducts(null, categoryId, pBrandId, pOriginPlaceId, pageIndex, pageSize, gradeId, out total, sortBy, sortDirection);

            List<ProductListItem> products = new List<ProductListItem>();

            ProductListItem item = null;

            List<int> productIds = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                int productId = 0;
                if (row["ProductId"] != DBNull.Value)
                {
                    productId = (int)row["ProductId"];

                    if (productId > 0)
                    {
                        productIds.Add(productId);
                    }
                }
            }

            Member member = GetMember(userId.ToSeesionId());

            Dictionary<int, string> promotionInfo = new Dictionary<int, string>();

            if (productIds.Count > 0)
            {
                if (member != null)
                {
                    promotionInfo = ProductBrowser.GetProductPromotionInfo(member, productIds);
                }
                else
                {
                    promotionInfo = ProductBrowser.GetAllProductPromotionInfo(productIds);
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                // bool bol;
                int cid = 0;
                if (row["CategoryId"] != DBNull.Value)
                {
                    cid = (int)row["CategoryId"];
                }

                string mainCategoryPath = "";
                if (row["MainCategoryPath"] != DBNull.Value)
                {
                    mainCategoryPath = row["MainCategoryPath"].ToString();
                }

                // bol = (int)(row["CategoryId"] == DBNull.Value ? "0" : row["CategoryId"]) != 8000044 && !(row["MainCategoryPath"] == DBNull.Value ? "" : row["MainCategoryPath"].ToString()).Contains("8000044");

                if (cid != 8000044 && !mainCategoryPath.StartsWith("8000044|"))
                {
                    item = new ProductListItem();

                    item.ProductId = 0;
                    if (row["ProductId"] != DBNull.Value)
                    {
                        item.ProductId = (int)row["ProductId"];
                    }

                    item.CategoryId = 0;
                    if (row["CategoryId"] != DBNull.Value)
                    {
                        item.CategoryId = (int)row["CategoryId"];
                    }

                    item.Title = "";
                    if (row["ProductName"] != DBNull.Value)
                    {
                        item.Title = (string)row["ProductName"];
                    }

                    item.ImageUrl = "";
                    if (row["ThumbnailUrl180"] != DBNull.Value)
                    {
                        item.ImageUrl = Util.AppendImageHost((string)row["ThumbnailUrl180"]);
                    }

                    item.SalePrice = 0;
                    if (row["SalePrice"] != DBNull.Value)
                    {
                        item.SalePrice = (decimal)row["SalePrice"];
                    }

                    item.MarketPrice = 0;
                    if (row["MarketPrice"] != DBNull.Value)
                    {
                        item.MarketPrice = (decimal)row["MarketPrice"];
                    }

                    item.TaxRate = 0;
                    if (row["TaxRate"] != DBNull.Value)
                    {
                        item.TaxRate = (decimal)row["TaxRate"];
                    }

                    item.Quantity = 0;
                    if (row["Stock"] != DBNull.Value)
                    {
                        item.Quantity = (int)row["Stock"];
                    }

                    item.HasSku = false;
                    if (row["fastbuy_skuid"] != DBNull.Value)
                    {
                        item.SkuId = (string)row["fastbuy_skuid"];
                        item.HasSku = item.SkuId.Equals("");
                    }

                    if (item.HasSku)
                    {
                        item.SkuItems = this.GetSkuItems(item.ProductId);
                        item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId));
                    }

                    item.ShortDescription = "";
                    if (row["ShortDescription"] != DBNull.Value)
                    {
                        item.ShortDescription = (string)row["ShortDescription"];
                    }

                    item.IsCustomsClearance = false;
                    if (row["IsCustomsClearance"] != DBNull.Value)
                    {
                        item.IsCustomsClearance = (bool)row["IsCustomsClearance"];
                    }

                    item.IsFreeShipping = false;
                    if (row["IsfreeShipping"] != DBNull.Value)
                    {
                        item.IsCustomsClearance = (bool)row["IsfreeShipping"];
                    }

                    item.ShippingMode = "";
                    if (row["ShippingMode"] != DBNull.Value)
                    {
                        item.ShippingMode = (string)row["ShippingMode"];
                    }

                    item.PromotionName = "";
                    if (promotionInfo.ContainsKey(item.ProductId))
                    {
                        string promotionName = "";
                        if (promotionInfo.TryGetValue(item.ProductId, out promotionName))
                        {
                            item.PromotionName = promotionName;
                        }
                    }

                    item.SaleCounts = 0;
                    if (row["SaleCounts"] != DBNull.Value)
                    {
                        item.SaleCounts = (int)row["SaleCounts"];
                    }

                    item.VistiCounts = 0;
                    if (row["VistiCounts"] != DBNull.Value)
                    {
                        item.SaleCounts = (int)row["VistiCounts"];
                    }

                    products.Add(item);
                }
            }

            ListResult<ProductListItem> data = new ListResult<ProductListItem>();
            data.TotalNumOfRecords = total;
            data.Results = products;

            return base.JsonActionResult(new StandardResult<ListResult<ProductListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpPost]

        #region 弃用代码
        //public IHttpActionResult List(int siteId, string userId, int categoryId, int brandId, int originPlaceId, bool isFreeShip, string keyword, int sortType, string sortDirection, int pageSize, int pageIndex, string accessToken, int channel, int platform, string ver)
        //{
        //    Logger.WriterLogger("Product.List, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&categoryId={5}&pageIndex={6}&pageSize={7}&sortType={8}&sortDirection={9}&brandId={10}&originPlaceId={11}&keyword={12}&isFreeShip={13}", siteId, accessToken, channel, platform, ver, categoryId, pageIndex, pageSize, sortType, sortDirection, brandId, originPlaceId, keyword, isFreeShip), LoggerType.Info);

        //    // 保存访问信息
        //    base.SaveVisitInfo(siteId, userId, channel, platform, ver);

        //    // 验证令牌
        //    int accessTookenCode = VerifyAccessToken(accessToken);
        //    if (accessTookenCode > 0)
        //    {
        //        return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.List");
        //    }

        //    int total = 0;
        //    string sortBy = GetSortTypeDesc(sortType);

        //    int? gradeId = new Nullable<int>();
        //    int? pCategoryId = new Nullable<int>();
        //    if (categoryId > 0)
        //    {
        //        pCategoryId = categoryId;
        //    }
        //    int? pBrandId = new Nullable<int>();
        //    if (brandId > 0)
        //    {
        //        pBrandId = brandId;
        //    }
        //    int? pOriginPlaceId = new Nullable<int>();
        //    if (originPlaceId > 0)
        //    {
        //        pOriginPlaceId = originPlaceId;
        //    }
        //    bool? pIsFreeShip = new Nullable<bool>();
        //    if (isFreeShip)
        //    {
        //        pIsFreeShip = true;
        //    }

        //    DataTable dt = ProductBrowser.GetProducts(null, pCategoryId, pBrandId, pOriginPlaceId, pIsFreeShip, keyword, pageIndex, pageSize, gradeId, out total, sortBy, sortDirection);

        //    List<ProductListItem> products = new List<ProductListItem>();

        //    ProductListItem item = null;

        //    List<int> productIds = new List<int>();

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        int productId = 0;
        //        if (row["ProductId"] != DBNull.Value)
        //        {
        //            productId = (int)row["ProductId"];

        //            if (productId > 0)
        //            {
        //                productIds.Add(productId);
        //            }
        //        }
        //    }

        //    Member member = GetMember(userId.ToSeesionId());

        //    Dictionary<int, string> promotionInfo = new Dictionary<int, string>();

        //    if (productIds.Count > 0)
        //    {
        //        if (member != null)
        //        {
        //            promotionInfo = ProductBrowser.GetProductPromotionInfo(member, productIds);
        //        }
        //        else
        //        {
        //            promotionInfo = ProductBrowser.GetAllProductPromotionInfo(productIds);
        //        }
        //    }

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        // bool bol;
        //        int cid = 0;
        //        if (row["CategoryId"] != DBNull.Value)
        //        {
        //            cid = (int)row["CategoryId"];
        //        }

        //        string mainCategoryPath = "";
        //        if (row["MainCategoryPath"] != DBNull.Value)
        //        {
        //            mainCategoryPath = row["MainCategoryPath"].ToString();
        //        }

        //        // bol = (int)(row["CategoryId"] == DBNull.Value ? "0" : row["CategoryId"]) != 8000044 && !(row["MainCategoryPath"] == DBNull.Value ? "" : row["MainCategoryPath"].ToString()).Contains("8000044");

        //        if (cid != 8000044 && !mainCategoryPath.StartsWith("8000044|"))
        //        {
        //            item = new ProductListItem();

        //            item.ProductId = 0;
        //            if (row["ProductId"] != DBNull.Value)
        //            {
        //                item.ProductId = (int)row["ProductId"];
        //            }

        //            item.CategoryId = 0;
        //            if (row["CategoryId"] != DBNull.Value)
        //            {
        //                item.CategoryId = (int)row["CategoryId"];
        //            }

        //            item.Title = "";
        //            if (row["ProductName"] != DBNull.Value)
        //            {
        //                item.Title = (string)row["ProductName"];
        //            }

        //            item.ImageUrl = "";
        //            if (row["ThumbnailUrl180"] != DBNull.Value)
        //            {
        //                item.ImageUrl = Util.AppendImageHost((string)row["ThumbnailUrl180"]);
        //            }

        //            item.SalePrice = 0;
        //            if (row["SalePrice"] != DBNull.Value)
        //            {
        //                item.SalePrice = (decimal)row["SalePrice"];
        //            }

        //            item.MarketPrice = 0;
        //            if (row["MarketPrice"] != DBNull.Value)
        //            {
        //                item.MarketPrice = (decimal)row["MarketPrice"];
        //            }

        //            item.TaxRate = 0;
        //            if (row["TaxRate"] != DBNull.Value)
        //            {
        //                item.TaxRate = (decimal)row["TaxRate"];
        //            }

        //            item.Quantity = 0;
        //            if (row["Stock"] != DBNull.Value)
        //            {
        //                item.Quantity = (int)row["Stock"];
        //            }

        //            item.HasSku = false;
        //            if (row["fastbuy_skuid"] != DBNull.Value)
        //            {
        //                item.SkuId = (string)row["fastbuy_skuid"];
        //                item.HasSku = item.SkuId.Equals("");
        //            }

        //            if (item.HasSku)
        //            {
        //                item.SkuItems = this.GetSkuItems(item.ProductId);
        //                item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId));
        //            }

        //            item.ShortDescription = "";
        //            if (row["ShortDescription"] != DBNull.Value)
        //            {
        //                item.ShortDescription = (string)row["ShortDescription"];
        //            }

        //            item.IsCustomsClearance = false;
        //            if (row["IsCustomsClearance"] != DBNull.Value)
        //            {
        //                item.IsCustomsClearance = (bool)row["IsCustomsClearance"];
        //            }

        //            item.IsFreeShipping = false;
        //            if (row["IsfreeShipping"] != DBNull.Value)
        //            {
        //                item.IsCustomsClearance = (bool)row["IsfreeShipping"];
        //            }

        //            item.ShippingMode = "";
        //            if (row["ShippingMode"] != DBNull.Value)
        //            {
        //                item.ShippingMode = (string)row["ShippingMode"];
        //            }

        //            item.PromotionName = "";
        //            if (promotionInfo.ContainsKey(item.ProductId))
        //            {
        //                string promotionName = "";
        //                if (promotionInfo.TryGetValue(item.ProductId, out promotionName))
        //                {
        //                    item.PromotionName = promotionName;
        //                }
        //            }

        //            item.SaleCounts = 0;
        //            if (row["SaleCounts"] != DBNull.Value)
        //            {
        //                item.SaleCounts = (int)row["SaleCounts"];
        //            }

        //            item.VistiCounts = 0;
        //            if (row["VistiCounts"] != DBNull.Value)
        //            {
        //                item.SaleCounts = (int)row["VistiCounts"];
        //            }

        //            products.Add(item);
        //        }
        //    }

        //    ListResult<ProductListItem> data = new ListResult<ProductListItem>();
        //    data.TotalNumOfRecords = total;
        //    data.Results = products;

        //    return base.JsonActionResult(new StandardResult<ListResult<ProductListItem>>()
        //    {
        //        code = 0,
        //        msg = "",
        //        data = data
        //    });
        //}
        #endregion

        public IHttpActionResult List(JObject request)
        {

            Logger.WriterLogger("Product.List, Params: " + request.ToString(), LoggerType.Info);

            ParamProductList param = new ParamProductList();

            try
            {
                param = request.ToObject<ParamProductList>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            string userId = param.UserId;
            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            #region 验证参数

            Regex regex = new Regex(@"\d");

            if (!string.IsNullOrWhiteSpace(param.CategoryIds))
            {
                var ids = param.CategoryIds.Split(',');

                foreach (var item in ids)
                {
                    if (!regex.IsMatch(item))
                    {
                        return base.JsonActionResult(new StandardResult<string>() { code = 1, msg = "参数异常", data = null });
                    }
                };
            }

            if (!string.IsNullOrWhiteSpace(param.BrandIds))
            {
                var ids = param.BrandIds.Split(',');

                foreach (var item in ids)
                {
                    if (!regex.IsMatch(item))
                    {
                        return base.JsonActionResult(new StandardResult<string>() { code = 1, msg = "参数异常", data = null });
                    }
                };
            }


            if (!string.IsNullOrWhiteSpace(param.ImportsourceIds))
            {
                var ids = param.ImportsourceIds.Split(',');

                foreach (var item in ids)
                {
                    if (!regex.IsMatch(item))
                    {
                        return base.JsonActionResult(new StandardResult<string>() { code = 1, msg = "参数异常", data = null });
                    }
                };
            }

            #endregion

            string sortBy = GetSortTypeDesc(param.sortType);


            if (string.IsNullOrWhiteSpace(param.sortDirection))
            {
                param.sortDirection = "desc";
            }


            ProductBrowseQuery query = new ProductBrowseQuery();

            query.SortBy = sortBy;

            if (param.sortDirection.ToLower() == "asc")
            {
                query.SortOrder = SortAction.Asc;
            }
            else
            {
                query.SortOrder = SortAction.Desc;
            }


            string topicBannerImg = "";
            string topicDesc = "";

            int? topid = null;
            if (param.TopicId > 0)
            {
                topid = param.TopicId;

                // 取专题Banner和描述

                if (param.pageIndex == 1)
                {
                    TopicInfo topicInfo = VshopBrowser.GetTopic(param.TopicId);

                    if (topicInfo != null)
                    {
                        topicBannerImg = Util.AppendImageHost(topicInfo.IconUrl);
                        //topicDesc = 

                        var topicRegex = new Regex(@"""/Storage/master");
                        topicDesc = topicRegex.Replace(topicInfo.Content, "\"" + base.STORAGE_HOST + @"/Storage/master");
                    }
                }
            }

            query.StrCategoryId = param.CategoryIds;

            if (topid.HasValue)
            {
                // query.TopId = topid.Value;    //Modified by Paul Shieh 特殊处理TopicId错传问题

                // 等正常后取消
                if (topid.Value < 8000000)
                {
                    query.TopId = topid.Value;
                }
                else
                {
                    if (param.CategoryIds.Equals("0"))
                    {
                        query.StrCategoryId = topid.Value.ToString();
                    }
                }

                // 结束
            }

            query.StrBrandId = param.BrandIds;

            query.StrImportsourceId = param.ImportsourceIds;

            if (param.SupplierId > 0)
            {
                query.supplierid = param.SupplierId;
            }

            query.PageIndex = param.pageIndex;

           
            query.PageSize = param.pageSize;


            if (!string.IsNullOrWhiteSpace(param.keyword))
            {
                query.Keywords = param.keyword;
            }

            DbQueryResult dr=new DbQueryResult();
            DataTable dt=new DataTable();

            //IOS端的商家商品列表没做分页，故作此处理
            if (param.platform == 2 && param.SupplierId > 0 && Util.ConvertVer(ver) <= 121)
            {
                dt = ProductBrowser.GetAllProductList(query);
            }
            else
            {
                dr = ProductBrowser.GetCurrBrowseProductList(query);
                dt=(DataTable)dr.Data;
            }

            List<ProductListItem> products = new List<ProductListItem>();

            List<int> productIds = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                int productId = 0;
                if (row["ProductId"] != DBNull.Value)
                {
                    productId = (int)row["ProductId"];

                    if (productId > 0)
                    {
                        productIds.Add(productId);
                    }
                }
            }

            Member member = GetMember(userId.ToSeesionId());

            if (member != null && !string.IsNullOrWhiteSpace(param.keyword))
            {
                HistorySearchHelp.NewSearchHistory(param.keyword.Trim(), member.UserId, Entities.ClientType.App);
            }


            Dictionary<int, string> promotionInfo = new Dictionary<int, string>();

            if (productIds.Count > 0)
            {
                if (member != null)
                {
                    promotionInfo = ProductBrowser.GetProductPromotionInfo(member, productIds);
                }
                else
                {
                    promotionInfo = ProductBrowser.GetAllProductPromotionInfo(productIds);
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                ProductListItem item = new ProductListItem();
                // bool bol;
                //int cid = 0;
                //if (row["CategoryId"] != DBNull.Value)
                //{
                //    cid = (int)row["CategoryId"];
                //}

                item.ProductId = 0;
                if (row["ProductId"] != DBNull.Value)
                {
                    item.ProductId = (int)row["ProductId"];
                }

                item.CategoryId = 0;
                if (row["CategoryId"] != DBNull.Value)
                {
                    item.CategoryId = (int)row["CategoryId"];
                }

                item.Title = "";
                if (row["ProductName"] != DBNull.Value)
                {
                    item.Title = (string)row["ProductName"];
                }

                item.ImageUrl = "";
                if (row["ThumbnailUrl180"] != DBNull.Value)
                {
                    item.ImageUrl = Util.AppendImageHost((string)row["ThumbnailUrl180"]);
                }

                item.SalePrice = 0;
                if (row["SalePrice"] != DBNull.Value)
                {
                    item.SalePrice = (decimal)row["SalePrice"];
                }

                item.MarketPrice = 0;
                if (row["MarketPrice"] != DBNull.Value)
                {
                    item.MarketPrice = (decimal)row["MarketPrice"];
                }

                item.Quantity = 0;
                if (row["Stock"] != DBNull.Value)
                {
                    item.Quantity = (int)row["Stock"];
                }

                item.HasSku = false;
                if (row["fastbuy_skuid"] != DBNull.Value)
                {
                    item.SkuId = (string)row["fastbuy_skuid"];
                    item.HasSku = item.SkuId.Equals("");
                }

                int saletype = 1;
                if (row["SaleType"] != DBNull.Value)
                {
                    saletype = 2;
                }

                if (item.HasSku)
                {
                    item.SkuItems = this.GetSkuItems(item.ProductId);
                    //item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId));

                    item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId), saletype, item.TaxRate);
                }

                item.ShortDescription = "";
                if (row["ShortDescription"] != DBNull.Value)
                {
                    item.ShortDescription = (string)row["ShortDescription"];
                }

                item.IsCustomsClearance = false;
                if (row["IsCustomsClearance"] != DBNull.Value)
                {
                    item.IsCustomsClearance = (bool)row["IsCustomsClearance"];
                }

                item.IsFreeShipping = false;
                if (row["IsfreeShipping"] != DBNull.Value)
                {
                    item.IsFreeShipping = (bool)row["IsfreeShipping"];
                }

                item.ShippingMode = "";
                if (row["ShippingMode"] != DBNull.Value)
                {
                    item.ShippingMode = (string)row["ShippingMode"];
                }

                item.PromotionName = "";
                if (promotionInfo.ContainsKey(item.ProductId))
                {
                    string promotionName = "";
                    if (promotionInfo.TryGetValue(item.ProductId, out promotionName))
                    {
                        item.PromotionName = promotionName;
                    }
                }

                item.SaleCounts = 0;
                if (row["SaleCounts"] != DBNull.Value)
                {
                    item.SaleCounts = (int)row["SaleCounts"];
                }

                item.VistiCounts = 0;
                if (row["VistiCounts"] != DBNull.Value)
                {
                    item.VistiCounts = (int)row["VistiCounts"];
                }

                item.BuyCardinality = 1;
                if (row["BuyCardinality"] != DBNull.Value)
                {
                    item.BuyCardinality = (int)row["BuyCardinality"];
                }

                item.Icon = "";
                if (row["Icon"] != DBNull.Value)
                {
                    item.Icon = Util.AppendImageHost((string)row["Icon"]);
                }

                item.ShopName = "";
                if (row["ShopName"] != DBNull.Value)
                {
                    item.ShopName = (string)row["ShopName"];
                }

                bool IsDisplayDiscount = false;
                if (row["IsDisplayDiscount"] != DBNull.Value)
                {
                    IsDisplayDiscount = (bool)row["IsDisplayDiscount"];
                }

                item.Discount = "";
                if (IsDisplayDiscount && item.MarketPrice > 0)
                {
                    item.Discount = (item.SalePrice * 10 / item.MarketPrice).ToString("0.00") + "折";
                }

                decimal mintaxrate = 0M;
                if (row["MinTaxRate"] != System.DBNull.Value)
                {

                    mintaxrate = (decimal)row["MinTaxRate"];

                }

                decimal maxtaxrate = 0M;
                if (row["MaxTaxRate"] != System.DBNull.Value)
                {


                    maxtaxrate = (decimal)row["MaxTaxRate"];

                }


                item.TaxRate = 0;
                if (row["TaxRate"] != DBNull.Value)
                {
                    item.TaxRate = (decimal)row["TaxRate"];
                }




                item.ExtendTaxRate = item.GetExtendTaxRate(item.TaxRate, mintaxrate, maxtaxrate);



                products.Add(item);
            }

            ListResultWithActivity<ProductListItem> data = new ListResultWithActivity<ProductListItem>();

            //IOS端的商家商品列表没做分页，故作此处理
            if (param.platform == 2 && param.SupplierId > 0 && Util.ConvertVer(ver) <= 121)
            {
                data.TotalNumOfRecords = products.Count;
            }
            else
            {
                data.TotalNumOfRecords = dr.TotalRecords;
            }
            
            data.Results = products;
            data.ActivityBannerImg = topicBannerImg;
            data.ActivityDesc = topicDesc;

            return base.JsonActionResult(new StandardResult<ListResultWithActivity<ProductListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        public IHttpActionResult AllCategoryiesList(JObject request)
        {

            Logger.WriterLogger("Product.List, Params: " + request.ToString(), LoggerType.Info);

            ParamProductList param = new ParamProductList();

            try
            {
                param = request.ToObject<ParamProductList>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            string userId = param.UserId;
            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            #region 验证参数

            Regex regex = new Regex(@"\d");

            if (!string.IsNullOrWhiteSpace(param.CategoryIds))
            {
                var ids = param.CategoryIds.Split(',');

                foreach (var item in ids)
                {
                    if (!regex.IsMatch(item))
                    {
                        return base.JsonActionResult(new StandardResult<string>() { code = 1, msg = "参数异常", data = null });
                    }
                };
            }

            if (!string.IsNullOrWhiteSpace(param.BrandIds))
            {
                var ids = param.BrandIds.Split(',');

                foreach (var item in ids)
                {
                    if (!regex.IsMatch(item))
                    {
                        return base.JsonActionResult(new StandardResult<string>() { code = 1, msg = "参数异常", data = null });
                    }
                };
            }


            if (!string.IsNullOrWhiteSpace(param.ImportsourceIds))
            {
                var ids = param.ImportsourceIds.Split(',');

                foreach (var item in ids)
                {
                    if (!regex.IsMatch(item))
                    {
                        return base.JsonActionResult(new StandardResult<string>() { code = 1, msg = "参数异常", data = null });
                    }
                };
            }

            #endregion

            string sortBy = GetSortTypeDesc(param.sortType);


            if (string.IsNullOrWhiteSpace(param.sortDirection))
            {
                param.sortDirection = "desc";
            }


            ProductBrowseQuery query = new ProductBrowseQuery();

            query.SortBy = sortBy;

            if (param.sortDirection.ToLower() == "asc")
            {
                query.SortOrder = SortAction.Asc;
            }
            else
            {
                query.SortOrder = SortAction.Desc;
            }


            string topicBannerImg = "";
            string topicDesc = "";

            int? topid = null;
            if (param.TopicId > 0)
            {
                topid = param.TopicId;

                // 取专题Banner和描述

                if (param.pageIndex == 1)
                {
                    TopicInfo topicInfo = VshopBrowser.GetTopic(param.TopicId);

                    if (topicInfo != null)
                    {
                        topicBannerImg = Util.AppendImageHost(topicInfo.IconUrl);
                        //topicDesc = 

                        var topicRegex = new Regex(@"""/Storage/master");
                        topicDesc = topicRegex.Replace(topicInfo.Content, "\"" + base.STORAGE_HOST + @"/Storage/master");
                    }
                }
            }

            query.StrCategoryId = param.CategoryIds;

            if (topid.HasValue)
            {
                // query.TopId = topid.Value;    //Modified by Paul Shieh 特殊处理TopicId错传问题

                // 等正常后取消
                if (topid.Value < 8000000)
                {
                    query.TopId = topid.Value;
                }
                else
                {
                    if (param.CategoryIds.Equals("0"))
                    {
                        query.StrCategoryId = topid.Value.ToString();
                    }
                }

                // 结束
            }

            query.StrBrandId = param.BrandIds;

            query.StrImportsourceId = param.ImportsourceIds;

            if (param.SupplierId > 0)
            {
                query.supplierid = param.SupplierId;
            }
            //query.PageIndex = param.pageIndex;
            //query.PageSize = param.pageSize;
            if (!string.IsNullOrWhiteSpace(param.keyword))
            {
                query.Keywords = param.keyword;
            }
            DataTable dt = new DataTable();
            dt = ProductBrowser.GetAllProductList(query);
            List<ProductListItem> products = new List<ProductListItem>();
            List<int> productIds = new List<int>();
            foreach (DataRow row in dt.Rows)
            {
                int productId = 0;
                if (row["ProductId"] != DBNull.Value)
                {
                    productId = (int)row["ProductId"];

                    if (productId > 0)
                    {
                        productIds.Add(productId);
                    }
                }
            }

            Member member = GetMember(userId.ToSeesionId());

            if (member != null && !string.IsNullOrWhiteSpace(param.keyword))
            {
                HistorySearchHelp.NewSearchHistory(param.keyword.Trim(), member.UserId, Entities.ClientType.App);
            }


            Dictionary<int, string> promotionInfo = new Dictionary<int, string>();

            if (productIds.Count > 0)
            {
                if (member != null)
                {
                    promotionInfo = ProductBrowser.GetProductPromotionInfo(member, productIds);
                }
                else
                {
                    promotionInfo = ProductBrowser.GetAllProductPromotionInfo(productIds);
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                ProductListItem item = new ProductListItem();
                // bool bol;
                //int cid = 0;
                //if (row["CategoryId"] != DBNull.Value)
                //{
                //    cid = (int)row["CategoryId"];
                //}

                item.ProductId = 0;
                if (row["ProductId"] != DBNull.Value)
                {
                    item.ProductId = (int)row["ProductId"];
                }

                item.CategoryId = 0;
                if (row["CategoryId"] != DBNull.Value)
                {
                    item.CategoryId = (int)row["CategoryId"];
                }

                item.Title = "";
                if (row["ProductName"] != DBNull.Value)
                {
                    item.Title = (string)row["ProductName"];
                }

                item.ImageUrl = "";
                if (row["ThumbnailUrl180"] != DBNull.Value)
                {
                    item.ImageUrl = Util.AppendImageHost((string)row["ThumbnailUrl180"]);
                }

                item.SalePrice = 0;
                if (row["SalePrice"] != DBNull.Value)
                {
                    item.SalePrice = (decimal)row["SalePrice"];
                }

                item.MarketPrice = 0;
                if (row["MarketPrice"] != DBNull.Value)
                {
                    item.MarketPrice = (decimal)row["MarketPrice"];
                }

                item.Quantity = 0;
                if (row["Stock"] != DBNull.Value)
                {
                    item.Quantity = (int)row["Stock"];
                }

                item.HasSku = false;
                if (row["fastbuy_skuid"] != DBNull.Value)
                {
                    item.SkuId = (string)row["fastbuy_skuid"];
                    item.HasSku = item.SkuId.Equals("");
                }

                int saletype = 1;
                if (row["SaleType"] != DBNull.Value)
                {
                    saletype = 2;
                }

                if (item.HasSku)
                {
                    item.SkuItems = this.GetSkuItems(item.ProductId);
                    //item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId));

                    item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId), saletype, item.TaxRate);
                }

                item.ShortDescription = "";
                if (row["ShortDescription"] != DBNull.Value)
                {
                    item.ShortDescription = (string)row["ShortDescription"];
                }

                item.IsCustomsClearance = false;
                if (row["IsCustomsClearance"] != DBNull.Value)
                {
                    item.IsCustomsClearance = (bool)row["IsCustomsClearance"];
                }

                item.IsFreeShipping = false;
                if (row["IsfreeShipping"] != DBNull.Value)
                {
                    item.IsFreeShipping = (bool)row["IsfreeShipping"];
                }

                item.ShippingMode = "";
                if (row["ShippingMode"] != DBNull.Value)
                {
                    item.ShippingMode = (string)row["ShippingMode"];
                }

                item.PromotionName = "";
                if (promotionInfo.ContainsKey(item.ProductId))
                {
                    string promotionName = "";
                    if (promotionInfo.TryGetValue(item.ProductId, out promotionName))
                    {
                        item.PromotionName = promotionName;
                    }
                }

                item.SaleCounts = 0;
                if (row["SaleCounts"] != DBNull.Value)
                {
                    item.SaleCounts = (int)row["SaleCounts"];
                }

                item.VistiCounts = 0;
                if (row["VistiCounts"] != DBNull.Value)
                {
                    item.VistiCounts = (int)row["VistiCounts"];
                }

                item.BuyCardinality = 1;
                if (row["BuyCardinality"] != DBNull.Value)
                {
                    item.BuyCardinality = (int)row["BuyCardinality"];
                }

                item.Icon = "";
                if (row["Icon"] != DBNull.Value)
                {
                    item.Icon = Util.AppendImageHost((string)row["Icon"]);
                }

                item.ShopName = "";
                if (row["ShopName"] != DBNull.Value)
                {
                    item.ShopName = (string)row["ShopName"];
                }

                bool IsDisplayDiscount = false;
                if (row["IsDisplayDiscount"] != DBNull.Value)
                {
                    IsDisplayDiscount = (bool)row["IsDisplayDiscount"];
                }

                item.Discount = "";
                if (IsDisplayDiscount && item.MarketPrice > 0)
                {
                    item.Discount = (item.SalePrice * 10 / item.MarketPrice).ToString("0.00") + "折";
                }
                decimal mintaxrate = 0M;
                if (row["MinTaxRate"] != System.DBNull.Value)
                {
                    mintaxrate = (decimal)row["MinTaxRate"];
                }
                decimal maxtaxrate = 0M;
                if (row["MaxTaxRate"] != System.DBNull.Value)
                {
                    maxtaxrate = (decimal)row["MaxTaxRate"];
                }
                item.TaxRate = 0;
                if (row["TaxRate"] != DBNull.Value)
                {
                    item.TaxRate = (decimal)row["TaxRate"];
                }
                item.ExtendTaxRate = item.GetExtendTaxRate(item.TaxRate, mintaxrate, maxtaxrate);
                products.Add(item);
            }

            ListResultWithActivity<ProductListItem> data = new ListResultWithActivity<ProductListItem>();
            data.TotalNumOfRecords = products.Count;
            data.Results = products;
            data.ActivityBannerImg = topicBannerImg;
            data.ActivityDesc = topicDesc;

            return base.JsonActionResult(new StandardResult<ListResultWithActivity<ProductListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        public IHttpActionResult AllList(JObject request)
        {
            Logger.WriterLogger("Product.AllList, Params: " + request.ToString(), LoggerType.Info);
            ParamProductList param = new ParamProductList();
            try
            {
                param = request.ToObject<ParamProductList>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }
            string accessToken = param.accessToken;
            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            string userId = param.UserId;
            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Regex regex = new Regex(@"\d");

            if (!string.IsNullOrWhiteSpace(param.CategoryIds))
            {
                if (!regex.IsMatch(param.CategoryIds))
                {
                    return base.JsonActionResult(new StandardResult<string>() { code = 1, msg = "参数异常", data = null });
                }
            }
            string sortBy = GetSortTypeDesc(param.sortType);
            if (string.IsNullOrWhiteSpace(param.sortDirection))
            {
                param.sortDirection = "desc";
            }
            ProductBrowseQuery query = new ProductBrowseQuery();
            query.SortBy = sortBy;
            if (param.sortDirection.ToLower() == "asc")
            {
                query.SortOrder = SortAction.Asc;
            }
            else
            {
                query.SortOrder = SortAction.Desc;
            }
            query.StrCategoryId = param.CategoryIds;
            query.PageIndex = param.pageIndex;
            query.PageSize = param.pageSize;

            DbQueryResult dr = new DbQueryResult();
            DataTable dt = new DataTable();
            dr = ProductBrowser.GetCurrBrowseProductList(query);
            dt = (DataTable)dr.Data;
            List<ProductListItem> products = new List<ProductListItem>();
            Member member = GetMember(userId.ToSeesionId());
            #region == productlistitem
            foreach (DataRow row in dt.Rows)
            {
                ProductListItem item = new ProductListItem();
                item.ProductId = 0;
                if (row["ProductId"] != DBNull.Value)
                {
                    item.ProductId = (int)row["ProductId"];
                }
                item.BrandId = "0";
                if (row["BrandId"] != DBNull.Value)
                {
                    item.BrandId = row["BrandId"].ToString();
                }
                item.CategoryId = 0;
                if (row["CategoryId"] != DBNull.Value)
                {
                    item.CategoryId = (int)row["CategoryId"];
                }
                item.Title = "";
                if (row["ProductName"] != DBNull.Value)
                {
                    item.Title = (string)row["ProductName"];
                }
                item.ImageUrl = "";
                if (row["ThumbnailUrl180"] != DBNull.Value)
                {
                    item.ImageUrl = Util.AppendImageHost((string)row["ThumbnailUrl180"]);
                }
                item.SalePrice = 0;
                if (row["SalePrice"] != DBNull.Value)
                {
                    item.SalePrice = (decimal)row["SalePrice"];
                }

                item.MarketPrice = 0;
                if (row["MarketPrice"] != DBNull.Value)
                {
                    item.MarketPrice = (decimal)row["MarketPrice"];
                }
                item.Quantity = 0;
                if (row["Stock"] != DBNull.Value)
                {
                    item.Quantity = (int)row["Stock"];
                }
                item.HasSku = false;
                if (row["fastbuy_skuid"] != DBNull.Value)
                {
                    item.SkuId = (string)row["fastbuy_skuid"];
                    item.HasSku = item.SkuId.Equals("");
                }
                int saletype = 1;
                if (row["SaleType"] != DBNull.Value)
                {
                    saletype = 2;
                }
                if (item.HasSku)
                {
                    item.SkuItems = this.GetSkuItems(item.ProductId);
                    //item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId));
                    item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId), saletype, item.TaxRate);
                }
                item.ShortDescription = "";
                if (row["ShortDescription"] != DBNull.Value)
                {
                    item.ShortDescription = (string)row["ShortDescription"];
                }
                item.IsCustomsClearance = false;
                if (row["IsCustomsClearance"] != DBNull.Value)
                {
                    item.IsCustomsClearance = (bool)row["IsCustomsClearance"];
                }
                item.IsFreeShipping = false;
                if (row["IsfreeShipping"] != DBNull.Value)
                {
                    item.IsFreeShipping = (bool)row["IsfreeShipping"];
                }
                item.ShippingMode = "";
                if (row["ShippingMode"] != DBNull.Value)
                {
                    item.ShippingMode = (string)row["ShippingMode"];
                }
                item.SaleCounts = 0;
                if (row["SaleCounts"] != DBNull.Value)
                {
                    item.SaleCounts = (int)row["SaleCounts"];
                }
                item.VistiCounts = 0;
                if (row["VistiCounts"] != DBNull.Value)
                {
                    item.VistiCounts = (int)row["VistiCounts"];
                }
                item.BuyCardinality = 1;
                if (row["BuyCardinality"] != DBNull.Value)
                {
                    item.BuyCardinality = (int)row["BuyCardinality"];
                }
                item.Icon = "";
                if (row["Icon"] != DBNull.Value)
                {
                    item.Icon = Util.AppendImageHost((string)row["Icon"]);
                }
                item.ShopName = "";
                if (row["ShopName"] != DBNull.Value)
                {
                    item.ShopName = (string)row["ShopName"];
                }
                bool IsDisplayDiscount = false;
                if (row["IsDisplayDiscount"] != DBNull.Value)
                {
                    IsDisplayDiscount = (bool)row["IsDisplayDiscount"];
                }
                item.Discount = "";
                if (IsDisplayDiscount && item.MarketPrice > 0)
                {
                    item.Discount = (item.SalePrice * 10 / item.MarketPrice).ToString("0.00") + "折";
                }
                decimal mintaxrate = 0M;
                if (row["MinTaxRate"] != System.DBNull.Value)
                {
                    mintaxrate = (decimal)row["MinTaxRate"];
                }
                decimal maxtaxrate = 0M;
                if (row["MaxTaxRate"] != System.DBNull.Value)
                {
                    maxtaxrate = (decimal)row["MaxTaxRate"];
                }
                item.TaxRate = 0;
                if (row["TaxRate"] != DBNull.Value)
                {
                    item.TaxRate = (decimal)row["TaxRate"];
                }
                item.ExtendTaxRate = item.GetExtendTaxRate(item.TaxRate, mintaxrate, maxtaxrate);
                products.Add(item);
            }
            #endregion
            ListResultWithActivity<ProductListItem> data = new ListResultWithActivity<ProductListItem>();
            data.TotalNumOfRecords = dr.TotalRecords;
            data.Results = products;
            data.NextCategoryItem = this.GetSecondCategoryByCategoryId(Convert.ToInt32(param.CategoryIds));
            return base.JsonActionResult(new StandardResult<ListResultWithActivity<ProductListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        public IHttpActionResult SecondItemList(JObject request)
        {
            Logger.WriterLogger("Product.SecondItemList, Params: " + request.ToString(), LoggerType.Info);
            ParamProductList param = new ParamProductList();
            try
            {
                param = request.ToObject<ParamProductList>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }
            string accessToken = param.accessToken;
            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            string userId = param.UserId;
            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Regex regex = new Regex(@"\d");

            if (!string.IsNullOrWhiteSpace(param.CategoryIds))
            {
                if (!regex.IsMatch(param.CategoryIds))
                {
                    return base.JsonActionResult(new StandardResult<string>() { code = 1, msg = "参数异常", data = null });
                }
            }
            string sortBy = GetSortTypeDesc(param.sortType);
            if (string.IsNullOrWhiteSpace(param.sortDirection))
            {
                param.sortDirection = "desc";
            }
            ProductBrowseQuery query = new ProductBrowseQuery();
            query.SortBy = sortBy;
            if (param.sortDirection.ToLower() == "asc")
            {
                query.SortOrder = SortAction.Asc;
            }
            else
            {
                query.SortOrder = SortAction.Desc;
            }
            query.StrCategoryId = param.CategoryIds;
            DataTable dt = new DataTable();
            dt = ProductBrowser.GetAllProductList(query);
            List<ProductListItem> products = new List<ProductListItem>();
            #region == productlistitem
            foreach (DataRow row in dt.Rows)
            {
                ProductListItem item = new ProductListItem();
                item.ProductId = 0;
                if (row["ProductId"] != DBNull.Value)
                {
                    item.ProductId = (int)row["ProductId"];
                }
                item.BrandId = "0";
                if (row["BrandId"] != DBNull.Value)
                {
                    item.BrandId = row["BrandId"].ToString();
                }
                item.CategoryId = 0;
                if (row["CategoryId"] != DBNull.Value)
                {
                    item.CategoryId = (int)row["CategoryId"];
                }
                item.Title = "";
                if (row["ProductName"] != DBNull.Value)
                {
                    item.Title = (string)row["ProductName"];
                }
                item.ImageUrl = "";
                if (row["ThumbnailUrl180"] != DBNull.Value)
                {
                    item.ImageUrl = Util.AppendImageHost((string)row["ThumbnailUrl180"]);
                }
                item.SalePrice = 0;
                if (row["SalePrice"] != DBNull.Value)
                {
                    item.SalePrice = (decimal)row["SalePrice"];
                }

                item.MarketPrice = 0;
                if (row["MarketPrice"] != DBNull.Value)
                {
                    item.MarketPrice = (decimal)row["MarketPrice"];
                }
                item.Quantity = 0;
                if (row["Stock"] != DBNull.Value)
                {
                    item.Quantity = (int)row["Stock"];
                }
                item.HasSku = false;
                if (row["fastbuy_skuid"] != DBNull.Value)
                {
                    item.SkuId = (string)row["fastbuy_skuid"];
                    item.HasSku = item.SkuId.Equals("");
                }
                int saletype = 1;
                if (row["SaleType"] != DBNull.Value)
                {
                    saletype = 2;
                }
                if (item.HasSku)
                {
                    item.SkuItems = this.GetSkuItems(item.ProductId);
                    //item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId));
                    item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId), saletype, item.TaxRate);
                }
                item.ShortDescription = "";
                if (row["ShortDescription"] != DBNull.Value)
                {
                    item.ShortDescription = (string)row["ShortDescription"];
                }
                item.IsCustomsClearance = false;
                if (row["IsCustomsClearance"] != DBNull.Value)
                {
                    item.IsCustomsClearance = (bool)row["IsCustomsClearance"];
                }
                item.IsFreeShipping = false;
                if (row["IsfreeShipping"] != DBNull.Value)
                {
                    item.IsFreeShipping = (bool)row["IsfreeShipping"];
                }
                item.ShippingMode = "";
                if (row["ShippingMode"] != DBNull.Value)
                {
                    item.ShippingMode = (string)row["ShippingMode"];
                }
                item.SaleCounts = 0;
                if (row["SaleCounts"] != DBNull.Value)
                {
                    item.SaleCounts = (int)row["SaleCounts"];
                }
                item.VistiCounts = 0;
                if (row["VistiCounts"] != DBNull.Value)
                {
                    item.VistiCounts = (int)row["VistiCounts"];
                }
                item.BuyCardinality = 1;
                if (row["BuyCardinality"] != DBNull.Value)
                {
                    item.BuyCardinality = (int)row["BuyCardinality"];
                }
                item.Icon = "";
                if (row["Icon"] != DBNull.Value)
                {
                    item.Icon = Util.AppendImageHost((string)row["Icon"]);
                }
                item.ShopName = "";
                if (row["ShopName"] != DBNull.Value)
                {
                    item.ShopName = (string)row["ShopName"];
                }
                bool IsDisplayDiscount = false;
                if (row["IsDisplayDiscount"] != DBNull.Value)
                {
                    IsDisplayDiscount = (bool)row["IsDisplayDiscount"];
                }
                item.Discount = "";
                if (IsDisplayDiscount && item.MarketPrice > 0)
                {
                    item.Discount = (item.SalePrice * 10 / item.MarketPrice).ToString("0.00") + "折";
                }
                decimal mintaxrate = 0M;
                if (row["MinTaxRate"] != System.DBNull.Value)
                {
                    mintaxrate = (decimal)row["MinTaxRate"];
                }
                decimal maxtaxrate = 0M;
                if (row["MaxTaxRate"] != System.DBNull.Value)
                {
                    maxtaxrate = (decimal)row["MaxTaxRate"];
                }
                item.TaxRate = 0;
                if (row["TaxRate"] != DBNull.Value)
                {
                    item.TaxRate = (decimal)row["TaxRate"];
                }
                item.ExtendTaxRate = item.GetExtendTaxRate(item.TaxRate, mintaxrate, maxtaxrate);
                products.Add(item);
            }
            #endregion
            ListResultWithActivity<ProductListItem> data = new ListResultWithActivity<ProductListItem>();
            data.TotalNumOfRecords = products.Count;
            data.Results = products;
            // 根据categoryId 判断其类型CategoryType
            string categorytype = CategoryBrowser.GetCategoryTypeByCategoryId(Convert.ToInt32(param.CategoryIds));
            int curr = Convert.ToInt32(categorytype);
            data.NextCategoryItem = new List<NextCategoryItem>();
            data.BrandListItem = new List<BrandListItems>();
            if ((CategoryType)curr == CategoryType.Category)
            {
                // 类别
                data.NextCategoryItem = this.GetThirdCategoryByCategoryId(Convert.ToInt32(param.CategoryIds));
            }
            else if ((CategoryType)curr == CategoryType.Brand)
            {
                // 品牌
                data.BrandListItem = this.GetBrandByCategoryId(Convert.ToInt32(param.CategoryIds));
            }
            
            return base.JsonActionResult(new StandardResult<ListResultWithActivity<ProductListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult Get(string userId, int productId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.Get, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&productId={5}", userId, accessToken, channel, platform, ver, productId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            string appId = "";
            int accessTookenCode = VerifyAccessToken(accessToken, out appId);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Get");
            }

            ProductItem data = null;

            bool isFavorite = false;

            Member member = GetMember(userId.ToSeesionId());

            int innerUserId = 0;
            string userName = "";
            PromotionInfo promotionInfo;

            List<OrderPromotionItem> orderpromotionlist = new List<OrderPromotionItem>();

            List<ProductPromotionItem> productpromotionlist = new List<ProductPromotionItem>();

            #region 是否收藏和促销信息处理
            if (member != null)
            {
                isFavorite = ProductBrowser.ExistsProduct(productId, member.UserId);

                promotionInfo = ProductBrowser.GetProductPromotionInfo(member, productId);

                DataTable dtorderpromotion = ProductBrowser.GetOrderPromotionInfo(member);


                if (dtorderpromotion != null)
                {
                    OrderPromotionItem item = null;
                    foreach (DataRow row in dtorderpromotion.Rows)
                    {

                        item = new OrderPromotionItem();

                        if (row["Name"] != DBNull.Value)
                        {
                            item.Name = (string)row["Name"];
                        }

                        if (row["PromoteType"] != DBNull.Value)
                        {
                            item.PromoteType = (int)row["PromoteType"];
                        }



                        orderpromotionlist.Add(item);
                    }
                }


                DataTable dtproductpromotion = ProductBrowser.GetProductPromotionList(member, productId);


                if (dtproductpromotion != null)
                {
                    ProductPromotionItem item = null;
                    foreach (DataRow row in dtproductpromotion.Rows)
                    {

                        item = new ProductPromotionItem();

                        if (row["Name"] != DBNull.Value)
                        {
                            item.Name = (string)row["Name"];
                        }

                        if (row["PromoteType"] != DBNull.Value)
                        {
                            item.PromoteType = (int)row["PromoteType"];
                        }



                        productpromotionlist.Add(item);
                    }
                }

                innerUserId = member.UserId;
                userName = member.Username;
            }
            else
            {
                userName = appId;

                promotionInfo = ProductBrowser.GetAllProductPromotionInfo(productId);


                DataTable dtorderpromotion = ProductBrowser.GetAllOrderPromotionInfo();


                if (dtorderpromotion != null)
                {
                    OrderPromotionItem item = null;
                    foreach (DataRow row in dtorderpromotion.Rows)
                    {

                        item = new OrderPromotionItem();

                        if (row["Name"] != DBNull.Value)
                        {
                            item.Name = (string)row["Name"];
                        }

                        if (row["PromoteType"] != DBNull.Value)
                        {
                            item.PromoteType = (int)row["PromoteType"];
                        }



                        orderpromotionlist.Add(item);
                    }
                }


                DataTable dtproductpromotion = ProductBrowser.GetAllProductPromotionList(productId);


                if (dtproductpromotion != null)
                {
                    ProductPromotionItem item = null;
                    foreach (DataRow row in dtproductpromotion.Rows)
                    {

                        item = new ProductPromotionItem();

                        if (row["Name"] != DBNull.Value)
                        {
                            item.Name = (string)row["Name"];
                        }

                        if (row["PromoteType"] != DBNull.Value)
                        {
                            item.PromoteType = (int)row["PromoteType"];
                        }



                        productpromotionlist.Add(item);
                    }
                }
            }
            #endregion

            UserbrowsehistoryInfo historyInfo = new UserbrowsehistoryInfo();
            historyInfo.UserName = userName;
            historyInfo.UserId = innerUserId;
            historyInfo.Url = "";
            historyInfo.UserIP = "";
            historyInfo.PlatType = (int)PayApplicationType.payOnApp;
            historyInfo.ProductId = productId;
            historyInfo.Ip = 0;
            historyInfo.Description = "";
            historyInfo.Sort = 0;

            UserbrowsehistoryHelper.SetUserBrowseHistory(historyInfo);

            ProductBrowseInfo info = new ProductBrowseInfo();
            info = ProductBrowser.GetProductBrowseInfo(productId, null, null);

            ProductInfo product = info.Product;
            //ProductInfo product = ProductBrowser.GetProductSimpleInfo(productId);
            if (product != null)
            {
                data = new ProductItem();

                if (product.BuyCardinality > 0)
                {
                    data.BuyCardinality = product.BuyCardinality;
                }
                else
                {
                    data.BuyCardinality = 1;
                }

                data.ProductId = product.ProductId;
                data.Title = product.ProductName;
                data.Code = product.ProductCode;
                data.SupplierId = product.SupplierId ?? 0;

                if (data.SupplierId > 0)
                {
                    SupplierInfo supplierInfo = SupplierHelper.GetSupplier(data.SupplierId);

                    if (supplierInfo != null)
                    {
                        data.SupplierName = supplierInfo.ShopName;
                        data.Logo = Util.AppendImageHost(supplierInfo.Logo);
                    }
                }
                //data.ImageUrls;
                List<ImageUrl> imageUrls = new List<ImageUrl>();
                AddImage(product.ImageUrl1, imageUrls);
                AddImage(product.ImageUrl2, imageUrls);
                AddImage(product.ImageUrl3, imageUrls);
                AddImage(product.ImageUrl4, imageUrls);
                AddImage(product.ImageUrl5, imageUrls);
                data.ImageUrls = imageUrls;
                data.SalePrice = product.MinSalePrice;
                data.MarketPrice = product.MarketPrice;

                data.Stock = product.Stock;
                data.HasSku = product.HasSKU;
                data.Description = product.ShortDescription;
                data.AttributeItems = this.GetAttributeItems(productId);

                data.SkuItems = this.GetSkuItems(productId);


                //data.Skus = this.GetSkus(product.Skus);
                //根据是否是组合商品获取SKUs

                data.Skus = this.GetSkus(product.Skus, product.SaleType, product.TaxRate);

                data.ShippingModeId = product.TemplateId ?? 0;
                data.ShippingModeName = "";
                if (data.ShippingModeId > 0)
                {
                    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(data.ShippingModeId);
                    if (shippingMode != null)
                    {
                        data.ShippingModeName = shippingMode.TemplateName;
                    }
                }
                data.IsFavorite = isFavorite;
                data.Url = string.Format(base.PRODUCT_SHARE_URL_BASE, product.ProductId);
                data.ThumbnailsUrl = Util.AppendImageHost(product.ThumbnailUrl180);
                data.IsCustomsClearance = product.IsCustomsClearance;

                data.PromotionName = "";
                if (promotionInfo != null)
                {
                    data.PromotionName = promotionInfo.Name;
                }
                data.SaleCounts = product.ShowSaleCounts;
                data.VistiCounts = product.VistiCounts;

                ImportSourceTypeInfo imSourceType = ProductBrowser.GetProductImportSourceType(product.ProductId);
                data.OriginalPlace = "";
                data.OriginalPlaceIcon = "";

                if (imSourceType != null)
                {
                    data.OriginalPlace = imSourceType.CnArea;
                    data.OriginalPlaceIcon = Util.AppendImageHost(imSourceType.Icon);
                }
                data.ProductCommentCount = info.ReviewCount;

                data.ProductSubTitle = product.ProductTitle;

                data.OrderPromotionList = orderpromotionlist;

                data.ProductPromotionList = productpromotionlist;

                data.Discount = "";
                if (product.IsDisplayDiscount && data.MarketPrice.HasValue && data.MarketPrice.Value > 0)
                {
                    data.Discount = (data.SalePrice * 10 / data.MarketPrice.Value).ToString("0.00") + "折";
                }

                data.IsPromotion = product.IsPromotion;

                data.TaxRate = product.TaxRate;

                data.ExtendTaxRate = product.GetExtendTaxRate();

                //计算税费
                if (product.SaleType == 2)
                {
                    data.Tax = ProductBrowser.GetTaxByProductId(product.ProductId);
                }

                else
                {
                    data.Tax = product.TaxRate * product.MinSalePrice;
                }

                if (product.SaleType == 2 && (product.SupplierId == null || product.SupplierId == 0))
                {
                    data.IsShowShop = false;
                }
                else
                {
                    data.IsShowShop = true;
                }


                return base.JsonActionResult(new StandardResult<ProductItem>()
                {
                    code = 0,
                    msg = "",
                    data = data
                });
            }

            else
            {
                return base.JsonActionResult(new StandardResult<string>()
                {
                    code = 1,
                    msg = "商品不存在",
                    data = null
                });
            }


        }

        [HttpGet]
        public IHttpActionResult GetDescription(int productId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.GetDescription, Params: " + string.Format("productId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", productId, accessToken, channel, platform, ver, productId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(-1, "", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.GetDescription");
            }

            string storageHost = base.STORAGE_HOST;

            ProductDescriptionItem data = new ProductDescriptionItem();

            bool isMobile = (platform == 2 || platform == 3);

            var regex = new Regex(@"""/Storage/master");

            string ext = "<meta name='viewport' content='width=device-width,target-densitydpi=high-dpi,initial-scale=0.5, minimum-scale=0.25, maximum-scale=1.0, user-scalable=no'/><p style=\"font-size:24px;width:100%;margin:0 auto\"><img src=\"" + base.HOST + "/templates/vshop/haimei/images/vproduct_msg.jpg\" style=\"font-size:24px;width:100%;\"/></p>";

            //如果不存在商品，则在限时抢购商品里取Description，为了解决app的bug而添加
            if (ProductBrowser.IsExistProduct(productId))
            {
                data.ProductId = productId;
                data.Description = ext + regex.Replace(ProductBrowser.GetProductDescription(productId, isMobile), "\"" + storageHost + @"/Storage/master");
            }

            else
            {
                data.ProductId = productId;
                data.Description = ext + regex.Replace(ProductBrowser.GetProductDescriptionByCountDownId(productId, isMobile), "\"" + storageHost + @"/Storage/master");
            }

            return base.JsonActionResult(new StandardResult<ProductDescriptionItem>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult Comments(int productId, int pageIndex, int pageSize, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.Comments, Params: " + string.Format("productId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}", productId, accessToken, channel, platform, ver, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(-1, "", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Comments");
            }

            ProductReviewQuery productReviewQuery = new ProductReviewQuery();

            productReviewQuery.productId = productId;
            productReviewQuery.IsCount = true;
            productReviewQuery.PageIndex = pageIndex;
            productReviewQuery.PageSize = pageSize;
            productReviewQuery.SortBy = "ReviewId";
            productReviewQuery.SortOrder = SortAction.Desc;

            DbQueryResult productReviews = ProductBrowser.GetProductReviews(productReviewQuery);

            DataTable dt = productReviews.Data as DataTable;

            List<ProductCommentListItem> items = new List<ProductCommentListItem>();

            int reviewCount = 0;
            int reviewScore = 0;

            ProductBrowser.GetProductReviewsSummary(productId, out reviewCount, out reviewScore);

            if (dt != null)
            {
                ProductCommentListItem item = null;

                foreach (DataRow current in dt.Rows)
                {
                    item = new ProductCommentListItem();

                    item.Id = 0;
                    if (current["ReviewId"] != DBNull.Value)
                    {
                        item.Id = (long)current["ReviewId"];
                    }

                    item.DisplayName = "";
                    if (current["UserName"] != DBNull.Value)
                    {
                        string displayName = (string)current["UserName"];

                        item.DisplayName = displayName.Substring(0, 1) + "*****" + displayName.Substring(displayName.Length - 1);
                    }

                    item.ReviewDate = "";
                    if (current["ReviewDate"] != DBNull.Value)
                    {
                        item.ReviewDate = (((DateTime)current["ReviewDate"])).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    item.Content = "";
                    if (current["ReviewText"] != DBNull.Value)
                    {
                        item.Content = (string)current["ReviewText"];
                    }

                    item.Score = 5;
                    if (current["Score"] != DBNull.Value)
                    {
                        item.Score = (int)current["Score"];
                    }

                    item.IsAnonymous = true;
                    if (current["IsAnonymous"] != DBNull.Value)
                    {
                        item.IsAnonymous = (bool)current["IsAnonymous"];
                    }

                    if (current["HeadImgUrl"] != DBNull.Value)
                    {
                        item.HeadImgUrl = Util.AppendImageHost((string)current["HeadImgUrl"]);
                    }

                    item.OrderDate = "";
                    if (current["OrderDate"] != DBNull.Value)
                    {
                        item.OrderDate = (((DateTime)current["OrderDate"])).ToString("yyyy-MM-dd HH:mm:ss");
                    }



                    items.Add(item);
                }
            }

            ProductCommentListResult data = new ProductCommentListResult();
            data.TotalScore = reviewScore;
            data.TotalNumOfRecords = productReviews.TotalRecords;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ProductCommentListResult>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult Questions(int productId, int pageIndex, int pageSize, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.Questions, Params: " + string.Format("productId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}", productId, accessToken, channel, platform, ver, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(-1, "", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Questions");
            }

            ProductConsultationAndReplyQuery productConsultationAndReplyQuery = new ProductConsultationAndReplyQuery();
            productConsultationAndReplyQuery.ProductId = productId;
            productConsultationAndReplyQuery.IsCount = true;
            productConsultationAndReplyQuery.PageIndex = pageIndex;
            productConsultationAndReplyQuery.PageSize = pageSize;
            productConsultationAndReplyQuery.SortBy = "ConsultationId";
            productConsultationAndReplyQuery.SortOrder = SortAction.Desc;
            //productConsultationAndReplyQuery.HasReplied = true;

            DbQueryResult productConsultations = ProductBrowser.GetProductConsultations(productConsultationAndReplyQuery);

            DataTable dt = productConsultations.Data as DataTable;

            List<ProductConsultationListItem> items = new List<ProductConsultationListItem>();

            if (dt != null)
            {
                ProductConsultationListItem item = null;

                foreach (DataRow current in dt.Rows)
                {
                    item = new ProductConsultationListItem();

                    item.Id = 0;
                    if (current["ConsultationId"] != DBNull.Value)
                    {
                        item.Id = (int)current["ConsultationId"];
                    }

                    item.QuestionType = 0;
                    //if (current["QuestionType"] != DBNull.Value)
                    //{
                    //    item.QuestionType = (int)current["QuestionType"];
                    //}

                    item.DisplayName = "";
                    if (current["UserName"] != DBNull.Value)
                    {
                        string displayName = (string)current["UserName"];

                        item.DisplayName = displayName.Substring(0, 1) + "*****" + displayName.Substring(displayName.Length - 1);
                    }

                    item.AskDate = "";
                    if (current["ConsultationDate"] != DBNull.Value)
                    {
                        item.AskDate = (((DateTime)current["ConsultationDate"])).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    item.Content = "";
                    if (current["ConsultationText"] != DBNull.Value)
                    {
                        item.Content = (string)current["ConsultationText"];
                    }

                    item.ReplyUsername = "海美客服";
                    //if (current["UserName"] != DBNull.Value)
                    //{
                    //    item.ReplyUsername = (string)current["UserName"];
                    //}

                    item.ReplyDate = "";
                    if (current["ReplyDate"] != DBNull.Value)
                    {
                        item.ReplyDate = (((DateTime)current["ReplyDate"])).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    item.ReplyContent = "";
                    if (current["ReplyText"] != DBNull.Value)
                    {
                        item.ReplyContent = (string)current["ReplyText"];
                    }

                    items.Add(item);
                }
            }

            ListResult<ProductConsultationListItem> data = new ListResult<ProductConsultationListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<ProductConsultationListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult CountDownCategory(string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.CountDownCategory, Params: " + string.Format("&accessToken={0}&channel={1}&platform={2}&ver={3}", accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo("", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.CountDownCategory");
            }

            int total = 0;
            List<CountDownCategoryListItem> categories = new List<CountDownCategoryListItem>();

            DbQueryResult dbQueryResult = new DbQueryResult();

            ////兼容android低版本
            //if (platform == 3 && Util.ConvertVer(ver) < 120)
            //{
            //    dbQueryResult = ProductBrowser.GetCountDownCategories(1, 5);

            //    if (dbQueryResult.TotalRecords > 5)
            //    {
            //        dbQueryResult.TotalRecords = 5;
            //    }
            //}

            //else
            //{
            //    dbQueryResult = ProductBrowser.GetCountDownCategories();
            //}

            dbQueryResult = ProductBrowser.GetCountDownCategories(1, 5);
            if (dbQueryResult.TotalRecords > 5)
            {
                dbQueryResult.TotalRecords = 5;
            }


            if (dbQueryResult != null)
            {
                total = dbQueryResult.TotalRecords;
                DataTable dt = (DataTable)dbQueryResult.Data;

                foreach (DataRow row in dt.Rows)
                {
                    CountDownCategoryListItem item = new CountDownCategoryListItem();

                    item.Id = 0;
                    if (row["CountDownCategoryId"] != DBNull.Value)
                    {
                        item.Id = (int)row["CountDownCategoryId"];
                    }

                    item.Title = "";
                    if (row["Title"] != DBNull.Value)
                    {
                        item.Title = (string)row["Title"];
                    }

                    item.AdImageUrl = "";
                    if (row["AdImageUrl"] != DBNull.Value)
                    {
                        item.AdImageUrl = Util.AppendImageHost((string)row["AdImageUrl"]);
                    }

                    item.AdImageLinkUrl = "";
                    if (row["AdImageLinkUrl"] != DBNull.Value)
                    {
                        item.AdImageLinkUrl = (string)row["AdImageLinkUrl"];
                    }

                    item.ProductCount = 0;
                    if (row["ProductCount"] != DBNull.Value)
                    {
                        item.ProductCount = (int)row["ProductCount"];
                    }

                    item.DisplaySequence = 0;
                    if (row["DisplaySequence"] != DBNull.Value)
                    {
                        item.DisplaySequence = (int)row["DisplaySequence"];
                    }

                    item.StartTime = "";
                    if (row["StartDate"] != DBNull.Value)
                    {
                        item.StartTime = ((DateTime)row["StartDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    item.EndTime = "";
                    if (row["EndDate"] != DBNull.Value)
                    {
                        item.EndTime = ((DateTime)row["EndDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    categories.Add(item);
                }
            }

            ListResult<CountDownCategoryListItem> data = new ListResult<CountDownCategoryListItem>();
            data.TotalNumOfRecords = total;
            data.Results = categories;

            return base.JsonActionResult(new StandardResult<ListResult<CountDownCategoryListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult CountDownList(string startTime, string endTime, int pageIndex, int pageSize, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.CountDownList, Params: " + string.Format("startTime={0:yyyy-MM-dd HH:mm:ss}&accessToken={1}&channel={2}&platform={3}&ver={4}&pageIndex={5}&pageSize={6}", startTime, accessToken, channel, platform, ver, pageIndex, pageSize), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo("", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.CountDownList");
            }


            string activityBannerImg = "";
            string activityDesc = "";


            // 取专题Banner和描述

            if (pageIndex == 1)
            {
                activityBannerImg = Util.AppendImageHost(@"");

                var topicRegex = new Regex(@"""/Storage/master");
                activityDesc = "";    // topicRegex.Replace(topicInfo.Content, "\"" + base.STORAGE_HOST + @"/Storage/master");
            }

            int total = 0;

            //如果当前时数小于传进来的参数，则天数+1
            DateTime start = DateTime.MinValue;
            if (!DateTime.TryParseExact(startTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out start))
            {
                return base.JsonFaultResult(new CommonException(40003).GetMessage(), "Product.CountDownList, startTime: " + startTime);
            }

            DateTime end = DateTime.MinValue;
            if (!DateTime.TryParseExact(endTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out end))
            {
                return base.JsonFaultResult(new CommonException(40003).GetMessage(), "Product.CountDownList, endTime: " + endTime);
            }

            pageIndex = 1;
            pageSize = int.MaxValue;

            DataTable dt = ProductBrowser.GetCountDownProductList(start, end, pageIndex, pageSize, out total);

            List<CountDownProductListItem> products = new List<CountDownProductListItem>();

            List<int> productIds = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                int productId = 0;
                if (row["ProductId"] != DBNull.Value)
                {
                    productId = (int)row["ProductId"];

                    if (productId > 0)
                    {
                        productIds.Add(productId);
                    }
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                CountDownProductListItem item = new CountDownProductListItem();

                item.ProductId = 0;
                if (row["ProductId"] != DBNull.Value)
                {
                    item.ProductId = (int)row["ProductId"];
                }

                item.CategoryId = 0;
                if (row["CategoryId"] != DBNull.Value)
                {
                    item.CategoryId = (int)row["CategoryId"];
                }

                item.Title = "";
                if (row["ProductName"] != DBNull.Value)
                {
                    item.Title = (string)row["ProductName"];
                }

                item.ImageUrl = "";
                if (row["ThumbnailUrl180"] != DBNull.Value)
                {
                    item.ImageUrl = Util.AppendImageHost((string)row["ThumbnailUrl180"]);
                }

                item.SalePrice = 0;
                if (row["SalePrice"] != DBNull.Value)
                {
                    item.SalePrice = (decimal)row["SalePrice"];
                }

                item.MarketPrice = 0;
                if (row["MarketPrice"] != DBNull.Value)
                {
                    item.MarketPrice = (decimal)row["MarketPrice"];
                }

                item.TaxRate = 0;
                if (row["TaxRate"] != DBNull.Value)
                {
                    item.TaxRate = (decimal)row["TaxRate"];
                }

                item.Quantity = 0;
                if (row["Stock"] != DBNull.Value)
                {
                    item.Quantity = (int)row["Stock"];
                }

                item.HasSku = false;
                //if (row["fastbuy_skuid"] != DBNull.Value)
                //{
                //    item.SkuId = (string)row["fastbuy_skuid"];
                //    item.HasSku = item.SkuId.Equals("");
                //}

                //if (item.HasSku)
                //{
                //    item.SkuItems = this.GetSkuItems(item.ProductId);
                //    item.Skus = this.GetSkus(ProductBrowser.GetProductSkus(item.ProductId));
                //}

                item.ShortDescription = "";
                if (row["ShortDescription"] != DBNull.Value)
                {
                    item.ShortDescription = (string)row["ShortDescription"];
                }

                //item.IsCustomsClearance = false;
                //if (row["IsCustomsClearance"] != DBNull.Value)
                //{
                //    item.IsCustomsClearance = (bool)row["IsCustomsClearance"];
                //}

                //item.IsFreeShipping = false;
                //if (row["IsfreeShipping"] != DBNull.Value)
                //{
                //    item.IsCustomsClearance = (bool)row["IsfreeShipping"];
                //}

                //item.ShippingMode = "";
                //if (row["ShippingMode"] != DBNull.Value)
                //{
                //    item.ShippingMode = (string)row["ShippingMode"];
                //}

                item.PromotionName = "";

                item.SaleCounts = 0;
                if (row["SaleCounts"] != DBNull.Value)
                {
                    item.SaleCounts = (int)row["SaleCounts"];
                }

                item.VistiCounts = 0;
                //if (row["VistiCounts"] != DBNull.Value)
                //{
                //    item.SaleCounts = (int)row["VistiCounts"];
                //}

                //item.BuyCardinality = 1;
                //if (row["BuyCardinality"] != DBNull.Value)
                //{
                //    item.BuyCardinality = (int)row["BuyCardinality"];
                //}


                // CountDown
                item.Id = 0;
                if (row["CountDownId"] != DBNull.Value)
                {
                    item.Id = (int)row["CountDownId"];
                }

                item.StartTime = "";
                if (row["StartDate"] != DBNull.Value)
                {
                    item.StartTime = ((DateTime)row["StartDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                }

                item.EndTime = "";
                if (row["EndDate"] != DBNull.Value)
                {
                    item.EndTime = ((DateTime)row["EndDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                }

                item.CountDownPrice = 0M;
                if (row["CountDownPrice"] != DBNull.Value)
                {
                    item.CountDownPrice = (decimal)row["CountDownPrice"];
                }

                item.ActivityMaxCount = 0;
                if (row["MaxCount"] != DBNull.Value)
                {
                    item.ActivityMaxCount = (int)row["MaxCount"];
                }

                item.ActivityPlanCount = 0;
                if (row["PlanCount"] != DBNull.Value)
                {
                    item.ActivityPlanCount = (int)row["PlanCount"];
                }

                item.ActivitySaleCount = 0;
                //if (row["SaleCount"] != DBNull.Value)
                //{
                //    item.ActivitySaleCount = (int)row["SaleCount"];
                //}
                item.ActivitySaleCount = ShoppingProcessor.AllCountDownOrderCount(item.ProductId, item.Id);
                item.EnableCoupon = false;
                item.EnableVoucher = false;
                item.EnableScore = false;

                products.Add(item);
            }

            ListResultWithActivity<CountDownProductListItem> data = new ListResultWithActivity<CountDownProductListItem>();
            data.TotalNumOfRecords = total;
            data.Results = products;
            data.ActivityBannerImg = activityBannerImg;
            data.ActivityDesc = activityDesc;

            return base.JsonActionResult(new StandardResult<ListResultWithActivity<CountDownProductListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult CountDownProduct(string userId, int id, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.CountDownProduct, Params: " + string.Format("userId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&id={5}", userId, accessToken, channel, platform, ver, id), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            string appId = "";
            int accessTookenCode = VerifyAccessToken(accessToken, out appId);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.CountDownProduct");
            }

            CountDownInfo countdownInfo = ProductBrowser.GetCountDownInfoByCountDownId(id);

            if (countdownInfo == null)
            {
                return base.JsonActionResult(new StandardResult<string>()
                {
                    code = 1,
                    msg = "商品不存在",
                    data = null
                });
            }

            int productId = countdownInfo.ProductId;

            CountDownProductItem data = null;

            bool isFavorite = false;

            Member member = GetMember(userId.ToSeesionId());

            int innerUserId = 0;
            string userName = "";

            #region 商品订单优惠信息
            //PromotionInfo promotionInfo;

            List<OrderPromotionItem> orderpromotionlist = new List<OrderPromotionItem>();

            if (member != null)
            {
                isFavorite = ProductBrowser.ExistsProduct(productId, member.UserId);

                //promotionInfo = ProductBrowser.GetProductPromotionInfo(member, productId);


                DataTable dtorderpromotion = ProductBrowser.GetOrderPromotionInfo(member);
                DataRow[] rows = dtorderpromotion.Select("PromoteType=17");
                if (rows.Length > 0)
                {
                    OrderPromotionItem item = null;
                    foreach (DataRow row in rows)
                    {

                        item = new OrderPromotionItem();

                        if (row["Name"] != DBNull.Value)
                        {
                            item.Name = (string)row["Name"];
                        }

                        if (row["PromoteType"] != DBNull.Value)
                        {
                            item.PromoteType = (int)row["PromoteType"];
                        }



                        orderpromotionlist.Add(item);
                    }
                }
                innerUserId = member.UserId;
                userName = member.Username;
            }
            else
            {
                userName = appId;
                //promotionInfo = ProductBrowser.GetAllProductPromotionInfo(productId);
                DataTable dtorderpromotion = ProductBrowser.GetAllOrderPromotionInfo();

                DataRow[] rows = dtorderpromotion.Select("PromoteType=17");
                if (rows.Length > 0)
                {
                    OrderPromotionItem item = null;
                    foreach (DataRow row in rows)
                    {

                        item = new OrderPromotionItem();

                        if (row["Name"] != DBNull.Value)
                        {
                            item.Name = (string)row["Name"];
                        }

                        if (row["PromoteType"] != DBNull.Value)
                        {
                            item.PromoteType = (int)row["PromoteType"];
                        }
                        orderpromotionlist.Add(item);
                    }
                }
            }

            #endregion

            UserbrowsehistoryInfo historyInfo = new UserbrowsehistoryInfo();
            historyInfo.UserName = userName;
            historyInfo.UserId = innerUserId;
            historyInfo.Url = "";
            historyInfo.UserIP = "";
            historyInfo.PlatType = (int)PayApplicationType.payOnApp;
            historyInfo.ProductId = productId;
            historyInfo.Ip = 0;
            historyInfo.Description = "";
            historyInfo.Sort = 0;

            UserbrowsehistoryHelper.SetUserBrowseHistory(historyInfo);

            ProductBrowseInfo info = new ProductBrowseInfo();
            info = ProductBrowser.GetProductBrowseInfo(productId, null, null);

            ProductInfo product = info.Product;
            //ProductInfo product = ProductBrowser.GetProductSimpleInfo(productId);
            if (product != null)
            {
                data = new CountDownProductItem();

                data.Id = countdownInfo.CountDownId;
                data.StartTime = countdownInfo.StartDate.ToString("yyyy-MM-dd HH:mm:ss");
                data.EndTime = countdownInfo.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
                data.CountDownPrice = countdownInfo.CountDownPrice;
                data.ActivityMaxCount = countdownInfo.MaxCount;
                data.ActivityPlanCount = countdownInfo.PlanCount;
                data.ActivitySaleCount = ShoppingProcessor.AllCountDownOrderCount(productId, id);//ProductBrowser.GetCountDownSaleCountByCountDownId(id); 
                data.EnableCoupon = false;
                data.EnableVoucher = false;
                data.EnableScore = false;


                data.ProductId = product.ProductId;
                data.Title = product.ProductName;
                data.Code = product.ProductCode;
                data.SupplierId = product.SupplierId ?? 0;

                if (data.SupplierId > 0)
                {
                    SupplierInfo supplierInfo = SupplierHelper.GetSupplier(data.SupplierId);

                    if (supplierInfo != null)
                    {
                        data.SupplierName = supplierInfo.SupplierName;
                        data.Logo = Util.AppendImageHost(supplierInfo.Logo);
                    }
                }
                //data.ImageUrls;
                List<ImageUrl> imageUrls = new List<ImageUrl>();
                AddImage(product.ImageUrl1, imageUrls);
                AddImage(product.ImageUrl2, imageUrls);
                AddImage(product.ImageUrl3, imageUrls);
                AddImage(product.ImageUrl4, imageUrls);
                AddImage(product.ImageUrl5, imageUrls);
                data.ImageUrls = imageUrls;
                data.SalePrice = product.MinSalePrice;
                data.MarketPrice = product.MarketPrice;
                data.TaxRate = product.TaxRate;
                data.Stock = product.Stock;
                data.HasSku = product.HasSKU;
                data.Description = product.ShortDescription;
                data.AttributeItems = this.GetAttributeItems(productId);

                data.SkuItems = this.GetSkuItems(productId);
                data.Skus = this.GetSkus(product.Skus);

                data.ShippingModeId = product.TemplateId ?? 0;
                data.ShippingModeName = "";
                if (data.ShippingModeId > 0)
                {
                    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(data.ShippingModeId);
                    if (shippingMode != null)
                    {
                        data.ShippingModeName = shippingMode.TemplateName;
                    }
                }
                data.IsFavorite = isFavorite;
                data.Url = string.Format(base.PRODUCT_SHARE_URL_BASE, product.ProductId);
                data.ThumbnailsUrl = Util.AppendImageHost(product.ThumbnailUrl180);
                data.IsCustomsClearance = product.IsCustomsClearance;

                data.PromotionName = "";
                //if (promotionInfo != null)
                //{
                //    data.PromotionName = promotionInfo.Name;
                //}

                data.SaleCounts = product.ShowSaleCounts;
                data.VistiCounts = product.VistiCounts;

                ImportSourceTypeInfo imSourceType = ProductBrowser.GetProductImportSourceType(product.ProductId);
                data.OriginalPlace = "";
                data.OriginalPlaceIcon = "";

                if (imSourceType != null)
                {
                    data.OriginalPlace = imSourceType.CnArea;
                    data.OriginalPlaceIcon = Util.AppendImageHost(imSourceType.Icon);
                }
                data.ProductCommentCount = info.ReviewCount;

                data.ExtendTaxRate = product.GetExtendTaxRate();

                //计算税费

                data.Tax = product.TaxRate * countdownInfo.CountDownPrice;

                data.ProductSubTitle = product.ProductTitle;

                data.OrderPromotionList = orderpromotionlist;

                if (product.SaleType == 2 && (product.SupplierId == null || product.SupplierId == 0))
                {
                    data.IsShowShop = false;
                }
                else
                {
                    data.IsShowShop = true;
                }

                return base.JsonActionResult(new StandardResult<CountDownProductItem>()
                {
                    code = 0,
                    msg = "",
                    data = data
                });
            }

            else
            {
                return base.JsonActionResult(new StandardResult<string>()
                {
                    code = 1,
                    msg = "商品不存在",
                    data = null
                });
            }


        }

        [HttpPost]
        public IHttpActionResult Comment(JObject request)
        {
            Logger.WriterLogger("Product.Comment, Params: " + request.ToString(), LoggerType.Info);

            ParamComment param = new ParamComment();

            try
            {
                Logger.WriterLogger("Product.Comment, Step 1", LoggerType.Info);

                param = request.ToObject<ParamComment>();
            }
            catch
            {
                Logger.WriterLogger("Product.Comment, Step 1-1", LoggerType.Info);
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            Logger.WriterLogger("Product.Comment, Step 2", LoggerType.Info);
            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            Logger.WriterLogger("Product.Comment, Step 3", LoggerType.Info);

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            string orderId = param.OrderId;
            string skuId = param.SkuId;
            int productId = param.ProductId;
            string content = param.Content;
            int score = param.Score;
            bool isAnonymous = param.IsAnonymous;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Logger.WriterLogger("Product.Comment, Step 4", LoggerType.Info);

            Member member = GetMember(userId.ToSeesionId());

            Logger.WriterLogger("Product.Comment, Step 5", LoggerType.Info);

            if (member != null)
            {
                try
                {
                    Logger.WriterLogger("Product.Comment, Step 6", LoggerType.Info);

                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
                    if (orderInfo.OrderStatus != OrderStatus.Finished)
                    {
                        return base.JsonActionResult(new StandardResult<string>()
                       {
                           code = 1,
                           msg = "您的订单还未完成，因此不能对该商品进行评论",
                           data = ""
                       });
                    }

                    Logger.WriterLogger("Product.Comment, Step 7", LoggerType.Info);

                    int num;
                    int num2;
                    ProductBrowser.LoadProductReview(member.UserId, productId, out num, out num2, orderId);
                    if (num == 0)
                    {
                        return base.JsonActionResult(new StandardResult<string>()
                        {
                            code = 1,
                            msg = "您没有购买此商品(或此商品的订单尚未完成)，因此不能进行评论",
                            data = ""
                        });

                    }

                    Logger.WriterLogger("Product.Comment, Step 8", LoggerType.Info);

                    if (num2 >= num)
                    {
                        return base.JsonActionResult(new StandardResult<string>()
                        {
                            code = 1,
                            msg = "您已经对此商品进行过评论(或此商品的订单尚未完成)，因此不能再次进行评论",
                            data = ""
                        });
                    }

                    Logger.WriterLogger("Product.Comment, Step 9", LoggerType.Info);

                    if (ProductBrowser.InsertProductReview(new ProductReviewInfo
                    {
                        ReviewDate = DateTime.Now,
                        ReviewText = content,
                        OrderID = orderId,
                        SkuId = skuId,
                        ProductId = productId,
                        Score = score,
                        UserEmail = string.IsNullOrEmpty(member.Email) ? "" : member.Email,
                        UserId = member.UserId,
                        UserName = string.IsNullOrEmpty(member.Username) ? "" : member.Username,
                        IsAnonymous = isAnonymous
                    }))
                    {
                        StandardResult<string> result = new StandardResult<string>()
                        {
                            code = 0,
                            msg = "操作成功",
                            data = ""
                        };

                        return base.JsonActionResult(result);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriterLogger("Product.Comment, Message: " + ex.Message, LoggerType.Info);
                }
            }
            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            return base.JsonFaultResult(new FaultInfo(40999, ""), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult Question(JObject request)
        {
            Logger.WriterLogger("Product.Question, Params: " + request.ToString(), LoggerType.Info);

            ParamQuestion param = new ParamQuestion();

            try
            {
                param = request.ToObject<ParamQuestion>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            int productId = param.ProductId;
            string content = param.Content;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                if (ProductBrowser.InsertProductConsultation(new ProductConsultationInfo
                {
                    ConsultationDate = DateTime.Now,
                    ConsultationText = content,
                    ProductId = productId,
                    UserEmail = member.Email,
                    UserId = member.UserId,
                    UserName = member.Username
                }))
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 0,
                        msg = "添加成功",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }

            }
            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            return base.JsonFaultResult(new FaultInfo(40999, ""), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult AddToFavorite(JObject request)
        {
            Logger.WriterLogger("Product.AddToFavorite, Params: " + request.ToString(), LoggerType.Info);

            ParamFavorite param = new ParamFavorite();

            try
            {
                param = request.ToObject<ParamFavorite>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            // 验证参数
            //ThrowParamException(skuId);

            string userId = param.UserId;
            int productId = param.ProductId;
            string remark = param.Remark;
            string tag = param.Tag;

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                if (ProductBrowser.ExistsProduct(productId, member.UserId))
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 1,
                        msg = "不可重复收藏商品",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }
                if (ProductBrowser.CollectProduct(productId, member.UserId, tag, remark))
                {
                    StandardResult<string> result = new StandardResult<string>()
                    {
                        code = 0,
                        msg = "添加成功",
                        data = ""
                    };

                    return base.JsonActionResult(result);
                }

            }
            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            return base.JsonFaultResult(new FaultInfo(40999, ""), request.ToString());
        }

        [HttpPost]
        public IHttpActionResult RemoveFavorite(JObject request)
        {
            Logger.WriterLogger("Product.RemoveFavorite, Params: " + request.ToString(), LoggerType.Info);

            ParamRemoveFavorite param = new ParamRemoveFavorite();

            try
            {
                param = request.ToObject<ParamRemoveFavorite>();
            }
            catch
            {
                // 参数无效
                return base.JsonFaultResult(new CommonException(40100).GetMessage(), request.ToString());
            }

            string accessToken = param.accessToken;

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), request.ToString());
            }

            string userId = param.UserId;
            string productIds = param.ProductIds;

            // 验证参数
            //ThrowParamException(productIds);

            int channel = param.channel;
            int platform = param.platform;
            string ver = param.ver;

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                if (!string.IsNullOrEmpty(productIds))
                {
                    List<int> ids = new List<int>();

                    string[] tempIds = productIds.Split(new char[] { ',' });

                    foreach (string current in tempIds)
                    {
                        int id = 0;

                        if (int.TryParse(current, out id))
                        {
                            ids.Add(id);
                        }
                    }

                    if (ids.Count > 0)
                    {
                        try
                        {
                            ProductBrowser.DeleteFavorites(member.UserId, string.Join(",", ids));
                        }
                        catch
                        { }
                    }
                }

                StandardResult<string> result = new StandardResult<string>()
                {
                    code = 0,
                    msg = "取消收藏成功",
                    data = ""
                };

                return base.JsonActionResult(result);

            }
            else
            {
                return base.JsonFaultResult(new CommonException(40201).GetMessage(), request.ToString());
            }

            return base.JsonFaultResult(new FaultInfo(40999, ""), request.ToString());
        }

        [HttpGet]
        public IHttpActionResult MyKeywords(int siteId, string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.MyKeywords, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}&userId={5}", siteId, accessToken, channel, platform, ver, userId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Search");
            }

            List<KeywordListItem> items = new List<KeywordListItem>();

            Member member = GetMember(userId.ToSeesionId());

            if (member != null)
            {
                DataTable dt = HistorySearchHelp.GetSearchHistory(member.UserId, ClientType.App, 6);
                //DataTable dt = CommentBrowser.GetMyHotKeywords(member.UserId, 8, true);

                if (dt != null)
                {
                    KeywordListItem item = null;

                    foreach (DataRow current in dt.Rows)
                    {
                        item = new KeywordListItem();

                        item.CategoryId = 0;
                        //if (current["CategoryId"] != DBNull.Value)
                        //{
                        //    item.CategoryId = (int)current["CategoryId"];
                        //}

                        item.Keyword = "";
                        if (current["Keywords"] != DBNull.Value)
                        {
                            item.Keyword = (string)current["Keywords"];
                        }

                        items.Add(item);
                    }
                }
            }

            ListResult<KeywordListItem> data = new ListResult<KeywordListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<KeywordListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult Keywords(int siteId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Product.Keywords, Params: " + string.Format("siteId={0}&accessToken={1}&channel={2}&platform={3}&ver={4}", siteId, accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Keywords");
            }

            DataTable dt = CommentBrowser.GetHotKeywords((int)ClientType.App, 8, true);

            List<KeywordListItem> items = new List<KeywordListItem>();

            if (dt != null)
            {
                KeywordListItem item = null;

                foreach (DataRow current in dt.Rows)
                {
                    item = new KeywordListItem();

                    item.CategoryId = 0;
                    if (current["CategoryId"] != DBNull.Value)
                    {
                        item.CategoryId = (int)current["CategoryId"];
                    }

                    item.Keyword = "";
                    if (current["Keywords"] != DBNull.Value)
                    {
                        item.Keyword = (string)current["Keywords"];
                    }

                    items.Add(item);
                }
            }

            ListResult<KeywordListItem> data = new ListResult<KeywordListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<KeywordListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult ProductFilter(string accessToken, int channel, int platform, string ver, int categoryId)
        {
            Logger.WriterLogger("Product.ProductFilter, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}&categoryId={4}", accessToken, channel, platform, ver, categoryId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo("", channel, platform, ver);

            // 验证令牌
            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.ProductFilter");
            }

            ProductFilterResult result = new ProductFilterResult();
            result.code = 0;
            result.msg = "";
            result.data = new ProductFilterData();

            //品牌
            result.data.Brand = this.GetBrand(categoryId);

            // 商品种类
            result.data.Category = this.GetCategory(categoryId);

            // 原产地
            result.data.OriginalPlace = this.GetOriginPlace(categoryId);


            return base.JsonActionResult(result);
        }

        #region Private
        private ListResult<CategoryListItem> GetCategory(int categoryId)
        {
            int parentCategoryId = categoryId;

            List<CategoryInfo> categories = null;

            if (parentCategoryId == 0)
            {
                categories = CategoryBrowser.GetMainCategories().ToList();
            }
            else
            {
                categories = CategoryBrowser.GetSubCategories(parentCategoryId).ToList();
            }

            List<CategoryListItem> items = new List<CategoryListItem>();

            CategoryListItem item = null;

            foreach (CategoryInfo current in categories)
            {
                if (current.CategoryId != 8000044 && !current.Path.Contains("8000044") && (current.ParentCategoryId == null ? 0 : current.ParentCategoryId) != 8000044)
                {
                    item = new CategoryListItem();

                    item.CategoryId = current.CategoryId;
                    item.CategoryName = current.Name;
                    item.ImageUrl = current.IconUrl;
                    item.Path = current.Path;
                    item.ParentCategoryId = current.ParentCategoryId;
                    item.DisplaySequence = current.DisplaySequence;
                    item.Children = null;

                    items.Add(item);
                }
            }

            ListResult<CategoryListItem> data = new ListResult<CategoryListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            return data;
        }

        private ListResult<BrandListItem> GetBrand(int categoryId)
        {
            DataTable brands = null;

            if (categoryId == 0)
            {
                brands = CatalogHelper.GetBrandCategories();
            }
            else
            {
                brands = CatalogHelper.GetBrandCategories(categoryId);
            }

            List<BrandListItem> items = new List<BrandListItem>();

            if (brands != null)
            {
                BrandListItem item = null;

                foreach (DataRow current in brands.Rows)
                {
                    item = new BrandListItem();

                    item.BrandId = 0;
                    if (current["BrandId"] != DBNull.Value)
                    {
                        item.BrandId = (int)current["BrandId"];
                    }

                    item.BrandName = "";
                    if (current["BrandName"] != DBNull.Value)
                    {
                        item.BrandName = (string)current["BrandName"];
                    }

                    item.Icon = "";
                    if (current["Logo"] != DBNull.Value)
                    {
                        item.Icon = Util.AppendImageHost(((string)current["Logo"]));
                    }

                    item.Description = "";
                    if (current["Description"] != DBNull.Value)
                    {
                        item.Description = (string)current["Description"];
                    }

                    item.DisplaySequence = 0;
                    if (current["DisplaySequence"] != DBNull.Value)
                    {
                        item.DisplaySequence = (int)current["DisplaySequence"];
                    }

                    items.Add(item);
                }
            }

            ListResult<BrandListItem> data = new ListResult<BrandListItem>();
            data.TotalNumOfRecords = items.Count;
            data.Results = items;

            return data;
        }


        private ListResult<OriginPlaceListItem> GetOriginPlace(int categoryId)
        {
            List<ImportSourceTypeInfo> categories = null;

            if (categoryId == 0)
            {
                categories = ImportSourceTypeHelper.GetAllImportSourceTypes().ToList();
            }
            else
            {
                categories = ImportSourceTypeHelper.GetAllImportSourceTypes(categoryId).ToList();
            }

            List<OriginPlaceListItem> items = new List<OriginPlaceListItem>();

            if (categories != null)
            {
                OriginPlaceListItem item = null;

                foreach (ImportSourceTypeInfo current in categories)
                {
                    item = new OriginPlaceListItem();

                    item.PlaceId = current.ImportSourceId;
                    item.PlaceName = current.CnArea;
                    item.Icon = Util.AppendImageHost(current.Icon);
                    item.DisplaySequence = current.DisplaySequence ?? 0;

                    items.Add(item);
                }
            }

            ListResult<OriginPlaceListItem> data = new ListResult<OriginPlaceListItem>();
            data.TotalNumOfRecords = items.Count;
            data.Results = items;

            return data;
        }



        private string GetSortTypeDesc(int sortType)
        {
            //sortType取值：DisplaySequence\SalePrice\ShowSaleCounts\AddedDate，sortAction取值：asc\desc
            string sortTypeDesc = "DisplaySequence";

            switch (sortType)
            {
                case 1:
                    sortTypeDesc = "DisplaySequence";
                    break;
                case 2:
                    sortTypeDesc = "SalePrice";
                    break;
                case 3:
                    sortTypeDesc = "ShowSaleCounts";
                    break;
                case 4:
                    sortTypeDesc = "AddedDate";
                    break;
                default:
                    sortTypeDesc = "DisplaySequence";
                    break;
            }

            return sortTypeDesc;

        }
        private List<CategoryListItem> GetSubCategories(int parentCategoryId)
        {
            List<CategoryListItem> items = new List<CategoryListItem>();

            IList<CategoryInfo> subCategories = CategoryBrowser.GetSubCategories(parentCategoryId);

            if (subCategories != null && subCategories.Count > 0)
            {
                CategoryListItem item = null;

                foreach (CategoryInfo current in subCategories)
                {
                    item = new CategoryListItem();

                    item.CategoryId = current.CategoryId;
                    item.CategoryName = current.Name;
                    item.ImageUrl = Util.AppendImageHost(current.Icon);
                    item.Path = current.Path;
                    item.ParentCategoryId = current.ParentCategoryId;
                    item.DisplaySequence = current.DisplaySequence;

                    item.Children = GetSubCategories(current.CategoryId);

                    items.Add(item);
                }
            }

            return items;
        }

        private List<NextCategoryItem> GetSecondCategoryByCategoryId(int CategoryId)
        {
            List<NextCategoryItem> items = new List<NextCategoryItem>();
            DataTable subCategories = CategoryBrowser.GetSecondCategoryByCategoryId(CategoryId);
            if (subCategories != null && subCategories.Rows.Count > 0)
            {
                NextCategoryItem item = new NextCategoryItem();
                item.CategoryId = subCategories.Rows[0]["ParentCategoryId"] == null ? 0 : Convert.ToInt32(subCategories.Rows[0]["ParentCategoryId"].ToString());
                item.CategoryName = subCategories.Rows[0]["ParentCategoryName"].ToString();
                List<NextCategory> nextitems = new List<NextCategory>();
                foreach (DataRow row in subCategories.Rows)
                {
                    NextCategory nextitem = new NextCategory();
                    nextitem.CategoryId = row["CategoryId"] == null ? 0 : Convert.ToInt32(row["CategoryId"].ToString());
                    nextitem.CategoryName = row["Name"].ToString();
                    nextitems.Add(nextitem);
                }
                item.NextCategory = nextitems;
                items.Add(item);
            }

            return items;
        }

        private List<NextCategoryItem> GetThirdCategoryByCategoryId(int CategoryId)
        {
            List<NextCategoryItem> items = new List<NextCategoryItem>();
            DataTable subCategories = CategoryBrowser.GetThirdCategoryByCategoryId(CategoryId);
            if (subCategories != null && subCategories.Rows.Count > 0)
            {
                NextCategoryItem item = new NextCategoryItem();
                item.CategoryId = subCategories.Rows[0]["ParentCategoryId"] == null ? 0 : Convert.ToInt32(subCategories.Rows[0]["ParentCategoryId"].ToString());
                item.CategoryName = subCategories.Rows[0]["ParentCategoryName"].ToString();
                List<NextCategory> nextitems = new List<NextCategory>();
                foreach (DataRow row in subCategories.Rows)
                {
                    NextCategory nextitem = new NextCategory();
                    nextitem.CategoryId = row["CategoryId"] == null ? 0 : Convert.ToInt32(row["CategoryId"].ToString());
                    nextitem.CategoryName = row["Name"].ToString();
                    nextitems.Add(nextitem);
                }
                item.NextCategory = nextitems;
                items.Add(item);
            }

            return items;
        }

        private List<BrandListItems> GetBrandByCategoryId(int CategoryId)
        {
            List<BrandListItems> items = new List<BrandListItems>();
            DataTable brands = CategoryBrowser.GetBrandsByCategoryId(CategoryId);
            if (brands != null && brands.Rows.Count > 0)
            {
                foreach (DataRow row in brands.Rows)
                {
                    BrandListItems brand = new BrandListItems();
                    brand.BrandId = row["BrandId"].ToString();
                    brand.BrandName = row["BrandName"].ToString();
                    items.Add(brand);
                }
            }
            return items;
        }

        private void AddImage(string imageUrl, List<ImageUrl> imageUrls)
        {
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                if (imageUrl.StartsWith("http://"))
                {
                    imageUrls.Add(new ImageUrl(imageUrl));
                }
                else
                {
                    string storageHost = System.Configuration.ConfigurationManager.AppSettings["STORAGE_HOST"].ToString();
                    imageUrls.Add(new ImageUrl(storageHost + (imageUrl.StartsWith("/") ? "" : "/") + imageUrl));
                }
            }
        }

        private List<AttributeItem> GetAttributeItems(int productId)
        {
            List<AttributeItem> items = new List<AttributeItem>();

            DataTable dt = ProductBrowser.GetExpandAttributes(productId);

            foreach (DataRow current in dt.Rows)
            {
                //a.AttributeId, AttributeName, ValueStr
                string attributeName = "";
                if (current["AttributeName"] != DBNull.Value)
                {
                    attributeName = (string)current["AttributeName"];
                }

                string attributeValue = "";
                if (current["ValueStr"] != DBNull.Value)
                {
                    attributeValue = (string)current["ValueStr"];
                }

                items.Add(new AttributeItem(attributeName, attributeValue));
            }

            return items;
        }

        private List<SkuItem> GetSkuItems(int productId)
        {
            string storageHost = System.Configuration.ConfigurationManager.AppSettings["STORAGE_HOST"].ToString();

            List<SkuItem> items = new List<SkuItem>();

            DataTable dt = ProductBrowser.GetUniqueSkus(productId);

            int attributeId = 0;

            SkuItem item = null;

            foreach (DataRow current in dt.Rows)
            {
                //SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl

                int currentAttributeId = 0;
                if (current["AttributeId"] != DBNull.Value)
                {
                    currentAttributeId = (int)current["AttributeId"];
                }

                if (currentAttributeId != attributeId)
                {
                    //if (attributeId != 0)
                    //{
                    //    items.Add(item);
                    //}

                    attributeId = currentAttributeId;

                    string attributeName = "";
                    if (current["AttributeName"] != DBNull.Value)
                    {
                        attributeName = (string)current["AttributeName"];
                    }

                    bool isUseImage = false;
                    if (current["UseAttributeImage"] != DBNull.Value)
                    {
                        isUseImage = (bool)current["UseAttributeImage"];
                    }

                    item = new SkuItem(attributeId, attributeName, isUseImage);
                    items.Add(item);
                }

                int valueId = 0;
                if (current["ValueId"] != DBNull.Value)
                {
                    valueId = (int)current["ValueId"];
                }

                string value = "";
                if (current["ValueStr"] != DBNull.Value)
                {
                    value = (string)current["ValueStr"];
                }

                string imageUrl = "";
                if (current["ImageUrl"] != DBNull.Value)
                {
                    imageUrl = (string)current["ImageUrl"];

                    if (!imageUrl.StartsWith("http://"))
                    {
                        imageUrl = storageHost + (imageUrl.StartsWith("/") ? "" : "/") + imageUrl;
                    }
                }

                item.Items.Add(new SkuItemOption(valueId, value, imageUrl));
            }

            return items;
        }

        private List<Sku> GetSkus(Dictionary<string, SKUItem> Skus)
        {
            List<Sku> items = new List<Sku>();

            Sku item = null;

            foreach (var sku in Skus)
            {
                string skuId = sku.Key;
                SKUItem skuItem = sku.Value;

                item = new Sku(skuId, skuItem.SKU, skuItem.Weight, 0M, skuItem.SalePrice, skuItem.Stock);

                foreach (var current in skuItem.SkuItems)
                {
                    int attribyteId = current.Key;
                    int valueId = current.Value;

                    item.SkuItems.Add(new SkuValue(attribyteId, valueId));
                }

                items.Add(item);
            }

            return items;
        }


        private List<Sku> GetSkus(Dictionary<string, SKUItem> Skus, int saletype, decimal taxrate)
        {
            List<Sku> items = new List<Sku>();

            Sku item = null;

            foreach (var sku in Skus)
            {
                string skuId = sku.Key;
                SKUItem skuItem = sku.Value;

                List<ProductsCombination> list = new List<ProductsCombination>();
                if (saletype == 2)
                {
                    list = ProductHelper.GetProductsCombinationsBySku(skuId);
                }

                //计算税费
                decimal tax = 0;
                if (list.Count > 0)
                {
                    foreach (ProductsCombination pcitem in list)
                    {
                        tax += pcitem.Price * pcitem.Quantity * pcitem.TaxRate;
                    }
                }
                else
                {
                    tax = skuItem.SalePrice * taxrate;
                }

                //item = new Sku(skuId, skuItem.SKU, skuItem.Weight, 0M, skuItem.SalePrice, skuItem.Stock);

                item = new Sku(skuId, skuItem.SKU, skuItem.Weight, 0M, skuItem.SalePrice, skuItem.Stock, tax);

                foreach (var current in skuItem.SkuItems)
                {
                    int attribyteId = current.Key;
                    int valueId = current.Value;

                    item.SkuItems.Add(new SkuValue(attribyteId, valueId));
                }

                items.Add(item);
            }

            return items;
        }

        #endregion
    }
}

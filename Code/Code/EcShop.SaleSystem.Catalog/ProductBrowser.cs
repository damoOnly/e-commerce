using Commodities;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Comments;
using EcShop.Entities.Commodities;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using EcShop.SqlDal.Comments;
using EcShop.SqlDal.Commodities;
using EcShop.SqlDal.Members;
using EcShop.SqlDal.Orders;
using EcShop.SqlDal.Promotions;
using System;
using System.Collections.Generic;
using System.Data;
namespace EcShop.SaleSystem.Catalog
{
    public static class ProductBrowser
    {
        public static DataTable GetSaleProductRanking(int? categoryId, int maxNum)
        {
            return new ProductBrowseDao().GetSaleProductRanking(categoryId, maxNum);
        }
        public static DataTable GetSubjectList(SubjectListQuery query)
        {
            return new ProductBrowseDao().GetSubjectList(query);
        }
        public static ProductInfo GetProductSimpleInfo(int productId)
        {
            return new ProductDao().GetProductDetails(productId);
        }
        public static Dictionary<string, SKUItem> GetProductSkus(int productId)
        {
            return new ProductDao().GetProductSkus(productId);
        }
         /// <summary>
        /// 获取活动商品
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="CateGoryId"></param>
        /// <param name="maxReviewNum"></param>
        /// <param name="maxConsultationNum"></param>
        /// <returns></returns>
        public static ProductBrowseInfo GetProductBrowseInfoByActive(int productId, int CateGoryId, int? maxReviewNum, int? maxConsultationNum)
        {
            return new ProductBrowseDao().GetProductBrowseInfoByActive(productId, CateGoryId, maxReviewNum, maxConsultationNum);
        }
         /// <summary>
        ///检查活动是否运行中
        /// </summary>
        /// <returns></returns>
        public static bool CheckActiveIsRunding(int CateGoryId, int productId)
        {
            return new ProductBrowseDao().CheckActiveIsRunding(CateGoryId, productId); 
        }
        public static ProductBrowseInfo GetProductBrowseInfo(int productId, int? maxReviewNum, int? maxConsultationNum)
        {
            return new ProductBrowseDao().GetProductBrowseInfo(productId, maxReviewNum, maxConsultationNum);
        }
        public static DbQueryResult GetShareProducts(int shareId, ProductBrowseQuery query)
        {
            return new ProductBrowseDao().GetShareProducts(shareId, query);
        }
        public static DbQueryResult GetBrowseProductList(ProductBrowseQuery query)
        {
            return new ProductBrowseDao().GetBrowseProductList(query);
        }

        public static DbQueryResult GetWMSBrowseProductList(ProductBrowseQuery query)
        {
            return new ProductBrowseDao().GetWMSBrowseProductList(query);
        }

        public static DataTable GetProductInfoByPId(string productId)
        {
            return new ProductBrowseDao().GetProductInfoByPId(productId);
        }


        public static DbQueryResult GetCurrBrowseProductList(ProductBrowseQuery query)
        {
            return new ProductBrowseDao().GetCurrBrowseProductList(query);
        }

        /// <summary>
        /// 根据筛选条件获取所有的商品，不做分页
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataTable GetAllProductList(ProductBrowseQuery query)
        {
            return new ProductBrowseDao().GetAllProductList(query);
        }
        public static ProductBrowseInfo GetBrowseHotProductList(int categoryId, int brandId)
        {
            try
            {
                return new ProductBrowseDao().GetBrowseHotProductList(categoryId, brandId);
            }
            catch (Exception)
            {

            }
            return new ProductBrowseInfo();

        }


        /// <summary>
        /// 门店商品销量排行
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="brandId"></param>
        /// <param name="supplierid"></param>
        /// <returns></returns>
        public static ProductBrowseInfo GetBrowseHotProductList(int categoryId, int brandId, int supplierid)
        {
            try
            {
                return new ProductBrowseDao().GetBrowseHotProductList(categoryId, brandId, supplierid);
            }
            catch (Exception)
            {

            }
            return new ProductBrowseInfo();

        }
        /// <summary>
        /// 查询搜索集合中的品牌，原产地
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataTable GetBrowseProductImportSourceIdAndBrandIdList(ProductBrowseQuery query)
        {
            return new ProductBrowseDao().GetBrowseProductImportSourceIdAndBrandIdList(query);
        }
        public static DbQueryResult GetUnSaleProductList(ProductBrowseQuery query)
        {
            query.ProductSaleStatus = ProductSaleStatus.UnSale;
            return new ProductBrowseDao().GetBrowseProductList(query);
        }

        public static DataTable GetVistiedProducts(IList<int> productIds)
        {
            return new ProductBrowseDao().GetVistiedProducts(productIds);
        }
        public static DataTable GetLimitProducts(IList<int> productIds)
        {
            return new ProductBrowseDao().GetLimitProducts(productIds);
        }

        public static DataTable GetSuggestProductsProducts(IList<int> productIds, int count)
        {
            return new ProductBrowseDao().GetSuggestProductsProducts(productIds, count);
        }
        /// <summary>
        /// 随机获取商品信息
        /// </summary>
        /// <param name="count">获取商品数量</param>
        /// <returns></returns>
        public static DataTable GetVistiedProducts(int count, int between, int and)
        {
            return new ProductBrowseDao().GetVistiedProducts(count, between, and);
        }
        /// <summary>
        /// 随机限时抢购的信息
        /// </summary>
        /// <param name="count"></param>
        /// <param name="between"></param>
        /// <param name="and"></param>
        /// <returns></returns>
        public static DataTable GetLimitProducts(int count, int between, int and)
        {
            return new ProductBrowseDao().GetLimitProducts(count, between, and);
        }
        public static DataTable GetSkus(int productId)
        {
            return new SkuDao().GetSkus(productId);
        }
        public static DataTable GetUniqueSkus(int productId)
        {
            return new SkuDao().GetUniqueSkus(productId);
        }
        public static DataTable GetExpandAttributes(int productId)
        {
            return new SkuDao().GetExpandAttributes(productId);
        }
        public static DataTable GetHomeProduct(ClientType clientType)
        {
            return new HomeProductDao().GetHomeProducts(clientType);
        }

        public static DataTable GetHomeProduct(ClientType clientType, bool isAnonymous)
        {
            return new HomeProductDao().GetHomeProducts(clientType, isAnonymous);
        }

        public static DataTable GetTopicProducts(int topicid, int maxNum)
        {
            return new ProductBrowseDao().GetTopicProducts(topicid, maxNum);
        }
        public static DataTable GetBrandProducts(int? brandId, int pageNumber, int maxNum, out int total)
        {
            return new ProductBrowseDao().GetBrandProducts(brandId, pageNumber, maxNum, out total);
        }
        public static DbQueryResult GetBrandProducts(int? brandId, ProductBrowseQuery query)//新增分页查询
        {
            return new ProductBrowseDao().GetBrandProducts(brandId, query);
        }
        public static bool CheckHasCollect(int memberId, int productId)
        {
            return new FavoriteDao().CheckHasCollect(memberId, productId);
        }
        public static int GetFavoriteId(int userId, int productId)
        {
            return new FavoriteDao().GetFavoriteId(userId, productId);
        }
        public static DataTable GetProducts(int? topicId, int? categoryId, string keyWord, int pageNumber, int maxNum, out int total, string sort, string order)
        {
            return new ProductBrowseDao().GetProducts(topicId, categoryId, keyWord, pageNumber, maxNum, out total, sort, order == "asc");
        }
        public static DataTable GetProducts(int? topicId, int? categoryId, int? brandId, int? originPlaceId, bool? isFreeShip, string keyword, int pageNumber, int maxNum, int? gradeId, out int total, string sort, string order)
        {
            return new ProductBrowseDao().GetProducts(topicId, categoryId, brandId, originPlaceId, isFreeShip, keyword, pageNumber, maxNum, gradeId, out total, sort, order == "asc");
        }
        public static DataTable GetProducts(int? topicId, int? categoryId, int? brandId, int? originPlaceId, int pageNumber, int maxNum, int? gradeId, out int total, string sort, string order)
        {
            return new ProductBrowseDao().GetProducts(topicId, categoryId, brandId, originPlaceId, false, "", pageNumber, maxNum, gradeId, out total, sort, order == "asc");
        }
        public static DataTable GetProducts(int? topicId, int? categoryId, string keyWord, int pageNumber, int maxNum, out int total, string sort, bool isAnonymous, string order)
        {
            return new ProductBrowseDao().GetProducts(topicId, categoryId, keyWord, pageNumber, maxNum, out total, sort, isAnonymous, order == "asc");
        }

        public static DataTable GetProducts(int? topicId, int? categoryId, int? supplierId, string keyWord, int pageNumber, int maxNum, out int total, string sort, bool isAnonymous, string order,bool issupplier)
        {
            return new ProductBrowseDao().GetProducts(topicId, categoryId, supplierId, keyWord, pageNumber, maxNum, out total, sort, isAnonymous, order == "asc");
        }

        public static DataTable GetProducts(int? topicId, int? categoryId, int? supplierId, string keyWord, int pageNumber, int maxNum, out int total, string sort, bool isAnonymous, string order, bool issupplier, int? brandid, int? importsourceid)
        {
            return new ProductBrowseDao().GetProducts(topicId, categoryId, supplierId, keyWord, pageNumber, maxNum, out total, sort, isAnonymous, brandid, importsourceid,order == "asc");
        }

        public static DataTable GetProducts(int? topicId, int? categoryId, string keyWord, int pageNumber, int maxNum, out int total, string sort, bool isAnonymous, int? gradeId, string order)
        {
            return new ProductBrowseDao().GetProducts(topicId, categoryId, keyWord, pageNumber, maxNum, out total, sort, isAnonymous, gradeId, order == "asc");
        }

        public static DataTable GetFavorites()
        {
            return new FavoriteDao().GetFavorites();
        }

        public static DbQueryResult GetProductFavorites(Pagination page)
        {
            return new FavoriteDao().GetProductFavorites(page);
        }
        public static DataTable GetFavorites(Member member)
        {
            return new FavoriteDao().GetFavorites(member);
        }

        public static DbQueryResult GetFavorites(ProductFavoriteQuery query)
        {
            return new FavoriteDao().GetFavorites(query);
        }


        public static DbQueryResult GetFavorites(string keyword, string tags, Pagination page)
        {
            return new FavoriteDao().GetFavorites(HiContext.Current.User.UserId, keyword, tags, page);
        }
        public static int DeleteFavorite(int favoriteId)
        {
            return new FavoriteDao().DeleteFavorite(favoriteId);
        }
        public static int DeleteFavoriteTags(string tagname)
        {
            return new FavoriteDao().DeleteFavoriteTags(tagname);
        }
        public static string GetFavoriteTags()
        {
            string text = string.Empty;
            DataSet favoriteTags = new FavoriteDao().GetFavoriteTags();
            if (favoriteTags.Tables.Count > 0)
            {
                foreach (DataRow dataRow in favoriteTags.Tables[0].Rows)
                {
                    object obj = text;
                    text = string.Concat(new object[]
					{
						obj,
						"{\"TagId\":\"",
						dataRow["TagId"],
						"\",\"TagName\":\"",
						dataRow["TagName"],
						"\"},"
					});
                }
            }
            if (text.Length > 1)
            {
                text = text.Substring(0, text.Length - 1);
            }
            return text;
        }
        public static bool DeleteFavorites(string ids)
        {
            return new FavoriteDao().DeleteFavorites(ids);
        }
        public static bool DeleteFavorites(int userId, string ids)
        {
            return new FavoriteDao().DeleteFavorites(userId, ids);
        }
        public static bool AddProductToFavorite(int productId, int userId, out int favoriteId)
        {
            favoriteId = GetFavoriteId(userId, productId);
            if (favoriteId > 0)
            {
                return true;
            }

            favoriteId = AddProduct(productId, userId);
            return favoriteId > 0;
        }
        public static bool AddProductToFavorite(int productId, int userId, string tags, string remark)
        {
            FavoriteDao favoriteDao = new FavoriteDao();
            return favoriteDao.ExistsProduct(productId, userId) || favoriteDao.AddProductToFavorite(productId, userId, tags, remark);
        }


        public static bool CollectProduct(int productId, int userId, string tags, string remark)
        {
            FavoriteDao favoriteDao = new FavoriteDao();
            return favoriteDao.AddProductToFavorite(productId, userId, tags, remark);
        }


        public static int AddProduct(int productId, int userId)
        {
            FavoriteDao favoriteDao = new FavoriteDao();
            return favoriteDao.AddProduct(productId, userId);
        }
        public static DataTable GetFavoritesTypeTags()
        {
            FavoriteDao favoriteDao = new FavoriteDao();
            return favoriteDao.GetTypeTags();
        }
        public static int UpdateFavorite(int favoriteId, string tags, string remark)
        {
            FavoriteDao favoriteDao = new FavoriteDao();
            string text = "";
            string[] array;
            if (tags.Contains(","))
            {
                array = tags.Split(new char[]
				{
					','
				});
            }
            else
            {
                array = new string[]
				{
					tags
				};
            }
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text2 = array2[i];
                if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(text2.Trim())))
                {
                    favoriteDao.UpdateOrAddFavoriteTags(text2.Trim());
                    string text3 = tags.Replace(text2, "");
                    int num = (tags.Length - text3.Length) / text2.Length;
                    if (num == 1 || !text.Contains(text2))
                    {
                        text = text + DataHelper.CleanSearchString(text2) + ",";
                    }
                }
            }
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Substring(0, text.Length - 1);
            }
            return new FavoriteDao().UpdateFavorite(favoriteId, tags, remark);
        }
        public static bool ExistsProduct(int productId, int userId)
        {
            return new FavoriteDao().ExistsProduct(productId, userId);
        }
        public static int GetUserFavoriteCount()
        {
            return new FavoriteDao().GetUserFavoriteCount();
        }

        public static int GetFavoriteCountByProductId(int productid)
        {
            return new FavoriteDao().GetFavoriteCountByProductId(productid);
        }

        public static int GetUserFavoriteCount(int UserId)
        {
            return new FavoriteDao().GetUserFavoriteCount(UserId);
        }

        public static bool InsertProductReview(ProductReviewInfo review)
        {
            Globals.EntityCoding(review, true);
            return new ProductReviewDao().InsertProductReview(review);
        }
        public static DbQueryResult GetProductReviews(ProductReviewQuery reviewQuery)
        {
            return new ProductReviewDao().GetProductReviews(reviewQuery);
        }
        public static void GetProductReviewsSummary(int productId, out int count, out int score)
        {
            new ProductReviewDao().GetProductReviewsSummary(productId, out count, out score);
        }
        public static DataTable GetProductReviewAll(string orderid)
        {
            return new ProductReviewDao().GetProductReviewAll(orderid);
        }

        public static DataTable GetProductReviewAll(string orderid, int userId)
        {
            return new ProductReviewDao().GetProductReviewAll(orderid, userId);
        }
        public static DataTable GetOrderReviewAll(string orderIds, int userId)
        {
            return new ProductReviewDao().GetOrderReviewAll(orderIds, userId);
        }
        public static DataTable GetProductReviews(int maxNum)
        {
            ProductReviewQuery productReviewQuery = new ProductReviewQuery();
            productReviewQuery.PageIndex = 1;
            productReviewQuery.PageSize = maxNum;
            return new ProductReviewDao().GetProductReviews(productReviewQuery).Data as DataTable;
        }
        public static void LoadProductReview(int productId, out int buyNum, out int reviewNum, string OrderId = "")
        {
            //new ProductReviewDao().LoadProductReview(productId, HiContext.Current.User.UserId, out buyNum, out reviewNum, OrderId);
            int userId = HiContext.Current.User.UserId;
            LoadProductReview(userId, productId, out buyNum, out reviewNum, OrderId);
        }
        public static void LoadProductReview(int userId, int productId, out int buyNum, out int reviewNum, string OrderId = "")
        {
            new ProductReviewDao().LoadProductReview(productId, userId, out buyNum, out reviewNum, OrderId);
        }
        public static int GetUserProductReviewsCount()
        {
            return new ProductReviewDao().GetUserProductReviewsCount();
        }
        public static DataSet GetUserProductReviewsAndReplys(UserProductReviewAndReplyQuery query, out int total)
        {
            return new ProductReviewDao().GetUserProductReviewsAndReplys(query, out total);
        }
        public static bool InsertProductConsultation(ProductConsultationInfo productConsultation)
        {
            Globals.EntityCoding(productConsultation, true);
            return new ProductConsultationDao().InsertProductConsultation(productConsultation);
        }
        public static DbQueryResult GetProductConsultations(ProductConsultationAndReplyQuery page)
        {
            return new ProductConsultationDao().GetConsultationProducts(page);
        }
        public static DataSet GetProductConsultationsAndReplys(ProductConsultationAndReplyQuery query, out int total)
        {
            return new ProductConsultationDao().GetProductConsultationsAndReplys(query, out total);
        }
        public static DataSet GetGroupByProductList(ProductBrowseQuery query, out int count)
        {
            return new GroupBuyDao().GetGroupByProductList(query, out count);
        }
        public static DataTable GetGroupByProductList(int maxnum)
        {
            return new GroupBuyDao().GetGroupByProductList(maxnum);
        }
        public static GroupBuyInfo GetProductGroupBuyInfo(int productId)
        {
            return new GroupBuyDao().GetProductGroupBuyInfo(productId);
        }
        public static int GetOrderCount(int groupBuyId)
        {
            return new GroupBuyDao().GetOrderCount(groupBuyId);
        }
        public static decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
        {
            return new GroupBuyDao().GetCurrentPrice(groupBuyId, prodcutQuantity);
        }
        public static GroupBuyInfo GetGroupBuy(int groupbuyId)
        {
            return new GroupBuyDao().GetGroupBuy(groupbuyId);
        }
        public static DataTable GetGroupBuyProducts(int? categoryId, string keywords, int page, int size, out int total, bool onlyUnFinished = true)
        {
            return new GroupBuyDao().GetGroupBuyProducts(categoryId, keywords, page, size, out total, onlyUnFinished);
        }
        public static DbQueryResult GetCountDownProductList(ProductBrowseQuery query)
        {
            return new CountDownDao().GetCountDownProductList(query);
        }

        public static DbQueryResult GetCountDownProductList_Active_temp(ProductBrowseQuery query)
        {
            return new CountDownDao().GetCountDownProductList_Active_temp(query);
        }
        public static DbQueryResult GetCountDownProductList_Active_temp2(ProductBrowseQuery query)
        {
            return new CountDownDao().GetCountDownProductList_Active_temp2(query);
        }
        public static DbQueryResult GetCountDownProductList_Active_temp3(ProductBrowseQuery query)
        {
            return new CountDownDao().GetCountDownProductList_Active_temp3(query);
        }
        public static DbQueryResult GetCountDownProductList_Active_temp4(ProductBrowseQuery query)
        {
            return new CountDownDao().GetCountDownProductList_Active_temp4(query);
        }
        public static DbQueryResult GetCountDownProductList_Active_temp5(ProductBrowseQuery query)
        {
            return new CountDownDao().GetCountDownProductList_Active_temp5(query);
        }



        public static DbQueryResult GetActivityProductList(ProductBrowseQuery query)
        {
            return new CountDownDao().GetActivityProductList(query);
        }

        public static DataTable GetCountDownProductList(int? categoryId, string keyWord, int page, int size, out int total, bool onlyUnFinished = true)
        {
            return new CountDownDao().GetCountDownProductList(categoryId, keyWord, page, size, out total, onlyUnFinished);
        }
        public static DataTable GetCountDownProductList(DateTime start, DateTime end, int pageIndex, int pageSize, out int total)
        {
            return new CountDownDao().GetCountDownProductList(start, end, pageIndex, pageSize, out total);
        }
        public static DataTable GetCountDownProductList(int count)
        {
            return new CountDownDao().GetCountDownProductList(count);
        }

        public static DbQueryResult GetCountDownCategories()
        {
            return new CountDownDao().GetCountDownCategories();
        }

        public static DbQueryResult GetCountDownCategories(int pageindex,int pagesize)
        {
            return new CountDownDao().GetCountDownCategories(pageindex,pagesize);
        }

        
        public static CountDownInfo GetCountDownInfoByCountDownId(int countDownId)
        {
            return new CountDownDao().GetCountDownInfo(countDownId);
        }
        public static int GetCountDownSaleCountByCountDownId(int countDownId)
        {
            return new CountDownDao().GetCountDownSaleCountByCountDownId(countDownId);
        }

        public static CountDownInfo GetCountDownInfo(int productId, int countDownId = 0)
        {
            return new CountDownDao().GetCountDownByProductId(productId, countDownId);
        }
        public static DataTable GetCounDownProducList(int maxnum)
        {
            return new CountDownDao().GetCounDownProducList(maxnum);
        }
        public static DbQueryResult GetBundlingProductList(BundlingInfoQuery query)
        {
            return new BundlingDao().GetBundlingProducts(query);
        }
        public static List<BundlingItemInfo> GetBundlingItemsByID(int BundlingID)
        {
            return new BundlingDao().GetBundlingItemsByID(BundlingID);
        }
        public static BundlingInfo GetBundlingInfo(int Bundlingid)
        {
            return new BundlingDao().GetBundlingInfo(Bundlingid);
        }

        public static PromotionInfo GetProductPromotionInfo(int productid)
        {
            Member member = HiContext.Current.User as Member;
            return GetProductPromotionInfo(member, productid);
        }
        public static PromotionInfo GetProductPromotionInfo(Member member, int productid)
        {
            //Member member = HiContext.Current.User as Member;
            PromotionInfo result;
            if (member != null)
            {
                result = new BundlingDao().GetProductPromotionInfo(productid, member);
            }
            else
            {
                result = null;
            }
            return result;
        }
        public static PromotionInfo GetAllProductPromotionInfo(int productid)
        {
            return new BundlingDao().GetAllProductPromotionInfo(productid);
        }


        public static DataTable GetProductPromotionList(Member member, int productid)
        {
            //Member member = HiContext.Current.User as Member;
            DataTable result;
            if (member != null)
            {
                result = new BundlingDao().GetProductPromotionList(productid, member);
            }
            else
            {
                result = null;
            }
            return result;
        }
        public static DataTable GetAllProductPromotionList(int productid)
        {
            return new BundlingDao().GetAllProductPromotionList(productid);
        }



        public static Dictionary<int, string> GetProductPromotionInfo(Member member, List<int> productIds)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            DataTable dt = new BundlingDao().GetProductPromotionInfo(member, productIds);

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int productId = 0;
                    if (row["ProductId"] != DBNull.Value)
                    {
                        productId = (int)row["ProductId"];

                        if (!result.ContainsKey(productId))
                        {
                            result.Add(productId, (string)row["PromotionName"].ToString());
                        }
                    }
                }

            }

            return result;
        }

        public static Dictionary<int, string> GetAllProductPromotionInfo(List<int> productIds)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            DataTable dt = new BundlingDao().GetAllProductPromotionInfo(productIds);

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int productId = 0;
                    if (row["ProductId"] != DBNull.Value)
                    {
                        productId = (int)row["ProductId"];

                        if (!result.ContainsKey(productId))
                        {
                            result.Add(productId, (string)row["PromotionName"].ToString());
                        }
                    }
                }

            }

            return result;
        }

        public static DbQueryResult GetOnlineGifts(GiftQuery page)
        {
            page.IsOnline = true;
            return new GiftDao().GetGifts(page);
        }
        public static IList<GiftInfo> GetOnlinePromotionGifts()
        {
            return new GiftDao().GetOnlinePromotionGifts();
        }
        public static GiftInfo GetGift(int giftId)
        {
            return new GiftDao().GetGiftDetails(giftId);
        }
        public static IList<GiftInfo> GetGifts(int maxnum)
        {
            return new GiftDao().GetGifts(maxnum);
        }
        public static int GetLineItemNumber(int productId)
        {
            return new LineItemDao().GetLineItemNumber(productId);
        }

        public static int GetLineItemCount(int productId)
        {
            return new LineItemDao().GetLineItemCount(productId);
        }


        public static DataTable GetLineItems(int productId, int maxNum)
        {
            return new LineItemDao().GetLineItems(productId, maxNum);
        }
        public static DbQueryResult GetLineItems(Pagination page, int productId)
        {
            return new LineItemDao().GetLineItems(page, productId);
        }
        public static bool IsBuyProduct(int productId)
        {
            return new LineItemDao().IsBuyProduct(productId);
        }
        public static DbQueryResult GetBatchBuyProducts(ProductQuery query)
        {
            return new ProductBrowseDao().GetBatchBuyProducts(query);
        }
        public static DataTable GetSkusByProductId(int productId)
        {
            return new ProductBrowseDao().GetSkusByProductId(productId);
        }

        public static ImportSourceTypeInfo GetProductImportSourceType(int productId)
        {
            return new ProductDao().GetProductImportSourceType(productId);
        }

        public static string GetProductDescription(int productId, bool isMobile = true)
        {
            return new ProductBrowseDao().GetProductDescription(productId, isMobile);
        }

        public static string GetProductDescriptionByCountDownId(int CountDownId, bool isMobile = true)
        {
            return new ProductBrowseDao().GetProductDescriptionByCountDownId(CountDownId, isMobile);
        }

        public static bool IsExistProduct(int productId)
        {
            return new ProductBrowseDao().IsExistProduct(productId);
        }

        public static DataTable GetSuggestProducts(int userId, int count)
        {
            return new ProductDao().GetSuggestProducts(userId, count);
        }

        public static DataTable GetSuggestProducts(int userId, string username, int count)
        {
            return new ProductDao().GetSuggestProducts(userId, username, count);
        }

        public static DataTable GetHotBuyProduct(int categoryId)
        {
            return new ProductDao().GetHotBuyProduct(categoryId);
        }

        public static DataTable GetHistoryProducts(int userId, string username, int pageIndex, int pageSize, out int count)
        {
            return new ProductDao().GetHistoryProducts(userId, username, pageIndex, pageSize, out count);
        }



        /// <summary>
        /// 获取会员订单促销信息
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static DataTable GetOrderPromotionInfo(Member member)
        {
            return new PromotionDao().GetOrderPromotionInfo(member);
        }


        /// <summary>
        /// 获取会员订单促销信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllOrderPromotionInfo()
        {
            return new PromotionDao().GetAllOrderPromotionInfo();
        }

        /// <summary>
        /// 组合商品计算税费
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static decimal GetTaxByProductId(int productid)
        {
            return new ProductBrowseDao().GetTaxByProductId(productid);
        }
    }
}

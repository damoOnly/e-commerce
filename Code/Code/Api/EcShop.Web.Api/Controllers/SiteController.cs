using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

using EcShop.Web.Api.ApiException;

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;

using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.VShop;

using EcShop.SqlDal;
using EcShop.SqlDal.Commodities;

using EcShop.SaleSystem.Catalog;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.SaleSystem.Vshop;
using EcShop.Membership.Context;

using EcShop.Web.Api.Model;
using EcShop.Web.Api.Model.Result;

using EcShop.Web.Api.Utility;
using EcShop.Entities.Sales;
using EcShop.SaleSystem.Shopping;

namespace EcShop.Web.Api.Controllers
{
    public class SiteController : EcdevApiController
    {
        [HttpGet]
        public IHttpActionResult AppBanner(string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Site.AppBanner, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}", accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo("", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Site.AppBanner");
            }

            List<AppBannerListItem> items = new List<AppBannerListItem>();

            List<AppRunImgInfo> list = APPHelper.GetAppRunImg();

            if (!(list == null || list.Count == 0))
            {
                AppBannerListItem item = null;

                foreach (AppRunImgInfo current in list)
                {
                    item = new AppBannerListItem();

                    item.DeviceType = current.phoneType;
                    item.BannerUrl = Util.AppendImageHost(current.imgSrc);

                    items.Add(item);

                }
            }

            StandardResult<ListResult<AppBannerListItem>> result = new StandardResult<ListResult<AppBannerListItem>>()
            {
                code = 0,
                msg = "",
                data = new ListResult<AppBannerListItem>()
                {
                    TotalNumOfRecords = items.Count,
                    Results = items
                }
            };

            return base.JsonActionResult(result);
        }

        [HttpGet]
        public IHttpActionResult List(string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Site.List, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}", accessToken, channel, platform, ver), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo("", channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Site.List");
            }

            DataTable dt = SitesManagementHelper.GetSites();

            int defaultSiteId = 0;
            string defaultSiteName = "";

            List<SiteListItem> items = new List<SiteListItem>();

            if (dt != null)
            {
                SiteListItem item = null;
                foreach (DataRow current in dt.Rows)
                {
                    //SitesId,SitesName,City,IsDefault,Province,Sort
                    item = new SiteListItem();
                    item.SiteId = 0;
                    if (current["SitesId"] != DBNull.Value)
                    {
                        item.SiteId = (int)current["SitesId"];
                    }
                    item.SiteName = "";
                    if (current["SitesName"] != DBNull.Value)
                    {
                        item.SiteName = (string)current["SitesName"];
                    }
                    item.RegionId = 0;
                    if (current["City"] != DBNull.Value)
                    {
                        item.RegionId = (int)current["City"];
                    }
                    else
                    {
                        if (current["Province"] != DBNull.Value)
                        {
                            item.RegionId = (int)current["Province"];
                        }
                    }
                    item.IsDefault = false;
                    if (current["IsDefault"] != DBNull.Value)
                    {
                        item.IsDefault = ((int)current["IsDefault"]).Equals(1);
                    }
                    if (item.IsDefault)
                    {
                        defaultSiteId = item.SiteId;
                        defaultSiteName = item.SiteName;
                    }
                    item.DisplaySequence = 0;
                    if (current["Sort"] != DBNull.Value)
                    {
                        item.DisplaySequence = (int)current["Sort"];
                    }

                    items.Add(item);
                }
            }

            SiteListResult siteResult = new SiteListResult();
            siteResult.TotalNumOfRecords = items.Count;
            siteResult.DefaultSiteId = defaultSiteId;
            siteResult.DefaultSiteName = defaultSiteName;
            siteResult.Results = items;

            StandardResult<SiteListResult> result = new StandardResult<SiteListResult>()
            {
                code = 0,
                msg = "",
                data = siteResult
            };

            return base.JsonActionResult(result);
        }

        [HttpGet]
        public IHttpActionResult Topic(int siteId, int pageIndex, int pageSize, string accessToken, int channel, int platform, string ver)
        {
            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            string appId = "";
            int accessTookenCode = VerifyAccessToken(accessToken, out appId);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Get");
            }

            ListResult<TopicListItem> data = this.GetTopic(siteId, pageIndex, pageSize);

            return base.JsonActionResult(new StandardResult<ListResult<TopicListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult Banner(int siteId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Site.Banner, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}&siteId={4}", accessToken, channel, platform, ver, siteId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            string appId = "";
            int accessTookenCode = VerifyAccessToken(accessToken, out appId);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Get");
            }

            ListResult<BannerListItem> data = this.GetBanner(siteId);

            return base.JsonActionResult(new StandardResult<ListResult<BannerListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult GetRegisterBanner(int siteId, string accessToken, int channel, int platform, string ver)
        {
            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            string appId = "";
            int accessTookenCode = VerifyAccessToken(accessToken, out appId);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Site.AppBanner");
            }
            ListResult<BannerListItem> data = null;
            IList<BannerInfo> allBanners = VShopHelper.GetAllBanners(ClientType.App);
            BannerInfo registerBanner = allBanners.Where(m => m.LocationType == LocationType.Register).FirstOrDefault();
            List<BannerListItem> items = new List<BannerListItem>();

            if (registerBanner != null)
            {
                BannerListItem item = new BannerListItem();

                item.Id = registerBanner.BannerId;
                item.Title = registerBanner.ShortDesc;
                item.ImageUrl = Util.AppendImageHost(registerBanner.ImageUrl);
                item.BannerType = (int)registerBanner.LocationType;
                item.Values = registerBanner.Url;
                item.DisplaySequence = registerBanner.DisplaySequence;

                items.Add(item);
            }

            data = new ListResult<BannerListItem>();
            data.TotalNumOfRecords = items.Count;
            data.Results = items;

            return base.JsonActionResult(new StandardResult<ListResult<BannerListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });

        }


        [HttpGet]
        public IHttpActionResult Navigation(int siteId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Site.Navigation, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}&siteId={4}", accessToken, channel, platform, ver, siteId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            string appId = "";
            int accessTookenCode = VerifyAccessToken(accessToken, out appId);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Get");
            }

            ListResult<NavigationListItem> data = this.GetNavigation(siteId);

            return base.JsonActionResult(new StandardResult<ListResult<NavigationListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult HotSale(int siteId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Site.HotSale, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}&siteId={4}", accessToken, channel, platform, ver, siteId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            string appId = "";
            int accessTookenCode = VerifyAccessToken(accessToken, out appId);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Get");
            }

            ListResult<PlateListItem> data = this.GetHotSale(siteId);

            return base.JsonActionResult(new StandardResult<ListResult<PlateListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult Recommend(int siteId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Site.Recommend, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}&siteId={4}", accessToken, channel, platform, ver, siteId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(siteId, "", channel, platform, ver);

            // 验证令牌
            string appId = "";
            int accessTookenCode = VerifyAccessToken(accessToken, out appId);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Product.Get");
            }

            ListResult<PlateListItem> data = this.GetRecommend(siteId);

            return base.JsonActionResult(new StandardResult<ListResult<PlateListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult Promotional(int siteId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Site.Promotional, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}&siteId={4}", accessToken, channel, platform, ver, siteId), LoggerType.Info);

            ListResult<PlateListItem> data = this.GetPromotional(siteId);

            return base.JsonActionResult(new StandardResult<ListResult<PlateListItem>>()
            {
                code = 0,
                msg = "",
                data = data
            });
        }

        [HttpGet]
        public IHttpActionResult AllInOne(int siteId, string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Site.AllInOne, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}&siteId={4}&userId={5}", accessToken, channel, platform, ver, siteId, userId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            string appId = "";
            int accessTookenCode = VerifyAccessToken(accessToken, out appId);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Site.AllInOne");
            }

   
            AllInOneResult result = new AllInOneResult();
            result.code = 0;
            result.msg = "";
            result.data = new AllInOneData();

            // Banner
            result.data.Banner = this.GetBanner(siteId);

            // Navigagtion
            result.data.Navigation = this.GetNavigation(siteId);

            // Topic
            result.data.Topic = this.GetTopic(siteId, 1, 10);

            // 热卖
            result.data.HotSale = this.GetHotSale(siteId);

            // 新品/推荐
            result.data.Recommend = this.GetRecommend(siteId);

            // 特惠
            result.data.Promotional = this.GetPromotional(siteId);

            // 猜你喜欢
            result.data.SuggestProduct = this.GetSuggestProducts(siteId, userId, appId, 9);

            result.data.Icon = this.GetIcon(siteId);

            if (System.Configuration.ConfigurationManager.AppSettings["IsActiveOpen"].ToString() == "1")
            {
                result.data.CountDownProduct = this.GetCountDownProduct(siteId, 1, 6);
            }
            else
            {
                result.data.CountDownProduct = new ListResult<SimpleCountDownProductListItem>();
                result.data.CountDownProduct.Results = new List<SimpleCountDownProductListItem>();
            }

            result.data.ShoppingcartQuantity = GetShoppingcartQuantityByUserId(userId);

            result.data.RegisterBanner = this.GetCurrentRegisterBanner(siteId);

            if (platform == 2)
            {
                return base.JsonActionResult(this.CovertToAllInOneIosResult(result));
            }

            else
            {
                return base.JsonActionResult(result);
            }
        }


        [HttpGet]
        public IHttpActionResult GetShoppingcartQuantity(string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Site.GetShoppingcartQuantity, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}&userId={4}", accessToken, channel, platform, ver, userId), LoggerType.Info);

            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            int accessTookenCode = VerifyAccessToken(accessToken);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Site.GetShoppingcartQuantity");
            }

            SimpleShoppingCart item = new SimpleShoppingCart();

            item.Quantity= GetShoppingcartQuantityByUserId(userId);

            return base.JsonActionResult(new StandardResult<SimpleShoppingCart>()
            {
                code = 0,
                msg = "",
                data = item
            });

        }
        [HttpGet]
        public async Task<IHttpActionResult> AllInOneAsync(int siteId, string userId, string accessToken, int channel, int platform, string ver)
        {
            Logger.WriterLogger("Site.AllinOneAsync, Params: " + string.Format("accessToken={0}&channel={1}&platform={2}&ver={3}&siteId={4}&userId={5}", accessToken, channel, platform, ver, siteId, userId), LoggerType.Info);


            // 保存访问信息
            base.SaveVisitInfo(userId, channel, platform, ver);

            // 验证令牌
            string appId = "";
            int accessTookenCode = VerifyAccessToken(accessToken, out appId);
            if (accessTookenCode > 0)
            {
                return base.JsonFaultResult(new CommonException(accessTookenCode).GetMessage(), "Site.AllInOne");
            }

            AllInOneResult result = null;

            await Task.Factory.StartNew(() =>
            {
                result = new AllInOneResult();
                result.code = 0;
                result.msg = "";
                result.data = new AllInOneData();

                // Banner
                result.data.Banner = this.GetBanner(siteId);

                // Navigagtion
                result.data.Navigation = this.GetNavigation(siteId);

                // Topic
                result.data.Topic = this.GetTopic(siteId, 1, 10);

                // 热卖
                result.data.HotSale = this.GetHotSale(siteId);

                // 新品/推荐
                result.data.Recommend = this.GetRecommend(siteId);

                // 特惠
                result.data.Promotional = this.GetPromotional(siteId);

                // 猜你喜欢
                result.data.SuggestProduct = this.GetSuggestProducts(siteId, userId, appId, 9);
            });

            return base.JsonActionResult(result);
        }

        #region

        private ListResult<TopicListItem> GetTopic(int siteId, int pageIndex, int pageSize)
        {
            ListResult<TopicListItem> data = null;

            if (base.IsUseCache)
            {
                data = MemoryCacher.GetValue("SITE-TOPIC") as ListResult<TopicListItem>;

                if (data != null)
                    return data;
            }

            DataTable dt = VshopBrowser.GetTopics(ClientType.App);

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

            data = new ListResult<TopicListItem>();
            data.TotalNumOfRecords = items.Count;
            data.Results = items;

            if (base.IsUseCache)
            {
                MemoryCacher.Add("SITE-TOPIC", data, DateTimeOffset.UtcNow.AddMinutes(SITE_CACHE_KEEP_TIME));
            }

            return data;
        }
        private ListResult<BannerListItem> GetBanner(int siteId)
        {
            ListResult<BannerListItem> data = null;

            if (base.IsUseCache)
            {
                data = MemoryCacher.GetValue("SITE-BANNER") as ListResult<BannerListItem>;

                if (data != null)
                    return data;
            }

            IList<BannerInfo> allBanners = VShopHelper.GetAllBanners(ClientType.App);
            List<BannerListItem> items = new List<BannerListItem>();

            if (allBanners != null)
            {
                BannerListItem item = null;
                allBanners = allBanners.Where(m => m.LocationType != LocationType.Register).ToList();
                foreach (var current in allBanners)
                {
                    item = new BannerListItem();

                    item.Id = current.BannerId;
                    item.Title = current.ShortDesc;
                    item.ImageUrl = Util.AppendImageHost(current.ImageUrl);
                    item.BannerType = (int)current.LocationType;
                    item.Values = current.Url;
                    item.DisplaySequence = current.DisplaySequence;

                    items.Add(item);
                }
            }

            data = new ListResult<BannerListItem>();
            data.TotalNumOfRecords = items.Count;
            data.Results = items;

            if (base.IsUseCache)
            {
                MemoryCacher.Add("SITE-BANNER", data, DateTimeOffset.UtcNow.AddMinutes(SITE_CACHE_KEEP_TIME));
            }

            return data;
        }

        private ListResult<NavigationListItem> GetNavigation(int siteId)
        {
            ListResult<NavigationListItem> data = null;

            if (base.IsUseCache)
            {
                data = MemoryCacher.GetValue("SITE-NAVIGATION") as ListResult<NavigationListItem>;

                if (data != null)
                    return data;
            }

            IList<NavigateInfo> allNavigates = VShopHelper.GetAllNavigate(ClientType.App);

            List<NavigationListItem> items = new List<NavigationListItem>();

            if (allNavigates != null)
            {
                NavigationListItem item = null;

                foreach (var current in allNavigates)
                {
                    item = new NavigationListItem();

                    item.Id = current.BannerId;
                    item.Title = current.ShortDesc;
                    item.Icon = Util.AppendImageHost(current.ImageUrl);
                    item.NavigationType = (int)current.LocationType;
                    item.Values = current.Url;
                    item.DisplaySequence = current.DisplaySequence;

                    items.Add(item);
                }
            }

            data = new ListResult<NavigationListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            if (base.IsUseCache)
            {
                MemoryCacher.Add("SITE-NAVIGATION", data, DateTimeOffset.UtcNow.AddMinutes(SITE_CACHE_KEEP_TIME));
            }

            return data;
        }

        private ListResult<PlateListItem> GetHotSale(int siteId)
        {
            ListResult<PlateListItem> data = null;

            if (base.IsUseCache)
            {
                data = MemoryCacher.GetValue("SITE-HOTSALE") as ListResult<PlateListItem>;

                if (data != null)
                    return data;
            }

            IList<HotSaleInfo> list = VShopHelper.GetAllHotSale(ClientType.App);

            List<PlateListItem> items = new List<PlateListItem>();

            if (list != null)
            {
                PlateListItem item = null;

                foreach (var current in list)
                {
                    item = new PlateListItem();

                    item.Id = current.BannerId;
                    item.Title = current.ShortDesc;
                    item.ImageUrl = Util.AppendImageHost(current.ImageUrl);
                    item.NavigationType = (int)current.LocationType;
                    item.Values = current.Url;
                    item.DisplaySequence = current.DisplaySequence;

                    items.Add(item);
                }
            }

            data = new ListResult<PlateListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            if (base.IsUseCache)
            {
                MemoryCacher.Add("SITE-HOTSALE", data, DateTimeOffset.UtcNow.AddMinutes(SITE_CACHE_KEEP_TIME));
            }

            return data;
        }

        private ListResult<PlateListItem> GetRecommend(int siteId)
        {
            ListResult<PlateListItem> data = null;

            if (base.IsUseCache)
            {
                data = MemoryCacher.GetValue("SITE-RECOMMEND") as ListResult<PlateListItem>;

                if (data != null)
                    return data;
            }

            IList<RecommendInfo> list = VShopHelper.GetAllRecommend(ClientType.App);

            List<PlateListItem> items = new List<PlateListItem>();

            if (list != null)
            {
                PlateListItem item = null;

                foreach (var current in list)
                {
                    item = new PlateListItem();

                    item.Id = current.BannerId;
                    item.Title = current.ShortDesc;
                    item.ImageUrl = Util.AppendImageHost(current.ImageUrl);
                    item.NavigationType = (int)current.LocationType;
                    item.Values = current.Url;
                    item.DisplaySequence = current.DisplaySequence;

                    items.Add(item);
                }
            }

            data = new ListResult<PlateListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            if (base.IsUseCache)
            {
                MemoryCacher.Add("SITE-RECOMMEND", data, DateTimeOffset.UtcNow.AddMinutes(SITE_CACHE_KEEP_TIME));
            }

            return data;
        }

        private ListResult<PlateListItem> GetPromotional(int siteId)
        {
            ListResult<PlateListItem> data = null;

            if (base.IsUseCache)
            {
                data = MemoryCacher.GetValue("SITE-PROMOTIONAL") as ListResult<PlateListItem>;

                if (data != null)
                    return data;
            }

            IList<PromotionalInfo> list = VShopHelper.GetAllPromotional(ClientType.App);

            List<PlateListItem> items = new List<PlateListItem>();

            if (list != null)
            {
                PlateListItem item = null;

                foreach (var current in list)
                {
                    item = new PlateListItem();

                    item.Id = current.BannerId;
                    item.Title = current.ShortDesc;
                    item.ImageUrl = Util.AppendImageHost(current.ImageUrl);
                    item.NavigationType = (int)current.LocationType;
                    item.Values = current.Url;
                    item.DisplaySequence = current.DisplaySequence;

                    items.Add(item);
                }
            }

            data = new ListResult<PlateListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            if (base.IsUseCache)
            {
                MemoryCacher.Add("SITE-PROMOTIONAL", data, DateTimeOffset.UtcNow.AddMinutes(SITE_CACHE_KEEP_TIME));
            }

            return data;
        }

        private ListResult<ProductListItem> GetSuggestProducts(int siteId, string userId, string appId, int count)
        {
            //猜你喜欢没有实现，准确的应该按客户的浏览历史来
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
                    if (row["ThumbnailUrl220"] != DBNull.Value)
                    {
                        item.ImageUrl = Util.AppendImageHost((string)row["ThumbnailUrl220"]);
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

                    if (row["IsCollect"] != DBNull.Value)
                    {
                        item.IsCollect = (int)row["IsCollect"];
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
            }

            ListResult<ProductListItem> data = new ListResult<ProductListItem>();
            data.TotalNumOfRecords = products.Count;
            data.Results = products;

            return data;
        }


        private ListResult<IconListItem> GetIcon(int siteId)
        {
            ListResult<IconListItem> data = null;

            if (base.IsUseCache)
            {
                data = MemoryCacher.GetValue("SITE-ICON") as ListResult<IconListItem>;

                if (data != null)
                    return data;
            }

            IList<IconInfo> list = VShopHelper.GetAllIcon(ClientType.App);

            List<IconListItem> items = new List<IconListItem>();

            if (list != null)
            {
                IconListItem item = null;

                foreach (var current in list)
                {
                    item = new IconListItem();
                    item.Title = current.ShortDesc;
                    item.ImageUrl = Util.AppendImageHost(current.ImageUrl);
                    item.DisplaySequence = current.DisplaySequence;

                    items.Add(item);
                }
            }

            data = new ListResult<IconListItem>();
            data.TotalNumOfRecords = items.Count; ;
            data.Results = items;

            if (base.IsUseCache)
            {
                MemoryCacher.Add("SITE-ICON", data, DateTimeOffset.UtcNow.AddMinutes(SITE_CACHE_KEEP_TIME));
            }

            return data;
        }

        private ListResult<SimpleCountDownProductListItem> GetCountDownProduct(int siteId, int pageIndex, int pageSize)
        {
            DataTable dt = ProductBrowser.GetCountDownProductList(pageSize);

            ListResult<SimpleCountDownProductListItem> data = null;

            List<SimpleCountDownProductListItem> items = new List<SimpleCountDownProductListItem>();

            if (dt != null)
            {
                SimpleCountDownProductListItem item = null;

                foreach (DataRow current in dt.Rows)
                {
                    item = new SimpleCountDownProductListItem();

                    //item.SalePrice = 0;
                    //if (current["SalePrice"] != DBNull.Value)
                    //{
                    //    item.SalePrice = (decimal)current["SalePrice"];
                    //}

                    item.MarketPrice = 0;
                    if (current["MarketPrice"] != DBNull.Value)
                    {
                        item.MarketPrice = (decimal)current["MarketPrice"];
                    }

                    item.CountDownPrice = 0;
                    if (current["CountDownPrice"] != DBNull.Value)
                    {
                        item.CountDownPrice = (decimal)current["CountDownPrice"];
                    }

                    item.ImageUrl = "";
                    if (current["ThumbnailUrl220"] != DBNull.Value)
                    {
                        item.ImageUrl = Util.AppendImageHost(((string)current["ThumbnailUrl220"]));
                    }

                    items.Add(item);
                }
            }

            data = new ListResult<SimpleCountDownProductListItem>();
            data.TotalNumOfRecords = items.Count;
            data.Results = items;

            return data;
        }

        private string GetShoppingcartQuantityByUserId(string userId)
        {
            Member member = GetMember(userId.ToSeesionId());

            string strQuantity = "";
            if (member != null)
            {

                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(member);

                if (shoppingCart != null)
                {
                    int Quantity = shoppingCart.GetQuantity();
                    if (Quantity > 0)
                    {
                        strQuantity = Quantity.ToString();
                    }
                }
            }

            return strQuantity;
        }

        private ListResult<BannerListItem> GetCurrentRegisterBanner(int siteId)
        {
            ListResult<BannerListItem> data = null;
            if (base.IsUseCache)
            {
                data = MemoryCacher.GetValue("SITE-REGISTERBANNER") as ListResult<BannerListItem>;

                if (data != null)
                    return data;
            }

            IList<BannerInfo> allBanners = VShopHelper.GetAllBanners(ClientType.App);
            BannerInfo registerBanner = allBanners.Where(m => m.LocationType == LocationType.Register).FirstOrDefault();
            List<BannerListItem> items = new List<BannerListItem>();

            if (registerBanner != null)
            {
                BannerListItem item = new BannerListItem();

                item.Id = registerBanner.BannerId;
                item.Title = registerBanner.ShortDesc;
                item.ImageUrl = Util.AppendImageHost(registerBanner.ImageUrl);
                item.BannerType = (int)registerBanner.LocationType;
                item.Values = registerBanner.Url;
                item.DisplaySequence = registerBanner.DisplaySequence;

                items.Add(item);
            }

            data = new ListResult<BannerListItem>();
            data.TotalNumOfRecords = items.Count;
            data.Results = items;

            if (base.IsUseCache)
            {
                MemoryCacher.Add("SITE-REGISTERBANNER", data, DateTimeOffset.UtcNow.AddMinutes(SITE_CACHE_KEEP_TIME));
            }

            return data;
        }
        #endregion

        #region


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

        #endregion

        private AllInOneIosResult CovertToAllInOneIosResult(AllInOneResult result)
        {
            AllInOneIosResult iosresult = new AllInOneIosResult();

            iosresult.code = result.code;
            iosresult.msg = result.msg;
            iosresult.data = new AllInOneIosData();

            // Banner
            iosresult.data.Banner = result.data.Banner;

            // Navigagtion
            iosresult.data.Navigation = result.data.Navigation;

            iosresult.data.Topic = result.data.Topic;

            // 热卖
            iosresult.data.HotSale = result.data.HotSale;

            // 新品/推荐
            iosresult.data.Recommend = result.data.Recommend;

            // 特惠
            iosresult.data.Promotional = result.data.Promotional;

            // 猜你喜欢
            iosresult.data.SuggestProduct = result.data.SuggestProduct;

            iosresult.data.Icon = result.data.Icon;

            ListResult<IOSSimpleCountDownProductListItem> countdownlist = new ListResult<IOSSimpleCountDownProductListItem>();
            
            countdownlist.Results=new List<IOSSimpleCountDownProductListItem>();

            foreach(var item in result.data.CountDownProduct.Results)
            {
                IOSSimpleCountDownProductListItem iositem=new IOSSimpleCountDownProductListItem();

                iositem.ImageUrl=item.ImageUrl;
                iositem.MarketPrice=item.MarketPrice.ToString("0.00");
                iositem.CountDownPrice=item.CountDownPrice.ToString("0.00");

                countdownlist.Results.Add(iositem);
            }
            iosresult.data.CountDownProduct=countdownlist;
            iosresult.data.ShoppingcartQuantity = result.data.ShoppingcartQuantity;

            iosresult.data.RegisterBanner = result.data.RegisterBanner;

            return iosresult;
        }


    }
}

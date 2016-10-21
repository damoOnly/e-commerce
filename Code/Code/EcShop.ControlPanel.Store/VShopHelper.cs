using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.SqlDal.Commodities;
using EcShop.SqlDal.VShop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
namespace EcShop.ControlPanel.Store
{
    public static class VShopHelper
    {
        public static System.Data.DataTable GetHomeProducts(ClientType client)
        {
            return new HomeProductDao().GetHomeProducts(client);
        }
        public static System.Data.DataTable GetHomeProducts(ClientType client, bool issupplier, int supplierId)
        {
            return new HomeProductDao().GetHomeProducts(client, issupplier, supplierId);
        }
        public static bool AddHomeProdcut(int productId, ClientType client)
        {
            return new HomeProductDao().AddHomeProdcut(productId, client);
        }
        public static bool AddHomeProdcut(int productId, ClientType client, int supplierId)
        {
            return new HomeProductDao().AddHomeProdcut(productId, client, supplierId);
        }
        public static bool RemoveHomeProduct(int productId, ClientType client)
        {
            return new HomeProductDao().RemoveHomeProduct(productId, client);
        }
        public static bool RemoveAllHomeProduct(ClientType client)
        {
            return new HomeProductDao().RemoveAllHomeProduct(client);
        }
        public static bool UpdateHomeProductSequence(ClientType client, int productId, int displaysequence)
        {
            return new HomeProductDao().UpdateHomeProductSequence(client, productId, displaysequence);
        }

        public static System.Data.DataTable GetHomeTopics(ClientType client)
        {
            return new HomeTopicDao().GetHomeTopics(client);
        }

        public static System.Data.DataTable GetHomeTopics(ClientType client, int supplierId)
        {
            return new HomeTopicDao().GetHomeTopics(client, supplierId);
        }

        public static bool AddHomeTopic(int topicId, ClientType client)
        {
            return new HomeTopicDao().AddHomeTopic(topicId, client);
        }

        public static bool AddHomeTopic(int topicId, ClientType client, int supplierId)
        {
            return new HomeTopicDao().AddHomeTopic(topicId, client, supplierId);
        }

        public static bool RemoveHomeTopic(int TopicId, ClientType client)
        {
            return new HomeTopicDao().RemoveHomeTopic(TopicId, client);
        }

        public static bool RemoveHomeTopic(int TopicId, ClientType client, int supplierId)
        {
            return new HomeTopicDao().RemoveHomeTopic(TopicId, client, supplierId);
        }

        public static bool RemoveHomeTopic(int TopicId)
        {
            return new HomeTopicDao().RemoveHomeTopic(TopicId);
        }
        public static bool RemoveAllHomeTopics(ClientType client)
        {
            return new HomeTopicDao().RemoveAllHomeTopics(client);
        }

        public static bool RemoveAllHomeTopics(ClientType client, int supplierId)
        {
            return new HomeTopicDao().RemoveAllHomeTopics(client, supplierId);
        }

        public static bool UpdateHomeTopicSequence(int TopicId, int displaysequence, ClientType clientType)
        {
            return new HomeTopicDao().UpdateHomeTopicSequence(TopicId, displaysequence, clientType);
        }
        public static IList<BannerInfo> GetAllBanners(ClientType client)
        {
            return new BannerDao().GetAllBanners(client);
        }

        public static IList<BannerInfo> GetAllBanners(ClientType client, int supplierId)
        {
            return new BannerDao().GetAllBanners(client, supplierId);
        }

        public static IList<NavigateInfo> GetAllNavigate(ClientType client)
        {
            return new BannerDao().GetAllNavigate(client);
        }
        public static IList<NavigateInfo> GetAllNavigate(ClientType client, int supplierId)
        {
            return new BannerDao().GetAllNavigate(client, supplierId);
        }
        public static IList<HotSaleInfo> GetAllHotSale(ClientType client)
        {
            return new BannerDao().GetAllHotSale(client);
        }

        public static IList<HotSaleInfo> GetAllHotSale(ClientType client, int supplierId)
        {
            return new BannerDao().GetAllHotSale(client, supplierId);
        }

        public static IList<RecommendInfo> GetAllRecommend(ClientType client)
        {
            return new BannerDao().GetAllRecommend(client);
        }

        public static IList<RecommendInfo> GetAllRecommend(ClientType client, int supplierId)
        {
            return new BannerDao().GetAllRecommend(client, supplierId);
        }
        public static IList<PromotionalInfo> GetAllPromotional(ClientType client)
        {
            return new BannerDao().GetAllPromotional(client);
        }


        public static IList<IconInfo> GetAllIcon(ClientType client)
        {
            return new BannerDao().GetAllIcon(client);
        }

        public static IList<PromotionalInfo> GetAllPromotional(ClientType client, int supplierId)
        {
            return new BannerDao().GetAllPromotional(client, supplierId);
        }
        public static bool SaveTplCfg(TplCfgInfo info)
        {
            return new BannerDao().SaveTplCfg(info);
        }
        public static bool UpdateTplCfg(TplCfgInfo info)
        {
            return new BannerDao().UpdateTplCfg(info);
        }
        public static TplCfgInfo GetTplCfgById(int id)
        {
            return new BannerDao().GetTplCfgById(id);
        }
        public static bool DelTplCfg(int id)
        {
            return new BannerDao().DelTplCfg(id);
        }
        /// <summary>
        /// 根据IP地址获取站点名称
        /// </summary>
        /// <param name="IpAddress"></param>
        /// <returns></returns>
        public static string GetSiteName(long IpAddress)
        {
            return new BannerDao().GetSiteName(IpAddress);
        }

        public static void SwapTplCfgSequence(int bannerId, int replaceBannerId)
        {
            BannerDao bannerDao = new BannerDao();
            TplCfgInfo tplCfgById = bannerDao.GetTplCfgById(bannerId);
            TplCfgInfo tplCfgById2 = bannerDao.GetTplCfgById(replaceBannerId);
            if (tplCfgById != null && tplCfgById2 != null)
            {
                int displaySequence = tplCfgById.DisplaySequence;
                tplCfgById.DisplaySequence = tplCfgById2.DisplaySequence;
                tplCfgById2.DisplaySequence = displaySequence;
                bannerDao.UpdateTplCfg(tplCfgById);
                bannerDao.UpdateTplCfg(tplCfgById2);
            }
        }
        public static bool SaveActivity(ActivityInfo activity)
        {
            int activityId = new ActivityDao().SaveActivity(activity);
            ReplyInfo replyInfo = new TextReplyInfo();
            replyInfo.Keys = activity.Keys;
            replyInfo.MatchType = MatchType.Equal;
            replyInfo.MessageType = MessageType.Text;
            replyInfo.ReplyType = ReplyType.SignUp;
            replyInfo.ActivityId = activityId;
            return new ReplyDao().SaveReply(replyInfo);
        }
        public static bool UpdateActivity(ActivityInfo activity)
        {
            return new ActivityDao().UpdateActivity(activity);
        }
        public static bool DeleteActivity(int activityId)
        {
            return new ActivityDao().DeleteActivity(activityId);
        }
        public static ActivityInfo GetActivity(int activityId)
        {
            return new ActivityDao().GetActivity(activityId);
        }
        public static IList<ActivityInfo> GetAllActivity()
        {
            return new ActivityDao().GetAllActivity();
        }
        public static IList<ActivitySignUpInfo> GetActivitySignUpById(int activityId)
        {
            return new ActivitySignUpDao().GetActivitySignUpById(activityId);
        }
        public static int SaveLotteryTicket(LotteryTicketInfo info)
        {
            string prizeSetting = JsonConvert.SerializeObject(info.PrizeSettingList);
            info.PrizeSetting = prizeSetting;
            return new LotteryActivityDao().SaveLotteryTicket(info);
        }
        public static bool UpdateLotteryTicket(LotteryTicketInfo info)
        {
            string prizeSetting = JsonConvert.SerializeObject(info.PrizeSettingList);
            info.PrizeSetting = prizeSetting;
            return new LotteryActivityDao().UpdateLotteryTicket(info);
        }
        public static bool DelteLotteryTicket(int activityId)
        {
            return new LotteryActivityDao().DelteLotteryTicket(activityId);
        }
        public static LotteryTicketInfo GetLotteryTicket(int activityid)
        {
            LotteryTicketInfo lotteryTicket = new LotteryActivityDao().GetLotteryTicket(activityid);
            lotteryTicket.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryTicket.PrizeSetting);
            return lotteryTicket;
        }
        public static DbQueryResult GetLotteryTicketList(LotteryActivityQuery page)
        {
            return new LotteryActivityDao().GetLotteryTicketList(page);
        }
        public static bool SaveAlarm(AlarmInfo info)
        {
            return new AlarmDao().Save(info);
        }
        public static bool DeleteAlarm(int id)
        {
            return new AlarmDao().Delete(id);
        }
        public static DbQueryResult GetAlarms(int pageIndex, int pageSize)
        {
            return new AlarmDao().List(pageIndex, pageSize);
        }
        public static bool SaveFeedBack(FeedBackInfo info)
        {
            return new FeedBackDao().Save(info);
        }
        public static FeedBackInfo GetFeedBack(int id)
        {
            return new FeedBackDao().Get(id);
        }
        public static FeedBackInfo GetFeedBack(string feedBackID)
        {
            return new FeedBackDao().Get(feedBackID);
        }
        public static bool DeleteFeedBack(int id)
        {
            return new FeedBackDao().Delete(id);
        }
        public static bool UpdateFeedBackMsgType(string feedBackId, string msgType)
        {
            return new FeedBackDao().UpdateMsgType(feedBackId, msgType);
        }
        public static DbQueryResult GetFeedBacks(int pageIndex, int pageSize, string msgType)
        {
            return new FeedBackDao().List(pageIndex, pageSize, msgType);
        }
        public static TopicInfo Gettopic(int topicId)
        {
            return new TopicDao().GetTopic(topicId);
        }
        public static DbQueryResult GettopicList(TopicQuery page)
        {
            return new TopicDao().GetTopicList(page);
        }
        public static IList<TopicInfo> Gettopics()
        {
            return new TopicDao().GetTopics();
        }
        public static int Deletetopics(IList<int> topics)
        {
            int result;
            if (topics == null || topics.Count == 0)
            {
                result = 0;
            }
            else
            {
                result = new TopicDao().DeleteTopics(topics);
            }
            return result;
        }
        public static bool Createtopic(TopicInfo topic, out int id)
        {
            id = 0;
            bool result;
            if (null == topic)
            {
                result = false;
            }
            else
            {
                Globals.EntityCoding(topic, true);
                id = new TopicDao().AddTopic(topic);
                ReplyInfo replyInfo = new TextReplyInfo();
                replyInfo.Keys = topic.Keys;
                replyInfo.MatchType = MatchType.Equal;
                replyInfo.MessageType = MessageType.Text;
                replyInfo.ReplyType = ReplyType.Topic;
                replyInfo.ActivityId = id;
                result = new ReplyDao().SaveReply(replyInfo);
            }
            return result;
        }
        public static bool Updatetopic(TopicInfo topic)
        {
            bool result;
            if (null == topic)
            {
                result = false;
            }
            else
            {
                Globals.EntityCoding(topic, true);
                result = new TopicDao().UpdateTopic(topic);
            }
            return result;
        }
        public static System.Data.DataTable GetRelatedTopicProducts(int topicid, bool verfiy = true)
        {
            return new TopicDao().GetRelatedTopicProducts(topicid, verfiy);
        }
        public static bool AddReleatesProdcutBytopicid(int topicid, int productId)
        {
            return new TopicDao().AddReleatesProdcutBytopicid(topicid, productId);
        }
        public static bool RemoveReleatesProductBytopicid(int topicid, int productId)
        {
            return new TopicDao().RemoveReleatesProductBytopicid(topicid, productId);
        }
        public static bool RemoveReleatesProductBytopicid(int topicid)
        {
            return new TopicDao().RemoveReleatesProductBytopicid(topicid);
        }
        public static bool Deletetopic(int topicId)
        {
            return new TopicDao().DeleteTopic(topicId);
        }
        public static bool SwapTopicSequence(int topicid, int displaysequence)
        {
            return new TopicDao().SwapTopicSequence(topicid, displaysequence);
        }
        public static bool UpdateRelateProductSequence(int TopicId, int RelatedProductId, int displaysequence)
        {
            return new TopicDao().UpdateRelateProductSequence(TopicId, RelatedProductId, displaysequence);
        }
        public static IList<MenuInfo> GetMenus(ClientType clientType)
        {
            IList<MenuInfo> list = new List<MenuInfo>();
            MenuDao menuDao = new MenuDao();
            IList<MenuInfo> topMenus = menuDao.GetTopMenus(clientType);
            IList<MenuInfo> result;
            if (topMenus == null)
            {
                result = list;
            }
            else
            {
                foreach (MenuInfo current in topMenus)
                {
                    list.Add(current);
                    IList<MenuInfo> menusByParentId = menuDao.GetMenusByParentId(current.MenuId, clientType);
                    if (menusByParentId != null)
                    {
                        foreach (MenuInfo current2 in menusByParentId)
                        {
                            list.Add(current2);
                        }
                    }
                }
                result = list;
            }
            return result;
        }
        public static IList<MenuInfo> GetMenusByParentId(int parentId, ClientType clientType)
        {
            return new MenuDao().GetMenusByParentId(parentId, clientType);
        }
        public static MenuInfo GetMenu(int menuId)
        {
            return new MenuDao().GetMenu(menuId);
        }
        public static IList<MenuInfo> GetTopMenus(ClientType clientType)
        {
            return new MenuDao().GetTopMenus(clientType);
        }
        public static bool CanAddMenu(int parentId, ClientType clientType)
        {
            int num = 3;
            int num2 = 5;
            if (clientType == ClientType.AliOH)
            {
                num = 3;
                num2 = 5;
            }
            IList<MenuInfo> menusByParentId = new MenuDao().GetMenusByParentId(parentId, clientType);
            bool result;
            if (menusByParentId == null || menusByParentId.Count == 0)
            {
                result = true;
            }
            else
            {
                if (parentId == 0)
                {
                    result = (menusByParentId.Count < num);
                }
                else
                {
                    result = (menusByParentId.Count < num2);
                }
            }
            return result;
        }
        public static bool UpdateMenu(MenuInfo menu)
        {
            return new MenuDao().UpdateMenu(menu);
        }
        public static bool SaveMenu(MenuInfo menu)
        {
            return new MenuDao().SaveMenu(menu);
        }
        public static bool DeleteMenu(int menuId)
        {
            return new MenuDao().DeleteMenu(menuId);
        }
        public static void SwapMenuSequence(int menuId, bool isUp)
        {
            new MenuDao().SwapMenuSequence(menuId, isUp);
        }
        public static IList<MenuInfo> GetInitMenus(ClientType clientType)
        {
            MenuDao menuDao = new MenuDao();
            IList<MenuInfo> topMenus = menuDao.GetTopMenus(clientType);
            foreach (MenuInfo current in topMenus)
            {
                current.Chilren = menuDao.GetMenusByParentId(current.MenuId, clientType);
                if (current.Chilren == null)
                {
                    current.Chilren = new List<MenuInfo>();
                }
            }
            return topMenus;
        }
        public static string UploadVipBGImage(HttpPostedFile postedFile)
        {
            string result;
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                result = string.Empty;
            }
            else
            {
                string text = HiContext.Current.GetStoragePath() + "/Vipcard/vipbg" + Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
                result = text;
            }
            return result;
        }
        public static string UploadDefautBg(HttpPostedFile postedFile)
        {
            string result;
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                result = string.Empty;
            }
            else
            {
                string text = HiContext.Current.GetVshopSkinPath(null) + "/images/ad/DefautPageBg" + Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
                result = text;
            }
            return result;
        }
        public static string UploadWeiXinCodeImage(HttpPostedFile postedFile)
        {
            string result;
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                result = string.Empty;
            }
            else
            {
                string text = HiContext.Current.GetStoragePath() + "/WeiXinCodeImageUrl" + Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
                result = text;
            }
            return result;
        }
        public static string UploadVipQRImage(HttpPostedFile postedFile)
        {
            string result;
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                result = string.Empty;
            }
            else
            {
                string text = HiContext.Current.GetStoragePath() + "/Vipcard/vipqr" + Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
                result = text;
            }
            return result;
        }
        public static string UploadTopicImage(HttpPostedFile postedFile)
        {
            string result;
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                result = string.Empty;
            }
            else
            {
                string text = HiContext.Current.GetStoragePath() + "/topic/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
                postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
                result = text;
            }
            return result;
        }
        public static int InsertLotteryActivity(LotteryActivityInfo info)
        {
            string prizeSetting = JsonConvert.SerializeObject(info.PrizeSettingList);
            info.PrizeSetting = prizeSetting;
            return new LotteryActivityDao().InsertLotteryActivity(info);
        }
        public static IList<LotteryActivityInfo> GetLotteryActivityByType(LotteryActivityType type)
        {
            return new LotteryActivityDao().GetLotteryActivityByType(type);
        }
        public static bool UpdateLotteryActivity(LotteryActivityInfo info)
        {
            string prizeSetting = JsonConvert.SerializeObject(info.PrizeSettingList);
            info.PrizeSetting = prizeSetting;
            return new LotteryActivityDao().UpdateLotteryActivity(info);
        }
        public static bool DeleteLotteryActivity(int activityid, string type = "")
        {
            return new LotteryActivityDao().DelteLotteryActivity(activityid, type);
        }
        public static LotteryActivityInfo GetLotteryActivityInfo(int activityid)
        {
            LotteryActivityInfo lotteryActivityInfo = new LotteryActivityDao().GetLotteryActivityInfo(activityid);
            lotteryActivityInfo.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryActivityInfo.PrizeSetting);
            return lotteryActivityInfo;
        }
        public static DbQueryResult GetLotteryActivityList(LotteryActivityQuery page)
        {
            return new LotteryActivityDao().GetLotteryActivityList(page);
        }
        public static List<PrizeRecordInfo> GetPrizeList(PrizeQuery page)
        {
            return new LotteryActivityDao().GetPrizeList(page);
        }
    }
}

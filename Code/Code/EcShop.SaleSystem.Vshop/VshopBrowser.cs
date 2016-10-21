using EcShop.Entities;
using EcShop.Entities.VShop;
using EcShop.SqlDal.Commodities;
using EcShop.SqlDal.VShop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
namespace EcShop.SaleSystem.Vshop
{
    public static class VshopBrowser
    {
        public static DataTable GetTopics(ClientType client = ClientType.VShop)
        {
            return new HomeTopicDao().GetHomeTopics(client);
        }
        public static DataSet GetHomeTopicsList(ClientType client, int pageSize, int pageIndex, ref int total)
        {
            return new HomeTopicDao().GetHomeTopicsList(client, pageSize, pageIndex, ref total);
        }
        public static TopicInfo GetTopic(int topicId)
        {
            return new TopicDao().GetTopic(topicId);
        }
        public static MessageInfo GetMessage(int messageId)
        {
            return new ReplyDao().GetMessage(messageId);
        }
        public static DataTable GetHomeProducts(ClientType client)
        {
            return new HomeProductDao().GetHomeProducts(client);
        }
        public static LotteryActivityInfo GetLotteryActivity(int activityid)
        {
            LotteryActivityInfo lotteryActivityInfo = new LotteryActivityDao().GetLotteryActivityInfo(activityid);
            if (lotteryActivityInfo != null)
            {
                lotteryActivityInfo.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryActivityInfo.PrizeSetting);
            }
            return lotteryActivityInfo;
        }
        public static int GetActivityCount(int activityId)
        {
            return new ActivitySignUpDao().GetActivityCount(activityId);
        }
        public static ActivityInfo GetActivity(int activityId)
        {
            return new ActivityDao().GetActivity(activityId);
        }
        public static bool SaveActivitySignUp(ActivitySignUpInfo info)
        {
            return new ActivitySignUpDao().SaveActivitySignUp(info);
        }
        public static LotteryTicketInfo GetLotteryTicket(int activityid)
        {
            LotteryTicketInfo lotteryTicket = new LotteryActivityDao().GetLotteryTicket(activityid);
            if (lotteryTicket != null)
            {
                lotteryTicket.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryTicket.PrizeSetting);
            }
            return lotteryTicket;
        }
        public static bool HasSignUp(int activityId, int userId)
        {
            return new PrizeRecordDao().HasSignUp(activityId, userId);
        }
        public static int GetCountBySignUp(int activityId)
        {
            return new PrizeRecordDao().GetCountBySignUp(activityId);
        }
        public static bool OpenTicket(int ticketId)
        {
            LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(ticketId);
            bool result;
            if (new PrizeRecordDao().OpenTicket(ticketId, lotteryTicket.PrizeSettingList))
            {
                lotteryTicket.IsOpened = true;
                result = new LotteryActivityDao().UpdateLotteryTicket(lotteryTicket);
            }
            else
            {
                result = false;
            }
            return result;
        }
        public static bool UpdatePrizeRecord(PrizeRecordInfo model)
        {
            return new PrizeRecordDao().UpdatePrizeRecord(model);
        }
        public static int AddPrizeRecord(PrizeRecordInfo model)
        {
            return new PrizeRecordDao().AddPrizeRecord(model);
        }
        public static bool UpdatePrizeRecord(int activityId, int userId, string realName, string cellPhone)
        {
            PrizeRecordDao prizeRecordDao = new PrizeRecordDao();
            PrizeRecordInfo userPrizeRecord = prizeRecordDao.GetUserPrizeRecord(activityId);
            userPrizeRecord.UserID = userId;
            userPrizeRecord.RealName = realName;
            userPrizeRecord.CellPhone = cellPhone;
            return prizeRecordDao.UpdatePrizeRecord(userPrizeRecord);
        }
        public static int GetUserPrizeCount(int activityid)
        {
            return new PrizeRecordDao().GetUserPrizeCount(activityid);
        }
        public static PrizeRecordInfo GetUserPrizeRecord(int activityid)
        {
            return new PrizeRecordDao().GetUserPrizeRecord(activityid);
        }
        public static List<PrizeRecordInfo> GetPrizeList(PrizeQuery page)
        {
            return new PrizeRecordDao().GetPrizeList(page);
        }
        public static IList<BannerInfo> GetAllBanners(ClientType clientType)
        {
            return new BannerDao().GetAllBanners(clientType);
        }
        public static IList<NavigateInfo> GetAllNavigate(ClientType clientType)
        {
            return new BannerDao().GetAllNavigate(clientType);
        }
        public static IList<TplCfgInfo> GetTplCfgInfoList(ClientType client, int type, int pageSize, int pageIndex, ref int total)
        {
            return new BannerDao().GetTplCfgInfoList(client, type, pageSize, pageIndex, ref total);
        }

        public static IList<HotSaleInfo> GetAllHotSaleNomarl(ClientType clientType)
        {
            return new BannerDao().GetAllHotSaleNomarl(clientType);
        }

        public static IList<HotSaleInfo> GetAllHotSaleNomarl(ClientType clientType, int supplierId)
        {
            return new BannerDao().GetAllHotSaleNomarl(clientType, supplierId);
        }

        /// <summary>
        /// 供应商首页热卖商品
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="between">开始页</param>
        /// <param name="and">结束页</param>
        /// <param name="supplierid">供应商ID</param>
        /// <param name="clientType">类型</param>
        /// <returns></returns>
        public static DataTable GetAllHotSaleNomarl(int count, int between, int and, int supplierid, ClientType clientType)
        {
            return new BannerDao().GetAllHotSaleNomarl(count, between, and, supplierid, clientType);
        }

        public static IList<HotSaleInfo> GetAllHotSaleTop(ClientType clientType)
        {
            return new BannerDao().GetAllHotSaleTop(clientType);
        }

        public static IList<HotSaleInfo> GetAllHotSaleTop(ClientType clientType, int supplierId)
        {
            return new BannerDao().GetAllHotSaleTop(clientType, supplierId);
        }

        public static IList<HotSaleInfo> GetAllHotSale(ClientType clientType)
        {
            return new BannerDao().GetAllHotSale(clientType);
        }
        public static IList<RecommendInfo> GetAllRecommend(ClientType clientType)
        {
            return new BannerDao().GetAllRecommend(clientType);
        }
        public static IList<RecommendInfo> GetAllRecommendTop(ClientType clientType)
        {
            return new BannerDao().GetAllRecommendTop(clientType);
        }

        public static IList<RecommendInfo> GetAllRecommendTop(ClientType clientType,int supplierId)
        {
            return new BannerDao().GetAllRecommendTop(clientType, supplierId);
        }

        public static IList<RecommendInfo> GetAllRecommendNormal(ClientType clientType)
        {
            return new BannerDao().GetAllRecommendNormal(clientType);
        }

        public static IList<RecommendInfo> GetAllRecommendNormal(ClientType clientType, int supplierId)
        {
            return new BannerDao().GetAllRecommendNormal(clientType, supplierId);
        }

        public static IList<PromotionalInfo> GetAllPromotional(ClientType clientType)
        {
            return new BannerDao().GetAllPromotional(clientType);
        }

        public static DataTable GetHomeActivityTopics(ClientType client, string topicids)
        {
            return new HomeTopicDao().GetHomeActivityTopics(client, topicids);
        }
    }
}

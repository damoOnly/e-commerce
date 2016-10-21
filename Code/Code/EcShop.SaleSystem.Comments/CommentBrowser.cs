using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Comments;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.SqlDal.Comments;
using EcShop.SqlDal.Promotions;
using EcShop.SqlDal.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Caching;
using System.Xml;
namespace EcShop.SaleSystem.Comments
{
    public static class CommentBrowser
    {
        public static IList<FriendlyLinksInfo> GetFriendlyLinksIsVisible(int? num)
        {
            return new FriendlyLinkDao().GetFriendlyLinksIsVisible(num);
        }
        public static DataSet GetHelps()
        {
            return new HelpDao().GetHelps();
        }
        public static DataTable GetFooterHelps()//新增获取脚部帮助文档信息
        {
            return new HelpDao().GetFooterHelps();
        }
        public static List<AfficheInfo> GetAfficheList()
        {
            return new AfficheDao().GetAfficheList();
        }
        public static AfficheInfo GetAffiche(int afficheId)
        {
            return new AfficheDao().GetAffiche(afficheId);
        }
        public static AfficheInfo GetFrontOrNextAffiche(int afficheId, string type)
        {
            return new AfficheDao().GetFrontOrNextAffiche(afficheId, type);
        }
        public static DataSet GetVoteByIsShow()
        {
            return new VoteDao().GetVotes(new bool?(true));
        }
        public static VoteInfo GetVoteById(long voteId)
        {
            return new VoteDao().GetVoteById(voteId);
        }
        public static IList<VoteItemInfo> GetVoteItems(long voteId)
        {
            return new VoteDao().GetVoteItems(voteId);
        }
        public static VoteItemInfo GetVoteItemById(long voteItemId)
        {
            return new VoteDao().GetVoteItem(voteItemId);
        }
        public static int Vote(long voteItemId)
        {
            return new VoteDao().Vote(voteItemId);
        }
        public static DataTable GetHotKeywords(int categoryId, int hotKeywordsNum)
        {
            return new HotkeywordDao().GetHotKeywords(categoryId, hotKeywordsNum);
        }

        public static DataTable GetHotKeywords(int? categoryId, int hotKeywordsNum ,ClientType clientType)
        {
            return new HotkeywordDao().GetHotKeywords(categoryId, hotKeywordsNum, clientType);
        }
        public static DataSet GetAllHotKeywords()
        {
            return new HotkeywordDao().GetAllHotKeywords();
        }
        public static ArticleCategoryInfo GetArticleCategory(int categoryId)
        {
            return new ArticleCategoryDao().GetArticleCategory(categoryId);
        }
        public static ArticleInfo GetArticle(int articleId)
        {
            return new ArticleDao().GetArticle(articleId);
        }
        public static ArticleInfo GetFrontOrNextArticle(int articleId, string type, int categoryId)
        {
            return new ArticleDao().GetFrontOrNextArticle(articleId, type, categoryId);
        }
        public static IList<ArticleInfo> GetArticleList(int categoryId, int maxNum)
        {
            return new ArticleDao().GetArticleList(categoryId, maxNum);
        }
        public static DbQueryResult GetArticleList(ArticleQuery articleQuery)
        {
            return new ArticleDao().GetArticleList(articleQuery);
        }
        public static IList<ArticleCategoryInfo> GetArticleMainCategories()
        {
            return new ArticleCategoryDao().GetMainArticleCategories();
        }
        public static DataTable GetArticlProductList(int arctid)
        {
            Pagination pagination = new Pagination();
            pagination.PageIndex = 1;
            pagination.PageSize = 20;
            return new ArticleDao().GetRelatedArticsProducts(pagination, arctid).Data as DataTable;
        }
        public static HelpCategoryInfo GetHelpCategory(int categoryId)
        {
            return new HelpCategoryDao().GetHelpCategory(categoryId);
        }
        public static DbQueryResult GetHelpList(HelpQuery helpQuery)
        {
            return new HelpDao().GetHelpList(helpQuery);
        }
        public static DataSet GetHelpTitleList()
        {
            return new HelpDao().GetHelps();
        }
        public static DataSet GetAllHelps()
        {
            return new HelpDao().GetAllHelps();
        }
        public static IList<HelpCategoryInfo> GetHelpCategorys()
        {
            return new HelpCategoryDao().GetHelpCategorys();
        }
        public static HelpInfo GetHelp(int helpId)
        {
            return new HelpDao().GetHelp(helpId);
        }
        public static HelpInfo GetFrontOrNextHelp(int helpId, int categoryId, string type)
        {
            return new HelpDao().GetFrontOrNextHelp(helpId, categoryId, type);
        }

        public static DataTable SearchHelps(Pagination pagination,string  searchcontent, out int totalHelps)
        {
            return new HelpDao().SearchHelps(pagination, searchcontent, out totalHelps);
        }
        public static DataTable GetPromotes(Pagination pagination, int promotiontype, out int totalPromotes)
        {
            return new PromotionDao().GetPromotes(pagination, promotiontype, out totalPromotes);
        }
        public static PromotionInfo GetPromote(int activityId)
        {
            return new PromotionDao().GetPromotion(activityId);
        }
        public static PromotionInfo GetFrontOrNextPromoteInfo(PromotionInfo promote, string type)
        {
            return new PromotionDao().GetFrontOrNextPromoteInfo(promote, type);
        }
        public static bool InsertLeaveComment(LeaveCommentInfo leave)
        {
            Globals.EntityCoding(leave, true);
            return new LeaveCommentDao().InsertLeaveComment(leave);
        }
        public static DbQueryResult GetLeaveComments(LeaveCommentQuery query)
        {
            return new LeaveCommentDao().GetLeaveComments(query);
        }
        public static bool SendMessage(MessageBoxInfo messageBoxInfo)
        {
            return new MessageDao().InsertMessage(messageBoxInfo);
        }
        public static int DeleteMemberMessages(IList<long> messageList)
        {
            return new MessageDao().DeleteMemberMessages(messageList);
        }

        public static bool DeleteMessage(long messageid, string username)
        {
            return new MessageDao().DeleteMessage(messageid, username);
        }


        public static DbQueryResult GetMemberSendedMessages(MessageBoxQuery query)
        {
            return new MessageDao().GetMemberSendedMessages(query);
        }
        public static DbQueryResult GetMemberReceivedMessages(MessageBoxQuery query)
        {
            return new MessageDao().GetMemberReceivedMessages(query);
        }
        public static MessageBoxInfo GetMemberMessage(long messageId)
        {
            return new MessageDao().GetMemberMessage(messageId);
        }
        public static bool PostMemberMessageIsRead(long messageId)
        {
            return new MessageDao().PostMemberMessageIsRead(messageId);
        }
        public static int GetUnreadMessageCount(string accepter)
        {
            return new MessageDao().GetUnreadMessageCount(accepter);
        }

        /// <summary>
        /// 设置会员所有未读消息为已读
        /// </summary>
        /// <param name="accepter"></param>
        /// <returns></returns>
         public static int SetMemberMessageIsRead(string accepter)
        {
            return new MessageDao().SetMemberMessageIsRead(accepter);
        }

        public static XmlDocument GetArticleSubjectDocument()
        {
            string key = "ArticleSubjectFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("ArticleSubjectFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            XmlDocument xmlDocument = HiCache.Get(key) as XmlDocument;
            if (xmlDocument == null)
            {
                HttpContext context = HiContext.Current.Context;
                string filename = context.Request.MapPath(HiContext.Current.GetSkinPath() + "/ArticleSubjects.xml");
                xmlDocument = new XmlDocument();
                xmlDocument.Load(filename);
                HiCache.Max(key, xmlDocument, new CacheDependency(filename));
            }
            return xmlDocument;
        }

        public static DataTable GetHotKeywords(int clientType, int hotKeywordsNum, bool IsRandom)
        {
            return new HotkeywordDao().GetHotKeywords(clientType, hotKeywordsNum, IsRandom);
        }

        public static DataTable GetMyHotKeywords(int userId, int hotKeywordsNum, bool IsRandom)
        {
            return new HotkeywordDao().GetMyHotKeywords(userId, hotKeywordsNum, IsRandom);
        }


        public static DataTable GetHelpCategories()
        {
            return new HelpDao().GetHelpCategories();
        }

        public static DataTable GetHelpByCateGoryId(int CategoryId)
        {
            return new HelpDao().GetHelpByCateGoryId(CategoryId);
        }

        public static DbQueryResult GetAfficheList(AfficheQuery afficheQuery)
        {
            return new AfficheDao().GetAfficheList(afficheQuery);
        }
    }
}

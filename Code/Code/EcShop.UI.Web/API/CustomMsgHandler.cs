using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using Ecdev.Weixin.MP;
using Ecdev.Weixin.MP.Domain;
using Ecdev.Weixin.MP.Handler;
using Ecdev.Weixin.MP.Request;
using Ecdev.Weixin.MP.Request.Event;
using Ecdev.Weixin.MP.Response;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
namespace EcShop.UI.Web.API
{
    public class CustomMsgHandler : RequestHandler
    {
        public CustomMsgHandler(System.IO.Stream inputStream)
            : base(inputStream)
        {
        }
        public bool IsOpenManyService()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            return masterSettings.OpenManyService;
        }
        public AbstractResponse GotoManyCustomerService(AbstractRequest requestMessage)
        {
            if (!this.IsOpenManyService())
            {
                return null;
            }
            return new AbstractResponse
            {
                FromUserName = requestMessage.ToUserName,
                ToUserName = requestMessage.FromUserName,
                MsgType = ResponseMsgType.transfer_customer_service
            };
        }
        public override AbstractResponse DefaultResponse(AbstractRequest requestMessage)
        {
            ReplyInfo mismatchReply = ReplyHelper.GetMismatchReply();
            if (mismatchReply == null && this.IsOpenManyService())
            {
                return this.GotoManyCustomerService(requestMessage);
            }
            AbstractResponse response = this.GetResponse(mismatchReply, requestMessage.FromUserName);
            if (response == null)
            {
                return null;
                //return this.GotoManyCustomerService(requestMessage);
            }
            response.ToUserName = requestMessage.FromUserName;
            response.FromUserName = requestMessage.ToUserName;
            return response;
        }
        public override AbstractResponse OnTextRequest(TextRequest textRequest)
        {
            System.Collections.Generic.IList<ReplyInfo> kefuReplies = ReplyHelper.GetReplies(ReplyType.Kefu);
            if (!string.IsNullOrEmpty(textRequest.Content) && this.IsOpenManyService() && kefuReplies != null && kefuReplies.Count > 0)
            {
                List<string> kefuKeys = new List<string>();
                foreach (ReplyInfo item in kefuReplies)
                {
                    if (item.MatchType == MatchType.Equal)
                    {
                        if (item.Keys == textRequest.Content)
                        {
                            return this.GotoManyCustomerService(textRequest);
                        }
                    }
                    else if (item.MatchType == MatchType.Like)
                    {
                        string[] keys = item.Keys.Split(',');
                        kefuKeys.AddRange(keys);
                    }
                }
                foreach (var key in kefuKeys)
                {
                    if (textRequest.Content.Contains(key))
                    {
                        return this.GotoManyCustomerService(textRequest);
                    }
                }
            }
            AbstractResponse keyResponse = this.GetKeyResponse(textRequest.Content, textRequest);
            if (keyResponse != null)
            {
                return keyResponse;
            }
            System.Collections.Generic.IList<ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Keys);
            //if (replies == null || (replies.Count == 0 && this.IsOpenManyService()))
            //{
            //    this.GotoManyCustomerService(textRequest);
            //}
            if (replies != null && replies.Count > 0)
            {
                foreach (ReplyInfo current in replies)
                {
                    if (current.MatchType == MatchType.Equal && current.Keys == textRequest.Content)
                    {
                        AbstractResponse response = this.GetResponse(current, textRequest.FromUserName);
                        response.ToUserName = textRequest.FromUserName;
                        response.FromUserName = textRequest.ToUserName;
                        AbstractResponse result = response;
                        return result;
                    }
                    if (current.MatchType == MatchType.Like && current.Keys.Contains(textRequest.Content))
                    {
                        AbstractResponse response2 = this.GetResponse(current, textRequest.FromUserName);
                        response2.ToUserName = textRequest.FromUserName;
                        response2.FromUserName = textRequest.ToUserName;
                        AbstractResponse result = response2;
                        return result;
                    }
                }
            }
            return this.DefaultResponse(textRequest);
        }
        public override AbstractResponse OnEvent_SubscribeRequest(SubscribeEventRequest subscribeEventRequest)
        {
            ReplyInfo subscribeReply = ReplyHelper.GetSubscribeReply();
            if (subscribeReply == null)
            {
                return null;
            }
            subscribeReply.Keys = "登录";
            AbstractResponse response = this.GetResponse(subscribeReply, subscribeEventRequest.FromUserName);
            if (response == null)
            {
                this.GotoManyCustomerService(subscribeEventRequest);
            }
            response.ToUserName = subscribeEventRequest.FromUserName;
            response.FromUserName = subscribeEventRequest.ToUserName;
            // 关注成功送优惠卷信息
            AddCoupon(subscribeEventRequest.FromUserName);
            return response;
        }

        /// <summary>
        /// 插入优惠券
        /// </summary>
        private void AddCoupon(string openId)
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_AddSubscribeCoupon");
            database.AddInParameter(storedProcCommand, "openId", DbType.String, openId);
            database.AddInParameter(storedProcCommand, "sendType", DbType.Int32, 2);
            database.ExecuteNonQuery(storedProcCommand);
        }

        public override AbstractResponse OnEvent_ClickRequest(ClickEventRequest clickEventRequest)
        {
            int menuId = System.Convert.ToInt32(clickEventRequest.EventKey);
            MenuInfo menu = VShopHelper.GetMenu(menuId);
            if (menu == null)
            {
                return null;
            }
            ReplyInfo reply = ReplyHelper.GetReply(menu.ReplyId);
            if (reply == null)
            {
                return null;
            }
            //if (reply.ReplyType == ReplyType.Kefu)不合法
            //{
            //    this.GotoManyCustomerService(clickEventRequest);
            //}
            AbstractResponse keyResponse = this.GetKeyResponse(reply.Keys, clickEventRequest);
            if (keyResponse != null)
            {
                return keyResponse;
            }
            AbstractResponse response = this.GetResponse(reply, clickEventRequest.FromUserName);
            if (response == null)
            {
                this.GotoManyCustomerService(clickEventRequest);
            }
            response.ToUserName = clickEventRequest.FromUserName;
            response.FromUserName = clickEventRequest.ToUserName;
            return response;
        }
        private AbstractResponse GetKeyResponse(string key, AbstractRequest request)
        {
            System.Collections.Generic.IList<ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Topic);
            if (replies != null && replies.Count > 0)
            {
                foreach (ReplyInfo current in replies)
                {
                    if (current.Keys == key)
                    {
                        TopicInfo topicInfo = VShopHelper.Gettopic(current.ActivityId);
                        if (topicInfo != null)
                        {
                            AbstractResponse result = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                //修改1
                                Articles = 
								{
									new Article
									{
										Description = topicInfo.Title,
										PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, topicInfo.IconUrl),
										Title = topicInfo.Title,
										Url = string.Format("http://{0}/vshop/Topics.aspx?TopicId={1}", System.Web.HttpContext.Current.Request.Url.Host, topicInfo.TopicId)
									}
								}
                            };
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> replies2 = ReplyHelper.GetReplies(ReplyType.Vote);
            if (replies2 != null && replies2.Count > 0)
            {
                foreach (ReplyInfo current2 in replies2)
                {
                    if (current2.Keys == key)
                    {
                        VoteInfo voteById = StoreHelper.GetVoteById((long)current2.ActivityId);
                        if (voteById != null && voteById.IsBackup)
                        {
                            AbstractResponse result = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                //修改1
                                Articles = 
								{
									new Article
									{
										Description = voteById.VoteName,
										PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, voteById.ImageUrl),
										Title = voteById.VoteName,
										Url = string.Format("http://{0}/vshop/Vote.aspx?voteId={1}", System.Web.HttpContext.Current.Request.Url.Host, voteById.VoteId)
									}
								}
                            };
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> replies3 = ReplyHelper.GetReplies(ReplyType.Wheel);
            if (replies3 != null && replies3.Count > 0)
            {
                foreach (ReplyInfo current3 in replies3)
                {
                    if (current3.Keys == key)
                    {
                        LotteryActivityInfo lotteryActivityInfo = VShopHelper.GetLotteryActivityInfo(current3.ActivityId);
                        if (lotteryActivityInfo != null)
                        {
                            AbstractResponse result = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                //修改1
                                Articles = 
								{
									new Article
									{
										Description = lotteryActivityInfo.ActivityDesc,
										PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryActivityInfo.ActivityPic),
										Title = lotteryActivityInfo.ActivityName,
										Url = string.Format("http://{0}/vshop/BigWheel.aspx?activityId={1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryActivityInfo.ActivityId)
									}
								}
                            };
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> replies4 = ReplyHelper.GetReplies(ReplyType.Scratch);
            if (replies4 != null && replies4.Count > 0)
            {
                foreach (ReplyInfo current4 in replies4)
                {
                    if (current4.Keys == key)
                    {
                        LotteryActivityInfo lotteryActivityInfo2 = VShopHelper.GetLotteryActivityInfo(current4.ActivityId);
                        if (lotteryActivityInfo2 != null)
                        {
                            AbstractResponse result = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                //修改1
                                Articles = 
								{
									new Article
									{
										Description = lotteryActivityInfo2.ActivityDesc,
										PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryActivityInfo2.ActivityPic),
										Title = lotteryActivityInfo2.ActivityName,
										Url = string.Format("http://{0}/vshop/Scratch.aspx?activityId={1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryActivityInfo2.ActivityId)
									}
								}
                            };
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> replies5 = ReplyHelper.GetReplies(ReplyType.SmashEgg);
            if (replies5 != null && replies5.Count > 0)
            {
                foreach (ReplyInfo current5 in replies5)
                {
                    if (current5.Keys == key)
                    {
                        LotteryActivityInfo lotteryActivityInfo3 = VShopHelper.GetLotteryActivityInfo(current5.ActivityId);
                        if (lotteryActivityInfo3 != null)
                        {
                            AbstractResponse result = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                //修改1
                                Articles = 
								{
									new Article
									{
										Description = lotteryActivityInfo3.ActivityDesc,
										PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryActivityInfo3.ActivityPic),
										Title = lotteryActivityInfo3.ActivityName,
										Url = string.Format("http://{0}/vshop/SmashEgg.aspx?activityId={1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryActivityInfo3.ActivityId)
									}
								}
                            };
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> replies6 = ReplyHelper.GetReplies(ReplyType.SignUp);
            if (replies6 != null && replies6.Count > 0)
            {
                foreach (ReplyInfo current6 in replies6)
                {
                    if (current6.Keys == key)
                    {
                        ActivityInfo activity = VShopHelper.GetActivity(current6.ActivityId);
                        if (activity != null)
                        {
                            AbstractResponse result = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                //修改1
                                Articles = 
								{
									new Article
									{
										Description = activity.Description,
										PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, activity.PicUrl),
										Title = activity.Name,
										Url = string.Format("http://{0}/vshop/Activity.aspx?id={1}", System.Web.HttpContext.Current.Request.Url.Host, activity.ActivityId)
									}
								}
                            };
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> replies7 = ReplyHelper.GetReplies(ReplyType.Ticket);
            if (replies7 != null && replies7.Count > 0)
            {
                foreach (ReplyInfo current7 in replies7)
                {
                    if (current7.Keys == key)
                    {
                        LotteryTicketInfo lotteryTicket = VShopHelper.GetLotteryTicket(current7.ActivityId);
                        if (lotteryTicket != null)
                        {
                            AbstractResponse result = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                //修改1
                                Articles = 
								{
									new Article
									{
										Description = lotteryTicket.ActivityDesc,
										PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryTicket.ActivityPic),
										Title = lotteryTicket.ActivityName,
										Url = string.Format("http://{0}/vshop/SignUp.aspx?id={1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryTicket.ActivityId)
									}
								}
                            };
                            return result;
                        }
                    }
                }
            }
            return null;
        }
        public AbstractResponse GetResponse(ReplyInfo reply, string openId)
        {
            if (reply.MessageType == MessageType.Text)
            {
                TextReplyInfo textReplyInfo = reply as TextReplyInfo;
                TextResponse textResponse = new TextResponse();
                textResponse.CreateTime = System.DateTime.Now;
                textResponse.Content = textReplyInfo.Text;
                if (reply.Keys == "登录")
                {
                    string arg = string.Format("http://{0}/Vshop/Login.aspx?SessionId={1}", System.Web.HttpContext.Current.Request.Url.Host, openId);
                    textResponse.Content = textResponse.Content.Replace("$login$", string.Format("<a href=\"{0}\">一键登录</a>", arg));
                }
                return textResponse;
            }
            NewsResponse newsResponse = new NewsResponse();
            newsResponse.CreateTime = System.DateTime.Now;
            newsResponse.Articles = new System.Collections.Generic.List<Article>();
            foreach (NewsMsgInfo current in (reply as NewsReplyInfo).NewsMsg)
            {
                Article item = new Article
                {
                    Description = current.Description,
                    PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, current.PicUrl),
                    Title = current.Title,
                    Url = string.IsNullOrEmpty(current.Url) ? string.Format("http://{0}/Vshop/ImageTextDetails.aspx?messageId={1}", System.Web.HttpContext.Current.Request.Url.Host, current.Id) : current.Url
                };
                newsResponse.Articles.Add(item);
            }
            return newsResponse;
        }
    }
}

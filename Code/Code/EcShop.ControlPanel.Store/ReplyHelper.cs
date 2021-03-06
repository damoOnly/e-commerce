using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.SqlDal.VShop;
using System;
using System.Collections.Generic;
namespace EcShop.ControlPanel.Store
{
	public class ReplyHelper
	{
		public static void DeleteNewsMsg(int id)
		{
			new ReplyDao().DeleteNewsMsg(id);
		}
		public static bool HasReplyKey(string key)
		{
			return new ReplyDao().HasReplyKey(key);
		}
		public static ReplyInfo GetReply(int id)
		{
			return new ReplyDao().GetReply(id);
		}
		public static bool DeleteReply(int id)
		{
			return new ReplyDao().DeleteReply(id);
		}
		public static bool SaveReply(ReplyInfo reply)
		{
			reply.LastEditDate = DateTime.Now;
			reply.LastEditor = HiContext.Current.User.Username;
			return new ReplyDao().SaveReply(reply);
		}
		public static bool UpdateReply(ReplyInfo reply)
		{
			reply.LastEditDate = DateTime.Now;
			reply.LastEditor = HiContext.Current.User.Username;
			return new ReplyDao().UpdateReply(reply);
		}
		public static bool UpdateReplyRelease(int id)
		{
			return new ReplyDao().UpdateReplyRelease(id);
		}
		public static IList<ReplyInfo> GetAllReply()
		{
			return new ReplyDao().GetAllReply();
		}
		public static IList<ReplyInfo> GetReplies(ReplyType type)
		{
			return new ReplyDao().GetReplies(type);
		}
		public static ReplyInfo GetSubscribeReply()
		{
			IList<ReplyInfo> replies = new ReplyDao().GetReplies(ReplyType.Subscribe);
			ReplyInfo result;
			if (replies != null && replies.Count > 0)
			{
				result = replies[0];
			}
			else
			{
				result = null;
			}
			return result;
		}
		public static ReplyInfo GetMismatchReply()
		{
			IList<ReplyInfo> replies = new ReplyDao().GetReplies(ReplyType.NoMatch);
			ReplyInfo result;
			if (replies != null && replies.Count > 0)
			{
				result = replies[0];
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}

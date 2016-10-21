using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SqlDal.Comments;
using System;
using System.Collections.Generic;
using System.Data;
namespace EcShop.ControlPanel.Comments
{
	public sealed class NoticeHelper
	{
		private NoticeHelper()
		{
		}
		public static List<AfficheInfo> GetAfficheList()
		{
			return new AfficheDao().GetAfficheList();
		}
		public static AfficheInfo GetAffiche(int afficheId)
		{
			return new AfficheDao().GetAffiche(afficheId);
		}
		public static int DeleteAffiches(List<int> affiches)
		{
			int result;
			if (affiches == null || affiches.Count == 0)
			{
				result = 0;
			}
			else
			{
				int num = 0;
				foreach (int current in affiches)
				{
					if (new AfficheDao().DeleteAffiche(current))
					{
						num++;
					}
				}
				result = num;
			}
			return result;
		}
		public static bool CreateAffiche(AfficheInfo affiche)
		{
			bool result;
			if (null == affiche)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(affiche, true);
				result = new AfficheDao().AddAffiche(affiche);
			}
			return result;
		}
		public static bool UpdateAffiche(AfficheInfo affiche)
		{
			bool result;
			if (null == affiche)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(affiche, true);
				result = new AfficheDao().UpdateAffiche(affiche);
			}
			return result;
		}
		public static bool DeleteAffiche(int afficheId)
		{
			return new AfficheDao().DeleteAffiche(afficheId);
		}
		public static LeaveCommentInfo GetLeaveComment(long leaveId)
		{
			return new LeaveCommentDao().GetLeaveComment(leaveId);
		}
		public static DbQueryResult GetLeaveComments(LeaveCommentQuery query)
		{
			return new LeaveCommentDao().GetLeaveComments(query);
		}
		public static bool DeleteLeaveComment(long leaveId)
		{
			return new LeaveCommentDao().DeleteLeaveComment(leaveId);
		}
		public static int DeleteLeaveComments(IList<long> leaveIds)
		{
			int result;
			if (leaveIds == null || leaveIds.Count <= 0)
			{
				result = 0;
			}
			else
			{
				int num = 0;
				foreach (long current in leaveIds)
				{
					if (new LeaveCommentDao().DeleteLeaveComment(current))
					{
						num++;
					}
				}
				result = num;
			}
			return result;
		}
		public static int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply)
		{
			leaveReply.ReplyDate = DateTime.Now;
			return new LeaveCommentDao().ReplyLeaveComment(leaveReply);
		}
		public static bool DeleteLeaveCommentReply(long leaveReplyId)
		{
			return new LeaveCommentDao().DeleteLeaveCommentReply(leaveReplyId);
		}
		public static DataTable GetReplyLeaveComments(long leaveId)
		{
			return new LeaveCommentDao().GetReplyLeaveComments(leaveId);
		}
		public static DbQueryResult GetManagerReceivedMessages(MessageBoxQuery query, UserRole role)
		{
			return new MessageBoxDao().GetManagerReceivedMessages(query, role);
		}
		public static DbQueryResult GetManagerSendedMessages(MessageBoxQuery query, UserRole role)
		{
			return new MessageBoxDao().GetManagerSendedMessages(query, role);
		}
		public static MessageBoxInfo GetManagerMessage(long messageId)
		{
			return new MessageBoxDao().GetManagerMessage(messageId);
		}
		public static int SendMessageToMember(IList<MessageBoxInfo> messageBoxInfos)
		{
			int num = 0;
			foreach (MessageBoxInfo current in messageBoxInfos)
			{
				if (new MessageBoxDao().InsertMessage(current, UserRole.Member))
				{
					num++;
				}
			}
			return num;
		}
		public static bool PostManagerMessageIsRead(long messageId)
		{
			return new MessageBoxDao().PostManagerMessageIsRead(messageId);
		}
		public static int DeleteManagerMessages(IList<long> messageList)
		{
			return new MessageBoxDao().DeleteManagerMessages(messageList);
		}
		public static int GetMemberUnReadMessageNum()
		{
			return new MessageBoxDao().GetMemberUnReadMessageNum();
		}
		public static IList<Member> GetMembersByRank(int? gradeId)
		{
			return new MessageBoxDao().GetMembersByRank(gradeId);
		}

        public static DataTable GetAffiches(int top)
        {
            return new AfficheDao().GetAffiches(top);
        }

        public static DbQueryResult GetAfficheList(AfficheQuery afficheQuery)
        {
            return new AfficheDao().GetAfficheList(afficheQuery);
        }
	}
}

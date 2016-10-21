using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Comments
{
	public class LeaveCommentQuery : Pagination
	{
		public int? AgentId
		{
			get;
			set;
		}
		public MessageStatus MessageStatus
		{
			get;
			set;
		}
        public int? FeedbackType { get; set; }
	}
}

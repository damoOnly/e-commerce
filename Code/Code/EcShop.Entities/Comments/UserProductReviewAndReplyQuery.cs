using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Comments
{
	public class UserProductReviewAndReplyQuery : Pagination
	{
		public int ProductId
		{
			get;
			set;
		}
	}
}

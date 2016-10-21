using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Comments
{
	public class ProductReviewAndReplyQuery : Pagination
	{
		public long ReviewId
		{
			get;
			set;
		}
		public virtual int ProductId
		{
			get;
			set;
		}
	}
}

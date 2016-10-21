using EcShop.Core;
using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Comments
{
	public class ProductReviewQuery : Pagination
	{
		[HtmlCoding]
		public string Keywords
		{
			get;
			set;
		}
		[HtmlCoding]
		public string ProductCode
		{
			get;
			set;
		}
		public int? CategoryId
		{
			get;
			set;
		}
		public int productId
		{
			get;
			set;
		}
	}
}

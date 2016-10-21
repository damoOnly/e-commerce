using EcShop.Core.Enums;
using System;
namespace EcShop.Core.Entities
{
	public class Pagination
	{
		public int PageIndex
		{
			get;
			set;
		}
		public int PageSize
		{
			get;
			set;
		}
		public string SortBy
		{
			get;
			set;
		}
		public SortAction SortOrder
		{
			get;
			set;
		}
		public bool IsCount
		{
			get;
			set;
		}
		public Pagination()
		{
			this.IsCount = true;
			this.PageSize = 10;
		}
	}
}

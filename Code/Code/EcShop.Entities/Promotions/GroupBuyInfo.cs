using EcShop.Core;
using System;
using System.Collections.Generic;
namespace EcShop.Entities.Promotions
{
	public class GroupBuyInfo
	{
		private System.Collections.Generic.IList<GropBuyConditionInfo> groupBuyConditions;
		public int GroupBuyId
		{
			get;
			set;
		}
		public int ProductId
		{
			get;
			set;
		}
		public decimal NeedPrice
		{
			get;
			set;
		}
		public System.DateTime StartDate
		{
			get;
			set;
		}
		public System.DateTime EndDate
		{
			get;
			set;
		}
		public int MaxCount
		{
			get;
			set;
		}
		[HtmlCoding]
		public string Content
		{
			get;
			set;
		}
		public GroupBuyStatus Status
		{
			get;
			set;
		}
		public int ProdcutQuantity
		{
			get;
			set;
		}
		public System.Collections.Generic.IList<GropBuyConditionInfo> GroupBuyConditions
		{
			get
			{
				if (this.groupBuyConditions == null)
				{
					this.groupBuyConditions = new System.Collections.Generic.List<GropBuyConditionInfo>();
				}
				return this.groupBuyConditions;
			}
		}
		public int Count
		{
			get;
			set;
		}
		public decimal Price
		{
			get;
			set;
		}
		public int SoldCount
		{
			get;
			set;
		}
        public int? SupplierId
        {
            get;
            set;
        }
	}
}

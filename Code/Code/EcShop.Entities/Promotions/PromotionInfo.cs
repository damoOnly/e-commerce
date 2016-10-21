using System;
using System.Collections.Generic;
namespace EcShop.Entities.Promotions
{
	[System.Serializable]
	public class PromotionInfo
	{
		public int ActivityId
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public PromoteType PromoteType
		{
			get;
			set;
		}
		public decimal Condition
		{
			get;
			set;
		}
		public decimal DiscountValue
		{
			get;
			set;
		}
        public int IsAscend
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
		public string Description
		{
			get;
			set;
		}
		private System.Collections.Generic.IList<int> memberGradeIds
		{
			get;
			set;
		}
		public System.Collections.Generic.IList<int> MemberGradeIds
		{
			get
			{
				if (this.memberGradeIds == null)
				{
					this.memberGradeIds = new System.Collections.Generic.List<int>();
				}
				return this.memberGradeIds;
			}
			set
			{
				this.memberGradeIds = value;
			}
		}
		public int GiftID
		{
			get;
			set;
		}
        public int SupplierId
        {
            get;
            set;
        }
	}
}

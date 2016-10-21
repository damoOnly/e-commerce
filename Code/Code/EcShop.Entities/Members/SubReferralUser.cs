using System;
namespace EcShop.Entities.Members
{
	public class SubReferralUser
	{
		public int UserID
		{
			get;
			set;
		}
		public string UserName
		{
			get;
			set;
		}
		public string RealName
		{
			get;
			set;
		}
		public string CellPhone
		{
			get;
			set;
		}
		public int ReferralOrderNumber
		{
			get;
			set;
		}
		public decimal SubReferralSplittin
		{
			get;
			set;
		}
		public System.DateTime CreateDate
		{
			get;
			set;
		}
		public System.DateTime? ReferralAuditDate
		{
			get;
			set;
		}
		public System.DateTime? LastReferralDate
		{
			get;
			set;
		}
	}
}

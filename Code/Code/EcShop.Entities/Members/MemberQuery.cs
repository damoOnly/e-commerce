using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Members
{
	public class MemberQuery : Pagination
	{
		public string Username
		{
			get;
			set;
		}
		public string Realname
		{
			get;
			set;
		}
		public string ReferralUsername
		{
			get;
			set;
		}
		public string CellPhone
		{
			get;
			set;
		}
		public int? GradeId
		{
			get;
			set;
		}
		public int? ReferralStatus
		{
			get;
			set;
		}
		public bool? IsApproved
		{
			get;
			set;
		}
		public System.DateTime? StartTime
		{
			get;
			set;
		}
		public System.DateTime? EndTime
		{
			get;
			set;
		}
        public System.DateTime? RstStartTime
        {
            get;
            set;
        }
        public System.DateTime? RstEndTime
        {
            get;
            set;
        }
		public int? OrderNumber
		{
			get;
			set;
		}
		public decimal? OrderMoney
		{
			get;
			set;
		}
		public string CharSymbol
		{
			get;
			set;
		}
		public string ClientType
		{
			get;
			set;
		}
		public bool? HasVipCard
		{
			get;
			set;
		}
        public int? IsDefault
        {
            get;
            set;
        }

        //注册用户类型
        public int? UserType
        {
            get;
            set;
        }
	}
}

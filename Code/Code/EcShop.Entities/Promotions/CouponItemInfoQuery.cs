using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Promotions
{
	public class CouponItemInfoQuery : Pagination
	{
		public int? CouponId
		{
			get;
			set;
		}
		public string OrderId
		{
			get;
			set;
		}
		public string UserName
		{
			get;
			set;
		}
		public string CounponName
		{
			get;
			set;
		}
		public int? CouponStatus
		{
			get;
			set;
		}
        public string AddUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 发劵开始时间
        /// </summary>
        public string BeginTime
        {
            get;
            set;
        }

        /// <summary>
        /// 发劵结束时间
        /// </summary>
        public string EndTime
        {
            get;
            set;
        }
	}
}

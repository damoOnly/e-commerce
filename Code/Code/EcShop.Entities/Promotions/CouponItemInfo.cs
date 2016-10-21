using System;
namespace EcShop.Entities.Promotions
{
	public class CouponItemInfo
	{
		public int CouponId
		{
			get;
			set;
		}
		public string ClaimCode
		{
			get;
			set;
		}
		public int? UserId
		{
			get;
			set;
		}
		public string UserName
		{
			get;
			set;
		}
		public string EmailAddress
		{
			get;
			set;
		}
		public System.DateTime GenerateTime
		{
			get;
			set;
		}
		public int? CouponStatus
		{
			get;
			set;
		}
		public System.DateTime? UsedTime
		{
			get;
			set;
		}
		public string OrderId
		{
			get;
			set;
		}

        public string SendOrderId
        {
            get;
            set;
        }

        /// <summary>
        /// 添加用户ID
        /// </summary>
        public int AddUserId
        {
            get;
            set;
        }
        /// <summary>
        /// 添加用户名称
        /// </summary>
        public string AddUserName
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }

        public System.DateTime? ClosingTime
        {
            get;
            set;
        }
        public System.DateTime? StartTime
        {
            get;
            set;
        }
        
        public decimal? Amount
        {
            get;
            set;
        }
        
        public decimal? DiscountValue
        {
            get;
            set;
        }

        public int? WarnStatus
        {
            get;
            set;
        }

		public CouponItemInfo()
		{
		}
        public CouponItemInfo(int couponId, string claimCode, int? userId, string username, string emailAddress, System.DateTime generateTime, DateTime? begintime, DateTime? endtime, decimal? amount, decimal? discountValue)
        {
            this.CouponId = couponId;
            this.ClaimCode = claimCode;
            this.UserId = userId;
            this.UserName = username;
            this.EmailAddress = emailAddress;
            this.GenerateTime = generateTime;
            this.StartTime = begintime;
            this.ClosingTime = endtime;
            this.Amount = amount;
            this.DiscountValue = discountValue;
        }

        public CouponItemInfo(int couponId, string claimCode, int? userId, string username, string emailAddress, System.DateTime generateTime, string remark,int? userid)
        {
            this.CouponId = couponId;
            this.ClaimCode = claimCode;
            this.UserId = userId;
            this.UserName = username;
            this.EmailAddress = emailAddress;
            this.GenerateTime = generateTime;
            this.Remark = remark;
        }

        public CouponItemInfo(int couponId, string claimCode, int? userId, string username, string emailAddress, System.DateTime generateTime,string sendorderid)
        {
            this.CouponId = couponId;
            this.ClaimCode = claimCode;
            this.UserId = userId;
            this.UserName = username;
            this.EmailAddress = emailAddress;
            this.GenerateTime = generateTime;
            this.SendOrderId = sendorderid;
        }
	}
}

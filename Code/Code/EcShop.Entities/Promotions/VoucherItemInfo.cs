using System;
namespace EcShop.Entities.Promotions
{
    public class VoucherItemInfo
    {
        public string ClaimCode
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
        public int VoucherId
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
		public int? VoucherStatus
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

        public decimal? UsedAmount
        {
            get;
            set;
        }

        public System.DateTime? Deadline
        {
            get;
            set;
        }

        public string KeyCode
        {
            get;
            set;
        }

        public string SendOrderId
        {
            get;
            set;
        }

		public VoucherItemInfo()
		{
		}
        public VoucherItemInfo(int voucherId, string claimCode, string password,int? userId, string username, string emailAddress, System.DateTime generateTime)
		{
            this.VoucherId = voucherId;
			this.ClaimCode = claimCode;
            this.Password = password;
			this.UserId = userId;
			this.UserName = username;
			this.EmailAddress = emailAddress;
			this.GenerateTime = generateTime;
		}

        public VoucherItemInfo(int voucherId, string claimCode, string password, int? userId, string username, string emailAddress, System.DateTime generateTime,System.DateTime deadline)
        {
            this.VoucherId = voucherId;
            this.ClaimCode = claimCode;
            this.Password = password;
            this.UserId = userId;
            this.UserName = username;
            this.EmailAddress = emailAddress;
            this.GenerateTime = generateTime;
            this.Deadline = deadline;
        }

        public VoucherItemInfo(string sendOrderId,int voucherId, string claimCode, string password, int? userId, string username, string emailAddress, System.DateTime generateTime, System.DateTime deadline)
        {
            this.VoucherId = voucherId;
            this.ClaimCode = claimCode;
            this.Password = password;
            this.UserId = userId;
            this.UserName = username;
            this.EmailAddress = emailAddress;
            this.GenerateTime = generateTime;
            this.Deadline = deadline;
            this.SendOrderId = sendOrderId;
        }

        public VoucherItemInfo(int voucherId, string claimCode, string password, int? userId, string username, string emailAddress, System.DateTime generateTime, System.DateTime deadline,string keyCode)
        {
            this.VoucherId = voucherId;
            this.ClaimCode = claimCode;
            this.Password = password;
            this.UserId = userId;
            this.UserName = username;
            this.EmailAddress = emailAddress;
            this.GenerateTime = generateTime;
            this.Deadline = deadline;
            this.KeyCode = keyCode;
        }
    }
}

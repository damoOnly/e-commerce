using EcShop.Core;
using Ecdev.Components.Validation;
using Ecdev.Components.Validation.Validators;
using System;

namespace EcShop.Entities.Promotions
{
    [HasSelfValidation]
     public  class VoucherInfo
     {
        #region 构造函数
         public VoucherInfo()
        {
        }
        public VoucherInfo(string name, System.DateTime closingTime, System.DateTime startTime, string description, decimal? amount, decimal discountValue)
        {
            this.Name = name;
            this.ClosingTime = closingTime;
            this.StartTime = startTime;
            this.Description = description;
            this.Amount = amount;
            this.DiscountValue = discountValue;
        }
        public VoucherInfo(int voucherId, string name, System.DateTime closingTime, System.DateTime startTime, string description, decimal? amount, decimal discountValue)
        {
            this.VoucherId = voucherId;
            this.Name = name;
            this.ClosingTime = closingTime;
            this.StartTime = startTime;
            this.Description = description;
            this.Amount = amount;
            this.DiscountValue = discountValue;
        }
       #endregion


        public int VoucherId
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "Voucher", MessageTemplate = "现金券名称不能为空，长度限制在1-50个字符之间")]
		public string Name
		{
			get;
			set;
		}
		public System.DateTime ClosingTime
		{
			get;
			set;
		}
		public System.DateTime StartTime
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}

        //满足金额
		[NotNullValidator(Negated = true, Ruleset = "Voucher"), RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "Voucher"), ValidatorComposition(CompositionType.Or, Ruleset = "Voucher", MessageTemplate = "满足金额，金额大小0.01-1000万之间")]
		public decimal? Amount
		{
			get;
			set;
		}

        //现金券金额
		[RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "Voucher", MessageTemplate = "现金券金额不能为空，金额大小0.01-1000万之间")]
		public decimal DiscountValue
		{
			get;
			set;
		}

        //发送总数
		public int SentCount
		{
			get;
			set;
		}

        //使用总数
		public int UsedCount
		{
			get;
			set;
		}

        //有效期
        public int Validity
        {
            get;
            set;
        }
        public int SendType
        {
            get;
            set;
        }

        public string SendTypeItem
        {
            get;
            set;
        }
		
        [SelfValidation(Ruleset = "Voucher")]
		public void CompareAmount(ValidationResults result)
		{
			if (this.Amount.HasValue && this.DiscountValue > this.Amount)
			{
				result.AddResult(new ValidationResult("现金券金额不能大于满足金额", this, "", "", null));
			}
		}
    }
}

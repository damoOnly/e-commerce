using Ecdev.Components.Validation;
using Ecdev.Components.Validation.Validators;
using System;
namespace EcShop.Entities.Members
{
	public class SplittinDetailInfo
	{
		public long JournalNumber
		{
			get;
			set;
		}
		public string OrderId
		{
			get;
			set;
		}
		public int UserId
		{
			get;
			set;
		}
		public string UserName
		{
			get;
			set;
		}
		public bool IsUse
		{
			get;
			set;
		}
		public System.DateTime TradeDate
		{
			get;
			set;
		}
		public SplittingTypes TradeType
		{
			get;
			set;
		}
		public int SubUserId
		{
			get;
			set;
		}
		[NotNullValidator(Negated = true, Ruleset = "ValBalanceDetail"), RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Exclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValBalanceDetail"), ValidatorComposition(CompositionType.Or, Ruleset = "ValBalanceDetail", MessageTemplate = "本次收入的金额，金额大小正负1000万之间")]
		public decimal? Income
		{
			get;
			set;
		}
		[NotNullValidator(Negated = true, Ruleset = "ValBalanceDetail"), RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Exclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValBalanceDetail"), ValidatorComposition(CompositionType.Or, Ruleset = "ValBalanceDetail", MessageTemplate = "本次支出的金额，金额大小正负1000万之间")]
		public decimal? Expenses
		{
			get;
			set;
		}
		public decimal Balance
		{
			get;
			set;
		}
		[StringLengthValidator(0, 300, Ruleset = "ValBalanceDetail", MessageTemplate = "备注的长度限制在300个字符以内")]
		public string Remark
		{
			get;
			set;
		}
	}
}

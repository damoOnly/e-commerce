using EcShop.Core;
using Ecdev.Components.Validation.Validators;
using System;
namespace EcShop.Entities.Members
{
	[HasSelfValidation]
	public class SplittinDrawInfo
	{
		public System.DateTime RequestDate
		{
			get;
			set;
		}
		[RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValBalanceDrawRequestInfo", MessageTemplate = "提现金额不能为空，金额大小0.01-1000万之间")]
		public decimal Amount
		{
			get;
			set;
		}
		public int UserId
		{
			get;
			set;
		}
		[HtmlCoding]
		public string UserName
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 30, Ruleset = "ValBalanceDrawRequestInfo", MessageTemplate = "开户人真实姓名不能为空,长度限制在30字符以内")]
		public string Account
		{
			get;
			set;
		}
		public int AuditStatus
		{
			get;
			set;
		}
		public System.DateTime? AccountDate
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(0, 300, Ruleset = "ValBalanceDrawRequestInfo", MessageTemplate = "备注长度限制在300字符以内")]
		public string ManagerRemark
		{
			get;
			set;
		}
	}
}

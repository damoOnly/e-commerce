using Ecdev.Components.Validation.Validators;
using System;
namespace EcShop.Entities.Store
{
	public class VoteItemInfo
	{
		public long VoteId
		{
			get;
			set;
		}
		public long VoteItemId
		{
			get;
			set;
		}
		[StringLengthValidator(1, 60, Ruleset = "VoteItem", MessageTemplate = "提供给用户选择的内容，长度限制在60个字符以内")]
		public string VoteItemName
		{
			get;
			set;
		}
		public int ItemCount
		{
			get;
			set;
		}
		public decimal Percentage
		{
			get;
			set;
		}
		public decimal Lenth
		{
			get
			{
                return (this.Percentage * System.Convert.ToDecimal(4.2)) + 1;//修改1
			}
		}
	}
}

using EcShop.Core;
using Ecdev.Components.Validation.Validators;
using System;
namespace EcShop.Entities.Comments
{
	public class HelpCategoryInfo
	{
		public int? CategoryId
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "ValHelpCategoryInfo", MessageTemplate = "分类名称不能为空，长度限制在60个字符以内")]
		public string Name
		{
			get;
			set;
		}
		public int DisplaySequence
		{
			get;
			set;
		}
		public string IconUrl
		{
			get;
			set;
		}
		public string IndexChar
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(0, 300, Ruleset = "ValHelpCategoryInfo", MessageTemplate = "分类介绍最多只能输入300个字符")]
		public string Description
		{
			get;
			set;
		}
		public bool IsShowFooter
		{
			get;
			set;
		}
	}
}

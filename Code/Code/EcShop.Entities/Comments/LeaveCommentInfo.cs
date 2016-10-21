using EcShop.Core;
using Ecdev.Components.Validation.Validators;
using System;
using System.Collections.Generic;
namespace EcShop.Entities.Comments
{
	public class LeaveCommentInfo
	{
       public LeaveCommentInfo()
        {
            Images = new List<string>();
        }
		public long LeaveId
		{
			get;
			set;
		}
		public int? UserId
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "Refer", MessageTemplate = "用户名为必填项，长度限制在60字符以内")]
		public string UserName
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "Refer", MessageTemplate = "标题为必填项，长度限制在60字符以内")]
		public string Title
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 300, Ruleset = "Refer", MessageTemplate = "留言内容为必填项，长度限制在300字符以内")]
		public string PublishContent
		{
			get;
			set;
		}
		public System.DateTime PublishDate
		{
			get;
			set;
		}
		public System.DateTime LastDate
		{
			get;
			set;
		}

        public List<string> Images { get; set; }

        public int? FeedbackType { get; set; }
        public string ContactWay { get; set; }
	}
}

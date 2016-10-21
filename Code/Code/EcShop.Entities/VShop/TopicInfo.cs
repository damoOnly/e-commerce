using EcShop.Core;
using Ecdev.Components.Validation.Validators;
using System;
namespace EcShop.Entities.VShop
{
	public class TopicInfo
	{
		public int TopicId
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "ValTopicInfo", MessageTemplate = "专题标题不能为空，长度限制在60个字符以内")]
		public string Title
		{
			get;
			set;
		}
		public string IconUrl
		{
			get;
			set;
		}
		[StringLengthValidator(1, 999999999, Ruleset = "ValTopicInfo", MessageTemplate = "专题内容不能为空")]
		public string Content
		{
			get;
			set;
		}
		public System.DateTime AddedDate
		{
			get;
			set;
		}
		public bool IsRelease
		{
			get;
			set;
		}
		public int DisplaySequence
		{
			get;
			set;
		}
		public string Keys
		{
			get;
			set;
		}

        public int SupplierId
        {
            get;
            set;
        }

        public string MobileListImageUrl
        {
            get;
            set;
        }

        public string MobileBannerImageUrl
        {
            get;
            set;
        }

        public string PCListImageUrl
        {
            get;
            set;
        }

        public string PCBannerImageUrl
        {
            get;
            set;
        }
	}
}

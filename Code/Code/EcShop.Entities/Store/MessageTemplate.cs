using System;
namespace EcShop.Entities.Store
{
	public class MessageTemplate
	{
		public string Name
		{
			get;
			private set;
		}
		public string MessageType
		{
			get;
			set;
		}
		public bool SendEmail
		{
			get;
			set;
		}
		public bool SendSMS
		{
			get;
			set;
		}
		public bool SendInnerMessage
		{
			get;
			set;
		}
		public bool SendWeixin
		{
			get;
			set;
		}
        public bool XinGeSend
        {
            get;
            set;
        }
        public string XinGeBody
        {
            get;
            set;
        }
        public string XinGeSubject
        {
            get;
            set;
        }

		public string WeixinTemplateId
		{
			get;
			set;
		}
		public string TagDescription
		{
			get;
			private set;
		}
		public string EmailSubject
		{
			get;
			set;
		}
		public string EmailBody
		{
			get;
			set;
		}
		public string InnerMessageSubject
		{
			get;
			set;
		}
		public string InnerMessageBody
		{
			get;
			set;
		}
		public string SMSBody
		{
			get;
			set;
		}
		public MessageTemplate()
		{
		}
		public MessageTemplate(string tagDescription, string name)
		{
			this.TagDescription = tagDescription;
			this.Name = name;
		}
	}
}

using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.SqlDal.Store;
using System;
using System.Collections.Generic;
using System.Net.Mail;
namespace EcShop.Messages
{
	public static class MessageTemplateHelper
	{
		private const string CacheKey = "Message-{0}";
		private const string DistributorCacheKey = "Message-{0}-{1}";
		internal static System.Net.Mail.MailMessage GetEmailTemplate(EcShop.Entities.Store.MessageTemplate template, string emailTo)
		{
			System.Net.Mail.MailMessage result;
			if (template == null || !template.SendEmail || string.IsNullOrEmpty(emailTo))
			{
				result = null;
			}
			else
			{
				System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
				{
					IsBodyHtml = true,
					Priority = System.Net.Mail.MailPriority.High,
					Body = template.EmailBody.Trim(),
					Subject = template.EmailSubject.Trim()
				};
				mailMessage.To.Add(emailTo);
				result = mailMessage;
			}
			return result;
		}
		internal static EcShop.Entities.Store.MessageTemplate GetTemplate(string messageType)
		{
			messageType = messageType.ToLower();
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			string key = string.Format("Message-{0}", messageType);
			EcShop.Entities.Store.MessageTemplate messageTemplate = HiCache.Get(key) as EcShop.Entities.Store.MessageTemplate;
			if (messageTemplate == null)
			{
				messageTemplate = MessageTemplateHelper.GetMessageTemplate(messageType);
				if (messageTemplate != null)
				{
					HiCache.Max(key, messageTemplate);
				}
			}
			return messageTemplate;
		}
		public static void UpdateSettings(System.Collections.Generic.IList<EcShop.Entities.Store.MessageTemplate> templates)
		{
			if (templates != null && templates.Count != 0)
			{
				new MessageTemplateDao().UpdateSettings(templates);
				foreach (EcShop.Entities.Store.MessageTemplate current in templates)
				{
					HiCache.Remove(string.Format("Message-{0}", current.MessageType.ToLower()));
				}
			}
		}
		public static void UpdateTemplate(EcShop.Entities.Store.MessageTemplate template)
		{
			if (template != null)
			{
				new MessageTemplateDao().UpdateTemplate(template);
				HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
			}
		}
		public static EcShop.Entities.Store.MessageTemplate GetMessageTemplate(string messageType)
		{
			EcShop.Entities.Store.MessageTemplate result;
			if (string.IsNullOrEmpty(messageType))
			{
				result = null;
			}
			else
			{
				result = new MessageTemplateDao().GetMessageTemplate(messageType);
			}
			return result;
		}
		public static System.Collections.Generic.IList<EcShop.Entities.Store.MessageTemplate> GetMessageTemplates()
		{
			return new MessageTemplateDao().GetMessageTemplates();
		}
	}
}

using EcShop.Core.Jobs;
using EcShop.Membership.Context;
using EcShop.Messages;
using System;
using System.Globalization;
using System.Xml;
namespace EcShop.Jobs
{
    public class SMSJob : IJob
	{
		private int failureInterval = 15;
		private int numberOfTries = 5;
		public void Execute(XmlNode node)
		{
			if (null != node)
			{
				XmlAttribute xmlAttribute = node.Attributes["failureInterval"];
				XmlAttribute xmlAttribute2 = node.Attributes["numberOfTries"];
				if (xmlAttribute != null)
				{
					try
					{
						this.failureInterval = int.Parse(xmlAttribute.Value, CultureInfo.InvariantCulture);
					}
					catch
					{
						this.failureInterval = 15;
					}
				}
				if (xmlAttribute2 != null)
				{
					try
					{
						this.numberOfTries = int.Parse(xmlAttribute2.Value, CultureInfo.InvariantCulture);
					}
					catch
					{
						this.numberOfTries = 5;
					}
				}
				
                this.SendQueuedSMSJob();
			}
		}
		public void SendQueuedSMSJob()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			if (masterSettings != null)
			{
				SMS.SendQueuedSMS(this.failureInterval, this.numberOfTries, masterSettings);
			}
		}
	}
}

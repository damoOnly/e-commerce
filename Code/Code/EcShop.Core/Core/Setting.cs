using EcShop.Core;
using System;
using System.Globalization;
using System.Xml;

namespace EcShop.Core
{
	public class Setting
	{
		public int ServiceStatus
		{
			get;
			set;
		}
		public int OpenTaobao
		{
			get;
			set;
		}
		public int OpenApp
		{
			get;
			set;
		}
		public int OpenAliho
		{
			get;
			set;
		}
		public int OpenWap
		{
			get;
			set;
		}
		public int OpenVstore
		{
			get;
			set;
		}
		
		public string SiteUrl
		{
			get;
			set;
		}
		public string CheckCode
		{
			get;
			set;
		}
		
		public Setting(string siteUrl)
		{
			this.SiteUrl = siteUrl;
			this.ServiceStatus = 1;
			this.OpenAliho = 0;
			this.OpenTaobao = 1;
			this.OpenApp = 0;
			this.OpenVstore = 0;
			this.OpenWap = 0;
		}
		
		public static Setting FromXml(XmlDocument doc)
		{
			XmlNode xmlNode = doc.SelectSingleNode("Settings");

			return new Setting(Setting.GetNodeValue(xmlNode, "SiteUrl", ""))
			{
				CheckCode = Setting.GetNodeValue(xmlNode, "CheckCode", ""),
				SiteUrl = Setting.GetNodeValue(xmlNode, "SiteUrl", ""),
				ServiceStatus = int.Parse(Setting.GetNodeValue(xmlNode, "ServiceStatus", "0")),
				OpenVstore = int.Parse(Setting.GetNodeValue(xmlNode, "OpenVstore", "0")),
				OpenAliho = int.Parse(Setting.GetNodeValue(xmlNode, "OpenAliho", "0")),
				OpenTaobao = int.Parse(Setting.GetNodeValue(xmlNode, "OpenTaobao", "0")),
				OpenWap = int.Parse(Setting.GetNodeValue(xmlNode, "OpenWap", "0")),
				OpenApp = int.Parse(Setting.GetNodeValue(xmlNode, "OpenMobbile", "0"))
			};
		}

        private static string GetNodeValue(XmlNode node, string key, string defaultValue)
        {
            try
            {
                return node.SelectSingleNode(key).InnerText;
            }
            catch { }

            return defaultValue;
        }
	}
}

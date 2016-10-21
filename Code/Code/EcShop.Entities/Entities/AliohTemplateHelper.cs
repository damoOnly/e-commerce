using EcShop.Core;
using EcShop.Membership.Context;
using System;
using System.Web;
using System.Xml;
namespace EcShop.Entities
{
	public class AliohTemplateHelper
	{
		public int GetTopicMaxNum()
		{
			int result = 1;
			XmlDocument xmlNode = new AliohTemplateHelper().GetXmlNode();
			int.TryParse(xmlNode.SelectSingleNode("root/TopicMaxNum").InnerText, out result);
			return result;
		}
		public int GetCategoryMaxNum()
		{
			int result = 1;
			XmlDocument xmlNode = new AliohTemplateHelper().GetXmlNode();
			int.TryParse(xmlNode.SelectSingleNode("root/CategoryMaxNum").InnerText, out result);
			return result;
		}
		public int GetTopicProductMaxNum()
		{
			int result = 1;
			XmlDocument xmlNode = new AliohTemplateHelper().GetXmlNode();
			int.TryParse(xmlNode.SelectSingleNode("root/TopicProductMaxNum").InnerText, out result);
			return result;
		}
		public int GetCategoryProductMaxNum()
		{
			int result = 1;
			XmlDocument xmlNode = new AliohTemplateHelper().GetXmlNode();
			int.TryParse(xmlNode.SelectSingleNode("root/CategoryProductMaxNum").InnerText, out result);
			return result;
		}
		public string GetDefaultBg()
		{
			XmlDocument xmlNode = new AliohTemplateHelper().GetXmlNode();
			return xmlNode.SelectSingleNode("root/DefaultBg").InnerText;
		}
		public XmlDocument GetXmlNode()
		{
			XmlDocument xmlDocument = HiCache.Get("AliohTemplateFileCache") as XmlDocument;
			if (xmlDocument == null)
			{
				xmlDocument = new XmlDocument();
				xmlDocument.Load(HttpContext.Current.Request.MapPath(HiContext.Current.GetAliOHshopSkinPath(null) + "/template.xml"));
			}
			return xmlDocument;
		}
	}
}

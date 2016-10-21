using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace EcShop.Core
{
	public static class SettingManager
	{
		private const string MasterSettingsCacheKey = "FileCache-Settings";

		public static Setting GetMasterSettings(bool cacheable)
		{
			if (!cacheable)
			{
                HiCache.Remove(MasterSettingsCacheKey);
			}
			XmlDocument xmlDocument = HiCache.Get(MasterSettingsCacheKey) as XmlDocument;
			Setting result;
			if (xmlDocument == null)
			{
				string masterSettingsFilename = SettingManager.GetSettingsFilename();

				if (!System.IO.File.Exists(masterSettingsFilename))
				{
					result = null;
					return result;
				}

				xmlDocument = new XmlDocument();
				xmlDocument.Load(masterSettingsFilename);
				if (cacheable)
				{
					HiCache.Max(MasterSettingsCacheKey, xmlDocument, new CacheDependency(masterSettingsFilename));
				}
			}

			result = Setting.FromXml(xmlDocument);

			return result;
		}

		private static string GetSettingsFilename()
		{
			HttpContext current = HttpContext.Current;
			return (current != null) ? current.Request.MapPath("~/config/SiteSettings.config") : System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "config\\SiteSettings.config");
		}
	}
}

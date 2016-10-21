using System;
using System.IO;
using System.Web;
using System.Web.Caching;

using EcShop.Framework;

namespace EcShop.Framework
{
	public static class LicenseHelper
	{
		public const string CachePublicKey = "Ecshop_PublicKey";
		public static string GetSiteHash()
		{
            //EcSiteSettings masterSettings = EcSettingsManager.GetMasterSettings(false);
            //return masterSettings.CheckCode;
		    return "";
		}
		public static string GetPublicKey()
		{
            string text = EcCache.Get(CachePublicKey) as string;
			if (string.IsNullOrEmpty(text))
			{
				HttpContext current = HttpContext.Current;
				string text2;
				if (current != null)
				{
					text2 = current.Request.MapPath("~/config/publickey.xml");
				}
				else
				{
					text2 = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "/config/publickey.xml");
				}
				text = System.IO.File.ReadAllText(text2);
                EcCache.Max(CachePublicKey, text, new CacheDependency(text2));
			}
			return text;
		}
	}
}

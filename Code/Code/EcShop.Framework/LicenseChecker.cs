using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace EcShop.Framework
{
	public static class LicenseChecker
	{
		private const string CacheCommercialKey = "FileCache_CommercialLicenser";
		public static void Check(out bool isValid, out bool expired, out int siteQty)
		{
			siteQty = 0;
			isValid = false;
			expired = true;
            HttpContext current = HttpContext.Current;
			XmlDocument xmlDocument = EcCache.Get("FileCache_CommercialLicenser") as XmlDocument;
			if (xmlDocument == null)
			{
                string text = (current != null) ? current.Request.MapPath("~/config/Certificates.cer") : System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "config\\Certificates.cer");
				if (!System.IO.File.Exists(text))
				{
					return;
				}
				xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(System.IO.File.ReadAllText(text));
				EcCache.Max("FileCache_CommercialLicenser", xmlDocument, new CacheDependency(text));
			}
			XmlNode hostNode = xmlDocument.DocumentElement.SelectSingleNode("//Host");
			XmlNode licenseDateNode = xmlDocument.DocumentElement.SelectSingleNode("//LicenseDate");
			XmlNode expiresNode = xmlDocument.DocumentElement.SelectSingleNode("//Expires");
			XmlNode siteQtyNode = xmlDocument.DocumentElement.SelectSingleNode("//SiteQty");
			XmlNode signNode = xmlDocument.DocumentElement.SelectSingleNode("//Signature");
			if (string.Compare(hostNode.InnerText, HttpContext.Current.Request.Url.Host, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
			{
				string s = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Host={0}&Expires={1}&SiteQty={2}&LicenseDate={3}", new object[]
				{
					HttpContext.Current.Request.Url.Host,
					expiresNode.InnerText,
					siteQtyNode.InnerText,
					licenseDateNode.InnerText
				});
				using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
				{
					rSACryptoServiceProvider.FromXmlString(LicenseHelper.GetPublicKey());
					RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
					rSAPKCS1SignatureDeformatter.SetHashAlgorithm("SHA1");
					byte[] rgbSignature = System.Convert.FromBase64String(signNode.InnerText);
					SHA1Managed sHA1Managed = new SHA1Managed();
					byte[] rgbHash = sHA1Managed.ComputeHash(System.Text.Encoding.UTF8.GetBytes(s));
					isValid = rSAPKCS1SignatureDeformatter.VerifySignature(rgbHash, rgbSignature);
				}
				expired = (System.DateTime.Now > System.DateTime.Parse(expiresNode.InnerText));
				if (isValid && !expired)
				{
					int.TryParse(siteQtyNode.InnerText, out siteQty);
				}
			}
		}
	}
}

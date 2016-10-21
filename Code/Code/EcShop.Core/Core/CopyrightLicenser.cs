using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace EcShop.Core
{
	public sealed class CopyrightLicenser
	{
		public const string CacheCopyrightKey = "EcshopSiteLicense";

		private CopyrightLicenser()
		{
            ServiceStatus = false;
            OpenPC = false;
            OpenApp = false;
            OpenVshop = false;
            OpenWap = false;
            OpenAliOH = false;
		}

		public static bool CheckCopyright()
		{
			HttpContext current = HttpContext.Current;
			XmlDocument xmlDocument = HiCache.Get(CacheCopyrightKey) as XmlDocument;

			bool result;

			if (xmlDocument == null)
			{
				string licenseFile;
                
                if (current != null)
				{
                    licenseFile = current.Request.MapPath("~/config/ecshop.lics");
				}
				else
				{
					licenseFile = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "ecshop.lics");
				}

				if (!File.Exists(licenseFile))
				{
					result = false;
					return result;
				}

                string license = "";

                try
                {
                    string text = File.ReadAllText(licenseFile);

                    license = HiCryptographer.Decrypt(text);
                }
                catch
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(license))
                {
                    return false;
                }

				xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(license);

                HiCache.Max(CacheCopyrightKey, xmlDocument, new CacheDependency(licenseFile));
			}

			XmlNode hostNode = xmlDocument.DocumentElement.SelectSingleNode("//Host");
			XmlNode licenseDateNode = xmlDocument.DocumentElement.SelectSingleNode("//LicenseDate");
			XmlNode expireDateNode = xmlDocument.DocumentElement.SelectSingleNode("//ExpiresDate");
			XmlNode signNode = xmlDocument.DocumentElement.SelectSingleNode("//Signature");
            XmlNode openPCNode = xmlDocument.DocumentElement.SelectSingleNode("//OpenPC");
            XmlNode openWeixinNode = xmlDocument.DocumentElement.SelectSingleNode("//OpenWeixin");
            XmlNode openAppNode = xmlDocument.DocumentElement.SelectSingleNode("//OpenApp");
            XmlNode openWapNode = xmlDocument.DocumentElement.SelectSingleNode("//OpenWap");
            XmlNode openAliohNode = xmlDocument.DocumentElement.SelectSingleNode("//OpenAliOH");

            string host = current.Request.Url.Host;

            if (!host.EndsWith(hostNode.InnerText))
			//if (string.Compare(hostNode.InnerText, host, true, System.Globalization.CultureInfo.InvariantCulture) != 0)
			{
				result = false;
			}
			else
			{
                string data = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                                "Host={0}&LicenseDate={1:yyyy-MM-dd}&ExpiresDate={2:yyyy-MM-dd}&Key={3}&OpenPC={4}&OpenWeixin={5}&OpenApp={6}&OpenWap={7}&OpenAliOH={8}",
                                hostNode.InnerText, licenseDateNode.InnerText, expireDateNode.InnerText, LicenseHelper.GetSiteHash(),
                                openPCNode.InnerText, openWeixinNode.InnerText, openAppNode.InnerText, openWapNode.InnerText, openAliohNode.InnerText);

				bool flag = false;
				using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
				{
					rsa.FromXmlString(LicenseHelper.GetPublicKey());

					RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(rsa);
					deformatter.SetHashAlgorithm("SHA1");

					byte[] rgbSignature = System.Convert.FromBase64String(signNode.InnerText);
					SHA1Managed sHA1Managed = new SHA1Managed();

					byte[] rgbHash = sHA1Managed.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
					flag = deformatter.VerifySignature(rgbHash, rgbSignature);
				}

				result = (flag && System.DateTime.Now < DateTime.Parse(expireDateNode.InnerText));

                if (result)
                {
                    ServiceStatus = true;

                    OpenPC = openPCNode.InnerText.Equals("1");
                    OpenApp = openAppNode.InnerText.Equals("1");
                    OpenVshop = openWeixinNode.InnerText.Equals("1");
                    OpenWap = openWapNode.InnerText.Equals("1");
                    OpenAliOH = openAliohNode.InnerText.Equals("1");
                }
			}

			return result;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType">服务类型：service\pc\vshop\app\wap\alioh</param>
        /// <returns></returns>
        public static bool CheckService(string serviceType)
        {
            bool result = false;

            if (CheckCopyright())
            {
                switch (serviceType)
                {
                    case "service":
                        result = ServiceStatus;
                        break;
                    case "pc":
                        result = OpenPC;
                        break;
                    case "vshop":
                        result = OpenVshop;
                        break;
                    case "app":
                        result = OpenApp;
                        break;
                    case "wap":
                        result = OpenWap;
                        break;
                    case "alioh":
                        result = OpenAliOH;
                        break;
                    default:
                        result = false;
                        break;
                }
            }

            return result;
        }

        private static bool ServiceStatus
        {
            get;
            set;
        }

        private static bool OpenWap
        {
            get;
            set;
        }

        private static bool OpenAliOH
        {
            get;
            set;
        }

        private static bool OpenPC
        {
            get;
            set;
        }

        private static bool OpenVshop
        {
            get;
            set;
        }

        private static bool OpenApp
        {
            get;
            set;
        }
	}
}

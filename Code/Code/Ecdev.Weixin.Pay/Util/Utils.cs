using Ecdev.Weixin.Pay.Notify;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
namespace Ecdev.Weixin.Pay.Util
{
	internal class Utils
	{
		private static readonly DateTime BaseTime = new DateTime(1970, 1, 1);
		private static Dictionary<string, XmlSerializer> parsers = new Dictionary<string, XmlSerializer>();
		public static DateTime ConvertSecondsToDateTime(long seconds)
		{
			return Utils.BaseTime.AddSeconds((double)seconds).AddHours(8.0);
		}
		public static long GetCurrentTimeSeconds()
		{
			return (long)(DateTime.UtcNow - Utils.BaseTime).TotalSeconds;
		}
		public static long GetTimeSeconds(DateTime dt)
		{
			return (long)(dt.ToUniversalTime() - Utils.BaseTime).TotalSeconds;
		}
		public static string CreateNoncestr()
		{
			return DateTime.Now.ToString("fffffff");
		}
		public static T GetNotifyObject<T>(string xml) where T : NotifyObject
		{
			Type typeFromHandle = typeof(T);
			string fullName = typeFromHandle.FullName;
			XmlSerializer xmlSerializer = null;
			bool flag = Utils.parsers.TryGetValue(fullName, out xmlSerializer);
			if (!flag || xmlSerializer == null)
			{
				XmlAttributes xmlAttributes = new XmlAttributes();
				xmlAttributes.XmlRoot = new XmlRootAttribute("xml");
				XmlAttributeOverrides xmlAttributeOverrides = new XmlAttributeOverrides();
				xmlAttributeOverrides.Add(typeFromHandle, xmlAttributes);
				xmlSerializer = new XmlSerializer(typeFromHandle, xmlAttributeOverrides);
				Utils.parsers[fullName] = xmlSerializer;
			}
			object obj = null;
			T result;
			try
			{
				using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
				{
					obj = xmlSerializer.Deserialize(stream);
				}
			}
			catch (Exception var_8_B6)
			{
				result = default(T);
				return result;
			}
			result = (obj as T);
			return result;
		}
		public static string GetToken(string appid, string secret)
		{
			string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
			string value = new WebUtils().DoGet(url);
			string result;
			if (string.IsNullOrEmpty(value))
			{
				result = string.Empty;
			}
			else
			{
				Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
				if (dictionary != null && dictionary.ContainsKey("access_token"))
				{
					result = dictionary["access_token"];
				}
				else
				{
					result = string.Empty;
				}
			}
			return result;
		}
	}
}

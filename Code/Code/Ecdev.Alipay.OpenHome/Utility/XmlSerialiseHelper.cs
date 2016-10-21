using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
namespace Ecdev.Alipay.OpenHome.Utility
{
	public class XmlSerialiseHelper
	{
		public static string Serialise<T>(T t)
		{
			System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
			System.IO.StreamWriter textWriter = new System.IO.StreamWriter(memoryStream, System.Text.Encoding.GetEncoding("GBK"));
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add(string.Empty, string.Empty);
			xmlSerializer.Serialize(textWriter, t, xmlSerializerNamespaces);
			string text = System.Text.Encoding.GetEncoding("GBK").GetString(memoryStream.ToArray()).Replace("\r", "").Replace("\n", "");
			while (text.Contains(" <"))
			{
				text = text.Replace(" <", "<");
			}
			return text.Substring(text.IndexOf("?>") + 2);
		}
		public static T Deserialize<T>(string xml)
		{
			System.IO.StringReader textReader = new System.IO.StringReader(xml);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			return (T)((object)xmlSerializer.Deserialize(textReader));
		}
	}
}

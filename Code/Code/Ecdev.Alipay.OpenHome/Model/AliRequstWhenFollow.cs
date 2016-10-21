using System;
using System.Xml.Serialization;
namespace Ecdev.Alipay.OpenHome.Model
{
	[XmlRoot("XML")]
	[System.Serializable]
	internal class AliRequstWhenFollow : AliRequest
	{
		[XmlElement("UserInfo")]
		public UserInfo UserInfo
		{
			get;
			set;
		}
	}
}

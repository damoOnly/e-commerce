using System;
namespace Ecdev.Alipay.OpenHome.AlipayOHException
{
	public class RequestException : AlipayOpenHomeException
	{
		public RequestException()
		{
		}
		public RequestException(string message, System.Exception innerException) : base(message, innerException)
		{
		}
	}
}

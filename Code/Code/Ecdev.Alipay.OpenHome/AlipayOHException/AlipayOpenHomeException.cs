using System;
namespace Ecdev.Alipay.OpenHome.AlipayOHException
{
	public class AlipayOpenHomeException : System.ApplicationException
	{
		public AlipayOpenHomeException()
		{
		}
		public AlipayOpenHomeException(string message) : base(message)
		{
		}
		public AlipayOpenHomeException(string message, System.Exception innerException) : base(message, innerException)
		{
		}
	}
}

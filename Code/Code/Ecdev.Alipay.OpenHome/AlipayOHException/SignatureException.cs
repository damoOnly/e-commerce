using System;
namespace Ecdev.Alipay.OpenHome.AlipayOHException
{
	public class SignatureException : AlipayOpenHomeException
	{
		public SignatureException()
		{
		}
		public SignatureException(string message) : base(message)
		{
		}
		public SignatureException(string message, System.Exception innerException) : base(message, innerException)
		{
		}
	}
}

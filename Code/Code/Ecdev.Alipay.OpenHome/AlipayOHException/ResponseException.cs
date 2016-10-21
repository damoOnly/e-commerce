using System;
namespace Ecdev.Alipay.OpenHome.AlipayOHException
{
	public class ResponseException : AlipayOpenHomeException
	{
		public ResponseException()
		{
		}
		public ResponseException(string message) : base(message)
		{
		}
		public ResponseException(string message, System.Exception innerException) : base(message, innerException)
		{
		}
	}
}

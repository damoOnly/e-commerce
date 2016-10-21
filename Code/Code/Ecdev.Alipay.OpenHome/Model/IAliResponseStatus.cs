using System;
namespace Ecdev.Alipay.OpenHome.Model
{
	public interface IAliResponseStatus
	{
		string Code
		{
			get;
		}
		string Message
		{
			get;
		}
	}
}

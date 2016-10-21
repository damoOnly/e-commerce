using System;
namespace Ecdev.Alipay.OpenHome.Request
{
	public interface IRequest
	{
		string GetMethodName();
		string GetBizContent();
	}
}

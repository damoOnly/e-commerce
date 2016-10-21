using System;
namespace Ecdev.Alipay.OpenHome.Model
{
	[System.Serializable]
	public abstract class AliResponse
	{
		public string sign
		{
			get;
			set;
		}
		public ErrorResponse error_response
		{
			get;
			set;
		}
	}
}

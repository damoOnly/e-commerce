using System;
namespace Ecdev.Alipay.OpenHome.Utility
{
	internal class TimeHelper
	{
		public static double TransferToMilStartWith1970(System.DateTime dateTime)
		{
			System.DateTime d = new System.DateTime(1970, 1, 1);
			return (dateTime - d).TotalMilliseconds;
		}
	}
}

using System;
using System.Collections.Generic;
namespace Ecdev.Alipay.OpenHome.Model
{
	public class Button
	{
		public string name
		{
			get;
			set;
		}
		public string actionParam
		{
			get;
			set;
		}
		public string actionType
		{
			get;
			set;
		}
		public string authType
		{
			get
			{
				return "loginAuth";
			}
		}
		public System.Collections.Generic.IEnumerable<Button> subButton
		{
			get;
			set;
		}
	}
}

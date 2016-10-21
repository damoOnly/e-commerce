using Ecdev.Alipay.OpenHome.Model;
using Newtonsoft.Json;
using System;
namespace Ecdev.Alipay.OpenHome.Request
{
	public class AddMenuRequest : IRequest
	{
		private Menu menu;
		public AddMenuRequest(Menu menu)
		{
			this.menu = menu;
		}
		public string GetMethodName()
		{
			return "alipay.mobile.public.menu.add";
		}
		public string GetBizContent()
		{
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
			jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			return JsonConvert.SerializeObject(this.menu, jsonSerializerSettings);
		}
	}
}

using Ecdev.Alipay.OpenHome.Model;
using Newtonsoft.Json;
using System;
namespace Ecdev.Alipay.OpenHome.Request
{
	public class UpdateMenuRequest : IRequest
	{
		private Menu menu;
		public UpdateMenuRequest(Menu menu)
		{
			this.menu = menu;
		}
		public string GetMethodName()
		{
			return "alipay.mobile.public.menu.update";
		}
		public string GetBizContent()
		{
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
			jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			return JsonConvert.SerializeObject(this.menu, jsonSerializerSettings);
		}
	}
}

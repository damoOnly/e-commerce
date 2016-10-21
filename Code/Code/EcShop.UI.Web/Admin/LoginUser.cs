using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using System;
using System.Web;
namespace EcShop.UI.Web.Admin
{
	public class LoginUser : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			string text = "";
			string text2 = context.Request.QueryString["action"];
			if (!string.IsNullOrEmpty(text2))
			{
				string a;
				if ((a = text2) != null)
				{
					if (!(a == "login"))
					{
						if (a == "chklogin")
						{
							IUser user = HiContext.Current.User;
							if (user == null || user.UserRole != UserRole.SiteManager)
							{
								text = "{\"status\":\"false\"}";
							}
							else
							{
								text = "{\"status\":\"true\"}";
							}
						}
					}
					else
					{
						SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
						text = "{\"sitename\":\"" + masterSettings.SiteName + "\",";
						text = text + "\"username\":\"" + HiContext.Current.User.Username + "\"";
                        //text = text + "\"taobaourl\":\"" + string.Format("http://vip.ecdev.cn/TaoBaoApi.aspx?Host={0}&ApplicationPath={1}", HiContext.Current.SiteUrl, Globals.ApplicationPath) + "\"}";
                        text += "}";
					}
				}
				context.Response.ContentType = "text/json";
				context.Response.Write(text);
			}
		}
	}
}

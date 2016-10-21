using System;
using System.Threading;
using System.Web;
namespace Ecdev.Plugins
{
	public class SMSPlugins : PluginContainer
	{
		private static readonly object LockHelper = new object();
		private static volatile SMSPlugins instance = null;
		protected override string PluginLocalPath
		{
			get
			{
                string path = "~/plugins/sms";

				//return HttpContext.Current.Request.MapPath("~/plugins/sms");

                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Request.MapPath(path);
                }
                else //非web程序引用
                {
                    path = path.Replace("/", "\\");
                    if (path.StartsWith("\\") || path.StartsWith("~"))
                    {
                        path = path.Substring(path.IndexOf('\\', 1)).TrimStart('\\');
                    }
                    return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                }
			}
		}
		protected override string PluginVirtualPath
		{
			get
			{
				return Utils.ApplicationPath + "/plugins/sms";
			}
		}
		protected override string IndexCacheKey
		{
			get
			{
				return "plugin-sms-index";
			}
		}
		protected override string TypeCacheKey
		{
			get
			{
				return "plugin-sms-type";
			}
		}
		private SMSPlugins()
		{
		}
		public static SMSPlugins Instance()
		{
			if (SMSPlugins.instance == null)
			{
				object lockHelper;
				Monitor.Enter(lockHelper = SMSPlugins.LockHelper);
				try
				{
					if (SMSPlugins.instance == null)
					{
						SMSPlugins.instance = new SMSPlugins();
					}
				}
				finally
				{
					Monitor.Exit(lockHelper);
				}
			}
			SMSPlugins.instance.VerifyIndex();
			return SMSPlugins.instance;
		}
		public override PluginItemCollection GetPlugins()
		{
			return base.GetPlugins("SMSSender");
		}
		public override PluginItem GetPluginItem(string fullName)
		{
			return base.GetPluginItem("SMSSender", fullName);
		}
	}
}

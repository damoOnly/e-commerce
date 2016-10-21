using Ecdev.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;

namespace Ecdev.Plugins
{
    public class XingePushPlugins : PluginContainer
    {
        private static readonly object LockHelper = new object();
        private static volatile XingePushPlugins instance = null;
		protected override string PluginLocalPath
		{
			get
			{
                return HttpContext.Current.Request.MapPath("~/plugins/xinge");
			}
		}
		protected override string PluginVirtualPath
		{
			get
			{
				return Utils.ApplicationPath + "/plugins/xinge";
			}
		}
		protected override string IndexCacheKey
		{
			get
			{
				return "plugin-xinge-index";
			}
		}
		protected override string TypeCacheKey
		{
			get
			{
				return "plugin-xinge-type";
			}
		}
        private XingePushPlugins()
		{
		}
        public static XingePushPlugins Instance()
		{
            if (XingePushPlugins.instance == null)
			{
				object lockHelper;
                Monitor.Enter(lockHelper = XingePushPlugins.LockHelper);
				try
				{
                    if (XingePushPlugins.instance == null)
					{
                        XingePushPlugins.instance = new XingePushPlugins();
					}
				}
				finally
				{
					Monitor.Exit(lockHelper);
				}
			}
            XingePushPlugins.instance.VerifyIndex();
            return XingePushPlugins.instance;
		}

		public override PluginItemCollection GetPlugins()
		{
			return base.GetPlugins("XingePush");
		}

		public override PluginItem GetPluginItem(string fullName)
		{
            return base.GetPluginItem("XingePush", fullName);
		}
    }
}

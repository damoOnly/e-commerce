using EcShop.Entities.Members;
using EcShop.SqlDal.Members;
using Ecdev.Plugins;
using System;
using System.Collections.Generic;
namespace EcShop.ControlPanel.Members
{
	public static class OpenIdHelper
	{
		public static void SaveSettings(OpenIdSettingsInfo settings)
		{
			new OpenIdSettingDao().SaveSettings(settings);
		}
		public static void DeleteSettings(string openIdType)
		{
			new OpenIdSettingDao().DeleteSettings(openIdType);
		}
		public static OpenIdSettingsInfo GetOpenIdSettings(string openIdType)
		{
			return new OpenIdSettingDao().GetOpenIdSettings(openIdType);
		}
		public static PluginItemCollection GetConfigedItems()
		{
			IList<string> configedTypes = new OpenIdSettingDao().GetConfigedTypes();
			PluginItemCollection result;
			if (configedTypes == null || configedTypes.Count == 0)
			{
				result = null;
			}
			else
			{
				PluginItemCollection plugins = OpenIdPlugins.Instance().GetPlugins();
				if (plugins != null && plugins.Count > 0)
				{
					PluginItem[] items = plugins.Items;
					PluginItem[] array = items;
					for (int i = 0; i < array.Length; i++)
					{
						PluginItem pluginItem = array[i];
						if (!configedTypes.Contains(pluginItem.FullName.ToLower()))
						{
							plugins.Remove(pluginItem.FullName.ToLower());
						}
					}
				}
				result = plugins;
			}
			return result;
		}
		public static PluginItemCollection GetEmptyItems()
		{
			PluginItemCollection plugins = OpenIdPlugins.Instance().GetPlugins();
			PluginItemCollection result;
			if (plugins == null || plugins.Count == 0)
			{
				result = null;
			}
			else
			{
				IList<string> configedTypes = new OpenIdSettingDao().GetConfigedTypes();
				if (configedTypes != null && configedTypes.Count > 0)
				{
					foreach (string current in configedTypes)
					{
						if (plugins.ContainsKey(current.ToLower()))
						{
							plugins.Remove(current.ToLower());
						}
					}
				}
				result = plugins;
			}
			return result;
		}
	}
}

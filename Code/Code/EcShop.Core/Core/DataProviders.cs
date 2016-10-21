using EcShop.Core.Configuration;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
namespace EcShop.Core
{
	public sealed class DataProviders
	{
		private DataProviders()
		{
		}
		public static object CreateInstance(Provider dataProvider)
		{
			object result;
			if (null == dataProvider)
			{
				result = null;
			}
			else
			{
				Type type = Type.GetType(dataProvider.Type);
				object obj = null;
				if (type != null)
				{
					obj = Activator.CreateInstance(type);
				}
				result = obj;
			}
			return result;
		}
		public static object CreateInstance(string typeStr)
		{
			object result;
			if (string.IsNullOrEmpty(typeStr))
			{
				result = null;
			}
			else
			{
				try
				{
					result = Activator.CreateInstance(Type.GetType(typeStr));
				}
				catch(Exception ex)
				{
					result = null;
				}
			}
			return result;
		}
       
	}
}

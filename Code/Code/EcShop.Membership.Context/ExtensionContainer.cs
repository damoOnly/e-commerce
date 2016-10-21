using EcShop.Core.Configuration;
using System;
using System.Collections;
using System.Xml;
namespace EcShop.Membership.Context
{
	internal class ExtensionContainer
	{
		private static readonly object Sync = new object();
		private static volatile ExtensionContainer instance = null;
		private static readonly System.Collections.Hashtable Extensions = new System.Collections.Hashtable();
		private ExtensionContainer()
		{
			ExtensionContainer.Extensions.Clear();
			HiConfiguration config = HiContext.Current.Config;
            XmlNode configSection = config.GetConfigSection("Ecdev/Extensions");
			if (configSection != null)
			{
				foreach (XmlNode xmlNode in configSection.ChildNodes)
				{
					if (xmlNode.NodeType != XmlNodeType.Comment && xmlNode.Name.Equals("add"))
					{
						string value = xmlNode.Attributes["name"].Value;
						string value2 = xmlNode.Attributes["type"].Value;
						XmlAttribute xmlAttribute = xmlNode.Attributes["enabled"];
						if (xmlAttribute == null || !(xmlAttribute.Value == "false"))
						{
							System.Type type = System.Type.GetType(value2);
							if (type == null)
							{
								throw new System.Exception(value2 + " does not exist");
							}
							IExtension extension = System.Activator.CreateInstance(type) as IExtension;
							if (extension == null)
							{
								throw new System.Exception(value2 + " does not implement IExtension or is not configured correctly");
							}
							extension.Init();
							ExtensionContainer.Extensions.Add(value, extension);
						}
					}
				}
			}
		}
		internal static void LoadExtensions()
		{
			if (ExtensionContainer.instance == null)
			{
				lock (ExtensionContainer.Sync)
				{
					if (ExtensionContainer.instance == null)
					{
						ExtensionContainer.instance = new ExtensionContainer();
					}
				}
			}
		}
	}
}

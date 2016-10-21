using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Diagnostics;
using System.Globalization;
using System.Web.Hosting;
using System.Xml;
namespace EcShop.Membership.ASPNETProvider
{
	internal static class SecUtility
	{
		internal const int Infinite = 2147483647;
		internal static string GetDefaultAppName()
		{
			string result;
			try
			{
				string text = HostingEnvironment.ApplicationVirtualPath;
				if (string.IsNullOrEmpty(text))
				{
					text = Process.GetCurrentProcess().MainModule.ModuleName;
					int num = text.IndexOf('.');
					if (num != -1)
					{
						text = text.Remove(num);
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					result = "/";
				}
				else
				{
					result = text;
				}
			}
			catch
			{
				result = "/";
			}
			return result;
		}
		internal static bool ValidatePasswordParameter(ref string param, int maxSize)
		{
			return param != null && param.Length >= 1 && (maxSize <= 0 || param.Length <= maxSize);
		}
		internal static bool ValidateParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize)
		{
			if (param == null)
			{
				return !checkForNull;
			}
			return (!checkIfEmpty || param.Length >= 1) && (maxSize <= 0 || param.Length <= maxSize) && (!checkForCommas || !param.Contains(","));
		}
		internal static void CheckPasswordParameter(ref string param, int maxSize, string paramName)
		{
			if (param == null)
			{
				throw new ArgumentNullException(paramName);
			}
			if (param.Length < 1)
			{
				throw new ArgumentException(SR.GetString("The parameter '{0}' must not be empty.", paramName), paramName);
			}
			if (maxSize > 0 && param.Length > maxSize)
			{
				throw new ArgumentException(SR.GetString("The parameter '{0}' is too long: it must not exceed {1} chars in length.", paramName, maxSize.ToString(CultureInfo.InvariantCulture)), paramName);
			}
		}
		internal static void CheckParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
		{
			if (param == null)
			{
				if (checkForNull)
				{
					throw new ArgumentNullException(paramName);
				}
				return;
			}
			else
			{
				if (checkIfEmpty && param.Length < 1)
				{
					throw new ArgumentException(SR.GetString("The parameter '{0}' must not be empty.", paramName), paramName);
				}
				if (maxSize > 0 && param.Length > maxSize)
				{
					throw new ArgumentException(SR.GetString("The parameter '{0}' is too long: it must not exceed {1} chars in length.", paramName, maxSize.ToString(CultureInfo.InvariantCulture)), paramName);
				}
				if (checkForCommas && param.Contains(","))
				{
					throw new ArgumentException(SR.GetString("The parameter '{0}' must not contain commas.", paramName), paramName);
				}
				return;
			}
		}
		internal static void CheckArrayParameter(ref string[] param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
		{
			if (param == null)
			{
				throw new ArgumentNullException(paramName);
			}
			if (param.Length < 1)
			{
				throw new ArgumentException(SR.GetString("The array parameter '{0}' should not be empty.", paramName), paramName);
			}
			Hashtable hashtable = new Hashtable(param.Length);
			for (int i = param.Length - 1; i >= 0; i--)
			{
				SecUtility.CheckParameter(ref param[i], checkForNull, checkIfEmpty, checkForCommas, maxSize, paramName + "[ " + i.ToString(CultureInfo.InvariantCulture) + " ]");
				if (hashtable.Contains(param[i]))
				{
					throw new ArgumentException(SR.GetString("The array '{0}' should not contain duplicate values.", paramName), paramName);
				}
				hashtable.Add(param[i], param[i]);
			}
		}
		internal static bool GetBooleanValue(NameValueCollection config, string valueName, bool defaultValue)
		{
			string text = config[valueName];
			if (text == null)
			{
				return defaultValue;
			}
			bool result;
			if (bool.TryParse(text, out result))
			{
				return result;
			}
			throw new ProviderException(SR.GetString("The value must be boolean (true or false) for property '{0}'.", valueName));
		}
		internal static int GetIntValue(NameValueCollection config, string valueName, int defaultValue, bool zeroAllowed, int maxValueAllowed)
		{
			string text = config[valueName];
			if (text == null)
			{
				return defaultValue;
			}
			int num;
			if (!int.TryParse(text, out num))
			{
				if (zeroAllowed)
				{
					throw new ProviderException(SR.GetString("The value must be a non-negative 32-bit integer for property '{0}'.", valueName));
				}
				throw new ProviderException(SR.GetString("The value must be a positive 32-bit integer for property '{0}'.", valueName));
			}
			else
			{
				if (zeroAllowed && num < 0)
				{
					throw new ProviderException(SR.GetString("The value must be a non-negative 32-bit integer for property '{0}'.", valueName));
				}
				if (!zeroAllowed && num <= 0)
				{
					throw new ProviderException(SR.GetString("The value must be a positive 32-bit integer for property '{0}'.", valueName));
				}
				if (maxValueAllowed > 0 && num > maxValueAllowed)
				{
					throw new ProviderException(SR.GetString("The value '{0}' can not be greater than '{1}'.", valueName, maxValueAllowed.ToString(CultureInfo.InvariantCulture)));
				}
				return num;
			}
		}
		private static bool IsDirectorySeparatorChar(char ch)
		{
			return ch == '\\' || ch == '/';
		}
		internal static bool IsAbsolutePhysicalPath(string path)
		{
			return path != null && path.Length >= 3 && ((path[1] == ':' && SecUtility.IsDirectorySeparatorChar(path[2])) || SecUtility.IsUncSharePath(path));
		}
		internal static bool IsUncSharePath(string path)
		{
			return path.Length > 2 && SecUtility.IsDirectorySeparatorChar(path[0]) && SecUtility.IsDirectorySeparatorChar(path[1]);
		}
		internal static XmlNode GetAndRemoveBooleanAttribute(XmlNode node, string attrib, ref bool val)
		{
			return SecUtility.GetAndRemoveBooleanAttributeInternal(node, attrib, false, ref val);
		}
		private static XmlNode GetAndRemoveBooleanAttributeInternal(XmlNode node, string attrib, bool fRequired, ref bool val)
		{
			XmlNode andRemoveAttribute = SecUtility.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				if (andRemoveAttribute.Value == "true")
				{
					val = true;
				}
				else
				{
					if (!(andRemoveAttribute.Value == "false"))
					{
						throw new ConfigurationErrorsException(SR.GetString("The '{0}' attribute must be set to 'true' or 'false'.", andRemoveAttribute.Name), andRemoveAttribute);
					}
					val = false;
				}
			}
			return andRemoveAttribute;
		}
		private static XmlNode GetAndRemoveAttribute(XmlNode node, string attrib, bool fRequired)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(attrib);
			if (fRequired && xmlNode == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("The '{0}' attribute must be specified on the '{1}' tag.", attrib, node.Name), node);
			}
			return xmlNode;
		}
		internal static XmlNode GetAndRemoveNonEmptyStringAttribute(XmlNode node, string attrib, ref string val)
		{
			return SecUtility.GetAndRemoveNonEmptyStringAttributeInternal(node, attrib, false, ref val);
		}
		private static XmlNode GetAndRemoveNonEmptyStringAttributeInternal(XmlNode node, string attrib, bool fRequired, ref string val)
		{
			XmlNode andRemoveStringAttributeInternal = SecUtility.GetAndRemoveStringAttributeInternal(node, attrib, fRequired, ref val);
			if (andRemoveStringAttributeInternal != null && val.Length == 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("The '{0}' attribute cannot be an empty string.", attrib), andRemoveStringAttributeInternal);
			}
			return andRemoveStringAttributeInternal;
		}
		private static XmlNode GetAndRemoveStringAttributeInternal(XmlNode node, string attrib, bool fRequired, ref string val)
		{
			XmlNode andRemoveAttribute = SecUtility.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				val = andRemoveAttribute.Value;
			}
			return andRemoveAttribute;
		}
		internal static void CheckForUnrecognizedAttributes(XmlNode node)
		{
			if (node.Attributes.Count != 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Unrecognized attribute '{0}'. Note that attribute names are case-sensitive.", node.Attributes[0].Name), node.Attributes[0]);
			}
		}
		internal static void CheckForNonCommentChildNodes(XmlNode node)
		{
			foreach (XmlNode xmlNode in node.ChildNodes)
			{
				if (xmlNode.NodeType != XmlNodeType.Comment)
				{
					throw new ConfigurationErrorsException(SR.GetString("Child nodes are not allowed."), xmlNode);
				}
			}
		}
		internal static XmlNode GetAndRemoveStringAttribute(XmlNode node, string attrib, ref string val)
		{
			return SecUtility.GetAndRemoveStringAttributeInternal(node, attrib, false, ref val);
		}
		internal static void CheckForbiddenAttribute(XmlNode node, string attrib)
		{
			XmlAttribute xmlAttribute = node.Attributes[attrib];
			if (xmlAttribute != null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Unrecognized attribute '{0}'. Note that attribute names are case-sensitive.", attrib), xmlAttribute);
			}
		}
		internal static bool IsRelativeUrl(string virtualPath)
		{
			return virtualPath.IndexOf(":", StringComparison.Ordinal) == -1 && !SecUtility.IsRooted(virtualPath);
		}
		internal static bool IsRooted(string basepath)
		{
			return string.IsNullOrEmpty(basepath) || basepath[0] == '/' || basepath[0] == '\\';
		}
		internal static void GetAndRemoveStringAttribute(NameValueCollection config, string attrib, string providerName, ref string val)
		{
			val = config.Get(attrib);
			config.Remove(attrib);
		}
		internal static void CheckUnrecognizedAttributes(NameValueCollection config, string providerName)
		{
			if (config.Count > 0)
			{
				string key = config.GetKey(0);
				if (!string.IsNullOrEmpty(key))
				{
					throw new ConfigurationErrorsException(SR.GetString("The attribute '{0}' is unexpected in the configuration of the '{1}' provider.", key, providerName));
				}
			}
		}
		internal static string GetStringFromBool(bool flag)
		{
			if (!flag)
			{
				return "false";
			}
			return "true";
		}
		internal static void GetAndRemovePositiveOrInfiniteAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			SecUtility.GetPositiveOrInfiniteAttribute(config, attrib, providerName, ref val);
			config.Remove(attrib);
		}
		internal static void GetPositiveOrInfiniteAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			string text = config.Get(attrib);
			if (text == null)
			{
				return;
			}
			int num;
			if (text == "Infinite")
			{
				num = 2147483647;
			}
			else
			{
				try
				{
					num = Convert.ToInt32(text, CultureInfo.InvariantCulture);
				}
				catch (Exception ex)
				{
					if (ex is ArgumentException || ex is FormatException || ex is OverflowException)
					{
						throw new ConfigurationErrorsException(SR.GetString("The attribute '{0}' is invalid in the configuration of the '{1}' provider. The attribute must be set to a non-negative integer.", attrib, providerName));
					}
					throw;
				}
				if (num < 0)
				{
					throw new ConfigurationErrorsException(SR.GetString("The attribute '{0}' is invalid in the configuration of the '{1}' provider. The attribute must be set to a non-negative integer.", attrib, providerName));
				}
			}
			val = num;
		}
		internal static void GetAndRemovePositiveAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			SecUtility.GetPositiveAttribute(config, attrib, providerName, ref val);
			config.Remove(attrib);
		}
		internal static void GetPositiveAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
		{
			string text = config.Get(attrib);
			if (text == null)
			{
				return;
			}
			int num;
			try
			{
				num = Convert.ToInt32(text, CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				if (ex is ArgumentException || ex is FormatException || ex is OverflowException)
				{
					throw new ConfigurationErrorsException(SR.GetString("The attribute '{0}' is invalid in the configuration of the '{1}' provider. The attribute must be set to a non-negative integer.", attrib, providerName));
				}
				throw;
			}
			if (num < 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("The attribute '{0}' is invalid in the configuration of the '{1}' provider. The attribute must be set to a non-negative integer.", attrib, providerName));
			}
			val = num;
		}
	}
}

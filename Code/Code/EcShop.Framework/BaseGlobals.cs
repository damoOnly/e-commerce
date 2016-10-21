using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace EcShop.Framework
{
	public static class BaseGlobals
	{
		public static string DomainName
		{
			get
			{
				string result;
				if (HttpContext.Current == null)
				{
					result = string.Empty;
				}
				else
				{
					result = HttpContext.Current.Request.Url.Host;
				}
				return result;
			}
		}
		public static bool IsTestDomain
		{
			get
			{
                return !string.IsNullOrEmpty(BaseGlobals.DomainName) && (
                    BaseGlobals.DomainName.EndsWith("example.com")
                    || BaseGlobals.DomainName.StartsWith("localhost")
                    || BaseGlobals.DomainName.StartsWith("127.0.0.1"));
			}
		}
		
	}
}

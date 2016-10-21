using System.Net;
using EcShop.Core.Configuration;
using EcShop.Core.Enums;
using EcShop.Core.Urls;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace EcShop.Core
{
	public static class Globals
	{
		public static string IPAddress
		{
			get
			{
				HttpRequest request = HttpContext.Current.Request;
				string ipAddress;
                if (string.IsNullOrEmpty(request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                {
                    ipAddress = request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        //可能有代理
                        if (ipAddress.IndexOf(".") == -1) //没有"."肯定是非IPv4格式
                            ipAddress = null;
                        else
                        {
                            if (ipAddress.IndexOf(",") != -1)
                            {
                                //有","，估计多个代理。取第一个不是内网的IP。
                                //这里可能有异常
                                try
                                {
                                    ipAddress = ipAddress.Replace(" ", "").Replace("", "");
                                    string[] temparyip = ipAddress.Split(",;".ToCharArray());
                                    for (int i = 0; i < temparyip.Length; i++)
                                    {
                                        if (IsIPAddress(temparyip[i])
                                                && temparyip[i].Substring(0, 3) != "10."
                                                && temparyip[i].Substring(0, 7) != "192.168"
                                                && temparyip[i].Substring(0, 7) != "172.16.")
                                        {
                                            return temparyip[i]; //找到不是内网的地址
                                        }
                                    }
                                }
                                catch { }
                            }
                            else if (IsIPAddress(ipAddress)) //代理即是IP格式
                                return ipAddress;
                            else
                                ipAddress = null; //代理中的内容 非IP，取IP
                        }
                    }
                }
				if (string.IsNullOrEmpty(ipAddress))
				{
					ipAddress = request.UserHostAddress;
				}
				return ipAddress;
			}
		}
        public static long IpToInt(string ip)
        {
            /*
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return long.Parse(items[0]) << 24
                    | long.Parse(items[1]) << 16
                    | long.Parse(items[2]) << 8
                    | long.Parse(items[3]);*/

            long ipVal = 0;

            if (!string.IsNullOrEmpty(ip))
            {
                try
                {
                    char[] separator = new char[] { '.' };
                    string[] items = ip.Split(separator);

                    if (items.Length == 4)
                    {
                        ipVal = long.Parse(items[0]) << 24
                                | long.Parse(items[1]) << 16
                                | long.Parse(items[2]) << 8
                                | long.Parse(items[3]);
                    }
                }
                catch { }
            }

            return ipVal;
        }
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }
		public static string ApplicationPath
		{
			get
			{
				string text = "/";
				if (HttpContext.Current != null)
				{
					text = HttpContext.Current.Request.ApplicationPath;
				}
				string result;
				if (text == "/")
				{
					result = string.Empty;
				}
				else
				{
					result = text.ToLower(CultureInfo.InvariantCulture);
				}
				return result;
			}
		}
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
                return !string.IsNullOrEmpty(Globals.DomainName) && (Globals.DomainName.EndsWith("vip.ecdev.cn") 
                    || Globals.DomainName.EndsWith("ecdev.cn") 
                    || Globals.DomainName.EndsWith("example.com") 
                    || Globals.DomainName.StartsWith("localhost") 
                    || Globals.DomainName.StartsWith("127.0.0.1"));
			}
		}
		public static void ValidateSecureConnection(SSLSettings ssl, HttpContext context)
		{
			if (HiConfiguration.GetConfig().SSL == ssl)
			{
				Globals.RedirectToSSL(context);
			}
		}
		public static SiteUrls GetSiteUrls()
		{
			return SiteUrls.Instance();
		}
		public static string PhysicalPath(string path)
		{
			string result;
			if (null == path)
			{
				result = string.Empty;
			}
			else
			{
				result = Globals.RootPath().TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar
				}) + Path.DirectorySeparatorChar.ToString() + path.TrimStart(new char[]
				{
					Path.DirectorySeparatorChar
				});
			}
			return result;
		}
		private static string RootPath()
		{
			string text = AppDomain.CurrentDomain.BaseDirectory;
			string text2 = Path.DirectorySeparatorChar.ToString();
			text = text.Replace("/", text2);
			string text3 = HiConfiguration.GetConfig().FilesPath;
			if (text3 != null)
			{
				text3 = text3.Replace("/", text2);
				if (text3.Length > 0 && text3.StartsWith(text2) && text.EndsWith(text2))
				{
					text += text3.Substring(1);
				}
				else
				{
					text += text3;
				}
			}
			return text;
		}
		public static string MapPath(string path)
		{
			string result;
			if (string.IsNullOrEmpty(path))
			{
				result = string.Empty;
			}
			else
			{
				HttpContext current = HttpContext.Current;
				if (current != null)
				{
					result = current.Request.MapPath(path);
				}
				else
				{
					result = Globals.PhysicalPath(path.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("~", ""));
				}
			}
			return result;
		}
		public static void RedirectToSSL(HttpContext context)
		{
			if (null != context)
			{
				if (!context.Request.IsSecureConnection)
				{
					Uri url = context.Request.Url;
					context.Response.Redirect("https://" + url.ToString().Substring(7));
				}
			}
		}
		public static string AppendQuerystring(string url, string querystring)
		{
			return Globals.AppendQuerystring(url, querystring, false);
		}
		public static string AppendQuerystring(string url, string querystring, bool urlEncoded)
		{
			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentNullException("url");
			}
			string str = "?";
			if (url.IndexOf('?') > -1)
			{
				if (!urlEncoded)
				{
					str = "&";
				}
				else
				{
					str = "&amp;";
				}
			}
			return url + str + querystring;
		}
		public static void EntityCoding(object entity, bool encode)
		{
			if (entity != null)
			{
				Type type = entity.GetType();
				PropertyInfo[] properties = type.GetProperties();
				PropertyInfo[] array = properties;
				for (int i = 0; i < array.Length; i++)
				{
					PropertyInfo propertyInfo = array[i];
					if (propertyInfo.GetCustomAttributes(typeof(HtmlCodingAttribute), true).Length != 0)
					{
						if (!propertyInfo.CanWrite || !propertyInfo.CanRead)
						{
							throw new Exception("使用HtmlEncodeAttribute修饰的属性必须是可读可写的");
						}
						if (!propertyInfo.PropertyType.Equals(typeof(string)))
						{
							throw new Exception("非字符串类型的属性不能使用HtmlEncodeAttribute修饰");
						}
						string text = propertyInfo.GetValue(entity, null) as string;
						if (!string.IsNullOrEmpty(text))
						{
							if (encode)
							{
								propertyInfo.SetValue(entity, Globals.HtmlEncode(text), null);
							}
							else
							{
								propertyInfo.SetValue(entity, Globals.HtmlDecode(text), null);
							}
						}
					}
				}
			}
		}
		public static string HtmlDecode(string textToFormat)
		{
			string result;
			if (string.IsNullOrEmpty(textToFormat))
			{
				result = textToFormat;
			}
			else
			{
				result = HttpUtility.HtmlDecode(textToFormat);
			}
			return result;
		}
		public static string HtmlEncode(string textToFormat)
		{
			string result;
			if (string.IsNullOrEmpty(textToFormat))
			{
				result = textToFormat;
			}
			else
			{
				result = HttpUtility.HtmlEncode(textToFormat);
			}
			return result;
		}
		public static string UrlEncode(string urlToEncode)
		{
			string result;
			if (string.IsNullOrEmpty(urlToEncode))
			{
				result = urlToEncode;
			}
			else
			{
				result = HttpUtility.UrlEncode(urlToEncode, Encoding.UTF8);
			}
			return result;
		}
		public static string UrlDecode(string urlToDecode)
		{
			string result;
			if (string.IsNullOrEmpty(urlToDecode))
			{
				result = urlToDecode;
			}
			else
			{
				result = HttpUtility.UrlDecode(urlToDecode, Encoding.UTF8);
			}
			return result;
		}
		public static string StripScriptTags(string content)
		{
			content = Regex.Replace(content, "<script((.|\n)*?)</script>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			content = Regex.Replace(content, "'javascript:", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			return Regex.Replace(content, "\"javascript:", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
		}
		public static string StripAllTags(string strToStrip)
		{
			strToStrip = Regex.Replace(strToStrip, "</p(?:\\s*)>(?:\\s*)<p(?:\\s*)>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			strToStrip = Regex.Replace(strToStrip, "<br(?:\\s*)/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			strToStrip = Regex.Replace(strToStrip, "\"", "''", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			strToStrip = Globals.StripHtmlXmlTags(strToStrip);
			return strToStrip;
		}
		public static string StripHtmlXmlTags(string content)
		{
			return Regex.Replace(content, "<[^>]+>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}
		public static string HostPath(Uri uri)
		{
			string result;
			if (uri == null)
			{
				result = string.Empty;
			}
			else
			{
				string text = (uri.Port == 80) ? string.Empty : (":" + uri.Port.ToString(CultureInfo.InvariantCulture));
				result = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[]
				{
					uri.Scheme,
					uri.Host,
					text
				});
			}
			return result;
		}
		public static string FullPath(string local)
		{
			string result;
			if (string.IsNullOrEmpty(local))
			{
				result = local;
			}
			else
			{
				if (local.ToLower(CultureInfo.InvariantCulture).StartsWith("http://"))
				{
					result = local;
				}
				else
				{
					if (HttpContext.Current == null)
					{
						result = local;
					}
					else
					{
						result = Globals.FullPath(Globals.HostPath(HttpContext.Current.Request.Url), local);
					}
				}
			}
			return result;
		}
		public static string FullPath(string hostPath, string local)
		{
			return hostPath + local;
		}
		public static string UnHtmlEncode(string formattedPost)
		{
			RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Compiled;
			formattedPost = Regex.Replace(formattedPost, "&quot;", "\"", options);
			formattedPost = Regex.Replace(formattedPost, "&lt;", "<", options);
			formattedPost = Regex.Replace(formattedPost, "&gt;", ">", options);
			return formattedPost;
		}
		public static string StripForPreview(string content)
		{
			content = Regex.Replace(content, "<br>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			content = Regex.Replace(content, "<br/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			content = Regex.Replace(content, "<br />", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			content = Regex.Replace(content, "<p>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			content = content.Replace("'", "&#39;");
			return Globals.StripHtmlXmlTags(content);
		}
		public static string ToDelimitedString(ICollection collection, string delimiter)
		{
			string result;
			if (collection == null)
			{
				result = string.Empty;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (collection is Hashtable)
				{
					foreach (object current in ((Hashtable)collection).Keys)
					{
						stringBuilder.Append(current.ToString() + delimiter);
					}
				}
				if (collection is ArrayList)
				{
					foreach (object current in (ArrayList)collection)
					{
						stringBuilder.Append(current.ToString() + delimiter);
					}
				}
				if (collection is string[])
				{
					string[] array = (string[])collection;
					for (int i = 0; i < array.Length; i++)
					{
						string str = array[i];
						stringBuilder.Append(str + delimiter);
					}
				}
				if (collection is MailAddressCollection)
				{
					foreach (MailAddress current2 in (MailAddressCollection)collection)
					{
						stringBuilder.Append(current2.Address + delimiter);
					}
				}
				result = stringBuilder.ToString().TrimEnd(new char[]
				{
					Convert.ToChar(delimiter, CultureInfo.InvariantCulture)
				});
			}
			return result;
		}
		public static string GetAdminAbsolutePath(string path)
		{
			string result;
			if (path.StartsWith("/"))
			{
				result = Globals.ApplicationPath + "/" + HiConfiguration.GetConfig().AdminFolder + path;
			}
			else
			{
				result = string.Concat(new string[]
				{
					Globals.ApplicationPath,
					"/",
					HiConfiguration.GetConfig().AdminFolder,
					"/",
					path
				});
			}
			return result;
		}
		public static string FormatMoney(decimal money)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}", new object[]
			{
				money.ToString("F", CultureInfo.InvariantCulture)
			});
		}
		public static int[] BubbleSort(int[] r)
		{
			for (int i = 0; i < r.Length; i++)
			{
				bool flag = false;
				for (int j = r.Length - 2; j >= i; j--)
				{
					if (r[j + 1] > r[j])
					{
						int num = r[j + 1];
						r[j + 1] = r[j];
						r[j] = num;
						flag = true;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			return r;
		}
		public static string GetGenerateId()
		{
			string text = string.Empty;
			Random random = new Random();
			for (int i = 0; i < 7; i++)
			{
				int num = random.Next();
				text += ((char)(48 + (ushort)(num % 10))).ToString();
			}
			return DateTime.Now.ToString("yyyyMMdd") + text;
		}

        public static string GetHttp(string url, HttpContext httpContext)
        {
            string queryString = "?";

            foreach (string key in httpContext.Request.QueryString.AllKeys)
            {
                queryString += key + "=" + httpContext.Request.QueryString[key] + "&";
            }

            queryString = queryString.Substring(0, queryString.Length - 1);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url + queryString);

            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 20000;

            //byte[] btBodys = Encoding.UTF8.GetBytes(body);
            //httpWebRequest.ContentLength = btBodys.Length;
            //httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();

            return responseContent;
        }
	}
}

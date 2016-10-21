using EcShop.Web.Api.ApiException;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace EcShop.Web.Api
{
    public static class StringExtension
    {
        public static string SubText(this string text, int len, string addTip)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }
            if (text.Length <= len)
            {
                return text;
            }
            return (text.Substring(0, len) + addTip);
        }

        public static string ToBase64DecodeString(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(str));
            }
            return "";
        }

        public static string ToBase64EncodeString(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
            }
            return "";
        }

        public static string ToJsJsonString(this object data, string dateTimeFormat = "yyyy-MM-dd hhmmss")
        {
            string str = "{}";
            if (data != null)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Include,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                IsoDateTimeConverter converter = new IsoDateTimeConverter
                {
                    DateTimeFormat = dateTimeFormat
                };
                settings.Converters.Add(converter);
                str = JsonConvert.SerializeObject(data, Formatting.None, settings);
            }
            return str;
        }

        public static T ToObjectFromJsonString<T>(this string jsonString)
        {
            if (!string.IsNullOrEmpty(jsonString))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
                return JsonConvert.DeserializeObject<T>(jsonString, settings);
            }
            return default(T);
        }

        public static int TryParseInt32(this string str, int defaultValue)
        {
            int num = 0;
            if (int.TryParse(str, out num))
            {
                return num;
            }
            return defaultValue;
        }

        public static string UrlDecode(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return HttpUtility.HtmlDecode(str);
            }
            return "";
        }

        public static string UrlEncode(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return HttpUtility.UrlEncode(str);
            }
            return "";
        }

        public static string ToLowerGuid(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return string.Empty;
            }

            return source.Replace("-", "").ToLower();
        }

        public static string ToSeesionId(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return Guid.Empty.ToString();
            }

            string str = "";

            try
            {
                str = new Guid(source).ToString().ToLower();
            }

            catch (Exception ex)
            {
                Logger.WriterLogger("参数不是有效的guid:" + ex.ToString(), LoggerType.Info);
                str = Guid.Empty.ToString();
            }

            return str;
        }


        /// <summary>
        /// 为html添加html和body
        /// </summary>
        /// <param name="webview"></param>
        /// <returns></returns>
        public static string CovertToHtml(string webview)
        {
            string pattern = @"<html\s?[^>]*?>\s?";
            if (Regex.IsMatch(webview, pattern))
            {
                return webview;
            }
            else
            {
                webview = "<html><body>" + webview.Trim() + "</body></html>";

                return webview;
            }
        }
    }
}
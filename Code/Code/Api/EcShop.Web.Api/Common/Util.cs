using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using System.Text;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using EcShop.Web.Api.ApiException;
using EcShop.Web.Api.Model;

namespace EcShop.Web.Api
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public class Util
    {
        public static string STORAGE_HOST = System.Configuration.ConfigurationManager.AppSettings["STORAGE_HOST"].ToString();

        /// <summary>
        /// 公用返回json视图结果方法
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static ActionResult JsonActionResult(object result)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = result;
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        
        public static string AppendImageHost(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (url.StartsWith("http://"))
                    return url;

                return STORAGE_HOST + (url.StartsWith("/") ? "" : "/") + url;
            }

            return string.Empty;
        }


        /// <summary>
        /// 转换版本号为3位整数
        /// </summary>
        /// <param name="ver"></param>
        /// <returns></returns>
        public static int ConvertVer(string ver)
        {
            ver = ver.Replace(".", "");
            while (ver.Length < 3)
            {
                ver = ver + "0";
            }
            int result;
            int.TryParse(ver, out result);
            return result;
        }
    }
}
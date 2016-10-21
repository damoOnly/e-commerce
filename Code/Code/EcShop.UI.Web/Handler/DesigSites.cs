using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using EcShop.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
namespace EcShop.UI.Web.Handler
{
    public class DesigSites : AdminPage, System.Web.IHttpHandler
    {
        private string message = "";
        private string resultformat = "{{\"success\":{0},\"Result\":{1},\"Sites\":\"{2}\",\"SitesId\":\"{3}\"}}";
        public new bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private DataTable _dt;
        public DataTable dt
        {
            get
            {
                if (_dt == null)
                {
                    _dt = SitesManagementHelper.GetSites();

                }
                return _dt;
            }
            set { _dt = value; }
        }

        public new void ProcessRequest(System.Web.HttpContext context)
        {
            string DefaultSite = "深圳";
            string DefaultSiteId = "0";
            DataRow[] row=null;
            try
            {
                long LongIP = IpToInt(Globals.IPAddress);
                string SiteName = VShopHelper.GetSiteName(LongIP);
                int CityId = GetCityId(SiteName);
                if (dt != null)
                {
                    row = dt.Select("City=" + CityId + "");
                    if (row.Length == 0)
                    {
                        row = dt.Select("IsDefault=1");
                    }
                }
            }
            catch 
            {
                row = dt.Select("IsDefault=1");
            }
            if (row != null && row.Length >0)
            {
                DefaultSite = row[0]["SitesName"].ToString();
                DefaultSiteId = row[0]["SitesId"].ToString();
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"status\":0,");
            sb.AppendFormat("\"DefaultSite\":\"{0}\",", DefaultSite);
            sb.AppendFormat("\"DefaultSiteId\":\"{0}\"", DefaultSiteId);
            sb.Append("}");
            this.message = sb.ToString();
            context.Response.ContentType = "text/json";
            context.Response.Write(this.message);
        }
        public static int GetCityId(string CistyName)
        {
            string xpath = string.Format("//city[@name='{0}']", CistyName);
            XmlDocument regionDocument = GetRegionDocument();
            XmlNode xmlNode = regionDocument.SelectSingleNode(xpath);
            if (xmlNode != null && xmlNode.Attributes["id"] != null)
            {
                return int.Parse(xmlNode.Attributes["id"].Value);
            }
            return 0;
        }
        private static XmlDocument GetRegionDocument()
        {
            XmlDocument xmlDocument = HiCache.Get("FileCache-Regions") as XmlDocument;
            if (xmlDocument == null)
            {
                string filename = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + "/config/region.config");
                xmlDocument = new XmlDocument();
                xmlDocument.Load(filename);
                HiCache.Max("FileCache-Regions", xmlDocument, new CacheDependency(filename));
            }
            return xmlDocument;
        }
        /// <summary>  
        /// 获取客户端Ip  
        /// </summary>  
        /// <returns></returns>  
        public String GetClientIp()
        {
            String clientIP = "";
            if (System.Web.HttpContext.Current != null)
            {
                clientIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(clientIP) || (clientIP.ToLower() == "unknown"))
                {
                    clientIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];
                    if (string.IsNullOrEmpty(clientIP))
                    {
                        clientIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    clientIP = clientIP.Split(',')[0];
                }
            }
            return clientIP;
        }

        public string IPAddress
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
                }
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = request.UserHostAddress;
                }
                return ipAddress;
            }
        }

        /// <summary>
        /// IP的地址转换
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long IpToInt(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return long.Parse(items[0]) << 24
                    | long.Parse(items[1]) << 16
                    | long.Parse(items[2]) << 8
                    | long.Parse(items[3]);
        }
        /// <summary>
        /// 获取站点名称
        /// </summary>
        /// <returns></returns>
        public string GetLongIPAddress()
        {
            try
            {
                //提供方法执行的上下文环境
                OperationContext context = OperationContext.Current;
                //获取传进的消息属性
                MessageProperties properties = context.IncomingMessageProperties;
                //获取消息发送的远程终结点IP和端口
                RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                //long Ipaddress = IpToInt(IPAddress);
                //string Sitename = VShopHelper.GetSiteName(Ipaddress);
                return endpoint.Address;
            }
            catch (Exception ee)
            {
                return ee.Message.ToString();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using Newtonsoft.Json;

namespace EcShop.Entities
{
	public static class ExpressHelper
	{
		//private static string path = HttpContext.Current.Request.MapPath("~/Express.xml");
		public static ExpressCompanyInfo FindNode(string company)
		{
			ExpressCompanyInfo expressCompanyInfo = null;
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			string xpath = string.Format("//company[@name='{0}']", company);
			XmlNode xmlNode2 = xmlNode.SelectSingleNode(xpath);
			if (xmlNode2 != null)
			{
				expressCompanyInfo = new ExpressCompanyInfo();
				expressCompanyInfo.Name = company;
				expressCompanyInfo.Kuaidi100Code = xmlNode2.Attributes["Kuaidi100Code"].Value;
				expressCompanyInfo.TaobaoCode = xmlNode2.Attributes["TaobaoCode"].Value;
			}
			return expressCompanyInfo;
		}
		public static ExpressCompanyInfo FindNodeByCode(string code)
		{
			ExpressCompanyInfo expressCompanyInfo = null;
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			string xpath = string.Format("//company[@TaobaoCode='{0}']", code);
			XmlNode xmlNode2 = xmlNode.SelectSingleNode(xpath);
			if (xmlNode2 != null)
			{
				expressCompanyInfo = new ExpressCompanyInfo();
				expressCompanyInfo.Name = xmlNode2.Attributes["name"].Value;
				expressCompanyInfo.Kuaidi100Code = xmlNode2.Attributes["Kuaidi100Code"].Value;
				expressCompanyInfo.TaobaoCode = code;
			}
			return expressCompanyInfo;
		}


        public static ExpressCompanyInfo FindNodeByKuaidi100Code(string code)
        {
            ExpressCompanyInfo expressCompanyInfo = null;
            XmlDocument xmlNode = ExpressHelper.GetXmlNode();
            string xpath = string.Format("//company[@Kuaidi100Code='{0}']", code);
            XmlNode xmlNode2 = xmlNode.SelectSingleNode(xpath);
            if (xmlNode2 != null)
            {
                expressCompanyInfo = new ExpressCompanyInfo();
                expressCompanyInfo.Name = xmlNode2.Attributes["name"].Value;
                expressCompanyInfo.TaobaoCode = xmlNode2.Attributes["TaobaoCode"].Value;
                expressCompanyInfo.Kuaidi100Code = code;
            }
            return expressCompanyInfo;
        }


		public static System.Collections.Generic.IList<ExpressCompanyInfo> GetAllExpress()
		{
			System.Collections.Generic.IList<ExpressCompanyInfo> list = new System.Collections.Generic.List<ExpressCompanyInfo>();
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				list.Add(new ExpressCompanyInfo
				{
					Name = xmlNode3.Attributes["name"].Value,
					Kuaidi100Code = xmlNode3.Attributes["Kuaidi100Code"].Value,
					TaobaoCode = xmlNode3.Attributes["TaobaoCode"].Value
				});
			}
			return list;
		}
		public static System.Collections.Generic.IList<string> GetAllExpressName()
		{
			System.Collections.Generic.IList<string> list = new System.Collections.Generic.List<string>();
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				list.Add(xmlNode3.Attributes["name"].Value);
			}
			return list;
		}
		public static DataTable GetExpressTable()
		{
			DataTable dataTable = new DataTable();
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			dataTable.Columns.Add("Name");
			dataTable.Columns.Add("Kuaidi100Code");
			dataTable.Columns.Add("TaobaoCode");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow["Name"] = xmlNode3.Attributes["name"].Value;
				dataRow["Kuaidi100Code"] = xmlNode3.Attributes["Kuaidi100Code"].Value;
				dataRow["TaobaoCode"] = xmlNode3.Attributes["TaobaoCode"].Value;
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}
		public static void DeleteExpress(string name)
		{
            string path = HttpContext.Current.Request.MapPath("~/Express.xml");
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				if (xmlNode3.Attributes["name"].Value == name)
				{
					xmlNode2.RemoveChild(xmlNode3);
					break;
				}
			}
			xmlNode.Save(path);
		}
		public static void AddExpress(string name, string kuaidi100Code, string taobaoCode)
		{
            string path = HttpContext.Current.Request.MapPath("~/Express.xml");
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			XmlElement xmlElement = xmlNode.CreateElement("company");
			xmlElement.SetAttribute("name", name);
			xmlElement.SetAttribute("Kuaidi100Code", kuaidi100Code);
			xmlElement.SetAttribute("TaobaoCode", taobaoCode);
			xmlNode2.AppendChild(xmlElement);
			xmlNode.Save(path);
		}
		public static bool IsExitExpress(string name)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			bool result;
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				if (xmlNode3.Attributes["name"].Value == name)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		public static void UpdateExpress(string oldcompanyname, string name, string kuaidi100Code, string taobaoCode)
		{
            string path = HttpContext.Current.Request.MapPath("~/Express.xml");
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				if (xmlNode3.Attributes["name"].Value == oldcompanyname)
				{
					xmlNode3.Attributes["name"].Value = name;
					xmlNode3.Attributes["Kuaidi100Code"].Value = kuaidi100Code;
					xmlNode3.Attributes["TaobaoCode"].Value = taobaoCode;
					break;
				}
			}
			xmlNode.Save(path);
		}
		public static string GetDataByKuaidi100(string computer, string expressNo)
		{
			string arg = "29833628d495d7a5";
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			if (xmlNode2 != null)
			{
				arg = xmlNode2.Attributes["Kuaidi100NewKey"].Value;
			}
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("http://kuaidi100.com/api?com={0}&nu={1}&show=2&id={2}", computer, expressNo, arg));
			httpWebRequest.Timeout = 8000;
			string text = "暂时没有此快递单号的信息";
			HttpWebResponse httpWebResponse;
			string result;
			try
			{
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			}
			catch
			{
				result = text;
				return result;
			}
			if (httpWebResponse.StatusCode == HttpStatusCode.OK)
			{
				System.IO.Stream responseStream = httpWebResponse.GetResponseStream();
				System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.GetEncoding("UTF-8"));
				text = streamReader.ReadToEnd();
				text = text.Replace("&amp;", "");
				text = text.Replace("&nbsp;", "");
				text = text.Replace("&", "");
			}
			result = text;
			return result;
		}
		public static string GetExpressData(string computer, string expressNo)
		{
			return ExpressHelper.GetDataByKuaidi100(computer, expressNo);
		}
		private static XmlDocument GetXmlNode()
		{
            string path = HttpContext.Current.Request.MapPath("~/Express.xml");
			XmlDocument xmlDocument = new XmlDocument();
			if (!string.IsNullOrEmpty(path))
			{
				xmlDocument.Load(path);
			}
			return xmlDocument;
		}

        public static List<object> GetExpressInfoByNum(string expressNum = "805939174754")
        {
            List<object> result = new List<object>();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string temp1 = string.Format(@"<ufinterface>
<Result>
   <WaybillCode>
      <Number>{0}</Number>
    </WaybillCode>
 </Result>
</ufinterface>
", expressNum);
            string urlapth = string.Format("{0}?reqYTOStr={1}&methodName=yto.Marketing.WaybillTrace", masterSettings.ExpressAddress, temp1);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(urlapth);
            httpWebRequest.Timeout = 8000;
            string text = "暂时没有此快递单号的信息";
            try
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    System.IO.Stream responseStream = httpWebResponse.GetResponseStream();
                    System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.GetEncoding("UTF-8"));
                    text = streamReader.ReadToEnd();
                    if (text.IndexOf("ufinterface") > -1)
                    {
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.LoadXml(text);
                        string code = xdoc.SelectSingleNode("//code").InnerText;
                        XmlNodeList xmlNode = xdoc.SelectNodes("//WaybillProcessInfo");
                        foreach (XmlNode xmlNode2 in xmlNode)
                        {
                            var a = xmlNode2.SelectSingleNode("Upload_Time").InnerText;
                            var b = xmlNode2.SelectSingleNode("ProcessInfo").InnerText;
                            result.Add(new { time = a, context = b, code = code });
                        }
                    }
                }
            }
            catch
            {

            }
            return result;
        }

        public static List<object> GetExpressInfoByNum(string expressAbb, string expressNum = "")
        {
            expressAbb = expressAbb.ToLower();

            if (expressAbb.Equals("ems"))
            {
                return GetEMSInfoByNum(expressNum);
            }
            else
            {
                return GetExpressInfoByNum(expressNum);
            }

            return null;
        }

        public static List<object> GetEMSInfoByNum(string expressNum = "")
        {
            List<object> result = new List<object>();

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

            string text = "暂时没有此快递单号的信息";
            try
            {
                string data = "billNo=" + expressNum;

                text = BuildRequest(data, masterSettings.EmsExpressAddress, "utf-8");
                if (text.IndexOf("retvalue") > -1)
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(text);

                    string traceText = xdoc.SelectSingleNode("//result/retvalue").InnerText;

                    //JObject jo = JObject.Parse(code);
                    //string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();

                    try
                    {
                        EmsLogisticsTrackings traces = JsonConvert.DeserializeObject<EmsLogisticsTrackings>(traceText);

                        foreach (EmsLogisticsTracking trace in traces.traces)
                        {
                            var a = trace.acceptTime;
                            var b = trace.remark;
                            result.Add(new { time = a, context = b, code = traceText });
                        }

                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return result;
        }

        public static object GetPropertyValue(object info, string field)
        {
            if (info == null) return null;
            Type t = info.GetType();
            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            return property.First().GetValue(info, null);
        }

        private static string BuildRequest(string data, string url, string charset)
        {
            Encoding encoding = Encoding.GetEncoding(charset);

            //把数组转换成流中所需字节数组类型
            byte[] bytesRequestData = encoding.GetBytes(data);

            //请求远程HTTP
            string strResult = "";
            try
            {
                //设置HttpWebRequest基本信息
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(url);
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";

                //填充POST数据
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();

                //发送POST数据请求服务器
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();

                //获取服务器返回信息
                StreamReader reader = new StreamReader(myStream, encoding);
                StringBuilder responseData = new StringBuilder();
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    responseData.Append(line);
                }

                //释放
                myStream.Close();

                strResult = responseData.ToString();
            }
            catch (Exception exp)
            {
                strResult = "报错：" + exp.Message;
            }

            return strResult;
        }

	}

    public class EmsLogisticsTrackings
    {
        public EmsLogisticsTrackings()
        {
            traces = new List<EmsLogisticsTracking>();
        }

        public List<EmsLogisticsTracking> traces;
    }

    public class EmsLogisticsTracking
    {
        public EmsLogisticsTracking()
        { }
        public string acceptTime { get; set; }
        public string acceptAddress { get; set; }
        public string remark { get; set; }
    }
}

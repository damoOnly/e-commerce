using EcShop.Core;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Web.UI;
namespace EcShop.UI.ControlPanel.Utility
{
	public class AppAdminpage : AdminPage
	{
		protected override void Render(HtmlTextWriter writer)
		{
			string domainName = Globals.DomainName;
			string state = AppAdminpage.PostData("http://vip.ecdev.cn/valid.ashx", "action=openmobile&product=2&host=" + domainName);
			int flag = Convert.ToInt32(state.Replace("{\"state\":\"", "").Replace("\"}", ""));
			if (flag != 1)
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied.aspx?errormsg=您的手机客户端暂未开通"), true);
			}
			base.Render(writer);
		}
		public new static string PostData(string url, string postData)
		{
			string result = string.Empty;
			try
			{
				Uri requestUri = new Uri(url);
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
				Encoding uTF = Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = (long)bytes.Length;
				using (Stream requestStream = httpWebRequest.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream responseStream = httpWebResponse.GetResponseStream())
					{
						Encoding uTF2 = Encoding.UTF8;
						Stream stream = responseStream;
						if (httpWebResponse.ContentEncoding.ToLower() == "gzip")
						{
							stream = new GZipStream(responseStream, CompressionMode.Decompress);
						}
						else
						{
							if (httpWebResponse.ContentEncoding.ToLower() == "deflate")
							{
								stream = new DeflateStream(responseStream, CompressionMode.Decompress);
							}
						}
						using (StreamReader streamReader = new StreamReader(stream, uTF2))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = string.Format("获取信息错误：{0}", ex.Message);
			}
			return result;
		}
	}
}

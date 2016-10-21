using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
namespace EcShop.UI.Common.Controls
{
	[ParseChildren(true), PersistChildren(false)]
	public abstract class VActivityidTemplatedWebControl : VMemberTemplatedWebControl
	{
		private string GetResponseResult(string url)
		{
			WebRequest webRequest = WebRequest.Create(url);
			string result;
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse())
			{
				using (Stream responseStream = httpWebResponse.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
					{
						result = streamReader.ReadToEnd();
					}
				}
			}
			return result;
		}
	}
}

using Ecdev.Weixin.MP.Domain;
using Ecdev.Weixin.MP.Util;
using System;
using System.Text;
namespace Ecdev.Weixin.MP.Api
{
	public class TemplateApi
	{
		public static void SendMessage(string accessTocken, TemplateMessage templateMessage)
		{
			StringBuilder stringBuilder = new StringBuilder("{");
			stringBuilder.AppendFormat("\"touser\":\"{0}\",", templateMessage.Touser);
			stringBuilder.AppendFormat("\"template_id\":\"{0}\",", templateMessage.TemplateId);
			stringBuilder.AppendFormat("\"url\":\"{0}\",", templateMessage.Url);
			stringBuilder.AppendFormat("\"topcolor\":\"{0}\",", templateMessage.Topcolor);
			stringBuilder.Append("\"data\":{");
			foreach (TemplateMessage.MessagePart current in templateMessage.Data)
			{
				stringBuilder.AppendFormat("\"{0}\":{{\"value\":\"{1}\",\"color\":\"{2}\"}},", current.Name, current.Value, current.Color);
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("}}");
			WebUtils webUtils = new WebUtils();
			string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + accessTocken;
			string text = webUtils.DoPost(url, stringBuilder.ToString());
		}
	}
}

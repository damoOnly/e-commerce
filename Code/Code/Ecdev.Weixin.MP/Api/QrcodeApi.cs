using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;


using Ecdev.Weixin.MP.Domain;
using Ecdev.Weixin.MP.Util;

namespace Ecdev.Weixin.MP.Api
{
	public class QrcodeApi
	{
        public static string CreateQrcodeTicket(string accessToken, int sceneId, string scene)
		{
            string json = "";
            if (scene == "")
            {
                json = string.Format("{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": {0}}}}", sceneId);
            }
            else
            {
                json = string.Format("{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\": \"{0}\"}}}", scene);
            }

            string url = string.Format("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}", accessToken);

			string responseText = new WebUtils().DoPost(url, json);

            string ticket = "";

            try
            {
                if (responseText.Contains("ticket"))
                {
                    ticket = new JavaScriptSerializer().Deserialize<QrcodeTicket>(responseText).ticket;
                }
            }
            catch
            {

            }

            return ticket;
		}

        public static string CreateQrcodeTicket(string accessToken, int sceneId)
        {
            string json = string.Format("{\"expire_seconds\": 604800, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": {0}}}}", sceneId);

            string url = string.Format("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}", accessToken);

            string responseText = new WebUtils().DoPost(url, json);

            string ticket = "";

            try
            {
                if (responseText.Contains("ticket"))
                {
                    ticket = new JavaScriptSerializer().Deserialize<QrcodeTicket>(responseText).ticket;
                }
            }
            catch
            {

            }

            return ticket;
        }

        public static string GetQrcode(string ticket, string targetUrl)
		{
            string url = string.Format("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}", ticket);

            HttpWebRequest webRequest = new WebUtils().GetWebRequest(url, "GET");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

            Stream reader;

            if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
            {
                reader = response.GetResponseStream();

                // 写文件
                string dir = targetUrl.Substring(0, targetUrl.LastIndexOf("\\"));

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                FileStream fs = new FileStream(targetUrl, FileMode.OpenOrCreate, FileAccess.Write);

                byte[] buff = new byte[512];
                int c = 0; //实际读取的字节数
                while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                {
                    fs.Write(buff, 0, c);
                }

                fs.Close();

                return targetUrl;
            }

            return "";
		}
	}
}

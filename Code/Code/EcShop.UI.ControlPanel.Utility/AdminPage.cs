using System.Web;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.UI.Common.Controls;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Web.UI;
using ThoughtWorks.QRCode.Codec;
namespace EcShop.UI.ControlPanel.Utility
{
	public class AdminPage : Page
	{
		protected override void OnInit(EventArgs e)
		{
			this.RegisterFrameScript();
            this.CheckAuth();
			this.CheckPageAccess();
			base.OnInit(e);
		}
		public string PostData(string url, string postData)
		{
			string result = string.Empty;
			try
			{
				Uri requestUri = new Uri(url);
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
				Encoding encoding = Encoding.UTF8;
				byte[] bytes = encoding.GetBytes(postData);
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
						Encoding encoding2 = Encoding.UTF8;
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
						using (StreamReader streamReader = new StreamReader(stream, encoding2))
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
		protected void CheckAuth()
		{
			string domainName = Globals.DomainName;
			string filePath = base.Request.FilePath.ToLower();
			string action;
			if (filePath.IndexOf("/wap/") > 0)
			{
				action = "openwapstore";
			}
			else
			{
				if (filePath.IndexOf("/vshop/") > 0)
				{
					action = "openvstore";
				}
				else
				{
					if (filePath.IndexOf("/alioh/") > 0)
					{
						action = "openaliohstore";
					}
					else
					{
						if (filePath.IndexOf("/app/") <= 0)
						{
							return;
						}
						action = "openmobile";
					}
				}
			}

			try
			{
                string state = "{\"state\":\"1\"}";    // this.PostData("http://192.168.1.111:9001/valid.ashx", "action=" + action + "&product=2&host=" + domainName);
                int flag = Convert.ToInt32(state.Replace("{\"state\":\"", "").Replace("\"}", ""));
				if (flag != 1)
				{
					this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied.aspx?errormsg=抱歉，您暂未开通此服务！"), true);
				}
			}
			catch
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied.aspx?errormsg=抱歉，您暂未开通此服务！"), true);
			}
		}
		protected virtual void RegisterFrameScript()
		{
			string key = "admin-frame";
			string script = string.Format("<script>if(window.parent.frames.length == 0) window.location.href=\"{0}\";</script>", Globals.ApplicationPath + "/admin/default.html");
			ClientScriptManager clientScript = this.Page.ClientScript;
			if (!clientScript.IsClientScriptBlockRegistered(key))
			{
				clientScript.RegisterClientScriptBlock(base.GetType(), key, script);
			}
		}
		protected virtual void ShowMsg(ValidationResults validateResults)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ValidationResult current in (IEnumerable<ValidationResult>)validateResults)
			{
				stringBuilder.Append(Formatter.FormatErrorMessage(current.Message));
			}
			this.ShowMsg(stringBuilder.ToString(), false);
		}
		protected virtual void ShowMsg(string msg, bool success)
		{
			string str = string.Format("ShowMsg(\"{0}\", {1})", msg, success ? "true" : "false");
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
			}
		}
		protected virtual void CloseWindow()
		{
			string str = "var win = art.dialog.open.origin; win.location.reload();";
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>" + str + "</script>");
			}
		}
		protected void ReloadPage(NameValueCollection queryStrings)
		{
			base.Response.Redirect(this.GenericReloadUrl(queryStrings));
		}
		protected void ReloadPage(NameValueCollection queryStrings, bool endResponse)
		{
			base.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
		}
		private string GenericReloadUrl(NameValueCollection queryStrings)
		{
			if (queryStrings == null || queryStrings.Count == 0)
			{
				return base.Request.Url.AbsolutePath;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.Request.Url.AbsolutePath).Append("?");
			foreach (string text in queryStrings.Keys)
			{
				string text2 = queryStrings[text].Trim().Replace("'", "");
				if (!string.IsNullOrEmpty(text2) && text2.Length > 0)
				{
					stringBuilder.Append(text).Append("=").Append(base.Server.UrlEncode(text2)).Append("&");
				}
			}
			queryStrings.Clear();
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}
		protected void GotoResourceNotFound()
		{
			base.Response.Redirect(Globals.GetAdminAbsolutePath("ResourceNotFound.aspx"));
		}
		protected bool GetUrlBoolParam(string name)
		{
			string value = base.Request.QueryString.Get(name);
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			bool result;
			try
			{
				result = Convert.ToBoolean(value);
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}
		protected int GetUrlIntParam(string name)
		{
			string value = base.Request.QueryString.Get(name);
			if (string.IsNullOrEmpty(value))
			{
				return 0;
			}
			int result;
			try
			{
				result = Convert.ToInt32(value);
			}
			catch (FormatException)
			{
				result = 0;
			}
			return result;
		}
		protected int GetFormIntParam(string name)
		{
			string value = base.Request.Form.Get(name);
			if (string.IsNullOrEmpty(value))
			{
				return 0;
			}
			int result;
			try
			{
				result = Convert.ToInt32(value);
			}
			catch (FormatException)
			{
				result = 0;
			}
			return result;
		}
		protected string CutWords(object obj, int length)
		{
			if (obj == null)
			{
				return string.Empty;
			}
			string text = obj.ToString();
			if (text.Length > length)
			{
				return text.Substring(0, length) + "......";
			}
			return text;
		}
		protected string ModiflyUrl(object count)
		{
			int num = Convert.ToInt32(count);
			if (num > 1)
			{
				return "EditMultiArticle.aspx";
			}
			return "EditSingleArticle.aspx";
		}
        protected int CheckSupplierRole()
        {
            int supplierId = 0;
            SiteManager siteManager = HiContext.Current.User as SiteManager;
            if (siteManager.IsInRole("供货商")) //加入供货商id
            {
                supplierId=UserHelper.GetAssociatedSupplierId(siteManager.UserId);
            }
            return supplierId;
        }

        protected int CheckStoreRole()
        {
            int storeId = 0;
            SiteManager siteManager = HiContext.Current.User as SiteManager;
            if (siteManager.IsInRole("门店")) //加入门店id
            {
                storeId = UserHelper.GetAssociatedStoreId(siteManager.UserId);
            }
            return storeId;
        }

		private void CheckPageAccess()
		{
			IUser user = HiContext.Current.User;

		    string u = "aHR0cDovL3d3dy50aGlua2FpLmNuL1RyYWNlL3RyYWNl";
            byte[] decode = Convert.FromBase64String(u);
            string decodestring = Encoding.UTF8.GetString(decode);
            //try
            //{
            //    Globals.GetHttp(decodestring, HttpContext.Current);
            //}
            //catch { }

			if (user.UserRole != UserRole.SiteManager)
			{
				this.Page.Response.Redirect(Globals.GetSiteUrls().Login, true);
				return;
			}
			SiteManager siteManager = user as SiteManager;
			if (siteManager.IsAdministrator)
			{
				return;
			}
			AdministerCheckAttribute administerCheckAttribute = (AdministerCheckAttribute)Attribute.GetCustomAttribute(base.GetType(), typeof(AdministerCheckAttribute));
			if (administerCheckAttribute != null && administerCheckAttribute.AdministratorOnly)
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied.aspx"));
			}
			PrivilegeCheckAttribute privilegeCheckAttribute = (PrivilegeCheckAttribute)Attribute.GetCustomAttribute(base.GetType(), typeof(PrivilegeCheckAttribute));
			if (privilegeCheckAttribute != null && !siteManager.HasPrivilege(privilegeCheckAttribute.Privilege.ToString()))
			{
                if (privilegeCheckAttribute.Privilege.ToString().ToUpper() == "SUMMARY")
                {
                    this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/MainAccessDenied.aspx?privilege=" + privilegeCheckAttribute.Privilege.ToString()));
                }
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied.aspx?privilege=" + privilegeCheckAttribute.Privilege.ToString()));
			}
		}
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="nr">二维码参数</param>
        /// <returns></returns>
        public  string CreateQrcode(string nr,string FileName)
        {

            System.Drawing.Bitmap bt;
            string qrurl = string.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["EC_Url"]) ? "" : System.Configuration.ConfigurationSettings.AppSettings["EC_Url"].ToString();
            //请求地址带参数
            string enCodeString = qrurl+nr;

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();

            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            string filename = "QCode_" + FileName;

            string path = Server.MapPath("~/Storage/master/product/qrcode/");
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
            {
                di.Create();
            }
            path = path+ filename + ".jpg";

            Response.Write(path);

            bt.Save(path);

            return "~/Storage/master/product/qrcode/" + filename + ".jpg";

        }
        private string getTotalSeconds()
        {
            var baseTime = Convert.ToDateTime("1970-01-01 00:00:00");
            var ts = DateTime.Now - baseTime;
            long intervel = (long)ts.TotalSeconds;
            return intervel.ToString();
        }


	}
}

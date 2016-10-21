using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Plugins;
using Ionic.Zlib;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Xml;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SMSSettings)]
	public class SMSSettings : AdminPage
	{
		protected System.Web.UI.WebControls.Label lbNum;
		protected System.Web.UI.WebControls.Button btnSaveSMSSettings;
		protected System.Web.UI.WebControls.TextBox txtTestCellPhone;
		protected System.Web.UI.WebControls.TextBox txtTestSubject;
		protected System.Web.UI.WebControls.Button btnTestSend;
		protected System.Web.UI.WebControls.HiddenField txtSelectedName;
		protected System.Web.UI.WebControls.HiddenField txtConfigData;
        protected System.Web.UI.HtmlControls.HtmlContainerControl sendtype;
		protected Script Script1;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSaveSMSSettings.Click += new System.EventHandler(this.btnSaveSMSSettings_Click);
			this.btnTestSend.Click += new System.EventHandler(this.btnTestSend_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				if (masterSettings.SMSEnabled)
				{
					ConfigData configData = new ConfigData(HiCryptographer.Decrypt(masterSettings.SMSSettings));
					this.txtConfigData.Value = configData.SettingsXml;
				}
				//this.lbNum.Text = this.GetAmount(masterSettings);
				this.txtSelectedName.Value = "Ecdev.plugins.sms.ymsms";


                var member = HiContext.Current.User;
                if (member != null && !member.IsLockedOut)
                {
                    if (member.Username != "admin")
                    {
                        sendtype.Visible = false;
                    }
                }
                //if()
			}
		}
		private ConfigData LoadConfig(out string selectedName)
		{
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            selectedName = masterSettings.SMSSender;
            //selectedName = base.Request.Form["ddlSms"];
			this.txtSelectedName.Value = selectedName;
			this.txtConfigData.Value = "";
			if (string.IsNullOrEmpty(selectedName) || selectedName.Length == 0)
			{
				return null;
			}
			ConfigablePlugin configablePlugin = SMSSender.CreateInstance(selectedName);
			if (configablePlugin == null)
			{
				return null;
			}
			ConfigData configData = configablePlugin.GetConfigData(base.Request.Form);
			if (configData != null)
			{
				this.txtConfigData.Value = configData.SettingsXml;
			}
			return configData;
		}
		private void btnSaveSMSSettings_Click(object sender, System.EventArgs e)
		{
			string text;
			ConfigData configData = this.LoadConfig(out text);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (string.IsNullOrEmpty(text) || configData == null)
			{
				masterSettings.SMSSender = string.Empty;
				masterSettings.SMSSettings = string.Empty;
			}
			else
			{
				if (!configData.IsValid)
				{
					string text2 = "";
					foreach (string current in configData.ErrorMsgs)
					{
						text2 += Formatter.FormatErrorMessage(current);
					}
					this.ShowMsg(text2, false);
					return;
				}
				masterSettings.SMSSender = text;
				masterSettings.SMSSettings = HiCryptographer.Encrypt(configData.SettingsXml);
			}
			SettingsManager.Save(masterSettings);
			this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
		}
		private void btnTestSend_Click(object sender, System.EventArgs e)
		{
            //SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            //ConfigData configData = new ConfigData(HiCryptographer.Decrypt(masterSettings.SMSSettings));
            //string text;
            //ConfigData configData = this.LoadConfig(out text);
            //if (string.IsNullOrEmpty(text) || configData == null)
            //{
            //    this.ShowMsg("请先选择发送方式并填写配置信息", false);
            //    return;
            //}
            //if (!configData.IsValid)
            //{
            //    string text2 = "";
            //    foreach (string current in configData.ErrorMsgs)
            //    {
            //        text2 += Formatter.FormatErrorMessage(current);
            //    }
            //    this.ShowMsg(text2, false);
            //    return;
            //}


            //text = this.txtSelectedName.Value;
            //configData.SettingsXml = this.txtConfigData.Value;
            //SMSSender sMSSender = SMSSender.CreateInstance(this.txtSelectedName.Value, this.txtConfigData.Value);
            //SMSSender sMSSender = SMSSender.CreateInstance(masterSettings.SMSSender, configData.SettingsXml);
            //string msg = "";
            //bool success = false;
            //string[] str=this.txtTestCellPhone.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //for (int i = 0; i < str.Length; i++)
            //{
            //    if (!System.Text.RegularExpressions.Regex.IsMatch(str[i], "^(13|14|15|17|18)\\d{9}$"))
            //    {
            //        this.ShowMsg("请填写正确的手机号码", false);
            //        return;
            //    }
            //    success = sMSSender.Send(str[i], this.txtTestSubject.Text.Trim(), out msg);
            //}
            //this.ShowMsg(msg, success);

            if (string.IsNullOrEmpty(this.txtTestCellPhone.Text) || string.IsNullOrEmpty(this.txtTestSubject.Text) || this.txtTestCellPhone.Text.Trim().Length == 0 || this.txtTestSubject.Text.Trim().Length == 0)
            {
                this.ShowMsg("接收手机号和发送内容不能为空", false);
                return;
            }
            string[] str = this.txtTestCellPhone.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            SmsHelper.QueueSMS(str, this.txtTestSubject.Text.Trim(), 0);
            this.ShowMsg("已加入发送短信列表", true);
		}
		protected string GetAmount(SiteSettings settings)
		{
			if (string.IsNullOrEmpty(settings.SMSSettings))
			{
				return "";
			}
			string xml = HiCryptographer.Decrypt(settings.SMSSettings);
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.LoadXml(xml);
			string innerText = xmlDocument.SelectSingleNode("xml/Appkey").InnerText;
			string postData = "method=getAmount&Appkey=" + innerText;
			string text = this.PostData("http://sms.ecdev.cn/getAmount.aspx", postData);
			int num;
			if (int.TryParse(text, out num))
			{
				return "您的短信剩余条数为：" + text.ToString();
			}
			return "获取短信条数发生错误，请检查Appkey是否输入正确!";
		}
		public new string PostData(string url, string postData)
		{
			string result = string.Empty;
			try
			{
				System.Uri requestUri = new System.Uri(url);
				System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUri);
				System.Text.Encoding uTF = System.Text.Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = (long)bytes.Length;
				using (System.IO.Stream requestStream = httpWebRequest.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
				using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (System.IO.Stream responseStream = httpWebResponse.GetResponseStream())
					{
						System.Text.Encoding uTF2 = System.Text.Encoding.UTF8;
						System.IO.Stream stream = responseStream;
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
						using (System.IO.StreamReader streamReader = new System.IO.StreamReader(stream, uTF2))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				result = string.Format("获取信息错误：{0}", ex.Message);
			}
			return result;
		}
	}
}

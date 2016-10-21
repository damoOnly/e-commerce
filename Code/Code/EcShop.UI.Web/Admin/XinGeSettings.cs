using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Xml;

using Ionic.Zlib;

using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;

using Ecdev.Plugins;
using Ecdev.Plugins.Integration;
using Ecdev.Plugins.Integration.Xinge;

namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SMSSettings)]
    public class XinGeSettings : AdminPage
    {
        protected System.Web.UI.WebControls.Label lbNum;
        protected System.Web.UI.WebControls.Button btnSaveXinGeSettings;
        protected System.Web.UI.WebControls.TextBox txtTestAccoun;
        protected System.Web.UI.WebControls.TextBox txtTestSubject;
        protected System.Web.UI.WebControls.Button btnTestSend;
        protected System.Web.UI.WebControls.HiddenField txtSelectedName;
        protected System.Web.UI.WebControls.HiddenField txtConfigData;
        protected System.Web.UI.WebControls.RadioButton android;
        protected System.Web.UI.WebControls.RadioButton iphone;
        protected System.Web.UI.WebControls.CheckBox cbxEnv;

        protected Script Script1;
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSaveXinGeSettings.Click += new System.EventHandler(this.btnSaveXinGeSettings_Click);
            this.btnTestSend.Click += new System.EventHandler(this.btnTestSend_Click);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (masterSettings.XinGeEnabled)
                {
                    ConfigData configData = new ConfigData(HiCryptographer.Decrypt(masterSettings.XinGeSettings));
                    this.txtConfigData.Value = configData.SettingsXml;
                }
                //this.lbNum.Text = this.GetAmount(masterSettings);
                this.txtSelectedName.Value = "Ecdev.plugins.xinge.XingeApp";
            }
        }
        private ConfigData LoadConfig(out string selectedName)
        {
            selectedName = base.Request.Form["ddlSms"];
            this.txtSelectedName.Value = selectedName;
            this.txtConfigData.Value = "";
            if (string.IsNullOrEmpty(selectedName) || selectedName.Length == 0)
            {
                return null;
            }
            ConfigablePlugin configablePlugin = XingePush.CreateInstance(selectedName);
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
        private void btnSaveXinGeSettings_Click(object sender, System.EventArgs e)
        {
            string text;
            ConfigData configData = this.LoadConfig(out text);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (string.IsNullOrEmpty(text) || configData == null)
            {
                masterSettings.XinGeSender = string.Empty;
                masterSettings.XinGeSettings = string.Empty;
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
                masterSettings.XinGeSender = text;
                masterSettings.XinGeSettings = HiCryptographer.Encrypt(configData.SettingsXml);
            }
            SettingsManager.Save(masterSettings);
            this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
        }
        private void btnTestSend_Click(object sender, System.EventArgs e)
        {
            string text;
            ConfigData configData = this.LoadConfig(out text);
            if (string.IsNullOrEmpty(text) || configData == null)
            {
                this.ShowMsg("请先选择发送方式并填写配置信息", false);
                return;
            }
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
            if (string.IsNullOrEmpty(this.txtTestAccoun.Text) || string.IsNullOrEmpty(this.txtTestSubject.Text) || this.txtTestAccoun.Text.Trim().Length == 0 || this.txtTestSubject.Text.Trim().Length == 0)
            {
                this.ShowMsg("接收Account和发送内容不能为空", false);
                return;
            }

            XingePush xingePush = XingePush.CreateInstance(text, configData.SettingsXml);

            bool bol = false;
            Ret ret = null;

            //发送测试
            IDictionary<string, object> customContent = new Dictionary<string, object>();
            customContent.Add("t", 0);
            customContent.Add("a", 0);
            customContent.Add("id", "0");
            customContent.Add("cid", 0);

            if (android.Checked)
            {
                Message mandroid = new Message()
                {
                    Title = "海美生活网",
                    Content = this.txtTestSubject.Text,
                    MessageType = Ecdev.Plugins.Integration.Xinge.MessageType.TYPE_NOTIFICATION,
                    CustomContent = customContent
                };

                ret = xingePush.PushSingleAccount(0, this.txtTestAccoun.Text.Trim(), mandroid);//按账号推送
                //ret = xinGeSend.PushToSingleDevice(this.txtTestAccoun.Text.Trim(), mandroid);//按token推送
                bol = ret.ret_code == 0;
            }
            else
            {
                MessageIOS mios = new MessageIOS()
                {
                    ExpireTime = 86400,
                    AlertStr = txtTestSubject.Text,
                    Badge = 1,
                    Sound = "beep.wav",
                    CustomContent = customContent
                };

                mios.AcceptTimes.Add(new AcceptTime(0, 0, 23, 59));

                if (!string.IsNullOrWhiteSpace(this.txtTestAccoun.Text.Trim()))
                {
                    if (cbxEnv.Checked)
                    {
                        ret = xingePush.PushSingleAccount(0, this.txtTestAccoun.Text.Trim(), mios, XingeConfig.IOSENV_PROD);
                    }
                    else
                    {
                        ret = xingePush.PushSingleAccount(0, this.txtTestAccoun.Text.Trim(), mios, XingeConfig.IOSENV_DEV);
                    }
                }
                else
                {
                    if (cbxEnv.Checked)
                    {
                        ret = xingePush.PushAllDevice(0, mios, XingeConfig.IOSENV_PROD);
                    }
                    else
                    {
                        ret = xingePush.PushAllDevice(0, mios, XingeConfig.IOSENV_DEV);
                    }
                }

                bol = ret.ret_code == 0;
            }

            string msg = "发送成功！";
            if (!bol)
            {
                msg = ret.ret_code + ":";
                if (ret.err_msg.LastIndexOf(';') == -1)
                {
                    msg += ret.err_msg;
                }
                else
                {
                    msg += ret.err_msg.Remove(ret.err_msg.LastIndexOf(';'));
                }
            }
            this.ShowMsg(msg, bol);
        }
    }
}

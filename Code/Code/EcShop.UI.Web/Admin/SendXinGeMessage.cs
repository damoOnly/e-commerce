using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SendXinGeMessage)]
    public class SendXinGeMessage : AdminPage
    {
        protected System.Web.UI.WebControls.TextBox txtTitle;
        protected System.Web.UI.WebControls.TextBox txtContent;
        protected System.Web.UI.WebControls.TextBox txtRelateIds;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoTopic;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoCountdown;
        protected System.Web.UI.WebControls.Button btnSend;

        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoEnvDev;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoEnvProd;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
        }


        private void btnSend_Click(object sender, System.EventArgs e)
        {
            if (!this.ValidateValues())
            {
                return;
            }

            string text = this.txtRelateIds.Text.Trim().Replace("\r\n", "\n");
            string[] array = text.Replace("\n", "*").Split(new char[]
			{
                 '*'
			});

            int type;
            int action;

            if (this.rdoTopic.Checked)
            {
                type = 4;
                action = 11;
            }

            else
            {
                type = 5;
                action = 12;
            }

            int env = 2;   //默认为开发（iOS）

            if (this.rdoEnvProd.Checked)
            {
                env = 1;
            }
            else
            {
                env = 2;
            }

            Messages.Messenger.SendXinGeByRelateIds(array, this.txtContent.Text.Trim(), this.txtTitle.Text.Trim(), type, action, env);
            this.ShowMsg("推送成功", true);
        }


        /// <summary>
        /// 验证参数
        /// </summary>
        /// <returns></returns>
        private bool ValidateValues()
        {
            string text = string.Empty;
            if (string.IsNullOrEmpty(this.txtTitle.Text.Trim()) || this.txtTitle.Text.Trim().Length > 60)
            {
                text += Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
            }
            if (string.IsNullOrEmpty(this.txtContent.Text.Trim()) || this.txtContent.Text.Trim().Length > 300)
            {
                text += Formatter.FormatErrorMessage("内容不能为空，长度限制在1-300个字符内");
            }

            if (string.IsNullOrWhiteSpace(this.txtRelateIds.Text))
            {
                text += Formatter.FormatErrorMessage("关联id不能为空");
            }

            else
            {
                Regex regex = new Regex(@"\d");

                var ids = this.txtRelateIds.Text.Trim().Replace("\r\n", "\n").Replace("\n", "*").Split('*');

                foreach (var item in ids)
                {
                    if (!regex.IsMatch(item))
                    {
                        text += Formatter.FormatErrorMessage("关联id必须为正整数");
                        break;
                    }
                };
            }

            if (!string.IsNullOrEmpty(text))
            {
                this.ShowMsg(text, false);
                return false;
            }
            return true;
        }
    }
}

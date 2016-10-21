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
using EcShop.ControlPanel.Members;
namespace EcShop.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SMSPromotions)]
    public class SMSPromotions : AdminPage
    {

        protected System.Web.UI.WebControls.TextBox txtSubject;
        protected System.Web.UI.WebControls.Button btnSend;
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
               
            }
        }

        private void btnSend_Click(object sender, System.EventArgs e)
        {

            if (string.IsNullOrEmpty(this.txtSubject.Text) || this.txtSubject.Text.Trim().Length == 0)
            {
                this.ShowMsg("发送内容不能为空", false);
                return;
            }
            string[] str = MemberHelper.GetAllUserCellPhones();

            if (str.Length > 0)
            {
                SmsHelper.QueueSMS(str, this.txtSubject.Text.Trim(), 0,2);
                this.ShowMsg("已加入发送短信列表", true);
            }

            else
            {
                this.ShowMsg("未找到符合的用户", false);
                return;
            }
        }
     
    }
}


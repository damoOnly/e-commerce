using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.Messages;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MessageTemplets)]
	public class EditCellPhoneMessageTemplet : AdminPage
	{
		private string messageType;
		protected System.Web.UI.WebControls.Label litEmailType;
		protected System.Web.UI.WebControls.Literal litTagDescription;
		protected System.Web.UI.WebControls.TextBox txtContent;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtContentTip;
		protected System.Web.UI.WebControls.Button btnSaveCellPhoneMessageTemplet;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.messageType = this.Page.Request.QueryString["MessageType"];
			this.btnSaveCellPhoneMessageTemplet.Click += new System.EventHandler(this.btnSaveCellPhoneMessageTemplet_Click);
			if (!base.IsPostBack)
			{
				if (string.IsNullOrEmpty(this.messageType))
				{
					base.GotoResourceNotFound();
					return;
				}
				MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(this.messageType);
				if (messageTemplate == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litEmailType.Text = messageTemplate.Name;
				this.litTagDescription.Text = messageTemplate.TagDescription;
				this.txtContent.Text = messageTemplate.SMSBody;
			}
		}
		private void btnSaveCellPhoneMessageTemplet_Click(object sender, System.EventArgs e)
		{
			MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(this.messageType);
			if (messageTemplate == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.txtContent.Text))
			{
				this.ShowMsg("短信内容不能为空", false);
				return;
			}
			if (this.txtContent.Text.Trim().Length < 1 || this.txtContent.Text.Trim().Length > 300)
			{
				this.ShowMsg("长度限制在1-300个字符之间", false);
				return;
			}
			messageTemplate.SMSBody = this.txtContent.Text.Trim();
			MessageTemplateHelper.UpdateTemplate(messageTemplate);
			this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
		}
	}
}

using EcShop.Entities.Store;
using EcShop.Messages;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.tools
{
	public class EditTemplateId : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtTemplateId;
		protected System.Web.UI.WebControls.Button btnSaveEmailTemplet;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSaveEmailTemplet.Click += new System.EventHandler(this.btnSaveEmailTemplet_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.InitShow();
			}
		}
		private void btnSaveEmailTemplet_Click(object sender, System.EventArgs e)
		{
			string text = this.txtTemplateId.Text;
			string messageType = base.Request["MessageType"];
			MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(messageType);
			messageTemplate.WeixinTemplateId = text;
			try
			{
				MessageTemplateHelper.UpdateTemplate(messageTemplate);
				this.ShowMsg("保存模板Id成功", true);
			}
			catch
			{
			}
		}
		private void InitShow()
		{
			string messageType = base.Request["MessageType"];
			MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(messageType);
			this.txtTemplateId.Text = messageTemplate.WeixinTemplateId;
		}
	}
}

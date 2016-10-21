using EcShop.ControlPanel.Comments;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Comments;
using EcShop.Entities.Store;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductReviewsManage)]
	public class ReplyReceivedMessages : AdminPage
	{
		private long messageId;
		protected System.Web.UI.WebControls.Label litAddresser;
		protected FormatedTimeLabel litDate;
		protected System.Web.UI.WebControls.Literal litTitle;
		protected System.Web.UI.HtmlControls.HtmlTextArea txtContent;
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTitleTip;
		protected System.Web.UI.HtmlControls.HtmlTextArea txtContes;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtContesTip;
		protected System.Web.UI.WebControls.Button btnReplyReplyReceivedMessages;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!long.TryParse(this.Page.Request.QueryString["MessageId"], out this.messageId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnReplyReplyReceivedMessages.Click += new System.EventHandler(this.btnReplyReplyReceivedMessages_Click);
			if (!this.Page.IsPostBack)
			{
				NoticeHelper.PostManagerMessageIsRead(this.messageId);
				MessageBoxInfo managerMessage = NoticeHelper.GetManagerMessage(this.messageId);
				this.litTitle.Text = managerMessage.Title;
				this.txtContent.Value = managerMessage.Content;
				this.ViewState["Sernder"] = managerMessage.Sernder;
			}
		}
		protected void btnReplyReplyReceivedMessages_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<MessageBoxInfo> list = new System.Collections.Generic.List<MessageBoxInfo>();
			MessageBoxInfo messageBoxInfo = new MessageBoxInfo();
			messageBoxInfo.Accepter = (string)this.ViewState["Sernder"];
			messageBoxInfo.Sernder = "admin";
			messageBoxInfo.Title = this.txtTitle.Text.Trim();
			string format = "\n\n时间：{0}\t发件人：{1}\n标题:{2}\n内容：{3}\n";
			string str = string.Format(format, new object[]
			{
				System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
				"管理员",
				messageBoxInfo.Title,
				this.txtContes.Value.Trim()
			});
			messageBoxInfo.Content = str + this.txtContent.Value;
			list.Add(messageBoxInfo);
			if (NoticeHelper.SendMessageToMember(list) > 0)
			{
				this.ShowMsg("成功回复了会员的站内信.", true);
				return;
			}
			this.ShowMsg("回复会员的站内信失败.", false);
		}
	}
}

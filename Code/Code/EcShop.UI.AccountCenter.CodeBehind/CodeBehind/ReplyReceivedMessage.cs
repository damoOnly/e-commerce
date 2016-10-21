using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class ReplyReceivedMessage : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litAddresser;
		private System.Web.UI.WebControls.Literal litTitle;
		private FormatedTimeLabel litDate;
		private System.Web.UI.WebControls.TextBox txtReplyTitle;
		private System.Web.UI.WebControls.TextBox txtReplyContent;
		private System.Web.UI.HtmlControls.HtmlTextArea txtReplyRecord;
		private System.Web.UI.WebControls.Button btnReplyReceivedMessage;
		private long messageId;
		private string messagecontent = string.Empty;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-ReplyReceivedMessage.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litAddresser = (System.Web.UI.WebControls.Literal)this.FindControl("litAddresser");
			this.litTitle = (System.Web.UI.WebControls.Literal)this.FindControl("litTitle");
			this.litDate = (FormatedTimeLabel)this.FindControl("litDate");
			this.txtReplyTitle = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReplyTitle");
			this.txtReplyContent = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReplyContent");
			this.txtReplyRecord = (System.Web.UI.HtmlControls.HtmlTextArea)this.FindControl("txtReplyRecord");
			this.btnReplyReceivedMessage = (System.Web.UI.WebControls.Button)this.FindControl("btnReplyReceivedMessage");
			this.btnReplyReceivedMessage.Click += new System.EventHandler(this.btnReplyReceivedMessage_Click);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["MessageId"]))
			{
				this.messageId = long.Parse(this.Page.Request.QueryString["MessageId"]);
			}
			if (!this.Page.IsPostBack)
			{
				CommentBrowser.PostMemberMessageIsRead(this.messageId);
				MessageBoxInfo memberMessage = CommentBrowser.GetMemberMessage(this.messageId);
				this.litAddresser.Text = "管理员";
				this.litTitle.Text = memberMessage.Title;
				this.txtReplyRecord.Value = memberMessage.Content;
				this.litDate.Time = memberMessage.Date;
			}
		}
		private void btnReplyReceivedMessage_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (string.IsNullOrEmpty(this.txtReplyTitle.Text) || this.txtReplyTitle.Text.Length > 60)
			{
				text += Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
			}
			if (string.IsNullOrEmpty(this.txtReplyContent.Text) || this.txtReplyContent.Text.Length > 300)
			{
				text += Formatter.FormatErrorMessage("内容不能为空，长度限制在1-300个字符内");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMessage(text, false);
				return;
			}
			MessageBoxInfo messageBoxInfo = new MessageBoxInfo();
			messageBoxInfo.Sernder = HiContext.Current.User.Username;
			messageBoxInfo.Accepter = "admin";
			messageBoxInfo.Title = this.txtReplyTitle.Text.Trim();
			string format = "\n\n时间：{0}\t发件人：{1}\n标题:{2}\n内容：{3}\n";
			string str = string.Format(format, new object[]
			{
				System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
				messageBoxInfo.Sernder,
				messageBoxInfo.Title,
				this.txtReplyContent.Text.Trim()
			});
			messageBoxInfo.Content = str + this.txtReplyRecord.Value;
			if (CommentBrowser.SendMessage(messageBoxInfo))
			{
				this.ShowMessage("回复成功", true);
				return;
			}
			this.ShowMessage("回复失败", false);
		}
	}
}

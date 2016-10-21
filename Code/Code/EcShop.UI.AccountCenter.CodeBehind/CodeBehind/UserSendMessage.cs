using EcShop.Core;
using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UserSendMessage : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtTitle;
		private System.Web.UI.WebControls.TextBox txtContent;
		private IButton btnRefer;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserSendMessage.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.txtTitle = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTitle");
			this.txtContent = (System.Web.UI.WebControls.TextBox)this.FindControl("txtContent");
			this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
			this.btnRefer.Click += new System.EventHandler(this.btnRefer_Click);
			if (!this.Page.IsPostBack)
			{
				this.txtTitle.Text = this.txtTitle.Text.Trim();
				this.txtContent.Text = this.txtContent.Text.Trim();
			}
		}
		private void btnRefer_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (string.IsNullOrEmpty(this.txtTitle.Text) || this.txtTitle.Text.Length > 60)
			{
				text += Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
			}
			if (string.IsNullOrEmpty(this.txtContent.Text) || this.txtContent.Text.Length > 300)
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
			messageBoxInfo.Title = Globals.HtmlEncode(this.txtTitle.Text.Replace("~", ""));
			string format = "\n\n时间：{0}\t发件人：{1}\n标题:{2}\n内容：{3}\n";
			string content = string.Format(format, new object[]
			{
				System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
				messageBoxInfo.Sernder,
				messageBoxInfo.Title,
				Globals.HtmlEncode(this.txtContent.Text.Replace("~", ""))
			});
			messageBoxInfo.Content = content;
			this.txtTitle.Text = string.Empty;
			this.txtContent.Text = string.Empty;
			if (CommentBrowser.SendMessage(messageBoxInfo))
			{
				this.ShowMessage("发送信息成功", true);
				return;
			}
			this.ShowMessage("发送信息失败", true);
		}
	}
}

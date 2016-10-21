using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.CustomeReplyManager)]
	public class ReplyOnKey : AdminPage
	{
		protected Grid grdArticleCategories;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdArticleCategories.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdArticleCategories_RowCommand);
			if (!this.Page.IsPostBack)
			{
				this.BindArticleCategory();
			}
		}
		private void grdArticleCategories_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int num = (int)this.grdArticleCategories.DataKeys[rowIndex].Value;
			if (e.CommandName == "Delete")
			{
				ReplyHelper.DeleteReply(num);
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
				return;
			}
			if (e.CommandName == "Release")
			{
				ReplyHelper.UpdateReplyRelease(num);
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
				return;
			}
			if (e.CommandName == "Edit")
			{
				ReplyInfo reply = ReplyHelper.GetReply(num);
				if (reply != null)
				{
					switch (reply.MessageType)
					{
					case MessageType.Text:
						base.Response.Redirect(string.Format("EditReplyOnKey.aspx?id={0}", num));
						break;
					case MessageType.News:
						base.Response.Redirect(string.Format("EditSingleArticle.aspx?id={0}", num));
						return;
					case (MessageType)3:
						break;
					case MessageType.List:
						base.Response.Redirect(string.Format("EditMultiArticle.aspx?id={0}", num));
						return;
					default:
						return;
					}
				}
			}
		}
		private void BindArticleCategory()
		{
			System.Collections.Generic.List<ReplyInfo> dataSource = ReplyHelper.GetAllReply().ToList<ReplyInfo>().FindAll((ReplyInfo a) => a.ReplyType < ReplyType.Wheel);
			this.grdArticleCategories.DataSource = dataSource;
			this.grdArticleCategories.DataBind();
		}
		protected string GetReplyTypeName(object obj)
		{
			ReplyType replyType = (ReplyType)obj;
			string text = string.Empty;
			bool flag = false;
			if (ReplyType.Subscribe == (replyType & ReplyType.Subscribe))
			{
				text += "[关注时回复]";
				flag = true;
			}
			if (ReplyType.NoMatch == (replyType & ReplyType.NoMatch))
			{
				text += "[无匹配回复]";
				flag = true;
			}
			if (ReplyType.Keys == (replyType & ReplyType.Keys))
			{
				text += "[关键字回复]";
				flag = true;
			}
            if (ReplyType.Kefu == replyType)
            {
                text = "[客服回复]";
                flag = true;
            }
			if (!flag)
			{
				text = replyType.ToShowText();
			}
			return text;
		}
	}
}

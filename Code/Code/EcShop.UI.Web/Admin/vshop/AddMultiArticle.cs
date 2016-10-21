using EcShop.ControlPanel.Store;
using EcShop.Entities.VShop;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using kindeditor.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.vshop
{
	public class AddMultiArticle : AdminPage
	{
		protected KindeditorControl fkContent;
		protected System.Web.UI.WebControls.CheckBox chkKeys;
		protected System.Web.UI.WebControls.CheckBox chkSub;
		protected System.Web.UI.WebControls.CheckBox chkNo;
		protected System.Web.UI.WebControls.TextBox txtKeys;
		protected YesNoRadioButtonList radMatch;
		protected YesNoRadioButtonList radDisable;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.radMatch.Items[0].Text = "模糊匹配";
			this.radMatch.Items[1].Text = "精确匹配";
			this.radDisable.Items[0].Text = "启用";
			this.radDisable.Items[1].Text = "禁用";
			this.chkNo.Enabled = (ReplyHelper.GetMismatchReply() == null);
			this.chkSub.Enabled = (ReplyHelper.GetSubscribeReply() == null);
			if (!this.chkNo.Enabled)
			{
				this.chkNo.ToolTip = "该类型已被使用";
			}
			if (!this.chkSub.Enabled)
			{
				this.chkSub.ToolTip = "该类型已被使用";
			}
			if (string.IsNullOrEmpty(base.Request.QueryString["cmd"]))
			{
				return;
			}
			if (string.IsNullOrEmpty(base.Request.Form["MultiArticle"]))
			{
				return;
			}
			string value = base.Request.Form["MultiArticle"];
			System.Collections.Generic.List<ArticleList> list = JsonConvert.DeserializeObject(value, typeof(System.Collections.Generic.List<ArticleList>)) as System.Collections.Generic.List<ArticleList>;
			if (list != null && list.Count > 0)
			{
				NewsReplyInfo newsReplyInfo = new NewsReplyInfo();
				newsReplyInfo.MessageType = MessageType.List;
				newsReplyInfo.IsDisable = (base.Request.Form["radDisable"] != "true");
				if (base.Request.Form["chkKeys"] == "true")
				{
					newsReplyInfo.Keys = base.Request.Form.Get("Keys");
				}
				if (!string.IsNullOrWhiteSpace(newsReplyInfo.Keys) && ReplyHelper.HasReplyKey(newsReplyInfo.Keys))
				{
					base.Response.Write("key");
					base.Response.End();
				}
				newsReplyInfo.MatchType = ((base.Request.Form["radMatch"] == "true") ? MatchType.Like : MatchType.Equal);
				newsReplyInfo.ReplyType = ReplyType.None;
				if (base.Request.Form["chkKeys"] == "true")
				{
					newsReplyInfo.ReplyType |= ReplyType.Keys;
				}
				if (base.Request.Form["chkSub"] == "true")
				{
					newsReplyInfo.ReplyType |= ReplyType.Subscribe;
				}
				if (base.Request.Form["chkNo"] == "true")
				{
					newsReplyInfo.ReplyType |= ReplyType.NoMatch;
				}
				System.Collections.Generic.List<NewsMsgInfo> list2 = new System.Collections.Generic.List<NewsMsgInfo>();
				foreach (ArticleList current in list)
				{
					if (current.Status != "del")
					{
						NewsMsgInfo newsMsgInfo = current;
						if (newsMsgInfo != null)
						{
							newsMsgInfo.Reply = newsReplyInfo;
							list2.Add(newsMsgInfo);
						}
					}
				}
				newsReplyInfo.NewsMsg = list2;
				if (ReplyHelper.SaveReply(newsReplyInfo))
				{
					base.Response.Write("true");
					base.Response.End();
				}
			}
		}
		protected void btnCreate_Click(object sender, System.EventArgs e)
		{
		}
	}
}

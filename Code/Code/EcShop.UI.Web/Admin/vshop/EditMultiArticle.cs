using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
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
	[PrivilegeCheck(Privilege.MutiArticleEdit)]
	public class EditMultiArticle : AdminPage
	{
		protected int MaterialID;
		protected string articleJson;
		protected KindeditorControl fkContent;
		protected System.Web.UI.WebControls.CheckBox chkKeys;
		protected System.Web.UI.WebControls.CheckBox chkSub;
		protected System.Web.UI.WebControls.CheckBox chkNo;
		protected System.Web.UI.WebControls.TextBox txtKeys;
		protected YesNoRadioButtonList radMatch;
		protected YesNoRadioButtonList radDisable;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(base.Request.QueryString["id"], out this.MaterialID))
			{
				base.Response.Redirect("ReplyOnKey.aspx");
			}
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
			if (!string.IsNullOrEmpty(base.Request.QueryString["cmd"]))
			{
				if (!string.IsNullOrEmpty(base.Request.Form["MultiArticle"]))
				{
					string value = base.Request.Form["MultiArticle"];
					System.Collections.Generic.List<ArticleList> list = JsonConvert.DeserializeObject(value, typeof(System.Collections.Generic.List<ArticleList>)) as System.Collections.Generic.List<ArticleList>;
					if (list != null && list.Count > 0)
					{
						NewsReplyInfo newsReplyInfo = ReplyHelper.GetReply(this.MaterialID) as NewsReplyInfo;
						newsReplyInfo.IsDisable = (base.Request.Form["radDisable"] != "true");
						string text = base.Request.Form.Get("Keys");
						if (base.Request.Form["chkKeys"] == "true")
						{
							if (!string.IsNullOrEmpty(text) && newsReplyInfo.Keys != text && ReplyHelper.HasReplyKey(text))
							{
								base.Response.Write("key");
								base.Response.End();
							}
							newsReplyInfo.Keys = text;
						}
						else
						{
							newsReplyInfo.Keys = string.Empty;
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
						foreach (NewsMsgInfo current in newsReplyInfo.NewsMsg)
						{
							ReplyHelper.DeleteNewsMsg(current.Id);
						}
						System.Collections.Generic.List<NewsMsgInfo> list2 = new System.Collections.Generic.List<NewsMsgInfo>();
						foreach (ArticleList current2 in list)
						{
							if (current2.Status != "del")
							{
								NewsMsgInfo newsMsgInfo = current2;
								if (newsMsgInfo != null)
								{
									newsMsgInfo.Reply = newsReplyInfo;
									list2.Add(newsMsgInfo);
								}
							}
						}
						newsReplyInfo.NewsMsg = list2;
						if (ReplyHelper.UpdateReply(newsReplyInfo))
						{
							base.Response.Write("true");
							base.Response.End();
							return;
						}
					}
				}
			}
			else
			{
				NewsReplyInfo newsReplyInfo2 = ReplyHelper.GetReply(this.MaterialID) as NewsReplyInfo;
				if (newsReplyInfo2 != null)
				{
					System.Collections.Generic.List<ArticleList> list3 = new System.Collections.Generic.List<ArticleList>();
					if (newsReplyInfo2.NewsMsg != null && newsReplyInfo2.NewsMsg.Count > 0)
					{
						int num = 1;
						foreach (NewsMsgInfo current3 in newsReplyInfo2.NewsMsg)
						{
							list3.Add(new ArticleList
							{
								PicUrl = current3.PicUrl,
								Title = current3.Title,
								Url = current3.Url,
								Description = current3.Description,
								Content = current3.Content,
								BoxId = num++.ToString(),
								Status = ""
							});
						}
						this.articleJson = JsonConvert.SerializeObject(list3);
					}
					this.fkContent.Text = newsReplyInfo2.NewsMsg[0].Content;
					this.txtKeys.Text = newsReplyInfo2.Keys;
					this.radMatch.SelectedValue = (newsReplyInfo2.MatchType == MatchType.Like);
					this.radDisable.SelectedValue = !newsReplyInfo2.IsDisable;
					this.chkKeys.Checked = (ReplyType.Keys == (newsReplyInfo2.ReplyType & ReplyType.Keys));
					this.chkSub.Checked = (ReplyType.Subscribe == (newsReplyInfo2.ReplyType & ReplyType.Subscribe));
					this.chkNo.Checked = (ReplyType.NoMatch == (newsReplyInfo2.ReplyType & ReplyType.NoMatch));
					if (this.chkNo.Checked)
					{
						this.chkNo.Enabled = true;
						this.chkNo.ToolTip = "";
					}
					if (this.chkSub.Checked)
					{
						this.chkSub.Enabled = true;
						this.chkSub.ToolTip = "";
					}
				}
			}
		}
		protected void btnCreate_Click(object sender, System.EventArgs e)
		{
		}
	}
}

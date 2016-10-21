using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.vshop
{
	[PrivilegeCheck(Privilege.AddArticle)]
	public class AddSingleArticle : AdminPage
	{
		protected System.Web.UI.WebControls.Label LbimgTitle;
		protected System.Web.UI.WebControls.Label Lbmsgdesc;
		protected System.Web.UI.WebControls.TextBox Tbtitle;
		protected System.Web.UI.WebControls.TextBox Tbdescription;
		protected System.Web.UI.WebControls.TextBox TbUrl;
		protected KindeditorControl fkContent;
		protected System.Web.UI.WebControls.HiddenField hdpic;
		protected System.Web.UI.WebControls.CheckBox chkKeys;
		protected System.Web.UI.WebControls.CheckBox chkSub;
		protected System.Web.UI.WebControls.CheckBox chkNo;
		protected System.Web.UI.WebControls.TextBox txtKeys;
		protected YesNoRadioButtonList radMatch;
		protected YesNoRadioButtonList radDisable;
		protected System.Web.UI.WebControls.Button btnCreate;
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
			if (!string.IsNullOrEmpty(base.Request.QueryString["iscallback"]) && System.Convert.ToBoolean(base.Request.QueryString["iscallback"]))
			{
				this.UploadImage();
				return;
			}
			if (!string.IsNullOrEmpty(base.Request.Form["del"]))
			{
				string path = base.Request.Form["del"];
				string path2 = Globals.PhysicalPath(path);
				try
				{
					if (System.IO.File.Exists(path2))
					{
						System.IO.File.Delete(path2);
						base.Response.Write("true");
					}
				}
				catch (System.Exception)
				{
					base.Response.Write("false");
				}
				base.Response.End();
			}
		}
		protected void btnCreate_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Tbtitle.Text) || string.IsNullOrEmpty(this.Tbdescription.Text) || string.IsNullOrEmpty(this.hdpic.Value))
			{
				this.ShowMsg("您填写的信息不完整!", false);
				return;
			}
			if (!string.IsNullOrEmpty(this.txtKeys.Text) && ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
			{
				this.ShowMsg("关键字重复!", false);
				return;
			}
			NewsReplyInfo newsReplyInfo = new NewsReplyInfo();
			newsReplyInfo.IsDisable = !this.radDisable.SelectedValue;
			if (this.chkKeys.Checked && !string.IsNullOrWhiteSpace(this.txtKeys.Text))
			{
				newsReplyInfo.Keys = this.txtKeys.Text.Trim();
			}
			if (this.chkKeys.Checked && string.IsNullOrWhiteSpace(this.txtKeys.Text))
			{
				this.ShowMsg("你选择了关键字回复，必须填写关键字！", false);
				return;
			}
			newsReplyInfo.MatchType = (this.radMatch.SelectedValue ? MatchType.Like : MatchType.Equal);
			newsReplyInfo.MessageType = MessageType.News;
			if (this.chkKeys.Checked)
			{
				newsReplyInfo.ReplyType |= ReplyType.Keys;
			}
			if (this.chkSub.Checked)
			{
				newsReplyInfo.ReplyType |= ReplyType.Subscribe;
			}
			if (this.chkNo.Checked)
			{
				newsReplyInfo.ReplyType |= ReplyType.NoMatch;
			}
			if (newsReplyInfo.ReplyType == ReplyType.None)
			{
				this.ShowMsg("请选择回复类型", false);
				return;
			}
			if (string.IsNullOrEmpty(this.Tbtitle.Text))
			{
				this.ShowMsg("请输入标题", false);
				return;
			}
			if (string.IsNullOrEmpty(this.hdpic.Value))
			{
				this.ShowMsg("请上传封面图", false);
				return;
			}
			if (string.IsNullOrEmpty(this.Tbdescription.Text))
			{
				this.ShowMsg("请输入摘要", false);
				return;
			}
			if (string.IsNullOrEmpty(this.fkContent.Text) && string.IsNullOrEmpty(this.TbUrl.Text))
			{
				this.ShowMsg("请输入内容或自定义链接", false);
				return;
			}
			NewsMsgInfo item = new NewsMsgInfo
			{
				Reply = newsReplyInfo,
				Content = this.fkContent.Text,
				Description = System.Web.HttpUtility.HtmlEncode(this.Tbdescription.Text),
				PicUrl = this.hdpic.Value,
				Title = System.Web.HttpUtility.HtmlEncode(this.Tbtitle.Text),
				Url = this.TbUrl.Text.Trim()
			};
			newsReplyInfo.NewsMsg = new System.Collections.Generic.List<NewsMsgInfo>();
			newsReplyInfo.NewsMsg.Add(item);
			if (ReplyHelper.SaveReply(newsReplyInfo))
			{
				this.ShowMsg("添加成功", true);
				return;
			}
			this.ShowMsg("添加失败", false);
		}
		private void UploadImage()
		{
			try
			{
				System.Web.HttpPostedFile httpPostedFile = base.Request.Files["Filedata"];
				string str = System.DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
				string str2 = "/Storage/master/reply/";
				string str3 = str + System.IO.Path.GetExtension(httpPostedFile.FileName);
				httpPostedFile.SaveAs(Globals.MapPath(str2 + str3));
				base.Response.StatusCode = 200;
				base.Response.Write(str2 + str3);
			}
			catch (System.Exception)
			{
				base.Response.StatusCode = 500;
				base.Response.Write("服务器错误");
				base.Response.End();
			}
			finally
			{
				base.Response.End();
			}
		}
	}
}

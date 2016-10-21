using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using kindeditor.Net;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.vshop
{
	[PrivilegeCheck(Privilege.EditArticle)]
	public class EditSingleArticle : AdminPage
	{
		private int id;
		protected System.Web.UI.WebControls.Label LbimgTitle;
		protected System.Web.UI.HtmlControls.HtmlImage uploadpic;
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
			if (!base.IsPostBack)
			{
				this.id = base.GetUrlIntParam("id");
				this.BindSingleArticle(this.id);
			}
			else
			{
				this.uploadpic.Src = this.hdpic.Value;
			}
			if (base.GetUrlBoolParam("iscallback"))
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
			int urlIntParam = base.GetUrlIntParam("id");
			NewsReplyInfo newsReplyInfo = ReplyHelper.GetReply(urlIntParam) as NewsReplyInfo;
			if (newsReplyInfo == null || newsReplyInfo.NewsMsg == null || newsReplyInfo.NewsMsg.Count == 0)
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!string.IsNullOrEmpty(this.txtKeys.Text) && newsReplyInfo.Keys != this.txtKeys.Text.Trim() && ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
			{
				this.ShowMsg("关键字重复!", false);
				return;
			}
			newsReplyInfo.IsDisable = !this.radDisable.SelectedValue;
			if (this.chkKeys.Checked && !string.IsNullOrWhiteSpace(this.txtKeys.Text))
			{
				newsReplyInfo.Keys = this.txtKeys.Text.Trim();
			}
			else
			{
				newsReplyInfo.Keys = string.Empty;
			}
			newsReplyInfo.MatchType = (this.radMatch.SelectedValue ? MatchType.Like : MatchType.Equal);
			newsReplyInfo.ReplyType = ReplyType.None;
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
			newsReplyInfo.NewsMsg[0].Content = this.fkContent.Text;
			newsReplyInfo.NewsMsg[0].Description = this.Tbdescription.Text;
			newsReplyInfo.NewsMsg[0].PicUrl = this.hdpic.Value;
			newsReplyInfo.NewsMsg[0].Title = this.Tbtitle.Text;
			newsReplyInfo.NewsMsg[0].Url = this.TbUrl.Text;
			if (ReplyHelper.UpdateReply(newsReplyInfo))
			{
				this.ShowMsg("修改成功", true);
				return;
			}
			this.ShowMsg("修改失败", false);
		}
		protected void BindSingleArticle(int id)
		{
			NewsReplyInfo newsReplyInfo = ReplyHelper.GetReply(id) as NewsReplyInfo;
			if (newsReplyInfo == null || newsReplyInfo.NewsMsg == null || newsReplyInfo.NewsMsg.Count == 0)
			{
				base.GotoResourceNotFound();
				return;
			}
			this.ViewState["MsgId"] = newsReplyInfo.Id;
			this.txtKeys.Text = newsReplyInfo.Keys;
			this.radMatch.SelectedValue = (newsReplyInfo.MatchType == MatchType.Like);
			this.radDisable.SelectedValue = !newsReplyInfo.IsDisable;
			this.chkKeys.Checked = (ReplyType.Keys == (newsReplyInfo.ReplyType & ReplyType.Keys));
			this.chkSub.Checked = (ReplyType.Subscribe == (newsReplyInfo.ReplyType & ReplyType.Subscribe));
			this.chkNo.Checked = (ReplyType.NoMatch == (newsReplyInfo.ReplyType & ReplyType.NoMatch));
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
			this.Tbtitle.Text = newsReplyInfo.NewsMsg[0].Title;
			this.LbimgTitle.Text = newsReplyInfo.NewsMsg[0].Title;
			this.Tbdescription.Text = System.Web.HttpUtility.HtmlDecode(newsReplyInfo.NewsMsg[0].Description);
			this.fkContent.Text = newsReplyInfo.NewsMsg[0].Content;
			this.Lbmsgdesc.Text = newsReplyInfo.NewsMsg[0].Description;
			this.TbUrl.Text = newsReplyInfo.NewsMsg[0].Url;
			this.uploadpic.Src = newsReplyInfo.NewsMsg[0].PicUrl;
			this.hdpic.Value = newsReplyInfo.NewsMsg[0].PicUrl;
		}
		private void UploadImage()
		{
			System.Drawing.Image image = null;
			System.Drawing.Image image2 = null;
			System.Drawing.Bitmap bitmap = null;
			System.Drawing.Graphics graphics = null;
			System.IO.MemoryStream memoryStream = null;
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
				if (bitmap != null)
				{
					bitmap.Dispose();
				}
				if (graphics != null)
				{
					graphics.Dispose();
				}
				if (image2 != null)
				{
					image2.Dispose();
				}
				if (image != null)
				{
					image.Dispose();
				}
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
				base.Response.End();
			}
		}
	}
}

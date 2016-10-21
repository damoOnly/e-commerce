using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.vshop
{
	[PrivilegeCheck(Privilege.TopicList)]
	public class EditTopic : AdminPage
	{
		private int topicId;
		protected System.Web.UI.WebControls.TextBox txtTopicTitle;
		protected System.Web.UI.WebControls.TextBox txtKeys;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected HiImage imgPic;
		protected ImageLinkButton btnPicDelete;
		protected KindeditorControl fcContent;
		protected System.Web.UI.WebControls.Button btnUpdateTopic;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["topicId"], out this.topicId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!this.Page.IsPostBack)
			{
				TopicInfo topicInfo = VShopHelper.Gettopic(this.topicId);
				if (topicInfo == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				Globals.EntityCoding(topicInfo, false);
				this.txtTopicTitle.Text = topicInfo.Title;
				this.txtKeys.Text = topicInfo.Keys;
				this.imgPic.ImageUrl = topicInfo.IconUrl;
				this.fcContent.Text = topicInfo.Content;
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			}
		}
		protected void btnUpdateTopic_Click(object sender, System.EventArgs e)
		{
			TopicInfo topicInfo = VShopHelper.Gettopic(this.topicId);
			if (topicInfo.Keys != this.txtKeys.Text.Trim() && ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
			{
				this.ShowMsg("关键字重复!", false);
				return;
			}
			if (this.fileUpload.HasFile)
			{
				try
				{
					ResourcesHelper.DeleteImage(topicInfo.IconUrl);
					topicInfo.IconUrl = VShopHelper.UploadTopicImage(this.fileUpload.PostedFile);
					this.imgPic.ImageUrl = topicInfo.IconUrl;
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
			}
			topicInfo.TopicId = this.topicId;
			topicInfo.Title = this.txtTopicTitle.Text.Trim();
			topicInfo.Keys = this.txtKeys.Text.Trim();
			topicInfo.IconUrl = topicInfo.IconUrl;
			topicInfo.Content = this.fcContent.Text;
			topicInfo.AddedDate = System.DateTime.Now;
			topicInfo.IsRelease = true;
			ValidationResults validationResults = Validation.Validate<TopicInfo>(topicInfo, new string[]
			{
				"ValTopicInfo"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
			}
			else
			{
				if (VShopHelper.Updatetopic(topicInfo))
				{
					this.ShowMsg("已经成功修改当前专题", true);
					return;
				}
				this.ShowMsg("修改专题失败", false);
				return;
			}
		}
		protected void btnPicDelete_Click(object sender, System.EventArgs e)
		{
			TopicInfo topicInfo = VShopHelper.Gettopic(this.topicId);
			try
			{
				ResourcesHelper.DeleteImage(topicInfo.IconUrl);
			}
			catch
			{
			}
			topicInfo.IconUrl = (this.imgPic.ImageUrl = null);
			if (VShopHelper.Updatetopic(topicInfo))
			{
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
				this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			}
		}
	}
}

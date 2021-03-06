using EcShop.ControlPanel.Store;
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
	public class AddTopic : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtTopicTitle;
		protected System.Web.UI.WebControls.TextBox txtKeys;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected KindeditorControl fcContent;
		protected System.Web.UI.WebControls.Button btnAddTopic;
		protected void Page_Load(object sender, System.EventArgs e)
		{
		}
		protected void btnAddTopic_Click(object sender, System.EventArgs e)
		{
			if (ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
			{
				this.ShowMsg("关键字重复!", false);
				return;
			}
			string iconUrl = string.Empty;
			if (this.fileUpload.HasFile)
			{
				try
				{
					iconUrl = VShopHelper.UploadTopicImage(this.fileUpload.PostedFile);
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
			}
			TopicInfo topicInfo = new TopicInfo();
			topicInfo.Title = this.txtTopicTitle.Text.Trim();
			topicInfo.Keys = this.txtKeys.Text.Trim();
			topicInfo.IconUrl = iconUrl;
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
				int num;
				if (VShopHelper.Createtopic(topicInfo, out num) && num > 0)
				{
					base.Response.Redirect("SetTopicProducts.aspx?topicid=" + num);
					return;
				}
				this.ShowMsg("添加专题错误", false);
				return;
			}
		}
	}
}

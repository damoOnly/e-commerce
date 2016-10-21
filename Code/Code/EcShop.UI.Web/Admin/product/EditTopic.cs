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
namespace EcShop.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.TopicEdit)]
	public class EditTopic : AdminPage
	{
		private int topicId;
		protected System.Web.UI.WebControls.TextBox txtTopicTitle;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
        protected System.Web.UI.WebControls.FileUpload fileUploadMobileListImg;
        protected System.Web.UI.WebControls.FileUpload fileUploadMobileBannerImg;
        protected System.Web.UI.WebControls.FileUpload fileUploadPCListImg;
        protected System.Web.UI.WebControls.FileUpload fileUploadPCBannerImg;
		protected HiImage imgPic;
        protected HiImage imgMobileList;
        protected HiImage imgMobileBanner;
        protected HiImage imgPcList;
        protected HiImage imgPcBanner;
		protected ImageLinkButton btnPicDelete;
        protected ImageLinkButton btnDeleteMobileListImg;
        protected ImageLinkButton btnDeleteMobileBannerImg;
        protected ImageLinkButton btnDeletePcListImg;
        protected ImageLinkButton btnDeletePcBannerImg;
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
				this.imgPic.ImageUrl = topicInfo.IconUrl;
                this.imgMobileList.ImageUrl = topicInfo.MobileListImageUrl;
                this.imgMobileBanner.ImageUrl = topicInfo.MobileBannerImageUrl;
                this.imgPcList.ImageUrl = topicInfo.PCListImageUrl;
                this.imgPcBanner.ImageUrl = topicInfo.PCBannerImageUrl;
				this.fcContent.Text = topicInfo.Content;
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                this.btnDeleteMobileListImg.Visible = !string.IsNullOrEmpty(this.imgMobileList.ImageUrl);
                this.btnDeleteMobileBannerImg.Visible = !string.IsNullOrEmpty(this.imgMobileBanner.ImageUrl);
                this.btnDeletePcListImg.Visible = !string.IsNullOrEmpty(this.imgPcList.ImageUrl);
                this.btnDeletePcBannerImg.Visible = !string.IsNullOrEmpty(this.imgPcBanner.ImageUrl);
			}
		}
		protected void btnUpdateTopic_Click(object sender, System.EventArgs e)
		{
			TopicInfo topicInfo = VShopHelper.Gettopic(this.topicId);
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

            if (this.fileUploadMobileListImg.HasFile)
            {
                try
                {
                    ResourcesHelper.DeleteImage(topicInfo.MobileListImageUrl);
                    topicInfo.MobileListImageUrl = VShopHelper.UploadTopicImage(this.fileUploadMobileListImg.PostedFile);
                    this.imgMobileList.ImageUrl = topicInfo.MobileListImageUrl;
                }
                catch
                {
                    this.ShowMsg("移动端列表图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }

            if (this.fileUploadMobileBannerImg.HasFile)
            {
                try
                {
                    ResourcesHelper.DeleteImage(topicInfo.MobileBannerImageUrl);
                    topicInfo.MobileBannerImageUrl = VShopHelper.UploadTopicImage(this.fileUploadMobileBannerImg.PostedFile);
                    this.imgMobileList.ImageUrl = topicInfo.MobileBannerImageUrl;
                }
                catch
                {
                    this.ShowMsg("移动端Banner图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }

            if (this.fileUploadPCListImg.HasFile)
            {
                try
                {
                    ResourcesHelper.DeleteImage(topicInfo.PCListImageUrl);
                    topicInfo.PCListImageUrl = VShopHelper.UploadTopicImage(this.fileUploadPCListImg.PostedFile);
                    this.imgPcList.ImageUrl = topicInfo.PCListImageUrl;
                }
                catch
                {
                    this.ShowMsg("PC端列表图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }

            if (this.fileUploadPCBannerImg.HasFile)
            {
                try
                {
                    ResourcesHelper.DeleteImage(topicInfo.PCBannerImageUrl);
                    topicInfo.PCBannerImageUrl = VShopHelper.UploadTopicImage(this.fileUploadPCBannerImg.PostedFile);
                    this.imgPcBanner.ImageUrl = topicInfo.PCBannerImageUrl;
                }
                catch
                {
                    this.ShowMsg("PC端Banner图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }

			topicInfo.TopicId = this.topicId;
			topicInfo.Title = this.txtTopicTitle.Text.Trim();
			topicInfo.Keys = "";
			//topicInfo.IconUrl = topicInfo.IconUrl;
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
            ImageLinkButton button = sender as ImageLinkButton;

            string arg = string.Empty;

            if (button != null)
            {
                arg = button.CommandArgument;
            }

            switch (arg)
            {
                case "pic":
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
                    break;
                case "mobile_list":
                    try
                    {
                        ResourcesHelper.DeleteImage(topicInfo.MobileListImageUrl);
                    }
                    catch
                    {
                    }

                    topicInfo.MobileListImageUrl = (this.imgMobileList.ImageUrl = null);

                    if (VShopHelper.Updatetopic(topicInfo))
                    {
                        this.btnDeleteMobileListImg.Visible = !string.IsNullOrEmpty(this.imgMobileList.ImageUrl);
                        this.imgMobileList.Visible = !string.IsNullOrEmpty(this.imgMobileList.ImageUrl);
                    }
                    break;
                case "mobile_banner":
                    try
                    {
                        ResourcesHelper.DeleteImage(topicInfo.MobileBannerImageUrl);
                    }
                    catch
                    {
                    }

                    topicInfo.MobileBannerImageUrl = (this.imgMobileBanner.ImageUrl = null);

                    if (VShopHelper.Updatetopic(topicInfo))
                    {
                        this.btnDeleteMobileBannerImg.Visible = !string.IsNullOrEmpty(this.imgMobileBanner.ImageUrl);
                        this.imgMobileBanner.Visible = !string.IsNullOrEmpty(this.imgMobileBanner.ImageUrl);
                    }
                    break;
                case "pc_list":
                    try
                    {
                        ResourcesHelper.DeleteImage(topicInfo.PCListImageUrl);
                    }
                    catch
                    {
                    }

                    topicInfo.PCListImageUrl = (this.imgPcList.ImageUrl = null);

                    if (VShopHelper.Updatetopic(topicInfo))
                    {
                        this.btnDeletePcBannerImg.Visible = !string.IsNullOrEmpty(this.imgPcList.ImageUrl);
                        this.imgPcList.Visible = !string.IsNullOrEmpty(this.imgPcList.ImageUrl);
                    }
                    break;
                case "pc_banner":
                    try
                    {
                        ResourcesHelper.DeleteImage(topicInfo.PCBannerImageUrl);
                    }
                    catch
                    {
                    }

                    topicInfo.PCBannerImageUrl = (this.imgPcBanner.ImageUrl = null);

                    if (VShopHelper.Updatetopic(topicInfo))
                    {
                        this.btnDeletePcBannerImg.Visible = !string.IsNullOrEmpty(this.imgPcBanner.ImageUrl);
                        this.imgPcBanner.Visible = !string.IsNullOrEmpty(this.imgPcBanner.ImageUrl);
                    }
                    break;
                default:
                    break;
            }

		}
	}
}

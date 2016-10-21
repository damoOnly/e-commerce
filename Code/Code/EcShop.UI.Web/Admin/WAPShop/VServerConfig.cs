using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.WAPShop
{
	public class VServerConfig : AdminPage
	{
		protected System.Web.UI.WebControls.Literal txtUrl;
		protected System.Web.UI.WebControls.Literal txtToken;
		protected System.Web.UI.WebControls.TextBox txtAppId;
		protected System.Web.UI.WebControls.TextBox txtAppSecret;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chkIsValidationService;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected System.Web.UI.WebControls.Button btnUpoad;
		protected HiImage imgPic;
		protected ImageLinkButton btnPicDelete;
		protected System.Web.UI.WebControls.TextBox txtWeixinNumber;
		protected System.Web.UI.WebControls.TextBox txtWeixinLoginUrl;
		protected System.Web.UI.WebControls.Button btnAdd;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnUpoad.Click += new System.EventHandler(this.btnUpoad_Click);
			this.btnPicDelete.Click += new System.EventHandler(this.btnPicDelete_Click);
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				if (string.IsNullOrEmpty(masterSettings.WeixinToken))
				{
					masterSettings.WeixinToken = this.CreateKey(8);
					SettingsManager.Save(masterSettings);
				}
				this.txtAppId.Text = masterSettings.WeixinAppId;
				this.txtAppSecret.Text = masterSettings.WeixinAppSecret;
				this.txtToken.Text = masterSettings.WeixinToken;
				this.chkIsValidationService.Checked = masterSettings.IsValidationService;
				this.imgPic.ImageUrl = masterSettings.WeiXinCodeImageUrl;
				this.txtWeixinNumber.Text = masterSettings.WeixinNumber;
				this.txtWeixinLoginUrl.Text = masterSettings.WeixinLoginUrl;
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(masterSettings.WeiXinCodeImageUrl);
				this.txtUrl.Text = string.Format("http://{0}/api/wx.ashx", base.Request.Url.Host, this.txtToken.Text);
			}
		}
		private void btnPicDelete_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (!string.IsNullOrEmpty(masterSettings.WeiXinCodeImageUrl))
			{
				ResourcesHelper.DeleteImage(masterSettings.WeiXinCodeImageUrl);
				this.btnPicDelete.Visible = false;
				masterSettings.WeiXinCodeImageUrl = (this.imgPic.ImageUrl = string.Empty);
				SettingsManager.Save(masterSettings);
			}
		}
		private void btnUpoad_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (this.fileUpload.HasFile)
			{
				try
				{
					if (!string.IsNullOrEmpty(masterSettings.WeiXinCodeImageUrl))
					{
						ResourcesHelper.DeleteImage(masterSettings.WeiXinCodeImageUrl);
					}
					this.imgPic.ImageUrl = (masterSettings.WeiXinCodeImageUrl = VShopHelper.UploadWeiXinCodeImage(this.fileUpload.PostedFile));
					this.btnPicDelete.Visible = true;
					SettingsManager.Save(masterSettings);
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
				}
			}
		}
		private string CreateKey(int len)
		{
			byte[] array = new byte[len];
			new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(array);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(string.Format("{0:X2}", array[i]));
			}
			return stringBuilder.ToString();
		}
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.WeixinAppId = this.txtAppId.Text;
			masterSettings.WeixinAppSecret = this.txtAppSecret.Text;
			masterSettings.IsValidationService = this.chkIsValidationService.Checked;
			masterSettings.WeixinNumber = this.txtWeixinNumber.Text;
			masterSettings.WeixinLoginUrl = this.txtWeixinLoginUrl.Text;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("修改成功", true);
		}
	}
}

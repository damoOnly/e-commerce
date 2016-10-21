using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPLoginGuide : WAPTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlImage imgWeixin;
		private System.Web.UI.HtmlControls.HtmlInputHidden hidWeixinNumber;
		private System.Web.UI.HtmlControls.HtmlInputHidden hidWeixinLoginUrl;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vLoginGuide.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.imgWeixin = (System.Web.UI.HtmlControls.HtmlImage)this.FindControl("imgWeixin");
			this.hidWeixinNumber = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidWeixinNumber");
			this.hidWeixinLoginUrl = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidWeixinLoginUrl");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			this.hidWeixinNumber.Value = masterSettings.WeixinNumber;
			this.imgWeixin.Src = masterSettings.WeiXinCodeImageUrl;
			this.hidWeixinLoginUrl.Value = masterSettings.WeixinLoginUrl;
			PageTitle.AddSiteNameTitle("登录向导");
		}
	}
}

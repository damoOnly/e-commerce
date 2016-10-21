using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPMyFavorites : WAPMemberTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vMyFavorites.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			string url = this.Page.Request.QueryString["returnUrl"];
			if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["returnUrl"]))
			{
				this.Page.Response.Redirect(url);
			}
			PageTitle.AddSiteNameTitle("我的收藏");

            WAPHeadName.AddHeadName("我的收藏");
		}
	}
}

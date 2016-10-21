using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AliOHLogin : AliOHTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VLogin.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("登录");
		}
	}
}

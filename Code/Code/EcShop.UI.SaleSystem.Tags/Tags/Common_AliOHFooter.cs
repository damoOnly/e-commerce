using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_AliOHFooter : AliOHTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "tags/skin-Common_Footer.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
		}
	}
}

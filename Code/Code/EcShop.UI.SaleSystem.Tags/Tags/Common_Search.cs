using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_Search : AscxTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_Search/Skin-Common_Search.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
		}
	}
}

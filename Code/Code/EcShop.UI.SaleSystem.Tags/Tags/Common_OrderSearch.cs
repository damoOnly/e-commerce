using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_OrderSearch : AscxTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Skin-Common_OrderSearch.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
		}
	}
}

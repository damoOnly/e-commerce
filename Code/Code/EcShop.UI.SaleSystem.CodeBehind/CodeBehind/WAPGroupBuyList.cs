using EcShop.Entities.Commodities;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	public class WAPGroupBuyList : WAPTemplatedWebControl
	{
		private HiImage imgUrl;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VGroupBuyList.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			PageTitle.AddSiteNameTitle("团购搜索页");
		}
	}
}

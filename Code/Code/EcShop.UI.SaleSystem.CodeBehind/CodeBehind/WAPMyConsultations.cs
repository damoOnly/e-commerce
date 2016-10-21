using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class WAPMyConsultations : WAPMemberTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMyConsultations.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			
			PageTitle.AddSiteNameTitle("我的咨询");
            WAPHeadName.AddHeadName("我的咨询");
		}
	}
}

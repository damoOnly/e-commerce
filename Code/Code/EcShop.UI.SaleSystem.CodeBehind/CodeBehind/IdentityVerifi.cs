using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
    public class IdentityVerifi : VMemberTemplatedWebControl
	{
        private System.Web.UI.HtmlControls.HtmlInputHidden hidd_Sumit;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
                this.SkinName = "skin-IdentityVerifi.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			System.Web.UI.HtmlControls.HtmlInputText control2 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtRealName");
            System.Web.UI.HtmlControls.HtmlInputText control5 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtIdentityCard");
			Member member = HiContext.Current.User as Member;
			if (member != null)
			{
				control2.SetWhenIsNotNull(member.RealName);
                control5.SetWhenIsNotNull(member.IdentityCard);
			}
            
            PageTitle.AddSiteNameTitle("身份信息验证");

		}
	}
}

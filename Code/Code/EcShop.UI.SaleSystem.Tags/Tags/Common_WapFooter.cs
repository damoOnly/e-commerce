using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_WapFooter : WAPTemplatedWebControl
	{
        private System.Web.UI.HtmlControls.HtmlAnchor footlogin;
        private System.Web.UI.HtmlControls.HtmlAnchor footregist;

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
            this.footlogin = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("footlogin");
            this.footregist = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("footregist");
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                this.footlogin.InnerText = HiContext.Current.User.Username;
                this.footlogin.HRef = ResolveUrl("/wapshop/MemberCenter.aspx");

                this.footregist.InnerText = "ÍË³ö";
                this.footregist.HRef = ResolveUrl("/wapshop/Logout.aspx");
            }
            else
            {
                this.footlogin.InnerText = "×¢²á";
                this.footlogin.HRef = ResolveUrl("/wapshop/Login.aspx?action=register");
                this.footregist.InnerText = "µÇÂ¼";
                this.footregist.HRef = ResolveUrl("/wapshop/Login.aspx");

            }
		}
	}
}

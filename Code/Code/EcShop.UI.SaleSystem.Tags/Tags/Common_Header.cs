using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_Header : AscxTemplatedWebControl
	{
        private System.Web.UI.HtmlControls.HtmlInputHidden hiid_AdUserId;
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Skin-Common_Header.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
            Member member=(HiContext.Current.User) as  Member;
            this.hiid_AdUserId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hiid_AdUserId");
            if(this.hiid_AdUserId!=null)
            {
                if(member!=null)
                {
                    this.hiid_AdUserId.Value = member.UserId.ToString();
                }
            }
		}
	}
}

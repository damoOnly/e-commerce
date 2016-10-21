using EcShop.Membership.Context;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AliOHUserInfo : AliOHMemberTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VUserInfo.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			System.Web.UI.HtmlControls.HtmlInputText control = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtUserName");
			System.Web.UI.HtmlControls.HtmlInputText control2 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtRealName");
			System.Web.UI.HtmlControls.HtmlInputText control3 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtPhone");
			System.Web.UI.HtmlControls.HtmlInputText control4 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtEmail");
			Member member = HiContext.Current.User as Member;
			if (member != null)
			{
				control.SetWhenIsNotNull(member.Username);
				control2.SetWhenIsNotNull(member.RealName);
				control3.SetWhenIsNotNull(member.CellPhone);
				control4.SetWhenIsNotNull(member.QQ);
			}
			PageTitle.AddSiteNameTitle("修改用户信息");
		}
	}
}

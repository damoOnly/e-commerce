using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_User_Menu : AscxTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "ascx/tags/Common_UserCenter/Skin-Common_User_Menu.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)this.FindControl("messageNum");
			System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("liReferralRegisterAgreement");
			System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl2 = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("liReferralLink");
			System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl3 = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("liReferralSplittin");
			System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl4 = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("liSubReferral");
			System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl5 = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("liSubMember");
			int num;
			int num2;
			int num3;
			MemberProcessor.GetStatisticsNum(out num, out num2, out num3);
			literal.Text = num2.ToString();
			Member member = HiContext.Current.User as Member;
			if (member != null && member.ReferralStatus == 2)
			{
				htmlGenericControl.Visible = false;
				htmlGenericControl2.Visible = true;
				htmlGenericControl3.Visible = true;
				htmlGenericControl4.Visible = true;
				htmlGenericControl5.Visible = true;
				return;
			}
			htmlGenericControl.Visible = true;
			htmlGenericControl2.Visible = false;
			htmlGenericControl3.Visible = false;
			htmlGenericControl4.Visible = false;
			htmlGenericControl5.Visible = false;
		}
	}
}

using EcShop.ControlPanel.Commodities;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VUserInfo : VMemberTemplatedWebControl
	{
        private System.Web.UI.HtmlControls.HtmlInputHidden hidd_Sumit;
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
            System.Web.UI.HtmlControls.HtmlInputText control4 = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtIdentityCard");
			Member member = HiContext.Current.User as Member;
			if (member != null)
			{
                DataTable dt = SitesManagementHelper.GetMySubMemberByUserId(member.UserId);
                control.SetWhenIsNotNull(member.Username);
                if (dt != null && dt.Rows.Count > 0)
                {
                    control2.Value = dt.Rows[0]["RealName"].ToString();
                    control3.Value = dt.Rows[0]["CellPhone"].ToString();
                    control4.Value = dt.Rows[0]["IdentityCard"].ToString();
                }
                else
                {
                    control2.Value =member.RealName;
                    control3.Value =member.CellPhone;
                    control4.Value =member.IdentityCard;
                }
			}
            PageTitle.AddSiteNameTitle("修改个人信息");

		}
	}
}

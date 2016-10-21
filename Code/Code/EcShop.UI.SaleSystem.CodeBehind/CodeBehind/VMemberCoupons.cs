using EcShop.Membership.Context;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMemberCoupons : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Repeater rptCoupons;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vmemberCoupons.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			int useType = 1;
			int.TryParse(this.Page.Request.QueryString["usedType"], out useType);
			DataTable userCoupons = MemberProcessor.GetUserCoupons(HiContext.Current.User.UserId, useType);
			if (userCoupons != null)
			{
				this.rptCoupons = (System.Web.UI.WebControls.Repeater)this.FindControl("rptCoupons");
				this.rptCoupons.DataSource = userCoupons;
				this.rptCoupons.DataBind();
			}
			PageTitle.AddSiteNameTitle("优惠券");
            //查看之后，更新状态为已查阅
            MemberProcessor.UpdateCouponsReaded(HiContext.Current.User.UserId);
		}
	}
}

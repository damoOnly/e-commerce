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
    public class VMemberVoucher : VMemberTemplatedWebControl
	{
        private System.Web.UI.WebControls.Repeater rptVoucher;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
                this.SkinName = "skin-vmemberVoucher.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			int useType = 1;
			int.TryParse(this.Page.Request.QueryString["usedType"], out useType);
            DataTable rptVoucher = MemberProcessor.GetUserVoucher(HiContext.Current.User.UserId, useType);
            if (rptVoucher != null && rptVoucher.Rows.Count > 0)
			{
                this.rptVoucher = (System.Web.UI.WebControls.Repeater)this.FindControl("rptVoucher");
                this.rptVoucher.DataSource = rptVoucher;
                this.rptVoucher.DataBind();
			}
			PageTitle.AddSiteNameTitle("现金券");

            //查看之后，更新状态为已查阅
            MemberProcessor.UpdateVoucherReaded(HiContext.Current.User.UserId);
		}
	}
}

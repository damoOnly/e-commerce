using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class MyChangeCoupons : MemberTemplatedWebControl
	{
		private Common_Coupon_ChangeCouponList changeCoupons;
        protected Literal lblNoDataDisplay;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-MyChangeCoupons.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.changeCoupons = (Common_Coupon_ChangeCouponList)this.FindControl("Common_Coupon_ChangeCouponList");
            lblNoDataDisplay = (Literal)this.FindControl("lblNoDataDisplay");
			this.changeCoupons.ItemCommand += new Common_Coupon_ChangeCouponList.CommandEventHandler(this.changeCoupons_ItemCommand);
			if (!this.Page.IsPostBack)
			{
				this.BindCoupons();
			}
		}
		private void changeCoupons_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Change")
			{
				int couponId = System.Convert.ToInt32(e.CommandArgument);
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litNeedPoint");
				int num = int.Parse(literal.Text);
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (num > member.Points)
				{
					this.ShowMessage("当前积分不够兑换此优惠券", false);
					return;
				}
				if (TradeHelper.PointChageCoupon(couponId, num, member.Points))
				{
					this.ShowMessage("兑换成功，请查看您的优惠券", true);
					return;
				}
				this.ShowMessage("兑换失败", false);
			}
		}
		private void BindCoupons()
		{
            System.Data.DataTable dt = TradeHelper.GetChangeCoupons();
            if (dt == null || dt.Rows.Count <= 0)
            {
                lblNoDataDisplay.Text = "&nbsp;&nbsp;暂无可选的优惠券";
                changeCoupons.Visible = false;
                return;
            }
			this.changeCoupons.DataSource = dt;
			this.changeCoupons.DataBind();
		}
	}
}

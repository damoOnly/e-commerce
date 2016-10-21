using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_Coupon_CouponList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_Coupons_CouponsList";
		private System.Web.UI.WebControls.Repeater repeaterCoupon;
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.repeaterCoupon.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.repeaterCoupon.DataSource = value;
			}
		}
		public Common_Coupon_CouponList()
		{
			base.ID = "Common_Coupons_CouponsList";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Coupon_CouponList.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.repeaterCoupon = (System.Web.UI.WebControls.Repeater)this.FindControl("repeaterCoupon");
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.repeaterCoupon.DataSource != null)
			{
				this.repeaterCoupon.DataBind();
			}
		}
	}
}

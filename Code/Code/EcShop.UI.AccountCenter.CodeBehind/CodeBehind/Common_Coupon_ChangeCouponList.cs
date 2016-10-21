using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_Coupon_ChangeCouponList : AscxTemplatedWebControl
	{
		public delegate void CommandEventHandler(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e);
		public const string TagID = "Common_Coupon_ChangeCouponList";
		private System.Web.UI.WebControls.Repeater repeaterCoupon;
		public event Common_Coupon_ChangeCouponList.CommandEventHandler ItemCommand;
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
		public Common_Coupon_ChangeCouponList()
		{
			base.ID = "Common_Coupon_ChangeCouponList";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Coupon_ChangeCouponList.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.repeaterCoupon = (System.Web.UI.WebControls.Repeater)this.FindControl("repeaterCoupon");
			this.repeaterCoupon.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.repeaterCoupon_ItemCommand);
		}
		private void repeaterCoupon_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			this.ItemCommand(source, e);
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

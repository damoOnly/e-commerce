using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class MyCoupons : MemberTemplatedWebControl
	{
		private Common_Coupon_CouponList coupons;
		private System.Web.UI.WebControls.TextBox txtCoupon;
		private IButton btnAddCoupon;
		private SmallStatusMessage status;
		private System.Web.UI.HtmlControls.HtmlSelect selectCouponStatus;
		private System.Web.UI.WebControls.ImageButton imgbtnSearch;
		private int CouponStatus = -1;
		private string ClaimCode = "";
		private Pager pager;
		private CouponItemInfo couponItemInfo = new CouponItemInfo();
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-MyCoupons.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.coupons = (Common_Coupon_CouponList)this.FindControl("Common_Coupons_CouponsList");
			this.txtCoupon = (System.Web.UI.WebControls.TextBox)this.FindControl("txtCoupon");
			this.status = (SmallStatusMessage)this.FindControl("status");
			this.btnAddCoupon = ButtonManager.Create(this.FindControl("btnAddCoupon"));
			this.selectCouponStatus = (System.Web.UI.HtmlControls.HtmlSelect)this.FindControl("selectCouponStatus");
			this.imgbtnSearch = (System.Web.UI.WebControls.ImageButton)this.FindControl("imgbtnSearch");
			this.pager = (Pager)this.FindControl("pager");
			this.btnAddCoupon.Click += new System.EventHandler(this.btnAddCoupon_Click);
			this.imgbtnSearch.Click += new System.Web.UI.ImageClickEventHandler(this.imgbtnSearch_Click);
			new System.Web.UI.WebControls.HyperLink();
			if (!this.Page.IsPostBack)
			{
				this.BindCoupons();
			}
		}
		public UserCouponQuery GetQuery()
		{

            UserCouponQuery userCouponQuery = new UserCouponQuery();
            userCouponQuery.UserID = new int?(HiContext.Current.User.UserId);
            userCouponQuery.Status = new int?(this.CouponStatus);
            userCouponQuery.ClaimCode = this.ClaimCode;

            userCouponQuery.PageIndex = this.pager.PageIndex;
            userCouponQuery.PageSize = this.pager.PageSize;
            userCouponQuery.SortBy = "VoucherId";
            userCouponQuery.SortOrder = SortAction.Desc;
            return userCouponQuery;

		}
		private void btnAddCoupon_Click(object sender, System.EventArgs e)
		{
			string text = this.txtCoupon.Text;
			if (!TradeHelper.ExitCouponClaimCode(text))
			{
				this.ShowMessage("你输入的优惠券号码无效，请重试", false);
				return;
			}
			if (TradeHelper.AddClaimCodeToUser(text, HiContext.Current.User.UserId) > 0)
			{
				this.BindCoupons();
				this.txtCoupon.Text = string.Empty;
				this.ShowMessage("成功的添加了优惠券到你的账户", true);
			}
		}
		private void BindCoupons()
		{
			UserCouponQuery query = this.GetQuery();

            DbQueryResult queryCoupon = TradeHelper.GetUserCouponInfo(query);
            this.coupons.DataSource = queryCoupon.Data;
            this.coupons.DataBind();
            this.pager.TotalRecords = queryCoupon.TotalRecords;

		}
		private void imgbtnSearch_Click(object sender, System.EventArgs e)
		{
			new UserCouponQuery();
			if (!string.IsNullOrEmpty(this.txtCoupon.Text.Trim()))
			{
				this.ClaimCode = this.txtCoupon.Text.Trim();
			}
			if (this.selectCouponStatus.Value != "-1")
			{
				this.CouponStatus = System.Convert.ToInt32(this.selectCouponStatus.Value);
			}
			this.BindCoupons();
		}
	}
}

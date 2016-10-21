using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UserPoints : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litUserPoint;
		private System.Web.UI.WebControls.Literal litMyCoupons;
		private Common_Point_PointList pointList;
		private Pager pager;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserPoints.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.pointList = (Common_Point_PointList)this.FindControl("Common_Point_PointList");
			this.pager = (Pager)this.FindControl("pager");
			this.litUserPoint = (System.Web.UI.WebControls.Literal)this.FindControl("litUserPoint");
			this.litMyCoupons = (System.Web.UI.WebControls.Literal)this.FindControl("litMyCoupons");
			PageTitle.AddSiteNameTitle("我的积分");
			this.pointList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.pointList_ItemDataBound);
			if (!this.Page.IsPostBack)
			{
				this.BindPoint();
				CouponItemInfo couponItemInfo = new CouponItemInfo();
				couponItemInfo.CouponStatus = new int?(0);
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (member != null)
				{
					this.litUserPoint.Text = member.Points.ToString();
					this.litMyCoupons.Text = TradeHelper.GetUserCoupons(member.UserId).Rows.Count.ToString();
				}
			}
		}
		protected void pointList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.Control control = e.Item.Controls[0];
				System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)control.FindControl("lblPointType");
				if (label != null)
				{
					if (label.Text == "0")
					{
						label.Text = "兑换优惠券";
						return;
					}
					if (label.Text == "1")
					{
						label.Text = "兑换礼品";
						return;
					}
					if (label.Text == "2")
					{
						label.Text = "购物奖励";
						return;
					}
					if (label.Text == "3")
					{
						label.Text = "退款扣积分";
					}
				}
			}
		}
		private void BindPoint()
		{
			DbQueryResult userPoints = TradeHelper.GetUserPoints(this.pager.PageIndex);
			this.pointList.DataSource = userPoints.Data;
			this.pointList.DataBind();
			this.pager.TotalRecords = userPoints.TotalRecords;
		}
	}
}

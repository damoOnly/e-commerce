using ASPNET.WebControls;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Coupons)]
	public class NewCoupons : AdminPage
	{
		protected Grid grdCoupons;
		protected System.Web.UI.WebControls.HiddenField txtcouponid;
		protected System.Web.UI.WebControls.TextBox tbcouponNum;
		protected System.Web.UI.WebControls.Button btnExport;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdCoupons.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdCoupons_RowDeleting);
			this.grdCoupons.ReBindData += new Grid.ReBindDataEventHandler(this.grdCoupons_ReBindData);
			if (!this.Page.IsPostBack)
			{
				this.BindCoupons();
			}
		}
		private void grdCoupons_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int couponId = (int)this.grdCoupons.DataKeys[e.RowIndex].Value;
			if (CouponHelper.DeleteCoupon(couponId))
			{
				this.BindCoupons();
				this.ShowMsg("成功删除了选定张优惠券", true);
				return;
			}
			this.ShowMsg("删除优惠券失败", false);
		}
		protected void btnExport_Click(object sender, System.EventArgs e)
		{
			int num;
			if (!int.TryParse(this.tbcouponNum.Text, out num))
			{
				this.ShowMsg("导出数量必须为正数", false);
				return;
			}
			if (num <= 0)
			{
				this.ShowMsg("导出数量必须为正数", false);
				return;
			}
			int couponId;
			if (!int.TryParse(this.txtcouponid.Value, out couponId))
			{
				this.ShowMsg("参数错误", false);
				return;
			}
			CouponInfo coupon = CouponHelper.GetCoupon(couponId);
			string empty = string.Empty;
			CouponActionStatus couponActionStatus = CouponHelper.CreateCoupon(coupon, num, out empty);
			if (couponActionStatus == CouponActionStatus.UnknowError)
			{
				this.ShowMsg("未知错误", false);
				return;
			}
			if (couponActionStatus == CouponActionStatus.CreateClaimCodeError)
			{
				this.ShowMsg("生成优惠券号码错误", false);
				return;
			}
			if (couponActionStatus == CouponActionStatus.CreateClaimCodeSuccess && !string.IsNullOrEmpty(empty))
			{
				System.Collections.Generic.IList<CouponItemInfo> couponItemInfos = CouponHelper.GetCouponItemInfos(empty);
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
				stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
				stringBuilder.AppendLine("<td>优惠券批次号</td>");
				stringBuilder.AppendLine("<td>优惠券号码</td>");
				stringBuilder.AppendLine("<td>优惠券金额</td>");
				stringBuilder.AppendLine("<td>过期时间</td>");
				stringBuilder.AppendLine("</tr>");
				foreach (CouponItemInfo current in couponItemInfos)
				{
					stringBuilder.AppendLine("<tr>");
					stringBuilder.AppendLine("<td>" + empty + "</td>");
					stringBuilder.AppendLine("<td>" + current.ClaimCode + "</td>");
					stringBuilder.AppendLine("<td>" + coupon.DiscountValue + "</td>");
					stringBuilder.AppendLine("<td>" + coupon.ClosingTime + "</td>");
					stringBuilder.AppendLine("</tr>");
				}
				stringBuilder.AppendLine("</table>");
				this.Page.Response.Clear();
				this.Page.Response.Buffer = false;
				this.Page.Response.Charset = "GB2312";
				this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=CouponsInfo_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
				this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
				this.Page.Response.ContentType = "application/ms-excel";
				this.Page.EnableViewState = false;
				this.Page.Response.Write(stringBuilder.ToString());
				this.Page.Response.End();
			}
		}
		protected bool IsCouponEnd(object endtime)
		{
            if(endtime==null)
            {
                return false;
            }
            if (endtime.ToString() == "1753/1/1 12:00:00")
            {
                return true;
            }

			return System.Convert.ToDateTime(endtime).CompareTo(System.DateTime.Now) > 0;
		}
        protected bool checkStartTime(object SendType)
        {
            if (SendType.ToString()=="4")
            {
                return false;
            }
          
            return true;
        }
        /// <summary>
        /// 有明细不可删除
        /// </summary>
        /// <param name="haveitem"></param>
        /// <returns></returns>
        protected bool IsDelete(object haveitem)
        {
            if (string.IsNullOrWhiteSpace(haveitem.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected bool IsSendToMember(object endtime,object sendtype)
        {
            if(endtime==null ||string.IsNullOrEmpty(sendtype.ToString()))
            {
                return false;
            }
            if (sendtype.ToString() == "4")//退款类型不隐藏，一直显示
            {
                return true;
            }
            if(System.Convert.ToDateTime(endtime).CompareTo(System.DateTime.Now)> 0&&(String.IsNullOrEmpty(sendtype.ToString())||int.Parse(sendtype.ToString())==0))
            {
                return true;
            }

            else
            {
                return false;
            }
        }
		private void grdCoupons_ReBindData(object sender)
		{
			this.BindCoupons();
		}
		private void BindCoupons()
		{
			DbQueryResult newCoupons = CouponHelper.GetNewCoupons(new Pagination
			{
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex
			});
			this.grdCoupons.DataSource = newCoupons.Data;
			this.grdCoupons.DataBind();
			this.pager.TotalRecords = newCoupons.TotalRecords;
		}

        protected bool IsBySelf(object sendtype)
        {
            if (string.IsNullOrEmpty(sendtype.ToString()))
            {
                return false;
            }
            if(int.Parse(sendtype.ToString())==3)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
	}
}

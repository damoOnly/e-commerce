using ASPNET.WebControls;
using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Entities.Promotions;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using Microsoft.Web.Services3.Security.Configuration;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Coupons)]
	public class SendCouponToUsers : AdminPage
	{
		private int couponId;
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoName;
		protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdoRank;
		protected MemberGradeDropDownList rankList;
		protected System.Web.UI.WebControls.TextBox txtMemberNames;
		protected System.Web.UI.WebControls.Button btnSend;
        protected System.Web.UI.WebControls.TextBox txtRemark;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox ckbSendSms;
        protected int SendType;//发送类型
        protected System.Web.UI.HtmlControls.HtmlGenericControl Div_refund;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        protected System.Web.UI.WebControls.TextBox txtAmount;
        protected System.Web.UI.WebControls.TextBox txtDiscountValue;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["couponId"], out this.couponId))
			{
				base.GotoResourceNotFound();
				return;
			}
            if (!int.TryParse(this.Page.Request.QueryString["SendType"], out this.SendType))
            {
                base.GotoResourceNotFound();
                return;
            }
            if(this.SendType!=4)
            {
                this.Div_refund.Visible = false;
            }
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			if (!this.Page.IsPostBack)
			{
				this.rankList.DataBind();
			}
		}
		private bool IsMembers(string name)
		{
			string pattern = "[\\u4e00-\\u9fa5a-zA-Z]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*";
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
			return regex.IsMatch(name) && name.Length >= 2 && name.Length <= 20;
		}
		private void btnSend_Click(object sender, System.EventArgs e)
        {
            #region 屏蔽代码
            /*
			CouponItemInfo item = new CouponItemInfo();
			IList<CouponItemInfo> list = new List<CouponItemInfo>();
			IList<Member> list2 = new List<Member>();

			if (this.rdoName.Checked)
			{
				if (!string.IsNullOrEmpty(this.txtMemberNames.Text.Trim()))
				{
					System.Collections.Generic.IList<string> list3 = new System.Collections.Generic.List<string>();
					string text = this.txtMemberNames.Text.Trim().Replace("\r\n", "\n");
					string[] array = text.Replace("\n", "*").Split(new char[]
					{
						'*'
					});
					for (int i = 0; i < array.Length; i++)
					{
						list3.Add(array[i]);
					}
					list2 = PromoteHelper.GetMemdersByNames(list3);
				}
				string claimCode = string.Empty;
				foreach (Member current in list2)
				{
					claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                    item = new CouponItemInfo(this.couponId, claimCode, new int?(current.UserId), current.Username, current.Email, System.DateTime.Now, txtRemark.Text.Trim(),HiContext.Current.User.UserId);
					list.Add(item);
				}
				if (list.Count <= 0)
				{
					this.ShowMsg("你输入的会员名中没有一个正确的，请输入正确的会员名", false);
					return;
				}
				CouponHelper.SendClaimCodes(list);
				this.txtMemberNames.Text = string.Empty;
				this.ShowMsg(string.Format("此次发送操作已成功，优惠券发送数量：{0}", list.Count), true);
			}
			if (this.rdoRank.Checked)
			{
				list2 = PromoteHelper.GetMembersByRank(this.rankList.SelectedValue);
				string claimCode2 = string.Empty;
				foreach (Member current2 in list2)
				{
					claimCode2 = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
					item = new CouponItemInfo(this.couponId, claimCode2, new int?(current2.UserId), current2.Username, current2.Email, System.DateTime.Now,txtRemark.Text.Trim());
					list.Add(item);
				}
				if (list.Count <= 0)
				{
					this.ShowMsg("您选择的会员等级下面没有会员", false);
					return;
				}
				CouponHelper.SendClaimCodes(list);
				this.txtMemberNames.Text = string.Empty;
				this.ShowMsg(string.Format("此次发送操作已成功，优惠券发送数量：{0}", list.Count), true);
			}
             * */
            #endregion

            string memberNames = "";
            int? gradeId = new Nullable<int>();
            bool isSendSms = ckbSendSms.Checked;

            if (this.rdoName.Checked)
            {
                if (!string.IsNullOrEmpty(this.txtMemberNames.Text.Trim()))
                {
                    memberNames = this.txtMemberNames.Text.Trim().Replace("\r\n", "*").Replace("\n", "*");
                }

                if (string.IsNullOrWhiteSpace(memberNames))
                {
                    this.ShowMsg("你没有输入的会员名或会员名不正确，请输入正确的会员名", false);
                    return;
                }
            }
            decimal amount = 0;
            decimal discount = 0;
            if(this.SendType==4)
            {
                if (!this.calendarStartDate.SelectedDate.HasValue )
                {
                    this.ShowMsg("请选择开始日期！", false);
                    return;
                }
                if (!this.calendarEndDate.SelectedDate.HasValue)
                {
                    this.ShowMsg("请选择结束日期！", false);
                    return;
                }
                if (this.calendarStartDate.SelectedDate.Value.Date.CompareTo(this.calendarEndDate.SelectedDate.Value.Date) > 0)
                {
                    this.ShowMsg("开始日期不能晚于结束日期！", false);
                    return;
                }
                if (!string.IsNullOrEmpty(this.txtAmount.Text.Trim()))
                {
                    decimal value;
                    if (decimal.TryParse(this.txtAmount.Text.Trim(), out value))
                    {
                        amount = Convert.ToDecimal(value);
                    }
                    else
                    {
                        this.ShowMsg(string.Format("满足金额必须为0-1000万之间：{0}", amount), true);
                        return;
                    }
                }
                if (!decimal.TryParse(this.txtDiscountValue.Text.Trim(), out discount))
                {
                    this.ShowMsg(string.Format("可抵扣金额必须在0.01-1000万之间：{0}", discount), true);
                    return;
                }
            }

            if (this.rdoRank.Checked)
            {
                gradeId = this.rankList.SelectedValue;
            }

            int count = 0;
            if (SendType == 4)
            {
                CouponHelper.SendClaimCodes(this.couponId, memberNames, gradeId, txtRemark.Text.Trim(), isSendSms, HiContext.Current.User.UserId, HiContext.Current.User.Username, (!this.calendarEndDate.SelectedDate.HasValue ? "1753/1/1 12:00:00" : calendarStartDate.SelectedDate.Value.ToString()), (!this.calendarEndDate.SelectedDate.HasValue ? "1753/1/1 12:00:00" : calendarEndDate.SelectedDate.Value.ToString()),
                amount, discount, out count);
            }
            else {
                CouponInfo coupon = CouponHelper.GetCoupon(this.couponId);
                if (coupon.Amount.HasValue)
                {
                    amount = (decimal)coupon.Amount;
                }
                if (coupon.DiscountValue > 0)
                {
                    discount = coupon.DiscountValue;
                }
                if (coupon != null)
                {
                    CouponHelper.SendClaimCodes(this.couponId, memberNames, gradeId, txtRemark.Text.Trim(), isSendSms, HiContext.Current.User.UserId, HiContext.Current.User.Username, coupon.StartTime.ToString(), coupon.ClosingTime.ToString(),amount, discount, out count);
                }
            }

            this.txtMemberNames.Text = string.Empty;

            if (count >= 0)
            {
                this.ShowMsg(string.Format("此次发送操作已成功，优惠券发送数量：{0}", count), true);
            }
            else
            {
                this.ShowMsg(string.Format("此次发送操作已失败，原因可能为：{0}", count == -1 ? "达到发送总量" : "其他错误"), true);
            }
		}
	}
}

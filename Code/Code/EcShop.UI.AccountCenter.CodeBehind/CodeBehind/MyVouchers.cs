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
    public class MyVouchers : MemberTemplatedWebControl
	{
        private Common_Voucher_VoucherList voucher;
        private System.Web.UI.WebControls.TextBox txtVoucher;
        private IButton btnAddVoucher;
		private SmallStatusMessage status;
        private System.Web.UI.HtmlControls.HtmlSelect selectVoucherStatus;
		private System.Web.UI.WebControls.ImageButton imgbtnSearch;
        private int VoucherStatus = -1;
		private string ClaimCode = "";
		private Pager pager;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
                this.SkinName = "User/Skin-MyVouchers.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
            this.voucher = (Common_Voucher_VoucherList)this.FindControl("Common_Voucher_VoucherList");
            this.txtVoucher = (System.Web.UI.WebControls.TextBox)this.FindControl("txtVoucher");
			this.status = (SmallStatusMessage)this.FindControl("status");
            this.btnAddVoucher = ButtonManager.Create(this.FindControl("btnAddVoucher"));
            this.selectVoucherStatus = (System.Web.UI.HtmlControls.HtmlSelect)this.FindControl("selectVoucherStatus");
			this.imgbtnSearch = (System.Web.UI.WebControls.ImageButton)this.FindControl("imgbtnSearch");
			this.pager = (Pager)this.FindControl("pager");
            this.btnAddVoucher.Click += new System.EventHandler(this.btnAddVoucher_Click);
			this.imgbtnSearch.Click += new System.Web.UI.ImageClickEventHandler(this.imgbtnSearch_Click);
			new System.Web.UI.WebControls.HyperLink();
			if (!this.Page.IsPostBack)
			{
                this.BindVoucher();
			}
		}
        public UserVoucherQuery GetQuery()
		{
            UserVoucherQuery userVourcherQuery = new UserVoucherQuery();
            userVourcherQuery.UserID = new int?(HiContext.Current.User.UserId);
            userVourcherQuery.Status = new int?(this.VoucherStatus);
            userVourcherQuery.ClaimCode = this.ClaimCode;

            userVourcherQuery.PageIndex = this.pager.PageIndex;
            userVourcherQuery.PageSize = this.pager.PageSize;
            userVourcherQuery.SortBy = "VoucherId";
            userVourcherQuery.SortOrder = SortAction.Desc;
            return userVourcherQuery;

		}
        private void btnAddVoucher_Click(object sender, System.EventArgs e)
		{
            string text = this.txtVoucher.Text;
			if (!TradeHelper.ExitVoucherClaimCode(text))
			{
                this.ShowMessage("你输入的现金券号码无效，请重试", false);
				return;
			}
            if (TradeHelper.AddVoucherClaimCodeToUser(text, HiContext.Current.User.UserId) > 0)
			{
                this.BindVoucher();
                this.txtVoucher.Text = string.Empty;
				this.ShowMessage("成功的添加了现金券到你的账户", true);
			}
		}
        private void BindVoucher()
		{
            UserVoucherQuery query = this.GetQuery();
            DbQueryResult voucherQuery = TradeHelper.GetUserVoucher(query);

            this.voucher.DataSource = voucherQuery.Data;
            this.voucher.DataBind();

            this.pager.TotalRecords = voucherQuery.TotalRecords;
		}
		private void imgbtnSearch_Click(object sender, System.EventArgs e)
		{
            new UserVoucherQuery();
            if (!string.IsNullOrEmpty(this.txtVoucher.Text.Trim()))
			{
                this.ClaimCode = this.txtVoucher.Text.Trim();
			}
            if (this.selectVoucherStatus.Value != "-1")
			{
                this.VoucherStatus = System.Convert.ToInt32(this.selectVoucherStatus.Value);
			}
            this.BindVoucher();
		}
	}
}

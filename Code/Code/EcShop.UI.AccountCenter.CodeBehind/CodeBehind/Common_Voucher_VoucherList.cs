using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_Voucher_VoucherList: AscxTemplatedWebControl
	{
        public const string TagID = "Common_Voucher_VoucherList";
        private System.Web.UI.WebControls.Repeater repeaterVoucher;
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
                return this.repeaterVoucher.DataSource;
			}
			set
			{
				this.EnsureChildControls();
                this.repeaterVoucher.DataSource = value;
			}
		}
        public Common_Voucher_VoucherList()
		{
            base.ID = "Common_Voucher_VoucherList";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Voucher_VoucherList.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
            this.repeaterVoucher = (System.Web.UI.WebControls.Repeater)this.FindControl("repeaterVoucher");
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
            if (this.repeaterVoucher.DataSource != null)
			{
                this.repeaterVoucher.DataBind();
			}
		}
	}
}

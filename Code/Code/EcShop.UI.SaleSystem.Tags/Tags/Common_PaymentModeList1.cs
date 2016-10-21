using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Data;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_PaymentModeList1 : ThemedTemplatedRepeater  //海美生活新支付方式列表
	{
        public const string TagID = "list_Common_PaymentModeList1";
		private int maxNum = 6;
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
		public override object DataSource
		{
			get
			{
				return base.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				base.DataSource = value;
			}
		}
		public int MaxNum
		{
			get
			{
				return this.maxNum;
			}
			set
			{
				this.maxNum = value;
			}
		}
        public Common_PaymentModeList1()
		{
            base.ID = "list_Common_PaymentModeList1";
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
            base.DataSource = DataSource;
			base.DataBind();
		}
	}
}


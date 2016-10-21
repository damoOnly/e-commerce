using EcShop.Core.Enums;
using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_OrderManage_RefundApply : AscxTemplatedWebControl
	{
		public const string TagID = "Common_OrderManage_RefundApply";
		private System.Web.UI.WebControls.Repeater listRefunds;
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
				return this.listRefunds.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.listRefunds.DataSource = value;
			}
		}
		public SortAction SortOrder
		{
			get
			{
				return SortAction.Desc;
			}
		}
		public Common_OrderManage_RefundApply()
		{
			base.ID = "Common_OrderManage_RefundApply";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_RefundApply.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.listRefunds = (System.Web.UI.WebControls.Repeater)this.FindControl("listRefundOrder");
			this.listRefunds.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.listRefundOrder_ItemDataBound);
		}
		private void listRefundOrder_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblHandleStatus");
				if (label.Text == "0")
				{
					label.Text = "待处理";
					return;
				}
				if (label.Text == "1")
				{
					label.Text = "已处理";
					return;
				}
				label.Text = "已拒绝";
			}
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			this.listRefunds.DataSource = this.DataSource;
			this.listRefunds.DataBind();
		}
	}
}

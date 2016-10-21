using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_OrderManage_OrderItems : AscxTemplatedWebControl
	{
		public const string TagID = "Common_OrderManage_OrderItems";
		private System.Web.UI.WebControls.Repeater dataListOrderItems;
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
				return this.dataListOrderItems.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dataListOrderItems.DataSource = value;
			}
		}
		public Common_OrderManage_OrderItems()
		{
			base.ID = "Common_OrderManage_OrderItems";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_OrderItems.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.dataListOrderItems = (System.Web.UI.WebControls.Repeater)this.FindControl("dataListOrderItems");
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.dataListOrderItems.DataSource != null)
			{
				this.dataListOrderItems.DataBind();
			}
		}
	}
}

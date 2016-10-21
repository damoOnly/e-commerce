using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_BatchBuy_ProductList : AscxTemplatedWebControl
	{
		public delegate void ItemEventHandler(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e);
		public const string TagID = "Common_BatchBuy_ProductList";
		private System.Web.UI.WebControls.Repeater rptProducts;
		public event Common_BatchBuy_ProductList.ItemEventHandler ItemDataBound;
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
				return this.rptProducts.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.rptProducts.DataSource = value;
			}
		}
		public Common_BatchBuy_ProductList()
		{
			base.ID = "Common_BatchBuy_ProductList";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_BatchBuy_ProductList.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.rptProducts = (System.Web.UI.WebControls.Repeater)this.FindControl("rptProducts");
			this.rptProducts.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptProducts_ItemDataBound);
		}
		private void rptProducts_ItemDataBound(object source, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			this.ItemDataBound(source, e);
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.rptProducts.DataSource != null)
			{
				this.rptProducts.DataBind();
			}
		}
	}
}

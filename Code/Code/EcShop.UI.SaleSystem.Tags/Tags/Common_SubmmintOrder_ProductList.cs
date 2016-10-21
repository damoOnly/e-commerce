using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_SubmmintOrder_ProductList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_SubmmintOrder_ProductList";
		private Repeater dataListShoppingCrat;
		private Panel pnlShopProductCart;
        public delegate void DataBindEventHandler(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e);
        public event Common_SubmmintOrder_ProductList.DataBindEventHandler ItemDataBound;
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
				return this.dataListShoppingCrat.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dataListShoppingCrat.DataSource = value;
			}
		}
		public Common_SubmmintOrder_ProductList()
		{
			base.ID = "Common_SubmmintOrder_ProductList";
		}
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_SubmmintOrder_ProductList.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.dataListShoppingCrat = (Repeater)this.FindControl("dataListShoppingCrat");
			this.pnlShopProductCart = (Panel)this.FindControl("pnlShopProductCart");
			this.pnlShopProductCart.Visible = false;
            this.dataListShoppingCrat.ItemDataBound += dataListShoppingCrat_ItemDataBound;
		}

        void dataListShoppingCrat_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            this.ItemDataBound(sender, e);
        }
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.dataListShoppingCrat.DataSource != null)
			{
				this.dataListShoppingCrat.DataBind();
			}
		}
		public void ShowProductCart()
		{
			if (this.DataSource == null)
			{
				this.pnlShopProductCart.Visible = false;
				return;
			}
			this.pnlShopProductCart.Visible = true;
		}
	}
}

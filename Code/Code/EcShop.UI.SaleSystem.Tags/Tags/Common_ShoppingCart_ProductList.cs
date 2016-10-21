using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_ShoppingCart_ProductList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_ShoppingCart_ProductList";
		private DataList dataListShoppingCrat;
		private Panel pnlShopProductCart;
		public event DataListCommandEventHandler ItemCommand;
        public delegate void DataBindEventHandler(object sender, System.Web.UI.WebControls.DataListItemEventArgs e);
        public event Common_ShoppingCart_ProductList.DataBindEventHandler ItemDataBound;
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
		public Common_ShoppingCart_ProductList()
		{
			base.ID = "Common_ShoppingCart_ProductList";
		}
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_ShoppingCart/Skin-Common_ShoppingCart_ProductList.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.dataListShoppingCrat = (DataList)this.FindControl("dataListShoppingCrat");
			this.pnlShopProductCart = (Panel)this.FindControl("pnlShopProductCart");
			this.pnlShopProductCart.Visible = false;
            this.dataListShoppingCrat.ItemDataBound += dataListShoppingCrat_ItemDataBound;
			this.dataListShoppingCrat.ItemCommand += new DataListCommandEventHandler(this.dataListShoppingCrat_ItemCommand);
		}

        void dataListShoppingCrat_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            this.ItemDataBound(sender, e);
        }
		private void dataListShoppingCrat_ItemCommand(object source, DataListCommandEventArgs e)
		{
			this.ItemCommand(source, e);
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

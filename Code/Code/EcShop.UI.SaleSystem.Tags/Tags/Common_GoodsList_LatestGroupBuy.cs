using EcShop.SaleSystem.Catalog;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class Common_GoodsList_LatestGroupBuy : AscxTemplatedWebControl
	{
		private Repeater regroupbuy;
		private int maxNum = 1;
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
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_GoodsList/Skin-Common_GoodsList_LatestGroupBuy.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.regroupbuy = (Repeater)this.FindControl("regroupbuy");
			this.regroupbuy.DataSource = ProductBrowser.GetGroupByProductList(this.maxNum);
			this.regroupbuy.DataBind();
		}
	}
}

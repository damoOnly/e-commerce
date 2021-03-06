using EcShop.Core;
using EcShop.Entities.Orders;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	public class Order_ItemsList : System.Web.UI.UserControl
	{
		private OrderInfo order;
		protected System.Web.UI.WebControls.DataList dlstOrderItems;
		protected System.Web.UI.WebControls.Literal litWeight;
		protected System.Web.UI.WebControls.Literal lblAmoutPrice;
		protected System.Web.UI.WebControls.HyperLink hlkReducedPromotion;
		protected FormatedMoneyLabel lblTotalPrice;
		protected System.Web.UI.WebControls.Literal lblBundlingPrice;
		protected System.Web.UI.WebControls.Label lblOrderGifts;
		protected System.Web.UI.WebControls.DataList grdOrderGift;
		public OrderInfo Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}
		protected override void OnLoad(System.EventArgs e)
		{
			this.dlstOrderItems.DataSource = this.order.LineItems.Values;
			this.dlstOrderItems.DataBind();
			if (this.order.Gifts.Count == 0)
			{
				this.grdOrderGift.Visible = false;
				this.lblOrderGifts.Visible = false;
			}
			else
			{
				this.grdOrderGift.DataSource = this.order.Gifts;
				this.grdOrderGift.DataBind();
			}
			this.litWeight.Text = this.order.Weight.ToString("F2");
			if (this.order.IsReduced)
			{
				this.lblAmoutPrice.Text = string.Format("商品金额：{0}", Globals.FormatMoney(this.order.GetAmount()));
				this.hlkReducedPromotion.Text = this.order.ReducedPromotionName + string.Format(" 优惠：{0}", Globals.FormatMoney(this.order.ReducedPromotionAmount));
				this.hlkReducedPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					this.order.ReducedPromotionId
				});
			}
			if (this.order.BundlingID > 0)
			{
				this.lblBundlingPrice.Text = string.Format("<span style=\"color:Red;\">捆绑价格：{0}</span>", Globals.FormatMoney(this.order.BundlingPrice));
			}
			this.lblTotalPrice.Money = this.order.GetAmount() - this.order.ReducedPromotionAmount;
		}
	}
}

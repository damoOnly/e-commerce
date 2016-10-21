using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class AppMemberOrders : AppshopTemplatedWebControl
	{
		private AppshopTemplatedRepeater rptOrders;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMemberOrders.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("会员订单");
			if (HiContext.Current.User == null)
			{
				return;
			}
			int num = 0;
			int.TryParse(System.Web.HttpContext.Current.Request.QueryString.Get("status"), out num);
			OrderQuery orderQuery = new OrderQuery();
			if (num == 1)
			{
				orderQuery.Status = OrderStatus.WaitBuyerPay;
			}
			else
			{
				if (num == 3)
				{
					orderQuery.Status = OrderStatus.SellerAlreadySent;
				}
			}
			orderQuery.ShowGiftOrder = false;
			this.rptOrders = (AppshopTemplatedRepeater)this.FindControl("rptOrders");
			this.rptOrders.DataSource = MemberProcessor.GetUserOrder(HiContext.Current.User.UserId, orderQuery);
			this.rptOrders.DataBind();
		}
	}
}

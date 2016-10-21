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
	public class WAPMemberOrders : WAPMemberTemplatedWebControl
	{
		private WapTemplatedRepeater rptOrders;

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

			PageTitle.AddSiteNameTitle("我的订单");

            WAPHeadName.AddHeadName("我的订单");
			int num = 0;
			int.TryParse(System.Web.HttpContext.Current.Request.QueryString.Get("status"), out num);
			OrderQuery orderQuery = new OrderQuery();
			orderQuery.ShowGiftOrder = false;
			if (num == 1)
			{
				orderQuery.Status = OrderStatus.WaitBuyerPay;
			}

            if(num==2)
            {
                orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
            }
			
		   if (num == 3)
		    {
			   orderQuery.Status = OrderStatus.SellerAlreadySent;
				
			}

            if(num==5)
            {
                orderQuery.Status = OrderStatus.Finished;
            }
			orderQuery.ShowGiftOrder = false;
			this.rptOrders = (WapTemplatedRepeater)this.FindControl("rptOrders");
			this.rptOrders.DataSource = MemberProcessor.GetWAPUserOrder(HiContext.Current.User.UserId, orderQuery);
			this.rptOrders.DataBind();
		}
	}
}

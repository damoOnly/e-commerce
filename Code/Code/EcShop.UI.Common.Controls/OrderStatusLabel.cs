using EcShop.Entities.Orders;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
	public class OrderStatusLabel : Label
	{
		public object OrderStatusCode
		{
			get
			{
				return this.ViewState["OrderStatusCode"];
			}
			set
			{
				this.ViewState["OrderStatusCode"] = value;
			}
		}

        public object SourceOrderIdCode
        {
            get
            {
                return this.ViewState["SourceOrderIdCode"];
            }
            set
            {
                this.ViewState["SourceOrderIdCode"] = value;
            }
        }

        public object PayDate
        {
            get
            {
                return this.ViewState["PayDate"];
            }
            set
            {
                this.ViewState["PayDate"] = value;
            }
        }

        public object IsRefund
        {
            get
            {
                return this.ViewState["IsRefund"];
            }
            set
            {
                this.ViewState["IsRefund"] = value;
            }
            
        }

		protected override void Render(HtmlTextWriter writer)
		{
            base.Text = OrderInfo.GetOrderStatusName((OrderStatus)this.OrderStatusCode, SourceOrderIdCode == null ? "" : SourceOrderIdCode.ToString(), this.PayDate);
            if (IsRefund != null && (int)IsRefund > 0)
            {
                int curisrefund = (int)IsRefund;
                if (curisrefund == 1 && (OrderStatus)this.OrderStatusCode == OrderStatus.Refunded)
                {
                    base.Text = "已取消(已退款)";
                }
                else if (curisrefund == 2 && (OrderStatus)this.OrderStatusCode == OrderStatus.ApplyForRefund)
                {
                    base.Text = "已取消(退款中)";
                }
            }
            else
            {
                if ((OrderStatus)this.OrderStatusCode == OrderStatus.ApplyForRefund || (OrderStatus)this.OrderStatusCode == OrderStatus.Refunded)
                {
                    base.Text = "已取消";
                }
            }
            base.Render(writer);
		}
	}
}

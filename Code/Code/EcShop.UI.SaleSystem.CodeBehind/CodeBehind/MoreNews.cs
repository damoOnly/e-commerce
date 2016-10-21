using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
    public class MoreNews : HtmlTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
                this.SkinName = "Skin-MoreNews.html";
			}
			base.OnInit(e);
		}
		protected override void OnLoad(System.EventArgs e)
		{
			this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>alert_h(\"活动还未开始或者已经结束！\",function(){window.location.href=\"/vshop/default.aspx\";})</script>");
			if (!string.IsNullOrEmpty(this.Page.Request.Params["OrderId"]))
			{
				this.SearchOrder();
			}
			base.OnLoad(e);
		}
		protected override void AttachChildControls()
		{
			if (HiContext.Current.User.UserRole == UserRole.Member && ((Member)HiContext.Current.User).ReferralStatus == 2 && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
			{
				string text = System.Web.HttpContext.Current.Request.Url.ToString();
				if (text.IndexOf("?") > -1)
				{
					text = text + "&ReferralUserId=" + HiContext.Current.User.UserId;
				}
				else
				{
					text = text + "?ReferralUserId=" + HiContext.Current.User.UserId;
				}
				this.Page.Response.Redirect(text);
				return;
			}
			HiContext current = HiContext.Current;
			PageTitle.AddTitle(current.SiteSettings.SiteName + " - " + current.SiteSettings.SiteDescription, HiContext.Current.Context);
		}
		private void SearchOrder()
		{
			string text = "[{";
			string orderId = this.Page.Request["OrderId"];
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
			if (orderInfo != null)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\"OrderId\":\"",
					orderInfo.OrderId,
					"\",\"ShippingStatus\":\"",
					OrderInfo.GetOrderStatusName(orderInfo.OrderStatus,orderInfo.SourceOrderId),
					"\",\"ShipOrderNumber\":\"",
					orderInfo.ShipOrderNumber,
					"\",\"ShipModeName\":\"",
					orderInfo.RealModeName,
					"\""
				});
			}
			text += "}]";
			this.Page.Response.ContentType = "text/plain";
			this.Page.Response.Write(text);
			this.Page.Response.End();
		}
	}
}

using EcShop.Entities.Orders;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMyLogistics : VMemberTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vMyLogistics.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			string text = this.Page.Request.QueryString["orderId"];
			if (string.IsNullOrEmpty(text))
			{
				base.GotoResourceNotFound("");
			}
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(text);
			System.Web.UI.WebControls.Literal control = this.FindControl("litCompanyName") as System.Web.UI.WebControls.Literal;
			System.Web.UI.WebControls.Literal control2 = this.FindControl("litLogisticsNumber") as System.Web.UI.WebControls.Literal;
			control.SetWhenIsNotNull(orderInfo.ExpressCompanyName);
			control2.SetWhenIsNotNull(orderInfo.ShipOrderNumber);
			PageTitle.AddSiteNameTitle("我的物流");
		}
	}
}

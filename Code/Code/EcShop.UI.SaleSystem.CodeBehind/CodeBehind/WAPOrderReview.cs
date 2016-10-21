using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Comments;
using EcShop.Entities.Commodities;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
	public class WAPOrderReview : WAPMemberTemplatedWebControl
    {
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-WOrderReview.html";
            }
            base.OnInit(e);
        }

        protected override void AttachChildControls()
        {
            WapTemplatedRepeater rptOrderProducts = this.FindControl("rptOrderProducts") as WapTemplatedRepeater;
            string text = this.Page.Request.QueryString["orderId"];
            if (string.IsNullOrEmpty(text))
            {
                base.GotoResourceNotFound("");
            }
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(text);

            rptOrderProducts.DataSource = orderInfo.LineItems.Values;
            rptOrderProducts.DataBind();

            PageTitle.AddSiteNameTitle("发表评价");

            WAPHeadName.AddHeadName("发表评价");
        }
    }
}

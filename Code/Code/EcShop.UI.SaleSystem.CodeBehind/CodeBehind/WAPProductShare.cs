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
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class WAPProductShare : WAPTemplatedWebControl
    {
        private int productId;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-WProductShare.html";
            }
            base.OnInit(e);
        }

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["ProductId"], out this.productId))
            {
                base.GotoResourceNotFound("");
            }
            WapTemplatedRepeater rptOrderProducts = this.FindControl("rptOrderProducts") as WapTemplatedRepeater;
           
            ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(this.productId, null, null);
            IList<ProductInfo> productInfolist = new List<ProductInfo>();
            productInfolist.Add(productBrowseInfo.Product);

            rptOrderProducts.DataSource = productInfolist;
            rptOrderProducts.DataBind();
        }
    }
}

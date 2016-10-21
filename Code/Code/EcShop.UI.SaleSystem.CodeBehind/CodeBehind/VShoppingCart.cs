using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.AccountCenter.CodeBehind;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Linq;
using System.Collections.Generic;
using System.Linq;

namespace EcShop.UI.SaleSystem.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class VShoppingCart : VMemberTemplatedWebControl
    {
        //private VshopTemplatedRepeater rptCartSuppliers;
        //private VshopTemplatedRepeater rptCartProducts;

        private System.Web.UI.WebControls.Literal litProductTotalPrice;
        private System.Web.UI.WebControls.Literal litTotalTax;
   

        private EcShop.UI.SaleSystem.Tags.Common_CartSupplierProducts listOrders;

        private System.Web.UI.WebControls.Literal litTotal;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VShoppingCart.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            this.litProductTotalPrice = (System.Web.UI.WebControls.Literal)this.FindControl("litProductTotalPrice");
            this.litTotalTax = (System.Web.UI.WebControls.Literal)this.FindControl("litTotalTax");
           

            this.listOrders = (EcShop.UI.SaleSystem.Tags.Common_CartSupplierProducts)this.FindControl("common_cartsupplierproducts");

            this.listOrders.ItemDataBound += new EcShop.UI.SaleSystem.Tags.Common_CartSupplierProducts.DataBindEventHandler(this.listOrders_ItemDataBound);

            this.litTotal = (System.Web.UI.WebControls.Literal)this.FindControl("litTotal");

            #region 处理cookie中的购物车和收藏信息
            Member curMember = HiContext.Current.User as Member;
            if (curMember != null && !curMember.IsAnonymous)
            {
                ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
                if (cookieShoppingCart != null)
                {
                    ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
                    ShoppingCartProcessor.ClearCookieShoppingCart();
                }

                System.Web.HttpCookie cookieFavorite = HiContext.Current.Context.Request.Cookies["Hid_Ecshop_Favorite_Data_New"];
                if (cookieFavorite != null && !string.IsNullOrEmpty(cookieFavorite.Value))
                {
                    string[] favoriteProductIds = cookieFavorite.Value.Split('|');
                    int productId = 0;
                    foreach (string fav in favoriteProductIds)
                    {
                        try
                        {
                            productId = int.Parse(fav);
                            int favoriteId;
                            ProductBrowser.AddProductToFavorite(productId, curMember.UserId, out favoriteId);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    cookieFavorite.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(cookieFavorite);
                }
            }
            #endregion

            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            if (shoppingCart != null)
            {
                //foreach (ShoppingCartItemInfo item in shoppingCart.LineItems)
                //{
                //    item.TaxRate = Math.Round(item.TaxRate * 100, 0);
                //}

                IList<ShoppingCartItemInfo> list = shoppingCart.LineItems;
                IQueryable<ShoppingCartItemInfo> queryableList = new EnumerableQuery<ShoppingCartItemInfo>(list);

                var query = from q in queryableList
                            group q by new { id = q.SupplierId }
                                into q1
                                select new
                                {
                                    SupplierId = q1.FirstOrDefault().SupplierId,
                                    //Amount = q1.Sum(x => x.SupplierId)
                                    SupplierName = q1.FirstOrDefault().SupplierName,
                                    SupplierLogo = q1.FirstOrDefault().Logo
                                };

                this.listOrders.DataSource = query;
                this.listOrders.DataBind();

                this.litTotal.Text = shoppingCart.GetNewAmount().ToString("F2");

                decimal totaltax = shoppingCart.CalTotalTax();// shoppingCart.CalTotalTax();
                //this.litTotalTax.Text = (totaltax < 50 ? "0.00" : totaltax.ToString("F2"));
                if (totaltax <= 50)
                {
                    this.litTotalTax.Text = string.Format("<span style='text-decoration: line-through;'>{0}</span>", totaltax.ToString("F2"));
                }
                else
                {
                    this.litTotalTax.Text = string.Format("{0}", totaltax.ToString("F2"));
                }
                this.litProductTotalPrice.Text = shoppingCart.GetNewAmount().ToString("F2"); //shoppingCart.GetTotal().ToString("F2");
            }



            HttpCookie httpCookieShoppingCart = new HttpCookie("cn");
            string quantity = "0";
            if (shoppingCart != null) quantity = shoppingCart.GetQuantity().ToString();
            httpCookieShoppingCart.Value = quantity;
            httpCookieShoppingCart.Expires = System.DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(httpCookieShoppingCart);


            HttpCookie cookieSkuIds = this.Page.Request.Cookies["UserSession-SkuIds"];
            if (cookieSkuIds != null)
            {
                cookieSkuIds.Expires = DateTime.Now.AddDays(-1);
                cookieSkuIds.Values.Clear();
                HttpContext.Current.Response.AppendCookie(cookieSkuIds);
            }
            PageTitle.AddSiteNameTitle("购物车");
        }


        protected void listOrders_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {

                System.Web.UI.HtmlControls.HtmlInputHidden hidsupplierId = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Item.FindControl("hidsupplierId");

                System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rpProduct");
                if (repeater != null)
                {
                    repeater.ItemDataBound += repeater_ItemDataBound;
                    ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();

                    if (shoppingCart != null)
                    {

                        List<ShoppingCartItemInfo> list = (List<ShoppingCartItemInfo>)shoppingCart.LineItems;

                        var llist = list.Where(p => p.SupplierId == Convert.ToInt32(hidsupplierId.Value)).Select(c=>c);
                        repeater.DataSource = llist;
                        repeater.DataBind();
                    }

                }

            }
        }

        void repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                System.Web.UI.HtmlControls.HtmlInputHidden promotionProductId = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Item.FindControl("promotionProductId");
                System.Web.UI.WebControls.Repeater dtPresendPro = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("dtPresendPro");
                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                if (shoppingCart != null )
                {
                    // 赠送活动
                    List<ShoppingCartPresentInfo> presentList = (List<ShoppingCartPresentInfo>)shoppingCart.LinePresentPro;
                    if (presentList != null && presentList.Count > 0)
                    {
                        var p = presentList.Where(m => m.PromotionProductId == (string.IsNullOrWhiteSpace(promotionProductId.Value) ? 0 : Convert.ToInt32(promotionProductId.Value))).Select(n => n);
                        dtPresendPro.DataSource = p;
                        dtPresendPro.DataBind();
                    }

                }

            }
        }
    }
}

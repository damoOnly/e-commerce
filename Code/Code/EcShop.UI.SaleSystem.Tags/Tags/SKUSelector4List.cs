using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Entities.Sales;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Shopping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Newtonsoft.Json;
using System.Collections;

namespace EcShop.UI.SaleSystem.Tags
{
    public class SKUSelector4List : SKUSelector
    {
        public SKUSelector4List()
        {
            base.ID = "SKUSelector4List";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(this.ProductId, null, null);
            //writer.Write("<input type=\"hidden\" name=\"productStock\" value=\"{0}\" />", productBrowseInfo.Product.Stock);
            //writer.Write("<input type=\"hidden\" name=\"productSkuId\" value=\"{0}\" />", productBrowseInfo.Product.SkuId);
            //writer.Write("<input type=\"hidden\" name=\"productQuantity\" value=\"{0}\" />", GetQuantity_Product(this.ProductId));
            var value = from item in productBrowseInfo.Product.Skus
                        select new { SkuId = item.Value.SkuId, Count = item.Value.Stock };
            writer.Write("<script type=\"text/javascript\">var productSkus_{0} = {1};var productQuantityInShoppingCart_{0} = {2};</script>"
                , this.ProductId, JsonConvert.SerializeObject(value)
                , JsonConvert.SerializeObject(GetProductQuantityInShoppingCart(this.ProductId, productBrowseInfo.Product.Skus)));
            this.DataSource = productBrowseInfo.DbSKUs;
            base.Render(writer);
        }

        private object GetProductQuantityInShoppingCart(int productId, Dictionary<string, SKUItem> dict)
        {
            var defaultValue = from item in dict
                               select new { SkuId = item.Value.SkuId, Count = 0 };
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            if (shoppingCart == null || shoppingCart.LineItems.Count <= 0)
            {
                return defaultValue;
            }
            IEnumerable<ShoppingCartItemInfo> items = shoppingCart.LineItems.Where(p => p.ProductId == productId);
            if (items == null || items.Count() <= 0)
            {
                return defaultValue;
            }
            return items.Select(p => new { SkuId = p.SkuId, Count = p.Quantity });
        }
    }
}

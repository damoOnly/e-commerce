using EcShop.Entities.Commodities;
using EcShop.Entities.Sales;
using EcShop.SaleSystem.Shopping;
using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using EcShop.Membership.Context;
using EcShop.ControlPanel.Commodities;

namespace EcShop.UI.SaleSystem.CodeBehind
{
    public class ShoppingHandler : System.Web.IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(System.Web.HttpContext context)
        {
            try
            {
                string action = context.Request["action"];
                if (string.IsNullOrEmpty(action))
                    return;
                switch (action)
                {
                    case "AddToCartBySkus":
                        this.ProcessAddToCartBySkus(context);
                        break;
                    case "GetSkuByOptions":
                        this.ProcessGetSkuByOptions(context);
                        break;
                    case "UnUpsellingSku":
                        this.ProcessUnUpsellingSku(context);
                        break;
                    case "ClearBrowsed":
                        this.ClearBrowsedProduct(context);
                        break;
                    case "CheckBuyCardinality":
                        this.CheckBuyCardinality(context);
                        break;
                   
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.Write("{\"Status\":\"" + ex.Message.Replace("\"", "'") + "\"}");
            }
        }
       
        private void ClearBrowsedProduct(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            BrowsedProductQueue.ClearQueue();
            context.Response.Write("{\"Status\":\"Succes\"}");
        }

        private void ProcessAddToCartBySkus(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int quantity = int.Parse(context.Request["quantity"], System.Globalization.NumberStyles.None);
            string skuId = context.Request["productSkuId"];


           
            //判断库存
            int oldQuan = ShoppingCartProcessor.GetSkuStock(skuId);
            if (quantity > oldQuan)
            {
                context.Response.Write("{\"Status\":\"1\",\"oldQuan\":\"" + oldQuan + "\"}");
                return;
            }
            //检查商品是否超过限购数量
            Member member = HiContext.Current.User as Member;
            if (member != null)
            {
                int MaxCount = 0;
                int Payquantity = ProductHelper.CheckPurchaseCount(skuId, member.UserId, out MaxCount);
                if ((Payquantity + quantity) > MaxCount&&MaxCount!=0) //当前购买数量大于限购剩余购买数量
                {
                    context.Response.Write("{\"Status\":\"4\"}");
                    return;
                }
            }
            if (ShoppingCartProcessor.AddLineItem(skuId, quantity, 0) != AddCartItemStatus.Successed)
            {
                context.Response.Write("{\"Status\":\"2\"}");
                return;
            }

            DataTable dt = ProductHelper.GetAdOrderInfo(skuId);
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            if (shoppingCart != null)
            {
                
                context.Response.Write(string.Concat(new object[]
				{
					"{\"Status\":\"OK\",\"TotalMoney\":\"",shoppingCart.GetTotal().ToString(".00"),
					"\",\"Quantity\":\"",shoppingCart.GetQuantity().ToString(),
                    "\",\"SkuQuantity\":\"",shoppingCart.GetQuantity_Sku(skuId),
                     "\",\"data\":",Newtonsoft.Json.JsonConvert.SerializeObject(dt),"}"
				}));
                
                return;
            }
            context.Response.Write("{\"Status\":\"3\"}");
        }

        private void ProcessGetSkuByOptions(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int productId = int.Parse(context.Request["productId"], System.Globalization.NumberStyles.None);
            string text = context.Request["options"];
            if (string.IsNullOrEmpty(text))
            {
                context.Response.Write("{\"Status\":\"0\"}");
                return;
            }
            if (text.EndsWith(","))
            {
                text = text.Substring(0, text.Length - 1);
            }
            SKUItem productAndSku = ShoppingProcessor.GetProductAndSku(productId, text);
            if (productAndSku == null)
            {
                context.Response.Write("{\"Status\":\"1\"}");
                return;
            }
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"Status\":\"OK\",");
            stringBuilder.AppendFormat("\"SkuId\":\"{0}\",", productAndSku.SkuId);
            stringBuilder.AppendFormat("\"SKU\":\"{0}\",", productAndSku.SKU);
            stringBuilder.AppendFormat("\"Weight\":\"{0}\",", productAndSku.Weight.ToString("F2"));
            stringBuilder.AppendFormat("\"Stock\":\"{0}\",", productAndSku.Stock);
            stringBuilder.AppendFormat("\"SalePrice\":\"{0}\"", productAndSku.SalePrice.ToString("F2"));
            stringBuilder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(stringBuilder.ToString());
        }

        private void ProcessUnUpsellingSku(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int productId = int.Parse(context.Request["productId"], System.Globalization.NumberStyles.None);
            int attributeId = int.Parse(context.Request["AttributeId"], System.Globalization.NumberStyles.None);
            int valueId = int.Parse(context.Request["ValueId"], System.Globalization.NumberStyles.None);
            DataTable unUpUnUpsellingSkus = ShoppingProcessor.GetUnUpUnUpsellingSkus(productId, attributeId, valueId);

            if (unUpUnUpsellingSkus == null || unUpUnUpsellingSkus.Rows.Count == 0)
            {
                context.Response.Write("{\"Status\":\"1\"}");
                return;
            }

            int stock = ShoppingProcessor.GetSkusStock(productId, attributeId, valueId);

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append("\"Status\":\"OK\",");
            stringBuilder.AppendFormat("\"Stock\":\"{0}\",", stock);
            stringBuilder.Append("\"SkuItems\":[");
            foreach (DataRow dataRow in unUpUnUpsellingSkus.Rows)
            {
                stringBuilder.Append("{");
                stringBuilder.AppendFormat("\"AttributeId\":\"{0}\",", dataRow["AttributeId"].ToString());
                stringBuilder.AppendFormat("\"ValueId\":\"{0}\"", dataRow["ValueId"].ToString());
                stringBuilder.Append("},");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append("]");
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
        }

        private void CheckBuyCardinality(System.Web.HttpContext context)
        {
            var unselectedProductId = context.Request["productIds"];
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            if (shoppingCart.LineItems.Count == 0)
            {
                return;
            }
            List<object> result = new List<object>();
            if (!string.IsNullOrEmpty(unselectedProductId))
            {
               
                string[] pid_skuids = unselectedProductId.Split(',');
                foreach (var s in pid_skuids)
                {
                    int productId = int.Parse(s.Split('|')[0]);
                    string skuId = s.Split('|')[1];
                    var item = shoppingCart.LineItems.FirstOrDefault(p => p.ProductId == productId && p.SkuId == skuId);
                    if (item != null)
                    {
                        shoppingCart.LineItems.Remove(item);
                    }
                }
            }
            var checkProduct = context.Request["checkProducts"];
            if (!string.IsNullOrEmpty(checkProduct))
            {
                //检查商品是否超过限购数量
                Member member = HiContext.Current.User as Member;
                string[] pid_skuids = checkProduct.Split(',');
                 foreach (var s in pid_skuids)
                 {
                     int productId = int.Parse(s.Split('|')[0]);
                     string skuId = s.Split('|')[1];
                     var item = shoppingCart.LineItems.FirstOrDefault(p => p.ProductId == productId && p.SkuId == skuId);
                     if (member != null)
                     {
                         int MaxCount = 0;
                         int count = ProductHelper.CheckPurchaseCount(skuId, member.UserId, out MaxCount);
                         if ((count + item.Quantity) > MaxCount && MaxCount!=0) //当前购买数量大于限购剩余购买数量
                         {
                             result.Add(new { ProductId = item.ProductId, BuyCardinality = ((MaxCount - count) < 0 ? 0 : MaxCount - count), ProductName = item.Name, Quantity = item.Quantity, Purchase = "1" });
                         }
                        
                     }
                 }
            }
            if (result.Count>0)
            {
                context.Response.ContentType = "application/json";
                context.Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            var productIds = shoppingCart.LineItems.Select(p => p.ProductId).Distinct();
            if (productIds.Count() <= 0)
            {
                return;
            }

            Dictionary<int, int> dict = new EcShop.SqlDal.Commodities.ProductDao().GetBuyCardinality(productIds.ToArray());
            if (dict == null || dict.Count == 0)
            {
                return;
            }

          
            foreach (KeyValuePair<int, int> item in dict)
            {
                string productName = shoppingCart.LineItems.First(p => p.ProductId == item.Key).Name;
                int quantity = shoppingCart.LineItems.Where(p => p.ProductId == item.Key).Sum(p => p.Quantity);
                if (quantity >= item.Value)
                {
                    continue;
                }

                result.Add(new { ProductId = item.Key, BuyCardinality = item.Value, ProductName = productName, Quantity = quantity, Purchase="0"});
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(result));
        }
    }
}

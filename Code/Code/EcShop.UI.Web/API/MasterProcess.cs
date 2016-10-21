using EcShop.Core;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Shopping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EcShop.UI.Web.API
{
    public class MasterProcess : System.Web.IHttpHandler
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
            string text = context.Request["action"];
            string key;
            switch (key = text)
            {
                case "SelectItem":
                    SelectItem(context);//重算购物车金额
                    break;
                case "GetThisCateGory":
                    GetThisCateGory(context);//获取指定类目
                    break;
            }
        }
        private void SelectItem(System.Web.HttpContext context)
        {
            string skuId = context.Request["skuId"];
            string skuIds = context.Request["pids"];
            int num = 1;
            int.TryParse(context.Request["quantity"], out num);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            
            int skuStock = ShoppingCartProcessor.GetSkuStock(skuId);
            
            if (num > skuStock)
            {
                stringBuilder.AppendFormat("\"Status\":\"{0}\"", skuStock);
                num = skuStock;
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                context.Response.End();
            }

            ShoppingCartProcessor.UpdateLineItemQuantity(skuId, (num > 0) ? num : 1);

            if (skuIds == null)
            {
                stringBuilder.Append("\"Success\":0");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                context.Response.End();
            }

            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetPartShoppingCartInfo(skuIds);
            if (shoppingCart == null || shoppingCart.LineItems.Count == 0)
            {
                stringBuilder.Append("\"Success\":1,");
                stringBuilder.Append("\"AmoutPrice\":\"0.00\",");
                stringBuilder.Append("\"ReducedPromotion\":\"0.00\",");
                stringBuilder.Append("\"ReducedPromotionName\":\"\",");
                stringBuilder.Append("\"Tax\":\"0.00\",");
                stringBuilder.Append("\"ActivityReduct\":\"0.00\",");
                stringBuilder.Append("\"TotalPrice\":\"0.00\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                context.Response.End();
                return;
            }

            HttpCookie cookie = new HttpCookie("UserSession-SkuIds");
            cookie.Value = Globals.UrlEncode(skuIds);
            cookie.Expires = DateTime.Now.AddDays(1);
            context.Response.AppendCookie(cookie);

            stringBuilder.Append("\"Success\":1,");
            stringBuilder.AppendFormat("\"AmoutPrice\":\"{0}\",", shoppingCart.GetNewAmount().ToString("F2"));
            stringBuilder.AppendFormat("\"ReducedPromotion\":\"{0}\",", shoppingCart.ReducedPromotionAmount.ToString("F2"));
            stringBuilder.AppendFormat("\"ReducedPromotionName\":\"{0}\",", shoppingCart.ReducedPromotionName);
            decimal tax = shoppingCart.CalTotalTax();
            stringBuilder.AppendFormat("\"Tax\":\"{0}\",",tax.ToString("F2"));
            decimal activityReduct = shoppingCart.GetActivityPrice();
            stringBuilder.AppendFormat("\"ActivityReduct\":\"{0}\",", activityReduct == 0 ? "0.00" : activityReduct.ToString("F2"));
            stringBuilder.AppendFormat("\"NavigateUrl\":\"{0}\",",Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					shoppingCart.ReducedPromotionId
				}).ClearForJson());
            stringBuilder.AppendFormat("\"TotalPrice\":\"{0}\"",shoppingCart.GetNewTotalIncludeTax().ToString("F2"));

            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
        }
        private void GetThisCateGory(System.Web.HttpContext context)
        {
            IList<CategoryPartInfo> list = new List<CategoryPartInfo>();
            int cid1 = 0;
            int cid2 = 0;
            int cid3 = 0;
            int cid4 = 0;
            int cid5 = 0;
            int cid6 = 0;
            int cid7 = 0;
            var c1 = context.Request["Cid1"];
            var c2 = context.Request["Cid2"];
            var c3 = context.Request["Cid3"];
            var c4 = context.Request["Cid4"];
            var c5 = context.Request["Cid5"];
            var c6 = context.Request["Cid6"];
            var c7 = context.Request["Cid7"];
            int.TryParse(c1, out cid1);
            int.TryParse(c2, out cid2);
            int.TryParse(c3, out cid3);
            int.TryParse(c4, out cid4);
            int.TryParse(c5, out cid5);
            int.TryParse(c6, out cid6);
            int.TryParse(c7, out cid7);
            try
            {
                var catagoryDT = CategoryBrowser.GetCategoriesByThisId(cid1,cid2,cid3,cid4,cid5,cid6,cid7);
     

                if (catagoryDT != null)
                {
                    for (int i = 0; i < catagoryDT.Rows.Count; i++)
                    {
                        list.Add(DataMapper.ConvertDataRowToPartCategory(catagoryDT.Rows[i]));
                    }
                }
            }
            catch (Exception)
            {

            }
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(list));
        }
    }
}
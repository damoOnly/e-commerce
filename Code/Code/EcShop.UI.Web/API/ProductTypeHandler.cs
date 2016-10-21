using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;

using Newtonsoft.Json;

using EcShop.ControlPanel.Commodities;
using EcShop.Core;
using EcShop.Entities.Commodities;
using EcShop.Membership.Context;
using EcShop.Entities.Promotions;
using EcShop.ControlPanel.Promotions;
namespace EcShop.UI.Web.API
{
    public class ProductTypeHandler : System.Web.IHttpHandler
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
                case "GetAllProduct":
                    this.GetAllProduct(context);
                    break;

                case "GetCouponById":
                    this.GetCouponById(context);
                    break;

                case "GetSelectProduct":
                    this.GetSelectProduct(context);
                    break;

                case "GetSelectProductType":
                    this.GetSelectProductType(context);
                    break;
            }
        }

        public void GetCouponById(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            StringBuilder stringBuilder = new StringBuilder();

            int couponId;

            if (!int.TryParse(context.Request["couponId"].ToString(), out couponId))
            {
                context.Response.Write("{\"success\":false,\"msg\":\"异常操作\"}");
                return;
            }

            else
            {
                CouponInfo coupon = CouponHelper.GetCoupon(couponId);
                if (coupon == null)
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"异常操作\"}");
                    return;
                }
                if (coupon.ClosingTime.CompareTo(System.DateTime.Now) < 0)
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"该优惠券已经结束！\"}");
                    return;
                }
                Globals.EntityCoding(coupon, false);
                stringBuilder.Append("{");
                stringBuilder.AppendFormat("\"Name\":\"{0}\",", coupon.Name);
                stringBuilder.AppendFormat("\"StartTime\":\"{0}\",", coupon.StartTime.ToString("yyyyMMdd"));
                stringBuilder.AppendFormat("\"ClosingTime\":\"{0}\",", coupon.ClosingTime.ToString("yyyyMMdd"));
                if (coupon.Amount.HasValue)
                {
                    stringBuilder.AppendFormat("\"Amount\":\"{0}\",", string.Format("{0:F2}", coupon.Amount));
                }
                stringBuilder.AppendFormat("\"DiscountValue\":\"{0}\",", coupon.DiscountValue.ToString("F2"));
                stringBuilder.AppendFormat("\"NeedPoint\":\"{0}\",", coupon.NeedPoint.ToString());
                stringBuilder.AppendFormat("\"UseType\":{0},", coupon.UseType);
                if (!string.IsNullOrEmpty(coupon.SendTypeItem))
                {
                    stringBuilder.AppendFormat("\"SendTypeItem\":\"{0}\",", coupon.SendTypeItem);
                }
                stringBuilder.AppendFormat("\"SendType\":{0}", coupon.SendType);
          
                stringBuilder.Append("}");

                context.Response.Write(stringBuilder.ToString());
            }
        }

        public void GetAllProduct(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            StringBuilder stringBuilder = new StringBuilder();

            IList<CategoryInfo> sequenceCategories = CatalogHelper.GetSequenceCategories();

            if(sequenceCategories!=null)
            {
                int i=0;
                stringBuilder.Append("[");
                foreach (CategoryInfo item in sequenceCategories)
                {
                    stringBuilder.Append("{");
                    stringBuilder.AppendFormat("\"id\":{0},",item.CategoryId);
                    stringBuilder.AppendFormat("\"pId\":{0},", item.ParentCategoryId);
                    stringBuilder.AppendFormat("\"name\":\"{0}\"", item.Name);
                    stringBuilder.Append("}");
                    if (i < sequenceCategories.Count - 1)
                    {
                        stringBuilder.Append(",");
                    }
                        i++;
                }

                stringBuilder.Append("]");
            }

            else
            {
                stringBuilder.Append("{\"success\":false}");
            }

            context.Response.Write(stringBuilder.ToString());


        }

        public void GetSelectProduct(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            StringBuilder stringBuilder = new StringBuilder();

            int couponId;

            if (!int.TryParse(context.Request["couponId"].ToString(), out couponId))
            {
                context.Response.Write("{\"success\":false,\"msg\":\"异常操作\"}");
                return;
            }

            else
            {
                //CouponInfo coupon = CouponHelper.GetCoupon(couponId);

                IList<CouponsSendTypeItem> listSendTypeItem = CouponHelper.GetCouponsSendTypeItems(couponId);

                IList<int> productIds = new List<int>();
                foreach (CouponsSendTypeItem item in listSendTypeItem)
                {
                    productIds.Add(item.BindId);
                }


                IList<ProductInfo> productlist = ProductHelper.GetProducts(productIds);

                if (productlist != null)
                {

                    int i = 0;
                    stringBuilder.Append("[");
                    foreach (ProductInfo product in productlist)
                    {
                        stringBuilder.Append("{");
                        stringBuilder.AppendFormat("\"id\":{0},", product.ProductId);
                        stringBuilder.AppendFormat("\"name\":\"{0}\"", product.ProductName);
                        stringBuilder.Append("}");
                        if (i < productlist.Count - 1)
                        {
                            stringBuilder.Append(",");
                        }
                        i++;
                    }

                    stringBuilder.Append("]");

                    context.Response.Write(stringBuilder.ToString());
                }
            }
        }

        public void GetSelectProductType(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            StringBuilder stringBuilder = new StringBuilder();

            int couponId;

            if (!int.TryParse(context.Request["couponId"].ToString(), out couponId))
            {
                context.Response.Write("{\"success\":false,\"msg\":\"异常操作\"}");
                return;
            }

            else
            {
                CouponInfo coupon = CouponHelper.GetCoupon(couponId);

                IList<CouponsSendTypeItem> listSendTypeItem = CouponHelper.GetCouponsSendTypeItems(coupon.CouponId);

                IList<int> productTypeIds = new List<int>();
                foreach (CouponsSendTypeItem item in listSendTypeItem)
                {
                    productTypeIds.Add(item.BindId);
                }


                IList<CategoryInfo> categorylist = CatalogHelper.GetCategoryByIds(productTypeIds);

                if (categorylist != null)
                {

                    int i = 0;
                    stringBuilder.Append("[");
                    foreach (CategoryInfo category in categorylist)
                    {
                        stringBuilder.Append("{");
                        stringBuilder.AppendFormat("\"id\":{0},", category.CategoryId);
                        stringBuilder.AppendFormat("\"pId\":{0},", category.ParentCategoryId);
                        stringBuilder.AppendFormat("\"name\":\"{0}\"", category.Name);
                        stringBuilder.Append("}");
                        if (i < categorylist.Count - 1)
                        {
                            stringBuilder.Append(",");
                        }
                        i++;
                    }

                    stringBuilder.Append("]");

                    context.Response.Write(stringBuilder.ToString());
                }
            }
        }


       


    }


}

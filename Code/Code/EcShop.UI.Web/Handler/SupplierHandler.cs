using Commodities;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.UI.Web.Handler
{
    public class SupplierHandler: System.Web.IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string message = "";
        public void ProcessRequest(System.Web.HttpContext context)
        {
            context.Response.ContentType = "text/json";
            string text = context.Request["action"];
            string key;
            switch (key = text)
            {
                case "GetBanner":
                    this.GetBanner(context);
                    break;
                case "CollectSupplier":
                    this.CollectSupplier(context);
                    break;



            }
            context.Response.Write(this.message);
        }


        /// <summary>
        /// 获取轮播图
        /// </summary>
        /// <param name="context"></param>
        public void GetBanner(System.Web.HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            context.Response.ContentType = "application/json";
            int supplierId;

            if(int.TryParse(context.Request["supplierId"],out supplierId))
            {
                IList<BannerInfo> info = VShopHelper.GetAllBanners(ClientType.PC, supplierId);

                if (info == null)
                {
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"Success\":0");
                    stringBuilder.Append("}");
                    this.message=stringBuilder.ToString();
                    return;
                }

                else
                {
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"Success\":1");
                    stringBuilder.Append(",");


                    stringBuilder.Append("\"Banners\":");
                    string strBanners = Newtonsoft.Json.JsonConvert.SerializeObject(info);
                    stringBuilder.Append(strBanners);
                    stringBuilder.Append("}");
                    this.message = stringBuilder.ToString();
                    return;
                }
            }

            else
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Success\":0");
                stringBuilder.Append("}");
                this.message = stringBuilder.ToString();
                return;
            }


          
        }

        /// <summary>
        /// 收藏店铺
        /// </summary>
        /// <param name="context"></param>
        public void CollectSupplier(System.Web.HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            context.Response.ContentType = "application/json";
            int supplierId;

            if(!int.TryParse(context.Request["supplierId"],out supplierId))
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Success\":0,");
                stringBuilder.Append("\"msg\":\"supplierid错误\"");
                stringBuilder.Append("}");
                this.message = stringBuilder.ToString();
                return;
            }
            Member member = HiContext.Current.User as Member;

            if (member != null)
            {
                if (SupplierHelper.GetSupplier(supplierId) == null)
                {
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"Success\":-1,");
                    stringBuilder.Append("\"msg\":\"店铺不存在\"");
                    stringBuilder.Append("}");
                    this.message = stringBuilder.ToString();
                    return;
                }
                if (SupplierHelper.SupplierIsCollect(member.UserId,supplierId))
                {
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"Success\":-2,");
                    stringBuilder.Append("\"msg\":\"店铺已收藏\"");
                    stringBuilder.Append("}");
                    this.message = stringBuilder.ToString();
                    return;
                }

                SupplierCollectInfo info = new SupplierCollectInfo();

                info.Remark = "";
                info.SupplierId = supplierId;
                info.UserId = member.UserId;
                int id = SupplierHelper.CollectSupplier(info);
                if (id > 0)
                {
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"Success\":1,");
                    stringBuilder.Append("\"msg\":\"店铺收藏成功\"");
                    stringBuilder.Append("}");
                    this.message = stringBuilder.ToString();
                    return;
                }

                else
                {
                    stringBuilder.Append("{");
                    stringBuilder.Append("\"Success\":-3,");
                    stringBuilder.Append("\"msg\":\"店铺收藏失败\"");
                    stringBuilder.Append("}");
                    this.message = stringBuilder.ToString();
                    return;
                }
            }

            else
            {
                stringBuilder.Append("{");
                stringBuilder.Append("\"Success\":-4,");
                stringBuilder.Append("\"msg\":\"您还未登陆\"");
                stringBuilder.Append("}");
                this.message = stringBuilder.ToString();
                return;
            }
        }

    }
}

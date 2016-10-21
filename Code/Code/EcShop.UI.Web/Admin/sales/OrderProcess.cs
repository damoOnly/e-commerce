using Ecdev.Weixin.MP.Domain;
using EcShop.ControlPanel.Commodities;
using EcShop.ControlPanel.Sales;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Comments;
using EcShop.Entities.Commodities;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Entities.Sales;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.SaleSystem.Vshop;
using EcShop.UI.ControlPanel.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace EcShop.UI.Web.Admin.sales
{
    public class OrderProcess : System.Web.IHttpHandler
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
                case "OrderRefundReceive":
                    this.OrderRefundReceive(context);
                    return;
                case "OrderReplaceReceive":
                    this.OrderReplaceReceive(context);
                    return;
                case "OrderReturnsReceive":
                    this.OrderReturnsReceive(context);
                    return;
                case "CheckIsPrivilege":
                    this.CheckIsPrivilege(context);
                    return;
           }
        }
        
        /// <summary>
        /// 退款单领取操作
        /// </summary>
        /// <param name="context"></param>
        public void OrderRefundReceive(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string RefundId = context.Request["RefundId"];
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");

            if (string.IsNullOrEmpty(RefundId))
            {
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }
            IUser user = HiContext.Current.User as SiteManager;

            RefundInfo refundInfo = new RefundInfo();
            refundInfo.RefundId = int.Parse(RefundId);
            refundInfo.ReceiveTime = DateTime.Now;
            refundInfo.Status = 3;//处理中
            if (user != null)
            {
               refundInfo.Operator = user.Username;
            }
            
            bool flag = TradeHelper.UpdateOrderRefundInfo(refundInfo);
            if (flag)
            {
                stringBuilder.Append("\"Status\":\"OK\"");
                stringBuilder.Append("}");
            }
            else
            {
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
            }

            context.Response.Write(stringBuilder.ToString());
        }

        /// <summary>
        /// 换货单领取操作
        /// </summary>
        /// <param name="context"></param>
        public void OrderReplaceReceive(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string ReplaceId = context.Request["ReplaceId"];
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");

            if (string.IsNullOrEmpty(ReplaceId))
            {
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }
            IUser user = HiContext.Current.User as SiteManager;
           
            ReplaceInfo replaceInfo = new ReplaceInfo();
            replaceInfo.ReplaceId = int.Parse(ReplaceId);
            replaceInfo.ReceiveTime = DateTime.Now;
            replaceInfo.HandleStatus = 3;//处理中
            if (user != null)
            {
                replaceInfo.Operator = user.Username;
            }


            bool flag = TradeHelper.UpdateOrderReplaceInfo(replaceInfo);
            if (flag)
            {
                stringBuilder.Append("\"Status\":\"OK\"");
                stringBuilder.Append("}");
            }
            else
            {
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
            }

            context.Response.Write(stringBuilder.ToString());
        }

        /// <summary>
        /// 退货单领取操作
        /// </summary>
        /// <param name="context"></param>
        public void OrderReturnsReceive(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string ReturnsId = context.Request["ReturnsId"];
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");

            if (string.IsNullOrEmpty(ReturnsId))
            {
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
                context.Response.Write(stringBuilder.ToString());
                return;
            }
            IUser user = HiContext.Current.User as SiteManager;

            ReturnsInfo returnsInfo = new ReturnsInfo();
            returnsInfo.ReturnsId = int.Parse(ReturnsId);
            returnsInfo.ReceiveTime = DateTime.Now;
            returnsInfo.HandleStatus = 3;//处理中
            if (user != null)
            {
                returnsInfo.Operator = user.Username;
            }

            bool flag = TradeHelper.UpdateOrderReturnsInfo(returnsInfo);
            if (flag)
            {
                stringBuilder.Append("\"Status\":\"OK\"");
                stringBuilder.Append("}");
            }
            else
            {
                stringBuilder.Append("\"Status\":\"-1\"");
                stringBuilder.Append("}");
            }

            context.Response.Write(stringBuilder.ToString());
        }

        /// <summary>
        /// 判断是否有权限
        /// </summary>
        /// <param name="context"></param>
        public void CheckIsPrivilege(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string rightname = context.Request["rightname"];

            object obj = (Privilege)Enum.Parse(typeof(Privilege),rightname);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");

            bool flag = ManagerHelper.CheckIsPrivilege((Privilege)obj);

            if (flag)
            {
                stringBuilder.Append("\"Status\":\"OK\"");
                stringBuilder.Append("}");
            }
            else
            {
                stringBuilder.Append("\"Status\":\"NO\""); //没有权限
                stringBuilder.Append("}");
            }

            context.Response.Write(stringBuilder.ToString());
        }
    }
}

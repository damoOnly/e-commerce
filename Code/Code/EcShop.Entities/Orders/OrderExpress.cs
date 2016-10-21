using EcShop.Entities.Promotions;
using Ecdev.Components.Validation.Validators;
using System;
using System.Collections.Generic;
namespace EcShop.Entities.Orders
{
    public class OrderExpress
    {
        /// <summary>
        /// 发货单号
        /// </summary>
        public string ShipOrderNumber
        {
            get;
            set;
        }
        
        //物流公司
        public string ExpressCompanyName
        {
            get;
            set;
        }

        public OrderStatus OrderStatus
        {
            get;
            set;
        }

        public string ExpressCompanyAbb { get; set; }
    }
}

using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace EcShop.Entities.PO
{
    public class PurchaseOrderItemQuery : Pagination
    {
        /// <summary>
        /// 
        /// </summary>
        public int POId
        {
            set;
            get;
        }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName
        {
            set;
            get;
        }
        /// <summary>
        /// 产品条码
        /// </summary>
        public string BarCode
        {
            set;
            get;
        }
    }
}

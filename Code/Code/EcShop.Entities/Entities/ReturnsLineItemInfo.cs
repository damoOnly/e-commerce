using EcShop.Entities.Promotions;
using System;
namespace EcShop.Entities
{
    public class OrderAppFormItems
    {
        public int Id
        {
            get;
            set;
        }
        public int AId
        {
            get;
            set;
        }
        /// <summary>
        /// 申请售后单类型，1表示退货单，2表示退换单
        /// </summary>
        public int ApplyType
        {
            get;
            set;
        }
        public string SkuId
        {
            get;
            set;
        }
        public int ProductId
        {
            get;
            set;
        }
        public int Quantity
        {
            get;
            set;
        }
        public decimal ItemAdjustedPrice
        {
            get;
            set;
        }
        public string ItemDescription
        {
            get;
            set;
        }
        public string ThumbnailsUrl
        {
            get;
            set;
        }
        public decimal ItemWeight
        {
            get;
            set;
        }
        public string SKUContent
        {
            get;
            set;
        }
        public int PromotionId
        {
            get;
            set;
        }
        public string PromotionName
        {
            get;
            set;
        }
        public decimal TaxRate
        {
            get;
            set;
        }
        public int TemplateId
        {
            get;
            set;
        }
        public int storeId
        {
            get;
            set;
        }
        public int SupplierId
        {
            get;
            set;
        }
        public string SupplierName
        {
            get;
            set;
        }
        public decimal DecuctFee
        {
            get;
            set;
        }
    }
}

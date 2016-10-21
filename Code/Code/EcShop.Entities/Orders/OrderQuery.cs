using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Orders
{
    public class OrderQuery : Pagination
    {
        public int IsRefund
        {
            get;
            set;
        }
        public enum OrderType
        {
            NormalProduct = 1,
            GroupBuy
        }
        private bool _ShowGiftOrder = true;
        public OrderStatus Status
        {
            get;
            set;
        }

        public UserStatus UseStatus
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }
        public string ShipTo
        {
            get;
            set;
        }
        public string ProductName
        {
            get;
            set;
        }
        public string OrderId
        {
            get;
            set;
        }
        public string ShipId
        {
            get;
            set;
        }
        public System.DateTime? StartDate
        {
            get;
            set;
        }
        public System.DateTime? EndDate
        {
            get;
            set;
        }
        public int? PaymentType
        {
            get;
            set;
        }
        public int? GroupBuyId
        {
            get;
            set;
        }
        public int? ShippingModeId
        {
            get;
            set;
        }

        public string CellPhone
        {
            get;
            set;
        }

        public int? IsPrinted
        {
            get;
            set;
        }
        public int? RegionId
        {
            get;
            set;
        }
        public int? SourceOrder
        {
            get;
            set;
        }
        public RefundStatus RefundState
        {
            get;
            set;
        }
        public int DataType
        {
            get;
            set;
        }
        public OrderQuery.OrderType? Type
        {
            get;
            set;
        }
        public int? UserId
        {
            get;
            set;
        }
        public bool ShowGiftOrder
        {
            get
            {
                return this._ShowGiftOrder;
            }
            set
            {
                this._ShowGiftOrder = value;
            }
        }

        public int? StoreId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否来自门店
        /// </summary>
        public bool IsStore
        {
            get;
            set;
        }
        public int? SupplierId
        {
            get;
            set;
        }
        public int? SiteId
        {
            get;
            set;
        }

        //订单的时间类型  1:下单时间(OrderDate) 2:付款时间（PayDate） 3:发货时间(ShippingDate)
        public int? OrderTimeType
        {
            get;
            set;
        }
        public bool IsSupplierManager
        {
            get;
            set;
        }

        public bool WaitToComment
        {
            get;
            set;
        }

        public string SourceOrderId { get; set; }

        public int? DateContrastType { get; set; }

        public int? DateContrastValue { get; set; }

        public int? SendWMSCount { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public string ShipOrderNumber { get; set; }


        /// <summary>
        /// 实名认证
        /// </summary>
        public int? PayerIdStatus { get; set; }


    }
}

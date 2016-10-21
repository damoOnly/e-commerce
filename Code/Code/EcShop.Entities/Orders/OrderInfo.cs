using EcShop.Entities.Promotions;
using Ecdev.Components.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using EcShop.Core;
using EcShop.Entities.Sales;
namespace EcShop.Entities.Orders
{
    public class OrderInfo : ICloneable
    {
        private System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems;
        private System.Collections.Generic.IList<ShoppingCartPresentInfo> presentproducts;
        private System.Collections.Generic.IList<OrderGiftInfo> gifts;
        private decimal adjustedFreigh;
        public static event System.EventHandler<System.EventArgs> Created;
        public static event System.EventHandler<System.EventArgs> Payment;
        public static event System.EventHandler<System.EventArgs> Deliver;
        public static event System.EventHandler<System.EventArgs> Refund;
        public static event System.EventHandler<System.EventArgs> Closed;
        public System.Collections.Generic.Dictionary<string, LineItemInfo> LineItems
        {
            get
            {
                if (this.lineItems == null)
                {
                    this.lineItems = new System.Collections.Generic.Dictionary<string, LineItemInfo>();
                }
                return this.lineItems;
            }
        }

        public System.Collections.Generic.IList<ShoppingCartPresentInfo> PresentProducts
        {
            get
            {
                if (this.presentproducts == null)
                {
                    this.presentproducts = new System.Collections.Generic.List<ShoppingCartPresentInfo>();
                }
                return this.presentproducts;
            }
            set {
                this.presentproducts = value;
            }
        }

        private List<ProductsCombination> combinationItemInfos;
        public List<ProductsCombination> CombinationItemInfos
        {
            get
            {
                if (this.combinationItemInfos == null)
                {
                    this.combinationItemInfos = new List<ProductsCombination>();
                }
                return this.combinationItemInfos;
            }
            set
            {
                this.combinationItemInfos = value;
            }
        }
        public System.Collections.Generic.IList<OrderGiftInfo> Gifts
        {
            get
            {
                if (this.gifts == null)
                {
                    this.gifts = new System.Collections.Generic.List<OrderGiftInfo>();
                }
                return this.gifts;
            }
        }
        public string OrderId
        {
            get;
            set;
        }

        //后台优惠抵扣
        public decimal Deductible
        {
            get;
            set;
        }

        public int ActivityType
        {
            get;
            set;
        }

        public string TaobaoOrderId
        {
            get;
            set;
        }
        public string GatewayOrderId
        {
            get;
            set;
        }
        public string Remark
        {
            get;
            set;
        }
        public OrderMark? ManagerMark
        {
            get;
            set;
        }
        public string ManagerRemark
        {
            get;
            set;
        }
        /// <summary>
        /// 备注人
        /// </summary>
        public string RemarkPeople
        {
            get;
            set;
        }


        public int payerIdStatus { get; set; }

        public int PaymentStatus { get; set; }

        public string Gateway
        {
            get;
            set;
        }
        [RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValOrder", MessageTemplate = "订单折扣不能为空，金额大小负1000万-1000万之间")]
        public decimal AdjustedDiscount
        {
            get;
            set;
        }
        public OrderStatus OrderStatus
        {
            get;
            set;
        }
        public string CloseReason
        {
            get;
            set;
        }
        public System.DateTime OrderDate
        {
            get;
            set;
        }
        public System.DateTime PayDate
        {
            get;
            set;
        }
        public System.DateTime ShippingDate
        {
            get;
            set;
        }
        public System.DateTime FinishDate
        {
            get;
            set;
        }
        public int ReferralUserId
        {
            get;
            set;
        }
        public int UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 清关（0：不清关；1：清关）
        /// </summary>
        public int IsCustomsClearance
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }
        public string EmailAddress
        {
            get;
            set;
        }
        public string RealName
        {
            get;
            set;
        }
        public string QQ
        {
            get;
            set;
        }
        public string Wangwang
        {
            get;
            set;
        }
        public string MSN
        {
            get;
            set;
        }
        public string ShippingRegion
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }
        public string ZipCode
        {
            get;
            set;
        }
        public string ShipTo
        {
            get;
            set;
        }
        public string TelPhone
        {
            get;
            set;
        }
        public string CellPhone
        {
            get;
            set;
        }
        public string ShipToDate
        {
            get;
            set;
        }
        public int ShippingModeId
        {
            get;
            set;
        }
        public string ModeName
        {
            get;
            set;
        }
        public int RealShippingModeId
        {
            get;
            set;
        }
        public string RealModeName
        {
            get;
            set;
        }
        public int RegionId
        {
            get;
            set;
        }
        public decimal Freight
        {
            get;
            set;
        }
        public int ShippingId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否推送到WMS，0未推送，1已推送
        /// </summary>
        public int IsSendWMS { get; set; }


        [RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValOrder", MessageTemplate = "运费不能为空，金额大小0-1000万之间")]
        public decimal AdjustedFreight
        {
            get
            {
                return this.adjustedFreigh;
            }
            set
            {
                this.adjustedFreigh = value;
            }
        }
        public string ShipOrderNumber
        {
            get;
            set;
        }
        public decimal Weight
        {
            get
            {
                decimal num = 0m;
                foreach (LineItemInfo current in this.LineItems.Values)
                {
                    num += current.ItemWeight * current.ShipmentQuantity;
                }
                return num;
            }
        }
        public string ExpressCompanyName
        {
            get;
            set;
        }
        public string ExpressCompanyAbb
        {
            get;
            set;
        }

        public int PaymentTypeId
        {
            get;
            set;
        }
        public string PaymentType
        {
            get;
            set;
        }
        public decimal PayCharge
        {
            get;
            set;
        }
        public RefundStatus RefundStatus
        {
            get;
            set;
        }
        public decimal RefundAmount
        {
            get;
            set;
        }
        public string RefundRemark
        {
            get;
            set;
        }
        public int Points
        {
            get;
            set;
        }
        public int ReducedPromotionId
        {
            get;
            set;
        }
        public string ReducedPromotionName
        {
            get;
            set;
        }
        public decimal ReducedPromotionAmount
        {
            get;
            set;
        }
        public bool IsReduced
        {
            get;
            set;
        }
        public int SentTimesPointPromotionId
        {
            get;
            set;
        }
        public string SentTimesPointPromotionName
        {
            get;
            set;
        }
        public decimal TimesPoint
        {
            get;
            set;
        }
        public bool IsSendTimesPoint
        {
            get;
            set;
        }
        public int FreightFreePromotionId
        {
            get;
            set;
        }
        public string FreightFreePromotionName
        {
            get;
            set;
        }
        public bool IsFreightFree
        {
            get;
            set;
        }
        public int GroupBuyId
        {
            get;
            set;
        }
        public int CountDownBuyId
        {
            get;
            set;
        }
        public int BundlingID
        {
            get;
            set;
        }
        public int? BundlingNum
        {
            get;
            set;
        }
        public decimal NeedPrice
        {
            get;
            set;
        }
        public GroupBuyStatus GroupBuyStatus
        {
            get;
            set;
        }
        public string CouponName
        {
            get;
            set;
        }
        public string CouponCode
        {
            get;
            set;
        }
        public decimal CouponAmount
        {
            get;
            set;
        }
        public decimal CouponValue
        {
            get;
            set;
        }
        public decimal BundlingPrice
        {
            get;
            set;
        }
        public decimal Tax
        {
            get;
            set;
        }
        public string InvoiceTitle
        {
            get;
            set;
        }
        public OrderSource OrderSource
        {
            get;
            set;
        }
        public string Sender
        {
            get;
            set;
        }
        public bool IsPrinted
        {
            get;
            set;
        }
        /// <summary>
        /// 收货人身份证号码
        /// </summary>
        public string IdentityCard
        {
            get;
            set;
        }
        /// <summary>
        /// 预清关时间
        /// </summary>
        public DateTime? PCustomsClearanceDate
        {
            get;
            set;
        }
        /// <summary>
        /// 退款状态
        /// </summary>
        public int IsRefund
        {
            get;
            set;
        }
        /// <summary>
        /// 已经执行退款
        /// </summary>
        public int IsCancelOrder
        {
            get;
            set;
        }
        public string SourceOrderId
        {
            get;
            set;
        }
        public int OrderType
        {
            get;
            set;
        }
        public decimal OriginalTax
        {
            get;
            set;
        }
        public int SiteId
        {
            get;
            set;
        }

        public int StoreId
        {
            get;
            set;
        }

        public int SupplierId
        {
            get;
            set;
        }

        /// <summary>
        /// 现金券名称
        /// </summary>
        public string VoucherName { get; set; }

        /// <summary>
        /// 现金券号码
        /// </summary>
        public string VoucherCode { get; set; }

        /// <summary>
        /// 现金券使用订单金额条件
        /// </summary>
        public decimal VoucherAmount { get; set; }

        /// <summary>
        /// 现金券抵扣金额
        /// </summary>
        public decimal VoucherValue { get; set; }
        /// <summary>
        /// 备注时间
        /// </summary>
        public DateTime? RemarkTime { get; set; }

        /// <summary>
        /// 所有备注信息
        /// </summary>
        public List<OrderRemark> Remarks { get; set; }

        /// <summary>
        /// 最新的备注
        /// </summary>
        public OrderRemark LatestRemark
        {
            get
            {
                if (this.Remarks == null || this.Remarks.Count <= 0)
                {
                    return null;
                }

                return this.Remarks.OrderByDescending(p => p.RecordTime).First();
            }
        }

        public OrderInfo()
        {
            this.OrderStatus = OrderStatus.WaitBuyerPay;
            this.RefundStatus = RefundStatus.None;
            this.Remarks = new List<OrderRemark>();
        }
        public decimal GetTotal()
        {
            decimal d = this.GetAmount();
            if (this.BundlingID > 0)
            {
                d = this.BundlingPrice;
            }
            if (this.IsReduced)
            {
                d -= this.ReducedPromotionAmount;
            }
            d += this.AdjustedFreight;
            d += this.PayCharge;
            d += this.Tax;
            if (!string.IsNullOrEmpty(this.CouponCode))
            {
                d -= this.CouponValue;
            }
            if (!string.IsNullOrEmpty(this.VoucherCode))
            {
                d -= this.VoucherValue;
            }
            return d + this.AdjustedDiscount;
        }
        public decimal GetTotal(bool isChildAndFirstChildThenOrOriginalOrder)
        {
            decimal d = this.GetAmount();
            if (this.BundlingID > 0)
            {
                d = this.BundlingPrice;
            }
            if (this.IsReduced)
            {
                d -= this.ReducedPromotionAmount;
            }
            //去掉运费，分单出了第一个单，其他的不用运费
            if (isChildAndFirstChildThenOrOriginalOrder)
            {
                d += this.AdjustedFreight;
            }
            d += this.PayCharge;
            d += this.Tax;
            if (!string.IsNullOrEmpty(this.CouponCode))
            {
                d -= this.CouponValue;
            }
            if (!string.IsNullOrEmpty(this.VoucherCode))
            {
                d -= this.VoucherValue;
            }
            return d + this.AdjustedDiscount;
        }


        public decimal GetNewTotal()
        {
            decimal d = this.GetNewAmount();
            if (this.BundlingID > 0)
            {
                d = this.BundlingPrice;
            }
            if (this.IsReduced)
            {
                d -= this.ReducedPromotionAmount;
            }
            d += this.AdjustedFreight;
            d += this.PayCharge;
            d += this.Tax;
            if (!string.IsNullOrEmpty(this.CouponCode))
            {
                d -= this.CouponValue;
            }
            if (!string.IsNullOrEmpty(this.VoucherCode))
            {
                d -= this.VoucherValue;
            }
            return d + this.AdjustedDiscount;
        }
        public decimal GetNewTotal(bool isChildAndFirstChildThenOrOriginalOrder)
        {
            decimal d = this.GetNewAmount();
            if (this.BundlingID > 0)
            {
                d = this.BundlingPrice;
            }
            if (this.IsReduced)
            {
                d -= this.ReducedPromotionAmount;
            }
            //去掉运费，分单出了第一个单，其他的不用运费
            if (isChildAndFirstChildThenOrOriginalOrder)
            {
                d += this.AdjustedFreight;
            }
            d += this.PayCharge;
            d += this.Tax;
            if (!string.IsNullOrEmpty(this.CouponCode))
            {
                d -= this.CouponValue;
            }
            if (!string.IsNullOrEmpty(this.VoucherCode))
            {
                d -= this.VoucherValue;
            }
            return d + this.AdjustedDiscount;

        }

        //活动优惠价
        public decimal GetActivityPrice()
        {
            decimal Price = 0m;
            if (lineItems != null || this.lineItems.Count > 0)
            {
                foreach (LineItemInfo current in this.LineItems.Values)
                {
                    switch (current.PromoteType)
                    {
                        case PromoteType.ProductPromotion:
                            // Price += current.ItemAdjustedPrice - current.PromotionPrice;
                            Price += (current.ItemListPrice - current.ItemAdjustedPrice) * current.Quantity;

                            break;
                        case PromoteType.SecondReducePrice:
                            //Price += (current.ItemAdjustedPrice - current.PromotionPrice)*current.Quantity;
                            Price += (current.ItemListPrice - current.ItemAdjustedPrice) * current.Quantity;
                            break;
                        case PromoteType.ProductDiscount:
                            //Price += current.ItemAdjustedPrice - current.PromotionPrice;
                            Price += (current.ItemListPrice - current.ItemAdjustedPrice) * current.Quantity;

                            break;
                    }
                }
            }
            return Price;
        }


        public decimal GetTotals(bool isChildAndFirstChildThenOrOriginalOrder)
        {
            decimal d = this.GetAmount();
            if (this.BundlingID > 0)
            {
                d = this.BundlingPrice;
            }
            if (this.IsReduced)
            {
                d -= this.ReducedPromotionAmount;
            }
            //去掉运费，分单出了第一个单，其他的不用运费
            if (isChildAndFirstChildThenOrOriginalOrder)
            {
                d += this.AdjustedFreight;
            }
            d += this.PayCharge;
            d += this.Tax;
            if (!string.IsNullOrEmpty(this.CouponCode))
            {
                d -= this.CouponValue;
            }
            if (!string.IsNullOrEmpty(this.VoucherCode))
            {
                d -= this.VoucherValue;
            }
            return d + this.AdjustedDiscount - Deductible;
        }

        public virtual decimal GetCostPrice()
        {
            decimal num = 0m;
            foreach (LineItemInfo current in this.LineItems.Values)
            {
                num += current.ItemCostPrice * current.ShipmentQuantity;
            }
            foreach (OrderGiftInfo current2 in this.Gifts)
            {
                num += current2.CostPrice * current2.Quantity;
            }
            return num;
        }
        public virtual decimal GetProfit()
        {
            return this.GetTotal() - this.RefundAmount - this.GetCostPrice();
        }

        public virtual decimal GetNewProfit()
        {
            return this.GetNewTotal() - this.RefundAmount - this.GetCostPrice();
        }

        public int GetGroupBuyOerderNumber()
        {
            int result;
            if (this.GroupBuyId > 0)
            {
                using (System.Collections.Generic.Dictionary<string, LineItemInfo>.ValueCollection.Enumerator enumerator = this.LineItems.Values.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        LineItemInfo current = enumerator.Current;
                        result = current.Quantity;
                        return result;
                    }
                }
            }
            result = 0;
            return result;
        }
        public decimal GetAmount()
        {
            decimal num = 0m;
            foreach (LineItemInfo current in this.LineItems.Values)
            {
                num += current.GetSubTotal();
            }
            return num;
        }

        public decimal GetNewAmount()
        {
            decimal num = 0m;
            foreach (LineItemInfo current in this.LineItems.Values)
            {
                num += current.GetNewSubTotal();
            }
            return num;
        }


        public bool CheckAction(OrderActions action)
        {
            bool result;
            if (this.OrderStatus == OrderStatus.Finished || this.OrderStatus == OrderStatus.Closed)
            {
                result = false;
            }
            else
            {
                switch (action)
                {
                    case OrderActions.BUYER_PAY:
                    case OrderActions.SELLER_CONFIRM_PAY:
                    case OrderActions.SELLER_MODIFY_TRADE:
                    case OrderActions.SELLER_CLOSE:
                    case OrderActions.BUYER_CANCEL:
                        result = (this.OrderStatus == OrderStatus.WaitBuyerPay);
                        return result;
                    case OrderActions.BUYER_CONFIRM_GOODS:
                    case OrderActions.SELLER_FINISH_TRADE:
                        result = (this.OrderStatus == OrderStatus.SellerAlreadySent);
                        return result;
                    case OrderActions.SELLER_SEND_GOODS:
                        result = (this.OrderStatus == OrderStatus.BuyerAlreadyPaid || (this.OrderStatus == OrderStatus.WaitBuyerPay && this.Gateway == "hishop.plugins.payment.podrequest"));
                        return result;
                    case OrderActions.SELLER_REJECT_REFUND:
                        result = (this.OrderStatus == OrderStatus.BuyerAlreadyPaid || this.OrderStatus == OrderStatus.SellerAlreadySent);
                        return result;
                    case OrderActions.MASTER_SELLER_MODIFY_DELIVER_ADDRESS:
                    case OrderActions.MASTER_SELLER_MODIFY_PAYMENT_MODE:
                    case OrderActions.MASTER_SELLER_MODIFY_SHIPPING_MODE:
                    case OrderActions.MASTER_SELLER_MODIFY_GIFTS:
                        result = (this.OrderStatus == OrderStatus.WaitBuyerPay || this.OrderStatus == OrderStatus.BuyerAlreadyPaid);
                        return result;
                }
                result = false;
            }
            return result;
        }
        public static string GetOrderStatusName(OrderStatus orderStatus, string sourceOrderId, object payTime = null)
        {
            string result = "-";
            switch (orderStatus)
            {
                case OrderStatus.WaitBuyerPay:
                    result = "等待买家付款";
                    break;
                case OrderStatus.BuyerAlreadyPaid:
                    result = "已付款,等待发货";
                    //if (OrderInfo.IsInClearance(payTime))
                    //{
                    //    result = "清关中,等待发货";
                    //}
                    break;
                case OrderStatus.SellerAlreadySent:
                    result = "已发货";
                    break;
                case OrderStatus.Closed:
                    result = "已关闭";
                    break;
                case OrderStatus.Finished:
                    result = "订单已完成";
                    break;
                case OrderStatus.ApplyForRefund:
                    result = "已申请退款";
                    break;
                case OrderStatus.ApplyForReturns:
                    result = "申请退货";
                    break;
                case OrderStatus.ApplyForReplacement:
                    result = "申请换货";
                    break;
                case OrderStatus.Refunded:
                    result = "已退款";
                    break;
                case OrderStatus.Returned:
                    result = "已退货";
                    break;
                case OrderStatus.UnpackOrMixed:
                    if (!string.IsNullOrEmpty(sourceOrderId))
                    {
                        if (sourceOrderId.IndexOf(",") > 0)
                        {
                            result = "已合单";
                        }
                        else
                        {
                            result = "已分单";
                        }
                    }
                    else
                    {
                        result = "已合单/已分单";
                    }
                    break;
                default:
                    if (orderStatus == OrderStatus.History)
                    {
                        result = "历史订单";
                    }
                    break;
            }
            return result;
        }

        public static string GetUserOrderStatusName(OrderStatus orderStatus, string sourceOrderId)
        {
            string result = "-";
            switch (orderStatus)
            {
                case OrderStatus.WaitBuyerPay:
                    result = "等待买家付款";
                    break;
                case OrderStatus.BuyerAlreadyPaid:
                    result = "已付款,等待发货";
                    break;
                case OrderStatus.SellerAlreadySent:
                    result = "已发货";
                    break;
                case OrderStatus.Closed:
                    result = "已关闭";
                    break;
                case OrderStatus.Finished:
                    result = "订单已完成";
                    break;
                case OrderStatus.ApplyForRefund:
                    result = "申请退款";
                    break;
                case OrderStatus.ApplyForReturns:
                    result = "申请退货";
                    break;
                case OrderStatus.ApplyForReplacement:
                    result = "申请换货";
                    break;
                case OrderStatus.Refunded:
                    result = "已退款";
                    break;
                case OrderStatus.Returned:
                    result = "已退货";
                    break;
            }
            return result;
        }

        public static void OnCreated(OrderInfo order)
        {
            if (OrderInfo.Created != null)
            {
                OrderInfo.Created(order, new System.EventArgs());
            }
        }
        public void OnCreated()
        {
            if (OrderInfo.Created != null)
            {
                OrderInfo.Created(this, new System.EventArgs());
            }
        }
        public static void OnPayment(OrderInfo order)
        {
            if (OrderInfo.Payment != null)
            {
                OrderInfo.Payment(order, new System.EventArgs());
            }
        }
        public void OnPayment()
        {
            if (OrderInfo.Payment != null)
            {
                OrderInfo.Payment(this, new System.EventArgs());
            }
        }
        public static void OnDeliver(OrderInfo order)
        {
            if (OrderInfo.Deliver != null)
            {
                OrderInfo.Deliver(order, new System.EventArgs());
            }
        }
        public void OnDeliver()
        {
            if (OrderInfo.Deliver != null)
            {
                OrderInfo.Deliver(this, new System.EventArgs());
            }
        }
        public static void OnRefund(OrderInfo order)
        {
            if (OrderInfo.Refund != null)
            {
                OrderInfo.Refund(order, new System.EventArgs());
            }
        }
        public void OnRefund()
        {
            if (OrderInfo.Refund != null)
            {
                OrderInfo.Refund(this, new System.EventArgs());
            }
        }
        public static void OnClosed(OrderInfo order)
        {
            if (OrderInfo.Closed != null)
            {
                OrderInfo.Closed(order, new System.EventArgs());
            }
        }
        public void OnClosed()
        {
            if (OrderInfo.Closed != null)
            {
                OrderInfo.Closed(this, new System.EventArgs());
            }
        }

        public decimal Amount { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        ///// <summary>
        ///// 是否清关中
        ///// </summary>
        ///// <param name="payDate"></param>
        ///// <returns></returns>
        //public static bool IsInClearance(object payDate)
        //{
        //    if (payDate != null)
        //    {
        //        DateTime payDateTime;
        //        DateTime.TryParse(payDate.ToString(), out payDateTime);
        //        if (payDateTime != DateTime.MinValue)
        //        {
        //            if (payDateTime.Date.AddDays(1) <= DateTime.Now)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// 是否清关中
        ///// </summary>
        //public bool InClearance
        //{
        //    get
        //    {
        //        if (this.PayDate != DateTime.MinValue)
        //        {
        //            if (this.PayDate.Date.AddDays(1) <= DateTime.Now)
        //            {
        //                return true;
        //            }
        //        }
        //        return false;
        //    }
        //}

        public OrderInfo ToChildOrder()
        {
            OrderInfo orderInfo = new OrderInfo();
            orderInfo.OrderDate = this.OrderDate;
            orderInfo.ReferralUserId = this.ReferralUserId;
            orderInfo.UserId = this.UserId;
            orderInfo.Username = this.Username;
            orderInfo.Wangwang = this.Wangwang;
            orderInfo.RealName = this.RealName;
            orderInfo.EmailAddress = this.EmailAddress;
            orderInfo.Remark = this.Remark;
            orderInfo.AdjustedDiscount = this.AdjustedDiscount;
            orderInfo.OrderStatus = this.OrderStatus;
            orderInfo.ShippingRegion = this.ShippingRegion;
            orderInfo.Address = this.Address;
            orderInfo.ZipCode = this.ZipCode;
            orderInfo.ShipTo = this.ShipTo;
            orderInfo.TelPhone = this.TelPhone;
            orderInfo.CellPhone = this.CellPhone;
            orderInfo.ShipToDate = this.ShipToDate;
            orderInfo.ShippingModeId = this.ShippingModeId;
            orderInfo.ModeName = this.ModeName;
            orderInfo.RegionId = this.RegionId;

            orderInfo.ShipOrderNumber = this.ShipOrderNumber;
            // orderInfo.Weight
            orderInfo.ExpressCompanyName = this.ExpressCompanyName;
            orderInfo.ExpressCompanyAbb = this.ExpressCompanyAbb;
            orderInfo.PaymentTypeId = this.PaymentTypeId;
            orderInfo.PaymentType = this.PaymentType;
            orderInfo.PayCharge = this.PayCharge;
            orderInfo.RefundStatus = this.RefundStatus;
            orderInfo.Gateway = this.Gateway;
            //orderInfo.OrderTotal
            orderInfo.Points = this.Points;
            // orderInfo.OrderCostPrice
            //orderInfo.OrderProfit
            //orderInfo.Amount
            orderInfo.ReducedPromotionId = this.ReducedPromotionId;//促销
            orderInfo.ReducedPromotionName = this.ReducedPromotionName;
            //orderInfo.ReducedPromotionAmount = this.ReducedPromotionAmount;
            //orderInfo.IsReduced = this.IsReduced;
            orderInfo.SentTimesPointPromotionId = this.SentTimesPointPromotionId;
            orderInfo.SentTimesPointPromotionName = this.SentTimesPointPromotionName;
            orderInfo.TimesPoint = this.TimesPoint;
            orderInfo.IsSendTimesPoint = this.IsSendTimesPoint;
            orderInfo.FreightFreePromotionId = this.FreightFreePromotionId;
            orderInfo.FreightFreePromotionName = this.FreightFreePromotionName;
            orderInfo.IsFreightFree = this.IsFreightFree;

            orderInfo.CouponName = this.CouponName;
            orderInfo.CouponCode = this.CouponCode;//优惠券
            orderInfo.CouponAmount = this.CouponAmount;
            //orderInfo.CouponValue = this.CouponValue;

            orderInfo.VoucherName = this.VoucherName;
            orderInfo.VoucherCode = this.VoucherCode;//现金券
            orderInfo.VoucherAmount = this.VoucherAmount;
            //orderInfo.VoucherValue = this.VoucherValue;

            orderInfo.IsCustomsClearance = this.IsCustomsClearance;
            //orderInfo.PCustomsClearanceDate = this.PCustomsClearanceDate;
            orderInfo.OrderType = this.OrderType;
            //orderInfo.OriginalTax = this.OriginalTax;
            orderInfo.IdentityCard = this.IdentityCard;
            orderInfo.GatewayOrderId = this.GatewayOrderId;
            orderInfo.SiteId = this.SiteId;

            orderInfo.TaobaoOrderId = this.TaobaoOrderId;
            orderInfo.GroupBuyId = this.GroupBuyId;
            orderInfo.NeedPrice = this.NeedPrice;
            orderInfo.CountDownBuyId = this.CountDownBuyId;
            orderInfo.BundlingID = this.BundlingID;
            orderInfo.BundlingPrice = this.BundlingPrice;
            orderInfo.OrderSource = this.OrderSource;
            orderInfo.InvoiceTitle = this.InvoiceTitle;
            orderInfo.SourceOrderId = this.OrderId;
            orderInfo.CombinationItemInfos = this.combinationItemInfos;
            return orderInfo;
        }
    }

    /// <summary>
    /// 后台订单备注列表
    /// </summary>
    public class OrderRemark
    {
        public string OrderId { get; set; }
        public int Tag { get; set; }
        public string Remark { get; set; }
        public string Operator { get; set; }
        public DateTime RecordTime { get; set; }
        public string TagImg
        {
            get
            {
                Dictionary<int, string> dict = new Dictionary<int, string> 
                {
                    {1,"iconaf"},
                    {2,"iconb"},
                    {3,"iconc"},
                    {4,"icona"},
                    {5,"iconad"},
                    {6,"iconae"}
                };
                if (!dict.ContainsKey(this.Tag))
                {
                    return string.Empty;
                }

                return string.Format("{0}/Admin/images/{1}.gif", Globals.ApplicationPath, dict[this.Tag]);
            }
        }
    }

    public class OrderApiCode
    {
        public string OrderId
        {
            get;
            set;
        }
    }

}

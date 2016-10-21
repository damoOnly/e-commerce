using System;

namespace EcShop.Entities.Orders
{
    public class TradeDetails
    {
        public int Id { get; set; }
        public DateTime TradingTime { get; set; }
        public string AccountId { get; set; }
        public string MerchantNumber { get; set; }
        public string SubMerchantNumber { get; set; }
        public string EquipmentNumber { get; set; }
        public string MicroOrderId { get; set; }
        public string MerchantOrderId { get; set; }
        public string UserIdentity { get; set; }
        public string TradeTypes { get; set; }
        public string TradingStatus { get; set; }
        public string PayingBank { get; set; }
        public string CurrencyType { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RedAmount { get; set; }
        public string WeChatRefundNumber { get; set; }
        public string MerchantsRefundNumber { get; set; }
        public decimal RefundAmount { get; set; }
        public decimal EnterpriseRefundAmount { get; set; }
        public string RefundType { get; set; }
        public string RefundStatus { get; set; }
        public string SkuName { get; set; }
        public string DataPacket { get; set; }
        public decimal CounterFee { get; set; }
        public string Rate { get; set; }
    }
}

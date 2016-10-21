using System;
namespace EcShop.Entities.Orders
{
	public enum OrderStatus
	{
		All,
        WaitBuyerPay, //等待买家付款 1
        BuyerAlreadyPaid,//已付款,等待发货 2
        SellerAlreadySent,//已发货  3
        Closed,//已关闭 
        Finished,//订单已完成 5
        ApplyForRefund,//申请退款 6 
        ApplyForReturns,//申请退货 7
        ApplyForReplacement,//申请换货 8
        Refunded,//已退款 9 
        Returned,//已退货 10
        UnpackOrMixed= 98,//拆分或者已经合并的原单
        History = 99 //历史订单
	}

    public enum OrderHandleReasonType
    {
        /// <summary>
        /// 退款
        /// </summary>
        Refund,
        /// <summary>
        /// 退货
        /// </summary>
        Return
    }

    /// <summary>
    /// 订单关闭类型
    /// </summary>
    public enum CloseOrderType
    {
        /// <summary>
        /// 系统自动关闭（到期未付款）
        /// </summary>
        Auto,
        /// <summary>
        /// 用户手动关闭
        /// </summary>
        Manually
    }

    /// <summary>
    /// 订单海关对接状态
    /// </summary>
    public enum HSStatus
    {
        /// <summary>
        /// 未开始
        /// </summary>
        NotStarted,
        /// <summary>
        /// 进行中
        /// </summary>
        Underway,
        /// <summary>
        /// 已完成
        /// </summary>
        Accomplish,
        /// <summary>
        /// 失败
        /// </summary>
        BeDefeated,
        /// <summary>
        /// 等待重试
        /// </summary>
        WaitingForRetry
    }
}

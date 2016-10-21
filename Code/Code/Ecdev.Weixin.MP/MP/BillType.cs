using System;

namespace Ecdev.Weixin.MP
{
    //对账单类型
    public enum BillType
    {
        ALL,//返回当日所有订单信息
        SUCCESS,//返回当日成功支付的订单
        REFUND,//返回当日退款订单
        REVOKED//已撤销的订单
    }
}

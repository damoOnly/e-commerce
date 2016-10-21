using System;
namespace EcShop.Entities.Members
{
	public enum TradeTypes
	{
		NotSet,
		SelfhelpInpour,
		BackgroundAddmoney,
		Consume,
		DrawRequest,
		RefundOrder,
		ReferralDeduct,
		ReturnOrder,
        /// <summary>
        /// 建行接入充值
        /// </summary>
        CcbRecharge
	}
}

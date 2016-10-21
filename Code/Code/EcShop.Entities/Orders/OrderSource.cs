using System;
namespace EcShop.Entities.Orders
{
	public enum OrderSource
	{
		All,
		PC,
		Taobao,
		WeiXin,
		Wap,
        storeAdd, //后台添加门店订单
        Android=11,
        IOS=12
	}
}

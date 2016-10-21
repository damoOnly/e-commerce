using System;
namespace EcShop.Entities.Orders
{
    public enum UserStatus
	{
		DefaultStatus,//默认状态为 0
        RecycleDelete,//删除到订单回收站 1
        CompleteDelete //订单回收站彻底删除 2
	}
}

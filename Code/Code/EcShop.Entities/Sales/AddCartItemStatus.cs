using System;
namespace EcShop.Entities.Sales
{
	public enum AddCartItemStatus
	{
		Offsell,
		Shortage,
		InvalidUser,
		Successed = 8,
		ProductNotExists
	}
}

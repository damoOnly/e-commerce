using System;
namespace EcShop.SaleSystem.Vshop
{
	public class OrderException : Exception
	{
		public OrderException()
		{
		}
		public OrderException(string message) : base(message)
		{
		}
	}
}

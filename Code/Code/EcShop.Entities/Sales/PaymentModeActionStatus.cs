using System;
namespace EcShop.Entities.Sales
{
	public enum PaymentModeActionStatus
	{
		Success,
		DuplicateName,
		OutofNumber,
		DuplicateGateway,
		UnknowError = 99
	}
}

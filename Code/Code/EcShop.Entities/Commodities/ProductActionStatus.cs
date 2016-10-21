using System;
namespace EcShop.Entities.Commodities
{
	public enum ProductActionStatus
	{
		Success,
		DuplicateName,
		DuplicateSKU,
		SKUError,
		AttributeError,
		ProductTagEroor = 6,
		UnknowError = 99
	}
}

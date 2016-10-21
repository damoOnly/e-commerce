using System;
namespace EcShop.Entities.Promotions
{
	public enum CouponActionStatus
	{
		Success,
		DuplicateName,
		InvalidClaimCode,
		Disabled,
		OutOfTimes,
		OutOfExpiryDate,
		CreateClaimCodeSuccess,
		CreateClaimCodeError,
		UnknowError = 99
	}
}

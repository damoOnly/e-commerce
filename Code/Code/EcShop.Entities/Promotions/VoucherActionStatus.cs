using System;
namespace EcShop.Entities.Promotions
{
    public enum VoucherActionStatus
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

using System;
namespace EcShop.Membership.Core.Enums
{
	public enum LoginUserStatus
	{
		InvalidCredentials,
		Success,
		AccountPending,
		AccountLockedOut = 5,
		UnknownError = 100
	}
}

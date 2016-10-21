using System;
namespace EcShop.Membership.Core.Enums
{
	public enum CreateUserStatus
	{
		UnknownFailure,
		Created,
		DuplicateUsername,
		DuplicateEmailAddress,
		InvalidFirstCharacter,
		DisallowedUsername,
		Updated,
		Deleted,
		InvalidQuestionAnswer,
		InvalidPassword,
		InvalidEmail,
		InvalidUserName,
        DuplicateCellphone
	}
}

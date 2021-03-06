using EcShop.Membership.Core.Enums;
using System;
using System.Globalization;
namespace EcShop.Membership.Core
{
    using System.Web.Security;//修改1
	public static class HiMembership
	{
		public static MembershipUser Create(string username, string password, string email)
		{
			MembershipUser membershipUser = null;
			CreateUserStatus createUserStatus = CreateUserStatus.UnknownFailure;
			try
			{
				if (!string.IsNullOrEmpty(email))
				{
					membershipUser = Membership.CreateUser(username, password, email);
					createUserStatus = ((membershipUser == null) ? CreateUserStatus.UnknownFailure : CreateUserStatus.Created);
				}
				else
				{
					membershipUser = Membership.CreateUser(username, password);
					createUserStatus = ((membershipUser == null) ? CreateUserStatus.UnknownFailure : CreateUserStatus.Created);
				}
			}
			catch (MembershipCreateUserException ex)
			{
				createUserStatus = HiMembership.GetCreateUserStatus(ex.StatusCode);
			}
			catch (Exception ex2)
			{
				createUserStatus = HiMembership.GetCreateUserStatus(ex2);
			}
			if (createUserStatus != CreateUserStatus.Created)
			{
				throw new CreateUserException(createUserStatus);
			}
			return membershipUser;
		}
		public static MembershipUser GetUser(object userId)
		{
			return HiMembership.GetUser(userId, false);
		}
		public static MembershipUser GetUser(string username)
		{
			return HiMembership.GetUser(username, false);
		}
		public static MembershipUser GetUser(string username, bool userIsOnline)
		{
			return Membership.GetUser(username, userIsOnline);
		}
		public static MembershipUser GetUser(object userId, bool userIsOnline)
		{
			return Membership.GetUser(userId, userIsOnline);
		}
		public static CreateUserStatus GetCreateUserStatus(MembershipCreateStatus msc)
		{
			CreateUserStatus result;
			switch (msc)
			{
			case MembershipCreateStatus.Success:
				result = CreateUserStatus.Created;
				break;
			case MembershipCreateStatus.InvalidUserName:
				result = CreateUserStatus.InvalidUserName;
				break;
			case MembershipCreateStatus.InvalidPassword:
				result = CreateUserStatus.InvalidPassword;
				break;
			case MembershipCreateStatus.InvalidQuestion:
				result = CreateUserStatus.InvalidQuestionAnswer;
				break;
			case MembershipCreateStatus.InvalidAnswer:
				result = CreateUserStatus.InvalidQuestionAnswer;
				break;
			case MembershipCreateStatus.InvalidEmail:
				result = CreateUserStatus.InvalidEmail;
				break;
			case MembershipCreateStatus.DuplicateUserName:
				result = CreateUserStatus.DuplicateUsername;
				break;
			case MembershipCreateStatus.DuplicateEmail:
				result = CreateUserStatus.DuplicateEmailAddress;
				break;
			case MembershipCreateStatus.UserRejected:
				result = CreateUserStatus.DisallowedUsername;
				break;
			case MembershipCreateStatus.InvalidProviderUserKey:
				result = CreateUserStatus.UnknownFailure;
				break;
			case MembershipCreateStatus.DuplicateProviderUserKey:
				result = CreateUserStatus.UnknownFailure;
				break;
			case MembershipCreateStatus.ProviderError:
				result = CreateUserStatus.UnknownFailure;
				break;
			default:
				result = CreateUserStatus.UnknownFailure;
				break;
			}
			return result;
		}
		public static void Update(MembershipUser user)
		{
			if (user == null)
			{
				throw new Exception("Member can not be null");
			}
			Membership.UpdateUser(user);
		}
		public static bool Delete(string username)
		{
			return Membership.DeleteUser(username, true);
		}
		public static bool PasswordIsMembershipCompliant(string newPassword, out string errorMessage)
		{
			errorMessage = "";
			bool result;
			if (null == newPassword)
			{
				result = false;
			}
			else
			{
				int minRequiredPasswordLength = Membership.MinRequiredPasswordLength;
				int minRequiredNonAlphanumericCharacters = Membership.MinRequiredNonAlphanumericCharacters;
				if (newPassword.Length < minRequiredPasswordLength)
				{
					errorMessage = string.Format(CultureInfo.InvariantCulture, "密码太短，最少需要 {0} 个字符", new object[]
					{
						Membership.MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture)
					});
					result = false;
				}
				else
				{
					int num = 0;
					for (int i = 0; i < newPassword.Length; i++)
					{
						if (!char.IsLetterOrDigit(newPassword, i))
						{
							num++;
						}
					}
					if (num < minRequiredNonAlphanumericCharacters)
					{
						errorMessage = string.Format(CultureInfo.InvariantCulture, "密码包含的特殊字符太少, 最少要包含 {0} 个特殊字符", new object[]
						{
							Membership.MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture)
						});
						result = false;
					}
					else
					{
						result = true;
					}
				}
			}
			return result;
		}
		public static CreateUserStatus GetCreateUserStatus(Exception ex)
		{
			MembershipCreateUserException ex2 = ex as MembershipCreateUserException;
			CreateUserStatus result;
			if (ex2 != null)
			{
				result = HiMembership.GetCreateUserStatus(ex2.StatusCode);
			}
			else
			{
				result = CreateUserStatus.UnknownFailure;
			}
			return result;
		}
		public static bool ValidateUser(string username, string password)
		{
			return Membership.ValidateUser(username, password);
		}
		public static string GeneratePassword(int length, int alphaNumbericCharacters)
		{
			return Membership.GeneratePassword(length, alphaNumbericCharacters);
		}
	}
}

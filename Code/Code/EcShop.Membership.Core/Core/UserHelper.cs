using EcShop.Membership.Core.Enums;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
namespace EcShop.Membership.Core
{
    using System.Web.Security;//修改1
	public static class UserHelper
	{
		public static CreateUserStatus Create(HiMembershipUser userToCreate, string[] roles)
		{
			return UserHelper.Create(userToCreate, null, null, roles);
		}
		public static CreateUserStatus Create(HiMembershipUser userToCreate, string passwordQuestion, string passwordAnswer, string[] roles)
		{
			CreateUserStatus result;
			if (userToCreate == null)
			{
				result = CreateUserStatus.UnknownFailure;
			}
			else
			{
				MemberUserProvider memberUserProvider = MemberUserProvider.Instance();
				try
				{
					CreateUserStatus createUserStatus = memberUserProvider.CreateMembershipUser(userToCreate, passwordQuestion, passwordAnswer);
					if (createUserStatus == CreateUserStatus.Created)
					{
						Roles.AddUserToRoles(userToCreate.Username, roles);
					}
				}
				catch (CreateUserException ex)
				{
					result = ex.CreateUserStatus;
					return result;
				}
				result = CreateUserStatus.Created;
			}
			return result;
		}
		public static HiMembershipUser GetMembershipUser(int userId, string username, bool userIsOnline)
		{
			MemberUserProvider memberUserProvider = MemberUserProvider.Instance();
			return memberUserProvider.GetMembershipUser(userId, username, userIsOnline);
		}
		public static bool UpdateUser(HiMembershipUser user)
		{
			bool result;
			if (user == null)
			{
				result = false;
			}
			else
			{
				MemberUserProvider memberUserProvider = MemberUserProvider.Instance();
				result = memberUserProvider.UpdateMembershipUser(user);
			}
			return result;
		}
		public static LoginUserStatus ValidateUser(HiMembershipUser user)
		{
			LoginUserStatus result;
			if (user == null)
			{
				result = LoginUserStatus.UnknownError;
			}
			else
			{
				if (!user.IsApproved)
				{
					result = LoginUserStatus.AccountPending;
				}
				else
				{
					if (user.IsLockedOut)
					{
						result = LoginUserStatus.AccountLockedOut;
					}
					else
					{
						if (!HiMembership.ValidateUser(user.Username, user.Password))
						{
							result = LoginUserStatus.InvalidCredentials;
						}
						else
						{
							result = LoginUserStatus.Success;
						}
					}
				}
			}
			return result;
		}
		public static string CreateSalt()
		{
			byte[] array = new byte[16];
			new RNGCryptoServiceProvider().GetBytes(array);
			return Convert.ToBase64String(array);
		}
		public static string EncodePassword(MembershipPasswordFormat format, string cleanString, string salt)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(salt.ToLower() + cleanString);
			string result;
			switch (format)
			{
			case MembershipPasswordFormat.Clear:
				result = cleanString;
				break;
			case MembershipPasswordFormat.Hashed:
			{
				byte[] value = ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(bytes);
				result = BitConverter.ToString(value);
				break;
			}
			default:
			{
				byte[] value = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes);
				result = BitConverter.ToString(value);
				break;
			}
			}
			return result;
		}
		public static string GetUsernameWithOpenId(string openId, string openIdType)
		{
			return MemberUserProvider.Instance().GetUsernameWithOpenId(openId, openIdType);
		}
        public static int GetUserIdByUserSessionId(string sessionid)
        {
            return MemberUserProvider.Instance().GetUserIdByUserSessionId(sessionid);
        }
		public static int GetUserIdBySessionId(string sessionid)
		{
			return MemberUserProvider.Instance().GetUserIdBySessionId(sessionid);
		}
		public static string UpdateSessionId(int userId)
		{
			return MemberUserProvider.Instance().UpdateSessionId(userId);
		}
        public static int GetAssociatedSupplierId(int userId)//获取管理员所属的供货商
        {
            return MemberUserProvider.Instance().GetAssociatedSupplierId(userId);
        }

        public static int GetAssociatedStoreId(int userId)//获取管理员所属的门店
        {
            return MemberUserProvider.Instance().GetAssociatedStoreId(userId);
        }

        public static bool UpdateUserTopRegionId(int userId, int topRegionId)
        {
            return MemberUserProvider.Instance().UpdateUserTopRegionId(userId, topRegionId);
        }
		public static bool BindOpenId(string username, string openId, string openIdType)
		{
			return MemberUserProvider.Instance().BindOpenId(username, openId, openIdType);
		}
		public static int GetUserIdByOpenId(string openId)
		{
			return MemberUserProvider.Instance().GetUserIdByOpenId(openId);
		}
        public static int GetUserIdByCcbOpenId(string ccbOpenId)
        {
            return MemberUserProvider.Instance().GetUserIdByCcbOpenId(ccbOpenId);
        }
		public static int GetUserIdByAliPayOpenId(string openId)
		{
			return MemberUserProvider.Instance().GetUserIdByAliPayOpenId(openId);
		}
		public static bool IsExistUserName(string userName)
		{
			return MemberUserProvider.Instance().IsExistUserName(userName);
		}

        public static bool IsExistUserNameOpenid(string userName)
        {
            return MemberUserProvider.Instance().IsExistUserNameOpenid(userName);
        }

        public static bool IsExistOpendByUserName(string cellphone)
        {
            return MemberUserProvider.Instance().IsExistOpendByUserName(cellphone);
        }

        public static bool  IsExistCellPhone(string cellPhone)
        {
            return MemberUserProvider.Instance().IsExistCellPhone(cellPhone);
        }

        public static int IsExistCellPhoneAndUserName(string cellPhone)
        {
            return MemberUserProvider.Instance().IsExistCellPhoneAndUserName(cellPhone);
        }

        public static int IsExistIdentityCard(string identityCard, int userId)
        {
            return MemberUserProvider.Instance().IsExistIdentityCard(identityCard, userId);
        }

        public static int IsCheckCellPhoneAndUserName(string cellPhone, string userName)
        {
            return MemberUserProvider.Instance().IsCheckCellPhoneAndUserName(cellPhone,userName);
        }

        public static int IsCheckEmialAndUserName(string Emial, string userName)
        {
            return MemberUserProvider.Instance().IsCheckEmialAndUserName( Emial,userName);
        }

        public static int IsExistEmailAndUserName(string email)
        {
            return MemberUserProvider.Instance().IsExistEmailAndUserName(email);
        }

        public static bool IsExistEmal(string email, string username)
		{
            return MemberUserProvider.Instance().IsExistEmal(email, username);
		}
		public static int GetUserIdByEmail(string email)
		{
			return MemberUserProvider.Instance().GetUserIdByEmail(email);
		}

        public static int GetDefaultMemberGrade()
        {
            return MemberUserProvider.Instance().GetDefaultMemberGrade();
        }

        public static bool UpdateUserOpenId(int userId, int userCurrent, string openId)
        {
            return MemberUserProvider.Instance().UpdateUserOpenId(userId, userCurrent, openId);
        }

        public static bool UpdateUserUserNameByCellPhone(int userId, string username, string cellphone, string password, string openId, int passwordformat, string passwordsalt)
        {
            return MemberUserProvider.Instance().UpdateUserUserNameByCellPhone(userId, username, cellphone, password, openId, passwordformat, passwordsalt);
        }
        public static bool RegisterPCUser(int userId, int userCurrent, string password,string email,string userName )
        {
            return MemberUserProvider.Instance().RegisterPCUser(userId, userCurrent, password, email, userName);
        }
        public static void BindPCAccount(int userId, string openId,ref int result)
        {
             MemberUserProvider.Instance().BindPCAccount(userId, openId, ref result);
        }

        public static string GetOpenIdByUserName(string userName)
        {
            return MemberUserProvider.Instance().GetOpenIdByUserName(userName);
        }

        public static DataTable GetSwitchUsers(string openId)
        {
            return MemberUserProvider.Instance().GetSwitchUsers(openId); 
        }

        public static IUser CreateUsersMemberUsersInRoles(string openId, string passwordSalt, string realName, string headImg, int RegionId)
        {
            return MemberUserProvider.Instance().CreateUsersMemberUsersInRoles(openId, passwordSalt, realName, headImg, RegionId); 
        }

        public static bool CreateCcbUsersMemberUsersInRoles(string ccbOpenId, string mobile, string email, string passwordSalt, int provinceId, out IUser user)
        {
            return MemberUserProvider.Instance().CreateCcbUsersMemberUsersInRoles(ccbOpenId, mobile, email, passwordSalt, provinceId, out user);
        }

        public static IUser UpdateUsersCurrent(int nowUserId, int switchUserId)
        {
            return MemberUserProvider.Instance().UpdateUsersCurrent(nowUserId, switchUserId);
        }

        public static int GetToalCountByOpenId(string openId)
        {
            return MemberUserProvider.Instance().GetToalCountByOpenId(openId);
        }

        public static bool UpdateUserAvatar(int userId, string avatar)
        {
            return MemberUserProvider.Instance().UpdateUserAvatar(userId, avatar);
        }

        public static bool UpdateUser(int userId, string realName, string email, string cellphone, string qq, string oldEmail, string oldCellphone, bool emailVerification, bool cellPhoneVerification)
        {
            return MemberUserProvider.Instance().UpdateUser(userId, realName, email, cellphone, qq, oldEmail, oldCellphone, emailVerification, cellPhoneVerification);
        }


        public static bool UpdateUser(int userId, string realName, string email, string cellphone, string qq, string oldEmail, string oldCellphone, bool emailVerification, bool cellPhoneVerification,int gender)
        {
            return MemberUserProvider.Instance().UpdateUser(userId, realName, email, cellphone, qq, oldEmail, oldCellphone, emailVerification, cellPhoneVerification,gender);
        }

        public static bool UpdateUser(int userId, string realName, string email, string cellphone, string qq, string oldEmail, string oldCellphone, bool emailVerification, bool cellPhoneVerification, int gender, string IdNo, int IsVerify, DateTime? VerifyDate)
        {
            return MemberUserProvider.Instance().UpdateUser(userId, realName, email, cellphone, qq, oldEmail, oldCellphone, emailVerification, cellPhoneVerification, gender, IdNo, IsVerify, VerifyDate);
        }

        public static string GetUserAvatar(int userId)
        {
            return MemberUserProvider.Instance().GetUserAvatar(userId);
        }

        public static bool GetMemberInfo(int userId, out string userName, out string realName, out string email, out string cellphone, out string qq, out string avatar, out decimal balance, out decimal expenditure)
        {
            return MemberUserProvider.Instance().GetMemberInfo(userId, out userName, out realName, out email, out cellphone, out qq, out avatar, out balance, out expenditure);
        }

        public static string GenerateSalt()
        {
            byte[] array = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(array);
            return Convert.ToBase64String(array);
        }


        public static bool BindUsersMemberUsersInRoles(string openIdType, string openId, string passwordSalt, string realName, string avatar, long regionId, int? bindUserId, out IUser user)
        {
            return MemberUserProvider.Instance().BindUsersMemberUsersInRoles(openIdType, openId, passwordSalt, realName, avatar, regionId, bindUserId, out user);
        }
    }
}

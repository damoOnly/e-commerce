using EcShop.Core;
using EcShop.Membership.Core.Enums;
using System;
using System.Data;
namespace EcShop.Membership.Core
{
    public abstract class MemberUserProvider
    {
        private static readonly MemberUserProvider _defaultInstance;
        static MemberUserProvider()
        {
            MemberUserProvider._defaultInstance = (DataProviders.CreateInstance("EcShop.Membership.Data.UserData,EcShop.Membership.Data") as MemberUserProvider);
        }
        public static MemberUserProvider Instance()
        {
            return MemberUserProvider._defaultInstance;
        }
        public abstract CreateUserStatus CreateMembershipUser(HiMembershipUser userToCreate, string passwordQuestion, string passwordAnswer);
        public abstract HiMembershipUser GetMembershipUser(int userId, string username, bool isOnline);
        public abstract AnonymousUser GetAnonymousUser();
        public abstract bool UpdateMembershipUser(HiMembershipUser user);
        public abstract bool ValidatePasswordAnswer(string username, string answer);
        public abstract bool ChangePasswordQuestionAndAnswer(string username, string newQuestion, string newAnswer);
        public abstract string GetUsernameWithOpenId(string openId, string openIdType);
        public abstract bool BindOpenId(string username, string openId, string openIdType);
        public abstract int GetUserIdByUserSessionId(string sessionid);
        public abstract int GetUserIdBySessionId(string sessionid);
        public abstract string UpdateSessionId(int userId);
        public abstract int GetUserIdByOpenId(string openId);
        public abstract int GetUserIdByCcbOpenId(string ccbOpenId);
        public abstract int GetUserIdByAliPayOpenId(string openId);
        public abstract bool IsExistUserName(string username);
        public abstract bool IsExistUserNameOpenid(string username);
        public abstract bool IsExistOpendByUserName(string cellphone);
        public abstract bool IsExistCellPhone(string cellPhone);
        public abstract int IsExistCellPhoneAndUserName(string cellPhone);
        public abstract int IsExistIdentityCard(string identityCard, int userId);
        public abstract int IsCheckCellPhoneAndUserName(string cellPhone, string userName);

        public abstract int IsCheckEmialAndUserName(string Emial, string userName);

        public abstract int IsExistEmailAndUserName(string email);

        public abstract bool IsExistEmal(string email, string username);

        public abstract int GetUserIdByEmail(string email);
        public abstract int GetDefaultMemberGrade();
        public abstract bool UpdateUserOpenId(int userId, int userCurrent, string openId);
        public abstract bool UpdateUserUserNameByCellPhone(int userId, string username, string cellphone, string password, string openId, int passwordformat, string passwordsalt);
        public abstract bool RegisterPCUser(int userId, int userCurrent, string password, string email, string userName);
        public abstract string GetOpenIdByUserName(string userName);
        public abstract DataTable GetSwitchUsers(string openId);
        public abstract IUser CreateUsersMemberUsersInRoles(string openId, string passwordSalt, string realName, string headimg, int provinceId);
        public abstract bool CreateCcbUsersMemberUsersInRoles(string ccbOpenId, string mobile, string email, string passwordSalt, int provinceId, out IUser user);
        public abstract IUser UpdateUsersCurrent(int nowUserId, int switchUserId);
        public abstract int GetToalCountByOpenId(string openId);
        public abstract void BindPCAccount(int userId, string openId, ref int result);
        public abstract bool UpdateUserTopRegionId(int userId, int topRegionId);
        public abstract int GetAssociatedSupplierId(int userId);//获取管理员所属的供货商
        public abstract int GetAssociatedStoreId(int userId);//获取管理员所属的门店
        public abstract bool ResetPassword(string mobile, string decryptPassword, string passwordSalt);
        public abstract bool UpdateUserAvatar(int userId, string avatar);
        public abstract bool UpdateUser(int userId, string realName, string email, string cellphone, string qq, string oldEmail, string oldCellphone, bool emailVerification, bool cellPhoneVerification);


        public abstract bool UpdateUser(int userId, string realName, string email, string cellphone, string qq, string oldEmail, string oldCellphone, bool emailVerification, bool cellPhoneVerification,int gender);

        public abstract bool UpdateUser(int userId, string realName, string email, string cellphone, string qq, string oldEmail, string oldCellphone, bool emailVerification, bool cellPhoneVerification, int gender, string IdNo, int IsVerify, DateTime? VerifyDate);

        public abstract string GetUserAvatar(int userId);
        public abstract bool GetMemberInfo(int userId, out string userName, out string realName, out string email, out string cellphone, out string qq, out string avatar, out decimal balance, out decimal expenditure);

        public abstract bool BindUsersMemberUsersInRoles(string openIdType, string openId, string passwordSalt, string realName, string avatar, long provinceId, int? bindUserId, out IUser user);

    }
}

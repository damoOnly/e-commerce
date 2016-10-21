using EcShop.Core;
using EcShop.Core.Enums;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using System;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace EcShop.Membership.Context
{
    public static class Users
    {
        public static AnonymousUser GetAnonymousUser()
        {
            AnonymousUser anonymousUser = HiCache.Get("DataCache-AnonymousUser") as AnonymousUser;
            if (anonymousUser == null)
            {
                anonymousUser = MemberUserProvider.Instance().GetAnonymousUser();
                if (anonymousUser != null && anonymousUser.Username != null && anonymousUser.UserId > 0)
                {
                    HiCache.Insert("DataCache-AnonymousUser", anonymousUser, 120);
                }
            }
            return anonymousUser;
        }
        public static IUser GetUser()
        {
            IUser user = Users.GetUser(0, Users.GetLoggedOnUsername(), true, true);
            IUser result;
            if (user.IsAnonymous)
            {
                result = user;
            }
            else
            {
                ApplicationType applicationType = HiContext.Current.ApplicationType;
                if (applicationType == ApplicationType.Unknown)
                {
                    result = Users.GetAnonymousUser();
                }
                else
                {
                    if (applicationType == ApplicationType.Admin && user.UserRole != UserRole.SiteManager)
                    {
                        result = Users.GetAnonymousUser();
                    }
                    else
                    {
                        if ((applicationType == ApplicationType.Member || applicationType == ApplicationType.Common) && user.UserRole != UserRole.Member)
                        {
                            result = Users.GetAnonymousUser();
                        }
                        else
                        {
                            result = user;
                        }
                    }
                }
            }
            return result;
        }
        public static IUser GetContexUser()
        {
            IUser user = Users.GetUser(0, Users.GetLoggedOnUsername(), true, true);
            IUser result;
            if (user.IsAnonymous)
            {
                result = user;
            }
            else
            {
                ApplicationType applicationType = HiContext.Current.ApplicationType;
                if (applicationType == ApplicationType.Unknown)
                {
                    result = Users.GetAnonymousUser();
                }
                else
                {
                    result = user;
                }
            }
            return result;
        }
        public static IUser GetUser(int userId)
        {
            return Users.GetUser(userId, null, true, false);
        }
        public static IUser GetUserIdByUserSessionId(string sessionId)
        {
            int userIdBySessionId = UserHelper.GetUserIdByUserSessionId(sessionId);
            return Users.GetUser(userIdBySessionId);
        }


        /// <summary>
        ///  «∑Ò π”√cache
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="isCacheable"></param>
        /// <returns></returns>
        public static IUser GetUserIdByUserSessionId(string sessionId,bool isCacheable)
        {
            int userIdBySessionId = UserHelper.GetUserIdByUserSessionId(sessionId);
            return Users.GetUser(userIdBySessionId,isCacheable);
        }


        public static IUser GetUserBySessionId(string sessionId)
        {
            int userIdBySessionId = UserHelper.GetUserIdBySessionId(sessionId);
            return Users.GetUser(userIdBySessionId);
        }
        public static IUser GetUserByOpenId(string openId)
        {
            int userIdByOpenId = UserHelper.GetUserIdByOpenId(openId);
            return Users.GetUser(userIdByOpenId);
        }
        public static IUser GetUserByCcbOpenId(string ccbOpenId)
        {
            int userId = UserHelper.GetUserIdByCcbOpenId(ccbOpenId);
            return Users.GetUser(userId);
        }
        public static IUser GetUserByAliPayOpenId(string openId)
        {
            int userIdByAliPayOpenId = UserHelper.GetUserIdByAliPayOpenId(openId);
            return Users.GetUser(userIdByAliPayOpenId);
        }
        public static IUser GetUser(int userId, bool isCacheable)
        {
            return Users.GetUser(userId, null, isCacheable, false);
        }
        public static IUser GetUser(int userId, string username, bool isCacheable, bool userIsOnline)
        {
            IUser result;
            if (userId == 0 && !string.IsNullOrEmpty(username) && username.Equals("Anonymous", System.StringComparison.CurrentCultureIgnoreCase))
            {
                result = Users.GetAnonymousUser();
            }
            else
            {
                System.Collections.Hashtable hashtable = Users.UserCache();
                string key = (userId > 0) ? Users.UserKey(userId.ToString(System.Globalization.CultureInfo.InvariantCulture)) : Users.UserKey(username);
                if (isCacheable)
                {
                    IUser user = hashtable[key] as IUser;
                    if (user != null)
                    {
                        result = user;
                        return result;
                    }
                }
                HiMembershipUser membershipUser = UserHelper.GetMembershipUser(userId, username, userIsOnline);
                if (membershipUser == null)
                {
                    result = Users.GetAnonymousUser();
                }
                else
                {
                    UserFactory userFactory = UserFactory.Create(membershipUser.UserRole);
                    IUser user = userFactory.GetUser(membershipUser);
                    if (isCacheable)
                    {
                        hashtable[Users.UserKey(user.Username)] = user;
                        hashtable[Users.UserKey(user.UserId.ToString(System.Globalization.CultureInfo.InvariantCulture))] = user;
                    }
                    result = user;
                }
            }
            return result;
        }

        public static string GetLoggedOnUsername()
        {
            HttpContext current = HttpContext.Current;
            HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Vshop-Member"];
            string result;
            if (httpCookie != null)
            {
                result = Globals.UrlDecode(httpCookie.Value);
            }
            else
            {
                if (!current.User.Identity.IsAuthenticated || string.IsNullOrEmpty(current.User.Identity.Name))
                {
                    result = "Anonymous";
                }
                else
                {
                    result = current.User.Identity.Name;
                }
            }
            return result;
        }
        public static string GenerateSalt()
        {
            byte[] array = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(array);
            return Convert.ToBase64String(array);
        }
        public static string UserKey(object key)
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "User-{0}", new object[]
			{
				key
			}).ToLower(System.Globalization.CultureInfo.InvariantCulture);
        }
        public static System.Collections.Hashtable UserCache()
        {
            System.Collections.Hashtable hashtable = HiCache.Get("DataCache-UserLookuptable") as System.Collections.Hashtable;
            if (hashtable == null)
            {
                hashtable = new System.Collections.Hashtable();
                HiCache.Insert("DataCache-UserLookuptable", hashtable, 300);
            }
            return hashtable;
        }
        public static CreateUserStatus CreateUser(IUser user, string role)
        {
            return Users.CreateUser(user, new string[]
			{
				role
			});
        }
        public static CreateUserStatus CreateUser(IUser user, string[] roles)
        {
            CreateUserStatus createUserStatus = UserHelper.Create(user.MembershipUser, roles);
            if (createUserStatus == CreateUserStatus.Created)
            {
                UserFactory userFactory = UserFactory.Create(user.UserRole);
                if (!userFactory.Create(user))
                {
                    HiMembership.Delete(user.Username);
                    createUserStatus = CreateUserStatus.UnknownFailure;
                }
            }
            return createUserStatus;
        }
        public static IUser FindUserByUsername(string username)
        {
            return Users.GetUser(0, username, true, false);
        }
        public static bool UpdateUser(IUser user)
        {
            bool result;
            if (null == user)
            {
                result = false;
            }
            else
            {
                bool flag = UserHelper.UpdateUser(user.MembershipUser);
                if (flag)
                {
                    UserFactory userFactory = UserFactory.Create(user.UserRole);
                    flag = userFactory.UpdateUser(user);
                    HiContext current = HiContext.Current;
                    if (current.User.UserId == user.UserId)
                    {
                        current.User = user;
                    }
                }
                Users.ClearUserCache(user);
                result = flag;
            }
            return result;
        }
        public static void ClearUserCache(IUser user)
        {
            System.Collections.Hashtable hashtable = Users.UserCache();
            hashtable[Users.UserKey(user.UserId.ToString(System.Globalization.CultureInfo.InvariantCulture))] = null;
            hashtable[Users.UserKey(user.Username)] = null;
        }
        public static LoginUserStatus ValidateUser(IUser user)
        {
            return UserHelper.ValidateUser(user.MembershipUser);
        }
        public static bool ValidTradePassword(IUser user)
        {
            UserFactory userFactory = UserFactory.Create(user.UserRole);
            return userFactory.ValidTradePassword(user.Username, user.TradePassword);
        }

        public static bool UpdateUserAvatar(int userId, string avatar)
        {
            return UserHelper.UpdateUserAvatar(userId, avatar);
        }

        public static bool UpdateUser(int userId, string realName, string email, string cellphone, string qq, string oldEmail, string oldCellphone, bool emailVerification, bool cellPhoneVerification)
        {
            return UserHelper.UpdateUser(userId, realName, email, cellphone, qq, oldEmail, oldCellphone, emailVerification, cellPhoneVerification);
        }


        public static bool UpdateUser(int userId, string realName, string email, string cellphone, string qq, string oldEmail, string oldCellphone, bool emailVerification, bool cellPhoneVerification,int gender)
        {
            return UserHelper.UpdateUser(userId, realName, email, cellphone, qq, oldEmail, oldCellphone, emailVerification, cellPhoneVerification, gender);
        }

        public static bool UpdateUser(int userId, string realName, string email, string cellphone, string qq, string oldEmail, string oldCellphone, bool emailVerification, bool cellPhoneVerification, int gender, string IdNo, int IsVerify, DateTime? VerifyDate)
        {
            return UserHelper.UpdateUser(userId, realName, email, cellphone, qq, oldEmail, oldCellphone, emailVerification, cellPhoneVerification, gender,IdNo, IsVerify,VerifyDate);
        }

        



        public static string GetUserAvatar(int userId)
        {
            return UserHelper.GetUserAvatar(userId);
        }

        public static bool GetMemberInfo(int userId, out string userName, out string realName, out string email, out string cellphone, out string qq, out string avatar, out decimal balance, out decimal expenditure)
        {
            return UserHelper.GetMemberInfo(userId, out userName, out realName, out email, out cellphone, out qq, out avatar, out balance, out expenditure);
        }
    }
}

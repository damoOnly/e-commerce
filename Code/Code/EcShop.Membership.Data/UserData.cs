using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;


namespace EcShop.Membership.Data
{
    public class UserData : MemberUserProvider
    {
        private Database database;
        public UserData()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public override CreateUserStatus CreateMembershipUser(HiMembershipUser userToCreate, string passwordQuestion, string passwordAnswer)
        {
            CreateUserStatus createUserStatus = CreateUserStatus.UnknownFailure;
            CreateUserStatus result;
            if (userToCreate == null)
            {
                result = CreateUserStatus.UnknownFailure;
            }
            else
            {
                bool flag = false;
                if (!string.IsNullOrEmpty(passwordQuestion) && !string.IsNullOrEmpty(passwordAnswer))
                {
                    flag = true;
                    if (passwordAnswer.Length > 128 || passwordQuestion.Length > 256)
                    {
                        throw new CreateUserException(CreateUserStatus.InvalidQuestionAnswer);
                    }
                }
                MembershipUser membershipUser = HiMembership.Create(userToCreate.Username, userToCreate.Password, userToCreate.Email);
                if (membershipUser != null)
                {
                    userToCreate.UserId = (int)membershipUser.ProviderUserKey;
                    if (userToCreate.SupplierId > 0)//管理员和供货商关联
                    {
                        DbCommand sqlStringCommand1 = this.database.GetSqlStringCommand("insert into Ecshop_SupplierUser values(@supplierId,@userId)");
                        this.database.AddInParameter(sqlStringCommand1, "supplierId", DbType.Int32, userToCreate.SupplierId);
                        this.database.AddInParameter(sqlStringCommand1, "userId", DbType.Int32, userToCreate.UserId);
                        try
                        {
                            this.database.ExecuteNonQuery(sqlStringCommand1);
                        }
                        catch
                        {

                        }
                    }

                    if (userToCreate.StoreId > 0)//管理员和门店管理
                    {
                        DbCommand sql = this.database.GetSqlStringCommand("insert into Ecshop_StoreIdUser values(@StoreId,@UserId)");
                        this.database.AddInParameter(sql, "StoreId", DbType.Int32, userToCreate.StoreId);
                        this.database.AddInParameter(sql, "UserId", DbType.Int32, userToCreate.UserId);
                        try
                        {
                            this.database.ExecuteNonQuery(sql);
                        }
                        catch
                        {

                        }
                    }


                    System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET IsAnonymous = @IsAnonymous, IsApproved = @IsApproved, PasswordQuestion = @PasswordQuestion, PasswordAnswer = @PasswordAnswer, Gender = @Gender, BirthDate = @BirthDate, UserRole = @UserRole, OpenId = @OpenId, AliOpenId = @AliOpenId, SessionId = @SessionId,Name=@Name,UserType=@UserType WHERE UserId = @UserId");
                    this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userToCreate.UserId);
                    this.database.AddInParameter(sqlStringCommand, "IsAnonymous", System.Data.DbType.Boolean, userToCreate.IsAnonymous);
                    this.database.AddInParameter(sqlStringCommand, "IsApproved", System.Data.DbType.Boolean, userToCreate.IsApproved);
                    this.database.AddInParameter(sqlStringCommand, "Gender", System.Data.DbType.Int32, (int)userToCreate.Gender);
                    this.database.AddInParameter(sqlStringCommand, "BirthDate", System.Data.DbType.DateTime, null);
                    this.database.AddInParameter(sqlStringCommand, "UserRole", System.Data.DbType.Int32, (int)userToCreate.UserRole);
                    this.database.AddInParameter(sqlStringCommand, "PasswordQuestion", System.Data.DbType.String, null);
                    this.database.AddInParameter(sqlStringCommand, "PasswordAnswer", System.Data.DbType.String, null);
                    this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, userToCreate.OpenId);
                    this.database.AddInParameter(sqlStringCommand, "AliOpenId", System.Data.DbType.String, userToCreate.AliOpenId);
                    this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, userToCreate.Name);
                    this.database.AddInParameter(sqlStringCommand, "UserType", System.Data.DbType.Int32, (int)userToCreate.UserType);

                    if (!string.IsNullOrEmpty(userToCreate.SessionId))
                    {
                        this.database.AddInParameter(sqlStringCommand, "SessionId", System.Data.DbType.Guid, new Guid(userToCreate.SessionId));
                    }
                    else
                    {
                        this.database.AddInParameter(sqlStringCommand, "SessionId", System.Data.DbType.Guid, Guid.NewGuid());
                    }

                    if (userToCreate.BirthDate.HasValue)
                    {
                        this.database.SetParameterValue(sqlStringCommand, "BirthDate", userToCreate.BirthDate.Value);
                    }
                    if (flag)
                    {
                        string text = null;
                        try
                        {
                            int num;
                            int format;
                            string salt;
                            this.GetPasswordWithFormat(userToCreate.Username, false, out num, out format, out salt);
                            if (num == 0)
                            {
                                text = UserHelper.EncodePassword((MembershipPasswordFormat)format, passwordAnswer, salt);
                                this.database.SetParameterValue(sqlStringCommand, "PasswordQuestion", passwordQuestion);
                                this.database.SetParameterValue(sqlStringCommand, "PasswordAnswer", text);
                            }
                            if (num != 0 || (!string.IsNullOrEmpty(text) && text.Length > 128))
                            {
                                HiMembership.Delete(userToCreate.Username);
                                throw new CreateUserException(CreateUserStatus.InvalidQuestionAnswer);
                            }
                        }
                        catch
                        {
                            HiMembership.Delete(userToCreate.Username);
                            throw new CreateUserException(CreateUserStatus.UnknownFailure);
                        }
                    }
                    if (this.database.ExecuteNonQuery(sqlStringCommand) != 1)
                    {
                        HiMembership.Delete(userToCreate.Username);
                        throw new CreateUserException(createUserStatus);
                    }
                    createUserStatus = CreateUserStatus.Created;
                }
                result = createUserStatus;
            }
            return result;
        }
        public override bool UpdateMembershipUser(HiMembershipUser user)
        {
            bool result;
            if (user == null)
            {
                result = false;
            }
            else
            {
                try
                {
                    HiMembership.Update(user.Membership);
                }
                catch
                {
                    result = false;
                    return result;
                }
                System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET MobilePIN = @MobilePIN, Gender = @Gender, BirthDate = @BirthDate, OpenId = @OpenId, AliOpenId = @AliOpenId WHERE UserId = @UserId");
                this.database.AddInParameter(sqlStringCommand, "MobilePIN", System.Data.DbType.String, user.MobilePIN);
                this.database.AddInParameter(sqlStringCommand, "Gender", System.Data.DbType.Int32, (int)user.Gender);
                this.database.AddInParameter(sqlStringCommand, "BirthDate", System.Data.DbType.DateTime, user.BirthDate);
                this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, user.UserId);
                this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, user.OpenId);
                this.database.AddInParameter(sqlStringCommand, "AliOpenId", System.Data.DbType.String, user.AliOpenId);
                result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
            }
            return result;
        }

        public override bool ResetPassword(string mobile, string password, string passwordSalt)
        {
            bool result;
            try
            {
                string pwd = new EcShop.Membership.ASPNETProvider.SqlMembershipProvider().GenericPassword(password, 1, passwordSalt);
                System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET [Password] = @Password,PasswordSalt=@PasswordSalt WHERE UserName = @UserName");
                this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, mobile);
                this.database.AddInParameter(sqlStringCommand, "Password", System.Data.DbType.String, pwd);
                this.database.AddInParameter(sqlStringCommand, "PasswordSalt", System.Data.DbType.String, passwordSalt);
                result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
            }
            catch
            {
                result = false;
                return result;
            }
            return result;
        }


        public override int GetAssociatedSupplierId(int userId)//获取管理员所属的供货商
        {
            int ret = 0;
            Database database = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT TOP 1 SupplierId from Ecshop_SupplierUser where UserId =@UserId");
            database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
            object obj = database.ExecuteScalar(sqlStringCommand);
            if (obj != null && obj != DBNull.Value)
            {
                ret = (int)obj;
            }
            return ret;

        }

        public override int GetAssociatedStoreId(int userId)//获取管理员所属的门店
        {
            int ret = 0;
            Database database = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT TOP 1 StoreId from Ecshop_StoreIdUser where UserId =@UserId");
            database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
            object obj = database.ExecuteScalar(sqlStringCommand);
            if (obj != null && obj != DBNull.Value)
            {
                ret = (int)obj;
            }
            return ret;

        }

        public override HiMembershipUser GetMembershipUser(int userId, string username, bool isOnline)
        {
            MembershipUser membershipUser = string.IsNullOrEmpty(username) ? HiMembership.GetUser(userId, isOnline) : HiMembership.GetUser(username, isOnline);
            HiMembershipUser result;
            if (membershipUser == null)
            {
                result = null;
            }
            else
            {
                HiMembershipUser hiMembershipUser = null;
                System.Data.Common.DbCommand sqlStringCommand;
                if (!string.IsNullOrEmpty(username))
                {
                    sqlStringCommand = this.database.GetSqlStringCommand("SELECT MobilePIN, IsAnonymous, Gender, BirthDate, UserRole, OpenId, AliOpenId,UserId,SessionId,HeadImgUrl FROM aspnet_Users WHERE LoweredUserName = LOWER(@Username)");
                    this.database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, username);
                }
                else
                {
                    sqlStringCommand = this.database.GetSqlStringCommand("SELECT MobilePIN, IsAnonymous, Gender, BirthDate, UserRole, OpenId, AliOpenId,SessionId,HeadImgUrl FROM aspnet_Users WHERE UserId = @UserId");
                    this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
                }
                using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    if (dataReader.Read())
                    {
                        hiMembershipUser = new HiMembershipUser((bool)dataReader["IsAnonymous"], (UserRole)Convert.ToInt32(dataReader["UserRole"]), membershipUser);
                        if (dataReader["MobilePIN"] != DBNull.Value)
                        {
                            hiMembershipUser.MobilePIN = (string)dataReader["MobilePIN"];
                        }
                        if (dataReader["Gender"] != DBNull.Value)
                        {
                            hiMembershipUser.Gender = (Gender)Convert.ToInt32(dataReader["Gender"]);
                        }
                        if (dataReader["BirthDate"] != DBNull.Value)
                        {
                            hiMembershipUser.BirthDate = new DateTime?((DateTime)dataReader["BirthDate"]);
                        }
                        if (dataReader["OpenId"] != DBNull.Value)
                        {
                            hiMembershipUser.OpenId = (string)dataReader["OpenId"];
                        }
                        if (dataReader["AliOpenId"] != DBNull.Value)
                        {
                            hiMembershipUser.AliOpenId = (string)dataReader["AliOpenId"];
                        }
                        if (dataReader["SessionId"] != DBNull.Value)
                        {
                            hiMembershipUser.SessionId = dataReader["SessionId"].ToString();
                        }

                        if (dataReader["HeadImgUrl"] != DBNull.Value)
                        {
                            hiMembershipUser.HeadImgUrl = (string)dataReader["HeadImgUrl"];
                        }
                    }
                    dataReader.Close();
                }
                result = hiMembershipUser;
            }
            return result;
        }
        public override AnonymousUser GetAnonymousUser()
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT @UserId = UserId FROM aspnet_Users WHERE IsAnonymous = 1");
            this.database.AddOutParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, 4);
            this.database.ExecuteNonQuery(sqlStringCommand);
            int userId = (int)this.database.GetParameterValue(sqlStringCommand, "UserId");
            HiMembershipUser membershipUser = this.GetMembershipUser(userId, "Anonymous", true);
            return new AnonymousUser(membershipUser);
        }
        public override bool ValidatePasswordAnswer(string username, string answer)
        {
            int num;
            int format;
            string salt;
            this.GetPasswordWithFormat(username, true, out num, out format, out salt);
            bool result;
            if (num != 0)
            {
                result = false;
            }
            else
            {
                string value = UserHelper.EncodePassword((MembershipPasswordFormat)format, answer, salt);
                System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName AND (PasswordAnswer = @PasswordAnswer OR (PasswordQuestion IS NULL AND PasswordAnswer IS NULL))");
                this.database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, username);
                this.database.AddInParameter(sqlStringCommand, "PasswordAnswer", System.Data.DbType.String, value);
                object obj = this.database.ExecuteScalar(sqlStringCommand);
                result = (obj != null && obj != DBNull.Value);
            }
            return result;
        }
        public override bool ChangePasswordQuestionAndAnswer(string username, string newQuestion, string newAnswer)
        {
            int num;
            int format;
            string salt;
            this.GetPasswordWithFormat(username, true, out num, out format, out salt);
            bool result;
            if (num != 0)
            {
                result = false;
            }
            else
            {
                string text = UserHelper.EncodePassword((MembershipPasswordFormat)format, newAnswer, salt);
                if (text.Length > 128)
                {
                    result = false;
                }
                else
                {
                    System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET PasswordQuestion = @PasswordQuestion, PasswordAnswer = @PasswordAnswer WHERE LOWER(@Username) = LoweredUserName");
                    this.database.AddInParameter(sqlStringCommand, "PasswordQuestion", System.Data.DbType.String, newQuestion);
                    this.database.AddInParameter(sqlStringCommand, "PasswordAnswer", System.Data.DbType.String, text);
                    this.database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, username);
                    result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
                }
            }
            return result;
        }
        private void GetPasswordWithFormat(string username, bool updateLastLoginActivityDate, out int status, out int passwordFormat, out string passwordSalt)
        {
            passwordFormat = 0;
            passwordSalt = null;
            status = -1;
            System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("aspnet_Membership_GetPasswordWithFormat");
            this.database.AddInParameter(storedProcCommand, "UserName", System.Data.DbType.String, username);
            this.database.AddInParameter(storedProcCommand, "UpdateLastLoginActivityDate", System.Data.DbType.Boolean, updateLastLoginActivityDate);
            this.database.AddInParameter(storedProcCommand, "CurrentTime", System.Data.DbType.DateTime, DateTime.Now);
            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                if (dataReader.Read())
                {
                    passwordFormat = dataReader.GetInt32(1);
                    passwordSalt = dataReader.GetString(2);
                    status = 0;
                }
            }
        }
        private void GetPasswordWithFormat(string username, bool updateLastLoginActivityDate, out int status, out string password, out int passwordFormat, out string passwordSalt, out int failedPasswordAttemptCount, out int failedPasswordAnswerAttemptCount, out bool isApproved, out DateTime lastLoginDate, out DateTime lastActivityDate)
        {
            password = null;
            passwordFormat = 0;
            passwordSalt = null;
            failedPasswordAttemptCount = 0;
            failedPasswordAnswerAttemptCount = 0;
            isApproved = false;
            lastLoginDate = DateTime.Now;
            lastActivityDate = DateTime.Now;
            status = -1;
            System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("aspnet_Membership_GetPasswordWithFormat");
            this.database.AddInParameter(storedProcCommand, "UserName", System.Data.DbType.String, username);
            this.database.AddInParameter(storedProcCommand, "UpdateLastLoginActivityDate", System.Data.DbType.Boolean, updateLastLoginActivityDate);
            this.database.AddInParameter(storedProcCommand, "CurrentTime", System.Data.DbType.DateTime, DateTime.Now);
            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                if (dataReader.Read())
                {
                    password = dataReader.GetString(0);
                    passwordFormat = dataReader.GetInt32(1);
                    passwordSalt = dataReader.GetString(2);
                    failedPasswordAttemptCount = dataReader.GetInt32(3);
                    failedPasswordAnswerAttemptCount = dataReader.GetInt32(4);
                    isApproved = dataReader.GetBoolean(5);
                    lastLoginDate = dataReader.GetDateTime(6);
                    lastActivityDate = dataReader.GetDateTime(7);
                    status = 0;
                }
            }
        }
        public override bool BindOpenId(string username, string openId, string openIdType)
        {
            System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("aspnet_OpenId_Bind");
            this.database.AddInParameter(storedProcCommand, "UserName", System.Data.DbType.String, username);
            this.database.AddInParameter(storedProcCommand, "OpenId", System.Data.DbType.String, openId);
            this.database.AddInParameter(storedProcCommand, "OpenIdType", System.Data.DbType.String, openIdType);
            return this.database.ExecuteNonQuery(storedProcCommand) == 1;
        }
        public override string GetUsernameWithOpenId(string openId, string openIdType)
        {
            string result = null;
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserName FROM aspnet_Users WHERE LOWER(OpenId)=LOWER(@OpenId) AND LOWER(OpenIdType)=LOWER(@OpenIdType)");
            this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, openId);
            this.database.AddInParameter(sqlStringCommand, "OpenIdType", System.Data.DbType.String, openIdType);
            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    result = dataReader.GetString(0);
                }
            }
            return result;
        }
        public override int GetUserIdByUserSessionId(string sessionId)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId FROM aspnet_Users WHERE  SessionId=@SessionId");
            this.database.AddInParameter(sqlStringCommand, "SessionId", System.Data.DbType.String, sessionId);
            int result;
            try
            {
                result = (int)this.database.ExecuteScalar(sqlStringCommand);
            }
            catch
            {
                result = 0;
            }
            return result;
        }
        public override int GetUserIdBySessionId(string sessionid)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId FROM aspnet_Members WHERE  SessionId=@SessionId");
            this.database.AddInParameter(sqlStringCommand, "SessionId", System.Data.DbType.String, sessionid);
            int result;
            try
            {
                result = (int)this.database.ExecuteScalar(sqlStringCommand);
            }
            catch
            {
                result = 0;
            }
            return result;
        }
        public override string UpdateSessionId(int userId)
        {
            string generateId = Globals.GetGenerateId();
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update aspnet_Members set SessionId=@SessionId WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "SessionId", System.Data.DbType.String, generateId);
            string result;
            if (this.database.ExecuteNonQuery(sqlStringCommand) > 0)
            {
                result = generateId;
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }
        public override int GetUserIdByOpenId(string openId)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId FROM aspnet_Users WHERE  OpenId = @OpenId AND UserCurrent = 1");
            this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, openId);
            int result;
            try
            {
                result = (int)this.database.ExecuteScalar(sqlStringCommand);
            }
            catch
            {
                result = 0;
            }
            return result;
        }
        public override int GetUserIdByCcbOpenId(string ccbOpenId)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId FROM aspnet_Users WHERE  CcbOpenId = @CcbOpenId");
            this.database.AddInParameter(sqlStringCommand, "@CcbOpenId", System.Data.DbType.String, ccbOpenId);
            int result;
            try
            {
                result = (int)this.database.ExecuteScalar(sqlStringCommand);
            }
            catch
            {
                result = 0;
            }
            return result;
        }
        public override int GetUserIdByAliPayOpenId(string openId)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId FROM aspnet_Users WHERE  AliOpenId = @AliOpenId");
            this.database.AddInParameter(sqlStringCommand, "AliOpenId", System.Data.DbType.String, openId);
            int result;
            try
            {
                result = (int)this.database.ExecuteScalar(sqlStringCommand);
            }
            catch
            {
                result = 0;
            }
            return result;
        }
        public override bool IsExistUserName(string username)
        {
            string sql = @"SELECT UserName FROM aspnet_Users WHERE UserName=@UserName ";
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, username);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            return obj != null ? true :false;
        }

        public override bool IsExistUserNameOpenid(string username)
        {
            string sql = @"SELECT UserName FROM aspnet_Users WHERE UserName=@UserName and OpenId is not null";
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, username);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            return obj != null ? true : false;
        }
        public override bool IsExistOpendByUserName(string cellphone)
        {
            string sql = @"SELECT OpenId FROM aspnet_Users WHERE UserName=@UserName ";
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, cellphone);
            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    if (dataReader["OpenId"] != DBNull.Value)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }               
            }
            return false;
        }

        public override bool IsExistCellPhone(string cellPhone)
        {
            bool result = false;
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CellPhone FROM aspnet_Members WHERE CellPhone=@CellPhone");
            this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, cellPhone);
            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    result = true;
                }
            }
            return result;
        }

        public override int IsExistCellPhoneAndUserName(string cellPhone)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(1) Qty FROM aspnet_Users WHERE LoweredUserName=@UserName UNION ALL SELECT COUNT(1) Qty FROM aspnet_Members WHERE CellPhone=@CellPhone");
            this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, cellPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, cellPhone);
            int result = 0;
            try
            {
                result = (int)this.database.ExecuteScalar(sqlStringCommand);
            }
            catch
            {
                result = 0;
            }
            return result;
        }


        /// <summary>
        /// 该身份证号码已认证，无法保存。
        /// </summary>
        /// <param name="identityCard"></param>
        /// <returns></returns>
        public override int IsExistIdentityCard(string identityCard,int userId)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select COUNT(1) from aspnet_Members where IsVerify = 1 and lower(IdentityCard)=@IdentityCard and UserId<>@UserId");
            this.database.AddInParameter(sqlStringCommand, "IdentityCard", System.Data.DbType.String, identityCard.ToLower());
            this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
            int result = 0;
            try
            {
                result = (int)this.database.ExecuteScalar(sqlStringCommand);
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public override int IsCheckCellPhoneAndUserName(string cellPhone, string userName)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("  SELECT COUNT(1) FROM aspnet_Members A  inner join aspnet_Users B on A.[UserId]=B.[UserId]  where B.[LoweredUserName]=@UserName and A.[CellPhone]=@cellPhone and A.[CellPhoneVerification]=1");
            this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, userName);
            this.database.AddInParameter(sqlStringCommand, "cellPhone", System.Data.DbType.String, cellPhone);
            int result = 0;
            try
            {
                result = (int)this.database.ExecuteScalar(sqlStringCommand);
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public override int IsCheckEmialAndUserName(string Emial, string userName)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("   SELECT COUNT(1) FROM aspnet_Members A  inner join aspnet_Users B on A.[UserId]=B.[UserId]  where B.[UserName]=@UserName and B.[Email]=@Emial and A.[EmailVerification]=1");
            this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, userName);
            this.database.AddInParameter(sqlStringCommand, "Emial", System.Data.DbType.String, Emial);
            int result = 0;
            try
            {
                result = (int)this.database.ExecuteScalar(sqlStringCommand);
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public override int IsExistEmailAndUserName(string email)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(1) FROM aspnet_Users WHERE LoweredUserName=@UserName or LoweredEmail=@Email");
            this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, email);
            this.database.AddInParameter(sqlStringCommand, "Email", System.Data.DbType.String, email);
            int result = 0;
            try
            {
                result = (int)this.database.ExecuteScalar(sqlStringCommand);
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public override bool IsExistEmal(string email, string username)
        {
            bool result = false;
            StringBuilder strql = new StringBuilder("select Email from aspnet_Users where Email=@Email");

            if (!string.IsNullOrEmpty(username))
            {
                strql.Append(" and UserName <> @UserName");
            }

            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strql.ToString());
            this.database.AddInParameter(sqlStringCommand, "Email", System.Data.DbType.String, email);

            if (!string.IsNullOrEmpty(username))
            {
                this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, username);
            }

            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    result = true;
                }
            }
            return result;
        }
        public override int GetUserIdByEmail(string email)
        {
            int result;
            if (string.IsNullOrEmpty(email))
            {
                result = 0;
            }
            else
            {
                email = email.ToLower();
                int num = 0;
                System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select userId from aspnet_Users where LOWER(Email)=@Email");
                this.database.AddInParameter(sqlStringCommand, "Email", System.Data.DbType.String, email);
                using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    if (dataReader.Read())
                    {
                        num = Convert.ToInt32(dataReader["userId"].ToString());
                    }
                }
                result = num;
            }
            return result;
        }

        public override int GetDefaultMemberGrade()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT GradeId FROM aspnet_MemberGrades WHERE IsDefault = 1");
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            int result;
            if (obj != null && obj != DBNull.Value)
            {
                result = (int)obj;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 绑定PC端账号
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userCurrent"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public override bool UpdateUserOpenId(int userId, int userCurrent, string openId)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update aspnet_Users set UserCurrent=@UserCurrent,OpenId=@OpenId WHERE UserId=@UserId;Update aspnet_Members set OpenId=@OpenId WHERE UserId=@UserId;");
            this.database.AddInParameter(sqlStringCommand, "UserCurrent", System.Data.DbType.Int32, userCurrent);
            this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, openId);
            this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
            bool result = false;
            if (this.database.ExecuteNonQuery(sqlStringCommand) > 0)
            {
                result = true;
            }
            return result;
        }

        public override bool UpdateUserUserNameByCellPhone(int userId, string username,string cellphone, string password, string openId, int passwordformat, string passwordsalt)
        {
            string newpass = new EcShop.Membership.ASPNETProvider.SqlMembershipProvider().GenericPassword(password, passwordformat, passwordsalt);
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update aspnet_Users set Password=@Password,PasswordFormat=@PasswordFormat,PasswordSalt=@PasswordSalt,UserName = @UserName,LoweredUsername=@LoweredUsername WHERE UserId=@UserId and OpenId=@OpenId;update aspnet_members set CellPhone=@CellPhone,CellPhoneVerification=1 where UserId=@UserId and OpenId=@OpenId");
            this.database.AddInParameter(sqlStringCommand, "Password", System.Data.DbType.String, newpass);
            this.database.AddInParameter(sqlStringCommand, "PasswordFormat", System.Data.DbType.String, (int)passwordformat);
            this.database.AddInParameter(sqlStringCommand, "PasswordSalt", System.Data.DbType.String, passwordsalt);
            this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, openId);
            this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, username);
            this.database.AddInParameter(sqlStringCommand, "LoweredUsername", System.Data.DbType.String, username);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, cellphone);
            bool result = false;
            if (this.database.ExecuteNonQuery(sqlStringCommand) > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 通过微信注册PC端账号
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userCurrent"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override bool RegisterPCUser(int userId, int userCurrent, string password, string email, string userName)
        {
            string passwordsalt = GenerateSalt();
            string newpassword = new EcShop.Membership.ASPNETProvider.SqlMembershipProvider().GenericPassword(password, 1, passwordsalt);
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update aspnet_Users set UserCurrent=@UserCurrent,Password=@Password,Email=@Email,UserName=@UserName,LoweredUserName=@LoweredUserName,PasswordSalt=@PasswordSalt,LoweredEmail=@LoweredEmail WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserCurrent", System.Data.DbType.Int32, userCurrent);
            this.database.AddInParameter(sqlStringCommand, "Password", System.Data.DbType.String, newpassword);
            this.database.AddInParameter(sqlStringCommand, "Email", System.Data.DbType.String, email);
            this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, userName);
            this.database.AddInParameter(sqlStringCommand, "PasswordSalt", System.Data.DbType.String, passwordsalt);
            this.database.AddInParameter(sqlStringCommand, "LoweredUserName", System.Data.DbType.String, userName.ToLower());
            this.database.AddInParameter(sqlStringCommand, "LoweredEmail", System.Data.DbType.String, email.ToLower());

            this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
            bool result = false;
            if (this.database.ExecuteNonQuery(sqlStringCommand) > 0)
            {
                result = true;
            }
            return result;
        }



        /// <summary>
        /// 绑定PC端账号
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="openId"></param>
        /// <param name="result"></param>
        public override void BindPCAccount(int userId, string openId, ref int result)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("BindPCAccount");

            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(storedProcCommand, "OpenId", DbType.String, openId);
            this.database.AddOutParameter(storedProcCommand, "Result", DbType.Int32, 4);

            result = (int)storedProcCommand.Parameters[2].Value;
        }


        public override string GetOpenIdByUserName(string userName)
        {

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OpenId FROM aspnet_Users WHERE UserName=@UserName");
            this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, userName);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            string result = "";
            if (obj != null && obj != DBNull.Value)
            {
                result = obj.ToString();
            }
            return result;
        }

        public override DataTable GetSwitchUsers(string openId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select b.RealName,isnull(a.UserCurrent,0) UserCurrent,a.UserId,a.UserType,a.HeadImgUrl,a.UserName,a.Email  from  dbo.[aspnet_Users] a join aspnet_Members b on a.UserId = b.UserId where a.OpenId=@OpenId");
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
            DataTable result = new DataTable();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        /// <summary>
        /// 注册用户
        /// </summary>
        /// <returns></returns>
        public override IUser CreateUsersMemberUsersInRoles(string openId, string passwordSalt, string realName, string headimg, int provinceId)
        {
            Member member = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("Users_Member_UsersInRoles_Create");
            this.database.AddInParameter(storedProcCommand, "OpenId", DbType.String, openId);
            this.database.AddInParameter(storedProcCommand, "PasswordSalt", DbType.String, passwordSalt);
            this.database.AddInParameter(storedProcCommand, "RealName", DbType.String, realName);
            this.database.AddInParameter(storedProcCommand, "HeadImgUrl", DbType.String, headimg);
            this.database.AddInParameter(storedProcCommand, "provinceId", DbType.Int32, provinceId);

            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                if (dataReader.Read())
                {
                    member = new Member(UserRole.Member);
                    member.GradeId = (int)dataReader["GradeId"];
                    if (dataReader["ReferralUserId"] != DBNull.Value)
                    {
                        member.ReferralUserId = new int?((int)dataReader["ReferralUserId"]);
                    }
                    member.ReferralStatus = (int)dataReader["ReferralStatus"];
                    if (dataReader["ReferralReason"] != DBNull.Value)
                    {
                        member.ReferralReason = (string)dataReader["ReferralReason"];
                    }
                    if (dataReader["ReferralRequetsDate"] != DBNull.Value)
                    {
                        member.ReferralRequetsDate = new DateTime?((DateTime)dataReader["ReferralRequetsDate"]);
                    }
                    if (dataReader["RefusalReason"] != DBNull.Value)
                    {
                        member.RefusalReason = (string)dataReader["RefusalReason"];
                    }
                    member.IsOpenBalance = (bool)dataReader["IsOpenBalance"];
                    member.TradePassword = (string)dataReader["TradePassword"];
                    member.TradePasswordFormat = (MembershipPasswordFormat)((int)dataReader["TradePasswordFormat"]);
                    member.OrderNumber = (int)dataReader["OrderNumber"];
                    member.Expenditure = (decimal)dataReader["Expenditure"];
                    member.Points = (int)dataReader["Points"];
                    member.Balance = (decimal)dataReader["Balance"];
                    member.RequestBalance = (decimal)dataReader["RequestBalance"];
                    member.EmailVerification = (bool)dataReader["EmailVerification"];
                    member.CellPhoneVerification = (bool)dataReader["CellPhoneVerification"];
                    if (dataReader["TopRegionId"] != DBNull.Value)
                    {
                        member.TopRegionId = (int)dataReader["TopRegionId"];
                    }
                    if (dataReader["RegionId"] != DBNull.Value)
                    {
                        member.RegionId = (int)dataReader["RegionId"];
                    }
                    if (dataReader["RealName"] != DBNull.Value)
                    {
                        member.RealName = (string)dataReader["RealName"];
                    }
                    if (dataReader["IdentityCard"] != DBNull.Value)
                    {
                        member.IdentityCard = (string)dataReader["IdentityCard"];
                    }
                    if (dataReader["Address"] != DBNull.Value)
                    {
                        member.Address = (string)dataReader["Address"];
                    }
                    if (dataReader["Zipcode"] != DBNull.Value)
                    {
                        member.Zipcode = (string)dataReader["Zipcode"];
                    }
                    if (dataReader["TelPhone"] != DBNull.Value)
                    {
                        member.TelPhone = (string)dataReader["TelPhone"];
                    }
                    if (dataReader["CellPhone"] != DBNull.Value)
                    {
                        member.CellPhone = (string)dataReader["CellPhone"];
                    }
                    if (dataReader["QQ"] != DBNull.Value)
                    {
                        member.QQ = (string)dataReader["QQ"];
                    }
                    if (dataReader["Wangwang"] != DBNull.Value)
                    {
                        member.Wangwang = (string)dataReader["Wangwang"];
                    }
                    if (dataReader["MSN"] != DBNull.Value)
                    {
                        member.MSN = (string)dataReader["MSN"];
                    }
                    if (dataReader["WeChat"] != DBNull.Value)
                    {
                        member.WeChat = (string)dataReader["WeChat"];
                    }
                    if (dataReader["VipCardNumber"] != DBNull.Value)
                    {
                        member.VipCardNumber = (string)dataReader["VipCardNumber"];
                    }
                    if (dataReader["SessionId"] != DBNull.Value)
                    {
                        member.SessionId = (string)dataReader["SessionId"];
                    }
                    if (dataReader["OpenId"] != DBNull.Value)
                    {
                        member.OpenId = (string)dataReader["OpenId"];
                    }
                    if (dataReader["UserId"] != DBNull.Value)
                    {
                        member.UserId = (int)dataReader["UserId"];
                    }
                    if (dataReader["UserName"] != DBNull.Value)
                    {
                        member.Username = (string)dataReader["UserName"];
                    }
                }
            }
            return member;
        }

        /// <summary>
        /// 注册CCB用户
        /// </summary>
        /// <param name="ccbOpenId"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <param name="passwordSalt"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public override bool CreateCcbUsersMemberUsersInRoles(string ccbOpenId, string mobile, string email, string passwordSalt, int provinceId, out IUser user)
        {
            Member member = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("CcbUsers_Member_UsersInRoles_Create");
            this.database.AddInParameter(storedProcCommand, "@CcbOpenId", DbType.String, ccbOpenId);
            this.database.AddInParameter(storedProcCommand, "@Mobile", DbType.String, mobile);
            this.database.AddInParameter(storedProcCommand, "@Email", DbType.String, email);
            this.database.AddInParameter(storedProcCommand, "@PasswordSalt", DbType.String, passwordSalt);
            this.database.AddInParameter(storedProcCommand, "@provinceId", DbType.Int32, provinceId);
            this.database.AddOutParameter(storedProcCommand, "@IsExists", DbType.Boolean, sizeof(bool));

            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {                
                if (dataReader.Read())
                {
                    member = new Member(UserRole.Member);
                    member.GradeId = (int)dataReader["GradeId"];
                    if (dataReader["ReferralUserId"] != DBNull.Value)
                    {
                        member.ReferralUserId = new int?((int)dataReader["ReferralUserId"]);
                    }
                    member.ReferralStatus = (int)dataReader["ReferralStatus"];
                    if (dataReader["ReferralReason"] != DBNull.Value)
                    {
                        member.ReferralReason = (string)dataReader["ReferralReason"];
                    }
                    if (dataReader["ReferralRequetsDate"] != DBNull.Value)
                    {
                        member.ReferralRequetsDate = new DateTime?((DateTime)dataReader["ReferralRequetsDate"]);
                    }
                    if (dataReader["RefusalReason"] != DBNull.Value)
                    {
                        member.RefusalReason = (string)dataReader["RefusalReason"];
                    }
                    member.IsOpenBalance = (bool)dataReader["IsOpenBalance"];
                    member.TradePassword = (string)dataReader["TradePassword"];
                    member.TradePasswordFormat = (MembershipPasswordFormat)((int)dataReader["TradePasswordFormat"]);
                    member.OrderNumber = (int)dataReader["OrderNumber"];
                    member.Expenditure = (decimal)dataReader["Expenditure"];
                    member.Points = (int)dataReader["Points"];
                    member.Balance = (decimal)dataReader["Balance"];
                    member.RequestBalance = (decimal)dataReader["RequestBalance"];
                    member.EmailVerification = (bool)dataReader["EmailVerification"];
                    member.CellPhoneVerification = (bool)dataReader["CellPhoneVerification"];
                    if (dataReader["TopRegionId"] != DBNull.Value)
                    {
                        member.TopRegionId = (int)dataReader["TopRegionId"];
                    }
                    if (dataReader["RegionId"] != DBNull.Value)
                    {
                        member.RegionId = (int)dataReader["RegionId"];
                    }
                    if (dataReader["RealName"] != DBNull.Value)
                    {
                        member.RealName = (string)dataReader["RealName"];
                    }
                    if (dataReader["IdentityCard"] != DBNull.Value)
                    {
                        member.IdentityCard = (string)dataReader["IdentityCard"];
                    }
                    if (dataReader["Address"] != DBNull.Value)
                    {
                        member.Address = (string)dataReader["Address"];
                    }
                    if (dataReader["Zipcode"] != DBNull.Value)
                    {
                        member.Zipcode = (string)dataReader["Zipcode"];
                    }
                    if (dataReader["TelPhone"] != DBNull.Value)
                    {
                        member.TelPhone = (string)dataReader["TelPhone"];
                    }
                    if (dataReader["CellPhone"] != DBNull.Value)
                    {
                        member.CellPhone = (string)dataReader["CellPhone"];
                    }
                    if (dataReader["QQ"] != DBNull.Value)
                    {
                        member.QQ = (string)dataReader["QQ"];
                    }
                    if (dataReader["Wangwang"] != DBNull.Value)
                    {
                        member.Wangwang = (string)dataReader["Wangwang"];
                    }
                    if (dataReader["MSN"] != DBNull.Value)
                    {
                        member.MSN = (string)dataReader["MSN"];
                    }
                    if (dataReader["WeChat"] != DBNull.Value)
                    {
                        member.WeChat = (string)dataReader["WeChat"];
                    }
                    if (dataReader["VipCardNumber"] != DBNull.Value)
                    {
                        member.VipCardNumber = (string)dataReader["VipCardNumber"];
                    }
                    if (dataReader["SessionId"] != DBNull.Value)
                    {
                        member.SessionId = (string)dataReader["SessionId"];
                    }
                    if (dataReader["OpenId"] != DBNull.Value)
                    {
                        member.OpenId = (string)dataReader["OpenId"];
                    }
                    if (dataReader["UserId"] != DBNull.Value)
                    {
                        member.UserId = (int)dataReader["UserId"];
                    }
                    if (dataReader["UserName"] != DBNull.Value)
                    {
                        member.Username = (string)dataReader["UserName"];
                    }
                }              
            }
            user = member;
            return (bool)this.database.GetParameterValue(storedProcCommand, "@IsExists");
        }

        /// <summary>
        /// 切换处理
        /// </summary>
        /// <returns></returns>
        public override IUser UpdateUsersCurrent(int nowUserId, int switchUserId)
        {
            Member member = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("Users_UpdateUserCurrent");
            this.database.AddInParameter(storedProcCommand, "NowUserId", DbType.Int32, nowUserId);
            this.database.AddInParameter(storedProcCommand, "SwitchUserId", DbType.Int32, switchUserId);

            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                if (dataReader.Read())
                {
                    member = new Member(UserRole.Member);
                    if (dataReader["UserName"] != DBNull.Value)
                    {
                        member.Username = (string)dataReader["UserName"];
                    }
                    member.GradeId = (int)dataReader["GradeId"];
                    if (dataReader["ReferralUserId"] != DBNull.Value)
                    {
                        member.ReferralUserId = new int?((int)dataReader["ReferralUserId"]);
                    }
                    member.ReferralStatus = (int)dataReader["ReferralStatus"];
                    if (dataReader["ReferralReason"] != DBNull.Value)
                    {
                        member.ReferralReason = (string)dataReader["ReferralReason"];
                    }
                    if (dataReader["ReferralRequetsDate"] != DBNull.Value)
                    {
                        member.ReferralRequetsDate = new DateTime?((DateTime)dataReader["ReferralRequetsDate"]);
                    }
                    if (dataReader["RefusalReason"] != DBNull.Value)
                    {
                        member.RefusalReason = (string)dataReader["RefusalReason"];
                    }
                    member.IsOpenBalance = (bool)dataReader["IsOpenBalance"];
                    member.TradePassword = (string)dataReader["TradePassword"];
                    member.TradePasswordFormat = (MembershipPasswordFormat)((int)dataReader["TradePasswordFormat"]);
                    member.OrderNumber = (int)dataReader["OrderNumber"];
                    member.Expenditure = (decimal)dataReader["Expenditure"];
                    member.Points = (int)dataReader["Points"];
                    member.Balance = (decimal)dataReader["Balance"];
                    member.RequestBalance = (decimal)dataReader["RequestBalance"];
                    member.EmailVerification = (bool)dataReader["EmailVerification"];
                    member.CellPhoneVerification = (bool)dataReader["CellPhoneVerification"];
                    if (dataReader["TopRegionId"] != DBNull.Value)
                    {
                        member.TopRegionId = (int)dataReader["TopRegionId"];
                    }
                    if (dataReader["RegionId"] != DBNull.Value)
                    {
                        member.RegionId = (int)dataReader["RegionId"];
                    }
                    if (dataReader["RealName"] != DBNull.Value)
                    {
                        member.RealName = (string)dataReader["RealName"];
                    }
                    if (dataReader["IdentityCard"] != DBNull.Value)
                    {
                        member.IdentityCard = (string)dataReader["IdentityCard"];
                    }
                    if (dataReader["Address"] != DBNull.Value)
                    {
                        member.Address = (string)dataReader["Address"];
                    }
                    if (dataReader["Zipcode"] != DBNull.Value)
                    {
                        member.Zipcode = (string)dataReader["Zipcode"];
                    }
                    if (dataReader["TelPhone"] != DBNull.Value)
                    {
                        member.TelPhone = (string)dataReader["TelPhone"];
                    }
                    if (dataReader["CellPhone"] != DBNull.Value)
                    {
                        member.CellPhone = (string)dataReader["CellPhone"];
                    }
                    if (dataReader["QQ"] != DBNull.Value)
                    {
                        member.QQ = (string)dataReader["QQ"];
                    }
                    if (dataReader["Wangwang"] != DBNull.Value)
                    {
                        member.Wangwang = (string)dataReader["Wangwang"];
                    }
                    if (dataReader["MSN"] != DBNull.Value)
                    {
                        member.MSN = (string)dataReader["MSN"];
                    }
                    if (dataReader["WeChat"] != DBNull.Value)
                    {
                        member.WeChat = (string)dataReader["WeChat"];
                    }
                    if (dataReader["VipCardNumber"] != DBNull.Value)
                    {
                        member.VipCardNumber = (string)dataReader["VipCardNumber"];
                    }
                    if (dataReader["SessionId"] != DBNull.Value)
                    {
                        member.SessionId = (string)dataReader["SessionId"];
                    }
                    if (dataReader["OpenId"] != DBNull.Value)
                    {
                        member.OpenId = (string)dataReader["OpenId"];
                    }
                    if (dataReader["UserId"] != DBNull.Value)
                    {
                        member.UserId = (int)dataReader["UserId"];
                    }
                }
            }
            return member;
        }

        public override int GetToalCountByOpenId(string openId)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select COUNT(1) from dbo.aspnet_Users where OpenId=@OpenId");
            this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, openId);
            int count;
            try
            {
                count = (int)this.database.ExecuteScalar(sqlStringCommand);
            }
            catch
            {
                count = 0;
            }
            return count;
        }
        public override bool UpdateUserTopRegionId(int userId, int topRegionId)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update dbo.aspnet_Members  set topRegionId =@topRegionId where userId=@userId");
            this.database.AddInParameter(sqlStringCommand, "userId", System.Data.DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "topRegionId", System.Data.DbType.Int32, topRegionId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        internal string GenerateSalt()
        {
            byte[] array = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(array);
            return Convert.ToBase64String(array);
        }

        public override bool UpdateUserAvatar(int userId, string avatar)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET HeadImgUrl = @Avatar WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "Avatar", System.Data.DbType.String, avatar);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public override string GetUserAvatar(int userId)
        {
            string avatar = "";

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT HeadImgUrl FROM aspnet_Users WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);

            avatar = this.database.ExecuteScalar(sqlStringCommand).ToString();

            return avatar;
        }

        public override bool GetMemberInfo(int userId, out string userName, out string realName, out string email, out string cellphone, out string qq, out string avatar, out decimal balance, out decimal expenditure)
        {
            bool retVal = false;

            userName = "";
            realName = "";
            email = "";
            cellphone = "";
            qq = "";
            avatar = "";
            balance = 0M;
            expenditure = 0M;

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT u.UserId, u.UserName, u.HeadImgUrl, u.Email, m.CellPhone, m.RealName, m.QQ, m.Balance, m.Expenditure FROM aspnet_Users u JOIN aspnet_Members m ON m.UserId = u.UserId WHERE u.UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);

            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    if (dataReader["UserName"] != DBNull.Value)
                    {
                        userName = (string)dataReader["UserName"];
                    }

                    expenditure = (decimal)dataReader["Expenditure"];
                    balance = (decimal)dataReader["Balance"];
                    if (dataReader["RealName"] != DBNull.Value)
                    {
                        realName = (string)dataReader["RealName"];
                    }
                    if (dataReader["CellPhone"] != DBNull.Value)
                    {
                        cellphone = (string)dataReader["CellPhone"];
                    }
                    if (dataReader["QQ"] != DBNull.Value)
                    {
                        qq = (string)dataReader["QQ"];
                    }
                    if (dataReader["Email"] != DBNull.Value)
                    {
                        email = (string)dataReader["Email"];
                    }
                    if (dataReader["HeadImgUrl"] != DBNull.Value)
                    {
                        avatar = (string)dataReader["HeadImgUrl"];
                    }

                    retVal = true;
                }
            }

            return retVal;
        }

        public override bool UpdateUser(int userId, string realName, string email, string cellphone, string qq, string oldEmail, string oldCellphone, bool emailVerification, bool cellPhoneVerification)
        {
            //去除手机号邮箱验证
            //if (CheckCellPhoneAndUserName(userId, email, cellphone) > 0)
            //    return false;

            emailVerification = (oldEmail.ToLower() == email.ToLower() ? emailVerification : false);
            cellPhoneVerification = (oldCellphone.ToLower() == cellphone.ToLower() ? cellPhoneVerification : false);

            try
            {
                DbCommand userCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET Email = @Email, LoweredEmail = @LoweredEmail WHERE UserId = @UserId");
                this.database.AddInParameter(userCommand, "UserId", System.Data.DbType.Int32, userId);
                this.database.AddInParameter(userCommand, "Email", System.Data.DbType.String, email);
                this.database.AddInParameter(userCommand, "LoweredEmail", System.Data.DbType.String, email.ToLower());

                int userRetVal = this.database.ExecuteNonQuery(userCommand);

                DbCommand memberCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET CellPhone = @CellPhone, QQ = @QQ, RealName = @RealName, EmailVerification = @EmailVerification, CellPhoneVerification = @CellPhoneVerification WHERE UserId = @UserId");
                this.database.AddInParameter(memberCommand, "UserId", System.Data.DbType.Int32, userId);
                this.database.AddInParameter(memberCommand, "CellPhone", System.Data.DbType.String, cellphone);
                this.database.AddInParameter(memberCommand, "QQ", System.Data.DbType.String, qq);
                this.database.AddInParameter(memberCommand, "RealName", System.Data.DbType.String, realName);
                this.database.AddInParameter(memberCommand, "EmailVerification", System.Data.DbType.Boolean, emailVerification);
                this.database.AddInParameter(memberCommand, "CellPhoneVerification", System.Data.DbType.Boolean, cellPhoneVerification);

                int memberRetVal = this.database.ExecuteNonQuery(memberCommand);

                return true;
            }

            catch (Exception ex)
            {

            }

            return false;
        }


        public override bool UpdateUser(int userId, string realName, string email, string cellphone, string qq, string oldEmail, string oldCellphone, bool emailVerification, bool cellPhoneVerification,int gender)
        {
            //去除手机号邮箱验证
            //if (CheckCellPhoneAndUserName(userId, email, cellphone) > 0)
            //    return false;

            emailVerification = (oldEmail.ToLower() == email.ToLower() ? emailVerification : false);
            cellPhoneVerification = (oldCellphone.ToLower() == cellphone.ToLower() ? cellPhoneVerification : false);

            try
            {
                DbCommand userCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET Email = @Email, LoweredEmail = @LoweredEmail,Gender=@Gender WHERE UserId = @UserId");
                this.database.AddInParameter(userCommand, "UserId", System.Data.DbType.Int32, userId);
                this.database.AddInParameter(userCommand, "Email", System.Data.DbType.String, email);
                this.database.AddInParameter(userCommand, "LoweredEmail", System.Data.DbType.String, email.ToLower());
                this.database.AddInParameter(userCommand, "Gender", System.Data.DbType.Int32, gender);


                int userRetVal = this.database.ExecuteNonQuery(userCommand);

                DbCommand memberCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET CellPhone = @CellPhone, QQ = @QQ, RealName = @RealName, EmailVerification = @EmailVerification, CellPhoneVerification = @CellPhoneVerification WHERE UserId = @UserId");
                this.database.AddInParameter(memberCommand, "UserId", System.Data.DbType.Int32, userId);
                this.database.AddInParameter(memberCommand, "CellPhone", System.Data.DbType.String, cellphone);
                this.database.AddInParameter(memberCommand, "QQ", System.Data.DbType.String, qq);
                this.database.AddInParameter(memberCommand, "RealName", System.Data.DbType.String, realName);
                this.database.AddInParameter(memberCommand, "EmailVerification", System.Data.DbType.Boolean, emailVerification);
                this.database.AddInParameter(memberCommand, "CellPhoneVerification", System.Data.DbType.Boolean, cellPhoneVerification);

                int memberRetVal = this.database.ExecuteNonQuery(memberCommand);

                return true;
            }

            catch (Exception ex)
            {

            }

            return false;
        }


        public override bool UpdateUser(int userId, string realName, string email, string cellphone, string qq, string oldEmail, string oldCellphone, bool emailVerification, bool cellPhoneVerification, int gender, string IdNo, int IsVerify, DateTime? VerifyDate)
        {
            //去除手机号邮箱验证
            //if (CheckCellPhoneAndUserName(userId, email, cellphone) > 0)
            //    return false;

            emailVerification = (oldEmail.ToLower() == email.ToLower() ? emailVerification : false);
            cellPhoneVerification = (oldCellphone.ToLower() == cellphone.ToLower() ? cellPhoneVerification : false);

            try
            {
                DbCommand userCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET Email = @Email, LoweredEmail = @LoweredEmail,Gender=@Gender WHERE UserId = @UserId");
                this.database.AddInParameter(userCommand, "UserId", System.Data.DbType.Int32, userId);
                this.database.AddInParameter(userCommand, "Email", System.Data.DbType.String, email);
                this.database.AddInParameter(userCommand, "LoweredEmail", System.Data.DbType.String, email.ToLower());
                this.database.AddInParameter(userCommand, "Gender", System.Data.DbType.Int32, gender);


                int userRetVal = this.database.ExecuteNonQuery(userCommand);

                DbCommand memberCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET CellPhone = @CellPhone, QQ = @QQ, RealName = @RealName, EmailVerification = @EmailVerification, CellPhoneVerification = @CellPhoneVerification,IdentityCard=@IdentityCard,VerifyDate=@VerifyDate,IsVerify=@IsVerify WHERE UserId = @UserId");
                this.database.AddInParameter(memberCommand, "UserId", System.Data.DbType.Int32, userId);
                this.database.AddInParameter(memberCommand, "CellPhone", System.Data.DbType.String, cellphone);
                this.database.AddInParameter(memberCommand, "QQ", System.Data.DbType.String, qq);
                this.database.AddInParameter(memberCommand, "RealName", System.Data.DbType.String, realName);
                this.database.AddInParameter(memberCommand, "EmailVerification", System.Data.DbType.Boolean, emailVerification);
                this.database.AddInParameter(memberCommand, "CellPhoneVerification", System.Data.DbType.Boolean, cellPhoneVerification);
                this.database.AddInParameter(memberCommand, "IdentityCard", System.Data.DbType.String, IdNo);
                this.database.AddInParameter(memberCommand, "VerifyDate", System.Data.DbType.DateTime, VerifyDate);
                this.database.AddInParameter(memberCommand, "IsVerify", System.Data.DbType.Int32, IsVerify);

                int memberRetVal = this.database.ExecuteNonQuery(memberCommand);

                return true;
            }

            catch (Exception ex)
            {

            }

            return false;
        }



        /// <summary>
        /// 绑定第三方用户
        /// </summary>
        /// <returns></returns>
        public override bool BindUsersMemberUsersInRoles(string openIdType, string openId, string passwordSalt, string realName, string avatar, long provinceId, int? bindUserId, out IUser user)
        {
            Member member = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("Users_Member_UsersInRoles_Bind");
            this.database.AddInParameter(storedProcCommand, "OpenIdType", DbType.String, openIdType);
            this.database.AddInParameter(storedProcCommand, "OpenId", DbType.String, openId);
            this.database.AddInParameter(storedProcCommand, "PasswordSalt", DbType.String, passwordSalt);
            this.database.AddInParameter(storedProcCommand, "RealName", DbType.String, realName);
            this.database.AddInParameter(storedProcCommand, "HeadImgUrl", DbType.String, avatar);
            this.database.AddInParameter(storedProcCommand, "ProvinceId", DbType.Int32, provinceId);
            this.database.AddInParameter(storedProcCommand, "@BindUserId", DbType.Int32, bindUserId);
            this.database.AddOutParameter(storedProcCommand, "@IsExists", DbType.Boolean, sizeof(bool));

            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                if (dataReader.Read())
                {
                    member = new Member(UserRole.Member);
                    member.GradeId = (int)dataReader["GradeId"];
                    if (dataReader["ReferralUserId"] != DBNull.Value)
                    {
                        member.ReferralUserId = new int?((int)dataReader["ReferralUserId"]);
                    }
                    member.ReferralStatus = (int)dataReader["ReferralStatus"];
                    if (dataReader["ReferralReason"] != DBNull.Value)
                    {
                        member.ReferralReason = (string)dataReader["ReferralReason"];
                    }
                    if (dataReader["ReferralRequetsDate"] != DBNull.Value)
                    {
                        member.ReferralRequetsDate = new DateTime?((DateTime)dataReader["ReferralRequetsDate"]);
                    }
                    if (dataReader["RefusalReason"] != DBNull.Value)
                    {
                        member.RefusalReason = (string)dataReader["RefusalReason"];
                    }
                    member.IsOpenBalance = (bool)dataReader["IsOpenBalance"];
                    member.TradePassword = (string)dataReader["TradePassword"];
                    member.TradePasswordFormat = (MembershipPasswordFormat)((int)dataReader["TradePasswordFormat"]);
                    member.OrderNumber = (int)dataReader["OrderNumber"];
                    member.Expenditure = (decimal)dataReader["Expenditure"];
                    member.Points = (int)dataReader["Points"];
                    member.Balance = (decimal)dataReader["Balance"];
                    member.RequestBalance = (decimal)dataReader["RequestBalance"];
                    member.EmailVerification = (bool)dataReader["EmailVerification"];
                    member.CellPhoneVerification = (bool)dataReader["CellPhoneVerification"];
                    if (dataReader["TopRegionId"] != DBNull.Value)
                    {
                        member.TopRegionId = (int)dataReader["TopRegionId"];
                    }
                    if (dataReader["RegionId"] != DBNull.Value)
                    {
                        member.RegionId = (int)dataReader["RegionId"];
                    }
                    if (dataReader["RealName"] != DBNull.Value)
                    {
                        member.RealName = (string)dataReader["RealName"];
                    }
                    if (dataReader["IdentityCard"] != DBNull.Value)
                    {
                        member.IdentityCard = (string)dataReader["IdentityCard"];
                    }
                    if (dataReader["Address"] != DBNull.Value)
                    {
                        member.Address = (string)dataReader["Address"];
                    }
                    if (dataReader["Zipcode"] != DBNull.Value)
                    {
                        member.Zipcode = (string)dataReader["Zipcode"];
                    }
                    if (dataReader["TelPhone"] != DBNull.Value)
                    {
                        member.TelPhone = (string)dataReader["TelPhone"];
                    }
                    if (dataReader["CellPhone"] != DBNull.Value)
                    {
                        member.CellPhone = (string)dataReader["CellPhone"];
                    }
                    if (dataReader["QQ"] != DBNull.Value)
                    {
                        member.QQ = (string)dataReader["QQ"];
                    }
                    if (dataReader["Wangwang"] != DBNull.Value)
                    {
                        member.Wangwang = (string)dataReader["Wangwang"];
                    }
                    if (dataReader["MSN"] != DBNull.Value)
                    {
                        member.MSN = (string)dataReader["MSN"];
                    }
                    if (dataReader["WeChat"] != DBNull.Value)
                    {
                        member.WeChat = (string)dataReader["WeChat"];
                    }
                    if (dataReader["VipCardNumber"] != DBNull.Value)
                    {
                        member.VipCardNumber = (string)dataReader["VipCardNumber"];
                    }
                    if (dataReader["SessionId"] != DBNull.Value)
                    {
                        member.SessionId = (string)dataReader["SessionId"];
                    }
                    if (dataReader["OpenId"] != DBNull.Value)
                    {
                        member.OpenId = (string)dataReader["OpenId"];
                    }
                    if (dataReader["UserId"] != DBNull.Value)
                    {
                        member.UserId = (int)dataReader["UserId"];
                    }
                    if (dataReader["UserName"] != DBNull.Value)
                    {
                        member.Username = (string)dataReader["UserName"];
                    }
                }
            }

            user = member;

            return (bool)this.database.GetParameterValue(storedProcCommand, "@IsExists");
        }

        #region private

        private int CheckCellPhoneAndUserName(int userId, string email, string cellPhone)
        {
            int result = 0;

            DbCommand sqlStringCommand = null;

            if (email != "")
            {
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(1) FROM aspnet_Users WHERE [UserId] != @UserId AND ([LoweredUserName] = @Email OR [LoweredEmail] = @Email)");
                this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, email.ToLower());
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);

                try
                {
                    result = (int)this.database.ExecuteScalar(sqlStringCommand);
                }
                catch
                {
                    result = 0;
                }
            }

            if (cellPhone != "")
            {
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(1) FROM aspnet_Members A INNER JOIN aspnet_Users B on A.[UserId]=B.[UserId] WHERE B.[LoweredUserName]=@cellPhone and A.[CellPhone]=@cellPhone AND A.[UserId] != @UserId");

                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
                this.database.AddInParameter(sqlStringCommand, "cellPhone", System.Data.DbType.String, cellPhone.ToLower());

                try
                {
                    result += (int)this.database.ExecuteScalar(sqlStringCommand);
                }
                catch
                {
                    result += 0;
                }
            }

            return result;
        }

        #endregion
    }
}

using EcShop.Core.Configuration;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using Ecdev.Components.Validation;
using Ecdev.Components.Validation.Validators;
using System;
using System.Text.RegularExpressions;
using System.Web.Security;
namespace EcShop.Membership.Context
{
    [HasSelfValidation]
    public class Member : IUser
    {
        public static event System.EventHandler<UserEventArgs> Register;
        public static event System.EventHandler<System.EventArgs> Login;
        public static event System.EventHandler<UserEventArgs> Logout;
        public static event System.EventHandler<UserEventArgs> FindPassword;
        public static event System.EventHandler<UserEventArgs> PasswordChanged;
        public static event System.EventHandler<UserEventArgs> DealPasswordChanged;
        public string VipCardNumber
        {
            get;
            set;
        }

        public string AccurateCreateDate
        {
            get;
            set;
        }
        public int GradeId
        {
            get;
            set;
        }
        public int? ParentUserId
        {
            get;
            set;
        }
        public int? ReferralUserId
        {
            get;
            set;
        }
        public int ReferralStatus
        {
            get;
            set;
        }
        public string ReferralReason
        {
            get;
            set;
        }
        public System.DateTime? ReferralRequetsDate
        {
            get;
            set;
        }
        public string RefusalReason
        {
            get;
            set;
        }
        public System.DateTime? ReferralAuditDate
        {
            get;
            set;
        }
        public int OrderNumber
        {
            get;
            set;
        }
        public int Points
        {
            get;
            set;
        }
        public decimal Expenditure
        {
            get;
            set;
        }
        public decimal Balance
        {
            get;
            set;
        }
        public decimal RequestBalance
        {
            get;
            set;
        }
        public int TopRegionId
        {
            get;
            set;
        }
        public int RegionId
        {
            get;
            set;
        }
        [StringLengthValidator(0, 20, Ruleset = "ValMember", MessageTemplate = "真实姓名必须控制在20个字符以内")]
        public string RealName
        {
            get;
            set;
        }
        public string IdentityCard
        {
            get;
            set;
        }
        [StringLengthValidator(0, 100, Ruleset = "ValMember", MessageTemplate = "详细地址必须控制在100个字符以内")]
        public string Address
        {
            get;
            set;
        }
        public string Zipcode
        {
            get;
            set;
        }
        public string TelPhone
        {
            get;
            set;
        }
        public string CellPhone
        {
            get;
            set;
        }
        public string QQ
        {
            get;
            set;
        }
        public string Wangwang
        {
            get;
            set;
        }
        public string MSN
        {
            get;
            set;
        }
        public string WeChat
        {
            get;
            set;
        }
        public bool EmailVerification
        {
            get;
            set;
        }
        public bool CellPhoneVerification
        {
            get;
            set;
        }
        public string SessionId
        {
            get;
            set;
        }
        public string OpenId
        {
            get
            {
                return this.MembershipUser.OpenId;
            }
            set
            {
                this.MembershipUser.OpenId = value;
            }
        }
        public HiMembershipUser MembershipUser
        {
            get;
            private set;
        }
        public int UserId
        {
            get
            {
                return this.MembershipUser.UserId;
            }
            set
            {
                this.MembershipUser.UserId = value;
            }
        }
        public string Username
        {
            get
            {
                return this.MembershipUser.Username;
            }
            set
            {
                this.MembershipUser.Username = value;
            }
        }
        /// <summary>
        /// 是否验证：1、已经验证，2、未验证
        /// </summary>
        public int IsVerify
        {
            get;
            set;
        }
        /// <summary>
        /// 验证时间
        /// </summary>
        public DateTime? VerifyDate
        {
            get;
            set;
        }
        public string MobilePIN
        {
            get
            {
                return this.MembershipUser.MobilePIN;
            }
            set
            {
                this.MembershipUser.MobilePIN = value;
            }
        }
        public bool IsAnonymous
        {
            get
            {
                return this.MembershipUser.IsAnonymous;
            }
        }
        public System.DateTime LastActivityDate
        {
            get
            {
                return this.MembershipUser.LastActivityDate;
            }
            set
            {
                this.MembershipUser.LastActivityDate = value;
            }
        }
        public string Password
        {
            get
            {
                return this.MembershipUser.Password;
            }
            set
            {
                this.MembershipUser.Password = value;
            }
        }

        public string PasswordSalt
        {
            get { return this.MembershipUser.PasswordSalt; }
            set { this.MembershipUser.PasswordSalt = value; }
        }

        public MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return this.MembershipUser.PasswordFormat;
            }
            set
            {
                this.MembershipUser.PasswordFormat = value;
            }
        }
        public string Email
        {
            get
            {
                return this.MembershipUser.Email;
            }
            set
            {
                this.MembershipUser.Email = value;
            }
        }
        public string PasswordQuestion
        {
            get
            {
                return this.MembershipUser.PasswordQuestion;
            }
        }
        public bool IsApproved
        {
            get
            {
                return this.MembershipUser.IsApproved;
            }
            set
            {
                this.MembershipUser.IsApproved = value;
            }
        }
        public bool IsLockedOut
        {
            get
            {
                return this.MembershipUser.IsLockedOut;
            }
        }
        public System.DateTime CreateDate
        {
            get
            {
                return this.MembershipUser.CreateDate;
            }

            set
            {
                this.MembershipUser.CreateDate = value;
            }
        }
        public System.DateTime LastLoginDate
        {
            get
            {
                return this.MembershipUser.LastLoginDate;
            }
        }
        public System.DateTime LastPasswordChangedDate
        {
            get
            {
                return this.MembershipUser.LastPasswordChangedDate;
            }
        }
        public System.DateTime LastLockoutDate
        {
            get
            {
                return this.MembershipUser.LastLockoutDate;
            }
        }
        public string Comment
        {
            get
            {
                return this.MembershipUser.Comment;
            }
            set
            {
                this.MembershipUser.Comment = value;
            }
        }
        public Gender Gender
        {
            get
            {
                return this.MembershipUser.Gender;
            }
            set
            {
                this.MembershipUser.Gender = value;
            }
        }
        public System.DateTime? BirthDate
        {
            get
            {
                return this.MembershipUser.BirthDate;
            }
            set
            {
                this.MembershipUser.BirthDate = value;
            }
        }
        public UserRole UserRole
        {
            get
            {
                return this.MembershipUser.UserRole;
            }
        }
        public bool IsOpenBalance
        {
            get
            {
                return this.MembershipUser.IsOpenBalance;
            }
            set
            {
                this.MembershipUser.IsOpenBalance = value;
            }
        }
        public string TradePassword
        {
            get
            {
                return this.MembershipUser.TradePassword;
            }
            set
            {
                this.MembershipUser.TradePassword = value;
            }
        }
        public MembershipPasswordFormat TradePasswordFormat
        {
            get
            {
                return this.MembershipUser.TradePasswordFormat;
            }
            set
            {
                this.MembershipUser.TradePasswordFormat = value;
            }
        }
        public string AliOpenId
        {
            get
            {
                return this.MembershipUser.AliOpenId;
            }
            set
            {
                this.MembershipUser.AliOpenId = value;
            }
        }

        public string HeadImgUrl
        {
            get
            {
                return this.MembershipUser.HeadImgUrl;
            }
            set
            {
                this.MembershipUser.HeadImgUrl = value;
            }
        }


        public Member(UserRole userRole)
        {
            if (userRole != UserRole.Member)
            {
                throw new System.Exception("UserRole must be Member or Underling");
            }
            this.MembershipUser = new HiMembershipUser(false, userRole);
        }
        public Member(UserRole userRole, HiMembershipUser membershipUser)
        {
            if (userRole != UserRole.Member)
            {
                throw new System.Exception("UserRole must be Member or Underling");
            }
            this.MembershipUser = membershipUser;
        }
        [SelfValidation(Ruleset = "ValMember")]
        public void CheckMemberEmail(ValidationResults results)
        {
            HiConfiguration config = HiConfiguration.GetConfig();
            //if (string.IsNullOrEmpty(this.Email) || this.Email.Length > 256)
            //{
            //    results.AddResult(new ValidationResult("电子邮件不能为空且长度必须小于256个字符", this, "", "", null));
            //}
            //else
            //{
            //    if (!Regex.IsMatch(this.Email, config.EmailRegex))
            //    {
            //        results.AddResult(new ValidationResult("电子邮件的格式错误", this, "", "", null));
            //    }
            //}
            if (!string.IsNullOrWhiteSpace(this.Email))
            {
                if (!Regex.IsMatch(this.Email, config.EmailRegex))
                {
                    results.AddResult(new ValidationResult("电子邮件的格式错误", this, "", "", null));
                }
            }
            if (!string.IsNullOrEmpty(this.QQ) && (this.QQ.Length > 20 || this.QQ.Length < 3 || !Regex.IsMatch(this.QQ, "^[0-9]*$")))
            {
                results.AddResult(new ValidationResult("QQ号长度限制在3-20个字符之间，只能输入数字", this, "", "", null));
            }
            if (!string.IsNullOrEmpty(this.Wangwang) && (this.Wangwang.Length > 20 || this.Wangwang.Length < 3))
            {
                results.AddResult(new ValidationResult("旺旺长度限制在3-20个字符之间", this, "", "", null));
            }
            if (!string.IsNullOrEmpty(this.CellPhone) && (this.CellPhone.Length > 20 || this.CellPhone.Length < 3 || !Regex.IsMatch(this.CellPhone, "^[0-9]*$")))
            {
                results.AddResult(new ValidationResult("手机号码长度限制在3-20个字符之间,只能输入数字", this, "", "", null));
            }
            if (!string.IsNullOrEmpty(this.TelPhone) && (this.TelPhone.Length > 20 || this.TelPhone.Length < 3 || !Regex.IsMatch(this.TelPhone, "^[0-9-]*$")))
            {
                results.AddResult(new ValidationResult("电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”", this, "", "", null));
            }
            if (!string.IsNullOrEmpty(this.MSN) && (this.MSN.Length > 256 || this.MSN.Length < 1 || !Regex.IsMatch(this.MSN, config.UsernameRegex)))
            {
                results.AddResult(new ValidationResult("请输入正确的微信号码，长度在1-256个字符以内", this, "", "", null));
            }
        }
        public bool IsInRole(string roleName)
        {
            return this.UserRole == UserRole.Member && roleName.Equals(HiContext.Current.Config.RolesConfiguration.Member);
        }
        public string ResetPassword(string answer)
        {
            return this.MembershipUser.ResetPassword(answer);
        }

        public bool ResetPassword(string mobile, string decryptPassword, string passwordSalt)
        {
            MemberUserProvider memberUserProvider = MemberUserProvider.Instance();
            return memberUserProvider.ResetPassword(mobile, decryptPassword, passwordSalt);
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            return this.MembershipUser.ChangePassword(oldPassword, newPassword);
        }
        public bool ChangePasswordWithAnswer(string answer, string newPassword)
        {
            return this.MembershipUser.ChangePasswordWithAnswer(answer, newPassword);
        }
        public bool ChangePasswordQuestionAndAnswer(string oldAnswer, string newQuestion, string newAnswer)
        {
            return this.MembershipUser.ChangePasswordQuestionAndAnswer(oldAnswer, newQuestion, newAnswer);
        }
        public bool ChangePasswordQuestionAndAnswer(string newQuestion, string newAnswer)
        {
            return this.MembershipUser.ChangePasswordQuestionAndAnswer(newQuestion, newAnswer);
        }
        public bool ValidatePasswordAnswer(string answer)
        {
            return this.MembershipUser.ValidatePasswordAnswer(answer);
        }
        public IUserCookie GetUserCookie()
        {
            return new UserCookie(this);
        }
        public bool ChangePassword(string newPassword)
        {
            bool result;
            if (this.UserRole == UserRole.Member && HiContext.Current.User.UserRole == UserRole.SiteManager)
            {
                string password = this.MembershipUser.Membership.ResetPassword();
                if (this.MembershipUser.ChangePassword(password, newPassword))
                {
                    result = true;
                    return result;
                }
            }
            result = false;
            return result;
        }
        public bool ChangeTradePassword(string oldPassword, string newPassword)
        {
            UserFactory memberFactory = this.GetMemberFactory();
            return memberFactory.ChangeTradePassword(this.Username, oldPassword, newPassword);
        }
        public bool ChangeTradePassword(string newPassword)
        {
            UserFactory memberFactory = this.GetMemberFactory();
            return memberFactory.ChangeTradePassword(this.Username, newPassword);
        }
        public bool OpenBalance(string tradePassword)
        {
            UserFactory memberFactory = this.GetMemberFactory();
            return memberFactory.OpenBalance(this.UserId, tradePassword);
        }
        public string ResetTradePassword(string username)
        {
            UserFactory memberFactory = this.GetMemberFactory();
            return memberFactory.ResetTradePassword(username);
        }
        public bool ChangePasswordWithoutAnswer(string newPassword)
        {
            string password = this.MembershipUser.Membership.ResetPassword();
            return this.MembershipUser.ChangePassword(password, newPassword);
        }
        private UserFactory GetMemberFactory()
        {
            if (this.UserRole == UserRole.Member)
            {
                return MemberFactory.Instance();
            }
            throw new System.Exception("UserRole must be Member or Underling");
        }
        public static void OnRegister(Member member, UserEventArgs args)
        {
            if (Member.Register != null)
            {
                Member.Register(member, args);
            }
        }
        public void OnRegister(UserEventArgs args)
        {
            if (Member.Register != null)
            {
                Member.Register(this, args);
            }
        }
        public static void OnLogin(Member member)
        {
            if (Member.Login != null)
            {
                Member.Login(member, new System.EventArgs());
            }
        }
        public void OnLogin()
        {
            if (Member.Login != null)
            {
                Member.Login(this, new System.EventArgs());
            }
        }
        public static void OnLogout(UserEventArgs args)
        {
            if (Member.Logout != null)
            {
                Member.Logout(null, args);
            }
        }
        public static void OnFindPassword(Member member, UserEventArgs args)
        {
            if (Member.FindPassword != null)
            {
                Member.FindPassword(member, args);
            }
        }
        public void OnFindPassword(UserEventArgs args)
        {
            if (Member.FindPassword != null)
            {
                Member.FindPassword(this, args);
            }
        }
        public static void OnPasswordChanged(Member member, UserEventArgs args)
        {
            if (Member.PasswordChanged != null)
            {
                Member.PasswordChanged(member, args);
            }
        }
        public void OnPasswordChanged(UserEventArgs args)
        {
            if (Member.PasswordChanged != null)
            {
                Member.PasswordChanged(this, args);
            }
        }
        public static void OnDealPasswordChanged(Member member, UserEventArgs args)
        {
            if (Member.DealPasswordChanged != null)
            {
                Member.DealPasswordChanged(member, args);
            }
        }
        public void OnDealPasswordChanged(UserEventArgs args)
        {
            if (Member.DealPasswordChanged != null)
            {
                Member.DealPasswordChanged(this, args);
            }
        }


        public UserType UserType
        {
            get
            {
                return this.MembershipUser.UserType;
            }
            set
            {
                this.MembershipUser.UserType = value;
            }
        }
    }
}

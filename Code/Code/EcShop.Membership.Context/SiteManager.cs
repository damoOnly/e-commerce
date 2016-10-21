using EcShop.Core.Configuration;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using Ecdev.Components.Validation;
using Ecdev.Components.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace EcShop.Membership.Context
{
    using System.Web.Security;//修改1
	[HasSelfValidation]
	public class SiteManager : IUser
	{
		public bool IsAdministrator
		{
			get
			{
				bool result;
				try
				{
					if (this.IsAnonymous)
					{
						result = false;
						return result;
					}
					RolesConfiguration rolesConfiguration = HiConfiguration.GetConfig().RolesConfiguration;
					result = this.IsInRole(rolesConfiguration.SystemAdministrator);
					return result;
				}
				catch
				{
				}
				result = false;
				return result;
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
		public string OpenId
		{
			get;
			set;
		}
        public string SessionId
        {
            get;
            set;
        }
        public int SupplierId//关联的供应商，表上没有
        {
            get
            {
                return this.MembershipUser.SupplierId;
            }
            set
            {
                this.MembershipUser.SupplierId = value;
            }
        }


        public int StoreId//关联的门店，表上没有
        {
            get
            {
                return this.MembershipUser.StoreId;
            }
            set
            {
                this.MembershipUser.StoreId = value;
            }
        }

        public string Name
        {
            get
            {
                return this.MembershipUser.Name;
            }
            set
            {
                this.MembershipUser.Name = value;
            }
        }
		public string AliOpenId
		{
			get;
			set;
		}
		public SiteManager()
		{
			this.MembershipUser = new HiMembershipUser(false, UserRole.SiteManager);
		}
		public SiteManager(HiMembershipUser membershipUser)
		{
			this.MembershipUser = membershipUser;
		}
		public bool HasPrivilege(string privilegeCode)
		{
            System.Collections.Generic.IList<string> userPrivileges = RoleHelper.GetUserPrivileges(this.Username);
			return userPrivileges != null && userPrivileges.Count != 0 && userPrivileges.Contains(privilegeCode);
		}
		[SelfValidation(Ruleset = "ValManagerName")]
		public void CheckManagerName(ValidationResults results)
		{
			HiConfiguration config = HiConfiguration.GetConfig();
			if (string.IsNullOrEmpty(this.Username) || this.Username.Length > config.UsernameMaxLength || this.Username.Length < config.UsernameMinLength)
			{
				results.AddResult(new ValidationResult(string.Format("用户名不能为空，长度限制在{0}-{1}个字符之间", config.UsernameMinLength, config.UsernameMaxLength), this, "", "", null));
			}
			else
			{
				if (string.Compare(this.Username, "anonymous", true) == 0)
				{
					results.AddResult(new ValidationResult("不能使用anonymous作为用户名", this, "", "", null));
				}
				else
				{
					if (!Regex.IsMatch(this.Username, config.UsernameRegex))
					{
						results.AddResult(new ValidationResult("用户名的格式不符合要求，用户名一般由字母、数字、下划线和汉字组成，且必须以汉字或字母开头", this, "", "", null));
					}
				}
			}
		}
		[SelfValidation(Ruleset = "ValManagerPassword")]
		public void CheckManagerPassword(ValidationResults results)
		{
			HiConfiguration config = HiConfiguration.GetConfig();
			if (string.IsNullOrEmpty(this.Password) || this.Password.Length < Membership.Provider.MinRequiredPasswordLength || this.Password.Length > config.PasswordMaxLength)
			{
				results.AddResult(new ValidationResult(string.Format("管理员登录密码的长度只能在{0}和{1}个字符之间", Membership.Provider.MinRequiredPasswordLength, config.PasswordMaxLength), this, "", "", null));
			}
		}
		[SelfValidation(Ruleset = "ValManagerEmail")]
		public void CheckManagerEmail(ValidationResults results)
		{
			HiConfiguration config = HiConfiguration.GetConfig();
			if (string.IsNullOrEmpty(this.Email) || this.Email.Length > 256)
			{
				results.AddResult(new ValidationResult("电子邮件的长度必须小于256个字符", this, "", "", null));
			}
			else
			{
				if (!Regex.IsMatch(this.Email, config.EmailRegex))
				{
					results.AddResult(new ValidationResult("电子邮件的格式错误", this, "", "", null));
				}
			}
		}
		public bool ChangePassword(string password, string newPassword)
		{
			return this.MembershipUser.Membership.ChangePassword(password, newPassword);
		}
		public bool ChangePassword(string newPassword)
		{
			bool result;
			if (HiContext.Current.User.UserRole == UserRole.SiteManager)
			{
				SiteManager siteManager = HiContext.Current.User as SiteManager;
				if (siteManager != null && siteManager.UserId != this.UserId && siteManager.IsAdministrator)
				{
					string oldPassword = this.MembershipUser.Membership.ResetPassword();
					result = this.MembershipUser.Membership.ChangePassword(oldPassword, newPassword);
					return result;
				}
			}
			result = false;
			return result;
		}
		public IUserCookie GetUserCookie()
		{
			return new UserCookie(this);
		}
		public bool IsInRole(string roleName)
		{
			return Roles.IsUserInRole(this.Username, roleName);
		}
		public string ResetPassword(string answer)
		{
			return this.MembershipUser.ResetPassword(answer);
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
		public bool ChangeTradePassword(string oldPassword, string newPassword)
		{
			return true;
		}
		public bool ChangeTradePassword(string newPassword)
		{
			return true;
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

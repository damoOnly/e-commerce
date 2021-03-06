using EcShop.Membership.Core.Enums;
using System;
using System.Web.Security;
namespace EcShop.Membership.Core
{
	public class AnonymousUser : IUser
	{
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
		public DateTime LastActivityDate
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
		public DateTime CreateDate
		{
			get
			{
				return this.MembershipUser.CreateDate;
			}
		}
		public DateTime LastLoginDate
		{
			get
			{
				return this.MembershipUser.LastLoginDate;
			}
		}
		public DateTime LastPasswordChangedDate
		{
			get
			{
				return this.MembershipUser.LastPasswordChangedDate;
			}
		}
		public DateTime LastLockoutDate
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
		public DateTime? BirthDate
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
		public string AliOpenId
		{
			get;
			set;
		}
        public string SessionId
        {
            get;
            set;
        }
		public AnonymousUser(HiMembershipUser membershipUser)
		{
			if (!membershipUser.IsAnonymous || membershipUser.UserRole != UserRole.Anonymous || membershipUser.Username != "Anonymous")
			{
				throw new Exception("Invalid AnonymousUser");
			}
			this.MembershipUser = membershipUser;
		}
		public bool IsInRole(string roleName)
		{
			return false;
		}
		public string ResetPassword(string answer)
		{
			return null;
		}
		public bool ChangePassword(string newPassword)
		{
			return true;
		}
		public bool ChangePassword(string oldPassword, string newPassword)
		{
			return true;
		}
		public bool ChangePasswordWithAnswer(string answer, string newPassword)
		{
			return true;
		}
		public bool ChangePasswordQuestionAndAnswer(string oldAnswer, string newQuestion, string newAnswer)
		{
			return true;
		}
		public bool ChangePasswordQuestionAndAnswer(string newQuestion, string newAnswer)
		{
			return true;
		}
		public bool ValidatePasswordAnswer(string answer)
		{
			return true;
		}
		public IUserCookie GetUserCookie()
		{
			return null;
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

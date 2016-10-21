using EcShop.Membership.Core.Enums;
using System;
using System.Web.Security;
namespace EcShop.Membership.Core
{
	public class HiMembershipUser
	{
		private DateTime lastActivityDate;
		private string email;
		private bool isApproved;
		private DateTime lastLoginDate;
		private string comment;
		public MembershipUser Membership
		{
			get;
			private set;
		}
		public int UserId
		{
			get;
			set;
		}
		public string Username
		{
			get;
			set;
		}
		public string MobilePIN
		{
			get;
			set;
		}
		public bool IsAnonymous
		{
			get;
			private set;
		}
		public DateTime LastActivityDate
		{
			get
			{
				return this.lastActivityDate;
			}
			set
			{
				this.lastActivityDate = value;
				if (this.Membership != null)
				{
					this.Membership.LastActivityDate = value;
				}
			}
		}
		public string Password
		{
			get;
			set;
		}

        public string PasswordSalt
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

		public MembershipPasswordFormat PasswordFormat
		{
			get;
			set;
		}
		public string Email
		{
			get
			{
				return this.email;
			}
			set
			{
				this.email = value;
				if (this.Membership != null)
				{
					this.Membership.Email = value;
				}
			}
		}
		public string PasswordQuestion
		{
			get;
			private set;
		}
		public bool IsApproved
		{
			get
			{
				return this.isApproved;
			}
			set
			{
				this.isApproved = value;
				if (this.Membership != null)
				{
					this.Membership.IsApproved = value;
				}
			}
		}
		public bool IsLockedOut
		{
			get;
			private set;
		}
		public DateTime CreateDate
		{
			get;
            set;
		}
		public DateTime LastLoginDate
		{
			get
			{
				return this.lastLoginDate;
			}
			set
			{
				this.lastLoginDate = value;
				if (this.Membership != null)
				{
					this.Membership.LastLoginDate = value;
				}
			}
		}
		public DateTime LastPasswordChangedDate
		{
			get;
			private set;
		}
		public DateTime LastLockoutDate
		{
			get;
			private set;
		}
		public string Comment
		{
			get
			{
				return this.comment;
			}
			set
			{
				this.comment = value;
				if (this.Membership != null)
				{
					this.Membership.Comment = value;
				}
			}
		}
		public Gender Gender
		{
			get;
			set;
		}
		public DateTime? BirthDate
		{
			get;
			set;
		}
		public bool IsOpenBalance
		{
			get;
			set;
		}
		public string TradePassword
		{
			get;
			set;
		}
		public string AliOpenId
		{
			get;
			set;
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
        public int SupplierId//供应商Id，数据库中不在表上
        {
            get;
            set;
        }

        public int StoreId//门店Id，数据库中不在表上
        {
            get;
            set;
        }

		public MembershipPasswordFormat TradePasswordFormat
		{
			get;
			set;
		}
		public UserRole UserRole
		{
			get;
			private set;
		}
		public HiMembershipUser(bool isAnonymous, UserRole userRole)
		{
			if (isAnonymous && userRole != UserRole.Anonymous)
			{
				throw new Exception(string.Format("Current user is Anonymous, But the user role is '{0}'", userRole.ToString()));
			}
			this.UserRole = userRole;
			this.IsAnonymous = (userRole == UserRole.Anonymous);
		}
		public HiMembershipUser(bool isAnonymous, UserRole userRole, MembershipUser mu) : this(isAnonymous, userRole)
		{
			this.RefreshMembershipUser(mu);
		}
		public void RefreshMembershipUser(MembershipUser mu)
		{
			if (mu == null)
			{
				throw new Exception("A null MembershipUser is not valid to instantiate a new User");
			}
			this.Membership = mu;
			this.Username = mu.UserName;
			this.UserId = (int)mu.ProviderUserKey;
			this.Comment = mu.Comment;
			this.LastLockoutDate = mu.LastLockoutDate;
			this.LastPasswordChangedDate = mu.LastPasswordChangedDate;
			this.LastLoginDate = mu.LastLoginDate;
			this.CreateDate = mu.CreationDate;
			this.IsLockedOut = mu.IsLockedOut;
			this.IsApproved = mu.IsApproved;
			this.PasswordQuestion = mu.PasswordQuestion;
			this.Email = mu.Email;
			this.LastActivityDate = mu.LastActivityDate;
		}
		public virtual bool ChangePassword(string password, string newPassword)
		{
			return this.Membership.ChangePassword(password, newPassword);
		}
		public virtual bool ChangePasswordWithAnswer(string answer, string newPassword)
		{
			bool result;
			try
			{
				string text = this.ResetPassword(answer);
				if (string.IsNullOrEmpty(text))
				{
					result = false;
				}
				else
				{
					result = this.ChangePassword(text, newPassword);
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public virtual string ResetPassword(string answer)
		{
			string result;
			try
			{
				if (this.ValidatePasswordAnswer(answer))
				{
					result = this.Membership.ResetPassword();
				}
				else
				{
					result = null;
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		public virtual bool ChangePasswordQuestionAndAnswer(string oldAnswer, string newQuestion, string newAnswer)
		{
			bool result;
			if (string.IsNullOrEmpty(newQuestion) || string.IsNullOrEmpty(newAnswer))
			{
				result = false;
			}
			else
			{
				if (newQuestion.Length > 256 || newAnswer.Length > 128)
				{
					result = false;
				}
				else
				{
					if (this.ValidatePasswordAnswer(oldAnswer))
					{
						MemberUserProvider memberUserProvider = MemberUserProvider.Instance();
						result = memberUserProvider.ChangePasswordQuestionAndAnswer(this.Username, newQuestion, newAnswer);
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}
		public bool ChangePasswordQuestionAndAnswer(string newQuestion, string newAnswer)
		{
			bool result;
			if (string.IsNullOrEmpty(newQuestion) || string.IsNullOrEmpty(newAnswer))
			{
				result = false;
			}
			else
			{
				if (newQuestion.Length > 256 || newAnswer.Length > 128)
				{
					result = false;
				}
				else
				{
					if (!string.IsNullOrEmpty(this.PasswordQuestion))
					{
						result = false;
					}
					else
					{
						MemberUserProvider memberUserProvider = MemberUserProvider.Instance();
						result = memberUserProvider.ChangePasswordQuestionAndAnswer(this.Username, newQuestion, newAnswer);
					}
				}
			}
			return result;
		}
		public virtual bool ValidatePasswordAnswer(string answer)
		{
			MemberUserProvider memberUserProvider = MemberUserProvider.Instance();
			return memberUserProvider.ValidatePasswordAnswer(this.Username, answer);
		}

        public string HeadImgUrl { get; set; }


        public UserType UserType { get; set; }


	}
}

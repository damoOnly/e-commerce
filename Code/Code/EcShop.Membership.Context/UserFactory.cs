using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using System;
namespace EcShop.Membership.Context
{
	internal abstract class UserFactory
	{
		public abstract bool Create(IUser userToCreate);
		public abstract IUser GetUser(HiMembershipUser membershipUser);
		public abstract bool UpdateUser(IUser user);
		public abstract bool ChangeTradePassword(string username, string oldPassword, string newPassword);
		public abstract bool ValidTradePassword(string username, string password);
		public abstract string ResetTradePassword(string username);
		public abstract bool ChangeTradePassword(string username, string newPassword);
		public abstract bool OpenBalance(int userId, string tradePassword);
		public static UserFactory Create(UserRole role)
		{
			UserFactory result;
			if (role == UserRole.Member)
			{
				result = MemberFactory.Instance();
			}
			else
			{
				if (role == UserRole.SiteManager)
				{
					result = ManagerFactory.Instance();
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
	}
}

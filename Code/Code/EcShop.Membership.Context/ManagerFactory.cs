using EcShop.Membership.Core;
using System;
namespace EcShop.Membership.Context
{
	internal class ManagerFactory : UserFactory
	{
		private static readonly ManagerFactory _defaultInstance;
		private BizActorProvider provider;
		private ManagerFactory()
		{
		}
		static ManagerFactory()
		{
			ManagerFactory._defaultInstance = new ManagerFactory();
			ManagerFactory._defaultInstance.provider = BizActorProvider.Instance();
		}
		public static ManagerFactory Instance()
		{
			return ManagerFactory._defaultInstance;
		}
		public override bool Create(IUser userToCreate)
		{
			bool result;
			try
			{
				result = this.provider.CreateManager(userToCreate as SiteManager);
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public override IUser GetUser(HiMembershipUser membershipUser)
		{
			return this.provider.GetManager(membershipUser);
		}
		public override bool UpdateUser(IUser user)
		{
			return true;
		}
		public override bool ChangeTradePassword(string username, string oldPassword, string newPassword)
		{
			return true;
		}
		public override bool ValidTradePassword(string username, string password)
		{
			return true;
		}
		public override bool ChangeTradePassword(string username, string newPassword)
		{
			return true;
		}
		public override string ResetTradePassword(string username)
		{
			return "000000";
		}
		public override bool OpenBalance(int userId, string tradePassword)
		{
			return true;
		}
	}
}

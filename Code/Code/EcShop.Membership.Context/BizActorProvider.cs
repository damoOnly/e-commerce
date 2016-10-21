using EcShop.Core;
using EcShop.Membership.Core;
using System;
namespace EcShop.Membership.Context
{
	public abstract class BizActorProvider
	{
		private static readonly BizActorProvider _defaultInstance;
		static BizActorProvider()
		{
			BizActorProvider._defaultInstance = (DataProviders.CreateInstance("EcShop.Membership.Data.BizActorData,EcShop.Membership.Data") as BizActorProvider);
		}
		public static BizActorProvider Instance()
		{
			return BizActorProvider._defaultInstance;
		}
		public abstract Member GetMember(HiMembershipUser membershipUser);
		public abstract bool CreateMember(Member member);
		public abstract bool UpdateMember(Member member);
		public abstract bool ChangeMemberTradePassword(string username, string oldPassword, string newPassword);
		public abstract bool ValidMemberTradePassword(string username, string password);
		public abstract SiteManager GetManager(HiMembershipUser membershipUser);
		public abstract bool CreateManager(SiteManager manager);
	}
}

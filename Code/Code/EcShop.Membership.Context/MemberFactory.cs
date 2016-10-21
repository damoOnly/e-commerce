using EcShop.Membership.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
namespace EcShop.Membership.Context
{
    using System.Web.Security;//ÐÞ¸Ä1
	internal class MemberFactory : UserFactory
	{
		private static readonly MemberFactory _defaultInstance;
		private BizActorProvider provider;
		private MemberFactory()
		{
		}
		static MemberFactory()
		{
			MemberFactory._defaultInstance = new MemberFactory();
			MemberFactory._defaultInstance.provider = BizActorProvider.Instance();
		}
		public static MemberFactory Instance()
		{
			return MemberFactory._defaultInstance;
		}
		public override bool Create(IUser userToCreate)
		{
			bool result;
			try
			{
				result = this.provider.CreateMember(userToCreate as Member);
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public override IUser GetUser(HiMembershipUser membershipUser)
		{
			return this.provider.GetMember(membershipUser);
		}
		public override bool UpdateUser(IUser user)
		{
			return this.provider.UpdateMember(user as Member);
		}
		public override bool ChangeTradePassword(string username, string oldPassword, string newPassword)
		{
			return this.provider.ChangeMemberTradePassword(username, oldPassword, newPassword);
		}
		public override bool ValidTradePassword(string username, string password)
		{
			return this.provider.ValidMemberTradePassword(username, password);
		}
		public override bool ChangeTradePassword(string username, string newPassword)
		{
			SiteManager siteManager = HiContext.Current.User as SiteManager;
			bool result;
			if (siteManager == null)
			{
				result = false;
			}
			else
			{
				string oldPassword = this.ResetTradePassword(username);
				result = this.ChangeTradePassword(username, oldPassword, newPassword);
			}
			return result;
		}
		public override bool OpenBalance(int userId, string tradePassword)
		{
			Database database = DatabaseFactory.CreateDatabase();
			System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_Members SET IsOpenBalance = 'true', TradePassword = @TradePassword, TradePasswordSalt = @TradePasswordSalt, TradePasswordFormat = @TradePasswordFormat WHERE UserId = @UserId");
			string text = UserHelper.CreateSalt();
			database.AddInParameter(sqlStringCommand, "TradePassword", System.Data.DbType.String, UserHelper.EncodePassword(MembershipPasswordFormat.Hashed, tradePassword, text));
			database.AddInParameter(sqlStringCommand, "TradePasswordSalt", System.Data.DbType.String, text);
			database.AddInParameter(sqlStringCommand, "TradePasswordFormat", System.Data.DbType.Int32, 1);
			database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.String, userId);
			return database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override string ResetTradePassword(string username)
		{
			string text = Membership.GeneratePassword(10, 0);
			int num;
			string text2;
			MemberFactory.GetTradePassword(username, out num, out text2);
			string text3 = UserHelper.EncodePassword((MembershipPasswordFormat)num, text, text2);
			string result;
			if (text3.Length > 128)
			{
				result = null;
			}
			else
			{
				Database database = DatabaseFactory.CreateDatabase();
				System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_Members SET TradePassword = @NewTradePassword, TradePasswordSalt = @PasswordSalt, TradePasswordFormat = @PasswordFormat WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
				database.AddInParameter(sqlStringCommand, "NewTradePassword", System.Data.DbType.String, text3);
				database.AddInParameter(sqlStringCommand, "PasswordSalt", System.Data.DbType.String, text2);
				database.AddInParameter(sqlStringCommand, "PasswordFormat", System.Data.DbType.Int32, num);
				database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, username);
				database.ExecuteNonQuery(sqlStringCommand);
				result = text;
			}
			return result;
		}
		private static void GetTradePassword(string username, out int passwordFormat, out string passwordSalt)
		{
			passwordFormat = 0;
			passwordSalt = null;
			Database database = DatabaseFactory.CreateDatabase();
			System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT TradePasswordFormat, TradePasswordSalt FROM aspnet_Members WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
			database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, username);
			using (System.Data.IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					passwordFormat = dataReader.GetInt32(0);
					passwordSalt = dataReader.GetString(1);
				}
			}
		}
	}
}

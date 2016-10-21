using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
namespace EcShop.Membership.Data
{
    using System.Web.Security;//ÐÞ¸Ä1
	public class BizActorData : BizActorProvider
	{
		private Database database;
		public BizActorData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override SiteManager GetManager(HiMembershipUser membershipUser)
		{
			SiteManager result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(UserId) FROM aspnet_Managers WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, membershipUser.UserId);
			if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) == 1)
			{
				result = new SiteManager(membershipUser);
			}
			return result;
		}
		public override bool CreateManager(SiteManager manager)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Managers (UserId) VALUES (@UserId)");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, manager.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override Member GetMember(HiMembershipUser membershipUser)
		{
			Member member = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, membershipUser.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					member = new Member(UserRole.Member, membershipUser);
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

                    if (dataReader["IsVerify"] != DBNull.Value)
                    {
                        member.IsVerify = (int)dataReader["IsVerify"];
                    }
				}
			}
			return member;
		}
		public override bool CreateMember(Member member)
		{
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Members (UserId, GradeId,ReferralUserId, TradePassword, TradePasswordSalt, TradePasswordFormat, OrderNumber, Expenditure, Points, Balance, TopRegionId, RegionId, RealName, Address, Zipcode, TelPhone, CellPhone, QQ, Wangwang, MSN, OpenId, SessionId,CellPhoneVerification,EmailVerification,IsVerify,VerifyDate) VALUES (@UserId, @GradeId, @ReferralUserId, @TradePassword, @TradePasswordSalt, @TradePasswordFormat, @OrderNumber, @Expenditure, @Points, @Balance, @TopRegionId, @RegionId, @RealName, @Address, @Zipcode, @TelPhone, @CellPhone, @QQ, @Wangwang, @MSN, @OpenId, @SessionId,@CellPhoneVerification,@EmailVerification,@IsVerify,@VerifyDate)");
			string text = UserHelper.CreateSalt();
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, member.UserId);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, member.GradeId);
			this.database.AddInParameter(sqlStringCommand, "ReferralUserId", System.Data.DbType.Int32, member.ReferralUserId);
			this.database.AddInParameter(sqlStringCommand, "TradePassword", System.Data.DbType.String, UserHelper.EncodePassword(member.TradePasswordFormat, member.TradePassword, text));
			this.database.AddInParameter(sqlStringCommand, "TradePasswordSalt", System.Data.DbType.String, text);
			this.database.AddInParameter(sqlStringCommand, "TradePasswordFormat", System.Data.DbType.Int32, member.TradePasswordFormat);
			this.database.AddInParameter(sqlStringCommand, "OrderNumber", System.Data.DbType.Int32, member.OrderNumber);
			this.database.AddInParameter(sqlStringCommand, "Expenditure", System.Data.DbType.Currency, member.Expenditure);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, member.Points);
			this.database.AddInParameter(sqlStringCommand, "Balance", System.Data.DbType.Currency, member.Balance);
			this.database.AddInParameter(sqlStringCommand, "TopRegionId", System.Data.DbType.Int32, member.TopRegionId);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, member.RegionId);
			this.database.AddInParameter(sqlStringCommand, "RealName", System.Data.DbType.String, member.RealName);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, member.Address);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, member.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, member.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, member.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "QQ", System.Data.DbType.String, member.QQ);
			this.database.AddInParameter(sqlStringCommand, "Wangwang", System.Data.DbType.String, member.Wangwang);
			this.database.AddInParameter(sqlStringCommand, "MSN", System.Data.DbType.String, member.MSN);
			this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, member.OpenId);
			this.database.AddInParameter(sqlStringCommand, "SessionId", System.Data.DbType.String, member.SessionId);
			this.database.AddInParameter(sqlStringCommand, "CellPhoneVerification", System.Data.DbType.String, member.CellPhoneVerification);
			this.database.AddInParameter(sqlStringCommand, "EmailVerification", System.Data.DbType.String, member.EmailVerification);

            this.database.AddInParameter(sqlStringCommand, "IsVerify", System.Data.DbType.Int32, member.IsVerify);
            this.database.AddInParameter(sqlStringCommand, "VerifyDate", System.Data.DbType.DateTime, member.VerifyDate);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateMember(Member member)
		{
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET GradeId = @GradeId, ReferralStatus = @ReferralStatus, ReferralReason = @ReferralReason, ReferralRequetsDate = @ReferralRequetsDate, RefusalReason = @RefusalReason, ReferralAuditDate = @ReferralAuditDate, IsOpenBalance = @IsOpenBalance, TopRegionId=@TopRegionId, RegionId = @RegionId, RealName = @RealName, IdentityCard = @IdentityCard, Address = @Address, CellPhone = @CellPhone, QQ = @QQ, Wangwang = @Wangwang,WeChat = @WeChat, VipCardNumber = @VipCardNumber, VipCardDate = getdate(),MSN = @MSN, OpenId = @OpenId,EmailVerification=@EmailVerification, CellPhoneVerification=@CellPhoneVerification, SessionId = @SessionId,TelPhone=@TelPhone,IsVerify=@IsVerify,VerifyDate=@VerifyDate WHERE UserId = @UserId");
			string text = UserHelper.CreateSalt();
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, member.UserId);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, member.GradeId);
			this.database.AddInParameter(sqlStringCommand, "ReferralStatus", System.Data.DbType.Int32, member.ReferralStatus);
			this.database.AddInParameter(sqlStringCommand, "ReferralReason", System.Data.DbType.String, member.ReferralReason);
			this.database.AddInParameter(sqlStringCommand, "ReferralRequetsDate", System.Data.DbType.DateTime, member.ReferralRequetsDate);
			this.database.AddInParameter(sqlStringCommand, "RefusalReason", System.Data.DbType.String, member.RefusalReason);
			this.database.AddInParameter(sqlStringCommand, "ReferralAuditDate", System.Data.DbType.DateTime, member.ReferralAuditDate);
			this.database.AddInParameter(sqlStringCommand, "IsOpenBalance", System.Data.DbType.Boolean, member.IsOpenBalance);
			this.database.AddInParameter(sqlStringCommand, "TopRegionId", System.Data.DbType.Int32, member.TopRegionId);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, member.RegionId);
			this.database.AddInParameter(sqlStringCommand, "RealName", System.Data.DbType.String, member.RealName);
			this.database.AddInParameter(sqlStringCommand, "IdentityCard", System.Data.DbType.String, member.IdentityCard);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, member.Address);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, member.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, member.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, member.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "QQ", System.Data.DbType.String, member.QQ);
			this.database.AddInParameter(sqlStringCommand, "Wangwang", System.Data.DbType.String, member.Wangwang);
			this.database.AddInParameter(sqlStringCommand, "MSN", System.Data.DbType.String, member.MSN);
			this.database.AddInParameter(sqlStringCommand, "WeChat", System.Data.DbType.String, member.WeChat);
			this.database.AddInParameter(sqlStringCommand, "VipCardNumber", System.Data.DbType.String, member.VipCardNumber);
			this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, member.OpenId);
			this.database.AddInParameter(sqlStringCommand, "EmailVerification", System.Data.DbType.Boolean, member.EmailVerification);
			this.database.AddInParameter(sqlStringCommand, "CellPhoneVerification", System.Data.DbType.Boolean, member.CellPhoneVerification);
			this.database.AddInParameter(sqlStringCommand, "SessionId", System.Data.DbType.String, member.SessionId);
            this.database.AddInParameter(sqlStringCommand, "IsVerify", System.Data.DbType.Int32, member.IsVerify);
            this.database.AddInParameter(sqlStringCommand, "VerifyDate", System.Data.DbType.DateTime, member.VerifyDate);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override bool ChangeMemberTradePassword(string username, string oldPassword, string newPassword)
		{
			return this.ChangeTradePassword("aspnet_Members", username, oldPassword, newPassword);
		}
		public override bool ValidMemberTradePassword(string username, string password)
		{
			return this.CheckTradePassword("aspnet_Members", username, password);
		}
		private bool ChangeTradePassword(string tableName, string username, string oldPassword, string newPassword)
		{
			string text;
			int num;
			bool result;
			if (!this.CheckTradePassword(tableName, username, oldPassword, out text, out num))
			{
				result = false;
			}
			else
			{
				MembershipProvider provider = Membership.Provider;
				if (newPassword.Length < provider.MinRequiredPasswordLength || newPassword.Length > 128)
				{
					result = false;
				}
				else
				{
					int num2 = 0;
					for (int i = 0; i < newPassword.Length; i++)
					{
						if (!char.IsLetterOrDigit(newPassword, i))
						{
							num2++;
						}
					}
					if (num2 < provider.MinRequiredNonAlphanumericCharacters)
					{
						result = false;
					}
					else
					{
						if (provider.PasswordStrengthRegularExpression.Length > 0)
						{
							if (!Regex.IsMatch(newPassword, provider.PasswordStrengthRegularExpression))
							{
								result = false;
								return result;
							}
						}
						string text2 = UserHelper.EncodePassword((MembershipPasswordFormat)num, newPassword, text);
						if (text2.Length > 128)
						{
							result = false;
						}
						else
						{
							System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE " + tableName + " SET TradePassword = @TradePassword, TradePasswordSalt = @TradePasswordSalt, TradePasswordFormat = @TradePasswordFormat WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
							this.database.AddInParameter(sqlStringCommand, "TradePassword", System.Data.DbType.String, text2);
							this.database.AddInParameter(sqlStringCommand, "TradePasswordSalt", System.Data.DbType.String, text);
							this.database.AddInParameter(sqlStringCommand, "TradePasswordFormat", System.Data.DbType.Int32, num);
							this.database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, username);
							result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
						}
					}
				}
			}
			return result;
		}
		private bool CheckTradePassword(string tableName, string username, string password)
		{
			string text;
			int num;
			return this.CheckTradePassword(tableName, username, password, out text, out num);
		}
		private bool CheckTradePassword(string tableName, string username, string password, out string salt, out int passwordFormat)
		{
			bool flag;
			string text;
			this.GetPasswordWithFormat(tableName, username, out flag, out passwordFormat, out salt, out text);
			bool result;
			if (!flag)
			{
				result = false;
			}
			else
			{
                string value = UserHelper.EncodePassword((MembershipPasswordFormat)passwordFormat, password, salt);//ÐÞ¸Ä1
				result = text.Equals(value);
			}
			return result;
		}
		private void GetPasswordWithFormat(string tableName, string username, out bool success, out int passwordFormat, out string passwordSalt, out string passwordFromDb)
		{
			passwordFormat = 0;
			passwordSalt = null;
			passwordFromDb = null;
			success = false;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT biz.TradePasswordFormat, biz.TradePasswordSalt, biz.TradePassword FROM " + tableName + " AS biz INNER JOIN aspnet_Users AS u ON biz.UserId = u.UserId WHERE u.LoweredUserName = LOWER(@Username)");
			this.database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, username);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					passwordFormat = dataReader.GetInt32(0);
					passwordSalt = dataReader.GetString(1);
					passwordFromDb = dataReader.GetString(2);
					success = true;
				}
			}
		}
	}
}

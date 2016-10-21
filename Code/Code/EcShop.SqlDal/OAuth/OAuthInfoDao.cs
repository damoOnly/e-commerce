using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.OAuth;
using EcShop.Membership.Context;

namespace EcShop.SqlDal.OAuth
{
    public class OAuthInfoDao
    {
        private Database database;

        public OAuthInfoDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

        public OAuthInfo CreateOAuthInfo(string deviceId, int deviceType, int channel, string appId, string appSecret)
        {
            DbCommand command = this.database.GetSqlStringCommand("INSERT INTO Ecshop_OAuth(AppId, AppSecret, DeviceType, DeviceId, Channel) VALUES (@AppId, @AppSecret, @DeviceType, @DeviceId, @Channel)");

            this.database.AddInParameter(command, "AppId", DbType.String, appId);
            this.database.AddInParameter(command, "AppSecret", DbType.String, appSecret);
            this.database.AddInParameter(command, "DeviceType", DbType.Int32, deviceType);
            this.database.AddInParameter(command, "DeviceId", DbType.String, deviceId);
            this.database.AddInParameter(command, "Channel", DbType.Int32, channel);
            
            if (this.database.ExecuteNonQuery(command) > 0)
                return GetOAuthInfo(appId);

            return null;
        }

        public OAuthInfo CreateOAuthInfo(string deviceId, int deviceType, int channel, string ver, string appId, string appSecret)
        {
            DbCommand command = this.database.GetSqlStringCommand("INSERT INTO Ecshop_OAuth(AppId, AppSecret, DeviceType, DeviceId, Channel, ApiVersion) VALUES (@AppId, @AppSecret, @DeviceType, @DeviceId, @Channel, @ApiVersion)");

            this.database.AddInParameter(command, "AppId", DbType.String, appId);
            this.database.AddInParameter(command, "AppSecret", DbType.String, appSecret);
            this.database.AddInParameter(command, "DeviceType", DbType.Int32, deviceType);
            this.database.AddInParameter(command, "DeviceId", DbType.String, deviceId);
            this.database.AddInParameter(command, "Channel", DbType.Int32, channel);
            this.database.AddInParameter(command, "ApiVersion", DbType.String, ver);

            if (this.database.ExecuteNonQuery(command) > 0)
                return GetOAuthInfo(appId);

            return null;
        }

        public bool Update(OAuthInfo oAuthInfo)
        {
            DbCommand command = this.database.GetSqlStringCommand("UPDATE Ecshop_OAuth SET Token = @Token, ExpirationUtc = @ExpirationUtc, IssueDateUtc = @IssueDateUtc, SessionKey = @SessionKey, SessionSecret = @SessionSecret WHERE Id = @Id");

            this.database.AddInParameter(command, "Token", DbType.String, oAuthInfo.Token);
            this.database.AddInParameter(command, "ExpirationUtc", DbType.DateTime, oAuthInfo.ExpirationUtc);
            this.database.AddInParameter(command, "IssueDateUtc", DbType.DateTime, oAuthInfo.IssueDateUtc);
            this.database.AddInParameter(command, "Id", DbType.Int32, oAuthInfo.Id);
            this.database.AddInParameter(command, "SessionKey", DbType.String, oAuthInfo.SessionKey);
            this.database.AddInParameter(command, "SessionSecret", DbType.String, oAuthInfo.SessionSecret);

            return (this.database.ExecuteNonQuery(command) > 0);

        }

		public OAuthInfo GetOAuthInfo(string appId)
		{
			string sql = "SELECT * FROM Ecshop_OAuth WHERE AppId = @AppId";

            DbCommand command = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(command, "AppId", DbType.String, appId);

            OAuthInfo oauthInfo = null;

            using (IDataReader dataReader = this.database.ExecuteReader(command))
			{
				if (dataReader.Read())
				{
                    oauthInfo = new OAuthInfo();
					oauthInfo.Id = (int)dataReader["Id"];
                    oauthInfo.AppId = appId;

					oauthInfo.AppSecret = "";
    				if (dataReader["AppSecret"] != DBNull.Value)
					{
						oauthInfo.AppSecret = (string)dataReader["AppSecret"];
					}

				    oauthInfo.Token = "";
    				if (dataReader["Token"] != DBNull.Value)
					{
						oauthInfo.Token = (string)dataReader["Token"];
					}

                    oauthInfo.ExpirationUtc = new Nullable<DateTime>();
					if (dataReader["ExpirationUtc"] != DBNull.Value)
					{
						oauthInfo.ExpirationUtc = (DateTime)dataReader["ExpirationUtc"];
					}

                    oauthInfo.IssueDateUtc = new Nullable<DateTime>();
                    if (dataReader["IssueDateUtc"] != DBNull.Value)
					{
                        oauthInfo.IssueDateUtc = (DateTime)dataReader["IssueDateUtc"];
					}

                    oauthInfo.DeviceId = "";
					if (dataReader["DeviceId"] != DBNull.Value)
					{
						oauthInfo.DeviceId = (string)dataReader["DeviceId"];
					}

                    oauthInfo.DeviceType = -1;
					if (dataReader["DeviceType"] != DBNull.Value)
					{
						oauthInfo.DeviceType = (int)dataReader["DeviceType"];
					}

                    oauthInfo.Channel = -1;
					if (dataReader["Channel"] != DBNull.Value)
					{
                        oauthInfo.Channel = (int)dataReader["Channel"];
					}

                    oauthInfo.ApiVersion = "";
					if (dataReader["ApiVersion"] != DBNull.Value)
					{
						oauthInfo.ApiVersion = (string)dataReader["ApiVersion"];
					}

                    oauthInfo.SessionKey = "";
                    if (dataReader["SessionKey"] != DBNull.Value)
                    {
                        oauthInfo.SessionKey = (string)dataReader["SessionKey"];
                    }

                    oauthInfo.SessionSecret = "";
                    if (dataReader["SessionSecret"] != DBNull.Value)
                    {
                        oauthInfo.SessionSecret = (string)dataReader["SessionSecret"];
                    }
				}
			}

			return oauthInfo;
		}

        public List<OAuthInfo> GetOnlineOAuthInfo()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_OAuth WHERE ExpirationUtc >= @ExpirationUtc");
            this.database.AddInParameter(sqlStringCommand, "ExpirationUtc", DbType.DateTime, DateTime.UtcNow);

            List<OAuthInfo> online = new List<OAuthInfo>();

            OAuthInfo oauthInfo = null;

            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    oauthInfo = new OAuthInfo();
                    oauthInfo.Id = (int)dataReader["Id"];
                    oauthInfo.AppId = (string)dataReader["AppId"];

                    oauthInfo.AppSecret = "";
                    if (dataReader["AppSecret"] != DBNull.Value)
                    {
                        oauthInfo.AppSecret = (string)dataReader["AppSecret"];
                    }

                    oauthInfo.Token = "";
                    if (dataReader["Token"] != DBNull.Value)
                    {
                        oauthInfo.Token = (string)dataReader["Token"];
                    }

                    oauthInfo.ExpirationUtc = new Nullable<DateTime>();
                    if (dataReader["ExpirationUtc"] != DBNull.Value)
                    {
                        oauthInfo.ExpirationUtc = (DateTime)dataReader["ExpirationUtc"];
                    }

                    oauthInfo.IssueDateUtc = new Nullable<DateTime>();
                    if (dataReader["IssueDateUtc"] != DBNull.Value)
                    {
                        oauthInfo.IssueDateUtc = (DateTime)dataReader["IssueDateUtc"];
                    }

                    oauthInfo.DeviceId = "";
                    if (dataReader["DeviceId"] != DBNull.Value)
                    {
                        oauthInfo.DeviceId = (string)dataReader["DeviceId"];
                    }

                    oauthInfo.DeviceType = -1;
                    if (dataReader["DeviceType"] != DBNull.Value)
                    {
                        oauthInfo.DeviceType = (int)dataReader["DeviceType"];
                    }

                    oauthInfo.Channel = -1;
                    if (dataReader["Channel"] != DBNull.Value)
                    {
                        oauthInfo.DeviceType = (int)dataReader["Channel"];
                    }

                    oauthInfo.ApiVersion = "";
                    if (dataReader["ApiVersion"] != DBNull.Value)
                    {
                        oauthInfo.ApiVersion = (string)dataReader["ApiVersion"];
                    }

                    oauthInfo.SessionKey = "";
                    if (dataReader["SessionKey"] != DBNull.Value)
                    {
                        oauthInfo.SessionKey = (string)dataReader["SessionKey"];
                    }

                    oauthInfo.SessionSecret = "";
                    if (dataReader["SessionSecret"] != DBNull.Value)
                    {
                        oauthInfo.SessionSecret = (string)dataReader["SessionSecret"];
                    }

                    online.Add(oauthInfo);
                }
            }

            return online;
        }
    }
}

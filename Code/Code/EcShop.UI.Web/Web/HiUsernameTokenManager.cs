using EcShop.Core;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Xml;
namespace EcShop.UI.Web
{
	public class HiUsernameTokenManager : UsernameTokenManager
	{
		public HiUsernameTokenManager()
		{
		}
		public HiUsernameTokenManager(System.Xml.XmlNodeList nodes) : base(nodes)
		{
		}
		protected override string AuthenticateToken(UsernameToken token)
		{
			LoginUserStatus loginUserStatus;
			try
			{
				SiteManager siteManager = Users.GetUser(0, token.Identity.Name, false, false) as SiteManager;
				if (siteManager != null && siteManager.IsAdministrator)
				{
					HiContext arg_29_0 = HiContext.Current;
					siteManager.Password = HiCryptographer.Decrypt(token.Password);
					loginUserStatus = Users.ValidateUser(siteManager);
				}
				else
				{
					loginUserStatus = LoginUserStatus.InvalidCredentials;
				}
			}
			catch
			{
				loginUserStatus = LoginUserStatus.InvalidCredentials;
			}
			if (loginUserStatus == LoginUserStatus.Success)
			{
				return token.Password;
			}
			return HiCryptographer.CreateHash(token.Password);
		}
	}
}

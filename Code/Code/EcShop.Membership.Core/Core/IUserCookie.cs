using System;
using System.Web;
namespace EcShop.Membership.Core
{
	public interface IUserCookie
	{
		void WriteCookie(HttpCookie cookie, int days, bool autoLogin);
		void DeleteCookie(HttpCookie cookie);
	}
}

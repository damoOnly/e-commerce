using System;
namespace EcShop.Membership.Context
{
	public class UserEventArgs : System.EventArgs
	{
		public string Username
		{
			get;
			private set;
		}
		public string Password
		{
			get;
			private set;
		}
		public string DealPassword
		{
			get;
			private set;
		}
		public UserEventArgs(string username, string password, string dealPassword)
		{
			this.Username = username;
			this.Password = password;
			this.DealPassword = dealPassword;
		}
	}
}

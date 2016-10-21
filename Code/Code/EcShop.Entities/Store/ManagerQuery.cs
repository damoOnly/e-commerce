using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Store
{
	public class ManagerQuery : Pagination
	{
		public string Username
		{
			get;
			set;
		}
		public System.Guid RoleId
		{
			get;
			set;
		}
	}
}

using EcShop.Entities.Store;
using System;
namespace EcShop.ControlPanel.Store
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Delegate, Inherited = true, AllowMultiple = true)]
	public class PrivilegeCheckAttribute : Attribute
	{
		private Privilege privilege;
		public Privilege Privilege
		{
			get
			{
				return this.privilege;
			}
		}
		public PrivilegeCheckAttribute(Privilege privilege)
		{
			this.privilege = privilege;
		}
	}
}

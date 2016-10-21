using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.SqlDal.Store;
using System;
using System.Web;
namespace EcShop.ControlPanel.Store
{
	public static class ManagerHelper
	{
		public static CreateUserStatus CreateAdministrator(SiteManager administrator)
		{
			return ManagerHelper.Create(administrator, HiContext.Current.Config.RolesConfiguration.SystemAdministrator);
		}
		public static CreateUserStatus Create(SiteManager managerToCreate, string department)
		{
			CreateUserStatus result;
			if (managerToCreate == null || managerToCreate.UserRole != UserRole.SiteManager)
			{
				result = CreateUserStatus.UnknownFailure;
			}
			else
			{
				string[] roles = new string[]
				{
					HiContext.Current.Config.RolesConfiguration.Manager,
					department
				};
				CreateUserStatus createUserStatus = Users.CreateUser(managerToCreate, roles);
				result = createUserStatus;
			}
			return result;
		}
		public static SiteManager GetManager(int userId)
		{
			IUser user = Users.GetUser(userId, false);
			SiteManager result;
			if (user != null && !user.IsAnonymous && user.UserRole == UserRole.SiteManager)
			{
				result = (user as SiteManager);
			}
			else
			{
				result = null;
			}
			return result;
		}
		public static DbQueryResult GetManagers(ManagerQuery query)
		{
			return new ManagerDao().GetManagers(query);
		}
		public static void ClearRolePrivilege(Guid roleId)
		{
			new ManagerDao().ClearRolePrivilege(roleId);
		}
        public static bool AddSupplierUser(int supplierId, int userId)
        {
            bool result = false;
            if (supplierId > 0 && userId > 0)
            {
              result=  new ManagerDao().AddSupplierUser(supplierId, userId);
            }
            return result;
        }

        public static bool AddStoreIdUser(int storeId, int userId)
        {
            bool result = false;
            if (storeId > 0 && userId > 0)
            {
                result = new ManagerDao().AddStoreIdUser(storeId, userId);
            }
            return result;
        }
		public static bool Delete(int userId)
		{
			SiteManager siteManager = HiContext.Current.User as SiteManager;
			return siteManager.UserId != userId && new ManagerDao().DeleteManager(userId);
		}
		public static bool Update(SiteManager manager)
		{
			return Users.UpdateUser(manager);
		}
		public static LoginUserStatus ValidLogin(SiteManager manager)
		{
			LoginUserStatus result;
			if (manager == null)
			{
				result = LoginUserStatus.InvalidCredentials;
			}
			else
			{
				result = Users.ValidateUser(manager);
			}
			return result;
		}
		public static void CheckPrivilege(Privilege privilege)
		{
			IUser user = HiContext.Current.User;
			if (user.IsAnonymous || user.UserRole != UserRole.SiteManager)
			{
				HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied.aspx?privilege=" + privilege.ToString()));
			}
			else
			{
				SiteManager siteManager = user as SiteManager;
				if (!siteManager.IsAdministrator)
				{
					if (!siteManager.HasPrivilege(privilege.ToString()))
					{
						HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied.aspx?privilege=" + privilege.ToString()));
					}
				}
			}
		}

        //根据用户Id获取门店Id
        public static int GetStoreIdByUserId(int userId)
        {
            return new ManagerDao().GetStoreIdByUserId(userId);
        }

        public static bool CheckIsPrivilege(Privilege privilege)
        {
            bool Flag = true;
            IUser user = HiContext.Current.User;
            if (user.IsAnonymous || user.UserRole != UserRole.SiteManager)
            {
                //没有权限
                Flag = false;
            }
            else
            {
                SiteManager siteManager = user as SiteManager;
                if (!siteManager.IsAdministrator)
                {
                    if (!siteManager.HasPrivilege(privilege.ToString()))
                    {
                        //没有权限
                        Flag = false;
                    }
                }
            }
            return Flag;
        }

	}
}

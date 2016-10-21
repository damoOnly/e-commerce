using EcShop.Entities;
using EcShop.Entities.VShop;
using EcShop.SqlDal.VShop;
using System;
using System.Collections.Generic;
namespace EcShop.ControlPanel.Store
{
	public class AliOHHelper
	{
		public static IList<MenuInfo> GetMenus(ClientType clientType)
		{
			IList<MenuInfo> list = new List<MenuInfo>();
			MenuDao menuDao = new MenuDao();
			IList<MenuInfo> topMenus = menuDao.GetTopMenus(clientType);
			IList<MenuInfo> result;
			if (topMenus == null)
			{
				result = list;
			}
			else
			{
				foreach (MenuInfo current in topMenus)
				{
					list.Add(current);
					IList<MenuInfo> menusByParentId = menuDao.GetMenusByParentId(current.MenuId, clientType);
					if (menusByParentId != null)
					{
						foreach (MenuInfo current2 in menusByParentId)
						{
							list.Add(current2);
						}
					}
				}
				result = list;
			}
			return result;
		}
		public static IList<MenuInfo> GetMenusByParentId(int parentId, ClientType clientType)
		{
			return new MenuDao().GetMenusByParentId(parentId, clientType);
		}
		public static MenuInfo GetMenu(int menuId)
		{
			return new MenuDao().GetMenu(menuId);
		}
		public static IList<MenuInfo> GetTopMenus(ClientType clientType)
		{
			return new MenuDao().GetTopMenus(clientType);
		}
		public static bool CanAddMenu(int parentId, ClientType clientType)
		{
			IList<MenuInfo> menusByParentId = new MenuDao().GetMenusByParentId(parentId, clientType);
			bool result;
			if (menusByParentId == null || menusByParentId.Count == 0)
			{
				result = true;
			}
			else
			{
				if (parentId == 0)
				{
					result = (menusByParentId.Count < 3);
				}
				else
				{
					result = (menusByParentId.Count < 5);
				}
			}
			return result;
		}
		public static bool UpdateMenu(MenuInfo menu)
		{
			return new MenuDao().UpdateMenu(menu);
		}
		public static bool SaveMenu(MenuInfo menu)
		{
			return new MenuDao().SaveMenu(menu);
		}
		public static bool DeleteMenu(int menuId)
		{
			return new MenuDao().DeleteMenu(menuId);
		}
		public static void SwapMenuSequence(int menuId, bool isUp)
		{
			new MenuDao().SwapMenuSequence(menuId, isUp);
		}
		public static IList<MenuInfo> GetInitMenus(ClientType clientType)
		{
			MenuDao menuDao = new MenuDao();
			IList<MenuInfo> topMenus = menuDao.GetTopMenus(clientType);
			foreach (MenuInfo current in topMenus)
			{
				current.Chilren = menuDao.GetMenusByParentId(current.MenuId, clientType);
				if (current.Chilren == null)
				{
					current.Chilren = new List<MenuInfo>();
				}
			}
			return topMenus;
		}
	}
}

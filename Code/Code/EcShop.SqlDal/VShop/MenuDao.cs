using EcShop.Entities;
using EcShop.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.SqlDal.VShop
{
	public class MenuDao
	{
		private Database database;
		public MenuDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public MenuInfo GetMenu(int menuId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Menu WHERE MenuId = @MenuId");
			this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menuId);
			MenuInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<MenuInfo>(dataReader);
			}
			return result;
		}
		public IList<MenuInfo> GetTopMenus(ClientType clientType)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Menu WHERE ParentMenuId = 0 AND Client = " + (int)clientType + " ORDER BY DisplaySequence ASC");
			IList<MenuInfo> result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MenuInfo>(dataReader);
			}
			return result;
		}
		public bool UpdateMenu(MenuInfo menu)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_Menu SET ParentMenuId = @ParentMenuId, Name = @Name, Type = @Type, ReplyId = @ReplyId, DisplaySequence = @DisplaySequence, Bind = @Bind, [Content] = @Content,Client=@Client WHERE MenuId = @MenuId");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, menu.ParentMenuId);
			this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "Type", DbType.String, menu.Type);
			this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, menu.ReplyId);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, menu.DisplaySequence);
			this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menu.MenuId);
			this.database.AddInParameter(sqlStringCommand, "Bind", DbType.Int32, (int)menu.BindType);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, menu.Content);
			this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)menu.Client);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public bool SaveMenu(MenuInfo menu)
		{
			int allMenusCount = this.GetAllMenusCount(menu.Client);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO vshop_Menu (ParentMenuId, Name, Type, ReplyId, DisplaySequence, Bind, [Content],Client) VALUES(@ParentMenuId, @Name, @Type, @ReplyId, @DisplaySequence, @Bind, @Content,@Client)");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, menu.ParentMenuId);
			this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "Type", DbType.String, menu.Type);
			this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, menu.ReplyId);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, allMenusCount);
			this.database.AddInParameter(sqlStringCommand, "Bind", DbType.Int32, (int)menu.BindType);
			this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, menu.Content);
			this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)menu.Client);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		private int GetAllMenusCount(ClientType client)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(*) from vshop_Menu WHERE Client = " + (int)client);
			return 1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}
		public bool DeleteMenu(int menuId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE vshop_Menu WHERE MenuId = @MenuId");
			this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menuId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public void SwapMenuSequence(int menuId, bool isUp)
		{
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Menu_SwapDisplaySequence");
			this.database.AddInParameter(storedProcCommand, "MenuId", DbType.Int32, menuId);
			this.database.AddInParameter(storedProcCommand, "ZIndex", DbType.Int32, isUp ? 0 : 1);
			this.database.ExecuteNonQuery(storedProcCommand);
		}
		public IList<MenuInfo> GetMenusByParentId(int parentId, ClientType clientType)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Menu WHERE ParentMenuId = @ParentMenuId AND Client = @Client ORDER BY DisplaySequence ASC");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, parentId);
			this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)clientType);
			IList<MenuInfo> result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MenuInfo>(dataReader);
			}
			return result;
		}
	}
}

using EcShop.Membership.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace EcShop.Membership.Data
{
	public class RoleData : MemberRoleProvider
	{
		private Database database;
		public RoleData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override void UpdateRole(RoleInfo role)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Roles SET RoleName = @Name, LoweredRoleName = Lower(@Name), Description = @Description WHERE RoleId = @RoleId");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, role.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, role.Description);
			this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.Guid, role.RoleID);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override RoleInfo GetRole(Guid roleId, string roleName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT RoleId, RoleName as [Name], r.Description FROM aspnet_Roles r  WHERE 1=1");
			if (roleId != Guid.Empty)
			{
				System.Data.Common.DbCommand expr_28 = sqlStringCommand;
				expr_28.CommandText += " AND RoleId = @RoleId";
				this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.Guid, roleId);
			}
			if (!string.IsNullOrEmpty(roleName))
			{
				System.Data.Common.DbCommand expr_67 = sqlStringCommand;
				expr_67.CommandText += " AND RoleName = @RoleName";
				this.database.AddInParameter(sqlStringCommand, "RoleName", System.Data.DbType.String, roleName);
			}
			RoleInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = MemberRoleProvider.PopulateRoleFromIDataReader(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}
		public override ArrayList GetRoles()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT RoleId, RoleName as [Name], Description FROM aspnet_Roles order by RoleId");
			ArrayList arrayList = new ArrayList();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					RoleInfo value = MemberRoleProvider.PopulateRoleFromIDataReader(dataReader);
					arrayList.Add(value);
				}
				dataReader.Close();
			}
			return arrayList;
		}
		public override ArrayList GetRoles(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DISTINCT R.RoleId, RoleName as [Name], Description FROM aspnet_UsersInRoles U, aspnet_Roles R WHERE U.RoleId = R.RoleId AND U.UserId = @UserId order by R.RoleId");
			ArrayList arrayList = new ArrayList();
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					RoleInfo value = MemberRoleProvider.PopulateRoleFromIDataReader(dataReader);
					arrayList.Add(value);
				}
				dataReader.Close();
			}
			return arrayList;
		}
        public override IList<string> GetPrivilegesForUser(string userName)
		{
            IList<string> result;
			if (string.IsNullOrEmpty(userName))
			{
                result = new List<string>();
			}
			else
			{
                IList<string> list = new List<string>();
                System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DISTINCT pm.PrivilegeENName FROM dbo.aspnet_Roles r INNER JOIN dbo.aspnet_UsersInRoles ur ON r.RoleId = ur.RoleId INNER JOIN dbo.Ecshop_PrivilegeInRoles pr ON pr.RoleId = r.RoleId  JOIN dbo.Ecshop_PrivilegeMenu pm ON pr.Privilege=pm.PrivilegeId WHERE ur.UserId = (SELECT UserId FROM dbo.aspnet_Users WHERE   LoweredUserName = LOWER(@UserName))");
				this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, userName);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					while (dataReader.Read())
					{
						list.Add(dataReader.GetString(0));
					}
					dataReader.Close();
				}
				result = list;
			}
			return result;
		}
		public override void AddDeletePrivilegeInRoles(Guid roleId, string privilege)
		{
			string text = string.Format("DELETE FROM Ecshop_PrivilegeInRoles WHERE  RoleId='{0}' ", roleId);
			string[] array = privilege.Split(new char[]
			{
				','
			});
			if (array != null)
			{
				string[] array2 = array;
                int arg=0;
				for (int i = 0; i < array2.Length; i++)
				{
                    int.TryParse(array2[i],out arg);
					text += string.Format(" INSERT INTO Ecshop_PrivilegeInRoles(RoleId,Privilege) VALUES ('{0}', {1})", roleId, arg);
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void DeletePrivilegeInRoles(Guid roleId)
		{
			string query = string.Format("DELETE FROM Ecshop_PrivilegeInRoles WHERE  RoleId='{0}' ", roleId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override IList<int> GetPrivilegeByRoles(Guid roleId)
		{
			IList<int> list = new List<int>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_PrivilegeInRoles WHERE RoleId=@RoleId");
			this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.Guid, roleId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)dataReader["Privilege"]);
				}
			}
			return list;
		}
		public override bool PrivilegeInRoles(Guid roleId, int privilege)
		{
			bool result = false;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_PrivilegeInRoles WHERE RoleId=@RoleId AND Privilege=@Privilege");
			this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.Guid, roleId);
			this.database.AddInParameter(sqlStringCommand, "Privilege", System.Data.DbType.Int32, privilege);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = true;
				}
			}
			return result;
		}
	}
}

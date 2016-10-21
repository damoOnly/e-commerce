using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.PCMenu
{
   public class PcMenuInfoDao
    {
       private Database database;
       public PcMenuInfoDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public DbQueryResult GetBundlingProducts(BundlingInfoQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND Name Like '%{0}%' ", DataHelper.CleanSearchString(query.ProductName));
			}
			string selectFields = "Bundlingid,Name,Num,price,SaleStatus,OrderCount,AddTime,DisplaySequence,ShortDescription";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_BundlingProducts", "Bundlingid", stringBuilder.ToString(), selectFields);
		}
        public List<PCMenuInfo> GetPCFirstMenuInfo(string username)
		{
			StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@"SELECT 
                                    PrivilegeId,
                                    PrivilegeENName,
                                    PrivilegeName,
                                    ParentId,
                                    Status,
                                    SortValue,
                                    LinkUrl,
                                    IsMenu,
                                    levels,0 levelId
                                    FROM Ecshop_PrivilegeMenu a WITH(NOLOCK)
                                    JOIN Ecshop_PrivilegeInRoles b WITH(NOLOCK) ON a.PrivilegeId=b.Privilege
                                     JOIN aspnet_UsersInRoles c WITH(NOLOCK) ON b.RoleId=c.RoleId
                                     JOIN dbo.aspnet_Users d ON c.UserId=d.UserId
                                    WHERE IsMenu IS NOT NULL  AND Status=1 AND ParentId=0  AND UserName=@username order by SortValue ");
//            stringBuilder.Append(@"SELECT 
//                                    PrivilegeId,
//                                    PrivilegeENName,
//                                    PrivilegeName,
//                                    ParentId,
//                                    Status,
//                                    SortValue,
//                                    LinkUrl,
//                                    IsMenu,
//                                    levels,0 levelId
//                                    FROM Ecshop_PrivilegeMenu a WITH(NOLOCK)
//                                   
//                                    WHERE IsMenu IS NOT NULL  AND Status=1 AND ParentId=0  ");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "username", DbType.String, username);
            List<PCMenuInfo> result = new List<PCMenuInfo>();
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
                while (dataReader.Read())
				{
                    result.Add(DataMapper.PcMenuInfoMapper(dataReader));
				}
			}
			return result;
		}
        public List<PCMenuInfo> GetPCALLMenuInfo(int userId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@"SELECT 
                                    PrivilegeId,
                                    PrivilegeENName,
                                    PrivilegeName,
                                    ParentId,
                                    Status,
                                    SortValue,
                                    LinkUrl,
                                    IsMenu,
                                    levels,0 levelId
                                    FROM Ecshop_PrivilegeMenu a
                                    WHERE IsMenu IS NOT NULL  AND Status=1");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());

            List<PCMenuInfo> result = new List<PCMenuInfo>();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    result.Add(DataMapper.PcMenuInfoMapper(dataReader));
                }
            }
            return result;
        }
        public List<PCMenuInfo> GetPCALLMenuInfoAndCheckItem(int userId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@"SELECT 
                                    PrivilegeId,
                                    PrivilegeENName,
                                    PrivilegeName,
                                    ParentId,
                                    Status,
                                    SortValue,
                                    LinkUrl,
                                    IsMenu,
                                    levels,0 levelId
                                    FROM Ecshop_PrivilegeMenu a
                                    WHERE IsMenu IS NOT NULL  AND Status=1");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());

            List<PCMenuInfo> result = new List<PCMenuInfo>();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    result.Add(DataMapper.PcMenuInfoMapper(dataReader));
                }
            }
            return result;
        }
        public List<PCMenuInfo> GetPCMenuInfoByFirstId(string username,int partendId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@";WITH ts 
                                    AS
                                    (
                                    SELECT *,1 levelId  FROM Ecshop_PrivilegeMenu WHERE IsMenu=1 AND ParentId=@partendId
                                    UNION all
                                    SELECT b.*,a.levelId+1 levelId FROM ts a JOIN Ecshop_PrivilegeMenu b ON b.ParentId=a.PrivilegeId WHERE b.IsMenu=1
                                    )
                                    SELECT a.*	
                                    FROM ts a  WITH(NOLOCK)
                                    JOIN Ecshop_PrivilegeInRoles b WITH(NOLOCK) ON a.PrivilegeId=b.Privilege
                                    JOIN aspnet_UsersInRoles c WITH(NOLOCK) ON b.RoleId=c.RoleId
                                    JOIN dbo.aspnet_Users d ON c.UserId=d.UserId
                                    WHERE IsMenu IS NOT NULL  AND Status=1  AND UserName=@username order by SortValue");
//            stringBuilder.Append(@";WITH ts 
//                                                AS
//                                                (
//                                                SELECT *,1 levelId  FROM Ecshop_PrivilegeMenu WHERE IsMenu=1 AND ParentId=@partendId
//                                                UNION all
//                                                SELECT b.*,a.levelId+1 levelId FROM ts a JOIN Ecshop_PrivilegeMenu b ON b.ParentId=a.PrivilegeId WHERE b.IsMenu=1
//                                                )
//                                                SELECT a.*	
//                                                FROM ts a  WITH(NOLOCK)
//                                              
//                                                WHERE IsMenu IS NOT NULL  AND Status=1 ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "partendId", DbType.String, partendId);
            this.database.AddInParameter(sqlStringCommand, "username", DbType.String, username);
            List<PCMenuInfo> result = new List<PCMenuInfo>();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    result.Add(DataMapper.PcMenuInfoMapper(dataReader));
                }
            }
            return result;
        }
        public List<PCMenuInfo> GetPCMenuInfoByRoleId(string roleId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@";WITH ts 
                                    AS
                                    (
                                    SELECT *,1 levelId,CAST(PrivilegeId AS nVARCHAR(50)) newPrivilegeId  FROM Ecshop_PrivilegeMenu WHERE IsMenu IS NOT NULL AND ParentId=0
                                    UNION all
                                    SELECT b.*,a.levelId+1 levelId,CAST(a.newPrivilegeId+','+CAST(b.PrivilegeId AS nVARCHAR(50)) AS nVARCHAR(50)) newPrivilegeId FROM ts a JOIN Ecshop_PrivilegeMenu b ON b.ParentId=a.PrivilegeId
                                    )
                                    SELECT *,
                                    CASE when(SELECT COUNT(1) FROM Ecshop_PrivilegeInRoles  WHERE Privilege=a.PrivilegeId AND RoleId=@roleId) >0 THEN 1 ELSE 0 END checked
                                     FROM ts a
                                    ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "roleId", DbType.String, roleId);
            List<PCMenuInfo> result = new List<PCMenuInfo>();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    result.Add(DataMapper.PcMenuInfoCheckedMapper(dataReader));
                }
            }
            return result;  
        }
		
    }
}

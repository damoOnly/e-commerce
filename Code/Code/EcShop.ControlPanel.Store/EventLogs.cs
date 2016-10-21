using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.SqlDal.Store;
using System;
using System.Collections.Generic;
namespace EcShop.ControlPanel.Store
{
	public static class EventLogs
	{
		public static void WriteOperationLog(Privilege privilege, string description)
		{
			OperationLogEntry entry = new OperationLogEntry
			{
				AddedTime = DateTime.Now,
				Privilege = privilege,
				Description = description,
				IpAddress = Globals.IPAddress,
				PageUrl = HiContext.Current.Context.Request.RawUrl,
				UserName = HiContext.Current.Context.User.Identity.Name
			};
			new LogDao().WriteOperationLogEntry(entry);
		}
		public static int DeleteLogs(string strIds)
		{
			return new LogDao().DeleteLogs(strIds);
		}
		public static bool DeleteLog(long logId)
		{
			return new LogDao().DeleteLog(logId);
		}
		public static bool DeleteAllLogs()
		{
			return new LogDao().DeleteAllLogs();
		}
		public static DbQueryResult GetLogs(OperationLogQuery query)
		{
			return new LogDao().GetLogs(query);
		}
		public static IList<string> GetOperationUseNames()
		{
			return new LogDao().GetOperationUserNames();
		}

        /// <summary>
        /// 获取当前登入的日志数据
        /// </summary>
        /// <param name="uername">会员名</param>
        /// <param name="logintype">登入位置：1 管理员登入 2  微信会员登入3 买家会员登入  4 商家登入</param>
        public static LoginLog GetLoginLogDetails(string username, int logintype)
        {
            return new LogDao().GetLoginLogDetails(username, logintype);
        }

        /// <summary>
        /// 更新登入日志
        /// </summary>
        /// <param name="detail"></param>
        public static bool UpdateLoginLog(LoginLog detail)
        {
            return new LogDao().UpdateLoginLog(detail);
        }
	}
}

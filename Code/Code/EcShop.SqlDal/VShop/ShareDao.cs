using EcShop.Entities;
using EcShop.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.VShop
{
    public class ShareDao
    {
        private Database database;

        public ShareDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 处理分享信息
        /// </summary>
        /// <param name="share"></param>
        /// <returns></returns>
        public bool ShareDeal(ShareInfo share)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("Ecshop_ShareDeal");
            this.database.AddInParameter(storedProcCommand, "SharePerson", DbType.String, share.SharePerson);
            this.database.AddInParameter(storedProcCommand, "ShareContent", DbType.String, share.ShareContent);
            this.database.AddInParameter(storedProcCommand, "Link", DbType.String, share.Link);
            this.database.AddInParameter(storedProcCommand, "ShareType", DbType.Int32, share.ShareType);
            this.database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, share.ProductId);
            this.database.AddInParameter(storedProcCommand, "OrderId", DbType.String, share.OrderId);
            this.database.AddInParameter(storedProcCommand, "ShareUserId", DbType.Int32, share.ShareUserId);
            return this.database.ExecuteNonQuery(storedProcCommand) == 1;
        }
    }
}

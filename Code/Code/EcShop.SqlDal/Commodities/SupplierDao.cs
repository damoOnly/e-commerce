using Commodities;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EcShop
{
    public class SupplierDao
    {
        private Database database;
        public SupplierDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public DataTable GetSupplier()
        {
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Ecshop_Supplier");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public bool GetSupplierApproveKey(int supplierId)
        {
            bool result = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ApproveKey FROM Ecshop_Supplier WHERE supplierId = @supplierId");
            this.database.AddInParameter(sqlStringCommand, "supplierId", DbType.Int32, supplierId);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj != DBNull.Value)
            {
                result = (bool)obj;
            }
            return result;
        }
        public SupplierInfo GetSupplier(int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Supplier WHERE supplierId = @supplierId");
            this.database.AddInParameter(sqlStringCommand, "supplierId", DbType.Int32, supplierId);
            SupplierInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<SupplierInfo>(dataReader);
            }
            return result;
        }

        public AppSupplierInfo GetAppSupplier(int supplierId, int userid)
        {
            StringBuilder strtable = new StringBuilder();
            strtable.Append("select A.*,");
            strtable.Append("(select count(C1.Id) from  Ecshop_SupplierCollect C1 where C1.SupplierId=A.SupplierId) as CollectCount,");
            strtable.AppendFormat("case when (select count(C2.SupplierId) from Ecshop_SupplierCollect C2 where C2.SupplierId=A.SupplierId and C2.Userid={0})>0 then 1 else 0 end as  IsCollect", userid);
            strtable.Append(" from Ecshop_Supplier A ");
            strtable.AppendFormat(" where  A.supplierId = @supplierId ");

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strtable.ToString());
            this.database.AddInParameter(sqlStringCommand, "supplierId", DbType.Int32, supplierId);
            AppSupplierInfo result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<AppSupplierInfo>(dataReader);
            }
            return result;
        }

        public string GetSupplierName(int SupplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT Supplierame  FROM  Ecshop_Supplier WHERE SupplierId = {0}", SupplierId));
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            string result;
            if (obj != null)
            {
                result = obj.ToString();
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }
        public int AddSupplier(SupplierInfo supplierInfo)
        {
            int result = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Supplier([SupplierName],[Phone],[Mobile],[Province],[City],[County],[Address],[Status],[Description],[CreateUser],[ApproveKey],[SupplierCode],[ShipWarehouseName],[ShopOwner],[Logo],[PCImage],[MobileImage],[ShopName],Contact,Email,Fax,Category,BeneficiaryName,SwiftCode,BankAccount,BankName,BankAddress,IBAN,Remark) VALUES(@SupplierName,@Phone,@Mobile,@Province,@City,@County,@Address,@Status,@Description,@CreateUser,@ApproveKey,@SupplierCode,@ShipWarehouseName,@ShopOwner,@Logo,@PCImage,@MobileImage,@ShopName,@Contact,@Email,@Fax,@Category,@BeneficiaryName,@SwiftCode,@BankAccount,@BankName,@BankAddress,@IBAN,@Remark);SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, supplierInfo.SupplierName);
            this.database.AddInParameter(sqlStringCommand, "Phone", DbType.String, supplierInfo.Phone);
            this.database.AddInParameter(sqlStringCommand, "Mobile", DbType.String, supplierInfo.Mobile);
            this.database.AddInParameter(sqlStringCommand, "Province", DbType.Int32, supplierInfo.Province);
            this.database.AddInParameter(sqlStringCommand, "City", DbType.Int32, supplierInfo.City);
            this.database.AddInParameter(sqlStringCommand, "County", DbType.Int32, supplierInfo.County);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, supplierInfo.Address);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, supplierInfo.Status);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, supplierInfo.Description);
            this.database.AddInParameter(sqlStringCommand, "CreateUser", DbType.Int32, supplierInfo.CreateUser);
            this.database.AddInParameter(sqlStringCommand, "ApproveKey", DbType.Boolean, supplierInfo.ApproveKey);
            this.database.AddInParameter(sqlStringCommand, "SupplierCode", DbType.String, supplierInfo.SupplierCode);
            this.database.AddInParameter(sqlStringCommand, "ShipWarehouseName", DbType.String, supplierInfo.ShipWarehouseName);
            this.database.AddInParameter(sqlStringCommand, "ShopOwner", DbType.String, supplierInfo.ShopOwner);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, supplierInfo.@Logo);
            this.database.AddInParameter(sqlStringCommand, "PCImage", DbType.String, supplierInfo.PCImage);
            this.database.AddInParameter(sqlStringCommand, "MobileImage", DbType.String, supplierInfo.MobileImage);
            this.database.AddInParameter(sqlStringCommand, "ShopName", DbType.String, supplierInfo.ShopName);

            this.database.AddInParameter(sqlStringCommand, "Contact", DbType.String, supplierInfo.Contact);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, supplierInfo.Email);
            this.database.AddInParameter(sqlStringCommand, "Fax", DbType.String, supplierInfo.Fax);
            this.database.AddInParameter(sqlStringCommand, "Category", DbType.String, supplierInfo.Category);
            this.database.AddInParameter(sqlStringCommand, "BeneficiaryName", DbType.String, supplierInfo.BeneficiaryName);
            this.database.AddInParameter(sqlStringCommand, "SwiftCode", DbType.String, supplierInfo.SwiftCode);
            this.database.AddInParameter(sqlStringCommand, "BankAccount", DbType.String, supplierInfo.BankAccount);
            this.database.AddInParameter(sqlStringCommand, "BankName", DbType.String, supplierInfo.BankName);
            this.database.AddInParameter(sqlStringCommand, "BankAddress", DbType.String, supplierInfo.BankAddress);
            this.database.AddInParameter(sqlStringCommand, "IBAN", DbType.String, supplierInfo.IBAN);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, supplierInfo.Remark);

            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj != null)
            {
                result = Convert.ToInt32(obj.ToString());
            }
            return result;
        }
        public bool UpdateSupplier(SupplierInfo supplierInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Supplier SET SupplierName=@SupplierName,[Phone]=@Phone,[Mobile]=@Mobile,[County]=@County,[Address]=@Address,[Description]=@Description,[SupplierCode]=@SupplierCode,[ShipWarehouseName]=@ShipWarehouseName,[ShopOwner]=@ShopOwner,[Logo]=@Logo,[PCImage]=@PCImage,[MobileImage]=@MobileImage,[ShopName] = @ShopName,Contact=@Contact,Email=@Email,Fax=@Fax,Category=@Category,BeneficiaryName=@BeneficiaryName,SwiftCode=@SwiftCode,BankAccount=@BankAccount,BankName=@BankName,BankAddress=@BankAddress,IBAN=@IBAN,Remark=@Remark  WHERE SupplierId=@SupplierId");
            this.database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, supplierInfo.SupplierName);
            this.database.AddInParameter(sqlStringCommand, "Phone", DbType.String, supplierInfo.Phone);
            this.database.AddInParameter(sqlStringCommand, "Mobile", DbType.String, supplierInfo.Mobile);
            this.database.AddInParameter(sqlStringCommand, "County", DbType.Int32, supplierInfo.County);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, supplierInfo.Address);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, supplierInfo.Description);
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, supplierInfo.SupplierId);
            // this.database.AddInParameter(sqlStringCommand, "ApproveKey", DbType.Boolean, supplierInfo.ApproveKey);
            this.database.AddInParameter(sqlStringCommand, "SupplierCode", DbType.String, supplierInfo.SupplierCode);
            this.database.AddInParameter(sqlStringCommand, "ShipWarehouseName", DbType.String, supplierInfo.ShipWarehouseName);
            this.database.AddInParameter(sqlStringCommand, "ShopOwner", DbType.String, supplierInfo.ShopOwner);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, supplierInfo.@Logo);
            this.database.AddInParameter(sqlStringCommand, "PCImage", DbType.String, supplierInfo.PCImage);
            this.database.AddInParameter(sqlStringCommand, "MobileImage", DbType.String, supplierInfo.MobileImage);
            this.database.AddInParameter(sqlStringCommand, "ShopName", DbType.String, supplierInfo.ShopName);

            this.database.AddInParameter(sqlStringCommand, "Contact", DbType.String, supplierInfo.Contact);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, supplierInfo.Email);
            this.database.AddInParameter(sqlStringCommand, "Fax", DbType.String, supplierInfo.Fax);
            this.database.AddInParameter(sqlStringCommand, "Category", DbType.String, supplierInfo.Category);
            this.database.AddInParameter(sqlStringCommand, "BeneficiaryName", DbType.String, supplierInfo.BeneficiaryName);
            this.database.AddInParameter(sqlStringCommand, "SwiftCode", DbType.String, supplierInfo.SwiftCode);
            this.database.AddInParameter(sqlStringCommand, "BankAccount", DbType.String, supplierInfo.BankAccount);
            this.database.AddInParameter(sqlStringCommand, "BankName", DbType.String, supplierInfo.BankName);
            this.database.AddInParameter(sqlStringCommand, "BankAddress", DbType.String, supplierInfo.BankAddress);
            this.database.AddInParameter(sqlStringCommand, "IBAN", DbType.String, supplierInfo.IBAN);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, supplierInfo.Remark);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool DeleteSupplier(int SupplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_Supplier WHERE SupplierId=@SupplierId;");
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, SupplierId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }


        public DbQueryResult GetSupplier(SupplierQuery query)
        {
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Ecshop_Supplier", "SupplierId", string.IsNullOrEmpty(query.SupplierName) ? string.Empty : string.Format("SupplierName LIKE '%{0}%'", DataHelper.CleanSearchString(query.SupplierName)), "*");
        }

        /// <summary>
        /// 收藏供应商
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int CollectSupplier(SupplierCollectInfo info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_SupplierCollect(SupplierId, UserId,Remark)VALUES(@SupplierId, @UserId,@Remark);select @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, info.UserId);
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, info.SupplierId);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, info.Remark);

            int result = 0;

            int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(), out result);

            return result;
        }


        /// <summary>
        /// 供应商是否已经收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public bool SupplierIsCollect(int userId, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(SupplierId) FROM Ecshop_SupplierCollect WHERE SupplierId=@SupplierId AND UserId=@UserId ");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, supplierId);

            int result = 0;

            int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(), out result);

            return result > 0;
        }

        /// <summary>
        /// 获取用户供应商收藏数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserSupplierCollectCount(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(SupplierId) FROM Ecshop_SupplierCollect WHERE  UserId=@UserId ");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);

            int result = 0;

            int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(), out result);

            return result;
        }


        /// <summary>
        /// 获取供货商收藏列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetSupplierCollect(SupplierCollectQuery query)
        {
            string strtable = "(select S.*, C.Id,C.UserId,(select count(C1.Id) from  Ecshop_SupplierCollect C1 where C1.SupplierId=C.SupplierId) as CollectCount from Ecshop_SupplierCollect C inner join  Ecshop_Supplier S on S.SupplierId=C.SupplierId) as SupplierCollectTable ";

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat(" UserId = {0}", query.UserId);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, strtable, "Id", stringBuilder.ToString(), "*");
        }


        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public bool DelCollectSupplier(int userId, int supplierId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE  FROM Ecshop_SupplierCollect WHERE SupplierId=@SupplierId AND UserId=@UserId ");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, supplierId);

            int result = this.database.ExecuteNonQuery(sqlStringCommand);

            return result > 0;
        }


        public DbQueryResult GetAppSupplier(SupplierQuery query)
        {
            StringBuilder strtable = new StringBuilder();
            strtable.Append("(");
            strtable.Append("select A.*,");
            strtable.Append("(select count(C1.Id) from  Ecshop_SupplierCollect C1 where C1.SupplierId=A.SupplierId) as CollectCount,");
            strtable.AppendFormat("case when (select count(C2.SupplierId) from Ecshop_SupplierCollect C2 where C2.SupplierId=A.SupplierId and C2.Userid={0})>0 then 1 else 0 end as  IsCollect", query.UserId);
            strtable.Append(" from Ecshop_Supplier A inner join Ecshop_SupplierConfig B on B.SupplierId=A.SupplierId where B.Client=3");
            strtable.Append(") as strtable ");


            StringBuilder stringBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(query.SupplierName))
            {
                stringBuilder.AppendFormat(" ShopName LIKE '%{0}%'", DataHelper.CleanSearchString(query.SupplierName));
            }


            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, strtable.ToString(), "SupplierId", stringBuilder.ToString(), "*");
        }

        /// <summary>
        /// 查询供货商下的管理员
        /// </summary>
        /// <param name="page"></param>
        /// <param name="SupplierId"></param>
        /// <returns></returns>
        public DbQueryResult GetSupplierManager(Pagination page, int SupplierId)
        {
            StringBuilder strtable = new StringBuilder();
            strtable.Append("(");
            strtable.Append("select A.*,B.SupplierId ");
            strtable.Append("from dbo.aspnet_Users A  inner join Ecshop_SupplierUser B on A.userid=B.userid ");
            strtable.Append(") as strtable ");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat(" SupplierId={0} ", SupplierId);

            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, strtable.ToString(), "UserId", stringBuilder.ToString(), "*");

        }


        public DbQueryResult GetWMSSupplier(SupplierQuery query)
        {
            StringBuilder strtable = new StringBuilder();
            strtable.Append("(  SELECT SupplierId,SupplierName,Phone,Mobile,Province,City,County,[Address],[Status],[Description],CreateUser,CreateDate,ApproveKey,SupplierCode,ShipWarehouseName,ShopOwner,Logo,PCImage,MobileImage,ShopName,CAST(DataVersion as bigint) AS DataVersion FROM Ecshop_Supplier ) as strtable");
            StringBuilder stringBuilder = new StringBuilder();
            //
            if (query.DateContrastType.HasValue)
            {
                if (query.DateContrastType.Value == 1)
                {
                    if (query.DateContrastValue.HasValue)
                    {
                        stringBuilder.AppendFormat("  CreateDate BETWEEN DATEADD(hour,-{0},GETDATE())  AND GETDATE() ", query.DateContrastValue.Value);
                    }
                }
                else if (query.DateContrastType.Value == 2)
                {
                    if (query.DateContrastValue.HasValue)
                    {
                        stringBuilder.AppendFormat("  CreateDate BETWEEN DATEADD(day,-{0},GETDATE())  AND GETDATE() ", query.DateContrastValue.Value);
                    }
                }
                else if (query.DateContrastType.Value == 2)
                {
                    if (query.DateContrastValue.HasValue)
                    {
                        stringBuilder.AppendFormat("  CreateDate BETWEEN DATEADD(month,-{0},GETDATE())  AND GETDATE() ", query.DateContrastValue.Value);
                    }
                }

            }
            //
            if (query.DataVersion != "")
            {
                stringBuilder.AppendFormat(" DataVersion > cast({0} as timestamp) ", query.DataVersion);
            }

            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, strtable.ToString(), "SupplierId", stringBuilder.ToString(), "*");
        }
    }
}

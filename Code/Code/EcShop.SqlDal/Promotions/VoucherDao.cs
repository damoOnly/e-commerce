using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Text;

namespace EcShop.SqlDal.Promotions
{
    public class VoucherDao
    {
        private Database database;
        public VoucherDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

        //创建一个现金券模板
        public VoucherActionStatus CreateVoucher(VoucherInfo voucher, int count, out string lotNumber, int pwdType)
		{
            VoucherActionStatus voucherActionStatus = VoucherActionStatus.UnknowError;
			lotNumber = string.Empty;
            VoucherActionStatus result;
			if (count <= 0)
			{
				lotNumber = string.Empty;
                //检查现金券券实体是否为空
                if (null == voucher)
				{
                    voucherActionStatus = VoucherActionStatus.UnknowError;
                    result = voucherActionStatus;
					return result;
				}
                //检查现金券名称是否重复
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT VoucherId  FROM Ecshop_Vouchers WHERE Name=@Name");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, voucher.Name);
				if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
				{
                    voucherActionStatus = VoucherActionStatus.DuplicateName;
                    result = voucherActionStatus;
					return result;
				}

                //新增现金券
                sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Vouchers ([Name],ClosingTime,StartTime, Description, Amount, DiscountValue,SentCount,UsedCount,SendType,SendTypeItem,Validity) VALUES(@Name, @ClosingTime,@StartTime, @Description, @Amount, @DiscountValue,0,0,@SendType,@SendTypeItem,@Validity); SELECT @@IDENTITY");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, voucher.Name);
                this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, voucher.ClosingTime);
                this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, voucher.StartTime);
                this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, voucher.Description);
                this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, voucher.DiscountValue);
                this.database.AddInParameter(sqlStringCommand, "SendType", DbType.Int32, voucher.SendType);
                this.database.AddInParameter(sqlStringCommand, "SendTypeItem", DbType.String, voucher.SendTypeItem);
                this.database.AddInParameter(sqlStringCommand, "Validity", DbType.Int32, voucher.Validity);
                if (voucher.Amount.HasValue)
				{
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, voucher.Amount.Value);
				}
				else
				{
					this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, DBNull.Value);
				}

                //检查数据操作返回值，并对函数返回结果进行标记
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				if (obj != null && obj != DBNull.Value)
				{
                    voucherActionStatus = VoucherActionStatus.CreateClaimCodeSuccess;
				}
			}
            else
            {
                voucherActionStatus = VoucherActionStatus.CreateClaimCodeSuccess;
                DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_voucher_Create");
                this.database.AddInParameter(storedProcCommand, "VoucherId", DbType.Int32, voucher.VoucherId);
                this.database.AddInParameter(storedProcCommand, "row", DbType.Int32, count);
                this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, null);
                this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, null);
                this.database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, null);
                this.database.AddOutParameter(storedProcCommand, "ReturnLotNumber", DbType.String, 300);
                this.database.AddInParameter(storedProcCommand, "deadline", DbType.DateTime, DateTime.Now.AddDays(voucher.Validity));
                this.database.AddInParameter(storedProcCommand, "PwdType", DbType.Int32,pwdType);
                
                try
                {
                    this.database.ExecuteNonQuery(storedProcCommand);
                    lotNumber = (string)this.database.GetParameterValue(storedProcCommand, "ReturnLotNumber");
                }
                catch
                {
                    voucherActionStatus =VoucherActionStatus.CreateClaimCodeError;
                }
            }
            result = voucherActionStatus;
			return result;
		}
        public IList<VoucherItemInfo> GetVoucherItemInfos(string lotNumber)
        {
            IList<VoucherItemInfo> result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_VoucherItems WHERE convert(nvarchar(300),LotNumber)=@LotNumber");
            this.database.AddInParameter(sqlStringCommand, "LotNumber", DbType.String, lotNumber);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<VoucherItemInfo>(dataReader);
            }
            return result;
        }

        //编辑现金券信息
        public VoucherActionStatus UpdateVoucher(VoucherInfo voucher)
        {
            VoucherActionStatus result;
            if (null == voucher)
            {
                result = VoucherActionStatus.UnknowError;
            }
            else
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT VoucherId  FROM Ecshop_Vouchers WHERE Name=@Name AND VoucherId<>@VoucherId ");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, voucher.Name);
                this.database.AddInParameter(sqlStringCommand, "VoucherId", DbType.Int32, voucher.VoucherId);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    result = VoucherActionStatus.DuplicateName;
                }
                else
                {
                    sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Vouchers SET [Name]=@Name, ClosingTime=@ClosingTime,StartTime=@StartTime, Description=@Description, Amount=@Amount, DiscountValue=@DiscountValue,SendType=@SendType,SendTypeItem=@SendTypeItem,Validity=@Validity WHERE VoucherId=@VoucherId");
                    this.database.AddInParameter(sqlStringCommand, "VoucherId", DbType.String, voucher.VoucherId);
                    this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, voucher.Name);
                    this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, voucher.ClosingTime);
                    this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, voucher.StartTime);
                    this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, voucher.Description);
                    this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, voucher.DiscountValue);
                    this.database.AddInParameter(sqlStringCommand, "SendType", DbType.Int32, voucher.SendType);
                    this.database.AddInParameter(sqlStringCommand, "SendTypeItem", DbType.String, voucher.SendTypeItem);
                    this.database.AddInParameter(sqlStringCommand, "Validity", DbType.Int32, voucher.Validity);
                    if (voucher.Amount.HasValue)
                    {
                        this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, voucher.Amount.Value);
                    }
                    else
                    {
                        this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, DBNull.Value);
                    }
                    if (this.database.ExecuteNonQuery(sqlStringCommand) == 1)
                    {
                        result = VoucherActionStatus.Success;
                    }
                    else
                    {
                        result = VoucherActionStatus.UnknowError;
                    }
                }
            }
            return result;
        }
        public bool DeleteVoucher(int voucherId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_Vouchers WHERE VoucherId = @VoucherId");
            this.database.AddInParameter(sqlStringCommand, "VoucherId", DbType.Int32, voucherId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 根据主键获取获取现金券信息
        /// </summary>
        /// <param name="voucherId"></param>
        /// <returns></returns>
        public VoucherInfo GetVoucherDetails(int voucherId)
        {
            VoucherInfo result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Vouchers WHERE VoucherId = @VoucherId");
            this.database.AddInParameter(sqlStringCommand, "VoucherId", DbType.Int32, voucherId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<VoucherInfo>(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 根据现金券码获取现金券信息
        /// </summary>
        /// <param name="claimCode"></param>
        /// <returns></returns>
        public VoucherInfo GetVoucherDetails(string claimCode)
        {
            VoucherInfo result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Vouchers WHERE @DateTime>StartTime AND  @DateTime <ClosingTime AND VoucherId = (SELECT VoucherId FROM Ecshop_VoucherItems WHERE ClaimCode =@ClaimCode AND VoucherStatus =0)");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, claimCode);
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.Now);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<VoucherInfo>(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 根据现金券明细号码和密码获取现金券模板
        /// </summary>
        /// <param name="voucherCode"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public VoucherInfo GetVoucherDetails(string claimCode,string password)
        {
            VoucherInfo result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"SELECT A.* FROM Ecshop_Vouchers A  inner join Ecshop_VoucherItems B on A.VoucherId=B.VoucherId where  @DateTime>A.StartTime  AND  @DateTime <A.ClosingTime AND @Datetime<B.Deadline  AND B.ClaimCode=@ClaimCode AND B.VoucherStatus=0 AND B.[Password]=@Password");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, claimCode);
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, password);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<VoucherInfo>(dataReader);
            }
            return result;
        }
//        public DataTable GetCoupon(decimal orderAmount)
//        {
//            DataTable result = new DataTable();
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Name, ClaimCode,Amount,DiscountValue FROM Ecshop_Coupons c INNER  JOIN Ecshop_CouponItems ci ON ci.CouponId = c.CouponId Where  @DateTime>c.StartTime and @DateTime <c.ClosingTime AND ((Amount>0 and @orderAmount>=Amount) or (Amount=0 and @orderAmount>=DiscountValue))    and  CouponStatus=0  AND UserId=@UserId");
//            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.UtcNow);
//            this.database.AddInParameter(sqlStringCommand, "orderAmount", DbType.Decimal, orderAmount);
//            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
//            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
//            {
//                result = DataHelper.ConverDataReaderToDataTable(dataReader);
//            }
//            return result;
//        }

       //分页查询
        public DbQueryResult GetNewVouchers(Pagination page)
        {
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, " (select B.*,(select count(1) from Ecshop_VoucherItems A where A.VoucherStatus=1 and A.voucherid=B.voucherid) as useAmount from Ecshop_Vouchers B) as C ", "VoucherId", string.Empty, "*");
        }

        public bool ExitVoucherClaimCode(string claimCode)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(ClaimCode) FROM Ecshop_VoucherItems WHERE ClaimCode = @ClaimCode AND ISNULL(UserId, 0) = 0");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, claimCode);
            return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
        }

        public int AddVoucherClaimCodeToUser(string claimCode, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_VoucherItems SET UserId = @UserId,UserName=@UserName WHERE ClaimCode = @ClaimCode");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, HiContext.Current.User.Username);
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, claimCode);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public DbQueryResult GetUserVoucher(UserVoucherQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "VoucherId";
            }
            if (query.Status.HasValue)
            {
                if (query.Status == 1)
                {
                    stringBuilder.AppendFormat(" AND VoucherStatus = 0 AND UsedTime is NULL and ClosingTime > '{0}'", DateTime.Now);
                }
                else
                {
                    if (query.Status == 2)
                    {
                        stringBuilder.AppendFormat(" AND UsedTime is not NULL and ClosingTime > '{0}'", DateTime.Now);
                    }
                    else
                    {
                        if (query.Status == 3)
                        {
                            stringBuilder.Append( " AND ClosingTime<getdate()");
                        }
                    }
                }
            }
            if (query.UserID.HasValue)
            {
                stringBuilder.AppendFormat(" AND UserId = {0}", query.UserID.Value);
            }
            if (!string.IsNullOrEmpty(query.ClaimCode))
            {
                stringBuilder.AppendFormat(" and ClaimCode='{0}'", DataHelper.CleanSearchString(query.ClaimCode));
            }
            string selectFields = "ClosingTime,VoucherId,Name,Amount,DiscountValue,StartTime,ClaimCode,VoucherStatus,UsedTime,UserId,GETDATE() CurrentTime";

            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_VoucherInfo", "VoucherId", stringBuilder.ToString(), selectFields);
            
        }

        public DbQueryResult GetVouchersList(VoucherItemInfoQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (query.VoucherId.HasValue)
            {
                stringBuilder.AppendFormat("VoucherId = {0}", query.VoucherId.Value);
            }
            if (!string.IsNullOrEmpty(query.VoucherName))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat("Name = '{0}'", DataHelper.CleanSearchString(query.VoucherName));
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat("UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
            }
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat("Orderid='{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (query.VoucherStatus.HasValue)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" VoucherStatus={0} ", query.VoucherStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_VoucherInfo", "ClaimCode", stringBuilder.ToString(), "*");
        }

//        //获取用户所有的优惠券（关联表查询）
//        public DataTable GetUserCoupons(int userId, int useType = 0)
//        {
//            DataTable result = null;
//            string str = "";
//            if (useType == 1)
//            {
//                str = "AND ci.CouponStatus = 0 AND ci.UsedTime is NULL and c.ClosingTime > @ClosingTime";
//            }
//            else
//            {
//                if (useType == 2)
//                {
//                    str = " AND ci.UsedTime is not NULL and c.ClosingTime > @ClosingTime";
//                }
//                else
//                {
//                    if (useType == 3)
//                    {
//                        str = " AND c.ClosingTime<getdate()";
//                    }
//                }
//            }
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT c.*, ci.ClaimCode,ci.CouponStatus  FROM Ecshop_CouponItems ci INNER JOIN Ecshop_Coupons c ON c.CouponId = ci.CouponId WHERE ci.UserId = @UserId " + str);
//            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
//            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
//            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
//            {
//                result = DataHelper.ConverDataReaderToDataTable(dataReader);
//            }
//            return result;
//        }
//        public DataSet GetUserCoupons(UserCouponQuery query)
//        {
//            DataSet dataSet = new DataSet();
//            string text = "SELECT c.*, ci.ClaimCode,ci.CouponStatus  FROM Ecshop_CouponItems ci INNER JOIN Ecshop_Coupons c ON c.CouponId = ci.CouponId ";
//            string text2 = " where 1=1 ";
//            if (query.Status.HasValue)
//            {
//                if (query.Status == 1)
//                {
//                    text2 += "AND ci.CouponStatus = 0 AND ci.UsedTime is NULL and c.ClosingTime > @ClosingTime";
//                }
//                else
//                {
//                    if (query.Status == 2)
//                    {
//                        text2 += " AND ci.UsedTime is not NULL and c.ClosingTime > @ClosingTime";
//                    }
//                    else
//                    {
//                        if (query.Status == 3)
//                        {
//                            text2 += " AND c.ClosingTime<getdate()";
//                        }
//                    }
//                }
//            }
//            if (query.UserID.HasValue)
//            {
//                text2 += " AND ci.UserId = @UserId";
//            }
//            if (!string.IsNullOrEmpty(query.ClaimCode))
//            {
//                text2 += " and ClaimCode=@ClaimCode";
//            }
//            text += text2;
//            text += " Order by c.CouponId desc";
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
//            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, query.UserID);
//            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
//            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, query.ClaimCode);
//            return this.database.ExecuteDataSet(sqlStringCommand);
//        }
        //public bool SendClaimCodes(CouponItemInfo couponItem)
        //{
        //    DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_CouponItems(CouponId, ClaimCode,LotNumber, GenerateTime, UserId,UserName,EmailAddress,CouponStatus) VALUES(@CouponId, @ClaimCode,@LotNumber, @GenerateTime, @UserId, @UserName,@EmailAddress,@CouponStatus)");
        //    this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponItem.CouponId);
        //    this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponItem.ClaimCode);
        //    this.database.AddInParameter(sqlStringCommand, "GenerateTime", DbType.DateTime, couponItem.GenerateTime);
        //    this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32);
        //    this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String);
        //    this.database.AddInParameter(sqlStringCommand, "LotNumber", DbType.Guid, Guid.NewGuid());
        //    if (couponItem.UserId.HasValue)
        //    {
        //        this.database.SetParameterValue(sqlStringCommand, "UserId", couponItem.UserId.Value);
        //    }
        //    else
        //    {
        //        this.database.SetParameterValue(sqlStringCommand, "UserId", DBNull.Value);
        //    }
        //    if (!string.IsNullOrEmpty(couponItem.UserName))
        //    {
        //        this.database.SetParameterValue(sqlStringCommand, "UserName", couponItem.UserName);
        //    }
        //    else
        //    {
        //        this.database.SetParameterValue(sqlStringCommand, "UserName", DBNull.Value);
        //    }
        //    this.database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, couponItem.EmailAddress);
        //    this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.String, 0);
        //    return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
        //}

        /// <summary>
        /// 批量添加优惠券
        /// </summary>
        /// <param name="voucherItems"></param>
        /// <returns></returns>
        public bool SendClaimCodes(IList<VoucherItemInfo> voucherItems)
        {
            int batchSize = 1000;
            int i = 0;

            if (voucherItems.Count > 0)
            {
                DataTable dtVoucherItem = new DataTable("VoucherItem");
                dtVoucherItem.Columns.Add("VoucherId", typeof(Int32));
                dtVoucherItem.Columns.Add("LotNumber", typeof(Guid));
                dtVoucherItem.Columns.Add("ClaimCode", typeof(string));
                dtVoucherItem.Columns.Add("Password", typeof(string));
                dtVoucherItem.Columns.Add("UserId", typeof(Int32));
                dtVoucherItem.Columns.Add("UserName", typeof(string));
                dtVoucherItem.Columns.Add("EmailAddress", typeof(string));
                dtVoucherItem.Columns.Add("GenerateTime", typeof(DateTime));
                dtVoucherItem.Columns.Add("VoucherStatus", typeof(int));
                dtVoucherItem.Columns.Add("UsedTime");
                dtVoucherItem.Columns.Add("OrderId", typeof(string));
                dtVoucherItem.Columns.Add("Deadline", typeof(DateTime));
                dtVoucherItem.Columns.Add("IsRead", typeof(short));
                dtVoucherItem.Columns.Add("UsedAmount", typeof(decimal));
                dtVoucherItem.Columns.Add("SendOrderId", typeof(string));
                DataRow dr = null;

                foreach (VoucherItemInfo current in voucherItems)
                {

                    if (i % batchSize == 0)
                    {
                        dtVoucherItem.Rows.Clear();
                    }

                    dr = dtVoucherItem.NewRow();
                    dr["VoucherId"] = current.VoucherId;
                    dr["ClaimCode"] = current.ClaimCode;
                    dr["Password"] = current.Password;
                    dr["LotNumber"] = Guid.NewGuid();
                    dr["GenerateTime"] = current.GenerateTime;
                    dr["UserId"] = current.UserId ?? 0;
                    dr["UserName"] = current.UserName ?? "";
                    dr["EmailAddress"] = current.EmailAddress;
                    dr["VoucherStatus"] = 0;
                    dr["UsedTime"] = null;
                    dr["OrderId"] = "";
                    dr["IsRead"] = 0;
                    dr["UsedAmount"] = 0;
                    dr["Deadline"] = current.Deadline;
                    dr["SendOrderId"] = current.SendOrderId;
                    dtVoucherItem.Rows.Add(dr);

                    i++;


                    if (dtVoucherItem.Rows.Count == 1000 || (voucherItems.Count == i))
                    {
                        // 1.填充数据
                        // 每批1000
                        using (SqlBulkCopy bcp = new SqlBulkCopy(this.database.ConnectionString))
                        {
                            //SqlBulkCopy bcp = new SqlBulkCopy(conn);
                            bcp.DestinationTableName = "[Ecshop_VoucherItems]";
                            bcp.ColumnMappings.Add("VoucherId", "VoucherId");
                            bcp.ColumnMappings.Add("ClaimCode", "ClaimCode");
                            bcp.ColumnMappings.Add("Password", "Password");
                            bcp.ColumnMappings.Add("LotNumber", "LotNumber");
                            bcp.ColumnMappings.Add("GenerateTime", "GenerateTime");
                            bcp.ColumnMappings.Add("UserId", "UserId");
                            bcp.ColumnMappings.Add("UserName", "UserName");
                            bcp.ColumnMappings.Add("EmailAddress", "EmailAddress");
                            bcp.ColumnMappings.Add("VoucherStatus", "VoucherStatus");
                            bcp.ColumnMappings.Add("Deadline", "Deadline");
                            bcp.ColumnMappings.Add("IsRead", "IsRead");
                            bcp.ColumnMappings.Add("UsedAmount", "UsedAmount");
                            bcp.ColumnMappings.Add("SendOrderId", "SendOrderId");

                            bcp.WriteToServer(dtVoucherItem);
                            bcp.Close();
                        }

                        // 2.汇总（设置优惠券发送数量，怎么放在循环里面？)
                        DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Vouchers SET SentCount = (SELECT COUNT(*) FROM dbo.Ecshop_VoucherItems WHERE VoucherId = @VoucherId) WHERE VoucherId = @VoucherId");
                        this.database.AddInParameter(sqlStringCommand, "VoucherId", DbType.Int32, current.VoucherId);

                        bool ret = this.database.ExecuteNonQuery(sqlStringCommand) >= 1;

                    }
                }
            }

            return true;
        }

//        public bool AddCouponUseRecord(OrderInfo orderinfo, DbTransaction dbTran)
//        {
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update  Ecshop_CouponItems  set userName=@UserName,Userid=@Userid,Orderid=@Orderid,CouponStatus=@CouponStatus,EmailAddress=@EmailAddress,UsedTime=@UsedTime WHERE ClaimCode=@ClaimCode ");//and CouponStatus!=1 这里去掉这个状态过滤，是为了给分单改写了母单的优惠券使用信息，可能会带来问题
//            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, orderinfo.CouponCode);
//            this.database.AddInParameter(sqlStringCommand, "userName", DbType.String, orderinfo.Username);
//            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, orderinfo.UserId);
//            this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.Int32, 1);
//            this.database.AddInParameter(sqlStringCommand, "UsedTime", DbType.DateTime, DateTime.Now);
//            this.database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, orderinfo.EmailAddress);
//            this.database.AddInParameter(sqlStringCommand, "Orderid", DbType.String, orderinfo.OrderId);
//            bool result;
//            if (dbTran != null)
//            {
//                result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
//            }
//            else
//            {
//                result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
//            }
//            return result;
//        }
//        /// <summary>
//        /// 分单的优惠券信息
//        /// </summary>
//        /// <param name="orderinfo"></param>
//        /// <param name="dbTran"></param>
//        /// <returns></returns>
//        public bool AddCouponUseRecordForChildOrder(OrderInfo orderinfo, DbTransaction dbTran)
//        {
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update  Ecshop_CouponItems  set userName=@UserName,Userid=@Userid,Orderid=@Orderid,CouponStatus=@CouponStatus,EmailAddress=@EmailAddress,UsedTime=@UsedTime WHERE ClaimCode=@ClaimCode");
//            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, orderinfo.CouponCode);
//            this.database.AddInParameter(sqlStringCommand, "userName", DbType.String, orderinfo.Username);
//            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, orderinfo.UserId);
//            this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.Int32, 1);
//            this.database.AddInParameter(sqlStringCommand, "UsedTime", DbType.DateTime, DateTime.Now);
//            this.database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, orderinfo.EmailAddress);
//            this.database.AddInParameter(sqlStringCommand, "Orderid", DbType.String, orderinfo.OrderId);
//            bool result;
//            if (dbTran != null)
//            {
//                result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
//            }
//            else
//            {
//                result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
//            }
//            return result;
//        }

//        public CouponInfo GetCoupon(string couponCode)
//        {
//            CouponInfo result = null;
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT c.* FROM Ecshop_Coupons c INNER  JOIN Ecshop_CouponItems ci ON ci.CouponId = c.CouponId Where ci.ClaimCode =@ClaimCode   and  CouponStatus=0 AND  @DateTime>c.StartTime and  @DateTime <c.ClosingTime");
//            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponCode);
//            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.UtcNow);
//            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
//            {
//                if (dataReader.Read())
//                {
//                    result = DataMapper.PopulateCoupon(dataReader);
//                }
//            }
//            return result;
//        }
//        public DataTable GetChangeCoupons()
//        {
//            DataTable result = null;
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Coupons WHERE NeedPoint > 0 AND ClosingTime > @ClosingTime");
//            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
//            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
//            {
//                result = DataHelper.ConverDataReaderToDataTable(dataReader);
//            }
//            return result;
//        }
//        public bool ExitCouponClaimCode(string claimCode)
//        {
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(ClaimCode) FROM Ecshop_CouponItems WHERE ClaimCode = @ClaimCode AND ISNULL(UserId, 0) = 0");
//            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, claimCode);
//            return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
//        }
//        public int AddClaimCodeToUser(string claimCode, int userId)
//        {
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_CouponItems SET UserId = @UserId,UserName=@UserName  WHERE ClaimCode = @ClaimCode");
//            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
//            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, HiContext.Current.User.Username);
//            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, claimCode);
//            return this.database.ExecuteNonQuery(sqlStringCommand);
//        }


//        /// <summary>
//        /// 获取当前用户未查阅的优惠券数(排除已使用和已过期的)
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <returns></returns>
//        public int GetUserNotReadCoupons(int userId)
//        {
//            int result = 0;
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(1) FROM Ecshop_CouponItems a INNER JOIN Ecshop_Coupons b ON a.CouponId = b.CouponId WHERE a.CouponStatus=0 and a.UsedTime is NULL and b.ClosingTime > @ClosingTime and UserId = @UserId and a.IsRead =@IsRead");
//            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
//            this.database.AddInParameter(sqlStringCommand, "IsRead", DbType.Int32, 0);
//            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
//            result = (int)this.database.ExecuteScalar(sqlStringCommand);
//            return result;
//        }

//        /// <summary>
//        /// 更新优惠券为已阅（排除已使用和已过期的)）
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <returns></returns>
//        public int UpdateCouponsReaded(int userId)
//        {
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"update a set a.IsRead = @IsRead
//                                                                                    from Ecshop_CouponItems a 
//                                                                                        JOIN Ecshop_Coupons b 
//                                                                                            ON a.CouponId = b.CouponId
//                                                                                            where a.CouponStatus=0 and a.UsedTime is NULL and b.ClosingTime > @ClosingTime and a.UserId=@UserId and a.IsRead <> @IsRead");
//            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
//            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
//            this.database.AddInParameter(sqlStringCommand, "IsRead", DbType.Int32, 1);
//            return this.database.ExecuteNonQuery(sqlStringCommand);
//        }

        public int GetVoucherItemAmount(int voucherId)
        {
            int result = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(1) from Ecshop_VoucherItems where VoucherId=@VoucherId");
            this.database.AddInParameter(sqlStringCommand, "VoucherId", DbType.Int32, voucherId);
            result = (int)this.database.ExecuteScalar(sqlStringCommand);
            return result;
            
        }

        /// <summary>
        /// 获取当前用户可用的代金券，找出符合当前订单金额的使用条件且没有过期的
        /// </summary>
        /// <param name="orderAmount">当前订单金额</param>
        /// <returns></returns>
        public DataTable GetVouchersByCurUser(decimal orderAmount)
        {
            DataTable result = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"
SELECT v.name,v.discountValue,vi.ClaimCode,vi.deadline
FROM Ecshop_Vouchers v inner join Ecshop_VoucherItems vi
	ON v.VoucherId=vi.VoucherId
WHERE @DateTime <vi.deadline AND ((v.Amount>0 and @orderAmount>=v.Amount) or (v.Amount=0))   and  vi.VoucherStatus=0  AND vi.UserId=@UserId AND v.ClosingTime>GETDATE()");
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.UtcNow);
            this.database.AddInParameter(sqlStringCommand, "orderAmount", DbType.Decimal, orderAmount);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 根据号码和密码获取代金券面额
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pwd"></param>
        /// <param name="orderAmount">当前订单金额</param>
        /// <returns>如果为0表示没有找到</returns>
        public decimal GetVoucherAmount(string code, string pwd,decimal orderAmount)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"SELECT v.name,v.discountValue FROM Ecshop_Vouchers v inner join Ecshop_VoucherItems vi	ON v.VoucherId=vi.VoucherId WHERE @DateTime <vi.deadline AND ((v.Amount>0 and @orderAmount>=v.Amount) or (v.Amount=0))   and  vi.VoucherStatus=0    AND ClaimCode = @code AND Password = @pwd ");
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.UtcNow);
            this.database.AddInParameter(sqlStringCommand, "orderAmount", DbType.Decimal, orderAmount);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "code", DbType.String, code);
            this.database.AddInParameter(sqlStringCommand, "pwd", DbType.String, pwd);
            decimal disAmount = 0;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if(dataReader!=null )
                {
                    while (dataReader.Read())
                    {

                        decimal.TryParse(dataReader["DiscountValue"].ToString(), out disAmount);
                    }
                }
            }
            return disAmount;
        }


        /// <summary>
        /// 根据号码和密码获取代金券信息
        /// </summary>
        /// <param name="claimCode"></param>
        /// <returns></returns>
        public VoucherInfo GetVoucherByItem(string code, string pwd, decimal orderAmount)
        {
            VoucherInfo result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"
                                                                           SELECT v.name,v.discountValue
FROM Ecshop_Vouchers v inner join Ecshop_VoucherItems vi
	ON v.VoucherId=vi.VoucherId
WHERE @DateTime <vi.deadline AND ((v.Amount>0 and @orderAmount>=v.Amount) or (v.Amount=0))   and  vi.VoucherStatus=0  
    AND ClaimCode = @code AND Password = @pwd 
                                                                           ");
            this.database.AddInParameter(sqlStringCommand, "code", DbType.String, code);
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "orderAmount", DbType.Decimal, orderAmount);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "pwd", DbType.String, pwd);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<VoucherInfo>(dataReader);
            }
            return result;
        }

        //通过发送方式获取现金券列表
        public IList<VoucherInfo> GetVoucherBySendType(int sendType)
        {
            IList<VoucherInfo> result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Vouchers WHERE SendType=@SendType and ClosingTime>=@ClosingTime");
            this.database.AddInParameter(sqlStringCommand, "SendType", DbType.Int32, sendType);
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now.Date);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<VoucherInfo>(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 使用优惠券之后更新优惠券状态
        /// </summary>
        /// <param name="claimCode"></param>
        /// <param name="userdAmount"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool UpdateVoucherItemByUsed(string claimCode, decimal userdAmount, string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_VoucherItems SET VoucherStatus=1,UsedTime=getdate(),UsedAmount=@UsedAmount,OrderId=@OrderId WHERE ClaimCode = @ClaimCode");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, claimCode);
            this.database.AddInParameter(sqlStringCommand, "UsedAmount", DbType.Decimal, userdAmount);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }


        /// <summary>
        /// 使用优惠券之后更新优惠券状态
        /// </summary>
        /// <param name="orderinfo">订单信息</param>
        /// <param name="dbTran"></param>
        /// <returns></returns>
        public bool UpdateVoucherItemByUsed(OrderInfo orderinfo, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand =
                this.database.GetSqlStringCommand(
                    "UPDATE Ecshop_VoucherItems SET userName=@userName,EmailAddress=@EmailAddress,Userid=@userid,VoucherStatus=@CouponStatus,UsedTime=@UsedTime,UsedAmount=@UsedAmount,OrderId=@OrderId WHERE ClaimCode = @ClaimCode");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, orderinfo.VoucherCode);
            this.database.AddInParameter(sqlStringCommand, "userName", DbType.String, orderinfo.Username);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, orderinfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "UsedTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "UsedAmount", DbType.Decimal, orderinfo.VoucherValue);
            this.database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, orderinfo.EmailAddress);
            this.database.AddInParameter(sqlStringCommand, "Orderid", DbType.String, orderinfo.OrderId);
            bool result;
            if (dbTran != null)
            {
                result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            else
            {
                result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return result;
        }

        //获取记录
        public int GetVoucherItemsCount(int userId, string keyCode)
        {
            int result = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(1) FROM Ecshop_VoucherItems where UserId = @UserId and KeyCode=@KeyCode");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "KeyCode", DbType.String, keyCode);

            result = (int)this.database.ExecuteScalar(sqlStringCommand);

            return result;
        }

        /// <summary>
        /// 批量添加优惠券
        /// </summary>
        /// <param name="voucherItems"></param>
        /// <returns></returns>
        public bool BulkAddVoucherItems(IList<VoucherItemInfo> voucherItems)
        {
            int batchSize = 1000;
            int i = 0;

            if (voucherItems.Count > 0)
            {
                DataTable dtVoucherItem = new DataTable("VoucherItem");
                dtVoucherItem.Columns.Add("VoucherId", typeof(Int32));
                dtVoucherItem.Columns.Add("LotNumber", typeof(Guid));
                dtVoucherItem.Columns.Add("ClaimCode", typeof(string));
                dtVoucherItem.Columns.Add("Password", typeof(string));
                dtVoucherItem.Columns.Add("UserId", typeof(Int32));
                dtVoucherItem.Columns.Add("UserName", typeof(string));
                dtVoucherItem.Columns.Add("EmailAddress", typeof(string));
                dtVoucherItem.Columns.Add("GenerateTime", typeof(DateTime));
                dtVoucherItem.Columns.Add("VoucherStatus", typeof(int));
                dtVoucherItem.Columns.Add("UsedTime");
                dtVoucherItem.Columns.Add("OrderId", typeof(string));
                dtVoucherItem.Columns.Add("Deadline", typeof(DateTime));
                dtVoucherItem.Columns.Add("IsRead", typeof(short));
                dtVoucherItem.Columns.Add("UsedAmount", typeof(decimal));
                dtVoucherItem.Columns.Add("KeyCode", typeof(string));

                DataRow dr = null;
                List<int> voucherIds = new List<int>();

                foreach (VoucherItemInfo current in voucherItems)
                {
                    if (i % batchSize == 0)
                    {
                        dtVoucherItem.Rows.Clear();
                        voucherIds.Clear();
                    }

                    dr = dtVoucherItem.NewRow();
                    dr["VoucherId"] = current.VoucherId;
                    dr["ClaimCode"] = current.ClaimCode;
                    dr["Password"] = current.Password;
                    dr["LotNumber"] = Guid.NewGuid();
                    dr["GenerateTime"] = current.GenerateTime;
                    dr["UserId"] = current.UserId ?? 0;
                    dr["UserName"] = current.UserName ?? "";
                    dr["EmailAddress"] = current.EmailAddress;
                    dr["VoucherStatus"] = 0;
                    dr["UsedTime"] = null;
                    dr["OrderId"] = "";
                    dr["IsRead"] = 0;
                    dr["UsedAmount"] = 0;
                    dr["Deadline"] = current.Deadline;
                    dr["KeyCode"] = current.KeyCode;
                    dtVoucherItem.Rows.Add(dr);

                    i++;

                    if (!voucherIds.Contains(current.VoucherId))
                    {
                        voucherIds.Add(current.VoucherId);
                    }

                    if (dtVoucherItem.Rows.Count == 1000 || (voucherItems.Count == i))
                    {
                        // 1.填充数据
                        // 每批1000
                        using (SqlBulkCopy bcp = new SqlBulkCopy(this.database.ConnectionString))
                        {
                            //SqlBulkCopy bcp = new SqlBulkCopy(conn);
                            bcp.DestinationTableName = "[Ecshop_VoucherItems]";
                            bcp.ColumnMappings.Add("VoucherId", "VoucherId");
                            bcp.ColumnMappings.Add("ClaimCode", "ClaimCode");
                            bcp.ColumnMappings.Add("Password", "Password");
                            bcp.ColumnMappings.Add("LotNumber", "LotNumber");
                            bcp.ColumnMappings.Add("GenerateTime", "GenerateTime");
                            bcp.ColumnMappings.Add("UserId", "UserId");
                            bcp.ColumnMappings.Add("UserName", "UserName");
                            bcp.ColumnMappings.Add("EmailAddress", "EmailAddress");
                            bcp.ColumnMappings.Add("VoucherStatus", "VoucherStatus");
                            bcp.ColumnMappings.Add("Deadline", "Deadline");
                            bcp.ColumnMappings.Add("IsRead", "IsRead");
                            bcp.ColumnMappings.Add("UsedAmount", "UsedAmount");
                            bcp.ColumnMappings.Add("KeyCode", "KeyCode");
                            bcp.WriteToServer(dtVoucherItem);
                            bcp.Close();
                        }

                        // 2.汇总（设置优惠券发送数量，怎么放在循环里面？)
                        DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE a SET SentCount = b.SentCount FROM Ecshop_Vouchers a JOIN (SELECT COUNT(*) SentCount, VoucherId FROM dbo.Ecshop_VoucherItems WHERE VoucherId IN ({0}) GROUP BY VoucherId) b ON a.VoucherId = b.VoucherId WHERE a.VoucherId IN ({0})", string.Join<int>(",", voucherIds)));

                        bool ret = this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
                    }
                }
            }

            return true;
        }

        public DataTable GetUserVoucher(int userId, int useType = 0)
        {
            DataTable result = null;
            string str = "";
            if (useType == 1)
            {
                str = "AND ci.VoucherStatus = 0 AND ci.UsedTime is NULL and c.ClosingTime > @ClosingTime";
            }
            else
            {
                if (useType == 2)
                {
                    str = " AND ci.UsedTime is not NULL and c.ClosingTime > @ClosingTime";
                }
                else
                {
                    if (useType == 3)
                    {
                        str = " AND c.ClosingTime<getdate()";
                    }
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT c.*, ci.ClaimCode,ci.VoucherStatus  FROM Ecshop_VoucherItems ci INNER JOIN Ecshop_Vouchers c ON c.VoucherId = ci.VoucherId WHERE ci.UserId = @UserId " + str);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        /// <summary>
        /// 更新现金券为已阅（排除已使用和已过期的)）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int UpdateVoucherReaded(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"update a set a.IsRead = @IsRead
                                                                                    from Ecshop_VoucherItems a 
                                                                                        JOIN Ecshop_Vouchers b 
                                                                                            ON a.VoucherId = b.VoucherId
                                                                                            where a.VoucherStatus=0 and a.UsedTime is NULL and b.ClosingTime > @ClosingTime and a.UserId=@UserId and a.IsRead <> @IsRead");
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "IsRead", DbType.Int32, 1);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }


        /// <summary>
        /// 获取当前用户未查阅的现金券数(排除已使用和已过期的)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserNotReadVoucher(int userId)
        {
            int result = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(1) FROM Ecshop_VoucherItems a INNER JOIN Ecshop_Vouchers b ON a.VoucherId = b.VoucherId WHERE a.VoucherStatus=0 and a.UsedTime is NULL and b.ClosingTime > @ClosingTime and UserId = @UserId and a.IsRead =@IsRead");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "IsRead", DbType.Int32, 0);
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now.Date);
            result = (int)this.database.ExecuteScalar(sqlStringCommand);
            return result;
        }

        public int GetCountVoucherItem(int voucherId, int userId)
        {
            int count = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(1) FROM Ecshop_VoucherItems WHERE VoucherId = @VoucherId and UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "VoucherId", DbType.Int32, voucherId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            try
            {
                count = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
            }

            catch
            {
                count = 0;
            }

            return count;

        }

        public int GetCountVoucherItem(int voucherId, string sendOrderId)
        {
            int count = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(1) FROM Ecshop_VoucherItems WHERE VoucherId = @VoucherId and SendOrderId=@SendOrderId");
            this.database.AddInParameter(sqlStringCommand, "VoucherId", DbType.Int32, voucherId);
            this.database.AddInParameter(sqlStringCommand, "SendOrderId", DbType.String, sendOrderId);
            try
            {
                count = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
            }

            catch
            {
                count = 0;
            }

            return count;

        }

        /// <summary>
        /// 还原现金券的使用 将orderId和UsedTime设为null
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool RevertVoucher(string orderId, DbTransaction dbTran)
        {
            bool result;
            string sql = "update Ecshop_VoucherItems set OrderId = null,UsedTime = null,VoucherStatus=0 where OrderId =@OrderId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            try
            {
                if (dbTran != null)
                {
                    result = this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
                }
                else
                {
                    result = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
                }
               
            }
            catch
            {
                result = false;
            }
            return result;

        }


        public int GetVoucherCounts(string orderId)
        {
            int result;
            string sql = "SELECT COUNT(1) FROM dbo.Ecshop_VoucherItems where OrderId =@OrderId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            try
            {
                result = (int)this.database.ExecuteScalar(sqlStringCommand);

            }
            catch
            {
                result = 0;
            }
            return result;
        }
    }
}

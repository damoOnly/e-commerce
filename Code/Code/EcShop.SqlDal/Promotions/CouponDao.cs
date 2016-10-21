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
    public class CouponDao
    {
        private Database database;
        public CouponDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber)
        {
            CouponActionStatus couponActionStatus = CouponActionStatus.UnknowError;
            lotNumber = string.Empty;
            CouponActionStatus result;
            if (count <= 0)
            {
                lotNumber = string.Empty;
                if (null == coupon)
                {
                    couponActionStatus = CouponActionStatus.UnknowError;
                    result = couponActionStatus;
                    return result;
                }
                // 去掉判断是否重复优惠券名称
                //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Ecshop_Coupons WHERE Name=@Name");
                //this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                //if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                //{
                //    couponActionStatus = CouponActionStatus.DuplicateName;
                //    result = couponActionStatus;
                //    return result;
                //}
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Coupons ([Name],  ClosingTime,StartTime, Description, Amount, DiscountValue,SentCount,UsedCount,NeedPoint,OutBeginDate,OutEndDate,TotalQ,Status,Remark) VALUES(@Name, @ClosingTime,@StartTime, @Description, @Amount, @DiscountValue,0,0,@NeedPoint,@OutBeginDate,@OutEndDate,@TotalQ,@Status,@Remark); SELECT @@IDENTITY");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, coupon.ClosingTime);
                this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, coupon.StartTime);
                this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, coupon.Description);
                this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, coupon.DiscountValue);
                this.database.AddInParameter(sqlStringCommand, "NeedPoint", DbType.Int32, coupon.NeedPoint);
                this.database.AddInParameter(sqlStringCommand, "OutBeginDate", DbType.DateTime, coupon.OutBeginDate);
                this.database.AddInParameter(sqlStringCommand, "OutEndDate", DbType.DateTime, coupon.OutEndDate);
                this.database.AddInParameter(sqlStringCommand, "TotalQ", DbType.Int32, coupon.TotalQ);
                this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, coupon.Status);
                this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, coupon.Remark);
                if (coupon.Amount.HasValue)
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, coupon.Amount.Value);
                }
                else
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, DBNull.Value);
                }
                object obj = this.database.ExecuteScalar(sqlStringCommand);
                if (obj != null && obj != DBNull.Value)
                {
                    couponActionStatus = CouponActionStatus.CreateClaimCodeSuccess;
                }
            }
            else
            {
                couponActionStatus = CouponActionStatus.CreateClaimCodeSuccess;
                DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ClaimCode_Create");
                this.database.AddInParameter(storedProcCommand, "CouponId", DbType.Int32, coupon.CouponId);
                this.database.AddInParameter(storedProcCommand, "row", DbType.Int32, count);
                this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, null);
                this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, null);
                this.database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, null);
                this.database.AddOutParameter(storedProcCommand, "ReturnLotNumber", DbType.String, 300);
                try
                {
                    this.database.ExecuteNonQuery(storedProcCommand);
                    lotNumber = (string)this.database.GetParameterValue(storedProcCommand, "ReturnLotNumber");
                }
                catch
                {
                    couponActionStatus = CouponActionStatus.CreateClaimCodeError;
                }
            }
            result = couponActionStatus;
            return result;
        }


        public CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber, out int couponid)
        {
            CouponActionStatus couponActionStatus = CouponActionStatus.UnknowError;
            lotNumber = string.Empty;
            couponid = 0;
            CouponActionStatus result;
            if (count <= 0)
            {
                lotNumber = string.Empty;
                if (null == coupon)
                {
                    couponActionStatus = CouponActionStatus.UnknowError;
                    result = couponActionStatus;
                    return result;
                }
                //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Ecshop_Coupons WHERE Name=@Name");
                //this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                //if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                //{
                //    couponActionStatus = CouponActionStatus.DuplicateName;
                //    result = couponActionStatus;
                //    return result;
                //}
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_Coupons ([Name],  ClosingTime,StartTime, Description, Amount, DiscountValue,SentCount,UsedCount,NeedPoint,UseType,SendType,SendTypeItem,OutBeginDate,OutEndDate,TotalQ,Status,Remark,Validity) VALUES(@Name, @ClosingTime,@StartTime, @Description, @Amount, @DiscountValue,0,0,@NeedPoint,@UseType,@SendType,@SendTypeItem,@OutBeginDate,@OutEndDate,@TotalQ,@Status,@Remark,@Validity); SELECT @@IDENTITY");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, coupon.ClosingTime);
                this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, coupon.StartTime);
                this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, coupon.Description);
                this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, coupon.DiscountValue);
                this.database.AddInParameter(sqlStringCommand, "NeedPoint", DbType.Int32, coupon.NeedPoint);
                this.database.AddInParameter(sqlStringCommand, "UseType", DbType.Int32, coupon.UseType);
                this.database.AddInParameter(sqlStringCommand, "SendType", DbType.Int32, coupon.SendType);
                this.database.AddInParameter(sqlStringCommand, "SendTypeItem", DbType.String, coupon.SendTypeItem);
                this.database.AddInParameter(sqlStringCommand, "OutBeginDate", DbType.DateTime, coupon.OutBeginDate);
                this.database.AddInParameter(sqlStringCommand, "OutEndDate", DbType.DateTime, coupon.OutEndDate);
                this.database.AddInParameter(sqlStringCommand, "TotalQ", DbType.Int32, coupon.TotalQ);
                this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, coupon.Status);
                this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, coupon.Remark);
                this.database.AddInParameter(sqlStringCommand, "Validity", DbType.Int32, coupon.Validity);
                if (coupon.Amount.HasValue)
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, coupon.Amount.Value);
                }
                else
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, DBNull.Value);
                }
                object obj = this.database.ExecuteScalar(sqlStringCommand);
                if (obj != null && obj != DBNull.Value)
                {
                    couponActionStatus = CouponActionStatus.CreateClaimCodeSuccess;
                    couponid = int.Parse(obj.ToString());
                }
            }
            else
            {
                couponActionStatus = CouponActionStatus.CreateClaimCodeSuccess;
                DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ClaimCode_Create");
                this.database.AddInParameter(storedProcCommand, "CouponId", DbType.Int32, coupon.CouponId);
                this.database.AddInParameter(storedProcCommand, "row", DbType.Int32, count);
                this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, null);
                this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, null);
                this.database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, null);
                this.database.AddOutParameter(storedProcCommand, "ReturnLotNumber", DbType.String, 300);
                try
                {
                    this.database.ExecuteNonQuery(storedProcCommand);
                    lotNumber = (string)this.database.GetParameterValue(storedProcCommand, "ReturnLotNumber");
                }
                catch
                {
                    couponActionStatus = CouponActionStatus.CreateClaimCodeError;
                }
            }
            result = couponActionStatus;
            return result;
        }
        public IList<CouponItemInfo> GetCouponItemInfos(string lotNumber)
        {
            IList<CouponItemInfo> result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_CouponItems WHERE convert(nvarchar(300),LotNumber)=@LotNumber");
            this.database.AddInParameter(sqlStringCommand, "LotNumber", DbType.String, lotNumber);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<CouponItemInfo>(dataReader);
            }
            return result;
        }

        public IList<CouponInfo> GetCouponsBySendType(int sendType)
        {
            IList<CouponInfo> result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Coupons WHERE  ((TotalQ != 0 and TotalQ > SentCount) or ToTalQ = 0) and Status = 1  and CONVERT(varchar(100), GETDATE(), 23) >= CONVERT(varchar(100), OutBeginDate, 23) and CONVERT(varchar(100), GETDATE(), 23) <= CONVERT(varchar(100), OutEndDate, 23)  and SendType=@SendType");
            this.database.AddInParameter(sqlStringCommand, "SendType", DbType.Int32, sendType);
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now.Date);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<CouponInfo>(dataReader);
            }
            return result;
        }
        public CouponActionStatus UpdateCoupon(CouponInfo coupon)
        {
            CouponActionStatus result;
            if (null == coupon)
            {
                result = CouponActionStatus.UnknowError;
            }
            else
            {
                //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Ecshop_Coupons WHERE Name=@Name AND CouponId<>@CouponId ");
                //this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                //this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, coupon.CouponId);
                //if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                //{
                //    result = CouponActionStatus.DuplicateName;
                //}
                //else
                //{
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Coupons SET [Name]=@Name,Validity=@Validity, ClosingTime=@ClosingTime,StartTime=@StartTime, Description=@Description, Amount=@Amount, DiscountValue=@DiscountValue, NeedPoint = @NeedPoint,OutBeginDate=@OutBeginDate,OutEndDate=@OutEndDate,TotalQ=@TotalQ,Status=@Status,Remark=@Remark  WHERE CouponId=@CouponId");
                    this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.String, coupon.CouponId);
                    this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                    this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, coupon.ClosingTime);
                    this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, coupon.StartTime);
                    this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, coupon.Description);
                    this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, coupon.DiscountValue);
                    this.database.AddInParameter(sqlStringCommand, "NeedPoint", DbType.Int32, coupon.NeedPoint);
                    this.database.AddInParameter(sqlStringCommand, "OutBeginDate", DbType.DateTime, coupon.OutBeginDate);
                    this.database.AddInParameter(sqlStringCommand, "OutEndDate", DbType.DateTime, coupon.OutEndDate);
                    this.database.AddInParameter(sqlStringCommand, "TotalQ", DbType.Int32, coupon.TotalQ);
                    this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, coupon.Status);
                    this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, coupon.Remark);
                    this.database.AddInParameter(sqlStringCommand, "Validity", DbType.Int32, coupon.Validity);
                    if (coupon.Amount.HasValue)
                    {
                        this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, coupon.Amount.Value);
                    }
                    else
                    {
                        this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, DBNull.Value);
                    }
                    if (this.database.ExecuteNonQuery(sqlStringCommand) == 1)
                    {
                        result = CouponActionStatus.Success;
                    }
                    else
                    {
                        result = CouponActionStatus.UnknowError;
                    }
                //}
            }
            return result;
        }


        public CouponActionStatus NewUpdateCoupon(CouponInfo coupon)
        {
            CouponActionStatus result;
            if (null == coupon)
            {
                result = CouponActionStatus.UnknowError;
            }
            else
            {
                //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Ecshop_Coupons WHERE Name=@Name AND CouponId<>@CouponId ");
                //this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                //this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, coupon.CouponId);
                //if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                //{
                //    result = CouponActionStatus.DuplicateName;
                //}
                //else
                //{
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Coupons SET [Name]=@Name,Validity=@Validity, ClosingTime=@ClosingTime,StartTime=@StartTime, Description=@Description, Amount=@Amount, DiscountValue=@DiscountValue, NeedPoint = @NeedPoint,UseType=@UseType,SendType=@SendType,SendTypeItem=@SendTypeItem,OutBeginDate=@OutBeginDate,OutEndDate=@OutEndDate,TotalQ=@TotalQ,Status=@Status  WHERE CouponId=@CouponId");
                    this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.String, coupon.CouponId);
                    this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                    this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, coupon.ClosingTime);
                    this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, coupon.StartTime);
                    this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, coupon.Description);
                    this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, coupon.DiscountValue);
                    this.database.AddInParameter(sqlStringCommand, "NeedPoint", DbType.Int32, coupon.NeedPoint);
                    this.database.AddInParameter(sqlStringCommand, "UseType", DbType.Int32, coupon.UseType);
                    this.database.AddInParameter(sqlStringCommand, "SendType", DbType.Int32, coupon.SendType);
                    this.database.AddInParameter(sqlStringCommand, "SendTypeItem", DbType.String, coupon.SendTypeItem);
                    this.database.AddInParameter(sqlStringCommand, "OutBeginDate", DbType.DateTime, coupon.OutBeginDate);
                    this.database.AddInParameter(sqlStringCommand, "OutEndDate", DbType.DateTime, coupon.OutEndDate);
                    this.database.AddInParameter(sqlStringCommand, "TotalQ", DbType.Int32, coupon.TotalQ);
                    this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, coupon.Status);
                    this.database.AddInParameter(sqlStringCommand, "Validity", DbType.Int32, coupon.Validity);
                    if (coupon.Amount.HasValue)
                    {
                        this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, coupon.Amount.Value);
                    }
                    else
                    {
                        this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, DBNull.Value);
                    }
                    if (this.database.ExecuteNonQuery(sqlStringCommand) == 1)
                    {
                        result = CouponActionStatus.Success;
                    }
                    else
                    {
                        result = CouponActionStatus.UnknowError;
                    }
                //}
            }
            return result;
        }
        public bool DeleteCoupon(int couponId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_Coupons WHERE CouponId = @CouponId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public CouponInfo GetCouponDetails(int couponId)
        {
            CouponInfo result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"SELECT a.[CouponId]
                      ,[Name]
                      ,a.[StartTime]
                      ,case  when Sendtype<>4 then  a.[ClosingTime]  else b.[ClosingTime] end as 'ClosingTime'
                      ,[Description]
                      ,case  when Sendtype<>4 then  a.[Amount] else   b.[Amount]  end  as 'Amount'
                      ,case  when Sendtype<>4 then  a.[DiscountValue] else b.DiscountValue  end as 'DiscountValue'
                      ,[SentCount]
                      ,[UsedCount]
                      ,[NeedPoint]
                      ,[UseType]
                      ,[SendType]
                      ,[UseTypeItem]
                      ,[SendTypeItem]
                      ,a.[Remark]
                      ,[OutBeginDate]
                      ,[OutEndDate]
                      ,[TotalQ]
                      ,[Status]
                       ,a.Validity 
                       FROM Ecshop_Coupons  as a 
                       left join Ecshop_CouponItems as b on a.CouponId=b.CouponId
                       WHERE a.CouponId = @CouponId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<CouponInfo>(dataReader);
            }
            return result;
        }

        public DataTable GetSupplierCoupon(int UseType, int SendType,int SupplierId)
        {
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select ec.CouponId,ec.Name as CouponName,ec.StartTime,ec.ClosingTime,ec.Amount,ec.DiscountValue,ec.SentCount,ec.UsedCount,ec.NeedPoint,
ec.UseType,ec.SendType,ec.UseTypeItem,ec.SendTypeItem,ecsend.BindId as SupplierId,ecsend.UserId,ec.OutBeginDate,ec.OutEndDate,ec.TotalQ,ec.Status,ec.Remark,ec.Validity 
from Ecshop_Coupons ec 
inner join Ecshop_CouponsSendTypeItem ecsend on ec.CouponId = ecsend.CouponId 
where ((ec.TotalQ != 0 and ec.TotalQ > ec.SentCount) or ec.ToTalQ = 0) and ec.Status = 1 and ec.UseType = @usertype and ec.SendType = @sendtype and ecsend.BindId = @supplierid and  GETDATE() >= ec.OutBeginDate and GETDATE() <= ec.OutEndDate ");
            this.database.AddInParameter(sqlStringCommand, "usertype", DbType.Int32, UseType);
            this.database.AddInParameter(sqlStringCommand, "sendtype", DbType.Int32, SendType);
            this.database.AddInParameter(sqlStringCommand, "supplierid", DbType.Int32, SupplierId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public CouponInfo GetCouponDetails(string couponCode)
        {
            CouponInfo result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Coupons WHERE @DateTime>StartTime AND  @DateTime <ClosingTime AND CouponId = (SELECT CouponId FROM Ecshop_CouponItems WHERE ClaimCode =@ClaimCode AND CouponStatus =0)");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponCode);
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.Now);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<CouponInfo>(dataReader);
            }
            return result;
        }
        public DataTable GetCoupon(decimal orderAmount, string bindId, string categoryId)
        {
            int userId = HiContext.Current.User.UserId;
            return GetCoupon(userId, orderAmount, bindId, categoryId);
        }
        public DataTable GetCoupon(int userId, decimal orderAmount, string bindId, string categoryId)
        {
            DataTable result = new DataTable();
            DbCommand sqlStringCommand = this.database.GetStoredProcCommand("GetCouponsInfo");
            this.database.AddInParameter(sqlStringCommand, "OrderAmount", DbType.Decimal, orderAmount);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "BindId", DbType.String, bindId);
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.String,categoryId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        public DataTable GetCoupon(decimal orderAmount)
        {
            int userId = HiContext.Current.User.UserId;
            return GetCoupon(userId, orderAmount);
        }
        public DataTable GetCoupon(int userId, decimal orderAmount)
        {
            DataTable result = new DataTable();
            //string sql = "SELECT Name,ClaimCode,c.Amount,c.DiscountValue  FROM Ecshop_Coupons c INNER  JOIN Ecshop_CouponItems ci ON ci.CouponId = c.CouponId Where  @DateTime>c.StartTime and @DateTime <c.ClosingTime AND ((Amount>0 and @orderAmount>=Amount) or (Amount=0 and @orderAmount>=DiscountValue)) and  CouponStatus=0  AND UserId=@UserId";
            string sql = @"
                        select * from 
(
                        SELECT Name,ClaimCode,c.SendType, 
                        ci.Amount  as Amount  ,
                            ci.DiscountValue  as DiscountValue,
                            ci.StartTime  as StartTime  ,
                            ci.ClosingTime  as ClosingTime  
                         ,ci.CouponStatus
                         ,ci.UserId
                        FROM Ecshop_Coupons c 
                        INNER  JOIN Ecshop_CouponItems ci ON ci.CouponId = c.CouponId ) as c 
                        Where  @DateTime>c.StartTime and @DateTime <c.ClosingTime AND ((Amount>0 and @orderAmount>=Amount) or (Amount=0 and @orderAmount>=DiscountValue)) and  CouponStatus=0  AND UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "orderAmount", DbType.Decimal, orderAmount);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }




        public DbQueryResult GetNewCoupons(Pagination page)
        {
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, @"(   select 
                   case  b.SendType 
                   when 0 then '手动发送'
                   when 1 then '满金额赠券'
                   when 2 then '关注赠券'
                   when 3 then '自助领券'
                   when 4 then '退款退券'
                   when 5 then '注册赠劵'
                   else '' end as 'SendTypeName',
                   B.*,(select count(1) from Ecshop_CouponItems A where A.CouponStatus=1 and A.couponid=B.couponid) as useAmount,
                (select CouponId from Ecshop_CouponItems item where item.CouponId = B.CouponId group by item.CouponId) as HaveItem   from Ecshop_Coupons B )  as C", "CouponId", string.Empty, "*");
        }
        public DbQueryResult GetCouponsList(CouponItemInfoQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (query.CouponId.HasValue)
            {
                stringBuilder.AppendFormat("CouponId = {0}", query.CouponId.Value);
            }
            if (!string.IsNullOrEmpty(query.CounponName))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat("Name = '{0}'", DataHelper.CleanSearchString(query.CounponName));
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
            if (query.CouponStatus.HasValue)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" CouponStatus={0} ", query.CouponStatus);
            }
            if (!string.IsNullOrWhiteSpace(query.AddUserName))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" AddUserName='{0}'", DataHelper.CleanSearchString(query.AddUserName));
            }
            if(!string.IsNullOrWhiteSpace(query.BeginTime))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" GenerateTime >='{0}'", DataHelper.CleanSearchString(query.BeginTime));
            }
            if (!string.IsNullOrWhiteSpace(query.EndTime))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" GenerateTime <='{0}'", DataHelper.CleanSearchString(query.EndTime));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_CouponInfo", "ClaimCode", stringBuilder.ToString(), "*");
        }

        public DbQueryResult GetCouponsListToExport(CouponItemInfoQuery query)
        {
            DbQueryResult dbQueryResult = new DbQueryResult();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" select * from vw_Ecshop_CouponInfo  where 1=1  ");
            if (query.CouponId.HasValue)
            {
                stringBuilder.AppendFormat(" and CouponId = {0}", query.CouponId.Value);
            }
            if (!string.IsNullOrEmpty(query.CounponName))
            {
                stringBuilder.AppendFormat(" and Name = '{0}'", DataHelper.CleanSearchString(query.CounponName));
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                stringBuilder.AppendFormat(" and UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
            }
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                stringBuilder.AppendFormat(" and Orderid='{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (query.CouponStatus.HasValue)
            {
                stringBuilder.AppendFormat(" CouponStatus={0} ", query.CouponStatus);
            }
            if (!string.IsNullOrWhiteSpace(query.AddUserName))
            {
                stringBuilder.AppendFormat(" AddUserName='{0}'", DataHelper.CleanSearchString(query.AddUserName));
            }
            if (!string.IsNullOrWhiteSpace(query.BeginTime))
            {
                stringBuilder.AppendFormat(" GenerateTime >='{0}'", DataHelper.CleanSearchString(query.BeginTime));
            }
            if (!string.IsNullOrWhiteSpace(query.EndTime))
            {
                stringBuilder.AppendFormat(" GenerateTime <='{0}'", DataHelper.CleanSearchString(query.EndTime));
            }
            stringBuilder.Append(" order by GenerateTime desc ");
            DbCommand sqlCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            using (IDataReader dataReader = this.database.ExecuteReader(sqlCommand))
            {
                dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return dbQueryResult;
        }
        public DataTable GetUserCoupons(int userId, int useType = 0)
        {
            DataTable result = null;
            string str = "";
            if (useType == 1)
            {
                str = "AND CouponStatus = 0 AND UsedTime is NULL and ClosingTime > @ClosingTime";
            }
            else
            {
                if (useType == 2)
                {
                    str = " AND UsedTime is not NULL and ClosingTime > @ClosingTime";
                }
                else
                {
                    if (useType == 3)
                    {
                        str = " AND ClosingTime<getdate()";
                    }
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select * from (SELECT c.[CouponId]
                    ,[Name]
                    ,[Description]
                    ,[SentCount]
                    ,[UsedCount]
                    ,[NeedPoint]
                    ,[UseType]
                    ,[SendType]
                    ,[UseTypeItem]
                    ,[SendTypeItem]
                    ,c.[Remark]
                    ,[OutBeginDate]
                    ,[OutEndDate]
                    ,[TotalQ]
                    ,[Status],
                case when c.SendType!=4  then  c.Amount else ci.Amount end as Amount  ,
                case when c.SendType!=4  then  c.DiscountValue else ci.DiscountValue end as DiscountValue,
                ci.StartTime  as StartTime ,
                 ci.ClosingTime  as ClosingTime ,
                ci.ClaimCode,ci.AddUserName,
                ci.UsedTime,
                ci.CouponStatus,
                ci.UserId
                FROM Ecshop_CouponItems ci 
                INNER JOIN Ecshop_Coupons c ON c.CouponId = ci.CouponId ) as c WHERE UserId = @UserId " + str);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        public DbQueryResult GetUserCouponInfo(UserCouponQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "CouponId";
            }
            if (query.Status.HasValue)
            {
                if (query.Status == 1)
                {
                    stringBuilder.AppendFormat(" AND CouponStatus = 0 AND UsedTime is NULL and ClosingTime > '{0}'", DateTime.Now);
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
                            stringBuilder.Append(" AND ClosingTime<getdate()");
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
                stringBuilder.AppendFormat(" and ClaimCode='{0}'", query.ClaimCode);
            }
            string selectFields = "ClosingTime,CouponId,Name,Amount,DiscountValue,StartTime,ClaimCode,CouponStatus,UsedTime,UserId,GETDATE() CurrentTime";

            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_CouponInfo", "CouponId", stringBuilder.ToString(), selectFields);

        }

        //public DataSet GetUserCoupons(UserCouponQuery query)
        //{
        //    DataSet dataSet = new DataSet();
        //    string text = "SELECT c.*, ci.ClaimCode,ci.CouponStatus  FROM Ecshop_CouponItems ci INNER JOIN Ecshop_Coupons c ON c.CouponId = ci.CouponId ";
        //    string text2 = " where 1=1 ";
        //    if (query.Status.HasValue)
        //    {
        //        if (query.Status == 1)
        //        {
        //            text2 += "AND ci.CouponStatus = 0 AND ci.UsedTime is NULL and c.ClosingTime > @ClosingTime";
        //        }
        //        else
        //        {
        //            if (query.Status == 2)
        //            {
        //                text2 += " AND ci.UsedTime is not NULL and c.ClosingTime > @ClosingTime";
        //            }
        //            else
        //            {
        //                if (query.Status == 3)
        //                {
        //                    text2 += " AND c.ClosingTime<getdate()";
        //                }
        //            }
        //        }
        //    }
        //    if (query.UserID.HasValue)
        //    {
        //        text2 += " AND ci.UserId = @UserId";
        //    }
        //    if (!string.IsNullOrEmpty(query.ClaimCode))
        //    {
        //        text2 += " and ClaimCode=@ClaimCode";
        //    }
        //    text += text2;
        //    text += " Order by c.CouponId desc";
        //    DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
        //    this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, query.UserID);
        //    this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
        //    this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, query.ClaimCode);
        //    return this.database.ExecuteDataSet(sqlStringCommand);
        //}

        public bool SendClaimCodes(CouponItemInfo couponItem, DbTransaction dbTran = null)
        {
            string sqlstr = string.Empty;
            var coupon = GetCouponDetails(couponItem.CouponId);
            if (coupon != null)
            {
                // 手动发送方式可以多次添加
                if (coupon.SendType == 0)
                {
                    sqlstr = "INSERT INTO Ecshop_CouponItems(CouponId, ClaimCode,LotNumber, GenerateTime, UserId,UserName,EmailAddress,CouponStatus,AddUserId,AddUserName,Remark,StartTime,ClosingTime,Amount,DiscountValue) VALUES(@CouponId, @ClaimCode,@LotNumber, @GenerateTime, @UserId, @UserName,@EmailAddress,@CouponStatus,@AddUserId,@AddUserName,@Remark,@StartTime,@ClosingTime,@Amount,@DiscountValue);UPDATE Ecshop_Coupons SET SentCount = (SELECT COUNT(*) FROM dbo.Ecshop_CouponItems WHERE CouponId = @CouponId) WHERE CouponId = @CouponId ";
                }
                else
                {
                    sqlstr = "if not exists (select * from Ecshop_CouponItems where CouponId=@CouponId and UserId=@UserId)  INSERT INTO Ecshop_CouponItems(CouponId, ClaimCode,LotNumber, GenerateTime, UserId,UserName,EmailAddress,CouponStatus,AddUserId,AddUserName,Remark,StartTime,ClosingTime,Amount,DiscountValue) VALUES(@CouponId, @ClaimCode,@LotNumber, @GenerateTime, @UserId, @UserName,@EmailAddress,@CouponStatus,@AddUserId,@AddUserName,@Remark,@StartTime,@ClosingTime,@Amount,@DiscountValue);UPDATE Ecshop_Coupons SET SentCount = (SELECT COUNT(*) FROM dbo.Ecshop_CouponItems WHERE CouponId = @CouponId) WHERE CouponId = @CouponId ";
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sqlstr);
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponItem.CouponId);
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponItem.ClaimCode);
            this.database.AddInParameter(sqlStringCommand, "GenerateTime", DbType.DateTime, couponItem.GenerateTime);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "LotNumber", DbType.Guid, Guid.NewGuid());
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String,couponItem.Remark);
            this.database.AddInParameter(sqlStringCommand, "AddUserId", DbType.Int32,HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "AddUserName", DbType.String, HiContext.Current.User.Username);
            this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, couponItem.StartTime);
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.String, couponItem.ClosingTime);
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Decimal, couponItem.Amount);
            this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.String, couponItem.DiscountValue);
            if (couponItem.UserId.HasValue)
            {
                this.database.SetParameterValue(sqlStringCommand, "UserId", couponItem.UserId.Value);
            }
            else
            {
                this.database.SetParameterValue(sqlStringCommand, "UserId", DBNull.Value);
            }
            if (!string.IsNullOrEmpty(couponItem.UserName))
            {
                this.database.SetParameterValue(sqlStringCommand, "UserName", couponItem.UserName);
            }
            else
            {
                this.database.SetParameterValue(sqlStringCommand, "UserName", DBNull.Value);
            }
            this.database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, couponItem.EmailAddress);
            this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.String, 0);
            bool result = false;
            if (dbTran != null)
            {
                result = (database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
            }
            else
            {
                result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
            }
            return result;
        }

        public bool SendClaimCodesByTran(IList<CouponItemInfo> couponItems,DbTransaction dbTran)
        {
            foreach (CouponItemInfo current in couponItems)
            {
                SendClaimCodes(current);
            }
            return true;
        }

        public bool SendClaimCodes(IList<CouponItemInfo> couponItems)
        {
            int batchSize = 1000;
            int i = 0;

            if (couponItems.Count > 0)
            {
                DataTable dtCouponItem = new DataTable("CouponItem");
                dtCouponItem.Columns.Add("CouponId", typeof(Int32));
                dtCouponItem.Columns.Add("LotNumber", typeof(Guid));
                dtCouponItem.Columns.Add("ClaimCode", typeof(string));
                dtCouponItem.Columns.Add("UserId", typeof(Int32));
                dtCouponItem.Columns.Add("UserName", typeof(string));
                dtCouponItem.Columns.Add("EmailAddress", typeof(string));
                dtCouponItem.Columns.Add("GenerateTime", typeof(DateTime));
                dtCouponItem.Columns.Add("CouponStatus", typeof(int));
                dtCouponItem.Columns.Add("UsedTime");
                dtCouponItem.Columns.Add("OrderId", typeof(string));
                dtCouponItem.Columns.Add("SendOrderId", typeof(string));
                DataRow dr = null;
                StringBuilder strsql = new StringBuilder();
                foreach (CouponItemInfo current in couponItems)
                {
                    if (i % batchSize == 0)
                    {
                        dtCouponItem.Rows.Clear();
                    }

                    dr = dtCouponItem.NewRow();
                    dr["CouponId"] = current.CouponId;
                    dr["ClaimCode"] = current.ClaimCode;
                    dr["LotNumber"] = Guid.NewGuid();
                    dr["GenerateTime"] = current.GenerateTime;
                    dr["UserId"] = current.UserId ?? 0;
                    dr["UserName"] = current.UserName ?? "";
                    dr["EmailAddress"] = current.EmailAddress;
                    dr["CouponStatus"] = 0;
                    dr["UsedTime"] = null;
                    dr["OrderId"] = "";
                    dr["SendOrderId"] = current.SendOrderId;

                    dtCouponItem.Rows.Add(dr);

                    i++;
                    strsql.AppendFormat("UPDATE Ecshop_Coupons SET SentCount = (SELECT COUNT(*) FROM dbo.Ecshop_CouponItems WHERE CouponId = {0}) WHERE CouponId = {0};", current.CouponId);
                    if (dtCouponItem.Rows.Count == 1000 || (couponItems.Count == i))
                    {
                        // 1.填充数据
                        // 每批1000
                        using (SqlBulkCopy bcp = new SqlBulkCopy(this.database.ConnectionString))
                        {
                            //SqlBulkCopy bcp = new SqlBulkCopy(conn);
                            bcp.DestinationTableName = "[Ecshop_CouponItems]";
                            bcp.ColumnMappings.Add("CouponId", "CouponId");
                            bcp.ColumnMappings.Add("ClaimCode", "ClaimCode");
                            bcp.ColumnMappings.Add("LotNumber", "LotNumber");
                            bcp.ColumnMappings.Add("GenerateTime", "GenerateTime");
                            bcp.ColumnMappings.Add("UserId", "UserId");
                            bcp.ColumnMappings.Add("UserName", "UserName");
                            bcp.ColumnMappings.Add("EmailAddress", "EmailAddress");
                            bcp.ColumnMappings.Add("CouponStatus", "CouponStatus");
                            bcp.ColumnMappings.Add("SendOrderId", "SendOrderId");
                            bcp.WriteToServer(dtCouponItem);
                            bcp.Close();
                        }

                        // 2.汇总
                        DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strsql.ToString());

                        bool ret = this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
                    }
                }
            }

            return true;
        }

        public bool SendClaimCodes(int couponId, string memberNames, int? gradeId, string remark, bool isSendSms, int createdBy, string createdByName, string StartTime, string ClosingTime, decimal Amount, decimal DiscountValue, out int count)
        {
            count = 0;

            DbCommand cmd = this.database.GetStoredProcCommand("cp_Coupon_SendToUsers");

            this.database.AddInParameter(cmd, "CouponId", DbType.Int32, couponId);
            this.database.AddInParameter(cmd, "MemberNames", DbType.String, memberNames);
            this.database.AddInParameter(cmd, "GradeId", DbType.Int32, gradeId);
            this.database.AddInParameter(cmd, "Remark", DbType.String, remark);
            this.database.AddInParameter(cmd, "IsSendSms", DbType.Boolean, isSendSms);
            this.database.AddInParameter(cmd, "CreatedBy", DbType.Int32, createdBy);
            this.database.AddInParameter(cmd, "CreatedByName", DbType.String, createdByName);

            this.database.AddInParameter(cmd, "StartTime", DbType.DateTime, StartTime);
            this.database.AddInParameter(cmd, "ClosingTime", DbType.DateTime, ClosingTime);
            this.database.AddInParameter(cmd, "Amount", DbType.Decimal, Amount);
            this.database.AddInParameter(cmd, "DiscountValue", DbType.Decimal, DiscountValue);

            this.database.AddOutParameter(cmd, "Count", DbType.Int32, 4);

            bool result = (this.database.ExecuteNonQuery(cmd) > 0);
            count = (int)this.database.GetParameterValue(cmd, "Count");

            return result;
        }

        public bool AddCouponUseRecord(OrderInfo orderinfo, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_Coupons set UsedCount +=1 where CouponId  = (select CouponId from Ecshop_CouponItems where ClaimCode =@ClaimCode and OrderId =@Orderid );update  Ecshop_CouponItems  set userName=@UserName,Userid=@Userid,Orderid=@Orderid,CouponStatus=@CouponStatus,EmailAddress=@EmailAddress,UsedTime=@UsedTime WHERE ClaimCode=@ClaimCode ");//and CouponStatus!=1 这里去掉这个状态过滤，是为了给分单改写了母单的优惠券使用信息，可能会带来问题
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, orderinfo.CouponCode);
            this.database.AddInParameter(sqlStringCommand, "userName", DbType.String, orderinfo.Username);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, orderinfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "UsedTime", DbType.DateTime, DateTime.Now);
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

        /// <summary>
        /// 分单的优惠券信息
        /// </summary>
        /// <param name="orderinfo"></param>
        /// <param name="dbTran"></param>
        /// <returns></returns>
        public bool AddCouponUseRecordForChildOrder(OrderInfo orderinfo, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update  Ecshop_CouponItems  set userName=@UserName,Userid=@Userid,Orderid=@Orderid,CouponStatus=@CouponStatus,EmailAddress=@EmailAddress,UsedTime=@UsedTime WHERE ClaimCode=@ClaimCode");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, orderinfo.CouponCode);
            this.database.AddInParameter(sqlStringCommand, "userName", DbType.String, orderinfo.Username);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, orderinfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "UsedTime", DbType.DateTime, DateTime.Now);
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

        public CouponInfo GetCoupon(string couponCode)
        {
            CouponInfo result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select * from 
                (
                SELECT ci.CouponStatus,ci.ClaimCode,c.CouponId,c.Description,c.SentCount,c.UsedCount,c.NeedPoint, c.name,
                    case when c.SendType!=4  then  ci.Amount else ci.Amount end as Amount  ,
                    case when c.SendType!=4  then  ci.DiscountValue else ci.DiscountValue end as DiscountValue,
                    case when c.SendType!=4  then  ci.StartTime else ci.StartTime end as StartTime  ,
                    case when c.SendType!=4  then  ci.ClosingTime else ci.ClosingTime end as ClosingTime 
                FROM Ecshop_Coupons c 
                INNER  JOIN Ecshop_CouponItems ci ON ci.CouponId = c.CouponId) as c Where ClaimCode =@ClaimCode  and  CouponStatus=0 AND  GETDATE() >StartTime and  GETDATE() <ClosingTime ");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponCode);
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.UtcNow);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    result = DataMapper.PopulateCoupon(dataReader);
                }
            }
            return result;
        }
        public DataTable GetChangeCoupons()
        {
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Coupons WHERE NeedPoint > 0 AND ClosingTime > @ClosingTime");
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public bool ExitCouponClaimCode(string claimCode)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(ClaimCode) FROM Ecshop_CouponItems WHERE ClaimCode = @ClaimCode AND ISNULL(UserId, 0) = 0");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, claimCode);
            return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
        }
        public int AddClaimCodeToUser(string claimCode, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_CouponItems SET UserId = @UserId,UserName=@UserName  WHERE ClaimCode = @ClaimCode");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, HiContext.Current.User.Username);
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, claimCode);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }


        /// <summary>
        /// 获取当前用户未查阅的优惠券数(排除已使用和已过期的)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserNotReadCoupons(int userId)
        {
            int result = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(1) FROM Ecshop_CouponItems a INNER JOIN Ecshop_Coupons b ON a.CouponId = b.CouponId WHERE a.CouponStatus=0 and a.UsedTime is NULL and a.ClosingTime > @ClosingTime and UserId = @UserId and a.CouponStatus =@CouponStatus");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.Int32, 0);
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
            result = (int)this.database.ExecuteScalar(sqlStringCommand);
            return result;
        }

        /// <summary>
        /// 获取当前用户未查阅的优惠券数(排除已使用和已过期的)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserNotReadVouchers(int userId)
        {
            int result = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(1) FROM Ecshop_VoucherItems a INNER JOIN Ecshop_Vouchers b ON a.VoucherId = b.VoucherId WHERE a.VoucherStatus=0 and a.UsedTime is NULL and a.Deadline > @ClosingTime and UserId = @UserId and a.IsRead =@IsRead");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "IsRead", DbType.Int32, 0);
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now.Date);
            result = (int)this.database.ExecuteScalar(sqlStringCommand);
            return result;
        }


        /// <summary>
        /// 更新优惠券为已阅（排除已使用和已过期的)）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int UpdateCouponsReaded(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"update a set a.IsRead = @IsRead
                                                                                    from Ecshop_CouponItems a 
                                                                                        JOIN Ecshop_Coupons b 
                                                                                            ON a.CouponId = b.CouponId
                                                                                            where a.CouponStatus=0 and a.UsedTime is NULL and b.ClosingTime > @ClosingTime and a.UserId=@UserId and a.IsRead <> @IsRead");
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "IsRead", DbType.Int32, 1);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public int CreateCouponsSendTypeItem(CouponsSendTypeItem item)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into Ecshop_CouponsSendTypeItem(CouponId,BindId,UserId) values(@CouponId,@BindId,@UserId)");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, item.CouponId);
            this.database.AddInParameter(sqlStringCommand, "BindId", DbType.Int32, item.BindId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, item.UserId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public IList<CouponsSendTypeItem> GetCouponsSendTypeItems(int couponid)
        {
            IList<CouponsSendTypeItem> result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_CouponsSendTypeItem WHERE CouponId=@CouponId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponid);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<CouponsSendTypeItem>(dataReader);
            }
            return result;
        }

        public bool DeleteCouponSendTypeItem(int couponId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_CouponsSendTypeItem WHERE CouponId = @CouponId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public int GetCountCouponItem(int couponId, int userId)
        {
            int count = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(1) FROM Ecshop_CouponItems WHERE CouponId = @CouponId and UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
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

        /// <summary>
        /// 已经领取的优惠卷
        /// </summary>
        /// <param name="userId">userid</param>
        /// <param name="sendtype">发送方式</param>
        /// <returns></returns>
        public int GetCountCouponItemed(int userId,int sendtype)
        {
            int count = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select COUNT(1) 
from  Ecshop_CouponItems ci 
join Ecshop_Coupons c on ci.CouponId = c.CouponId 
where ci.UserId = @UserId and c.SendType = @SendType
");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "SendType", DbType.Int32, sendtype); 
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


        public int GetCountCouponItem(int couponid, string sendorderid)
        {
            int count = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(1) FROM Ecshop_CouponItems WHERE CouponId = @CouponId and SendOrderId=@SendOrderId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponid);
            this.database.AddInParameter(sqlStringCommand, "SendOrderId", DbType.String, sendorderid);
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
        /// 还原优惠券的使用 将orderId和UsedTime设为null
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool RevertCoupon(string orderId,DbTransaction dbTran)
        {
            bool result;
            string sql = "update Ecshop_CouponItems set OrderId = null,UsedTime = null,CouponStatus = 0 where OrderId=@OrderId";

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


        public int GetCouponCounts(string orderId)
        {
            int result;
            string sql = "SELECT COUNT(1) FROM dbo.Ecshop_CouponItems where OrderId =@OrderId";
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

        public CouponInfo GetCouponByName(string couponName)
        {
            CouponInfo couponinfo = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT c.* FROM Ecshop_Coupons c INNER  JOIN Ecshop_CouponItems ci ON ci.CouponId = c.CouponId Where c.Name  like @Name   and  CouponStatus=0 AND  @DateTime>c.StartTime and  @DateTime <c.ClosingTime");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, "'%" + couponName + "%'");
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.UtcNow);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    couponinfo = DataMapper.PopulateCoupon(dataReader);
                }
            }
            return couponinfo;
        }

        /// <summary>
        /// 根据特定的时间获取需要提醒的优惠券
        /// </summary>
        /// <param name="curDate"></param>
        /// <returns></returns>
        public DataTable GetAllCountDownCoupons(DateTime curDate)
        {
            DataTable result = new DataTable();
            string sql = @"select am.CellPhone,ec.*  
from Ecshop_CouponItems ec 
inner join aspnet_Members am on am.UserId = ec.UserId 
where ec.CouponStatus != 1 and ec.ClosingTime >GETDATE() and am.CellPhone is not null 
and isnull(ec.WarnStatus,0) !=1 
and @curDate <= ec.ClosingTime and ec.ClosingTime < DATEADD(hh,24,@curDate)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "curDate", DbType.DateTime, curDate);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;

        }
    }
}

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Members
{
	public class BalanceDetailDao
	{
		private Database database;
		public BalanceDetailDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public bool InsertBalanceDetail(BalanceDetailInfo balanceDetails)
		{
			bool result;
			if (null == balanceDetails)
			{
				result = false;
			}
			else
			{
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_BalanceDetails (UserId, UserName, TradeDate, TradeType, Income, Expenses, Balance, Remark, InpourId) VALUES(@UserId, @UserName, @TradeDate, @TradeType, @Income, @Expenses, @Balance, @Remark, @InpourId);");
				this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balanceDetails.UserId);
				this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balanceDetails.UserName);
				this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, balanceDetails.TradeDate);
				this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int)balanceDetails.TradeType);
				this.database.AddInParameter(sqlStringCommand, "Income", DbType.Currency, balanceDetails.Income);
				this.database.AddInParameter(sqlStringCommand, "Expenses", DbType.Currency, balanceDetails.Expenses);
				this.database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, balanceDetails.Balance);
				this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, balanceDetails.Remark);
                this.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, balanceDetails.InpourId);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
        public bool IsTradeNoExists(string tradeNo)
        {
            if (string.IsNullOrEmpty(tradeNo))
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(0) FROM dbo.Ecshop_BalanceDetails WHERE InpourId=@InpourId;");
            this.database.AddInParameter(sqlStringCommand, "@InpourId", DbType.String, tradeNo);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj == null)
            {
                return false;
            }
            return ((int)obj) > 0;
        }
        /// <summary>
        /// 获取供应商品订单交易明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetVendorSalesDetail(VendorSalesDetailQuery query)
        {
            DbQueryResult result;
            if (null == query)
            {
                result = new DbQueryResult();
            }
            else
            {
                DbQueryResult dbQueryResult = new DbQueryResult();
                StringBuilder stringBuilder = new StringBuilder();
                int StartIndex = (query.PageIndex - 1) * query.PageSize;
                int EndIndex = query.PageIndex * query.PageSize;
                string text = this.BuildVendorSalesDetailsQuery(query);
                stringBuilder.AppendFormat(@"select * from (select 
                                            Row_Number() OVER (ORDER BY PayDate desc) 'JournalNumber',
                                            a.OrderId,
                                            c.ProductName,
                                            c.ProductCode,
                                            b.ItemAdjustedPrice,
                                            d.SupplierName,
                                            a.PayDate from dbo.Ecshop_Orders  as a
                                            left join Ecshop_OrderItems as b on a.OrderId=b.OrderId
                                            left join Ecshop_Products as c on b.ProductId=c.ProductId
                                            inner join Ecshop_Supplier as d on c.SupplierId=d.SupplierId
                                            where OrderStatus<10 and OrderStatus!=1 and OrderStatus!=4 
                                            {0} 
                                            ) as c where JournalNumber>{1} and JournalNumber<{2}", text, StartIndex, EndIndex);
              
                if (query.IsCount)
                {
                    stringBuilder.AppendFormat(@";
                                                select COUNT(1) from dbo.Ecshop_Orders  as a
                                                left join Ecshop_OrderItems as b on a.OrderId=b.OrderId
                                                left join Ecshop_Products as c on b.ProductId=c.ProductId
                                                inner join Ecshop_Supplier as d on c.SupplierId=d.SupplierId
                                                where OrderStatus<10 and OrderStatus!=1 and OrderStatus!=4 {0} ", text);
                }



                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
                    if (query.IsCount && dataReader.NextResult())
                    {
                        dataReader.Read();
                        dbQueryResult.TotalRecords = dataReader.GetInt32(0);
                    }
                }
                result = dbQueryResult;
            }
            return result;
        }
        public decimal GetBalanceAmountByUserId(int userId,string cloumn)
        {
            decimal amount;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select sum({0}) as Total from Ecshop_BalanceDetails where userId={1}",cloumn, userId));
            decimal.TryParse( this.database.ExecuteScalar(sqlStringCommand).ToString(),out amount);
            return amount;
        }
		public DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
		{
			DbQueryResult result;
			if (null == query)
			{
				result = new DbQueryResult();
			}
			else
			{
				DbQueryResult dbQueryResult = new DbQueryResult();
				StringBuilder stringBuilder = new StringBuilder();
				string text = this.BuildBalanceDetailsQuery(query);
				stringBuilder.AppendFormat("SELECT TOP {0} *", query.PageSize);
				stringBuilder.Append(" FROM Ecshop_BalanceDetails B where 0=0 ");
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC", text);
				}
				else
				{
					stringBuilder.AppendFormat(" and JournalNumber < (select min(JournalNumber) from (select top {0} JournalNumber from Ecshop_BalanceDetails where 0=0 {1} ORDER BY JournalNumber DESC ) as tbltemp) {1} ORDER BY JournalNumber DESC", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(";select count(JournalNumber) as Total from Ecshop_BalanceDetails where 0=0 {0}", text);
				}
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
					if (query.IsCount && dataReader.NextResult())
					{
						dataReader.Read();
						dbQueryResult.TotalRecords = dataReader.GetInt32(0);
					}
				}
				result = dbQueryResult;
			}
			return result;
		}
        public DbQueryResult GetVendorSalesReport(VendorSalesReportQuery query)
        {
            DbQueryResult result;
            if (null == query)
            {
                result = new DbQueryResult();
            }
            else
            {
                DbQueryResult dbQueryResult = new DbQueryResult();
                StringBuilder stringBuilder = new StringBuilder();
                string text = this.BuildVendorSalesReportQueryQuery(query);
                int StartIndex = (query.PageIndex - 1) * query.PageSize;
                int EndIndex = query.PageIndex * query.PageSize;
                stringBuilder.AppendFormat(@"select * from 
                                            (
	                                            select COUNT(1) as 'OrderCount',SUM(ItemAdjustedPrice) as 'OrderTotal',SupplierName as 'SupplierName',SupplierId,
	                                            Row_Number() OVER (ORDER BY SupplierName desc) 'JournalNumber' 
	                                            from 
	                                            (
		                                            select distinct a.OrderId,b.ItemAdjustedPrice*Quantity as 'ItemAdjustedPrice',c.SupplierId,d.SupplierName from dbo.Ecshop_Orders  as a
		                                            left join Ecshop_OrderItems as b on a.OrderId=b.OrderId
		                                            left join Ecshop_Products as c on b.ProductId=c.ProductId
		                                            inner join Ecshop_Supplier as d on c.SupplierId=d.SupplierId
		                                            where (OrderStatus != 9 and OrderStatus!=1 and OrderStatus!=4 and OrderStatus!=98) 
		                                            {0}
	                                            ) as c group by SupplierName,SupplierId
                                            ) as RS  where JournalNumber>{1} and JournalNumber<{2}", text, StartIndex, EndIndex);

                if (query.IsCount)
                {
                    stringBuilder.AppendFormat(@";select count(1) from 
                                                 (
	                                                select COUNT(1) as 'OrderCount',SUM(ItemAdjustedPrice) as 'OrderTotal',SupplierName as 'SupplierName',
	                                              	Row_Number() OVER (partition by SupplierName ORDER BY SupplierName desc) 'JournalNumber' 
	                                                from 
	                                                (
		                                                select distinct a.OrderId,b.ItemAdjustedPrice*Quantity as 'ItemAdjustedPrice',c.SupplierId,d.SupplierName from dbo.Ecshop_Orders  as a
		                                                left join Ecshop_OrderItems as b on a.OrderId=b.OrderId
		                                                left join Ecshop_Products as c on b.ProductId=c.ProductId
		                                                inner join Ecshop_Supplier as d on c.SupplierId=d.SupplierId
		                                                where (OrderStatus != 9 and OrderStatus!=1 and OrderStatus!=4 and OrderStatus!=98)
		                                                {0}
	                                                ) as c group by SupplierName
                                                 ) as RS ", text);
                }

                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
                    if (query.IsCount && dataReader.NextResult())
                    {
                        dataReader.Read();
                        dbQueryResult.TotalRecords = dataReader.GetInt32(0);
                    }
                }
                result = dbQueryResult;
            }
            return result;
        }

		public DbQueryResult GetMemberBlanceList(MemberQuery query)
		{
			DbQueryResult result;
			if (null == query)
			{
				result = new DbQueryResult();
			}
			else
			{
				DbQueryResult dbQueryResult = new DbQueryResult();
				StringBuilder stringBuilder = new StringBuilder();
				string text = string.Empty;
				if (!string.IsNullOrEmpty(query.Username))
				{
					text = string.Format("AND UserId IN (SELECT UserId FROM vw_aspnet_Members WHERE UserName LIKE '%{0}%')", DataHelper.CleanSearchString(query.Username));
				}
				if (!string.IsNullOrEmpty(query.Realname))
				{
					text += string.Format(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
				}
				stringBuilder.AppendFormat("SELECT TOP {0} *", query.PageSize);
				stringBuilder.Append(" FROM vw_aspnet_Members WHERE 0=0");
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat("{0} ORDER BY CreateDate DESC", text);
				}
				else
				{
					stringBuilder.AppendFormat("AND CreateDate < (select min(CreateDate) FROM (SELECT TOP {0} CreateDate FROM vw_aspnet_Members WHERE 0=0 {1} ORDER BY CreateDate DESC ) AS tbltemp) {1} ORDER BY CreateDate DESC", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(";SELECT COUNT(CreateDate) AS Total FROM vw_aspnet_Members WHERE 0=0 {0}", text);
				}
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
					if (query.IsCount && dataReader.NextResult())
					{
						dataReader.Read();
						dbQueryResult.TotalRecords = dataReader.GetInt32(0);
					}
				}
				result = dbQueryResult;
			}
			return result;
		}
		public DbQueryResult GetBalanceDetailsNoPage(BalanceDetailQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			string arg = this.BuildBalanceDetailsQuery(query);
			stringBuilder.Append("SELECT * FROM Ecshop_BalanceDetails WHERE 0=0 ");
			stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC", arg);
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";select count(JournalNumber) as Total from Ecshop_BalanceDetails where 0=0 {0}", arg);
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}


		public DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			string text = this.BuildBalanceDrawRequestQuery(query);
			stringBuilder.AppendFormat("select top {0} *", query.PageSize);
			stringBuilder.Append(" from Ecshop_BalanceDrawRequest B where 0=0 ");
			if (query.PageIndex == 1)
			{
				stringBuilder.AppendFormat("{0} ORDER BY RequestTime DESC", text);
			}
			else
			{
				stringBuilder.AppendFormat(" and RequestTime < (select min(RequestTime) from (select top {0} RequestTime from Ecshop_BalanceDrawRequest where 0=0 {1} ORDER BY RequestTime DESC ) as tbltemp) {1} ORDER BY RequestTime DESC", (query.PageIndex - 1) * query.PageSize, text);
			}
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";select count(*) as Total from Ecshop_BalanceDrawRequest where 0=0 {0}", text);
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}
		public DbQueryResult GetBalanceDrawRequestsNoPage(BalanceDrawRequestQuery query)
		{
			DbQueryResult result;
			if (null == query)
			{
				result = new DbQueryResult();
			}
			else
			{
				DbQueryResult dbQueryResult = new DbQueryResult();
				StringBuilder stringBuilder = new StringBuilder();
				string arg = this.BuildBalanceDrawRequestQuery(query);
				stringBuilder.Append("select *");
				stringBuilder.Append(" from Ecshop_BalanceDrawRequest B where 0=0 ");
				stringBuilder.AppendFormat("{0} ORDER BY RequestTime DESC", arg);
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(";select count(*) as Total from Ecshop_BalanceDrawRequest where 0=0 {0}", arg);
				}
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
					if (query.IsCount && dataReader.NextResult())
					{
						dataReader.Read();
						dbQueryResult.TotalRecords = dataReader.GetInt32(0);
					}
				}
				result = dbQueryResult;
			}
			return result;
		}
		public bool DealBalanceDrawRequest(int userId, bool agree)
		{
			DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_BalanceDrawRequest_Update");
			this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
			this.database.AddInParameter(storedProcCommand, "Agree", DbType.Boolean, agree);
			this.database.ExecuteNonQuery(storedProcCommand);
			object parameterValue = this.database.GetParameterValue(storedProcCommand, "Status");
			return parameterValue != DBNull.Value && parameterValue != null && (int)this.database.GetParameterValue(storedProcCommand, "Status") == 0;
		}
		public bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_BalanceDrawRequest (UserId,UserName,RequestTime, Amount, AccountName, BankName, MerchantCode, Remark) VALUES (@UserId,@UserName,@RequestTime, @Amount, @AccountName, @BankName, @MerchantCode, @Remark)");
			this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balanceDrawRequest.UserId);
			this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balanceDrawRequest.UserName);
			this.database.AddInParameter(sqlStringCommand, "RequestTime", DbType.DateTime, balanceDrawRequest.RequestTime);
			this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, balanceDrawRequest.Amount);
			this.database.AddInParameter(sqlStringCommand, "AccountName", DbType.String, balanceDrawRequest.AccountName);
			this.database.AddInParameter(sqlStringCommand, "BankName", DbType.String, balanceDrawRequest.BankName);
			this.database.AddInParameter(sqlStringCommand, "MerchantCode", DbType.String, balanceDrawRequest.MerchantCode);
			this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, balanceDrawRequest.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		private string BuildBalanceDetailsQuery(BalanceDetailQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
			}
			if (query.TradeType != TradeTypes.NotSet)
			{
				stringBuilder.AppendFormat(" AND TradeType = {0}", (int)query.TradeType);
			}
			return stringBuilder.ToString();
		}

        private string BuildVendorSalesDetailsQuery(VendorSalesDetailQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if(!string.IsNullOrEmpty(query.SupplierId))
            {
               stringBuilder.AppendFormat(" AND d.SupplierId={0}", query.SupplierId);
            }
            
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                stringBuilder.AppendFormat(" AND a.OrderId   like '%{0}%'", query.OrderId);
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                stringBuilder.AppendFormat(" AND c.ProductCode  like '%{0}%'", query.ProductCode);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                stringBuilder.AppendFormat(" AND c.ProductName  like '%{0}%'", query.ProductName);
            }
            if (query.FromDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND PayDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
            }
            if (query.ToDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND PayDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
            }
            return stringBuilder.ToString();
        }


        private string BuildVendorSalesReportQueryQuery(VendorSalesReportQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.SupplierName))
            {
                stringBuilder.AppendFormat(" AND SupplierName  like  '%{0}%'", query.SupplierName);
            }
            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND PayDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND PayDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            
            return stringBuilder.ToString();
        }

		private string BuildBalanceDrawRequestQuery(BalanceDrawRequestQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestTime <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
			}
			return stringBuilder.ToString();
		}
	}
}

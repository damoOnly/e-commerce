using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Members;
using Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Members
{
	public class ReconciliationOrdersDao
	{
		private Database database;
        public ReconciliationOrdersDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
        /// <summary>
        /// 获取对账订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetReconciliationOrders(ReconciliationOrdersQuery query)
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
                string text = this.BuildReconciliationQuery(query);
                if(string.IsNullOrEmpty(text))
                {
                    return result = new DbQueryResult();
                }
				if (query.PageIndex == 1)
				{
                    stringBuilder.AppendFormat(" SELECT TOP {0} * FROM VW_GetTrade B where 0=0 {1} ORDER BY TradingTime DESC",query.PageSize,text);
				}
				else
				{
                    stringBuilder.AppendFormat("select TOP {0} * from (select *, Row_Number() OVER (ORDER BY TradingTime desc) 'Number' from VW_GetTrade where 0=0 {1} ) B  where Number <={2} and  Number>{3} ", query.PageSize, text, query.PageIndex * query.PageSize, (query.PageIndex - 1) * query.PageSize);
				}
				if (query.IsCount)
				{
                    stringBuilder.AppendFormat(";select count(1) as Total from VW_GetTrade where 0=0 {0} ", text);
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
        /// <summary>
        /// 导出对账订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult ExportReconciliationOrders(ReconciliationOrdersQuery query)
        {
           DbQueryResult result;
           if (null == query)
           {
               result = new DbQueryResult();
           }
           else
           {
               DbQueryResult dbQueryResult = new DbQueryResult();
               string text = this.BuildReconciliationQuery(query);
               if (string.IsNullOrEmpty(text))
               {
                   return result = new DbQueryResult();
               }
               string Sql = string.Format("SELECT  TradingTime  as [交易日期],[ShippingDate] as 发货时间,[RefundTime] as [退款时间],OrderId  as [订单号],ActualAmount  as [实际金额],TotalAmount  as [收款金额],RefundAmount  as [退款金额],RealName  as [买家昵称],OrderDate  as [拍单时间],PayDate  as [付款时间],Amount  as [商品总金额],OrderTotal  as [订单金额],DiscountAmount  as [优惠金额],AdjustedFreight  as [运费],Tax  as [税额],CouponValue as 优惠券,VoucherValue as 现金劵,PayCharge  as [货到付款服务费],ManagerRemark  as [订单备注],Remark  as [买家留言],Address  as [收货地址],ShipTo  as [收货人名称],Country  as [收货国家],Province  as [州 省],City  as [城市],Area  as [区],ZipCode  as [邮编],TelPhone  as [联系电话],CellPhone  as [手机],ModeName  as [买家选择物流],LatestDelivery  as [最晚发货时间],OverseasOrders  as [海外订单],CashDelivery  as [是否货到付款],OrderStatus  as [订单状态],ShipOrderNumber  as [发货快递单号],ExpressCompanyName  as [快递公司],ownerId  as [货主Id],ownername  as [货主名称]  FROM VW_GetTrade B where 0=0 {0}", text);
               DbCommand sqlStringCommand = this.database.GetSqlStringCommand(Sql.ToString());
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



        /// <summary>
        /// 获取对账订单 明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetReconciliationOrdersDetailed(ReconciliationOrdersQuery query)
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
                string text = this.BuildReconciliationQuery(query);
                if (string.IsNullOrEmpty(text))
                {
                    return result = new DbQueryResult();
                }
                if (query.PageIndex == 1)
                {
                    stringBuilder.AppendFormat(" SELECT TOP {0} *,ItemAdjustedPrice*TaxRate*(case when Quantity-ReturnQuantity <0 then 0 else Quantity-ReturnQuantity end) as Tax FROM VW_GetTradeDetails B where 0=0 {1} ORDER BY TradingTime DESC", query.PageSize, text);
                }
                else
                {
                    stringBuilder.AppendFormat("select TOP {0} *,ItemAdjustedPrice*TaxRate*(case when Quantity-ReturnQuantity <0 then 0 else Quantity-ReturnQuantity end) as Tax from (select *, Row_Number() OVER (ORDER BY TradingTime desc) 'Number' from VW_GetTradeDetails where 0=0 {1} ) B  where Number <={2} and  Number>{3} ", query.PageSize, text, query.PageIndex * query.PageSize, (query.PageIndex - 1) * query.PageSize);
                }
                if (query.IsCount)
                {
                    stringBuilder.AppendFormat(";select count(1) as Total from VW_GetTradeDetails where 0=0 {0} ", text);
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
        public DbQueryResult GetReconciliationExpressFeeDetails(ReconciliationExpressFeeQuery query)
        {
            DbQueryResult result;
            if (null == query||string.IsNullOrWhiteSpace(query.BeginTime)||string.IsNullOrWhiteSpace(query.EndTime))
            {
                result = new DbQueryResult();
            }
            else
            {
                DbQueryResult dbQueryResult = new DbQueryResult();
                StringBuilder stringBuilder = new StringBuilder();
               // string text = this.BuildReconciliationQuery(query);
                //if (string.IsNullOrEmpty(text))
                //{
                //    return result = new DbQueryResult();
                //}
                //if (query.PageIndex == 1)
                //{
                //    stringBuilder.AppendFormat(" SELECT TOP {0} * FROM VW_GetTradeDetails B where 0=0 {1} ORDER BY TradingTime DESC", query.PageSize, text);
                //}
                //else
                //{
                //    stringBuilder.AppendFormat("select TOP {0} * from (select *, Row_Number() OVER (ORDER BY TradingTime desc) 'Number' from VW_GetTradeDetails where 0=0 {1} ) B  where Number <={2} and  Number>{3} ", query.PageSize, text, query.PageIndex * query.PageSize, (query.PageIndex - 1) * query.PageSize);
                //}
                //if (query.IsCount)
                //{
                //    stringBuilder.AppendFormat(";select count(1) as Total from VW_GetTradeDetails where 0=0 {0} ", text);
                //}
                DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_GetExpressFee");
                //@pageIndex 
                //@pageSize 
                //@DetailsType
                //@PayDate
                //@beginTime
                //@endTime
                this.database.AddInParameter(storedProcCommand, "pageIndex", DbType.Int32, query.PageIndex);
                this.database.AddInParameter(storedProcCommand, "pageSize", DbType.Int32, query.PageSize);
                this.database.AddInParameter(storedProcCommand, "DetailsType", DbType.String, string.IsNullOrEmpty(query.DetailsType) ? "" : query.DetailsType);
                this.database.AddInParameter(storedProcCommand, "PayDate", DbType.String, string.IsNullOrEmpty(query.PayDate) ? "" : query.PayDate);
                this.database.AddInParameter(storedProcCommand, "beginTime", DbType.String, string.IsNullOrEmpty(query.BeginTime) ? "" : query.BeginTime);
                this.database.AddInParameter(storedProcCommand, "endTime", DbType.String, string.IsNullOrEmpty(query.EndTime) ? "" : query.EndTime);
                this.database.AddInParameter(storedProcCommand, "supplierId", DbType.Int32, query.Supplier);
                storedProcCommand.CommandTimeout = 300;
                using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
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

        /// <summary>
        /// 导出对账订单明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult ExportReconciliationOrdersDetailed(ReconciliationOrdersQuery query)
        {
            DbQueryResult result;
            if (null == query)
            {
                result = new DbQueryResult();
            }
            else
            {
                DbQueryResult dbQueryResult = new DbQueryResult();
                string text = this.BuildReconciliationQuery(query);
                if (string.IsNullOrEmpty(text))
                {
                    return result = new DbQueryResult();
                }
                string Sql = string.Format("SELECT [TradingTime] as [交易日期],[ShippingDate] as 发货时间,[RefundTime] as [退款时间],[OrderId] as [订单号],ChildOrderId as 子订单号,[ActualAmount] as [实际金额],[TotalAmount] as [收款金额],[RefundAmount] as [退款金额],[RealName] as [买家昵称],[OrderDate] as [拍单时间],[PayDate] as [付款时间],[Amount] as [商品总金额],[OrderTotal] as [订单金额],[DiscountAmount] as [优惠金额],ItemAdjustedPrice*TaxRate*(case when Quantity-ReturnQuantity <0 then 0 else Quantity-ReturnQuantity end) as [税费],[AdjustedFreight] as [运费],CouponValue as 优惠券,VoucherValue as 现金劵,[PayCharge] as [货到付款服务费],DeductFee as 扣点,CostPrice as 成本价,[ManagerRemark] as [订单备注],[Remark] as [买家留言],[Address] as [收货地址],[ShipTo] as [收货人名称],[Country] as [收货国家],[Province] as [州/省],[City] as [城市],[Area] as [区],[ZipCode] as [邮编],[TelPhone] as [联系电话],[CellPhone] as [手机],[ModeName] as [买家选择物流],[LatestDelivery] as [最晚发货时间],[OverseasOrders] as [海外订单],[CashDelivery] as [是否货到付款],[OrderStatus] as [订单状态],[ShipOrderNumber] as [发货快递单号],ReturnsExpressNumber as 退货快递单号,[ItemDescription] as [商品名称],[SKU] as [商品条码],[Quantity] as [购买数量],[ReturnQuantity] as [退货数量],[ItemListPrice] as [售价],[ItemAdjustedPrice] as [销售价格],[SupplierId] as [货主Id],[SupplierName] as [货主名称],ShipWarehouseName as 发货仓库,[Template] as [是否匹配促销模板]  FROM VW_GetTradeDetails where 0=0 {0}", text);
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(Sql.ToString());
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
        /// <summary>
        /// 获取应付总汇
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet GetReport(ReconciliationOrdersQuery query)
        {
            DataSet ds = new DataSet();
            DbQueryResult dbQueryResult = new DbQueryResult();
            string text = this.BuildReconciliationQuery(query);
            if (string.IsNullOrEmpty(text))
            {
                return new DataSet();
            }
            if(!string.IsNullOrEmpty(text))
            {
                //根据供应商品的商品金额
                DbCommand storedProcCommand = this.database.GetStoredProcCommand("proc_AggregatePayable");
                this.database.AddInParameter(storedProcCommand, "Strwhere", DbType.String, text);
                using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
                {
                    ds.Tables.Add(DataHelper.ConverDataReaderToDataTable(dataReader));
                }

                //平台汇总金额
                storedProcCommand = this.database.GetStoredProcCommand("proc_AggregatePayableByAllData");
                this.database.AddInParameter(storedProcCommand, "Strwhere", DbType.String, text);
                using (IDataReader dataReader1 = this.database.ExecuteReader(storedProcCommand))
                {
                    ds.Tables.Add(DataHelper.ConverDataReaderToDataTable(dataReader1));
                }
                #region 屏蔽代码
                //                string Sql = string.Format("select SupplierName    as '供应商',  ISNULL(sum(Amount),0)   as '商品金额',ISNULL(SUM(RefundAmount),0)  as '退款金额' from VW_AggregatePayable  where 0=0 {0} group by SupplierName;", text);
//                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(Sql.ToString());
//                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
//                {
//                    ds.Tables.Add(DataHelper.ConverDataReaderToDataTable(dataReader));  
//                }
//                string sql1 = string.Format(@"select  
//                                                ISNULL(SUM(RefundAmount),0) as '退款金额',
//                                                ISNULL(SUM(OrderTotal),0) as '银行收款', 
//                                                ISNULL(SUM(Amount)-SUM(RefundAmount),0)    as '商品总金额', 
//                                                ISNULL(sum(AdjustedFreight),0)  as '代收快递运费' , 
//                                                ISNULL(SUM(OrderCounterFee)+SUM(RefunCounterFee),0) as '微信转账手续费'    
//                                               from  VW_AggregatePayable  where 0=0 {0} ;", text);
//                sqlStringCommand = this.database.GetSqlStringCommand(sql1);
//                using (IDataReader dataReader1 = this.database.ExecuteReader(sqlStringCommand))
//                {
//                    ds.Tables.Add(DataHelper.ConverDataReaderToDataTable(dataReader1));
                //                }
                #endregion 
            }
            return ds;
        
        }
        private string BuildReconciliationQuery(ReconciliationOrdersQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.Supplier))
            {
                stringBuilder.AppendFormat(" AND SupplierId={0}", query.Supplier);
            }
            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND TradingTime >= '{0}'",Convert.ToDateTime(query.StartDate.Value).ToString("yyyy-MM-dd") );
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND TradingTime <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            
            return stringBuilder.ToString();
        }
	}
}

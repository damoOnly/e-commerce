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
        /// ��ȡ���˶���
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
        /// �������˶���
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
               string Sql = string.Format("SELECT  TradingTime  as [��������],[ShippingDate] as ����ʱ��,[RefundTime] as [�˿�ʱ��],OrderId  as [������],ActualAmount  as [ʵ�ʽ��],TotalAmount  as [�տ���],RefundAmount  as [�˿���],RealName  as [����ǳ�],OrderDate  as [�ĵ�ʱ��],PayDate  as [����ʱ��],Amount  as [��Ʒ�ܽ��],OrderTotal  as [�������],DiscountAmount  as [�Żݽ��],AdjustedFreight  as [�˷�],Tax  as [˰��],CouponValue as �Ż�ȯ,VoucherValue as �ֽ���,PayCharge  as [������������],ManagerRemark  as [������ע],Remark  as [�������],Address  as [�ջ���ַ],ShipTo  as [�ջ�������],Country  as [�ջ�����],Province  as [�� ʡ],City  as [����],Area  as [��],ZipCode  as [�ʱ�],TelPhone  as [��ϵ�绰],CellPhone  as [�ֻ�],ModeName  as [���ѡ������],LatestDelivery  as [������ʱ��],OverseasOrders  as [���ⶩ��],CashDelivery  as [�Ƿ��������],OrderStatus  as [����״̬],ShipOrderNumber  as [������ݵ���],ExpressCompanyName  as [��ݹ�˾],ownerId  as [����Id],ownername  as [��������]  FROM VW_GetTrade B where 0=0 {0}", text);
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
        /// ��ȡ���˶��� ��ϸ
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
        /// �������˶�����ϸ
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
                string Sql = string.Format("SELECT [TradingTime] as [��������],[ShippingDate] as ����ʱ��,[RefundTime] as [�˿�ʱ��],[OrderId] as [������],ChildOrderId as �Ӷ�����,[ActualAmount] as [ʵ�ʽ��],[TotalAmount] as [�տ���],[RefundAmount] as [�˿���],[RealName] as [����ǳ�],[OrderDate] as [�ĵ�ʱ��],[PayDate] as [����ʱ��],[Amount] as [��Ʒ�ܽ��],[OrderTotal] as [�������],[DiscountAmount] as [�Żݽ��],ItemAdjustedPrice*TaxRate*(case when Quantity-ReturnQuantity <0 then 0 else Quantity-ReturnQuantity end) as [˰��],[AdjustedFreight] as [�˷�],CouponValue as �Ż�ȯ,VoucherValue as �ֽ���,[PayCharge] as [������������],DeductFee as �۵�,CostPrice as �ɱ���,[ManagerRemark] as [������ע],[Remark] as [�������],[Address] as [�ջ���ַ],[ShipTo] as [�ջ�������],[Country] as [�ջ�����],[Province] as [��/ʡ],[City] as [����],[Area] as [��],[ZipCode] as [�ʱ�],[TelPhone] as [��ϵ�绰],[CellPhone] as [�ֻ�],[ModeName] as [���ѡ������],[LatestDelivery] as [������ʱ��],[OverseasOrders] as [���ⶩ��],[CashDelivery] as [�Ƿ��������],[OrderStatus] as [����״̬],[ShipOrderNumber] as [������ݵ���],ReturnsExpressNumber as �˻���ݵ���,[ItemDescription] as [��Ʒ����],[SKU] as [��Ʒ����],[Quantity] as [��������],[ReturnQuantity] as [�˻�����],[ItemListPrice] as [�ۼ�],[ItemAdjustedPrice] as [���ۼ۸�],[SupplierId] as [����Id],[SupplierName] as [��������],ShipWarehouseName as �����ֿ�,[Template] as [�Ƿ�ƥ�����ģ��]  FROM VW_GetTradeDetails where 0=0 {0}", text);
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
        /// ��ȡӦ���ܻ�
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
                //���ݹ�Ӧ��Ʒ����Ʒ���
                DbCommand storedProcCommand = this.database.GetStoredProcCommand("proc_AggregatePayable");
                this.database.AddInParameter(storedProcCommand, "Strwhere", DbType.String, text);
                using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
                {
                    ds.Tables.Add(DataHelper.ConverDataReaderToDataTable(dataReader));
                }

                //ƽ̨���ܽ��
                storedProcCommand = this.database.GetStoredProcCommand("proc_AggregatePayableByAllData");
                this.database.AddInParameter(storedProcCommand, "Strwhere", DbType.String, text);
                using (IDataReader dataReader1 = this.database.ExecuteReader(storedProcCommand))
                {
                    ds.Tables.Add(DataHelper.ConverDataReaderToDataTable(dataReader1));
                }
                #region ���δ���
                //                string Sql = string.Format("select SupplierName    as '��Ӧ��',  ISNULL(sum(Amount),0)   as '��Ʒ���',ISNULL(SUM(RefundAmount),0)  as '�˿���' from VW_AggregatePayable  where 0=0 {0} group by SupplierName;", text);
//                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(Sql.ToString());
//                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
//                {
//                    ds.Tables.Add(DataHelper.ConverDataReaderToDataTable(dataReader));  
//                }
//                string sql1 = string.Format(@"select  
//                                                ISNULL(SUM(RefundAmount),0) as '�˿���',
//                                                ISNULL(SUM(OrderTotal),0) as '�����տ�', 
//                                                ISNULL(SUM(Amount)-SUM(RefundAmount),0)    as '��Ʒ�ܽ��', 
//                                                ISNULL(sum(AdjustedFreight),0)  as '���տ���˷�' , 
//                                                ISNULL(SUM(OrderCounterFee)+SUM(RefunCounterFee),0) as '΢��ת��������'    
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

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
	public class RetailBusinessDa
	{
		private Database database;
        public RetailBusinessDa()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
        /// <summary>
        /// ��Ʒ�ķ��ʺ͹������ͼ
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetaccessPurchaseRatio(CommoditySalesQuery query)
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
                stringBuilder.AppendFormat(@"select top {0} * from (select   Row_Number() OVER (ORDER BY ProductId desc) 'JournalNumber',ProductCode,ProductName,VistiCounts,
                                            isnull((select sum(Quantity) from Ecshop_OrderItems b where b.ProductId=a.ProductId),0) as 'Quantity'
                                            from dbo.Ecshop_Products as a ) as  B where 0=0 ", query.PageSize);
				if (query.PageIndex == 1)
				{
                    stringBuilder.AppendFormat("{0} ORDER BY ProductId DESC", text);
				}
				else
				{
                    stringBuilder.AppendFormat("{0} and JournalNumber <{1} and  JournalNumber>={2} ", text,query.PageIndex * query.PageSize,(query.PageIndex - 1) * query.PageSize );
				}
				if (query.IsCount)
				{
                    stringBuilder.AppendFormat(";select count(1) as Total from Ecshop_Products where 0=0 {0} ", text);
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
        /// ��Ʒ������ϸ
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetCommoditySalesDetail(CommoditySalesQuery query)
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
                stringBuilder.AppendFormat("SELECT TOP {0} *", query.PageSize);
                stringBuilder.AppendFormat(@" FROM 
                                             ( select Row_Number() OVER (ORDER BY Quantity desc) 'JournalNumber',* from 
                                               (
                                                select b.SKU,b.SkuId,b.ItemDescription as 'ProductName',sum(b.Quantity) as 'Quantity' from dbo.Ecshop_Orders as a
                                                left join dbo.Ecshop_OrderItems as b on a.OrderId=b.OrderId 
                                                where 1=1 {0}  group by b.SKU,b.SkuId,b.ItemDescription 
                                                ) as T
                                             )  as M ", text);
                if (query.PageIndex == 1)
                {
                    stringBuilder.AppendFormat("{0} ORDER BY Quantity DESC", text);
                }
                else
                {
                    stringBuilder.AppendFormat(" {0} and JournalNumber <{1} and  JournalNumber>={2} ", text, query.PageIndex * query.PageSize, (query.PageIndex - 1) * query.PageSize);
                }
                if (query.IsCount)
                {
                    stringBuilder.AppendFormat(@";select COUNT(1) from (
                                                select b.SKU,b.SkuId,b.ItemDescription,sum(b.Quantity) as 'Quantity' from dbo.Ecshop_Orders as a
                                                left join dbo.Ecshop_OrderItems as b on a.OrderId=b.OrderId 
                                                where  0=0 {0}  group by b.SKU,b.SkuId,b.ItemDescription ) as T ", text);
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
        /// ��ȡӦ���ܻ�
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet GetReport(CommoditySalesQuery query)
        {
            DataSet ds = new DataSet();
            DbQueryResult dbQueryResult = new DbQueryResult();
            string text = this.BuildReconciliationQuery(query);
            if(!string.IsNullOrEmpty(text))
            {
                string Sql = string.Format("select SupplierName    as '��Ӧ��',  ISNULL(sum(Amount),0)   as '��Ʒ���',ISNULL(SUM(RefundAmount),0)  as '�˿���' from VW_AggregatePayable  where 0=0 {0} group by SupplierName;", text);
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(Sql.ToString());
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    ds.Tables.Add(DataHelper.ConverDataReaderToDataTable(dataReader));  
                }
                string sql1 = string.Format(@"select  
                                                ISNULL(SUM(RefundAmount),0) as '�˿���',
                                                ISNULL(SUM(OrderTotal),0) as '�����տ�', 
                                                ISNULL(SUM(Amount)-SUM(RefundAmount),0)    as '��Ʒ�ܽ��', 
                                                ISNULL(sum(AdjustedFreight),0)  as '���տ���˷�' , 
                                                ISNULL(SUM(OrderCounterFee)+SUM(RefunCounterFee),0) as '΢��ת��������'    
                                               from  VW_AggregatePayable  where 0=0 {0} ;", text);
                sqlStringCommand = this.database.GetSqlStringCommand(sql1);
                using (IDataReader dataReader1 = this.database.ExecuteReader(sqlStringCommand))
                {
                    ds.Tables.Add(DataHelper.ConverDataReaderToDataTable(dataReader1));
                }
            }
            return ds;
        
        }
        private string BuildReconciliationQuery(CommoditySalesQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                stringBuilder.AppendFormat(" AND ProductName like '%{0}%'", query.ProductName);
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                stringBuilder.AppendFormat(" AND ProductCode like '%{0}%'", query.ProductCode);
            }
            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND PayDate >= '{0}'", Convert.ToDateTime(query.StartDate.Value).ToString("yyyy-MM-dd"));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND PayDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            
            return stringBuilder.ToString();
        }
	}
}

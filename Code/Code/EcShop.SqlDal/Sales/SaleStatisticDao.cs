using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web;
namespace EcShop.SqlDal.Sales
{
    public class SaleStatisticDao
    {
        private Database database;
        public SaleStatisticDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSales_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, productSale.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, productSale.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, productSale.IsCount);
            //this.database.AddInParameter(storedProcCommand, "ProductName", DbType.String, productSale.ProductName);
            //this.database.AddInParameter(storedProcCommand, "ProductCode", DbType.String, productSale.ProductCode);

            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, SaleStatisticDao.BuildProductSaleQuery(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return result;
        }
        public DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSalesNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, SaleStatisticDao.BuildProductSaleQuery(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return result;
        }

        public DataTable GetProductSaleAsBrand(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSaleAsBrand_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, productSale.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, productSale.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, productSale.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, SaleStatisticDao.BuildProductSaleAsBrandQuery(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return result;
        }
        public DataTable GetProductSaleAsBrandNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSaleAsBrandNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, SaleStatisticDao.BuildProductSaleAsBrandQuery(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return result;
        }

        public DataTable GetProductSaleAsImportSource(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSaleAsImportSource_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, productSale.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, productSale.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, productSale.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, SaleStatisticDao.BuildProductSaleAsImportSource(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return result;
        }
        public DataTable GetProductSaleAsImportSourceNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSaleAsImportSourceNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, SaleStatisticDao.BuildProductSaleAsImportSource(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return result;
        }

        public IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalRegionsUsers)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TopRegionId as RegionId,COUNT(UserId) as UserCounts,(select count(*) from aspnet_Members) as AllUserCounts FROM aspnet_Members  GROUP BY TopRegionId ");
            IList<UserStatisticsInfo> list = new List<UserStatisticsInfo>();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                UserStatisticsInfo userStatisticsInfo = null;
                while (dataReader.Read())
                {
                    userStatisticsInfo = DataMapper.PopulateUserStatistics(dataReader);
                    list.Add(userStatisticsInfo);
                }
                if (userStatisticsInfo != null)
                {
                    totalRegionsUsers = int.Parse(userStatisticsInfo.AllUserCounts.ToString());
                }
                else
                {
                    totalRegionsUsers = 0;
                }
            }
            return list;
        }
        public OrderStatisticsInfo GetUserOrders(OrderQuery userOrder)
        {
            OrderStatisticsInfo orderStatisticsInfo = new OrderStatisticsInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_OrderStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, userOrder.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, userOrder.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, userOrder.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, SaleStatisticDao.BuildUserOrderQuery(userOrder));
            this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);
            DataTable dataTable = new DataTable();
            dataTable.TableName = "orderxml";
            dataTable.Columns.Add(new DataColumn("search"));
            DataRow dataRow = dataTable.NewRow();
            dataRow["search"] = string.Concat(new object[]
			{
				SaleStatisticDao.BuildOrdersQuery(userOrder),
				"---",
				userOrder.OrderId,
				"---",
				userOrder.EndDate,
				"---",
				userOrder.StartDate,
				"---",
				userOrder.ShipTo
			});
            dataTable.Rows.Add(dataRow);
            dataTable.WriteXml(HttpContext.Current.Request.MapPath("/searchlog.xml"));
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                orderStatisticsInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
                if (dataReader.NextResult())
                {
                    dataReader.Read();
                    if (dataReader["OrderTotal"] != DBNull.Value)
                    {
                        orderStatisticsInfo.TotalOfPage += (decimal)dataReader["OrderTotal"];
                    }
                    if (dataReader["Profits"] != DBNull.Value)
                    {
                        orderStatisticsInfo.ProfitsOfPage += (decimal)dataReader["Profits"];
                    }
                }
                if (dataReader.NextResult())
                {
                    dataReader.Read();
                    if (dataReader["OrderTotal"] != DBNull.Value)
                    {
                        orderStatisticsInfo.TotalOfSearch += (decimal)dataReader["OrderTotal"];
                    }
                    if (dataReader["Profits"] != DBNull.Value)
                    {
                        orderStatisticsInfo.ProfitsOfSearch += (decimal)dataReader["Profits"];
                    }
                }
            }
            orderStatisticsInfo.TotalCount = (int)this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
            return orderStatisticsInfo;
        }
        public OrderStatisticsInfo GetUserOrdersNoPage(OrderQuery userOrder)
        {
            OrderStatisticsInfo orderStatisticsInfo = new OrderStatisticsInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_OrderStatisticsNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, SaleStatisticDao.BuildUserOrderQuery(userOrder));
            this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                orderStatisticsInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
                if (dataReader.NextResult())
                {
                    dataReader.Read();
                    if (dataReader["OrderTotal"] != DBNull.Value)
                    {
                        orderStatisticsInfo.TotalOfSearch += (decimal)dataReader["OrderTotal"];
                    }
                    if (dataReader["Profits"] != DBNull.Value)
                    {
                        orderStatisticsInfo.ProfitsOfSearch += (decimal)dataReader["Profits"];
                    }
                }
            }
            orderStatisticsInfo.TotalCount = (int)this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
            return orderStatisticsInfo;
        }
        public DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_MemberStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, SaleStatisticDao.BuildMemberStatisticsQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return result;
        }
        public DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(SaleStatisticDao.BuildMemberStatisticsQuery(query));
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductVisitAndBuyStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, SaleStatisticDao.BuildProductVisitAndBuyStatisticsQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return result;
        }
        public DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ProductName,VistiCounts,SaleCounts as BuyCount ,(SaleCounts/(case when VistiCounts=0 then 1 else VistiCounts end))*100 as BuyPercentage ");
            stringBuilder.Append("FROM Ecshop_Products WHERE SaleCounts>0 ORDER BY BuyPercentage DESC;");
            stringBuilder.Append("SELECT COUNT(*) as TotalProductSales FROM Ecshop_Products WHERE SaleCounts>0;");
            sqlStringCommand.CommandText = stringBuilder.ToString();
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    result = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                if (dataReader.NextResult() && dataReader.Read())
                {
                    totalProductSales = (int)dataReader["TotalProductSales"];
                }
                else
                {
                    totalProductSales = 0;
                }
            }
            return result;
        }
        public DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND orderDate >= '{0}'", query.StartDate.Value);
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND orderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            if (query.SupplierId.HasValue && query.SupplierId != 0)
            {
                stringBuilder.AppendFormat(" AND SupplierId = {0}", query.SupplierId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_SaleDetails", "OrderId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
        }
        /// <summary>
        /// 导出Excel数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetProductVisitAndDt(SaleStatisticsQuery query)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select * from [vw_Ecshop_SaleStoreDetails] where 1=1 ");
            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND orderDate >= '{0}'", query.StartDate.Value);
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND orderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            if (query.SupplierId.HasValue && query.SupplierId != 0)
            {
                stringBuilder.AppendFormat(" AND SupplierId = {0}", query.SupplierId);
            }
            sqlStringCommand.CommandText = stringBuilder.ToString();
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    result = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
            }
            return result;
        }

        public DbQueryResult GetSaleStoreOrderLineItemsStatistics(SaleStatisticsQuery query)
        {
            //StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.AppendFormat(" 1=1 ");
            //if (query.StartDate.HasValue)
            //{
            //    stringBuilder.AppendFormat(" AND orderDate >= '{0}'", query.StartDate.Value);
            //}
            //if (query.EndDate.HasValue)
            //{
            //    stringBuilder.AppendFormat(" AND orderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
            //}
            //if (!string.IsNullOrEmpty(query.ProductName))
            //{
            //    stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            //}
            //if (query.SupplierId.HasValue && query.SupplierId != 0)
            //{
            //    stringBuilder.AppendFormat(" AND SupplierId = {0}", query.SupplierId);
            //}
            //return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_SaleStoreDetails", "SkuId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");

            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Ecshop_SaleStoreDetails");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            if (!query.StartDate.HasValue)
            {
                query.StartDate = new DateTime(2000, 1, 1);
            }
            this.database.AddInParameter(storedProcCommand, "ParamStartTime", DbType.DateTime, query.StartDate.Value);
            if (!query.EndDate.HasValue)
            {
                query.EndDate = DateTime.Now;                
            }
            else
            {
                query.EndDate = query.EndDate.Value.AddDays(1);
            }
            this.database.AddInParameter(storedProcCommand, "ParamEndTime", DbType.DateTime, query.EndDate.Value);
            
                this.database.AddInParameter(storedProcCommand, "ParamProductName", DbType.String, DataHelper.CleanSearchString(query.ProductName));
            
            
                this.database.AddInParameter(storedProcCommand, "ParamSupplierId", DbType.Int32, query.SupplierId);
            
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            int totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            DbQueryResult dbresult = new DbQueryResult();
            dbresult.Data = result;
            dbresult.TotalRecords = totalProductSales;
            return dbresult;
        }
        public DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query)
        {
            DbQueryResult dbQueryResult = new DbQueryResult();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Ecshop_SaleDetails WHERE 1=1");
            if (query.StartDate.HasValue)
            {
                DbCommand expr_31 = sqlStringCommand;
                expr_31.CommandText += string.Format(" AND OrderDate >= '{0}'", query.StartDate);
            }
            if (query.EndDate.HasValue)
            {
                DbCommand expr_70 = sqlStringCommand;
                expr_70.CommandText += string.Format(" AND OrderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
            }
            DbCommand expr_B2 = sqlStringCommand;
            expr_B2.CommandText += string.Format("AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return dbQueryResult;
        }
        public DbQueryResult GetSaleTargets()
        {
            DbQueryResult dbQueryResult = new DbQueryResult();
            string query = string.Empty;
            query = string.Format("select (select Count(OrderId) from Ecshop_orders WHERE OrderStatus != {0} AND OrderStatus != {1}  AND OrderStatus != {2}) as OrderNumb,", 1, 4, 9) + string.Format("(select ISNULL(sum(OrderTotal) - ISNUll(sum(RefundAmount),0),0) from Ecshop_orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}) as OrderPrice, ", 1, 4, 9) + " (select COUNT(*) from aspnet_Members) as UserNumb,  (select count(*) from aspnet_Members where UserID in (select userid from Ecshop_orders)) as UserOrderedNumb,  ISNULL((select sum(VistiCounts) from Ecshop_products),0) as ProductVisitNumb ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return dbQueryResult;
        }
        public AdminStatisticsInfo GetStatistics()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new string[]
			{
				"SELECT  (SELECT COUNT(OrderId) FROM Ecshop_Orders WHERE OrderStatus = 2  OR (OrderStatus = 1 AND Gateway = 'ecdev.plugins.payment.podrequest')) AS orderNumbWaitConsignment,(select Count(LeaveId) from Ecshop_LeaveComments l where (select count(replyId) from Ecshop_LeaveCommentReplys where leaveId =l.leaveId)=0) as leaveComments,(select Count(ConsultationId) from Ecshop_ProductConsultations where ReplyUserId is null) as productConsultations,(select Count(*) from Ecshop_ManagerMessageBox where IsRead=0 and Accepter='admin' and Sernder in (select UserName from vw_aspnet_Members)) as messages, isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from Ecshop_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)   and OrderDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"'),0) as orderPriceToday, isnull((select sum(OrderProfit) from Ecshop_Orders where  (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)  and OrderDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"'),0) as orderProfitToday, (select count(*) from vw_aspnet_Members where CreateDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"' ) as userNewAddToday, isnull((select sum(balance) from vw_aspnet_Members),0) as memberBalance,(select count(*) from Ecshop_BalanceDrawRequest) as memberBlancedraw,(select count(*) from Ecshop_Orders where datediff(dd,getdate(),OrderDate)=0 and (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)) as todayFinishOrder,(select count(*) from Ecshop_Orders where datediff(dd,getdate()-1,OrderDate)=0 and (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)) as yesterdayFinishOrder, isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from Ecshop_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)   and datediff(dd,getdate()-1,OrderDate)=0),0) as orderPriceYesterDay,(select count(*) from vw_aspnet_Members where datediff(dd,getdate()-1,CreateDate)=0) as userNewAddYesterToday,(select count(*) from vw_aspnet_Members) as TotalMembers,(select count(*) from Ecshop_Products where SaleStatus!=0) as TotalProducts, isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from Ecshop_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)   and datediff(dd,OrderDate,getdate())<=30),0) as orderPriceMonth"
			}));
            AdminStatisticsInfo adminStatisticsInfo = new AdminStatisticsInfo();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    adminStatisticsInfo.OrderNumbWaitConsignment = (int)dataReader["orderNumbWaitConsignment"];
                    adminStatisticsInfo.LeaveComments = (int)dataReader["LeaveComments"];
                    adminStatisticsInfo.ProductConsultations = (int)dataReader["ProductConsultations"];
                    adminStatisticsInfo.Messages = (int)dataReader["Messages"];
                    adminStatisticsInfo.OrderProfitToday = (decimal)dataReader["orderProfitToday"];
                    adminStatisticsInfo.UserNewAddToday = (int)dataReader["userNewAddToday"];
                    adminStatisticsInfo.MembersBalance = (decimal)dataReader["memberBalance"];
                    adminStatisticsInfo.OrderPriceToday = (decimal)dataReader["orderPriceToday"];
                    adminStatisticsInfo.MemberBlancedrawRequest = (int)dataReader["memberBlancedraw"];
                    adminStatisticsInfo.TodayFinishOrder = (int)dataReader["todayFinishOrder"];
                    adminStatisticsInfo.YesterdayFinishOrder = (int)dataReader["yesterdayFinishOrder"];
                    adminStatisticsInfo.OrderPriceYesterDay = (decimal)dataReader["orderPriceYesterDay"];
                    adminStatisticsInfo.UserNewAddYesterToday = (int)dataReader["userNewAddYesterToday"];
                    adminStatisticsInfo.TotalMembers = (int)dataReader["TotalMembers"];
                    adminStatisticsInfo.TotalProducts = (int)dataReader["TotalProducts"];
                    adminStatisticsInfo.OrderPriceMonth = (decimal)dataReader["OrderPriceMonth"];
                }
            }
            return adminStatisticsInfo;
        }
        private static string BuildProductSaleQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ProductId, SUM(o.Quantity) AS ProductSaleCounts, SUM(o.ItemAdjustedPrice * o.Quantity) AS ProductSaleTotals,");
            stringBuilder.Append("  (SUM(o.ItemAdjustedPrice * o.Quantity) - SUM(o.CostPrice * o.ShipmentQuantity) )AS ProductProfitsTotals,isnull(s.SupplierName,'') SupplierName");
            stringBuilder.AppendFormat(" FROM Ecshop_OrderItems o left join Ecshop_Supplier s on s.SupplierId = o.SupplierId WHERE 0=0 ", new object[0]);
            //stringBuilder.AppendFormat(" AND o.OrderId IN (SELECT  OrderId FROM Ecshop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2})", 1, 4, 9);
            stringBuilder.AppendFormat(" AND o.OrderId IN (SELECT  OrderId FROM Ecshop_Orders WHERE OrderStatus not in ({0}) )", "1, 4, 9,98,99");
            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND o.OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE OrderDate >= '{0}')", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND o.OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE OrderDate <= '{0}')", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.SupplierId.HasValue && query.SupplierId.Value != 0)
            {
                stringBuilder.AppendFormat(" AND o.SupplierId = {0}", query.SupplierId.Value);
            }
            stringBuilder.Append(" GROUP BY ProductId,s.SupplierName HAVING ProductId IN");

            stringBuilder.Append(" (SELECT ProductId FROM Ecshop_Products  ");
            if (query.ProductName != "")
            {
                stringBuilder.Append(" where ProductName like '%" + query.ProductName + "%' ");
            }
            if (query.ProductCode != "")
            {
                if (query.ProductName != "")
                {
                    stringBuilder.Append("  and ProductCode like '%" + query.ProductCode + "%' ");
                }
                else
                {
                    stringBuilder.Append(" where ProductCode like '%" + query.ProductCode + "%' ");
                }
            }
            stringBuilder.Append(" )  ");

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return stringBuilder.ToString();
        }

        private static string BuildProductSaleAsBrandQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT o.ProductId,b.BrandId,b.BrandName,SUM(o.Quantity) AS ProductSaleCounts, SUM(o.ItemAdjustedPrice * o.Quantity) AS ProductSaleTotals,");
            stringBuilder.Append("  (SUM(o.ItemAdjustedPrice * o.Quantity) - SUM(o.CostPrice * o.ShipmentQuantity) )AS ProductProfitsTotals ");
            stringBuilder.AppendFormat(@" FROM Ecshop_OrderItems o join Ecshop_Products p 
                                          on o.ProductId = p.ProductId 
                                          left join Ecshop_BrandCategories b
                                          on p.BrandId = b.BrandId WHERE 0=0 and b.BrandId is not null ", new object[0]);
            //stringBuilder.AppendFormat(" AND OrderId IN (SELECT  OrderId FROM Ecshop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2})", 1, 4, 9);

            stringBuilder.AppendFormat(" AND OrderId IN (SELECT  OrderId FROM Ecshop_Orders WHERE OrderStatus not in ({0}) )","1,4,9,98,99");
            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE OrderDate >= '{0}')", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE OrderDate <= '{0}')", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }

            if (query.BrandId.HasValue)
            {
                stringBuilder.AppendFormat(" AND b.BrandId = {0}", query.BrandId.Value);
            }

            stringBuilder.Append(" GROUP BY b.BrandName,b.BrandId,o.ProductId");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }

            return stringBuilder.ToString();
        }

        private static string BuildProductSaleAsImportSource(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT o.ProductId,b.ImportSourceId,b.CnArea,SUM(o.Quantity) AS ProductSaleCounts, SUM(o.ItemAdjustedPrice * o.Quantity) AS ProductSaleTotals,");
            stringBuilder.Append("  (SUM(o.ItemAdjustedPrice * o.Quantity) - SUM(o.CostPrice * o.ShipmentQuantity) )AS ProductProfitsTotals ");
            stringBuilder.AppendFormat(@" FROM Ecshop_OrderItems o join Ecshop_Products p 
                                          on o.ProductId = p.ProductId 
                                          left join Ecshop_ImportSourceType b
                                          on p.ImportSourceId = b.ImportSourceId WHERE 0=0 and b.ImportSourceId is not null ", new object[0]);
            //stringBuilder.AppendFormat(" AND OrderId IN (SELECT  OrderId FROM Ecshop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2})", 1, 4, 9);

            stringBuilder.AppendFormat(" AND OrderId IN (SELECT  OrderId FROM Ecshop_Orders WHERE OrderStatus not in({0}))", "1,4,9,98,99");
            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE OrderDate >= '{0}')", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE OrderDate <= '{0}')", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }

            if (query.ImportSourceId.HasValue)
            {
                stringBuilder.AppendFormat(" AND b.ImportSourceId = {0}", query.ImportSourceId.Value);
            }

            stringBuilder.Append(" GROUP BY b.CnArea,b.ImportSourceId,o.ProductId");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }

            return stringBuilder.ToString();
        }

        private static string BuildRegionsUserQuery(Pagination page)
        {
            if (null == page)
            {
                throw new ArgumentNullException("page");
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" SELECT r.RegionId, r.RegionName, SUM(au.UserCount) AS Usercounts,");
            stringBuilder.Append(" (SELECT (SELECT SUM(COUNT) FROM aspnet_Members)) AS AllUserCounts ");
            stringBuilder.Append(" FROM vw_Allregion_Members au, Ecshop_Regions r ");
            stringBuilder.Append(" WHERE (r.AreaId IS NOT NULL) AND ((au.path LIKE r.path + LTRIM(RTRIM(STR(r.RegionId))) + ',%') OR au.RegionId = r.RegionId)");
            stringBuilder.Append(" group by r.RegionId, r.RegionName ");
            stringBuilder.Append(" UNION SELECT 0, '0', sum(au.Usercount) AS Usercounts,");
            stringBuilder.Append(" (SELECT (SELECT count(*) FROM aspnet_Members)) AS AllUserCounts ");
            stringBuilder.Append(" FROM vw_Allregion_Members au, Ecshop_Regions r  ");
            stringBuilder.Append(" WHERE au.regionid IS NULL OR au.regionid = 0 group by r.RegionId, r.RegionName");
            if (!string.IsNullOrEmpty(page.SortBy))
            {
                stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(page.SortBy), page.SortOrder.ToString());
            }
            return stringBuilder.ToString();
        }
        private static string BuildUserOrderQuery(OrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT OrderId FROM Ecshop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
            string result;
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
                result = stringBuilder.ToString();
            }
            else
            {
                if (!string.IsNullOrEmpty(query.UserName))
                {
                    stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
                }
                if (!string.IsNullOrEmpty(query.ShipTo))
                {
                    stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
                }
                if (query.StartDate.HasValue)
                {
                    stringBuilder.AppendFormat(" AND  OrderDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    stringBuilder.AppendFormat(" AND  OrderDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                if (query.SiteId.HasValue)
                {
                    if (query.SiteId >= 0)
                        stringBuilder.AppendFormat(" AND SiteId={0}", query.SiteId.Value);//jiaSiteId
                }
                if (query.SourceOrder != null)
                {
                    if (query.SourceOrder > 0)
                    {
                        stringBuilder.AppendFormat(" AND SourceOrder={0}", query.SourceOrder.Value);//jiaSourceOrder
                    }
                }
                stringBuilder.Append(BuildUserOrderQueryByProductNameAndSupplier(query));
                if (!string.IsNullOrEmpty(query.SortBy))
                {
                    stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
                }
                result = stringBuilder.ToString();
            }
            return result;
        }
        private static string BuildUserOrderQueryByProductNameAndSupplier(OrderQuery query)
        {
            if (string.IsNullOrEmpty(query.ProductName) && (!query.SupplierId.HasValue || query.SupplierId.Value == 0))
            {
                return string.Empty;
            }

            string result = " AND OrderId in (SELECT OrderId from dbo.Ecshop_OrderItems where {0})";
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                sb.AppendFormat("ItemDescription LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            if (query.SupplierId.HasValue && query.SupplierId.Value != 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" AND ");
                }
                sb.AppendFormat("SupplierId = {0}", query.SupplierId);
            }
            return string.Format(result, sb);
        }
        private static string BuildOrdersQuery(OrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT OrderId FROM Ecshop_Orders WHERE 1 = 1 ", new object[0]);
            if (query.OrderId != string.Empty && query.OrderId != null)
            {
                stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            else
            {
                if (query.PaymentType.HasValue)
                {
                    stringBuilder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
                }
                if (query.GroupBuyId.HasValue)
                {
                    stringBuilder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
                }
                if (!string.IsNullOrEmpty(query.ProductName))
                {
                    stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Ecshop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
                }
                if (!string.IsNullOrEmpty(query.ShipTo))
                {
                    stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
                }
                if (query.RegionId.HasValue)
                {
                    stringBuilder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
                }
                if (!string.IsNullOrEmpty(query.UserName))
                {
                    stringBuilder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
                }
                if (query.Status == OrderStatus.History)
                {
                    stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", new object[]
					{
						1,
						4,
						9,
						DateTime.Now.AddMonths(-3)
					});
                }
                else
                {
                    if (query.Status == OrderStatus.BuyerAlreadyPaid)
                    {
                        stringBuilder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'ecdev.plugins.payment.podrequest'))", (int)query.Status);
                    }
                    else
                    {
                        if (query.Status != OrderStatus.All)
                        {
                            stringBuilder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
                        }
                    }
                }
                if (query.StartDate.HasValue)
                {
                    stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                if (query.ShippingModeId.HasValue)
                {
                    stringBuilder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
                }
                if (query.IsPrinted.HasValue)
                {
                    stringBuilder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
                }
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return stringBuilder.ToString();
        }
        private static string BuildMemberStatisticsQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT UserId,(select username from aspnet_users as u where u.userid=aspnet_Members.userid) as username ");
            if (query.StartDate.HasValue || query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(",  ( select isnull(SUM(OrderTotal),0) from Ecshop_Orders where OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                if (query.StartDate.HasValue)
                {
                    stringBuilder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    stringBuilder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                stringBuilder.Append(" and userId = aspnet_Members.UserId) as SaleTotals");
                stringBuilder.AppendFormat(",(select Count(OrderId) from Ecshop_Orders where OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                if (query.StartDate.HasValue)
                {
                    stringBuilder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    stringBuilder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                stringBuilder.Append(" and userId = aspnet_Members.UserId) as OrderCount ");
            }
            else
            {
                stringBuilder.Append(",ISNULL(Expenditure,0) as SaleTotals,ISNULL(OrderNumber,0) as OrderCount ");
            }
            stringBuilder.Append(" from aspnet_Members where Expenditure > 0");
            if (query.StartDate.HasValue || query.EndDate.HasValue)
            {
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return stringBuilder.ToString();
        }
        private static string BuildProductVisitAndBuyStatisticsQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ProductId,(SaleCounts*100/(case when VistiCounts=0 then 1 else VistiCounts end)) as BuyPercentage");
            stringBuilder.Append(" FROM Ecshop_products where SaleCounts>0");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return stringBuilder.ToString();
        }
    }
}

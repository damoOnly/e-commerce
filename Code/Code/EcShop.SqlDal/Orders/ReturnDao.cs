using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Orders;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Orders
{
    public class ReturnDao
    {
        private Database database;
        public ReturnDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        //处理特殊的退货单
        public bool UpdateReturn(string orderId, string Operator, decimal refundMoney, string adminRemark, int refundType, bool accept)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" update Ecshop_OrderReturns set Operator=@Operator, AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime,RefundMoney=@RefundMoney where HandleStatus=3 and OrderId = @OrderId;");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundMoney);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool UpdateReturn(string orderId, string Operator, decimal refundMoney, decimal expressFee, decimal customsClearanceFee, string feeAffiliation, string adminRemark, int refundType, bool accept)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" update Ecshop_OrderReturns set Operator=@Operator, AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime,RefundMoney=@RefundMoney,ExpressFee=@ExpressFee,CustomsClearanceFee=@CustomsClearanceFee,FeeAffiliation=@FeeAffiliation where HandleStatus=3 and OrderId = @OrderId;");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundMoney);
            this.database.AddInParameter(sqlStringCommand, "ExpressFee", DbType.Decimal, expressFee);
            this.database.AddInParameter(sqlStringCommand, "CustomsClearanceFee", DbType.Decimal, customsClearanceFee);
            this.database.AddInParameter(sqlStringCommand, "FeeAffiliation", DbType.String, feeAffiliation);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool CheckReturn(string orderId, string Operator, decimal refundMoney, string adminRemark, int refundType, bool accept)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE Ecshop_Orders SET OrderStatus = @OrderStatus,RefundAmount=@refundMoney  WHERE OrderId = @OrderId;");
            stringBuilder.Append(" update Ecshop_OrderReturns set Operator=@Operator, AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime,RefundMoney=@RefundMoney where HandleStatus=3 and OrderId = @OrderId;");
            if (refundType == 1 && accept)
            {
                stringBuilder.Append(" insert into Ecshop_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income");
                stringBuilder.AppendFormat(",Balance,Remark) select UserId,Username,getdate() as TradeDate,{0} as TradeType,", 7);
                stringBuilder.Append("@RefundMoney as Income,isnull((select top 1 Balance from Ecshop_BalanceDetails b");
                stringBuilder.AppendFormat(" where b.UserId=a.UserId order by JournalNumber desc),0)+@RefundMoney as Balance,'订单{0}退货' as Remark ", orderId);
                stringBuilder.Append("from Ecshop_Orders a where OrderId = @OrderId;");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 10);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundMoney);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool CheckReturn(string orderId, string Operator, decimal refundMoney, decimal expressFee, decimal customsClearanceFee, string feeAffiliation, string adminRemark, int refundType, bool accept)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE Ecshop_Orders SET OrderStatus = @OrderStatus,RefundAmount=@refundMoney  WHERE OrderId = @OrderId;");
            stringBuilder.Append(" update Ecshop_OrderReturns set Operator=@Operator, AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime,RefundMoney=@RefundMoney,ExpressFee=@ExpressFee,CustomsClearanceFee=@CustomsClearanceFee,FeeAffiliation=@FeeAffiliation where HandleStatus=3 and OrderId = @OrderId;");
            if (refundType == 1 && accept)
            {
                stringBuilder.Append(" insert into Ecshop_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income");
                stringBuilder.AppendFormat(",Balance,Remark) select UserId,Username,getdate() as TradeDate,{0} as TradeType,", 7);
                stringBuilder.Append("@RefundMoney as Income,isnull((select top 1 Balance from Ecshop_BalanceDetails b");
                stringBuilder.AppendFormat(" where b.UserId=a.UserId order by JournalNumber desc),0)+@RefundMoney as Balance,'订单{0}退货' as Remark ", orderId);
                stringBuilder.Append("from Ecshop_Orders a where OrderId = @OrderId;");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 10);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundMoney);
            this.database.AddInParameter(sqlStringCommand, "ExpressFee", DbType.Decimal, expressFee);
            this.database.AddInParameter(sqlStringCommand, "CustomsClearanceFee", DbType.Decimal, customsClearanceFee);
            this.database.AddInParameter(sqlStringCommand, "FeeAffiliation", DbType.String, feeAffiliation);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public void GetRefundTypeFromReturn(string orderId, out int refundType, out string remark)
        {
            refundType = 0;
            remark = "";
            string query = "select RefundType,Comments from Ecshop_OrderReturns where HandleStatus=0 and OrderId='" + orderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
            if (dataReader.Read())
            {
                refundType = (int)dataReader["RefundType"];
                remark = (string)dataReader["Comments"];
            }
        }
        public DbQueryResult GetReturnsApplysMoney(ReturnsApplyQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
            }
            if (query.HandleStatus.HasValue)
            {
                stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
            }
            if (query.UserId.HasValue)
            {
                stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
            }
            if (!string.IsNullOrEmpty(query.StratTime))
            {
                stringBuilder.AppendFormat(" and ApplyForTime>='{0}'", query.StratTime);
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                stringBuilder.AppendFormat(" and ApplyForTime<='{0} 23:59:59'", query.EndTime.Trim());
            }
            if (!string.IsNullOrEmpty(query.Operator))
            {
                stringBuilder.AppendFormat(" AND Operator LIKE '%{0}%'", DataHelper.CleanSearchString(query.Operator));
            }
            if (query.SupplierId.HasValue && query.SupplierId.Value > 0)
            {
                stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Ecshop_OrderItems WHERE SupplierId ={0})", query.SupplierId.Value);
            }

            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_OrderReturns", "ReturnsId", stringBuilder.ToString(), "*");
        }
        public DbQueryResult GetReturnsApplys(ReturnsApplyQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
            }
            if (query.HandleStatus.HasValue)
            {
                stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
            }
            if (query.UserId.HasValue)
            {
                stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
            }
            if (!string.IsNullOrEmpty(query.StratTime))
            {
                stringBuilder.AppendFormat(" and HandleTime>='{0}'", query.StratTime);
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                stringBuilder.AppendFormat(" and HandleTime<='{0} 23:59:59'", query.EndTime.Trim());
            }

            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_OrderReturns", "ReturnsId", stringBuilder.ToString(), "*");
        }


        public DataSet NewGetReturnsApplys(ReturnsApplyQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
            }
            if (query.HandleStatus.HasValue)
            {
                stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
            }
            if (query.UserId.HasValue)
            {
                stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
            }
            if (!string.IsNullOrEmpty(query.StratTime))
            {
                stringBuilder.AppendFormat(" and HandleTime>='{0}'", query.StratTime);
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                stringBuilder.AppendFormat(" and HandleTime<='{0} 23:59:59'", query.EndTime.Trim());
            }

            string sqlText = "SELECT * FROM vw_Ecshop_OrderReturns where ";
            sqlText += stringBuilder.ToString();

            sqlText += " order by ReturnsId desc ;";
            sqlText = sqlText + " SELECT AId, SkuId, ProductId, Quantity, ItemAdjustedPrice, ItemDescription,ThumbnailsUrl,Weight,SKUContent FROM Ecshop_OrderAppFormItems as A  WHERE ApplyType=1 and AId IN (SELECT ReturnsId FROM vw_Ecshop_OrderReturns WHERE  " + stringBuilder.ToString() + ")";

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sqlText);
            DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = dataSet.Tables[0].Columns["ReturnsId"];
            DataColumn childColumn = dataSet.Tables[1].Columns["AId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn, false);
            dataSet.Relations.Add(relation);
            return dataSet;  
        }
        /// <summary>
        /// 导出退款单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet ExportReturnsApplys(ReturnsApplyQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select * from  vw_Ecshop_OrderReturns where  1=1 ");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
            }
            if (query.HandleStatus.HasValue)
            {
                stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
            }
            if (query.UserId.HasValue)
            {
                stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
            }
            if (!string.IsNullOrEmpty(query.StratTime))
            {
                stringBuilder.AppendFormat(" and HandleTime>='{0}'", query.StratTime);
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                stringBuilder.AppendFormat(" and HandleTime<='{0} 23:59:59'", query.EndTime.Trim());
            }
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public DataTable GetReturnsApplysTable(int returnsId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Ecshop_OrderReturns where ReturnsId=@ReturnsId");
            this.database.AddInParameter(sqlStringCommand, "ReturnsId", DbType.String, returnsId);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public bool DelReturnsApply(int returnsId)
        {
            string query = string.Format("DELETE FROM Ecshop_OrderReturns WHERE ReturnsId={0} and HandleStatus >0 ", returnsId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool ApplyForReturn(string orderId, string remark, int refundType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE Ecshop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            stringBuilder.Append(" insert into Ecshop_OrderReturns(OrderId,ApplyForTime,Comments,HandleStatus,RefundType,RefundMoney) values(@OrderId,@ApplyForTime,@Comments,0,@RefundType,0);");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 7);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Comments", DbType.String, remark);
            this.database.AddInParameter(sqlStringCommand, "RefundType", DbType.Int32, refundType);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool ApplyForReturn(string orderId, string remark, int refundType, string LogisticsCompany, string LogisticsId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE Ecshop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            stringBuilder.Append(" insert into Ecshop_OrderReturns(OrderId,ApplyForTime,Comments,HandleStatus,RefundType,RefundMoney,LogisticsCompany,LogisticsId) values(@OrderId,@ApplyForTime,@Comments,0,@RefundType,0,@LogisticsCompany,@LogisticsId);");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 7);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Comments", DbType.String, remark);
            this.database.AddInParameter(sqlStringCommand, "RefundType", DbType.Int32, refundType);
            this.database.AddInParameter(sqlStringCommand, "LogisticsCompany", DbType.String, LogisticsCompany);
            this.database.AddInParameter(sqlStringCommand, "LogisticsId", DbType.String, LogisticsId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool ApplyForPartReturn(string orderId, string remark, int refundType, string skuIds)//新添加 部分退货
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE Ecshop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            stringBuilder.Append(" insert into Ecshop_OrderReturns(OrderId,ApplyForTime,Comments,HandleStatus,RefundType,RefundMoney) values(@OrderId,@ApplyForTime,@Comments,0,@RefundType,0);");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 7);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Comments", DbType.String, remark);
            this.database.AddInParameter(sqlStringCommand, "RefundType", DbType.Int32, refundType);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool CanReturn(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select OrderId,HandleStatus from Ecshop_OrderReturns where OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            bool result = false;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    int num = Convert.ToInt32(dataReader["HandleStatus"]);
                    if (num == 2)
                    {
                        result = true;
                    }
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }
        public decimal GetRefundMoney(OrderInfo order, out decimal refundMoney)
        {
            string query = string.Format("SELECT RefundMoney FROM dbo.Ecshop_OrderReturns WHERE HandleStatus=1 AND OrderId='{0}'", order.OrderId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            decimal num = Convert.ToDecimal(this.database.ExecuteScalar(sqlStringCommand));
            return refundMoney = num;
        }
        public int GetRefundCount(int userId)
        {
            string text2 = "SELECT COUNT(1)  FROM vw_Ecshop_OrderReturns o WHERE UserId = @UserId";
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text2);
            sqlStringCommand.CommandType = System.Data.CommandType.Text;
            this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        /// <summary>
        /// 领取操作，把处理人改为当前用户，状态改为处理中，领取时间为当前时间
        /// </summary>
        /// <param name="refundInfo"></param>
        /// <returns></returns>
        public bool UpdateOrderReturnsInfo(ReturnsInfo returnsInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_OrderReturns set HandleStatus = @HandleStatus,Operator=@Operator,ReceiveTime=@ReceiveTime where ReturnsId=@ReturnsId and HandleStatus = 0");
            this.database.AddInParameter(sqlStringCommand, "ReturnsId", DbType.Int32, returnsInfo.ReturnsId);
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, returnsInfo.HandleStatus);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, returnsInfo.Operator);
            this.database.AddInParameter(sqlStringCommand, "ReceiveTime", DbType.DateTime, returnsInfo.ReceiveTime);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }


        /// <summary>
        /// 导出退货申请单
        /// </summary>
        /// <param name="startPayTime">申请开始时间</param>
        /// <param name="endPayTime">申请结束时间</param>
        /// <param name="returnsIds">退货单号</param>
        /// <returns></returns>
        public DataSet GetExcelOrderReturns(string startApplyForTime, string endApplyForTime, string returnsIds, int handleStatus)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("select ReturnsId,OrderId,Operator,RefundMoney,OrderTotal,ApplyForTime,Comments,case HandleStatus when 0 then '待处理' when 3 then '处理中' when 1 then '已处理' when 2 then '已拒绝' end as HandleStatusStr,HandleStatus,HandleTime,AdminRemark,Remark from vw_Ecshop_OrderReturns where 1=1 ");
            if (!string.IsNullOrEmpty(startApplyForTime) && !string.IsNullOrEmpty(endApplyForTime))
            {
                stringBuilder.AppendFormat(" AND ApplyForTime>='{0}'  AND ApplyForTime<='{1}'", startApplyForTime, endApplyForTime);
            }
            if (!string.IsNullOrEmpty(returnsIds))
            {
                stringBuilder.AppendFormat(" AND ReturnsId in ({0})", returnsIds);
            }
            if (handleStatus != -1)
            {
                stringBuilder.AppendFormat(" AND HandleStatus = {0}", handleStatus);
            }


            stringBuilder.Append(" ORDER BY ApplyForTime,ReturnsId");
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        public DataSet GetExcelOrderReturnsDetails(string startApplyForTime, string endApplyForTime, string returnsIds, int handleStatus)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("select ReturnsId,OrderId,Operator,RefundMoney,OrderTotal,ApplyForTime,Comments,case HandleStatus when 0 then '待处理' when 3 then '处理中' when 1 then '已处理' when 2 then '已拒绝' end as HandleStatusStr,HandleStatus,HandleTime,AdminRemark,Remark,ProductId,ItemDescription,ItemAdjustedPrice,Quantity,ItemTaxRate,SupplierName,SKUContent from vw_Ecshop_OrderReturnsDetails where 1=1 ");//e.SupplierName,c.SKUContent,
            if (!string.IsNullOrEmpty(startApplyForTime) && !string.IsNullOrEmpty(endApplyForTime))
            {
                stringBuilder.AppendFormat(" AND ApplyForTime>='{0}'  AND ApplyForTime<='{1}'", startApplyForTime, endApplyForTime);
            }
            if (!string.IsNullOrEmpty(returnsIds))
            {
                stringBuilder.AppendFormat(" AND ReturnsId in ({0})", returnsIds);
            }
            if (handleStatus != -1)
            {
                stringBuilder.AppendFormat(" AND HandleStatus = {0}", handleStatus);
            }


            stringBuilder.Append(" ORDER BY ApplyForTime,ReturnsId");
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);

        }
        public bool AddReturnsItem(ReturnsInfo returnsInfo, DbTransaction dbTran)
        {
            bool result;
            if (returnsInfo.ReturnsId == 0)
            {
                result = false;
                return result;
            }
            if (returnsInfo.ReturnsLineItem == null || returnsInfo.ReturnsLineItem.Count == 0)
            {
                result = false;
            }
            else
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
                StringBuilder stringBuilder = new StringBuilder();
                int num = 0;
                foreach (OrderAppFormItems lineItem in returnsInfo.ReturnsLineItem)
                {
                    stringBuilder.Append(" INSERT INTO Ecshop_OrderAppFormItems ([AId],[SkuId],[ProductId],[Quantity],[ItemAdjustedPrice],[ItemDescription],[ThumbnailsUrl],[Weight],[SKUContent],[PromotionId],[PromotionName],[TaxRate],[TemplateId],[storeId],[SupplierId],[ApplyType])  VALUES(").Append("@AId").Append(num).Append(",@SkuId").Append(num).Append(", @ProductId").Append(num).Append(", @Quantity").Append(num).Append(", @ItemAdjustedPrice").Append(num).Append(", @ItemDescription").Append(num).Append(",@ThumbnailsUrl").Append(num).Append(",@Weight").Append(num).Append(", @SKUContent").Append(num).Append(",@PromotionId").Append(num).Append(", @PromotionName").Append(num).Append(",@TaxRate").Append(num).Append(", @TemplateId").Append(num).Append(", @storeId").Append(num).Append(", @SupplierId").Append(num).Append(", @ApplyType").Append(num).Append(")");
                    this.database.AddInParameter(sqlStringCommand, "AId" + num, DbType.Int32, returnsInfo.ReturnsId);
                    this.database.AddInParameter(sqlStringCommand, "SkuId" + num, DbType.String, lineItem.SkuId);
                    this.database.AddInParameter(sqlStringCommand, "ProductId" + num, DbType.Int32, lineItem.ProductId);
                    this.database.AddInParameter(sqlStringCommand, "Quantity" + num, DbType.Int32, lineItem.Quantity);
                    this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice" + num, DbType.String, lineItem.ItemAdjustedPrice);
                    this.database.AddInParameter(sqlStringCommand, "ItemDescription" + num, DbType.String, lineItem.ItemDescription);
                    this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + num, DbType.String, lineItem.ThumbnailsUrl);
                    this.database.AddInParameter(sqlStringCommand, "Weight" + num, DbType.Decimal, lineItem.ItemWeight);
                    this.database.AddInParameter(sqlStringCommand, "SKUContent" + num, DbType.String, lineItem.SKUContent);
                    this.database.AddInParameter(sqlStringCommand, "PromotionId" + num, DbType.Int32, lineItem.PromotionId);
                    this.database.AddInParameter(sqlStringCommand, "PromotionName" + num, DbType.String, lineItem.PromotionName);
                    this.database.AddInParameter(sqlStringCommand, "TaxRate" + num, DbType.Decimal, lineItem.TaxRate);
                    this.database.AddInParameter(sqlStringCommand, "TemplateId" + num, DbType.Int32, lineItem.TemplateId);
                    this.database.AddInParameter(sqlStringCommand, "storeId" + num, DbType.Int32, lineItem.storeId);
                    this.database.AddInParameter(sqlStringCommand, "SupplierId" + num, DbType.Int32, lineItem.SupplierId);
                    this.database.AddInParameter(sqlStringCommand, "ApplyType" + num, DbType.Int32, lineItem.ApplyType);
                    num++;
                }
                sqlStringCommand.CommandText = stringBuilder.ToString();
                if (dbTran != null)
                {
                    result = this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
                }
                else
                {
                    result = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
                }
            }
            return result;

        }
        public bool UpdateOrderStatusBeApplyForReturns(string orderId, DbTransaction dbTran)
        {
            bool result = false;
            if (string.IsNullOrEmpty(orderId))
            {
                return result;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Orders SET OrderStatus = 7 WHERE OrderId = @OrderId;");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
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
        public int AddReturnInfo(ReturnsInfo ReturnsInfo, DbTransaction dbTran)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" insert into Ecshop_OrderReturns(OrderId,ApplyForTime,Comments,HandleStatus,RefundType,RefundMoney,LogisticsCompany,LogisticsId) values(@OrderId,@ApplyForTime,@Comments,0,@RefundType,0,@LogisticsCompany,@LogisticsId);SELECT @@identity ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, ReturnsInfo.OrderId);
            this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Comments", DbType.String, ReturnsInfo.Comments);
            this.database.AddInParameter(sqlStringCommand, "RefundType", DbType.Int32, ReturnsInfo.RefundType);
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 0);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, ReturnsInfo.GetAmount());
            this.database.AddInParameter(sqlStringCommand, "LogisticsCompany", DbType.String,ReturnsInfo.LogisticsCompany);
            this.database.AddInParameter(sqlStringCommand, "LogisticsId", DbType.String, ReturnsInfo.LogisticsId);
            int returnsId;
            object obj;
            if (dbTran != null)
            {
                obj = this.database.ExecuteScalar(sqlStringCommand, dbTran);
            }
            else
            {
                obj = this.database.ExecuteScalar(sqlStringCommand);
            }
            if (obj != DBNull.Value)
            {
                returnsId = Convert.ToInt32(obj);
            }
            else
            {
                returnsId = 0;
            }
            return returnsId;
        }
        public bool CreateReturn(ReturnsInfo returnInfo)//创建退回单
        {
            bool result;
            if (returnInfo == null)
            {
                result = false;
            }
            else
            {
                Database database = DatabaseFactory.CreateDatabase();
                using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
                {
                    dbConnection.Open();
                    System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
                    try
                    {
                        if (!UpdateOrderStatusBeApplyForReturns(returnInfo.OrderId, dbTransaction))
                        {
                            dbTransaction.Rollback();
                            result = false;
                            return result;
                        }
                        returnInfo.ReturnsId = AddReturnInfo(returnInfo, dbTransaction);
                        if (returnInfo.ReturnsId <= 0)
                        {
                            dbTransaction.Rollback();
                            result = false;
                            return result;
                        }
                        if (returnInfo.ReturnsLineItem.Count > 0)
                        {
                            if (!AddReturnsItem(returnInfo, dbTransaction))
                            {
                                dbTransaction.Rollback();
                                result = false;
                                return result;
                            }
                        }
                        dbTransaction.Commit();
                        result = true;
                    }
                    catch
                    {
                        dbTransaction.Rollback();
                        result = false;
                    }
                    finally
                    {
                        dbConnection.Close();
                    }
                }
            }
            return result;
        }
        public ReturnsInfo GetReturnsInfo(int returnsId)
        {
            ReturnsInfo returnsInfo = null;
            //Username,EmailAddress,RealName,ZipCode,CellPhone
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT A.ReturnsId,A.OrderId,A.ApplyForTime,A.RefundType,A.RefundMoney,A.Comments,A.HandleStatus,A.HandleTime,A.AdminRemark,A.Operator,A.ReceiveTime,A1.Username,A1.EmailAddress,A1.RealName,A1.ZipCode,A1.CellPhone,A.LogisticsCompany,A.LogisticsId,A.ExpressFee,A.CustomsClearanceFee,A.FeeAffiliation FROM Ecshop_OrderReturns as A left join Ecshop_Orders as A1 on A.orderId=A1.orderId  where returnsId=@returnsId;SELECT o.Id,o.AId,o.SkuId,o.ProductId,o.ApplyType,o.Quantity,o.ItemAdjustedPrice,o.ItemDescription,o.ThumbnailsUrl,o.Weight,o.SKUContent,o.PromotionId,o.PromotionName,o.TaxRate,o.TemplateId,o.storeId,o.SupplierId,o.DeductFee,s.SupplierName FROM Ecshop_OrderAppFormItems as o left JOIN Ecshop_Supplier as s ON o.SupplierId=s.SupplierId where o.AId=@returnsId and o.ApplyType=1");
            this.database.AddInParameter(sqlStringCommand, "returnsId", DbType.Int32, returnsId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                returnsInfo = ReaderConvert.ReaderToModel<ReturnsInfo>(dataReader);
                if (returnsInfo != null)
                {
                    returnsInfo.ReturnsLineItem = new List<OrderAppFormItems>();
                }
                dataReader.NextResult();
                OrderAppFormItems appFormItem = null;
                while (1 == 1)
                {
                    appFormItem = ReaderConvert.ReaderToModel<OrderAppFormItems>(dataReader);
                    if (appFormItem != null && returnsInfo.ReturnsLineItem != null)
                    {
                        if (appFormItem.SupplierName == null)
                        {
                            appFormItem.SupplierName = "";
                        }
                        returnsInfo.ReturnsLineItem.Add(appFormItem);
                    }
                    if (appFormItem == null)
                    {
                        break;
                    }
                }
            }
            return returnsInfo;
        }
        public decimal  GetReturnsAmount(int aid)
        {
            decimal returnsAmount = 0m;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select SUM(Quantity*ItemAdjustedPrice) from Ecshop_OrderAppFormItems where AId=@aid AND ApplyType=1 ");
            this.database.AddInParameter(sqlStringCommand,"aid", DbType.Int32, aid);
           object obj= this.database.ExecuteScalar(sqlStringCommand);
           if (obj != DBNull.Value)
           {
               returnsAmount = Convert.ToDecimal(obj);
           }
           return returnsAmount;
        }
    }
}

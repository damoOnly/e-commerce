using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal
{
	public class RefundDao
	{
		private Database database;
		public RefundDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public void AddRefund(RefundInfo refundInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("insert into Ecshop_OrderRefund(OrderId,ApplyForTime,RefundRemark,HandleStatus) values('{0}','{1}','{2}',{3})", new object[]
			{
				refundInfo.OrderId,
				refundInfo.ApplyForTime,
				refundInfo.RefundRemark,
				(int)refundInfo.HandleStatus
			});
			this.database.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString());
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
		public void GetRefundType(string orderId, out int refundType, out string remark)
		{
			refundType = 0;
			remark = "";
			string query = "select RefundType,RefundRemark from Ecshop_OrderRefund where OrderId='" + orderId + "'";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					refundType = ((dataReader["RefundType"] != DBNull.Value) ? ((int)dataReader["RefundType"]) : 0);
					remark = (string)dataReader["RefundRemark"];
				}
			}
		}
		public RefundInfo GetByOrderId(string orderId)
		{
			string query = "select * from Ecshop_OrderRefund where OrderId='" + orderId + "'";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			RefundInfo result = null;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<RefundInfo>(dataReader);
			}
			return result;
		}
		public void UpdateByOrderId(RefundInfo refundInfo)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_OrderRefund set AdminRemark=@AdminRemark,ApplyForTime=@ApplyForTime,HandleStatus=@HandleStatus,HandleTime=@HandleTime,Operator=@Operator,RefundRemark=@RefundRemark where OrderId =@OrderId");
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, refundInfo.AdminRemark);
			this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.String, refundInfo.ApplyForTime);
			this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, refundInfo.HandleStatus);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, refundInfo.HandleTime);
			this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, refundInfo.Operator);
			this.database.AddInParameter(sqlStringCommand, "RefundRemark", DbType.String, refundInfo.RefundRemark);
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, refundInfo.OrderId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
       //更新特殊的退款单
        public bool UpdateOrderRefund(OrderInfo order, string Operator, string adminRemark, int refundType, decimal refundMoney, bool accept)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" update Ecshop_OrderRefund set Operator=@Operator,AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime,RefundMoney=@RefundMoney where HandleStatus=3 and OrderId = @OrderId;");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundMoney);

            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool CheckRefund(OrderInfo order, string Operator, string adminRemark, int refundType, decimal refundMoney, bool accept, bool isReurnCoupon)
		{
			StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE Ecshop_Orders SET OrderStatus = @OrderStatus, RefundAmount=@refundMoney  WHERE OrderId = @OrderId;");
            stringBuilder.Append(" update Ecshop_OrderRefund set Operator=@Operator,AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime,RefundMoney=@RefundMoney where (HandleStatus=3  OR HandleStatus=0)  and OrderId = @OrderId;");

            if (isReurnCoupon)
            {
                //还原优惠券
                stringBuilder.Append("Update dbo.Ecshop_CouponItems Set OrderId = null,UsedTime = null,CouponStatus = 0 where OrderId=@OrderId;");
                //还原现金券
                stringBuilder.Append("Update dbo.Ecshop_VoucherItems Set OrderId = null,UsedTime = null,VoucherStatus = 0 where OrderId =@OrderId;");
            }

            if (refundType == 1 && accept)
			{
				if (order != null)
				{
					Member member = Users.GetUser(order.UserId, false) as Member;
					decimal num = order.GetTotal();
					decimal num2 = member.Balance + num;
					if (order.GroupBuyStatus != GroupBuyStatus.Failed)
					{
						num -= order.NeedPrice;
						num2 -= order.NeedPrice;
					}
					stringBuilder.Append("insert into Ecshop_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income,Balance,Remark)");
					stringBuilder.AppendFormat("values({0},'{1}',{2},{3},{4},{5},'{6}')", new object[]
					{
						member.UserId,
						member.Username,
						"getdate()",
						5,
						num,
						num2,
						"订单" + order.OrderId + "退款"
					});
				}
			}
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 9);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 2);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
			}
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundMoney);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool UpdateRefundOrderStock(string orderId)
		{
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi  Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId)");

            StringBuilder sb = new StringBuilder();
            sb.Append("Update Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId)  WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId);");
            sb.Append("Update Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM ecshop_orderpresents oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId)  WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM ecshop_orderpresents Where OrderId =@OrderId);");
            //组合商品库存
            sb.Append(@"Update Ecshop_SKUs Set Stock =  Stock + (select SUM(oi.ShipmentQuantity*pc.Quantity) from Ecshop_ProductsCombination pc inner join Ecshop_OrderItems oi

on oi.SkuId =pc.CombinationSkuId where pc.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId)  

WHERE Ecshop_SKUs.SkuId  IN (Select pc2.SkuId from Ecshop_ProductsCombination pc2 inner join   Ecshop_OrderItems oi2 on  oi2.SkuId =pc2.CombinationSkuId Where oi2.OrderId =@OrderId);");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

        public void AddOrderFactStock(string orderId)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Ecshop_SKUs Set FactStock = FactStock + (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi  Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId)");
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Ecshop_SKUs Set FactStock = FactStock + (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId)  WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId);");
            sb.Append("Update Ecshop_SKUs Set FactStock = FactStock + (SELECT SUM(oi.ShipmentQuantity) FROM ecshop_orderpresents oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId)  WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM ecshop_orderpresents Where OrderId =@OrderId);");
            //组合商品库存
            sb.Append(@"Update Ecshop_SKUs Set FactStock =  FactStock + (select SUM(oi.ShipmentQuantity*pc.Quantity) from Ecshop_ProductsCombination pc inner join Ecshop_OrderItems oi

on oi.SkuId =pc.CombinationSkuId where pc.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId)  

WHERE Ecshop_SKUs.SkuId  IN (Select pc2.SkuId from Ecshop_ProductsCombination pc2 inner join   Ecshop_OrderItems oi2 on  oi2.SkuId =pc2.CombinationSkuId Where oi2.OrderId =@OrderId);");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

		public DbQueryResult GetRefundApplys(RefundApplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
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
                stringBuilder.AppendFormat(" and HandleTime>='{0}'", DataHelper.CleanSearchString(query.StratTime));
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                stringBuilder.AppendFormat(" and HandleTime<='{0} 23:59:59'", DataHelper.CleanSearchString(query.EndTime));
            }

			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_OrderRefund", "RefundId", stringBuilder.ToString(), "*");
		}

        public DataSet NewGetRefundApplys(RefundApplyQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
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
                stringBuilder.AppendFormat(" and HandleTime>='{0}'", DataHelper.CleanSearchString(query.StratTime));
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                stringBuilder.AppendFormat(" and HandleTime<='{0} 23:59:59'", DataHelper.CleanSearchString(query.EndTime));
            }

            string sqlText = "SELECT * FROM vw_Ecshop_OrderRefund where ";
            sqlText += stringBuilder.ToString();

            sqlText += " order by RefundId desc ;";
            sqlText = sqlText + " SELECT OrderId, ThumbnailsUrl, ItemDescription, SKUContent, SKU, ProductId FROM Ecshop_OrderItems  WHERE OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE " + stringBuilder.ToString()+")";

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sqlText);
            DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = dataSet.Tables[0].Columns["OrderId"];
            DataColumn childColumn = dataSet.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn,false);
            dataSet.Relations.Add(relation);
            return dataSet;  
        }
        public DbQueryResult GetRefundApplysMoney(RefundApplyQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
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
                stringBuilder.AppendFormat(" and ApplyForTime>='{0}'", DataHelper.CleanSearchString(query.StratTime));
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                stringBuilder.AppendFormat(" and ApplyForTime<='{0} 23:59:59'", DataHelper.CleanSearchString(query.EndTime));
            }
            if (!string.IsNullOrEmpty(query.Operator))
            {
                stringBuilder.AppendFormat(" AND Operator LIKE '%{0}%'", DataHelper.CleanSearchString(query.Operator));
            }
            if (query.SupplierId.HasValue && query.SupplierId.Value > 0)
            {
                stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Ecshop_OrderItems WHERE SupplierId ={0})", query.SupplierId.Value);
            }

            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_OrderRefund", "RefundId", stringBuilder.ToString(), "*");
        }

        /// <summary>
        /// 导出退款单信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet ExportRefundApplys(RefundApplyQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select * from vw_Ecshop_OrderRefund where  1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                stringBuilder.AppendFormat(" AND OrderId = '{0}'", query.OrderId);
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
                stringBuilder.AppendFormat(" and HandleTime<='{0} 23:59:59'", query.EndTime);
            }

            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

		public DataTable GetRefundApplysTable(int refundId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Ecshop_OrderRefund  where RefundId=@RefundId");
			this.database.AddInParameter(sqlStringCommand, "RefundId", DbType.Int32, refundId);
			DataTable result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public bool DelRefundApply(int refundId)
		{
			string query = string.Format("DELETE FROM Ecshop_OrderRefund WHERE RefundId={0} and HandleStatus >0 ", refundId);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool ApplyForRefund(string orderId, string remark, int refundType)
		{
			StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE Ecshop_Orders SET OrderStatus = @OrderStatus,IsSendWMS=1 WHERE OrderId = @OrderId;");
			stringBuilder.Append(" insert into Ecshop_OrderRefund(OrderId,ApplyForTime,RefundRemark,HandleStatus,RefundType) values(@OrderId,@ApplyForTime,@RefundRemark,0,@RefundType);");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 6);
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "RefundRemark", DbType.String, remark);
			this.database.AddInParameter(sqlStringCommand, "RefundType", DbType.Int32, refundType);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool CanRefund(string orderId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select OrderId,HandleStatus from Ecshop_OrderRefund where OrderId = @OrderId");
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
        public int GetRefundCount(int userId)
        {
            string text2 = "SELECT COUNT(1)  FROM vw_Ecshop_OrderRefund o WHERE UserId = @UserId";
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
        public bool UpdateOrderRefundInfo(RefundInfo refundInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_OrderRefund set HandleStatus = @HandleStatus,Operator=@Operator,ReceiveTime=@ReceiveTime where RefundId=@RefundId and HandleStatus = 0");
            this.database.AddInParameter(sqlStringCommand, "RefundId", DbType.Int32, refundInfo.RefundId);
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, refundInfo.Status);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, refundInfo.Operator);
            this.database.AddInParameter(sqlStringCommand, "ReceiveTime", DbType.DateTime, refundInfo.ReceiveTime);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 导出退款申请单
        /// </summary>
        /// <param name="startPayTime">申请开始时间</param>
        /// <param name="endPayTime">申请结束时间</param>
        /// <param name="refundIds">退款单号</param>
        /// <returns></returns>
        public DataSet GetExcelOrderRefund(string startApplyForTime, string endApplyForTime, string refundIds, int handleStatus)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("select RefundId,OrderId,Operator,RefundMoney,OrderTotal,ApplyForTime,RefundRemark,case HandleStatus when 0 then '待处理' when 3 then '处理中' when 1 then '已处理' when 2 then '已拒绝' end as HandleStatusStr,HandleStatus,HandleTime,AdminRemark,Remark from vw_Ecshop_OrderRefund where 1=1 ");
            if (!string.IsNullOrEmpty(startApplyForTime) && !string.IsNullOrEmpty(endApplyForTime))
            {
                stringBuilder.AppendFormat(" AND ApplyForTime>='{0}'  AND ApplyForTime<='{1}'", startApplyForTime, endApplyForTime);
            }
            if (!string.IsNullOrEmpty(refundIds))
            {
                stringBuilder.AppendFormat(" AND RefundId in ({0})", refundIds);
            }
            if (handleStatus != -1)
            {
                stringBuilder.AppendFormat(" AND HandleStatus = {0}", handleStatus);
            }

            stringBuilder.Append(" ORDER BY ApplyForTime,RefundId");
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public DataSet GetExcelOrderRefundDetails(string startApplyForTime, string endApplyForTime, string refundIds, int handleStatus)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("select RefundId,OrderId,Operator,RefundMoney,OrderTotal,ApplyForTime,RefundRemark,case HandleStatus when 0 then '待处理' when 3 then '处理中' when 1 then '已处理' when 2 then '已拒绝' end as HandleStatusStr,HandleStatus,HandleTime,AdminRemark,Remark,ProductId,ItemDescription,ItemAdjustedPrice,Quantity,ItemTaxRate,SupplierName,SKUContent  from vw_Ecshop_OrderRefundDetails where 1=1 ");
            if (!string.IsNullOrEmpty(startApplyForTime) && !string.IsNullOrEmpty(endApplyForTime))
            {
                stringBuilder.AppendFormat(" AND ApplyForTime>='{0}'  AND ApplyForTime<='{1}'", startApplyForTime, endApplyForTime);
            }
            if (!string.IsNullOrEmpty(refundIds))
            {
                stringBuilder.AppendFormat(" AND RefundId in ({0})", refundIds);
            }
            if (handleStatus != -1)
            {
                stringBuilder.AppendFormat(" AND HandleStatus = {0}", handleStatus);
            }

            stringBuilder.Append(" ORDER BY ApplyForTime,RefundId");
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

       /// <summary>
       /// 更新退款申请商品入库状态
       /// </summary>
       /// <param name="orderId"></param>
       /// <param name="remark"></param>
       /// <returns></returns>
        public bool  UpdateRefundOrderReturntatus(string orderId,string remark)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_OrderRefund set ProductReturnType=1,ProductReturnRemark=@ProductReturnRemark where  OrderId=@OrderId ");

            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "ProductReturnRemark", DbType.String, remark);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }


        /// <summary>
        /// 根据订单id更新商品销量
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="dbTran"></param>
        /// <returns></returns>
        public bool UpdateRefundOrderProductSaleCount(string orderId, DbTransaction dbTran)
        {



            StringBuilder sb = new StringBuilder();

            //更新商品销量
            sb.Append(@"update Ecshop_Products set SaleCounts=(case when (SaleCounts - (SELECT sum(o.ShipmentQuantity) AS Quantity
  
  FROM Ecshop_OrderItems AS o 
  
  where O.OrderId =@OrderId and o.ProductId=Ecshop_Products.ProductId))<0 then 0 else 

  (SaleCounts - (SELECT sum(o.ShipmentQuantity) AS Quantity
  
  FROM Ecshop_OrderItems AS o 
  
  where O.OrderId =@OrderId and o.ProductId=Ecshop_Products.ProductId)) end)
  
  where Ecshop_Products.ProductId in (Select ProductId from Ecshop_OrderItems where Ecshop_OrderItems.OrderId=@OrderId);");

            //更新商品显示销量
            sb.Append(@"update Ecshop_Products set ShowSaleCounts=(case when (ShowSaleCounts - (SELECT sum(o.ShipmentQuantity) AS Quantity
  
  FROM Ecshop_OrderItems AS o 
  
  where O.OrderId =@OrderId and o.ProductId=Ecshop_Products.ProductId))<0 then 0 else

  (ShowSaleCounts - (SELECT sum(o.ShipmentQuantity) AS Quantity
  
  FROM Ecshop_OrderItems AS o 
  
  where O.OrderId =@OrderId and o.ProductId=Ecshop_Products.ProductId)) end) 

  
  where Ecshop_Products.ProductId in (Select ProductId from Ecshop_OrderItems where Ecshop_OrderItems.OrderId=@OrderId);");



            //更新组合商品明细销量SaleCounts
            sb.Append(@"update Ecshop_Products set SaleCounts=(case when (SaleCounts - (SELECT sum(oc.Quantity*o.ShipmentQuantity) AS Quantity
  
  FROM Ecshop_OrderItems AS o 
  
  JOIN Ecshop_OrderCombinationItems AS oc ON oc.orderId=o.orderId AND oc.CombinationSkuId=o.skuId
  
  where O.OrderId =@OrderId and oc.ProductId=Ecshop_Products.ProductId))<0 then 0 else 
 
  (SaleCounts - (SELECT sum(oc.Quantity*o.ShipmentQuantity) AS Quantity
  
  FROM Ecshop_OrderItems AS o 
  
  JOIN Ecshop_OrderCombinationItems AS oc ON oc.orderId=o.orderId AND oc.CombinationSkuId=o.skuId
  
  where O.OrderId =@OrderId and oc.ProductId=Ecshop_Products.ProductId)) end)
  
  where Ecshop_Products.ProductId in (Select ProductId from Ecshop_OrderCombinationItems 
  
  
  where Ecshop_OrderCombinationItems.OrderId=@OrderId);");

            //更新组合商品明细显示销量ShowSaleCounts
            sb.Append(@"update Ecshop_Products set ShowSaleCounts=(case when (ShowSaleCounts-(SELECT sum(oc.Quantity*o.ShipmentQuantity) AS Quantity
  
  FROM Ecshop_OrderItems AS o 
  
  JOIN Ecshop_OrderCombinationItems AS oc ON oc.orderId=o.orderId AND oc.CombinationSkuId=o.skuId
  
  where O.OrderId =@OrderId and oc.ProductId=Ecshop_Products.ProductId))<0 then 0 else
 
  (ShowSaleCounts-(SELECT sum(oc.Quantity*o.ShipmentQuantity) AS Quantity
  
  FROM Ecshop_OrderItems AS o 
  
  JOIN Ecshop_OrderCombinationItems AS oc ON oc.orderId=o.orderId AND oc.CombinationSkuId=o.skuId
  
  where O.OrderId =@OrderId and oc.ProductId=Ecshop_Products.ProductId)) end)
  
  where Ecshop_Products.ProductId in (Select ProductId from Ecshop_OrderCombinationItems 
  
  
  where Ecshop_OrderCombinationItems.OrderId=@OrderId);");


            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
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

	}
}

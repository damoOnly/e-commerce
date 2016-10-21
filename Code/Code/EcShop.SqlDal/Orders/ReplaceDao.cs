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
	public class ReplaceDao
	{
		private Database database;
		public ReplaceDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public bool CheckReplace(string orderId, string adminRemark, bool accept)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Ecshop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
			stringBuilder.Append(" update Ecshop_OrderReplace set AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=3 and OrderId = @OrderId;");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
			}
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public string GetReplaceComments(string orderId)
		{
			string query = "select Comments from Ecshop_OrderReplace where HandleStatus=0 and OrderId='" + orderId + "'";
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj == null || obj is DBNull)
			{
				result = "";
			}
			else
			{
				result = obj.ToString();
			}
			return result;
		}
		public DbQueryResult GetReplaceApplys(ReplaceApplyQuery query)
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

			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_OrderReplace", "ReplaceId", stringBuilder.ToString(), "*");
		}


        public DataSet NewGetReplaceApplys(ReplaceApplyQuery query)
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


            string sqlText = "SELECT * FROM vw_Ecshop_OrderReplace where ";
            sqlText += stringBuilder.ToString();

            sqlText += " order by ReplaceId desc ;";
            sqlText = sqlText + " SELECT AId, SkuId, ProductId, Quantity, ItemAdjustedPrice, ItemDescription,ThumbnailsUrl,Weight,SKUContent FROM Ecshop_OrderAppFormItems as A  WHERE ApplyType=2 and AId IN (SELECT ReplaceId FROM vw_Ecshop_OrderReplace WHERE  " + stringBuilder.ToString() + ")";

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sqlText);
            DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = dataSet.Tables[0].Columns["ReplaceId"];
            DataColumn childColumn = dataSet.Tables[1].Columns["AId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn, false);
            dataSet.Relations.Add(relation);
            return dataSet;  
            
        }


		public DataTable GetReplaceApplysTable(int replaceId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Ecshop_OrderReplace where ReplaceId=@ReplaceId");
			this.database.AddInParameter(sqlStringCommand, "ReplaceId", DbType.String, replaceId);
			DataTable result;
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public bool DelReplaceApply(int replaceId)
		{
			string query = string.Format("DELETE FROM Ecshop_OrderReplace WHERE ReplaceId={0} and HandleStatus >0 ", replaceId);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool ApplyForReplace(string orderId, string remark)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Ecshop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
			stringBuilder.Append(" insert into Ecshop_OrderReplace(OrderId,ApplyForTime,Comments,HandleStatus) values(@OrderId,@ApplyForTime,@Comments,0);");
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 8);
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "Comments", DbType.String, remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public bool CanReplace(string orderId)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(*) from Ecshop_OrderReplace where OrderId = @OrderId and HandleStatus = 0");
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) < 1;
		}
        public int GetReplaceCount(int userId)
        {
            string text2 = "SELECT COUNT(1)  FROM vw_Ecshop_OrderReplace o WHERE UserId = @UserId";
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
        public bool UpdateOrderReplaceInfo(ReplaceInfo replaceInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_OrderReplace set HandleStatus = @HandleStatus,Operator=@Operator,ReceiveTime=@ReceiveTime where ReplaceId=@ReplaceId and HandleStatus = 0");
            this.database.AddInParameter(sqlStringCommand, "ReplaceId", DbType.Int32, replaceInfo.ReplaceId);
            this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, replaceInfo.HandleStatus);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, replaceInfo.Operator);
            this.database.AddInParameter(sqlStringCommand, "ReceiveTime", DbType.DateTime, replaceInfo.ReceiveTime);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }


        /// <summary>
        /// 导出换货申请单
        /// </summary>
        /// <param name="startPayTime">申请开始时间</param>
        /// <param name="endPayTime">申请结束时间</param>
        /// <param name="refundIds">退款单号</param>
        /// <returns></returns>
        public DataSet GetExcelOrdeReplace(string startApplyForTime, string endApplyForTime, string replaceIds, int handleStatus)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("select ReplaceId,OrderId,Operator,RefundMoney,OrderTotal,ApplyForTime,Comments,case HandleStatus when 0 then '待处理' when 3 then '处理中' when 1 then '已处理' when 2 then '已拒绝' end as HandleStatusStr,HandleStatus,HandleTime,AdminRemark,Remark from vw_Ecshop_OrderReplace where 1=1 ");
            if (!string.IsNullOrEmpty(startApplyForTime) && !string.IsNullOrEmpty(endApplyForTime))
            {
                stringBuilder.AppendFormat(" AND ApplyForTime>='{0}'  AND ApplyForTime<='{1}'", startApplyForTime, endApplyForTime);
            }
            if (!string.IsNullOrEmpty(replaceIds))
            {
                stringBuilder.AppendFormat(" AND ReplaceId in ({0})", replaceIds);
            }
            if (handleStatus !=-1)
            {
                stringBuilder.AppendFormat(" AND HandleStatus = {0}", handleStatus);
            }
            
            stringBuilder.Append(" ORDER BY ApplyForTime,ReplaceId");
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        public DataSet GetExcelOrdeReplaceDetails(string startApplyForTime, string endApplyForTime, string replaceIds, int handleStatus)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("select ReplaceId,OrderId,Operator,RefundMoney,OrderTotal,ApplyForTime,Comments,case HandleStatus when 0 then '待处理' when 3 then '处理中' when 1 then '已处理' when 2 then '已拒绝' end as HandleStatusStr,HandleStatus,HandleTime,AdminRemark,Remark,ProductId,ItemDescription,ItemAdjustedPrice,Quantity,ItemTaxRate,SupplierName,SKUContent  from vw_Ecshop_OrderReplaceDetails where 1=1 ");
            if (!string.IsNullOrEmpty(startApplyForTime) && !string.IsNullOrEmpty(endApplyForTime))
            {
                stringBuilder.AppendFormat(" AND ApplyForTime>='{0}'  AND ApplyForTime<='{1}'", startApplyForTime, endApplyForTime);
            }
            if (!string.IsNullOrEmpty(replaceIds))
            {
                stringBuilder.AppendFormat(" AND ReplaceId in ({0})", replaceIds);
            }
            if (handleStatus != -1)
            {
                stringBuilder.AppendFormat(" AND HandleStatus = {0}", handleStatus);
            }

            stringBuilder.Append(" ORDER BY ApplyForTime,ReplaceId");
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public bool AddReplaceItem(ReplaceInfo replaceInfo, DbTransaction dbTran)
        {
            bool result;
            if (replaceInfo.ReplaceId == 0)
            {
                result = false;
                return result;
            }
            if (replaceInfo.ReplaceLineItem == null || replaceInfo.ReplaceLineItem.Count == 0)
            {
                result = false;
            }
            else
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
                StringBuilder stringBuilder = new StringBuilder();
                int num = 0;
                foreach (OrderAppFormItems lineItem in replaceInfo.ReplaceLineItem)
                {
                    stringBuilder.Append(" INSERT INTO Ecshop_OrderAppFormItems ([AId],[SkuId],[ProductId],[Quantity],[ItemAdjustedPrice],[ItemDescription],[ThumbnailsUrl],[Weight],[SKUContent],[PromotionId],[PromotionName],[TaxRate],[TemplateId],[storeId],[SupplierId],[ApplyType])  VALUES(").Append("@AId").Append(num).Append(",@SkuId").Append(num).Append(", @ProductId").Append(num).Append(", @Quantity").Append(num).Append(", @ItemAdjustedPrice").Append(num).Append(", @ItemDescription").Append(num).Append(",@ThumbnailsUrl").Append(num).Append(",@Weight").Append(num).Append(", @SKUContent").Append(num).Append(",@PromotionId").Append(num).Append(", @PromotionName").Append(num).Append(",@TaxRate").Append(num).Append(", @TemplateId").Append(num).Append(", @storeId").Append(num).Append(", @SupplierId").Append(num).Append(", @ApplyType").Append(num).Append(")");
                    this.database.AddInParameter(sqlStringCommand, "AId" + num, DbType.Int32, replaceInfo.ReplaceId);
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
        public bool UpdateOrderStatusBeApplyForReplacement(string orderId, DbTransaction dbTran)
        {
            bool result = false;
            if (string.IsNullOrEmpty(orderId))
            {
                return result;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Orders SET OrderStatus = 8 WHERE OrderId = @OrderId;");
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
        public int AddReplaceInfo(ReplaceInfo replaceInfo, DbTransaction dbTran)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //换货表中没有关于物流相关的字段 ldy
            //stringBuilder.Append(" insert into Ecshop_OrderReplace(OrderId,ApplyForTime,Comments,HandleStatus,LogisticsCompany,LogisticsId) values(@OrderId,@ApplyForTime,@Comments,0,@LogisticsCompany,@LogisticsId);SELECT @@identity ");
            stringBuilder.Append(" insert into Ecshop_OrderReplace(OrderId,ApplyForTime,Comments,HandleStatus) values(@OrderId,@ApplyForTime,@Comments,0);SELECT @@identity");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, replaceInfo.OrderId);
            this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Comments", DbType.String, replaceInfo.Comments);
            //this.database.AddInParameter(sqlStringCommand, "LogisticsCompany", DbType.String, replaceInfo.LogisticsCompany);
            //this.database.AddInParameter(sqlStringCommand, "LogisticsId", DbType.String, replaceInfo.LogisticsId);
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
        public bool CreateReturn(ReplaceInfo replaceInfo)//创建退换单
        {
            bool result;
            if (replaceInfo == null)
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
                        if (!UpdateOrderStatusBeApplyForReplacement(replaceInfo.OrderId, dbTransaction))
                        {
                            dbTransaction.Rollback();
                            result = false;
                            return result;
                        }
                        replaceInfo.ReplaceId = AddReplaceInfo(replaceInfo, dbTransaction);
                        if (replaceInfo.ReplaceId <= 0)
                        {
                            dbTransaction.Rollback();
                            result = false;
                            return result;
                        }
                        if (replaceInfo.ReplaceLineItem.Count > 0)
                        {
                            if (!AddReplaceItem(replaceInfo, dbTransaction))
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
        public ReplaceInfo GetReplaceInfo(int ReplaceId)
        {
            ReplaceInfo replaceInfo = null;
            //Username,EmailAddress,RealName,ZipCode,CellPhone
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT A.ReplaceId,A.OrderId,A.ApplyForTime,A.RefundMoney,A.Comments,A.HandleStatus,A.HandleTime,A.AdminRemark,A.Operator,A.ReceiveTime,A1.Username,A1.EmailAddress,A1.RealName,A1.ZipCode,A1.CellPhone FROM Ecshop_OrderReplace as A left join Ecshop_Orders as A1 on A.orderId=A1.orderId  where ReplaceId=@ReplaceId;SELECT o.Id,o.AId,o.SkuId,o.ProductId,o.ApplyType,o.Quantity,o.ItemAdjustedPrice,o.ItemDescription,o.ThumbnailsUrl,o.Weight,o.SKUContent,o.PromotionId,o.PromotionName,o.TaxRate,o.TemplateId,o.storeId,o.SupplierId,o.DeductFee,s.SupplierName FROM Ecshop_OrderAppFormItems as o left JOIN Ecshop_Supplier as s ON o.SupplierId=s.SupplierId where o.AId=@ReplaceId and o.ApplyType=2");
            this.database.AddInParameter(sqlStringCommand, "ReplaceId", DbType.Int32, ReplaceId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                replaceInfo = ReaderConvert.ReaderToModel<ReplaceInfo>(dataReader);
                if (replaceInfo != null)
                {
                    replaceInfo.ReplaceLineItem = new List<OrderAppFormItems>();
                }
                dataReader.NextResult();
                OrderAppFormItems appFormItem = null;
                while (1 == 1)
                {
                    appFormItem = ReaderConvert.ReaderToModel<OrderAppFormItems>(dataReader);
                    if (appFormItem != null && replaceInfo.ReplaceLineItem != null)
                    {
                        replaceInfo.ReplaceLineItem.Add(appFormItem);
                    }
                    if (appFormItem == null)
                    {
                        break;
                    }
                }
            }
            return replaceInfo;
        }
	}
}

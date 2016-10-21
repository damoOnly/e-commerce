using System;
using System.Data;
using System.Data.Common;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.SqlDal.Promotions;
using System.Collections.Generic;
using EcShop.Core.ErrorLog;
using System.Web;
using EcShop.Membership.Context;
using EcShop.Entities.Sales;

namespace EcShop.SqlDal.Orders
{
    public class OrderDao
    {
        private Database database;
        public OrderDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 获取退款列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetRefundOrderList()
        {
            string sql = @"select top 50  OrderId,OrderTotal,PaymentType,SourceOrderId,GatewayOrderId,SourceOrder from [dbo].[Ecshop_Orders]  with(nolock)
                            where IsRefund=2 and IsCancelOrder=1 and OrderStatus=6
                            order by PayDate desc  ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
            if (dataSet!=null)
            {
              return   dataSet.Tables[0];
            }
            return null;
        }

        public DataSet GetUserOrder(int userId, OrderQuery query, int delaytimes=30)
        {
            string sqlWhereText = string.Empty;
            if (query.Status == OrderStatus.WaitBuyerPay)
            {
                sqlWhereText += " AND OrderStatus = 1 AND Gateway <> 'ecdev.plugins.payment.podrequest'";
            }
            else
            {
                if (query.Status == OrderStatus.SellerAlreadySent)
                {
                    sqlWhereText += " AND (OrderStatus = 2 OR OrderStatus = 3 OR (OrderStatus = 1 AND Gateway = 'ecdev.plugins.payment.podrequest'))";
                }
                else
                {
                    sqlWhereText += " AND OrderStatus<>98 ";
                }
            }
            string sqlText = @"SELECT OrderId, OrderDate, OrderStatus,PaymentTypeId, OrderTotal,AdjustedFreight, (SELECT SUM(Quantity) FROM Ecshop_OrderItems WHERE OrderId = o.OrderId) as ProductSum,GateWay,o.SourceOrderId,
		                       case when (o.PayDate>DATEADD(n,-" + delaytimes + ",getdate()) and o.OrderStatus=2)  then 1 else 0  end 'IsCancelOrder',isnull(IsRefund,0) as 'IsRefund' FROM Ecshop_Orders as o WHERE UserId  = @UserId";
            sqlText += sqlWhereText;
            if (!query.ShowGiftOrder)
            {
                sqlText += " And (select count(OrderID) from Ecshop_OrderItems where OrderID=o.OrderId)>0";
            }
            sqlText += " ORDER BY OrderDate DESC";
            sqlText = sqlText + " SELECT OrderId, (select top 1 ThumbnailUrl220 from Ecshop_Products where ProductId= Ecshop_OrderItems.ProductId) ThumbnailsUrl, ItemDescription, SKUContent, SKU,SkuId, ProductId,Quantity,ItemAdjustedPrice FROM Ecshop_OrderItems  WHERE OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE UserId = @UserId" + sqlWhereText + ")";
            if (query.ShowGiftOrder)
            {
                sqlText = sqlText + "select * from Ecshop_OrderGifts where OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE UserId = @UserId" + sqlWhereText + ")";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sqlText);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = dataSet.Tables[0].Columns["OrderId"];
            DataColumn childColumn = dataSet.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn);
            if (query.ShowGiftOrder)
            {
                DataColumn childColumn2 = dataSet.Tables[2].Columns["OrderId"];
                DataRelation relation2 = new DataRelation("GiftItems", parentColumn, childColumn2);
                dataSet.Relations.Add(relation2);
            }
            dataSet.Relations.Add(relation);
            return dataSet;
        }


        /// <summary>
        /// WAP端订单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet GetWAPUserOrder(int userId, OrderQuery query)
        {
            string sqlWhereText = string.Empty;
            if (query.Status == OrderStatus.WaitBuyerPay)
            {
                sqlWhereText += " AND OrderStatus = 1 AND Gateway <> 'ecdev.plugins.payment.podrequest'";
            }

            else if (query.Status == OrderStatus.BuyerAlreadyPaid)
            {
                sqlWhereText += " AND OrderStatus = 2 ";
            }
            else if (query.Status == OrderStatus.SellerAlreadySent)
            {
                sqlWhereText += " AND OrderStatus = 3 ";
            }
            else if (query.Status == OrderStatus.Finished)
            {
                sqlWhereText += " AND OrderStatus = 5 ";
            }
            else
            {

                sqlWhereText += " AND OrderStatus<>98 ";

            }
            string sqlText = "SELECT OrderId, OrderDate, OrderStatus,PaymentTypeId, OrderTotal, (SELECT SUM(Quantity) FROM Ecshop_OrderItems WHERE OrderId = o.OrderId) as ProductSum,GateWay,AdjustedFreight FROM Ecshop_Orders as o WHERE UserId  = @UserId";
            sqlText += sqlWhereText;
            if (!query.ShowGiftOrder)
            {
                sqlText += " And (select count(OrderID) from Ecshop_OrderItems where OrderID=o.OrderId)>0";
            }
            sqlText += " ORDER BY OrderDate DESC";
            sqlText = sqlText + " SELECT A.OrderId, A.ThumbnailsUrl, A.ItemDescription, A.SKUContent, A.SKU, A.ProductId,B.ThumbnailUrl220,B.ThumbnailUrl60  FROM Ecshop_OrderItems A  left join Ecshop_Products B on A.ProductId=B.ProductId    WHERE A.OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE UserId = @UserId" + sqlWhereText + ")";
            if (query.ShowGiftOrder)
            {
                sqlText = sqlText + "select * from Ecshop_OrderGifts where OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE UserId = @UserId" + sqlWhereText + ")";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sqlText);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = dataSet.Tables[0].Columns["OrderId"];
            DataColumn childColumn = dataSet.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn);
            if (query.ShowGiftOrder)
            {
                DataColumn childColumn2 = dataSet.Tables[2].Columns["OrderId"];
                DataRelation relation2 = new DataRelation("GiftItems", parentColumn, childColumn2);
                dataSet.Relations.Add(relation2);
            }
            dataSet.Relations.Add(relation);
            return dataSet;
        }

        public DbQueryResult GetMyUserOrder(int userId, OrderQuery query)
        {
            if (string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "OrderDate";
            }
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" UserId = {0}", userId);
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                sbWhere.AppendFormat(" AND OrderId like '%{0}%'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (!string.IsNullOrEmpty(query.ShipId))
            {
                sbWhere.AppendFormat(" AND ShipOrderNumber = '{0}'", DataHelper.CleanSearchString(query.ShipId));
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                sbWhere.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (query.Status == OrderStatus.History)
            {
                sbWhere.AppendFormat(" AND OrderStatus = {0} AND OrderDate < '{1}'", 5, DateTime.Now.AddMonths(-3));
            }
            else
            {
                if (query.Status != OrderStatus.All)
                {
                    sbWhere.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
                }
            }
            if (query.StartDate.HasValue)
            {
                sbWhere.AppendFormat(" AND OrderDate > '{0}'", query.StartDate);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                sbWhere.AppendFormat(" And OrderID In (select OrderID from Ecshop_OrderItems where ItemDescription like '%" + query.ProductName + "%')", new object[0]);
            }
            if (query.EndDate.HasValue)
            {
                sbWhere.AppendFormat(" AND OrderDate < '{0}'", query.EndDate);
            }
            //排除
            sbWhere.AppendFormat(" AND OrderStatus != {0}", (int)OrderStatus.UnpackOrMixed);

            sbWhere.AppendFormat(" AND UserStatus = {0}", (int)query.UseStatus);

            if (!string.IsNullOrEmpty(query.CellPhone))
            {
                sbWhere.AppendFormat(" AND CellPhone LIKE '%{0}%'", DataHelper.CleanSearchString(query.CellPhone));
            }
            string selectFields = "*";
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, true, "Ecshop_Orders", null, sbWhere.ToString(), selectFields);
        }

        /// <summary>
        /// WAP端获取订单数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public int WAPGetUserOrderCount(int userId, OrderQuery query)
        {
            string text = string.Empty;
            if (query.Status == OrderStatus.WaitBuyerPay)
            {
                text += " AND OrderStatus = 1 AND Gateway <> 'ecdev.plugins.payment.podrequest'";
            }

            else if (query.Status == OrderStatus.BuyerAlreadyPaid)
            {
                text += " AND OrderStatus = 2 ";
            }
            else if (query.Status == OrderStatus.SellerAlreadySent)
            {
                text += " AND OrderStatus = 3 ";
            }
            else if (query.Status == OrderStatus.Finished)
            {
                text += " AND OrderStatus = 5 ";
            }
            else
            {

                text += " AND OrderStatus<>98 ";

            }
            string text2 = "SELECT COUNT(1)  FROM Ecshop_Orders o WHERE UserId = @UserId";
            text2 += text;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text2);
            sqlStringCommand.CommandType = CommandType.Text;
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        /// <summary>
        /// App端获取订单数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public int AppGetUserOrderCount(int userId, OrderQuery query)
        {
            string text = string.Empty;
            if (query.Status == OrderStatus.WaitBuyerPay)
            {
                text += " AND OrderStatus = 1 AND Gateway <> 'ecdev.plugins.payment.podrequest'";
            }

            else if (query.Status == OrderStatus.BuyerAlreadyPaid)
            {
                text += " AND OrderStatus = 2 ";
            }
            else if (query.Status == OrderStatus.SellerAlreadySent)
            {
                text += " AND OrderStatus = 3 ";
            }
            else if (query.Status == OrderStatus.Finished)
            {
                text += " AND OrderStatus = 5 ";
            }
            else
            {

                text += " AND OrderStatus<>98 ";

            }
            string text2 = "SELECT COUNT(1)  FROM Ecshop_Orders o WHERE UserId = @UserId";
            text2 += text;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text2);
            sqlStringCommand.CommandType = CommandType.Text;
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        public int GetWaitCommentOrderCount(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"
            select count(1) from Ecshop_Orders where OrderStatus = 5 and UserId=@UserId 
            and OrderId in(select i.OrderId 
                            from Ecshop_OrderItems i
	                            left join Ecshop_ProductReviews r on r.OrderId = i.OrderId and r.SkuId = i.SkuId
                            where i.OrderId in (select OrderId from Ecshop_Orders where OrderStatus = 5 and UserId = @UserId) 
                            group by i.OrderId having(count(*) > sum( case when r.ReviewId is null then 0 else 1 end))) ");

            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        public int GetUserOrderCount(int userId, OrderQuery query)
        {
            string text = string.Empty;
            if (query.Status == OrderStatus.WaitBuyerPay)
            {
                text += " AND OrderStatus = 1 AND Gateway <> 'ecdev.plugins.payment.podrequest'";
            }
            else
            {
                if (query.Status == OrderStatus.SellerAlreadySent)
                {
                    text += " AND (OrderStatus = 2 OR OrderStatus = 3 OR (OrderStatus = 1 AND Gateway = 'ecdev.plugins.payment.podrequest'))";
                }
                else if (query.Status == OrderStatus.All)
                {
                    text += " AND OrderStatus<>98";
                }
            }
            string text2 = "SELECT COUNT(1)  FROM Ecshop_Orders o WHERE UserId = @UserId";
            text2 += text;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text2);
            sqlStringCommand.CommandType = CommandType.Text;
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        public bool EditOrderShipNumber(string orderId, string shipNumber)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Orders SET ShipOrderNumber=@ShipOrderNumber WHERE OrderId =@OrderId");
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipNumber);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public DbQueryResult GetOrders(OrderQuery query)
        {
            StringBuilder sbWhere = new StringBuilder("1=1");
            GetOrderQuery(query, sbWhere);
            string selectFields = "*";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Ecshop_Orders", "OrderId", sbWhere.ToString(), selectFields);

        }

        public DataTable GetSendGoodsOrders(string orderIds)
        {
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Ecshop_Orders WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'ecdev.plugins.payment.podrequest')) AND OrderId IN ({0}) order by OrderDate desc", orderIds));
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public DataSet GetOrdersAndLines(string orderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT * FROM Ecshop_Orders WHERE OrderStatus > 0 AND OrderStatus < 4 AND OrderId IN ({0}) order by OrderDate desc ", orderIds);
            stringBuilder.AppendFormat(" SELECT * FROM Ecshop_OrderItems WHERE OrderId IN ({0});", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        public DataSet GetOrderGoods(string orderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT OrderId, ItemDescription AS ProductName, SKU, SKUContent, ShipmentQuantity,");
            stringBuilder.Append(" (SELECT Stock FROM Ecshop_SKUs WHERE SkuId = oi.SkuId) + oi.ShipmentQuantity AS Stock, (SELECT Remark FROM Ecshop_Orders WHERE OrderId = oi.OrderId) AS Remark");
            stringBuilder.Append(" FROM Ecshop_OrderItems oi WHERE OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='ecdev.plugins.payment.podrequest'))");
            stringBuilder.AppendFormat(" AND OrderId IN ({0}) ORDER BY OrderId;", orderIds);
            stringBuilder.AppendFormat("SELECT OrderId AS GiftOrderId,GiftName,Quantity FROM dbo.Ecshop_OrderGifts WHERE OrderId IN({0}) AND OrderId IN(SELECT OrderId FROM Ecshop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='ecdev.plugins.payment.podrequest'))", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        //新增按时间查询、待发货的订单
        public System.Data.DataSet GetOrderGoods(string startPayTime, string endPayTime, string orderIds, int orderStatus, int orderTimeType)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT [订单号] = o.OrderId,[买家昵称] = REPLACE(u.RealName, '\"', '“'),[拍单时间] = o.OrderDate,[付款时间] = o.PayDate,[商品总金额] = o.Amount,[订单金额] = o.OrderTotal,[优惠金额] = o.OrderTotal - o.Amount - o.AdjustedFreight,[运费] = o.AdjustedFreight,[货到付款服务费] = o.PayCharge,[订单备注] = ISNULL(o.ManagerRemark, ''),[买家留言] = ISNULL(o.Remark, ''),[收货地址] = o.[Address],[收货人名称] = o.ShipTo,[收货国家] = ''	,[州/省] = (SELECT MAX(Value) FROM [dbo].[ufn_SplitToTable](o.ShippingRegion, '，') WHERE Id = 1)	,[城市] = (SELECT MAX(Value) FROM [dbo].[ufn_SplitToTable](o.ShippingRegion, '，') WHERE Id = 2)	,[区] = (SELECT MAX(Value) FROM [dbo].[ufn_SplitToTable](o.ShippingRegion, '，') WHERE Id = 3),[邮编] = o.ZipCode,[联系电话] = ISNULL(o.TelPhone, ''),[手机] = ISNULL(o.CellPhone, ''),[买家选择物流] = o.ModeName,[最晚发货时间] = '',[海外订单] = '',[是否货到付款] = '否',[是否已发货] = '否',[发货快递单号] = '',[商品名称] = i.ItemDescription,[商品条码] = i.SKU,[购买数量] = i.Quantity,[售价] = i.ItemListPrice,[规格]=i.skuContent,[是否匹配促销模板] = 0,[支付方式]=ISNULL(o.PaymentType, ''), [交易流水号]=ISNULL(o.GatewayOrderId, ''),[收货人证件号] = o.IdentityCard,[供货商]=isnull(s.supplierName,'') ");
            stringBuilder.Append(",[订单状态]=o.OrderStatus,[优惠券金额]=o.CouponAmount,[优惠券面值]=o.CouponAmount,[用户名]=au.UserName,[发货时间]=o.ShippingDate,[完成时间]=o.FinishDate");
            stringBuilder.Append(" FROM Ecshop_Orders o JOIN Ecshop_OrderItems i ON i.OrderID = o.OrderID JOIN [aspnet_Members] u ON u.UserId = o.UserId JOIN [aspnet_Users] au ON au.UserId=o.UserId  left join Ecshop_Products as p on i.productid=p.productid left join Ecshop_Supplier as s on i.supplierid=s.supplierid  WHERE 1=1  ");

            switch (orderTimeType)
            {
                case 1:
                    if (!string.IsNullOrEmpty(startPayTime) && !string.IsNullOrEmpty(endPayTime))
                    {
                        stringBuilder.AppendFormat(" AND o.OrderDate>='{0}'  AND o.OrderDate<='{1}' ", startPayTime, endPayTime);
                    }
                    break;
                case 2:
                    if (!string.IsNullOrEmpty(startPayTime) && !string.IsNullOrEmpty(endPayTime))
                    {
                        stringBuilder.AppendFormat(" AND o.PayDate>='{0}'  AND o.PayDate<='{1}' ", startPayTime, endPayTime);
                    }

                    SelPayTime(orderStatus, stringBuilder);


                    break;
                case 3:
                    if (!string.IsNullOrEmpty(startPayTime) && !string.IsNullOrEmpty(endPayTime))
                    {
                        stringBuilder.AppendFormat(" AND o.ShippingDate>='{0}'  AND o.ShippingDate<='{1}' ", startPayTime, endPayTime);
                    }

                    SelSendTime(orderStatus, stringBuilder);

                    break;
            }

            if (!string.IsNullOrEmpty(orderIds))
            {
                stringBuilder.AppendFormat(" AND o.OrderId in ({0})", orderIds);
            }
            if (orderStatus != 0)
            {
                stringBuilder.AppendFormat(" AND o.OrderStatus = {0}", orderStatus);
            }
            stringBuilder.Append(" ORDER BY o.PayDate, o.OrderId");
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        public System.Data.DataSet GetOrderGoods(string startPayTime, string endPayTime, string orderIds, int orderStatus, int orderTimeType, int supplierId)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT [订单号] = o.OrderId,[买家昵称] = REPLACE(u.RealName, '\"', '“'),[拍单时间] = o.OrderDate,[付款时间] = o.PayDate,[商品总金额] = o.Amount,[订单金额] = o.OrderTotal,[优惠金额] = o.OrderTotal - o.Amount - o.AdjustedFreight,[运费] = o.AdjustedFreight,[货到付款服务费] = o.PayCharge,[订单备注] = ISNULL(o.ManagerRemark, ''),[买家留言] = ISNULL(o.Remark, ''),[收货地址] = o.[Address],[收货人名称] = o.ShipTo,[收货国家] = ''	,[州/省] = (SELECT MAX(Value) FROM [dbo].[ufn_SplitToTable](o.ShippingRegion, '，') WHERE Id = 1)	,[城市] = (SELECT MAX(Value) FROM [dbo].[ufn_SplitToTable](o.ShippingRegion, '，') WHERE Id = 2)	,[区] = (SELECT MAX(Value) FROM [dbo].[ufn_SplitToTable](o.ShippingRegion, '，') WHERE Id = 3),[邮编] = o.ZipCode,[联系电话] = ISNULL(o.TelPhone, ''),[手机] = ISNULL(o.CellPhone, ''),[买家选择物流] = o.ModeName,[最晚发货时间] = '',[海外订单] = '',[是否货到付款] = '否',[是否已发货] = '否',[发货快递单号] = '',[商品名称] = i.ItemDescription,[商品条码] = i.SKU,[购买数量] = i.Quantity,[售价] = i.ItemListPrice,[规格]=i.skuContent,[是否匹配促销模板] = 0,[支付方式]=ISNULL(o.PaymentType, ''), [交易流水号]=ISNULL(o.GatewayOrderId, ''),[收货人证件号] = o.IdentityCard,[供货商]=isnull(s.supplierName,'') ");
            stringBuilder.Append(",[订单状态]=o.OrderStatus,[优惠券金额]=o.CouponAmount,[优惠券面值]=o.CouponAmount,[用户名]=au.UserName,[发货时间]=o.ShippingDate,[完成时间]=o.FinishDate");
            stringBuilder.Append(" FROM Ecshop_Orders o JOIN Ecshop_OrderItems i ON i.OrderID = o.OrderID JOIN [aspnet_Members] u ON u.UserId = o.UserId JOIN [aspnet_Users] au ON au.UserId=o.UserId left join Ecshop_Products as p on i.productid=p.productid left join Ecshop_Supplier as s on i.supplierid=s.supplierid  WHERE 1=1  ");

            switch (orderTimeType)
            {
                case 1:
                    if (!string.IsNullOrEmpty(startPayTime) && !string.IsNullOrEmpty(endPayTime))
                    {
                        stringBuilder.AppendFormat(" AND o.OrderDate>='{0}'  AND o.OrderDate<='{1}' ", startPayTime, endPayTime);
                    }
                    //默认下单时间排除拆分或者已经合并的原单
                    stringBuilder.AppendFormat(" And o.OrderStatus<> {0}", (int)OrderStatus.UnpackOrMixed);
                    break;
                case 2:
                    if (!string.IsNullOrEmpty(startPayTime) && !string.IsNullOrEmpty(endPayTime))
                    {
                        stringBuilder.AppendFormat(" AND o.PayDate>='{0}'  AND o.PayDate<='{1}' ", startPayTime, endPayTime);
                    }

                    SelPayTime(orderStatus, stringBuilder);

                    break;
                case 3:
                    if (!string.IsNullOrEmpty(startPayTime) && !string.IsNullOrEmpty(endPayTime))
                    {
                        stringBuilder.AppendFormat(" AND o.ShippingDate>='{0}'  AND o.ShippingDate<='{1}' ", startPayTime, endPayTime);
                    }

                    SelSendTime(orderStatus, stringBuilder);

                    break;
            }

            if (!string.IsNullOrEmpty(orderIds))
            {
                stringBuilder.AppendFormat(" AND o.OrderId in ({0})", orderIds);
            }
            if (orderStatus != 0)
            {
                stringBuilder.AppendFormat(" AND o.OrderStatus = {0}", orderStatus);
            }
            if (supplierId != 0)
            {
                stringBuilder.AppendFormat(" AND i.SupplierId = {0} ", supplierId);
            }
            stringBuilder.Append(" ORDER BY o.PayDate, o.OrderId");
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        public System.Data.DataSet GetProductGoods(string orderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ItemDescription AS ProductName, SKU, SKUContent, sum(ShipmentQuantity) as ShipmentQuantity,");
            stringBuilder.Append(" (SELECT Stock FROM Ecshop_SKUs WHERE SkuId = oi.SkuId) + sum(ShipmentQuantity) AS Stock FROM Ecshop_OrderItems oi");
            stringBuilder.Append(" WHERE OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='ecdev.plugins.payment.podrequest'))");
            stringBuilder.AppendFormat(" AND OrderId in ({0}) GROUP BY ItemDescription, SkuId, SKU, SKUContent;", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        public System.Data.DataSet GetProductGoods(string orderIds, int supplierId)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ItemDescription AS ProductName, SKU, SKUContent, sum(ShipmentQuantity) as ShipmentQuantity,");
            stringBuilder.Append(" (SELECT Stock FROM Ecshop_SKUs WHERE SkuId = oi.SkuId) + sum(ShipmentQuantity) AS Stock FROM Ecshop_OrderItems oi");
            stringBuilder.AppendFormat(" WHERE OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='ecdev.plugins.payment.podrequest'))");
            if (supplierId > 0)
            {
                stringBuilder.AppendFormat(" And oi.SupplierId={0} ", supplierId);
            }
            stringBuilder.AppendFormat(" AND OrderId in ({0}) GROUP BY ItemDescription, SkuId, SKU, SKUContent;", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        //新增按时间查询、清关的订单
        public System.Data.DataSet GetClearOrderGoods(string startPayTime, string endPayTime, string orderIds, int orderStatus, int orderTimeType)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder stringBuilder = new StringBuilder();
            //清关的订单金额不包含运费和税。所以公式 = 订单总金额-税-运费(o.OrderTotal-o.Tax-o.AdjustedFreight)
            stringBuilder.Append("SELECT [销售订单号] = o.OrderId,[电商平台名称]='',[电商平台备案号]='',[电商客户名称]='',[电商商户备案号]='',[电商客户电话]='',[订单人姓名] = REPLACE(o.ShipTo, '\"', '“'),[订单人证件类型] = '身份证',[订单人证件号] = o.IdentityCard,[订单人电话] = ISNULL(u.CellPhone, ''),[订单日期] = convert(varchar(10),o.OrderDate,120),[收件人姓名] = o.ShipTo,[收件人证件号码] = o.IdentityCard,[收件人地址] = (SELECT MAX(Value) FROM [dbo].[ufn_SplitToTable](o.ShippingRegion, '，') WHERE Id = 1)+(SELECT MAX(Value) FROM [dbo].[ufn_SplitToTable](o.ShippingRegion, '，') WHERE Id = 2)+(SELECT MAX(Value) FROM [dbo].[ufn_SplitToTable](o.ShippingRegion, '，') WHERE Id = 3)+ o.[Address],[收件人电话] = ISNULL(o.CellPhone, ''),[商品名称] = i.ItemDescription,[商品货号] = i.SKU,[申报数量] = i.Quantity,[申报单价] = i.ItemListPrice,[申报总价] = o.Amount,[运费] = o.AdjustedFreight,[保价费]=0,[税款]=o.Tax,[毛重] = ISNULL(i.[Weight], 0),[净重] = ISNULL(i.[Weight], 0),[选用的快递公司] = REPLACE(ISNULL(o.ExpressCompanyName, ''), '\"', '“'),[网址] = '',[供货商]=isnull(s.supplierName,'') ");
            stringBuilder.Append(" FROM Ecshop_Orders o JOIN Ecshop_OrderItems i ON i.OrderID = o.OrderID JOIN [aspnet_Members] u ON u.UserId = o.UserId left join Ecshop_Products as p on i.productid=p.productid left join Ecshop_Supplier as s on i.supplierid=s.supplierid WHERE o.IsCustomsClearance = 1 ");

            switch (orderTimeType)
            {
                case 1:
                    if (!string.IsNullOrEmpty(startPayTime) && !string.IsNullOrEmpty(endPayTime))
                    {
                        stringBuilder.AppendFormat(" AND o.OrderDate>='{0}'  AND o.OrderDate<='{1}' ", startPayTime, endPayTime);
                    }
                    break;
                case 2:
                    if (!string.IsNullOrEmpty(startPayTime) && !string.IsNullOrEmpty(endPayTime))
                    {
                        stringBuilder.AppendFormat(" AND o.PayDate>='{0}'  AND o.PayDate<='{1}' ", startPayTime, endPayTime);
                    }
                    SelPayTime(orderStatus, stringBuilder);
                    break;
                case 3:
                    if (!string.IsNullOrEmpty(startPayTime) && !string.IsNullOrEmpty(endPayTime))
                    {
                        stringBuilder.AppendFormat(" AND o.ShippingDate>='{0}'  AND o.ShippingDate<='{1}' ", startPayTime, endPayTime);
                    }
                    SelSendTime(orderStatus, stringBuilder);
                    break;
            }

            if (!string.IsNullOrEmpty(orderIds))
            {
                stringBuilder.AppendFormat(" AND o.OrderId in ({0})", orderIds);
            }

            if (orderStatus != 0)
            {
                stringBuilder.AppendFormat("  AND o.OrderStatus ={0}", orderStatus);
            }
            stringBuilder.Append(" ORDER BY o.PayDate, o.OrderId");
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        public System.Data.DataSet GetClearOrderGoods(string startPayTime, string endPayTime, string orderIds, int orderStatus, int orderTimeType, int supplierId)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder stringBuilder = new StringBuilder();
            //清关的订单金额不包含运费和税。所以公式 = 订单总金额-税-运费(o.OrderTotal-o.Tax-o.AdjustedFreight)
            stringBuilder.Append("SELECT [销售订单号] = o.OrderId,[电商平台名称]='',[电商平台备案号]='',[电商客户名称]='',[电商商户备案号]='',[电商客户电话]='',[订单人姓名] = REPLACE(o.ShipTo, '\"', '“'),[订单人证件类型] = '身份证',[订单人证件号] = o.IdentityCard,[订单人电话] = ISNULL(u.CellPhone, ''),[订单日期] = convert(varchar(10),o.OrderDate,120),[收件人姓名] = o.ShipTo,[收件人证件号码] = o.IdentityCard,[收件人地址] = (SELECT MAX(Value) FROM [dbo].[ufn_SplitToTable](o.ShippingRegion, '，') WHERE Id = 1)+(SELECT MAX(Value) FROM [dbo].[ufn_SplitToTable](o.ShippingRegion, '，') WHERE Id = 2)+(SELECT MAX(Value) FROM [dbo].[ufn_SplitToTable](o.ShippingRegion, '，') WHERE Id = 3)+ o.[Address],[收件人电话] = ISNULL(o.CellPhone, ''),[商品名称] = i.ItemDescription,[商品货号] = i.SKU,[申报数量] = i.Quantity,[申报单价] = i.ItemListPrice,[申报总价] = o.Amount,[运费] = o.AdjustedFreight,[保价费]=0,[税款]=o.Tax,[毛重] = ISNULL(i.[Weight], 0),[净重] = ISNULL(i.[Weight], 0),[选用的快递公司] = REPLACE(ISNULL(o.ExpressCompanyName, ''), '\"', '“'),[网址] = '',[供货商]=isnull(s.supplierName,'') ");
            stringBuilder.Append(" FROM Ecshop_Orders o JOIN Ecshop_OrderItems i ON i.OrderID = o.OrderID JOIN [aspnet_Members] u ON u.UserId = o.UserId left join Ecshop_Products as p on i.productid=p.productid left join Ecshop_Supplier as s on i.supplierid=s.supplierid WHERE o.IsCustomsClearance = 1 ");

            switch (orderTimeType)
            {
                case 1:
                    if (!string.IsNullOrEmpty(startPayTime) && !string.IsNullOrEmpty(endPayTime))
                    {
                        stringBuilder.AppendFormat(" AND o.OrderDate>='{0}'  AND o.OrderDate<='{1}' ", startPayTime, endPayTime);
                    }
                    //默认下单时间排除拆分或者已经合并的原单
                    stringBuilder.AppendFormat(" And o.OrderStatus<> {0}", (int)OrderStatus.UnpackOrMixed);
                    break;
                case 2:
                    if (!string.IsNullOrEmpty(startPayTime) && !string.IsNullOrEmpty(endPayTime))
                    {
                        stringBuilder.AppendFormat(" AND o.PayDate>='{0}'  AND o.PayDate<='{1}' ", startPayTime, endPayTime);
                    }

                    SelPayTime(orderStatus, stringBuilder);

                    break;

                case 3:
                    if (!string.IsNullOrEmpty(startPayTime) && !string.IsNullOrEmpty(endPayTime))
                    {
                        stringBuilder.AppendFormat(" AND o.ShippingDate>='{0}'  AND o.ShippingDate<='{1}' ", startPayTime, endPayTime);
                    }

                    SelSendTime(orderStatus, stringBuilder);

                    break;
            }

            if (!string.IsNullOrEmpty(orderIds))
            {
                stringBuilder.AppendFormat(" AND o.OrderId in ({0})", orderIds);
            }

            if (orderStatus != 0)
            {
                stringBuilder.AppendFormat("  AND o.OrderStatus ={0}", orderStatus);
            }
            if (supplierId > 0)
            {
                stringBuilder.AppendFormat("  AND i.SupplierId ={0}", supplierId);
            }
            stringBuilder.Append(" ORDER BY o.PayDate, o.OrderId");
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        private static void SelSendTime(int orderStatus, StringBuilder stringBuilder)
        {
            //选择发货时间，历史订单，排除等待买家付款、已关闭、已退款的，发货时间为三个月之前的。
            if (orderStatus == (int)OrderStatus.History)
            {
                stringBuilder.AppendFormat(" AND o.ShippingDate < '{3}'", new object[] { DateTime.Now.AddMonths(-3) });
            }
            else if (orderStatus == (int)OrderStatus.All)
            {
                //所有订单，排除等待买家付款、等待发货、已关闭、历史订单、已退款、申请退款的
                stringBuilder.AppendFormat(" AND o.OrderStatus <> {0} And o.OrderStatus<> {1} AND o.OrderStatus<>{2}", (int)OrderStatus.BuyerAlreadyPaid, (int)OrderStatus.UnpackOrMixed, (int)OrderStatus.ApplyForRefund);
            }
            else
            {
                //订单状态等于当前状态，并且排除已关闭、已退款、等待发货、等待买家付款 
                stringBuilder.AppendFormat(" AND o.OrderStatus = {0} And o.OrderStatus<> {1}", orderStatus, (int)OrderStatus.BuyerAlreadyPaid);
            }

            //订单状态等于当前状态，并且排除已关闭、已退款、等待发货、等待买家付款 
            stringBuilder.AppendFormat(" AND o.OrderStatus = {0} And o.OrderStatus<> {1} AND o.OrderStatus<>{2}", (int)OrderStatus.Closed, (int)OrderStatus.Refunded, (int)OrderStatus.WaitBuyerPay);
        }

        private static void SelPayTime(int orderStatus, StringBuilder stringBuilder)
        {

            //选择付款时间，历史订单，付款时间为三个月之前的。
            if (orderStatus == (int)OrderStatus.History)
            {
                stringBuilder.AppendFormat(" AND o.PayDate < '{3}'", new object[] { DateTime.Now.AddMonths(-3) });
            }
            else if (orderStatus == (int)OrderStatus.All)
            {
                //所有订单,排除等待发货
                stringBuilder.AppendFormat(" AND o.OrderStatus >= {0}", (int)OrderStatus.BuyerAlreadyPaid);
            }
            else
            {
                //订单状态等于当前状态
                stringBuilder.AppendFormat(" AND o.OrderStatus = {0}", orderStatus);
            }

            //排除已关闭、已退款、等待买家付款
            stringBuilder.AppendFormat(" And o.OrderStatus<> {0} AND o.OrderStatus<>{1} AND o.OrderStatus<>{2}", (int)OrderStatus.Closed, (int)OrderStatus.Refunded, (int)OrderStatus.WaitBuyerPay);
        }

        public System.Data.DataSet GetClearProductGoods(string orderIds, int orderStatus)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ItemDescription AS ProductName, SKU, SKUContent, sum(ShipmentQuantity) as ShipmentQuantity,");
            stringBuilder.Append(" (SELECT Stock FROM Ecshop_SKUs WHERE SkuId = oi.SkuId) + sum(ShipmentQuantity) AS Stock FROM Ecshop_OrderItems oi");
            stringBuilder.Append(" WHERE OrderId IN (SELECT OrderId FROM Ecshop_Orders WHERE IsCustomsClearance = 1 and OrderStatus = 2 )");
            stringBuilder.AppendFormat(" AND OrderId in ({0}) GROUP BY ItemDescription, SkuId, SKU, SKUContent;", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        /// <summary>
        /// 获取子单列表
        /// </summary>
        /// <param name="SourceOrderId"></param>
        /// <returns></returns>
        public List<OrderApiCode> Getlistofchildren(string SourceOrderId)
        {
            List<OrderApiCode> list = new List<OrderApiCode>();
            try
            {
                string sql = "select OrderId from [dbo].[Ecshop_Orders] where SourceOrderId=@SourceOrderId";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
                this.database.AddInParameter(sqlStringCommand, "SourceOrderId", DbType.String, SourceOrderId);
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    OrderApiCode or = null;
                    while (dataReader.Read())
                    {
                        or = new OrderApiCode();
                        or.OrderId = dataReader["OrderId"].ToString();
                        list.Add(or);
                    }
                }
                return list;
              
            }
            catch (Exception ee)
            {
                ErrorLog.Write("Getlistofchildren："+ee.Message,ee);
                return list;
            }
        }
        /// <summary>
        /// 根据订单号查询是否已申请报告
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public bool CheckOrderIsbg(string OrderId)
        {
            try
            {
                string sql = "SELECT count(1) FROM dbo.HS_Docking WHERE DeclareStatus<>0 AND OrderId=@OrderId";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
                this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
                object obj = this.database.ExecuteScalar(sqlStringCommand);
                if (obj!=null)
                {
                    int count = 0;
                    int.TryParse(obj.ToString(), out count);
                    return count > 0 ? false : true;
                }
                return false;
            }
            catch (Exception ee)
            {
                ErrorLog.Write("CheckOrderIsbg:"+ee.Message);
                return false;
            }

        }

        public OrderInfo GetOrderInfo(string orderId)
        {
            OrderInfo orderInfo = null;
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Orders Where OrderId = @OrderId;  SELECT A1.*,A3.SupplierName,A3.SupplierId,(select OrderStatus from Ecshop_Orders where OrderId=@OrderId) OrderStatus,A4.ThumbnailUrl220  FROM Ecshop_OrderItems  as A1 	LEFT JOIN Ecshop_Supplier as A3 ON A1.SupplierId =A3.SupplierId  left join Ecshop_Products as A4 on A1.ProductId=A4.ProductId  Where OrderId = @OrderId;Select * from Ecshop_OrderGifts where OrderID=@OrderID;select * from ecshop_orderpresents  where orderid = @OrderID");
            string sql = @"SELECT  case when (o.PayDate>DATEADD(n,-@delaytimes,getdate()) and o.OrderStatus=2) and o.IsRefund=0  then 1 else 0  end as 'NewIsCancelOrder',
o.*,ISNULL(h.payerIdStatus,0) AS payerIdStatus,ISNULL(h.PaymentStatus,0) AS PaymentStatus FROM Ecshop_Orders  AS o  LEFT JOIN HS_Docking AS h ON o.orderId=h.orderId Where O.OrderId = @OrderId;  SELECT A1.*,A3.SupplierName,A3.SupplierId,(select OrderStatus from Ecshop_Orders where OrderId=@OrderId) OrderStatus,A4.ThumbnailUrl220  FROM Ecshop_OrderItems  as A1 	LEFT JOIN Ecshop_Supplier as A3 ON A1.SupplierId =A3.SupplierId  left join Ecshop_Products as A4 on A1.ProductId=A4.ProductId  Where OrderId = @OrderId;Select * from Ecshop_OrderGifts where OrderID=@OrderID;select * from ecshop_orderpresents  where orderid = @OrderID";

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            int delaytime = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["OrderRefunTime"]) ? 30 : int.Parse(System.Configuration.ConfigurationManager.AppSettings["OrderRefunTime"].ToString());
            this.database.AddInParameter(sqlStringCommand, "delaytimes", DbType.Int32, delaytime);
            PromotionDao promotionDao = new PromotionDao();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    orderInfo = DataMapper.PopulateOrder(dataReader);

                    if (dataReader["IsSendWMS"] != System.DBNull.Value)
                    {
                        orderInfo.IsSendWMS = (int)dataReader["IsSendWMS"];
                    }

                    if (dataReader["payerIdStatus"] != System.DBNull.Value)
                    {
                        orderInfo.payerIdStatus = (int)dataReader["payerIdStatus"];
                    }

                    if (dataReader["PaymentStatus"] != System.DBNull.Value)
                    {
                        orderInfo.PaymentStatus = (int)dataReader["PaymentStatus"];
                    }
                    if (dataReader["IsRefund"] != System.DBNull.Value)
                    {
                        orderInfo.IsRefund = (int)dataReader["IsRefund"];
                    }
                    if (dataReader["IsCancelOrder"] != System.DBNull.Value)
                    {
                        orderInfo.IsCancelOrder = (int)dataReader["IsCancelOrder"];
                    }
                    if (dataReader["NewIsCancelOrder"] != System.DBNull.Value)
                    {
                        orderInfo.IsCancelOrder = (int)dataReader["NewIsCancelOrder"];
                    }

                }

                if (orderInfo == null)
                {
                    return orderInfo;
                }


                dataReader.NextResult();
                while (dataReader.Read())
                {
                    LineItemInfo lineItemInfo = DataMapper.PopulateLineItem(dataReader);
                    if (lineItemInfo.PromotionId > 0)
                    {
                        PromotionInfo promotion = promotionDao.GetPromotion(lineItemInfo.PromotionId);
                        if (promotion != null)
                        {
                            lineItemInfo.PromoteType = promotion.PromoteType;
                        }
                    }
                    orderInfo.LineItems.Add((string)dataReader["SkuId"], lineItemInfo);
                }
                dataReader.NextResult();
                while (dataReader.Read())
                {
                    OrderGiftInfo item = DataMapper.PopulateOrderGift(dataReader);
                    orderInfo.Gifts.Add(item);
                }
                // 赠送商品
                dataReader.NextResult();
                while (dataReader.Read())
                {
                    ShoppingCartPresentInfo item = DataMapper.PopulateOrderPresents(dataReader);
                    orderInfo.PresentProducts.Add(item);
                }
            }
            orderInfo.Remarks = GetOrderRemarks(orderId);
            return orderInfo;
        }

        /// <summary>
        /// 获取订单物流信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderExpress GetOrderExpressInfo(string orderId)
        {
            OrderExpress orderExpress = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ShipOrderNumber,ExpressCompanyName,OrderStatus,ExpressCompanyAbb FROM Ecshop_Orders Where OrderId = @OrderId;");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    orderExpress = DataMapper.PopulateOrderExpress(dataReader);
                }
            }
            return orderExpress;
        }

        public DataTable GetOrderItemThumbnailsUrl(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ROW_NUMBER() over(order by OrderId desc) as RowNumber,ThumbnailsUrl,ItemDescription,ProductId,Quantity,ItemListPrice FROM Ecshop_OrderItems WHERE OrderId=@OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
            return dataSet.Tables[0];
        }
        public int DeleteOrders(string orderIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Ecshop_Orders WHERE OrderId IN({0})", orderIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        /// <summary>
        /// 会员中心，逻辑删除订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="saleStatus"></param>
        /// <returns></returns>
        public int LogicDeleteOrder(string orderId, int userStatus)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Orders SET UserStatus = {0} WHERE OrderId = '{1}'", userStatus, orderId));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        /// <summary>
        /// 获取3分钟内的订单数以及当天的订单总数。
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderCountIn3Min"></param>
        /// <param name="todayOrderCount"></param>
        public void GetOrderCount4MaliciousOrder(int userId, out int orderCountIn3Min, out int todayOrderCount)
        {
            orderCountIn3Min = 0;
            todayOrderCount = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT COUNT(1) FROM dbo.Ecshop_Orders WHERE OrderDate>=DATEADD(MINUTE,-3,GETDATE()) AND SourceOrderId IS NULL AND UserId={0};select COUNT(1) FROM dbo.Ecshop_Orders WHERE datediff(day,OrderDate,getdate())=0 AND SourceOrderId IS NULL AND UserId={0}", userId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    orderCountIn3Min = (int)reader[0];
                }
                reader.NextResult();
                if (reader.Read())
                {
                    todayOrderCount = (int)reader[0];
                }
            }
        }

        public bool CreatOrder(OrderInfo orderInfo, DbTransaction dbTran, bool isChildAndFirstChildThenOrOriginalOrder)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_CreateOrder");
            this.database.AddInParameter(storedProcCommand, "OrderId", DbType.String, orderInfo.OrderId);
            this.database.AddInParameter(storedProcCommand, "OrderDate", DbType.DateTime, orderInfo.OrderDate);
            this.database.AddInParameter(storedProcCommand, "ReferralUserId", DbType.Int32, orderInfo.ReferralUserId);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, orderInfo.UserId);
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, orderInfo.Username);
            this.database.AddInParameter(storedProcCommand, "Wangwang", DbType.String, orderInfo.Wangwang);
            this.database.AddInParameter(storedProcCommand, "RealName", DbType.String, orderInfo.RealName);
            this.database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, orderInfo.EmailAddress);
            this.database.AddInParameter(storedProcCommand, "Remark", DbType.String, orderInfo.Remark);
            this.database.AddInParameter(storedProcCommand, "AdjustedDiscount", DbType.Currency, orderInfo.AdjustedDiscount);
            this.database.AddInParameter(storedProcCommand, "OrderStatus", DbType.Int32, (int)orderInfo.OrderStatus);
            this.database.AddInParameter(storedProcCommand, "ShippingRegion", DbType.String, orderInfo.ShippingRegion);
            this.database.AddInParameter(storedProcCommand, "Address", DbType.String, orderInfo.Address);
            this.database.AddInParameter(storedProcCommand, "ZipCode", DbType.String, orderInfo.ZipCode);
            this.database.AddInParameter(storedProcCommand, "ShipTo", DbType.String, orderInfo.ShipTo);
            this.database.AddInParameter(storedProcCommand, "TelPhone", DbType.String, orderInfo.TelPhone);
            this.database.AddInParameter(storedProcCommand, "CellPhone", DbType.String, orderInfo.CellPhone);
            this.database.AddInParameter(storedProcCommand, "ShipToDate", DbType.String, orderInfo.ShipToDate);
            this.database.AddInParameter(storedProcCommand, "ShippingModeId", DbType.Int32, orderInfo.ShippingModeId);
            this.database.AddInParameter(storedProcCommand, "ModeName", DbType.String, orderInfo.ModeName);
            this.database.AddInParameter(storedProcCommand, "RegionId", DbType.Int32, orderInfo.RegionId);
            this.database.AddInParameter(storedProcCommand, "Freight", DbType.Currency, orderInfo.Freight);
            this.database.AddInParameter(storedProcCommand, "AdjustedFreight", DbType.Currency, orderInfo.AdjustedFreight);
            this.database.AddInParameter(storedProcCommand, "ShipOrderNumber", DbType.String, orderInfo.ShipOrderNumber);
            this.database.AddInParameter(storedProcCommand, "Weight", DbType.Int32, orderInfo.Weight);
            this.database.AddInParameter(storedProcCommand, "ExpressCompanyName", DbType.String, orderInfo.ExpressCompanyName);
            this.database.AddInParameter(storedProcCommand, "ExpressCompanyAbb", DbType.String, orderInfo.ExpressCompanyAbb);
            this.database.AddInParameter(storedProcCommand, "PaymentTypeId", DbType.Int32, orderInfo.PaymentTypeId);
            this.database.AddInParameter(storedProcCommand, "PaymentType", DbType.String, orderInfo.PaymentType);
            this.database.AddInParameter(storedProcCommand, "PayCharge", DbType.Currency, orderInfo.PayCharge);
            this.database.AddInParameter(storedProcCommand, "RefundStatus", DbType.Int32, (int)orderInfo.RefundStatus);
            this.database.AddInParameter(storedProcCommand, "Gateway", DbType.String, orderInfo.Gateway);
            this.database.AddInParameter(storedProcCommand, "OrderTotal", DbType.Currency, orderInfo.GetTotal(isChildAndFirstChildThenOrOriginalOrder));
            this.database.AddInParameter(storedProcCommand, "OrderPoint", DbType.Int32, orderInfo.GetAmount() * orderInfo.TimesPoint);
            this.database.AddInParameter(storedProcCommand, "OrderCostPrice", DbType.Currency, orderInfo.GetCostPrice());
            this.database.AddInParameter(storedProcCommand, "OrderProfit", DbType.Currency, orderInfo.GetProfit());
            this.database.AddInParameter(storedProcCommand, "Amount", DbType.Currency, orderInfo.GetAmount());
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionId", DbType.Int32, orderInfo.ReducedPromotionId);
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionName", DbType.String, orderInfo.ReducedPromotionName);
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionAmount", DbType.Currency, orderInfo.ReducedPromotionAmount);
            this.database.AddInParameter(storedProcCommand, "IsReduced", DbType.Boolean, orderInfo.IsReduced);
            this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionId", DbType.Int32, orderInfo.SentTimesPointPromotionId);
            this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionName", DbType.String, orderInfo.SentTimesPointPromotionName);
            this.database.AddInParameter(storedProcCommand, "TimesPoint", DbType.Currency, orderInfo.TimesPoint);
            this.database.AddInParameter(storedProcCommand, "IsSendTimesPoint", DbType.Boolean, orderInfo.IsSendTimesPoint);
            this.database.AddInParameter(storedProcCommand, "FreightFreePromotionId", DbType.Int32, orderInfo.FreightFreePromotionId);
            this.database.AddInParameter(storedProcCommand, "FreightFreePromotionName", DbType.String, orderInfo.FreightFreePromotionName);
            this.database.AddInParameter(storedProcCommand, "IsFreightFree", DbType.Boolean, orderInfo.IsFreightFree);
            this.database.AddInParameter(storedProcCommand, "CouponName", DbType.String, orderInfo.CouponName);
            this.database.AddInParameter(storedProcCommand, "CouponCode", DbType.String, orderInfo.CouponCode);
            this.database.AddInParameter(storedProcCommand, "CouponAmount", DbType.Currency, orderInfo.CouponAmount);
            this.database.AddInParameter(storedProcCommand, "CouponValue", DbType.Currency, orderInfo.CouponValue);
            this.database.AddInParameter(storedProcCommand, "IsCustomsClearance", DbType.Int32, orderInfo.IsCustomsClearance);
            this.database.AddInParameter(storedProcCommand, "PCustomsClearanceDate", DbType.Date, orderInfo.PCustomsClearanceDate);
            this.database.AddInParameter(storedProcCommand, "OrderType", DbType.String, orderInfo.OrderType);
            this.database.AddInParameter(storedProcCommand, "OriginalTax", DbType.Decimal, orderInfo.OriginalTax);
            this.database.AddInParameter(storedProcCommand, "IdentityCard", DbType.String, orderInfo.IdentityCard);
            this.database.AddInParameter(storedProcCommand, "GatewayOrderId", DbType.String, orderInfo.GatewayOrderId);
            this.database.AddInParameter(storedProcCommand, "SiteId", DbType.Int32, orderInfo.SiteId);
            this.database.AddInParameter(storedProcCommand, "VoucherName", DbType.String, orderInfo.VoucherName);
            this.database.AddInParameter(storedProcCommand, "VoucherCode", DbType.String, orderInfo.VoucherCode);
            this.database.AddInParameter(storedProcCommand, "VoucherAmount", DbType.Currency, orderInfo.VoucherAmount);
            this.database.AddInParameter(storedProcCommand, "VoucherValue", DbType.Currency, orderInfo.VoucherValue);
            this.database.AddInParameter(storedProcCommand, "StoreId", DbType.Int32, orderInfo.StoreId);
            this.database.AddInParameter(storedProcCommand, "SupplierId", DbType.Int32, orderInfo.SupplierId);

            if (!string.IsNullOrEmpty(orderInfo.TaobaoOrderId))
            {
                this.database.AddInParameter(storedProcCommand, "TaobaoOrderId", DbType.String, orderInfo.TaobaoOrderId);
            }
            if (orderInfo.GroupBuyId > 0)
            {
                this.database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, orderInfo.GroupBuyId);
                this.database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, orderInfo.NeedPrice);
                this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, DBNull.Value);
            }
            if (orderInfo.CountDownBuyId > 0)
            {
                this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", DbType.Int32, orderInfo.CountDownBuyId);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", DbType.Int32, DBNull.Value);
            }
            if (orderInfo.BundlingID > 0)
            {
                this.database.AddInParameter(storedProcCommand, "BundlingID", DbType.Int32, orderInfo.BundlingID);
                this.database.AddInParameter(storedProcCommand, "BundlingPrice", DbType.Currency, orderInfo.BundlingPrice);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "BundlingID ", DbType.Int32, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "BundlingPrice", DbType.Currency, DBNull.Value);
            }
            this.database.AddInParameter(storedProcCommand, "Tax", DbType.Currency, orderInfo.Tax);
            this.database.AddInParameter(storedProcCommand, "SourceOrder", DbType.Int32, (int)orderInfo.OrderSource);
            this.database.AddInParameter(storedProcCommand, "InvoiceTitle", DbType.String, orderInfo.InvoiceTitle);
            this.database.AddInParameter(storedProcCommand, "SourceOrderId", DbType.String, orderInfo.SourceOrderId);
            this.database.AddInParameter(storedProcCommand, "ActivityType", DbType.Int32, orderInfo.ActivityType);

            return this.database.ExecuteNonQuery(storedProcCommand, dbTran) == 1;
        }

        public bool UpdateOrder(OrderInfo order, DbTransaction dbTran = null)
        {
            string sql = "UPDATE Ecshop_Orders SET  OrderStatus = @OrderStatus, CloseReason=@CloseReason,UpdateDate=getdate(), PayDate = @PayDate, ShippingDate=@ShippingDate, FinishDate = @FinishDate, RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone, ShippingModeId=@ShippingModeId ,ModeName=@ModeName, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, ShipOrderNumber = @ShipOrderNumber,  ExpressCompanyName = @ExpressCompanyName,ExpressCompanyAbb = @ExpressCompanyAbb, PaymentTypeId=@PaymentTypeId,PaymentType=@PaymentType, Gateway = @Gateway, ManagerMark=@ManagerMark,ManagerRemark=@ManagerRemark,IsPrinted=@IsPrinted, OrderTotal = @OrderTotal, OrderProfit=@OrderProfit,Amount=@Amount,OrderCostPrice=@OrderCostPrice, AdjustedFreight = @AdjustedFreight, PayCharge = @PayCharge, AdjustedDiscount=@AdjustedDiscount,OrderPoint=@OrderPoint,GatewayOrderId=@GatewayOrderId,OrderType=@OrderType,IdentityCard=@IdentityCard" + (order.RemarkTime == null ? "" : ",RemarkTime=@RemarkTime") + ",RemarkPeople=@RemarkPeople WHERE OrderId = @OrderId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, (int)order.OrderStatus);
            this.database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, order.CloseReason);
            DateTime arg_51_0 = order.PayDate;
            if (order.RemarkTime != null)
            {
                this.database.AddInParameter(sqlStringCommand, "RemarkTime", DbType.DateTime, order.RemarkTime);
            }
            this.database.AddInParameter(sqlStringCommand, "RemarkPeople", DbType.String, string.IsNullOrWhiteSpace(order.RemarkPeople) ? "" : order.RemarkPeople);
            if (order.PayDate != DateTime.MinValue)
            {
                this.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, order.PayDate);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, DateTime.Now);
            }
            DateTime arg_B0_0 = order.ShippingDate;
            if (order.ShippingDate != DateTime.MinValue)
            {
                this.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, order.ShippingDate);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, DateTime.Now);
            }
            DateTime arg_10F_0 = order.FinishDate;
            if (order.FinishDate != DateTime.MinValue)
            {
                this.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, order.FinishDate);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, DateTime.Now);
            }
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.String, order.RegionId);
            this.database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, order.ShippingRegion);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, order.Address);
            this.database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, order.ZipCode);
            this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, order.ShipTo);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, order.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, order.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, order.ShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, order.ModeName);
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, order.RealShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, order.RealModeName);
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, order.ShipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, order.ExpressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, order.ExpressCompanyAbb);
            this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, order.PaymentTypeId);
            this.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, order.PaymentType);
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, order.Gateway);
            this.database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, order.ManagerMark);
            this.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, order.ManagerRemark);
            this.database.AddInParameter(sqlStringCommand, "IsPrinted", DbType.Boolean, order.IsPrinted);
            this.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Currency, order.GetTotal());
            this.database.AddInParameter(sqlStringCommand, "OrderProfit", DbType.Currency, order.GetProfit());
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, order.GetAmount());
            this.database.AddInParameter(sqlStringCommand, "OrderCostPrice", DbType.Currency, order.GetCostPrice());
            this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Currency, order.AdjustedFreight);
            this.database.AddInParameter(sqlStringCommand, "PayCharge", DbType.Currency, order.PayCharge);
            this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, order.AdjustedDiscount);
            this.database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int32, order.Points);
            this.database.AddInParameter(sqlStringCommand, "GatewayOrderId", DbType.String, order.GatewayOrderId);
            this.database.AddInParameter(sqlStringCommand, "OrderType", DbType.Int32, order.OrderType);
            this.database.AddInParameter(sqlStringCommand, "IdentityCard", DbType.String, order.IdentityCard);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
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
        /// 更改子单的支付信息
        /// </summary>
        /// <param name="order"></param>
        /// <param name="dbTran"></param>
        /// <returns></returns>
        public bool ModifyOrderPaymentType(OrderInfo order,string orderId,DbTransaction dbTran = null)
        {
            string sql = string.Format("UPDATE Ecshop_Orders SET PaymentTypeId=@PaymentTypeId,PaymentType=@PaymentType, Gateway = @Gateway WHERE OrderId in({0})", orderId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, order.PaymentTypeId);
            this.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, order.PaymentType);
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, order.Gateway);
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


        public bool ChangeOrderPaymentType(OrderInfo order, DbTransaction dbTran = null)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_Orders SET UpdateDate=getdate(), PaymentTypeId=@PaymentTypeId, PaymentType=@PaymentType, Gateway = @Gateway WHERE OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, order.PaymentTypeId);
            this.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, order.PaymentType);
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, order.Gateway);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);

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
        /// 合并订单操作，子单修改状态
        /// </summary>
        /// <param name="orderId1">当前支付的订单</param>
        /// <param name="orderId2">和当前支付的订单合并的订单</param>
        /// <returns></returns>
        public bool UpdateChildOrderStatusForMerge(string orderId1, string orderId2)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Ecshop_Orders SET orderstatus=98,ordertype=5,updatedate=getdate() where orderid =@orderId1 or  orderid =@orderId2");
            this.database.AddInParameter(sqlStringCommand, "orderId1", DbType.String, orderId1);
            this.database.AddInParameter(sqlStringCommand, "orderId2", DbType.String, orderId2);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool SetOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Orders SET RealShippingModeId=@RealShippingModeId,RealModeName=@RealModeName WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='ecdev.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, realShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, realModeName);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool SetOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Ecshop_Orders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='ecdev.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, expressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, expressCompanyAbb);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        public bool AddRemark(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO dbo.Ecshop_OrderRemarks(OrderId,Tag,Remark,Operator,RecordTime) VALUES(@OrderId,@Tag,@Remark,@Operator,GETDATE())");
            this.database.AddInParameter(sqlStringCommand, "@OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "@Tag", DbType.Int32, (int)order.ManagerMark);
            this.database.AddInParameter(sqlStringCommand, "@Remark", DbType.String, order.ManagerRemark);
            this.database.AddInParameter(sqlStringCommand, "@Operator", DbType.String, HiContext.Current.User.Username);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        /// <summary>
        /// 根据订单Id获取备注列表
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<OrderRemark> GetOrderRemarks(string orderId)
        {
            List<OrderRemark> result = new List<OrderRemark>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM dbo.Ecshop_OrderRemarks WHERE OrderId=@OrderId ORDER BY RecordTime DESC");
            this.database.AddInParameter(sqlStringCommand, "@OrderId", DbType.String, orderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    result.Add(new OrderRemark
                    {
                        OrderId = orderId,
                        Tag = (int)reader["Tag"],
                        Remark = reader["Remark"].ToString(),
                        Operator = reader["Operator"].ToString(),
                        RecordTime = DateTime.Parse(reader["RecordTime"].ToString())
                    });
                }
            }

            return result;
        }
        /// <summary>
        /// 扣销售库存(在同一个事务)
        /// </summary>
        /// <param name="orderId"></param>
        public bool UpdatePayOrderStock(string orderId, DbTransaction dbTran)
        {
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"Update Ecshop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId);Update Ecshop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM ecshop_orderpresents oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END 
//WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM ecshop_orderpresents Where OrderId =@OrderId)");


            StringBuilder sb = new StringBuilder();
            sb.Append("Update Ecshop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId);");
            sb.Append("Update Ecshop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM ecshop_orderpresents oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM ecshop_orderpresents Where OrderId =@OrderId);");
            //组合商品扣库存
            sb.Append(@"Update Ecshop_SKUs Set Stock = CASE WHEN (Stock-(
select SUM(oi.ShipmentQuantity*pc.Quantity) from Ecshop_ProductsCombination pc inner join Ecshop_OrderItems oi

on oi.SkuId =pc.CombinationSkuId where pc.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0

ELSE Stock - (select SUM(oi.ShipmentQuantity*pc.Quantity) from Ecshop_ProductsCombination pc inner join Ecshop_OrderItems oi

on oi.SkuId =pc.CombinationSkuId where pc.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END 

WHERE Ecshop_SKUs.SkuId  IN (Select pc2.SkuId from Ecshop_ProductsCombination pc2 inner join   Ecshop_OrderItems oi2 on  oi2.SkuId =pc2.CombinationSkuId Where oi2.OrderId =@OrderId);");

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());

            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);

            return this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
        }

        /// <summary>
        /// 扣销售库存
        /// </summary>
        /// <param name="orderId"></param>
        public void UpdatePayOrderStock(string orderId)
        {
//            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"Update Ecshop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId);Update Ecshop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM ecshop_orderpresents oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END 
//WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM ecshop_orderpresents Where OrderId =@OrderId)");


            StringBuilder sb = new StringBuilder();
            sb.Append("Update Ecshop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId);");
            sb.Append("Update Ecshop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM ecshop_orderpresents oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM ecshop_orderpresents Where OrderId =@OrderId);");
            //组合商品扣库存
            sb.Append(@"Update Ecshop_SKUs Set Stock = CASE WHEN (Stock-(
select SUM(oi.ShipmentQuantity*pc.Quantity) from Ecshop_ProductsCombination pc inner join Ecshop_OrderItems oi

on oi.SkuId =pc.CombinationSkuId where pc.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0

ELSE Stock - (select SUM(oi.ShipmentQuantity*pc.Quantity) from Ecshop_ProductsCombination pc inner join Ecshop_OrderItems oi

on oi.SkuId =pc.CombinationSkuId where pc.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END 

WHERE Ecshop_SKUs.SkuId  IN (Select pc2.SkuId from Ecshop_ProductsCombination pc2 inner join   Ecshop_OrderItems oi2 on  oi2.SkuId =pc2.CombinationSkuId Where oi2.OrderId =@OrderId);");

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        /// <summary>
        /// 扣商品实际库存 2015-08-19
        /// </summary>
        /// <param name="orderId"></param>
        public void DebuctFactStock(string orderId)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Ecshop_SKUs Set FactStock = CASE WHEN (FactStock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE FactStock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId);Update Ecshop_SKUs Set FactStock = CASE WHEN (FactStock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE FactStock - (SELECT SUM(oi.ShipmentQuantity) FROM ecshop_orderpresents oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM ecshop_orderpresents Where OrderId =@OrderId)");

            StringBuilder sb = new StringBuilder();
            sb.Append("Update Ecshop_SKUs Set FactStock = CASE WHEN (FactStock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE FactStock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId);");
            sb.Append("Update Ecshop_SKUs Set FactStock = CASE WHEN (FactStock - (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE FactStock - (SELECT SUM(oi.ShipmentQuantity) FROM ecshop_orderpresents oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM ecshop_orderpresents Where OrderId =@OrderId);");
            //组合商品扣实际库存
            sb.Append(@"Update Ecshop_SKUs Set FactStock = CASE WHEN (FactStock-(
select SUM(oi.ShipmentQuantity*pc.Quantity) from Ecshop_ProductsCombination pc inner join Ecshop_OrderItems oi

on oi.SkuId =pc.CombinationSkuId where pc.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0

ELSE FactStock - (select SUM(oi.ShipmentQuantity*pc.Quantity) from Ecshop_ProductsCombination pc inner join Ecshop_OrderItems oi

on oi.SkuId =pc.CombinationSkuId where pc.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END 

WHERE Ecshop_SKUs.SkuId  IN (Select pc2.SkuId from Ecshop_ProductsCombination pc2 inner join   Ecshop_OrderItems oi2 on  oi2.SkuId =pc2.CombinationSkuId Where oi2.OrderId =@OrderId);");

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool UpdateRefundOrderStock(string orderId)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi  Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId);Update Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderPresents oi  Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderPresents Where OrderId =@OrderId)");
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId);");
            sb.Append("Update Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM ecshop_orderpresents oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM ecshop_orderpresents Where OrderId =@OrderId);");
            //组合商品库存
            sb.Append(@"Update Ecshop_SKUs Set Stock =  Stock + (select SUM(oi.ShipmentQuantity*pc.Quantity) from Ecshop_ProductsCombination pc inner join Ecshop_OrderItems oi

on oi.SkuId =pc.CombinationSkuId where pc.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END 

WHERE Ecshop_SKUs.SkuId  IN (Select pc2.SkuId from Ecshop_ProductsCombination pc2 inner join   Ecshop_OrderItems oi2 on  oi2.SkuId =pc2.CombinationSkuId Where oi2.OrderId =@OrderId);");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
        }

        public bool UpdateRefundOrderStock(string orderId, DbTransaction dbTran)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi  Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId);Update Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderPresents oi  Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderPresents Where OrderId =@OrderId)");

            StringBuilder sb = new StringBuilder();
            sb.Append("Update Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId);");
            sb.Append("Update Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM ecshop_orderpresents oi Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM ecshop_orderpresents Where OrderId =@OrderId);");
            //组合商品库存
            sb.Append(@"Update Ecshop_SKUs Set Stock =  Stock + (select SUM(oi.ShipmentQuantity*pc.Quantity) from Ecshop_ProductsCombination pc inner join Ecshop_OrderItems oi

on oi.SkuId =pc.CombinationSkuId where pc.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) END 

WHERE Ecshop_SKUs.SkuId  IN (Select pc2.SkuId from Ecshop_ProductsCombination pc2 inner join   Ecshop_OrderItems oi2 on  oi2.SkuId =pc2.CombinationSkuId Where oi2.OrderId =@OrderId);");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());

            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1;
        }

        public bool CheckRefund(string orderId, string Operator, string adminRemark, int refundType, bool accept)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE Ecshop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            stringBuilder.Append(" update Ecshop_OrderRefund set Operator=@Operator,AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId;");
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
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
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
        public DataSet GetTradeOrders(OrderQuery query, out int records, int delaytimes=30)
        {
            DataSet dataSet = new DataSet();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_API_Orders_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, this.BuildOrdersQuery(query));
            this.database.AddInParameter(storedProcCommand, "delaytimes", DbType.Int32, delaytimes);
            this.database.AddOutParameter(storedProcCommand, "TotalOrders", DbType.Int32, 4);
          
            DataSet dataSet2;
            dataSet = (dataSet2 = this.database.ExecuteDataSet(storedProcCommand));
            try
            {
                dataSet.Relations.Add("OrderRelation", dataSet.Tables[0].Columns["OrderId"], dataSet.Tables[1].Columns["OrderId"]);
            }
            finally
            {
                if (dataSet2 != null)
                {
                    ((IDisposable)dataSet2).Dispose();
                }
            }
            records = (int)this.database.GetParameterValue(storedProcCommand, "TotalOrders");
            return dataSet;
        }
        public DataSet GetTradeOrders(string orderId)
        {
            DataSet dataSet = new DataSet();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new string[]
			{
				"SELECT OrderId,0 as SellerUid,Username,EmailAddress,ShipTo,ShippingRegion,Address,ZipCode,CellPhone,TelPhone,Remark,ManagerMark,ManagerRemark,(select sum(Quantity) from Ecshop_OrderItems where Ecshop_OrderItems.OrderId=p.OrderId) as Nums,OrderTotal,OrderTotal,AdjustedFreight,DiscountValue,AdjustedDiscount,PayDate,ShippingDate,ReFundStatus,OrderStatus FROM Ecshop_Orders as p Where OrderId in (",
				orderId,
				") order by OrderId; SELECT 0 as Tid,OrderId,ProductId,ItemDescription,SKU,SKUContent,Quantity,ItemListPrice,ItemAdjustedPrice,'0.00' as DiscountFee,'0.00' as Fee,'-1' as RefundStatus,'-1' as [Types],'-1' as [Status] FROM Ecshop_OrderItems Where OrderId in (",
				orderId,
				")  order by OrderId"
			}));
            DataSet dataSet2;
            dataSet = (dataSet2 = this.database.ExecuteDataSet(sqlStringCommand));
            try
            {
                dataSet.Relations.Add("OrderDetailsRelation", dataSet.Tables[0].Columns["OrderId"], dataSet.Tables[1].Columns["OrderId"]);
            }
            finally
            {
                if (dataSet2 != null)
                {
                    ((IDisposable)dataSet2).Dispose();
                }
            }
            return dataSet;
        }
        private string BuildOrdersQuery(OrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT OrderId FROM Ecshop_Orders WHERE UserStatus = 0 /*1 = 1*/ ", new object[0]);
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
                if (query.SourceOrder.HasValue)
                {
                    stringBuilder.AppendFormat(" AND SourceOrder={0}", query.SourceOrder.Value);
                }
                if (!string.IsNullOrEmpty(query.UserName))
                {
                    stringBuilder.AppendFormat(" AND  UserName = '{0}' ", DataHelper.CleanSearchString(query.UserName));
                }
                if (query.UserId.HasValue)
                {
                    stringBuilder.AppendFormat(" AND UserId = {0} ", query.UserId.Value);
                }
                if (!string.IsNullOrEmpty(query.SourceOrderId))
                {
                    stringBuilder.AppendFormat(" AND SourceOrderId = '{0}' ", query.SourceOrderId);
                }
                stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", new object[]
					{
						97,
						98,
						99
					});
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
                    if (query.Status != OrderStatus.All)
                    {
                        stringBuilder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
                    }
                }
                if (query.DataType == 1)
                {
                    if (query.StartDate.HasValue)
                    {
                        stringBuilder.AppendFormat(" AND datediff(ss,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                    }
                    if (query.EndDate.HasValue)
                    {
                        stringBuilder.AppendFormat(" AND datediff(ss,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                    }
                }
                else
                {
                    if (query.StartDate.HasValue)
                    {
                        stringBuilder.AppendFormat(" AND datediff(ss,'{0}',UpdateDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                    }
                    if (query.EndDate.HasValue)
                    {
                        stringBuilder.AppendFormat(" AND datediff(ss,'{0}',UpdateDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                    }
                }
                if (query.ShippingModeId.HasValue)
                {
                    stringBuilder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
                }
                if (query.IsPrinted.HasValue)
                {
                    stringBuilder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
                }

                if (query.WaitToComment && query.UserId.HasValue)
                {
                    //stringBuilder.AppendFormat(" and OrderId not in(select  OrderId from  Ecshop_ProductReviews where UserId={0})", query.UserId.Value);
                    stringBuilder.AppendFormat(@"and OrderId in(
                                        select i.OrderId /*, count(*) SkuCount, sum( case when r.ReviewId is null then 0 else 1 end) ReviewCount */
                                        from Ecshop_OrderItems i
	                                        left join Ecshop_ProductReviews r on r.OrderId = i.OrderId and r.SkuId = i.SkuId
                                        where i.OrderId in (select OrderId from Ecshop_Orders where OrderStatus = {1} and UserId = {0}) 
                                        group by i.OrderId having(count(*) > sum( case when r.ReviewId is null then 0 else 1 end))) ",
                                                                                           query.UserId.Value, (int)query.Status);
                }
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return stringBuilder.ToString();
        }

        private string BuildServiceOrdersQuery(ServiceOrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }

            if (query.Status == null)
            {
                throw new ArgumentNullException("query");
            }

            if (query.Status.Count == 0)
            {
                throw new ArgumentNullException("query");
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT OrderId FROM Ecshop_Orders WHERE UserStatus = 0 /*1 = 1*/ ", new object[0]);
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

                if (!string.IsNullOrEmpty(query.UserName))
                {
                    stringBuilder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
                }
                if (query.UserId.HasValue)
                {
                    stringBuilder.AppendFormat(" AND UserId  = '{0}' ", query.UserId.Value);
                }

                if (query.Status.Count > 0)
                {
                    List<int> status = new List<int>();
                    foreach (OrderStatus current in query.Status)
                    {
                        status.Add((int)current);
                    }

                    string orderStatus = string.Join<int>(",", status);

                    stringBuilder.AppendFormat(" AND OrderStatus IN ({0}) ", orderStatus);
                }


            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return stringBuilder.ToString();
        }

        public DataSet GetServiceOrders(ServiceOrderQuery query, out int records)
        {
            DataSet dataSet = new DataSet();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_API_ServiceOrders_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, this.BuildServiceOrdersQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalOrders", DbType.Int32, 4);
            DataSet dataSet2;
            dataSet = (dataSet2 = this.database.ExecuteDataSet(storedProcCommand));
            try
            {
                dataSet.Relations.Add("OrderRelation", dataSet.Tables[0].Columns["OrderId"], dataSet.Tables[1].Columns["OrderId"]);
            }
            finally
            {
                if (dataSet2 != null)
                {
                    ((IDisposable)dataSet2).Dispose();
                }
            }
            records = (int)this.database.GetParameterValue(storedProcCommand, "TotalOrders");
            return dataSet;
        }

        public DataTable GetServiceOrder(string orderId)
        {
            DataTable result;

            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_API_ServiceOrders_GetByOrderId");
            this.database.AddInParameter(storedProcCommand, "OrderId", DbType.String, orderId);

            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }

            return result;
        }

        public DataTable GetYetShipOrders(int days)
        {
            DataTable result;
            if (days <= 0)
            {
                result = null;
            }
            else
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from Ecshop_Orders where OrderStatus=@OrderStatus and ShippingDate>=@FromDate and ShippingDate<=@ToDate;");
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
                this.database.AddInParameter(sqlStringCommand, "FromDate", DbType.DateTime, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays((double)(-(double)days)));
                this.database.AddInParameter(sqlStringCommand, "ToDate", DbType.DateTime, DateTime.Now);
                DataTable dataTable = null;
                using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
                }
                result = dataTable;
            }
            return result;
        }
        public bool CheckIsUnpack(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(1) from Ecshop_Orders as A inner join Ecshop_Orders A1 on A.orderId=A1.SourceOrderId where A.orderId=@orderId");
            this.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
            return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
        }
        public bool UpdateOrderWhereHasChild(string orderId, string GatewayOrderId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_UpdateOrderWhereHasChild");
            this.database.AddInParameter(storedProcCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(storedProcCommand, "GatewayOrderId", DbType.String, GatewayOrderId);
            int ret = this.database.ExecuteNonQuery(storedProcCommand);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }
        public List<OrderInfo> GetChildOrdersBySourceOrder(string sourceOrderId)//未完成
        {
            
            List<OrderInfo> listOrder = new List<OrderInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_Orders Where SourceOrderId = @sourceOrderId");
            this.database.AddInParameter(sqlStringCommand, "sourceOrderId", DbType.String, sourceOrderId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    if (dataReader["OrderId"] != DBNull.Value)
                    {
                        OrderInfo orderInfo = new OrderInfo();
                        orderInfo.OrderId = (string)dataReader["OrderId"];
                        listOrder.Add(orderInfo);
                    }
                }
            }
            return listOrder;
        }
        public DataTable GetOrderItems(string orderId)
        {

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SkuId,ItemDescription,sku,SKUContent,Quantity,isnull(TaxRate,0) TaxRate,ProductId,ThumbnailsUrl,ItemListPrice,ItemAdjustedPrice,Weight FROM Ecshop_OrderItems where OrderId = @OrderId");

            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
            return dataSet.Tables[0];
        }


        public DataTable GetWMSOrderItems(string orderId)
        {

            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT o.*,p.Unit,ISNULL(p.ConversionRelation,1) AS ConversionRelation FROM Ecshop_OrderItems AS o LEFT JOIN Ecshop_SKUs AS s ON s.SkuId=o.SkuId LEFT JOIN Ecshop_Products AS p ON p.ProductId=s.ProductId where p.SupplierId IN (SELECT SupplierId FROM dbo.Ecshop_Supplier) and OrderId = @OrderId");
            /**正常商品**/
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" SELECT o.SkuId AS SkuId,o.Quantity AS Quantity,o.ItemAdjustedPrice AS ItemAdjustedPrice,p.Unit AS Unit,ISNULL(p.ConversionRelation,1) AS ConversionRelation  " +
            " FROM Ecshop_OrderItems AS o  " +
            " JOIN Ecshop_SKUs AS s ON s.SkuId=o.SkuId " +
            " JOIN Ecshop_Products AS p ON p.ProductId=s.ProductId " +
            " where p.SupplierId IN (SELECT SupplierId FROM dbo.Ecshop_Supplier) AND p.SaleType=1 and O.OrderId = @OrderId " +
            " UNION ALL " +
            /**组合商品**/
            " SELECT oc.SkuId AS SkuId,(oc.Quantity*o.ShipmentQuantity) AS Quantity,oc.ItemAdjustedPrice AS ItemAdjustedPrice,p.Unit AS Unit,ISNULL(p.ConversionRelation,1) AS ConversionRelation  " +
            " FROM Ecshop_OrderItems AS o " +
            /**组合明细和商品的关联**/
            " JOIN Ecshop_OrderCombinationItems AS oc ON oc.orderId=o.orderId AND oc.CombinationSkuId=o.skuId " +
            " JOIN Ecshop_SKUs AS s ON s.SkuId=oc.SkuId " +
            " JOIN Ecshop_Products AS p ON p.ProductId=s.ProductId " +
            /**组合主数据和商品关联**/
            " JOIN Ecshop_Products AS op ON op.ProductId=o.ProductId  " +
            " where op.SupplierId IN (SELECT SupplierId FROM dbo.Ecshop_Supplier) AND op.SaleType=2 and O.OrderId = @OrderId");

            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
            return dataSet.Tables[0];
        }

        public string GetOrderIdIsCanMerge(int templateId, decimal tax, int userId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_GetIsCanMergeOrder");
            this.database.AddInParameter(storedProcCommand, "userId", DbType.Int32, userId);
            this.database.AddInParameter(storedProcCommand, "tax", DbType.Decimal, tax);
            this.database.AddInParameter(storedProcCommand, "TemplateId", DbType.Int32, templateId);
            object obj = this.database.ExecuteScalar(storedProcCommand);
            if (obj != null)
            {
                return obj.ToString();
            }
            return "";
        }

        public decimal GetChildOrderTotal(string masterOrderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"SELECT SUM(OrderTotal)  as OrderTotal FROM Ecshop_Orders where SourceOrderId=@orderId");
            this.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, masterOrderId);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj == null || obj is DBNull)
            {
                return 0;
            }
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        /// 根据用户Id获取最新的清关时间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DateTime GetUserLastOrderPCustomsClearanceDate(int userId)
        {
            DateTime PCustomsClearanceDate;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"SELECT top 1 PCustomsClearanceDate FROM Ecshop_Orders where UserId=@userId Order by PCustomsClearanceDate DESC");
            this.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, userId);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj == DBNull.Value || obj == null)
            {
                PCustomsClearanceDate = DateTime.Now.AddDays(1);
            }
            else
            {
                PCustomsClearanceDate = DateTime.Parse(obj.ToString());
                if (PCustomsClearanceDate < DateTime.Now.Date.AddDays(1))
                {
                    PCustomsClearanceDate = DateTime.Now.AddDays(1);
                }
            }
            return PCustomsClearanceDate;
        }

        /// <summary>
        /// 拆单成功后，修改原单金额
        /// </summary>
        /// <param name="orderId">主单ID</param>
        /// <param name="tax">税费</param>
        /// <param name="orderTotal">订单总金额</param>
        /// <param name="unpackedFreight">主单运费</param>
        /// <returns></returns>
        public bool UpdateWillSplitOrder(string orderId, decimal tax, decimal orderTotal, decimal unpackedFreight)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"update Ecshop_Orders SET Tax=@tax,OrderTotal=@OrderTotal,Freight=@Freight,AdjustedFreight=@AdjustedFreight,SupplierId=0
where OrderId=@orderId");
            this.database.AddInParameter(sqlStringCommand, "tax", DbType.Decimal, tax);
            this.database.AddInParameter(sqlStringCommand, "orderTotal", DbType.Decimal, orderTotal);
            this.database.AddInParameter(sqlStringCommand, "Freight", DbType.Decimal, unpackedFreight);
            this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Decimal, unpackedFreight);
            this.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
            int ret = (int)this.database.ExecuteNonQuery(sqlStringCommand);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 拆单成功后，修改原单金额
        /// </summary>
        /// <param name="orderId">原单订单OrderId</param>
        /// <param name="tax">税费</param>
        /// <param name="orderTotal">订单总额</param>
        /// <returns></returns>
        public bool UpdateWillSplitNewOrder(string orderId, decimal tax, decimal orderTotal)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"update Ecshop_Orders SET Tax=@tax,OrderTotal=@OrderTotal where OrderId=@orderId");
            this.database.AddInParameter(sqlStringCommand, "tax", DbType.Decimal, tax);
            this.database.AddInParameter(sqlStringCommand, "orderTotal", DbType.Decimal, orderTotal);
            this.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
            int ret = (int)this.database.ExecuteNonQuery(sqlStringCommand);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }

        public bool AllocateOrderToStore(string orderIds, int storeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format(@"update Ecshop_Orders SET storeId=@storeId
where OrderId in ( {0} )", orderIds));
            this.database.AddInParameter(sqlStringCommand, "storeId", DbType.Int32, storeId);
            int ret = this.database.ExecuteNonQuery(sqlStringCommand);
            if (ret > 0)
            {
                return true;
            }
            return false;

        }
        public bool UpdateOrderPayStatus(string orderId, int payStatus)
        {
            string sql = "update Ecshop_Orders SET payStatus=@payStatus";
            if (payStatus == 2)
            {
                sql += ",PayDate=getdate()";
            }
            sql += "  where OrderId=@orderId";

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "payStatus", DbType.Int32, payStatus);

            if (payStatus == 2)
            {
                this.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, DateTime.Now.Date);
            }

            int ret = this.database.ExecuteNonQuery(sqlStringCommand);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }

        public bool UpdateOrderPayStatus(string orderId, int payStatus, int paymentTypeId, string paymentTypeName, string gateway, string gatewayOrderId)
        {
            string sql = "update Ecshop_Orders SET payStatus=@payStatus";
            if (payStatus == 2)
            {
                sql += ",PayDate=getdate(),PaymentTypeId=@PaymentTypeId,PaymentType=@PaymentType,Gateway=@Gateway,GatewayOrderId=@GatewayOrderId";
            }
            sql += "  where OrderId=@orderId";

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "payStatus", DbType.Int32, payStatus);

            if (payStatus == 2)
            {
                //this.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, DateTime.Now);
                this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, paymentTypeId);
                this.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, paymentTypeName);
                this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, gateway);
                this.database.AddInParameter(sqlStringCommand, "GatewayOrderId", DbType.String, gatewayOrderId);
            }

            int ret = this.database.ExecuteNonQuery(sqlStringCommand);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }

        public bool UpdateOrderPayStatus(string orderId, int orderStatus, int payStatus, int paymentTypeId, string paymentTypeName, string gateway, string gatewayOrderId)
        {
            string sql = "update Ecshop_Orders SET OrderStatus = @OrderStatus, payStatus=@payStatus";
            if (payStatus == 2)
            {
                sql += ",PayDate=getdate(),PaymentTypeId=@PaymentTypeId,PaymentType=@PaymentType,Gateway=@Gateway,GatewayOrderId=@GatewayOrderId";
            }
            sql += "  where OrderId=@orderId";

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "payStatus", DbType.Int32, payStatus);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, orderStatus);

            if (payStatus == 2)
            {
                //this.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, DateTime.Now);
                this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, paymentTypeId);
                this.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, paymentTypeName);
                this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, gateway);
                this.database.AddInParameter(sqlStringCommand, "GatewayOrderId", DbType.String, gatewayOrderId);
            }

            int ret = this.database.ExecuteNonQuery(sqlStringCommand);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }

        public bool UpdateOrderStatus(string orderId, int orderStatus)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"update Ecshop_Orders SET OrderStatus=@orderStatus where OrderId=@orderId and OrderStatus = 4");
            this.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "orderStatus", DbType.Int32, orderStatus);
            int ret = this.database.ExecuteNonQuery(sqlStringCommand);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 微信扫码支付完成时调用 只有订单状态为等待买家付款时才能修改
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="orderStatus">支付完成是调用状态为2 默认为2</param>
        /// <returns></returns>
        public bool UpdateOrderStatus(string orderId, string transaction_id, int orderStatus = 2)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"update Ecshop_Orders SET orderStatus=@orderStatus,GatewayOrderId=@GatewayOrderId,PayStatus=@PayStatus,PayDate=getdate()
where OrderId=@orderId and orderStatus=1");
            this.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "GatewayOrderId", DbType.String, transaction_id);
            this.database.AddInParameter(sqlStringCommand, "PayStatus", DbType.Int32, 2);
            this.database.AddInParameter(sqlStringCommand, "orderStatus", DbType.Int32, orderStatus);
            int ret = this.database.ExecuteNonQuery(sqlStringCommand);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }
        public DataTable GetNoWriteBackOrders()
        {
            DataTable dt = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"SELECT A.MicroOrderId as OrderId,A.TradingTime as PayDate,A.MerchantOrderId as GatewayOrderId from dbo.Ecshop_TradeDetails as A where A.TradingStatus='SUCCESS' AND EXISTS (select orderid from dbo.Ecshop_Orders AS A1  where OrderStatus=1 AND PayStatus=1 AND OrderId=A.MerchantOrderId)");
            DataSet dsOrders = this.database.ExecuteDataSet(sqlStringCommand);
            if (dsOrders != null && dsOrders.Tables.Count > 0)
            {
                dt = dsOrders.Tables[0];
            }
            return dt;
        }
        public IList<string> GetPayingOrder()
        {
            IList<string> list = new List<string>();
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OrderId from dbo.Ecshop_Orders where  OrderStatus=1 AND PayStatus=1");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    if (System.DBNull.Value != dataReader["OrderId"])
                    {
                        list.Add(dataReader["OrderId"].ToString());
                    }
                }
            }
            return list;
        }
        public IDictionary<string, string> GetWxPayingOrder()
        {
            IDictionary<string, string> list = new Dictionary<string, string>();
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OrderId, Gateway from dbo.Ecshop_Orders where OrderStatus=1 and PayStatus=1 and Gateway in ('Ecdev.plugins.payment.WxpayQrCode.QrCodeRequest', 'Ecdev.plugins.payment.wx_apppay.wxwappayrequest', 'Ecdev.plugins.payment.weixinrequest');");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    string orderId = "";
                    if (System.DBNull.Value != dataReader["OrderId"])
                    {
                        orderId = dataReader["OrderId"].ToString();
                    }

                    string gateway = "";
                    if (System.DBNull.Value != dataReader["Gateway"])
                    {
                        gateway = dataReader["Gateway"].ToString();
                    }

                    list.Add(orderId, gateway);
                }
            }
            return list;
        }
        public IList<OrderInfo> GetOrderList(string date, int top)
        {
            IList<OrderInfo> list = new List<OrderInfo>();
            DbCommand sqlStringCommand;

            sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT TOP({0}) UserId,EmailAddress,UserName,FinishDate,Amount,OrderId,SourceOrderId  FROM Ecshop_Orders WHERE FinishDate>@FinishDate ORDER BY FinishDate", top));
            this.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, date);

            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    OrderInfo info = new OrderInfo();
                    info.UserId = (int)dataReader["UserId"];
                    info.EmailAddress = dataReader["EmailAddress"].ToString();
                    info.Username = dataReader["UserName"].ToString();
                    info.FinishDate = Convert.ToDateTime(dataReader["FinishDate"].ToString());
                    info.Amount = Convert.ToDecimal(dataReader["Amount"].ToString());
                    info.OrderId = dataReader["OrderId"].ToString();
                    info.SourceOrderId = dataReader["SourceOrderId"].ToString();
                    list.Add(info);

                }
            }
            return list;
        }


        public decimal GetOrderAmount(string orderId)
        {
            decimal amount = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 1 Amount  FROM Ecshop_Orders WHERE OrderId=@OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            try
            {
                amount = Convert.ToDecimal(this.database.ExecuteScalar(sqlStringCommand).ToString());
            }

            catch
            {
                amount = 0;
            }

            return amount;
        }

        /// <summary>
        /// 判断该用户是否为首单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int CheckIsFirstOrder(int userId, int sourceOrder)
        {
            int result = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select COUNT(*) from Ecshop_Orders where SourceOrder=@SourceOrder and UserId =@UserId and OrderStatus in(2,3,5,6,7,8,9,10)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "SourceOrder", DbType.Int32, sourceOrder);
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

        public void CheckIsFirstOrder(int userId, int orderSource, string orderId, out int result)
        {
            result = 0;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("CheckIsFirstOrder");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(storedProcCommand, "OrderSource", DbType.Int32, orderSource);
            this.database.AddInParameter(storedProcCommand, "OrderId", DbType.String, orderId);
            this.database.AddOutParameter(storedProcCommand, "Result", DbType.Int32, 4);

            this.database.ExecuteNonQuery(storedProcCommand);

            int.TryParse(this.database.GetParameterValue(storedProcCommand, "Result").ToString(), out result);
        }

        #region Order Push

        public bool InsertOrderPush(string pushPlatform, string orderId, int status, string message)
        {
            DbCommand command = this.database.GetSqlStringCommand("INSERT INTO Ecshop_OrderPush(PushPlatform, OrderId, PushStatus, PushMessage, PushDate) VALUES(@PushPlatform, @OrderId, @PushStatus, @PushMessage, @PushDate);");

            this.database.AddInParameter(command, "PushStatus", DbType.Int32, status);
            this.database.AddInParameter(command, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(command, "PushPlatform", DbType.String, pushPlatform);
            this.database.AddInParameter(command, "PushMessage", DbType.String, message);
            this.database.AddInParameter(command, "PushDate", DbType.DateTime, DateTime.Now);

            return this.database.ExecuteNonQuery(command) > 0;
        }

        public DataTable GetOrderPush(string pushPlatform, int status)
        {
            DbCommand command = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_OrderPush WHERE PushPlatform = @PushPlatform AND PushPlatform = @PushStatus;");

            this.database.AddInParameter(command, "PushStatus", DbType.Int32, status);
            this.database.AddInParameter(command, "PushPlatform", DbType.String, pushPlatform);

            DataTable dataTable = null;
            using (IDataReader dataReader = this.database.ExecuteReader(command))
            {
                dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
            }

            return dataTable;
        }

        public DbQueryResult GetPushOrders(string pushPlatform, int? status, int? expressStatus, int pageIndex, int pageSize)
        {
            OrderPushQuery query = new OrderPushQuery();
            if (status.HasValue)
            {
                query.PushStatus = status;
            }
            if (expressStatus.HasValue)
            {
                query.ExpressStatus = expressStatus;
            }
            query.PageIndex = pageIndex;
            query.PageSize = pageSize;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(" PushPlatform = '{0}' ", pushPlatform);

            if (query.PushStatus.HasValue)
            {
                stringBuilder.AppendFormat(" AND PushStatus = {0}", (int)query.PushStatus);
            }

            if (query.ExpressStatus.HasValue)
            {
                stringBuilder.AppendFormat(" AND ExpressStatus = {0}", (int)query.ExpressStatus);
            }

            string selectFields = "Id, PushPlatform, OrderId, PushStatus, ExpressStatus ";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Ecshop_OrderPush p", "Id", stringBuilder.ToString(), selectFields);
        }

        #endregion

        public bool UpdatePushOrders(string pushPlatform, Dictionary<string, string> orders)
        {
            StringBuilder sbSQL = new StringBuilder();

            return false;
        }



        /// <summary>
        /// 是否已经推送到WMS
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="isSendWMS">默认0，已推送1，只有推送WMS时会使用，其他业务不使用</param>
        /// <returns></returns>
        public bool UpdateOrderWMSStatus(string orderId, int isSendWMS)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"update Ecshop_Orders SET IsSendWMS=@IsSendWMS,SendWMSCount=ISNULL(SendWMSCount,0)+1 where OrderId=@orderId ");
            this.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "IsSendWMS", DbType.Int32, isSendWMS);
            int ret = this.database.ExecuteNonQuery(sqlStringCommand);
            if (ret > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="userOrder"></param>
        /// <returns></returns>
        public OrdersInfo GetWMSUserOrders(OrderQuery query)
        {
            //
            StringBuilder sbWhere = new StringBuilder();

            sbWhere.Append(" SELECT o.OrderId FROM Ecshop_Orders AS o ");

            sbWhere.Append(" INNER JOIN HS_Docking AS h ON o.orderId=h.orderId ");

            sbWhere.Append(" WHERE 1 = 1 ");

            sbWhere.Append(" and o.OrderStatus=2 and (o.IsSendWMS=0 OR o.IsSendWMS IS NULL) ");

            sbWhere.Append(" and ISNULL(payerIdStatus,0) =2 ");//实名认证通过

            sbWhere.Append(" and ISNULL(PaymentStatus,0)=2 ");//支付单上传成功


            if (query.SendWMSCount.HasValue)
            {
                sbWhere.AppendFormat(" and ISNULL(o.SendWMSCount,0)<={0} ", query.SendWMSCount.Value);
            }

            if (query.DateContrastType.HasValue)
            {
                if (query.DateContrastType.Value == 1)
                {
                    if (query.DateContrastValue.HasValue)
                    {
                        sbWhere.AppendFormat(" and  o.PayDate BETWEEN DATEADD(hour,-{0},GETDATE())  AND GETDATE() ", query.DateContrastValue.Value);
                    }
                }
                else if (query.DateContrastType.Value == 2)
                {
                    if (query.DateContrastValue.HasValue)
                    {
                        sbWhere.AppendFormat(" and o.PayDate BETWEEN DATEADD(day,-{0},GETDATE())  AND GETDATE() ", query.DateContrastValue.Value);
                    }
                }
                else if (query.DateContrastType.Value == 2)
                {
                    if (query.DateContrastValue.HasValue)
                    {
                        sbWhere.AppendFormat(" and o.PayDate BETWEEN DATEADD(month,-{0},GETDATE())  AND GETDATE() ", query.DateContrastValue.Value);
                    }
                }

            }




            OrdersInfo ordersInfo = new OrdersInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_GetOrders");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, sbWhere.ToString());
            this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                ordersInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
                if (dataReader.NextResult())
                {
                    dataReader.Read();
                    if (dataReader["OrderTotal"] != DBNull.Value)
                    {
                        ordersInfo.TotalOfPage += (decimal)dataReader["OrderTotal"];
                    }
                    if (dataReader["Profits"] != DBNull.Value)
                    {
                        ordersInfo.ProfitsOfPage += (decimal)dataReader["Profits"];
                    }
                }
                if (dataReader.NextResult())
                {
                    dataReader.Read();
                    if (dataReader["OrderTotal"] != DBNull.Value)
                    {
                        ordersInfo.TotalOfSearch += (decimal)dataReader["OrderTotal"];
                    }
                    if (dataReader["Profits"] != DBNull.Value)
                    {
                        ordersInfo.ProfitsOfSearch += (decimal)dataReader["Profits"];
                    }
                }
            }
            ordersInfo.TotalCount = (int)this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
            return ordersInfo;
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="userOrder"></param>
        /// <returns></returns>
        public OrdersInfo GetUserOrders(OrderQuery userOrder)
        {
            OrdersInfo ordersInfo = new OrdersInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_GetOrders");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, userOrder.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, userOrder.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, userOrder.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUserOrderQuery(userOrder));
            this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);
            using (IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
            {
                ordersInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
                if (dataReader.NextResult())
                {
                    dataReader.Read();
                    if (dataReader["OrderTotal"] != DBNull.Value)
                    {
                        ordersInfo.TotalOfPage += (decimal)dataReader["OrderTotal"];
                    }
                    if (dataReader["Profits"] != DBNull.Value)
                    {
                        ordersInfo.ProfitsOfPage += (decimal)dataReader["Profits"];
                    }
                }
                if (dataReader.NextResult())
                {
                    dataReader.Read();
                    if (dataReader["OrderTotal"] != DBNull.Value)
                    {
                        ordersInfo.TotalOfSearch += (decimal)dataReader["OrderTotal"];
                    }
                    if (dataReader["Profits"] != DBNull.Value)
                    {
                        ordersInfo.ProfitsOfSearch += (decimal)dataReader["Profits"];
                    }
                }
            }
            ordersInfo.TotalCount = (int)this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
            return ordersInfo;
        }

        /// <summary>
        /// 过滤订单查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static string BuildUserOrderQuery(OrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("SELECT O.OrderId AS OrderId FROM Ecshop_Orders AS O LEFT JOIN HS_Docking AS H ON O.orderId=H.orderId WHERE 1 = 1 ", new object[0]);
            GetOrderQuery(query, sbWhere);
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                sbWhere.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return sbWhere.ToString();
        }

        private static StringBuilder GetOrderQuery(OrderQuery query, StringBuilder sbWhere)
        {
            if (query.Type.HasValue)
            {
                if (query.Type.Value == OrderQuery.OrderType.GroupBuy)
                {
                    sbWhere.Append(" And O.GroupBuyId > 0 ");
                }
                else
                {
                    sbWhere.Append(" And O.GroupBuyId is null ");
                }
            }
            if (query.OrderId != string.Empty && query.OrderId != null)
            {
                sbWhere.AppendFormat(" AND O.OrderId LIKE '{0}%'", DataHelper.CleanSearchString(query.OrderId));
                if (query.SupplierId.HasValue && query.SupplierId != 0 && query.IsSupplierManager)
                {
                    sbWhere.AppendFormat(" AND O.OrderId IN (SELECT OrderId FROM Ecshop_OrderItems WHERE SupplierId ={0})", query.SupplierId);
                }
            }
            //else
            //{
            if (query.UserId.HasValue)
            {
                sbWhere.AppendFormat(" AND O.UserId = '{0}'", query.UserId.Value);
            }
            if (query.PaymentType.HasValue)
            {
                sbWhere.AppendFormat(" AND O.PaymentTypeId = '{0}'", query.PaymentType.Value);
            }
            if (query.GroupBuyId.HasValue)
            {
                sbWhere.AppendFormat(" AND O.GroupBuyId = {0}", query.GroupBuyId.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                sbWhere.AppendFormat(" AND O.OrderId IN (SELECT OrderId FROM Ecshop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
            }
            if (query.SupplierId.HasValue && query.SupplierId != 0)
            {
                sbWhere.AppendFormat(" AND O.OrderId IN (SELECT OrderId FROM Ecshop_OrderItems WHERE SupplierId ={0})", query.SupplierId);
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                sbWhere.AppendFormat(" AND O.ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
            }

            if (!string.IsNullOrEmpty(query.ShipOrderNumber))
            {
                sbWhere.AppendFormat(" AND  O.ShipOrderNumber  = '{0}' ", DataHelper.CleanSearchString(query.ShipOrderNumber));
            }

            if (!string.IsNullOrEmpty(query.CellPhone))
            {
                sbWhere.AppendFormat(" AND O.CellPhone LIKE '%{0}%'", DataHelper.CleanSearchString(query.CellPhone));
            }
            if (query.RegionId.HasValue)
            {
                sbWhere.AppendFormat(" AND O.ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                sbWhere.AppendFormat(" AND  O.UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
            }
            switch (query.IsRefund)
            {
                case 1:
                case 2:
                    sbWhere.AppendFormat(" And O.IsRefund={0}", query.IsRefund);
                 break;
                case 3:
                    sbWhere.AppendFormat("and isnull(IsRefund,0)=0");
                 break;
            }
            //订单时间类型  1:下单时间(OrderDate) 2:付款时间（PayDate） 3:发货时间(ShippingDate)
            //选择了付款时间，只能查询 OrderStatus >=2
            //选择发货时间, 只能查询 OrderStatus >=3  
            //发货后，退款状态就不能查.
            /*所有订单    AND OrderStatus<>98 
            等待买家发货  AND OrderStatus =1
            等待发货      AND (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'ecdev.plugins.payment.podrequest')
            已发货        AND OrderStatus =3
            成功订单      AND OrderStatus =5
            已关闭        AND OrderStatus =4 x
            历史订单      AND OrderStatus != 1 AND OrderStatus != 4 AND OrderStatus != 9 AND OrderDate < '2015/4/3 18:21:42 
            All,    0  
            WaitBuyerPay,1 
            BuyerAlreadyPaid, 2 
            SellerAlreadySent,3
            Closed,           4
            Finished,         5 
            ApplyForRefund,   6
            ApplyForReturns,  7
            ApplyForReplacement, 8
            Refunded,            9
            Returned,            10 
            UnpackOrMixed= 98,//拆分或者已经合并的原单
            History = 99
             */
            switch (query.OrderTimeType)
            {
                case 1:
                    if (query.StartDate.HasValue)
                    {
                        sbWhere.AppendFormat(" AND O.OrderDate>='{0}'", query.StartDate.Value.ToString());
                    }
                    if (query.EndDate.HasValue)
                    {
                        sbWhere.AppendFormat(" AND O.OrderDate<='{0}'", query.EndDate.Value.ToString());
                    }

                    //if (query.StartDate.HasValue)
                    //{
                    //    sbWhere.AppendFormat(" AND datediff(HOUR,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                    //}
                    //if (query.EndDate.HasValue)
                    //{
                    //    sbWhere.AppendFormat(" AND datediff(HOUR,'{0}',OrderDate)<0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                    //}

                    if (query.Status == OrderStatus.History)
                    {
                        sbWhere.AppendFormat(" AND O.OrderStatus != {0} AND O.OrderStatus != {1} AND O.OrderStatus != {2} AND O.OrderDate < '{3}'", new object[] { 1, 4, 9, DateTime.Now.AddMonths(-3) });
                    }
                    else
                    {
                        if (query.Status == OrderStatus.BuyerAlreadyPaid)
                        {
                            sbWhere.AppendFormat(" AND (O.OrderStatus = {0} OR (O.OrderStatus = 1 AND O.Gateway = 'ecdev.plugins.payment.podrequest'))", (int)query.Status);
                        }
                        else
                        {
                            if (query.Status != OrderStatus.All)
                            {
                                sbWhere.AppendFormat(" AND O.OrderStatus = {0}", (int)query.Status);
                            }
                            else
                            {
                                sbWhere.Append(" AND O.OrderStatus<>98");
                            }
                        }
                    }
                    break;
                case 2:
                    if (query.StartDate.HasValue)
                    {
                        sbWhere.AppendFormat(" AND O.PayDate>='{0}'", query.StartDate.Value.ToString());
                    }
                    if (query.EndDate.HasValue)
                    {
                        sbWhere.AppendFormat(" AND O.PayDate<='{0}'", query.EndDate.Value.ToString());
                    }

                    //选择付款时间，历史订单，付款时间为三个月之前的。
                    if (query.Status == OrderStatus.History)
                    {
                        sbWhere.AppendFormat(" AND O.PayDate < '{3}'", new object[] { DateTime.Now.AddMonths(-3) });
                    }
                    else if (query.Status == OrderStatus.All)
                    {
                        //所有订单,排除等待发货
                        sbWhere.AppendFormat(" AND O.OrderStatus >= {0}", (int)OrderStatus.BuyerAlreadyPaid);
                    }
                    else
                    {
                        //订单状态等于当前状态
                        sbWhere.AppendFormat(" AND O.OrderStatus = {0}", (int)query.Status);
                    }

                    //排除已关闭、已退款、等待买家付款
                    sbWhere.AppendFormat(" And O.OrderStatus<> {0} AND O.OrderStatus<>{1} AND O.OrderStatus<>{2}", (int)OrderStatus.Closed, (int)OrderStatus.Refunded, (int)OrderStatus.WaitBuyerPay);

                    break;
                case 3:
                    if (query.StartDate.HasValue)
                    {
                        sbWhere.AppendFormat(" AND O.ShippingDate>='{0}'", query.StartDate.Value.ToString());
                    }
                    if (query.EndDate.HasValue)
                    {
                        sbWhere.AppendFormat(" AND O.ShippingDate<='{0}'", query.EndDate.Value.ToString());
                    }

                    //选择发货时间，历史订单，排除等待买家付款、已关闭、已退款的，发货时间为三个月之前的。
                    if (query.Status == OrderStatus.History)
                    {
                        sbWhere.AppendFormat(" AND O.ShippingDate < '{3}'", new object[] { DateTime.Now.AddMonths(-3) });
                    }
                    else if (query.Status == OrderStatus.All)
                    {
                        //所有订单，排除等待买家付款、等待发货、已关闭、历史订单、已退款、申请退款的
                        sbWhere.AppendFormat(" AND O.OrderStatus <> {0} And O.OrderStatus<> {1} AND O.OrderStatus<>{2}", (int)OrderStatus.BuyerAlreadyPaid, (int)OrderStatus.UnpackOrMixed, (int)OrderStatus.ApplyForRefund);
                    }
                    else
                    {
                        //订单状态等于当前状态，并且排除已关闭、已退款、等待发货、等待买家付款 
                        sbWhere.AppendFormat(" AND O.OrderStatus = {0} And O.OrderStatus<> {1}", (int)query.Status, (int)OrderStatus.BuyerAlreadyPaid);
                    }

                    //订单状态等于当前状态，并且排除已关闭、已退款、等待发货、等待买家付款 
                    sbWhere.AppendFormat(" AND O.OrderStatus = {0} And O.OrderStatus<> {1} AND O.OrderStatus<>{2}", (int)OrderStatus.Closed, (int)OrderStatus.Refunded, (int)OrderStatus.WaitBuyerPay);

                    break;
            }



            if (query.ShippingModeId.HasValue)
            {
                sbWhere.AppendFormat(" AND O.ShippingModeId = {0}", query.ShippingModeId.Value);
            }
            if (query.IsPrinted.HasValue)
            {
                sbWhere.AppendFormat(" AND ISNULL(O.IsPrinted, 0)={0}", query.IsPrinted.Value);
            }
            if (query.SourceOrder.HasValue)
            {
                sbWhere.AppendFormat(" And O.SourceOrder = {0}", query.SourceOrder.Value);
            }


            if (query.PayerIdStatus.HasValue)
            {
                sbWhere.AppendFormat(" And H.PayerIdStatus = {0}", query.PayerIdStatus.Value);
            }


            if (query.IsStore == true)
            {
                sbWhere.Append(" and  ISNULL(O.StoreId,0)>0");
            }
            if (query.StoreId.HasValue)
            {
                if (query.StoreId.Value != 0)
                {
                    sbWhere.AppendFormat(" and  O.StoreId={0}", query.StoreId.Value);
                }
            }
            if (query.SiteId.HasValue)
            {
                sbWhere.AppendFormat(" and O.SiteId={0}", query.SiteId.Value);
            }

            //}
            return sbWhere;
        }

        public List<string> GetOrderHandleReason(OrderHandleReasonType type)
        {
            List<string> result = new List<string>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from dbo.Ecshop_OrderHandleReason where Type=@Type");
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.String, type);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    string reason = dataReader["Name"].ToString();
                    if (string.IsNullOrEmpty(reason))
                    {
                        continue;
                    }
                    result.Add(reason);
                }
            }
            return result;
        }

        /// <summary>
        /// 修改未发货订单货号。
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="skuId"></param>
        /// <param name="productId"></param>
        /// <param name="newSku"></param>
        /// <returns></returns>
        public bool UpdateOrderItemSku(string orderId, string skuId, int productId, string newSku)
        {
            int orderStatus = GetOrderStatus(orderId);
            if (orderStatus != 2)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE dbo.Ecshop_OrderItems SET SKU=@SKU WHERE OrderId=@OrderId AND SkuId=@SkuId AND ProductId=@ProductId;");
            this.database.AddInParameter(sqlStringCommand, "@SKU", DbType.String, newSku);
            this.database.AddInParameter(sqlStringCommand, "@OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "@SkuId", DbType.String, skuId);
            this.database.AddInParameter(sqlStringCommand, "@ProductId", DbType.Int32, productId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 修改未发货订单供货商。
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="skuId"></param>
        /// <param name="productId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public bool UpdateOrderItemSupplier(string orderId, string skuId, int productId, int supplierId)
        {
            int orderStatus = GetOrderStatus(orderId);
            if (orderStatus != 2)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE dbo.Ecshop_OrderItems SET SupplierId=@SupplierId WHERE OrderId=@OrderId AND SkuId=@SkuId AND ProductId=@ProductId;");
            this.database.AddInParameter(sqlStringCommand, "@SupplierId", DbType.Int32, supplierId);
            this.database.AddInParameter(sqlStringCommand, "@OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "@SkuId", DbType.String, skuId);
            this.database.AddInParameter(sqlStringCommand, "@ProductId", DbType.Int32, productId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public int GetOrderStatus(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OrderStatus from dbo.Ecshop_Orders where OrderId=@OrderId");
            this.database.AddInParameter(sqlStringCommand, "@OrderId", DbType.String, orderId);
            object obj = this.database.ExecuteScalar(sqlStringCommand);
            if (obj == null)
            {
                return -1;
            }
            return (int)obj;
        }

        /// <summary>
        /// 锁定取消退款
        /// </summary>
        /// <param name="SoureCode">来源单号</param>
        /// <param name="OrderId">订单号</param>
        /// <param name="OrderStatus">订单状态（是否拆单）</param>
        /// <returns></returns>
        public bool ChangeRefundType(string OrderId)
        {
            try
            {
                string sql = "update [Ecshop_Orders]  set IsCancelOrder=1,IsRefund=2,OrderStatus=6 where OrderId=@OrderId";
                DbCommand command = this.database.GetSqlStringCommand(sql);
                this.database.AddInParameter(command, "@OrderId", DbType.String, OrderId);
  
                bool result = this.database.ExecuteNonQuery(command) > 0;
                return result;
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        #region  Api
        /// <summary>
        /// 锁定取消退款
        /// </summary>
        /// <param name="SoureCode">来源单号</param>
        /// <param name="OrderId">订单号</param>
        /// <param name="OrderStatus">订单状态（是否拆单）</param>
        /// <returns></returns>
        public bool ChangeRefundType(string OrderId,int type)
        {
            try
            {
                DbCommand command;
                if (type == 1) //子单
                {
                    string sql = "update [Ecshop_Orders]  set IsCancelOrder=1,IsRefund=2,OrderStatus=6,Refundtime=getdate() where OrderId=@OrderId";
                    command = this.database.GetSqlStringCommand(sql);
                    this.database.AddInParameter(command, "@OrderId", DbType.String, OrderId);
                }
                else  //母单
                {
                    string sql = "update [Ecshop_Orders]  set IsCancelOrder=1,IsRefund=2,OrderStatus=6,Refundtime=getdate() where SourceOrderId=@OrderId";
                    command = this.database.GetSqlStringCommand(sql);
                    this.database.AddInParameter(command, "@OrderId", DbType.String, OrderId);
                }

                bool result = this.database.ExecuteNonQuery(command) > 0;
                return result;
            }
            catch (Exception ee)
            {
                return false;
            }
        }


       /// <summary>
       /// 检查订单是否可申请退款
       /// </summary>
       /// <param name="OrderId"></param>
       /// <param name="type"></param>
       /// <returns></returns>
        public bool CheckOrderIsRefund(string OrderId, int type, int delaytime)
        {
            try
            {
                int rest = 0;
                DbCommand command;
                if (type == 1)
                {
                    string sql = @"select count(1) from  Ecshop_Orders where PayDate>DATEADD(n,-"+delaytime+@",getdate())
                                  and OrderId =@OrderId and OrderStatus=2 and isnull(IsRefund,0)!=1";
                    command = this.database.GetSqlStringCommand(sql);
                    this.database.AddInParameter(command, "@OrderId", DbType.String, OrderId);
                    object result = this.database.ExecuteScalar(command);
                    int.TryParse(result.ToString(), out rest);
                    return rest > 0 ? true : false;
                }
                else
                {
                    string sql = @"select count(1) from  Ecshop_Orders where PayDate>DATEADD(n,-"+delaytime+@",getdate())
                                  and SourceOrderId=@SourceOrderId and OrderStatus=2 and isnull(IsRefund,0)!=1";
                    command = this.database.GetSqlStringCommand(sql);
                    this.database.AddInParameter(command, "@SourceOrderId", DbType.String, OrderId);
                    object result = this.database.ExecuteScalar(command);
                    int.TryParse(result.ToString(), out rest);
                    return rest > 0 ? true : false;
                }
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        #endregion

        /// <summary>
       /// 退款成功，修改退款状态
       /// </summary>
       /// <param name="SoureCode"></param>
       /// <param name="OrderId"></param>
       /// <param name="OrderStatus"></param>
       /// <returns></returns>
        public bool RefundSuccess(string OrderId,int type)
        {
            try
            {
                string sql = string.Empty;
                if (type == 1)
                {
                    sql = "update [Ecshop_Orders]  set IsRefund=@type,OrderStatus=9 where OrderId=@OrderId";
                }
                else
                {
                    sql = "update [Ecshop_Orders]  set IsRefund=@type,OrderStatus=9 where OrderId=@OrderId";
                }
                DbCommand command = this.database.GetSqlStringCommand(sql);
                this.database.AddInParameter(command, "@OrderId", DbType.String, OrderId);
                this.database.AddInParameter(command, "@type", DbType.Int32, type);
                bool result = this.database.ExecuteNonQuery(command) > 0;
                return result;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        /// <summary>
        /// 关闭订单 Added by zhangjunlin
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="closeType"></param>
        /// <returns></returns>
        public bool CloseOrder(string orderId, CloseOrderType closeType)
        {
            const string ManuallyCloseReason = "用户取消订单";
            const string AutoCloseReason = "过期未付款，自动关闭";
            using (DbConnection connection = this.database.CreateConnection())
            {
                DbCommand command = this.database.GetSqlStringCommand("UPDATE dbo.Ecshop_Orders SET OrderStatus = @OrderStatus,CloseReason=@CloseReason WHERE OrderId=@OrderId and OrderStatus=1 ");
                this.database.AddInParameter(command, "@OrderStatus", DbType.Int32, (int)OrderStatus.Closed);
                this.database.AddInParameter(command, "@CloseReason", DbType.String, closeType == CloseOrderType.Auto ? AutoCloseReason : ManuallyCloseReason);
                this.database.AddInParameter(command, "@OrderId", DbType.String, orderId);
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    //更新订单状态
                    bool result = this.database.ExecuteNonQuery(command, transaction) > 0;
                    if (!result)
                    {
                        transaction.Rollback();
                        return result;
                    }

                    command.Parameters.Clear();
                    this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                    //还原库存
                    command.CommandText = "Update dbo.Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi  Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId)";
                    
                    //组合商品还原库存
                    command.CommandText+=@"Update Ecshop_SKUs Set Stock = Stock+(select SUM(oi.ShipmentQuantity*pc.Quantity) from Ecshop_ProductsCombination pc inner join Ecshop_OrderItems oi

on oi.SkuId =pc.CombinationSkuId where pc.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId)

WHERE Ecshop_SKUs.SkuId  IN (Select pc2.SkuId from Ecshop_ProductsCombination pc2 inner join   Ecshop_OrderItems oi2 on  oi2.SkuId =pc2.CombinationSkuId Where oi2.OrderId =@OrderId);";
                    result = this.database.ExecuteNonQuery(command, transaction) > 0;
                    if (!result)
                    {
                        transaction.Rollback();
                        return result;
                    }

                    //还原优惠券
                    command.CommandText = "SELECT COUNT(1) FROM dbo.Ecshop_CouponItems where OrderId =@OrderId";
                    command.Parameters.Clear();
                    this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                    int count = (int)this.database.ExecuteScalar(command, transaction);
                    if (count > 0)
                    {
                        command.CommandText = "Update dbo.Ecshop_CouponItems Set OrderId = null,UsedTime = null,CouponStatus = 0 where OrderId=@OrderId";
                        command.Parameters.Clear();
                        this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                        result = this.database.ExecuteNonQuery(command, transaction) == count;
                        if (!result)
                        {
                            transaction.Rollback();
                            return result;
                        }
                    }

                    //还原现金券
                    command.CommandText = "SELECT COUNT(1) FROM dbo.Ecshop_VoucherItems where OrderId =@OrderId";
                    command.Parameters.Clear();
                    this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                    count = (int)this.database.ExecuteScalar(command, transaction);
                    if (count > 0)
                    {
                        command.CommandText = "Update dbo.Ecshop_VoucherItems Set OrderId = null,UsedTime = null,VoucherStatus = 0 where OrderId =@OrderId";
                        command.Parameters.Clear();
                        this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                        result = this.database.ExecuteNonQuery(command, transaction) == count;
                        if (!result)
                        {
                            transaction.Rollback();
                            return result;
                        }
                    }

                    transaction.Commit();
                    return result;
                }
            }
        }



        /// <summary>
        /// 子单退款，还原优惠卷和库存
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="closeType"></param>
        /// <returns></returns>
        public bool RefundOrder_Split(string orderId,string SourceOrderId)
        {
            bool result = false;
            using (DbConnection connection = this.database.CreateConnection())
            {
                DbCommand command = this.database.GetSqlStringCommand("UPDATE dbo.Ecshop_Orders SET OrderStatus = @OrderStatus,CloseReason=@CloseReason WHERE SourceOrderId=@SourceOrderId and OrderStatus=1 ");
                this.database.AddInParameter(command, "@OrderStatus", DbType.Int32, (int)OrderStatus.Refunded);
                this.database.AddInParameter(command, "@CloseReason", DbType.String, "用户申请退款");
                this.database.AddInParameter(command, "@SourceOrderId", DbType.String, SourceOrderId);
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
//                    //更新订单状态
//                    bool result = this.database.ExecuteNonQuery(command, transaction) > 0;
//                    if (!result)
//                    {
//                        transaction.Rollback();
//                        return result;
//                    }
//                    command.Parameters.Clear();
//                    this.database.AddInParameter(command, "SourceOrderId", DbType.String, SourceOrderId);
//                    //还原库存
//                    command.CommandText = "Update dbo.Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi  Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId in(select OrderId from Ecshop_Orders where SourceOrderId=@SourceOrderId)) WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId in(select OrderId from Ecshop_Orders where SourceOrderId=@SourceOrderId))";
//                    //组合商品还原库存
//                    command.CommandText += @"Update Ecshop_SKUs Set 
//                                             Stock = Stock+(select SUM(oi.ShipmentQuantity*pc.Quantity) 
//                                                 from Ecshop_ProductsCombination pc 
//                                                 inner join Ecshop_OrderItems oi on oi.SkuId =pc.CombinationSkuId 
//                                                 where pc.SkuId =Ecshop_SKUs.SkuId AND OrderId in(select OrderId from Ecshop_Orders where SourceOrderId=@SourceOrderId))
//                                             WHERE Ecshop_SKUs.SkuId  IN (Select pc2.SkuId from Ecshop_ProductsCombination pc2 inner join   Ecshop_OrderItems oi2 on  oi2.SkuId =pc2.CombinationSkuId Where oi2.OrderId OrderId in(select OrderId from Ecshop_Orders where SourceOrderId=@SourceOrderId));";
//                    result = this.database.ExecuteNonQuery(command, transaction) > 0;
//                    if (!result)
//                    {
//                        transaction.Rollback();
//                        return result;
//                    }
                    #region 还原优惠券
                    ////还原优惠券
                    command.CommandText = "SELECT COUNT(1) FROM dbo.Ecshop_CouponItems where OrderId =@SourceOrderId";
                    command.Parameters.Clear();
                    this.database.AddInParameter(command, "@SourceOrderId", DbType.String, SourceOrderId);

                    int count = (int)this.database.ExecuteScalar(command, transaction);
                    if (count > 0)
                    {
                        command.CommandText = "Update dbo.Ecshop_CouponItems Set OrderId = null,UsedTime = null,CouponStatus = 0 where OrderId=@SourceOrderId";
                        command.Parameters.Clear();
                        this.database.AddInParameter(command, "@SourceOrderId", DbType.String, SourceOrderId);
                        result = this.database.ExecuteNonQuery(command, transaction) == count;
                        if (!result)
                        {
                            transaction.Rollback();
                            return result;
                        }
                    }
                    //还原优惠券
                    command.CommandText = "SELECT COUNT(1) FROM dbo.Ecshop_CouponItems where OrderId =@OrderId";
                    command.Parameters.Clear();
                    this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                    count = (int)this.database.ExecuteScalar(command, transaction);
                    if (count > 0)
                    {
                        command.CommandText = "Update dbo.Ecshop_CouponItems Set OrderId = null,UsedTime = null,CouponStatus = 0 where OrderId=@OrderId";
                        command.Parameters.Clear();
                        this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                        result = this.database.ExecuteNonQuery(command, transaction) == count;
                        if (!result)
                        {
                            transaction.Rollback();
                            return result;
                        }
                    }
                    #endregion

                    #region 现金卷屏蔽
                    //还原现金券
                    //command.CommandText = "SELECT COUNT(1) FROM dbo.Ecshop_VoucherItems where OrderId =@OrderId";
                    //command.Parameters.Clear();
                    //this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                    //count = (int)this.database.ExecuteScalar(command, transaction);
                    //if (count > 0)
                    //{
                    //    command.CommandText = "Update dbo.Ecshop_VoucherItems Set OrderId = null,UsedTime = null,VoucherStatus = 0 where OrderId =@OrderId";
                    //    command.Parameters.Clear();
                    //    this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                    //    result = this.database.ExecuteNonQuery(command, transaction) == count;
                    //    if (!result)
                    //    {
                    //        transaction.Rollback();
                    //        return result;
                    //    }
                    //}
                    #endregion 


                    transaction.Commit();
                    return result;
                }
            }
        }


        /// <summary>
        /// 申请退货还原库存和优惠卷
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="closeType"></param>
        /// <returns></returns>
        public bool RefundOrder(string orderId)
        {
            using (DbConnection connection = this.database.CreateConnection())
            {
                DbCommand command = this.database.GetSqlStringCommand("UPDATE dbo.Ecshop_Orders SET OrderStatus = @OrderStatus,CloseReason=@CloseReason WHERE OrderId=@OrderId ");
                this.database.AddInParameter(command, "@OrderStatus", DbType.Int32, (int)OrderStatus.Refunded);
                this.database.AddInParameter(command, "@CloseReason", DbType.String, "用户申请退款");
                this.database.AddInParameter(command, "@OrderId", DbType.String, orderId);
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    //更新订单状态
                    bool result = this.database.ExecuteNonQuery(command, transaction) > 0;
                    if (!result)
                    {
                        transaction.Rollback();
                        return result;
                    }

                    command.Parameters.Clear();
                    this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                    //还原库存
                    command.CommandText = "Update dbo.Ecshop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Ecshop_OrderItems oi  Where oi.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId) WHERE Ecshop_SKUs.SkuId  IN (Select SkuId FROM Ecshop_OrderItems Where OrderId =@OrderId)";
                    //组合商品还原库存
                    command.CommandText += @"Update Ecshop_SKUs Set 
                                              Stock = Stock+(select SUM(oi.ShipmentQuantity*pc.Quantity) 
                                                 from Ecshop_ProductsCombination pc 
                                                 inner join Ecshop_OrderItems oi on oi.SkuId =pc.CombinationSkuId 
                                                 where pc.SkuId =Ecshop_SKUs.SkuId AND OrderId =@OrderId)
                                             WHERE Ecshop_SKUs.SkuId  IN (Select pc2.SkuId from Ecshop_ProductsCombination pc2 inner join   Ecshop_OrderItems oi2 on  oi2.SkuId =pc2.CombinationSkuId Where oi2.OrderId =@OrderId);";
                    result = this.database.ExecuteNonQuery(command, transaction) > 0;
                    if (!result)
                    {
                        transaction.Rollback();
                        return result;
                    }

                    //还原优惠券
                    command.CommandText = "SELECT COUNT(1) FROM dbo.Ecshop_CouponItems where OrderId =@OrderId";
                    command.Parameters.Clear();
                    this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                    int count = (int)this.database.ExecuteScalar(command, transaction);
                    if (count > 0)
                    {
                        command.CommandText = "Update dbo.Ecshop_CouponItems Set OrderId = null,UsedTime = null,CouponStatus = 0 where OrderId=@OrderId";
                        command.Parameters.Clear();
                        this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                        result = this.database.ExecuteNonQuery(command, transaction) == count;
                        if (!result)
                        {
                            transaction.Rollback();
                            return result;
                        }
                    }

                    //还原现金券
                    command.CommandText = "SELECT COUNT(1) FROM dbo.Ecshop_VoucherItems where OrderId =@OrderId";
                    command.Parameters.Clear();
                    this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                    count = (int)this.database.ExecuteScalar(command, transaction);
                    if (count > 0)
                    {
                        command.CommandText = "Update dbo.Ecshop_VoucherItems Set OrderId = null,UsedTime = null,VoucherStatus = 0 where OrderId =@OrderId";
                        command.Parameters.Clear();
                        this.database.AddInParameter(command, "OrderId", DbType.String, orderId);

                        result = this.database.ExecuteNonQuery(command, transaction) == count;
                        if (!result)
                        {
                            transaction.Rollback();
                            return result;
                        }
                    }

                    transaction.Commit();
                    return result;
                }
            }
        }

        public DataTable GetUserInvoices(int userId)
        {
            DataTable result;

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select InvoiceTitle, max(OrderDate) OrderDate from dbo.Ecshop_Orders where InvoiceTitle <> '个人' and InvoiceTitle <> '' and InvoiceTitle is not null and UserId = @UserId group by InvoiceTitle order by OrderDate desc");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);

            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }

            return result;
        }


        /// <summary>
        /// 根据订单id更新商品销量
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="dbTran"></param>
        /// <returns></returns>
        public bool UpdatePayOrderProductSaleCount(string orderId, DbTransaction dbTran)
        {
            


            StringBuilder sb = new StringBuilder();

            //更新商品销量
            sb.Append(@"update Ecshop_Products set SaleCounts=SaleCounts+(SELECT sum(o.ShipmentQuantity) AS Quantity
  
  FROM Ecshop_OrderItems AS o 
  
  where O.OrderId =@OrderId and o.ProductId=Ecshop_Products.ProductId)
  
  where Ecshop_Products.ProductId in (Select ProductId from Ecshop_OrderItems where Ecshop_OrderItems.OrderId=@OrderId);");

            //更新商品显示销量
            sb.Append(@"update Ecshop_Products set ShowSaleCounts=ShowSaleCounts+(SELECT sum(o.ShipmentQuantity) AS Quantity
  
  FROM Ecshop_OrderItems AS o 
  
  where O.OrderId =@OrderId and o.ProductId=Ecshop_Products.ProductId)
  
  where Ecshop_Products.ProductId in (Select ProductId from Ecshop_OrderItems where Ecshop_OrderItems.OrderId=@OrderId);");



            //更新组合商品明细销量SaleCounts
            sb.Append(@"update Ecshop_Products set SaleCounts=SaleCounts+(SELECT sum(oc.Quantity*o.ShipmentQuantity) AS Quantity
  
  FROM Ecshop_OrderItems AS o 
  
  JOIN Ecshop_OrderCombinationItems AS oc ON oc.orderId=o.orderId AND oc.CombinationSkuId=o.skuId
  
  where O.OrderId =@OrderId and oc.ProductId=Ecshop_Products.ProductId)
  
  where Ecshop_Products.ProductId in (Select ProductId from Ecshop_OrderCombinationItems 
  
  
  where Ecshop_OrderCombinationItems.OrderId=@OrderId);");

            //更新组合商品明细显示销量ShowSaleCounts
            sb.Append(@"update Ecshop_Products set ShowSaleCounts=ShowSaleCounts+(SELECT sum(oc.Quantity*o.ShipmentQuantity) AS Quantity
  
  FROM Ecshop_OrderItems AS o 
  
  JOIN Ecshop_OrderCombinationItems AS oc ON oc.orderId=o.orderId AND oc.CombinationSkuId=o.skuId
  
  where O.OrderId =@OrderId and oc.ProductId=Ecshop_Products.ProductId)
  
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

        /// <summary>
        /// 根据快递单号更新订单状态（对外接口，供快递公司调用）
        /// </summary>
        /// <param name="orderShip"></param>
        /// <returns></returns>
        public void UpdateShipOrderStatusByShipOrderNumber(Dictionary<string,string> orderShip)
        { 
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in orderShip)
            {
                //更新商品销量
                sb.Append(string.Format(@"update Ecshop_Orders set ShipOrderStatus={1} where ShipOrderNumber='{0}';", kvp.Key, kvp.Value));
            }
            DbCommand command = this.database.GetSqlStringCommand(sb.ToString());
            this.database.ExecuteNonQuery(command);
        }

        /// <summary>
        /// 保存广告订单
        /// </summary>
        /// <param name="adInfo"></param>
        /// <returns></returns>
        public bool InsertAdOrderInfo(orders adInfo, string JsonStr)
        {
            try
            {
                string sql = @"INSERT INTO [AdOrderInfo]
                        (  
                           OrderNo,
                           orderTime ,
                           updateTime,
                           JsonStr,
                           cid,
                           feedback
                         )
                        VALUES
                        (
                           @OrderNo,
                           @orderTime ,
                           @updateTime,
                           @JsonStr,
                           @cid,
                           @feedback
                        )";
                DbCommand command = this.database.GetSqlStringCommand(sql);
                this.database.AddInParameter(command, "@OrderNo", DbType.String, adInfo.OrderNo);
                this.database.AddInParameter(command, "@orderTime", DbType.DateTime, adInfo.OrderTime);
                this.database.AddInParameter(command, "@updateTime", DbType.String, adInfo.UpdateTime);
                this.database.AddInParameter(command, "@JsonStr", DbType.String,JsonStr);
                this.database.AddInParameter(command, "@cid", DbType.String, adInfo.Campaignid);
                this.database.AddInParameter(command, "@feedback", DbType.String, adInfo.Feedback);
                //新增广告订单
                bool result = this.database.ExecuteNonQuery(command) > 0;
                return result;
            }
            catch (Exception ee)
            {
                ErrorLog.Write("InsertAdOrderInfo",ee);
            }
            return false;
            
        }

        /// <summary>
        /// 根据单号获取订单明细
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public DataTable GetOrderItemInfo(string OrderId)
        {
            DataTable result = new DataTable();

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"SELECT   a.CouponValue,b.ItemAdjustedPrice, a.OrderTotal,c.CategoryId,SkuId,c.ProductName,Quantity  FROM  Ecshop_Orders AS a
                                        INNER JOIN Ecshop_OrderItems AS b ON a.OrderId=b.OrderId 
                                        INNER JOIN Ecshop_Products  AS c ON b.ProductId=c.ProductId
                                        WHERE a.OrderId=@OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);

            DataSet dsOrders = this.database.ExecuteDataSet(sqlStringCommand);
            if (dsOrders != null && dsOrders.Tables.Count > 0)
            {
                result = dsOrders.Tables[0];
            }
            return result;

           
        }
        /// <summary>
        /// 根据时间查询广告订单
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="StartTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataTable SelectAdOrderInfo(int cid,DateTime StartTime,DateTime endTime,int type)
        {
            DataTable result = new DataTable();
            try
            {
                  string sql = "SELECT  * FROM AdOrderInfo WHERE cid=@cid AND OrderTime BETWEEN @StartTime AND @endTime";
                  if (type == 1)
                  {
                      sql = @" SELECT CASE OrderStatus  
                                 WHEN 1 THEN '等待买家付款'
                                 WHEN 2 THEN '等待发货'
                                 WHEN 3 THEN '已发货'
                                 WHEN 4 THEN '已关闭'
                                 WHEN 5 THEN '订单已完成'
                                 WHEN 6 THEN '申请退款'
                                 WHEN 7 THEN '申请退货'
                                 WHEN 8 THEN '申请换货'
                                 WHEN 9 THEN '已经退款'
                                 WHEN 10 THEN '已经退货'
                                 WHEN 98 THEN '拆分订单'
                                 WHEN 99 THEN '历史订单'
                                 ELSE '未知' END AS 'OrderStatus',PaymentType,CASE WHEN OrderStatus>1 THEN '已支付' ELSE '未支付' END AS 'paymentStatus',
                                 b.UpdateDate,
                                 a.feedback,
                                 a.OrderNo
                                 FROM  AdOrderInfo AS a
                                 INNER JOIN  Ecshop_orders AS b ON a.OrderNo=b.OrderId
                                 WHERE b.UpdateDate  @StartTime AND @endTime";
                  }
                  DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
                  this.database.AddInParameter(sqlStringCommand, "@cid", DbType.String, cid);
                  this.database.AddInParameter(sqlStringCommand, "@StartTime", DbType.DateTime, StartTime);
                  this.database.AddInParameter(sqlStringCommand, "@endTime", DbType.DateTime, endTime);
              
                  DataSet dsOrders = this.database.ExecuteDataSet(sqlStringCommand);
                  if (dsOrders != null && dsOrders.Tables.Count > 0)
                  {
                      result = dsOrders.Tables[0];
                  }
                  return result;
            }
            catch (Exception ee)
            {
                return new DataTable();
            }
        }

        /// <summary>
        /// 获取已发货的订单数
        /// </summary>
        /// <returns></returns>
        public int GetDeliveredOrderCount()
        {
            int result = 0;
            try
            {
                string sql = "select count(*) from Ecshop_Orders where OrderStatus>2 and ShipOrderStatus<>3 and ShipOrderNumber IS NOT NULL and datediff(minute,ShippingDate,getdate())>7*60 ";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
                result = (int)this.database.ExecuteScalar(sqlStringCommand);
                
            }
            catch (Exception)
            {
                
            }
            return result;
        }

        /// <summary>
        /// 获取已发货的订单
        /// </summary>
        /// <returns></returns>
        public DataTable GetDeliveredOrderDt(int pageSize,int pageIndex)
        {
            DataTable result = new DataTable();
            try
            {
                string sql = string.Format(@"SELECT TOP {0} *
                                        FROM Ecshop_Orders
                                        WHERE OrderStatus>2 and ShipOrderStatus<>3 and ShipOrderNumber IS NOT NULL and datediff(minute,ShippingDate,getdate())>7*60 and OrderId NOT IN
                                        (
                                        SELECT TOP ({0} *({1}-1)) OrderId FROM Ecshop_Orders where OrderStatus>2 and ShipOrderStatus<>3 and ShipOrderNumber IS NOT NULL ORDER BY OrderId
                                        )
                                        ORDER BY OrderId", pageSize, pageIndex);
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);

                DataSet dsOrders = this.database.ExecuteDataSet(sqlStringCommand);
                if (dsOrders != null && dsOrders.Tables.Count > 0)
                {
                    result = dsOrders.Tables[0];
                }
                return result;
            }
            catch (Exception ee)
            {
                return new DataTable();
            }
        }
    }
}

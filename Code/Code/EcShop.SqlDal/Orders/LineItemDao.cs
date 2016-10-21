using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Data;

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using System.Collections.Generic;
using EcShop.Entities.Sales;
using EcShop.Entities;

namespace EcShop.SqlDal.Orders
{
	public class LineItemDao
	{
		private Database database;
		public LineItemDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
        public bool AddOrderLineItems(string orderId, ICollection lineItems, DbTransaction dbTran, IList<ShoppingCartPresentInfo> linePresents= null)
		{
			bool result;
			if (lineItems == null || lineItems.Count == 0)
			{
				result = false;
			}
			else
			{
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
				this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
				int num = 0;
				StringBuilder stringBuilder = new StringBuilder();
				foreach (LineItemInfo lineItemInfo in lineItems)
				{
					string text = num.ToString();
                    stringBuilder.Append("INSERT INTO Ecshop_OrderItems(OrderId, SkuId, ProductId, SKU, Quantity, ShipmentQuantity, CostPrice,PromotionPrice").Append(",ItemListPrice, ItemAdjustedPrice, ItemDescription, ThumbnailsUrl, Weight, SKUContent, PromotionId, PromotionName,TaxRate,TemplateId,StoreId,SupplierId,DeductFee) VALUES( @OrderId").Append(",@SkuId").Append(text).Append(",@ProductId").Append(text).Append(",@SKU").Append(text).Append(",@Quantity").Append(text).Append(",@ShipmentQuantity").Append(text).Append(",@CostPrice").Append(text).Append(",@PromotionPrice").Append(text).Append(",@ItemListPrice").Append(text).Append(",@ItemAdjustedPrice").Append(text).Append(",@ItemDescription").Append(text).Append(",@ThumbnailsUrl").Append(text).Append(",@Weight").Append(text).Append(",@SKUContent").Append(text).Append(",@PromotionId").Append(text).Append(",@PromotionName").Append(text).Append(",@TaxRate").Append(text).Append(",@TemplateId").Append(text).Append(",@StoreId").Append(text).Append(",@SupplierId").Append(text).Append(",@DeductFee").Append(text).Append(");");
					this.database.AddInParameter(sqlStringCommand, "SkuId" + text, DbType.String, lineItemInfo.SkuId);
					this.database.AddInParameter(sqlStringCommand, "ProductId" + text, DbType.Int32, lineItemInfo.ProductId);
					this.database.AddInParameter(sqlStringCommand, "SKU" + text, DbType.String, lineItemInfo.SKU);
					this.database.AddInParameter(sqlStringCommand, "Quantity" + text, DbType.Int32, lineItemInfo.Quantity);
					this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity" + text, DbType.Int32, lineItemInfo.ShipmentQuantity);
					this.database.AddInParameter(sqlStringCommand, "CostPrice" + text, DbType.Currency, lineItemInfo.ItemCostPrice);
					this.database.AddInParameter(sqlStringCommand, "ItemListPrice" + text, DbType.Currency, lineItemInfo.ItemListPrice);
					this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice" + text, DbType.Currency, lineItemInfo.ItemAdjustedPrice);
					this.database.AddInParameter(sqlStringCommand, "ItemDescription" + text, DbType.String, lineItemInfo.ItemDescription);
					this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + text, DbType.String, lineItemInfo.ThumbnailsUrl);
					this.database.AddInParameter(sqlStringCommand, "Weight" + text, DbType.Int32, lineItemInfo.ItemWeight);
					this.database.AddInParameter(sqlStringCommand, "SKUContent" + text, DbType.String, lineItemInfo.SKUContent);
					this.database.AddInParameter(sqlStringCommand, "PromotionId" + text, DbType.Int32, lineItemInfo.PromotionId);
					this.database.AddInParameter(sqlStringCommand, "PromotionName" + text, DbType.String, lineItemInfo.PromotionName);
                    this.database.AddInParameter(sqlStringCommand, "TaxRate" + text, DbType.String, lineItemInfo.TaxRate);
                    this.database.AddInParameter(sqlStringCommand, "TemplateId" + text, DbType.Int32, lineItemInfo.TemplateId);
                    this.database.AddInParameter(sqlStringCommand, "StoreId" + text, DbType.Int32, lineItemInfo.storeId);
                    this.database.AddInParameter(sqlStringCommand, "SupplierId" + text, DbType.Int32, lineItemInfo.SupplierId);
                    this.database.AddInParameter(sqlStringCommand, "PromotionPrice" + text, DbType.Int32, lineItemInfo.PromotionPrice);
                    this.database.AddInParameter(sqlStringCommand, "DeductFee" + text, DbType.Decimal, lineItemInfo.DeductFee);

                    // 判断是否是组合商品
                    if (lineItemInfo.CombinationItemInfos != null && lineItemInfo.CombinationItemInfos.Count() > 0)
                    {
                        StringBuilder comsql = new StringBuilder();
                        DbCommand comCommand = this.database.GetSqlStringCommand(" ");
                        this.database.AddInParameter(comCommand, "OrderId", DbType.String, orderId);
                        int comnum = 0;
                        foreach (ProductsCombination com in lineItemInfo.CombinationItemInfos)
                        {
                            string comtext = comnum.ToString();
                            comsql.AppendFormat(@" INSERT INTO [Ecshop_OrderCombinationItems]
           ([OrderId],[SkuId],[CombinationSkuId],[ProductId],[SKU],[Quantity],[ShipmentQuantity],[CostPrice],[ItemListPrice],[ItemAdjustedPrice],[ItemDescription],[ThumbnailsUrl],[Weight],[SKUContent],[PromotionId],[PromotionName],[TaxRate],[TemplateId],[storeId],[SupplierId],[DeductFee],[PromotionPrice])
     VALUES (@OrderId,@SkuId{0},@CombinationSkuId{0},@ProductId{0},@SKU{0},@Quantity{0},@ShipmentQuantity{0},@CostPrice{0},@ItemListPrice{0},@ItemAdjustedPrice{0},@ItemDescription{0},@ThumbnailsUrl{0},@Weight{0}
           ,@SKUContent{0},@PromotionId{0},@PromotionName{0},@TaxRate{0},@TemplateId{0},@storeId{0},@SupplierId{0},@DeductFee{0},@PromotionPrice{0});", comtext);
                            this.database.AddInParameter(comCommand, "SkuId" + comtext, DbType.String, com.SkuId);
                            this.database.AddInParameter(comCommand, "CombinationSkuId" + comtext, DbType.String, lineItemInfo.SkuId);
                            this.database.AddInParameter(comCommand, "ProductId" + comtext, DbType.Int32, com.ProductId);
                            this.database.AddInParameter(comCommand, "SKU" + comtext, DbType.String, com.SKU);
                            this.database.AddInParameter(comCommand, "Quantity" + comtext, DbType.Int32, com.Quantity);
                            this.database.AddInParameter(comCommand, "ShipmentQuantity" + comtext, DbType.Int32, com.Quantity);
                            this.database.AddInParameter(comCommand, "CostPrice" + comtext, DbType.Currency, com.Price);
                            this.database.AddInParameter(comCommand, "ItemListPrice" + comtext, DbType.Currency, com.Price);
                            this.database.AddInParameter(comCommand, "ItemAdjustedPrice" + comtext, DbType.Currency, com.Price);
                            this.database.AddInParameter(comCommand, "ItemDescription" + comtext, DbType.String, com.ProductName);//显示子商品的名称
                            this.database.AddInParameter(comCommand, "ThumbnailsUrl" + comtext, DbType.String, com.ThumbnailsUrl);
                            this.database.AddInParameter(comCommand, "Weight" + comtext, DbType.Int32, com.Weight);
                            this.database.AddInParameter(comCommand, "SKUContent" + comtext, DbType.String, com.SKUContent);
                            this.database.AddInParameter(comCommand, "PromotionId" + comtext, DbType.Int32, lineItemInfo.PromotionId);
                            this.database.AddInParameter(comCommand, "PromotionName" + comtext, DbType.String, lineItemInfo.PromotionName);
                            this.database.AddInParameter(comCommand, "TaxRate" + comtext, DbType.String, com.TaxRate);
                            this.database.AddInParameter(comCommand, "TemplateId" + comtext, DbType.Int32, lineItemInfo.TemplateId);
                            this.database.AddInParameter(comCommand, "StoreId" + comtext, DbType.Int32, lineItemInfo.storeId);
                            this.database.AddInParameter(comCommand, "SupplierId" + comtext, DbType.Int32, lineItemInfo.SupplierId);
                            this.database.AddInParameter(comCommand, "PromotionPrice" + comtext, DbType.Int32, lineItemInfo.PromotionPrice);
                            this.database.AddInParameter(comCommand, "DeductFee" + comtext, DbType.Decimal, lineItemInfo.DeductFee);
                            comnum++;
                        }
                        if (comsql.ToString().Length > 0)
                        {
                            comCommand.CommandText = comsql.ToString();
                            if (dbTran != null)
                            {
                                result = (this.database.ExecuteNonQuery(comCommand, dbTran) > 0);
                            }
                            else
                            {
                                result = (this.database.ExecuteNonQuery(comCommand) > 0);
                            }
                        }
                       
                    }

                    // 判断该商品是否有赠品
                    if (linePresents != null && linePresents.Count > 0)
                    {
                        var p = linePresents.Where(m => m.PromotionProductId == lineItemInfo.ProductId).ToList<ShoppingCartPresentInfo>();
                        if (p != null && p.Count() > 0)
                        {
                            AddOrderPresents(orderId, p, dbTran);
                        }
                    }
					num++;
					if (num == 50)
					{
						sqlStringCommand.CommandText = stringBuilder.ToString();
						int num2;
						if (dbTran != null)
						{
							num2 = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
						}
						else
						{
							num2 = this.database.ExecuteNonQuery(sqlStringCommand);
						}
						if (num2 <= 0)
						{
							result = false;
							return result;
						}
						stringBuilder.Remove(0, stringBuilder.Length);
						sqlStringCommand.Parameters.Clear();
						this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
						num = 0;
					}
				}
				if (stringBuilder.ToString().Length > 0)
				{
					sqlStringCommand.CommandText = stringBuilder.ToString();
					if (dbTran != null)
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
					}
					else
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
					}
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

        public bool AddOrderPresents(string orderId, IList<ShoppingCartPresentInfo> linePresents, DbTransaction dbTran)
        {
            bool result;
            if (linePresents == null || linePresents.Count == 0)
            {
                result = false;
            }
            else
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
                this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                int num = 0;
                StringBuilder stringBuilder = new StringBuilder();
                foreach (ShoppingCartPresentInfo lineItemInfo in linePresents)
                {
                    string text = num.ToString();
                    stringBuilder.AppendFormat(@" INSERT INTO [Ecshop_OrderPresents]
           ([OrderId]
           ,[SkuId]
           ,[ProductId]
           ,[PromotionProductId]
           ,[SKU]
           ,[ShipmentQuantity]
           ,[CostPrice]
           ,[ItemListPrice]
           ,[ItemAdjustedPrice]
           ,[ItemDescription]
           ,[SKUContent]
           ,[PromotionId]
           ,[PromotionName]
           ,[ThumbnailsUrl]
           ,[storeId]
           ,[SupplierId])
     VALUES
           (@OrderId
           ,@SkuId{0}
           ,@ProductId{0}
           ,@PromotionProductId{0}
           ,@SKU{0}
           ,@ShipmentQuantity{0}
           ,@CostPrice{0}
           ,@ItemListPrice{0}
           ,@ItemAdjustedPrice{0}
           ,@ItemDescription{0}
           ,@SKUContent{0}
           ,@PromotionId{0}
           ,@PromotionName{0}
           ,@ThumbnailsUrl{0}
           ,@StoreId{0}
           ,@SupplierId{0});", text);
                    this.database.AddInParameter(sqlStringCommand, "SkuId" + text, DbType.String, lineItemInfo.SkuId);
                    this.database.AddInParameter(sqlStringCommand, "ProductId" + text, DbType.Int32, lineItemInfo.ProductId);
                    this.database.AddInParameter(sqlStringCommand, "PromotionProductId" + text, DbType.Int32, lineItemInfo.PromotionProductId);
                    this.database.AddInParameter(sqlStringCommand, "SKU" + text, DbType.String, lineItemInfo.SKU);
                    this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity" + text, DbType.Int32, lineItemInfo.ShipmentQuantity);
                    this.database.AddInParameter(sqlStringCommand, "CostPrice" + text, DbType.Currency, lineItemInfo.CostPrice);
                    this.database.AddInParameter(sqlStringCommand, "ItemListPrice" + text, DbType.Currency, lineItemInfo.ItemListPrice);
                    this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice" + text, DbType.Currency, lineItemInfo.ItemAdjustedPrice);
                    this.database.AddInParameter(sqlStringCommand, "ItemDescription" + text, DbType.String, lineItemInfo.ProductName);
                    this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + text, DbType.String, lineItemInfo.ThumbnailUrl40);
                    this.database.AddInParameter(sqlStringCommand, "SKUContent" + text, DbType.String, lineItemInfo.SkuContent);
                    this.database.AddInParameter(sqlStringCommand, "PromotionId" + text, DbType.Int32, lineItemInfo.PromotionId);
                    this.database.AddInParameter(sqlStringCommand, "PromotionName" + text, DbType.String, lineItemInfo.PromotionName);
                    this.database.AddInParameter(sqlStringCommand, "StoreId" + text, DbType.Int32, lineItemInfo.StoreId);
                    this.database.AddInParameter(sqlStringCommand, "SupplierId" + text, DbType.Int32, lineItemInfo.SupplierId);
                    num++;
                    if (num == 50)
                    {
                        sqlStringCommand.CommandText = stringBuilder.ToString();
                        int num2;
                        if (dbTran != null)
                        {
                            num2 = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                        }
                        else
                        {
                            num2 = this.database.ExecuteNonQuery(sqlStringCommand);
                        }
                        if (num2 <= 0)
                        {
                            result = false;
                            return result;
                        }
                        stringBuilder.Remove(0, stringBuilder.Length);
                        sqlStringCommand.Parameters.Clear();
                        this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                        num = 0;
                    }
                }
                if (stringBuilder.ToString().Length > 0)
                {
                    sqlStringCommand.CommandText = stringBuilder.ToString();
                    if (dbTran != null)
                    {
                        result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
                    }
                    else
                    {
                        result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
                    }
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }

		public bool DeleteLineItem(string skuId, string orderId, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Ecshop_OrderItems WHERE OrderId=@OrderId AND SkuId=@SkuId ");
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public bool UpdateLineItem(string orderId, LineItemInfo lineItem, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Ecshop_OrderItems SET ShipmentQuantity=@ShipmentQuantity,ItemAdjustedPrice=@ItemAdjustedPrice,Quantity=@Quantity, PromotionId = NULL, PromotionName = NULL WHERE OrderId=@OrderId AND SkuId=@SkuId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, lineItem.SkuId);
			this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity", DbType.Int32, lineItem.ShipmentQuantity);
			this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice", DbType.Currency, lineItem.ItemAdjustedPrice);
			this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, lineItem.Quantity);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public int GetLineItemNumber(int productId)
		{
            string query = string.Format("select ShowSaleCounts from Ecshop_Products where ProductId=@ProductId");
			//string query = string.Format("select count(*) from dbo.Ecshop_OrderItems as items left join Ecshop_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!={0} and orders.OrderStatus!={1} and items.ProductId=@ProductId", 1, 4);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}

        public int GetLineItemCount(int productId)
        {
            string query = string.Format("select count(*) from vw_Ecshop_OrderItem where ProductId=@ProductId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

		public DbQueryResult GetLineItems(Pagination page, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ProductId = {0} ", productId);
			return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Ecshop_OrderItem", "OrderId", stringBuilder.ToString(), "*");
		}
		public DataTable GetLineItems(int productId, int maxNum)
		{
			DataTable result = new DataTable();
			string query = string.Format("select top " + maxNum + " items.*,orders.PayDate,orders.Username,orders.ShipTo from dbo.Ecshop_OrderItems as items left join Ecshop_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!={0} and orders.OrderStatus!={1} and items.ProductId=@ProductId  order by orders.PayDate desc", 1, 4);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public bool IsBuyProduct(int productId)
		{
			bool result = false;
			try
			{
				string query = "select top 1 orders.UserId from Ecshop_OrderItems as items left join Ecshop_Orders orders on items.OrderId=orders.OrderId where ProductId=@ProductId and orders.UserId=@UserId and orders.OrderStatus=@OrderStatus";
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
				this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 5);
				using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						result = true;
					}
				}
			}
			catch (Exception var_4_AD)
			{
				result = true;
			}
			return result;
		}
		public int CountDownOrderCount(int productid)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select   isnull(sum(quantity),0) as Quanity from dbo.Ecshop_OrderItems where productid=@productid and  orderid in(select orderid from dbo.Ecshop_Orders where userid=@userid and orderstatus!=4 AND ISNULL(CountDownBuyId, 0) > 0)");
			this.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
			this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, HiContext.Current.User.UserId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}

        
        public int CountDownOrderCount(int productid,int userid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select   isnull(sum(quantity),0) as Quanity from dbo.Ecshop_OrderItems where productid=@productid and  orderid in(select orderid from dbo.Ecshop_Orders where userid=@userid and orderstatus!=4 AND ISNULL(CountDownBuyId, 0) > 0)");
            this.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, userid);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        public int CountDownOrderCount(int productid, int userid, int countDownId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select   isnull(sum(quantity),0) as Quanity from dbo.Ecshop_OrderItems where productid=@productid and  orderid in(select orderid from dbo.Ecshop_Orders where userid=@userid and orderstatus!=4 AND CountDownBuyId = @CountDownId)");
            this.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, userid);
            this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        /// <summary>
        /// 获取限时抢购的商品的已购数量
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
         public int AllCountDownOrderCount(int productid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select   isnull(sum(quantity),0) as Quanity from dbo.Ecshop_OrderItems where productid=@productid and  orderid in(select orderid from dbo.Ecshop_Orders where  orderstatus!=4 AND ISNULL(CountDownBuyId, 0) > 0)");
            this.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

         /// <summary>
         /// 获取限时抢购的商品的已购数量
         /// </summary>
         /// <param name="productid"></param>
         /// <returns></returns>
         public int AllCountDownOrderCount(int productid, int countDownId)
         {
             DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select   isnull(sum(quantity),0) as Quanity from dbo.Ecshop_OrderItems where productid=@productid and  orderid in(select orderid from dbo.Ecshop_Orders where  orderstatus <> 4 and OrderStatus <> 98 AND CountDownBuyId = @CountDownId)");
             this.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
             this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
             return (int)this.database.ExecuteScalar(sqlStringCommand);
         }

        public bool AddBackOrderLineItems(string orderId, ICollection lineItems, DbTransaction dbTran)
        {
            bool result;
            if (lineItems == null || lineItems.Count == 0)
            {
                result = false;
            }
            else
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
                this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                int num = 0;
                StringBuilder stringBuilder = new StringBuilder();
                foreach (LineItemInfo lineItemInfo in lineItems)
                {
                    string text = num.ToString();
                    stringBuilder.Append("INSERT INTO Ecshop_OrderItems(OrderId, SkuId, ProductId, SKU, Quantity, ShipmentQuantity, CostPrice,DeductFee").Append(",ItemListPrice, ItemAdjustedPrice, ItemDescription, ThumbnailsUrl, Weight, SKUContent, PromotionId, PromotionName,TaxRate,TemplateId,StoreId,SupplierId) VALUES( @OrderId").Append(",@SkuId").Append(text).Append(",@ProductId").Append(text).Append(",@SKU").Append(text).Append(",@Quantity").Append(text).Append(",@ShipmentQuantity").Append(text).Append(",@CostPrice").Append(text).Append(",@DeductFee").Append(text).Append(",@ItemListPrice").Append(text).Append(",@ItemAdjustedPrice").Append(text).Append(",@ItemDescription").Append(text).Append(",@ThumbnailsUrl").Append(text).Append(",@Weight").Append(text).Append(",@SKUContent").Append(text).Append(",@PromotionId").Append(text).Append(",@PromotionName").Append(text).Append(",@TaxRate").Append(text).Append(",@TemplateId").Append(text).Append(",@StoreId").Append(text).Append(",@SupplierId").Append(text).Append(");");
                    this.database.AddInParameter(sqlStringCommand, "SkuId" + text, DbType.String, lineItemInfo.SkuId);
                    this.database.AddInParameter(sqlStringCommand, "ProductId" + text, DbType.Int32, lineItemInfo.ProductId);
                    this.database.AddInParameter(sqlStringCommand, "SKU" + text, DbType.String, lineItemInfo.SKU);
                    this.database.AddInParameter(sqlStringCommand, "Quantity" + text, DbType.Int32, lineItemInfo.Quantity);
                    this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity" + text, DbType.Int32, lineItemInfo.ShipmentQuantity);
                    this.database.AddInParameter(sqlStringCommand, "CostPrice" + text, DbType.Currency, lineItemInfo.ItemCostPrice);
                    this.database.AddInParameter(sqlStringCommand, "DeductFee" + text, DbType.Decimal, lineItemInfo.DeductFee);
                    this.database.AddInParameter(sqlStringCommand, "ItemListPrice" + text, DbType.Currency, lineItemInfo.ItemListPrice);
                    this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice" + text, DbType.Currency, lineItemInfo.ItemAdjustedPrice);
                    this.database.AddInParameter(sqlStringCommand, "ItemDescription" + text, DbType.String, lineItemInfo.ItemDescription);
                    this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + text, DbType.String, lineItemInfo.ThumbnailsUrl);
                    this.database.AddInParameter(sqlStringCommand, "Weight" + text, DbType.Int32, lineItemInfo.ItemWeight);
                    this.database.AddInParameter(sqlStringCommand, "SKUContent" + text, DbType.String, lineItemInfo.SKUContent);
                    this.database.AddInParameter(sqlStringCommand, "PromotionId" + text, DbType.Int32, lineItemInfo.PromotionId);
                    this.database.AddInParameter(sqlStringCommand, "PromotionName" + text, DbType.String, lineItemInfo.PromotionName);
                    this.database.AddInParameter(sqlStringCommand, "TaxRate" + text, DbType.String, lineItemInfo.TaxRate);
                    this.database.AddInParameter(sqlStringCommand, "TemplateId" + text, DbType.Int32, lineItemInfo.TemplateId);
                    this.database.AddInParameter(sqlStringCommand, "StoreId" + text, DbType.Int32, lineItemInfo.storeId);
                    this.database.AddInParameter(sqlStringCommand, "SupplierId" + text, DbType.Int32, lineItemInfo.SupplierId);
                    num++;
                    if (num == 50)
                    {
                        sqlStringCommand.CommandText = stringBuilder.ToString();
                        int num2;
                        if (dbTran != null)
                        {
                            num2 = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                        }
                        else
                        {
                            num2 = this.database.ExecuteNonQuery(sqlStringCommand);
                        }
                        if (num2 <= 0)
                        {
                            result = false;
                            return result;
                        }
                        stringBuilder.Remove(0, stringBuilder.Length);
                        sqlStringCommand.Parameters.Clear();
                        this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                        num = 0;
                    }
                }
                if (stringBuilder.ToString().Length > 0)
                {
                    sqlStringCommand.CommandText = stringBuilder.ToString();
                    if (dbTran != null)
                    {
                        result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
                    }
                    else
                    {
                        result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
                    }
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }
	}
}

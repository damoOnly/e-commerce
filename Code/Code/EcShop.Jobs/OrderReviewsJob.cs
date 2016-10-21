using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Sales;
using EcShop.Core.ErrorLog;
using EcShop.Core.Jobs;
using EcShop.Entities;
using EcShop.Entities.Comments;
using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.Entities.Promotions;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Xml;
namespace EcShop.Jobs
{
    public class OrderReviewsJob : IJob
    {
        private Database database;
        private string reviewContent = "";//评论内容

        public void Execute(XmlNode node)
        {
            ErrorLog.Write("执行了OrderReviewsJob：" + DateTime.Now + "\n");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            this.database = DatabaseFactory.CreateDatabase();
            if (null != node)
            {
                XmlAttribute reviewContentAttribute = node.Attributes["reviewContent"];

                if (reviewContentAttribute != null)
                {
                    reviewContent = reviewContentAttribute.Value;
                }
            }
            //自动评价订单
            AutoReviewOrders(masterSettings.EndOrderDays);
        }

        /// <summary>
        /// 自动评价到期未评价的订单
        /// </summary>
        /// <param name="closeOrderDays"></param>
        private void AutoReviewOrders(int endOrderDays)
        {
            endOrderDays = endOrderDays == 0 ? 1 : endOrderDays;
            ErrorLog.Write("执行了OrderReviewsJob-->AutoReviewOrders：" + DateTime.Now + "\n,延迟时间：" + endOrderDays.ToString());
            List<string> orderIdList = new List<string>();
            //获取已完成，还未评价的订单
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 10 orderId FROM dbo.Ecshop_Orders AS a  WHERE a.OrderStatus=5 AND  NOT EXISTS (SELECT 1 FROM Ecshop_ProductReviews AS b  WHERE a.OrderId=b.OrderId) AND a.ShippingDate <= @ShippingDate;");
            this.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, DateTime.Now.AddDays(-endOrderDays));
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    if (!string.IsNullOrEmpty(dataReader["OrderId"].ToString()))
                    {
                        orderIdList.Add(dataReader["OrderId"].ToString());
                    }
                }
            }

            OrderInfo                      orderInfo = null;
            List<ProductReviewInfo> productReviewAll = null;
            LineItemInfo                lineItemInfo = null;
            ProductReviewInfo      productReviewInfo = null;
            foreach (string orderId in orderIdList)
            {
                orderInfo = TradeHelper.GetOrderInfo(orderId);
                ErrorLog.Write("执行了OrderReviewsJob-->GetOrderInfo：" + DateTime.Now + "\n");

                productReviewAll = GetProductReviewAll(orderId);
                if (productReviewAll.Count > 0)
                {
                    lineItemInfo = new LineItemInfo();
                    foreach (System.Collections.Generic.KeyValuePair<string, LineItemInfo> current in orderInfo.LineItems)
                    {
                        lineItemInfo = current.Value;
                        for (int i = 0; i < productReviewAll.Count; i++)
                        {
                            if (lineItemInfo.ProductId.ToString() == productReviewAll[i].ProductId.ToString() && lineItemInfo.SkuId.ToString().Trim() == productReviewAll[i].SkuId.ToString().Trim())
                            {
                                productReviewInfo = new ProductReviewInfo();
                                productReviewInfo.ReviewDate = System.DateTime.Now;
                                productReviewInfo.ProductId = lineItemInfo.ProductId;
                                productReviewInfo.UserId =orderInfo.UserId;
                                productReviewInfo.UserName = orderInfo.Username;
                                productReviewInfo.UserEmail = "";
                                productReviewInfo.ReviewText = this.reviewContent;
                                productReviewInfo.OrderID = orderId;
                                productReviewInfo.SkuId = lineItemInfo.SkuId;
                                productReviewInfo.Score = 5;

                                bool result = InsertProductReview(productReviewInfo);
                                if (!result)
                                {
                                    ErrorLog.Write("【到期未评价】系统未能自动评价订单：" + orderId + ";商品：" + lineItemInfo.ProductId);
                                }
                                break;
                            }
                        }

                    }
                }
                System.Threading.Thread.Sleep(new System.TimeSpan(0, 0, 0,4, 0));
            }
        }

        /// <summary>
        /// 获取商品明细
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public List<ProductReviewInfo> GetProductReviewAll(string orderid)
        {
            List<ProductReviewInfo> list = new List<ProductReviewInfo>();
            try
            {
                ErrorLog.Write("执行了OrderReviewsJob-->GetProductReviewAll：" + DateTime.Now + "\n");
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"select Productid,SkuId from Ecshop_OrderItems  AS a 
                                      WHERE not EXISTS (SELECT 1 FROM Ecshop_ProductReviews AS b WHERE (a.Productid=b.productId  OR a.SKuid=b.SKuId)  AND b.OrderId=@OrderId)
                                      and OrderId=@OrderId");
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
                this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);

                ProductReviewInfo info = null;
                using (IDbConnection conn = this.database.CreateConnection())
                {

                    conn.Open();
                    using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                    {
                        if (dataReader.Read())
                        {
                            info = new ProductReviewInfo();
                            if (dataReader["SkuId"] != System.DBNull.Value)
                            {
                                info.SkuId = (string)dataReader["SkuId"];
                            }
                            if (dataReader["ProductId"] != System.DBNull.Value)
                            {
                                info.ProductId = (int)dataReader["ProductId"];
                            }
                            list.Add(info);
                        }
                        dataReader.Dispose();
                        dataReader.Close();
                    }
                    conn.Dispose();
                    conn.Close();
                }

                System.Threading.Thread.Sleep(new System.TimeSpan(0, 0, 0, 3, 0));
            }
            catch (Exception ee)
            {
                ErrorLog.Write("执行了OrderReviewsJob-->GetProductReviewAll:异常："+ee.Message,ee);
            }
            return list;
        }
        /// <summary>
        /// 添加商品评价
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        public bool InsertProductReview(ProductReviewInfo review)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_ProductReviews (ProductId, UserId, ReviewText, UserName, UserEmail, ReviewDate,OrderId,SkuId,Score,IsAnonymous,IsType) VALUES(@ProductId, @UserId, @ReviewText, @UserName, @UserEmail, @ReviewDate,@OrderId,@SkuId,@Score,@IsAnonymous,1)");
                this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, review.ProductId);
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, review.UserId);
                this.database.AddInParameter(sqlStringCommand, "ReviewText", DbType.String, review.ReviewText);
                this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, (string.IsNullOrEmpty(review.UserName) ? "" : review.UserName));
                this.database.AddInParameter(sqlStringCommand, "UserEmail", DbType.String, (string.IsNullOrEmpty(review.UserEmail) ? "" : review.UserEmail));
                this.database.AddInParameter(sqlStringCommand, "ReviewDate", DbType.DateTime, DateTime.Now);
                this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, review.OrderID);
                this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, review.SkuId);
                this.database.AddInParameter(sqlStringCommand, "Score", DbType.Int32, review.Score);
                this.database.AddInParameter(sqlStringCommand, "IsAnonymous", DbType.Boolean, review.IsAnonymous);
                bool issucess = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
                return issucess;
            }
            catch (Exception ee)
            {
                ErrorLog.Write("添加商品评价:InsertProductReview:"+ee.Message,ee);
            }
            return false;
        }
    }
}

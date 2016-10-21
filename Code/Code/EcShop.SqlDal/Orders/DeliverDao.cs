using EcShop.Core.ErrorLog;
using EcShop.Entities.Orders;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EcShop.SqlDal.Orders
{
    public class DeliverDao
    {
        private Database database;
        public DeliverDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}


        /// <summary>
        /// 批量出库订单状态插入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool AddDeliverStatus(List<DeliverStatusInfo> list, out List<ErrorDeliverStatusInfo> errorlist)
        {
            errorlist = new List<ErrorDeliverStatusInfo>();
            bool result = false;
            using (DbConnection connection = this.database.CreateConnection())
            {
                //打开链接
                connection.Open();
                //创建事务
                DbTransaction Tran = connection.BeginTransaction();

                try
                {


                    foreach (DeliverStatusInfo item in list)
                    {
                        DbCommand sqlStringCommand1 = this.database.GetSqlStringCommand("select count(OrderId) from Ecshop_DeliverStatus where OrderId=@OrderId and OrderStatus=@OrderStatus");
                        this.database.AddInParameter(sqlStringCommand1, "OrderId", DbType.String, item.OrderId);
                        this.database.AddInParameter(sqlStringCommand1, "OrderStatus", DbType.Int32, item.OrderStatus);
                       int count = 0;
                        int.TryParse(this.database.ExecuteScalar(sqlStringCommand1, Tran).ToString(),out count);
                        if (count > 0)
                        {
                            ErrorDeliverStatusInfo errorDeliverStatusInfo = new ErrorDeliverStatusInfo();
                            errorDeliverStatusInfo.OrderId = item.OrderId;
                            errorDeliverStatusInfo.OrderStatus = item.OrderStatus;
                            errorDeliverStatusInfo.Describe = item.Describe;
                            errorDeliverStatusInfo.Warehouse = item.Warehouse;
                            errorDeliverStatusInfo.ShipOrderNumber = item.ShipOrderNumber;
                            errorDeliverStatusInfo.ExpressCompanyName = item.ExpressCompanyName;
                            errorDeliverStatusInfo.UpdateDate = item.UpdateDate;
                            errorDeliverStatusInfo.errorcode = "0001";
                            errorDeliverStatusInfo.errordescr = "该订单状态已存在";

                            errorlist.Add(errorDeliverStatusInfo);
                        }

                        else
                        {
                            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into  Ecshop_DeliverStatus(OrderId,OrderStatus,Describe,Warehouse,UpdateDate,ShipOrderNumber,ExpressCompanyName) values(@OrderId,@OrderStatus,@Describe,@Warehouse,@UpdateDate,@ShipOrderNumber,@ExpressCompanyName)");
                            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, item.OrderId);
                            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, item.OrderStatus);
                            this.database.AddInParameter(sqlStringCommand, "Describe", DbType.String, item.Describe);
                            this.database.AddInParameter(sqlStringCommand, "Warehouse", DbType.String, item.Warehouse);
                            this.database.AddInParameter(sqlStringCommand, "UpdateDate", DbType.DateTime, item.UpdateDate);
                            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, item.ShipOrderNumber);
                            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, item.ExpressCompanyName);

                            this.database.ExecuteNonQuery(sqlStringCommand, Tran);

                            //修改订单表WMSStatus状态
                            DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(" UPDATE Ecshop_Orders SET WMSStatus=@OrderStatus  WHERE OrderId=@OrderId ");
                            this.database.AddInParameter(sqlStringCommand2, "OrderId", DbType.String, item.OrderId);
                            this.database.AddInParameter(sqlStringCommand2, "OrderStatus", DbType.Int32, item.OrderStatus);
                            this.database.ExecuteNonQuery(sqlStringCommand2, Tran);


                            //修改订单快递单号
                            if (!string.IsNullOrWhiteSpace(item.ShipOrderNumber))
                            {
                                DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand(" UPDATE Ecshop_Orders SET ShipOrderNumber=@ShipOrderNumber,ExpressCompanyName=@ExpressCompanyName,ShipOrderStatus=@ShipOrderStatus  WHERE OrderId=@OrderId and ShipOrderStatus=0 ");
                                this.database.AddInParameter(sqlStringCommand3, "OrderId", DbType.String, item.OrderId);
                                this.database.AddInParameter(sqlStringCommand3, "ShipOrderNumber", DbType.String, item.ShipOrderNumber);
                                this.database.AddInParameter(sqlStringCommand3, "ExpressCompanyName", DbType.String, item.ExpressCompanyName);
                                this.database.AddInParameter(sqlStringCommand3, "ShipOrderStatus", DbType.Int32, 1);
                                this.database.ExecuteNonQuery(sqlStringCommand3, Tran);
                            }

                        }
                    }


                    //提交事务
                    Tran.Commit();

                    result = true;
                }
                catch (Exception Ex)
                {
                    //出错回滚
                    Tran.Rollback();
                    ErrorLog.Write(Ex.ToString());
                }
                finally
                {
                    //关闭连接
                    connection.Close();
                }

                return result;
            }
        }
    }
}

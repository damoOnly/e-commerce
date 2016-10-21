using EcShop.Core;
using EcShop.Core.ErrorLog;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.SqlDal.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace EcShop.SqlDal.Commodities
{
    public class SkuDao
    {
        private Database database;
        public SkuDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public SKUItem GetSkuItem(string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_SKUs WHERE SkuId=@SkuId;");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            SKUItem result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    result = DataMapper.PopulateSKU(dataReader);
                }
            }
            return result;
        }

        public SKUItem GetSkuItem(string skuId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_SKUs WHERE SkuId=@SkuId;");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            SKUItem result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand, dbTran))
            {
                if (dataReader.Read())
                {
                    result = DataMapper.PopulateSKU(dataReader);
                }
            }
            return result;
        }


        public DataTable GetProductInfoBySku(string skuId)
        {
            DataTable result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr FROM Ecshop_SKUs s left join Ecshop_SKUItems si on s.SkuId = si.SkuId left join Ecshop_Attributes a on si.AttributeId = a.AttributeId left join Ecshop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId AND s.ProductId IN (SELECT ProductId FROM Ecshop_Products WHERE SaleStatus=1)");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public SKUItem GetProductAndSku(Member currentMember, int productId, string options)
        {
            SKUItem result;
            if (string.IsNullOrEmpty(options))
            {
                result = null;
            }
            else
            {
                string[] array = options.Split(new char[]
				{
					','
				});
                if (array == null || array.Length <= 0)
                {
                    result = null;
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    if (currentMember != null)
                    {
                        int discount = new MemberGradeDao().GetMemberGrade(currentMember.GradeId).Discount;
                        stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock,FactStock,CostPrice,DeductFee,");
                        stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", currentMember.GradeId);
                        stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Ecshop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", currentMember.GradeId, discount);
                        stringBuilder.Append(" FROM Ecshop_SKUs s WHERE ProductId = @ProductId");
                    }
                    else
                    {
                        stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice, SalePrice,DeductFee  FROM Ecshop_SKUs WHERE ProductId = @ProductId");
                    }
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string text = array2[i];
                        string[] array3 = text.Split(new char[]
						{
							':'
						});
                        // 避免注入攻击
                        int attId = 0;
                        if (!string.IsNullOrEmpty(array3[0]))
                        {
                            attId = int.Parse(array3[0]);
                        }
                        int valueId = 0;
                        if (!string.IsNullOrEmpty(array3[1]))
                        {
                            valueId = int.Parse(array3[1]);
                        }
                        stringBuilder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Ecshop_SKUItems WHERE AttributeId = {0} AND ValueId = {1}) ", attId, valueId);
                    }
                    SKUItem sKUItem = null;
                    DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
                    this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
                    using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                    {
                        if (dataReader.Read())
                        {
                            sKUItem = DataMapper.PopulateSKU(dataReader);
                        }
                    }
                    result = sKUItem;
                }
            }
            return result;
        }
        public DataTable GetSkus(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl FROM Ecshop_SKUItems s join Ecshop_Attributes a on s.AttributeId = a.AttributeId join Ecshop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Ecshop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public DataTable GetUniqueSkus(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DISTINCT a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl, a.DisplaySequence FROM Ecshop_SKUItems s join Ecshop_Attributes a on s.AttributeId = a.AttributeId join Ecshop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Ecshop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 根据商品Id获取sku的信息
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns></returns>
        public DataTable GetSkusByProductId(int productId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT s.SkuId, s.ProductId, s.SKU,isnull(s.Weight,0) [Weight],ISNULL(s.GrossWeight,0) GrossWeight, s.Stock, s.CostPrice,s.saleprice,p.ThumbnailUrl40,p.ProductName ");
            stringBuilder.Append(" FROM Ecshop_SKUs s inner join Ecshop_products p on s.productid = p.productid  WHERE s.ProductId = @ProductId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        public DataTable GetExtendSkusByProductId(int productId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT s.SkuId, s.ProductId, s.SKU,s.Weight, s.Stock, s.CostPrice,s.saleprice,p.marketprice ");
            stringBuilder.Append(" FROM Ecshop_SKUs s inner join Ecshop_Products p on p.ProductId=s.ProductId WHERE s.ProductId = @ProductId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }


        public DataTable GetExpandAttributes(int productId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" SELECT a.AttributeId, AttributeName, ValueStr FROM Ecshop_ProductAttributes pa JOIN Ecshop_Attributes a ON pa.AttributeId = a.AttributeId");
            stringBuilder.Append(" JOIN Ecshop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            DataTable dataTable;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            DataTable dataTable2 = new DataTable();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                dataTable2 = dataTable.Clone();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    bool flag = false;
                    if (dataTable2.Rows.Count > 0)
                    {
                        foreach (DataRow dataRow2 in dataTable2.Rows)
                        {
                            if ((int)dataRow2["AttributeId"] == (int)dataRow["AttributeId"])
                            {
                                flag = true;
                                DataRow dataRow3;
                                (dataRow3 = dataRow2)["ValueStr"] = dataRow3["ValueStr"] + ", " + dataRow["ValueStr"];
                            }
                        }
                    }
                    if (!flag)
                    {
                        DataRow dataRow4 = dataTable2.NewRow();
                        dataRow4["AttributeId"] = dataRow["AttributeId"];
                        dataRow4["AttributeName"] = dataRow["AttributeName"];
                        dataRow4["ValueStr"] = dataRow["ValueStr"];
                        dataTable2.Rows.Add(dataRow4);
                    }
                }
            }
            return dataTable2;
        }
        public IList<string> GetSkuIdsBysku(string sku)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SkuId FROM Ecshop_SKUs WHERE SKU = @SKU");
            this.database.AddInParameter(sqlStringCommand, "SKU", DbType.String, sku);
            IList<string> list = new List<string>();
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    list.Add((string)dataReader["SkuId"]);
                }
            }
            return list;
        }
        public DataTable GetUnUpUnUpsellingSkus(int productId, int attributeId, int valueId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Ecshop_SKUItems WHERE SKUId IN (SELECT SKUId FROM Ecshop_SKUs WHERE ProductId = @ProductId) AND (SKUId in (SELECT SKUId FROM Ecshop_SKUItems WHERE AttributeId = @AttributeId AND ValueId=@ValueId) OR AttributeId = @AttributeId)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, valueId);
            DataTable result = null;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        public Dictionary<string, decimal> GetCostPriceForItems(string skuIds)
        {
            Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT s.SkuId, s.CostPrice FROM Ecshop_SKUs s WHERE SkuId IN ({0})", skuIds));
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (dataReader.Read())
                {
                    decimal value = (dataReader["CostPrice"] == DBNull.Value) ? 0m : ((decimal)dataReader["CostPrice"]);
                    dictionary.Add((string)dataReader["SkuId"], value);
                }
            }
            return dictionary;
        }


        /// <summary>
        /// 根据规格属性，商品Id,返回库存
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="attributeId"></param>
        /// <param name="valueId"></param>
        /// <returns></returns>
        public int GetSkusStock(int productId, int attributeId, int valueId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select isnull(a.stock,0) stock FROM Ecshop_SKUs a join Ecshop_SKUItems b on a.SkuId = b.SkuId where b.AttributeId = @AttributeId and b.ValueId = @ValueId and a.ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, valueId);
            int result = 0;

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


       



        /// <summary>
        /// 入库推送
        /// </summary>
        /// <param name="skulist"></param>
        /// <returns></returns>
        public bool AddSkusStock(List<SimpleSKUUpdateInfo> skulist, out List<ErrorSimpleSKUUpdateInfo> errorskulist)
        {
            errorskulist = new List<ErrorSimpleSKUUpdateInfo>();
            bool result = false;
            using (DbConnection connection = this.database.CreateConnection())
            {
                //打开链接
                connection.Open();
                //创建事务
                DbTransaction Tran = connection.BeginTransaction();

                try
                {


                    foreach (var item in skulist)
                    {
                        //查询sku是否存在
                        DbCommand sqlStringCommand1 = this.database.GetSqlStringCommand("select COUNT(SKuId) from Ecshop_SKUs A  inner join Ecshop_Products B  on A.ProductId=B.ProductId where A.SKuId=@SKuId");
                        this.database.AddInParameter(sqlStringCommand1, "SKuId", DbType.String, item.SkuId);
                        int count = 0;
                        int.TryParse(this.database.ExecuteScalar(sqlStringCommand1, Tran).ToString(), out count);
                        if (count > 0)
                        {
                            DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("select top 1 B.ConversionRelation from Ecshop_SKUs A  inner join Ecshop_Products B  on A.ProductId=B.ProductId where A.SKuId=@SKuId");
                            this.database.AddInParameter(sqlStringCommand2, "SKuId", DbType.String, item.SkuId);
                            int conversionRelation = 0;
                            object objconversionRelation = this.database.ExecuteScalar(sqlStringCommand2, Tran);
                            if (objconversionRelation != null)
                            {
                                int.TryParse(objconversionRelation.ToString(), out conversionRelation);
                            }

                            if (conversionRelation == 0)
                            {
                                conversionRelation = 1;
                            }


                            DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("update Ecshop_SKUs set Stock=Stock+@Amount,FactStock=FactStock+@Amount,WMSStock=WMSStock+@Amount where SKuId=@SKuId");
                            this.database.AddInParameter(sqlStringCommand3, "Amount", DbType.Int32, (item.Amount) / conversionRelation);
                            this.database.AddInParameter(sqlStringCommand3, "SKuId", DbType.String, item.SkuId);
                            this.database.ExecuteNonQuery(sqlStringCommand3, Tran);
                        }

                        else
                        {
                            ErrorSimpleSKUUpdateInfo errorSimpleSKUUpdateInfo = new ErrorSimpleSKUUpdateInfo();
                            errorSimpleSKUUpdateInfo.Amount = item.Amount;
                            errorSimpleSKUUpdateInfo.SkuId = item.SkuId;

                            errorskulist.Add(errorSimpleSKUUpdateInfo);
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

        /// <summary>
        /// 入库推送修改库存
        /// </summary>
        /// <param name="skulist"></param>
        /// <returns></returns>
        public bool UpdateSkusStock(string PONumber, List<SimpleSKUUpdateInfo> skulist)
        {
            bool result = false;
            using (DbConnection connection = this.database.CreateConnection())
            {
                //打开链接
                connection.Open();
                //创建事务
                DbTransaction Tran = connection.BeginTransaction();

                try
                {
                    //修改采购入库状态
                    DbCommand sqlStringCommand1 = this.database.GetSqlStringCommand("update EcShop_PurchaseOrder set POStatus=8 where PONumber=@PONumber");
                    this.database.AddInParameter(sqlStringCommand1, "PONumber", DbType.String, PONumber);
                    this.database.ExecuteNonQuery(sqlStringCommand1, Tran);

                    foreach (var item in skulist)
                    {
                        //修改入库记录
                        DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("update EcShop_PurchaseOrderItems set PracticalQuantity=@PracticalQuantity where SkuId=@SkuId");
                        this.database.AddInParameter(sqlStringCommand3, "PracticalQuantity", DbType.Int32, item.Amount);
                        this.database.AddInParameter(sqlStringCommand3, "SKuId", DbType.String, item.SkuId);
                        this.database.ExecuteNonQuery(sqlStringCommand3, Tran);
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

        /// <summary>
        /// 库存同步
        /// </summary>
        /// <param name="skulist"></param>
        /// <returns></returns>
        public bool AdjustSkusStock(List<SimpleSKUUpdateInfo> skulist, out List<ErrorSimpleSKUUpdateInfo> errorskulist)
        {
            errorskulist = new List<ErrorSimpleSKUUpdateInfo>();
            bool result = false;
            using (DbConnection connection = this.database.CreateConnection())
            {
                //打开链接
                connection.Open();
                //创建事务
                DbTransaction Tran = connection.BeginTransaction();

                try
                {


                    foreach (var item in skulist)
                    {
                        SKUItem skuItem = new SKUItem();
                        skuItem = this.GetSkuItem(item.SkuId, Tran);

                        if (skuItem == null)
                        {
                            ErrorSimpleSKUUpdateInfo errorSimpleSKUUpdateInfo = new ErrorSimpleSKUUpdateInfo();
                            errorSimpleSKUUpdateInfo.Amount = item.Amount;
                            errorSimpleSKUUpdateInfo.SkuId = item.SkuId;
                            errorSimpleSKUUpdateInfo.errorcode = "0001";
                            errorSimpleSKUUpdateInfo.errordescr = "SKU不存在";

                            errorskulist.Add(errorSimpleSKUUpdateInfo);
                        }
                        else
                        {
                            DbCommand sqlStringCommand1 = this.database.GetSqlStringCommand("select top 1 B.ConversionRelation from Ecshop_SKUs A  inner join Ecshop_Products B  on A.ProductId=B.ProductId where A.SKuId=@SKuId");
                            this.database.AddInParameter(sqlStringCommand1, "SKuId", DbType.String, item.SkuId);
                            int conversionRelation = 0;
                            object objconversionRelation = this.database.ExecuteScalar(sqlStringCommand1, Tran);
                            if (objconversionRelation != null)
                            {
                                int.TryParse(objconversionRelation.ToString(), out conversionRelation);
                            }

                            if (conversionRelation == 0)
                            {
                                conversionRelation = 1;
                            }

                            //if (skuItem.Stock > item.Amount && skuItem.FactStock > item.Amount)
                            int convertCount = item.Amount / conversionRelation;  //WMS库存除以转换系数


                            #region 注释代码
                            //计算未付款的商品数量
                            //DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(" select count(ShipmentQuantity) from  dbo.Ecshop_OrderItems A  inner join  dbo.Ecshop_Orders B on A.OrderId=B.OrderId where B.OrderStatus=1 and A.SkuId=@SKuId");
                            //this.database.AddInParameter(sqlStringCommand2, "SKuId", DbType.String, item.SkuId);
                            //int waitpaycount = 0;
                            //object objwaitpaidcount = this.database.ExecuteScalar(sqlStringCommand2, Tran);
                            //if (objwaitpaidcount != null)
                            //{
                            //    int.TryParse(objwaitpaidcount.ToString(), out waitpaycount);
                            //}


                            //计算已付款未推送至wms的商品数量
                            //DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand(" select count(ShipmentQuantity) from  dbo.Ecshop_OrderItems A  inner join  dbo.Ecshop_Orders B on A.OrderId=B.OrderId where B.OrderStatus=2 and B.IsSendWMS=0 and A.SkuId=@SKuId");
                            //this.database.AddInParameter(sqlStringCommand3, "SKuId", DbType.String, item.SkuId);
                            //int paidcount = 0;
                            //object objpaidcount = this.database.ExecuteScalar(sqlStringCommand3, Tran);
                            //if (objpaidcount != null)
                            //{
                            //    int.TryParse(objpaidcount.ToString(), out paidcount);
                            //}



                            //if (skuItem.Stock + waitpaycount + paidcount > convertCount)
                            //{

                            //DbCommand sqlStringCommand4 = this.database.GetSqlStringCommand("update Ecshop_SKUs set Stock=@Amount where SKuId=@SKuId");
                            //this.database.AddInParameter(sqlStringCommand4, "Amount", DbType.Int32, (convertCount > paidcount + waitpaycount) ? (convertCount - paidcount - waitpaycount) : 0);
                            //this.database.AddInParameter(sqlStringCommand4, "SKuId", DbType.String, item.SkuId);
                            //this.database.ExecuteNonQuery(sqlStringCommand4, Tran);

                            //if(skuItem.FactStock+paidcount> convertCount)
                            //{
                            //DbCommand sqlStringCommand5 = this.database.GetSqlStringCommand("update Ecshop_SKUs set factStock=@Amount where SKuId=@SKuId");
                            //this.database.AddInParameter(sqlStringCommand5, "Amount", DbType.Int32, (convertCount > paidcount) ? (convertCount - paidcount) : 0);
                            //this.database.AddInParameter(sqlStringCommand5, "SKuId", DbType.String, item.SkuId);
                            //this.database.ExecuteNonQuery(sqlStringCommand5, Tran);
                            //}
                            //}

                            //else
                            //{
                            //    ErrorSimpleSKUUpdateInfo errorSimpleSKUUpdateInfo = new ErrorSimpleSKUUpdateInfo();
                            //    errorSimpleSKUUpdateInfo.Amount = item.Amount;
                            //    errorSimpleSKUUpdateInfo.SkuId = item.SkuId;
                            //    errorSimpleSKUUpdateInfo.errorcode = "0002";
                            //    errorSimpleSKUUpdateInfo.errordescr = "推送sku库存比站点库存大,未同步库存";

                            //    errorskulist.Add(errorSimpleSKUUpdateInfo);
                            //}
                            #endregion 


                            DbCommand sqlStringCommand4 = this.database.GetSqlStringCommand("update Ecshop_SKUs set WMSStock=@Amount where SKuId=@SKuId");
                            this.database.AddInParameter(sqlStringCommand4, "Amount", DbType.Int32, convertCount);
                            this.database.AddInParameter(sqlStringCommand4, "SKuId", DbType.String, item.SkuId);
                            this.database.ExecuteNonQuery(sqlStringCommand4, Tran);
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


        /// <summary>
        /// 根据skuid获取库存，添加组合商品逻辑
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public int GetMinStockBySku(string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select case when p.saletype=2 then   
(select min(s1.Stock/pc.Quantity)  from  dbo.Ecshop_SKUs s1 inner join

 Ecshop_ProductsCombination pc 
 
 on pc.Skuid=s1.Skuid

where pc.CombinationSkuId=s.Skuid)   

else s.stock  end  as stock,

case when p.saletype=2 then   
(select min(s1.WMSStock/pc.Quantity)  from  dbo.Ecshop_SKUs s1 inner join

 Ecshop_ProductsCombination pc 
 
 on pc.Skuid=s1.Skuid

where pc.CombinationSkuId=s.Skuid)   

else s.WMSStock  end  as WMSstock

from dbo.Ecshop_SKUs s inner join

dbo.Ecshop_Products p on p.productid=s.productid WHERE s.SkuId=@SkuId;");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);

            

            int stock = 0;
            int wmsstock=0;

            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (dataReader.Read())
                {
                    object stockobject;
                    stockobject = dataReader[0];
                    if (stockobject != null)
                    {
                        int.TryParse(stockobject.ToString(), out stock);
                    }

                    else
                    {
                        stock = 0;
                    }

                    stockobject = dataReader[1];

                    if (stockobject != null)
                    {
                        int.TryParse(stockobject.ToString(), out wmsstock);
                    }

                    else
                    {
                        wmsstock = 0;
                    }
                }
            }
           

           

            return (stock < wmsstock ? stock : wmsstock);
           
        }


        /// <summary>
        /// WMS设置毛重
        /// </summary>
        /// <param name="skulist"></param>
        /// <param name="errorskulist"></param>
        /// <returns></returns>
        public bool SetGrossWeight(List<SimpleSKUUpdateInfo> skulist, out List<ErrorSimpleSKUUpdateInfo> errorskulist)
        {
            errorskulist = new List<ErrorSimpleSKUUpdateInfo>();
            bool result = false;
            using (DbConnection connection = this.database.CreateConnection())
            {
                //打开链接
                connection.Open();
                //创建事务
                DbTransaction Tran = connection.BeginTransaction();

                try
                {


                    foreach (var item in skulist)
                    {
                        //查询sku是否存在
                        DbCommand sqlStringCommand1 = this.database.GetSqlStringCommand("select COUNT(SKuId) from Ecshop_SKUs A  inner join Ecshop_Products B  on A.ProductId=B.ProductId where A.SKuId=@SKuId");
                        this.database.AddInParameter(sqlStringCommand1, "SKuId", DbType.String, item.SkuId);
                        int count = 0;
                        int.TryParse(this.database.ExecuteScalar(sqlStringCommand1, Tran).ToString(), out count);
                        if (count > 0)
                        {

                            DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("update Ecshop_SKUs set GrossWeight=@GrossWeight where SKuId=@SKuId");
                            this.database.AddInParameter(sqlStringCommand2, "GrossWeight", DbType.Decimal, item.GrossWeight);
                            this.database.AddInParameter(sqlStringCommand2, "SKuId", DbType.String, item.SkuId);
                            this.database.ExecuteNonQuery(sqlStringCommand2, Tran);
                        }

                        else
                        {
                            ErrorSimpleSKUUpdateInfo errorSimpleSKUUpdateInfo = new ErrorSimpleSKUUpdateInfo();
                            errorSimpleSKUUpdateInfo.GrossWeight = item.GrossWeight;
                            errorSimpleSKUUpdateInfo.SkuId = item.SkuId;

                            errorskulist.Add(errorSimpleSKUUpdateInfo);
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

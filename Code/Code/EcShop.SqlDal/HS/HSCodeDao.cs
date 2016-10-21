using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.HS;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EcShop.SqlDal.HS
{
    public class HSCodeDao
    {
        private Database database;
        public HSCodeDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }

        public IList<HSCodeInfo> GetHSCode()
        {
            IList<HSCodeInfo> result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM FND_HS_CODE where VOIDED=0");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<HSCodeInfo>(dataReader);
            }
            return result;
        }

        public IList<ElmentsInfo> GetElments()
        {
            IList<ElmentsInfo> result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT HS_ELMENTS_ID, HS_ELMENTS_NAME, HS_ELMENTS_DESC FROM FND_HS_ELMENTS");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<ElmentsInfo>(dataReader);
            }
            return result;
        }


        public IList<ElmentsInfo> GetElments(string HSElmentsName)
        {
            IList<ElmentsInfo> result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT HS_ELMENTS_ID, HS_ELMENTS_NAME, HS_ELMENTS_DESC FROM FND_HS_ELMENTS where HS_ELMENTS_NAME like '%" + DataHelper.CleanSearchString(HSElmentsName) + "%'");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<ElmentsInfo>(dataReader);
            }
            return result;
        }

        public DbQueryResult GetHSCode(HSCodeQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" VOIDED=0 ");

            if (!string.IsNullOrEmpty(query.HS_CODE))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" HS_CODE LIKE '%{0}%'", DataHelper.CleanSearchString(query.HS_CODE));
            }
            if (!string.IsNullOrEmpty(query.HS_NAME))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" HS_NAME LIKE '%{0}%'", DataHelper.CleanSearchString(query.HS_NAME));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "FND_HS_CODE ", "HS_CODE_ID", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
        }

        public DbQueryResult GetHSDocking(HSDockingQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(query.OrderId))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" OrderId LIKE '%{0}%'", DataHelper.CleanSearchString(query.OrderId));
            }
            //if (!string.IsNullOrEmpty(query.OrderPacketsID.ToString()))
            //{
            //    if (stringBuilder.Length > 0)
            //    {
            //        stringBuilder.Append(" AND ");
            //    }
            //    stringBuilder.AppendFormat(" OrderPacketsID LIKE '%{0}%'", DataHelper.CleanSearchString(query.OrderPacketsID.ToString()));
            //}
            if (!string.IsNullOrEmpty(query.OrderStatus.ToString()) && query.OrderStatus.ToString() != "-1")
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" OrderStatus = {0} ", int.Parse(query.OrderStatus.ToString()));
            }
            if (!string.IsNullOrEmpty(query.LogisticsStatus.ToString()) && query.LogisticsStatus.ToString() != "-1")
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" LogisticsStatus = {0} ", int.Parse(query.LogisticsStatus.ToString()));
            }
            if (!string.IsNullOrEmpty(query.PaymentStatus.ToString()) && query.PaymentStatus.ToString() != "-1")
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" PaymentStatus = {0} ", int.Parse(query.PaymentStatus.ToString()));
            }

            if (!string.IsNullOrEmpty(query.payerIdStatus.ToString()) && query.payerIdStatus.ToString() != "-1")
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" payerIdStatus = {0} ", int.Parse(query.payerIdStatus.ToString()));
            }

            //return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "FND_HS_CODE ", "HS_CODE_ID", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Ecshop_OrderHS_Docking ", "HS_Docking_ID", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
        }

        public DbQueryResult GetHSDeclare(HSDeclareQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Pagination page = query.Page;
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" OrderId LIKE '%{0}%'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (query.FromDate.HasValue)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat("  OrderDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
            }
            if (query.ToDate.HasValue)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat("  OrderDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
            }

            page.SortBy = "OrderDate";
            page.SortOrder = EcShop.Core.Enums.SortAction.Desc;
            if (!string.IsNullOrEmpty(query.DeclareStatus) && query.DeclareStatus != "-1")
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" DeclareStatus = {0} ", int.Parse(query.DeclareStatus.ToString()));
                //按申报状态来控制排序
                if (query.DeclareStatus.ToString() == "0" || query.DeclareStatus.ToString() == "4" || query.DeclareStatus.ToString() == "1" || query.DeclareStatus.ToString() == "3")
                {
                    page.SortBy = "OrderDate";
                    page.SortOrder = EcShop.Core.Enums.SortAction.Asc;
                }
            }

            if (!string.IsNullOrEmpty(query.WMSStatus) && query.WMSStatus != "-1")
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                if (query.WMSStatus == "1")//放行
                {
                    stringBuilder.AppendFormat(" isSendWMS = 2 ");
                }
                else
                {
                    stringBuilder.AppendFormat(" isSendWMS = 1 and DeclareStatus = 2 ");
                }
            }

            if (!string.IsNullOrEmpty(query.OperationUserName))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" DeclareName LIKE '%{0}%'", DataHelper.CleanSearchString(query.OperationUserName));
            }

            if (!string.IsNullOrEmpty(query.OutNo))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" OutNo LIKE '%{0}%'", DataHelper.CleanSearchString(query.OutNo));
            }

            if (!string.IsNullOrEmpty(query.LogisticsNo))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" LogisticsNo LIKE '%{0}%'", DataHelper.CleanSearchString(query.LogisticsNo));
            }

            if (!string.IsNullOrEmpty(query.ShipOrderNumber))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" ShipOrderNumber LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipOrderNumber));
            }

            if (query.SuspensionTime != null && query.SuspensionTime != 0)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" AND ");
                }
                stringBuilder.AppendFormat(" DATEADD(MINUTE,{0},PayDate) <GETDATE()", query.SuspensionTime);
            }




            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Ecshop_OrderHS_Declare ", "HS_Docking_ID", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
            //return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Ecshop_OrderHS_Declare", "HS_Docking_ID", stringBuilder.ToString(), "*");
        }
        public IList<HSCodeInfo> GetHSCode(string HS_CODE)
        {
            IList<HSCodeInfo> result = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM FND_HS_CODE where HS_CODE=@HS_CODE and VOIDED=0");
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<HSCodeInfo>(dataReader);
            }
            return result;
        }

        public bool UpdateHSCodeInfo(HSCodeInfo hscodeinfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE FND_HS_CODE SET HS_CODE=@HS_CODE ,HS_NAME=@HS_NAME ,LOW_RATE=@LOW_RATE ,HIGH_RATE=@HIGH_RATE ,OUT_RATE=@OUT_RATE ,TAX_RATE=@TAX_RATE ,TSL_RATE=@TSL_RATE ,UNIT_1=@UNIT_1 ,UNIT_2=@UNIT_2 ,CONTROL_MA=@CONTROL_MA ,NOTE_S=@NOTE_S ,TEMP_IN_RATE=@TEMP_IN_RATE ,TEMP_OUT_RATE=@TEMP_OUT_RATE ,CONTROL_INSPECTION=@CONTROL_INSPECTION ,CONSUMPTION_RATE=@CONSUMPTION_RATE where HS_CODE_ID=@HS_CODE_ID");

            this.database.AddInParameter(sqlStringCommand, "HS_CODE_ID", DbType.Int32, hscodeinfo.HS_CODE_ID);
            this.database.AddInParameter(sqlStringCommand, "HS_CODE", DbType.String, hscodeinfo.HS_CODE);
            this.database.AddInParameter(sqlStringCommand, "HS_NAME", DbType.String, hscodeinfo.HS_NAME);
            this.database.AddInParameter(sqlStringCommand, "LOW_RATE", DbType.Decimal, hscodeinfo.LOW_RATE);
            this.database.AddInParameter(sqlStringCommand, "HIGH_RATE", DbType.Decimal, hscodeinfo.HIGH_RATE);
            this.database.AddInParameter(sqlStringCommand, "OUT_RATE", DbType.Decimal, hscodeinfo.OUT_RATE);
            this.database.AddInParameter(sqlStringCommand, "TAX_RATE", DbType.Decimal, hscodeinfo.TAX_RATE);
            this.database.AddInParameter(sqlStringCommand, "TSL_RATE", DbType.Decimal, hscodeinfo.TSL_RATE);
            this.database.AddInParameter(sqlStringCommand, "UNIT_1", DbType.String, hscodeinfo.UNIT_1);
            this.database.AddInParameter(sqlStringCommand, "UNIT_2", DbType.String, hscodeinfo.UNIT_2);
            this.database.AddInParameter(sqlStringCommand, "CONTROL_MA", DbType.String, hscodeinfo.CONTROL_MA);
            this.database.AddInParameter(sqlStringCommand, "TEMP_IN_RATE", DbType.Decimal, hscodeinfo.TEMP_IN_RATE);
            this.database.AddInParameter(sqlStringCommand, "TEMP_OUT_RATE", DbType.Decimal, hscodeinfo.TEMP_OUT_RATE);
            this.database.AddInParameter(sqlStringCommand, "NOTE_S", DbType.String, hscodeinfo.NOTE_S);
            this.database.AddInParameter(sqlStringCommand, "CONTROL_INSPECTION", DbType.String, hscodeinfo.CONTROL_INSPECTION);
            this.database.AddInParameter(sqlStringCommand, "CONSUMPTION_RATE", DbType.Decimal, hscodeinfo.CONSUMPTION_RATE);

            int result = this.database.ExecuteNonQuery(sqlStringCommand);

            if (result > 0)
            {
                if (hscodeinfo.FND_HS_ELMENTS != null)
                {
                    DbCommand sqlStringCommand1 = this.database.GetSqlStringCommand("DELETE FROM FND_HS_CODE_ELMENTS WHERE HS_CODE_ID = @HS_CODE_ID");
                    this.database.AddInParameter(sqlStringCommand1, "HS_CODE_ID", DbType.Int32, hscodeinfo.HS_CODE_ID);
                    result = this.database.ExecuteNonQuery(sqlStringCommand1);

                    if (result >= 0)
                    {
                        DataSet ds = hscodeinfo.FND_HS_ELMENTS;

                        DataTable dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            DbCommand sqlStringCommand2 =
                            this.database.GetSqlStringCommand("INSERT INTO FND_HS_CODE_ELMENTS ( HS_ELMENTS_ID, HS_ELMENTS_NAME , HS_ELMENTS_DESC , HS_CODE_ID , VOIDED ) VALUES (@HS_ELMENTS_ID, @HS_ELMENTS_NAME, @HS_ELMENTS_DESC, @HS_CODE_ID, @VOIDED)");
                            this.database.AddInParameter(sqlStringCommand2, "HS_ELMENTS_ID", DbType.Int32, dr["HS_ELMENTS_ID"]);
                            this.database.AddInParameter(sqlStringCommand2, "HS_ELMENTS_NAME", DbType.String, dr["HS_ELMENTS_NAME"].ToString());
                            this.database.AddInParameter(sqlStringCommand2, "HS_ELMENTS_DESC", DbType.String, dr["HS_ELMENTS_DESC"].ToString());
                            this.database.AddInParameter(sqlStringCommand2, "HS_CODE_ID", DbType.Int32, hscodeinfo.HS_CODE_ID);
                            this.database.AddInParameter(sqlStringCommand2, "VOIDED", DbType.Int32, 0);

                            result = this.database.ExecuteNonQuery(sqlStringCommand2);
                        }

                        if (result > 0)
                        {
                            return true;
                        }

                    }
                }
                else
                {
                    DbCommand sqlStringCommand1 = this.database.GetSqlStringCommand("DELETE FROM FND_HS_CODE_ELMENTS WHERE HS_CODE_ID = @HS_CODE_ID");
                    this.database.AddInParameter(sqlStringCommand1, "HS_CODE_ID", DbType.Int32, hscodeinfo.HS_CODE_ID);
                    result = this.database.ExecuteNonQuery(sqlStringCommand1);
                    if (result >= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="HS_CODE_ID"></param>
        /// <returns></returns>
        public bool DeleteHSCode(int HS_CODE_ID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update FND_HS_CODE set VOIDED=1,VOIDED_DTM=getdate() WHERE HS_CODE_ID = @HS_CODE_ID");
            this.database.AddInParameter(sqlStringCommand, "HS_CODE_ID", DbType.Int32, HS_CODE_ID);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public int AddHSCodeInfo(HSCodeInfo hscodeinfo)
        {
            //DbCommand sqlStringCommand = 
            //    this.database.GetSqlStringCommand("INSERT INTO Ecshop_BrandCategories(BrandName, Logo, CompanyUrl,RewriteName,MetaKeywords,MetaDescription, Description, DisplaySequence) VALUES(@BrandName, @Logo, @CompanyUrl,@RewriteName,@MetaKeywords,@MetaDescription, @Description, @DisplaySequence);");

            DbCommand sqlStringCommand = this.database.GetStoredProcCommand("cp_HSCode_Create");

            this.database.AddOutParameter(sqlStringCommand, "HS_CODE_ID", DbType.Int32, 32);

            this.database.AddInParameter(sqlStringCommand, "HS_CODE", DbType.String, hscodeinfo.HS_CODE);
            this.database.AddInParameter(sqlStringCommand, "HS_NAME", DbType.String, hscodeinfo.HS_NAME);
            this.database.AddInParameter(sqlStringCommand, "LOW_RATE", DbType.Decimal, hscodeinfo.LOW_RATE);
            this.database.AddInParameter(sqlStringCommand, "HIGH_RATE", DbType.Decimal, hscodeinfo.HIGH_RATE);
            this.database.AddInParameter(sqlStringCommand, "OUT_RATE", DbType.Decimal, hscodeinfo.OUT_RATE);
            this.database.AddInParameter(sqlStringCommand, "TAX_RATE", DbType.Decimal, hscodeinfo.TAX_RATE);
            this.database.AddInParameter(sqlStringCommand, "TSL_RATE", DbType.Decimal, hscodeinfo.TSL_RATE);
            this.database.AddInParameter(sqlStringCommand, "UNIT_1", DbType.String, hscodeinfo.UNIT_1);
            this.database.AddInParameter(sqlStringCommand, "UNIT_2", DbType.String, hscodeinfo.UNIT_2);
            this.database.AddInParameter(sqlStringCommand, "CONTROL_MA", DbType.String, hscodeinfo.CONTROL_MA);
            this.database.AddInParameter(sqlStringCommand, "TEMP_IN_RATE", DbType.Decimal, hscodeinfo.TEMP_IN_RATE);
            this.database.AddInParameter(sqlStringCommand, "TEMP_OUT_RATE", DbType.Decimal, hscodeinfo.TEMP_OUT_RATE);
            this.database.AddInParameter(sqlStringCommand, "NOTE_S", DbType.String, hscodeinfo.NOTE_S);
            this.database.AddInParameter(sqlStringCommand, "CONTROL_INSPECTION", DbType.String, hscodeinfo.CONTROL_INSPECTION);
            this.database.AddInParameter(sqlStringCommand, "CONSUMPTION_RATE", DbType.Decimal, hscodeinfo.CONSUMPTION_RATE);
            this.database.AddInParameter(sqlStringCommand, "VOIDED", DbType.Int32, 0);

            int result = this.database.ExecuteNonQuery(sqlStringCommand);

            int HS_CODE_ID = (int)this.database.GetParameterValue(sqlStringCommand, "HS_CODE_ID");

            if (hscodeinfo.FND_HS_ELMENTS != null)
            {
                DataSet ds = hscodeinfo.FND_HS_ELMENTS;

                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    DbCommand sqlStringCommand1 =
                    this.database.GetSqlStringCommand("INSERT INTO FND_HS_CODE_ELMENTS ( HS_ELMENTS_ID, HS_ELMENTS_NAME , HS_ELMENTS_DESC , HS_CODE_ID , VOIDED ) VALUES (@HS_ELMENTS_ID, @HS_ELMENTS_NAME, @HS_ELMENTS_DESC, @HS_CODE_ID, @VOIDED)");
                    this.database.AddInParameter(sqlStringCommand1, "HS_ELMENTS_ID", DbType.Int32, dr["HS_ELMENTS_ID"]);
                    this.database.AddInParameter(sqlStringCommand1, "HS_ELMENTS_NAME", DbType.String, dr["HS_ELMENTS_NAME"].ToString());
                    this.database.AddInParameter(sqlStringCommand1, "HS_ELMENTS_DESC", DbType.String, dr["HS_ELMENTS_DESC"].ToString());
                    this.database.AddInParameter(sqlStringCommand1, "HS_CODE_ID", DbType.Int32, HS_CODE_ID);
                    this.database.AddInParameter(sqlStringCommand1, "VOIDED", DbType.Int32, 0);

                    result = this.database.ExecuteNonQuery(sqlStringCommand1);
                }
            }
            return result;
        }

        /// <summary>
        /// 根据id获取海关编码及申报要素信息
        /// </summary>
        /// <param name="HS_CODE_ID"></param>
        /// <returns></returns>
        public DataSet GetHSCodeInfo(int HS_CODE_ID)
        {
            DbCommand sqlStringCommand =
                this.database.GetSqlStringCommand("SELECT HS_CODE ,HS_NAME ,LOW_RATE ,HIGH_RATE ,OUT_RATE ,TAX_RATE ,TSL_RATE ,UNIT_1 ,UNIT_2 ,CONTROL_MA ,NOTE_S ,TEMP_IN_RATE ,TEMP_OUT_RATE ,CONTROL_INSPECTION ,CONSUMPTION_RATE FROM FND_HS_CODE WHERE HS_CODE_ID =@HS_CODE_ID and VOIDED=0;" +
                                                    "SELECT HS_ELMENTS_ID, HS_ELMENTS_NAME , HS_ELMENTS_DESC , HS_CODE_ID , VOIDED FROM FND_HS_CODE_ELMENTS WHERE HS_CODE_ID=@HS_CODE_ID;");
            this.database.AddInParameter(sqlStringCommand, "HS_CODE_ID", DbType.Int32, HS_CODE_ID);
            return this.database.ExecuteDataSet(sqlStringCommand); ;
        }

        /// <summary>
        /// 根据海关编码获取数据 
        /// </summary>
        /// <param name="Code">海关编码</param>
        /// <returns></returns>
        public DataTable GetHSCodeBuyCode(string Code)
        {
            string text = " VOIDED=0";
            if (!string.IsNullOrEmpty(Code))
            {
                text = text + " AND HS_CODE LIKE '%" + DataHelper.CleanSearchString(Code) + "%'";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT HS_CODE_ID,HS_CODE label,HS_NAME,HS_DESC,LOW_RATE,HIGH_RATE,OUT_RATE,REG_MARK,REG_RATE,TAX_TYPE,TAX_RATE,TSL_RATE,COMM_RATE,TAIWAN_RATE,OTHERTYPE,OTHER_RATE,UNIT_1,UNIT_2,ILOW_PRICE,IHIGH_PRIC,ELOW_PRICE,EHIGH_PRICE,MAX_IN,MAX_OUT,CONTROL_MA,CONTROL_NW,PRODUCTTYPE,ELEMENTS,CHK_PRICE,TARIF_MARK,NOTE_S,TEMP_IN_RATE,TEMP_OUT_RATE,CONTROL_INSPECTION,HK_HS_CODE,CONSUMPTION_RATE FROM FND_HS_CODE  WHERE " + text + " ORDER BY HS_CODE_ID");
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 获取海关编码对应申报要素数据 
        /// </summary>
        /// <param name="CodeId">海关编码Id</param>
        /// <returns></returns>
        public DataTable GetHSCODEELMENTS(string CodeId, string ProductId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT HS_CODE_ELMENTS_ID,HS_ELMENTS_ID,HS_ELMENTS_NAME,HS_ELMENTS_DESC,HS_CODE_ID,isnull(b.ELMENTS_VALUE,'') ELMENTS_VALUE FROM FND_HS_CODE_ELMENTS a LEFT JOIN dbo.FND_PRODUCT_ELMENTS b ON a.HS_ELMENTS_ID=b.ELMENTS_ID AND b.VOIDED=0 and b.PRODUCT_ID = " + DataHelper.CleanSearchString(ProductId) + " WHERE a.VOIDED=0 and a.HS_CODE_ID = " + DataHelper.CleanSearchString(CodeId) + " ORDER BY a.HS_CODE_ELMENTS_ID");
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 获取订单监控数据
        /// </summary>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <param name="Hour">结转时间</param>
        /// <returns></returns>
        public DataSet GetOrderMonitoring(string StartDate, string EndDate, string Hour)
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand storedProcCommand = database.GetStoredProcCommand("up_OrderMonitoring");
            this.database.AddInParameter(storedProcCommand, "StartDate", DbType.String, StartDate);
            this.database.AddInParameter(storedProcCommand, "EndDate", DbType.String, EndDate);
            this.database.AddInParameter(storedProcCommand, "Hour", DbType.String, Hour);
            return database.ExecuteDataSet(storedProcCommand);
        }




        public DataTable GetOrderItems(string orderid)
        {
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ShipmentQuantity*(ISNULL(items.[Weight], 0) / 1000) [Weight] ,ShipmentQuantity*(ISNULL(items.[Weight], 0) / 1000) + 0.2 GrossWeigh ,sku.LJNo ,pro.HSProductName,ShipmentQuantity* CASE WHEN pro.ConversionRelation IS NULL THEN 1 WHEN pro.ConversionRelation=0 THEN 1 ELSE pro.ConversionRelation END ShipmentQuantity,ItemAdjustedPrice,ShipmentQuantity*ItemAdjustedPrice ItemAdjustedSumPrice,pro.HSUnit + '('+pro.HSUnitCode+')' Unit,sku.ProductRegistrationNumber FROM Ecshop_OrderItems items LEFT JOIN Ecshop_SKUs sku ON sku.SkuId = items.SkuId LEFT JOIN dbo.Ecshop_Products pro ON pro.ProductId = items.ProductId where orderid='" + orderid + "' ORDER BY items.SkuId ASC");
            DbCommand sqlStringCommand = this.database.GetStoredProcCommand("cp_Ecshop_OrderHS_DeclareItem");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        #region 订单保存数据库
        /// <summary>
        /// 订单保存数据库
        /// </summary>
        /// <returns></returns>
        public void SaveHSDocking(int sType, string OrderId, string LogisticsNo, string OPacketsID, int Status, string Remark, string Data, string PayerID, string PayerName, string paymentBillAmount, string tradeNo)
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_HSDocking_Update");
            database.AddInParameter(storedProcCommand, "sType", System.Data.DbType.Int32, sType);
            database.AddInParameter(storedProcCommand, "OrderId", System.Data.DbType.String, OrderId);
            database.AddInParameter(storedProcCommand, "LogisticsNo", System.Data.DbType.String, LogisticsNo);
            database.AddInParameter(storedProcCommand, "OPacketsID", System.Data.DbType.String, OPacketsID);
            database.AddInParameter(storedProcCommand, "Status", System.Data.DbType.Int32, Status);
            database.AddInParameter(storedProcCommand, "Remark", System.Data.DbType.String, Remark);
            database.AddInParameter(storedProcCommand, "Data", System.Data.DbType.String, Data);
            database.AddInParameter(storedProcCommand, "PayerID", System.Data.DbType.String, PayerID);
            database.AddInParameter(storedProcCommand, "PayerName", System.Data.DbType.String, PayerName);
            database.AddInParameter(storedProcCommand, "paymentBillAmount", System.Data.DbType.Decimal, Decimal.Parse(paymentBillAmount));
            database.AddInParameter(storedProcCommand, "tradeNo", System.Data.DbType.String, tradeNo);
            database.ExecuteNonQuery(storedProcCommand);
        }
        #endregion

        /// <summary>
        /// 获取需要三单对碰的订单及其信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetOrders()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Orders_GetHSXmlData");
            return database.ExecuteDataSet(storedProcCommand);
        }

        /// <summary>
        /// 获取需要三单对碰的运单及其信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetLogs()
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Orders_GetHSXmlDataByLogs");
            return database.ExecuteDataSet(storedProcCommand);
        }

        /// <summary>
        /// 查询订单认领状态
        /// </summary>
        /// <param name="orderid">单号</param>
        /// <returns>true:已认领</returns>
        public bool GetOrderClaimStatus(string orderid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(1) from HS_Docking where DeclareStatus in(0,3) and orderid='" + orderid + "'");
            return int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString()) == 0;
        }

        /// <summary>
        /// 查询订单认领人
        /// </summary>
        /// <param name="orderid">单号</param>
        /// <returns></returns>
        public object GetOrderClaimUser(string orderid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select DeclareUserId from HS_Docking where orderid='" + orderid + "'");
            return this.database.ExecuteScalar(sqlStringCommand);
        }

        /// <summary>
        /// 重发四单
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <param name="UserId">操作人</param>
        public void ResendHSDocking(string orderid, int UserId)
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand storedProcCommand = database.GetStoredProcCommand("up_Resend_HSDocking");
            database.AddInParameter(storedProcCommand, "OrderId", System.Data.DbType.String, orderid);
            database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, UserId);
            database.ExecuteNonQuery(storedProcCommand);
        }

        /// <summary>
        /// 作废某个订单明细
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <param name="SkuId">SkuId</param>
        /// <param name="UserId">操作人</param>
        public void ValidHSDocking(string orderid, string SkuId, int UserId)
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand storedProcCommand = database.GetStoredProcCommand("cp_Ecshop_ReValid_DeclareItem");
            database.AddInParameter(storedProcCommand, "OrderId", System.Data.DbType.String, orderid);
            database.AddInParameter(storedProcCommand, "SkuId", System.Data.DbType.String, SkuId);
            database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, UserId);
            database.ExecuteNonQuery(storedProcCommand);
        }
        public void SetOrderStatus(string orderid, string status, string Remark, int UserId, string Username)
        {
            DbCommand sqlStringCommand;
            if (status == "1")//申报
            {
                sqlStringCommand = this.database.GetSqlStringCommand("update HS_Docking set DeclareStatus=@DeclareStatus,OutNo=@Remark,DeclareDate=getdate(),ShipOrderNumber=CONVERT(VARCHAR(8),GETDATE(),112)+'01' where OrderId=@OrderId");
            }
            else if (status == "4")//认领
            {
                sqlStringCommand = this.database.GetSqlStringCommand("update HS_Docking set DeclareStatus=@DeclareStatus,DeclareName=@DeclareName,DeclareUserId=@DeclareUserId where OrderId=@OrderId");
                database.AddInParameter(sqlStringCommand, "DeclareName", System.Data.DbType.String, Username);
                database.AddInParameter(sqlStringCommand, "DeclareUserId", System.Data.DbType.Int32, UserId);
            }
            else if (status == "0")//认领退回
            {
                sqlStringCommand = this.database.GetSqlStringCommand("update HS_Docking set DeclareStatus=@DeclareStatus,DeclareName='',DeclareUserId=0 where OrderId=@OrderId");
            }
            else if (status == "2")//申报成功
            {
                sqlStringCommand = this.database.GetSqlStringCommand("update HS_Docking set DeclareStatus=@DeclareStatus,Remark=@Remark,DeclareFinishDate=getdate() where OrderId=@OrderId");
            }
            else if (status == "98")//重新放行
            {
                sqlStringCommand = this.database.GetSqlStringCommand("UPDATE dbo.HS_Docking SET isSendWMS=0,SendWMSDate=getdate() WHERE OrderId=@OrderId and isSendWMS=1");
            }
            else
            {
                sqlStringCommand = this.database.GetSqlStringCommand("update HS_Docking set DeclareStatus=@DeclareStatus,Remark=@Remark where OrderId=@OrderId");
            }

            database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderid);
            database.AddInParameter(sqlStringCommand, "DeclareStatus", System.Data.DbType.Int32, int.Parse(status));
            database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, Remark);
            database.ExecuteNonQuery(sqlStringCommand);
        }

        public void SetPayerIDStatus(string ShipToName, string OrderId, string Status, string codeMessage, string ShipToID, string ShipToaddress, string ShipToPhone, string runType)
        {
            string sql = "update hs_docking set payerIdStatus=@payerIdStatus, payerIdRemark=@payerIdRemark,payerSendDate=GETDATE() WHERE OrderId=@OrderId;";
            Database database = DatabaseFactory.CreateDatabase();
            if (Status == "1" || Status == "3")
            {
                System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(sql);
                database.AddInParameter(sqlStringCommand, "payerIdStatus", DbType.String, int.Parse(Status));
                database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
                database.AddInParameter(sqlStringCommand, "payerIdRemark", DbType.String, codeMessage);
                database.ExecuteNonQuery(sqlStringCommand);
            }
            if (Status == "2")
            {
                if (runType == "0")
                {
                    sql += "insert into Ecshop_IdentityCard (OrderID,ShipToID,ShipToName,ShipToaddress,ShipToPhone) values(@OrderID,@ShipToID,@ShipToName,@ShipToaddress,@ShipToPhone);";
                }
                System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(sql);
                database.AddInParameter(sqlStringCommand, "payerIdStatus", DbType.Int32, int.Parse(Status));
                database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
                database.AddInParameter(sqlStringCommand, "payerIdRemark", DbType.String, codeMessage);
                database.AddInParameter(sqlStringCommand, "ShipToID", DbType.String, ShipToID);
                database.AddInParameter(sqlStringCommand, "ShipToName", DbType.String, ShipToName);
                database.AddInParameter(sqlStringCommand, "ShipToaddress", DbType.String, ShipToaddress);
                database.AddInParameter(sqlStringCommand, "ShipToPhone", DbType.String, ShipToPhone);
                database.ExecuteNonQuery(sqlStringCommand);
            }

        }

        public DataSet GetOrderMonitoringItem(string StartDate, string EndDate, string Hour, string type, string status)
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand storedProcCommand = database.GetStoredProcCommand("up_OrderMonitoringItem");
            this.database.AddInParameter(storedProcCommand, "type", DbType.String, type);
            this.database.AddInParameter(storedProcCommand, "status", DbType.Int32, int.Parse(status));
            this.database.AddInParameter(storedProcCommand, "StartDate", DbType.String, StartDate);
            this.database.AddInParameter(storedProcCommand, "EndDate", DbType.String, EndDate);
            this.database.AddInParameter(storedProcCommand, "Hour", DbType.String, Hour);
            return database.ExecuteDataSet(storedProcCommand);
        }
    }
}

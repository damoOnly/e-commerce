using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.PO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EcShop.SqlDal.PO
{
    public class PurchaseOrderDao
    {
        private Database database;
        public PurchaseOrderDao()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 获取采购订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetPurchaseOrderList(PurchaseOrderQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" isdel=0 ");


            if (!string.IsNullOrEmpty(query.PONumber))
            {
                stringBuilder.AppendFormat(" AND PONumber LIKE '%{0}%'", DataHelper.CleanSearchString(query.PONumber));
            }
            if (!string.IsNullOrEmpty(query.HSInOut))
            {
                stringBuilder.AppendFormat(" AND HSInOut LIKE '%{0}%'", DataHelper.CleanSearchString(query.HSInOut));
            }
            if (query.SupplierId != null && query.SupplierId != 0)
            {
                stringBuilder.AppendFormat(" AND SupplierId ={0}", query.SupplierId);
            }

            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND ExpectedTime >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND ExpectedTime <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }

            if (query.POStatus != -1)
            {
                stringBuilder.AppendFormat(" AND POStatus={0}", query.POStatus);
            }
            if (query.QPStatus != -1)
            {
                stringBuilder.AppendFormat(" AND QPStatus={0}", query.QPStatus);
            }
            if (query.CIStatus != -1)
            {
                stringBuilder.AppendFormat(" AND CIStatus={0}", query.CIStatus);
            }
            string selectFields = "id,PONumber,POType,POStatus,QPStatus,CIStatus,SupplierId,SupplierCode,SupplierName,ExpectedTime,SendWMSByPO,WMSCount,HSInOut,Remark,CreateTime,CreateUserId,CreateUserName,UpdateTime,UpdateUserId,IsDel,ExtendInt1,ExtendInt2,ExtendInt3,ExtendChar1,ExtendChar2,ExtendChar3,ExtendChar4,DeclareUserId";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "EcShop_PurchaseOrder", "id", stringBuilder.ToString(), selectFields);
        }

        public DataSet GetPurchaseOrder(int Top ,string strWhere)
        {
            StringBuilder sb = new StringBuilder("SELECT");
            if (Top != 0)
            {
                sb.Append(" top " + Top);
            }
            sb.Append(" * FROM dbo.EcShop_PurchaseOrder ");
            if (strWhere != "")
            {
                sb.Append(" Where " + strWhere);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        /// <summary>
        /// 获取采购订单明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetPurchaseOrderItemList(PurchaseOrderItemQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" isdel=0 ");

            if (query.POId != 0)
            {
                stringBuilder.AppendFormat(" AND POId ={0}", query.POId);
            }


            if (!string.IsNullOrEmpty(query.ProductName))
            {
                stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            if (!string.IsNullOrEmpty(query.BarCode))
            {
                stringBuilder.AppendFormat(" AND BarCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.BarCode));
            }

            string selectFields = "id,POId,SkuId,BoxBarCode,ExpectQuantity,PracticalQuantity,IsSample,ManufactureDate,EffectiveDate,BatchNumber,RoughWeight,Rate,"
                        + "CostPrice,TotalCostPrice,SalePrice,TotalSalePrice,CartonSize,Cases,CartonMeasure,CreateTime,CreateUserId,UpdateTime,NetWeight,OriginalCurrencyPrice,OriginalCurrencyTotalPrice,"
                        + "UpdateUserId,DelTime,DelUserId,IsDel,ExtendInt1,ExtendInt2,ExtendInt3,ExtendChar1,ExtendChar2,ExtendChar3,ExtendChar4,SupplierName,[Address],Mobile,Phone,ImageUrl1,EnglishName,ProductStandard,"
                        + "Weight,ProductName,ProductId,BarCode,CnArea,BrandId,BrandName,Name_CN,Name_EN,sysProductName,PONumber,Ingredient,Contact,Email,Fax,Category,BeneficiaryName,SwiftCode,BankAccount,BankName,BankAddress,IBAN,Unit";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_EcShop_PurchaseOrderItem", "id", stringBuilder.ToString(), selectFields);
        }


        /// <summary>
        /// 获取采购订单明细数量和金额总计
        /// </summary>
        /// <param name="POId"></param>
        /// <returns></returns>
        public DataSet GetPOTotalQuyAndAmount(int PoId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT isnull(SUM(ExpectQuantity),0) ExpectQuantity,isnull(SUM(OriginalCurrencyTotalPrice),0) OriginalCurrencyTotalPrice,isnull(SUM(TotalSalePrice),0) TotalSalePrice FROM dbo.EcShop_PurchaseOrderItems WHERE IsDel=0 AND POId=@POId");
            this.database.AddInParameter(sqlStringCommand, "@POId", DbType.Int32, PoId);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        /// <summary>
        /// 获取采购订单明细导出
        /// </summary>
        /// <param name="POId"></param>
        /// <returns></returns>
        public DataSet GetPurchaseOrderExportItem(int PoId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ROW_NUMBER() OVER(ORDER BY po.CreateTime) '序号',po.SkuId '商品代码',pro.ProductName '商品名称',pro.BarCode '产品条码',BoxBarCode '外箱条形码',ExpectQuantity '订单数量',CASE WHEN IsSample=0 THEN '否' ELSE '是' END '是否样品',CONVERT(VARCHAR(10),ManufactureDate,121) '生产日期',CONVERT(VARCHAR(10),EffectiveDate,121) '有效日期',BatchNumber '生产批号',NetWeight '商品总净重(kg)',RoughWeight '商品总毛重(kg)',bc.Name_CN '币别',OriginalCurrencyPrice '原币单价',po.CostPrice '成本价',po.SalePrice '销售价',CartonSize '装箱规格',CartonMeasure '箱子尺寸',Cases '箱数' FROM dbo.EcShop_PurchaseOrderItems po LEFT JOIN dbo.BaseCurrency bc ON po.CurrencyId=bc.ID JOIN dbo.Ecshop_SKUs sku ON sku.SkuId = po.SkuId JOIN dbo.Ecshop_Products pro ON pro.ProductId = sku.ProductId WHERE POId=@POId AND po.IsDel=0");
            this.database.AddInParameter(sqlStringCommand, "@POId", DbType.Int32, PoId);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        /// <summary>
        /// 新增采购订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool AddPurchaseOrderInfo(PurchaseOrderInfo Info)
        {
            DbCommand sqlStringCommand = this.database.GetStoredProcCommand("cp_PurchaseOrder_Create");
            this.database.AddInParameter(sqlStringCommand, "@POType", DbType.Int32, Info.POType);
            this.database.AddInParameter(sqlStringCommand, "@SupplierId", DbType.Int32, Info.SupplierId);
            this.database.AddInParameter(sqlStringCommand, "@ExpectedTime", DbType.DateTime, Info.ExpectedTime);
            this.database.AddInParameter(sqlStringCommand, "@Remark", DbType.String, Info.Remark);
            this.database.AddInParameter(sqlStringCommand, "@CreateUserId", DbType.Int32, Info.CreateUserId);
            this.database.AddInParameter(sqlStringCommand, "@ExtendInt1", DbType.Int32, Info.ExtendInt1);
            this.database.AddInParameter(sqlStringCommand, "@ExtendInt2", DbType.Int32, Info.ExtendInt2);
            this.database.AddInParameter(sqlStringCommand, "@ExtendInt3", DbType.Int32, Info.ExtendInt3);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar1", DbType.String, Info.ExtendChar1);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar2", DbType.String, Info.ExtendChar2);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar3", DbType.String, Info.ExtendChar3);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar4", DbType.String, Info.ExtendChar4);

            this.database.AddInParameter(sqlStringCommand, "@DepartTime", DbType.DateTime, Info.DepartTime);
            this.database.AddInParameter(sqlStringCommand, "@ArrivalTime", DbType.DateTime, Info.ArrivalTime);
            this.database.AddInParameter(sqlStringCommand, "@ContainerNumber", DbType.String, Info.ContainerNumber);
            this.database.AddInParameter(sqlStringCommand, "@BillNo", DbType.String, Info.BillNo);
            this.database.AddInParameter(sqlStringCommand, "@StartPortID", DbType.String, Info.StartPortID);
            this.database.AddInParameter(sqlStringCommand, "@StartPort", DbType.String, Info.StartPort);
            this.database.AddInParameter(sqlStringCommand, "@EndPortID", DbType.String, Info.EndPortID);
            this.database.AddInParameter(sqlStringCommand, "@EndPort", DbType.String, Info.EndPort);

            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 采购订单明细添加商品
        /// </summary>
        /// <param name="POId"></param>
        /// <param name="text">商品1,商品2</param>
        /// <param name="UserId">操作人</param>
        /// <returns></returns>
        public bool POAddProducts(int POId, string text, int UserId)
        {
            DbCommand sqlStringCommand = this.database.GetStoredProcCommand("cp_PurchaseOrderItem_AddProducts");
            this.database.AddInParameter(sqlStringCommand, "@POId", DbType.Int32, POId);
            this.database.AddInParameter(sqlStringCommand, "@text", DbType.String, text);
            this.database.AddInParameter(sqlStringCommand, "@CreateUserId", DbType.Int32, UserId);

            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 获取报关单导出合同、发票、装箱单数据
        /// </summary>
        /// <param name="Id">POId</param>
        /// <returns></returns>
        public DataSet exportPurchaseOrder(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetStoredProcCommand("up_exportPurchaseOrder");
            this.database.AddInParameter(sqlStringCommand, "@Id", DbType.Int32, Id);

            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        /// <summary>
        /// 插入数据库文件地址
        /// </summary>
        /// <param name="POId"></param>
        /// <param name="text">商品列表（14,5546,34）</param>
        /// <returns></returns>
        public bool SaveFilePath(string path, string strHeaderID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [dbo].[Ecshop_POCDHeader] SET ContractUrl=@path+contractNo+'-POContract.pdf',InvoiceUrl=@path+contractNo+'-POInvoice.pdf',PackingUrl=@path+contractNo+'-POPacking.pdf' WHERE HeaderID IN(" + strHeaderID + ") ");
            this.database.AddInParameter(sqlStringCommand, "@path", DbType.String, path);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 修改采购订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool EditPurchaseOrderInfo(PurchaseOrderInfo Info)
        {
            DbCommand sqlStringCommand = this.database.GetStoredProcCommand("cp_PurchaseOrder_Update");
            this.database.AddInParameter(sqlStringCommand, "@Id", DbType.Int32, Info.id);
            this.database.AddInParameter(sqlStringCommand, "@POType", DbType.Int32, Info.POType);
            this.database.AddInParameter(sqlStringCommand, "@SupplierId", DbType.Int32, Info.SupplierId);
            this.database.AddInParameter(sqlStringCommand, "@ExpectedTime", DbType.DateTime, Info.ExpectedTime);
            this.database.AddInParameter(sqlStringCommand, "@Remark", DbType.String, Info.Remark);
            this.database.AddInParameter(sqlStringCommand, "@UpdateUserId", DbType.Int32, Info.CreateUserId);
            this.database.AddInParameter(sqlStringCommand, "@ExtendInt1", DbType.Int32, Info.ExtendInt1);
            this.database.AddInParameter(sqlStringCommand, "@ExtendInt2", DbType.Int32, Info.ExtendInt2);
            this.database.AddInParameter(sqlStringCommand, "@ExtendInt3", DbType.Int32, Info.ExtendInt3);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar1", DbType.String, Info.ExtendChar1);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar2", DbType.String, Info.ExtendChar2);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar3", DbType.String, Info.ExtendChar3);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar4", DbType.String, Info.ExtendChar4);

            this.database.AddInParameter(sqlStringCommand, "@DepartTime", DbType.DateTime, Info.DepartTime);
            this.database.AddInParameter(sqlStringCommand, "@ArrivalTime", DbType.DateTime, Info.ArrivalTime);
            this.database.AddInParameter(sqlStringCommand, "@ContainerNumber", DbType.String, Info.ContainerNumber);
            this.database.AddInParameter(sqlStringCommand, "@BillNo", DbType.String, Info.BillNo);
            this.database.AddInParameter(sqlStringCommand, "@StartPortID", DbType.String, Info.StartPortID);
            this.database.AddInParameter(sqlStringCommand, "@StartPort", DbType.String, Info.StartPort);
            this.database.AddInParameter(sqlStringCommand, "@EndPortID", DbType.String, Info.EndPortID);
            this.database.AddInParameter(sqlStringCommand, "@EndPort", DbType.String, Info.EndPort);

            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 修改采购订单明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool EditPurchaseOrderItemInfo(PurchaseOrderItemInfo Info)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update item set ");
            strSql.Append("BoxBarCode=@BoxBarCode,");
            strSql.Append("ExpectQuantity=@ExpectQuantity,");
            //strSql.Append("PracticalQuantity=@PracticalQuantity,");
            strSql.Append("IsSample=@IsSample,");
            strSql.Append("ManufactureDate=@ManufactureDate,");
            strSql.Append("EffectiveDate=@EffectiveDate,");
            strSql.Append("BatchNumber=@BatchNumber,");
            strSql.Append("RoughWeight=@RoughWeight,");

            strSql.Append("NetWeight=@NetWeight,");

            strSql.Append("CurrencyId=@CurrencyId,");
            strSql.Append("Rate=@Rate,");
            strSql.Append("CostPrice=@CostPrice,");
            strSql.Append("TotalCostPrice=@TotalCostPrice,");
            strSql.Append("SalePrice=@SalePrice,");
            strSql.Append("TotalSalePrice=@TotalSalePrice,");

            strSql.Append("OriginalCurrencyPrice=@OriginalCurrencyPrice,");
            strSql.Append("OriginalCurrencyTotalPrice=@OriginalCurrencyTotalPrice,");

            strSql.Append("CartonSize=@CartonSize,");
            strSql.Append("CartonMeasure=@CartonMeasure,");
            strSql.Append("Cases=@Cases,");

            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("UpdateUserId=@UpdateUserId,");
            strSql.Append("ExtendInt1=@ExtendInt1,");
            strSql.Append("ExtendInt2=@ExtendInt2,");
            strSql.Append("ExtendInt3=@ExtendInt3,");
            strSql.Append("ExtendChar1=@ExtendChar1,");
            strSql.Append("ExtendChar2=@ExtendChar2,");
            strSql.Append("ExtendChar3=@ExtendChar3,");
            strSql.Append("ExtendChar4=@ExtendChar4");
            strSql.Append(" from EcShop_PurchaseOrderItems item join EcShop_PurchaseOrder po on item.POId=po.id where item.id=@id AND item.CreateUserId=@UpdateUserId AND po.POStatus=0");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql.ToString());

            this.database.AddInParameter(sqlStringCommand, "@BoxBarCode", DbType.String, Info.BoxBarCode);
            this.database.AddInParameter(sqlStringCommand, "@ExpectQuantity", DbType.Int32, Info.ExpectQuantity);
            //this.database.AddInParameter(sqlStringCommand, "@PracticalQuantity", DbType.Int32, Info.PracticalQuantity);
            this.database.AddInParameter(sqlStringCommand, "@IsSample", DbType.Boolean, Info.IsSample);
            this.database.AddInParameter(sqlStringCommand, "@ManufactureDate", DbType.DateTime, Info.ManufactureDate);
            this.database.AddInParameter(sqlStringCommand, "@EffectiveDate", DbType.DateTime, Info.EffectiveDate);
            this.database.AddInParameter(sqlStringCommand, "@BatchNumber", DbType.String, Info.BatchNumber);
            this.database.AddInParameter(sqlStringCommand, "@RoughWeight", DbType.Decimal, Info.RoughWeight);
            this.database.AddInParameter(sqlStringCommand, "@NetWeight", DbType.Decimal, Info.NetWeight);

            this.database.AddInParameter(sqlStringCommand, "@CurrencyId", DbType.Int32, Info.CurrencyId);
            this.database.AddInParameter(sqlStringCommand, "@Rate", DbType.Decimal, Info.Rate);
            this.database.AddInParameter(sqlStringCommand, "@CostPrice", DbType.Decimal, Info.CostPrice);
            this.database.AddInParameter(sqlStringCommand, "@TotalCostPrice", DbType.Decimal, Info.TotalCostPrice);
            this.database.AddInParameter(sqlStringCommand, "@SalePrice", DbType.Decimal, Info.SalePrice);
            this.database.AddInParameter(sqlStringCommand, "@TotalSalePrice", DbType.Decimal, Info.TotalSalePrice);

            this.database.AddInParameter(sqlStringCommand, "@OriginalCurrencyPrice", DbType.Decimal, Info.OriginalCurrencyPrice);
            this.database.AddInParameter(sqlStringCommand, "@OriginalCurrencyTotalPrice", DbType.Decimal, Info.OriginalCurrencyTotalPrice);

            this.database.AddInParameter(sqlStringCommand, "@CartonSize", DbType.String, Info.CartonSize);
            this.database.AddInParameter(sqlStringCommand, "@CartonMeasure", DbType.String, Info.CartonMeasure);
            this.database.AddInParameter(sqlStringCommand, "@Cases", DbType.Int32, Info.Cases);
            this.database.AddInParameter(sqlStringCommand, "@UpdateTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "@UpdateUserId", DbType.Int32, Info.UpdateUserId);
            this.database.AddInParameter(sqlStringCommand, "@ExtendInt1", DbType.Int32, Info.ExtendInt1);
            this.database.AddInParameter(sqlStringCommand, "@ExtendInt2", DbType.Int32, Info.ExtendInt2);
            this.database.AddInParameter(sqlStringCommand, "@ExtendInt3", DbType.Int32, Info.ExtendInt3);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar1", DbType.String, Info.ExtendChar1);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar2", DbType.String, Info.ExtendChar2);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar3", DbType.String, Info.ExtendChar3);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar4", DbType.String, Info.ExtendChar4);
            this.database.AddInParameter(sqlStringCommand, "@id", DbType.Int32, Info.id);

            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 导入PO明细
        /// </summary>
        /// <param name="PurchaseOrderItemInfo"></param>
        /// <returns></returns>
        public string ImportPoItem(List<PurchaseOrderItemInfo> listInfo)
        {
            bool sflg = false;
            bool flg = false;
            foreach (PurchaseOrderItemInfo item in listInfo)
            {
                if (!EditPurchaseOrderItemInfoByImport(item))
                {
                    sflg = true;
                }
                else
                {
                    flg = true;
                }
            }
            if (sflg && flg)
            {
                return "部分更新成功";
            }
            else if (!sflg && flg)
            {
                return "全部更新成功";
            }
            else if (sflg && !flg)
            {
                return "全部更新失败";
            }
            else 
            {
                return "无需更新数据";
            }
        }

        /// <summary>
        /// 修改采购订单明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool EditPurchaseOrderItemInfoByImport(PurchaseOrderItemInfo Info)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update EcShop_PurchaseOrderItems set ");
            strSql.Append("BoxBarCode=@BoxBarCode,");
            strSql.Append("ExpectQuantity=@ExpectQuantity,");
            //strSql.Append("PracticalQuantity=@PracticalQuantity,");
            strSql.Append("IsSample=@IsSample,");
            strSql.Append("ManufactureDate=@ManufactureDate,");
            strSql.Append("EffectiveDate=@EffectiveDate,");
            strSql.Append("BatchNumber=@BatchNumber,");
            strSql.Append("RoughWeight=@RoughWeight,");

            strSql.Append("NetWeight=@NetWeight,");

            strSql.Append("CurrencyId=@CurrencyId,");
            strSql.Append("Rate=@Rate,");
            strSql.Append("CostPrice=@CostPrice,");
            strSql.Append("TotalCostPrice=@TotalCostPrice,");
            strSql.Append("SalePrice=@SalePrice,");
            strSql.Append("TotalSalePrice=@TotalSalePrice,");

            strSql.Append("OriginalCurrencyPrice=@OriginalCurrencyPrice,");
            strSql.Append("OriginalCurrencyTotalPrice=@OriginalCurrencyTotalPrice,");

            strSql.Append("CartonSize=@CartonSize,");
            strSql.Append("CartonMeasure=@CartonMeasure,");
            strSql.Append("Cases=@Cases,");

            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("UpdateUserId=@UpdateUserId,");
            strSql.Append("ExtendInt1=@ExtendInt1,");
            strSql.Append("ExtendInt2=@ExtendInt2,");
            strSql.Append("ExtendInt3=@ExtendInt3,");
            strSql.Append("ExtendChar1=@ExtendChar1,");
            strSql.Append("ExtendChar2=@ExtendChar2,");
            strSql.Append("ExtendChar3=@ExtendChar3,");
            strSql.Append("ExtendChar4=@ExtendChar4");
            strSql.Append(" from EcShop_PurchaseOrderItems where POId=@POId and SkuId=@SkuId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql.ToString());

            this.database.AddInParameter(sqlStringCommand, "@BoxBarCode", DbType.String, Info.BoxBarCode);
            this.database.AddInParameter(sqlStringCommand, "@ExpectQuantity", DbType.Int32, Info.ExpectQuantity);
            this.database.AddInParameter(sqlStringCommand, "@IsSample", DbType.Boolean, Info.IsSample);
            this.database.AddInParameter(sqlStringCommand, "@ManufactureDate", DbType.DateTime, Info.ManufactureDate);
            this.database.AddInParameter(sqlStringCommand, "@EffectiveDate", DbType.DateTime, Info.EffectiveDate);
            this.database.AddInParameter(sqlStringCommand, "@BatchNumber", DbType.String, Info.BatchNumber);
            this.database.AddInParameter(sqlStringCommand, "@RoughWeight", DbType.Decimal, Info.RoughWeight);
            this.database.AddInParameter(sqlStringCommand, "@NetWeight", DbType.Decimal, Info.NetWeight);

            this.database.AddInParameter(sqlStringCommand, "@CurrencyId", DbType.Int32, Info.CurrencyId);
            this.database.AddInParameter(sqlStringCommand, "@Rate", DbType.Decimal, Info.Rate);
            this.database.AddInParameter(sqlStringCommand, "@CostPrice", DbType.Decimal, Info.CostPrice);
            this.database.AddInParameter(sqlStringCommand, "@TotalCostPrice", DbType.Decimal, Info.TotalCostPrice);
            this.database.AddInParameter(sqlStringCommand, "@SalePrice", DbType.Decimal, Info.SalePrice);
            this.database.AddInParameter(sqlStringCommand, "@TotalSalePrice", DbType.Decimal, Info.TotalSalePrice);

            this.database.AddInParameter(sqlStringCommand, "@OriginalCurrencyPrice", DbType.Decimal, Info.OriginalCurrencyPrice);
            this.database.AddInParameter(sqlStringCommand, "@OriginalCurrencyTotalPrice", DbType.Decimal, Info.OriginalCurrencyTotalPrice);

            this.database.AddInParameter(sqlStringCommand, "@CartonSize", DbType.String, Info.CartonSize);
            this.database.AddInParameter(sqlStringCommand, "@CartonMeasure", DbType.String, Info.CartonMeasure);
            this.database.AddInParameter(sqlStringCommand, "@Cases", DbType.Int32, Info.Cases);
            this.database.AddInParameter(sqlStringCommand, "@UpdateTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "@UpdateUserId", DbType.Int32, Info.UpdateUserId);
            this.database.AddInParameter(sqlStringCommand, "@ExtendInt1", DbType.Int32, Info.ExtendInt1);
            this.database.AddInParameter(sqlStringCommand, "@ExtendInt2", DbType.Int32, Info.ExtendInt2);
            this.database.AddInParameter(sqlStringCommand, "@ExtendInt3", DbType.Int32, Info.ExtendInt3);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar1", DbType.String, Info.ExtendChar1);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar2", DbType.String, Info.ExtendChar2);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar3", DbType.String, Info.ExtendChar3);
            this.database.AddInParameter(sqlStringCommand, "@ExtendChar4", DbType.String, Info.ExtendChar4);
            this.database.AddInParameter(sqlStringCommand, "@POId", DbType.Int32, Info.POId);
            this.database.AddInParameter(sqlStringCommand, "@SkuId", DbType.String, Info.SkuId);

            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 删除采购订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool Deletet(int Id, int DelUserId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPdate EcShop_PurchaseOrder set isDel=1,DelTime=getdate(),DelUserId=@DelUserId WHERE Id = @Id and CreateUserId=@DelUserId AND POStatus=0");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            this.database.AddInParameter(sqlStringCommand, "DelUserId", DbType.Int32, DelUserId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 删除采购订单明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool DeletetItem(int Id, int DelUserId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPdate item set isDel=1,DelTime=getdate(),DelUserId=@DelUserId from EcShop_PurchaseOrderItems item join EcShop_PurchaseOrder po on item.POId=po.id where item.id=@id AND item.CreateUserId=@DelUserId AND po.POStatus=0");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            this.database.AddInParameter(sqlStringCommand, "DelUserId", DbType.Int32, DelUserId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }


        public IList<PurchaseOrderInfo> GetPurchaseOrder(string Where)
        {
            IList<PurchaseOrderInfo> result = null;
            StringBuilder sb = new StringBuilder("SELECT id,PONumber,POType,POStatus,SupplierId,SupplierCode,SupplierName,ExpectedTime,SendWMSByPO,WMSCount,HSInOut,Remark,CreateTime,CreateUserId,UpdateTime,UpdateUserId,IsDel,ExtendInt1,ExtendInt2,ExtendInt3,ExtendChar1,ExtendChar2,ExtendChar3,ExtendChar4,[DepartTime] ,[ArrivalTime] ,[ContainerNumber] ,[BillNo] ,[StartPort] ,[EndPort] ,[EndPortID] ,[StartPortID] FROM EcShop_PurchaseOrder where isdel=0 ");
            if (Where.Length > 0)
            {
                sb.AppendFormat(" and {0}", Where);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<PurchaseOrderInfo>(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 获取采购订单明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet GetPurchaseOrderItem(string Where)
        {
            StringBuilder sb = new StringBuilder("SELECT id,POId,SkuId,BoxBarCode,ExpectQuantity,PracticalQuantity,IsSample,ManufactureDate,EffectiveDate,BatchNumber,RoughWeight,CurrencyId,Rate,CostPrice,TotalCostPrice,SalePrice,TotalSalePrice,CartonSize,Cases,CartonMeasure,CreateTime,CreateUserId,UpdateTime,UpdateUserId,DelTime,DelUserId,IsDel,ExtendInt1,ExtendInt2,ExtendInt3,ExtendChar1,ExtendChar2,ExtendChar3,ExtendChar4,NetWeight,OriginalCurrencyPrice,OriginalCurrencyTotalPrice FROM EcShop_PurchaseOrderItems where isdel=0");
            if (Where.Length > 0)
            {
                sb.AppendFormat(" and {0}", Where);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        /// <summary>
        /// 获取采购订单和明细
        /// </summary>
        /// <returns></returns>
        public DataSet GetPurchaseOrderAndItem(string Where)
        {
            StringBuilder sb = new StringBuilder("SELECT po.id,PONumber,POType,POStatus,SupplierId,SupplierCode,SupplierName,ExpectedTime,SendWMSByPO,WMSCount,HSInOut,Remark,po.CreateTime,po.CreateUserId,AffirmUserId,AffirmName,AffirmDate,DeclareUserId,DeclareName,DeclareDate,DeclareRemark,SysRemark,poitem.id AS itemId,SkuId,BoxBarCode,ExpectQuantity,PracticalQuantity,IsSample,ManufactureDate,EffectiveDate,BatchNumber,RoughWeight,CurrencyId,Rate,CostPrice,TotalCostPrice,SalePrice,TotalSalePrice,CartonSize,CartonMeasure,Casesm,NetWeight,OriginalCurrencyPrice,OriginalCurrencyTotalPrice ");
            sb.Append(" FROM dbo.EcShop_PurchaseOrder po JOIN dbo.EcShop_PurchaseOrderItems poitem ON po.id=poitem.POId and poitem.isdel=0 where po.isdel=0 ");
            if (Where.Length > 0)
            {
                sb.AppendFormat(" and {0}", Where);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        /// <summary>
        /// 获取币别
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet GetBaseCurrency()
        {
            StringBuilder sb = new StringBuilder("SELECT ID,Name_CN,Name_EN,EntryNo,CreateUser,CreateTime,ForbitFlag,DeleteFlag,DeleteUser,DeleteTime FROM dbo.BaseCurrency WHERE DeleteFlag=0 ORDER BY EntryNo");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public bool SetPOStatus(string id, string status, string Remark, int UserId, string Username)
        {
            DbCommand sqlStringCommand;
            if (status == "0")//取消，关务未认领，并且是自己的
            {
                sqlStringCommand = this.database.GetSqlStringCommand("update EcShop_PurchaseOrder set POStatus=@status,SysRemark=SysRemark+@Remark where id=@id and POStatus in(1,6) and CreateUserId=@UserId");
            }
            else if (status == "1")//确认
            {
                sqlStringCommand = this.database.GetSqlStringCommand("update EcShop_PurchaseOrder set POStatus=@status,AffirmDate=getdate(),AffirmUserid=@UserId,AffirmName=@Username where id=@id and POStatus=0 and CreateUserId=@UserId");
            }
            else if (status == "2")//认领
            {
                sqlStringCommand = this.database.GetSqlStringCommand("update EcShop_PurchaseOrder set POStatus=@status,DeclareUserId=@UserId,DeclareName=@Username,DeclareDate=getdate() where id=@id and POStatus in(1,6) ");
            }
            else if (status == "-1")//退回
            {
                sqlStringCommand = this.database.GetSqlStringCommand("update EcShop_PurchaseOrder set POStatus=1,SysRemark=SysRemark+@Remark where id=@id and POStatus=2 and DeclareUserId=@UserId ");
            }
            else if (status == "7")//申报成功
            {
                sqlStringCommand = this.database.GetSqlStringCommand("update EcShop_PurchaseOrder set POStatus=@status,SendWMSByPO=1 where id=@id and POStatus=5 ");//三个都成功
            }
            else
            {
                sqlStringCommand = this.database.GetSqlStringCommand("update EcShop_PurchaseOrder set POStatus=@status,SysRemark=SysRemark+@Remark where id=@id");
            }

            database.AddInParameter(sqlStringCommand, "@id", System.Data.DbType.String, id);
            database.AddInParameter(sqlStringCommand, "@status", System.Data.DbType.Int32, int.Parse(status));
            database.AddInParameter(sqlStringCommand, "@Remark", System.Data.DbType.String, Remark);

            database.AddInParameter(sqlStringCommand, "@Username", System.Data.DbType.String, Username);
            database.AddInParameter(sqlStringCommand, "@UserId", System.Data.DbType.Int32, UserId);
            return database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool IsExistsPOItem(string id)
        {
            StringBuilder sb = new StringBuilder("SELECT count(*) from EcShop_PurchaseOrderItems where isdel=0 and POId=" + id);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            return int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString()) > 0;
        }


        public DataTable GetBaseEnumDictList(string DictType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DictValue,DictName FROM BaseEnumDict WHERE DictType=@DictType");
            database.AddInParameter(sqlStringCommand, "@DictType", System.Data.DbType.String, DictType);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public IList<PODeclareInfo> GetPODeclareInfo(string id)
        {
            IList<PODeclareInfo> result = null;
            StringBuilder sb = new StringBuilder("SELECT id,PONumber,transname,cabinno,FriNum,GetTax,GetTaxCode,TransportType,TransportTypeCode,BusinessType,BusinessTypeCode,WrapType,WrapTypeCode,TradeType,TradeTypeCode,useType,useTypeCode,applyort,applyortCode,ContainerNumberType,voyage FROM EcShop_PurchaseOrder WHERE isdel=0 and " + id + "");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sb.ToString());
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToList<PODeclareInfo>(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 保存PO申报信息
        /// </summary>
        public bool SavePODeclareInfo(PODeclareInfo PODeclareInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPdate EcShop_PurchaseOrder " +
"SET transname=@transname,cabinno=@cabinno,FriNum=@FriNum,GetTax=@GetTax,GetTaxCode=@GetTaxCode,TransportType=@TransportType,TransportTypeCode=@TransportTypeCode," +
"BusinessType=@BusinessType,BusinessTypeCode=@BusinessTypeCode,WrapType=@WrapType,WrapTypeCode=@WrapTypeCode,TradeType=@TradeType,TradeTypeCode=@TradeTypeCode,useType=@useType,useTypeCode=@useTypeCode,applyort=@applyort,applyortCode=@applyortCode,ContainerNumberType=@ContainerNumberType,voyage=@voyage,POStatus=4 " +
"WHERE isdel=0 and Id = @Id ");
            this.database.AddInParameter(sqlStringCommand, "@Id", DbType.Int32, PODeclareInfo.id);
            this.database.AddInParameter(sqlStringCommand, "@transname", DbType.String, PODeclareInfo.transname);
            this.database.AddInParameter(sqlStringCommand, "@cabinno", DbType.String, PODeclareInfo.cabinno);
            this.database.AddInParameter(sqlStringCommand, "@FriNum", DbType.String, PODeclareInfo.FriNum);
            this.database.AddInParameter(sqlStringCommand, "@GetTax", DbType.String, PODeclareInfo.getTax);
            this.database.AddInParameter(sqlStringCommand, "@GetTaxCode", DbType.Int32, PODeclareInfo.getTaxCode);
            this.database.AddInParameter(sqlStringCommand, "@TransportType", DbType.String, PODeclareInfo.TransportType);
            this.database.AddInParameter(sqlStringCommand, "@TransportTypeCode", DbType.Int32, PODeclareInfo.TransportTypeCode);
            this.database.AddInParameter(sqlStringCommand, "@BusinessType", DbType.String, PODeclareInfo.BusinessType);
            this.database.AddInParameter(sqlStringCommand, "@BusinessTypeCode", DbType.String, PODeclareInfo.BusinessTypeCode);
            this.database.AddInParameter(sqlStringCommand, "@WrapType", DbType.String, PODeclareInfo.WrapType);
            this.database.AddInParameter(sqlStringCommand, "@WrapTypeCode", DbType.String, PODeclareInfo.WrapTypeCode);
            this.database.AddInParameter(sqlStringCommand, "@TradeType", DbType.String, PODeclareInfo.TradeType);
            this.database.AddInParameter(sqlStringCommand, "@TradeTypeCode", DbType.Int32, PODeclareInfo.TradeTypeCode);
            this.database.AddInParameter(sqlStringCommand, "@useType", DbType.String, PODeclareInfo.useType);
            this.database.AddInParameter(sqlStringCommand, "@useTypeCode", DbType.Int32, PODeclareInfo.useTypeCode);
            this.database.AddInParameter(sqlStringCommand, "@applyort", DbType.String, PODeclareInfo.applyort);
            this.database.AddInParameter(sqlStringCommand, "@applyortCode", DbType.Int32, PODeclareInfo.applyortCode);
            this.database.AddInParameter(sqlStringCommand, "@ContainerNumberType", DbType.String, PODeclareInfo.ContainerNumberType);
            this.database.AddInParameter(sqlStringCommand, "@voyage", DbType.String, PODeclareInfo.voyage);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 获取PO申报JSON数据
        /// </summary>
        public DataTable GetPODeclareJSON(string id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT applyort,applyortCode,transportType,transportTypeCode,businessType,ExpectQuantity,wrapType,wrapTypeCode,tradeType,tradeTypeCode,RoughWeight,NetWeight,StartPort,StartPortID,cabinno,orders.transname FROM dbo.EcShop_PurchaseOrder orders" +
                    " JOIN (SELECT items.POId,SUM(RoughWeight) RoughWeight,SUM(ExpectQuantity) ExpectQuantity,SUM(NetWeight) NetWeight FROM dbo.Ecshop_SKUs JOIN dbo.EcShop_PurchaseOrderItems items ON items.SkuId = Ecshop_SKUs.SkuId AND items.IsDel=0 GROUP BY items.POId) PO ON po.POId=orders.id WHERE orders.isdel=0 and orders.id=" + id + "");
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        /// <summary>
        /// 获取PO申报JSON表体数据
        /// </summary>
        public DataTable GetPODeclareBodyJSON(string id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT sku.SkuId,sku.LJNo,sku.ProductRegistrationNumber,hs.HS_CODE,prod.HSProductName,item.ExpectQuantity,item.TotalCostPrice,sku.GrossWeight/1000 GrossWeight,import.StandardCName,import.HSCode,orders.getTax,orders.GetTaxCode,orders.useType,orders.useTypeCode,item.CostPrice " +
                    "FROM dbo.EcShop_PurchaseOrderItems item " +
                    "LEFT JOIN dbo.Ecshop_SKUs sku ON sku.SkuId = item.SkuId " +
                    "LEFT JOIN dbo.Ecshop_Products prod ON prod.ProductId = sku.ProductId " +
                    "LEFT JOIN dbo.FND_HS_CODE hs ON prod.HSCodeId=hs.HS_CODE_ID " +
                    "LEFT JOIN dbo.Ecshop_ImportSourceType import ON  import.ImportSourceId = prod.ImportSourceId " +
                    "LEFT JOIN dbo.EcShop_PurchaseOrder orders ON orders.id = item.POId where orders.isdel=0 and item.isdel=0 and orders.id=" + id + "");
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }
        /// <summary>
        /// 获取入库报关单数据
        /// </summary>
        public void SavePOcustomsDeclaration(POJsonInfo POJsonInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Ecshop_POCDHeader (POId,formId,contractNo,createtime) VALUES(@POId,@formId,@contractNo,GETDATE());SELECT @@IDENTITY ");

            this.database.AddInParameter(sqlStringCommand, "@POId", System.Data.DbType.Int32, POJsonInfo.POId);
            this.database.AddInParameter(sqlStringCommand, "@formId", System.Data.DbType.String, POJsonInfo.header.formId);
            this.database.AddInParameter(sqlStringCommand, "@contractNo", System.Data.DbType.String, POJsonInfo.contractNo);

            object obj = this.database.ExecuteScalar(sqlStringCommand);
            int num;
            int HeaderID;
            if (!int.TryParse(obj.ToString(), out num))
            {
                HeaderID = 0;
            }
            else
            {
                HeaderID = num;
            }

            foreach (goods goods in POJsonInfo.goods)
            {
                DbCommand sqlStringCommand1 = this.database.GetSqlStringCommand("INSERT INTO Ecshop_POCDBody (HeaderID,num_no,amount,amount1,country_name,currency_name,goods_name,goods_no,goods_spec,totalamount,unit_name,unit_name1)" +
                    "VALUES(@HeaderID,@num_no,@amount,@amount1,@country_name,@currency_name,@goods_name,@goods_no,@goods_spec,@totalamount,@unit_name,@unit_name1);SELECT @@IDENTITY");
                this.database.AddInParameter(sqlStringCommand1, "@HeaderID", System.Data.DbType.Int32, HeaderID);
                this.database.AddInParameter(sqlStringCommand1, "@num_no", System.Data.DbType.Int32, Convert.ToInt32(goods.num_no));
                this.database.AddInParameter(sqlStringCommand1, "@amount", System.Data.DbType.Int32, Convert.ToInt32(Convert.ToDecimal(goods.amount)));
                this.database.AddInParameter(sqlStringCommand1, "@amount1", System.Data.DbType.Int32, Convert.ToInt32(Convert.ToDecimal(goods.amount1)));
                this.database.AddInParameter(sqlStringCommand1, "@country_name", System.Data.DbType.String, goods.country_name);
                this.database.AddInParameter(sqlStringCommand1, "@currency_name", System.Data.DbType.String, goods.currency_name);
                this.database.AddInParameter(sqlStringCommand1, "@goods_name", System.Data.DbType.String, goods.goods_name);
                this.database.AddInParameter(sqlStringCommand1, "@goods_no", System.Data.DbType.String, goods.goods_no);
                this.database.AddInParameter(sqlStringCommand1, "@goods_spec", System.Data.DbType.String, goods.goods_spec);
                this.database.AddInParameter(sqlStringCommand1, "@totalamount", System.Data.DbType.Decimal, goods.totalamount);
                this.database.AddInParameter(sqlStringCommand1, "@unit_name", System.Data.DbType.String, goods.unit_name);
                this.database.AddInParameter(sqlStringCommand1, "@unit_name1", System.Data.DbType.String, goods.unit_name1);

                int BodyID;
                obj = this.database.ExecuteScalar(sqlStringCommand1);
                if (!int.TryParse(obj.ToString(), out num))
                {
                    BodyID = 0;
                }
                else
                {
                    BodyID = num;
                }

                foreach (lj lj in goods.lj)
                {
                    DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("INSERT INTO Ecshop_POCDBodyLj (BodyID,LJNo,SkuId) VALUES(@BodyID,@LJNo,@SkuId)");
                    this.database.AddInParameter(sqlStringCommand2, "@BodyID", System.Data.DbType.Int32, BodyID);
                    this.database.AddInParameter(sqlStringCommand2, "@LJNo", System.Data.DbType.String, lj.ljno);
                    this.database.AddInParameter(sqlStringCommand2, "@SkuId", System.Data.DbType.String, lj.SkuId);
                    this.database.ExecuteScalar(sqlStringCommand2);
                }
            }
        }
        /// <summary>
        /// 生成自动单号，可配置
        /// </summary>
        public string UP_GetNewRecordNo(int pModID, string pType, string pValue)
        {
            DbCommand sqlStringCommand = this.database.GetStoredProcCommand("UP_GetNewRecordNo");
            this.database.AddInParameter(sqlStringCommand, "pModID", System.Data.DbType.Int32, pModID);
            this.database.AddInParameter(sqlStringCommand, "pType", System.Data.DbType.String, pType);
            this.database.AddInParameter(sqlStringCommand, "pValue", System.Data.DbType.String, pType);
            this.database.AddOutParameter(sqlStringCommand, "pRu", System.Data.DbType.String, 30);
            this.database.ExecuteNonQuery(sqlStringCommand);
            return (string)this.database.GetParameterValue(sqlStringCommand, "pRu");
        }
        /// <summary>
        /// WebService外部调用修改PO状态
        /// </summary>

        public bool SetPOCDStatus(string formId, int POStatus, string PORemark, string CommonNum)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE dbo.EcShop_PurchaseOrder SET QPStatus=@POStatus WHERE IsDel=0 and id in (" +
                "SELECT orders.id FROM Ecshop_POCDHeader header JOIN dbo.EcShop_PurchaseOrder orders ON header.POId=orders.id WHERE isdel=0 and header.formId =@formId GROUP BY orders.id);" +
                "UPDATE Ecshop_POCDHeader set POStatus=@POStatus,PORemark=@PORemark,CommonNum=@CommonNum where formId =@formId;");
            this.database.AddInParameter(sqlStringCommand, "@formId", System.Data.DbType.String, formId);
            this.database.AddInParameter(sqlStringCommand, "@POStatus", System.Data.DbType.Int32, POStatus);
            this.database.AddInParameter(sqlStringCommand, "@PORemark", System.Data.DbType.String, PORemark);
            this.database.AddInParameter(sqlStringCommand, "@CommonNum", System.Data.DbType.String, CommonNum);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        /// <summary>
        /// WebService外部调用获取QP字段信息
        /// </summary>
        public DataTable GetPOCDInfo(string formId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT formId,cabinno,transname,StartCountryNo,LoadportNo,EndCountryNo,voyage,'http://" + Globals.DomainName + "'+ContractUrl ContractUrl,'http://" + Globals.DomainName + "'+InvoiceUrl InvoiceUrl,'http://" + Globals.DomainName + "'+PackingUrl PackingUrl,EndPortID FROM vw_GetPOCDInfo where isdel=0 and formId=@formId");
            this.database.AddInParameter(sqlStringCommand, "@formId", System.Data.DbType.String, formId);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 获取PO申报列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetPODeclareList(PODeclareListQuery query)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" isdel=0 ");


            if (!string.IsNullOrEmpty(query.PONumber))
            {
                stringBuilder.AppendFormat(" AND PONumber LIKE '%{0}%'", DataHelper.CleanSearchString(query.PONumber));
            }
            if (!string.IsNullOrEmpty(query.fromID))
            {
                stringBuilder.AppendFormat(" AND formId LIKE '%{0}%'", DataHelper.CleanSearchString(query.fromID));
            }

            if (query.StartDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND createtime >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                stringBuilder.AppendFormat(" AND createtime <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }

            //if (query.POCDStatus != -1)
            //{
            //    stringBuilder.AppendFormat(" AND POCDStatus={0}", query.POCDStatus);
            //}

            string selectFields = "* ";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_GetPODeclareList", "POId", stringBuilder.ToString(), selectFields);
        }

        /// <summary>
        /// 获取报关单主表信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DataTable GetPOCDPurchaseOrderList(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetStoredProcCommand("up_POCDPurchaseOrder");
            this.database.AddInParameter(sqlStringCommand, "@id", DbType.Int32, Id);

            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        /// <summary>
        /// 获取报关单主表信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DataTable GetPOCDPurchaseOrderBodyLj(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from vw_Ecshop_POCDPurchaseOrderList where BodyID=@bodyID ");
            this.database.AddInParameter(sqlStringCommand, "@bodyID", DbType.Int32, Id);

            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public DataTable GetPOCDList(string formId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT formId,POStatus,PORemark FROM dbo.Ecshop_POCDHeader WHERE formId IN (" + formId + ") ");
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT formId,POStatus,PORemark FROM dbo.Ecshop_POCDHeader a JOIN dbo.ufn_SplitToTable(@formId,',') b ON a.formId=b.value ");
            //this.database.AddInParameter(sqlStringCommand, "formId", System.Data.DbType.String, formId);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public DataTable POCommodityInspectionInfo(string id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT RoughWeight [weight],NetWeight netWt,ExpectQuantity packNo,orders.BusinessTypeCode businessType," +
                " CAST(orders.TransportTypeCode AS NVARCHAR(10))+'/'+TransportType storageTrafMode,CAST(orders.applyortCode AS NVARCHAR(10))+'/'+orders.applyort storagePortCode,orders.transname shipName,orders.voyage voyageNo,orders.cabinno billNo,CAST(orders.WrapTypeCode AS NVARCHAR(10))+'/'+orders.WrapType storageWrapType,CAST(orders.TradeTypeCode AS NVARCHAR(10))+'/'+orders.TradeType storageTransMode" +
                " FROM dbo.EcShop_PurchaseOrder orders " +
                " LEFT JOIN ( SELECT items.POId,SUM(items.RoughWeight) RoughWeight,SUM(items.NetWeight) NetWeight,SUM(items.ExpectQuantity) ExpectQuantity FROM dbo.EcShop_PurchaseOrderItems items WHERE items.IsDel=0 GROUP BY items.POId) items1 ON items1.POId = orders.id WHERE orders.IsDel=0 and orders.id=@id");
            this.database.AddInParameter(sqlStringCommand, "@id", System.Data.DbType.String, id);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public DataTable POCommodityInspectionInfoDes(string id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT body.num_no,items.ManufactureDate,items.EffectiveDate,items.BatchNumber,items.ExpectQuantity,items.CostPrice,items.RoughWeight,products.BarCode "+
                "FROM dbo.EcShop_PurchaseOrderItems items "+
                "LEFT JOIN dbo.EcShop_PurchaseOrder orders ON orders.id = items.POId  "+
                "LEFT JOIN dbo.Ecshop_SKUs sku ON sku.SkuId = items.SkuId  "+
                "LEFT JOIN dbo.Ecshop_Products products ON products.ProductId = sku.ProductId "+
                "LEFT JOIN dbo.Ecshop_POCDHeader header ON header.POId = orders.id "+
                "LEFT JOIN dbo.Ecshop_POCDBody body ON body.HeaderID = header.HeaderID "+
                "WHERE items.IsDel=0 AND orders.IsDel=0 and orders.id=@id");
            this.database.AddInParameter(sqlStringCommand, "@id", System.Data.DbType.String, id);
            DataTable result;
            using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = DataHelper.ConverDataReaderToDataTable(dataReader);
            }
            return result;
        }

        public bool SetCIStatus(string id, string status, string Remark, int p, string Username)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update EcShop_PurchaseOrder set CIStatus=@status,SysRemark=SysRemark+@Remark where id=@id"); ;

            database.AddInParameter(sqlStringCommand, "@id", System.Data.DbType.String, id);
            database.AddInParameter(sqlStringCommand, "@status", System.Data.DbType.Int32, int.Parse(status));
            database.AddInParameter(sqlStringCommand, "@Remark", System.Data.DbType.String, Remark);

            //database.AddInParameter(sqlStringCommand, "@Username", System.Data.DbType.String, Username);
            //database.AddInParameter(sqlStringCommand, "@UserId", System.Data.DbType.Int32, p);
            return database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
    }
}

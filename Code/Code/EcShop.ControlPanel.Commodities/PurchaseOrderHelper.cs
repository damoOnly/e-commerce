using EcShop.Core.Entities;
using EcShop.Entities.PO;
using EcShop.SqlDal.Commodities;
using EcShop.SqlDal.PO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EcShop.ControlPanel.Commodities
{
    public class PurchaseOrderHelper
    {
        /// <summary>
        /// 获取采购订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetPurchaseOrderList(PurchaseOrderQuery purchaseOrderQuery)
        {
            return new PurchaseOrderDao().GetPurchaseOrderList(purchaseOrderQuery);
        }

        public static DataSet GetPurchaseOrder(int Top, string strWhere)
        {
            return new PurchaseOrderDao().GetPurchaseOrder(Top, strWhere);
        }
        /// <summary>
        /// 获取采购订单明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataSet GetPurchaseOrderItem(string Where)
        {
            return new PurchaseOrderDao().GetPurchaseOrderItem(Where);
        }

        /// <summary>
        /// 获取采购订单和明细
        /// </summary>
        /// <param name="Where"></param>
        /// <returns></returns>
        public static DataSet GetPurchaseOrderAndItem(string Where)
        {
            return new PurchaseOrderDao().GetPurchaseOrderAndItem(Where);
        }
        

        /// <summary>
        /// 获取币别
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBaseCurrency()
        {
            return new PurchaseOrderDao().GetBaseCurrency();
        }
        

        /// <summary>
        /// 获取采购订单明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetPurchaseOrderItemList(PurchaseOrderItemQuery purchaseOrderItemQuery)
        {
            return new PurchaseOrderDao().GetPurchaseOrderItemList(purchaseOrderItemQuery);
        }

        /// <summary>
        /// 获取采购订单明细数量和金额总计
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataSet GetPOTotalQuyAndAmount(int POId)
        {
            return new PurchaseOrderDao().GetPOTotalQuyAndAmount(POId);
        }
        

        /// <summary>
        /// 获取采购订单导出明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataSet GetPurchaseOrderExportItem(int POId)
        {
            return new PurchaseOrderDao().GetPurchaseOrderExportItem(POId);
        }
        
        
        /// <summary>
        /// 获取报关单导出合同、发票、装箱单数据
        /// </summary>
        /// <param name="Id">POId</param>
        /// <returns></returns>
        public static DataSet exportPurchaseOrder(int Id)
        {
            return new PurchaseOrderDao().exportPurchaseOrder(Id);
        }
        public static bool AddPurchaseOrderInfo(PurchaseOrderInfo PurchaseOrderInfo)
        {
            return new PurchaseOrderDao().AddPurchaseOrderInfo(PurchaseOrderInfo);
        }

        /// <summary>
        /// 采购订单明细添加商品
        /// </summary>
        /// <param name="POId"></param>
        /// <param name="text">商品列表（14,5546,34）</param>
        /// <returns></returns>
        public static bool POAddProducts(int POId, string text, int UserId)
        {
            return new PurchaseOrderDao().POAddProducts(POId, text, UserId);
        }
        
        /// <summary>
        /// 插入数据库文件地址
        /// </summary>
        /// <param name="POId"></param>
        /// <param name="text">商品列表（14,5546,34）</param>
        /// <returns></returns>
        public static bool SaveFilePath(string path, string strHeaderID)
        {
            return new PurchaseOrderDao().SaveFilePath(path, strHeaderID);
        }
        

        public static bool EditPurchaseOrderInfo(PurchaseOrderInfo PurchaseOrderInfo)
        {
            return new PurchaseOrderDao().EditPurchaseOrderInfo(PurchaseOrderInfo);
        }

        public static bool EditPurchaseOrderItemInfo(PurchaseOrderItemInfo PurchaseOrderItemInfo)
        {
            return new PurchaseOrderDao().EditPurchaseOrderItemInfo(PurchaseOrderItemInfo);
        }
        

        public static bool Deletet(int id, int DelUserId)
        {
            return new PurchaseOrderDao().Deletet(id, DelUserId);
        }

        public static bool DeletetItem(int id, int DelUserId)
        {
            return new PurchaseOrderDao().DeletetItem(id, DelUserId);
        }
        

        public static IList<PurchaseOrderInfo> GetPurchaseOrder(string Where)
        {
            return new PurchaseOrderDao().GetPurchaseOrder(Where);
        }
        public static bool SetPOStatus(string orderid, string status, string Remark, int UserId, string Username)
        {
            return new PurchaseOrderDao().SetPOStatus(orderid, status, Remark, UserId, Username);
        }

        public static bool IsExistsPOItem(string id)
        {
            return new PurchaseOrderDao().IsExistsPOItem(id);
        }
        /// <summary>
        /// 获取BaseEnumDict表中的数据
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBaseEnumDictList(string DictType)
        {
            return new PurchaseOrderDao().GetBaseEnumDictList(DictType);
        }

        public static IList<PODeclareInfo> GetPODeclareInfo(string id)
        {
            return new PurchaseOrderDao().GetPODeclareInfo(id);
        }

        public static bool SavePODeclareInfo(PODeclareInfo PODeclareInfo)
        {
            return new PurchaseOrderDao().SavePODeclareInfo(PODeclareInfo);
        }

        public static DataTable GetPODeclareJSON(string id)
        {
            return new PurchaseOrderDao().GetPODeclareJSON(id);
        }

        public static DataTable GetPODeclareBodyJSON(string id)
        {
            return new PurchaseOrderDao().GetPODeclareBodyJSON(id);
        }

        public static void SavePOcustomsDeclaration(POJsonInfo POJsonInfo)
        {
            new PurchaseOrderDao().SavePOcustomsDeclaration(POJsonInfo);
        }

        public static string UP_GetNewRecordNo(int pModID, string pType, string pValue)
        {
            return new PurchaseOrderDao().UP_GetNewRecordNo(pModID,pType,pValue);
        }

        public static bool SetPOCDStatus(string formId, int POStatus, string PORemark,string CommonNum)
        {
            return new PurchaseOrderDao().SetPOCDStatus(formId, POStatus, PORemark, CommonNum);
        }

        public static DataTable GetPOCDInfo(string formId)
        {
            return new PurchaseOrderDao().GetPOCDInfo(formId);
        }

        public static string ImportPoItem(List<PurchaseOrderItemInfo> PurchaseOrderItemInfo)
        {
            return new PurchaseOrderDao().ImportPoItem(PurchaseOrderItemInfo);
        }
        

        public static DbQueryResult GetPODeclareList(PODeclareListQuery PODeclareListQuery)
        {
            return new PurchaseOrderDao().GetPODeclareList(PODeclareListQuery);
        }

        public static DataTable GetPOCDPurchaseOrderList(int Id)
        {
            return new PurchaseOrderDao().GetPOCDPurchaseOrderList(Id);
        }

        public static DataTable GetPOCDPurchaseOrderBodyLj(int Id)
        {
            return new PurchaseOrderDao().GetPOCDPurchaseOrderBodyLj(Id);
        }

        public static DataTable GetPOCDList(string formId)
        {
            return new PurchaseOrderDao().GetPOCDList(formId);
        }

        public static DataTable POCommodityInspectionInfo(string id)
        {
            return new PurchaseOrderDao().POCommodityInspectionInfo(id);
        }

        public static DataTable POCommodityInspectionInfoDes(string id)
        {
            return new PurchaseOrderDao().POCommodityInspectionInfoDes(id);
        }

        public static bool SetCIStatus(string id, string status, string Remark, int p, string Username)
        {
            return new PurchaseOrderDao().SetCIStatus(id, status, Remark, p, Username);
        }
    }
}

using Commodities;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.Store;
using EcShop.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EcShop.ControlPanel.Commodities
{
    public sealed class StoreManagementHelper
    {
        private StoreManagementHelper()
        {
        }

        public static DataTable GetStore()
        {
            return new StoreManagementDao().GetStore();
        }

        public static DataTable GetUserStore(int storeId)
        {
            return new StoreManagementDao().GetUserStore(storeId); 
        }

        public static DbQueryResult GetStore(StoreQuery query)
        {
            return new StoreManagementDao().GetStore(query);
        }

        public static StoreManagementInfo GetStore(int SupplierId)
        {
            return new StoreManagementDao().GetStore(SupplierId);
        }
        public static DataTable GetRelatedStoreProducts(int StoreId)
        {
            return new StoreManagementDao().GetRelatedStoreProducts(StoreId);
        }
        public static bool RemoveReleatesProductByStore(int StoreId, int productId)
        {
            return new StoreManagementDao().RemoveReleatesProductByStore(StoreId, productId);
        }

        public static bool RemoveReleatesProductByStore(int StoreId)
        {
            return new StoreManagementDao().RemoveReleatesProductByStore(StoreId);
        }
        public static bool UpdateRelateProductSequenceByStore(int StoreId, int RelatedProductId, int displaysequence)
        {
            return new StoreManagementDao().UpdateRelateProductSequenceByStore(StoreId, RelatedProductId, displaysequence);
        }
        public static bool AddReleatesProdcutByStore(int StoreId, int prodcutId, string QRcode)
        {
            return new StoreManagementDao().AddReleatesProdcutByStore(StoreId, prodcutId, QRcode);
        }
        public static string GetStoreName(int SupplierId)
        {
            StoreManagementDao SupplierDao = new StoreManagementDao();
            string supplierName = SupplierDao.GetStoreName(SupplierId);
            return supplierName;
        }
        public static int AddStore(StoreManagementInfo supplierInfo)
        {
            int result;
            if (supplierInfo == null)
            {
                result = 0;
            }
            else
            {
                StoreManagementDao SupplierDao = new StoreManagementDao();
                Globals.EntityCoding(supplierInfo, true);
                int num = SupplierDao.AddStore(supplierInfo);
                if (num > 0)
                {
                    EventLogs.WriteOperationLog(Privilege.StoreAdd, string.Format(CultureInfo.InvariantCulture, "创建了一个新的门店:”{0}”", new object[]
					{
						supplierInfo.StoreName
					}));
                }
                result = num;
            }
            return result;
        }
        public static bool UpdateStore(StoreManagementInfo supplierInfo)
        {
            bool result;
            if (supplierInfo == null)
            {
                result = false;
            }
            else
            {
                StoreManagementDao SupplierDao = new StoreManagementDao();
                Globals.EntityCoding(supplierInfo, true);
                bool flag = SupplierDao.UpdateStore(supplierInfo);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.SupplierEdit, string.Format(CultureInfo.InvariantCulture, "修改了编号为”{0}”的门店", new object[]
					{
						supplierInfo.StoreId
					}));
                }
                result = flag;
            }
            return result;
        }
        public static bool DeleteStore(int StoreId)
        {
            //ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
            bool flag = new StoreManagementDao().DeleteStore(StoreId);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.SupplierDelete, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的门店", new object[]
				{
					StoreId
				}));
            }
            return flag;
        }
        /// <summary>
        /// 打印商品
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns></returns>
        public static DataTable PrintProducts(int storeId, string productId)
        {
            return new StoreManagementDao().PrintProducts(storeId,productId);
        }


    }
}

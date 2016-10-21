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
    public sealed class SupplierHelper
    {
        private SupplierHelper()
        {
        }

        public static DataTable GetSupplier()
        {
            return new SupplierDao().GetSupplier();
        }

        public static DbQueryResult GetSupplier(SupplierQuery query)
        {
            return new SupplierDao().GetSupplier(query);
        }

        public static SupplierInfo GetSupplier(int SupplierId)
        {
            return new SupplierDao().GetSupplier(SupplierId);
        }

        public static AppSupplierInfo GetAppSupplier(int supplierId, int userid)
        {
            return new SupplierDao().GetAppSupplier(supplierId, userid);
        }

        public static string GetSupplierName(int SupplierId)
        {
            SupplierDao SupplierDao = new SupplierDao();
            string supplierName = SupplierDao.GetSupplierName(SupplierId);
            return supplierName;
        }
        public static bool GetSupplierApproveKey(int supplierId)
        {
            SupplierDao SupplierDao = new SupplierDao();
            return SupplierDao.GetSupplierApproveKey(supplierId);

        }
        public static int AddSupplier(SupplierInfo supplierInfo)
        {
            int result;
            if (supplierInfo == null)
            {
                result = 0;
            }
            else
            {
                SupplierDao SupplierDao = new SupplierDao();
                Globals.EntityCoding(supplierInfo, true);
                int num = SupplierDao.AddSupplier(supplierInfo);
                if (num > 0)
                {
                    EventLogs.WriteOperationLog(Privilege.SupplierAdd, string.Format(CultureInfo.InvariantCulture, "创建了一个新的供货商:”{0}”", new object[]
					{
						supplierInfo.SupplierName
					}));
                }
                result = num;
            }
            return result;
        }
        public static bool UpdateSupplier(SupplierInfo supplierInfo)
        {
            bool result;
            if (supplierInfo == null)
            {
                result = false;
            }
            else
            {
                SupplierDao SupplierDao = new SupplierDao();
                Globals.EntityCoding(supplierInfo, true);
                bool flag = SupplierDao.UpdateSupplier(supplierInfo);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.SupplierEdit, string.Format(CultureInfo.InvariantCulture, "修改了编号为”{0}”的供货商", new object[]
					{
						supplierInfo.SupplierId
					}));
                }
                result = flag;
            }
            return result;
        }
        public static bool DeleteSupplier(int supplierId)
        {
            //ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
            bool flag = new SupplierDao().DeleteSupplier(supplierId);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.SupplierDelete, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的供货商", new object[]
				{
					supplierId
				}));
            }
            return flag;
        }

        /// <summary>
        /// 收藏供应商
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static int CollectSupplier(SupplierCollectInfo info)
        {
            return new SupplierDao().CollectSupplier(info);
        }

        /// <summary>
        /// 供应商是否已经收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public static bool SupplierIsCollect(int userId, int supplierId)
        {
            return new SupplierDao().SupplierIsCollect(userId, supplierId);
        }



        /// <summary>
        /// 获取用户供应商收藏数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetUserSupplierCollectCount(int userId)
        {
            return new SupplierDao().GetUserSupplierCollectCount(userId);
        }


        /// <summary>
        /// 获取供货商收藏列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetSupplierCollect(SupplierCollectQuery query)
        {
            return new SupplierDao().GetSupplierCollect(query);
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public static bool DelCollectSupplier(int userId, int supplierId)
        {
            return new SupplierDao().DelCollectSupplier(userId, supplierId);
        }


        /// <summary>
        /// 获取App端商家列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetAppSupplier(SupplierQuery query)
        {
            return new SupplierDao().GetAppSupplier(query);
        }


        /// <summary>
        /// 获取商家列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetWMSSupplier(SupplierQuery query)
        {
            return new SupplierDao().GetWMSSupplier(query);
        }

    }
}

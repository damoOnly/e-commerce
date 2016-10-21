using EcShop.Entities;
using EcShop.Entities.Supplier;
using EcShop.SqlDal.Store;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EcShop.ControlPanel.Store
{
    public class SupplierConfigHelper
    {
        /// <summary>
        /// 添加商家配置
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static int SaveSupplierCfg(SupplierConfigInfo info)
        {
            return new SupplierConfigDao().SaveSupplierCfg(info);
        }

        /// <summary>
        /// 获取商家配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SupplierConfigInfo GetSupplierCfgById(int id)
        {
            return new SupplierConfigDao().GetSupplierCfgById(id);
        }

        /// <summary>
        /// 修改商家配置
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool UpdateSupplierCfg(SupplierConfigInfo info)
        {
            return new SupplierConfigDao().UpdateSupplierCfg(info);
        }


        /// <summary>
        /// 根据类型获取商家配置列表
        /// </summary>
        /// <param name="client"></param>
        /// <param name="cfgType"></param>
        /// <returns></returns>
        public static IList<SupplierConfigInfo> GetSupplierCfgByType(ClientType client, SupplierCfgType cfgType)
        {
            return new SupplierConfigDao().GetSupplierCfgByType(client, cfgType);
        }


        public static void SwapSupplierCfgSequence(int Id, int replaceId)
        {
            SupplierConfigDao supplierConfigDao = new SupplierConfigDao();
            SupplierConfigInfo info = supplierConfigDao.GetSupplierCfgById(Id);
            SupplierConfigInfo replaceInfo = supplierConfigDao.GetSupplierCfgById(replaceId);
            if (info != null && replaceInfo != null)
            {
                int displaySequence = info.DisplaySequence;
                info.DisplaySequence = replaceInfo.DisplaySequence;
                replaceInfo.DisplaySequence = displaySequence;
                supplierConfigDao.UpdateSupplierCfg(info);
                supplierConfigDao.UpdateSupplierCfg(replaceInfo);
            }
        }

        public static  bool DelSupplierCfg(int id)
        {
            return new SupplierConfigDao().DelSupplierCfg(id);
        }


        /// <summary>
        /// 获取配置的商家
        /// </summary>
        /// <param name="client">枚举：客户端</param>
        /// <param name="cfgType">枚举：配置类型</param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static DataTable GetConfigSupplier(ClientType client, SupplierCfgType cfgType,int userid)
        {
            return new SupplierConfigDao().GetConfigSupplier(client, cfgType, userid);
        }
    }
}

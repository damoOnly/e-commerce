using EcShop.Entities;
using EcShop.SqlDal.Store;
using System;
using System.Collections.Generic;
using System.Data;
namespace EcShop.ControlPanel.Store
{
	public static class APPHelper
	{
		public static AppVersionRecordInfo GetLatestAppVersionRecord(string device)
		{
			return new AppVersionRecordDao().GetLatestAppVersionRecord(device);
		}

        public static AppVersionRecordInfo GetLatestAppVersionRecordOrderById(string device)
        {
            return new AppVersionRecordDao().GetLatestAppVersionRecordOrderById(device);
        }


		public static bool AddAppVersionRecord(AppVersionRecordInfo appVersionRecord)
		{
			return new AppVersionRecordDao().AddAppVersionRecord(appVersionRecord);
		}

        public static bool UpdateAppVersionRecord(AppVersionRecordInfo appVersionRecord)
        {
            return new AppVersionRecordDao().UpdateAppVersionRecord(appVersionRecord);
        }


		public static bool IsForcibleUpgrade(string device, decimal version)
		{
			return new AppVersionRecordDao().IsForcibleUpgrade(device, version);
		}
		public static bool AddAppInstallRecord(AppInstallRecordInfo appInstallRecord)
		{
			return new AppVersionRecordDao().AddAppInstallRecord(appInstallRecord);
		}
        public static bool ChangeAppRunImg(Dictionary<string,string> appRunImgInfo)
        {
            List<AppRunImgInfo> list = new List<AppRunImgInfo>();
            foreach (string r in appRunImgInfo.Keys)
            {
                list.Add(new AppRunImgInfo() {  imgSrc=appRunImgInfo[r], phoneType=r});
            }
            return new AppVersionRecordDao().ChangeAppRunImg(list);
        }
        public static List<AppRunImgInfo> GetAppRunImg()
        {
            return new AppVersionRecordDao().GetAppRunImg();

        }


        public static DataTable GetAppVersionRecordList(string device)
        {
            return new AppVersionRecordDao().GetAppVersionRecordList(device);
        }


        /// <summary>
        /// 根据id获取版本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static AppVersionRecordInfo GetAppVersionById(int id)
        {
            return new AppVersionRecordDao().GetAppVersionById(id);
        }


        /// <summary>
        /// 根据先前的版本号判断是否强制升级
        /// </summary>
        /// <param name="version"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public static bool IsForcibleUpgradeByPreviousVersion(decimal version, string device)
        {
            return new AppVersionRecordDao().IsForcibleUpgradeByPreviousVersion(version, device);
        }

        
        /// <summary>
        /// 根据id删除版本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DelAppVersionById(int id)
        {
            return new AppVersionRecordDao().DelAppVersionById(id);
        }
	}
}

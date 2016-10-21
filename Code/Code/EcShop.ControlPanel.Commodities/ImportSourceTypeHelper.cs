using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.Commodities;
using EcShop.Entities.HOP;
using EcShop.Entities.Store;
using EcShop.Membership.Context;
using EcShop.SqlDal.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Web;
namespace EcShop.ControlPanel.Commodities
{
    public static class ImportSourceTypeHelper
	{
        public static DbQueryResult GetImportSourceTypes(ImportSourceTypeQuery query)
        {
            return new ImportSourceTypeDao().GetImportSourceTypes(query);
        }

        public static int DeleteImportSourceTypes(string importSourceIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.ImportSourceTypeDelete);
            int result;
            if (string.IsNullOrEmpty(importSourceIds))
            {
                result = 0;
            }
            else
            {
               result = new ImportSourceTypeDao().DeleteImportSourceTypes(importSourceIds);
            }
            return result;
        }

        public static int AddImportSourceType(ImportSourceTypeInfo importSourceType)
        {
            return new ImportSourceTypeDao().AddImportSourceType(importSourceType); 
        }

        public static int UpdateImportSourceType(ImportSourceTypeInfo importSourceType)
        {
            return new ImportSourceTypeDao().UpdateImportSourceType(importSourceType); 
        }

        public static ImportSourceTypeInfo GetImportSourceTypeInfo(int importSourceId)
        {
            return new ImportSourceTypeDao().GetImportSourceTypeInfo(importSourceId); 
        }

        public static string UploadImportSourceTypeIcon(HttpPostedFile postedFile)
        {
            string result;
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                result = string.Empty;
            }
            else
            {
                if (!Directory.Exists(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + HiContext.Current.GetStoragePath() + "/ImportSourceType/")))
                {
                    Directory.CreateDirectory(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + HiContext.Current.GetStoragePath() + "/ImportSourceType/"));
                }
                string text = HiContext.Current.GetStoragePath() + "/ImportSourceType/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
                postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
                result = text;
            }
            return result;
        }

        public static IList<ImportSourceTypeInfo> GetAllImportSourceTypes()
        {
            IList<ImportSourceTypeInfo> importSourceTypeInfo = HiCache.Get("DataCache-ImportSourceTypeInfo") as List<ImportSourceTypeInfo>;
            if (importSourceTypeInfo != null)
            {
                return importSourceTypeInfo;
            }
            IList<ImportSourceTypeInfo> list = new List<ImportSourceTypeInfo>();
            System.Data.DataTable importSourceTypes = new ImportSourceTypeDao().GetAllImportSourceTypes();
            System.Data.DataRow[] array = importSourceTypes.Select();
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToImportSourceType(array[i]));
            }
            HiCache.Insert("DataCache-ImportSourceTypeInfo", list, 60);//3¸öÐ¡Ê±»º´æ
            return list;
        }

        public static IList<ImportSourceTypeInfo> GetAllImportSourceTypes(int categoryId)
        {
            IList<ImportSourceTypeInfo> list = new List<ImportSourceTypeInfo>();
            System.Data.DataTable importSourceTypes = new ImportSourceTypeDao().GetAllImportSourceTypes(categoryId);
            System.Data.DataRow[] array = importSourceTypes.Select();

            for (int i = 0; i < array.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToImportSourceType(array[i]));
            }
            
            return list;
        }

        public static IList<ImportSourceTypeInfo> GetAllImportSourceBySupplierId(int supplierid)
        {
            IList<ImportSourceTypeInfo> list = new List<ImportSourceTypeInfo>();
            System.Data.DataTable importSourceTypes = new ImportSourceTypeDao().GetAllImportSourceBySupplierId(supplierid);
            System.Data.DataRow[] array = importSourceTypes.Select();

            for (int i = 0; i < array.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToImportSourceType(array[i]));
            }

            return list;
        }
	}
}

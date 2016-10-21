using EcShop.Entities.VShop;
using EcShop.SqlDal.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EcShop.ControlPanel.Store
{
    public class WXHelper
    {
        public static bool InsertLog(WXLog log)
        {
            return new WXLogDao().InsertLog(log);
        }
        public static bool UpdateLog(WXLog log)
        {
            return new WXLogDao().UpdateLog(log);
        }
        public static bool BathAddWXMsgRecord(DataTable dtMsgRecord)
        {
            return new WXMsgRecordDao().BathAddWXMsgRecord(dtMsgRecord);
        }
        public static WXLog GetWXLog(string strWhere)
        {
            return new WXLogDao().GetWXLog(strWhere);
        }
    }
}

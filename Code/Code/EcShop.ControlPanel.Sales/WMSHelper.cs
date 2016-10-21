using EcShop.SqlDal.WMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.ControlPanel.Sales
{
    public class WMSHelper
    {
        private WMSHelper()
		{
		}

         /// <summary>
        /// 记录WMS日志
        /// </summary>
        /// <param name="method">方法名</param>
        /// <param name="param">参数</param>
        /// <param name="logcontent">日志内容</param>
        /// <param name="logtype">日志类型 info error</param>
        /// <param name="methodtype">方法类型  in out </param>
        /// <returns></returns>
        public static  bool SaveLog(string method,string param,string logcontent,string logtype,string methodtype)
		{

            return new WMSLogDao().SaveLog(method, param, logcontent, logtype, methodtype);
		}
    }
}

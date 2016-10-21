using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Core.ErrorLog
{
    public sealed class ErrorLog
    {
        /// <summary>
        /// 写入操作日志到文件中
        /// </summary>
        /// <param name="moduleName">模块名字</param>
        /// <param name="message">错误文本信息</param>
        /// <param name="ex">异常</param>
        public static void Write(string moduleName, string message, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(moduleName);
            if (ex != null)
            {
                log.Error(message + ";" + ex.Message, ex);
            }
            if (!string.IsNullOrEmpty(message))
            {
                log.Info(message);
            }
            log = null;
        }

        public static void Write(string moduleName, object obj, Exception ex)
        {
            if (ex == null)
            {
                ex = new Exception();
            }
            string message = Newtonsoft.Json.JsonConvert.SerializeObject(obj) + "异常信息:" + ex.Message;
            log4net.ILog log = log4net.LogManager.GetLogger(moduleName);
            if (ex != null)
            {
                log.Error(message + ";" + ex.Message, ex);
            }
            if (!string.IsNullOrEmpty(message))
            {
               log.Info(message);
            }
            log = null;
        }

        /// <summary>
        /// 写入操作日志到文件中
        /// </summary>
        /// <param name="moduleName">模块名字</param>
        /// <param name="ex">异常</param>
        public static void Write(string moduleName, Exception ex)
        {
            Write(moduleName, moduleName, ex);
        }

        /// <summary>
        /// 写入过程数据或说明到文件中，以便跟踪
        /// </summary>
        /// <param name="moduleName">模块名字</param>
        /// <param name="ex">异常</param>
        public static void Write(string message)
        {
            Write(String.Empty, message, null);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using log4net;

namespace EcShop.Web.Api.Helper
{
    public class LoggerHelper
    {
        public static void Debug(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Api_Logger");
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
            log = null;
        }

        public static void Error(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Api_Logger");
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
            log = null;
        }

        public static void Fatal(string message)
        {

            log4net.ILog log = log4net.LogManager.GetLogger("Api_Logger");
            if (log.IsFatalEnabled)
            {
                log.Fatal(message);
            }
            log = null;
        }
        public static void Info(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Api_Logger");
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
            log = null;
        }

        public static void Warn(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Api_Logger");
            if (log.IsWarnEnabled)
            {
                log.Warn(message);
            }
            log = null;
        }
    }
}
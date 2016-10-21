using System;
using log4net;
using log4net.Config;

namespace EcShop.Web.Api.ApiException
{
    public enum LoggerType
    {
        /// <summary>
        /// 日志
        /// </summary>
        Info,
        /// <summary>
        /// 警告
        /// </summary>
        Warn,
        /// <summary>
        /// Debug
        /// </summary>
        Debug,
        /// <summary>
        /// 异常
        /// </summary>
        Error,
        /// <summary>
        /// 严重异常
        /// </summary>
        Fatal
    }

    /// <summary>
    /// 记录日志
    /// By Ocean 2012/3/7
    /// </summary>
    public class Logger
    {
        static Logger()
        {
            XmlConfigurator.Configure();
        }

        private static readonly ILog InfoLogger = LogManager.GetLogger("InfoLogger");
        private static readonly ILog WarnLogger = LogManager.GetLogger("WarnLogger");
        private static readonly ILog DebugLogger = LogManager.GetLogger("DebugLogger");
        private static readonly ILog ErrorLogger = LogManager.GetLogger("ErrorLogger");
        private static readonly ILog FatalLogger = LogManager.GetLogger("FatalLogger");
        private static readonly ILog SmtpLogger = LogManager.GetLogger("SmtpLogger");

                /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="loggerType">日志类型</param>
        public static void WriterLogger(object message)
        {
            WriterLogger(message, LoggerType.Info);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="loggerType">日志类型</param>
        public static void WriterLogger(object message, LoggerType loggerType)
        {
            switch (loggerType)
            {
                case LoggerType.Info:
                    InfoLogger.Info(message);
                    break;
                case LoggerType.Warn:
                    WarnLogger.Warn(message);
                    break;
                case LoggerType.Debug:
                    DebugLogger.Debug(message);
                    break;
                case LoggerType.Error:
                    ErrorLogger.Error(message);
                    SmtpLogger.Error(message);
                    break;
                case LoggerType.Fatal:
                    FatalLogger.Fatal(message);
                    SmtpLogger.Error(message);
                    break;
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="loggerType">日志类型</param>
        public static void WriterLogger(object message, System.Exception ex, LoggerType loggerType)
        {
            switch (loggerType)
            {
                case LoggerType.Info:
                    InfoLogger.Info(message, ex);
                    break;
                case LoggerType.Warn:
                    WarnLogger.Warn(message, ex);
                    break;
                case LoggerType.Debug:
                    DebugLogger.Debug(message, ex);
                    break;
                case LoggerType.Error:
                    ErrorLogger.Error(message, ex);
                    break;
                case LoggerType.Fatal:
                    FatalLogger.Fatal(message, ex);
                    break;
            }
        }

    }
}

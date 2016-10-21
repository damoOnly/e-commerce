using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace EcShop.Web.Api.ApiException
{
    /// <summary>
    /// 异常信息类
    /// By Ocean.deng @ 20131014 add
    /// </summary>
    public class Message
    {

        /// <summary>
        /// 装载异常信息
        /// </summary>
        public static List<FaultInfo> FaultInfoList = Load("EcShop.Web.Api.ApiException.Message.xml");

        /// <summary>
        /// 根据异常ID获得异常描述信息
        /// todo 待处理异常ID为 7、8
        /// </summary>
        /// <param name="exceptionId"></param>
        /// <returns></returns>
        public static FaultInfo GetInfo(int exceptionId)
        {
            FaultInfo result = null;
            try
            {
                result = (from faultInfo in FaultInfoList
                          where faultInfo.Code == exceptionId
                          select faultInfo).SingleOrDefault();
            }
            catch (System.Exception)
            {
               //
            }
            if (result == null)
            {
                //
            }
            return result;
        }

        /// <summary>
        /// 装载xml文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static List<FaultInfo> Load(string file)
        {
            Stream stream = null;
            var faultInfoList = new List<FaultInfo>();
            try
            {
                stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(file);
                var xmlSerializer = new XmlSerializer(typeof(List<FaultInfo>));
                if (stream != null)
                {
                    faultInfoList = (List<FaultInfo>)xmlSerializer.Deserialize(stream);
                }
            }
            finally
            {
                if (stream != null) stream.Close();
            }
            return faultInfoList;
        }
    }
}

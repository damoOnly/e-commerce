using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Web.Api.ApiException
{
    public class FaultInfo
    {
        public FaultInfo()
        {

        }

        public FaultInfo(string message)
        {
            this.Message = message;
        }

        public FaultInfo(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        /// <summary>
        /// 异常描述信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 异常ID
        /// </summary>
        public int Code { set; get; }
    }
}

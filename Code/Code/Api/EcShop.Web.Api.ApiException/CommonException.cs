using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Web.Api.ApiException
{
    /// <summary>
    /// 公共异常类
    /// </summary>
    public class CommonException : System.Exception
    {
        private readonly FaultInfo _faultInfo = new FaultInfo();
        public CommonException(int exceptionId)
        {
            _faultInfo = ApiException.Message.GetInfo(exceptionId);
        }

        public FaultInfo GetMessage()
        {
            return _faultInfo;
        }
    }
}

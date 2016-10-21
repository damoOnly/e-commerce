using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Web.Api.ApiException
{
    public class ItemsCannotbeRemovedException : System.Exception
    {
        private readonly FaultInfo _faultInfo = new FaultInfo();

        public ItemsCannotbeRemovedException(int id)
        {
            _faultInfo.Code = 8;
            _faultInfo.Message = string.Format("Some of items cannot be removed. ID:{0}", id);
        }

        public FaultInfo GetMessage()
        {
            return _faultInfo;
        }
    }
}

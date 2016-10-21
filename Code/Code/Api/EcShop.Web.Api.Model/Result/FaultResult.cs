using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Web.Api.Model.Result
{
    public class FaultResult
    {
        public FaultResult()
        { }

        public FaultResult(Fault errorResponse)
        {
            this.code = errorResponse.code;
            this.msg = errorResponse.msg;
            this.parameters = errorResponse.parameters;
        }

        public FaultResult(int code, string msg, string param)
        {
            this.code = code;
            this.msg = msg;
            this.parameters = param;
        }

        public int code { get; set; }
        public string msg { get; set; }
        public string parameters { get; set; }
    }

    public class Fault
    {
        public Fault()
        {

        }

        public Fault(int code, string msg, string param)
        {
            this.code = code;
            this.msg = msg;
            this.parameters = param;
        }

        public int code { get; set; }
        public string msg { get; set; }
        public string parameters { get; set; }
    }
}

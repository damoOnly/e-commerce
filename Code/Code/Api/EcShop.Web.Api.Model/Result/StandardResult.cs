using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Web.Api.Model.Result
{
    public class StandardResult<T>
    {
        /// <summary>
        /// 返回code
        /// </summary>
        public int code { set; get; }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public string msg { set; get; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public T data { set; get; }

    }
}

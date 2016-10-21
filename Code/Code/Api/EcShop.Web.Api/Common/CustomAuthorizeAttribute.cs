using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcShop.Web.Api.ApiException;

namespace EcShop.Web.Api
{

    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //如果是在测试模式下就取消授权验证
            if(!string.IsNullOrEmpty(httpContext.Request["UnitTest"]))
            {
                return ("true".Equals(httpContext.Request["UnitTest"]));
            }
            return base.AuthorizeCore(httpContext);
        }
    }
}
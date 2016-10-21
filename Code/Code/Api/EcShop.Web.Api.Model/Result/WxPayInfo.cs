using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class WxPayInfo
    {

        public string AppId{get;set;}
        public string Key{get;set;}

        public string AppSecret{get;set;}

        public string Mch_id{get;set;}
    }
}

using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Entities;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.SqlDal.Commodities;
using EcShop.SqlDal.VShop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;

namespace EcShop.ControlPanel.Store
{
   public class PCHelper
    {
       public static DbQueryResult GettopicList(TopicQuery page)
       {
           return new TopicDao().GetTopicList(page);
       }
    }
}

using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commodities
{
    public class BrandTagQuery : Pagination
    {
       public string TagName
       {
           get;
           set;
       }
    }
}

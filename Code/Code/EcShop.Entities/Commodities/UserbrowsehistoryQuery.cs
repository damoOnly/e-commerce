using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop
{
    public class UserbrowsehistoryQuery : Pagination
    {
        public string UserName
        {
            get;
            set;
        }
    }
}

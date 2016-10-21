using EcShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commodities
{
    public class ProductFavoriteQuery: Pagination
    {
        public int UserId { get; set; }

        public int GradeId { get; set; }
    }
}

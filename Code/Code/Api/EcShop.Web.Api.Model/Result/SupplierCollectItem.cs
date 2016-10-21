using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class SupplierCollectItem
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string ShopOwner { get; set; }

        public string Province { get; set; }

        public string CreateDate { get; set; }

        public string Logo { get; set; }

        public int CollectCount { get; set; }

        public string Background { get; set; }
    }

    public class SupplierCollectResult
    {
        public int ProductCollectCount { get; set; }


        public int SupplierCollectCount { set; get; }

        /// <summary>
        /// 返回的记录集
        /// </summary>
        public List<SupplierCollectItem> Results { set; get; }
    }
}

using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Promotions
{
    public class VoucherItemInfoQuery : Pagination
    {
        public int? VoucherId
        {
            get;
            set;
        }
        public string OrderId
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
        public string VoucherName
        {
            get;
            set;
        }
        public int? VoucherStatus
        {
            get;
            set;
        }
    }
}

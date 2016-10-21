using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class GiftItem
    {
        public int GiftId
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public decimal CostPrice
        {
            get;
            set;
        }
        public int NeedPoint
        {
            get;
            set;
        }
        public int Quantity
        {
            get;
            set;
        }
        public string ThumbnailUrl40
        {
            get;
            set;
        }
        public string ThumbnailUrl60
        {
            get;
            set;
        }
        public string ThumbnailUrl100
        {
            get;
            set;
        }
        public int PromoType
        {
            get;
            set;
        }
        public int SubPointTotal
        {
            get;
            set;
        }
    }
}

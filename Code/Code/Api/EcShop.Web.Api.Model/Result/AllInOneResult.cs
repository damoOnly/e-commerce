using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class AllInOneResult
    {
        public int code { get; set; }
        public string msg { get; set; }
        public AllInOneData data { get; set; }
    }

    public class AllInOneIosResult
    {
        public int code { get; set; }
        public string msg { get; set; }
        public AllInOneIosData data { get; set; }
    }

    

    public class AllInOneData
    {
        public AllInOneData() { }
        public ListResult<TopicListItem> Topic { get; set; }
        public ListResult<BannerListItem> Banner { get; set; }
        public ListResult<NavigationListItem> Navigation { get; set; }
        public ListResult<PlateListItem> HotSale { get; set; }
        public ListResult<PlateListItem> Recommend { get; set; }
        public ListResult<PlateListItem> Promotional { get; set; }
        public ListResult<ProductListItem> SuggestProduct { get; set; }

        public ListResult<IconListItem> Icon { get; set; }

        public ListResult<SimpleCountDownProductListItem> CountDownProduct { get; set; }

        public ListResult<BannerListItem> RegisterBanner { get;set;}

        public string ShoppingcartQuantity { get; set; }

    }

    public class AllInOneIosData
    {
        public AllInOneIosData() { }
        public ListResult<TopicListItem> Topic { get; set; }
        public ListResult<BannerListItem> Banner { get; set; }
        public ListResult<NavigationListItem> Navigation { get; set; }
        public ListResult<PlateListItem> HotSale { get; set; }
        public ListResult<PlateListItem> Recommend { get; set; }
        public ListResult<PlateListItem> Promotional { get; set; }
        public ListResult<ProductListItem> SuggestProduct { get; set; }

        public ListResult<IconListItem> Icon { get; set; }

        public ListResult<IOSSimpleCountDownProductListItem> CountDownProduct { get; set; }

        public ListResult<BannerListItem> RegisterBanner { get; set; }

        public string ShoppingcartQuantity { get; set; }

    }


    public class SimpleCountDownProductListItem
    {
        public string ImageUrl { get; set; }

        //public decimal SalePrice { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal CountDownPrice { get; set; }
    }

    public class IOSSimpleCountDownProductListItem
    {
        public string ImageUrl { get; set; }
        public string MarketPrice { get; set; }
        public string CountDownPrice { get; set; }
    }

    public class SimpleShoppingCart
    {
        public string Quantity { get; set; }
    }
}

using System;
using System.Data;
namespace EcShop.Entities.Commodities
{
	public class ProductBrowseInfo
	{
		public ProductInfo Product
		{
			get;
			set;
		}
		public string BrandName
		{
			get;
			set;
		}
		public string CategoryName
		{
			get;
			set;
		}
		public DataTable DbAttribute
		{
			get;
			set;
		}

        public int cIsDisable { get; set; }

		public DataTable DbSKUs
		{
			get;
			set;
		}
		public DataTable DbCorrelatives
		{
			get;
			set;
		}
		public DataTable DBReviews
		{
			get;
			set;
		}
		public DataTable DBConsultations
		{
			get;
			set;
		}
        public DataTable DBHotSale//热卖，按销售量过滤
        {
            get;
            set;
        }
        public string CategoryNote3//商品分类的广告图3
        {
            get;
            set;
        }
        public string CategoryId
        {
            get;
            set;
        }
		public int ReviewCount
		{
			get;
			set;
		}
		public int ConsultationCount
		{
			get;
			set;
		}
        public bool IsApproved
        {
            get;
            set;
        }


        public string SupplierName
        {
            get;
            set;
        }

        public string SupplierImageUrl
        {
            get;
            set;
        }

        public string SupplierLogo
        {
            get;
            set;
        }
       

	}
}

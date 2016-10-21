using EcShop.Core;
using EcShop.Entities.Promotions;
using EcShop.SaleSystem.Catalog;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.Tags
{
	public class VVoucherImage : WebControl
	{
        public const string TagID = "vVoucherImage";
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		public int ProductId
		{
			get;
			set;
		}

        public int DataField
        {
            get;
            set;
        }

		public bool IsAnonymous
		{
			get;
			set;
		}
        public VVoucherImage()
		{
            base.ID = "vVoucherImage";
		}
		protected override void Render(HtmlTextWriter writer)
		{
            string value = "<div class='CouponImage CouponFour'></div><div class='CouponImage CouponThree'></div><div class='CouponImage CouponSecond'></div><div class='CouponImage CouponSecond'></div><div class='CouponImage CouponFirst'></div>";
		    writer.Write(value);
		}
	}
}

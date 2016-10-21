using EcShop.Core;
using System;
namespace EcShop.Entities.Sales
{
	public class ShippingAddressInfo
	{
		public bool _isdefault = false;
		public int ShippingId
		{
			get;
			set;
		}
		public int RegionId
		{
			get;
			set;
		}
		public int UserId
		{
			get;
			set;
		}
		[HtmlCoding]
		public string ShipTo
		{
			get;
			set;
		}
		[HtmlCoding]
		public string Address
		{
			get;
			set;
		}
		public string Zipcode
		{
			get;
			set;
		}
		[HtmlCoding]
		public string TelPhone
		{
			get;
			set;
		}
		[HtmlCoding]
		public string CellPhone
		{
			get;
			set;
		}
		public bool IsDefault
		{
			get
			{
				return this._isdefault;
			}
			set
			{
				this._isdefault = value;
			}
		}
        [HtmlCoding]
        public string IdentityCard
        {
            get;
            set;
        }
	}
}

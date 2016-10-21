using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class AddressItem
    {
        public AddressItem() { }
        public AddressItem(int id, int regionId, string province, string city, string district, string town, string address, string zipcode, string receiver, string telephone, string cellphone, bool isDefault)
        {
            this.Id = id;
            this.RegionId = regionId;
            this.Province = province;
            this.City = city;
            this.District = district;
            this.Town = town;
            this.Address = address;
            this.Zipcode = zipcode;
            this.Receiver = receiver;
            this.Telephone = telephone;
            this.Cellphone = cellphone;
            this.IsDefault = IsDefault;
        }

        public int Id
        {
            get;
            set;
        }
        public int RegionId
        {
            get;
            set;
        }

        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Town { get; set; }

        public string Receiver
        {
            get;
            set;
        }

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
        public string Telephone
        {
            get;
            set;
        }

        public string Cellphone
        {
            get;
            set;
        }
        public bool IsDefault
        {
            get;
            set;
        }

        public string IdNo
        {
            get;
            set;
        }

        public bool IsLatest { get; set; }
    }
}

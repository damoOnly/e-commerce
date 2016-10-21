using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamAddress : ParamBase
    {
        public int Id { get; set; }
        public string UserId { get; set; }
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
    }

}

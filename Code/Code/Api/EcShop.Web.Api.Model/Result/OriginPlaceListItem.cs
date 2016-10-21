using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class OriginPlaceListItem
    {
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string Icon { get; set; }
        public int DisplaySequence { get; set; }

    }
}

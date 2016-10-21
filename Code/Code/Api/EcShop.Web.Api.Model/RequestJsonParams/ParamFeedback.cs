using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.RequestJsonParams
{
    public class ParamFeedback : ParamUserBase
    {
        public int FeedbackType { get; set; }
        public string Content { get; set; }
        public string ContactWay { get; set; }

        public List<SubmitImage> Images { get; set; }
    }


    public class SubmitImage
    {
        public string Image { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class TopicListItem
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string Icon { get; set; }
        public string Content { get; set; }
        public int DisplaySequence { get; set; }

        public int SupplierId { get; set; }
    }
}

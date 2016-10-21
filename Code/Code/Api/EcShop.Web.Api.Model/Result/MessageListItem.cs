using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class MessageListItem
    {
        public long Id { get; set; }
        public string Accepter { get; set; }
        public string Sender { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string SendDate { get; set; }
        public bool IsRead { get; set; }
    }
}

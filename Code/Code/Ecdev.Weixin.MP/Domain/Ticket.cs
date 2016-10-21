using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecdev.Weixin.MP.Domain
{
    /// <summary>
    /// js api ticket
    /// </summary>
    public class Ticket
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public string expires_in { get; set; }
    }
}

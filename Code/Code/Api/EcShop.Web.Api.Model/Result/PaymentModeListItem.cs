using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class PaymentModeListItem
    {
        public PaymentModeListItem() { }
        public PaymentModeListItem(int id, string name, string icon, string description, bool isLatest, int displaySequence, string gateway)
        {
            this.Id = id;
            this.Name = name;
            this.Icon = icon; ;
            this.Description = description;
            this.IsLatest = isLatest;
            this.DisplaySequence = displaySequence;
            this.Gateway = gateway;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public bool IsLatest { get; set; }
        public int DisplaySequence { get; set; }
        public string Gateway { get; set; }

    }
}

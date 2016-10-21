using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class ShippingModeListItem
    {
        public ShippingModeListItem() { }
        public ShippingModeListItem(int id, string name, int templateId, string templateName, string description, int displaySequence)
        {
            this.Id = id;
            this.Name = name;
            this.TemplateId = templateId;
            this.TemplateName = TemplateName;
            this.DisplaySequence = displaySequence;
            this.Description = description;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public int DisplaySequence { get; set; }
        public string Description { get; set; }
    }
}

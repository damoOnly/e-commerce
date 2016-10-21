using System;
namespace EcShop.Entities
{
    public class VshopDesignTemplate
    {
        public int Id
        {
            get;
            set;
        }

        public int? SupplierId { get; set; }

        public string TemplateName
        {
            get;
            set;
        }
        public DateTime AddTime
        {
            get;
            set;
        }
        public bool InUse
        {
            get;
            set;
        }
        public int TemplateType
        {
            get;
            set;
        }
        public string Content
        {
            get;
            set;
        }
    }
}

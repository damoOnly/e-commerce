using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class DictionaryListItem
    {
        public DictionaryListItem()
        {

        }

        public DictionaryListItem(int id, string code, string name, string description, int displaySequence)
        {
            this.Id = id;
            this.Code = code;
            this.Name = name;
            this.Description = description;
            this.DisplaySequence = displaySequence;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplaySequence { get; set; }
    }
}

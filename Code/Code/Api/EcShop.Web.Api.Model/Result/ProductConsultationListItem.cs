using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class ProductConsultationListItem
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string AskDate { get; set; }
        public string Content { get; set; }
        public int QuestionType { get; set; }
        public string ReplyContent { get; set; }
        public string ReplyDate { get; set; }
        public string ReplyUsername { get; set; }
    }
}

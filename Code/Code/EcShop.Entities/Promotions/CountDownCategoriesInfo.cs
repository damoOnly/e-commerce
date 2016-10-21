using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.Promotions
{
    public class CountDownCategoriesInfo
        {
            public int CountDownCategoryId
            {
                get;
                set;
            }
            public string Title
            {
                get;
                set;
            }
            public System.DateTime StartTime
            {
                get;
                set;
            }
            public System.DateTime EndTime
            {
                get;
                set;
            }
            public string AdImageUrl
            {
                get;
                set;
            }
            public string AdImageLinkUrl
            {
                get;
                set;
            }
            public int DisplaySequence
            {
                get;
                set;
            }

            public int CreatedBy
            {
                get;
                set;
            }
            public System.DateTime CreatedOn { get; set; }

            public int UpdatedBy
            {
                get;
                set;
            }

            public System.DateTime UpdatedOn { get; set; }
        }
 
}

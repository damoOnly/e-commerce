using Ecdev.Components.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.Commodities
{
   public class BrandTagInfo
    {
       private System.Collections.Generic.IList<int> brandCategoryInfos;
        public int BrandTagId
        {
            get;
            set;
        }
        [StringLengthValidator(1, 14, Ruleset = "ValProductType", MessageTemplate = "长度限制在25个字符以内")]
        public string TagName
        {
            get;
            set;
        }
        public int TagSort
        {
            get;
            set;
        }       
        public System.Collections.Generic.IList<int> BrandCategoryInfos
        {
            get
            {
                if (this.brandCategoryInfos == null)
                {
                    this.brandCategoryInfos = new System.Collections.Generic.List<int>();
                }
                return this.brandCategoryInfos;
            }
            set
            {
                this.brandCategoryInfos = value;
            }
        }
    }
}

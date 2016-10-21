using Ecdev.Components.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.Commodities
{
    public class BaseUnitsInfo
    {
        public int ID
        {
            get;
            set;
        }

        [StringLengthValidator(1, 4, Ruleset = "ValBaseUnits", MessageTemplate = "长度限制在4个字符以内")]
        public string HSJoinID
        {
            get;
            set;
        }

        [StringLengthValidator(1, 50, Ruleset = "ValBaseUnits", MessageTemplate = "长度限制在50个字符以内")]
        public string Name_CN
        {
            get;
            set;
        }
        public string Name_En
        {
            get;
            set;
        }
        public int ForbitFlag
        {
            get;
            set;
        }
        public int Sort
        {
            get;
            set;
        }
        public int DeleteFlag
        {
            get;
            set;
        }

        public string CreateUser
        {
            get;
            set;
        }
        public DateTime CreateTime
        {
            get;
            set;
        }
        public string UpdateUser
        {
            get;
            set;
        }
        public DateTime UpdateTime
        {
            get;
            set;
        }

        public string DeleteUser
        {
            get;
            set;
        }

        public DateTime DeleteTime
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class MemberResult
    {
        public MemberResult() {}
        public MemberResult(string avatar, string mobile, string email, string name, string qq, string username, decimal balance, decimal consumption, int gender, int addresscount, int referralStatus)
        {
            this.Avatar = avatar;
            this.Mobile = mobile;
            this.Email = email;
            this.Name = name;
            this.QQ = qq;
            this.Username = username;
            this.Balance = balance;
            this.Consumption = consumption;
            this.Gender = gender;
            this.AddressCount = addresscount;
            this.ReferralStatus = referralStatus;
        }

        public MemberResult(string avatar, string mobile, string email, string name, string qq, string username, decimal balance, decimal consumption, int gender, int addresscount, int referralStatus, bool IsVerify,string IdNo)
        {
            this.Avatar = avatar;
            this.Mobile = mobile;
            this.Email = email;
            this.Name = name;
            this.QQ = qq;
            this.Username = username;
            this.Balance = balance;
            this.Consumption = consumption;
            this.Gender = gender;
            this.AddressCount = addresscount;
            this.ReferralStatus = referralStatus;

            this.IsVerify = IsVerify;

            this.IdNo = IdNo;
        }

        public string Avatar { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string QQ { get; set; }
        public string Username { get; set; }
        public decimal Balance { get; set; }
        public decimal Consumption { get; set; }

        public int Gender { get; set; }

        public int AddressCount { get; set; }

        public int ReferralStatus { get; set; }


        public bool IsVerify { get; set; }

        public string IdNo { get; set; }



    }
}

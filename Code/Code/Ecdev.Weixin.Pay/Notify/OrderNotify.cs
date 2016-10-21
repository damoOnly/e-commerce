using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecdev.Weixin.Pay.Notify
{
    public class OrderNotify : NotifyObject
    {
        public string return_code
        {
            get;
            set;
        }
        public string return_msg
        {
            get;
            set;
        }
        public string appid
        {
            get;
            set;
        }
        public string mch_id
        {
            get;
            set;
        }
        public string device_info
        {
            get;
            set;
        }
        public string nonce_str
        {
            get;
            set;
        }
        public string sign
        {
            get;
            set;
        }
        public string result_code
        {
            get;
            set;
        }
        public string openid
        {
            get;
            set;
        }
        public string is_subscribe
        {
            get;
            set;
        }
        public string trade_type
        {
            get;
            set;
        }
        public string bank_type
        {
            get;
            set;
        }
        public int total_fee
        {
            get;
            set;
        }
        public string fee_type
        {
            get;
            set;
        }
        public string transaction_id
        {
            get;
            set;
        }
        public string out_trade_no
        {
            get;
            set;
        }
        public string attach
        {
            get;
            set;
        }
        public string time_end
        {
            get;
            set;
        }
        public string trade_state
        {
            get;
            set;
        }
    }
}

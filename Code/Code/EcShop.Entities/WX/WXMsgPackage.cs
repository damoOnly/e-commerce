using Newtonsoft.Json;
using System;
namespace EcShop.Entities.VShop
{
    public class WXMsgPackage
    {
        [JsonProperty("errcode")]
        public string ErrCode
        {
            get;
            set;
        }
        [JsonProperty("errmsg")]
        public string ErrMsg
        {
            get;
            set;
        }
        [JsonProperty("retcode")]
        public string RetCode
        {
            get;
            set;
        }
        [JsonProperty("recordlist")]
        public WXMsgRecord[] RecordList
        {
            get;
            set;
        }
    }
}

using EcShop.Core;
using Newtonsoft.Json;
using System;

namespace EcShop.Entities.VShop
{
    public class WXMsgRecord
    {

        public int Id
        {
            get;
            set;
        }
        [JsonProperty("openid")]
        public string OpenId
        {
            get;
            set;
        }
        [JsonProperty("opercode")]
        public string OperCode
        {
            get;
            set;
        }
        [JsonProperty("text")]
        public string Text
        {
            get;
            set;
        }

        public DateTime Time
        {
            get
            {
               return DataHelper.ConvertTimeFromUniversal(this.UniversalTime);
            }
        }
        [JsonProperty("worker")]
        public string Worker
        {
            get;
            set;
        }
        [JsonProperty("time")]
        public long UniversalTime
        {
            get;
            set;
        }
        public string WorkerNo
        {
            get
            {
                if (string.IsNullOrEmpty(this.Worker) || this.Worker.Trim() == "")
                {
                    return " ";
                }
                else
                {
                    return this.Worker.Substring(0, this.Worker.IndexOf('@'));
                }
            }
        }
        public DateTime HappenDate
        {
            get 
            {
                return this.Time.Date;
            }
        }
        public string HappenMonth
        {
            get
            {
                return this.Time.ToString("yyyy-MM");
            }
        }

    }
}

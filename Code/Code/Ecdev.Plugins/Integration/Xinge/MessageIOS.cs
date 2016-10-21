using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ecdev.Plugins.Integration.Xinge
{
    public class MessageIOS
    {
        public MessageIOS()
        {
            this.SendTime = "2014-03-13 16:13:00";
            this.AcceptTimes = new List<AcceptTime>();
            this.Raw = "";
            this.AlertStr = "";
            this.AlertJson = new JObject();
            this.Badge = 0;
            this.Sound = "";
            this.Category = "";
            this.LoopInterval = -1;
            this.LoopTimes = -1;

            this.CustomContent = new Dictionary<string, object>();
        }

        public bool IsValid()
	    {
		    if (ExpireTime < 0 || ExpireTime > 3*24*60*60)
			    return false;

            DateTime dtSendTime = DateTime.MinValue;
            
            if (!DateTime.TryParse(SendTime, out dtSendTime))
            {
                return false;
            }

		    if (!string.IsNullOrEmpty(Raw)) 
                return true;

		    foreach (AcceptTime ti in AcceptTimes) {
			    if(!ti.IsValid()) 
                    return false;
		    }

		    if(string.IsNullOrEmpty(AlertStr) && !AlertJson.HasValues)
			    return false;
		
		    return true;
	    }

        public string ToJsonString()
        {
            if (!string.IsNullOrEmpty(Raw)) 
                return Raw;

            JObject json = new JObject();

            if (CustomContent.Count > 0)
            {
                JObject customJson = new JObject();

                foreach (KeyValuePair<string, object> pair in CustomContent)
                {
                    object o = pair.Value;

                    if (o is int)
                    {
                        customJson.Add(pair.Key, Convert.ToInt32(o));
                    }
                    else if(o is DateTime)
                    {
                        customJson.Add(pair.Key, Convert.ToDateTime(o).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        customJson.Add(pair.Key, Convert.ToString(o));
                    }
                }

                json.Add("custom_content", customJson);
            }
            
            if (AcceptTimes.Count > 0)
            {
                JArray array = new JArray();

                foreach (AcceptTime at in this.AcceptTimes)
                {
                    array.Add(at.ToJsonObject());
                }

                json.Add("accept_time", array);
            }
            
            JObject aps = new JObject();
            if (AlertJson.HasValues)
                aps.Add("alert", AlertJson);
            else
                aps.Add("alert", AlertStr);

            if (Badge != 0) 
                aps.Add("badge", Badge);

            if (!string.IsNullOrEmpty(Sound)) 
                aps.Add("sound", Sound);
            if (!string.IsNullOrEmpty(Category)) 
                aps.Add("category", Category);

            json.Add("aps", aps);

            return json.ToString();
        }

        public int ExpireTime { get; set; }
        public string SendTime { get; set; }
        public List<AcceptTime> AcceptTimes { get; set; }
        public IDictionary<string, object> CustomContent { get; set; }
        public string Raw { get; set; }
        public string AlertStr { get; set; }
        public JObject AlertJson { get; set; }
        public int Badge { get; set; }
        public string Sound { get; set; }
        public string Category { get; set; }
        public int LoopInterval { get; set; }
        public int LoopTimes { get; set; }
    }
}

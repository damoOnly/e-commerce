using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ecdev.Plugins.Integration.Xinge
{
    public enum MessageType
    {
        TYPE_NOTIFICATION  = 1,
	    TYPE_MESSAGE = 2
    }

    public class Message
    {
        public Message()
        {
            this.Title = "";
            this.Content = "";
            this.SendTime = "2013-12-20 18:31:00";
            this.AcceptTimes = new List<AcceptTime>();
            this.MultiPkg = 0;
            this.Raw = "";
            this.LoopInterval = -1;
            this.LoopTimes = -1;
            this.Action = new ClickAction();
            this.Style = new Style(0);

            this.CustomContent = new Dictionary<string, object>();
        }

        public bool IsValid()
	    {
		    if (!string.IsNullOrEmpty(Raw)) 
                return true;

		    if (MessageType < MessageType.TYPE_NOTIFICATION || MessageType > MessageType.TYPE_MESSAGE) 
			    return false;

		    if (MultiPkg < 0 || MultiPkg > 1)
			    return false;

		    if (MessageType == MessageType.TYPE_NOTIFICATION)
		    {
			    if(!Style.IsValid()) 
                    return false;

			    if(!Action.IsValid()) 
                    return false;
		    }

		    if (ExpireTime < 0 || ExpireTime > 3 * 24 * 60 * 60)
			    return false;

            DateTime dtSendTime = DateTime.MinValue;

            if (!DateTime.TryParse(SendTime, out dtSendTime))
            {
                return false;
            }

		    foreach (AcceptTime ti in AcceptTimes) {
			    if(!ti.IsValid()) 
                    return false;
		    }

		    if (LoopInterval > 0 && LoopTimes > 0 
				    && ((LoopTimes - 1) * LoopInterval + 1) > 15) {
			    return false;
		    }
		
		    return true;
	    }

        public string ToJsonString()
        {
            if (!string.IsNullOrEmpty(Raw)) 
                return Raw;

            JObject json = new JObject();

            if (MessageType == MessageType.TYPE_NOTIFICATION)
            {
                json.Add("title", Title);
                json.Add("content", Content);
                json.Add("builder_id", Style.BuilderId);
                json.Add("ring", Style.Ring);
                json.Add("vibrate", Style.Vibrate);
                json.Add("clearable", Style.Clearable);
                json.Add("n_id", Style.Nid);
                json.Add("ring_raw", Style.RingRaw);
                json.Add("lights", Style.Lights);
                json.Add("icon_type", Style.IconType);
                json.Add("icon_res", Style.IconRes);
                json.Add("style_id", Style.StyleId);
                json.Add("small_icon", Style.SmallIcon);
                json.Add("action", Action.ToJsonObject());
            }
            else if (MessageType == MessageType.TYPE_MESSAGE)
            {
                json.Add("title", Title);
                json.Add("content", Content);
                if (AcceptTimes.Count > 0)
                {
                    JArray array = new JArray(this.AcceptTimes);
                    json.Add("accept_time", array);
                }
            }

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


            return json.ToString();
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public int ExpireTime { get; set; }
        public string SendTime { get; set; }
        public List<AcceptTime> AcceptTimes { get; set; }
        public MessageType MessageType { get; set; }
        public int MultiPkg { get; set; }
        public Style Style { get; set; }
        public ClickAction Action { get; set; }
        public IDictionary<string, object> CustomContent { get; set; }
        public string Raw { get; set; }
        public int LoopInterval { get; set; }
        public int LoopTimes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ecdev.Plugins.Integration.Xinge
{
    public class AcceptTime
    {
        public AcceptTime()
        {

        }

        public AcceptTime(int startHour, int startMin, int endHour, int endMin)
        {
            this.Start = new TimeInterval(startHour, startMin);
            this.End = new TimeInterval(endHour, endMin);
        }

        public TimeInterval Start { get; set; }
        public TimeInterval End { get; set; }
        
        public bool IsValid()
        {
            if (this.Start.Hour >= 0 && this.Start.Hour <= 23 &&
                this.Start.Min >= 0 && this.Start.Min <= 59 &&
                this.End.Hour >= 0 && this.End.Hour <= 23 &&
                this.End.Min >= 0 && this.End.Min <= 59)
                return true;
            else
                return false;
        }

        public JObject ToJsonObject()
        {
            JObject json = new JObject();
            JObject js = new JObject();
            JObject je = new JObject();

            js.Add("hour", this.Start.Hour.ToString());
            js.Add("min", this.Start.Min.ToString());
            je.Add("hour", this.End.Hour.ToString());
            je.Add("min", this.End.Min.ToString());

            json.Add("start", js);
            json.Add("end", je);

            return json;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Ecdev.Plugins.Integration.Xinge
{
    public class TimeInterval
    {
        public TimeInterval() { }
        public TimeInterval(int hour, int min)
        {
            this.Hour = hour;
            this.Min = min;
        }

        public int Hour { get; set; }
        public int Min { get; set; }
    }
}

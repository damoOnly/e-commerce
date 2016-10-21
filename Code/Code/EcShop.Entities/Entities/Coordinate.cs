using Newtonsoft.Json;
using System;
namespace Entities
{
    /// <summary>
    /// 坐标信息，调用百度api返回的信息
    /// </summary>
    public class CoordinateInfo
    {
        [JsonProperty("status")]
        public int Status
        {
            get;
            set;
        }
        [JsonProperty("result")]
        public Coordinate[] result
        {
            get;
            set;
        }
    }
    public class Coordinate
    {
        [JsonProperty("x")]
        public double X
        {
            get;
            set;
        }
        [JsonProperty("y")]
        public double Y
        {
            get;
            set;
        }
    }
}
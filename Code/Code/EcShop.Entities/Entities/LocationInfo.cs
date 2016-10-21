using Newtonsoft.Json;
using System;
namespace Entities
{
    /// <summary>
    /// 地理位置信息，调用百度api返回的信息
    /// </summary>
    public class LocationInfo
    {
        [JsonProperty("status")]
        public int Status
        {
            get;
            set;
        }
          [JsonProperty("result")]
        public Result Result
        {
            get;
            set;
        }
    }
    public class Result
    {
        //[JsonProperty("location")]
        //public Location Location { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("business")]
        public string Business { get; set; }

        [JsonProperty("addressComponent")]
        public AddressComponent AddressComponent { get; set; }

        //[JsonProperty("pois")]
        //public Pois[] Pois { get; set; }

        //[JsonProperty("poiRegions")]
        //public PoiRegion[] PoiRegions { get; set; }

        [JsonProperty("sematic_description")]
        public string SematicDescription { get; set; }

        [JsonProperty("cityCode")]
        public int CityCode { get; set; }
    }
    public class AddressComponent
    {
        [JsonProperty("country")]
        public string Country
        {
            get;
            set;
        }
        [JsonProperty("province")]
        public string Province
        {
            get;
            set;
        }
        [JsonProperty("city")]
        public string City
        {
            get;
            set;
        }
        [JsonProperty("district")]
        public string District
        {
            get;
            set;
        }
        [JsonProperty("street")]
        public string Street
        {
            get;
            set;
        }
        [JsonProperty("street_number")]
        public string StreetNumber
        {
            get;
            set;
        }
        [JsonProperty("country_code")]
        public string CountryCode
        {
            get;
            set;
        }
    }
}

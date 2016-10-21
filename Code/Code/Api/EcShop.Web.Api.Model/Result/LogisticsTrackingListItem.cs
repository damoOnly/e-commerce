using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcShop.Web.Api.Model.Result
{
    public class LogisticsTrackingResult
    {
        public LogisticsTrackingResult()
        {
            this.TrackingItems = new List<TrackingDataListItem>();
        }

        public string TrackingNumber { get; set; }
        public string LogisticsName { get; set; }
        public string DeliveryDate { get; set; }

        public List<TrackingDataListItem> TrackingItems { get; set; }
    }

    public class TrackingDataListItem
    {
        public TrackingDataListItem()
        {

        }

        public TrackingDataListItem(string time, string content)
        {
            this.Time = time;
            this.Content = content;
        }
        public string Time { get; set; }
        public string Content { get; set; }
    }
}

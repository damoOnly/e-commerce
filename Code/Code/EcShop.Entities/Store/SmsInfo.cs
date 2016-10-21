using System;
using EcShop.Core.Entities;

namespace EcShop.Entities.Store
{
    public class SmsInfo
    {
        public Guid SmsId { get; set; } 
        public int Priority { get; set; } 
        public string Mobile { get; set; } 
        public string Subject { get; set; } 
        public string Body { get; set; } 
        public string NextTryTime { get; set; } 
        public string NumberOfTries { get; set; }

        public int type { get; set; }

        public string ClaimCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Entities.OAuth
{
    public class OAuthInfo
    {
        public int Id { get; set; } 
        public string AppId { get; set; } 
        public string AppSecret { get; set; } 
        public string Token { get; set; } 
        public DateTime? ExpirationUtc { get; set; } 
        public DateTime? IssueDateUtc { get; set; } 
        public string DeviceId { get; set; } 
        public int DeviceType { get; set; } 
        public int Channel { get; set; }
        public string ApiVersion { get; set; }
        public string SessionKey { get; set; }
        public string SessionSecret { get; set; } 
    }
}

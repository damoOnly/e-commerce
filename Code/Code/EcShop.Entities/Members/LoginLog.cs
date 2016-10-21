using System;
namespace EcShop.Entities.Members
{
    public class LoginLog
	{
        public DateTime? AddTime { get; set; }
        public int ErrorCount { get; set; }
        public int ID { get; set; }
        public string LoginIP { get; set; }
        public int LogType { get; set; }
        public string MemberName { get; set; }
        public int Type { get; set; }
	}
}

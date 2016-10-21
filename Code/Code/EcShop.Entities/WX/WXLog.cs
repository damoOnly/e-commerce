using System;
namespace EcShop.Entities.VShop
{
    public class WXLog
    {
        public int Id
        {
            get;
            set;
        }
        public DateTime AddTime
        {
            get;
            set;
        }
        public DateTime UpdateTime
        {
            get;
            set;
        }
        public DateTime StartTime
        {
            get;
            set;
        }
        public DateTime EndTime
        {
            get;
            set;
        }
        public int Type
        {
            get;
            set;
        }
        public string Remark
        {
            get;
            set;
        }
        public bool IsSuccess
        {
            get;
            set;
        }
    }
}
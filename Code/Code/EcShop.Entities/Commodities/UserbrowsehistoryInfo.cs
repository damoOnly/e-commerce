using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop
{
    public class UserbrowsehistoryInfo
    {
        public int HistoryId
        {
            get;
            set;
        }

        public int ProductId
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public DateTime? BrowseTime
        {
            get;
            set;
        }

        public string UserIP
        {
            get;
            set;
        }

        public long Ip
        {
            get;
            set;
        }

        public int Sort
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        //用户浏览URL
        public string Url
        {
            get;
            set;
        }

        //平台类型，1：PC， 2:微信
        public int PlatType
        {
            get;
            set;
        }

        //浏览次数
        public int BrowerTimes
        {
            get;
            set;
        }

        public int CategoryId
        {
            get;
            set;
        }
    }
}

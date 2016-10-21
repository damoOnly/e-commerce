using EcShop.Core;
using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.Commodities
{
    public class ImportSourceTypeQuery : Pagination
	{
		[HtmlCoding]
        public string Remark
		{
			get;
			set;
		}

        public string Icon
        {
            get;
            set;
        }

        public string EnArea
        {
            get;
            set;
        }

        public string CnArea
        {
            get;
            set;
        }

        public int? DisplaySequence
        {
            get;
            set;
        }

        public System.DateTime? StartDate
		{
			get;
			set;
		}
		public System.DateTime? EndDate
		{
			get;
			set;
		}
        public int? ImportSourceId
		{
			get;
			set;
		}

        public DateTime? AddTime
        {
            get;
            set;
        }

        /// <summary>
        /// 海关代码
        /// </summary>
        public string HSCode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否最惠国
        /// </summary>
        public bool FavourableFlag
        {
            get;
            set;
        }
	}
}

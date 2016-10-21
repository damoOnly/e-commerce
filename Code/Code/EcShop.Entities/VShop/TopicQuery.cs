using EcShop.Core.Entities;
using System;
namespace EcShop.Entities.VShop
{
	public class TopicQuery : Pagination
	{
		public bool? IsRelease
		{
			get;
			set;
		}
		public bool? IsincludeHomeProduct
		{
			get;
			set;
		}
		public int? Client
		{
			get;
			set;
		}

        public int? SupplierId
        {
            get;
            set;
        }
	}
}

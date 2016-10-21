using System;
namespace EcShop.Core.Entities
{
	public class DbQueryResult
	{
		public int TotalRecords
		{
			get;
			set;
		}
		public object Data
		{
			get;
			set;
		}
	}
}

using System;
namespace EcShop.Entities.Orders
{
	public class SendNoteInfo
	{
		public string NoteId
		{
			get;
			set;
		}
		public string OrderId
		{
			get;
			set;
		}
		public string Operator
		{
			get;
			set;
		}
		public string Remark
		{
			get;
			set;
		}
	}
}

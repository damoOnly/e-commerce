using System;
namespace EcShop.Entities.VShop
{
	[System.Serializable]
	public abstract class AbstractResponseMessage
	{
		public int MaterialId
		{
			get;
			set;
		}
		public string MsgType
		{
			get;
			set;
		}
	}
}

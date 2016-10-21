using System;
using System.Xml;
namespace EcShop.Core.Jobs
{
	public interface IJob
	{
		void Execute(XmlNode node);
	}
}

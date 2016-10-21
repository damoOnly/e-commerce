using System;
namespace EcShop.Core
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public sealed class HtmlCodingAttribute : Attribute
	{
	}
}

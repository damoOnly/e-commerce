using ASPNET.WebControls;
using EcShop.Membership.Context;
using System;
namespace EcShop.UI.Common.Controls
{
	public class ThemedTemplatedList : TemplatedList
	{
		public override string TemplateFile
		{
			get
			{
				return base.TemplateFile;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					if (value.StartsWith("/"))
					{
						base.TemplateFile = HiContext.Current.GetSkinPath() + value;
					}
					else
					{
						base.TemplateFile = HiContext.Current.GetSkinPath() + "/" + value;
					}
				}
				if (!base.TemplateFile.StartsWith("/templates"))
				{
					base.TemplateFile = base.TemplateFile.Substring(base.TemplateFile.IndexOf("/templates"));
				}
			}
		}
	}
}

using ASPNET.WebControls;
using EcShop.Membership.Context;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
	public class ThemedTemplatedRepeater : Repeater
	{
		private string skinName = string.Empty;
		public string TemplateFile
		{
			get
			{
				if (!string.IsNullOrEmpty(this.skinName) && !Utils.IsUrlAbsolute(this.skinName.ToLower()))
				{
					return Utils.ApplicationPath + this.skinName;
				}
				return this.skinName;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					if (value.StartsWith("/"))
					{
						this.skinName = HiContext.Current.GetSkinPath() + value;
					}
					else
					{
						this.skinName = HiContext.Current.GetSkinPath() + "/" + value;
					}
				}
				if (!this.skinName.StartsWith("/templates"))
				{
					this.skinName = this.skinName.Substring(this.skinName.IndexOf("/templates"));
				}
			}
		}
		protected override void CreateChildControls()
		{
			if (this.ItemTemplate == null && !string.IsNullOrEmpty(this.TemplateFile))
			{
				this.ItemTemplate = this.Page.LoadTemplate(this.TemplateFile);
			}
		}
	}
}

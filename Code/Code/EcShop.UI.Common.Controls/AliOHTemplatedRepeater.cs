using ASPNET.WebControls;
using EcShop.Membership.Context;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Common.Controls
{
	public class AliOHTemplatedRepeater : Repeater
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
				string aliOHshopSkinPath = HiContext.Current.GetAliOHshopSkinPath(null);
				if (!string.IsNullOrEmpty(value))
				{
					if (value.StartsWith("/"))
					{
						this.skinName = aliOHshopSkinPath + value;
					}
					else
					{
						this.skinName = aliOHshopSkinPath + "/" + value;
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

using EcShop.ControlPanel.Members;
using EcShop.Core;
using EcShop.Entities.Members;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class MemberPriceDropDownList : DropDownList
	{
		private bool allowNull = true;
		private string nullToDisplay = "";
		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
			}
		}
		public string NullToDisplay
		{
			get
			{
				return this.nullToDisplay;
			}
			set
			{
				this.nullToDisplay = value;
			}
		}
		public new int? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
				}
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			base.Items.Add(new ListItem("成本价", "-2"));
			base.Items.Add(new ListItem("一口价", "-3"));
			IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
			foreach (MemberGradeInfo current in memberGrades)
			{
				this.Items.Add(new ListItem(Globals.HtmlDecode(current.Name + "价"), current.GradeId.ToString()));
			}
		}
	}
}

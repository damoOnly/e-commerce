using EcShop.ControlPanel.Members;
using EcShop.Core;
using EcShop.Entities.Members;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.ControlPanel.Utility
{
	public class MemberGradeCheckBoxList : CheckBoxList
	{
		public new IList<int> SelectedValue
		{
			get
			{
				IList<int> list = new List<int>();
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Selected)
					{
						list.Add(int.Parse(this.Items[i].Value));
					}
				}
				return list;
			}
			set
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					this.Items[i].Selected = false;
				}
				foreach (int current in value)
				{
					for (int j = 0; j < this.Items.Count; j++)
					{
						if (this.Items[j].Value == current.ToString())
						{
							this.Items[j].Selected = true;
						}
					}
				}
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
			int num = 0;
			foreach (MemberGradeInfo current in memberGrades)
			{
				this.Items.Add(new ListItem(Globals.HtmlDecode(current.Name), current.GradeId.ToString()));
				this.Items[num++].Selected = true;
			}
		}
	}
}

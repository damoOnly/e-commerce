using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_Referral_MemberList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_Referral_MemberList";
		private System.Web.UI.WebControls.Repeater rptSubMembers;
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.rptSubMembers.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.rptSubMembers.DataSource = value;
			}
		}
		public Common_Referral_MemberList()
		{
			base.ID = "Common_Referral_MemberList";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_Referral_MemberList.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.rptSubMembers = (System.Web.UI.WebControls.Repeater)this.FindControl("rptSubMembers");
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.rptSubMembers.DataSource != null)
			{
				this.rptSubMembers.DataBind();
			}
		}
	}
}

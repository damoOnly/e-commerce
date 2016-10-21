using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_Advance_AccountList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_Advance_AccountList";
		private System.Web.UI.WebControls.Repeater repeaterAccountDetails;
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.repeaterAccountDetails.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.repeaterAccountDetails.DataSource = value;
			}
		}
		public Common_Advance_AccountList()
		{
			base.ID = "Common_Advance_AccountList";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_Advance_AccountList.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.repeaterAccountDetails = (System.Web.UI.WebControls.Repeater)this.FindControl("repeaterAccountDetails");
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.repeaterAccountDetails.DataSource != null)
			{
				this.repeaterAccountDetails.DataBind();
			}
		}
	}
}

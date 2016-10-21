using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_Messages_UserReceivedMessageList : AscxTemplatedWebControl
	{
		public delegate void CommandEventHandler(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e);
		public delegate void DataBindEventHandler(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e);
		public const string TagID = "Grid_Common_Messages_UserReceivedMessageList";
		private System.Web.UI.WebControls.Repeater repeaterMessageList;
		public event Common_Messages_UserReceivedMessageList.CommandEventHandler ItemCommand;
		public event Common_Messages_UserReceivedMessageList.DataBindEventHandler ItemDataBound;
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
				return this.repeaterMessageList.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.repeaterMessageList.DataSource = value;
			}
		}
		public Common_Messages_UserReceivedMessageList()
		{
			base.ID = "Grid_Common_Messages_UserReceivedMessageList";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Messages_UserReceivedMessageList.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.repeaterMessageList = (System.Web.UI.WebControls.Repeater)this.FindControl("repeaterMessageList");
			this.repeaterMessageList.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.repeaterMessageList_ItemCommand);
		}
		private void repeaterMessageList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			this.ItemCommand(source, e);
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.repeaterMessageList.DataSource != null)
			{
				this.repeaterMessageList.DataBind();
			}
		}
	}
}

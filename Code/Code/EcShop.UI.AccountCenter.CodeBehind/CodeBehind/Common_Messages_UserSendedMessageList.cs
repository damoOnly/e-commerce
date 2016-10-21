using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_Messages_UserSendedMessageList : AscxTemplatedWebControl
	{
		public delegate void CommandEventHandler(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e);
		public const string TagID = "Common_Messages_UserSendedMessageList";
		private System.Web.UI.WebControls.Repeater messagesList;
		public event Common_Messages_UserSendedMessageList.CommandEventHandler ItemCommand;
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
				return this.messagesList.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.messagesList.DataSource = value;
			}
		}
		public Common_Messages_UserSendedMessageList()
		{
			base.ID = "Common_Messages_UserSendedMessageList";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Messages_UserSendedMessageList.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.messagesList = (System.Web.UI.WebControls.Repeater)this.FindControl("messagesList");
			this.messagesList.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.messagesList_ItemCommand);
		}
		private void messagesList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			this.ItemCommand(source, e);
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.messagesList.DataSource != null)
			{
				this.messagesList.DataBind();
			}
		}
	}
}

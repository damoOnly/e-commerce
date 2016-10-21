using EcShop.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class Common_Address_AddressList : AscxTemplatedWebControl
	{
		public delegate void CommandEventHandler(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e);
		public const string TagID = "list_Common_Consignee_ConsigneeList";
		private System.Web.UI.WebControls.Repeater repeaterRegionsSelect;
		public event Common_Address_AddressList.CommandEventHandler ItemCommand;
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
		public System.Web.UI.WebControls.RepeaterItemCollection Items
		{
			get
			{
				return this.repeaterRegionsSelect.Items;
			}
		}
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.repeaterRegionsSelect.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.repeaterRegionsSelect.DataSource = value;
			}
		}
		public Common_Address_AddressList()
		{
			base.ID = "list_Common_Consignee_ConsigneeList";
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Address_AddressList.ascx";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.repeaterRegionsSelect = (System.Web.UI.WebControls.Repeater)this.FindControl("repeaterRegionsSelect");
			this.repeaterRegionsSelect.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.repeaterRegionsSelect_ItemCommand);
		}
		private void repeaterRegionsSelect_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			this.ItemCommand(source, e);
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			this.repeaterRegionsSelect.DataSource = this.DataSource;
			this.repeaterRegionsSelect.DataBind();
		}
	}
}

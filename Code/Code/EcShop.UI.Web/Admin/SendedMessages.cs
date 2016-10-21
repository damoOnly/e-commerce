using ASPNET.WebControls;
using EcShop.ControlPanel.Comments;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Entities.Comments;
using EcShop.Entities.Store;
using EcShop.Membership.Core.Enums;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SendedMessages)]
	public class SendedMessages : AdminPage
	{
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton btnDeleteSelect;
		protected Grid messagesList;
		protected ImageLinkButton btnDeleteSelect1;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnDeleteSelect.Click += new System.EventHandler(this.btnDeleteSelect_Click);
			this.btnDeleteSelect1.Click += new System.EventHandler(this.btnDeleteSelect_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void btnDeleteSelect_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<long> list = new System.Collections.Generic.List<long>();
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.messagesList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox != null && checkBox.Checked)
				{
					long item = (long)this.messagesList.DataKeys[gridViewRow.RowIndex].Value;
					list.Add(item);
				}
			}
			if (list.Count > 0)
			{
				NoticeHelper.DeleteManagerMessages(list);
				this.ShowMsg("成功删除了选择的消息.", true);
			}
			else
			{
				this.ShowMsg("请选择需要删除的消息.", false);
			}
			this.BindData();
		}
		private void BindData()
		{
			DbQueryResult managerSendedMessages = NoticeHelper.GetManagerSendedMessages(new MessageBoxQuery
			{
				Sernder = "admin",
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize
			}, UserRole.Member);
			this.messagesList.DataSource = managerSendedMessages.Data;
			this.messagesList.DataBind();
			this.pager.TotalRecords = managerSendedMessages.TotalRecords;
			this.pager1.TotalRecords = managerSendedMessages.TotalRecords;
		}
	}
}

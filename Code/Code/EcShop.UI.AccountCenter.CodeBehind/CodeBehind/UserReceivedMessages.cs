using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UserReceivedMessages : MemberTemplatedWebControl
	{
		private Common_Messages_UserReceivedMessageList CmessagesList;
		private System.Web.UI.WebControls.Repeater repeaterMessageList;
		private Pager pager;
		private IButton btnDeleteSelect;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserReceivedMessages.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.CmessagesList = (Common_Messages_UserReceivedMessageList)this.FindControl("Grid_Common_Messages_UserReceivedMessageList");
			this.pager = (Pager)this.FindControl("pager");
			this.btnDeleteSelect = ButtonManager.Create(this.FindControl("btnDeleteSelect"));
			this.btnDeleteSelect.Click += new System.EventHandler(this.btnDeleteSelect_Click);
			this.CmessagesList.ItemCommand += new Common_Messages_UserReceivedMessageList.CommandEventHandler(this.CmessagesList_ItemCommand);
			if (!this.Page.IsPostBack)
			{
				PageTitle.AddSiteNameTitle("收件箱");
				this.BindData();
			}
		}
		private void CmessagesList_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				CommentBrowser.DeleteMemberMessages(new System.Collections.Generic.List<long>
				{
					System.Convert.ToInt64(e.CommandArgument)
				});
				this.BindData();
			}
		}
		private void btnDeleteSelect_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<long> list = new System.Collections.Generic.List<long>();
			string text = this.Page.Request["CheckBoxGroup"];
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					string value = array[i];
					list.Add(System.Convert.ToInt64(value));
				}
				if (list.Count > 0)
				{
					CommentBrowser.DeleteMemberMessages(list);
				}
			}
			else
			{
				this.ShowMessage("请选中要删除的收件", false);
			}
			this.BindData();
		}
		private void BindData()
		{
			MessageBoxQuery messageBoxQuery = new MessageBoxQuery();
			messageBoxQuery.PageIndex = this.pager.PageIndex;
			messageBoxQuery.PageSize = this.pager.PageSize;
			messageBoxQuery.Accepter = HiContext.Current.User.Username;
			DbQueryResult memberReceivedMessages = CommentBrowser.GetMemberReceivedMessages(messageBoxQuery);
			if (((DataTable)memberReceivedMessages.Data).Rows.Count <= 0)
			{
				memberReceivedMessages = CommentBrowser.GetMemberReceivedMessages(messageBoxQuery);
			}
			this.CmessagesList.DataSource = memberReceivedMessages.Data;
			this.CmessagesList.DataBind();
			this.pager.TotalRecords = memberReceivedMessages.TotalRecords;
		}
	}
}

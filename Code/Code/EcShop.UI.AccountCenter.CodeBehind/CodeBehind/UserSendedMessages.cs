using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Entities.Comments;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Comments;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	public class UserSendedMessages : MemberTemplatedWebControl
	{
		private Common_Messages_UserSendedMessageList CmessagesList;
		private Pager pager;
		private IButton btnDeleteSelect;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserSendedMessages.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.CmessagesList = (Common_Messages_UserSendedMessageList)this.FindControl("Common_Messages_UserSendedMessageList");
			this.pager = (Pager)this.FindControl("pager");
			this.btnDeleteSelect = ButtonManager.Create(this.FindControl("btnDeleteSelect"));
			this.CmessagesList.ItemCommand += new Common_Messages_UserSendedMessageList.CommandEventHandler(this.CmessagesList_ItemCommand);
			this.btnDeleteSelect.Click += new System.EventHandler(this.btnDeleteSelect_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
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
					this.BindData();
					return;
				}
			}
			else
			{
				this.ShowMessage("请选中要删除的信息", false);
			}
		}
		private void CmessagesList_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			System.Collections.Generic.IList<long> list = new System.Collections.Generic.List<long>();
			if (e.CommandName == "Delete")
			{
				list.Add(System.Convert.ToInt64(e.CommandArgument));
				CommentBrowser.DeleteMemberMessages(list);
				this.BindData();
			}
		}
		private void BindData()
		{
			DbQueryResult memberSendedMessages = CommentBrowser.GetMemberSendedMessages(new MessageBoxQuery
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				Sernder = HiContext.Current.User.Username
			});
			this.CmessagesList.DataSource = memberSendedMessages.Data;
			this.CmessagesList.DataBind();
			this.pager.TotalRecords = memberSendedMessages.TotalRecords;
		}
	}
}

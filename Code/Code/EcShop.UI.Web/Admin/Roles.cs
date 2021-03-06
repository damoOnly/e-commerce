using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Membership.Core;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Components.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class Roles : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlInputHidden txtRoleId;
		protected System.Web.UI.HtmlControls.HtmlInputHidden txtRoleName;
		protected Grid grdGroupList;
		protected System.Web.UI.WebControls.TextBox txtAddRoleName;
		protected System.Web.UI.WebControls.TextBox txtRoleDesc;
		protected System.Web.UI.WebControls.TextBox txtEditRoleName;
		protected System.Web.UI.WebControls.TextBox txtEditRoleDesc;
		protected System.Web.UI.WebControls.Button btnEditRoles;
		protected System.Web.UI.WebControls.Button btnSubmitRoles;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSubmitRoles.Click += new System.EventHandler(this.btnSubmitRoles_Click);
			this.grdGroupList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdGroupList_RowDeleting);
			this.btnEditRoles.Click += new System.EventHandler(this.btnEditRoles_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindUserGroup();
			}
		}
		public void BindUserGroup()
		{
			System.Collections.ArrayList roles = RoleHelper.GetRoles();
			System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
			foreach (RoleInfo roleInfo in roles)
			{
				if (!RoleHelper.IsBuiltInRole(roleInfo.Name))
				{
                    if (roleInfo.Name == "SystemAdministrator")
                    {
                        roleInfo.Name = "超级管理员";
                    }
					arrayList.Add(roleInfo);
				}
			}
			this.grdGroupList.DataSource = arrayList;
			this.grdGroupList.DataBind();
		}
		private void btnEditRoles_Click(object sender, System.EventArgs e)
		{
			RoleInfo roleInfo = new RoleInfo();
			if (string.IsNullOrEmpty(this.txtEditRoleName.Text.Trim()))
			{
				this.ShowMsg("部门名称不能为空，长度限制在60个字符以内", false);
				return;
			}
			if (string.Compare(this.txtRoleName.Value, this.txtEditRoleName.Text) != 0 && RoleHelper.RoleExists(this.txtEditRoleName.Text.Trim().Replace(",", "")))
			{
				this.ShowMsg("已经存在相同的部门名称", false);
				return;
			}
			roleInfo.RoleID = new System.Guid(this.txtRoleId.Value);
			roleInfo.Name = Globals.HtmlEncode(this.txtEditRoleName.Text.Trim()).Replace(",", "");
			roleInfo.Description = Globals.HtmlEncode(this.txtEditRoleDesc.Text.Trim());
			ValidationResults validationResults = Validation.Validate<RoleInfo>(roleInfo, new string[]
			{
				"ValRoleInfo"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
				return;
			}
			RoleHelper.UpdateRole(roleInfo);
			this.BindUserGroup();
		}
		private void grdGroupList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)this.grdGroupList.Rows[e.RowIndex].FindControl("lblRoleName");
			string text = label.Text;
			if (RoleHelper.IsBuiltInRole(text))
			{
				this.ShowMsg("系统默认创建的部门不能删除", false);
				return;
			}
			RoleInfo role = new RoleInfo();
			role = RoleHelper.GetRole((System.Guid)this.grdGroupList.DataKeys[e.RowIndex].Value);
			try
			{
				RoleHelper.DeleteRole(role);
				this.BindUserGroup();
				this.ShowMsg("成功删除了选择的部门", true);
			}
			catch
			{
				this.ShowMsg("删除失败，该部门下已有管理员", true);
			}
		}
		protected void btnSubmitRoles_Click(object sender, System.EventArgs e)
		{
			string text = Globals.HtmlEncode(this.txtAddRoleName.Text.Trim()).Replace(",", "");
			string description = Globals.HtmlEncode(this.txtRoleDesc.Text.Trim());
			if (string.IsNullOrEmpty(text) || text.Length > 60)
			{
				this.ShowMsg("部门名称不能为空，长度限制在60个字符以内", false);
				return;
			}
			if (RoleHelper.RoleExists(text))
			{
				this.ShowMsg("已经存在相同的部门名称", false);
				return;
			}
			RoleHelper.AddRole(text);
			RoleInfo role = RoleHelper.GetRole(text);
			role.Name = text;
			role.Description = description;
			ValidationResults validationResults = Validation.Validate<RoleInfo>(role, new string[]
			{
				"ValRoleInfo"
			});
			string text2 = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text2 += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text2, false);
				return;
			}
			RoleHelper.UpdateRole(role);
			this.BindUserGroup();
			this.ShowMsg("成功添加了一个部门", true);
		}
		private void Reset()
		{
			this.txtAddRoleName.Text = string.Empty;
			this.txtRoleDesc.Text = string.Empty;
		}
	}
}

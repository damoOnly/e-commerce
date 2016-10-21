using EcShop.ControlPanel.Store;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    //[AdministerCheck(true)]
	public class AddManager : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.TextBox txtPasswordagain;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected RoleDropDownList dropRole;
		protected System.Web.UI.WebControls.Button btnCreate;
        protected System.Web.UI.WebControls.TextBox txtName;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropRole.DataBind();
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SupplierId"]))
                {
                    ListItem item = this.dropRole.Items.FindByText("供货商");
                    if (item != null)
                    {
                        item.Selected = true;
                        this.dropRole.Enabled = false;
                    }
                }

                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreId"]))
                {
                    ListItem item = this.dropRole.Items.FindByText("门店");
                    if (item != null)
                    {
                        item.Selected = true;
                        this.dropRole.Enabled = false;
                    }
                }
			}
		}
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			CreateUserStatus createUserStatus = CreateUserStatus.UnknownFailure;
			SiteManager siteManager = new SiteManager();
			siteManager.IsApproved = true;
			siteManager.Username = this.txtUserName.Text.Trim();
			siteManager.Email = this.txtEmail.Text.Trim();
			siteManager.Password = this.txtPassword.Text.Trim();
            siteManager.Name = this.txtName.Text.Trim();
          
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SupplierId"]))
            {
                int supplierId = 0;
                if (int.TryParse(this.Page.Request.QueryString["SupplierId"], out supplierId))
                {
                    siteManager.SupplierId = supplierId;
                }
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreId"]))
            {
                int storeId = 0;
                if (int.TryParse(this.Page.Request.QueryString["StoreId"], out storeId))
                {
                    siteManager.StoreId = storeId;
                }
            }

			siteManager.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			
            if (string.Compare(this.txtPassword.Text, this.txtPasswordagain.Text) != 0)
			{
				this.ShowMsg("请确保两次输入的密码相同", false);
				return;
			}

			if (!this.ValidationAddManager(siteManager))
			{
				return;
			}
			try
			{
				string text = this.dropRole.SelectedItem.Text;
				if (string.Compare(text, "超级管理员") == 0)
				{
					text = "SystemAdministrator";
				}
				createUserStatus = ManagerHelper.Create(siteManager, text);
			}
			catch (CreateUserException ex)
			{
				createUserStatus = ex.CreateUserStatus;
			}
			switch (createUserStatus)
			{
			case CreateUserStatus.UnknownFailure:
				this.ShowMsg("未知错误", false);
				return;
			case CreateUserStatus.Created:
				this.txtEmail.Text = string.Empty;
				this.txtUserName.Text = string.Empty;
                this.txtName.Text = string.Empty;
                this.ShowMsg("成功添加了一个管理员", true);
				break;
			case CreateUserStatus.DuplicateUsername:
				this.ShowMsg("您输入的用户名已经被注册使用", false);
				return;
			case CreateUserStatus.DuplicateEmailAddress:
				this.ShowMsg("您输入的电子邮件地址已经被注册使用", false);
				return;
			case CreateUserStatus.InvalidFirstCharacter:
			case CreateUserStatus.Updated:
			case CreateUserStatus.Deleted:
			case CreateUserStatus.InvalidQuestionAnswer:
				break;
			case CreateUserStatus.DisallowedUsername:
				this.ShowMsg("用户名被禁止注册", false);
				return;
			case CreateUserStatus.InvalidPassword:
				this.ShowMsg("无效的密码", false);
				return;
			case CreateUserStatus.InvalidEmail:
				this.ShowMsg("无效的电子邮件地址", false);
				return;
			default:
				return;
			}
		}
		private bool ValidationAddManager(SiteManager siteManager)
		{
			bool flag = true;
			ValidationResults validationResults = Validation.Validate<SiteManager>(siteManager, new string[]
			{
				"ValManagerName"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				flag = false;
			}
			validationResults = Validation.Validate<SiteManager>(siteManager, new string[]
			{
				"ValManagerPassword"
			});
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current2 in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current2.Message);
				}
				flag = false;
			}
			validationResults = Validation.Validate<SiteManager>(siteManager, new string[]
			{
				"ValManagerEmail"
			});
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current3 in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current3.Message);
				}
				flag = false;
			}
			if (!flag)
			{
				this.ShowMsg(text, false);
			}
			return flag;
		}
	}
}

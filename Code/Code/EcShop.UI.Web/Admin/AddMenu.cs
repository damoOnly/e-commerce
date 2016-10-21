using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AddMenu)]
	public class AddMenu : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtCategoryName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl liParent;
		protected System.Web.UI.WebControls.Literal lblParent;
		protected System.Web.UI.WebControls.DropDownList ddlType;
		protected System.Web.UI.HtmlControls.HtmlGenericControl liValue;
		protected System.Web.UI.HtmlControls.HtmlGenericControl liTitle;
		protected System.Web.UI.WebControls.DropDownList ddlValue;
		protected System.Web.UI.HtmlControls.HtmlGenericControl liUrl;
		protected System.Web.UI.WebControls.TextBox txtUrl;
		protected System.Web.UI.WebControls.Button btnAddMenu;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddMenu.Click += new System.EventHandler(this.btnAddMenu_Click);
			if (!this.Page.IsPostBack)
			{
				this.liValue.Visible = false;
				this.liUrl.Visible = false;
				if (base.GetUrlIntParam("pid") == 0)
				{
					this.liParent.Visible = false;
					return;
				}
				this.lblParent.Text = VShopHelper.GetMenu(base.GetUrlIntParam("pid")).Name;
			}
		}
		private void btnAddMenu_Click(object sender, System.EventArgs e)
		{
			MenuInfo menuInfo = new MenuInfo();
			menuInfo.Name = this.txtCategoryName.Text;
			menuInfo.ParentMenuId = base.GetUrlIntParam("pid");
			if (!VShopHelper.CanAddMenu(menuInfo.ParentMenuId, ClientType.VShop))
			{
				this.ShowMsg("一级菜单不能超过三个，对应二级菜单不能超过五个", false);
				return;
			}
			menuInfo.Bind = System.Convert.ToInt32(this.ddlType.SelectedValue);
			BindType bindType = menuInfo.BindType;
			switch (bindType)
			{
			case BindType.Key:
				menuInfo.ReplyId = System.Convert.ToInt32(this.ddlValue.SelectedValue);
				break;
			case BindType.Topic:
				menuInfo.Content = this.ddlValue.SelectedValue;
				break;
			default:
				if (bindType == BindType.Url)
				{
					menuInfo.Content = this.txtUrl.Text.Trim();
				}
				break;
			}
			menuInfo.Client = ClientType.VShop;
			menuInfo.Type = "click";
			if (menuInfo.ParentMenuId == 0)
			{
				menuInfo.Type = "view";
			}
			else
			{
				if (string.IsNullOrEmpty(this.ddlType.SelectedValue) || this.ddlType.SelectedValue == "0")
				{
					this.ShowMsg("二级菜单必须绑定一个对象", false);
					return;
				}
			}
			if (VShopHelper.SaveMenu(menuInfo))
			{
				base.Response.Redirect("ManageMenu.aspx");
				return;
			}
			this.ShowMsg("添加失败", false);
		}
		protected void ddlType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BindType bindType = (BindType)System.Convert.ToInt32(this.ddlType.SelectedValue);
			BindType bindType2 = bindType;
			switch (bindType2)
			{
			case BindType.Key:
			case BindType.Topic:
				this.liUrl.Visible = false;
				this.liValue.Visible = true;
				break;
			default:
				if (bindType2 == BindType.Url)
				{
					this.liUrl.Visible = true;
					this.liValue.Visible = false;
				}
				else
				{
					this.liUrl.Visible = false;
					this.liValue.Visible = false;
				}
				break;
			}
			switch (bindType)
			{
			case BindType.Key:
				this.liTitle.InnerText = "选择关键字：";
				this.ddlValue.DataSource = 
					from a in ReplyHelper.GetAllReply()
					where !string.IsNullOrWhiteSpace(a.Keys)
					select a;
				this.ddlValue.DataTextField = "Keys";
				this.ddlValue.DataValueField = "Id";
				this.ddlValue.DataBind();
				return;
			case BindType.Topic:
				this.liTitle.InnerText = "选择专题：";
				this.ddlValue.DataSource = VShopHelper.Gettopics();
				this.ddlValue.DataTextField = "Title";
				this.ddlValue.DataValueField = "TopicId";
				this.ddlValue.DataBind();
				return;
			default:
				return;
			}
		}
	}
}

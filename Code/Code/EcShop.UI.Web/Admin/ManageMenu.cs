using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using Ecdev.Weixin.MP.Api;
using Ecdev.Weixin.MP.Domain;
using Ecdev.Weixin.MP.Domain.Menu;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ManageMenu)]
	public class ManageMenu : AdminPage
	{
		private ClientType clientType;
		protected Grid grdMenu;
		protected System.Web.UI.WebControls.Button btnSubmit;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdMenu.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdMenu_RowCommand);
			this.grdMenu.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdMenu_RowDataBound);
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			this.clientType = ClientType.VShop;
			if (!string.IsNullOrWhiteSpace(base.Request["client"]))
			{
				this.clientType = (ClientType)int.Parse(base.Request["client"]);
			}
			if (!this.Page.IsPostBack)
			{
				this.BindData(this.clientType);
			}
		}
		private void grdMenu_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				int num = (int)System.Web.UI.DataBinder.Eval(e.Row.DataItem, "ParentMenuId");
				string text = System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Name").ToString();
				if (num == 0)
				{
					text = "<b>" + text + "</b>";
				}
				if (num > 0)
				{
					text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + text;
				}
				System.Web.UI.WebControls.Literal literal = e.Row.FindControl("lblCategoryName") as System.Web.UI.WebControls.Literal;
				literal.Text = text;
			}
		}
		private void BindData(ClientType clientType)
		{
			this.grdMenu.DataSource = VShopHelper.GetMenus(clientType);
			this.grdMenu.DataBind();
		}
		private void grdMenu_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int menuId = (int)this.grdMenu.DataKeys[rowIndex].Value;
			if (e.CommandName == "Fall")
			{
				VShopHelper.SwapMenuSequence(menuId, false);
			}
			else
			{
				if (e.CommandName == "Rise")
				{
					VShopHelper.SwapMenuSequence(menuId, true);
				}
				else
				{
					if (e.CommandName == "DeleteMenu")
					{
						if (VShopHelper.DeleteMenu(menuId))
						{
							this.ShowMsg("成功删除了指定的分类", true);
						}
						else
						{
							this.ShowMsg("分类删除失败，未知错误", false);
						}
					}
				}
			}
			this.BindData(this.clientType);
		}
		protected string RenderInfo(object menuIdObj)
		{
			ReplyInfo reply = ReplyHelper.GetReply((int)menuIdObj);
			if (reply != null)
			{
				return reply.Keys;
			}
			return string.Empty;
		}
		private SingleButton BuildMenu(MenuInfo menu)
		{
			switch (menu.BindType)
			{
			case BindType.Key:
				return new SingleClickButton
				{
					name = menu.Name,
					key = menu.MenuId.ToString()
				};
			case BindType.Topic:
			case BindType.HomePage:
			case BindType.ProductCategory:
			case BindType.ShoppingCar:
			case BindType.OrderCenter:
			case BindType.MemberCard:
			case BindType.Url:
				return new SingleViewButton
				{
					name = menu.Name,
					url = menu.Url
				};
			}
			return new SingleClickButton
			{
				name = menu.Name,
				key = "None"
			};
		}
		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<MenuInfo> initMenus = VShopHelper.GetInitMenus(ClientType.VShop);
			Ecdev.Weixin.MP.Domain.Menu.Menu menu = new Ecdev.Weixin.MP.Domain.Menu.Menu();
			foreach (MenuInfo current in initMenus)
			{
				if (current.Chilren == null || current.Chilren.Count == 0)
				{
					menu.menu.button.Add(this.BuildMenu(current));
				}
				else
				{
					SubMenu subMenu = new SubMenu
					{
						name = current.Name
					};
					foreach (MenuInfo current2 in current.Chilren)
					{
						subMenu.sub_button.Add(this.BuildMenu(current2));
					}
					menu.menu.button.Add(subMenu);
				}
			}
			string json = JsonConvert.SerializeObject(menu.menu);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (string.IsNullOrEmpty(masterSettings.WeixinAppId) || string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
			{
				base.Response.Write("<script>alert('您的服务号配置存在问题，请您先检查配置！');location.href='VServerConfig.aspx'</script>");
				return;
			}
			string text = TokenApi.GetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
			try
			{
				text = (JsonConvert.DeserializeObject(text, typeof(Token)) as Token).access_token;
				string text2 = MenuApi.CreateMenus(text, json);
				if (text2.Contains("ok"))
				{
					this.ShowMsg("成功的把自定义菜单保存到了微信", true);
				}
				else
				{
					this.ShowMsg("操作失败!服务号配置信息错误或没有微信自定义菜单权限，请检查配置信息以及菜单的长度。", false);
				}
			}
			catch (System.Exception ex)
			{
				base.Response.Write(string.Concat(new string[]
				{
					ex.Message,
					"---",
					text,
					"---",
					masterSettings.WeixinAppId,
					"---",
					masterSettings.WeixinAppSecret
				}));
				base.Response.End();
			}
		}
	}
}

using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.Membership.Context;
using EcShop.UI.ControlPanel.Utility;
using EcShop.UI.Web.App_Code;
using Ecdev.Alipay.OpenHome;
using Ecdev.Alipay.OpenHome.AlipayOHException;
using Ecdev.Alipay.OpenHome.Model;
using Ecdev.Alipay.OpenHome.Request;
using Ecdev.Alipay.OpenHome.Response;
using Ecdev.Weixin.MP.Domain.Menu;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.AliOH
{
	[PrivilegeCheck(Privilege.AliohManageMenu)]
	public class ManageMenu : AdminPage
	{
		protected Grid grdMenu;
		protected System.Web.UI.WebControls.Button btnSubmit;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdMenu.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdMenu_RowCommand);
			this.grdMenu.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdMenu_RowDataBound);
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindData(ClientType.AliOH);
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
			this.BindData(ClientType.AliOH);
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
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (string.IsNullOrEmpty(masterSettings.AliOHAppId))
			{
				base.Response.Write("<script>alert('您的服务号配置存在问题，请您先检查配置！');location.href='AliOHServerConfig.aspx'</script>");
				return;
			}
			System.Collections.Generic.IList<MenuInfo> initMenus = VShopHelper.GetInitMenus(ClientType.AliOH);
			System.Collections.Generic.List<Ecdev.Alipay.OpenHome.Model.Button> list = new System.Collections.Generic.List<Ecdev.Alipay.OpenHome.Model.Button>();
			foreach (MenuInfo current in initMenus)
			{
				if (current.Chilren == null || current.Chilren.Count == 0)
				{
					list.Add(new Ecdev.Alipay.OpenHome.Model.Button
					{
						name = current.Name,
						actionParam = string.IsNullOrEmpty(current.Url) ? "http://javasript:;" : current.Url,
						actionType = current.Type
					});
				}
				else
				{
					Ecdev.Alipay.OpenHome.Model.Button button = new Ecdev.Alipay.OpenHome.Model.Button
					{
						name = current.Name
					};
					System.Collections.Generic.List<Ecdev.Alipay.OpenHome.Model.Button> list2 = new System.Collections.Generic.List<Ecdev.Alipay.OpenHome.Model.Button>();
					foreach (MenuInfo current2 in current.Chilren)
					{
						list2.Add(new Ecdev.Alipay.OpenHome.Model.Button
						{
							name = current2.Name,
							actionParam = string.IsNullOrEmpty(current2.Url) ? "http://javasript:;" : current2.Url,
							actionType = current2.Type
						});
					}
					button.subButton = list2;
					list.Add(button);
				}
			}
			Ecdev.Alipay.OpenHome.Model.Menu menu = new Ecdev.Alipay.OpenHome.Model.Menu
			{
				button = list
			};
			AlipayOHClient alipayOHClient = AliOHClientHelper.Instance(base.Server.MapPath("~/"));
			bool flag = false;
			try
			{
				AddMenuRequest request = new AddMenuRequest(menu);
				alipayOHClient.Execute<MenuAddResponse>(request);
				this.ShowMsg("保存到服务窗成功！", true);
				flag = true;
			}
			catch (AliResponseException)
			{
			}
			catch (System.Exception ex)
			{
				this.ShowMsg("保存到服务窗失败，失败原因：" + ex.Message, false);
				flag = true;
			}
			if (!flag)
			{
				try
				{
					UpdateMenuRequest request2 = new UpdateMenuRequest(menu);
					alipayOHClient.Execute<MenuUpdateResponse>(request2);
					this.ShowMsg("保存到服务窗成功！", true);
				}
				catch (System.Exception ex2)
				{
					this.ShowMsg("保存到服务窗失败，失败原因：" + ex2.Message, false);
				}
			}
		}
	}
}

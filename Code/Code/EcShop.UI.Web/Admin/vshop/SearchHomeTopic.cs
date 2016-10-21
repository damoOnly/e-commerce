using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.Common.Controls;
using EcShop.UI.ControlPanel.Utility;
using System;
namespace EcShop.UI.Web.Admin.vshop
{
	[PrivilegeCheck(Privilege.TopicList)]
	public class SearchHomeTopic : AdminPage
	{
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected ImageLinkButton btnAdd;
		protected Grid grdTopics;
		protected Pager pager1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.BindTopics();
			}
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		protected void btnAdd_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请选择一个专题！", false);
				return;
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			int num = 0;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string value = array2[i];
				if (VShopHelper.AddHomeTopic(System.Convert.ToInt32(value), ClientType.VShop))
				{
					num++;
				}
			}
			if (num > 0)
			{
				this.CloseWindow();
				return;
			}
			this.ShowMsg("添加首页专题失败！", false);
		}
		protected void BindTopics()
		{
			DbQueryResult dbQueryResult = VShopHelper.GettopicList(new TopicQuery
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				IsincludeHomeProduct = new bool?(false),
				SortBy = "DisplaySequence",
				SortOrder = SortAction.Asc,
				Client = new int?(1)
			});
			this.grdTopics.DataSource = dbQueryResult.Data;
			this.grdTopics.DataBind();
			this.pager1.TotalRecords = (this.pager.TotalRecords = dbQueryResult.TotalRecords);
		}
	}
}

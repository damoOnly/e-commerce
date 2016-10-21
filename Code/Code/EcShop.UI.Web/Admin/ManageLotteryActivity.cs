using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ManageLotteryTicket)]
	public class ManageLotteryActivity : AdminPage
	{
		protected System.Web.UI.WebControls.Repeater rpMaterial;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindMaterial();
			}
		}
		protected void BindMaterial()
		{
			DbQueryResult lotteryTicketList = VShopHelper.GetLotteryTicketList(new LotteryActivityQuery
			{
				ActivityType = LotteryActivityType.Ticket,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "ActivityId",
				SortOrder = SortAction.Desc
			});
			this.rpMaterial.DataSource = lotteryTicketList.Data;
			this.rpMaterial.DataBind();
			this.pager.TotalRecords = lotteryTicketList.TotalRecords;
		}
		protected void rpMaterial_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (!(e.CommandName == "del"))
			{
				if (e.CommandName == "start")
				{
					int activityid = System.Convert.ToInt32(e.CommandArgument);
					LotteryTicketInfo lotteryTicket = VShopHelper.GetLotteryTicket(activityid);
					if (lotteryTicket.OpenTime > System.DateTime.Now)
					{
						lotteryTicket.OpenTime = System.DateTime.Now;
						VShopHelper.UpdateLotteryTicket(lotteryTicket);
					}
					this.ShowMsg("开启成功", true);
					this.BindMaterial();
				}
				return;
			}
			int activityId = System.Convert.ToInt32(e.CommandArgument);
			if (VShopHelper.DelteLotteryTicket(activityId))
			{
				this.ShowMsg("删除成功", true);
				this.BindMaterial();
				return;
			}
			this.ShowMsg("删除失败", false);
		}
		public string GetUrl(object activityId)
		{
			return string.Concat(new object[]
			{
				"http://",
				Globals.DomainName,
				"/Vshop/Ticket.aspx?id=",
				activityId
			});
		}
	}
}

using ASPNET.WebControls;
using EcShop.ControlPanel.Store;
using EcShop.Core;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.vshop
{
	[PrivilegeCheck(Privilege.ManageLotteryActivity)]
	public class ManageLotteryActivity : AdminPage
	{
		protected int type;
		protected System.Web.UI.WebControls.Literal LitTitle;
		protected System.Web.UI.WebControls.Literal Litdesc;
		protected System.Web.UI.HtmlControls.HtmlAnchor addactivity;
		protected System.Web.UI.WebControls.Repeater rpMaterial;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (int.TryParse(base.Request.QueryString["type"], out this.type))
			{
				this.addactivity.HRef = "VLotteryActivity.aspx?type=" + this.type;
				switch (this.type)
				{
				case 1:
					this.addactivity.InnerText = "添加大转盘";
					this.LitTitle.Text = "大转盘活动管理";
					this.Litdesc.Text = "大转盘";
					break;
				case 2:
					this.addactivity.InnerText = "添加刮刮卡";
					this.LitTitle.Text = "刮刮卡活动管理";
					this.Litdesc.Text = "刮刮卡";
					break;
				case 3:
					this.addactivity.InnerText = "添加砸金蛋";
					this.LitTitle.Text = "砸金蛋活动管理";
					this.Litdesc.Text = "砸金蛋";
					break;
				}
				if (!this.Page.IsPostBack)
				{
					this.BindMaterial();
					return;
				}
			}
			else
			{
				this.ShowMsg("参数错误", false);
			}
		}
		protected void BindMaterial()
		{
			DbQueryResult lotteryActivityList = VShopHelper.GetLotteryActivityList(new LotteryActivityQuery
			{
				ActivityType = (LotteryActivityType)this.type,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "ActivityId",
				SortOrder = SortAction.Desc
			});
			this.rpMaterial.DataSource = lotteryActivityList.Data;
			this.rpMaterial.DataBind();
			this.pager.TotalRecords = lotteryActivityList.TotalRecords;
		}
		protected void rpMaterial_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "del")
			{
				int activityid = System.Convert.ToInt32(e.CommandArgument);
				if (VShopHelper.DeleteLotteryActivity(activityid, ((LotteryActivityType)this.type).ToString()))
				{
					this.ShowMsg("删除成功", true);
					this.BindMaterial();
					return;
				}
				this.ShowMsg("删除失败", false);
			}
		}
		public string GetUrl(object activityId)
		{
			string result = string.Empty;
			switch (this.type)
			{
			case 1:
				result = string.Concat(new object[]
				{
					"http://",
					Globals.DomainName,
					"/Vshop/BigWheel.aspx?activityid=",
					activityId
				});
				break;
			case 2:
				result = string.Concat(new object[]
				{
					"http://",
					Globals.DomainName,
					"/Vshop/Scratch.aspx?activityid=",
					activityId
				});
				break;
			case 3:
				result = string.Concat(new object[]
				{
					"http://",
					Globals.DomainName,
					"/Vshop/SmashEgg.aspx?activityid=",
					activityId
				});
				break;
			}
			return result;
		}
	}
}

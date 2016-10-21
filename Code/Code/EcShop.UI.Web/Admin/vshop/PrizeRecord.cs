using EcShop.ControlPanel.Store;
using EcShop.Entities.Store;
using EcShop.Entities.VShop;
using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin.vshop
{
	[PrivilegeCheck(Privilege.ManageLotteryTicket)]
	public class PrizeRecord : AdminPage
	{
		protected int activeid;
		protected System.Web.UI.WebControls.Literal LitTitle;
		protected System.Web.UI.WebControls.Literal Litdesc;
		protected System.Web.UI.WebControls.Repeater rpMaterial;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (int.TryParse(base.Request.QueryString["id"], out this.activeid))
			{
				if (!this.Page.IsPostBack)
				{
					this.BindPrizeList();
					return;
				}
			}
			else
			{
				this.ShowMsg("参数错误", false);
			}
		}
		protected void BindPrizeList()
		{
			System.Collections.Generic.List<PrizeRecordInfo> prizeList = VShopHelper.GetPrizeList(new PrizeQuery
			{
				ActivityId = this.activeid
			});
			if (prizeList != null && prizeList.Count > 0)
			{
				this.LitTitle.Text = prizeList[0].ActivityName;
			}
			this.rpMaterial.DataSource = prizeList;
			this.rpMaterial.DataBind();
		}
	}
}

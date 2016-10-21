using EcShop.Entities.Members;
using EcShop.Entities.Orders;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VSplittinDetail_Detail : VMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litOrderNo;
		private System.Web.UI.WebControls.Literal litSplittinDate;
		private System.Web.UI.WebControls.Literal litStatus;
		private System.Web.UI.WebControls.Literal litMark;
		private SplittingTypeNameLabel litSplittinType;
		private FormatedMoneyLabel litOrderAmount;
		private FormatedMoneyLabel litSplittinAmount;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SplittinDetail_Detail.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litOrderAmount = (FormatedMoneyLabel)this.FindControl("litOrderAmount");
			this.litSplittinAmount = (FormatedMoneyLabel)this.FindControl("litSplittinAmount");
			this.litOrderNo = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderNo");
			this.litSplittinType = (SplittingTypeNameLabel)this.FindControl("litSplittinType");
			this.litSplittinDate = (System.Web.UI.WebControls.Literal)this.FindControl("litSplittinDate");
			this.litStatus = (System.Web.UI.WebControls.Literal)this.FindControl("litStatus");
			this.litMark = (System.Web.UI.WebControls.Literal)this.FindControl("litMark");
			PageTitle.AddSiteNameTitle("佣金明细");
			long journalNumber = 0L;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["id"]))
			{
				long.TryParse(this.Page.Request.QueryString["id"], out journalNumber);
			}
			SplittinDetailInfo splittinDetail = MemberProcessor.GetSplittinDetail(journalNumber);
			if (splittinDetail == null)
			{
				this.ShowMessage("错误的明细ID", false);
			}
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(splittinDetail.OrderId);
			if (!string.IsNullOrEmpty(splittinDetail.OrderId) && orderInfo != null)
			{
				this.litOrderAmount.Money = orderInfo.GetAmount();
			}
			else
			{
				this.litOrderAmount.Money = 0;
			}
			this.litSplittinAmount.Money = splittinDetail.Income;
			this.litOrderNo.Text = splittinDetail.OrderId;
			this.litMark.Text = splittinDetail.Remark;
			this.litSplittinType.SplittingType = ((int)splittinDetail.TradeType).ToString();
			this.litSplittinDate.Text = splittinDetail.TradeDate.ToString("yyyy-MM-dd hh:mm:ss");
			if (splittinDetail.TradeType == SplittingTypes.DrawRequest)
			{
				this.litSplittinAmount.Money = splittinDetail.Expenses;
			}
			this.litStatus.Text = (splittinDetail.IsUse ? "可用" : "不可用");
		}
	}
}

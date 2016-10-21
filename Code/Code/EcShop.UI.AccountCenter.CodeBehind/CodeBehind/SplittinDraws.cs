using ASPNET.WebControls;
using EcShop.Core.Entities;
using EcShop.Entities.Members;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using Ecdev.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class SplittinDraws : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal lblBanlance;
		private System.Web.UI.WebControls.TextBox txtAmount;
		private System.Web.UI.WebControls.TextBox txtAccount;
		private System.Web.UI.WebControls.TextBox txtTradePassword;
		private IButton btnDraw;
		private Common_Referral_SplittinDraw rptSplittinDraw;
		private Pager pager;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-SplittinDraws.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.lblBanlance = (System.Web.UI.WebControls.Literal)this.FindControl("lblBanlance");
			this.txtAmount = (System.Web.UI.WebControls.TextBox)this.FindControl("txtAmount");
			this.txtAccount = (System.Web.UI.WebControls.TextBox)this.FindControl("txtAccount");
			this.txtTradePassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTradePassword");
			this.btnDraw = ButtonManager.Create(this.FindControl("btnDraw"));
			this.rptSplittinDraw = (Common_Referral_SplittinDraw)this.FindControl("Common_Referral_Splitin");
			this.pager = (Pager)this.FindControl("pager");
			this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
			PageTitle.AddSiteNameTitle("申请提现");
			if (!this.Page.IsPostBack)
			{
				this.lblBanlance.Text = MemberProcessor.GetUserUseSplittin(HiContext.Current.User.UserId).ToString("F2");
				this.BindSplittinDraw();
			}
		}
		private void btnDraw_Click(object sender, System.EventArgs e)
		{
			DbQueryResult mySplittinDraws = MemberProcessor.GetMySplittinDraws(new BalanceDrawRequestQuery
			{
				PageIndex = 1,
				PageSize = this.pager.PageSize,
				UserId = new int?(HiContext.Current.User.UserId)
			}, new int?(1));
			if (mySplittinDraws.TotalRecords > 0)
			{
				this.ShowMessage("上笔提现管理员还没有处理，只有处理完后才能再次申请提现", false);
				return;
			}
			decimal num = 0m;
			if (!decimal.TryParse(this.txtAmount.Text.Trim(), out num))
			{
				this.ShowMessage("提现金额输入错误,请重新输入提现金额", false);
				return;
			}
			if (num > decimal.Parse(this.lblBanlance.Text))
			{
				this.ShowMessage("可提现佣金不足,请重新输入提现金额", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtTradePassword.Text))
			{
				this.ShowMessage("请输入交易密码", false);
				return;
			}
			Member member = HiContext.Current.User as Member;
			member.TradePassword = this.txtTradePassword.Text;
			if (!Users.ValidTradePassword(member))
			{
				this.ShowMessage("交易密码不正确,请重新输入", false);
				return;
			}
			if (MemberProcessor.SplittinDrawRequest(new SplittinDrawInfo
			{
				UserId = member.UserId,
				UserName = member.Username,
				Amount = num,
				Account = this.txtAccount.Text,
				RequestDate = System.DateTime.Now,
				AuditStatus = 1
			}))
			{
				this.ShowMessage("提现申请成功，等待管理员的审核", true);
				this.BindSplittinDraw();
				return;
			}
			this.ShowMessage("提现申请失败，请重试", false);
		}
		private void BindSplittinDraw()
		{
			DbQueryResult mySplittinDraws = MemberProcessor.GetMySplittinDraws(new BalanceDrawRequestQuery
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				UserId = new int?(HiContext.Current.User.UserId)
			}, null);
			this.rptSplittinDraw.DataSource = mySplittinDraws.Data;
			this.rptSplittinDraw.DataBind();
			this.pager.TotalRecords = mySplittinDraws.TotalRecords;
		}
		private bool ValidateBalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
		{
			ValidationResults validationResults = Validation.Validate<BalanceDrawRequestInfo>(balanceDrawRequest, new string[]
			{
				"ValBalanceDrawRequestInfo"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMessage(text, false);
			}
			return validationResults.IsValid;
		}
	}
}

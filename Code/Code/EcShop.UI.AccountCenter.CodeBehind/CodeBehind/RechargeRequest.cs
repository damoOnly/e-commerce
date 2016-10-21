using EcShop.Core;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.AccountCenter.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class RechargeRequest : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litUserName;
		private System.Web.UI.WebControls.RadioButtonList rbtnPaymentMode;
		private FormatedMoneyLabel litUseableBalance;
		private System.Web.UI.WebControls.TextBox txtReChargeBalance;
		private IButton btnReCharge;
		private FormatedMoneyLabel litAccountAmount;
		private FormatedMoneyLabel litRequestBalance;
		private FormatedMoneyLabel litUseableBalance1;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-RechargeRequest.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litUserName = (System.Web.UI.WebControls.Literal)this.FindControl("litUserName");
			this.rbtnPaymentMode = (System.Web.UI.WebControls.RadioButtonList)this.FindControl("rbtnPaymentMode");
			this.txtReChargeBalance = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReChargeBalance");
			this.btnReCharge = ButtonManager.Create(this.FindControl("btnReCharge"));
			this.litUseableBalance = (FormatedMoneyLabel)this.FindControl("litUseableBalance");
			this.litAccountAmount = (FormatedMoneyLabel)this.FindControl("litAccountAmount");
			this.litRequestBalance = (FormatedMoneyLabel)this.FindControl("litRequestBalance");
			this.litUseableBalance1 = (FormatedMoneyLabel)this.FindControl("litUseableBalance1");
			PageTitle.AddSiteNameTitle("预付款充值");
			this.btnReCharge.Click += new System.EventHandler(this.btnReCharge_Click);
			if (!this.Page.IsPostBack)
			{
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (!member.IsOpenBalance)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/OpenBalance.aspx?ReturnUrl={0}", System.Web.HttpContext.Current.Request.Url));
				}
				this.BindPaymentMode();
				this.litUserName.Text = HiContext.Current.User.Username;
				this.litUseableBalance.Money = member.Balance - member.RequestBalance;
				this.litAccountAmount.Money = member.Balance;
				this.litRequestBalance.Money = member.RequestBalance;
				this.litUseableBalance1.Money = member.Balance - member.RequestBalance;
			}
		}
		protected void btnReCharge_Click(object sender, System.EventArgs e)
		{
			if (this.rbtnPaymentMode.Items.Count == 0)
			{
				this.ShowMessage("无法充值,因为后台没有添加支付方式", false);
				return;
			}
			if (this.rbtnPaymentMode.SelectedValue == null)
			{
				this.ShowMessage("选择要充值使用的支付方式", false);
				return;
			}
			int num = 0;
			if (this.txtReChargeBalance.Text.Trim().IndexOf(".") > 0)
			{
				num = this.txtReChargeBalance.Text.Trim().Substring(this.txtReChargeBalance.Text.Trim().IndexOf(".") + 1).Length;
			}
			decimal num2;
			if (!decimal.TryParse(this.txtReChargeBalance.Text, out num2) || decimal.Parse(this.txtReChargeBalance.Text) <= 0m || num > 2)
			{
				this.ShowMessage("请输入大于0的充值金额且金额整数位数在1到10之间,且不能超过2位小数", false);
				return;
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("user_RechargeConfirm", new object[]
			{
				this.Page.Server.UrlEncode(this.rbtnPaymentMode.SelectedValue),
				this.Page.Server.UrlEncode(this.txtReChargeBalance.Text)
			}));
		}
		private void BindPaymentMode()
		{
			System.Collections.Generic.IList<PaymentModeInfo> paymentModes = TradeHelper.GetPaymentModes(PayApplicationType.payOnAll);
			if (paymentModes.Count > 0)
			{
				foreach (PaymentModeInfo current in paymentModes)
				{
					string text = current.Gateway.ToLower();
					if (current.IsUseInpour && !text.Equals("ecdev.plugins.payment.advancerequest") && !text.Equals("ecdev.plugins.payment.bankrequest"))
					{
						if (text.Equals("ecdev.plugins.payment.alipay_shortcut.shortcutrequest"))
						{
							System.Web.HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
							if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
							{
								this.rbtnPaymentMode.Items.Add(new System.Web.UI.WebControls.ListItem(current.Name, current.ModeId.ToString()));
							}
						}
						else
						{
							this.rbtnPaymentMode.Items.Add(new System.Web.UI.WebControls.ListItem(current.Name, current.ModeId.ToString()));
						}
					}
				}
				this.rbtnPaymentMode.SelectedIndex = 0;
			}
		}
	}
}

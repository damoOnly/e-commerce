using EcShop.Core;
using EcShop.Entities.Members;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using Ecdev.Plugins;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Entities;
using System.Xml;
namespace EcShop.UI.AccountCenter.CodeBehind
{
    [System.Web.UI.ParseChildren(true)]
    public class RechargeConfirm : MemberTemplatedWebControl
    {
        private int paymentModeId;
        private decimal balance;
        private System.Web.UI.WebControls.Literal litUserName;
        private FormatedMoneyLabel lblBlance;
        private System.Web.UI.WebControls.Literal litPayCharge;
        private HiImage imgPayment;
        private System.Web.UI.WebControls.Literal lblPaymentName;
        private IButton btnConfirm;
        protected override void OnInit(System.EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-RechargeConfirm.html";
            }
            base.OnInit(e);
        }
        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["modeId"], out this.paymentModeId);
            decimal.TryParse(this.Page.Request.QueryString["blance"], out this.balance);
            this.litUserName = (System.Web.UI.WebControls.Literal)this.FindControl("litUserName");
            this.lblPaymentName = (System.Web.UI.WebControls.Literal)this.FindControl("lblPaymentName");
            this.imgPayment = (HiImage)this.FindControl("imgPayment");
            this.lblBlance = (FormatedMoneyLabel)this.FindControl("lblBlance");
            this.litPayCharge = (System.Web.UI.WebControls.Literal)this.FindControl("litPayCharge");
            this.btnConfirm = ButtonManager.Create(this.FindControl("btnConfirm"));
            PageTitle.AddSiteNameTitle("充值确认");
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            if (!this.Page.IsPostBack)
            {
                if (this.paymentModeId == 0 || this.balance == 0m)
                {
                    this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("user_InpourRequest"));
                    return;
                }
                PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(this.paymentModeId);
                this.litUserName.Text = HiContext.Current.User.Username;
                if (paymentMode != null)
                {
                    this.lblPaymentName.Text = paymentMode.Name;
                    this.lblBlance.Money = this.balance;
                    this.ViewState["PayCharge"] = paymentMode.CalcPayCharge(this.balance);
                    this.litPayCharge.Text = Globals.FormatMoney(paymentMode.CalcPayCharge(this.balance));
                }
            }
        }
        private void btnConfirm_Click(object sender, System.EventArgs e)
        {
            PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(this.paymentModeId);
            InpourRequestInfo inpourRequestInfo = new InpourRequestInfo
            {
                InpourId = this.GenerateInpourId(),
                TradeDate = System.DateTime.Now,
                InpourBlance = this.balance,
                UserId = HiContext.Current.User.UserId,
                PaymentId = paymentMode.ModeId
            };
            if (MemberProcessor.AddInpourBlance(inpourRequestInfo))
            {
                string attach = "";
                System.Web.HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
                if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
                {
                    attach = httpCookie.Value;
                }
                string orderId = inpourRequestInfo.InpourId.ToString(System.Globalization.CultureInfo.InvariantCulture);
                string configXml = HiCryptographer.Decrypt(paymentMode.Settings);
                decimal amount = inpourRequestInfo.InpourBlance + (decimal)this.ViewState["PayCharge"];
                PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, configXml, orderId, amount, "预付款充值", "操作流水号-" + orderId, HiContext.Current.User.Email, inpourRequestInfo.TradeDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("InpourReturn_url", new object[]
				{
					paymentMode.Gateway
				})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("InpourNotify_url", new object[]
				{
					paymentMode.Gateway
				})), attach);
            }
        }
        private string GenerateInpourId()
        {
            string text = string.Empty;
            System.Random random = new System.Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                text += ((char)(48 + (ushort)(num % 10))).ToString();
            }
            return System.DateTime.Now.ToString("yyyyMMdd") + text;
        }
    }
}

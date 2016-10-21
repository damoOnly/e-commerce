using EcShop.Core;
using EcShop.Entities.Members;
using EcShop.Entities.Sales;
using EcShop.Membership.Context;
using EcShop.SaleSystem.Member;
using EcShop.UI.Common.Controls;
using Ecdev.Plugins;
using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Entities;
using System.Xml;
using EcShop.Membership.Core;
using EcShop.ControlPanel.Members;

namespace EcShop.UI.AccountCenter.CodeBehind
{
    public class ThirdPartRecharge : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BalanceRecharge();
            }
        }

        private void BalanceRecharge()
        {
            string userid = Request["userid"];
            string userdata = Request["userdata"];
            string timestamp = Request["timestamp"];
            string source = Request["source"];
            string amount = Request["amount"];
            string tradeno = Request["tradeno"];

            if (string.IsNullOrWhiteSpace(userid)
                || string.IsNullOrWhiteSpace(userdata)
                || string.IsNullOrWhiteSpace(timestamp)
                || string.IsNullOrWhiteSpace(source)
                || string.IsNullOrWhiteSpace(amount)
                || string.IsNullOrWhiteSpace(tradeno))
            {
                Response.Write("fail，缺少必要参数");
                Response.End();
            }

            string key = ConfigurationManager.AppSettings["Key_CCB"];
            string iv = ConfigurationManager.AppSettings["IV_CCB"];

            string dataKey = Cryptographer.DESDecrypt(userdata, key, iv);
            string deUserId = Cryptographer.DESDecrypt(userid, dataKey, iv);
            string deTimestamp = Cryptographer.DESDecrypt(timestamp, dataKey, iv);
            string deAmount = Cryptographer.DESDecrypt(amount, dataKey, iv);
            string deTradeno = Cryptographer.DESDecrypt(tradeno, dataKey, iv);

            if (string.IsNullOrWhiteSpace(dataKey)
                || string.IsNullOrWhiteSpace(deUserId)
                || string.IsNullOrWhiteSpace(deTimestamp)
                || string.IsNullOrWhiteSpace(deAmount)
                || string.IsNullOrWhiteSpace(deTradeno))
            {
                Response.Write("fail，请求参数无效");
                Response.End();
            }

            DateTime time = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).Add(new TimeSpan(long.Parse(deTimestamp + "0000000")));
            TimeSpan span = DateTime.Now - time;
            if (span.TotalSeconds > 60)
            {
                Response.Write("fail，请求已过期");
                Response.End();
            }

            decimal money = 0;
            if (!decimal.TryParse(deAmount, out money))
            {
                Response.Write("fail，充值金额只能是数值");
                Response.End();
            }

            money = money / 100;
            if (money <= 0 || money > 10000000m)
            {
                Response.Write("fail，充值金额必须大于0且小于等于1000万元");
                Response.End();
            }

            Member member = Users.GetUserByCcbOpenId(deUserId) as Member;
            if (member == null || !member.IsOpenBalance)
            {
                // fail，返回原因
                Response.Write("fail，该用户不存在或还没有开通预付款账户");
                Response.End();
            }

            deTradeno = "CCB" + deTradeno;//加上ccb前缀以区分是建行的充值。
            // 检查交易号是否重复
            if (MemberHelper.IsTradeNoExists(deTradeno))
            {
                Response.Write("fail，交易号已失效");
                Response.End();
            }

            BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
            balanceDetailInfo.UserId = member.UserId;
            balanceDetailInfo.UserName = member.Username;
            balanceDetailInfo.TradeDate = DateTime.Now;
            balanceDetailInfo.TradeType = TradeTypes.CcbRecharge;
            balanceDetailInfo.Income = money;
            balanceDetailInfo.Balance = money + member.Balance;
            balanceDetailInfo.Remark = "建行预付款充值";
            balanceDetailInfo.InpourId = deTradeno;

            // success: 充值成功，将金额充值到用户预存款帐户上
            if (MemberHelper.AddBalance(balanceDetailInfo, money))
            {
                //Response.Redirect("/User/UserDefault.aspx", true);
                Response.Write("success，充值成功");
                Response.End();
            }

            // fail，返回原因
            Response.Write("fail，充值失败，请稍后重试");
            Response.End();
        }
    }
}

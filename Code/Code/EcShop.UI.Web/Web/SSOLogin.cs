using System;
using System.Configuration;
using System.Web.UI;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.Membership.Core;
using EcShop.Membership.Core.Enums;
using EcShop.Messages;
using EcShop.SaleSystem.Member;
using EcShop.SaleSystem.Shopping;
using EcShop.Core.ErrorLog;
using EcShop.ControlPanel.Sales;
using EcShop.Entities.Sales;
using EcShop.Core;

namespace EcShop.UI.Web
{
    public class SSOLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Login();
            }
        }

        private void Login()
        {
            string userid = Request["userid"];
            string userdata = Request["userdata"];
            string timestamp = Request["timestamp"];
            string source = Request["source"];

            if (string.IsNullOrWhiteSpace(userid)
                || string.IsNullOrWhiteSpace(userdata)
                || string.IsNullOrWhiteSpace(timestamp)
                || string.IsNullOrWhiteSpace(source))
            {
                Response.Write("fail，缺少必要参数");
                Response.End();
            }

            string key = ConfigurationManager.AppSettings["Key_CCB"];
            string iv = ConfigurationManager.AppSettings["IV_CCB"];

            string dataKey = Cryptographer.DESDecrypt(userdata, key, iv);
            string deUserId = Cryptographer.DESDecrypt(userid, dataKey, iv);
            string deTimestamp = Cryptographer.DESDecrypt(timestamp, dataKey, iv);

            if (string.IsNullOrWhiteSpace(dataKey)
                || string.IsNullOrWhiteSpace(deUserId)
                || string.IsNullOrWhiteSpace(deTimestamp))
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

            Member member = Users.GetUserByCcbOpenId(deUserId) as Member;
            if (member == null)
            {
                // fail，返回原因
                Response.Write("fail，用户不存在");
                Response.End();
            }

            System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
            IUserCookie userCookie = member.GetUserCookie();
            userCookie.WriteCookie(authCookie, 30, false);
            ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
            HiContext.Current.User = member;
            if (cookieShoppingCart != null)
            {
                ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
                ShoppingCartProcessor.ClearCookieShoppingCart();
            }

            // 登录成功跳转到海美生活用户中心
            Response.Redirect("/User/UserDefault.aspx");
        }
    }
}

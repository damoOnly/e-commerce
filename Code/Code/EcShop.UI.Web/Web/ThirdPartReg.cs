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
using System.Security.Cryptography;
using EcShop.Entities;

namespace EcShop.UI.Web
{
    public class ThirdPartReg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                RegisterCcbUser();
            }
        }

        private void RegisterCcbUser()
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

            string mobile = Request["mobile"];
            string email = Request["email"];

            string key = ConfigurationManager.AppSettings["Key_CCB"];
            string iv = ConfigurationManager.AppSettings["IV_CCB"];

            string dataKey = Cryptographer.DESDecrypt(userdata, key, iv);
            string deUserId = Cryptographer.DESDecrypt(userid, dataKey, iv);
            string deTimestamp = Cryptographer.DESDecrypt(timestamp, dataKey, iv);
            string deMobile = Cryptographer.DESDecrypt(mobile, dataKey, iv);
            string deEmail = Cryptographer.DESDecrypt(email, dataKey, iv);

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

            // 注册用户，与微信类似，将deUserId写到aspnet_Users的CCBOpenId字段上
            // success，将在海美生活注册一个新的用户，与userid关联
            IUser user;
            bool isExists = UserHelper.CreateCcbUsersMemberUsersInRoles(deUserId, deMobile, deEmail, UserHelper.GenerateSalt(), GetProvinceId(), out user);
            if (isExists)
            {
                Response.Write("fail，用户已存在");
                Response.End();
            }

            if (user != null)
            {
                Response.Write("success");
                Response.End();
            }
            // fail，返回原因
            Response.Write("fail，创建用户失败");
            Response.End();
        }

        private int GetProvinceId()
        {
            long ip = Globals.IpToInt(Globals.IPAddress);
            string provinceName = TradeHelper.GetProvinceName(ip);
            if (!string.IsNullOrEmpty(provinceName))
            {
                return RegionHelper.GetProvinceId(provinceName);
            }
            return 0;
        }
    }
}

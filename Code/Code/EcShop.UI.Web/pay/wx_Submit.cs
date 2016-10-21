using EcShop.ControlPanel.Sales;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using Ecdev.Weixin.Pay;
using Ecdev.Weixin.Pay.Domain;
using System;
using System.Web.UI;
using EcShop.Core;
using EcShop.SaleSystem.Member;
using EcShop.Core.ErrorLog;
namespace EcShop.UI.Web.Pay
{
	public class wx_Submit : System.Web.UI.Page
	{
		public string pay_json = string.Empty;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request.QueryString.Get("orderId");
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
            OrderHelper.SetOrderPayStatus(orderInfo.OrderId, 1);
            
			if (orderInfo == null)
			{
				return;
			}
			PackageInfo packageInfo = new PackageInfo();
			packageInfo.Body = orderInfo.OrderId;
			packageInfo.NotifyUrl = string.Format("http://{0}/pay/wx_Pay.aspx", base.Request.Url.Host);
			packageInfo.OutTradeNo = orderInfo.OrderId;

            packageInfo.TotalFee = (int)(orderInfo.GetTotal() * 100m);
            if (packageInfo.TotalFee < 1m)
            {
                packageInfo.TotalFee = 1m;
            }
			string openId = "";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (HiContext.Current.User != null)
            {
                openId = HiContext.Current.User.OpenId;
            }
            //openId = "oUBLTvixVJV6q0FtN7kpKW7f9Ur8";
            if (string.IsNullOrEmpty(openId))
            {
                string code = Request.QueryString["code"];
                if (string.IsNullOrEmpty(code))
                {
                    string text2 = string.Format(
                                "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=123#wechat_redirect",
                                masterSettings.WeixinAppId,
                                Globals.UrlEncode(Request.Url.ToString()));
                    this.Page.Response.Redirect(text2);
                }
                else
                {
                    Ecdev.Weixin.MP.Domain.Token token = Ecdev.Weixin.MP.Api.TokenApi.GetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, code, "authorization_code");
                    openId = token.openid;
                }
            }
            if (string.IsNullOrEmpty(openId))
            { 
            Response.Write("<script>alert('无法获取登录信息，建议退出并重新登录或注册新帐号。');location.href = '/Vshop/logout.aspx';</script>");
            Response.End();
            }
			packageInfo.OpenId = openId;
            
            PayClient payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);
            PayRequestInfo req  = null;

            int loop = 0;

            do
            {
                req = payClient.BuildPayRequest(packageInfo);

                if (!req.package.Equals("prepay_id=FAIL"))
                {
                    break;
                }

                loop++;
                
                System.Threading.Thread.Sleep(500);

            } while (loop < 5);

			this.pay_json = this.ConvertPayJson(req);
            ErrorLog.Write("开始请求支付：" + this.pay_json);
		}
		public string ConvertPayJson(PayRequestInfo req)
		{
			string str = "{";
            
            if (req != null)
            {
                str = str + "\"appId\":\"" + req.appId + "\",";
                str = str + "\"timeStamp\":\"" + req.timeStamp + "\",";
                str = str + "\"nonceStr\":\"" + req.nonceStr + "\",";
                str = str + "\"package\":\"" + req.package + "\",";
                str = str + "\"signType\":\"" + req.signType + "\",";
                str = str + "\"paySign\":\"" + req.paySign + "\"";
            }

            return str + "}";
		}
	}
}

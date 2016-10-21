using EcShop.ControlPanel.Sales;
using EcShop.Core.ErrorLog;
using EcShop.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class OrderReturn : PaymentTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litMessage;
		public OrderReturn() : base(false)
		{
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-PaymentReturn.html";
			}
			base.OnInit(e);
		}
		protected override void AttachChildControls()
		{
			this.litMessage = (System.Web.UI.WebControls.Literal)this.FindControl("litMessage");
		}
        private int cid =0;
        private string wid = string.Empty;
		protected override void DisplayMessage(string status)
		{
			switch (status)
			{
			case "ordernotfound":
				this.litMessage.Text = string.Format("没有找到对应的订单信息，订单号：{0}", this.OrderId);
				return;
			case "gatewaynotfound":
				this.litMessage.Text = "没有找到与此订单对应的支付方式，系统无法自动完成操作，请联系管理员";
				return;
			case "verifyfaild":
				this.litMessage.Text = "支付返回验证失败，操作已停止";
				return;
			case "success":
                #region 推送广告信息
                try
                {
                    if (System.Web.HttpContext.Current.Request.Cookies["AdCookies_cid"] != null && System.Web.HttpContext.Current.Request.Cookies["AdCookies_wi"] != null)
                    {
                        int.TryParse(System.Web.HttpContext.Current.Request.Cookies["AdCookies_cid"].Value.ToString(), out cid);
                        wid = System.Web.HttpContext.Current.Request.Cookies["AdCookies_wi"].Value.ToString();
                        Action ac = new Action(() =>
                        {
                            List<orderStatus> orderst = new List<orderStatus>();
                            orderStatus adInfo = new orderStatus();
                            adInfo.orderNo = this.OrderId;
                            adInfo.feedback = wid;
                            adInfo.updateTime = DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss");
                            adInfo.orderstatus = "active";
                            adInfo.paymentStatus = "2";
                            adInfo.paymentType = "支付宝支付";
                            orderst.Add(adInfo);
                            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(orderst);

                            string url = "http://o.yiqifa.com/servlet/handleCpsInterIn";
                            string interId = System.Configuration.ConfigurationManager.AppSettings["interIdType"].ToString();
                            string DataStr = "interId=" + interId + "&json=" + HttpUtility.UrlEncode(jsonStr) + "&encoding=UTF-8";
                            string RqRest = HttpGet(url, DataStr);
                            ErrorLog.Write("推送CPS返回结果：" + RqRest.ToString());
                        });
                        ac.BeginInvoke(null, ac);
                    }
                    else
                    {
                        ErrorLog.Write("没有获取Cookies值！");
                    }
                }
                catch (Exception ee)
                { }
                #endregion
				this.litMessage.Text = string.Format("恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}", this.OrderId, this.Amount.ToString("F"));
				return;
			case "exceedordermax":
				this.litMessage.Text = "订单为团购订单，订购数量超过订购总数，支付失败";
				return;
			case "groupbuyalreadyfinished":
				this.litMessage.Text = "订单为团购订单，团购活动已结束，支付失败";
				return;
			case "fail":
				this.litMessage.Text = string.Format("订单支付已成功，但是系统在处理过程中遇到问题，请联系管理员</br>支付金额：{0}", this.Amount.ToString("F"));
				return;
			}
			this.litMessage.Text = "未知错误，操作已停止";
		}

        public static string HttpGet(string Url, string postDataStr)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception ee)
            {
                ErrorLog.Write("推送CPS返回结果：" + ee.Message, ee);
                return ee.Message.ToString();
            }
        }
    }
}

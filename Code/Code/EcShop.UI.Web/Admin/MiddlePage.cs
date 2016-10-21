using EcShop.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EcShop.UI.Web.Admin
{
    public class MiddlePage : Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    AdEnterCore ad = new AdEnterCore();
                    if (!string.IsNullOrEmpty(Request.QueryString["cid"]))
                    {
                        ad.Cid = Request.QueryString["cid"];
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["wi"]))
                    {
                        ad.Wi = Request.QueryString["wi"];
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["source"]))
                    {
                        ad.Source = Request.QueryString["source"];
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["channel"]))
                    {
                        ad.Channel = Request.QueryString["channel"];
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["target"]))
                    {
                        ad.Target = Request.QueryString["target"];
                    }

                    HttpCookie cookies_Cid = new HttpCookie("AdCookies_cid");
                    cookies_Cid.Value = ad.Cid;
                    cookies_Cid.Expires = DateTime.Now.AddMonths(+1);
                    Response.AppendCookie(cookies_Cid);


                    HttpCookie cookies_wi = new HttpCookie("AdCookies_wi");
                    cookies_wi.Value = ad.Wi;
                    cookies_wi.Expires = DateTime.Now.AddMonths(+1);
                    Response.AppendCookie(cookies_wi);


                    HttpCookie cookies_source = new HttpCookie("AdCookies_source");
                    cookies_source.Value = ad.Source;
                    cookies_source.Expires = DateTime.Now.AddMonths(+1);
                    Response.AppendCookie(cookies_source);


                    Response.Redirect(ad.Target);
                }
            }
            catch (Exception ee)
            {
                Response.Write("参数错误:"+ee.Message.ToString());

            }
        }
    }
    public class AdEnterCore
    {
        private string cid;
        /// <summary>
        /// 广告主在亿起发推广的标识
        /// </summary>
        public string Cid
        {
            get { return cid; }
            set { cid = value; }
        }
        private string wi;
        /// <summary>
        /// 亿起发下级网站信息
        /// </summary>
        public string Wi
        {
            get { return wi; }
            set { wi = value; }
        }
        private string source;
        /// <summary>
        /// 广告主为亿起发指定的标识
        /// </summary>
        public string Source
        {
            get { return source; }
            set { source = value; }
        }
        private string channel;
        /// <summary>
        /// 渠道
        /// </summary>
        public string Channel
        {
            get { return channel; }
            set { channel = value; }
        }
        private string target;
        /// <summary>
        /// 广告最终着陆地址
        /// </summary>
        public string Target
        {
            get { return target; }
            set { target = value; }
        }

    }
}
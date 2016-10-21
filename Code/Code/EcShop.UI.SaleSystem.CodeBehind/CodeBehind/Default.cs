using EcShop.ControlPanel.Promotions;
using EcShop.ControlPanel.Store;
using EcShop.Core.Entities;
using EcShop.Core.Enums;
using EcShop.Entities.Comments;
using EcShop.Entities.Commodities;
using EcShop.Entities.Orders;
using EcShop.Membership.Context;
using EcShop.Membership.Core.Enums;
using EcShop.SaleSystem.Catalog;
using EcShop.SaleSystem.Shopping;
using EcShop.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
namespace EcShop.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class Default : HtmlTemplatedWebControl
	{
        private System.Web.UI.HtmlControls.HtmlGenericControl div_activelist;
        private ThemedTemplatedRepeater rptProduct;
		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Default.html";
			}
			base.OnInit(e);
		}
		protected override void OnLoad(System.EventArgs e)
		{
			this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>alert_h(\"活动还未开始或者已经结束！\",function(){window.location.href=\"/vshop/default.aspx\";})</script>");
			if (!string.IsNullOrEmpty(this.Page.Request.Params["OrderId"]))
			{
				this.SearchOrder();
			}
			base.OnLoad(e);
            BindList();
            TestDownCount();
		}
        private void BindList()
        {
            if (this.rptProduct != null)
            {
                //DataTable table = ProductBrowser.GetLimitProducts(20, 1, 20);
                //this.rptProduct.DataSource = table;
                //this.rptProduct.DataBind();
                ProductBrowseQuery productBrowseQuery = this.GetProductBrowseQuery();
                DbQueryResult countDownProductList = ProductBrowser.GetCountDownProductList(productBrowseQuery);
                this.rptProduct.DataSource = countDownProductList.Data;
                this.rptProduct.DataBind();
            }
        }
        private ProductBrowseQuery GetProductBrowseQuery()
        {
            return new ProductBrowseQuery
            {
                IsCount = true,
                PageIndex = 1,
                PageSize = 20,
                SortBy = "StartDate",
                SortOrder = SortAction.Asc
            };
        }
		protected override void AttachChildControls()
		{
            this.rptProduct = (ThemedTemplatedRepeater)this.FindControl("rptProduct");
            if (!this.Page.IsPostBack)
            {
                this.BindList();
            }
            this.div_activelist = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("div_activelist");
            if (this.div_activelist!=null)
            {
                string type=string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["IsActiveOpen"])?"1":System.Configuration.ConfigurationManager.AppSettings["IsActiveOpen"].ToString();
                this.div_activelist.Visible =(type == "1") ? true : false;
            }

			if (HiContext.Current.User.UserRole == UserRole.Member && ((Member)HiContext.Current.User).ReferralStatus == 2 && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
			{
				string text = System.Web.HttpContext.Current.Request.Url.ToString();
				if (text.IndexOf("?") > -1)
				{
					text = text + "&ReferralUserId=" + HiContext.Current.User.UserId;
				}
				else
				{
					text = text + "?ReferralUserId=" + HiContext.Current.User.UserId;
				}
				this.Page.Response.Redirect(text);
				return;
			}
			HiContext current = HiContext.Current;
			PageTitle.AddTitle(current.SiteSettings.SiteName + " - " + current.SiteSettings.SiteDescription, HiContext.Current.Context);
		}
        /// <summary>
        /// 查询订单
        /// </summary>
		private void SearchOrder()
		{
			string text = "[{";
			string orderId = this.Page.Request["OrderId"];
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
			if (orderInfo != null)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\"OrderId\":\"",
					orderInfo.OrderId,
					"\",\"ShippingStatus\":\"",
					OrderInfo.GetOrderStatusName(orderInfo.OrderStatus,orderInfo.SourceOrderId),
					"\",\"ShipOrderNumber\":\"",
					orderInfo.ShipOrderNumber,
					"\",\"ShipModeName\":\"",
					orderInfo.RealModeName,
					"\""
				});
			}
			text += "}]";
			this.Page.Response.ContentType = "text/plain";
			this.Page.Response.Write(text);
			this.Page.Response.End();
		}

        private void TestDownCount()
        {
            int curHours = 0;
            int curPoint = 0;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (!string.IsNullOrWhiteSpace(masterSettings.CountDownCouponHours))
            {
                int.TryParse(masterSettings.CountDownCouponHours, out curHours);
            }
            int.TryParse(masterSettings.CountDownCouponPoint, out curPoint);
            // 判断是否是该点发送提醒短信
            if (DateTime.Now.Hour == curPoint)
            {
                DateTime curDate = DateTime.Now.AddHours(curHours);
                List<CountDownCoupons> list = new List<CountDownCoupons>();
                DataTable dtCoupons = CouponHelper.GetAllCountDownCoupons(curDate);
                if (dtCoupons != null && dtCoupons.Rows.Count > 0)
                {
                    foreach (DataRow r in dtCoupons.Rows)
                    {
                        CountDownCoupons cc = new CountDownCoupons();
                        string currcellphone = string.Empty;
                        string currcontent = string.Empty;
                        int value = 0;
                        if (r["CellPhone"] != null && !string.IsNullOrWhiteSpace(r["CellPhone"].ToString()))
                        {
                            currcellphone = r["CellPhone"].ToString();
                        }
                        if (r["DiscountValue"] != null && !string.IsNullOrWhiteSpace(r["DiscountValue"].ToString()))
                        {
                            value = Convert.ToInt32(r["DiscountValue"]);
                        }
                        if (r["ClosingTime"] != null && !string.IsNullOrWhiteSpace(r["ClosingTime"].ToString()) && value > 0)
                        {
                            currcontent = string.Format(masterSettings.CountDownCouponContent, value, r["ClosingTime"].ToString());
                        }
                        if (!string.IsNullOrWhiteSpace(currcellphone) && !string.IsNullOrWhiteSpace(currcontent))
                        {
                            cc.CellPhone = currcellphone;
                            cc.Content = currcontent;
                            cc.ClaimCode = r["ClaimCode"].ToString();
                            list.Add(cc);
                        }

                    }
                    if (list.Count > 0)
                    {
                        SmsHelper.QueueSMS(list, 0, 0);
                    }
                }
            }
        }
	}
}
